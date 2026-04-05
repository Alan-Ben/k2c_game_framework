using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace ALPackage
{
    public class ALCommonCreateMenu
    {
        //生成空对象
        [MenuItem("Assets/ALCreateMenu/ALCommon/Create Empty ScriptObject")]
        public static void makeBaseAnimationInfoObj()
        {
            string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);

            ALEmptyScriptObject obj = ScriptableObject.CreateInstance<ALEmptyScriptObject>();

            AssetDatabase.CreateAsset(obj, assetPath + "/Obj.asset");
        }
        //生成ALSetting对象
        [MenuItem("Assets/ALCreateMenu/ALCommon/Create SO ALGlobalSetting")]
        public static void createALSettingSO()
        {
            ALSOGlobalSetting obj = ScriptableObject.CreateInstance<ALSOGlobalSetting>();

            AssetDatabase.CreateAsset(obj, "Assets/Resources/ALGlobalSetting.asset");
        }
    }
}
