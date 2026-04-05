using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoMarsBuildingExploreEntranceBtn : ALGGUIMonoCommonFollowItem
    {
        [ALHeader("探索入口按钮")]
        public GameObject btnExploreEntrance;
        [ALHeader("探索次数比例显示相关")]
        public float energyShowRatio = 0.5f;
        public List<GameObject> listEnergyRatioReachShow;
        
        
        public void refreshExploreEnergyShow(long _count, long _maxCount)
        {
            float ratio = _maxCount > 0 ? (float) _count / _maxCount : 0f;
            bool show = ratio >= energyShowRatio;
            ALUGUICommon.setGameObjEnable(listEnergyRatioReachShow, show);
        }
    }
}
