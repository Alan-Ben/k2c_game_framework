using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoCommonTargetRewardShow : _AALBasicUIWndMono
    {
        [ALHeader("已领取需要显示的GO列表")]
        public List<GameObject> goAlreadyGetShowList;
        [ALHeader("已领取需要隐藏的GO列表")]
        public List<GameObject> goAlreadyGetHideList;
    }
}