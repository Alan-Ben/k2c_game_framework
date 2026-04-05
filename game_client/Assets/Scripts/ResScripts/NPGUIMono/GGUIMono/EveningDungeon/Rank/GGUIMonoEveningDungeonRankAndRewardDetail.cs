using System;
using System.Collections.Generic;
using Common.ActivityEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 页签类型
    /// </summary>
    public enum EEveningDungeonRankAndRewardDetailTabType
    {
        [InspectorName("REWARD（奖励界面）")]
        REWARD,
        [InspectorName("RANK（排行榜界面）")]
        RANK,
    }
    
    /// <summary>
    /// 晚间副本排行榜详情主界面页签
    /// </summary>
    [System.Serializable]
    public class GGUIEveningDungeonRankAndRewardDetailTabMono
    {
        [ALHeader("页签类型")]
        public EEveningDungeonRankAndRewardDetailTabType tabType;
        [ALHeader("通用页签脚本")]
        public NPGGUIMonoCommonTab monoTab;
        [ALHeader("页签对应的子窗口")]
        public NPCommonAssetPathInfo tabAssetPathInfo;
        [ALHeader("点击需要显示的GO列表")]
        public List<GameObject> goClickShowList;
        [ALHeader("点击需要隐藏的GO列表")]
        public List<GameObject> goClickHideList;
    }
    
    [Serializable]
    public class GGUIEveningDungeonActivityStateShow
    {
        [ALHeader("活动状态")]
        public EEveningDungeonActivityState activityState;
        [ALHeader("需要显示的GO列表")]
        public List<GameObject> goShowList;
        [ALHeader("倒计时文本颜色")]
        public Color cdTextColor;
    }
    
    /// <summary>
    /// 排行榜和奖励详情窗口
    /// </summary>
    public class GGUIMonoEveningDungeonRankAndRewardDetail : _ANPBasicUIWndResBarMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("页签列表")]
        public List<GGUIEveningDungeonRankAndRewardDetailTabMono> monoTabList;
        [ALHeader("页面父节点")]
        public Transform pageParent;
        [ALHeader("前几名玩家信息展示，排名按配置顺序")]
        public List<GGUIMonoEveningDungeonRankTopPlayerInfo> monoTopPlayerList;
        [ALHeader("倒计时")]
        public Text txtCD;
        [ALHeader("我的排名")]
        public Text txtMyRank;
        [ALHeader("我的分数")]
        public Text txtMyScore;
        [ALHeader("活动展示状态配置列表")]
        public List<GGUIEveningDungeonActivityStateShow> showStateList;

        /// <summary>
        /// 获取CD文本颜色
        /// </summary>
        /// <param name="_state"></param>
        /// <returns></returns>
        public Color getCDTextColor(EEveningDungeonActivityState _state)
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

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(5504); } }
        public static string objName { get { return UIResPathAssistant.getObjName(5504); } }
    }
}