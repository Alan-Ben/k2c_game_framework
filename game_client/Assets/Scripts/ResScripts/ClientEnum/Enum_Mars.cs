using UnityEngine;

namespace GOE
{
    public enum EMarsIntelligentControlState
    {
        NONE,
        [InspectorName("未解锁")]
        LOCK,
        [InspectorName("生效中")]
        EFFECTIVE,
        [InspectorName("冷却中")]
        COOLING_DOWN,
        [InspectorName("不能使用(不满足使用条件 或 需要满意值不足)")]
        CANNOT_USE,
        [InspectorName("可使用")]
        CAN_USE,
    }
    
    /// <summary>
    /// 火星基地 - 民意求助状态
    /// </summary>
    public enum EMarsPopularWillHelpState
    {
        NONE,
        [InspectorName("已处理")]
        HANDLED,
        [InspectorName("待处理")]
        WAIT_HANDLE
    }
    
    /// <summary>
    /// 火星科技类型
    /// </summary>
    public enum EMarsTechnologyType
    {
        NONE,
        [InspectorName("发展")]
        DEVELOPMENT,
        [InspectorName("战斗")]
        COMBAT,
    }

    /// <summary>
    /// 火星进入科技树页面参数类型
    /// </summary>
    public enum EMarsEnterTreeWndParamType
    {
        NONE,
        [InspectorName("科技类型")]
        TECHNOLOGY_TYPE,
        [InspectorName("科技ID")]
        TECHNOLOGY_ID,
        [InspectorName("科技技能类型")]
        TECHNOLOGY_SKILL_TYPE,
    }
}