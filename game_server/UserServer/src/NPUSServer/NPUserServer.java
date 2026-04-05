package NPUSServer;

import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALServerAsynTask.ALAsynTaskManager;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._AALAsynCallAndBackTask;
import ALBasicServer.ALTask._IALAsynCallBackTask;
import ALBasicServer.ALTask._IALAsynCallTask;
import ALBasicServer.ALTask._IALAsynRunnableTask;
import ChatSystem.ChatRoomMgr;
import Common.NpServerObj.NpServerObj_SYS_ServerHoldInfo;
import MJLog.SynTask.SynTask_MJOnLineLog;
import NP2US_RB.p001_BasicOp.NP2US_RB_001_020_RetGuildMsg;
import NPCommon.CommonCache.Hero.Getter.PlayerHeroCacheGetter;
import NPCommon.CommonCache.Player.Getter.PlayerCacheGetter;
import NPCommon.Enum.EUsParam;
import NPCommon.Enum.NPCommonEnum;
import NPCommon.ErrMain.GuildErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.NPVersion;
import NPCommon.Util.CallBack._ICallBackIntT;
import NPCommon.Util.CommonFunc;
import NPCommon.Util._ABasicServerObj;
import NPEnum.ENPChatRoomType;
import NPEnum.ENPPlayerParam;
import NPEnum.EServerOnlineState;
import NPGameRes.LogicEvent.MetaData.EventMetaMgr_Refdata;
import NPGameRes.NPGRefdataCoreMgr;
import NPGameRes.NPRefDataMgr;
import NPGameRes.ServerVersionInfo;
import NPGameRes.UsHotRefDataMgr.ActivityHotRefDataMgr;
import NPServerProtocolWriter.NP2GS.Msg.NP2GS_B_Writer_001_BasicOp;
import NPServerProtocolWriter.NP2PS.Request.NP2PS_R_Writer_001_BasicOp;
import NPServerProtocolWriter.NP2US.Msg.NP2US_Writer_001_BasicOp;
import NPUSServer.ActivityPlan.ActivityPlanMgr;
import NPUSServer.AiService.AiServiceFunc;
import NPUSServer.Arena.ArenaCelebrityRankMgr;
import NPUSServer.Cache.Hero.UsPlayerHeroCacheGetterEnv;
import NPUSServer.Cache.Player.UsPlayerCacheGetterEnv;
import NPUSServer.ChatSys.ChatUserMgr;
import NPUSServer.CollectLikesMgr.CollectLikeMgr;
import NPUSServer.CommBoxMgr.CommBoxMgr;
import NPUSServer.Common.Event.Event_Place_Holder;
import NPUSServer.Common.Task.ActivityRefFileDownloadFunc;
import NPUSServer.CommonActivityMgr.CommonActivityMgr;
import NPUSServer.CommonActivityMgr.Factory.CommonActivityFactory;
import NPUSServer.CommonMarquee.CommonMarqueeMgr;
import NPUSServer.CommonMarquee.PHPMarqueeContentMgr;
import NPUSServer.CrossDataBasicPack.CrossDataCore;
import NPUSServer.DinnerMgr.DinnerPool;
import NPUSServer.Dungeon.Evening.EveningDungeonMgr;
import NPUSServer.Dungeon.Midday.MiddayDungeonMgr;
import NPUSServer.EarningsMarqueeMgr.EarningsMarqueeMgr;
import NPUSServer.FriendTip.FriendTipMgr;
import NPUSServer.GachaPublicRecord.GachaPublicRecordMgr;
import NPUSServer.GeneralV.EGeneralVType;
import NPUSServer.GeneralV.GeneralVMgr;
import NPUSServer.GiftPackLifeCycleMgr.GiftPackLifeCycleMgr;
import NPUSServer.GraveMgr.GraveMgr;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.Guild.GuildMgr;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter._ATGuildUserMsgRedirectCommiter;
import NPUSServer.GuildMsgDispather.NPUSGuildRequestDispather;
import NPUSServer.HotFixActivityMgr.HotFixActivityMgr;
import NPUSServer.MarsGoRouteMsgMgr.MarsGoRouteMsgMgr;
import NPUSServer.MatchAdultMgr.MatchAdultPool;
import NPUSServer.NPEvent.EventMgr.NPGlobalEventHandlerMgr;
import NPUSServer.NPGeneralListener.MsgDispather.NPUSGeneralMsgDispather;
import NPUSServer.NPGeneralListener.MsgDispather.USBroadMsgDispather;
import NPUSServer.NPGeneralListener.NPUSGeneralBasicServerListener;
import NPUSServer.NPGeneralListener.RequestDispather.NPUSGeneralRequestDispather;
import NPUSServer.NPGeneralListener.Writer.NP2US_RB_Writer_001_BasicOp;
import NPUSServer.NPUSUserMgr.GameSystem.MailSystem.AllServerMail.NPAllServerMailTemplateMgr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.NPUSUserMgr;
import NPUSServer.NPUSUserMgr.PlayerFreezeMgr.PlayerFreezeMgr;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUSUserMgr.UserNameEqualMgr.UserNameEqualMgr;
import NPUSServer.NPUserMsgDispather.NPUserMsgDispatcher;
import NPUSServer.ProtocolShieldMgr.ProtocolShieldMgr;
import NPUSServer.QuestionnaireMgr.QuestionnaireMgr;
import NPUSServer.QueueMgr.QueueMgr;
import NPUSServer.RankFixedMgr.RankFixedMgr;
import NPUSServer.RankGift.RankGiftPackMgr;
import NPUSServer.RankPlayerDataMgr.RankFixedObjDataListMgr;
import NPUSServer.RankingEvent.USRankingEventEnv;
import NPUSServer.RankingEvent.USRankingEventFunc;
import NPUSServer.RankingInstance.USRankingInstanceFunc;
import NPUSServer.RefuseMarryMgr.RefuseMarryMgr;
import NPUSServer.ServerCallback.NPUS_RBDealerRegUSServer;
import NPUSServer.ServerDeplomacy.ServerDeplomacyCore;
import NPUSServer.ShieldCidMgr.ShieldCidMgr;
import NPUSServer.StageGoalFirstReachMgr.StageGoalFirstReachMgr;
import NPUSServer.StepReward.StepRewardListMgr;
import NPUSServer.SynTask.USGetPayCallbackListTask;
import NPUSServer.SynTask.USServerHSPlatformInfoSynTask;
import NPUSServer.SynTask.USServerServerStateSynTask;
import NPUSServer.SynTask.USServerUpdateHoldInfoToCSSynTask;
import NPUSServer.TeamActivityMgr.TeamActivityMgr;
import NPUSServer.Tower.TowerMgr;
import NPUSServer.USGroup.LocalActivityController.LocalActivityController;
import NPUSServer.USGroup.LocalCrossServerGroupMgr;
import NPUSServer.USRank.TreasureHuntOreRank.TreasureHuntRankMgr;
import NPUSServer.USRank.USRankListMgr;
import NPUSServer.UsActivityScheduleMgr.UsActivityScheduleMgr;
import NPUSServer.UsMars.MineCore.UsMarsMineCore;
import NPUSServer.UsMars.UsMarsAction.UsMarsActionCore;
import NPUSServer.UserCounterMgr.UserCounterMgr;
import NPUSServer.UserIndexMgr.UserIndexMgr;
import NPUSServer.UserOfflineTmpDataMgr.AdultInfo.UsOfflineTmpDataMgr_AdultInfo;
import NPUSServer.UserOfflineTmpDataMgr.PlayerCacheInfo.UsOfflineTmpDataMgr_PlayerCache;
import NPUSServer.UserOfflineTmpDataMgr.PlayerHeroListInfo.UsOfflineTmpDataMgr_HeroListCache;
import NPUSServer.UserOfflineTmpDataMgr.UsOfflineTmpDataCore;
import NPUSServer.UserServerLoaderMgr.UsLoaderMgr;
import RPC.RpcSender;
import RPC._ARPCData;
import RPC._IRpcDealer;
import RankingEvent.RankingEventMgr;
import USDB.UserDBConf;
import USDB.UserDBInitializer;
import USLOGDB.UserLogDBConf;
import USLOGDB.UserLogDBInitializer;
import USServer.RPCDispatcher.USLocalRPCListener;
import USServer.RPCDispatcher.UsRpcDispatcher;
import WCGBasicServer.WCGBSRecieverListener._AWCGBSReceiverListener;
import WCGBasicServer.WCGPSClientListener.WCGPSRequestCommiter;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;

