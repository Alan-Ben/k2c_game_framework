using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 晚间副本排行榜奖励item
    /// </summary>
    public class GGUIMonoEveningDungeonRankRewardContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("名次描述")]
        public Text txtRankingDesc;
        [ALHeader("单名次描述key, 一个参数, 排名")]
        public string singleRankingDescKey;
        [ALHeader("范围名次描述key, 两个参数, 最小排名, 最大排名")]
        public string rangeRankingDescKey;
        [ALHeader("参与奖名次key描述, 不需要参数")]
        public string participationAwardRankingDescKey;
        
        [ALHeader("奖励列表")]
        public NPGGUIMonoCommonItemContainer monoItemContainer;
        [ALHeader("是自己的名次区间时需要显示的GO列表")]
        public List<GameObject> goSelfRankShowList;
        [ALHeader("是自己的名次区间时需要隐藏的GO列表")]
        public List<GameObject> goSelfRankHideList;
        [ALHeader("是参与奖时显示的GO列表")]
        public List<GameObject> goParticipationAwardShowList;
        [ALHeader("是参与奖时隐藏的GO列表")]
        public List<GameObject> goParticipationAwardHideList;
        [ALHeader("需要特殊展示的排名列表")]
        public List<GGUIMonoRankRushDetailRankSpecialShow> specialRankShowList;
        [ALHeader("普通排名需要展示的GO列表")]
        public List<GameObject> goNormalRankShowList;

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