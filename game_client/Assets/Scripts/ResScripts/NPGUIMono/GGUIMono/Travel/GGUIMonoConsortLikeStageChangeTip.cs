using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 妃子好感阶段变化提示
    /// </summary>
    public class GGUIMonoConsortLikeStageChangeTip : _AALBasicUIWndMono
    {
        [ALHeader("妃子半身像")]
        public RawImage imgConsort;
        
        [ALHeader("妃子形象ShowCase")]
        public GGUIMonoCommonShowCase monoConsortShowCase;
        [ALHeader("妃子形象在ShowCase中的索引")]
        public int consortActorShowCaseIndex;
        
        [ALHeader("描述文本")]
        public TextEx txtDesc;
        [ALHeader("描述文本key(一个参数, 妃子名)")]
        public string txtDescKey;

        [ALHeader("好感度进度条")]
        public NPGGUIMonoProgress likeProgress;
        
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(3613); } }
        public static string objName { get { return UIResPathAssistant.getObjName(3613); } }
    }
}