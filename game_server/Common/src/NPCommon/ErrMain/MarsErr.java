package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 火星系统错误
 ****/
 
public class MarsErr implements _IErrHolder
{
    public static final Result MARS_NOT_UNLOCK = Result.constInit(560001,"火星系统未解锁");
    public static final Result MARS_GO_ROUTE_NEXT_NOT_FIND = Result.constInit(560002,"前往火星下一个阶段未找到");
    public static final Result MARS_GO_ROUTE_PRE_NOT_FIND = Result.constInit(560003,"前往火星上一个阶段未找到");
    public static final Result MARS_GO_ROUTE_NEXT_ERROR = Result.constInit(560004,"前往火星下一个阶段错误");
    public static final Result MARS_GO_ROUTE_NEXT_SECS_NOT_FULL = Result.constInit(560005,"前往火星下一个阶段时间不足");
    public static final Result MARS_GO_ROUTE_NOT_DONE = Result.constInit(560006,"前往火星未完成");
    public static final Result MARS_GO_ROUTE_ALREADY_SENT_STAGE_MSG = Result.constInit(560007,"前往火星-当前阶段已发送过留言");
    public static final Result MARS_GO_ROUTE_STAGE_NOT_ALLOW_MSG = Result.constInit(560008,"前往火星-当前阶段不允许留言");
    public static final Result MARS_PEOPLE_IMMIGRANT_NOT_START = Result.constInit(560010,"火星居民-移民未开始");
    public static final Result MARS_PEOPLE_IMMIGRANT_NOT_END = Result.constInit(560011,"火星居民-之前的移民未结束");
    public static final Result MARS_PEOPLE_IMMIGRANT_NUM_ERROR = Result.constInit(560012,"火星居民-移民数量错误");
    public static final Result MARS_PEOPLE_IMMIGRANT_USED_LIMIT = Result.constInit(560013,"火星居民-进入移民次数已满");
    public static final Result MARS_PEOPLE_INTELLIGENT_NOT_FOUND = Result.constInit(560020,"火星居民-决策未找到");
    public static final Result MARS_PEOPLE_INTELLIGENT_NOT_UNLOCK = Result.constInit(560021,"火星居民-决策未解锁");
    public static final Result MARS_PEOPLE_INTELLIGENT_IN_COOLING = Result.constInit(560022,"火星居民-决策CD未结束");
    public static final Result MARS_PEOPLE_LETTER_NOT_FOUND = Result.constInit(560030,"火星居民-信件未找到");
    public static final Result MARS_PEOPLE_LETTER_DEALED = Result.constInit(560031,"火星居民-信件已处理");
    public static final Result MARS_PEOPLE_LETTER_NOT_DEAL = Result.constInit(560032,"火星居民-信件不能处理");
    public static final Result MARS_PEOPLE_HELP_NOT_FOUND = Result.constInit(560040,"火星居民-帮助未找到");
    public static final Result MARS_PEOPLE_HELP_NOT_REWARD = Result.constInit(560041,"火星居民-不是奖励型帮助");
    public static final Result MARS_PEOPLE_HELP_NOT_CHOICE = Result.constInit(560042,"火星居民-不是选择型帮助");
    public static final Result MARS_PEOPLE_HELP_DEALED = Result.constInit(560043,"火星居民-帮助已处理");
    public static final Result MARS_PEOPLE_HELP_OPTION_ERROR = Result.constInit(560044,"火星居民-选择型帮助的选项下标错误");
    public static final Result MARS_PEOPLE_IDLE_NOT_ENOUGH = Result.constInit(560045,"火星居民-没有足够的空闲人口");
    public static final Result MARS_BUILDING_NOT_FOUND = Result.constInit(560050,"火星建筑-未找到");
    public static final Result MARS_BUILDING_BUILT = Result.constInit(560051,"火星建筑-已建造");
    public static final Result MARS_BUILDING_BUILD_FAIL = Result.constInit(560052,"火星建筑-建造失败");
    public static final Result MARS_BUILDING_NOT_BUILD = Result.constInit(560053,"火星建筑-尚未建造");
    public static final Result MARS_BUILDING_UPGRADING = Result.constInit(560054,"火星建筑-正在创建（升级）中");
    public static final Result MARS_BUILDING_NOT_UPGRADING = Result.constInit(560055,"火星建筑-不在创建（升级）中");
    public static final Result MARS_BUILDING_UPGRADING_NOT_END = Result.constInit(560056,"火星建筑-未达到创建（升级）时间");
    public static final Result MARS_BUILDING_EQUIPMENT_NOT_FOUND = Result.constInit(560057,"火星建筑-部件未找到");
    public static final Result MARS_BUILDING_DISPATCH_LIMIT = Result.constInit(560058,"火星建筑-派遣人数超过上限");
    public static final Result MARS_BUILDING_PEOPLE_SUM_LIMIT = Result.constInit(560059,"火星建筑-达到建筑允许的人口上限");
    public static final Result MARS_BUILDING_EQUIPMENT_LVL_LIMIT = Result.constInit(560060,"火星建筑-部件等级到达上限");
    public static final Result MARS_BUILDING_EQUIPMENT_LVL_NOT_FULL = Result.constInit(560061,"火星建筑-部件等级未满级");
    public static final Result MARS_BUILDING_UPGRADING_LIMIT = Result.constInit(560062,"火星建筑-建造队列超过上限");
    public static final Result MARS_BUILDING_UPGRADE_DONE = Result.constInit(560063,"火星建筑-已完成升级");
    public static final Result MARS_TECH_NOT_FOUND = Result.constInit(560070,"火星科技-科技未找到");
    public static final Result MARS_TECH_PARENT_NOT_UNLOCK = Result.constInit(560071,"火星科技-前置科技未解锁");
    public static final Result MARS_TECH_UPGRADING = Result.constInit(560072,"火星科技-正在升级中");
    public static final Result MARS_TECH_NOT_UPGRADING = Result.constInit(560073,"火星科技-不在升级中");
    public static final Result MARS_TECH_UPGRADING_SECS_NOT_FULL = Result.constInit(560074,"火星科技-升级时间不够");
    public static final Result MARS_TECH_UPGRADING_LINES_FULL = Result.constInit(560075,"火星科技-升级队列不足");
    public static final Result MARS_TECH_UPGRADE_DONE = Result.constInit(560076,"火星科技-完成升级");
    public static final Result MARS_EXPLORE_UP_LVL_SUM_NOT_ENOUGH = Result.constInit(560080,"火星探索-探索次数不够");
    public static final Result MARS_EXPLORE_TEAM_NOT_FOUND = Result.constInit(560081,"火星探索队伍-队伍不存在");
    public static final Result MARS_EXPLORE_TEAM_NOT_UNLOCK = Result.constInit(560082,"火星探索队伍-尚未解锁");
    public static final Result MARS_EXPLORE_TEAM_STATE_ERROR = Result.constInit(560083,"火星探索队伍-队伍状态错误");
    public static final Result MARS_EXPLORE_TEAM_HERO_USED = Result.constInit(560084,"火星探索队伍-大臣已经被使用");
    public static final Result MARS_EXPLORE_TEAM_HERO_LIMIT = Result.constInit(560085,"火星探索队伍-大臣数量达到上限");
    public static final Result MARS_EXPLORE_TEAM_HERO_REPEAT = Result.constInit(560086,"火星探索队伍-大臣重复使用");
    public static final Result MARS_EXPLORE_EVENT_NOT_FOUND = Result.constInit(560087,"火星探索事件-事件不存在");
    public static final Result MARS_EXPLORE_EVENT_OCCUOIED = Result.constInit(560088,"火星探索事件-事件已经被占据");
    public static final Result MARS_EXPLORE_TEAM_TRANS_FAIL = Result.constInit(560089,"火星探索队伍-队伍切换状态失败");
    public static final Result MARS_EXPLORE_EVENT_TYPE_ERROR = Result.constInit(560090,"火星探索事件-事件类型错误");
    public static final Result MARS_EXPLORE_EVENT_NOT_DONE = Result.constInit(560091,"火星探索事件-事件未完成");
    public static final Result MARS_EXPLORE_NO_IDLE_POS = Result.constInit(560092,"火星探索事件-没有空闲位置");
    public static final Result MARS_EXPLORE_EVENT_HAD_REWARD = Result.constInit(560093,"火星探索事件-已领奖");
    public static final Result MARS_MINE_NOT_FOUND = Result.constInit(560094,"火星探索-未找到火星矿");
    public static final Result MARS_MINE_EXPIRED = Result.constInit(560095,"火星探索-火星矿已过期");
    public static final Result MARS_MINE_HAD_OCCUPIED = Result.constInit(560096,"火星探索-火星矿已占领");
    public static final Result MARS_MINE_ATTACK_FAIL = Result.constInit(560097,"火星探索-火星矿进攻失败");
    public static final Result MARS_MINE_RES_EMPTY = Result.constInit(560098,"火星探索-火星矿无资源");
    public static final Result MARS_TEAM_LOSS_NOT_EMPTY = Result.constInit(560099,"火星探索队伍-尚未修复队伍损耗");
    public static final Result MARS_TEAM_LOSS_EMPTY = Result.constInit(560100,"火星探索队伍-队伍无损耗");
    public static final Result MARS_TEAM_NOT_OCCUPY = Result.constInit(560101,"火星探索队伍-无占领火星矿");
    public static final Result MARS_BAG_ITEM_RED_TIME_TYPE_ERR = Result.constInit(560102,"火星减少时间道具-道具类型不支持");
    public static final Result MARS_MINE_OTHER_PLAYER_OCCUPY = Result.constInit(560103,"火星探索-火星矿被其他玩家占领");
    public static final Result MARS_MINE_COLLECT_SPEED_ERROR = Result.constInit(560104,"火星探索-火星矿采集速度错误");
    public static final Result MARS_MINE_OTHER_PLAYER_FORWARD = Result.constInit(560105,"火星探索-火星矿有其他玩家前往");
    public static final Result MARS_MINE_ALREADY_OCCUPY = Result.constInit(560106,"火星探索-已经占有");
    public static final Result GUILD_MARS_MINE_SHARE_NOT_FOUND = Result.constInit(560107,"火星公会分享矿-未找到分享记录");
    public static final Result MARS_MINE_ALREADY_OCCUPY_GUILD_MATE = Result.constInit(560108,"火星探索-公会成员已占领该矿");
    public static final Result MARS_TEAM_LOSS_NUM_ERROR = Result.constInit(560109,"火星探索队伍-准备修复的伤兵数超过当前已有的伤兵数");
}
