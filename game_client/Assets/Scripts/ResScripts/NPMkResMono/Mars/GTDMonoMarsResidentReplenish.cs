using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星居民补充状态
    /// </summary>
    public enum EMarsResidentReplenishState
    {
        None,
        [InspectorName("空闲状态")]
        Idle,
        [InspectorName("补充中")]
        Replenishing,
        [InspectorName("补充完成")]
        ReplenishComplete,
        [InspectorName("补充次数达到上限")]
        ReplenishCountLimit,
    }
    
    /// <summary>
    /// 居民补充
    /// </summary>
    public class GTDMonoMarsResidentReplenish : MonoBehaviour
    {
        [ALHeader("UI跟随的目标")]
        public Transform uiFollowTarget;
        [ALHeader("UI跟随的资源ID")]
        public int followUiAssetPathId;
        
        [ALHeader("点击区域")]
        public GTDCommonPosClickMono monoClick;
        
        [ALHeader("居民补充状态显示信息")]
        public List<NPCommonEnumStatMutexShowInfo<EMarsResidentReplenishState>> replenishStateShowList;
    }
}