import java.nio.ByteBuffer;
import java.util.ArrayList;

/**************
 * 登录服务器的服务器处理对象
 * @author Administrator
 *
 */
public class NPUserServer extends _ABasicServerObj implements _IRpcDealer
{
    //开启服务器的序号
    private int _m_iServerIdx;
    private int _m_iServerTypeId;

    private UsLoaderMgr _m_serverLoaderMgr = new UsLoaderMgr(this); //服务器启动加载器

    private RpcSender _m_rpc2UserServer = null;
    private RpcSender _m_rpc2MarryMatchServer = null;
    private RpcSender _m_rpc2DinnerServer = null;
    private RpcSender _m_rpc2CrossTeamServer = null;
    private RpcSender _m_rpc2GameLogicServer = null;

    private USLocalRPCListener _m_mLocalRPCListener = new USLocalRPCListener(this);

    //US登录状态
    private int _m_usOnlineState;
    //US对外展示状态
    private int _m_usShowState;
    //US开服时间
    private String _m_usStartDate = "";
    //是否完成初始化处理
    private boolean _m_hasDealInit;

    ///后续是相关服务器配置的读取部分
    private UserBasicServerConf _m_cBasicServerConf;
    private UserDBConf _m_cUserDBConf;
    private UserLogDBConf _m_cUserLogDBConf;
    ///相关配置读取部分结束

    ///后续是整合进来的单例对象
    private UserDBInitializer _m_dbInitializer;
    private UserLogDBInitializer _m_logDBInitializer;

    //跨服数据管理器
    private CrossDataCore _m_cdcCrossDataCore;

    private QueueMgr _m_qmQueueMgr;
    private UserIndexMgr _m_uimUserIdxMgr;
    private USParams _m_pUSParams;
    private GeneralVMgr _m_gvGernalVMgr;
    private FriendTipMgr _m_ftmFrirendTipMgr;
    private UsActivityScheduleMgr _m_asmActivityScheduleMgr;
    private USRankListMgr _m_rlmRankListMgr;
    private RankFixedMgr _m_rfmRankFixedMgr;
    private RankFixedObjDataListMgr _m_rfmRankFixedObjDataListMgr;
    private CommonActivityMgr _m_commonActivityMgr;

    //US服务器上的所有聊天房间管理器
    private ChatRoomMgr _m_crmChatRoomMgr;
    //US服务器上在线聊天用户管理器
    private ChatUserMgr _m_cumChatUserMgr;
    //全服聊天房间数据
    private UsChatRoomInfo _m_ucrUsChatRoomInfo;

    private UserCounterMgr _m_ucmUserCounterMgr;
    private PlayerFreezeMgr _m_pfmPlayerFreezeMgr;
    private CommBoxMgr _m_cbmCommBoxMgr;
    private UserNameEqualMgr _m_unmUserNameEqualMgr;
    private LocalCrossServerGroupMgr _m_csgmLocalCrossServerGroupMgr;
    private MatchAdultPool _m_mgpMatchAdultPool;
    private DinnerPool _m_dpDinnerPool;
    private StepRewardListMgr _m_srlmStepRewardListMgr;
    private NPAllServerMailTemplateMgr _m_smmAllServerMailTemplateMgr;
    private LocalActivityController _m_lacLocalActivityController;
    private USRankingInstanceFunc _m_rifRankingInstanceFunc;
    private USRankingEventFunc _m_refRankingEventFunc;

    private RankingEventMgr _m_remRankingEventMgr;
    private NPGlobalEventHandlerMgr _m_globalEventHandlerMgr;
    private PlayerCacheGetter _m_pcgPlayerCacheGetter;
    private PlayerHeroCacheGetter _m_playerHeroCacheGetter;
    private QuestionnaireMgr _m_questionnaireMgr;
    private CollectLikeMgr _m_collectLikeMgr;
    private GuildMgr _m_guildMgr;
    private GachaPublicRecordMgr _m_gachaPublicRecordMgr;
    private ArenaCelebrityRankMgr _m_arenaCelebrityRankMgr;
    private ActivityPlanMgr _m_activityPlanMgr;
    private MiddayDungeonMgr _m_middayDungeonMgr;
    private EveningDungeonMgr _m_eveningDungeonMgr;
    private AiServiceFunc _m_aiServiceFunc;
    private TowerMgr _m_towerMgr;
    private TreasureHuntRankMgr _m_treasureHuntRankMgr;
    private GiftPackLifeCycleMgr _m_giftPackLifeCycleMgr;
    private StageGoalFirstReachMgr _m_stageGoalFirstReachMgr;
    private EarningsMarqueeMgr _m_earningsMarqueeMgr;
    private RankGiftPackMgr _m_rankGiftPackMgr;

    //玩家离线时相关临时数据存储管理类
    private UsOfflineTmpDataCore _m_dcOfflineTmpDataCore;

    //玩家屏蔽CID数据
    private ShieldCidMgr _m_scmShieldCidMgr;

    //协议屏蔽管理器
    private ProtocolShieldMgr _m_psmProtocolShieldMgr;

    //拒绝联姻数据管理
    private RefuseMarryMgr _m_refuseMarryMgr;

    //跑马灯数据管理
    private CommonMarqueeMgr _m_mmMarqueeMgr;
    private PHPMarqueeContentMgr _m_pmcmPHPMarqueeContentMgr;
    
    //杰出者管理
    private GraveMgr _m_grmGraveMgr;
    
    //火星资源管理
    //火星矿管理器
    private UsMarsMineCore _m_mcMarsMineCore;
    //火星行为管理器
    private UsMarsActionCore _m_acMarsActionCore;
    //火星前往路线-阶段留言管理器
    private MarsGoRouteMsgMgr _m_marsGoRouteMsgMgr;

    //活动队伍与活动关系管理
    private TeamActivityMgr _m_tamActivityMgr;

    private NPUSUserMgr _m_umUserMgr;

    //外交相关
    private ServerDeplomacyCore _m_sdCore;

    ///单例对象结束

    ///后续是相关的消息处理对象
    private NPUserMsgDispatcher _m_usMsgDispatcher;
    private NPUSGeneralMsgDispather _m_gdGeneralMsgDispather;
    private NPUSGeneralRequestDispather _m_grGeneralRequestDispather;
    private USBroadMsgDispather _m_bmdBroadMsgDispather;
    private NPUSGuildRequestDispather _m_guildRequestDispather;
    ///消息处理对象结束

