using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 晚间活动状态
    /// </summary>
    public enum EEveningDungeonActivityState
    {
        [InspectorName("活动未开始, 预览状态")]
        PREVIEW,
        [InspectorName("进行中")]
        ONGOING,
        [InspectorName("结束")]
        END,
        [InspectorName("关闭")]
        CLOSE,
    }
    
    /// <summary>
    /// 晚间活动游戏状态
    /// </summary>
    public enum EEveningDungeonGameState
    {
        STOP,
        IDLE,
        [InspectorName("攻击状态")]
        ATTACKING,
        [InspectorName("等待boss复活状态")]
        WAITING_BOSS_REVIVE,
        [InspectorName("BOSS完全死亡状态(没有复活次数)")]
        BOSS_COMPLETELY_DEAD,
    }

    /// <summary>
    /// 晚间活动boss状态
    /// </summary>
    public enum EEveningDungeonBossState
    {
        NONE,
        IDLE,
        [InspectorName("受攻击表现状态, 一个表现过程, 不是持续状态")]
        BY_ATTACK,
        [InspectorName("死亡表现状态, 一个表现过程, 不是持续状态")]
        DEAD,
        [InspectorName("等待复活状态")]
        WAITING_REVIVE,
        [InspectorName("BOSS完全死亡状态(没有复活次数)")]
        BOSS_COMPLETELY_DEAD,
    }
}