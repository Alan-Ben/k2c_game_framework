using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 随机邀约表现过程
    /// </summary>
    public class GGUIMonoConsortRandomInviteProcess : _AALBasicUIWndMono
    {
        [ALHeader("妃子名")]
        public TextEx txtConsortName;
        [ALHeader("妃子头像icon")]
        public RawImage consortHeadIcon;
        [ALHeader("妃子卡牌icon")]
        public RawImage consortCardIcon;
        [ALHeader("妃子卡牌背景")]
        public GGUIMonoConsortCardBg consortCardBg;
        [ALHeader("妃子头像的RectTransform, 用来移动的")]
        public RectTransform consortHeadTrans;
        [ALHeader("头像移动目标点的RectTransform")]
        public RectTransform headMoveTargetTrans;
        [ALHeader("妃子头像移动时间(秒)")]
        public float consortHeadMoveTime = 1f;

        [ALHeader("当妃子头像移动时动画名")]
        public string onConsortHeadMoveAniName;

        [ALHeader("当妃子头像移动结束动画名")] 
        public string onConsortHeadMoveDoneAniName;
        
        [ALHeader("显示对话动画名")]
        public string dialogShowAniName;
        
        [ALHeader("背景 showcase")]
        public GGUIMonoCommonShowCase monoBgShowcase;
        [ALHeader("背景的下标")]
        public int bgShowcaseIndex = 0;
        [ALHeader("加载的背景的默认材质")] 
        public Material defaultMaterial;
        [ALHeader("加载的背景的默认动画")] 
        public string defaultAniName;
        
        [ALHeader("对话子窗口")]
        public NPGGUIMonoSubDialogue monoSubDialogue;
        
        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1426); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1426);} }
    }
}