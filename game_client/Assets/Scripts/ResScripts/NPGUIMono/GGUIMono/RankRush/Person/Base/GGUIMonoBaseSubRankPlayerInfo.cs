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
    public class GGUIMonoRankRushDetailRankSpecialShow
    {
        [ALHeader("排名")]
        public long rank;
        [ALHeader("需要特殊展示的GO")]
        public List<GameObject> goShowList;
    }

    /// <summary>
    /// 排行榜详情玩家信息附加窗口
    /// </summary>
    public class GGUIMonoBaseSubRankPlayerInfo : _TALUGUIMonoGridItem
    {
        [ALHeader("需要特殊展示的排名列表")]
        public List<GGUIMonoRankRushDetailRankSpecialShow> specialRankShowList;
        [ALHeader("普通排名需要展示的GO列表")]
        public List<GameObject> goNormalRankShowList;
        [ALHeader("头像")]
        public NPGGUIMonoPlayerIcon monoPlayerIcon;
        [ALHeader("分数")]
        public Text txtScore;
        [ALHeader("分数文本翻译key")]
        public string scoreTransKey;
        [ALHeader("排名")]
        public Text txtRank;
        [ALHeader("玩家详情按钮")]
        public GameObject btnDetail;
        [ALHeader("是自己时需要显示的GO列表")]
        public List<GameObject> goSelfShowList;
        [ALHeader("是自己时需要隐藏的GO列表")]
        public List<GameObject> goSelfHideList;
        [ALHeader("是自己时需要变色的文本列表")]
        public List<Text> txtSelfNeedChangeColorList;
        [ALHeader("是自己时文本的颜色")]
        public Color selfTextColor;
        [ALHeader("不是自己时文本的颜色")]
        public Color otherTextColor;
        [ALHeader("正在请求数据时需要显示的GO列表")]
        public List<GameObject> goReqInfoShowList;
        [ALHeader("正在请求数据时需要隐藏的GO列表")]
        public List<GameObject> goReqInfoHideList;
        [ALHeader("跨服活动时需要显示的GO列表")]
        public List<GameObject> goCrossShowList;
        [ALHeader("跨服活动时需要隐藏的GO列表")]
        public List<GameObject> goCrossHideList;


        /// <summary>
        /// 设置排名
        /// </summary>
        /// <param name="_rank"></param>
        public void setRank(long _rank)
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

            ALUGUICommon.setLabelTxt(txtRank, _rank);
            ALUGUICommon.setGameObjEnable(goNormalRankShowList, !isSpecial);
        }
    }
}