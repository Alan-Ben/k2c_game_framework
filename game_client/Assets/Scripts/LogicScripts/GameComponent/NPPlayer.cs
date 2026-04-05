
using System.Collections.Generic;
using ALPackage;
using GOE.BonusSpace;
using GOE.Condition;
using GOE.Variable;
using NPEnum;

namespace GOE
{
    //玩家数据管理类
    public partial class NPPlayer : _ITNPConditionDealerData<ENPPlayerConditionType>, _ITNPVariableDealerData<ENPPlayerVariableType>
    {
        private static NPPlayer _g_instance;
        protected internal static NPPlayer __getInstanceObj() { return _g_instance; }

        public static NPPlayer instance
        {
            get
            {
                //如果游戏都还未初始化完成则直接返回错误
                if (!GRefdataCoreMgr.instance.isInitDone)
                    return _g_instance;

                if (_g_instance == null)
                {
                    _g_instance = new NPPlayer();
                }
                return _g_instance;
            }
        }
        public static void resetNPPlayer()
        {
            if (null != _g_instance)
                _g_instance.discard();
            _g_instance = new NPPlayer();
        }

        /// <summary>
        /// 客户端数据序列号，用于避免gs消息被错误处理增加的识别序列号
        /// </summary>
        private long _m_lPlayerSerialize;

        //玩家信息
        private PlayerInfoComponent _m_piPlayerInfo;

        //玩家资源信息
        private NPPlayerResourceComponent _m_prPlayerRescource;
        //特殊物品数据
        private PlayerSpecialItemComponent _m_specialItemComponent;

        //玩家背包
        private PlayerBagComponent _m_pbcPlayerBag;
        //玩家称号
        private PlayerTitleComponent _m_ptPlayerTitleComponent;
        //玩家皮肤
        private PlayerSkinComponent _m_ptPlayerSkinComponent;
        //玩家头像
        private NPPlayerIconComponent _m_piPlayerIconComponent;
        //玩家头像框
        private NPPlayerIconBgkComponent _m_pibPlayerIconBgkComponent;
        //玩家气泡框
        private NPPlayerBubbleComponent _m_pbPlayerBubbleComponent;

        //玩家buff信息
        private NPPlayerBuffComponent _m_pbcPlayerBuffComponent;

        //玩家各组件管理对象
        private NPPlayerComponentMgr _m_mgrCompMgr;

        //战斗外引导提示管理对象
        private NPPlayerTutorialComponent _m_tcTutorialComponent;
        
        //玩家属性管理器
        private NPPlayerPropertyMgr _m_pmPropertyMgr;
        
        //玩家邮件组件
        private GPlayerMailComponent _m_pmcMailComponent;
        // 聊天组件
        private NPPlayerChatComponent _m_chatComponent;

        //骑士组件
        private PlayerHeroComponent _m_heroComponent;

        //通用排行榜组件
        private NPRankCommonComponent _m_rankCommonComp;
        //CD组件
        private NPPlayerLazyCdComponent _m_lazyCdComp;
        //任务组件
        private PlayerQuestComponent _m_questComp;

        //日常任务组件
        private PlayerDailyQuestComponent _m_dailyQuestComp;

        //玩家计数组件
        private NPPlayerRecordComponent _m_recordComp;

        //玩家行为计数组件
        private PlayerEventRecordComponent _m_eventRecordComp;

        //功能解锁组件
        private PlayerFuncUnlockComponent _m_funcUnlockComp;

        //玩家身上所有加成的总管理对象,这个是最高父节点，后续所有模块的外围加成把父节点设成这个mgr
        private PlayerUnionBonusMgr _m_playerBonusMgr;
        
        //固定时间恢复的CD组件
        private NPPlayerFixedCDComponent _m_fixedCdComp;

        //好友组件
        private PlayerFriendsComponent _m_friendsComp;
        
        //成就组件
        private PlayerAchieveComponent _m_achieveComp;
        //通用目标奖励组件
        private CommonTargetRewardComponent _m_commonTargetRewardComp;
        //商店组件
        private NPPlayerShopComponent _m_shopComp;
        //离线奖励组件
        private PlayerOfflineRewardComponent _m_offlineRewardComp;

        //情人组件
        private GConsortComponent _m_ConsortComp;

        private PlayerChapterComponent _m_chapterComp;

        private GPlayerDinnerComponent _m_dinnerComp;

        private CommonActivityComponent _m_commonActivityComponent;//通用活动组件
        private CommonActivityHotRefComponent _m_commonActivityHotRefComponent;//活动热更配表组件

        private GPlayerTravelComponent _m_travelComp;

        //跑马灯组件
        private MarqueeComponent _m_marqueeComp;
        private GWeekCardComponent _m_weekCardComp;
        private RankGiftPackComponent _m_rankGiftPackComp;

        private PlayerCuteActorComponent _m_cuteActorComp;//Q版形象组件

