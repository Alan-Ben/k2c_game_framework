package NPUSServer.NPUSUserMgr;

import ALBasicCommon.ALBasicCommonFun;
import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALBasicMutex.MutexObject;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.CrossTeamEnum.ENPCrossTeamJoinCond;
import Common.CrossTeamObj.CrossTeam_SetInfo_Join;
import Common.MailObj.Mail_Data;
import Common.NpChatObj.NPCommon_ChatPlayerContent;
import Common.PlayerEnum.EPlayerEventRecordType;
import Common.ServerObj.ServerObj_ActivityTeamUser;
import CommonEnum.ECurrency;
import CommonEnum.ESpecialItemType;
import MJLog.MJLog;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.CommonObj.NPCommonItem;
import NPCommon.CommonObj.NPItemCostCollector_nosafe;
import NPCommon.Common_ItemExchangeInfo;
import NPCommon.DB.BM.BM;
import NPCommon.Dispather.NPCustomMsgDispatcher._IWCGMsgLocker;
import NPCommon.Enum.NPCommonEnum.ENPInsteadItemType;
import NPCommon.Enum.NPServerEnum;
import NPCommon.Enum.NPServerEnum.ENPUserDataState;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.LoginErr;
import NPCommon.NPCommon_ItemInfo;
import NPCommon.NPCommon_ItemList;
import NPCommon.NPLogDB.BaseOptLogBo;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate.ADelegateOne;
import NPCommon.Util.OptLock;
import NPEnum.*;
import NPGameRes.GameObjs.CommonObj.Condition.InterfaceObj._ITNPConditionDealerData;
import NPGameRes.GameObjs.CommonObj.Variable.InterfaceObj._ITNPVariableDealerData;
import NPGameRes.GameObjs.Reward.RewardMgr;
import NPGameRes.GameObjs.Reward.RewardObj;
import NPGameRes.GameObjs.UnionBonusMgr.UnionBonusMgr;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.Refs.Quest.RefQuest;
import NPGameRes.Refs.RefGeneral;
import NPGameRes.Refs.RefItemAlter;
import NPGameRes.Refs.RefItemExchange;
import NPServerProtocolWriter.NP2GS.Msg.NP2GS_Writer_001_BasicOp;
import NPUSServer.Cache.Player.PlayerCacheFunc;
import NPUSServer.ChatSys.ChatUserInfo;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_CROSS_DAY;
import NPUSServer.NPEvent.EventMgr.EventObj.NPGlobalUserEventObj;
import NPUSServer.NPEvent.EventMgr.NPPlayerEventHandlerMgr;
import NPUSServer.NPGeneralListener.NPUSGeneralBasicServerListener;
import NPUSServer.NPGeneralListener.Writer.NP2US_RB_Writer_002_GSOp;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.GameSystem.MailSystem.MailSystem;
import NPUSServer.NPUSUserMgr.ItemDealer.Impl.ShareItemDefDealer;
import NPUSServer.NPUSUserMgr.ItemDealer.Impl.UserItemDefDealer;
import NPUSServer.NPUSUserMgr.ItemDealer.UserItemDealerMgr;
import NPUSServer.NPUSUserMgr.PlayerChatRoomDealer.PlayerChatRoomDealer;
import NPUSServer.NPUSUserMgr.SynTask.NPSynPlayerEvnetRecordTask;
import NPUSServer.NPUSUserMgr.SynTask.NPSynUserDataOnlineChgTask;
import NPUSServer.NPUSUserMgr.UserComp.AchieveComp.AchieveComponent;
import NPUSServer.NPUSUserMgr.UserComp.ActivityCurrency.ActivityCurrencyComponent;
import NPUSServer.NPUSUserMgr.UserComp.ActivityFund.ActivityFundComponent;
import NPUSServer.NPUSUserMgr.UserComp.AnecdoteComp.AnecdoteComponent;
import NPUSServer.NPUSUserMgr.UserComp.AnnouncementComp.AnnouncementComponent;
import NPUSServer.NPUSUserMgr.UserComp.ArenaComp.ArenaComponent;
import NPUSServer.NPUSUserMgr.UserComp.BagItemComp.BagItemComponent;
import NPUSServer.NPUSUserMgr.UserComp.BubbleComp.PlayerBubbleComponent;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.BuildingComponent;
import NPUSServer.NPUSUserMgr.UserComp.CacheComp.PlayerCacheComponent;
import NPUSServer.NPUSUserMgr.UserComp.ChapterComp.ChapterComponent;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.ChildComponent;
import NPUSServer.NPUSUserMgr.UserComp.ClientDataComp.ClientDataComponent;
import NPUSServer.NPUSUserMgr.UserComp.ClockRewardComp.ClockRewardComponent;
import NPUSServer.NPUSUserMgr.UserComp.Common.UserItemBasicDealer_NONE;
import NPUSServer.NPUSUserMgr.UserComp.Common.UserUnionBonusPropertyChgDealer;
import NPUSServer.NPUSUserMgr.UserComp.ConsortChatComp.ConsortChatComponent;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortComponent;
import NPUSServer.NPUSUserMgr.UserComp.CountdownEventComp.CountdownEventComponent;
import NPUSServer.NPUSUserMgr.UserComp.CurrencyComp.CurrencyComponent;
import NPUSServer.NPUSUserMgr.UserComp.CuteActorComp.PlayerCuteActorComponent;
import NPUSServer.NPUSUserMgr.UserComp.DailyCheckComp.DailyCheckComponent;
import NPUSServer.NPUSUserMgr.UserComp.DailyQuestComp.DailyQuestComponent;
import NPUSServer.NPUSUserMgr.UserComp.DinnerComp.DinnerComponent;
import NPUSServer.NPUSUserMgr.UserComp.EmoteGroupComp.PlayerEmoteGroupComponent;
import NPUSServer.NPUSUserMgr.UserComp.EquipComp.EquipComponent;
import NPUSServer.NPUSUserMgr.UserComp.EveningDungeonComp.EveningDungeonComponent;
import NPUSServer.NPUSUserMgr.UserComp.ForbidChatComp.ForbidChatComponent;
import NPUSServer.NPUSUserMgr.UserComp.ForeverAddComp.ForeverAddComponent;
import NPUSServer.NPUSUserMgr.UserComp.FriendComp.FriendComponent;
import NPUSServer.NPUSUserMgr.UserComp.FuncUnlockComp.FuncUnlockComponent;
import NPUSServer.NPUSUserMgr.UserComp.GachaComp.GachaComponent;
import NPUSServer.NPUSUserMgr.UserComp.GuildBoxComp.GuildBoxComponent;
import NPUSServer.NPUSUserMgr.UserComp.GuildBoxComp.GuildBoxItemDealer;
import NPUSServer.NPUSUserMgr.UserComp.GuildComp.GuildComponent;
import NPUSServer.NPUSUserMgr.UserComp.GuildCooperateComp.GuildCooperateComponent;
import NPUSServer.NPUSUserMgr.UserComp.GuildDungeonComp.GuildDungeonComponent;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroComponent;
import NPUSServer.NPUSUserMgr.UserComp.HeroRecommend.HeroRecommendComponent;
import NPUSServer.NPUSUserMgr.UserComp.IconBgkComp.PlayerIconBgkComponent;
import NPUSServer.NPUSUserMgr.UserComp.IconComp.PlayerIconComponent;
import NPUSServer.NPUSUserMgr.UserComp.InnComp.InnComponent;
import NPUSServer.NPUSUserMgr.UserComp.LikeRecordComp.LikeRecordComponent;
import NPUSServer.NPUSUserMgr.UserComp.LoverCollectComp.LoverCollectComponent;
import NPUSServer.NPUSUserMgr.UserComp.MailComp.MailComponent;
import NPUSServer.NPUSUserMgr.UserComp.MailPlanComp.MailPlanComponent;
import NPUSServer.NPUSUserMgr.UserComp.MarsComp.MarsComponent;
import NPUSServer.NPUSUserMgr.UserComp.MarsMineComp.MarsMineComponent;
import NPUSServer.NPUSUserMgr.UserComp.MarsSoldierComp.MarsSoldierComponent;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep1_GoRouteComp.MarsGoRouteComponent;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep2_BuildingComp.MarsBuildingComponent;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep3_PeopleComp.MarsPeopleComponent;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep4_TechComp.MarsTechComponent;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.MarsExploreComponent;
import NPUSServer.NPUSUserMgr.UserComp.MiddayDungeonComp.MiddayDungeonComponent;
import NPUSServer.NPUSUserMgr.UserComp.MuseumComp.MuseumComponent;
import NPUSServer.NPUSUserMgr.UserComp.NPPlayerRemoteEffectDealMgr;
import NPUSServer.NPUSUserMgr.UserComp.NPUserComponentMgr;
import NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineRewardComponent;
import NPUSServer.NPUSUserMgr.UserComp.OrderComp.OrderComponent;
import NPUSServer.NPUSUserMgr.UserComp.OtherItemDealer.NPPlayerRewardItemDealer;
import NPUSServer.NPUSUserMgr.UserComp.OtherItemDealer.RemoteEffectItemDefDealer;
import NPUSServer.NPUSUserMgr.UserComp.PlayerBuffComp.PlayerBuffComponent;
import NPUSServer.NPUSUserMgr.UserComp.PlayerComp.NPPlayerComponent;
import NPUSServer.NPUSUserMgr.UserComp.PlayerComp.PlayerSysInfoDealer;
import NPUSServer.NPUSUserMgr.UserComp.PlayerEventRecordComp.NPPlayerEventRecordComp;
import NPUSServer.NPUSUserMgr.UserComp.PlayerFixedCdComp.PlayerFixedCdComp;
import NPUSServer.NPUSUserMgr.UserComp.PlayerLazyCDComp.PlayerLazyCDComponent;
import NPUSServer.NPUSUserMgr.UserComp.PlayerPermissionsComp.PlayerPermissionsComponent;
import NPUSServer.NPUSUserMgr.UserComp.PlayerShowComp.PlayerShowComponent;
import NPUSServer.NPUSUserMgr.UserComp.PlayerSkinComp.PlayerSkinComponent;
import NPUSServer.NPUSUserMgr.UserComp.PrivilegeCardComp.PrivilegeCardComponent;
import NPUSServer.NPUSUserMgr.UserComp.PushGiftPackComp.PushGiftComponent;
import NPUSServer.NPUSUserMgr.UserComp.QuestComp.PlayerQuestComponent;
import NPUSServer.NPUSUserMgr.UserComp.RankGiftPackComp.RankGiftPackComponent;
import NPUSServer.NPUSUserMgr.UserComp.RecordComp.PlayerRecordComponent;
import NPUSServer.NPUSUserMgr.UserComp.RecruitComp.RecruitComponent;
import NPUSServer.NPUSUserMgr.UserComp.RedDotComp.RedDotComponent;
import NPUSServer.NPUSUserMgr.UserComp.RefreshComp.RefreshComponent;
import NPUSServer.NPUSUserMgr.UserComp.ReportComp.ReportComponent;
import NPUSServer.NPUSUserMgr.UserComp.RoomSkinComp.PlayerRoomSkinComponent;
import NPUSServer.NPUSUserMgr.UserComp.RushExchangeComp.RushExchangeComponent;
import NPUSServer.NPUSUserMgr.UserComp.SevenDayGoals.SevenDayGoalsComponent;
import NPUSServer.NPUSUserMgr.UserComp.SevenLoginComp.SevenLoginComponent;
import NPUSServer.NPUSUserMgr.UserComp.ShopComp.PlayerShopComponent;
import NPUSServer.NPUSUserMgr.UserComp.SpecialItemComponent.Dealer.SpecialItemDealer_Gold;
import NPUSServer.NPUSUserMgr.UserComp.SpecialItemComponent.SpecialItemComponent;
import NPUSServer.NPUSUserMgr.UserComp.StageGlobalComp.StageGoalComponent;
import NPUSServer.NPUSUserMgr.UserComp.SystemQuestComp.SystemQuestComponent;
import NPUSServer.NPUSUserMgr.UserComp.TargetRewardComp.TargetRewardComponent;
import NPUSServer.NPUSUserMgr.UserComp.TitleComp.PlayerComboTitleComp;
import NPUSServer.NPUSUserMgr.UserComp.TitleComp.PlayerTitleComponent;
import NPUSServer.NPUSUserMgr.UserComp.TowerComp.TowerComponent;
import NPUSServer.NPUSUserMgr.UserComp.TravelComp.TravelComponent;
import NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp.TreasureHuntComponent;
import NPUSServer.NPUSUserMgr.UserComp.WeekCardComp.WeekCardComponent;
import NPUSServer.NPUSUserMgr.UserEvents.UserEvents;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUSUserMgr.UserMsgMgr.NPUSUserMsgMgr;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
import WCGCommon.Enum.NPEnum.EWCGKickOutGateType;

