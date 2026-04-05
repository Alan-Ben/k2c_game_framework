using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 火星科技状态
    /// </summary>
    public enum EMarsTechnologyState
    {
        NONE,
        [InspectorName("1 - 未解锁状态(自身0级 且 升级前置条件未达成)")]
        LOCK,
        [InspectorName("2 - 自身0级 且 升级前置条件已达成")]
        LEVEL_0_CONDITION_REACH,
        [InspectorName("3 - 自身非0级 且 升级前置条件未达成")]
        LEVEL_NOT0_CONDITION_NOTREACH,
        [InspectorName("4 - 普通状态(自身已经升级过 且 升级前置条件已经达成)")]
        NORMAL,
        [InspectorName("5 - 升级中状态")]
        UPGRADEING,
        [InspectorName("6 - 升级完成待确认状态")]
        UPGRADED,
        [InspectorName("7- 最高级状态")]
        MAX_LEVEL,
    }
    
    public class GGUIMonoMarsTechnologyInfo : _AALBasicUIWndMono
    {
        [ALHeader("科技图标")]
        public RawImage monoIcon;

        [ALHeader("科技名称")]
        public TextEx txtName;

        [ALHeader("科技描述")]
        public TextEx txtDesc;
        
        [ALHeader("科技等级")]
        public TextEx txtLvl;
        
        [ALHeader("科技状态显示信息列表")]
        public List<NPCommonEnumStatMutexShowInfo<EMarsTechnologyState>> stateShowInfoList;
        
        [ALHeader("可升级时显示列表")]
        public List<GameObject> canUpgradeShowList;
    }
}