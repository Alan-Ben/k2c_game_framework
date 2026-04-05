using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟排行榜详情玩家信息附加窗口
    /// </summary>
    public class GGUIMonoBaseSubRankGuildInfo : _TALUGUIMonoGridItem
    {
        [ALHeader("需要特殊展示的排名列表")]
        public List<GGUIMonoRankRushDetailRankSpecialShow> specialRankShowList;
        [ALHeader("普通排名需要展示的GO列表")]
        public List<GameObject> goNormalRankShowList;
        [ALHeader("联盟基础信息脚本")]
        public GGUIMonoGuildSubBaseInfo guildBaseInfoMono;
        [ALHeader("分数")]
        public Text txtScore;
        [ALHeader("分数文本翻译key")]
        public string scoreTransKey;
        [ALHeader("排名")]
        public Text txtRank;
        [ALHeader("联盟信息详情按钮")]
        public GameObject btnDetail;
        [ALHeader("成员分数详情按钮")]
        public GameObject btnMemberScoreDetail;
        [ALHeader("是自己联盟时需要显示的GO列表")]
        public List<GameObject> goSelfShowList;
        [ALHeader("是自己联盟时需要隐藏的GO列表")]
        public List<GameObject> goSelfHideList;
        [ALHeader("是自己联盟时需要变色的文本列表")]
        public List<Text> txtSelfNeedChangeColorList;
        [ALHeader("是自己联盟时文本的颜色")]
        public Color selfTextColor;
        [ALHeader("不是自己联盟时文本的颜色")]
        public Color otherTextColor;
        [ALInfo("建议层级关系：请求联盟数据显隐GO 为 解散联盟显隐GO 的父节点")]
        [ALHeader("正在请求联盟数据时需要显示的GO列表")]
        public List<GameObject> goReqInfoShowList;
        [ALHeader("正在请求联盟数据时需要隐藏的GO列表")]
        public List<GameObject> goReqInfoHideList;
        [ALHeader("联盟解散时需要显示的GO列表")]
        public List<GameObject> goDisbandShowList;
        [ALHeader("联盟解散时需要隐藏的GO列表")]
        public List<GameObject> goDisbandHideList;
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