    protected NPUserServer(int _idx)
    {
        _m_iServerIdx = _idx;
        _m_iServerTypeId = UserServerConf.getInstance().getTypeId(_m_iServerIdx);

        _m_rpc2UserServer = new RpcSender(this, EServerType.USER);
        _m_rpc2MarryMatchServer = new RpcSender(this, EServerType.SINGLE, ENPSingleServerType.MARRY_MATCH.ordinal());
        _m_rpc2DinnerServer = new RpcSender(this, EServerType.SINGLE, ENPSingleServerType.DINNER.ordinal());
        _m_rpc2CrossTeamServer = new RpcSender(this, EServerType.SINGLE, ENPSingleServerType.CROSS_TEAM.ordinal());
        _m_rpc2GameLogicServer = new RpcSender(this, EServerType.GAME_LOGIC);

        //初始化相关设置对象
        _m_cBasicServerConf = new UserBasicServerConf(_m_iServerIdx, _m_iServerTypeId);
        _m_cUserDBConf = new UserDBConf(_m_iServerIdx, _m_iServerTypeId);
        _m_cUserLogDBConf = new UserLogDBConf(_m_iServerIdx, _m_iServerTypeId);

        //初始化单例对象
        _m_dbInitializer = new UserDBInitializer(this);
        _m_logDBInitializer = new UserLogDBInitializer(this);

        _m_cdcCrossDataCore = new CrossDataCore();
        _m_qmQueueMgr = new QueueMgr(this);
        _m_uimUserIdxMgr = new UserIndexMgr(this);
        _m_pUSParams = new USParams(this);
        _m_gvGernalVMgr = new GeneralVMgr(this);
        _m_ftmFrirendTipMgr = new FriendTipMgr(this);
        _m_asmActivityScheduleMgr = new UsActivityScheduleMgr(this);
        _m_rlmRankListMgr = new USRankListMgr(this);
        _m_rfmRankFixedMgr = new RankFixedMgr(this);
        _m_rfmRankFixedObjDataListMgr = new RankFixedObjDataListMgr(this);
        _m_commonActivityMgr = new CommonActivityMgr(this);

        _m_crmChatRoomMgr = new ChatRoomMgr(this);
        _m_cumChatUserMgr = new ChatUserMgr(this);
        _m_ucrUsChatRoomInfo = new UsChatRoomInfo(this, ENPChatRoomType.US_SERVER, getServerTypeId());

        _m_ucmUserCounterMgr = new UserCounterMgr(this);
        _m_pfmPlayerFreezeMgr = new PlayerFreezeMgr(this);
        _m_cbmCommBoxMgr = new CommBoxMgr(this);
        _m_unmUserNameEqualMgr = new UserNameEqualMgr(this);
        _m_csgmLocalCrossServerGroupMgr = new LocalCrossServerGroupMgr(this);
        _m_mgpMatchAdultPool = new MatchAdultPool(this);
        _m_dpDinnerPool = new DinnerPool(this);
        _m_srlmStepRewardListMgr = new StepRewardListMgr(this);
        _m_smmAllServerMailTemplateMgr = new NPAllServerMailTemplateMgr(this);
        _m_lacLocalActivityController = new LocalActivityController(this);
        _m_rifRankingInstanceFunc = new USRankingInstanceFunc(this);
        _m_refRankingEventFunc = new USRankingEventFunc(this);

        _m_remRankingEventMgr = new RankingEventMgr();
        _m_globalEventHandlerMgr = new NPGlobalEventHandlerMgr();
        _m_pcgPlayerCacheGetter = new PlayerCacheGetter(this, new UsPlayerCacheGetterEnv(this));
        _m_playerHeroCacheGetter = new PlayerHeroCacheGetter(this, new UsPlayerHeroCacheGetterEnv(this));
        _m_dcOfflineTmpDataCore = new UsOfflineTmpDataCore(this);
        _m_scmShieldCidMgr = new ShieldCidMgr(this);
        _m_psmProtocolShieldMgr = new ProtocolShieldMgr(this);
        _m_refuseMarryMgr = new RefuseMarryMgr(this);

        _m_mmMarqueeMgr = new CommonMarqueeMgr(this);
        _m_pmcmPHPMarqueeContentMgr = new PHPMarqueeContentMgr(this);
        _m_questionnaireMgr = new QuestionnaireMgr(this);
        _m_collectLikeMgr = new CollectLikeMgr(this);
        _m_guildMgr = new GuildMgr(this);
        _m_gachaPublicRecordMgr = new GachaPublicRecordMgr(this);
        _m_arenaCelebrityRankMgr = new ArenaCelebrityRankMgr(this);
        _m_activityPlanMgr = new ActivityPlanMgr(this);
        _m_middayDungeonMgr = new MiddayDungeonMgr(this);
        _m_eveningDungeonMgr = new EveningDungeonMgr(this);
        _m_aiServiceFunc = new AiServiceFunc(this);
        _m_towerMgr = new TowerMgr(this);
        _m_treasureHuntRankMgr = new TreasureHuntRankMgr(this);
        _m_giftPackLifeCycleMgr = new GiftPackLifeCycleMgr(this);
        _m_stageGoalFirstReachMgr = new StageGoalFirstReachMgr(this);
        _m_earningsMarqueeMgr = new EarningsMarqueeMgr(this);
        _m_rankGiftPackMgr = new RankGiftPackMgr(this);

        _m_grmGraveMgr = new GraveMgr(this);

        _m_mcMarsMineCore = new UsMarsMineCore(this);
        _m_acMarsActionCore = new UsMarsActionCore(this);
        _m_marsGoRouteMsgMgr = new MarsGoRouteMsgMgr(this);

        _m_tamActivityMgr = new TeamActivityMgr(this);

        _m_umUserMgr = new NPUSUserMgr(this);

        _m_sdCore = new ServerDeplomacyCore(this);

        //初始化消息处理对象
        _m_usMsgDispatcher = new NPUserMsgDispatcher(this);
        _m_gdGeneralMsgDispather = new NPUSGeneralMsgDispather(this);
        _m_grGeneralRequestDispather = new NPUSGeneralRequestDispather(this);
        _m_bmdBroadMsgDispather = new USBroadMsgDispather(this);
        _m_guildRequestDispather = new NPUSGuildRequestDispather(this);
    }

    public int getServerIdx() {return _m_iServerIdx;}

    public String getStartDate()
    {
        return _m_usStartDate;
    }

    public int getUsOnlineState()
    {
        return _m_usOnlineState;
    }

    public int getUsShowState()
    {
        return _m_usShowState;
    }

    /**
     * 设置US的在线状态
     * @param _usOnlineState
     * @param _usShowState
     * @param _startDate
     */
    public void setUsState(int _usOnlineState, int _usShowState, String _startDate)
    {
        int oriState = _m_usOnlineState;
        String oriServerStartDate = _m_usStartDate;

        _m_usOnlineState = _usOnlineState;
        _m_usShowState = _usShowState;
        _m_usStartDate = _startDate;

        USLog.info(this, "UsStateChg onlineState:{} showState:{} startDate:{}", _usOnlineState, _usShowState, _startDate);

        //服务器关闭或维护时，强制所有玩家下线
        if (oriState == EServerOnlineState.OPEN.ordinal() &&
                (_m_usOnlineState == EServerOnlineState.CLOSED.ordinal() || _m_usOnlineState == EServerOnlineState.TEMP_CLOSED.ordinal()))
        {
            getUsUserMgr().forceKickAllUser();
        }

        //如果开服时间发生变化，则通知US用户管理器
        if (!oriServerStartDate.equals(_m_usStartDate))
            getUsUserMgr().onServerStartDateChg();
    }

    public RpcSender rpc2us() {return _m_rpc2UserServer;}
    public RpcSender rpc2marryMatch() {return _m_rpc2MarryMatchServer;}
    public RpcSender rpc2dinner() {return _m_rpc2DinnerServer;}
    public RpcSender rpc2crossTeam() {return _m_rpc2CrossTeamServer;}
    public RpcSender rpc2gameLogic() {return _m_rpc2GameLogicServer;}

    //获取配置对象
    public UserBasicServerConf getConf() { return _m_cBasicServerConf; }
    public UserDBConf getDBConf() { return _m_cUserDBConf; }
    public UserLogDBConf getLogDBConf() { return _m_cUserLogDBConf; }

    ///获取单例对象
    public UserDBInitializer getDBInitializer() { return _m_dbInitializer; }
    public UserLogDBInitializer getLogDBInitializer() { return _m_logDBInitializer; }

