package NPCommon.Enum;


/********************
 * UsL服务器参数枚举
 *
 * @author Administrator
 *

 */
public enum EUsParam
{
    LAST_RUN_DAY, //0==最后运行日期
    SERVER_FIRST_LAUNCH_DAY, //1==服务器首次启动日期
    SERVER_FIRST_LAUNCH_TIME, //2==服务器首次启动时间
    SERVER_LAUNCH_TIME, //3==服务器启动时间（毫秒）
    MIDDAY_DUNGEON_ROUND_START_TIME_MS, //4==午间副本开始时间（毫秒）
    MIDDAY_DUNGEON_ROUND_DROP_BOX_NUM, //5==午间副本掉落宝箱数量
    ADULT_POOL_GROUP_ID, //6==子嗣池分组ID
    DINNER_POOL_GROUP_ID, //7==宴会池分组ID
    GUILD_ID, //8==联盟ID
    AI_SERVICE_SERIAL, //9==AI服务序列号
    HERO_PLACE_SERIAL, //10==大臣放置序列号
    IS_ACTIVITY_SCHEDULE_INIT, //11==活动计划是否初始化
    GM_SERVER_START_DATE, //12==作弊设置开服时间 用于活动计划管理器使用
    ORDER_SERIAL, //13==订单序列号
    MARS_MINE_POOL_GROUP_ID,//14==火星矿产池分组ID
    HAD_MARS_POWER_RANK_OPENED,//15==是否开启过火星实力排行榜 0-否 1-是
    HAD_REPAIR_GUILD_ID,//16==是否修复过联盟ID 0-否 1-是
    MARS_GO_ROUTE_ARRIVE_COUNT,//17==累计抵达火星的玩家数量
    ;

}
