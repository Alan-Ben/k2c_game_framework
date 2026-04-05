package ActivitiesV01.Activities.TileMatchActivity.Game.Logic;

/**
 * 三消逻辑枚举
 */
public enum ETileMatchLogicEnum
{
    RESET,//初始化棋盘
    SWITCH,//交换逻辑
    COMBINE,//指定点连接检测逻辑
    ITEM_ACTIVE,//道具激活逻辑
    RAINBOW_ACTIVE,//彩虹道具激活逻辑
    RAINBOW_TRANS,//彩虹转换激活逻辑
    DROP,//下落
    FULL_MAP_CHECK,//下落
    DEAD_CHECK,//死局检测
    ITEM_GEN,//道具生成逻辑
    RAINBOW_RAINBOW,
    ROCKET_ROCKET,
    BOX_BOX,
    ROCKET_BOX,
}
