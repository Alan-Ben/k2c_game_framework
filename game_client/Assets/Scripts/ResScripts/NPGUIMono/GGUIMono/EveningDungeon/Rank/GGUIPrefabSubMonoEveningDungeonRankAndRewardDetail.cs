using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIPrefabSubMonoEveningDungeonRankAndRewardDetail : _AALBasicUIWndMono
    {
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

    }
}