        //问卷调查
        private PlayerQuestionnaireComponent _m_questionnaireComp;
        //联盟组件
        private GuildComponent _m_guildComp;
        
        private PlayerBuildingComponent _m_buildingComp;
        //藏品组件
        private PlayerEquipComponent _m_equipComp;

        private PlayerChildComponent _m_childComp;
        
        private GachaComponent _m_gachaComp;//抽卡组件
        
        private RecruitComponent _m_recruitComp;//招募组件
        //竞技场组件
        private ArenaComponent _m_arenaComp;
        //贸易站组件
        private StationComponent _m_stationComp;
        // 阶段目标组件
        private StageGoalComponent _m_stageGoalComp;
        
        private PlayerCommonRefreshComponent _m_commonRefreshComp;
        
        private PlayerAnecdoteComponent _m_anecdoteComp;
        
        // 爬塔组件
        private TowerComponent _m_towerComp;

        private MiddayDungeonComponent _m_middayDungeonComp; //午间副本组件

        private CommonItemCountComponent _m_commonItemCountComp;
        
        private SevenDayLoginComponent _m_sevenDayLoginComp; //七天登录组件

        private SevenDayGoalsComponent _m_sevenDayGoalsComp; //七日目标组件
        
        private EarningGoalComponent _m_earningGoalComp; //收益目标组件
        
        private ConsortChatComponent _m_consortChatComp; //情人聊天组件
        
        private PlayerInnComponent _m_innComp; //旅店组件

        private PlayerMuseumComponent _m_museumComp;
        
        /// <summary>
        /// 晚间副本组件
        /// </summary>
        private EveningDungeonComponent _m_eveningDungeonComp;

        //倒计时事件组件
        private CountdownEventComponent _m_countdownEventComp;
        
        private TreasureHuntComponent _m_treasureHuntComp; //太空寻宝组件

        private PlayerSystemQuestComponent _m_systemQuestComp;
        
        //礼包组件
        private GiftPackComponent _m_giftPackComp;
        //支付订单组件
        private PayOrderComponent _m_payOrderComp;

        //杰出者大厅组件
        private GraveComponent _m_graveComp;
        
        private GuildDungeonComponent _m_guildDungeonComp;
        
        private GuildMarsHelpComponent _m_guildMarsHelpComp;

        private MarsComponent _m_marsComp; // 火星基地组件

        //联盟协作组件
        private GuildCooperateComponent _m_guildCooperateComp;
        //充值返利组件
        private RechargeRebateComponent _m_rechargeRebateComp;
        //通用红点组件
        private RedDotComponent _m_redDotComp;
        //权益卡组件
        private PrivilegeCardComponent _m_privilegeCardComp;
        //玩家权限组件
        private PlayerPermissionsComponent _m_playerPermissionsComp;
        //推送礼包组件
        private PushGiftComponent _m_pushGiftComp;
        //永久属性加成组件
        private PlayerForeverAddComponent _m_foreverAddComponent;
        private GuildBoxComponent _m_guildBoxComp;
        //基金组件
        private PlayerFundComponent _m_fundComp;
        private RushExchangeComponent _m_rushExchangeComp; // 限时兑换组件
        // 房间皮肤组件
        private PlayerRoomSkinComponent _m_roomSkinComp;
        private LoverCollectComponent _m_loverCollectComp; // 情人收集组件

