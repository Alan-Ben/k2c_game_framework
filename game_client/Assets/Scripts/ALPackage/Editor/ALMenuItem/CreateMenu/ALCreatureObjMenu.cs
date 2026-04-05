using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

#if AL_CREATURE_SYS
namespace ALPackage
{
    public class ALCreatureObjMenu
    {
        //生成ALSetting对象
        [MenuItem("Assets/ALCreateMenu/ALCreature/Create SO ALCreatureBoneObjInfo")]
        public static void makeCreatureBoneObjInfo()
        {
            string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);

            ALSOCreatureBoneObjInfo obj = ScriptableObject.CreateInstance<ALSOCreatureBoneObjInfo>();

            AssetDatabase.CreateAsset(obj, assetPath + "/ALSOCreatureBoneObjInfo.asset");
        }

        //生成附加物信息
        [MenuItem("Assets/ALCreateMenu/ALCreature/Create SO ALAdditionObjInfo")]
        public static void makeAdditionObjInfo()
        {
            string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);

            _AALSOBasicAdditionObjInfo obj = ScriptableObject.CreateInstance<_AALSOBasicAdditionObjInfo>();

            AssetDatabase.CreateAsset(obj, assetPath + "/ALSOAdditionObjInfo.asset");
        }
    }
}

#endif
