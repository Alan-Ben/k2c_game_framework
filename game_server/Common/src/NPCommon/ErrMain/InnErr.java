package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 旅店相关错误
 ****/
 
public class InnErr implements _IErrHolder
{
    public static final Result INN_MEDAL_LEVEL_MAX = Result.constInit(430001,"旅店勋章等级已达上限");
    public static final Result INN_LEVEL_NOT_ENOUGH = Result.constInit(430002,"旅店等级不足");
    public static final Result INN_DISH_LEVEL_REACH_MAX = Result.constInit(430003,"旅店菜品等级已达上限");
    public static final Result INN_DISH_FINESSE_NOT_ENOUGH = Result.constInit(430004,"旅店菜品熟练度不足");
    public static final Result INN_RECEIVE_GUEST_NOT_ENOUGH = Result.constInit(430007,"旅店接待客人数量不足");
    public static final Result INN_STATION_ALREADY_BUILT = Result.constInit(430008,"旅店设施已建造");
    public static final Result INN_STATION_LEVEL_REACH_MAX = Result.constInit(430009,"旅店设施等级已达上限");
    public static final Result INN_DISH_ALREADY_UNLOCKED = Result.constInit(430010,"旅店菜品已解锁");
    public static final Result INN_REQUIRE_STATION_NOT_EXIST = Result.constInit(430011,"旅店设施不存在");
    public static final Result INN_DISH_UNLOCK_CONDITION_NOT_MET = Result.constInit(430012,"旅店菜品解锁条件未满足");
    public static final Result INN_SPECIAL_GUEST_HAD_SERVE = Result.constInit(430013,"旅店特殊客人已服务过");
    public static final Result INN_HANDBOOK_REWARD_HAD_DRAW = Result.constInit(430014,"旅店图鉴奖励已领取");
    public static final Result INN_GUEST_NOT_BEEN_SERVE = Result.constInit(430015,"旅店客人未被服务");
    public static final Result INN_RECEIVE_CD = Result.constInit(430016,"旅店接待冷却中");
    public static final Result INN_NO_GUEST_CAN_RECEIVE = Result.constInit(430017,"旅店没有可接待的客人");
    public static final Result INN_NO_DISH_CAN_RECEIVE = Result.constInit(430018,"旅店没有可接待的菜品");
    public static final Result INN_NO_GUEST_CAN_SETTLE = Result.constInit(430019,"旅店没有可结算的客人");
    public static final Result INN_STATION_NOT_FOUND = Result.constInit(430021,"旅店设施未找到");
    public static final Result INN_DISH_NOT_FOUND = Result.constInit(430022,"旅店菜品未找到");
    public static final Result INN_SPECIAL_GUEST_MUSEUM_ITEM_HAD_DRAW = Result.constInit(430023,"旅店特殊客人博物馆物品已领取");
    public static final Result INN_GUEST_NOT_FOUND = Result.constInit(430024,"旅店客人未找到");
    public static final Result INN_SPECIAL_GUEST_NOT_FOUND = Result.constInit(430025,"旅店特殊客人未找到");
    public static final Result INN_DISH_RECIPE_NOT_GAINED = Result.constInit(430026,"旅店菜品菜谱未获得");
    public static final Result INN_GUEST_HAD_UNLOCK = Result.constInit(430027,"旅店客人已解锁");
    public static final Result INN_GUEST_UNLOCK_CONDITION_NOT_MEET = Result.constInit(430028,"旅店客人解锁条件未满足");
    public static final Result INN_INLINE_GUEST_NUM_LIMIT = Result.constInit(430029,"旅店队伍内客人数量已达上限");
}
