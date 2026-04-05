using UnityEngine;

/// <summary>
/// 本地推送类型
/// </summary>
public enum ELocalPushType
{
    //枚举用于二进制处理，需小于64

    [InspectorName("MIDDAY_DUNGEON（午间活动）")]
    MIDDAY_DUNGEON,
    [InspectorName("EVENING_DUNGEON（晚间活动）")]
    EVENING_DUNGEON,
    [InspectorName("MARS_BUILDING_ENERGY_FULL（火星能源建筑能量满）")]
    MARS_BUILDING_ENERGY_FULL,
    [InspectorName("MARS_BUILDING_CONSTRUCT_UPGRADE_COMPLETE（火星建筑建造或升级完成）")]
    MARS_BUILDING_CONSTRUCT_UPGRADE_COMPLETE,
    [InspectorName("MARS_PEOPLE_IMMIGRANT_COUNT_RESET（火星招募居民次数重置）")]
    MARS_PEOPLE_IMMIGRANT_COUNT_RESET,
    [InspectorName("MARS_EXPLORE_TEAM_RETURN（火星队伍派遣返回）")]
    MARS_EXPLORE_TEAM_RETURN,
}
