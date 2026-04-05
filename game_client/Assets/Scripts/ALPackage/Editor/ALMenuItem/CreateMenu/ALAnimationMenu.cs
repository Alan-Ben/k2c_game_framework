using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

#if AL_CREATURE_SYS
namespace ALPackage
{
    public class ALAnimationMenu
    {
        //生成动作层级事件对象
        //添加对象事件
        [MenuItem("Assets/ALCreateMenu/ALAnimation/Event/Create SO ALAnimationAddObjEvent")]
        public static void makeAnimationAddObjEvent()
        {
            string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);

            ALSOAnimationAdditionAddEvent obj = ScriptableObject.CreateInstance<ALSOAnimationAdditionAddEvent>();

            AssetDatabase.CreateAsset(obj, assetPath + "/ALSOAnimationAddEvent.asset");
        }
        //删除对象事件
        [MenuItem("Assets/ALCreateMenu/ALAnimation/Event/Create SO ALAnimationRemoveObjEvent")]
        public static void makeAnimationRemoveObjEvent()
        {
            string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);

            ALSOAnimationAdditionRemoveEvent obj = ScriptableObject.CreateInstance<ALSOAnimationAdditionRemoveEvent>();

            AssetDatabase.CreateAsset(obj, assetPath + "/ALSOAnimationRemoveEvent.asset");
        }
        //终止动作事件
        [MenuItem("Assets/ALCreateMenu/ALAnimation/Event/Create SO ALAnimationFinishEvent")]
        public static void makeAnimationFinishEvent()
        {
            string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);

            ALSOAnimationFinishEvent obj = ScriptableObject.CreateInstance<ALSOAnimationFinishEvent>();

            AssetDatabase.CreateAsset(obj, assetPath + "/ALSOAnimationFinishEvent.asset");
        }
        //调整动画播放速度事件
        [MenuItem("Assets/ALCreateMenu/ALAnimation/Event/Create SO ALAnimationChgTimeScaleEvent")]
        public static void makeAnimationChgTimeScaleEvent()
        {
            string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);

            ALSOAnimationChgTimeScaleEvent obj = ScriptableObject.CreateInstance<ALSOAnimationChgTimeScaleEvent>();

            AssetDatabase.CreateAsset(obj, assetPath + "/ALSOAnimationChgTimeScaleEvent.asset");
        }

        //生成动作组对象
        [MenuItem("Assets/ALCreateMenu/ALAnimation/Create SO ALBaseAnimationInfo")]
        public static void makeBaseAnimationInfo()
        {
            string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);

            ALSOBaseAnimationInfo obj = ScriptableObject.CreateInstance<ALSOBaseAnimationInfo>();

            AssetDatabase.CreateAsset(obj, assetPath + "/ALSOBaseAnimationInfo.asset");
        }
        //生成动作对象
        [MenuItem("Assets/ALCreateMenu/ALAnimation/Create SO ALBaseAnimationInfoObj")]
        public static void makeBaseAnimationInfoObj()
        {
            string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);

            ALSOBaseAnimationInfoObj obj = ScriptableObject.CreateInstance<ALSOBaseAnimationInfoObj>();

            AssetDatabase.CreateAsset(obj, assetPath + "/ALSOBaseAnimationInfoObj.asset");
        }
    }
}
#endif
