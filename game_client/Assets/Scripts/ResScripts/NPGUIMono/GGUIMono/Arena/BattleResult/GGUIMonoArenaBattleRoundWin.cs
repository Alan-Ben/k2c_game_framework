using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 竞技场回合胜利弹窗
    /// </summary>
    public class GGUIMonoArenaBattleRoundWin : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("距离获取连胜奖励描述")]
        public Text txtGainRewardDesc;
        [ALHeader("银币图标")]
        public RawImage imgCoin;
        [ALHeader("我方影响力图标")]
        public RawImage imgSelfInfluence;
        [ALHeader("对方影响力图标")]
        public RawImage imgOpponentInfluence;
        [ALHeader("获得的商会硬币")]
        public Text txtGainCoin;
        [ALHeader("我方增加的影响力")]
        public Text txtSelfInfluenceChg;
        [ALHeader("对方减少的影响力")]
        public Text txtOpponentInfluenceChg;
        [ALHeader("连胜次数")]
        public Text txtWinCount;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(5214); } }
        public static string objName { get { return UIResPathAssistant.getObjName(5214); } }
    }
}