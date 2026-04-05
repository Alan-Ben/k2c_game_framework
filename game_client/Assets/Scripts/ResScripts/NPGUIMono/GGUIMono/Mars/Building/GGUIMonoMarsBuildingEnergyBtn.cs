using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoMarsBuildingEnergyBtn : ALGGUIMonoCommonFollowItem
    {
        [ALHeader("获取能量按钮")]
        public GameObject btnGetEnergy;
        [ALHeader("能量积累进度条")]
        public Slider sldEnergyProgress;
        [ALHeader("达到general表mars_can_get_energy_progress_threshold配置的显示比例时显示")]
        public List<GameObject> listOnProgressThresholdShow;
        public List<GameObject> listOnProgressThresholdHide;
        [ALHeader("能量满时显示的 GO ")]
        public List<GameObject> listFullEnergyShow;
        public List<GameObject> listFullEnergyHide;
    }
}