using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 游历主场景地点mono
    /// </summary>
    public class GTDTravelMainPosItemMono : MonoBehaviour
    {
        [ALHeader("点击脚本")]
        public GTDCommonPosClickMono clickMono;
        [ALHeader("建筑动画")]
        public Animation animation;
        
        [ALHeader("解锁状态展示信息列表")]
        public List<NPCommonEnumStatMutexShowInfo<EGameCommonUnlockType>> unlockStatShowInfo;
        
        [ALHeader("飞机停靠位置")]
        public Transform aircraftParkingPos;
        
        [ALHeader("当前停靠展示go列表")]
        public List<GameObject> curParkingShowGoList;
        [ALHeader("停靠动画名")]
        public string parkingAniName;
        
        [ALHeader("UI跟随父节点")]
        public Transform uiFollowParent;

        [ALHeader("建筑名TextMeshPro")]
        public TextMeshPro txtBuildingName;
        
        [ALHeader("跟随UI资源路径Id")]
        public int followResPathId = -1;

        [ALHeader("首次进入动画名")]
        public string firstEnterAniName;
        
        [ALHeader("解锁动画名")]
        public string unlockAniName;
        
        [ALHeader("重复游历动画名")]
        public string repeatTravelAniName;
    }
}