        public NPPlayer()
        {
            _m_lPlayerSerialize = ALSerializeOpMgr.next();

            _m_mgrCompMgr = new NPPlayerComponentMgr();
            _m_playerBonusMgr = new PlayerUnionBonusMgr();

            _m_piPlayerInfo = new PlayerInfoComponent(_m_mgrCompMgr);
            _m_prPlayerRescource = new NPPlayerResourceComponent(_m_mgrCompMgr);
            _m_specialItemComponent = new PlayerSpecialItemComponent(_m_mgrCompMgr);
            _m_pbcPlayerBag = new PlayerBagComponent(_m_mgrCompMgr);

            _m_pbcPlayerBuffComponent = new NPPlayerBuffComponent(_m_mgrCompMgr);
            _m_ptPlayerTitleComponent = new PlayerTitleComponent(_m_mgrCompMgr);
            _m_ptPlayerSkinComponent = new PlayerSkinComponent(_m_mgrCompMgr);
            _m_piPlayerIconComponent = new NPPlayerIconComponent(_m_mgrCompMgr);
            _m_pibPlayerIconBgkComponent = new NPPlayerIconBgkComponent(_m_mgrCompMgr);
            _m_pbPlayerBubbleComponent = new NPPlayerBubbleComponent(_m_mgrCompMgr);
            _m_rankCommonComp = new NPRankCommonComponent(_m_mgrCompMgr);
            _m_lazyCdComp = new NPPlayerLazyCdComponent(_m_mgrCompMgr);
            _m_questComp = new PlayerQuestComponent(_m_mgrCompMgr);
            _m_dailyQuestComp = new PlayerDailyQuestComponent(_m_mgrCompMgr);
            //引导数据组件
            _m_tcTutorialComponent = new NPPlayerTutorialComponent(_m_mgrCompMgr);

            _m_pmcMailComponent = new GPlayerMailComponent(_m_mgrCompMgr);

            _m_chatComponent = new NPPlayerChatComponent(_m_mgrCompMgr);

            _m_heroComponent = new PlayerHeroComponent(_m_mgrCompMgr);
            
            _m_recordComp = new NPPlayerRecordComponent(_m_mgrCompMgr);

            _m_eventRecordComp = new PlayerEventRecordComponent(_m_mgrCompMgr);

            _m_funcUnlockComp = new PlayerFuncUnlockComponent(_m_mgrCompMgr);

            _m_fixedCdComp = new NPPlayerFixedCDComponent(_m_mgrCompMgr);

            _m_friendsComp = new PlayerFriendsComponent(_m_mgrCompMgr);

            _m_achieveComp = new PlayerAchieveComponent(_m_mgrCompMgr);
            _m_commonTargetRewardComp = new CommonTargetRewardComponent(_m_mgrCompMgr);

            _m_shopComp = new NPPlayerShopComponent(_m_mgrCompMgr);
            _m_offlineRewardComp = new PlayerOfflineRewardComponent(_m_mgrCompMgr);
            
            _m_ConsortComp = new GConsortComponent(_m_mgrCompMgr);

            _m_dinnerComp = new GPlayerDinnerComponent(_m_mgrCompMgr);
            _m_travelComp = new GPlayerTravelComponent(_m_mgrCompMgr);
            _m_weekCardComp = new GWeekCardComponent(_m_mgrCompMgr);
            _m_rankGiftPackComp = new RankGiftPackComponent(_m_mgrCompMgr);

            _m_commonActivityComponent = new CommonActivityComponent(_m_mgrCompMgr);
            _m_commonActivityHotRefComponent = new CommonActivityHotRefComponent(_m_mgrCompMgr);

            _m_chapterComp = new PlayerChapterComponent(_m_mgrCompMgr);
            _m_marqueeComp = new MarqueeComponent(_m_mgrCompMgr);
            _m_cuteActorComp = new PlayerCuteActorComponent(_m_mgrCompMgr);
            _m_questionnaireComp = new PlayerQuestionnaireComponent(_m_mgrCompMgr);
            _m_guildComp = new GuildComponent(_m_mgrCompMgr);
            _m_buildingComp = new PlayerBuildingComponent(_m_mgrCompMgr);
            _m_equipComp = new PlayerEquipComponent(_m_mgrCompMgr);
            _m_childComp = new PlayerChildComponent(_m_mgrCompMgr);
            _m_gachaComp = new GachaComponent(_m_mgrCompMgr);
            _m_recruitComp = new RecruitComponent(_m_mgrCompMgr);
            _m_arenaComp = new ArenaComponent(_m_mgrCompMgr);
            _m_stationComp = new StationComponent(_m_mgrCompMgr);
            _m_stageGoalComp = new StageGoalComponent(_m_mgrCompMgr);
            _m_commonRefreshComp = new PlayerCommonRefreshComponent(_m_mgrCompMgr);
            _m_anecdoteComp = new PlayerAnecdoteComponent(_m_mgrCompMgr);
            
            _m_towerComp = new TowerComponent(_m_mgrCompMgr);
            _m_middayDungeonComp = new MiddayDungeonComponent(_m_mgrCompMgr);


            _m_commonItemCountComp = new CommonItemCountComponent(_m_mgrCompMgr);

            _m_eveningDungeonComp = new EveningDungeonComponent(_m_mgrCompMgr);
            _m_sevenDayLoginComp = new SevenDayLoginComponent(_m_mgrCompMgr);
            _m_sevenDayGoalsComp = new SevenDayGoalsComponent(_m_mgrCompMgr);
            _m_earningGoalComp = new EarningGoalComponent(_m_mgrCompMgr);
            _m_countdownEventComp = new CountdownEventComponent(_m_mgrCompMgr);

            _m_consortChatComp = new ConsortChatComponent(_m_mgrCompMgr);
            _m_systemQuestComp = new PlayerSystemQuestComponent(_m_mgrCompMgr);
            _m_giftPackComp = new GiftPackComponent(_m_mgrCompMgr);
            _m_payOrderComp = new PayOrderComponent(_m_mgrCompMgr);
            
            _m_innComp = new PlayerInnComponent(_m_mgrCompMgr);
            _m_museumComp = new PlayerMuseumComponent(_m_mgrCompMgr);

            _m_treasureHuntComp = new TreasureHuntComponent(_m_mgrCompMgr);
            
            _m_graveComp = new GraveComponent(_m_mgrCompMgr);

            _m_marsComp = new MarsComponent(_m_mgrCompMgr);
            _m_guildDungeonComp = new GuildDungeonComponent(_m_mgrCompMgr);
            _m_guildMarsHelpComp = new GuildMarsHelpComponent(_m_mgrCompMgr);
            _m_guildCooperateComp = new GuildCooperateComponent(_m_mgrCompMgr);
            _m_rechargeRebateComp = new RechargeRebateComponent(_m_mgrCompMgr);
            _m_redDotComp = new RedDotComponent(_m_mgrCompMgr);
            _m_privilegeCardComp = new PrivilegeCardComponent(_m_mgrCompMgr);
            _m_playerPermissionsComp = new PlayerPermissionsComponent(_m_mgrCompMgr);
            _m_pushGiftComp = new PushGiftComponent(_m_mgrCompMgr);
            _m_foreverAddComponent = new PlayerForeverAddComponent(_m_mgrCompMgr);
            _m_guildBoxComp = new GuildBoxComponent(_m_mgrCompMgr);
            _m_fundComp = new PlayerFundComponent(_m_mgrCompMgr);
            _m_rushExchangeComp = new RushExchangeComponent(_m_mgrCompMgr);
            _m_roomSkinComp = new PlayerRoomSkinComponent(_m_mgrCompMgr);
            _m_loverCollectComp = new LoverCollectComponent(_m_mgrCompMgr);



            //玩家属性管理
            _m_pmPropertyMgr = new NPPlayerPropertyMgr();

            //初始化注册相关属性容器
            _m_pmPropertyMgr.regPropertyContainer(_m_piPlayerInfo.playerInfo.playerPropertyContainer);
            _m_pmPropertyMgr.regPropertyContainer(_m_pbcPlayerBuffComponent.playerPropertyContainer);
            _m_pmPropertyMgr.regPropertyContainer(heroComponent.playerPropertyContainer);
            _m_pmPropertyMgr.regPropertyContainer(_m_privilegeCardComp.playerPropertyContainer);
            // 妃子带来的玩家属性加成
            if(_m_ConsortComp.getPlayerPropertyContainer() != null)
                _m_pmPropertyMgr.regPropertyContainer(_m_ConsortComp.getPlayerPropertyContainer());
            // 太空寻宝带来的玩家属性加成
            if(_m_treasureHuntComp.getPlayerPropertyContainer() != null)
                _m_pmPropertyMgr.regPropertyContainer(_m_treasureHuntComp.getPlayerPropertyContainer());
            // 火星基地带来的玩家属性加成
            _m_pmPropertyMgr.regPropertyContainer(_m_marsComp.playerPropertyContainer);
            _m_pmPropertyMgr.regPropertyContainer(_m_marsComp.technologySubComponent.playerPropertyContainer);
            //永久加成带来的属性加成
            _m_pmPropertyMgr.regPropertyContainer(_m_foreverAddComponent.playerPropertyContainer);

            
            // 收藏品属性在获得建筑才加上，改到建筑comp里面注册
            // _m_pmPropertyMgr.regPropertyContainer(_m_museumComp.playerPropertyContainer);
            _m_pmPropertyMgr.initProperties();

            //每帧执行容器计算
            ALMonoTaskMgr.instance.addMonoTask(new CalculatePlayerPropertiesTask());
        }

