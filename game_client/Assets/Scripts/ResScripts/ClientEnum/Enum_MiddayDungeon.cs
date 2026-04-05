using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 晚间活动状态
    /// </summary>
    public enum EMiddayDungeonActivityState
    {
        [InspectorName("活动未开始, 预览状态")]
        PREVIEW,
        [InspectorName("进行中")]
        ONGOING,
        [InspectorName("结束")]
        END,
    }
}