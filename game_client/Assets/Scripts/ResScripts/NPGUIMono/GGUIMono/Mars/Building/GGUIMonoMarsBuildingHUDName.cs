using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoMarsBuildingHUDName : ALGGUIMonoCommonFollowItem
    {
        [ALHeader("建筑名称")]
        public Text txtName;
        public TextMeshProUGUIEx txtNameTMP;
        [ALHeader("建筑等级")]
        public Text txtLevel;
        public TextMeshProUGUIEx txtLevelTMP;
        [ALHeader("点击按钮")]
        public GameObject btnClick;
        [ALHeader("可升级时需要显示隐藏的内容")]
        public List<GameObject> listUpgradableShow;
        public List<GameObject> listUpgradableHide;


        public void setUpgradable(bool _upgradable)
        {
            ALUGUICommon.setGameObjEnable(listUpgradableShow, false);
            ALUGUICommon.setGameObjEnable(listUpgradableHide, false);
            ALUGUICommon.setGameObjEnable(_upgradable ? listUpgradableShow : listUpgradableHide, true);
        }
    }
}
