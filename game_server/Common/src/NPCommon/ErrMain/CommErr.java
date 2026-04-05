package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 通用错误
 ****/
 
public class CommErr implements _IErrHolder
{
    public static final Result SYS_ERR = Result.constInit(10001,"系统错误");
    public static final Result REF_NOT_FOUND = Result.constInit(10002,"配表找不到");
    public static final Result SYS_BUSY = Result.constInit(10003,"系统忙");
    public static final Result UNKNOW_ERR = Result.constInit(10004,"未知错误");
    public static final Result PARAM_ERROR = Result.constInit(10005,"参数错误");
    public static final Result RPC_CALL_ERR = Result.constInit(10006,"远程调用失败");
    public static final Result CONSUME_FAIL = Result.constInit(10007,"消耗失败");
    public static final Result ITEM_NOT_ENOUGH = Result.constInit(10008,"物品不足");
    public static final Result FILE_READ_ERR = Result.constInit(10009,"文件读取错误");
    public static final Result PLAYER_NOT_FOUND = Result.constInit(10010,"玩家找不到");
    public static final Result SYSTEM_UNLOCK = Result.constInit(10011,"系统未开放");
    public static final Result PRICE_ERR = Result.constInit(10012,"价格配置错误");
    public static final Result NOT_CROSS_GROUP_ERR = Result.constInit(10013,"当前不是跨服分组");
    public static final Result PLAYER_CACHE_ERR = Result.constInit(10014,"玩家缓存错误");
    public static final Result REF_ERROR = Result.constInit(10015,"配表错误");
    public static final Result TIME_PRICE_CAL_ERR = Result.constInit(10017,"道具消耗计算错误");
    public static final Result NUM_REACH_LIMIT = Result.constInit(10018,"数量超过限制");
    public static final Result CONDITION_NOT_ENABLE = Result.constInit(10019,"不满足条件");
    public static final Result OP_DISABLE = Result.constInit(10020,"操作无效");
    public static final Result DATA_STATE_ERR = Result.constInit(10021,"数据对象状态变化引发的通用错误");
    public static final Result OBJ_ERR = Result.constInit(10022,"数据对象错误");
    public static final Result PROTOCOL_ERR = Result.constInit(10023,"通用协议处理错误");
    public static final Result CID_ILLEGAL = Result.constInit(10024,"CID格式错误");
    public static final Result US_NOT_FOUND = Result.constInit(10025,"US服务器找不到");
    public static final Result DATA_LOST = Result.constInit(10026,"数据已丢失");
    public static final Result PROCESS_TIMEOUT = Result.constInit(10027,"处理超时");
    public static final Result SING_CHECK_FAIL = Result.constInit(10028,"签名校验失败");
    public static final Result PARAM_NUM_ERROR = Result.constInit(10029,"参数数量错误");
    public static final Result PROTOCOL_BLOCKED = Result.constInit(10030,"系统暂未开放，请耐心等待");
    public static final Result REWARD_NOT_FOUND = Result.constInit(10031,"奖励物品未找到");
}
