using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 召唤系统主页面
    /// </summary>
    public class GGUIMonoSummonMain : _ANPBasicUIWndResBarMono
    {
        [ALHeader("召唤计数按钮")]
        public GameObject btnCumulativeNum;

        [ALHeader("奖励概率按钮")]
        public GameObject btnRewardProbability;
        
        [ALHeader("单抽消耗道具")]
        public NPGGUIMonoCommonItem oneDrawCostItem;
        [ALHeader("单抽按钮")]
        public GameObject btnOneDraw;
        
        [ALHeader("十抽消耗道具")]
        public NPGGUIMonoCommonItem tenDrawCostItem;
        [ALHeader("十抽按钮")]
        public GameObject btnTenDraw;

        [ALHeader("免费抽卡固定CD展示")]
        public GGUIMonoCommonFixedCd freeDrawFixedCd;
        [ALHeader("免费抽卡按钮")]
        public GameObject btnFreeDraw;

        [ALHeader("跳过抽卡动画开关")]
        public NPGGUIMonoCommonToggleEx monoSkipDrawAnimationToggle;

        [ALHeader("召唤记录Grid")]
        public GGUIMonoSummonPublicRollRecordGrid monoRollRecordGrid;
        
        [ALHeader("招募按钮")]
        public GameObject btnRecruit;

        [ALHeader("返回按钮")]
        public GameObject btnReturn;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(2201); } }
        public static string objName { get { return UIResPathAssistant.getObjName(2201); } }
    }
}