    public CrossDataCore getCrossDataCore() {return _m_cdcCrossDataCore;}
    public QueueMgr getQueueMgr() { return _m_qmQueueMgr; }
    public UserIndexMgr getUserIdxMgr() { return _m_uimUserIdxMgr; }
    public USParams getUSParams() { return _m_pUSParams; }
    public GeneralVMgr getGeneralVMgr() { return _m_gvGernalVMgr; }
    public FriendTipMgr getFriendTipMgr() { return _m_ftmFrirendTipMgr; }
    public UsActivityScheduleMgr getUsActivityScheduleMgr() { return _m_asmActivityScheduleMgr; }
    public USRankListMgr getRankListMgr() { return _m_rlmRankListMgr; }
    public RankFixedMgr getRankFixedMgr() { return _m_rfmRankFixedMgr; }
    public RankFixedObjDataListMgr getRankFixedObjDataListMgr() { return _m_rfmRankFixedObjDataListMgr; }
    public CommonActivityMgr getCommActivityMgr() {return _m_commonActivityMgr;}

    public ChatRoomMgr getChatRoomMgr() {return _m_crmChatRoomMgr;}
    public ChatUserMgr getChatUserMgr() {return _m_cumChatUserMgr;}
    public UsChatRoomInfo getUSChatRoomInfo() {return _m_ucrUsChatRoomInfo;}

    public UserCounterMgr getUserCounterMgr() {return _m_ucmUserCounterMgr;}
    public PlayerFreezeMgr getPlayerFreezeMgr() {return _m_pfmPlayerFreezeMgr;}
    public CommBoxMgr getCommBoxMgr() {return _m_cbmCommBoxMgr;}
    public UserNameEqualMgr getUserNameEqualMgr() {return _m_unmUserNameEqualMgr;}
    public LocalCrossServerGroupMgr getLocalCrossServerGroupMgr() {return _m_csgmLocalCrossServerGroupMgr;}
    public MatchAdultPool getMatchAdultPool() {return _m_mgpMatchAdultPool;}
    public DinnerPool getDinnerPool() {return _m_dpDinnerPool;}
    public StepRewardListMgr getStepRewardListMgr() {return _m_srlmStepRewardListMgr;}
    public NPAllServerMailTemplateMgr getAllServerMailTemplateMgr() {return _m_smmAllServerMailTemplateMgr;}
    public LocalActivityController getLocalActivityController() {return _m_lacLocalActivityController;}
    public USRankingInstanceFunc getRankingInstanceFunc() {return _m_rifRankingInstanceFunc;}
    public USRankingEventFunc getRankingEventFunc() {return _m_refRankingEventFunc;}
    public QuestionnaireMgr getQuestionnaireMgr() {return _m_questionnaireMgr;}
    public CollectLikeMgr getCollectLikeMgr() {return _m_collectLikeMgr;}
    public GuildMgr getGuildMgr() {return _m_guildMgr;}
    public GachaPublicRecordMgr getGachaPublicRecordMgr() {return _m_gachaPublicRecordMgr;}
    public ArenaCelebrityRankMgr getArenaCelebrityRankMgr() {return _m_arenaCelebrityRankMgr;}
    public ActivityPlanMgr getActivityPlanMgr() {return _m_activityPlanMgr;}
    public MiddayDungeonMgr getMiddayDungeonMgr() {return _m_middayDungeonMgr;}
    public EveningDungeonMgr getEveningDungeonMgr() {return _m_eveningDungeonMgr;}
    public AiServiceFunc getAiServiceFunc() {return _m_aiServiceFunc;}
    public TowerMgr getTowerMgr() {return _m_towerMgr;}
    public TreasureHuntRankMgr getTreasureHuntRankMgr() {return _m_treasureHuntRankMgr;}
    public GiftPackLifeCycleMgr getGiftPackLifeCycleMgr() {return _m_giftPackLifeCycleMgr;}
    public StageGoalFirstReachMgr getStageGoalFirstReachMgr() {return _m_stageGoalFirstReachMgr;}
    public EarningsMarqueeMgr getEarningsMarqueeMgr() {return _m_earningsMarqueeMgr;}
    public RankGiftPackMgr getRankGiftPackMgr() {return _m_rankGiftPackMgr;}

    public RankingEventMgr getRankingEventMgr() { return _m_remRankingEventMgr; }
    public NPGlobalEventHandlerMgr getGlobalEventHandlerMgr() { return _m_globalEventHandlerMgr; }
    public PlayerCacheGetter getPlayerCacheGetter() {return _m_pcgPlayerCacheGetter;}
    public PlayerHeroCacheGetter getPlayerHeroCacheGetter() {return _m_playerHeroCacheGetter;}

    public UsOfflineTmpDataCore getOfflineTmpDataCore() {return _m_dcOfflineTmpDataCore;}
    public ShieldCidMgr getShieldCidMgr() {return _m_scmShieldCidMgr;}
    public ProtocolShieldMgr getProtocolShieldMgr() {return _m_psmProtocolShieldMgr;}
    public RefuseMarryMgr getRefuseMarryMgr() {return _m_refuseMarryMgr;}

    public CommonMarqueeMgr getMarqueeMgr() {return _m_mmMarqueeMgr;}
    public PHPMarqueeContentMgr getPHPMarqueeContentMgr() {return _m_pmcmPHPMarqueeContentMgr;}
    
    public GraveMgr getGraveMgr() {return _m_grmGraveMgr;}

    public UsMarsMineCore getMarsMineCore() {return _m_mcMarsMineCore;}
    public UsMarsActionCore getMarsActionCore() {return _m_acMarsActionCore;}
    public MarsGoRouteMsgMgr getMarsGoRouteMsgMgr() {return _m_marsGoRouteMsgMgr;}

    public TeamActivityMgr getTeamActivityMgr() {return _m_tamActivityMgr;}

    public NPUSUserMgr getUsUserMgr() {return _m_umUserMgr;}

    public ServerDeplomacyCore getServerDeplomacyCore() {return _m_sdCore;}

    //获取消息处理对象
    public NPUserMsgDispatcher getUSMsgDispatcher() {return _m_usMsgDispatcher;}
    public NPUSGeneralMsgDispather getGeneralMsgDispather() {return _m_gdGeneralMsgDispather;}
    public NPUSGeneralRequestDispather getGeneralRequestDispather() {return _m_grGeneralRequestDispather;}
    public USBroadMsgDispather getBroadMsgDispather() {return _m_bmdBroadMsgDispather;}
    public NPUSGuildRequestDispather getGuildRequestDispather() {return _m_guildRequestDispather;}

    /**
     * 返回服务器类型信息
     */
    @Override
    public int getServerType()
    {
        return EServerType.USER.ordinal();
    }

    @Override
    public int getServerTypeId() { return _m_iServerTypeId; }

    public UsLoaderMgr getLoaderMgr()
    {
        return _m_serverLoaderMgr;
    }

    public boolean isServerReady()
    {
        return _m_serverLoaderMgr.isAllLoad();
    } //服务器数据加载完成才返回true


