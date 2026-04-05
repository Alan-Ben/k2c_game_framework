
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoChildGet : _AALBasicUIWndMono
    {
        [ALHeader("子嗣的信息")]
        public GGUIMonoChildInfo monoChildInfo;
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("跳转按钮")]
        public GameObject btnJumpTo;
        [ALHeader("跳过动画按钮")]
        public GameObject btnSkipAnim;
        [ALHeader("跳过动画的Animation组件")]
        public Animation skipAnimation;
        [ALHeader("跳过动画的动画名字")]
        public string skipAnimationName;


        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1202); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1202); } }
    }
}