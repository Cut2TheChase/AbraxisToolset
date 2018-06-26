using HBS.Text;
using HBS.Collections;
using Necro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AbraxisToolset;
using UnityEngine;
using HBS.Data;
using System.Collections;

namespace Patches {
    [MonoMod.MonoModPatch("global::Necro.DataManager")]
    class patch_DataManger : DataManager {

        [MonoMod.MonoModIgnore]
        private Dictionary<string, Action<TextFieldParser, EffectDef>> geMethodParsers;
        public Dictionary<string, Action<TextFieldParser, EffectDef>> GetMethodParsers() {
            return geMethodParsers;
        }

        [MonoMod.MonoModIgnore]
        private extern TagWeights ParseTagWeights(TextFieldParser parser, string fieldName, bool nullIfEmpty);

        [MonoMod.MonoModIgnore]
        private extern float TryParseFloat(TextFieldParser parser, string fieldName, string varGroup);



        public float TryParseFloatProxy(TextFieldParser parser, string fieldName, string varGroup)
        {
            return TryParseFloat(parser, fieldName, varGroup);
        }

        public TagWeights ParseTagWeightsProxy(TextFieldParser parser, string fieldName, bool nullIfEmpty)
        {
            return ParseTagWeights(parser, fieldName, nullIfEmpty);
        }

       

        public extern void orig_Awake();
        public void Awake() {
            Debug.Log("Pre parser load");
            NecroHooks.preParserLoad();
            orig_Awake();
        }

       

    }
}
