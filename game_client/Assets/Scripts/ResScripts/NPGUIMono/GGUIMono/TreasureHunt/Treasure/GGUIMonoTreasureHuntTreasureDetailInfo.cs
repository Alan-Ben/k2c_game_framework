using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 奇物详情信息窗口
    /// </summary>
    public class GGUIMonoTreasureHuntTreasureDetailInfo : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        
        [ALHeader("奇物信息")]
        public GGUIMonoTreasureHuntTreasureInfo monoTreasureInfo;

        [ALHeader("获取时间文本")]
        public TextEx txtGainTime;
        
        [ALHeader("技能信息")]
        public GGUIMonoTreasureHuntSkillInfo monoSkillInfo;

        [ALHeader("上一个奇物按钮")]
        public GameObject btnPre;
        [ALHeader("下一个奇物按钮")]
        public GameObject btnNext;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6820); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6820); } }
    }
}