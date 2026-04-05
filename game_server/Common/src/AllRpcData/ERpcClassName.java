package AllRpcData;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

/********************
 * 跨服事件定义表，只能新增，不能删除，否则会引起ID变化，导致前后版本服务器不兼容，特别是Pay服务器版本可能和Ps版本不一致。
 * @author Administrator
 *
 */
public enum ERpcClassName
{
    NONE(0),
    AllRPC_RegistServerHandler(1),
    AllExecGmCommand,
    PushPHPParamList,

    ChatRoomErr,//聊天房间-错误处理
    GetChatRoom,//聊天房间-获取聊天房间数据
    JoinChatRoom,//聊天房间-加入聊天房间
    QuitChatRoom,//聊天房间-退出聊天房间
    SendChatRoomMsg,//聊天房间-向聊天房间发消息

    //////////////////////////////////////////////
    US_BEGIN(10000),//US
    UsExample,//示例RPC
    ExecPlayerGmCommand, //执行玩家GM命令
    ExecServerGmCommand, //执行服务器GM命令

    SendPersonMarryApply,//发起子嗣联姻请求
    CancelPersonMarryApply,//取消子嗣联姻请求
    RefusePersonMarryApply,//拒绝子嗣联姻请求
    AgreePersonMarryApply, //同意子嗣联姻请求
    GetServerPoolAdult, //获取全服联姻池子嗣数据
    AgreeServerMarryApply, //同意全服联姻
    GetAdultIsMarried, //获取子嗣是否已婚

    UsGetDinnerIdx,//获取宴会索引数据
    UsGetDinnerInfo,//获取宴会详细数据
    UsJoinDinner,//参加宴会
    UsAddBeJoinedCount,//玩家宴会交互记录
    
    UsPlayerGainItemList,
    UsGetPlayerData,//获取玩家数据
    UsSendMail, //发送玩家邮件
    
    AgreeFriendApply,
    SendFriendApply,
    RemoveFriend,
    
    AddGraveNewInfo,//杰出者系统-增加新晋杰出者数据
    GetGravePlayerTitleRecordList,//杰出者系统-获取指定玩家的杰出者的称号列表
    
    MarsMineSettle,//火星探险-火星矿结算
    MarsMineOccupyResult,//火星探险-占领结果

    MarsMineInfoReq,    //请求火星矿的信息
    MarsMineHasOtherGuildAttacked,//是否有其他联盟的正在攻击
    MarsMineOccupyReq,//请求前往火星矿
    MarsMineLeave,//离开火星矿
    UsRemoveChatRoom,//移除聊天房间
    UsCrossTeamMemberAdd,//增加队伍成员
    UsNotifySyncGroupTeam,//通知US同步Group队伍成员数据
    UsNotifyRemoveGroup,//通知US移除Group数据（队伍解散时）
    MarsMineAddPVPLog,//火星矿PVP战报跨服添加
    MarsRallyJoinToWait,//集结加入-行军到达后切换等待集结状态
    MarsRallyJoinFailBack,//集结加入失败后遣返队伍
    MarsRallySInitedCheck,//启动阶段检查集结与成员是否存在
    UsGetActivityGroupPlayerInfo,//向US查询指定活动群组下玩家的数据


    //////////////////////////////////////////////
    GUILD_BEGIN(19000),//Guild
    GuildMemberInitState,//公会玩家初始化
    GuildMarsEnsureAutoHelp,//公会火星-自动互助添加
    GuildMarsRmvAutoHelp,//公会火星-自动互助移除
    GuildMarsEnsureAutoDeal_2C,//公会火星-自动互助处理
    GuildMemberOnlineState,//公会玩家在线状态同步
    GuildMemberHeroDispatchValueChg,//公会玩家派遣大臣值变化
    GuildMemberHeroDispatchInfoChg,//公会玩家派遣大臣信息变化
    GuildSimpleNameChg_2C,//公会缩写名称变更
    GuildRecalMemberBuilding_2C,//公会成员重新计算赚速
    GuildMemberRmv_2C,//公会成员重新计算赚速
    GuildMemberSettleGuildBox_2C,//公会成员重新计算赚速
    GuildJoin_2C,//公会成员加入的前置处理
    GuildAddPlayerRequest_2C,//增加玩家申请信息
    GuildRmvPlayerRequest_2C,//增加玩家申请信息
    GuildRmvGuildRequest,//增加公会的申请信息
    GuildRequestLoadRankData,//重新加载排行数据
    GuildLevelChg_2C,//公会等级变更
    GuildPlayerRequestDungeonLvl,//获取联盟副本等级
    GuildAddMarsBattleReport,//新增火星矿联盟战报
    GuildAddMarsMine,//分享火星矿到联盟
    GuildOnScoreChg,//联盟相关排行数据变动处理
    GuildGetDispatchHeroInfo,//获取玩家派遣到联盟的大臣信息
    GuildRmvMarsHelp,//移除火星互助数据
    GuildAddPlayerHelpCount_2C,//玩家增加互助次数
    GuildRefreshPlayerBox,//刷新玩家宝箱
    GuildAddGuildPoint,//增加公会宝箱活跃点
    GuildAddGuildBox,//增加公会宝箱
    
