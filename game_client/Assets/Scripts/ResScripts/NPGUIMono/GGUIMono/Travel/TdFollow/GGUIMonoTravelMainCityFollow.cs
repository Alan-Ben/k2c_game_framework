using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoTravelMainCityFollow : ALGGUIMonoCommonFollowItem
    {
        [ALHeader("当前停靠展示go列表")]
        public List<GameObject> curParkingShowGoList;
        [ALHeader("停靠动画名")]
        public string parkingAniName;
        
        [ALHeader("首次进入动画名")]
        public string firstEnterAniName;
    }
}