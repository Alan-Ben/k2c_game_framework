using UnityEngine;

namespace GOE
{
    public enum EWeekCardStat
    {
        [InspectorName("没有用过周卡")]
        NONE,//没有用过周卡
        [InspectorName("周卡周期内")]
        IN_WEEK_CARD,//周卡周期内
        [InspectorName("周卡已经过期")]
        EXPIREED,//周卡已经过期
    }
}