using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 宴会席位加入的状态
    /// </summary>
    public enum EDinnerSeatJoinStat
    {
        [InspectorName("NONE:什么都不能做")]
        NONE,
        [InspectorName("SEATED:已入座的席位")]
        SEATED,
        [InspectorName("SEATEDING:自己已入座的席位")]
        SEATEDING,
        [InspectorName("CAN_SEAT:可以入座")]
        CAN_SEAT,
    }
}