using System.Collections.Generic;
using UnityEngine;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 玩家装扮状态枚举
    /// </summary>
    public enum EPlayerInfoDressStateType
    {
        [InspectorName("LOCK（未解锁）")]
        LOCK,
        [InspectorName("UNLOCK_WEAR（已解锁已佩戴）")]
        UNLOCK_WEAR,
        [InspectorName("UNLOCK_NO_WEAR（已解锁未佩戴）")]
        UNLOCK_NO_WEAR,
    }

    /// <summary>
    /// 玩家装扮
    /// </summary>
    public class _ANPGGUIMonoPlayerInfoDressBasePage : _AALBasicUIWndMono
    {
        [ALHeader("佩戴按钮")]
        public GameObject btnWear;
        [ALHeader("不同使用状态显示的GO列表")]
        public List<NPCommonEnumStatInfo<EPlayerInfoDressStateType>> useStatusList;
    }
}