import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.List;

/*****************
 * 用户数据对象
 *
 * @author Administrator
 */
public class NPUSUserData implements _IALProtocolReceiver, _IWCGMsgLocker, _ITNPConditionDealerData, _ITNPVariableDealerData<ENPPlayerVariableType> {
    private static final int STACK_PROTECT = 5;//物品获得时的递归调用保护
    private static long _g_lSerialize;

    private synchronized static long _GetNewSeraizlie() {
        return _g_lSerialize++;
    }

    private MutexObject _m_msgMutex = new MutexObject();

    private NPUserServer _m_usUSServer;

    /**
     * 玩家序列号
     */
    private long _m_lSerialzie;
    /**
     * 用户Id
     */
    private long _m_lCid;
    /**
     * 用户Uid
     */
    private String _m_lUid;
    /**
     * 当前用户所在的连接服务器对象
     */
    private NPUSGeneralBasicServerListener _m_lUserGSListener;
    /**
     * 消息管理对象
     */
    private NPUSUserMsgMgr _m_mmMsgMgr;

    //用户相关标记数据的锁对象
    private MutexAtom _m_mInfoMutex;

    //用户相关标记
    private long _m_lClientSessionId;
    private int _m_lMsgDealerSerialize;

    /**
     * 是否在线
     */
    private boolean _m_bOnline = false;
    /**
     * 玩家此次登入的时间
     */
    private long _m_lOnlineTimeMs;
    /**
     * 用户在线标记
     */
    private long _m_lUserOnlineTag;
    /**
     * 用户逻辑意义上是否在线（即用户是否登入/登出游戏）
     */
    private boolean _m_bLogicOnline = false;

    //玩家数据状态
    private ENPUserDataState _m_eUserDataState;

    //玩家数据加载完成后的处理对象
    private _IWCGBasicRequestCommiter _m_rcLoadDataCommiter;

    //玩家异步处理对象管理对象
    private NPPlayerRemoteEffectDealMgr _m_rdmRemoteEffectDealMgr;

    //Item通用的处理器管理对象
    private UserItemDealerMgr _m_dmItemDealerMgr;

    //玩家事件集合
    public UserEvents Events;

    private OptLock _m_optLock = new OptLock(3000);

    public OptLock getOptLock() {
        return _m_optLock;
    }

    /**
     * 玩家事件触发管理对象
     */
    private NPPlayerEventHandlerMgr _m_mgrEventHandlerMgr;

    /**
     * 组件管理
     */
    private NPUserComponentMgr _m_mgrComponentMgr;

    /**
     * 玩家对象的全局属性管理器对象
     */
    private UnionBonusMgr _m_ubmUnionBonusMgr;

    /***
     * 玩家AreaTag
     */
    private String _m_sAreaTag = "common";

    public String getAreaTag() {
        return _m_sAreaTag;
    }

    public void setAreaTag(String _areaTag) {
        _m_sAreaTag = _areaTag;
    }

    //客户端SDK信息
    private NPUSUserDataSDKInfo _m_sdkInfo;

    /**
     * 组件列表
     */
    private NPPlayerComponent _m_compPlayerComponent;
    private CurrencyComponent _m_compCurrencyComponent;
    private ActivityCurrencyComponent _m_compActivityCurrencyComponent;
    private PlayerBuffComponent _m_compPlayerBuff;
    private BagItemComponent _m_compBagItemComponent;
    private ClientDataComponent _m_compClientData;
    private MailComponent _m_compMailComponent;
    private PlayerTitleComponent _m_compTitleComponent;
    private PlayerIconComponent _m_compIconComponent;
    private PlayerIconBgkComponent _m_compIconBgkComponent;
    private PlayerBubbleComponent _m_compBubbleComponent;
    private PlayerRoomSkinComponent _m_compRoomSkinComponent;//房间皮肤组件
    private PlayerLazyCDComponent _m_compLazyCDComponent;
    private PlayerQuestComponent _m_compQuestComponent;
    private PlayerRecordComponent _m_compRecordComponent;
    private NPPlayerEventRecordComp _m_compPlayerEventRecordComponent;
    private DailyQuestComponent _m_dailyQuestComponent;
    private ClockRewardComponent _m_clockRewardComponent;
    private PlayerFixedCdComp _m_compFixedCDComponent;
    private FriendComponent _m_compFriendComponent;
    private AchieveComponent _m_compAchieveComponent;
    private PlayerShopComponent _m_compShopComponent;
    private OfflineRewardComponent _m_compOfflineRewardComponent;
    private ConsortComponent _m_compConsortComponent;
    private ConsortChatComponent _m_compConsortChatComponent;
    private ChildComponent _m_compChildComponent;
    private HeroComponent _m_compHeroComponent;
    private PlayerShowComponent _m_compPlayerShowComponent;
    private DinnerComponent _m_compDinner;//宴会组件
    private ChapterComponent _m_compChapter;//关卡组件
    private AnecdoteComponent _m_compAnecdoteComponent;//政务组件
    private DailyCheckComponent _m_compDailyCheck;//每日签到组件
    private TravelComponent _m_compTravelComponent;//游历组件
    private HeroRecommendComponent _m_compHeroRecommendComponent;//大臣推荐组件
    private PlayerEmoteGroupComponent _m_compEmoteGroupComponent;//表情包组件
    private PlayerCuteActorComponent _m_compCuteActorComponent;//Q版形象组件
    private FuncUnlockComponent _m_compFuncUnlockComponent;//功能解锁组件
    private WeekCardComponent _m_compWeekCardComponent;//周卡组件
    private AnnouncementComponent _m_compAnnouncementComponent;//周卡组件
    private LikeRecordComponent _m_compLikeRecordComponent;//周卡组件
    private StageGoalComponent _m_compStageGoalComponent;//阶段目标组件
    private GuildComponent _m_compGuildComponent;//联盟组件
    private GuildCooperateComponent _m_compGuildCooperateComponent;//联盟协作组件
    private BuildingComponent _m_compBuildingComponent;//建筑组件
    private SpecialItemComponent _m_compSpecialItemComponent;//特殊物品组件
    private EquipComponent _m_compEquipComponent;//藏品组件
    private GachaComponent _m_compGachaComponent;//抽卡组件
    private RecruitComponent _m_compRecruitComponent;//兑换组件
    private ArenaComponent _m_compArenaComponent;//竞技场组件
    private TargetRewardComponent _m_compTargetRewardComponent;//目标奖励组件
    private RefreshComponent _m_compRefreshComponent;//通用刷新组件
    private TowerComponent _m_compTowerComponent;//爬塔组件
    private MiddayDungeonComponent _m_compMiddayDungeonComponent;//午间副本组件
    private EveningDungeonComponent _m_compEveningDungeonComponent;//晚间副本组件
    private PlayerComboTitleComp _m_compComboTitleComponent;//组合称号组件
    private PlayerSkinComponent _m_compPlayerSkinComponent;//玩家皮肤组件
    private SevenLoginComponent _m_compSevenLoginComponent;//七日登录组件
    private SevenDayGoalsComponent _m_compSevenDayGoalsComponent;//七日目标组件
    private CountdownEventComponent _m_compCountdownEventComponent;//倒计时事件组件
    private InnComponent _m_compInnComponent;//旅店组件
    private MuseumComponent _m_compMuseumComponent;//博物馆组件
    private SystemQuestComponent _m_compSystemQuestComponent;//系统任务组件
    private TreasureHuntComponent _m_compTreasureHuntComponent;//太空寻宝组件
    private PushGiftComponent _m_compPushGiftComponent;//推送礼包组件
    private GuildDungeonComponent _m_compGuildDungeonComponent;//公会副本组件
    private OrderComponent _m_compOrderComponent;//订单组件
    private MailPlanComponent _m_compMailPlanComponent;//邮件计划组件
    private RedDotComponent _m_compRedDotComponent;//红点组件
    private PrivilegeCardComponent _m_compPrivilegeCardComponent;//权益卡组件
    private ActivityFundComponent _m_compActivityFundComponent;//活动基金组件
    private RankGiftPackComponent _m_compRankGiftPackComponent;//冲榜礼包组件
    private RushExchangeComponent _m_compRushExchangeComponent;//限时兑换组件
    private LoverCollectComponent _m_compLoverCollectComponent;//情人收集组件
    private ForbidChatComponent _m_compForbidChatComponent; // 禁言组件
    private ReportComponent _m_compReportComponent;//举报组件

    //火星系统相关组件
    private MarsComponent _m_compMarsComponent;//火星-各系统数据汇总组件
    private MarsGoRouteComponent _m_compMarsGoRouteComponent;//火星-前往火星组件
    private MarsBuildingComponent _m_compMarsBuildingComponent;//火星-火星建筑组件
    private MarsPeopleComponent _m_compMarsPeopleComponent;//火星-火星居民组件
    private MarsTechComponent _m_compMarsTechComponent;//火星-火星科研组件
    private MarsExploreComponent _m_compMarsExploreComponent;//火星-火星探索组件
    private MarsMineComponent _m_compMarsMineComponent;//火星-火星矿产组件
    private MarsSoldierComponent _m_compMarsSoldierComponent;//火星-火星士兵组件
    //玩家权限组件
    private PlayerPermissionsComponent _m_compPlayerPermissionsComponent;
    //玩家永久加成组件
    private ForeverAddComponent _m_compForeverAddComponent;
    //联盟宝箱组件
    private GuildBoxComponent _m_compGuildBoxComponent;

    //玩家缓存数据管理组件
    private PlayerCacheComponent _m_pcPlayerCacheComponent;

    //玩家聊天处理对象
    private PlayerChatRoomDealer _m_dealerPlayerChatRoomDealer;

    //跨天监听
    public ADelegateOne<Integer> OnCrossDay;

    //时间相关
    private boolean _m_bFirstCreate = false;
    private UserSafeCall _m_safeCall; //玩家加载后的安全调用回调
    private NPPlayerContext _m_ctxPlayerInitContext = NPPlayerContext.createNew(ENPGameEvent.PLAYER_INIT); //玩家数据初始化事件

    //玩家注册成功后的聊天用户序列号
    private long _m_lChatUserSerial;

    public long getChatUserSerial() {
        return _m_lChatUserSerial;
    }

    public void setChatUserSerial(ChatUserInfo _user) {
        _m_lChatUserSerial = _user.getSerial();
    }

