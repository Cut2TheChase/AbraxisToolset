using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HBS;
using HBS.Logging;
using Necro;

namespace Patches
{
    /*
    [MonoMod.MonoModPatch("global::Necro.PrefabManager")]
    public class patch_PrefabManager : PrefabManager
    {

        [MonoMod.MonoModPublic]
        private static readonly char[] DASH;

        [MonoMod.MonoModPublic]
        private static readonly ILog log;

        [MonoMod.MonoModIgnore]
        private extern void CachePrefab<T>(string prefabName, T prefab) where T : UnityEngine.Object;


        public extern void orig_ResolveAssetPath(string AssetName, out string bundleName);
        public static string ResolveAssetPath(string assetName, out string bundleName)
        {
            if(assetName != "eq-weapon-shortsword-bb8")
            {
                orig_ResolveAssetPath(assetName, out bundleName);
            }
            bundleName = null;
            string value = null;
            if (assetName.StartsWith("ab-") || assetName.StartsWith("fx-") || assetName.StartsWith("eq-") || assetName.StartsWith("ms-") || assetName.StartsWith("pf-") || assetName.StartsWith("ui-") || assetName.StartsWith("im-") || assetName.StartsWith("ma-"))
            {
                value = "Prefabs/";
            }
            if (assetName.IndexOf('-') < 0 || assetName.IndexOf('/') >= 0)
            {
                return assetName;
            }
            string[] array = assetName.Split(DASH);
            if (array.Length < 3)
            {
                log.Error("Attempting to load a malnamed gameobject: " + assetName);
                return null;
            }
            StringBuilder stringBuilder = new StringBuilder();
            PrefabManifest prefabManifest = LazySingletonBehavior<PrefabManager>.Instance.PrefabManifest;
            string baseDirectory = prefabManifest.GetBaseDirectory(array[1]);
            if (string.IsNullOrEmpty(baseDirectory))
            {
                log.Error("Attempting to load from a path that isn't in manifest: " + assetName);
                return null;
            }
            if (!string.IsNullOrEmpty(value))
            {
                stringBuilder.Append(value);
            }
            stringBuilder.Append(baseDirectory);
            stringBuilder.Append('/');
            for (int i = 1; i < array.Length - 1; i++)
            {
                stringBuilder.Append(array[i]);
                stringBuilder.Append('/');
            }
            if (assetName.StartsWith("ma-"))
            {
                stringBuilder.Append("Materials/");
            }
            stringBuilder.Append(assetName);
            return stringBuilder.ToString();
        }

        public void CachePrefabProxy<T>(string prefabName, T prefab) where T : UnityEngine.Object
        {
            CachePrefab<T>(prefabName, prefab);
        }
    }
    */
}