    /**********
     * 在成功注册时调用的函数
     */
    @Override
    public void onPSRegSuc()
    {
        USLog.sys(this, "Plat Server Reg Suc!");
        
        //发送注册消息到平台服务器
        sendRequestToPlat(NP2PS_R_Writer_001_BasicOp.make_007_RegUS(), new NPUS_RBDealerRegUSServer(this));

        //服务器注册成功后，开启加载其它数据
        getLoaderMgr().startAsyncLoad();
        //同步负载信息给CS
        usServerUpdateHoldInfoToCS();
        //广播服务器版本信息给GS
        usServerBroadcastServerVersionToGS();
        //前往HS同步本服所在平台的平台信息
        synHSPlatformInfo();
        //向paycenter获取回调订单
        ALSynTaskManager.getInstance().regTask(new USGetPayCallbackListTask(this));

        //发送服务器状态到平台服务器
        if (!_m_hasDealInit)
        {
            _m_hasDealInit = true;

            //开启对服务器在线人数日志记录
            ALSynTaskManager.getInstance().regTask(new SynTask_MJOnLineLog(this));

            //全服邮件刷新
            getAllServerMailTemplateMgr().init();
            //开启房间管理器tick任务
            getQuestionnaireMgr().startTick();

            //排期管理器
            getUsActivityScheduleMgr().onInited();
            //本地活动调度管理器
            getLocalActivityController().onInited();
            //初始化常驻排行榜
            getRankFixedMgr().onInited();
            //活动计划管理器
            getActivityPlanMgr().run();
            //副本
            getEveningDungeonMgr().startTick();
            getMiddayDungeonMgr().startTick();

            //加载用户收益
            getStageGoalFirstReachMgr().loadUserEarnings();

            getRankGiftPackMgr().tick5Sec();
        }
    }

    /**********
     * 在从平台服务器断开连接时的事件函数
     */
    @Override
    public void onPSDisconnect()
    {
        USLog.sys(this, "Plat Server Disconnect!");
    }

    /******************
     * 处理平台服务器的自定义消息
     *
     * @author alzq.z
     * @time Feb 19, 2013 1:36:05 PM
     */
    @Override
    public void dealPlatServerCustomMsg(ByteBuffer _msg)
    {
        getGeneralMsgDispather().DealProtocol(null, _msg);
    }

    /******************
     * 处理平台服务器的请求
     *
     * @author alzq.z
     * @time Feb 19, 2013 1:36:05 PM
     */
    @Override
    public void dealPlatServerRequest(WCGPSRequestCommiter _commiter, ByteBuffer _msg)
    {
        getGeneralRequestDispather().DealProtocol(_commiter, _msg);
    }

    /******************
     * 根据对应服务器类型构造接受协议的处理对象的函数
     *
     * @author alzq.z
     * @time Feb 19, 2013 1:36:05 PM
     */
    @Override
    public _AWCGBSReceiverListener createBasicServerReceiverListener(int _serverType, int _serverTypeId)
    {
        //转化枚举类型，根据不同服务器进行不同的处理
        //用户服务器只针对连接服务器进行处理和监听
        EServerType serverType = EServerType.values()[_serverType];
        return new NPUSGeneralBasicServerListener(this, serverType, _serverTypeId);
    }

    /***************
     * 初始化接口函数，返回初始化成功或失败
     * @return
     */
    @Override
    protected boolean _init()
    {
        //设置时区
        CommonFunc.setTimeZone(UserServerConf.getInstance().getTimeZone());
        USLog.info(this, "Set Time Zone:" + UserServerConf.getInstance().getTimeZone());

        //加载服务器资源版本
        if (!ServerVersionInfo.getInstance().initLoadVersion())
        {
            USLog.fatal("can not load game res version!");
            return false;
        }

        //初始化事件元数据
        if (!EventMetaMgr_Refdata.getInstance().s_init(Event_Place_Holder.class.getPackage().getName()))
        {
            USLog.error("EventMetaMgr init failed!");
        }

        //初始化配表数据
        if (!NPRefDataMgr.getInstance().loadData(ENPUserServerAsynEnum.REF_RELOAD.ordinal()))
        {
            USLog.fatal("NPRefDataMgr init Fail!!!");
            return false;
        }
        //初始化客户端配置对象
        NPGRefdataCoreMgr.getInstance().init();

        //初始化数据库连接
        if (!getDBInitializer().initConnections(_m_cUserDBConf))//主库
        {
            USLog.fatal(this, "NP User Server DB Init connection Fail!!!");
            return false;
        }
        if (!getLogDBInitializer().initConnections(_m_cUserLogDBConf))//日志库
        {
            USLog.fatal(this, "NP User Server Log DB Init connection Fail!!!");
            return false;
        }

        //初始化数据库表 
        if (!getDBInitializer().initDB())//主库
        {
            USLog.fatal(this, "NP User Server DB Init table Fail!!!");
            return false;
        }
        if (!getLogDBInitializer().initDB())//日志库
        {
            USLog.fatal(this, "NP User Server Log DB Init table Fail!!!");
            return false;
        }

        //初始化服务器参数
        if (!getUSParams().initFromDB())
        {
            USLog.fatal(this, "NP User Server init NPUSParams Fail!!!");
            return false;
        }

        //初始化服务器Id管理器
        if (!getGeneralVMgr().s_init())
        {
            USLog.error(this, "GeneralVMgr init failed!");
            return false;
        }

        //加载热更活动包
        Result result = HotFixActivityMgr.getInstance().loadAll();
        if(!result.isSucc())
        {
            USLog.error("HotFixActivityMgr init failed!");
            result.log();
            return false;
        }

        //初始化好友推荐管理器
        if (!getFriendTipMgr().initFromDb())
        {
            USLog.error(this, "FriendTipMgr init failed!");
            return false;
        }

        //本地跨服分组管理器
        if (!getLocalCrossServerGroupMgr().initFromDB())
        {
            USLog.error(this, "LocalCrossServerGroupMgr init failed!");
            return false;
        }
        
        //初始化活动排期数据
        if(!getUsActivityScheduleMgr().initFromDB())
        {
            USLog.error(this, "UsZoneActivityScheduleMgr init failed!");
            return false;
        }

        //初始化排行榜事件系统
        getRankingEventMgr().init(new USRankingEventEnv(this));

        //排行榜
        //初始化数据库对象
        getRankListMgr().initDBObj();
        //由于活动系统需要排行榜数据，所以排行榜数据要在活动系统之前初始化
        if (!getRankListMgr().initFromDB())
        {
            USLog.error(this, "NPUSRankListMgr init failed!");
            return false;
        }

        //初始化常驻排行榜
        if (!getRankFixedMgr().initFromDB())
        {
            USLog.error(this, "RankFixedMgr init failed!");
            return false;
        }

        //初始化常驻排行榜额外数据
        if (!getRankFixedObjDataListMgr().initFromDB())
        {
            USLog.error(this, "RankFixedObjDataListMgr init failed!");
            return false;
        }

        //初始化注册活动对象
        if (!CommonActivityFactory.getInstance().s_init())
        {
            USLog.error(this, "CommonActivityFactory init failed!");
            return false;
        }

        //初始化礼包生命周期管理器
        if (!getGiftPackLifeCycleMgr().initFromDB())
        {
            USLog.error(this, "GiftPackLifeCycleMgr init failed!");
            return false;
        }

        //初始化活动实例数据
        if (!getCommActivityMgr().initFromDB())
        {
            USLog.error(this, "NPCommActivityMgr init failed!");
            return false;
        }
        
        //玩家计数数据管理
        if(!getUserCounterMgr().initFromDB())
        {
            USLog.error(this, "UserCounterMgr initFromDB failed!");
            return false;
        }

        //初始化外交相关处理
        if(!getServerDeplomacyCore().initFromDB())
        {
            USLog.error(this, "ServerDeplomacy initFromDB failed!");
            return false;
        }

        if (!getPlayerFreezeMgr().initFromDB())
        {
            USLog.error(this, "PlayerFreezeMgr initFromDB failed!");
            return false;
        }

        //US通用宝箱
        if (!getCommBoxMgr().initFromDB())
        {
            USLog.error(this, "NPCommBoxMgr init failed!");
            return false;
        }
        
        //初始化角色名占用
        if (!getUserNameEqualMgr().initFromDB())
        {
            USLog.error(this, "UserNameEqualMgr init failed!");
            return false;
        }

        //同步本服全服联姻池数据
        if(!getMatchAdultPool().init())
        {
            USLog.error(this, "MatchAdultPool init failed!");
            return false;
        }
        
        //本服宴会池数据加载
        if(!getDinnerPool().s_init())
        {
            USLog.error(this, "DinnerPool init failed!");
            return false;
        }

        //初始化活动阶段奖励数据
        if (!getStepRewardListMgr().initFromDB())
        {
            USLog.error(this, "StepRewardListMgr initFromDB failed!");
            return false;
        }

        //初始化跑马灯数据
        if(!getMarqueeMgr().sInit())
        {
            USLog.error(this, "MarqueeMgr init failed!");
            return false;
        }
        if(!getPHPMarqueeContentMgr().sInit())
        {
            USLog.error(this, "PHPMarqueeContentMgr init failed!");
            return false;
        }

        //US屏蔽CID数据
        if (!getShieldCidMgr().initFromDB())
        {
            USLog.error(this, "ShieldCidMgr init failed!");
            return false;
        }

        //协议屏蔽管理器
        if (!getProtocolShieldMgr().initFromDB())
        {
            USLog.error(this, "ProtocolShieldMgr init failed!");
            return false;
        }

        //拒绝联姻数据
        if (!getRefuseMarryMgr().initFromDB())
        {
            USLog.error(this, "RefuseMarryMgr init failed!");
            return false;
        }

        //初始化问卷管理器
        if (!getQuestionnaireMgr().s_init())
        {
            USLog.error(this, "QuestionnaireMgr init failed!");
            return false;
        }

        //初始化集赞管理器
        if (!getCollectLikeMgr().s_init())
        {
            USLog.error(this, "CollectLikeMgr init failed!");
            return false;
        }

        //初始化竞技场名人榜管理器
        if (!getArenaCelebrityRankMgr().s_init())
        {
            USLog.error(this, "ArenaCelebrityRankMgr init failed!");
            return false;
        }

        //初始化活动计划管理器
        if (!getActivityPlanMgr().s_init())
        {
            USLog.error(this, "ActivityScheduleMgr init failed!");
            return false;
        }

        //初始化联盟管理器
        if (!getGuildMgr().init())
        {
            USLog.error(this, "GuildMgr init failed!");
            return false;
        }

        //初始化抽卡公屏记录管理器
        if (!getGachaPublicRecordMgr().init())
        {
            USLog.error(this, "GachaPublicRecordMgr init failed!");
            return false;
        }

        //初始化午间副本管理器
        if (!getMiddayDungeonMgr().init())
        {
            USLog.error(this, "MiddayDungeonMgr init failed!");
            return false;
        }

        //初始化晚间副本宝箱管理器
        if (!getEveningDungeonMgr().init())
        {
            USLog.error(this, "EveningDungeonMgr init failed!");
            return false;
        }

        //初始化爬塔管理器
        if (!getTowerMgr().initFromDB())
        {
            USLog.error(this, "TowerMgr init failed!");
            return false;
        }

        //初始化太空寻宝排行榜管理器
        if (!getTreasureHuntRankMgr().initFromDB())
        {
            USLog.error(this, "TreasureHuntRankMgr init failed!");
            return false;
        }

        //杰出者系统数据加载
        if(!getGraveMgr().s_init())
        {
            USLog.error(this, "GraveMgr init failed!");
            return false;
        }

        //杰出者系统数据加载
        if(!getStageGoalFirstReachMgr().initFromDB())
        {
            USLog.error(this, "StageGoalFirstReachMgr init failed!");
            return false;
        }

        //赚速跑马灯管理器数据加载
        if(!getEarningsMarqueeMgr().initFromDB())
        {
            USLog.error(this, "EarningsMarqueeMgr init failed!");
            return false;
        }

        //本服火星矿产池数据加载
        if(!getMarsMineCore().s_init())
        {
            USLog.error(this, "MarsMineCore init failed!");
            return false;
        }
        //本服火星行为数据加载
        if(!getMarsActionCore().s_init())
        {
            USLog.error(this, "MarsActionCore init failed!");
            return false;
        }

        //火星前往路线阶段留言数据加载
        if (!getMarsGoRouteMsgMgr().init())
        {
            USLog.error(this, "MarsGoRouteMsgMgr init failed!");
            return false;
        }

        //钉钉预警初始化
        getDDAlert().setServer(this);

        //初始化离线缓存处理器
        _m_dcOfflineTmpDataCore.initRegUsOfflineTmpDataMgr_Unsafe(new UsOfflineTmpDataMgr_AdultInfo(_m_dcOfflineTmpDataCore));
        _m_dcOfflineTmpDataCore.initRegUsOfflineTmpDataMgr_Unsafe(new UsOfflineTmpDataMgr_PlayerCache(_m_dcOfflineTmpDataCore));
        _m_dcOfflineTmpDataCore.initRegUsOfflineTmpDataMgr_Unsafe(new UsOfflineTmpDataMgr_HeroListCache(_m_dcOfflineTmpDataCore));

        //初始化活动热更数据管理器
        ActivityHotRefDataMgr.getInstance().initFromLocalFolder(new ActivityRefFileDownloadFunc(), getDDAlert());

        //创建固定聊天房间
        getChatRoomMgr().regRoom(_m_ucrUsChatRoomInfo);

        //开始用户管理器定时处理
        getUsUserMgr().startTimeCheck();

        //记录服务器启动时间
        ensureServerLunchTime();

        //队伍活动关系数据管理
        getTeamActivityMgr().loadTeamActivityFromCTS();

        USLog.info(this, "[US Init Done]-[" + getServerTypeId() + "]");

        return true;
    }

