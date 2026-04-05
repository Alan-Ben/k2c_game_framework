using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoBuildingEffect : _AALBasicUIWndMono
    {
        [ALHeader("动画组件")]
        public Animation anim;
        [ALHeader("动画名")]
        public string animName;


        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1123); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1123); } }
    }
}