        public long npplayerSerialize { get { return _m_lPlayerSerialize; } }

        //玩家各组件管理对象
        public PlayerInfo playerInfo { get { return _m_piPlayerInfo.playerInfo; } }

        public NPPlayerComponentMgr compMgr { get { return _m_mgrCompMgr; } }
        //玩家需要提示的获取物品信息对象
        public PlayerInfoComponent playerInfoComp { get { return _m_piPlayerInfo; } }
        public NPPlayerResourceComponent rescourceComp { get { return _m_prPlayerRescource; } }
        public PlayerSpecialItemComponent specialItemComp { get { return _m_specialItemComponent; } }
        public PlayerBagComponent bagComp { get { return _m_pbcPlayerBag; } }
        public PlayerTitleComponent titleComp { get { return _m_ptPlayerTitleComponent; } }
        public PlayerSkinComponent skinComp { get { return _m_ptPlayerSkinComponent; } }
        public NPPlayerIconComponent iconComp { get { return _m_piPlayerIconComponent; } }
        public NPPlayerIconBgkComponent iconBgkComp { get { return _m_pibPlayerIconBgkComponent; } }
        public NPPlayerBubbleComponent bubbleComp { get { return _m_pbPlayerBubbleComponent; } }
        public NPPlayerBuffComponent playerBuffComp { get { return _m_pbcPlayerBuffComponent; } }

        public NPPlayerTutorialComponent tutorialComp { get { return _m_tcTutorialComponent; } }
        
        public NPPlayerPropertyMgr playerPropertyMgr { get { return _m_pmPropertyMgr; } }

        public GPlayerMailComponent mailComp { get { return _m_pmcMailComponent; } }

        // 火星基地组件
        public MarsComponent marsComp { get { return _m_marsComp; } }

        /// <summary>
        /// 聊天组件
        /// </summary>
        public NPPlayerChatComponent chatComp { get { return _m_chatComponent; } }