    /******************
     * 处理平台服务器广播的自定义消息
     *
     * @author alzq.z
     * @time Feb 19, 2013 1:36:05 PM
     */
    @Override
    public void dealBroadcastCustomMsg(int _serverType, int _serverId, ByteBuffer _msg)
    {
        getBroadMsgDispather().DealProtocol(null, _msg);
    }

    /*****************
     * 是否直接发送消息到BasicServer，建议默认都使用false
     * @return
     */
    @Override
    public boolean isBSDirectlySend(int _serverType, int _serverTypeId)
    {
        return false;
    }

    /*******************
     * 缓存的总线消息数量过大的时候触发的报警处理
     * @param _msgCount
     */
    @Override
    public void dealCacheBusMsgAlert(int _msgCount)
    {
        USLog.fatal(this, "Bus Server Msg Cache for [" + _msgCount + "]");
    }

    /*******************
     * 尝试获取负载本服的总线服务器失败
     */
    @Override
    public void tryGetHandleBusServerFail()
    {
        USLog.fatal(this, "Try Get Handle Bus Server Fail!!!");
    }

    /*******************
     * 在发送请求到总线服务器的连接对象断开连接的时候的处理函数
     */
    @Override
    protected void _onBusServerSenderDisconnect(int _serverTypeId)
    {
        USLog.fatal(this, "Bus Server [" + _serverTypeId + "] Disconnect!!!");
    }

    /******************
     * 处理平台服务器的自定义消息
     *
     * @author alzq.z
     * @time Feb 19, 2013 1:36:05 PM
     */
    @Override
    public void dealUndealBusServerCustomMsg(int _busTypeId, ByteBuffer _msg)
    {

    }

    /******************
     * 处理平台服务器的请求
     *
     * @author alzq.z
     * @time Feb 19, 2013 1:36:05 PM
     */
    @Override
    public void dealUndealBusServerRequest(int _busTypeId, _IWCGBasicRequestCommiter _commiter, ByteBuffer _msg)
    {

    }

