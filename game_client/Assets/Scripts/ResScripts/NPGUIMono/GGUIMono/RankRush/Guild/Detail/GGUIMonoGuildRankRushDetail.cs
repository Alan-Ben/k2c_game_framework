using Common.ActivityEnum;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟冲榜详情主界面
    /// </summary>
    public class GGUIMonoGuildRankRushDetail : _ANPBasicUIWndResBarMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("礼包按钮")]
        public GameObject btnGift;
        [ALHeader("冲榜名称")]
        public Text txtRankRushName;
        [ALHeader("获取途径按钮")]
        public GameObject btnAccess;
        [ALHeader("页签列表")]
        public List<GGUIRankRushDetailTabMono> monoTabList;
        [ALHeader("页面父节点")]
        public Transform pageParent;
        [ALHeader("前几名玩家信息展示，排名按配置顺序")]
        public List<GGUIMonoRankRushTopGuildInfo> monoTopPlayerList;
        [ALHeader("倒计时")]
        public Text txtCD;
        [ALHeader("我的排名")]
        public Text txtMyRank;
        [ALHeader("我的分数")]
        public Text txtMyScore;
        [ALHeader("排行榜提示按钮")]
        public GameObject btnRankTip;
        [ALHeader("领取奖励按钮")]
        public GameObject btnGetReward;
        [ALInfo("活动状态：\nPLAN 待开启\nPLAYING 运行中\nSETTLING 结算中\nFROZEN 冻结中 -> 领奖期\nCLOSED 已关闭\nCAN_DISCARD 可销毁\nRESTORE 活动状态恢复中")]
        [ALHeader("活动展示状态配置列表")]
        public List<GGUIRankRushItemShowState> showStateList;
        [ALHeader("领奖状态配置列表")]
        public List<NPCommonGetStatInfo> getRewardStateInfoList;
        [ALHeader("排行榜提示tip的X偏移")]
        public float tipIntervalX;
        [ALHeader("排行榜提示tip的Y偏移")]
        public float tipIntervalY;
        [ALHeader("冲榜初始值展示")]
        public Text txtInitialValue;
        [ALHeader("不展示冲榜初始值时需要隐藏的GO列表")]
        public List<GameObject> goNoInititalValueHideList;

        [ALInfo("====跨服冲榜相关配置====")]
        [ALHeader("参与区服列表描述")]
        public Text txtServerList;
        [ALHeader("参与区服详情按钮")]
        public GameObject btnServerListDetail;
        [ALHeader("参与区服列表详情tip的偏移")]
        public float serverListInterval;
        [ALHeader("跨服活动时需要显示的GO列表")]
        public List<GameObject> goCrossShowList;
        [ALHeader("跨服活动时需要隐藏的GO列表")]
        public List<GameObject> goCrossHideList;

        /// <summary>
        /// 获取CD文本颜色
        /// </summary>
        /// <param name="_state"></param>
        /// <returns></returns>
        public Color getCDTextColor(EActivityState _state)
        {
            if (showStateList == null)
                return Color.white;

            for (int i = 0; i < showStateList.Count; i++)
            {
                if (showStateList[i] != null && showStateList[i].activityState == _state)
                    return showStateList[i].cdTextColor;
            }
            return Color.white;
        }

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(3909); } }
        public static string objName { get { return UIResPathAssistant.getObjName(3909); } }
    }
}
