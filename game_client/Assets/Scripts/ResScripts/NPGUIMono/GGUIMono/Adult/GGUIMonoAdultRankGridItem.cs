using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    [Serializable]
    public class GGUIMonoAdultRankGridItemItemList
    {
        [ALHeader("展示列表")]
        public List<GameObject> showList;
    }
    public class GGUIMonoAdultRankGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("成年人信息")]
        public GGUIMonoChildInfo monoAdultInfo;
        [ALInfo("按顺序表示第一名，第二名，第三名")]
        [ALHeader("特殊展示列表")]
        public List<GGUIMonoAdultRankGridItemItemList> listSpecialRankShow;
        [ALHeader("如果不是特殊展示名次的展示列表")]
        public List<GameObject> listNormalRankShow;
        [ALHeader("名次文本")]
        public Text txtRank;
        [ALHeader("查看详情按钮")]
        public GameObject btnDetail;


        public void setRank(int _rank)
        {
            ALUGUICommon.setLabelTxt(txtRank, _rank);
            ALUGUICommon.setGameObjEnable(listNormalRankShow, false);
            foreach (GGUIMonoAdultRankGridItemItemList unit in listSpecialRankShow)
                ALUGUICommon.setGameObjEnable(unit.showList, false);
            GGUIMonoAdultRankGridItemItemList specialShow = listSpecialRankShow.SafeGet(_rank - 1);
            ALUGUICommon.setGameObjEnable(specialShow != null ? specialShow.showList : listNormalRankShow, true);
        }
    }
}