    /********************
     * 根据带入的标记，获取监控服务器请求本服务器的状态字符串
     * 空默认为全局状态
     * @return
     */
    @Override
    public String getMonitorServerStateStr(String _infoTag)
    {
        USMonitorInfo monitorInfo = new USMonitorInfo();
        monitorInfo.setOnlineState(EServerOnlineState.EServerOnlineState_FromInt(getUsOnlineState()));
        monitorInfo.setOpenGM(UserServerConf.getInstance().getOpenGM());
        monitorInfo.setCachedUserCount(getUsUserMgr().getAllCacheUserDataCount());
        monitorInfo.setOnlineUserCount(getUsUserMgr().getAllOnlineUserDataCount());
        monitorInfo.setTotalMemory(Runtime.getRuntime().totalMemory());
        monitorInfo.setMaxMemory(Runtime.getRuntime().maxMemory());
        monitorInfo.setFreeMemory(Runtime.getRuntime().freeMemory());
        monitorInfo.setVersion(NPVersion.getString());
        monitorInfo.setTimeZone(CommonFunc.getTimeZone().getDisplayName());
        monitorInfo.setServerType(EServerType.USER);
        monitorInfo.setServerTypeId(getServerTypeId());
        monitorInfo.setIp(getConf().getInternalIp());
        monitorInfo.setInnerPort(getConf().getPort());
        monitorInfo.setRefVersion(ServerVersionInfo.getInstance().getVersion());
        monitorInfo.setReadyRefVersion(NPRefDataMgr.getInstance().getRefDataReloader().getVersion());
        monitorInfo.setIsReadyRefLoading(NPRefDataMgr.getInstance().getRefDataReloader().isLoading());
        monitorInfo.setIsReadyRefReload(NPRefDataMgr.getInstance().getRefDataReloader().isReadyReload());
        monitorInfo.setPlatId(UserServerConf.getInstance().getPlatformId());
        monitorInfo.setAreaId(UserServerConf.getInstance().getPlatAreaId());
        monitorInfo.setDBVersion(getDBInitializer().getDBVersion());
        return monitorInfo.makeJsonObj().toString();
    }

    /**********
     * 允许服务器对数据进行转换
     * @param _srcTag
     * @return
     */
    @Override
    public NPCommonEnum.EDBTag switchDBTag(NPCommonEnum.EDBTag _srcTag)
    {
        //根据索引Id返回数据库标记
        if(_srcTag == NPCommonEnum.EDBTag.main)
        {
            return getDBInitializer().getDBTag();
        }
        else if(_srcTag == NPCommonEnum.EDBTag.us_log)
        {
            return getLogDBInitializer().getDBTag();
        }

        return _srcTag;
    }

    /**********
     * 根据标记获取执行的线程索引
     * @param _srcTag
     * @return
     */
    public int getDBThreadIdx(NPCommonEnum.EDBTag _srcTag)
    {
        //根据索引Id返回数据库标记
        if(_srcTag == NPCommonEnum.EDBTag.main)
        {
            return getDBInitializer().getThreadIdx();
        }
        else if(_srcTag == NPCommonEnum.EDBTag.us_log)
        {
            return getLogDBInitializer().getThreadIdx();
        }

        return ENPUserServerAsynEnum.USER_DB.ordinal();
    }

    /******
     * 返回本服所在的分区，欧洲区，亚洲区。。。。
     * @return
     */
    public long getServerAreaId()
    {
        return UserServerConf.getInstance().getPlatAreaId();
    }

    /******
     * 返回大区标识
     * @return
     */
    public int getRegionId()
    {
        return UserServerConf.getInstance().getPlatformId() * 100 + UserServerConf.getInstance().getPlatAreaId();
    }

    /*****
     * 返回服务器启动时间
     * @return
     */
    public int getLaunchTime()
    {
        return (int) (getUSParams().getParam(EUsParam.SERVER_LAUNCH_TIME) / 1000L);
    }

    private void ensureServerLunchTime()
    {

        long nowMs = CommonFunc.getNowTimeMS();
        getUSParams().setParam(EUsParam.SERVER_LAUNCH_TIME, nowMs);
        long serverStartTime = getUSParams().getParam(EUsParam.SERVER_FIRST_LAUNCH_TIME);
        if (serverStartTime == 0)
        {
            getUSParams().setParam(EUsParam.SERVER_FIRST_LAUNCH_DAY, CommonFunc.getNowTagYYYYMMDD());
            getUSParams().setParam(EUsParam.SERVER_FIRST_LAUNCH_TIME, nowMs);
        }
    }

    /**
     * 通过解析cid获取玩家所属游戏服服务器typeId的方法
     * @param _cid 玩家cid
     * @return 游戏服typeId
     */
    public int getUSServerTypeIdByParseCid(Long _cid)
    {
        return CommonFunc.parseServerTypeIdFromCid(_cid);
    }

    /**
     * 同步负载信息给CS
     */
    public void usServerUpdateHoldInfoToCS()
    {
        ALSynTaskManager.getInstance().regTask(new USServerUpdateHoldInfoToCSSynTask(this));
    }

    /**
     * 广播服务器版本信息给GS
     */
    public void usServerBroadcastServerVersionToGS()
    {
        broadcastMessage(EServerType.GATE.ordinal(),
                NP2GS_B_Writer_001_BasicOp.make_005_USVersionChg(getServerTypeId(), NPVersion.getString(), ServerVersionInfo.getInstance().getVersion()));
    }

    /**
     * 构造注册的负载信息
     * @return NpServerObj_SYS_ServerHoldInfo
     */
    public NpServerObj_SYS_ServerHoldInfo makeHoldInfo()
    {
        NpServerObj_SYS_ServerHoldInfo proto = new NpServerObj_SYS_ServerHoldInfo();
        proto.setServerTypeId(getServerTypeId());
        //cid序号值
        proto.setHoldCount((int)getGeneralVMgr().getVObj(EGeneralVType.CID).getV());
        proto.setTotalCount(UserServerConf.getInstance().getMaxUserCount());
        return proto;
    }

    /**
     * 前往CS同步本服服务器在线状态信息
     */
    public void synUSOnlineState()
    {
        ALSynTaskManager.getInstance().regTask(new USServerServerStateSynTask(this));
    }

    /**
     * 前往HS同步本服所在平台的平台信息
     */
    public void synHSPlatformInfo()
    {
        ALSynTaskManager.getInstance().regTask(new USServerHSPlatformInfoSynTask(this));
    }

    /***************
     * 直接的注册游戏数据库操作异步任务函数
     *
     * @author alzq.z
     * @time Apr 28, 2013 1:04:10 AM
     */
    public <T> void regUserDBAsynTask(_IALAsynCallTask<T> _callTask, _IALAsynCallBackTask<T> _CallBackTask)
    {
        ALAsynTaskManager.getInstance().regTask(NPUSMain.GetUserDBAsynThreadIdx(getServerIdx()), _callTask, _CallBackTask);
    }

    public <T> void regUserDBAsynTask(_AALAsynCallAndBackTask<T> _CallBackTask)
    {
        ALAsynTaskManager.getInstance().regTask(NPUSMain.GetUserDBAsynThreadIdx(getServerIdx()), _CallBackTask);
    }

    public void regUserDBAsynTask(_IALAsynRunnableTask _runnableTask)
    {
        ALAsynTaskManager.getInstance().regTask(NPUSMain.GetUserDBAsynThreadIdx(getServerIdx()), _runnableTask);
    }

    public String getServerName()
    {
        return "[USER-" + getServerTypeId() + "] ";
    }

    /**
     * 获取服务器开服天数
     * @return
     */
    public int getServerStartDay()
    {
        long serverStartDateMs = 0;
        //获取开服时间
        if (!getStartDate().isEmpty())
        {
            //查询平台配置
            serverStartDateMs = CommonFunc.simpleDateFormatTimeMs(getStartDate(), "yyyy-MM-dd");
        } else if (getUSParams().getParam(EUsParam.GM_SERVER_START_DATE) != 0)
        {
            //查询GM命令配置的服务器参数
            serverStartDateMs = CommonFunc.getZeroFromTimeTag((int) getUSParams().getParam(EUsParam.GM_SERVER_START_DATE));
        }

        if (serverStartDateMs <= 0)
            return 1;

        return (int) ((CommonFunc.getNowTimeMS() - serverStartDateMs) / (CommonFunc.DAY_SEC * 1000L)) + 1;
    }

