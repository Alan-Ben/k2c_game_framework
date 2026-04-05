using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 排名特殊展示的配置
    /// </summary>
    [System.Serializable]
    public class GGUIMonoArenaRankSpecialShow
    {
        [ALHeader("排名")]
        public long rank;
        [ALHeader("需要特殊展示的GO")]
        public List<GameObject> goShowList;
    }

    /// <summary>
    /// 竞技场排行榜列表item
    /// </summary>
    public class GGUIMonoArenaRankGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("头像")]
        public NPGGUIMonoPlayerIcon monoPlayerIcon;
        [ALHeader("需要特殊展示的排名列表")]
        public List<GGUIMonoArenaRankSpecialShow> specialRankShowList;
        [ALHeader("排名")]
        public Text txtRank;
        [ALHeader("影响力")]
        public Text txtInfluence;


        /// <summary>
        /// 设置排名
        /// </summary>
        /// <param name="_rank"></param>
        public void setRank(long _rank)
        {
            if (specialRankShowList != null)
            {
                for (int i = 0; i < specialRankShowList.Count; i++)
                {
                    if (specialRankShowList[i] != null)
                    {
                        ALUGUICommon.setGameObjEnable(specialRankShowList[i].goShowList, specialRankShowList[i].rank == _rank);
                    }
                }
            }

            ALUGUICommon.setLabelTxt(txtRank, _rank);
        }
    }
}