    public NPUSUserData(NPUserServer _usServer, long _cid, String _uid, String _customData) {
        _m_usUSServer = _usServer;

        _m_msgMutex.addPriority(30);//默认优先30，执行后变成0，是最大优先级了

        _m_lSerialzie = _GetNewSeraizlie();
        _m_lClientSessionId = 0;
        _m_lMsgDealerSerialize = 0;
        //用户ID及其连接服务器
        _m_lCid = _cid;
        _m_lUid = _uid;
        _m_lUserGSListener = null;
        //构造消息管理器
        _m_mmMsgMgr = new NPUSUserMsgMgr(this);

        //在线标记
        _m_lUserOnlineTag = 0;

        _m_eUserDataState = ENPUserDataState.NONE;
        _m_rcLoadDataCommiter = null;

        _m_mInfoMutex = new MutexAtom();

        _m_safeCall = new UserSafeCall(this);

        Events = new UserEvents(this);

        _m_mgrEventHandlerMgr = new NPPlayerEventHandlerMgr();

        //远程效果处理管理对象
        _m_rdmRemoteEffectDealMgr = new NPPlayerRemoteEffectDealMgr(this);

        //创建处理管理对象
        _m_dmItemDealerMgr = new UserItemDealerMgr(this);

        //sdk信息
        _m_sdkInfo = new NPUSUserDataSDKInfo(this, _customData);

        //组件管理
        _m_mgrComponentMgr = new NPUserComponentMgr(this);

        //玩家总属性管理对象，使用lazy触发，200毫秒间隔
        _m_ubmUnionBonusMgr = new UnionBonusMgr(true, 200);

        //组件列表
        _m_compPlayerComponent = new NPPlayerComponent(this);
        _m_compCurrencyComponent = new CurrencyComponent(this);
        _m_compActivityCurrencyComponent = new ActivityCurrencyComponent(this);
        _m_compPlayerBuff = new PlayerBuffComponent(this);
        _m_compBagItemComponent = new BagItemComponent(this);
        _m_compClientData = new ClientDataComponent(this);
        _m_compMailComponent = new MailComponent(this);
        _m_compTitleComponent = new PlayerTitleComponent(this);
        _m_compIconComponent = new PlayerIconComponent(this);
        _m_compIconBgkComponent = new PlayerIconBgkComponent(this);
        _m_compBubbleComponent = new PlayerBubbleComponent(this);
        _m_compRoomSkinComponent = new PlayerRoomSkinComponent(this);
        _m_compLazyCDComponent = new PlayerLazyCDComponent(this);
        _m_compQuestComponent = new PlayerQuestComponent(this);
        _m_compRecordComponent = new PlayerRecordComponent(this);
        _m_compPlayerEventRecordComponent = new NPPlayerEventRecordComp(this);
        _m_dailyQuestComponent = new DailyQuestComponent(this);
        _m_clockRewardComponent = new ClockRewardComponent(this);
        _m_compFixedCDComponent = new PlayerFixedCdComp(this);
        _m_compFriendComponent = new FriendComponent(this);
        _m_compAchieveComponent = new AchieveComponent(this);
        _m_compShopComponent = new PlayerShopComponent(this);
        _m_compOfflineRewardComponent = new OfflineRewardComponent(this);

        _m_compConsortComponent = new ConsortComponent(this);
        _m_compConsortChatComponent = new ConsortChatComponent(this);
        _m_compChildComponent = new ChildComponent(this);
        _m_compHeroComponent = new HeroComponent(this);
        _m_compPlayerShowComponent = new PlayerShowComponent(this);
        _m_compDinner = new DinnerComponent(this);
        _m_compChapter = new ChapterComponent(this);
        _m_compAnecdoteComponent = new AnecdoteComponent(this);
        _m_compDailyCheck = new DailyCheckComponent(this);
        _m_compTravelComponent = new TravelComponent(this);
        _m_compHeroRecommendComponent = new HeroRecommendComponent(this);
        _m_compEmoteGroupComponent = new PlayerEmoteGroupComponent(this);
        _m_compCuteActorComponent = new PlayerCuteActorComponent(this);
        _m_compFuncUnlockComponent = new FuncUnlockComponent(this);
        _m_compWeekCardComponent = new WeekCardComponent(this);
        _m_compAnnouncementComponent = new AnnouncementComponent(this);
        _m_compLikeRecordComponent = new LikeRecordComponent(this);
        _m_compStageGoalComponent = new StageGoalComponent(this);
        _m_compGuildComponent = new GuildComponent(this);
        _m_compGuildCooperateComponent = new GuildCooperateComponent(this);
        _m_compBuildingComponent = new BuildingComponent(this);
        _m_compSpecialItemComponent = new SpecialItemComponent(this);
        _m_compEquipComponent = new EquipComponent(this);
        _m_compGachaComponent = new GachaComponent(this);
        _m_compRecruitComponent = new RecruitComponent(this);
        _m_compArenaComponent = new ArenaComponent(this);
        _m_compTargetRewardComponent = new TargetRewardComponent(this);
        _m_compRefreshComponent = new RefreshComponent(this);
        _m_compTowerComponent = new TowerComponent(this);
        _m_compMiddayDungeonComponent = new MiddayDungeonComponent(this);
        _m_compEveningDungeonComponent = new EveningDungeonComponent(this);
        _m_compComboTitleComponent = new PlayerComboTitleComp(this);
        _m_compPlayerSkinComponent = new PlayerSkinComponent(this);
        _m_compSevenLoginComponent = new SevenLoginComponent(this);
        _m_compSevenDayGoalsComponent = new SevenDayGoalsComponent(this);
        _m_compCountdownEventComponent = new CountdownEventComponent(this);
        _m_compInnComponent = new InnComponent(this);
        _m_compMuseumComponent = new MuseumComponent(this);
        _m_compSystemQuestComponent = new SystemQuestComponent(this);
        _m_compTreasureHuntComponent = new TreasureHuntComponent(this);
        _m_compPushGiftComponent = new PushGiftComponent(this);
        _m_compGuildDungeonComponent = new GuildDungeonComponent(this);
        _m_compOrderComponent = new OrderComponent(this);
        _m_compMailPlanComponent = new MailPlanComponent(this);
        _m_compRedDotComponent = new RedDotComponent(this);
        _m_compPrivilegeCardComponent = new PrivilegeCardComponent(this);
        _m_compActivityFundComponent = new ActivityFundComponent(this);
        _m_compRankGiftPackComponent = new RankGiftPackComponent(this);
        _m_compRushExchangeComponent = new RushExchangeComponent(this);
        _m_compLoverCollectComponent = new LoverCollectComponent(this);
        _m_compForbidChatComponent = new ForbidChatComponent(this);
        _m_compReportComponent = new ReportComponent(this);

        //火星系统相关组件
        _m_compMarsComponent = new MarsComponent(this);
        _m_compMarsGoRouteComponent = new MarsGoRouteComponent(this);
        _m_compMarsBuildingComponent = new MarsBuildingComponent(this);
        _m_compMarsPeopleComponent = new MarsPeopleComponent(this);
        _m_compMarsTechComponent = new MarsTechComponent(this);
        _m_compMarsExploreComponent = new MarsExploreComponent(this);
        _m_compMarsMineComponent = new MarsMineComponent(this);
        _m_compMarsSoldierComponent = new MarsSoldierComponent(this);

        _m_compPlayerPermissionsComponent = new PlayerPermissionsComponent(this);

        _m_compForeverAddComponent = new ForeverAddComponent(this);

        _m_compGuildBoxComponent = new GuildBoxComponent(this);

        _m_pcPlayerCacheComponent = new PlayerCacheComponent(this);

        _m_dealerPlayerChatRoomDealer = new PlayerChatRoomDealer(this);

        //跨天监听
        OnCrossDay = new ADelegateOne<>(this);

        //注册需要tick的组件
        getComponentMgr().registTickable(getPlayerComponent());
        getComponentMgr().registTickable(getBuffComponent());
        getComponentMgr().registTickable(getClockRewardComponent());
        getComponentMgr().registTickable(getBubbleComponent());
        getComponentMgr().registTickable(getTitleComponent());
        getComponentMgr().registTickable(getCuteActorComponent());
        getComponentMgr().registTickable(getEmoteGroupComponent());
        getComponentMgr().registTickable(getIconBgkComponent());
        getComponentMgr().registTickable(getIconComponent());
        getComponentMgr().registTickable(getRoomSkinComponent());
        getComponentMgr().registTickable(_m_compPrivilegeCardComponent);

        //注册ItemDealer处理
        _m_dmItemDealerMgr.regDealer(UserItemBasicDealer_NONE.getInstance());
        _m_dmItemDealerMgr.regDealer(_m_compBagItemComponent);
        _m_dmItemDealerMgr.regDealer(_m_compMailComponent);
        _m_dmItemDealerMgr.regDealer(_m_compCurrencyComponent);
        _m_dmItemDealerMgr.regDealer(_m_compActivityCurrencyComponent);
        _m_dmItemDealerMgr.regDealer(_m_compHeroComponent);
        _m_dmItemDealerMgr.regDealer(_m_compHeroComponent.getSkinItemDealer());
        _m_dmItemDealerMgr.regDealer(_m_compTitleComponent);
        _m_dmItemDealerMgr.regDealer(_m_compIconComponent);
        _m_dmItemDealerMgr.regDealer(_m_compIconBgkComponent);
        _m_dmItemDealerMgr.regDealer(_m_compRoomSkinComponent);
        _m_dmItemDealerMgr.regDealer(_m_compBubbleComponent);
        _m_dmItemDealerMgr.regDealer(_m_compEmoteGroupComponent);
        _m_dmItemDealerMgr.regDealer(_m_compCuteActorComponent);
        _m_dmItemDealerMgr.regDealer(new NPPlayerRewardItemDealer(this));
        _m_dmItemDealerMgr.regDealer(_m_compLazyCDComponent);
        _m_dmItemDealerMgr.regDealer(getQuestComponent());
        _m_dmItemDealerMgr.regDealer(new UserItemDefDealer(this));
        _m_dmItemDealerMgr.regDealer(getFixedCdComponent());
        _m_dmItemDealerMgr.regDealer(_m_compRecordComponent);
        _m_dmItemDealerMgr.regDealer(new ShareItemDefDealer(this));
        _m_dmItemDealerMgr.regDealer(getBuffComponent());
        //情人相关组件
        _m_dmItemDealerMgr.regDealer(_m_compConsortComponent);
        _m_dmItemDealerMgr.regDealer(_m_compConsortComponent.getCGMgr());
//        _m_dmItemDealerMgr.regDealer(new ConsortSkinItemDefDealer(this));
        _m_dmItemDealerMgr.regDealer(_m_compAchieveComponent);
        //大臣推荐事件
        _m_dmItemDealerMgr.regDealer(_m_compHeroRecommendComponent);
        //远程效果
        _m_dmItemDealerMgr.regDealer(new RemoteEffectItemDefDealer(this));
        _m_dmItemDealerMgr.regDealer(_m_compWeekCardComponent);
        _m_dmItemDealerMgr.regDealer(_m_compEquipComponent);
        _m_dmItemDealerMgr.regDealer(_m_compAnecdoteComponent);
        _m_dmItemDealerMgr.regDealer(_m_compBuildingComponent);
        //组合称号
        _m_dmItemDealerMgr.regDealer(_m_compComboTitleComponent.getUnitMgr(ENPPlayerComboTitleType.PRE));
        _m_dmItemDealerMgr.regDealer(_m_compComboTitleComponent.getUnitMgr(ENPPlayerComboTitleType.SFX));
        _m_dmItemDealerMgr.regDealer(_m_compComboTitleComponent.getUnitMgr(ENPPlayerComboTitleType.BG));
        //玩家皮肤
        _m_dmItemDealerMgr.regDealer(_m_compPlayerSkinComponent);
        _m_dmItemDealerMgr.regDealer(_m_compCountdownEventComponent);
        _m_dmItemDealerMgr.regDealer(_m_compInnComponent.getDishMgr());
        _m_dmItemDealerMgr.regDealer(_m_compMuseumComponent.getItemMgr());
        //权益组件
        _m_dmItemDealerMgr.regDealer(_m_compPrivilegeCardComponent);
        //SYS数据处理
        _m_dmItemDealerMgr.regDealer(new PlayerSysInfoDealer(this));
        //公会宝箱
        _m_dmItemDealerMgr.regDealer(new GuildBoxItemDealer(this));
        //永久加成
        _m_dmItemDealerMgr.regDealer(_m_compForeverAddComponent);
        _m_dmItemDealerMgr.regDealer(_m_compRoomSkinComponent);
    }

