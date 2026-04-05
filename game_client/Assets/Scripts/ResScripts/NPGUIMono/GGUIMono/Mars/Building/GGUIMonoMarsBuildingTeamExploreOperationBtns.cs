using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoMarsBuildingTeamExploreOperationBtns : ALGGUIMonoCommonFollowItem
    {
        [ALHeader("详情按钮")]
        public GameObject btnDetail;

        [ALHeader("升级按钮")]
        public GameObject btnUpgrade;

        [ALHeader("加速按钮")]
        public GameObject btnSpeedUp;

        [ALHeader("修复按钮")]
        public GameObject btnRepair;

        [ALHeader("是否在升级的展示")]
        public List<GameObject> listUpgradingShow;
        public List<GameObject> listNormalShow;
        [ALHeader("联盟互助按钮")]
        public GameObject btnAssist;
        [ALHeader("可联盟求助需要显隐藏的物体列表")]
        public List<GameObject> canAssistShowGos;
        public List<GameObject> canAssistHideGos;
        
        
        public void setUpgradingState(bool _isUpgrading)
        {
            ALUGUICommon.setGameObjEnable(listUpgradingShow, false);
            ALUGUICommon.setGameObjEnable(listNormalShow, false);
            ALUGUICommon.setGameObjEnable(_isUpgrading ? listUpgradingShow : listNormalShow, true);
        }
    }
}
