using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoTravelPosFollow : ALGGUIMonoCommonFollowItem
    {
        [ALHeader("地点名称文本")]
        public TextEx txtName;
        
        [ALHeader("解锁状态展示信息列表")]
        public List<NPCommonEnumStatMutexShowInfo<EGameCommonUnlockType>> unlockStatShowInfo;
        
        [ALHeader("当前停靠展示go列表")]
        public List<GameObject> curParkingShowGoList;
        [ALHeader("停靠动画名")]
        public string parkingAniName;
        
        [ALHeader("解锁动画名")]
        public string unlockAniName;
        
        [ALHeader("重复游历动画名")]
        public string repeatTravelAniName;
    }
}