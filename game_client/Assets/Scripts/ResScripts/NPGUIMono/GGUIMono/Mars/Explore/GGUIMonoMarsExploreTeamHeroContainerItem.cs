using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoMarsExploreTeamHeroContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("大臣头像")]
        public RawImage imgHero;
        [ALHeader("大臣品质框")]
        public Image imgQualityHeadBg;
        [ALHeader("大臣实力")]
        public Text txtHeroPower;
        [ALHeader("各种状态下的显示内容")]
        public List<GameObject> listEmptyShow;
        public List<GameObject> listNormalShow;
        [ALHeader("大臣星级")]
        public GGUIMonoHeroCommonStar monoHeroStar;
        [ALHeader("队伍实力加成值（百分比）")]
        public Text txtTeamPowerBonus;
        [ALHeader("点击按钮")]
        public GameObject btnClick;
        
        public void setState(bool _isEmpty)
        {
            ALUGUICommon.setGameObjEnable(listEmptyShow, false);
            ALUGUICommon.setGameObjEnable(listNormalShow, false);
            ALUGUICommon.setGameObjEnable(_isEmpty ? listEmptyShow : listNormalShow, true);
        }
    }
}