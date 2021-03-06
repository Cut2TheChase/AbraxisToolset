# AbraxisToolset
The base toolset for the game, which includes a CSV parser/patcher and a few other bits and pieces.

Feel free to support Zandra (The original creator of this project) through [his Patreon](https://www.patreon.com/zerndererer). He has no plans to continue development on this specific project, for now.

This version of AbraxisToolset has been edited by me, CutToTheChase, to further encourage mod/patch development of Necropolis while Zandra has focused on other projects.

# Setup
For setup, you will first need to install Partiality ver 0.3.4 (You can learn how to do that here - https://github.com/PartialityModding/PartialityLauncher) and run Necropolis once after patching the game with it. After that, just stick the AbraxisToolset.dll into the "Mods" folder, check off the mod in the Partiality Launcher, and that's it! Patch the game through the Partiality Launcher and AbraxisToolset should be ready to go.

# Regular Modding/Patching
All method and code modding/Patching has now been moved to Partiality, as AbraxisToolset has been gutted to simply focus on Necropolis specific stuff, especially CSVs.

If you'd like to learn how to use Partiality for modding purposes, check out the github and look at the "Tutorial" file - https://github.com/PartialityModding/PartialityLauncher

# CSV Patching
Now here is the bread and butter of AbraxisToolset. Necropolis gathers a lot of its' data about Creatures, Items, etc, from these few CSV files that are within the game's folders. By editing/adding to these files, we can fundamentally change how Necropolis plays and, along with regular mods, create new functionality that can exceed what the previous version of the game could do.

With that in mind, familiarize yourself with these CSVs, they will come in handy, and you should be able to view them by going to this file path in your Necropolis folder - Necropolis/Necropolis_Data/StreamingAssets/data

To make a CSV patch, you will need to create a new CSV with the name of the CSV you want to patch, and place it in the "Mods" folder (I would suggest also putting it in there within a folder that's labeled with your mod name for easy organizing, as well as that can be a place to put in any .dll mods associated with you CSV patch).

## Patching How-To
So if you want to patch the CSVs, AbraxisToolset has three prefixes you can use- patchAdd, patchOver, and patchAnim. These three prefixes will be your friends :D

The way each of these patches work is simple, if you want to add or write over anything to a CSV entry, you simply take the ID of that row (usually the first field, but with CSVs that start with "Group"/"GroupID"/etc the ID field is second. (EXCEPT FOR SHOP LIST.CSV, THAT ONE IS JUST WEIRD, PUT THE PREFIX ON THE ITEM NOT THE ID)) and in front of it put a prefix.

So for example, if we take Creatures.CSV, lets say we wanted to mess with a Gemeater, specifically GemeaterBull. We would then go into our new CSV also titled Creatures, write down the group in the first field, go to the second field (because that's the ID for this CSV) and put my desired prefix in front of it, so GemeaterBull would become patchOver_GemeaterBull, and then I could fill in the fields on that row that I would want to overwrite.

In cases that you want to add something completely new to the CSV, using patchAdd or patchOver is fine, as it will simply see that the ID you used isn't in the CSV and will make a new entry. patchAnim is a lil' special, and we will get to that in a bit.

### patchOver Prefix
The patchOver prefix is used to write over fields on a particular row in a CSV. 

so in this case, lets say you wanted to give the SlowHookedBlade Speed -9001 rather than Speed -20. In your new Item CSV, write down a row with the first field being the group ShortSword (because Items.csv uses groups), then with the second entry being the ID you write down patchOver_SlowHookedBlade. Then simply go/count to the field where Passive Traits would be, and just write down Speed -9001. With this CSV in your Mods folder, when you run the game the blade should now have everything the same except that now when you check the CSV it's passive trait has been changed, and in the game this will translate.

There is also a special case scenario where you just might want to, delete a field. Leaving it blank won't do anything, as it will skip over it, but if you write in the field "[remove]" the CSV patcher will take that as you want to erase the field entirely.

### patchAdd Prefix
So lets say you don't want to write over a row, but rather just add some values to one/a few if it's fields. The patchAdd prefix does just that!

So lets say you wanted to change the Game Action PassUnseen to not only make you invisible but also increase your jump. You simply, in your new Game Actions CSV, write the first field as patchAdd_PassUnseen (because that's the ID), then skip over to the field where Params would be and simply write JumpModifier +100. Now when you turn invisible you can also jump really high!

### MutltiElement List Patching (LootTables.csv & Variables.csv)
So if you look at both the LootTables CSV and Variables CSV, you will see they are not like other CSVs. The lists are both horizontal and vertical! So how on earth do you patch that? Well, all you need to do is create your patch CSV in the same format as the multielement CSV, with white spaces in the same places, and simply as before put either the patchAdd or patchOver prefix in front of the id name within the first column.

If you dont want to affect a certain row for whatever reason, or need to go down to say the third row in a given id's list,  simply put "[skip]" in the field you want skip over. The parser will go over that field and not affect it.

#### Adding new rows to MultiElement Lists
You can now add completely new rows to MultiElement Lists! Simply use the patchOver prefix for the particular ID you want to add a row to, put "[skip]" in the next field over, then on the row underneath in the 2nd column put the prefix "patchAppend_" in front of the field before typing whatever it is your adding, then on any row underneath before another prefix it will treat that as being added to that ID!

![Example](images/MultiElementAppendExample.PNG?raw=true "Title")

### patchAnim & Special Notes
As of right now, because of how I thought I needed to rework the Anim Actions CSV, it does not use the normal patchOver or patchAdd (I probably will, in the future, have it where I can bring it back to that, but for now this will be better until completely new entries need to be made)

So patchAnim is for specifically patching the Anim Actions CSV. Anim Actions is interesting because it controls a lot of things in terms of what attacks a Creature can do and how those attacks affect certain game aspects. 

With your new Anim Action CSV, if you want to edit a row you must first in the group column write what row number you want to edit. Then, in the next field, write patchAnim_ and then whatever creature you want to add, or you can just leave the second part blank if you want to just edit values. The rest of the values work like patchOver, so it will  write over any other field you write to.

It's a bit confusing, but trust me when I say this was the easiest way to implement it.

-SPECIAL NOTES-
I will try to add in patching in completely new enemies for patchAnim once that's needed, but for now I feel like it's fine the way it is.

There are some wonky multilist CSVs, such as Loot Tables.csv, which may be a little confusing to figure out the syntax for. I will be sure to leave an example in my test mod for you guys to look at.

### MonoMod Utils
Abraxis Toolset now has some Utils that you can use from Necropolis! As I was making my Zeke's New Throwables mod I realized there were some things that I had to call from DataManager that were private, so I had AbraxisToolset and my mod use MonoMod.MonoInterop so my mod as well as others wouldnt have to patch the game for those! If you need to look at how to use it, you can refer to my Zeke's New Throwables mod or just talk to me.

#### TEST MOD
I've included a test mod that will patch over CSVs in Necropolis, they dont do anything too crazy but they just show you how to do some simple stuff :) Feel free to throw that entire folder into your Partiality Mods folder and Abraxis Toolset should use them to patch your CSVs!

Finally, if there are any more questions, please feel free to message me on Necropolis Discord channel, particularly in the modding channel, and I'll be happy to answer any questions/concerns.