        /// <summary>
        /// 骑士组件
        /// </summary>
        public PlayerHeroComponent heroComponent { get { return _m_heroComponent; } }
        
        /// <summary>
        /// 排行榜组件
        /// </summary>
        public NPRankCommonComponent rankCommonComp { get { return _m_rankCommonComp; } }

        /// <summary>
        /// CD组件
        /// </summary>
        public NPPlayerLazyCdComponent lazyCdComp { get { return _m_lazyCdComp; } }

        /// <summary>
        /// 任务组件
        /// </summary>
        public PlayerQuestComponent questComp { get { return _m_questComp; } }

        /// <summary>
        /// 日常周常任务组件
        /// </summary>
        public PlayerDailyQuestComponent dailyQuestComp { get { return _m_dailyQuestComp; } }
        
        /// <summary>
        /// 玩家计数组件
        /// </summary>
        public NPPlayerRecordComponent recordComp { get { return _m_recordComp; } }

        /// <summary>
        /// 玩家行为计数组件
        /// </summary>
        public PlayerEventRecordComponent eventRecordComp { get { return _m_eventRecordComp; } }

        /// <summary>
        /// 功能解锁组件
        /// </summary>
        public PlayerFuncUnlockComponent funcUnlockComp { get { return _m_funcUnlockComp; } }

        /// <summary>
        /// 玩家身上所有加成的总管理对象
        /// </summary>
        public PlayerUnionBonusMgr playerBonusMgr { get { return _m_playerBonusMgr; } }
        
        /// <summary>
        /// 固定时间恢复的CD组件
        /// </summary>
        public NPPlayerFixedCDComponent fixedCdComp { get { return _m_fixedCdComp; } }

        /// <summary>
        /// 好友组件
        /// </summary>
        public PlayerFriendsComponent friendsComp { get { return _m_friendsComp; } }
        
        /// <summary>
        /// 成就组件
        /// </summary>
        public PlayerAchieveComponent achieveComp { get { return _m_achieveComp; } }

        /// <summary>
        /// 通用目标奖励组件
        /// </summary>
        public CommonTargetRewardComponent commonTargetRewardComp { get { return _m_commonTargetRewardComp; } }
        
        /// <summary>
        /// 商店组件
        /// </summary>
        public NPPlayerShopComponent shopComp { get { return _m_shopComp; } }
        
        /// <summary>
        /// 离线奖励组件
        /// </summary>
        public PlayerOfflineRewardComponent offlineRewardComp { get => _m_offlineRewardComp; }
        
        /// <summary>
        /// 情人组件
        /// </summary>
        public GConsortComponent consortComp { get { return _m_ConsortComp; } }

        public PlayerChapterComponent chapterComp { get { return _m_chapterComp; } }
        
        /// <summary>
        /// 游历组件
        /// </summary>
        public GPlayerTravelComponent travelComp { get { return _m_travelComp; } }
        /// <summary>
        /// 周卡
        /// </summary>
        public GWeekCardComponent weekCardComp { get { return _m_weekCardComp; } }
        
        /// <summary>
        /// 冲榜礼包组件
        /// </summary>
        public RankGiftPackComponent rankGiftPackComp { get { return _m_rankGiftPackComp; } }

        /// <summary>
        /// 通用活动组件
        /// </summary>
        public CommonActivityComponent commonActivityComp { get { return _m_commonActivityComponent; } }
        
        /// <summary>
        /// 活动热更配表组件
        /// </summary>
        public CommonActivityHotRefComponent commonActivityHotRefComp { get { return _m_commonActivityHotRefComponent; } }

        /// <summary>
        /// 大学组件
        /// </summary>
        public GPlayerDinnerComponent dinnerComp { get { return _m_dinnerComp; } }
        
        /// <summary>
        /// 跑马灯组件
        /// </summary>
        public MarqueeComponent marqueeComp { get { return _m_marqueeComp; } }

        public PlayerCuteActorComponent cuteActorComp { get { return _m_cuteActorComp; } }
        /// <summary>
        /// 问卷调查组件
        /// </summary>
        public PlayerQuestionnaireComponent questionnaireComp { get { return _m_questionnaireComp; } }
        /// <summary>
        /// 联盟组件
        /// </summary>
        public GuildComponent guildComp { get { return _m_guildComp; } }
        /// <summary>
        /// 建筑组件
        /// </summary>
        public PlayerBuildingComponent buildingComp { get { return _m_buildingComp; } }
        /// <summary>
        /// 藏品组件
        /// </summary>
        public PlayerEquipComponent equipComp { get { return _m_equipComp; } }
        /// <summary>
        /// 子嗣组件
        /// </summary>
        public PlayerChildComponent childComp { get { return _m_childComp; } }
        
        /// <summary>
        /// 抽卡组件
        /// </summary>
        public GachaComponent gachaComp { get { return _m_gachaComp; } } 
        
