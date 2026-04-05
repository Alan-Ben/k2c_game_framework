package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 联盟相关错误
 ****/
 
public class GuildErr implements _IErrHolder
{
    public static final Result GUILD_NAME_EXIST = Result.constInit(370001,"联盟名称已存在");
    public static final Result GUILD_NOT_EXIST = Result.constInit(370002,"联盟不存在");
    public static final Result NOT_MEMBER_OF_GUILD = Result.constInit(370003,"不是联盟成员");
    public static final Result DONT_HAVE_PERMISSION = Result.constInit(370004,"没有权限");
    public static final Result GUILD_SIMPLE_NAME_EXIST = Result.constInit(370005,"联盟简称已存在");
    public static final Result JOIN_REQUEST_NOT_FOUND = Result.constInit(370006,"加入请求不存在");
    public static final Result JOIN_REQUEST_REACH_LIMIT = Result.constInit(370007,"加入请求已达上限");
    public static final Result PLAYER_ALREADY_IN_GUILD = Result.constInit(370008,"玩家已加入其它联盟");
    public static final Result GUILD_MEMBER_NUM_REACH_LIMIT = Result.constInit(370009,"联盟成员已达上限");
    public static final Result NOT_FIT_GUILD_JOIN_LIMIT = Result.constInit(370010,"不符合联盟加入条件");
    public static final Result GUILD_NOT_ALLOW_TO_JOIN = Result.constInit(370011,"联盟不允许加入");
    public static final Result NO_GUILD_TO_JOIN = Result.constInit(370012,"没有可加入的联盟");
    public static final Result GUILD_HAD_DISSOLVE = Result.constInit(370013,"联盟已解散");
    public static final Result MEMBER_NOT_FOUND = Result.constInit(370014,"成员不存在");
    public static final Result MEMBER_NOT_DEPUTY_LEADER = Result.constInit(370015,"不是副盟主");
    public static final Result STILL_HAVE_OTHER_MEMBER = Result.constInit(370016,"还有其他成员");
    public static final Result GUILD_POSITION_NOT_EXIST = Result.constInit(370017,"联盟职位不存在");
    public static final Result GUILD_CONTRIBUTION_NOT_ENOUGH = Result.constInit(370018,"联盟贡献度不足");
    public static final Result CANT_HANDLE_NON_GUILD_REQUEST = Result.constInit(370019,"不能处理非本盟的请求");
    public static final Result LEADER_NOT_FOUND = Result.constInit(370020,"找不到盟主");
    public static final Result GUILD_POSITION_NUM_REACH_LIMIT = Result.constInit(370021,"联盟职位已达上限");
    public static final Result STRING_LENGTH_OVER_LIMIT = Result.constInit(370022,"字符长度超过限制");
    public static final Result JOIN_GUILD_CD = Result.constInit(370023,"加入联盟冷却中");
    public static final Result NO_JOIN_REQUESTS_TO_PROCESS = Result.constInit(370024,"没有可以处理的加入请求");
    public static final Result IMPEACH_EVENT_CANT_OPERATE = Result.constInit(370025,"弹劾事件不能操作");
    public static final Result IMPEACH_EVENT_HAS_VOTE = Result.constInit(370026,"弹劾事件已经投票");
    public static final Result EVENT_NOT_FOUND = Result.constInit(370027,"事件不存在");
    public static final Result GUILD_LEADER_PROACTIVE_TRANSFER_CD = Result.constInit(370028,"盟主主动转让冷却中");
    public static final Result JOIN_REQUEST_ALREADY_EXIST = Result.constInit(370029,"重复申请加入");
    public static final Result GUILD_FULL = Result.constInit(370030,"联盟满员");
    public static final Result GUILD_RECRUIT_CD = Result.constInit(370031,"联盟公开招募冷却中");
    public static final Result GUILD_HAD_DRAW_CONSTRUCT_REWARD = Result.constInit(370032,"联盟已领取建设阶段奖励");
    public static final Result GUILD_HAD_DRAW_ACTIVE_BOX_LIMIT = Result.constInit(370033,"超过领取联盟活跃宝箱上限");
    public static final Result GUILD_HAD_DRAW_ACTIVE_BOX = Result.constInit(370034,"已领取联盟活跃宝箱");
    public static final Result GUILD_HAD_DRAW_GREAT_REWARD_LIMIT = Result.constInit(370035,"超过领取联盟大礼上限");
    public static final Result GUILD_ACTIVE_BOX_NOT_EXIST = Result.constInit(370036,"联盟活跃宝箱不存在");
    public static final Result NO_GUILD_GREAT_REWARD_CAN_DRAW = Result.constInit(370037,"没有可以领取的联盟大礼");
    public static final Result NO_GUILD_ACTIVE_BOX_CAN_DRAW = Result.constInit(370038,"没有可以领取的联盟活跃宝箱");
    public static final Result GUILD_ACTIVE_BOX_EXPIRED = Result.constInit(370039,"联盟活跃宝箱已过期");
    public static final Result GUILD_HAD_DRAW_GREAT_REWARD = Result.constInit(370040,"已领取联盟大礼");
    public static final Result GUILD_GREAT_REWARD_EXPIRED = Result.constInit(370041,"联盟大礼已过期");
    public static final Result GUILD_CONSTRUCT_REWARD_POINT_NOT_ENOUGH = Result.constInit(370042,"联盟建设阶段积分不足");
    public static final Result GUILD_HERO_DISPATCH_ATTR_LIMIT = Result.constInit(370043,"联盟大臣派遣属性上限");
    public static final Result GUILD_DUNGEON_ERR = Result.constInit(370050,"公会副本数据错误");
    public static final Result GUILD_DUNGEON_NOT_FOUND = Result.constInit(370051,"无法找到公会副本");
    public static final Result GUILD_DUNGEON_NOT_UNLOCK = Result.constInit(370052,"公会副本没有解锁");
    public static final Result GUILD_DUNGEON_AUTO_TIME_ERROR = Result.constInit(370053,"公会副本自动启动时间不符合要求");
    public static final Result GUILD_DUNGEON_NOT_START = Result.constInit(370054,"公会副本没有开启");
    public static final Result GUILD_DUNGEON_STARTED = Result.constInit(370055,"公会副本已开启");
    public static final Result GUILD_DUNGEON_START_FAIL = Result.constInit(370056,"公会副本开启失败");
    public static final Result GUILD_DUNGEON_SET_LVL_FAIL = Result.constInit(370057,"公会副本设置等级失败");
    public static final Result GUILD_DUNGEON_LVL_UPDATED = Result.constInit(370058,"公会副本等级已变化");
    public static final Result GUILD_DUNGEON_ATTACK_FAIL = Result.constInit(370059,"公会副本攻击失败");
    public static final Result GUILD_DUNGEON_MONSTER_NOT_FOUND = Result.constInit(370060,"公会副本怪物不存在");
    public static final Result GUILD_DUNGEON_FIGHT_HERO_FAIL = Result.constInit(370061,"公会副本出战大臣失败");
    public static final Result GUILD_DUNGEON_MONSTER_KILLED = Result.constInit(370062,"公会副本怪物已击杀");
    public static final Result GUILD_DUNGEON_MONSTER_PRE_MONSTER_NOT_FOUND = Result.constInit(370063,"公会副本怪物前置怪物不存在");
    public static final Result GUILD_DUNGEON_MONSTER_PRE_MONSTER_NOT_KILLED = Result.constInit(370064,"公会副本怪物前置怪物未击杀");
    public static final Result GUILD_DUNGEON_NOT_RECOVER_HERO = Result.constInit(370065,"公会副本出战大臣无需恢复");
    public static final Result GUILD_DUNGEON_RECOVER_LIMIT = Result.constInit(370066,"公会副本恢复次数超过上限");
    public static final Result GUILD_DUNGEON_MONSTER_NOT_KILLED = Result.constInit(370067,"公会副本怪物未击杀");
    public static final Result GUILD_DUNGEON_MONSTER_NOT_REWARD = Result.constInit(370068,"公会副本怪物无奖励");
    public static final Result GUILD_DUNGEON_MONSTER_GAINED_REWARD = Result.constInit(370069,"公会副本怪物已领奖");
    public static final Result GUILD_DUNGEON_REWARD_EMPTY = Result.constInit(370070,"公会副本奖励为空");
    public static final Result GUILD_MARS_HELP_BUILDING_NOT_FOUND = Result.constInit(370075,"公会火星求助建筑不存在");
    public static final Result GUILD_MARS_HELP_NOT_FOUND = Result.constInit(370076,"公会火星求助目标不存在");
    public static final Result GUILD_MARS_HELP_SELF = Result.constInit(370077,"公会火星求助自身数据");
    public static final Result GUILD_MARS_HELP_OBJ_EXISTED = Result.constInit(370078,"公会火星求助目标已发起");
    public static final Result GUILD_MARS_HELP_OBJ_FAIL = Result.constInit(370079,"公会火星求助目标不能求助");
    public static final Result GUILD_MARS_HELP_SEND_FAIL = Result.constInit(370080,"公会火星求助目标发起失败");
    public static final Result GUILD_BOX_TYPE_ERR = Result.constInit(370090,"公会宝箱类型错误");
    public static final Result GUILD_BOX_NOT_FOUND = Result.constInit(370091,"公会宝箱不存在");
    public static final Result GUILD_BOX_NOT_REWARD = Result.constInit(370092,"公会宝箱不存在可领取的宝箱");
}