    public NPUserServer getUSServer() {
        return _m_usUSServer;
    }

    public NPPlayerRemoteEffectDealMgr getRemoteEffectDealMgr() {
        return _m_rdmRemoteEffectDealMgr;
    }

    public NPPlayerEventHandlerMgr getEventHandlerMgr() {
        return _m_mgrEventHandlerMgr;
    }

    public NPUSUserDataSDKInfo getSdkInfo() {
        return _m_sdkInfo;
    }

    //玩家组件相关GETTER
    public NPUserComponentMgr getComponentMgr() {
        return _m_mgrComponentMgr;
    }

    public UnionBonusMgr getBonusMgr() {
        return _m_ubmUnionBonusMgr;
    }

    public NPPlayerComponent getPlayerComponent() {
        return _m_compPlayerComponent;
    }

    public CurrencyComponent getCurrencyComponent() {
        return _m_compCurrencyComponent;
    }

    public ActivityCurrencyComponent getActivityCurrencyComponent() {
        return _m_compActivityCurrencyComponent;
    }

    public PlayerBuffComponent getBuffComponent() {
        return _m_compPlayerBuff;
    }

    public BagItemComponent getBagItemComponent() {
        return _m_compBagItemComponent;
    }

    public ClientDataComponent getClientDataComp() {
        return _m_compClientData;
    }

    public MailComponent getMailComponent() {
        return _m_compMailComponent;
    }

    public PlayerTitleComponent getTitleComponent() {
        return _m_compTitleComponent;
    }

    public PlayerIconComponent getIconComponent() {
        return _m_compIconComponent;
    }

    public PlayerIconBgkComponent getIconBgkComponent() {
        return _m_compIconBgkComponent;
    }

    public PlayerBubbleComponent getBubbleComponent() {
        return _m_compBubbleComponent;
    }

    public PlayerRoomSkinComponent getRoomSkinComponent()
    {
        return _m_compRoomSkinComponent;
    }

    public PlayerLazyCDComponent getLazyCDComponent() {
        return _m_compLazyCDComponent;
    }

    public PlayerQuestComponent getQuestComponent() {
        return _m_compQuestComponent;
    }

    public PlayerRecordComponent getRecordComponent() {
        return _m_compRecordComponent;
    }

    public NPPlayerEventRecordComp getEventRecordComp() {
        return _m_compPlayerEventRecordComponent;
    }

    public DailyQuestComponent getDailyQuestComponent() {
        return _m_dailyQuestComponent;
    }

    public ClockRewardComponent getClockRewardComponent() {
        return _m_clockRewardComponent;
    }

    public PlayerFixedCdComp getFixedCdComponent() {
        return _m_compFixedCDComponent;
    }

    public FriendComponent getFriendComponent() {
        return _m_compFriendComponent;
    }

    public AchieveComponent getAchieveComponent() {
        return _m_compAchieveComponent;
    }

    public PlayerShopComponent getShopComponent() {
        return _m_compShopComponent;
    }

    public OfflineRewardComponent getOfflineRewardComponent() {
        return _m_compOfflineRewardComponent;
    }

    public ConsortComponent getConsortComponent() {
        return _m_compConsortComponent;
    }

    public ConsortChatComponent getConsortChatComponent() {
        return _m_compConsortChatComponent;
    }

    public ChildComponent getChildComponent() {
        return _m_compChildComponent;
    }

    public HeroComponent getHeroComponent() {
        return _m_compHeroComponent;
    }

    public PlayerShowComponent getPlayerShowComponent() {
        return _m_compPlayerShowComponent;
    }

    public DinnerComponent getDinnerComponent() {
        return _m_compDinner;
    }

    public ChapterComponent getChapterComponent() {
        return _m_compChapter;
    }

    public AnecdoteComponent getAnecdoteComponent() {
        return _m_compAnecdoteComponent;
    }

    public DailyCheckComponent getDailyCheckComponent() {
        return _m_compDailyCheck;
    }

    public TravelComponent getTravelComponent() {
        return _m_compTravelComponent;
    }

    public HeroRecommendComponent getHeroRecommendComponent() {
        return _m_compHeroRecommendComponent;
    }

    public PlayerEmoteGroupComponent getEmoteGroupComponent() {
        return _m_compEmoteGroupComponent;
    }

    public PlayerCuteActorComponent getCuteActorComponent() {
        return _m_compCuteActorComponent;
    }

    public FuncUnlockComponent getFuncUnlockComponent() {
        return _m_compFuncUnlockComponent;
    }

    public WeekCardComponent getWeekCardComponent() {
        return _m_compWeekCardComponent;
    }

    public AnnouncementComponent getAnnouncementComponent() {
        return _m_compAnnouncementComponent;
    }

    public LikeRecordComponent getLikeRecordComponent() {
        return _m_compLikeRecordComponent;
    }

    public StageGoalComponent getStageGoalComponent() {
        return _m_compStageGoalComponent;
    }

    public GuildComponent getGuildComponent() {
        return _m_compGuildComponent;
    }

    public GuildCooperateComponent getGuildCooperateComponent() {
        return _m_compGuildCooperateComponent;
    }

    public BuildingComponent getBuildingComponent() {
        return _m_compBuildingComponent;
    }

    public SpecialItemComponent getSpecialItemComponent() {
        return _m_compSpecialItemComponent;
    }

    public EquipComponent getEquipComponent() {
        return _m_compEquipComponent;
    }

    public GachaComponent getGachaComponent() {
        return _m_compGachaComponent;
    }

    public RecruitComponent getRecruitComponent() {
        return _m_compRecruitComponent;
    }

    public ArenaComponent getArenaComponent() {
        return _m_compArenaComponent;
    }

    public TargetRewardComponent getTargetRewardComponent() {
        return _m_compTargetRewardComponent;
    }

    public RefreshComponent getRefreshComponent() {
        return _m_compRefreshComponent;
    }

    public TowerComponent getTowerComponent() {
        return _m_compTowerComponent;
    }

    public MiddayDungeonComponent getMiddayDungeonComponent() {
        return _m_compMiddayDungeonComponent;
    }

    public EveningDungeonComponent getEveningDungeonComponent() {
        return _m_compEveningDungeonComponent;
    }

    public PlayerComboTitleComp getPlayerComboTitleComp() {
        return _m_compComboTitleComponent;
    }

    public PlayerSkinComponent getPlayerSkinComp() {
        return _m_compPlayerSkinComponent;
    }

    public SevenLoginComponent getSevenLoginComponent() {
        return _m_compSevenLoginComponent;
    }

    public SevenDayGoalsComponent getSevenDayGoalsComponent() {
        return _m_compSevenDayGoalsComponent;
    }

    public CountdownEventComponent getCountdownEventComponent() {
        return _m_compCountdownEventComponent;
    }

    public InnComponent getInnComponent() {
        return _m_compInnComponent;
    }

    public MuseumComponent getMuseumComponent() {
        return _m_compMuseumComponent;
    }

    public SystemQuestComponent getSystemQuestComponent() {
        return _m_compSystemQuestComponent;
    }

    public TreasureHuntComponent getTreasureHuntComponent() {
        return _m_compTreasureHuntComponent;
    }

    public PushGiftComponent getPushGiftPackComponent() {
        return _m_compPushGiftComponent;
    }

    public GuildDungeonComponent getGuildDungeonComponent() {
        return _m_compGuildDungeonComponent;
    }

    public OrderComponent getOrderComponent() {
        return _m_compOrderComponent;
    }

    public MailPlanComponent getMailPlanComponent() {
        return _m_compMailPlanComponent;
    }

    public RedDotComponent getRedDotComponent() {
        return _m_compRedDotComponent;
    }

    public PrivilegeCardComponent getPrivilegeCardComponent() {
        return _m_compPrivilegeCardComponent;
    }

    public ActivityFundComponent getActivityFundComponent() {
        return _m_compActivityFundComponent;
    }

    public RankGiftPackComponent getRankGiftPackComponent() {
        return _m_compRankGiftPackComponent;
    }

    public RushExchangeComponent getRushExchangeComponent() {
        return _m_compRushExchangeComponent;
    }

    public LoverCollectComponent getLoverCollectComponent() {
        return _m_compLoverCollectComponent;
    }

    public MarsComponent getMarsComponent() {
        return _m_compMarsComponent;
    }

    public MarsGoRouteComponent getMarsGoRouteComponent() {
        return _m_compMarsGoRouteComponent;
    }

    public MarsBuildingComponent getMarsBuildingComponent() {
        return _m_compMarsBuildingComponent;
    }

    public MarsPeopleComponent getMarsPeopleComponent() {
        return _m_compMarsPeopleComponent;
    }

    public MarsTechComponent getMarsTechComponent() {
        return _m_compMarsTechComponent;
    }

    public MarsExploreComponent getMarsExploreComponent() {
        return _m_compMarsExploreComponent;
    }

    public MarsMineComponent getMarsMineComponent() {
        return _m_compMarsMineComponent;
    }

    public MarsSoldierComponent getMarsSoldierComponent() {
        return _m_compMarsSoldierComponent;
    }

    public PlayerPermissionsComponent getPlayerPermissionsComponent() {
        return _m_compPlayerPermissionsComponent;
    }

    public ForeverAddComponent getForeverAddComponent() {
        return _m_compForeverAddComponent;
    }

    public GuildBoxComponent getGuildBoxComponent() {
        return _m_compGuildBoxComponent;
    }

    public PlayerCacheComponent getCacheComponent() {return _m_pcPlayerCacheComponent;}

    public ForbidChatComponent getForbidChatComponent() {return _m_compForbidChatComponent;}

    public ReportComponent getReportComponent() {return _m_compReportComponent;}

    public PlayerChatRoomDealer getPlayerChatRoomDealer() {return _m_dealerPlayerChatRoomDealer;}

    //返回玩家初始化上下文对象
    public NPPlayerContext getPlayerInitContext() {
        return _m_ctxPlayerInitContext;
    }

    public long getCurTimeMs() {
        return CommonFunc.getNowTimeMS();
    }

    public long getOnlineTimeMs() {
        return _m_lOnlineTimeMs;
    }

    //玩家锁对象（与用户相关标记数据的锁对象 保持一致）
    public void lockUser() {
        _m_msgMutex.lock();
    }

    public void unlockUser() {
        _m_msgMutex.unlock();
    }

    public MutexObject getUserLock() {
        return _m_msgMutex;
    }//返回用户锁

    public boolean isOnline() {
        return _m_bOnline;
    }

    public ENPUserDataState getUserDataState() {
        return _m_eUserDataState;
    }

    public boolean isLogicOnline() {
        return _m_bLogicOnline;
    }

