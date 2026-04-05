using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 玩家卧室皮肤状态
    /// </summary>
    public enum EPlayerRoomSkinState
    {
        NONE,
        [InspectorName("未获得")]
        NOT_GAIN,
        [InspectorName("已拥有 不在使用中")]
        POSSESS_UNOCCUPIED,
        [InspectorName("已拥有 使用中")]
        POSSESS_IN_USE,
    }
    
    public class GGUIMonoPlayerRoomSkinItem : NPGGUIMonoCommonSelectItem
    {
        [ALHeader("皮肤图标")]
        public RawImage texIcon;
        
        [ALHeader("不同状态配置列表")]
        public List<NPCommonEnumStatMutexShowInfo<EPlayerRoomSkinState>> stateConfigList;

        [ALHeader("点击使用按钮")]
        public GameObject btnUse;
    }
}