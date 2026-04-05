using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟冲榜详情奖励页面列表item
    /// </summary>
    public class GGUIMonoGuildRankRushDetailRewardContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("名次描述")]
        public Text txtRankingDesc;
        [ALHeader("盟主奖励列表")]
        public NPGGUIMonoCommonItemContainer monoLeaderItemContainer;
        [ALHeader("成员奖励列表")]
        public NPGGUIMonoCommonItemContainer monoMemberItemContainer;
        [ALHeader("是自己联盟的名次区间时需要显示的GO列表")]
        public List<GameObject> goSelfRankShowList;
        [ALHeader("是自己联盟的名次区间时需要隐藏的GO列表")]
        public List<GameObject> goSelfRankHideList;
        [ALHeader("是参与奖时显示的GO列表")]
        public List<GameObject> goParticipationAwardShowList;
        [ALHeader("是参与奖时隐藏的GO列表")]
        public List<GameObject> goParticipationAwardHideList;
        [ALHeader("需要特殊展示的排名列表")]
        public List<GGUIMonoRankRushDetailRankSpecialShow> specialRankShowList;
        [ALHeader("普通排名需要展示的GO列表")]
        public List<GameObject> goNormalRankShowList;
        [ALHeader("奖励容器，用于刷新布局")]
        public RectTransform rewardContainer;

        /// <summary>
        /// 设置排名展示
        /// </summary>
        /// <param name="_rank"></param>
        public void setRankShow(long _rank)
        {
            bool isSpecial = false;
            if (specialRankShowList != null)
            {
                for (int i = 0; i < specialRankShowList.Count; i++)
                {
                    if (specialRankShowList[i] != null)
                    {
                        if (!isSpecial)
                            isSpecial = specialRankShowList[i].rank == _rank;
                        ALUGUICommon.setGameObjEnable(specialRankShowList[i].goShowList, specialRankShowList[i].rank == _rank);
                    }
                }
            }

            ALUGUICommon.setGameObjEnable(goNormalRankShowList, !isSpecial);
        }
    }
}