    //////////////////////////////////////////////
    HS_BEGIN(20000),//HS
    HSDDAlert,//钉钉预警


    //////////////////////////////////////////////
    PS_BEGIN(30000),//PS


    //////////////////////////////////////////////
    CS_BEGIN(40000),//Common Server
    GetPHPParamList,//获取平台参数

    //////////////////////////////////////////////
    RS_BEGIN(50000),//Room server

    //////////////////////////////////////////////
    LS_BEGIN(60000),//Login server
    
    //////////////////////////////////////////////
    MarryMatchS_BEGIN(70000),//联姻服务器
    MmsDiscardGroup,
    MmsAddMatchItem,
    MmsRemoveMatchItem,
    MmsGetMatchItemList,
    MmsUploadMatchItemList,

    //////////////////////////////////////////////
    DINNER_SERVER_BEGIN(80000),//宴会服务器
    DnsDiscardGroup,//销毁分组
    DnsAddDinner,//增加宴会
    DnsDelDinner,//移除宴会
    DnsGetDinnerIdxList,//获取宴会索引数据列表
    DnsGetDinnerInfoBySort,//获取排序规则的指定宴会数据
    DnsGetPreDinnerInfoBySort,//获取排序规则的前一个宴会数据
    DnsGetNextDinnerInfoBySort,//获取排序规则的前一个宴会数据
    DnsJoinDinner,//参加宴会

    //////////////////////////////////////////////
    CROSSTEAM_SERVER_BEGIN(90000),//组队服务器
    CTSGetTeam,//获取队伍
    CTSGetUsAllTeam,//获取队长是指定US的所有队伍列表
    CTSCreateTeam,//创建队伍
    CTSSetTeamApplyCond,//设置队伍申请条件
    CTSDissolveTeam,//解散队伍
    CTSJoinTeam,//加入队伍
    CTSQuitTeam,//退出队伍
    CTSQuitTeamGroup,//退出队伍分组
    CTSRegUsGroup,//注册US到队伍分组
    CTSGetPlayerTeam,//获取玩家的队伍信息
    CTSGetPlayerTeamBase,//获取玩家的队伍基础信息
    CTSGetPlayerTeamRoom,//获取玩家的队伍聊天房间ID
    CTSKickTeamPlayer,//踢出队伍成员
    CTSApplyJoinTeam,//申请加入队伍
    CTSGetTeamApplyList,//获取队伍申请列表
    CTSGetTeamPlayerApplyList,//获取玩家发起请求数据列表
    CTSAgreeTeamApply,//同意玩家入队申请
    CTSRefuseTeamApply,//拒绝玩家入队申请
    CTSTeamBroadcastMsg,//队伍成员广播消息
    CTSGetGLSTeamData,//获取GLS GroupMgr所需的指定队伍数据
    CTSGetGroupTeamList,//获取指定分组的分页队伍列表
    CTSSetTeamSetting,//修改队伍设置
    ;


    /*************************
     *public methods
     **************************/
    public int number()
    {
        return _mNumber;
    }

    public static ERpcClassName valueOf(int _number)
    {
        return _gValueMap.get(_number);
    }

    public static List<Integer> numbers()
    {
        return _gNumbers;
    }

    /*************************
     *private methods
     ***************************/
    private static int _gLastNumber = 0;
    private static Map<Integer, ERpcClassName> _gValueMap;
    private static ArrayList<Integer> _gNumbers;

    private static void setLastNumber(int _number)
    {
        _gLastNumber = _number;
    }

    private static int getLastNumber()
    {
        return _gLastNumber;
    }

    public static int maxNumber()
    {
        return getLastNumber();
    }

    private static void regItem(int _number, ERpcClassName e)
    {
        if (null == _gValueMap)
        {
            _gValueMap = new ConcurrentHashMap<Integer, ERpcClassName>();
        }
        if (null == _gNumbers)
        {
            _gNumbers = new ArrayList<Integer>();
        }
        _gValueMap.put(_number, e);
        _gNumbers.add(_number);
    }

    private int _mNumber;

    private ERpcClassName(int _number)
    {
        _mNumber = _number;
        setLastNumber(_number);
        regItem(_mNumber, this);
    }

    private ERpcClassName()
    {
        _mNumber = getLastNumber() + 1;
        setLastNumber(this._mNumber);
        regItem(_mNumber, this);
    }


}
