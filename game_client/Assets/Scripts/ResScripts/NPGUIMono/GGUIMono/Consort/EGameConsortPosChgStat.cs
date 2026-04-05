/// <summary>
/// 妃位变化的状态枚举
/// </summary>
public enum EGameConsortPosChgStat
{
    CAN_CHG,//
    CHARM_LIMIT,//魅力值不足
    INTIMACY_LIMIT,//亲密度不足
    POS_COUNT_LIMIT,//妃位数量不足
    POS_MAX,//妃位已达最高
    POS_MIN,//妃位已达最低
}