    public void setLogicOnline(boolean _logicOnline) {
        lockUser();
        try {
            _m_bLogicOnline = _logicOnline;

            long nowTimeMS = CommonFunc.getNowTimeMS();
            if (_logicOnline) {
                //好友推荐标记在线
                getUSServer().getFriendTipMgr().onlineUpdateInfo(getCid());
                //标记最后一次登录时间
                getPlayerComponent().setParam(ENPPlayerParam.LATEST_LOGIN_TIME_MS, nowTimeMS);

                //获取离线时间
                long offlineTimeMs = getParam(ENPPlayerParam.LAST_OFFLINE_MS);
                //获取上线时间
                long onlineTimeMs = getParam(ENPPlayerParam.LATEST_LOGIN_TIME_MS);
                //触发在线周卡结算
                boolean weekCardSettle = getWeekCardComponent().onlineSettle(offlineTimeMs, onlineTimeMs);
                if (weekCardSettle) {
                    //标记奖励时长
                    getPlayerComponent().setParam(ENPPlayerParam.OFFLINE_PERIOD_REWARD_DURATION_MS, onlineTimeMs - offlineTimeMs);
                }
                //通知联盟上线
                getGuildComponent().onOnline();

                //运营日志
                MJLog.logLogin(this, 2);
            } else {
                //好友推荐标记下线
                getUSServer().getFriendTipMgr().offlineUpdateInfo(getCid());
                //记录下线时间
                getPlayerComponent().setParam(ENPPlayerParam.LAST_OFFLINE_MS, nowTimeMS);
                //触发在线周卡结算
                getWeekCardComponent().onOffline();
                //通知联盟下线
                getGuildComponent().onOffline();

                //运营日志
                MJLog.logLogin(this, 3);
            }

            //进行金币结算
            SpecialItemDealer_Gold goldDealer = getSpecialItemComponent().getDealer(ESpecialItemType.GOLD, SpecialItemDealer_Gold.class);
            if (goldDealer != null) {
                goldDealer.onlineStateChg(_logicOnline);
            }
        } finally {
            unlockUser();
        }
    }

    /************
     * 响应事件后向下分发
     */
    public void onLogicEvent(_ALogicEventBase _evt) {
        if (_evt.getContext().getDeep() >= 10) {
            USLog.fatal(getUSServer(), "Dealing onLogicEvent event {} indent >= 10", _evt.getEventName(), new Exception(""));
            return;
        }
        lockUser();
        try {
            //玩家事件响应
            _m_mgrEventHandlerMgr.handle(_evt, this);
            //全局事件响应
            getUSServer().getGlobalEventHandlerMgr().handle(_evt, new NPGlobalUserEventObj(this));
        } finally {
            unlockUser();
        }
    }

    protected void _lockInfo() {
        _m_mInfoMutex.lock();
    }

    protected void _unlockInfo() {
        _m_mInfoMutex.unlock();
    }

    //设置数据状态
    public void setDataState(ENPUserDataState _state) {
        _lockInfo();

        try {
            _m_eUserDataState = _state;
        } finally {
            _unlockInfo();
        }
    }

    //设置数据加载回调对象，一个用户只有一个回调对象，新对象将顶替旧对象，同时对应连接服务器也会进行处理
    public void setDataLoadRequestCommiter(_IWCGBasicRequestCommiter _commiter) {
        _lockInfo();

        try {
            _IWCGBasicRequestCommiter preCommiter = _m_rcLoadDataCommiter;

            //设置新对象
            _m_rcLoadDataCommiter = _commiter;

            //提交旧数据失败处理
            if (null != preCommiter) preCommiter.commitFailRes(CommErr.DATA_STATE_ERR.getCode());

            //判断数据加载状态
            if (_m_eUserDataState == ENPUserDataState.LOADED) {
                _m_rcLoadDataCommiter.commitSucRes(NP2US_RB_Writer_002_GSOp.make_001_RegUserGateSuc(getSerialize()));
                _m_rcLoadDataCommiter = null;
            }
        } finally {
            _unlockInfo();
        }
    }

    //设置数据加载完成，需要调整状态同时需要根据在线情况进行相关后续处理
    public void setDataLoadSuc() {
        lockUser();
        _lockInfo();

        try {
            //先做状态判断，避免重复处理
            if (_m_eUserDataState == ENPUserDataState.LOADED) {
                USLog.error(getUSServer(), "Set User Data State Loaded multi times!");
                return;
            }

            //第一次创建加载
            if (isFirstCreate()) {
                safeCall(() ->
                {
                    //初始化给物品列表
                    initGiveItems();

                    //发送初始化邮件
                    sendInitMail();

                    //开启初始化任务
                    startInitQuest();

                    //重置首次创角标记位
                    _m_bFirstCreate = false;
                });
            }

            if (null != _m_rcLoadDataCommiter)
                _m_rcLoadDataCommiter.commitSucRes(NP2US_RB_Writer_002_GSOp.make_001_RegUserGateSuc(getSerialize()));
            _m_rcLoadDataCommiter = null;

            _m_eUserDataState = ENPUserDataState.LOADED;

            //根据用户状态同步在线情况，离线则不处理，当作没上线
            if (_m_bOnline) {
                //根据用户状态同步在线情况，离线则不处理，当作没上线
                ALSynTaskManager.getInstance().regTask(new NPSynUserDataOnlineChgTask(this, true));
            }
            //处理需要保证用户加载城后之后的相关处理操作
            _m_safeCall.dealPendingCalls();
            //各组件自检数据
            _checkData();
        } finally {
            _unlockInfo();
            unlockUser();
        }

        //从OfflineTmpDataCore中删除Cid数据
        getUSServer().getOfflineTmpDataCore().clearCidData(getCid());
    }

    /*
    发送初始化邮件
     */
    public void sendInitMail() {
        Mail_Data mailData = new Mail_Data();
        mailData.setMailRefId(RefGeneral.Ref().player_init_mail_id);
        mailData.getItemList().getItemList().addAll(CommonFunc.costItemListToProto(RefGeneral.Ref().player_init_mail_give_item));
        MailSystem.addMail(getUSServer(), getCid(), mailData, getPlayerInitContext());
    }

    /**
     * 开启初始化任务
     */
    public void startInitQuest() {
        for (long questId : RefGeneral.Ref().quest_init_open_list) {
            RefQuest ref = RefQuest.getMgr().get(questId);
            if (null == ref) {
                USLog.error(getUSServer(), "can not find init quest: " + questId);
                continue;
            }

            getQuestComponent().startQuestByServer(ref, getPlayerInitContext());
        }
    }

    //设置数据加载失败，需要调整状态同时需要根据在线情况进行相关后续处理
    public void setDataLoadFail() {
        USLog.warn(getUSServer(), "USUserData load fail cid:{}", getCid(), new Exception());
        //注销用户
        getUSServer().getUsUserMgr().unregUserData(this);

        _lockInfo();

        try {
            if (null != _m_rcLoadDataCommiter)
                _m_rcLoadDataCommiter.commitFailRes(LoginErr.PLAYER_DATA_LOAD_FAIL.getCode());
            _m_rcLoadDataCommiter = null;
        } finally {
            _unlockInfo();
        }

        //锁定用户数据进行后续处理
        lockUser();

        try {
            //注销资源
            dispose();
        } finally {
            unlockUser();
        }
    }

    /*************
     * 设置用户在线标记
     * @param _isOnline
     */
    public void setOnlineTag(boolean _isOnline) {
        _lockInfo();

        try {
            _m_bOnline = _isOnline;
            _m_lOnlineTimeMs = CommonFunc.getNowTimeMS();

            //刷新在线标记，在线的时候刷新序列号，设置时间戳
            if (_m_bOnline) {
                resetOnlineTag();
            } else {
                refreshOnlineTag();
            }

            //输出相关日志
            if (_m_bOnline) {
                USLog.info(getUSServer(), "player cid:{} online", getCid());
            } else {
                USLog.info(getUSServer(), "player cid:{} offline", getCid());
            }

            //处理在线状态变化
            if (_m_eUserDataState == ENPUserDataState.LOADED) {
                //根据用户状态同步在线情况，离线则不处理，当作没上线
                ALSynTaskManager.getInstance().regTask(new NPSynUserDataOnlineChgTask(this, _m_bOnline));
            }
        } finally {
            _unlockInfo();
        }
    }

    /****************
     * 检测各组件数据是否完整，需要校对的在这边进行处理
     */
    protected void _checkData() {
        //检测头像、头像框、称号
        _m_compIconComponent.checkCurPlayerIcon();
        _m_compIconBgkComponent.checkCurPlayerIconBgk();
        _m_compBubbleComponent.checkCurPlayerBubble();
        _m_compCuteActorComponent.checkCurPlayerCuteActor();
        _m_compRoomSkinComponent.checkCurPlayerRoomSkin();

        //注册玩家属性变更处理
        _m_ubmUnionBonusMgr.setOnPropertyChg(new UserUnionBonusPropertyChgDealer(this));
    }

    @Override
    public void lockMsg() {
        _m_msgMutex.lock();
    }

    @Override
    public void unlockMsg() {
        _m_msgMutex.unlock();
    }

    public long getSerialize() {
        return _m_lSerialzie;
    }

    public long getClinetSessionId() {
        return _m_lClientSessionId;
    }

    public int getMsgDealerSerialize() {
        return _m_lMsgDealerSerialize;
    }

    public void setClientSessionId(long _clientSessionId) {
        _m_lClientSessionId = _clientSessionId;
    }

    public void setMsgDealerSerialize(int _serialize) {
        _m_lMsgDealerSerialize = _serialize;
    }

    public long getCid() {
        return _m_lCid;
    }

    public String getUid() {
        return _m_lUid;
    }

    public NPUSGeneralBasicServerListener getGSListener() {
        return _m_lUserGSListener;
    }

    /**
     * 设置连接服务器，并返回原先的连接服务端
     */
    public NPUSGeneralBasicServerListener chgGSListener(NPUSGeneralBasicServerListener _gsListener) {
        _lockInfo();

        try {
            NPUSGeneralBasicServerListener preListener = _m_lUserGSListener;
            _m_lUserGSListener = _gsListener;

            return preListener;
        } finally {
            _unlockInfo();
        }
    }

    public NPUSUserMsgMgr getMsgMgr() {
        return _m_mmMsgMgr;
    }

    public long getOnlineTag() {
        return _m_lUserOnlineTag;
    }

    public boolean isBanned() {
        return false;
    }

    /**
     * 刷新在线标记
     */
    public void refreshOnlineTag() {
        _m_lUserOnlineTag = ALBasicCommonFun.getNowTimeMS();
        _m_lSerialzie = _GetNewSeraizlie();
    }

    /**
     * 重置在线标记，避免被删除数据
     */
    public void resetOnlineTag() {
        _m_lUserOnlineTag = 0;
        _m_lSerialzie = _GetNewSeraizlie();
    }

    public void dispose() {
        //释放资源
        _m_eUserDataState = ENPUserDataState.DISCARD;

        //释放所有组件
        getComponentMgr().dispose();
        getEventHandlerMgr().dispose();

        // 设置服务器Id
        NPUSGeneralBasicServerListener preGsListener = chgGSListener(null);
        //发送消息通知用户连接被踢
        if (null != preGsListener) {
            preGsListener.sendCustomMsg(NP2GS_Writer_001_BasicOp.make_002_UserGateKicked(getCid(), getClinetSessionId(), EWCGKickOutGateType.SYSTEM));
        }

        //释放监听
        OnCrossDay.clear();

        //移除聊天凭证并退出所有聊天房间
        getUSServer().getChatUserMgr().unRegChatUser(this);
    }

