using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoMarsExploreTeamHeroSelectGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("选中的序号")]
        public Text txtSelectNum;
        [ALHeader("玩家信息")]
        public GGUIMonoHeroCommonCardItem monoHeroCard;
        [ALHeader("选择状态的显示")]
        public List<GameObject> listSelectShow;
        public List<GameObject> listUnSelectShow;
        [ALHeader("队伍实力加成值（百分比）")]
        public Text txtTeamPowerBonus;


        public void setSelected(bool _isSelected)
        {
            ALUGUICommon.setGameObjEnable(listSelectShow, false);
            ALUGUICommon.setGameObjEnable(listUnSelectShow, false);
            ALUGUICommon.setGameObjEnable(_isSelected ? listSelectShow : listUnSelectShow, true);
        }
    }
}