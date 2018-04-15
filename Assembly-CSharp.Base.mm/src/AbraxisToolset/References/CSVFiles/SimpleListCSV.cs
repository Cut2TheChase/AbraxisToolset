﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using HBS.Text;
using HBS.DebugConsole;
using UnityEngine;

namespace AbraxisToolset.CSVFiles {
    public class SimpleListCSV: ICSVFile {
        public Dictionary<string, ListEntry> entries = new Dictionary<string, ListEntry>(); //Possibly make new class which holds ListEntry & a number, need a way to reference row #
        public Dictionary<int, ListEntry> animEntries = new Dictionary<int, ListEntry>(); //Holds entries for anim actions, but by row instead
        public ListEntry defEntry;
        public bool useGroups = true;
        public bool shopList = false;
        public bool animList = false;
        public int totRowCount = 0;

        //ID prefixes
        public const string patchAddPrefix = "patchAdd";
        public const string patchOverPrefix = "patchOver";
        public const string patchAnimPrefix = "patchAnim";
        //Element Tags
        public const string elementRemoveTag = "[remove]";

        public void ReadFromFile(string path) {
            if( !File.Exists( path ) )
                throw new System.IO.FileNotFoundException( "File not found at " + path );
            string[] lines = File.ReadAllLines( path );
            string fileName = Path.GetFileNameWithoutExtension( path );

            //Defenition entry
            {
                //Split by commas
                List<string> seperatedLines = new List<string>();
                StringUtil.SplitCSV( lines[0], seperatedLines );

                //Set entry
                defEntry = new ListEntry() {
                    values = seperatedLines.ToArray()
                };

                defEntry.rowNumber = 1;
            }

            string defFirstValue = defEntry.values[0];
            useGroups = defFirstValue == "Group ID" || defFirstValue == "Group" || defFirstValue == "GroupID";


            //Shop List is weird and makes its group names "ID" instead of "Group" soooooo, gotta make this check
            if(fileName == "Shop List")
            {
                shopList = true;
            }
            else if(fileName == "Anim Actions")
            {
                animList = true;
                useGroups = false;
            }

            int emptyCount = 0;
            int noLineCount = 0;
            int skippedCount = 0;
            int addedCount = 0;
            int rowCount = 1;

            //Add entries
            for( int i = 1; i < lines.Length; i++ ) {
                try {
                    rowCount++; //Count row at beginning of each line check
                    if( lines[i].Length == 0 ) {
                        skippedCount++;
                        continue;
                    }
                    List<string> seperatedLines = new List<string>();
                    StringUtil.SplitCSV( lines[i], seperatedLines );

                    if( seperatedLines.Count <= 1 ) {
                        noLineCount++;
                        continue;
                    }

                    bool isEmpty = true;
                    foreach( string s in seperatedLines ) {
                        if( s != string.Empty ) {
                            isEmpty = false;
                            break;
                        }
                    }
                    if( isEmpty ) {
                        emptyCount++;
                        continue;
                    }

                    //Create entry
                    ListEntry entry = new ListEntry() {
                        values = seperatedLines.ToArray()
                    };

                    entry.rowNumber = rowCount; //If entry is to be added, give it a row #
                    AddEntry( entry );
                    addedCount++;
                } catch( System.Exception e ) {
                    Debug.LogError( e );
                    Debug.LogError( "Failed to read entry " + lines[i] );
                }
            }
            totRowCount = rowCount;
            Debug.Log( string.Format( "File {0} had {1} empty entries, {2} no-line entries, {3} skips, and {4} normal entries, {5} total lines in the file", fileName, emptyCount, noLineCount, skippedCount, addedCount, lines.Length ) );
        }