    /********************
     * 发送消息给客户端
     *
     * @param _protocol
     */
    public void sendMsgToGC(_IALProtocolStructure _protocol) {
        if (null == _m_lUserGSListener || null == _protocol) return;

        // 构造协议发送给对应的服务器
        _m_lUserGSListener.sendCustomMsg(NP2GS_Writer_001_BasicOp.make_001_SendbackUserMsg(getClinetSessionId(), getMsgDealerSerialize(), _protocol.makeFullPackage()));
    }

    /*************
     * 客户端发送回调式处理的时候返回消息的处理函数
     * @param _clientRequestSerialize
     * @param _errCode
     */
    public void sendBackRequestFailToGC(long _clientRequestSerialize, int _errCode) {
        sendBackRequestFailToGC(_clientRequestSerialize, _errCode, null);
    }

    public void sendBackRequestFailToGC(long _clientRequestSerialize, int _errCode, _IALProtocolStructure _protocol) {
        if (null == _m_lUserGSListener) return;

        // 构造协议发送给对应的服务器
        if (null != _protocol) {
            _m_lUserGSListener.sendCustomMsg(NP2GS_Writer_001_BasicOp.make_010_SendbackUserClientRequest(getClinetSessionId(),
                    getMsgDealerSerialize(), _clientRequestSerialize, false, _errCode, _protocol.makeFullPackage()));
        } else {
            _m_lUserGSListener.sendCustomMsg(NP2GS_Writer_001_BasicOp.make_010_SendbackUserClientRequest(getClinetSessionId(),
                    getMsgDealerSerialize(), _clientRequestSerialize, false, _errCode, null));
        }
    }

    public void sendBackRequestResToGC(long _clientRequestSerialize, _IALProtocolStructure _protocol) {
        if (null == _m_lUserGSListener) return;

        // 构造协议发送给对应的服务器
        _m_lUserGSListener.sendCustomMsg(NP2GS_Writer_001_BasicOp.make_010_SendbackUserClientRequest(getClinetSessionId(),
                getMsgDealerSerialize(), _clientRequestSerialize, true, 0, null == _protocol ? null : _protocol.makeFullPackage()));
    }

    public void sendBackRequestResToGC(long _clientRequestSerialize, ByteBuffer _protocol) {
        if (null == _m_lUserGSListener) return;

        // 构造协议发送给对应的服务器
        _m_lUserGSListener.sendCustomMsg(NP2GS_Writer_001_BasicOp.make_010_SendbackUserClientRequest(getClinetSessionId(),
                getMsgDealerSerialize(), _clientRequestSerialize, true, 0, _protocol));
    }

    //只在玩家加载成功后才进行推送协议
    public void pushMsgToGC(_IALProtocolStructure _protocol) {
        if (_m_eUserDataState != ENPUserDataState.LOADED) return;

        //玩家下线情况也不推送
        if (!isLogicOnline()) return;

        sendMsgToGC(_protocol);
    }

    /********************
     * 发送消息给客户端
     *
     * @param _msg
     */
    public void sendMsgToGC(ByteBuffer _msg) {
        if (null == _m_lUserGSListener) {
            return;
        }
        // 构造协议发送给对应的服务器
        _m_lUserGSListener.sendCustomMsg(NP2GS_Writer_001_BasicOp.make_001_SendbackUserMsg(getClinetSessionId(), getMsgDealerSerialize(), _msg));
    }

    /********************
     * 玩家角色消息数据的处理函数
     *
     * @author alzq.z
     * @time Mar 4, 2013 10:46:06 AM
     */
    public void dealMsg(_ANPUSUserBasicMsgItem _commiter, final ByteBuffer _msg) {
        if (getUSServer().getUSMsgDispatcher().DealProtocol(_commiter, _msg)) {
            return;
        }

        //用户消息没有处理完毕的情况下，直接将消息转发到战斗服务器进行处理
        //后续的处理由于已经不做即时战斗处理，因此全部屏蔽进行报错处理
        _msg.position(0);
        final byte mainId = _msg.get();
        final byte subId = _msg.get();
        _msg.position(0);

        USLog.warn(getUSServer(), "player:{} msg {}-{} not dealed!", getCid(), mainId, subId);
    }

    /*******************
     * 用户的上下线操作，只可以通过UserMgr进行设置，避免数据状态值设置问题
     */
    public void online() {
        setOnlineTag(true);
    }

    public void offline() {
        setOnlineTag(false);
    }

    /**********************
     * 处理在线状态变化的处理
     * @param _isOnline
     */
    public void dealOnlineStateChg(long _serialize, boolean _isOnline) {
        _lockInfo();

        try {
            //判断序列号是否一致
            if (getSerialize() != _serialize) return;
        } finally {
            _unlockInfo();
        }

        //异步执行玩家身上针对于在线状态的处理
        ALSynTaskManager.getInstance().regTask(() ->
        {
            if (_isOnline) {
                //修改玩家Cache中最后在线时间
                PlayerCacheFunc.updateLastOnlineTime(this, CommonFunc.getNowTimeMS());
                //上线时检测跨天
                int nowSec = CommonFunc.getNowTimeSec();
                int nowTag = CommonFunc.getTimeTagYYYYMMDD(nowSec);
                checkLoginCrossDay(nowTag);
            } else {
                //修改玩家Cache中最后离线时间
                PlayerCacheFunc.updateLastOfflineTime(this, CommonFunc.getNowTimeMS());
            }

            //通知联盟玩家上下线

        });

    }