        /// <summary>
        /// 招募组件
        /// </summary>
        public RecruitComponent recruitComp { get { return _m_recruitComp; } }
        /// <summary>
        /// 竞技场组件
        /// </summary>
        public ArenaComponent arenaComp { get { return _m_arenaComp; } }
        /// <summary>
        /// 贸易站组件
        /// </summary>
        public StationComponent stationComp { get { return _m_stationComp; } }

        /// <summary>
        /// 阶段目标组件
        /// </summary>
        public StageGoalComponent stageGoalComp { get { return _m_stageGoalComp; } }
        
        /// <summary>
        /// 通用刷新组件
        /// </summary>
        public PlayerCommonRefreshComponent commonRefreshComp { get { return _m_commonRefreshComp; } }
        
        /// <summary>
        /// 经营事件组件
        /// </summary>
        public PlayerAnecdoteComponent anecdoteComp { get { return _m_anecdoteComp; } }
        
        public TowerComponent towerComp { get {return _m_towerComp;} }
        
        /// <summary>
        /// 午间副本组件
        /// </summary>
        public MiddayDungeonComponent middayDungeonComp { get { return _m_middayDungeonComp; } }
        
        public CommonItemCountComponent commonItemCountComp { get { return _m_commonItemCountComp; } }
        
        public EveningDungeonComponent eveningDungeonComp { get { return _m_eveningDungeonComp; } }
        
        public SevenDayLoginComponent sevenDayLoginComp { get { return _m_sevenDayLoginComp; } }
        
        public SevenDayGoalsComponent sevenDayGoalsComp { get { return _m_sevenDayGoalsComp; } }
        
        public EarningGoalComponent earningGoalComp { get { return _m_earningGoalComp; } }
        /// <summary>
        /// 倒计时事件组件
        /// </summary>
        public CountdownEventComponent countdownEventComp { get { return _m_countdownEventComp; } }
        /// <summary>
        /// 系统任务组件
        /// </summary>
        public PlayerSystemQuestComponent systemQuestComp { get { return _m_systemQuestComp; } }
        /// <summary>
        /// 礼包组件
        /// </summary>
        public GiftPackComponent giftPackComp { get { return _m_giftPackComp; } }
        /// <summary>
        /// 支付订单组件
        /// </summary>
        public PayOrderComponent payOrderComp { get { return _m_payOrderComp; } }
        
        public ConsortChatComponent consortChatComp { get { return _m_consortChatComp; } }
        
        public PlayerInnComponent innComp { get { return _m_innComp; } }
        
        public PlayerMuseumComponent museumComp { get { return _m_museumComp; } }

        public TreasureHuntComponent treasureHuntComponent { get { return _m_treasureHuntComp; } }

        
        /// <summary>
        /// 杰出者大厅组件
        /// </summary>
        public GraveComponent graveComp { get { return _m_graveComp; } }
        
        public GuildDungeonComponent guildDungeonComp { get { return _m_guildDungeonComp; } }
        
        public GuildMarsHelpComponent guildMarsHelpComp { get { return _m_guildMarsHelpComp; } }

        /// <summary>
        /// 联盟协作任务组件
        /// </summary>
        public GuildCooperateComponent guildCooperateComp { get { return _m_guildCooperateComp; } }
        /// <summary>
        /// 充值返利组件
        /// </summary>
        public RechargeRebateComponent rechargeRebateComp { get { return _m_rechargeRebateComp; } }
        /// <summary>
        /// 红点组件
        /// </summary>
        public RedDotComponent redDotComp { get { return _m_redDotComp; } }
        /// <summary>
        /// 权益卡组件
        /// </summary>
        public PrivilegeCardComponent privilegeCardComp { get { return _m_privilegeCardComp; } }
        /// <summary>
        /// 玩家权限组件
        /// </summary>
        public PlayerPermissionsComponent playerPermissionsComp { get { return _m_playerPermissionsComp; } }
		/// <summary>
        /// 推送礼包组件
        /// </summary>
        public PushGiftComponent pushGiftComp { get { return _m_pushGiftComp; } }
        /// <summary>
        /// 联盟宝箱组件
        /// </summary>
        public GuildBoxComponent guildBoxComp { get { return _m_guildBoxComp; } }
        /// <summary>
        /// 基金组件
        /// </summary>
        public PlayerFundComponent fundComp { get { return _m_fundComp; } }

        public PlayerForeverAddComponent foreverAddComponent
        {
            get { return _m_foreverAddComponent; }
        }
        /// <summary>
        /// 限时秒杀组件
        /// </summary>
        public RushExchangeComponent rushExchangeComp { get { return _m_rushExchangeComp; } }
        
        /// <summary>
        /// 房间皮肤组件
        /// </summary>
        public PlayerRoomSkinComponent roomSkinComp { get { return _m_roomSkinComp; } }

        /// <summary>
        /// 情人收集组件
        /// </summary>
        public LoverCollectComponent loverCollectComp { get { return _m_loverCollectComp; } }