        public void PatchFromFile(string path) {
            if( !File.Exists( path ) )
                throw new System.IO.FileNotFoundException( "File not found at " + path );
            string[] lines = File.ReadAllLines( path );

            for( int i = 0; i < lines.Length; i++ ) {
                List<string> seperatedLines = new List<string>();
                StringUtil.SplitCSV( lines[i], seperatedLines );

                //Create entry
                ListEntry entry = new ListEntry() {
                    values = seperatedLines.ToArray()
                };

                string entryIDPrefix = null;
                string entryIDWithoutPrefix = null;

                //If you are using groups or Shop List CSV
                if (useGroups || shopList)
                {
                    entryIDPrefix = ATCSVUtil.GetPrefix(entry.ID);
                    entryIDWithoutPrefix = ATCSVUtil.GetWithoutPrefix(entry.ID);
                    entry.rowNumber = ++totRowCount;
                }
                //If you are using Anim Actions CSV
                else if(animList)
                {
                    entryIDPrefix = ATCSVUtil.GetPrefix(entry.ID);
                    entryIDWithoutPrefix = ATCSVUtil.GetWithoutPrefix(entry.ID);
                    //Puts row number specified in Anim Actions Patch to row number of new entry
                    try
                    {
                        entry.rowNumber = Convert.ToInt32(entry.group);
                    }
                    catch (FormatException e)
                    {
                        Debug.Log("Error Patching Anim Actions - Input string is not a sequence of digits. " + entry.group);
                    }
                    catch (OverflowException e)
                    {
                        Debug.Log("Error Patching Anim Actions - Row Number cannot fir in an Int32." + entry.group);
                    }
                }
                else
                {
                    entryIDPrefix = ATCSVUtil.GetPrefix(entry.group);
                    entryIDWithoutPrefix = ATCSVUtil.GetWithoutPrefix(entry.group);
                }

                //Patch adding
                if ( entryIDPrefix == patchAddPrefix ) {
                    PatchAdd( entry, entryIDWithoutPrefix );
                } else
                //Patch over
                if( entryIDPrefix == patchOverPrefix ) {
                    PatchOver( entry, entryIDWithoutPrefix );
                } else
                //Patch Anim Actions CSV
                if( entryIDPrefix == patchAnimPrefix) {
                    AnimPatchAdd(entry, entryIDWithoutPrefix);
                }
            }
        }
        public void WriteToFile(string path) {
            try {
                List<string> lines = new List<string>();
                string fileName = Path.GetFileNameWithoutExtension( path );
                //Def entry
                {
                    string line = string.Empty;
                    //Append values
                    foreach( string s in defEntry.values ) {
                        line += s + ',';
                    }
                    //Append new line
                    lines.Add( line );
                }

                //When dealing with Not Anim Actions CSV
                if (!animList)
                {
                    foreach (ListEntry entry in entries.Values)
                    {
                        string line = string.Empty;
                        List<string> components = new List<string>();
                        //Append values
                        foreach (string s in entry.values)
                        {
                            StringUtil.SplitCSV(s, components);
                            if (components.Count > 1)
                            {
                                line += '"' + s + '"' + ',';
                            }
                            else
                            {
                                line += s + ',';
                            }
                            //Debug.Log(string.Format("Filename {0} with Entry - {1} ", fileName, line)); 
                        }
                        //Append new line
                        lines.Add(line);
                    }
                } else
                //When dealing with Anim Actions CSV
                {
                    foreach (ListEntry entry in animEntries.Values)
                    {
                        string line = string.Empty;
                        List<string> components = new List<string>();
                        //Append values
                        foreach (string s in entry.values)
                        {
                            StringUtil.SplitCSV(s, components);
                            if (components.Count > 1)
                            {
                                line += '"' + s + '"' + ',';
                            }
                            else
                            {
                                line += s + ',';
                            }
                            //Debug.Log(string.Format("Filename {0} with Entry - {1} ", fileName, line)); 
                        }
                        //Append new line
                        lines.Add(line);
                    }
                }
                // Debug.Log( string.Format( "Writing {0} lines to file {1}, out of {2} entries", lines.Count, fileName, entries.Values.Count ) );
                File.WriteAllLines( path, lines.ToArray() );
            } catch( System.Exception e ) {
                Debug.LogError( e );
            }
        }

        public void AddEntry(ListEntry entry) {
            try {
                if (useGroups || shopList) {
                    //Add entry to entry dictionary
                    entries[entry.ID] = entry;
                    //entriesByRow[entry.rowNumber] = entry;
                } else if (animList)
                {
                    animEntries[entry.rowNumber] = entry;
                } else {
                    entries[entry.group] = entry;
                    //entriesByRow[entry.rowNumber] = entry;
                }
            } catch( System.Exception e ) {
                Debug.LogError( e );
            }
        }
        public void Clear() {
            entries.Clear();
            defEntry = null;
        }