    /***************
     * 给与玩家
     *
     * @author alzq.z
     * @time 2019 下午11:44:53
     */
    public void initGiveItems() {
        //创建上下文对象
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.PLAYER_CREATE);
        //初始化给与玩家物品（配置于general表）
        initGainItem(RefGeneral.Ref().player_init_give_item, context);
    }

    public boolean isFirstCreate() {
        return _m_bFirstCreate;
    }

    public void markFirstCreate() {
        _m_bFirstCreate = true;
    }

    /// //////////////////////////// item deal start ////////////////////////////////
    public long getItemCount(ENPItemType _type, long _itemId) {
        return _m_dmItemDealerMgr.getItemCount(_type, _itemId);
    }

    public long getItemCount(NPCommonItem _commonItem) {
        return _m_dmItemDealerMgr.getItemCount(_commonItem.getItemType(), _commonItem.getItemId());
    }

    //获取玩家值
    public long getValue(ENPPlayerValueType _valueType) {
        switch (_valueType) {
            case LVL:
                return getPlayerComponent().getParamV(ENPPlayerParam.LEVEL);
            case VIP_LVL:
                return getPlayerComponent().getParamV(ENPPlayerParam.VIP_LVL);
            case RMB_NUM:
                return 0;
            case FRIEND_COUNT:
                return getFriendComponent().getFriendMgr().getFriendCount();
            case HERO_NUM:
                return getHeroComponent().getHeroNum();
            case HERO_IN_BUILDING_NUM:
                return getHeroComponent().getHeroInBuildingNum();
            case EARNINGS:
                return getPlayerComponent().getEarnings();
            case CHILD_SUM:
                return getChildComponent().getChildMgr().getChildCount();
            case TOTAL_HERO_POWER:
                return getHeroComponent().getTotalPower();
            case TOTAL_HERO_TALENT:
                return getHeroComponent().getTotalTalent();
            case CONSORT_NUM:
                return getConsortComponent().getConsortNum();
            case CHAPTER_POINT:
                return getChapterComponent().getChapterInfo().getChapterProgress();
            case BUILDING_NUM:
                return getBuildingComponent().getBuildingNum();
            case TOTAL_HERO_LEVEL:
                return getHeroComponent().getTotalHeroLevel();
            case TOTAL_CONSORT_INTIMACY:
                return getConsortComponent().getTotalConsortIntimacy();
            case TOTAL_CONSORT_CHARM:
                return getConsortComponent().getTotalConsortCharm();
            case DAILY_CHECK_SUM:
                return getDailyCheckComponent().getDailyCheckSum();
            case DONE_MAIN_QUEST_COUNT:
                return getQuestComponent().getCountMgr().getMainQuestCount();
            case STAGE_GOAL_DONE_STEP:
                return getStageGoalComponent().getDoneStep();
            case INN_POPULARITY:
                return getInnComponent().getPopularity();
            case INN_LEVEL:
                return getInnComponent().getInnLevel();
            case TOWER_PASSED_CHAPTER:
                return getTowerComponent().getPassedChapter();
            case INN_HAD_UNLOCK_DISH_NUM:
                return getInnComponent().getDishMgr().getHadUnlockDishNum();
            case TOWER_PASSED_LVL:
                return getTowerComponent().getPassedTotalLevel();
            case INN_STATION_TOTAL_LEVEL:
                return getInnComponent().getStationMgr().getTotalStationLevel();
            case INN_HAD_RECEIVE_GUEST_COUNT:
                return getInnComponent().getHadReceiveGuestNum();
            case TREASURE_HUNT_STATION_LEVEL:
                return getTreasureHuntComponent().getStationLevel();
            case TREASURE_HUNT_HAD_GAIN_ORE_TYPE_COUNT:
                return getTreasureHuntComponent().getOreMgr().getOreNum();
            case TREASURE_HUNT_HAD_GAIN_TREASURE_TYPE_COUNT:
                return getTreasureHuntComponent().getTreasureMgr().getTreasureNum();
            case TREASURE_HUNT_HAD_COLLECT_COMPOSITE_COUNT:
                return getTreasureHuntComponent().getCompositeMgr().getHadCollectCount();
            case NAMED_CHILD_NUM:
                return getChildComponent().getChildMgr().getNamedChildNum();
            case MUSEUM_ITEM_NUM:
                return getMuseumComponent().getItemMgr().getItemNum();
            case CHAPTER_ID:
                return getChapterComponent().getChapterInfo().getChapterId();
            case GUILD_LEVEL: {
                return getGuildComponent().getGuildLvl();
            }
            case UNMARRY_ADULT_SUM:
                return getChildComponent().getAdultMgr().getUnmarryAdultCount();
            case MARS_EXPLORER_NUM:
                return getMarsExploreComponent().getExploreInfo().getExploreSum();
            case TOWER_ACTIVE_RESEARCH_COUNT:
                return getTowerComponent().getActiveResearchCount();

            //GOB-7523【GOB-0】增加成就计数器----火星所有建筑所有部件总等级
            //https://www.teambition.com/task/693b88cf29c8b54653f2247d
            case MARS_BUILDING_EQUIP_LVL_SUM:
                return getMarsBuildingComponent().getAllBuildingEquipLvlSum();

            //GOB-7686【GOB-0】增加成就计数器----火星建筑总等级
            //https://www.teambition.com/task/6942707a5bf8619f47227e78
            case MARS_BUILDING_LVL_SUM:
                return getMarsBuildingComponent().getAllBuildingLvlSum();
            case INN_MEDAL_LEVEL:
                return getInnComponent().getMedalLevel();
            case TOTAL_GAIN_GOLD_COUNT:
                return getCurrencyComponent().getTotalGainCount(ECurrency.SILVER);

            default:
                return 0L;
        }
    }

    public boolean hasCostItemList(List<NPCommonCostItem> _itemList) {
        return hasCostItemList(_itemList, ENPInsteadItemType.NONE);
    }

    public boolean hasCostItemList(List<NPCommonCostItem> _itemList, ENPInsteadItemType _eAlterType) {
        if (null == _itemList) return true;

        for (int i = 0; i < _itemList.size(); i++) {
            NPCommonCostItem obj = _itemList.get(i);
            if (null == obj) continue;
            if (obj.getCount() <= 0) continue;

            long itemCount = getItemCount(obj.getItemType(), obj.getItemId());
            if (itemCount < obj.getCount()) {
                ArrayList<RefItemAlter> alterList = RefItemAlter.getMgr().lookupItemAlterEDList(_eAlterType, obj.getItemType(), obj.getItemId());
                if (alterList == null) {
                    return false;
                }
                long alterCount = 0;
                for (int j = 0; j < alterList.size(); j++) {
                    RefItemAlter refAlter = alterList.get(j);
                    if (null == refAlter) continue;
                    alterCount += getItemCount(refAlter.alter_item_type, refAlter.alter_item_id);
                }
                if (alterCount + itemCount < obj.getCount()) return false;
            }
        }

        return true;
    }

    public boolean hasItem(ENPItemType _type, long _itemId, long _count) {
        return _m_dmItemDealerMgr.hasItem(_type, _itemId, _count);
    }

    public boolean hasItem(NPCommonCostItem _item) {
        if (null == _item) return false;
        return hasItem(_item.getItemType(), _item.getItemId(), _item.getCount());
    }

    public boolean hasItem(NPCommonItem _commItem, long _count) {
        return hasItem(_commItem.getItemType(), _commItem.getItemId(), _count);
    }

    /***************************************************
     * 获得批量物：合并（否） + 推送（是）
     ***************************************************/
    public void gainItemListP(NPCommon_ItemList _itemList, NPPlayerContext _context) {
        if (null == _itemList)
            return;

        this.gainItemListP(_itemList.getItemList(), _context);
    }

    public void gainItemListP(List<NPCommon_ItemInfo> _itemList, NPPlayerContext _context) {
        if (null == _itemList)
            return;

        for (NPCommon_ItemInfo itemInfo : _itemList) {
            if (itemInfo == null)
                continue;

            ENPItemType enpItemType = ENPItemType.ENPItemType_FromInt(itemInfo.getItemType());
            if (enpItemType == null)
                continue;

            gainItem(enpItemType, itemInfo.getSubId(), itemInfo.getCount(), _context);
        }
    }

    public void gainItemList(List<NPCommonCostItem> _itemList, NPPlayerContext _context) {
        if (null == _itemList) return;

        for (int i = 0; i < _itemList.size(); i++) {
            NPCommonCostItem obj = _itemList.get(i);
            if (null == obj)
                continue;

            gainItem(obj.getItemType(), obj.getItemId(), obj.getCount(), false, 0, _context);
        }
    }

    /***************************************************
     * 获得批量物品：合并（选） + 推送（选）
     ***************************************************/
    public void gainItemList(List<NPCommonCostItem> _itemList, boolean _isNotMerge, NPPlayerContext _context) {
        if (null == _itemList) return;

        for (int i = 0; i < _itemList.size(); i++) {
            NPCommonCostItem obj = _itemList.get(i);
            if (null == obj)
                continue;

            gainItem(obj.getItemType(), obj.getItemId(), obj.getCount(), _isNotMerge, 0, _context);
        }
    }

    public void gainItemList(List<NPCommonCostItem> _itemList, int _multi, boolean _isNotMerge, NPPlayerContext _context) {
        if (null == _itemList) return;

        for (int i = 0; i < _itemList.size(); i++) {
            NPCommonCostItem obj = _itemList.get(i);
            if (null == obj)
                continue;

            gainItem(obj.getItemType(), obj.getItemId(), obj.getCount() * _multi, _isNotMerge, 0, _context);
        }
    }

    /***************************************************
     * 获得物品：合并（否） + 推送（是）
     ***************************************************/
    public void gainItemP(NPCommon_ItemInfo _item, NPPlayerContext _context) {
        if (null == _item)
            return;

        gainItem(ENPItemType.ENPItemType_FromInt(_item.getItemType()), _item.getSubId(), _item.getCount(), _context);
    }

    public void gainItem(NPCommonItem _item, long _count, NPPlayerContext _context) {
        gainItem(_item.getItemType(), _item.getItemId(), _count, _context);
    }

    public void gainItem(NPCommonCostItem _item, NPPlayerContext _context) {
        gainItem(_item.getItemType(), _item.getItemId(), _item.getCount(), _context);
    }

    public void gainItem(ENPItemType _type, long _itemId, NPPlayerContext _context) {
        gainItem(_type, _itemId, 1, _context);
    }

    public void gainItem(ENPItemType _type, long _itemId, long _count, NPPlayerContext _context) {
        gainItem(_type, _itemId, _count, false, 0, _context);
    }

    /***************************************************
     * 获得物品：合并（选） + 推送（选）
     ***************************************************/
    public void gainItem(NPCommonCostItem _item, boolean _isNotMerge, NPPlayerContext _context) {
        gainItem(_item.getItemType(), _item.getItemId(), _item.getCount(), _isNotMerge, 0, _context);
    }

    public void gainItem(ENPItemType _type, long _itemId, long _count, boolean _isNotMerge, NPPlayerContext _context) {
        gainItem(_type, _itemId, _count, _isNotMerge, 0, _context);
    }

    public void gainItem(ENPItemType _type, long _itemId, long _count, boolean _isNotMerge, int _exchangeStackNum, NPPlayerContext _context) {
        //先检测替换获得，如果没有替换获得就直接获得
        if (!__exchangeItemOnGainTime(_type, _itemId, _count, _isNotMerge, _exchangeStackNum, _context)) {
            //调用处理对象，具体的事件等处理，在每个处理类中自行处理
            _m_dmItemDealerMgr.gainItem(_type, _itemId, _count, _isNotMerge, _context);
        }
    }

    /**
     * 替换获得道具，查找itemExchange表，检查要获得的道具是否能进行转换，如果可以则转换获得，不可以返回false
     *
     * @return 是否成功
     */
    private boolean __exchangeItemOnGainTime(ENPItemType _type, long _itemId, long _count, boolean _isNotMerge, int _exchangeStackNum, NPPlayerContext _context) {
        //递归调用保护
        if (_exchangeStackNum >= STACK_PROTECT) {
            return false;
        }

        //查找道具替换获得配置
        RefItemExchange dealRef = getRefItemExchange(_type, _itemId, EExchangeItemDealType.GAIN_ITEM_TIME);
        //没有满足条件的转换
        if (dealRef == null) {
            return false;
        }

        //构造新的上下文对象，用于递归调用时记录下一层获得的物品
        NPPlayerContext newContext = NPPlayerContext.createNew(_context);
        //递归次数增加
        int exchangeStackNum = _exchangeStackNum + 1;
        //替换获得
        NPCommonCostItem targetItem = CommonFunc.itemMultiple(dealRef.target_item, (int) _count);
        //获得替换道具
        gainItem(targetItem.getItemType(), targetItem.getItemId(), targetItem.getCount(), _isNotMerge, exchangeStackNum, newContext);

        //构造展示对象
        NPCommon_ItemInfo itemInfo = new NPCommon_ItemInfo();
        itemInfo.setItemType(_type.ordinal());
        itemInfo.setSubId(_itemId);
        itemInfo.setCount(_count);
        Common_ItemExchangeInfo itemInfoList = new Common_ItemExchangeInfo();
        itemInfoList.setExchangeRefId(dealRef.id);
        newContext.getCollector().fillProtoList(itemInfoList.getItemList());
        itemInfo.setExtData(itemInfoList.makePackage());
        _context.getCollector().addNoMergeItem(itemInfo);

        return true;
    }

    /**
     * 获取一个可处理的物品转换配置
     *
     * @param _type     原物品类型
     * @param _itemId   原物品id
     * @param _dealType 处理时机类型
     * @return RefItemExchange
     */
    private RefItemExchange getRefItemExchange(ENPItemType _type, long _itemId, EExchangeItemDealType _dealType) {
        //查找道具替换获得配置
        ArrayList<RefItemExchange> refList = RefItemExchange.getMgr().lookupExchange(_type, _itemId, _dealType);
        //本次要处理的配置
        RefItemExchange dealRef = null;
        for (RefItemExchange ref : refList) {
            //检查转换条件
            boolean isEnable = NPPlayerConditionDealerMgr.IsEnable(ref.cond, this, null);
            //不满足转换条件
            if (!isEnable) {
                continue;
            }
            dealRef = ref;
        }
        return dealRef;
    }

    public void initGainItem(List<NPCommonCostItem> _itemList, NPPlayerContext _context) {
        if (null == _itemList) return;

        for (int i = 0; i < _itemList.size(); i++) {
            NPCommonCostItem obj = _itemList.get(i);
            if (null == obj) continue;

            initGainItem(obj.getItemType(), obj.getItemId(), obj.getCount(), _context);
        }
    }

    public void initGainItem(ENPItemType _type, long _itemId, long _count, NPPlayerContext _context) {
        _m_dmItemDealerMgr.initGainItem(_type, _itemId, _count, _context);
    }

    /***************************************************
     * 消耗物品：推送（是）
     ***************************************************/
    public boolean spendItem(NPCommonItem _commItem, long _count, NPPlayerContext _context) {
        return spendItem(_commItem.getItemType(), _commItem.getItemId(), _count, _context);
    }

    public boolean spendItem(NPCommonCostItem _costItem, NPPlayerContext _context) {
        return spendItem(_costItem.getItemType(), _costItem.getItemId(), _costItem.getCount(), _context);
    }

    public boolean spendItem(List<NPCommonCostItem> _itemList, NPPlayerContext _context) {
        if (null == _itemList)
            return true;

        for (int i = 0; i < _itemList.size(); i++) {
            NPCommonCostItem obj = _itemList.get(i);
            if (null == obj)
                continue;

            if (!spendItem(obj.getItemType(), obj.getItemId(), obj.getCount(), _context))
                return false;
        }

        return true;
    }

    public boolean spendCostItemList(List<NPCommonCostItem> _itemList, NPPlayerContext _context) {
        return spendCostItemList(_itemList, ENPInsteadItemType.NONE, _context);
    }

    public boolean spendCostItemList(List<NPCommonCostItem> _itemList, ENPInsteadItemType _alterType, NPPlayerContext _context) {
        if (null == _itemList) return true;

        for (int i = 0; i < _itemList.size(); i++) {
            NPCommonCostItem obj = _itemList.get(i);
            if (null == obj) continue;

            //使用替代扣除接口处理
            if (!_m_dmItemDealerMgr.spendItem(obj.getItemType(), obj.getItemId(), obj.getCount(), _alterType, _context))
                return false;
        }

        return true;
    }

    //消耗掉现拥有的所有数量物品
    public void spendAll(NPCommonItem _item, NPPlayerContext _context) {
        long itemCount = getItemCount(_item.getItemType(), _item.getItemId());

        if (itemCount > 0) {
            spendItem(_item.getItemType(), _item.getItemId(), itemCount, _context);
        }
    }

    public boolean spendItem(ENPItemType _type, long _itemId, long _count, NPPlayerContext _context) {
        return _m_dmItemDealerMgr.spendItem(_type, _itemId, _count, _context);
    }

    /***************************************************
     * 获得奖励：合并（配置/否） + 推送（选）
     ***************************************************/
    public boolean gainReward(long _rewardId, NPPlayerContext _context) {
        RewardObj rewardObj = RewardMgr.getInstance().lookupReward(_rewardId);
        if (null == rewardObj) {
            USLog.error(getUSServer(), "Can not find reward for id:{}", _rewardId, new Exception());
            return false;
        }

        List<NPCommonCostItem> itemList = rewardObj.getItemList();

        //获取物品
        gainItemList(itemList, !rewardObj.getRef().is_merge, _context);

        return true;
    }

    public boolean gainReward(long _rewardId, long _count, NPPlayerContext _context) {
        RewardObj rewardObj = RewardMgr.getInstance().lookupReward(_rewardId);
        if (null == rewardObj) {
            USLog.error(getUSServer(), "Can not find reward for id:{}", _rewardId, new Exception());
            return false;
        }

        NPItemCostCollector_nosafe itemCollect = new NPItemCostCollector_nosafe();
        for (int i = 0; i < _count; i++) {
            itemCollect.addItemList(rewardObj.getItemList());
        }

        //获取物品
        gainItemList(itemCollect.getItemList(), !rewardObj.getRef().is_merge, _context);

        return true;
    }

    public void gainReward(ArrayList<Long> _rewardList, NPPlayerContext _context) {
        if (null == _rewardList || _rewardList.isEmpty()) return;

        for (int i = 0; i < _rewardList.size(); i++) {
            //必须使用 gain item 获取，这样才能正常触发事件
            gainItem(ENPItemType.REWARD, _rewardList.get(i).longValue(), 1, false, _context);
        }
    }

    public boolean gainRewardMultiple(long _rewardId, int _multiple, NPPlayerContext _context) {
        RewardObj rewardObj = RewardMgr.getInstance().lookupReward(_rewardId);
        if (null == rewardObj) {
            USLog.error(getUSServer(), "Can not find reward for id:{}", _rewardId, new Exception());
            return false;
        }

        //构造奖励物品列表数据
        NPItemCostCollector_nosafe itemCollect = new NPItemCostCollector_nosafe();
        List<NPCommonCostItem> itemList = rewardObj.getItemList();
        for (int i = 0; i < itemList.size(); i++) {
            NPCommonCostItem item = itemList.get(i);
            if (null == item)
                continue;

            itemCollect.addItem(item.multi(_multiple));
        }

        //获取物品
        gainItemList(itemCollect.getItemList(), !rewardObj.getRef().is_merge, _context);

        return true;
    }

    /// //////////////////////////// item deal end ////////////////////////////////

    public void tick1Sec() {
        //玩家登录游戏后才可以tick
        if (!_m_bLogicOnline) return;

        //加载完成之后才可以进行tick
        if (_m_eUserDataState != ENPUserDataState.LOADED) return;

        getComponentMgr().tickComponents();
    }

    public void checkLoginCrossDay(int nowDateTag) {
        //加载完成之后才可以进行tick
        if (_m_eUserDataState != ENPUserDataState.LOADED) return;

        int lastTag = (int) getParam(ENPPlayerParam.LAST_LOGIN_DATE);
        //跨天登录，触发事件
        if (nowDateTag != lastTag) {
            getPlayerComponent().setParam(ENPPlayerParam.LAST_LOGIN_DATE, nowDateTag);
            getPlayerComponent().incParam(ENPPlayerParam.LOGIN_DAY_COUNT);
            onLoginCrossDay(nowDateTag);

            //检查是否跨周
            int nowWeekTag = CommonFunc.getWeekId();
            int lastWeekTag = (int) getParam(ENPPlayerParam.LAST_LOGIN_WEEK_TAG);
            //跨周处理
            if (lastWeekTag != nowWeekTag) {
                //设置玩家登录的周TAG
                getPlayerComponent().setParam(ENPPlayerParam.LAST_LOGIN_WEEK_TAG, nowWeekTag);
                //重置玩家每周的登录次数
                getPlayerComponent().setParam(ENPPlayerParam.WEEK_LOGIN_DAY_COUNT, 1);
            } else {
                //增加玩家每周的登录次数
                getPlayerComponent().incParam(ENPPlayerParam.WEEK_LOGIN_DAY_COUNT);
            }
        }
    }

    public void onLoginCrossDay(int nowTag) {
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.CORSSDAY_LOGIN);

        //登录天数
        NPSynPlayerEvnetRecordTask.asyncRecord(this, ENCounterDealType.SET_GT,
                EPlayerEventRecordType.LOGIN_DAY_SUM.ordinal(), 0, getPlayerComponent().getParamV(ENPPlayerParam.LOGIN_DAY_COUNT));

        //更新开服天数，设置的数值不会更新，但是会进行重新计算并推送
        getPlayerComponent().setParam(ENPPlayerParam.SERVER_START_DAYS, -1);
        //更新七日登录天数
        getPlayerComponent().setParam(ENPPlayerParam.SEVEN_DAYS_LOGIN_COUNT, -1);
        getPlayerComponent().setParam(ENPPlayerParam.DAY_HAD_SEND_PAY_AI_TIMES, -1);

        //触发事件
        Event_P_CROSS_DAY evt = new Event_P_CROSS_DAY(context, nowTag);
        onLogicEvent(evt);

        //跨天监听事件执行
        OnCrossDay.onEvent(nowTag);
    }

    /**
     * 通用检查数量是否合法
     * <p>
     * GOB-6108【优化-0】服务端在购买或者相关带入数字消耗的协议处理上，要对数量做合法性校验，过大过小都不允许
     * https://www.teambition.com/task/69048cbe30597a4ec8a4bfc9
     *
     * @param _count
     * @return
     */
    public boolean checkItemCount(long _count) {
        return _count > 0 && _count <= RefGeneral.Ref().paramLimit;
    }

    public boolean checkItemCount(ArrayList<NPCommon_ItemInfo> _itemList) {
        for (int i = 0; i < _itemList.size(); i++) {
            NPCommon_ItemInfo item = _itemList.get(i);
            if (null == item)
                continue;

            if (!checkItemCount(item.getCount()))
                return false;
        }

        return true;
    }

    /**
     * 返回玩家数据是否加载完成，如未加载完成部分操作不允许操作
     * @return
     */
    public boolean isLoaded()
    {
        return (getUserDataState() == NPServerEnum.ENPUserDataState.LOADED);
    }
    /*****
     *安全加锁调用，确保玩家加载完成后调用
     */
    public void safeCall(final UserSafeCall._IUserLoadOverHandler _handler) {
        _m_safeCall.safeCall(_handler);

    }

    /*****
     *不加锁调用，确保玩家加载完成后调用
     */
    public void unlockedCall(final UserSafeCall._IUserLoadOverHandler _handler) {
        _m_safeCall.unlockedCall(_handler);
    }

    /*****
     * 返回玩家参数
     * @param _eParam
     * @return
     */
    public long getParam(ENPPlayerParam _eParam) {
        return getPlayerComponent().getParamV(_eParam);
    }

    /**
     * 设置玩家参数
     *
     * @param _eParam
     * @param _value
     */
    public void setParam(ENPPlayerParam _eParam, long _value) {
        getPlayerComponent().setParam(_eParam, _value);
    }

    /**
     * 增加玩家参数
     *
     * @param _eParam
     * @param _value
     */
    public void incParam(ENPPlayerParam _eParam, long _value) {
        getPlayerComponent().incParam(_eParam, _value);
    }


    /**
     * 主动转换物品,先消耗背包里的指定物品，在做转换获得
     *
     * @param _itemType 需要转换的物品类型
     * @param _itemId   需要转换的物品id
     * @param _type     转换时机
     * @param _context  上下文
     */
    public void exchangeItem(ENPItemType _itemType, long _itemId, EExchangeItemDealType _type, NPPlayerContext _context) {
        //原物品为空
        long count = getItemCount(_itemType, _itemId);
        if (count <= 0) {
            return;
        }
        exchangeItem(_itemType, _itemId, count, _type, _context);
    }

    /**
     * 主动转换物品,先消耗背包里的指定物品，在做转换获得
     *
     * @param _itemType 需要转换的物品类型
     * @param _itemId   需要转换的物品id
     * @param _type     转换时机
     * @param _context  上下文
     */
    public void exchangeItem(ENPItemType _itemType, long _itemId, long _count, EExchangeItemDealType _type, NPPlayerContext _context) {

        //原物品为空
        if (_count <= 0) {
            return;
        }
        //找到转换配置
        RefItemExchange ref = getRefItemExchange(_itemType, _itemId, _type);
        if (ref == null) {
            return;
        }

        if (!spendItem(ref.ori_item.getItemType(), ref.ori_item.getItemId(), _count, _context)) {
            return;
        }
        //在context里添加一个替换道具，这个用来做转换弹窗
        _context.getCollector().addItem(ENPItemType.EXCHANGE_ITEM, ref.id, _count);
        //替换获得
        NPCommonCostItem targetItem = CommonFunc.itemMultiple(ref.target_item, (int) _count);

        //获得转换后的物品
        gainItem(targetItem.getItemType(), targetItem.getItemId(), targetItem.getCount(), false, 1, _context);
    }

    /********
     * 构造聊天玩家协议
     * @return
     */
    public NPCommon_ChatPlayerContent toChatPlayerProto()
    {
        NPCommon_ChatPlayerContent proto = new NPCommon_ChatPlayerContent();
        proto.setCid(getCid());
        proto.setCName(getPlayerComponent().getName());
        proto.setBubbleId(getParam(ENPPlayerParam.BUBBLE));
        proto.setIconId(getParam(ENPPlayerParam.ICON));
        proto.setIconBgkId(getParam(ENPPlayerParam.ICON_BGK));
        proto.setCurTitle(getPlayerComponent().getCurTitleMgr().getCurTitle());
        proto.setIsShow(getPlayerComponent().getCurTitleMgr().getShow());
        proto.setPlayerSkinId(getParam(ENPPlayerParam.PLAYER_SKIN));

        return proto;
    }

    /**
     * 构造活动队伍玩家协议
     * @return
     */
    public ServerObj_ActivityTeamUser toActivityTeamUserProto()
    {
        ServerObj_ActivityTeamUser proto = new ServerObj_ActivityTeamUser();
        proto.setCid(getCid());

        return proto;
    }

    /**
     * 构造跨服队伍申请信息协议列表，用于确认玩家是否符合请求条件
     * @return
     */
    public ArrayList<CrossTeam_SetInfo_Join> makeCrossTeamApplyValueList()
    {
        ArrayList<CrossTeam_SetInfo_Join> list = new ArrayList<>();

        // 英雄总战力
        CrossTeam_SetInfo_Join powerCond = new CrossTeam_SetInfo_Join();
        powerCond.setCond(ENPCrossTeamJoinCond.MIN_POWER);
        powerCond.setValue(getHeroComponent().getTotalPower());
        list.add(powerCond);

        // 火星最大战力
        CrossTeam_SetInfo_Join marsPowerCond = new CrossTeam_SetInfo_Join();
        marsPowerCond.setCond(ENPCrossTeamJoinCond.MIN_MARS_POWER);
        marsPowerCond.setValue(getMarsComponent().getMarsPower());
        list.add(marsPowerCond);

        return list;
    }

    /***************
     * 更新玩家当前语言
     * @param _lang
     */
    public void setLang(String _lang)
    {
        safeCall(() ->
        {
            getPlayerComponent().getBo().savePlayerLang(getUSServer().getBM(), _lang);

            //修改玩家缓存
            PlayerCacheFunc.updateLanguage(this, _lang);
        });
    }

    /**
     * 记录玩家操作事件
     * @param _logBo   日志
     * @param _context 上下文
     */
    public void logEvent(BaseOptLogBo _logBo, NPPlayerContext _context)
    {
        BM bmObj = getUSServer().getBM();

        _logBo.setCid(bmObj, getCid());
        _logBo.setLevel(bmObj, (int) getParam(ENPPlayerParam.LEVEL));
        _logBo.setVipLvl(bmObj, (int) getParam(ENPPlayerParam.VIP_LVL));
        _logBo.setEventId(bmObj, _context.getContextId());
        _logBo.setGuid(bmObj, _context.getGuid());
        _logBo.setDateTime(bmObj, CommonFunc.getNowTagYYYYMMDD());
        _logBo.setTimestamp(bmObj, CommonFunc.getNowTimeSec());
        _logBo.insert(bmObj);
    }
}
