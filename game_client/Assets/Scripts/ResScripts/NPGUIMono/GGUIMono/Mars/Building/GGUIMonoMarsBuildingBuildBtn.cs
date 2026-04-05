using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoMarsBuildingBuildBtn : ALGGUIMonoCommonFollowItem
    {
        [ALHeader("建造按钮")]
        public GameObject btnBuild;
        
        [ALHeader("可建造时显示物体列表")]
        public List<GameObject> canBuildShowGoList;
    }
}