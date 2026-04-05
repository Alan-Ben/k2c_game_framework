// using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

public enum EGameCommonUnlockRewardType
{
    [InspectorName("未解锁")]
    LOCK,//未解锁
    [InspectorName("已解锁-已领取")]
    UNLOCK_HAS_GET,//已解锁-已领取
    [InspectorName("已解锁-未领取")]
    UNLOCK_UN_GET,//已解锁-未领取
}
//
// //游历地图列表item
// public class GGUIMonoTarvelMapItem:_AALBasicUIWndMono
// {
//     [ALHeader("查看按钮")]
//     public GameObject btnLook;
//     [ALHeader("领取解锁奖励按钮")]
//     public GameObject btnGetUnlockReward;
//     [ALHeader("地点icon")]
//     public RawImage icon;
//     [ALHeader("地点名")]
//     public TextEx txtPosName;
//     [ALHeader("npc、妃子加载容器")]
//     public GGUIMonoTravelMessActorContainer itemContainer;
//     [ALHeader("解锁状态显示配置")]
//     public List<NPCommonEnumStatInfo<EGameCommonUnlockRewardType>> statInfos;
//     [ALHeader("解锁表现动画名字")]
//     public string unlockAniName;
//     [ALHeader("未解锁置灰列表")]
//     public List<MaskableGraphic> lockGrayList;
// }