    /**
     * 玩家抵达火星时调用，累计全服抵达人数并同步给所有在线玩家
     */
    public void onMarsGoRouteArrived()
    {
        long newCount = getUSParams().getParam(EUsParam.MARS_GO_ROUTE_ARRIVE_COUNT) + 1;
        getUSParams().setParam(EUsParam.MARS_GO_ROUTE_ARRIVE_COUNT, newCount);

        ArrayList<NPUSUserData> allCacheUserData = getUsUserMgr().getAllCacheUserData();
        for (NPUSUserData userData : allCacheUserData)
        {
            userData.safeCall(() ->
            {
                userData.setParam(ENPPlayerParam.MARS_GO_ROUTE_ARRIVE_COUNT, -1);
            });
        }
    }

    /**
     * 标记已经开启过火星能量排行
     */
    public void markHadOpenMarsPowerRank()
    {
        if (getUSParams().getParam(EUsParam.HAD_MARS_POWER_RANK_OPENED) == 1)
            return;

        getUSParams().setParam(EUsParam.HAD_MARS_POWER_RANK_OPENED, 1);

        ArrayList<NPUSUserData> allCacheUserData = getUsUserMgr().getAllCacheUserData();
        for (NPUSUserData userData : allCacheUserData)
        {
            userData.safeCall(() ->
            {
                userData.setParam(ENPPlayerParam.HAD_MARS_POWER_RANK_OPENED, -1);
            });
        }
    }

    /***************
     *
     * === 跨服处理接口的统一处理 开始 ===
     *
     ***********/

    public void dispatchRpc(final _IWCGBasicRequestCommiter _committer, final _ARPCData _rpcData)
    {
        //直接处理
        UsRpcDispatcher.getInstance().dispatchRpc(_committer, _rpcData);
    }
    public _IALProtocolReceiver getLocalRpcDealer()
    {
        return _m_mLocalRPCListener;
    }

    /**
     * 对指定玩家发送消息
     * @param _cid
     * @param _proto
     */
    public void sendMsgToGC(long _cid, _IALProtocolStructure _proto)
    {
        int usTypeId = CommonFunc.parseServerTypeIdFromCid(_cid);

        //根据是否本服进行不同处理
        if(usTypeId == getServerTypeId())
        {
            // 查询对应的用户对象，并处理对应的用户消息
            NPUSUserData userData = getUsUserMgr().lookupCacheUserData(_cid);
            if (null == userData)
            {
                return;
            }
            // 加入处理数据并进行处理
            userData.sendMsgToGC(_proto.makeFullPackage());
        }
        else {
            //跨服发送消息进行处理
            sendMessageToBSServer(EServerType.USER.ordinal(), usTypeId
                    , NP2US_Writer_001_BasicOp.make_002_SendbackUserMsg(_cid, _proto));
        }
    }

    /***
     * 向公会发送请求消息，并进行相关处理
     * @param _committer
     * @param _cid
     * @param _guidId
     * @param _msg
     * @param _addInfo
     */
    public void dealGuildMsg(_ANPUSUserBasicMsgItem _committer, long _cid, long _guidId, _IALProtocolStructure _msg, _IALProtocolStructure _addInfo)
    {
        dealGuildMsg(_committer, _cid, _guidId, _msg, _addInfo, null);
    }
    public void dealGuildMsg(_ANPUSUserBasicMsgItem _committer, long _cid, long _guidId, _IALProtocolStructure _msg, _IALProtocolStructure _addInfo, _ICallBackIntT<_ANPUSUserBasicMsgItem> _failCallback) {
        //获取公会归属UsId
        int serverTypeId = CommonFunc.parseServerTypeIdFromInstanced(_guidId);

        //如果是本服则直接处理
        if (serverTypeId == getServerTypeId()) {
            //查询对应公会对象
            GuildInfo guildInfo = getGuildMgr().lookupGuild(_guidId);
            //不存在则报错
            if (null == guildInfo) {
                _committer.commitFailRes(GuildErr.GUILD_NOT_EXIST.getCode());
                return;
            }

            //调用公会对象的消息处理操作
            getGuildRequestDispather().DealProtocol(
                    new GuildMsgCommiter(_cid, guildInfo, _committer, null == _addInfo ? null : _addInfo.makePackage(), true)
                    , _msg);
        } else {
            //构造转发协议，发送到对应服务器进行处理
            sendRequestToBSServer(EServerType.USER.ordinal(), serverTypeId
                    , NP2US_RB_Writer_001_BasicOp.make_020_GuildMsg(_cid, _guidId, _msg, _addInfo)
                    , new _IWCGCallbackDealer() {
                        @Override
                        public _IALProtocolStructure createProtocolObj() {
                            return new NP2US_RB_001_020_RetGuildMsg();
                        }

                        @Override
                        public void dealSuc(_IALProtocolStructure _retMsg) {
                            //直接将消息转发回去
                            _committer.commitSucResByBuffer(((NP2US_RB_001_020_RetGuildMsg) _retMsg).get_buffer_Msg());
                        }

                        @Override
                        public void dealFail(int _errCode) {
                            if (null != _failCallback)
                                _failCallback.onRunOver(_errCode, _committer);
                            else
                                _committer.commitFailRes(_errCode);
                        }
                    });
        }
    }
    public void dealGuildMsgByRedirectCommiter(_ATGuildUserMsgRedirectCommiter _commiter, long _cid, long _guidId, _IALProtocolStructure _msg, _IALProtocolStructure _addInfo) {
        dealGuildMsgByRedirectCommiter(_commiter, _cid, _guidId, _msg, _addInfo, null);
    }
    public void dealGuildMsgByRedirectCommiter(_ATGuildUserMsgRedirectCommiter _commiter, long _cid, long _guidId, _IALProtocolStructure _msg, _IALProtocolStructure _addInfo, _ICallBackIntT<_ANPUSUserBasicMsgItem> _failCallback) {
        //获取公会归属UsId
        int serverTypeId = CommonFunc.parseServerTypeIdFromInstanced(_guidId);

        //如果是本服则直接处理
        if (serverTypeId == getServerTypeId()) {
            //查询对应公会对象
            GuildInfo guildInfo = getGuildMgr().lookupGuild(_guidId);
            //不存在则报错
            if (null == guildInfo) {
                _commiter.commitFailRes(GuildErr.GUILD_NOT_EXIST.getCode());
                return;
            }

            //调用公会对象的消息处理操作
            getGuildRequestDispather().DealProtocol(
                    new GuildMsgCommiter(_cid, guildInfo, _commiter, null == _addInfo ? null : _addInfo.makePackage(), true)
                    , _msg);
        } else {
            //构造转发协议，发送到对应服务器进行处理
            sendRequestToBSServer(EServerType.USER.ordinal(), serverTypeId
                    , NP2US_RB_Writer_001_BasicOp.make_020_GuildMsg(_cid, _guidId, _msg, _addInfo)
                    , new _IWCGCallbackDealer() {
                        @Override
                        public _IALProtocolStructure createProtocolObj() {
                            return new NP2US_RB_001_020_RetGuildMsg();
                        }

                        @Override
                        public void dealSuc(_IALProtocolStructure _retMsg) {
                            //直接将消息转发回去
                            _commiter.commitSucResByBuffer(((NP2US_RB_001_020_RetGuildMsg) _retMsg).get_buffer_Msg());
                        }

                        @Override
                        public void dealFail(int _errCode) {
                            if (null != _failCallback)
                                _failCallback.onRunOver(_errCode, _commiter);
                            else
                                _commiter.commitFailRes(_errCode);
                        }
                    });
        }
    }

    /***************
     *
     * === 跨服处理接口的统一处理 结束 ===
     *
     ***********/
}
