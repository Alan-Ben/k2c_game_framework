using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEditor;

#if AL_CREATURE_SYS
namespace ALPackage
{
    public class ALMoveSOMenu
    {
        //生成移动模式对象
        [MenuItem("Assets/ALCreateMenu/ALMoveMode/Create SO ALMoveAnimationModeObj")]
        public static void makeMoveAnimationModeObj()
        {
            string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);

            ALSOMoveAnimationModeObj obj = ScriptableObject.CreateInstance<ALSOMoveAnimationModeObj>();

            AssetDatabase.CreateAsset(obj, assetPath + "/ALSOMoveAnimationModeObj.asset");
        }

        //生成移动状态信息对象
        [MenuItem("Assets/ALCreateMenu/ALMoveMode/ALMoveAnimationInfo/Create SO ALMoveAnimationInfo")]
        public static void makeMoveAnimationInfo()
        {
            string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);

            ALSOMoveAnimationInfo obj = ScriptableObject.CreateInstance<ALSOMoveAnimationInfo>();

            AssetDatabase.CreateAsset(obj, assetPath + "/ALSOMoveAnimationInfo.asset");
        }

        //生成移动状态信息子对象
        [MenuItem("Assets/ALCreateMenu/ALMoveMode/ALMoveAnimationInfo/Create SO ALMoveAnimationInfoObj")]
        public static void makeMoveAnimationInfoObj()
        {
            string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);

            ALSOMoveAnimationInfoObj obj = ScriptableObject.CreateInstance<ALSOMoveAnimationInfoObj>();

            AssetDatabase.CreateAsset(obj, assetPath + "/ALSOMoveAnimationInfoObj.asset");
        }
    }
}
#endif
