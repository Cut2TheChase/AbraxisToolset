using HBS.DebugConsole;
using HBS.Scripting.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;
using Necro;

namespace AssetImporting
{
    [ScriptBinding("")]
    class LoadModObject
    {

        [ScriptBinding]
        public static void LoadTheBoi()
        {
            Debug.Log("Lets Load The Boi.");
            Debug.Log("Unity version = " + Application.unityVersion);


            
            var myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "AssetBundles/Windows/starwars"));
            if (myLoadedAssetBundle == null)
            {
                Debug.Log("Failed to load AssetBundle!");
                return;
            }

            var prefab = myLoadedAssetBundle.LoadAsset<GameObject>("eq-weapon-shortsword-bb8");

            MonoBehaviour.Instantiate(prefab);
            Actor bub = ThirdPersonCameraControl.Instance.CharacterActor;
            prefab.transform.position = new Vector3(bub.Position.x, bub.Position.y, bub.Position.z);
            myLoadedAssetBundle.Unload(false);
            
        }
    }
}