        //设置玩家名字
        public void setPlayerName(string _name)
        {
            if (playerInfo != null && playerInfo.setPlayerName(_name))
                WinMsg.SendMsg(WinMsgType.ON_PLAYER_NAME_CHANGE, playerInfo);
        }

        /*************
         * 获取玩家对应的值
         **/
        public long getValue(ENPPlayerValueType _valueType)
        {
            switch (_valueType)
            {
                case ENPPlayerValueType.LVL://玩家等级
                    return playerInfo.getValue(ENPPlayerParam.LEVEL);
                case ENPPlayerValueType.VIP_LVL://玩家VIP等级
                    return playerInfo.getValue(ENPPlayerParam.VIP_LVL);
                case ENPPlayerValueType.RMB_NUM://玩家充值RMB值
                    return 0;
                case ENPPlayerValueType.FRIEND_COUNT://好友数量
                    return friendsComp.getFriendsListCount();
                case ENPPlayerValueType.HERO_NUM://大臣数量
                    return heroComponent.getTotalHeroCount();
                case ENPPlayerValueType.HERO_IN_BUILDING_NUM://入驻建筑的大臣数量
                    return heroComponent.getHeroInBuildingNum();
                case ENPPlayerValueType.EARNINGS://国力
                    return specialItemComp.goldData.earnings;
                case ENPPlayerValueType.CHILD_SUM://未成年子嗣数量
                    return childComp.getChildTotalCount();
                case ENPPlayerValueType.TOTAL_HERO_POWER://总大臣实力
                    return heroComponent.totalPower;
                case ENPPlayerValueType.TOTAL_HERO_TALENT://总大臣资质
                    return heroComponent.getTotalHeroTalent();
                case ENPPlayerValueType.CONSORT_NUM://妃子数量
                    return consortComp.getConsortCount();
                case ENPPlayerValueType.DONE_MAIN_QUEST_COUNT://主线任务完成数（如果多次完成也算1次）
                    return questComp.getMainQuestDoneCount();
                case ENPPlayerValueType.STAGE_GOAL_DONE_STEP:
                    return stageGoalComp.stageRefObj.step - 1;
                case ENPPlayerValueType.NAMED_CHILD_NUM://已命名未成年子嗣数量
                     return childComp.getChildNamedCount();
                case ENPPlayerValueType.TOTAL_HERO_LEVEL://总大臣等级
                    return heroComponent.getTotalHeroLevel();
                case ENPPlayerValueType.TOTAL_CONSORT_INTIMACY://总情人亲密度
                    return consortComp.allIntimacyNum;
                case ENPPlayerValueType.TOTAL_CONSORT_CHARM://总情人加护力
                    return consortComp.allCharmNum;
                case ENPPlayerValueType.CHAPTER_POINT://章节位置 chapterId*1000+point
                    return chapterComp.curChapterId * 1000 + chapterComp.curPointId;
                case ENPPlayerValueType.BUILDING_NUM://建筑数量
                    return buildingComp.getOwnBuildingNum();
                case ENPPlayerValueType.TOWER_PASSED_CHAPTER://爬塔通关章节
                    return towerComp.towerPassedChapter;
                case ENPPlayerValueType.TOWER_PASSED_LVL://爬塔所在关卡
                    return towerComp.towerCurTotalLevel;
                case ENPPlayerValueType.INN_POPULARITY:
                    return innComp.popularity;
                case ENPPlayerValueType.INN_LEVEL:
                    return innComp.levelRef?.level ?? 0;
                case ENPPlayerValueType.INN_HAD_UNLOCK_DISH_NUM:
                    return innComp.getUnlockedDishCount();
                case ENPPlayerValueType.INN_STATION_TOTAL_LEVEL:
                    return innComp.getStationTotalLevel();
                case ENPPlayerValueType.INN_HAD_RECEIVE_GUEST_COUNT:
                {
                    // 如果当前表现层运行了，要以表现层的 Tick 数据为准
                    GNodeInnMain innNode = QueueMgr.instance.findLastNode(typeof(GNodeInnMain)) as GNodeInnMain;
                    if (innNode is { viewMgr: { isLoaded: true } })
                        return innNode.viewMgr.getHadSettleGuestsCount();
                    
                    // 否则从数据组件中直接计算得出
                    return innComp.receiveInfo.getHadSettleGuestsCount();
                }
                case ENPPlayerValueType.TREASURE_HUNT_STATION_LEVEL:
                    return treasureHuntComponent?.stationInfo?.stationLevel ?? 0;
                case ENPPlayerValueType.TREASURE_HUNT_HAD_GAIN_ORE_TYPE_COUNT:
                    return treasureHuntComponent?.gotOreInfoList.Count ?? 0;
                case ENPPlayerValueType.TREASURE_HUNT_HAD_GAIN_TREASURE_TYPE_COUNT:
                    return treasureHuntComponent?.gotTreasureInfoList.Count ?? 0;
                case ENPPlayerValueType.TREASURE_HUNT_HAD_COLLECT_COMPOSITE_COUNT:
                    return treasureHuntComponent?.getCollectedCompositeCount() ?? 0;
                case ENPPlayerValueType.MUSEUM_ITEM_NUM:
                    return museumComp?.getObtainedItemCount() ?? 0;
                case ENPPlayerValueType.CHAPTER_ID:
                    return chapterComp.curChapterId;
                case ENPPlayerValueType.GUILD_LEVEL:
                    return guildComp?.guildInfo != null ? guildComp.guildInfo.level : 0;
                case ENPPlayerValueType.UNMARRY_ADULT_SUM://未婚成年子嗣数量
                    return childComp.getUnmarriedChildCount();
                case ENPPlayerValueType.MARS_EXPLORER_NUM://火星探索次数
                    return marsComp.exploreSubComponent.exploreNumTotal;
                case ENPPlayerValueType.TOWER_ACTIVE_RESEARCH_COUNT://爬塔已激活研究数量
                    return towerComp.getActiveResearchCount();
                case ENPPlayerValueType.MARS_BUILDING_EQUIP_LVL_SUM://火星基地建筑装备等级总和
                    return marsComp.buildingSubComponent.getBuildingEquipLevelSum();
                case ENPPlayerValueType.MARS_BUILDING_LVL_SUM:
                    return marsComp.buildingSubComponent.getBuildingLvlSum();
                case ENPPlayerValueType.INN_MEDAL_LEVEL:
                    return innComp.medalLevelRef?.level ?? 0;
                case ENPPlayerValueType.TOTAL_GAIN_GOLD_COUNT:
                    return specialItemComp.goldData.getTotalGainGoldCount();
                default:
                    return 0;
            }
        }

