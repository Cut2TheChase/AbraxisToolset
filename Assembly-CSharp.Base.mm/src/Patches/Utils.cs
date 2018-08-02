using System;
using System.Collections.Generic;
using Necro;
using HBS;
using HBS.Collections;
using HBS.Text;
using Patches;
using Necro.Data.Stats;

namespace AbraxisToolset.src.Patches
{
    public static class Utils
    {

        public static Dictionary<string, Action<TextFieldParser, EffectDef>> GetMethodParsers()
        {
            return LazySingletonBehavior<patch_DataManger>.Instance.GetMethodParsers();
        }

        public static TagWeights ParseTagWeights(TextFieldParser parser, string fieldName, bool nullIfEmpty)
        {
            return LazySingletonBehavior<patch_DataManger>.Instance.ParseTagWeightsProxy(parser, fieldName, nullIfEmpty);
        }

       public static float TryParseFloat(TextFieldParser parser, string fieldName, string varGroup)
        {
            return LazySingletonBehavior<patch_DataManger>.Instance.TryParseFloatProxy(parser, fieldName, varGroup);
        }

        public static StatModPair[] ParseStatMods(TextFieldParser parser, string fieldName, bool nullIfEmpty)
        {
            return LazySingletonBehavior<patch_DataManger>.Instance.ParseStatModsProxy(parser, fieldName, nullIfEmpty);
        }

        public static void NetworkAddIngredient(string id, int value)
        {
            LazySingletonBehavior<patch_Inventory>.Instance.NetworkAddIngredientProxy(id, value);
        }

        public static void RemoveEquipped(Equippable item)
        {
            LazySingletonBehavior<patch_Inventory>.Instance.removeEquipped(item);
        }


    }
}