        public void PatchAdd(ListEntry newEntry, string entryIDWithoutPrefix, string groupID = "MODDED") {
            //If there's no entry to patch, just add this entry.
            if( !entries.ContainsKey( entryIDWithoutPrefix ) ) {
                //Set ID to be the non-prefix version
                if (useGroups)
                {
                    newEntry.values[0] = newEntry.group;
                    newEntry.values[1] = entryIDWithoutPrefix;
                }
                else if (shopList || animList)
                {
                    newEntry.values[0] = newEntry.group;
                    newEntry.values[1] = entryIDWithoutPrefix;
                }
                else
                {
                    newEntry.values[0] = entryIDWithoutPrefix;
                }

                //Add the entry
                AddEntry( newEntry );
            } else {

                //Get Item Entry to add to
                ListEntry patchEntry = entries[entryIDWithoutPrefix];

                int start = 1;
                if( useGroups )
                    start++;

                //Add elements to existing entry
                for( int i = start; i < newEntry.values.Length; i++ ) {
                    patchEntry.values[i] += newEntry.values[i];
                }

                entries[entryIDWithoutPrefix] = patchEntry;
            }
        }
        public void AnimPatchAdd(ListEntry newEntry, string entryIDWithoutPrefix, string groupID = "MODDED")
        {
            //If there's no entry using the specified row number to patch, just add this entry.
            //Should use first element in Anim Actions CSV to find row to add entry to
            if (!animEntries.ContainsKey(newEntry.row))
            {
                //Set ID to be the non-prefix version
                    newEntry.rowNumber = ++totRowCount;
                    newEntry.values[0] = groupID;
                    newEntry.values[1] = entryIDWithoutPrefix;

                //Add the entry
                AddEntry(newEntry);
            }
            else
            {
                newEntry.values[1] = entryIDWithoutPrefix;
                Debug.Log("WAKADOO - newEntry.values[0] = " + newEntry.values[0] + " & newEntry.values[1] = " + newEntry.values[1]);

                //Get Item Entry to add to
                ListEntry patchEntry = animEntries[newEntry.row];

                int start = 1;
                
                if (useGroups)
                    start++;

                //Add elements to existing entry
                for (int i = start; i < newEntry.values.Length; i++)
                {
                    //If the new element doesnt have what already is in the element being patched
                    if (patchEntry.values[i] != newEntry.values[i])
                    {
                        //If the element is not empty, add a comma
                        if (patchEntry.values[i] != null || patchEntry.values[i] != "")
                            patchEntry.values[i] += ", ";

                        patchEntry.values[i] += newEntry.values[i];
                    }
                }

                animEntries[newEntry.row] = patchEntry;
            }
        }
        public void PatchOver(ListEntry newEntry, string entryIDWithoutPrefix, string groupID = "MODDED") {
            //If there's no entry to patch, just add this entry.
            if( !entries.ContainsKey( entryIDWithoutPrefix )) {
                //Set ID to be the non-prefix version
                if( useGroups) {
                    newEntry.values[0] = newEntry.group;
                    newEntry.values[1] = entryIDWithoutPrefix;
                }
                else if (shopList || animList)
                {
                    newEntry.values[0] = newEntry.group;
                    newEntry.values[1] = entryIDWithoutPrefix;
                }
                else {
                    newEntry.values[0] = entryIDWithoutPrefix;
                }

                //Add the entry
                AddEntry( newEntry );
            } else {

                //Get Item Entry to add to
                ListEntry patchEntry = entries[entryIDWithoutPrefix];

                int start = 0;
                
                if( useGroups )
                    start++;

                //Patch over existing entries
                for( int i = start; i < newEntry.values.Length; i++ ) {
                    //store element
                    string element = newEntry.values[i];

                    //If element is empty, skip it.
                    if( element == string.Empty )
                        continue;
                    //If the element is the tag to remove, remove the original element.
                    else if( element == elementRemoveTag )
                        patchEntry.values[i] = string.Empty;
                    //If the other two checks aren't true, just overwrite.
                    else
                        patchEntry.values[i] = element;

                }

                entries[entryIDWithoutPrefix] = patchEntry;
            }
        }

        public class ListEntry {
            public string[] values;
            public int rowNumber;

            public string group { get { return values[0]; } }
            public string ID { get { return values[1]; } }
            public int row { get { return rowNumber; } }
        }
    }
}