        private long _getFixValue(long _value)
        {
            return _value > 0 ? _value : 0;
        }

        //判断是否玩家自己
        public bool isPlayerSelf(long _uiId)
        {
            return playerInfo == null ? false : playerInfo.CID == _uiId;
        }

        /** 计算可以领取的奖励 */
        public NPGVersionUpRewardRefObj calNewVersionReward(long _newClientVersion)
        {
            long lastTakeVersion = playerInfo[ENPPlayerParam.LAST_TAKE_VERSION];
            int localClientVersion = ClientVersionSetting.instance.ClientVersionInfo.clientVersion;

            //已经领取过最新的版本更新奖励，返回False
            if (_newClientVersion == lastTakeVersion || localClientVersion <= lastTakeVersion)
                return null;

            //客户端版本小于上一次领取的此时直接返回Fasle
            if (localClientVersion != _newClientVersion)
                return null;

            NPGVersionUpRewardRefObj tempRewardInfo = null;
            NPGVersionUpRewardRefObj tarRewradInfo = null;

            //计算详细Reward
            List<NPGVersionUpRewardRefObj> versionUpRewardList = GRefdataCoreMgr.instance.versionUpRewardMap.refList;

            //这里不排序，在VersionUpReward数据初始化的时候就排过序了
            for (int i = 0; i < versionUpRewardList.Count; i++)
            {
                tempRewardInfo = versionUpRewardList[i];

                if (tempRewardInfo == null)
                    continue;

                //最新版本号和当前版本号同除不相等
                if (_newClientVersion / tempRewardInfo.div_num != lastTakeVersion / tempRewardInfo.div_num)
                {
                    //如果当前没有目标版号奖励对象，那就直接替换
                    if (tarRewradInfo == null)
                    {
                        tarRewradInfo = tempRewardInfo;
                        continue;
                    }

                    //如果有，要比对DivID大小，取大的
                    if (tarRewradInfo.div_num < tempRewardInfo.div_num)
                    {
                        tarRewradInfo = tempRewardInfo;
                        continue;
                    }
                }
            }

            return tarRewradInfo;
        }

        /// <summary>
        /// 根据类型获取子属性加成管理对象
        /// </summary>
        /// <param name="_tag"></param>
        /// <returns></returns>
        public _AUnionBonusMgr getUnionBonusMgrByTag(EUnionBonusMgrTag _tag)
        {
            return _m_playerBonusMgr?.findMgrByTag(_tag);
        }

        /** 进入阵容设置视图 */
        public void enterLineup()
        {
            
        }

        /**************
         * 释放相关资源
         **/
        public void discard()
        {
            _m_mgrCompMgr.discard();
            HotfixStaticFunc.discardHotfixDataComponent();
        }
    }

    public class CalculatePlayerPropertiesTask : _IALBaseMonoTask
    {
        public void deal()
        {
            if (null == NPPlayer.instance)
            {
                ALMonoTaskMgr.instance.addNextFrameTask(this);
                return;
            }

            NPPlayer.instance.playerPropertyMgr.calculateChgProperties();
            NPPlayer.instance.marsComp.marsPropertyMgr.calculateChgProperties();

            ALMonoTaskMgr.instance.addNextFrameTask(this);
        }
    }
}
