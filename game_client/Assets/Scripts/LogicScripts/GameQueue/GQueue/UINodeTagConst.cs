using System;
using ALPackage;

namespace GOE
{
    public class UINodeTagConst
    {
        public const string C_Empty = "empty";

        public const string C_Login_AllWay = "l_allway";
        public const string C_Login_Start_Game = "l_start_game";
        public const string C_Login_Account = "l_account";
        public const string C_Login_Loading = "l_loading";
        public const string C_Login_SN = "l_sn_code";
        public const string C_ADD_Login_Server_List = "l_server_list";
        public const string C_GS_Login_Loading = "l_gs_loading";    //登录具体US的登录流程处理
        public const string C_GS_Process_Loading = "l_gs_process_loading";    //登录具体US的数据交互流程的处理
        public const string C_Login_Maintain_Notice = "l_maintain_notice";      //根据服务器不同状态的提示窗节点
        public const string C_Login_Queue = "l_queue";      //玩家排队的队伍信息

        public const string C_Main_Space = "space";
        public const string C_Main_BagNode = "bag";
        public const string C_Main_MailNode = "mail";


        public const string C_Main_ChatNode = "chat";
        public const string C_Main_PlayerInfoNode = "player_info";
        public const string C_Main_OtherPlayerInfoNode = "other_player_info";
        public const string C_Main_Chat_SHARE_HERO_DETAIL = "chat_share_hero_detail";
        public const string C_Main_Chat_SHARE_CHILD_DETAIL = "chat_share_child_detail";
        public const string C_Main_Chat_SHARE_CONSORT_DETAIL = "chat_share_consort_detail";

        public const string C_Main_Chat_SHARE_CONSORT = "chat_share_consort";

        public const string C_ADD_Bag_UseItemNode = "bag_use_item";
        public const string C_ADD_Bag_ConvertItemNode = "bag_convert_item";
        public const string C_ADD_Bag_UseShowRealGainItemNode = "bag_use_item_show_real_gain_item";
        public const string C_ADD_Bag_Item_Use_Select = "bag_use_item_select";
        public const string C_ADD_Bag_Item_Use_Percent = "bag_use_item_percent";

        public const string C_GET_ITEM = "get_item";//获取物品弹窗
        public const string C_ITEM_PERCENT_DETAIL = "item_percent_detail";//玩家物品概率详情界面

        public const string C_ADD_Build_Info = "build_info";
        public const string C_ADD_Build_Repair = "build_repair";
        public const string C_ADD_Build_Upgrade = "build_upgrade";
        public const string C_ADD_Build_Item_Tip = "build_item_tip";
        public const string C_ADD_Build_Condition_Tip = "build_condition_tip";

        public const string C_ADD_SPACE_HINT_MAP = "space_hint_map";

        public const string C_Mail_Detail = "mail_detail";

        public const string C_Sys_CheatNode = "cheat";



        public const string C_OP_PlayerInfo_Tab = "player_info_tab";
        public const string C_OP_Shop_Tab = "shop_tab";

        public const string C_ADD_COMMON_PLAYER_BUSINESS_CARD = "common_player_business_card";

        public const string C_BATTLE_MAIN = "win_battle_main";
        public const string C_MISSION_RESULT = "win_mission_result";


        public const string C_COMMON_ACCESS_WAYS = "win_common_access_ways";
        public const string C_COMMON_ONCE_COMBINE = "win_common_once_combine";
        public const string C_COMMON_SIMPLE_COMBINE = "win_common_simple_combine";

        public const string C_MAIN_SPACE_MAP_TELEPORT_CONFIRM = "win_space_map_teleport_confirm";
        public const string C_ADD_SMALL_MAP_FITTER = "small_map_fitter";

        public const string C_MAIN_SPACE_DIALOG = "win_space_npc_dialog";

        public const string C_MAIN_DAILY = "achieve_daily_node";//日常界面
        public const string C_MAIN_DAILY_ACHIEVE = "prefab_achieve_page";//日常界面成就页签
        public const string C_MAIN_DAILY_QUEST = "win_daily_quest_main";//日常界面任务页签
        public const string C_MAIN_DAILY_CHECK = "win_daily_check_page";//日常界面签到页签
        public const string C_ADD_DAILY_CHECK = "win_daily_check_main";//日常界面签到弹窗

        public const string C_COMMON_ACHIEVE = "common_achieve";//通用成就界面
        public const string C_ADD_ACHIEVE_STEP = "achieve_step_node";


        public const string C_ADD_TREASURE_MAIN = "treasure_map_main";

        public const string C_ADD_MACHINE_MAIN = "machine_main";
        public const string C_ADD_COMMON_SHARE = "common_share";
        public const string C_MES_DEALER = "mes_dealer";

        public const string C_EFFECT_CUSTOM_MAIN_UI = "C_EFFECT_CUSTOM_MAIN_UI";//效果加载自定义main窗口
        public const string C_EFFECT_CUSTOM_ADD_UI = "C_EFFECT_CUSTOM_ADD_UI";//效果加载自定义add窗口

        public const string C_ACCOUNT_BIND = "account_bind";//绑定账号窗口
        public const string C_ACCOUNT_SWITCH = "account_switch";//切换账号窗口
        public const string C_SETTING_MAIN = "setting_main";//设置窗口
        public const string C_SETTING_SWITCH_LANGUAGE = "language_switch";//切换语言窗口
        public const string C_SETTING_AUDIO = "audio_setting";//音效设置窗口
        public const string C_SETTING_EFFECT = "effect_setting";//效果设置窗口
        public const string C_SETTING_LOCAL_PUSH = "local_push_setting";//本地推送设置窗口

        public const string C_ADD_RULE_LIST = "rule_list";//系统规则列表
        public const string C_ADD_RULE_MAIN = "rule_main";//系统规则

        public const string C_BATTLE_PAUSE = "battle_pause";//战斗暂停页面


        public const string C_CONSORT_INTIMACY_PROCESS_REWARD_ADD = "consort_intimacy_process_reward_add";//情人亲密历程奖励新增
        public const string C_CONSORT_ENTER = "consort_enter";//情人入口
        public const string C_CONSORT_MAIN = "consort_main";//家人主页面
        public const string C_CONSORT_GET = "consort_get";//家人获得界面
        public const string C_LOCK_CONSORT_DETAIL = "lock_consort_detail";//未解锁妃子详情
        public const string C_UNLOCK_CONSORT_DETAIL = "unlock_consort_detail";//已解锁妃子详情
        public const string C_UNLOCK_CONSORT_DETAIL_HALO_PAGE = "unlock_consort_detail_halo_page";//已解锁妃子详情_星辉页签
        public const string C_UNLOCK_CONSORT_DETAIL_FETTER_PAGE = "unlock_consort_detail_fetter_page";//已解锁妃子详情_羁绊页签
        public const string C_UNLOCK_CONSORT_DETAIL_BUSINESS_PAGE = "unlock_consort_detail_business_page";//已解锁妃子详情_经营页签
        public const string C_UNLOCK_CONSORT_DETAIL_BLESS_PAGE = "unlock_consort_detail_bless_page";//已解锁妃子详情_加护页签
        public const string C_UNLOCK_CONSORT_DETAIL_INTERACTION_PAGE_STORY = "unlock_consort_detail_interaction_page_story";//已解锁妃子详情_互动页签_送礼界面
        public const string C_UNLOCK_CONSORT_DETAIL_INTERACTION_PAGE_TRAVEL = "unlock_consort_detail_interaction_page_travel";//已解锁妃子详情_互动页签_培养界面
        public const string C_UNLOCK_CONSORT_DETAIL_INTERACTION_PAGE_SEND_GIFT = "unlock_consort_detail_interaction_page_send_gift";//已解锁妃子详情_互动页签_送礼界面

        
        
        public const string C_CONSORT_INTIMACY_PAGE = "consort_intimacy_page";//情人亲密关系列表
        public const string C_CONSORT_DETAIL = "consort_detail";//情人详情界面
        public const string C_CONSORT_INTIMACY_PROGRESS = "consort_intimacy_progress";//亲密历程界面
        public const string C_CONSORT_POWER_RELATIONS = "consort_power_relations";//势力关系弹窗
        public const string C_CONSORT_SKIN_LITS = "consort_skin_list";//情人皮肤列表
        public const string C_CONSORT_CG_MAIN = "consort_cg_main";//情人CG主界面
        public const string C_CONSORT_CG_DETAIL = "consort_cg_detail";//情人CG详情页面
        public const string C_CONSORT_CG_GET = "consort_cg_get";//情人CG获取页面
        public const string C_CONSORT_CG_SHARE_CONFIRM = "consort_cg_share_confirm";//情人CG分享确认页面
        public const string C_CONSORT_SKIN_MAIN = "consort_skin_main";//妃子皮肤主界面
        public const string C_CONSORT_RANDOM_INVITE_REWARD = "consort_random_invite_reward";//随机邀约奖励窗口
        public const string C_CONSORT_AKEY_INVITE_REWARD = "consort_akey_invite_reward";//一键邀约奖励窗口
        public const string C_CONSORT_LEVELUP_FETTER = "consort_levelup_fetter";//妃子升级羁绊弹窗
        public const string C_CONSORT_HALO_EFFECT_DETAIL = "consort_halo_effect_detail";//妃子星辉效果总览
        public const string C_CONSORT_HALO_LEVELUP = "consort_halo_levelup";//妃子星辉升级弹窗
        public const string C_CONSORT_BUSINESS_SKILL_UNLOCK = "consort_business_skill_unlock";//妃子经营技能解锁弹窗
        public const string C_CONSORT_STORY_UNLOCK = "c_consort_story_unlock";//妃子故事解锁弹窗
        public const string C_UNLOCK_CONSORT_DETAIL_PROFILE_WND = "unlock_consort_detail_profile";//妃子解锁详情页面简介窗口
        public const string C_UNLOCK_CONSORT_DETAIL_STORY_WND = "unlock_consort_detail_story";//妃子解锁详情页面故事窗口
        public const string C_CHANGE_ENTRANCE_CONSORT = "change_entrance_consort";//切换入口妃子窗口
        public const string C_CONSORT_APPOINT_CALL_PROCESS = "consort_appoint_call_process";//妃子指定邀约流程窗口

        public const string C_HERO_BATTLE_ENTRY = "hero_battle_entry";//骑士挑战入口
        public const string C_HERO_INTRODUCTION = "hero_introduction";//骑士简介
        public const string C_HERO_TALENT_UPGRADE = "hero_talent_upgrade";//骑士资质技能升级弹窗
        public const string C_HERO_STEP_UPGRADE = "hero_step_upgrade";//骑士升阶弹窗
        public const string C_HERO_ATTR_DETAIL = "hero_attr_detail";//骑士属性详情弹窗
        public const string C_HERO_MAIN = "win_hero_main";//骑士列表界面
        public const string C_HERO_INFO = "hero_info";//骑士详情
        public const string C_HERO_INFO_BUSINESS_PAGE = "hero_info_business_page";//骑士经营页签
        public const string C_HERO_INFO_TALENT_PAGE = "hero_info_talent_page";//骑士资质页签
        public const string C_HERO_INFO_STAR_PAGE = "hero_info_star_page";//骑士觉醒页签
        public const string C_HERO_INFO_HALO_PAGE = "hero_info_halo_page";//骑士星辉页签
        public const string C_HERO_INFO_BLESS_PAGE = "hero_info_bless_page";//骑士加护页签

        public const string C_HERO_TRAIN = "hero_train";//骑士培养
        public const string C_HERO_SKIN = "hero_skin";//骑士皮肤
        public const string C_HERO_UNLOCK_INFO = "hero_unlock_info";//未解锁骑士信息
        public const string C_HERO_HALO = "hero_halo";//骑士光环
        public const string C_HERO_HALO_LOCK = "hero_halo_lock";//未解锁骑士光环
        public const string C_HERO_RECOMMEND_MAIN = "hero_recommend_main";//骑士推荐主界面
        public const string C_HERO_OTHER_INFO = "hero_other_info";//查看骑士弹窗
        public const string C_HERO_STEP_UPGRADE_SUC = "hero_step_upgrade_suc";//伙伴升阶成功弹窗
        public const string C_HERO_TALENT_VALUE_DETAIL = "hero_talent_value_detail";//伙伴资质详情
        public const string C_HERO_BUSINESS_SKILL_UNLOCK = "hero_business_skill_unlock";//伙伴经营技能解锁弹窗
        public const string C_HERO_STAR_SKILL_LEVEL_DETAIL = "hero_star_skill_level_detail";//伙伴觉醒技能等级详情弹窗
        public const string C_HERO_STAR_UPGRADE_CHECK = "hero_star_upgrade_check";//伙伴觉醒升级确认弹窗
        public const string C_HERO_HALO_OWN_INFO = "hero_halo_own_info";//伙伴光环已提升详情界面
        public const string C_HERO_SUIT_AND_POWER = "hero_suit_and_power";//伙伴套系及战力详情界面
        public const string C_HERO_EQUIP_INFO = "hero_equip_info";//伙伴佩戴的藏品信息界面
        public const string C_HERO_EQUIP_CHANGE = "hero_equip_change";//伙伴替换藏品界面
        public const string C_HERO_GET = "hero_get";//伙伴获得界面

        public const string C_PLAYER_SKIN_GET = "win_player_info_get_skin";//玩家皮肤获得界面
        
        public const string C_LEVY = "levy";//征收
        public const string C_RANK = "win_rank_details";//的排行榜界面


        public const string C_MAIN_QUEST_NODE = "quest_main_node";//任务主界面

        public const string C_DINNER_ENTER_MAIN = "dinner_enter_main";//宴会入口主界面
        public const string C_DINNER_COST_USE = "dinner_cost_use";//宴会使用消耗赴宴
        public const string C_DINNER_CREATE = "dinner_create";//宴会举办弹窗
        public const string C_DINNER_MAIN = "dinner_main";//宴会主界面
        public const string C_DINNER_LIST = "dinner_list";//宴会列表
        public const string C_DINNER_LOG = "dinner_log";//宴会历史记录页面
        public const string C_DINNER_GUEST_LIST = "dinner_cur_guest_list";//宴会当前赴宴信息
        public const string C_DINNER_INVITE = "dinner_invite";//宴会邀请弹窗
        public const string C_DINNER_RANK = "dinner_rank";//宴会排行榜
        public const string C_DINNER_DETAIL_LOG = "dinner_detail_log";//宴会详情日志

        public const string C_TOWER_MAIN = "tower_main";//爬塔主界面
        public const string C_TOWER_CHAPTER = "tower_chapter";//爬塔关卡界面
        public const string C_TOWER_CHALLENGE = "tower_challenge";//爬塔挑战界面
        public const string C_TOWER_BATTLE = "tower_battle";//爬塔战斗界面
        public const string C_TOWER_LOG = "tower_log";//爬塔日志界面
        public const string C_TOWER_BATTLE_SHOW = "tower_battle_show";//爬塔战斗开场界面
        public const string C_TOWER_BATTLE_PVP = "tower_battle_pvp";//爬塔战斗pvp界面
        public const string C_TOWER_BATTLE_PVE = "tower_battle_pve";//爬塔战斗pve界面
        public const string C_TOWER_CHAPTER_PVP = "tower_chapter_pvp";//爬塔PVP关卡界面
        public const string C_TOWER_RESEARCH = "tower_research";//爬塔研究界面

        public const string C_MIDDAY_DUNGEON_ENTER = "midday_dungeon_enter";//午间副本入口
        public const string C_MIDDAY_DUNGEON_BATTLE = "midday_dungeon_battle";//午间副本战斗
        public const string C_MIDDAY_DUNGEON_SELECT_HERO = "midday_dungeon_select_hero";//午间副本战斗选择大臣
        public const string C_MIDDAY_DUNGEON_RANK = "midday_dungeon_rank";//午间副本排行榜
        public const string C_MIDDAY_DUNGEON_BOX_RECORD = "midday_dungeon_box_record";//午间副本宝箱领取记录

        public const string C_DAILY_QUEST_ACTIVE_REWARD_PREVIEW = "daily_quest_active_reward_preview";//每日任务活跃积分奖励预览

        public const string C_ANECDOTE_MAIN = "anecdote_main";//政务主窗口
        public const string C_ANECDOTE_CD = "anecdote_cd";//政务倒计时窗口
        public const string C_ANECDOTE_ONE_KEY_REWARD = "anecdote_one_key_reward";//一键完成奖励展示窗口

        public const string C_MAIN_PLOT_DIALOG = "win_plot_dialog";//剧情对话窗口

        #region 游历

        public const string C_MAIN_TRAVEL = "travel_main";//游历主界面
        public const string C_MAIN_TRAVEL_CONSORT_LIST = "travel_consort_list";//游历妃子列表

        public const string C_TRAVEL_EVENT_COMMON_RESULT = "travel_event_common_result";//游历事件通用结果展示窗口

        public const string C_TRAVEL_INVITATION_EVENT_SELECT_CONSORT = "travel_invitation_event_select_consort";//游历邀请事件 - 选择妃子窗口
        public const string C_TRAVEL_INVITATION_EVENT_RESULT = "travel_invitation_event_result";//游历邀请事件 - 结果展示窗口

        public const string C_TRAVEL_GIFTED_EVENT_RESULT = "travel_gifted_event_result";//游历卷王事件 - 结果展示窗口

        public const string C_TRAVEL_ADD_POWER_DEAL_WND = "travel_add_power_deal_wnd";//游历增加实力事件 - 处理窗口
        public const string C_TRAVEL_ADD_POWER_RESULT = "travel_add_power_result";//游历增加实力事件 - 结果展示窗口

        public const string C_TRAVEL_GAMBLE_ANTE_DEAL_WND = "travel_gamble_ante_deal_wnd";//游历博彩事件 - 押注处理窗口
        public const string C_TRAVEL_GAMBLE_EVENT_SHOW_WND = "travel_gamble_event_show_wnd";//游历博彩事件 - 主展示窗口
        public const string C_TRAVEL_GAMBLE_EVENT_RESULT = "travel_gamble_event_result";//游历博彩事件 - 结果展示窗口

        public const string C_TRAVEL_RESULT_MEET_CONSORT_ADD_LIKE_TIP = "travel_result_meet_consort_add_like_tip";//游历结果 - 遇见妃子增加好感度的提示窗口
        public const string C_TRAVEL_CONSORT_EVENT_RESULT = "travel_consort_event_result";//游历妃子事件 - 结果展示窗口
        public const string C_TRAVEL_CONSORT_BAR_EVNENT_CHOOSE_CONSORT = "travel_consort_bar_event_choose_consort";//游历妃子酒馆事件 - 选择妃子窗口
        public const string C_TRAVEL_CONSORT_BAR_EVNENT_CHOOSE_COST = "travel_consort_bar_event_choose_cost";//游历妃子酒馆事件 - 选择消耗窗口

        public const string C_TRAVEL_AKEY_RESULT = "travel_akey_result";//游历一键游历结果展示窗口

        public const string C_TRAVEL_RANDOM_TRAVEL_PROCESS = "travel_random_travel_process";//游历随机游历流程窗口

        public const string C_TRAVEL_POS_INFO = "travel_pos_info";//游历地点信息窗口

        public const string C_TRAVEL_POS_UNLOCK = "travel_pos_unlock";//游历地点解锁弹窗
        public const string C_TRAVEL_DEAL_EVENT_BG = "travel_deal_event_bg";//游历处理事件背景窗口

        #endregion

        public const string C_ADD_LEVY_FOOD = "levy_food";//征收粮食
        public const string C_ADD_LEVY_SOLDIER = "levy_soldier";//征收士兵
        public const string C_ADD_LEVY_SILVER = "levy_silver";//征收银币

        public const string C_MINI_GAME_BASE = "mini_game_base";//小游戏界面
        public const string C_MINI_GAME_FIND_THINGS = "mini_game_find_things";//找东西小游戏
        public const string C_MINI_GAME_QTE_GAME = "mini_game_qte_game";//QTE小游戏

        public const string C_SIMPLE_COMIC = "win_simple_comic";//简易漫画
        public const string C_CREAT_PLAYER = "win_creat_player";//创角
        public const string C_AVATAR_STAGE_DISPLAY = "win_clothes_show";//玩家形象舞台展示
        public const string C_PRE_CREATE_PLAYER = "c_pre_create_player";//预创角node
        public const string C_NODE_CITY = "win_main_home";//主城node
        public const string C_NODE_MAIN = "Main";
        public const string C_NODE_ROOM = "win_main_room";//卧室node
        public const string C_NODE_SPACE_STATION = "win_main_space_station";//空间站窗口
        public const string C_NODE_MARS = "win_main_mars";//火星窗口

        public const string C_DIALOGUE_HISTORY = "win_dialogue_history";//对话历史回顾窗口

        public const string C_FUNC_DETAIL = "win_func_detail";//系统预告详情窗口

        public const string C_CLOTHES_SUIT_GET = "clothes_suit_get";//套装收集完成展示

        public const string C_FUNC_UNLOCK_TIP = "func_unlock_tip";//解锁提示
        public const string C_ANNOUNCEMENT_NODE = "game_announcement";//运营公告
        public const string C_MAIN_AVATAR_GACHA = "win_avatar_gacha_main";//抽卡主界面
        public const string C_MAIN_AVATARGACHA_DRAW = "avatargacha_draw";//抽卡表现

        public const string C_CLOTHES_NX1 = "win_clothes_nx1";//服装n选1
        public const string C_QUESTIONNAIRE = "win_questionnaire";//问卷调查
        public const string C_BUILDING = "Building";
        public const string C_BUILDING_MAIN = "win_city_business_building_main";
        public const string C_BUILDING_BUILD_SUC = "win_city_business_building_build_success";
        public const string C_BUILDING_EFFECT = "building_effect";
        public const string C_BUSINESS_BUILDING_RND = "business_building_rnd";
        public const string C_BUSINESS_BUILDING_PRODUCT_UNLOCK = "business_building_product_unlock";//经营建筑产品解锁
        public const string C_BUSINESS_BUILDING_UPGRADE = "business_building_upgrade";//经营建筑升级
        public const string C_BUSINESS_BUILDING_UPGRADE_SUC = "business_building_upgrade_suc";//经营建筑升级成功


        public const string C_EQUIP_MAIN = "equip_main";//藏品主界面
        public const string C_EQUIP_RECYCLE = "equip_recycle";//藏品分解界面
        public const string C_EQUIP_RECYCLE_CONFIRM = "equip_recycle_confirm";//藏品分解确认界面
        public const string C_EQUIP_DETAIL = "equip_detail";//藏品详情界面
        public const string C_EQUIP_PREVIEW = "equip_preview";//藏品图鉴界面
        public const string C_EQUIP_GET = "equip_get";//藏品获得界面

        public const string C_SHOW_OFFITEM_GET = "c_show_offitem_get";//获得装扮类道具界面（头像、头像框、气泡框、称号）

        
        public const string C_SUMMON_MAIN = "summon_main";//召唤系统主页面
        public const string C_SUMMON_REWARD_PROBABILITY = "summon_reward_probability";//召唤系统奖励概率窗口
        public const string C_SUMMON_RESULT = "summon_result";//召唤结果展示窗口
        public const string C_SUMMON_CUMULATIVE_NUM_REWARD_DRAW = "summon_cumulative_num_reward_draw";//召唤累计次数奖励展示窗口
        public const string C_SUMMON_SHOW_PROCESS = "summon_show_process";//召唤表现流程窗口

        public const string C_RECRUIT_MAIN = "recruit_main";//招募主界面
        public const string C_RECRUIT_SHOP = "recruit_shop";//招募商店
        public const string C_RECRUIT_CONSORT = "recruit_consort";//招募妃子窗口
        public const string C_RECRUIT_HERO = "recruit_hero";//招募大臣窗口

        public const string C_RANK_GIFT_PACK = "recruit_hero";//招募大臣窗口

        
        public const string C_ARENA_MAIN = "arena_main";//竞技场主界面
        public const string C_ARENA_BATTLE = "arena_battle";//竞技场谈判主界面
        public const string C_ARENA_BATTLE_REPORT = "arena_battle_report";//竞技场战报界面
        public const string C_ARENA_ADD_RANDOM_COUNT = "arena_add_random_count";//竞技场增加随机谈判次数
        public const string C_ARENA_STATION_INFO = "arena_station_info";//竞技场贸易站信息
        public const string C_ARENA_STATION_UPGRADE_SUC = "arena_station_upgrade_suc";//竞技场贸易站升级成功
        public const string C_ARENA_CONVENTENT_SETTING = "arena_convenient_setting";//竞技场便捷设置弹窗
        public const string C_ARENA_RANK = "arena_rank";//竞技场排行榜弹窗
        public const string C_ARENA_SELECT_HERO = "arena_select_hero";//竞技场战斗选择伙伴弹窗
        public const string C_ARENA_BATTLE_PREPARE = "arena_battle_prepare";//竞技场战斗准备界面
        public const string C_ARENA_BATTLE_SELECT_BUFF = "arena_battle_select_buff";//竞技场战斗选择增益弹窗
        public const string C_ARENA_BATTLE_ONE_KEY = "arena_battle_one_key";//竞技场一键谈判弹窗
        public const string C_ARENA_BATTLE_SHOW = "arena_battle_show";//竞技场战斗展示界面
        public const string C_ARENA_BATTLE_ROUND_WIN = "arena_battle_round_win";//竞技场战斗回合胜利界面
        public const string C_ARENA_BATTLE_ROUND_LOST = "arena_battle_round_lost";//竞技场战斗回合失败界面
        public const string C_ARENA_BATTLE_ROUND_REWARD = "arena_battle_round_reward";//竞技场战斗连胜奖励界面
        public const string C_ARENA_BATTLE_FINAL_RESULT = "arena_battle_final_result";//竞技场战斗结算界面
        public const string C_ARENA_BATTLE_FINAL_HERO_ADD_POWER = "arena_battle_final_hero_add_power";//竞技场战斗结算伙伴提升实力界面

        public const string C_STAGE_GOAL = "win_stageTask_main";//阶段目标
        public const string C_STAGE_GOAL_BIG_STEP_UNLOCK = "win_stageTask_big_step_unlock";//阶段目标大阶段解锁
        public const string C_STAGE_GOAL_BIG_STEP_COMPLETE = "win_stageTask_big_step_complete";//阶段目标大阶段完成
        public const string C_STAGE_GOAL_COMPLETE = "stage_goal_complete";//阶段目标主任务完成
        public const string C_STAGE_GOAL_UNLOCK = "stage_goal_unlock";//阶段目标主任务解锁
        public const string C_STAGE_GOAL_TASK_INFO = "stage_goal_task_info";//阶段目标任务详情弹窗
        public const string C_STAGE_GOAL_PEAK_DETAIL = "stage_goal_peak_detail";//阶段目标到达时代之巅详情弹窗
        public const string C_STAGE_GOAL_PEAK_PAGE = "stage_goal_peak_page";//阶段目标时代之巅页面
        public const string C_STAGE_GOAL_TASK_PAGE = "stage_goal_task_page";//阶段目标任务页面
        public const string C_STAGE_GOAL_OVERVIEW_PAGE = "stage_goal_overview_page";//阶段目标预览页面
        public const string C_COMMON_TARGET_REWARD = "win_common_target_reward";//通用目标奖励

        public const string C_LIMIT_ACTIVITY_MAIN = "limit_activity_main";//限时活动界面
        public const string C_RANK_RUSH_ACCESS = "rank_rush_acceess";//限时冲榜获取途径弹窗
        public const string C_RANK_RUSH_DETAIL = "rank_rush_detail";//限时冲榜详情界面
        public const string C_GUILD_RANK_RUSH_DETAIL = "guild_rank_rush_detail";//联盟冲榜详情界面
        public const string C_RANK_RUSH_MULTIPLE_DETAIL = "rank_rush_multiple_detail";//限时冲榜多个详情界面
        public const string C_GUILD_RANK_RUSH_MEMBER_SCORE_DETAIL = "guild_rank_rush_member_score_detail";//联盟冲榜成员分数详情界面

        public const string C_BAG_ITEM_USE_HERO = "bag_item_use_hero";//背包使用道具-选择伙伴窗口
        public const string C_BAG_ITEM_USE_CONSORT = "bag_item_use_consort";//背包使用道具-选择情人窗口
        public const string C_BAG_ITEM_HERO_CONSORT_USE_RESULT = "bag_item_hero_consort_use_result";//伙伴家人使用道具完成界面

        public const string C_CHAT_SELECT_SHARE_HERO = "chat_select_share_hero";//聊天选择伙伴分享界面
        public const string C_CHAT_SELECT_SHARE_CHILD = "chat_select_share_child";//聊天选择子嗣分享界面
        public const string C_CHAT_SELECT_SHARE_CONSORT_CG = "chat_select_share_consort_cg";//聊天选择家人CG分享界面
        public const string C_CHAT_SELECT_SHARE_CONSORT_CG_DETAIL = "chat_select_share_consort_cg_detail";//聊天选择家人CG分享详情界面
        public const string C_CHAT_SHARE_CHILD_TEAM_UP = "chat_share_child_team_up";//子嗣分享组队弹窗

        public const string C_ADULT_ENGAGE_REQUEST_SEND = "adult_engage_request_send";//联姻请求发送窗口
        public const string C_ADULT_ENGAGE_SELECT = "adult_engage_select";//联姻请求选择窗口
        public const string C_ADULT_MARRIED_DETAIL_TOOL_TIP = "adult_married_detail_tooltip";//联姻详情tooltip
        public const string C_ADULT_TEAM_UP_REQUIREMENT_SETTING = "adult_team_up_requirement_setting";//子嗣组队要求设置窗口
        

        public const string C_EVENING_DUNGEON_GAME_MAIN = "evening_dungeon_game_main";//晚间副本游戏主界面
        public const string C_EVENING_DUNGEON_KILLED_RESULT = "evening_dungeon_killed_result";//晚间副本击杀结果界面
        public const string C_EVENING_DUNGEON_ATTACK_RESULT = "evening_dungeon_attack_result";//晚间副本攻击结算界面
        public const string C_EVENING_DUNGEON_SELECT_HERO = "evening_dungeon_select_hero";//晚间副本选择伙伴界面
        public const string C_EVENING_DUNGEON_RANK_AND_REWARD_DETAIL = "evening_dungeon_rank_and_reward_detail";//晚间副本排行和奖励详情窗口
        public const string C_EVENING_DUNGEON_RANK_DETAIL = "evening_dungeon_rank_detail";//晚间副本排行详情窗口
        public const string C_EVENING_DUNGEON_FINAL_ATTACK_RECORD = "evening_dungeon_final_attack_record";//尾刀记录窗口
        public const string C_EVENING_DUNGEON_ENTRANCE = "evening_dungeon_entrance";//晚间副本入口窗口
        public const string C_DUNGEON_ENTRANCE = "dungeon_entrance";//午间和晚间副本入口窗口
        public const string C_DUNGEON_RANK = "dungeon_rank";//午间和晚间副本排行榜窗口

        public const string C_SEVEN_DAY_LOGIN = "server_day_login";//七天登录
        public const string C_ACTIVITY_CONSUME_SHOP = "activity_consume_shop";//通用活动消耗商店
        public const string C_ACTIVITY_EXCHANGE_SHOP = "activity_exchange_shop";//通用活动兑换商店
        public const string C_ACTIVITY_GIFT_PACK = "activity_gift_pack";//通用活动礼包界面
        public const string C_ACTIVITY_STEP_REWARD = "activity_step_reward";//通用活动阶段奖励界面
        public const string C_ACTIVITY_STEP_REWARD_STEP = "activity_step_reward_step";//通用活动阶段奖励步骤界面

        public const string C_SEVEN_DAY_GOALS = "seven_day_goals";//七天目标

        public const string C_EARNING_GOAL_MAIN = "earning_goal_main";//赚速目标主界面
        public const string C_EARNING_GOAL_GLOBAL_REWARD = "earning_goal_global_reward";//赚速目标全服奖励
        public const string C_EARNING_GOAL_SELF_REWARD = "earning_goal_self_reward";//赚速目标个人奖励

        public const string C_GIFT_PACK_BATCH_BUY = "gift_pack_batch_buy";//礼包批量购买商品弹窗
        public const string C_OPEN_SET_PREFAB_WND = "win_player_prefab_set"; //打开设置预制体窗口

        public const string C_ACTIVITY_CENTER_WND = "activity_center"; //活动中心界面
        public const string C_STEP_TASK_RUSH_RANK_ACTIVITY_MAIN = "C_STEP_TASK_RUSH_RANK_ACTIVITY_MAIN"; //阶段任务冲榜活动主界面

        public const string C_COUNTDOWN_EVENT_WND = "countdown_event"; //倒计时事件界面

        public const string C_CONSORT_CHAT_MAIN = "consort_chat_main";//情人聊天主界面
        public const string C_CONSORT_CHAT_MAIN_CHAT_PAGE = "consort_chat_main_chat_page";//情人聊天主界面_聊天页签
        public const string C_CONSORT_CHAT_MAIN_MOMENTS_PAGE = "consort_chat_main_moments_page";//情人聊天主界面_朋友圈页签
        public const string C_CONSORT_CHAT_MAIN_CONSORT_LIST_PAGE = "consort_chat_main_consort_list_page";//情人聊天主界面_情人列表页签
        public const string C_CONSORT_CHAT_MSG_DETAIL = "consort_chat_detail";//情人聊天消息界面
        public const string C_CONSORT_CHAT_IMAGE_DETAIL = "consort_chat_image_detail";//情人聊天图片详情界面
        public const string C_CONSORT_CHAT_MOMENT_INTERATION = "consort_chat_moment_interation";//情人朋友圈互动消息
        public const string C_CONSORT_CHAT_MOMENT_DETAIL = "consort_chat_moment_detail";//情人朋友圈详情

        public const string C_INN_MAIN = "inn_main";//旅店主界面
        public const string C_INN_LEVEL_UP_SUCCESS = "inn_level_up_success";//旅店升级成功界面
        public const string C_INN_CASH_REGISTER_UPGRADE_TIP = "inn_cash_register_upgrade_tip";//旅店收银机升级提示界面
        public const string C_INN_STATION_LIST = "inn_station_list";//旅店设施列表
        public const string C_INN_MENU = "inn_menu";//旅店菜单
        public const string C_INN_STATION_BUILD = "inn_station_build";//旅店设施建造界面
        public const string C_INN_STATION_BUILD_SUCCESS = "inn_station_build_sucess";//旅店设施建造成功界面
        public const string C_INN_STATION_LEVEL_UP = "inn_station_level_up";//旅店设施升级界面
        public const string C_INN_GET_CASH_REGISTER_REWARD = "inn_get_cash_register_reward";//获得收银机奖励
        public const string C_INN_GUEST_LIST = "inn_guest_list";//旅店客人列表
        public const string C_INN_NORMAL_GUEST_INFO = "inn_normal_guest_info";//旅店普通客人信息
        public const string C_INN_SPECIAL_GUEST_INFO = "inn_special_guest_info";//旅店特殊客人信息
        public const string C_INN_STATION_LEVEL_UP_SUCCESS = "inn_station_level_up_success";//旅店设施升级成功界面
        public const string C_INN_DISH_LEVEL_UP = "inn_dish_level_up";//旅店菜品升级界面
        public const string C_INN_DISH_UNLOCK = "inn_dish_unlock";//旅店菜品解锁界面
        public const string C_INN_DISH_UNLOCK_SUCCESS = "inn_dish_unlock_success";//旅店菜品解锁成功界面
        public const string C_INN_LEVEL_DETAIL = "inn_level_detail";//旅店等级详情界面
        public const string C_INN_LEVEL_INFO = "inn_level_info";//旅店等级信息界面
        public const string C_INN_MEDAL_INFO = "inn_medal_info";//旅店勋章信息界面
        public const string C_INN_MEDAL_UPGRADE_SUCCESS = "inn_medal_upgrade_success";//旅店勋章升级成功界面
        public const string C_INN_SPECIAL_GUEST_SERVED = "inn_special_guest_served";//旅店特殊客人服务成功界面
        public const string C_INN_MUSEUM_GIFT_TIP = "inn_museum_gift_tip";//旅店博物馆礼品提示界面
        public const string C_INN_NEW_GUEST_UNLOCK_SUCCESS = "inn_new_guest_unlock_success";//旅店新客人解锁成功界面
        public const string C_INN_SPECIAL_GUEST_SERVE_CHOICE = "inn_special_guest_serve_choice";//旅店特殊客人服务选择界面

        public const string C_MUSEUM_ITEM_LIST = "museum_item_list";//博物馆藏品列表
        public const string C_MUSEUM_ITEM_INFO = "museum_item_info";//博物馆藏品信息
        public const string C_MUSEUM_ITEM_STORY = "museum_item_story";//博物馆藏品故事
        public const string C_MUSEUM_ITEM_GET = "museum_item_get";//博物馆藏品获得
        public const string C_HIRE_MAIN = "hire_main";//招聘体验界面
        public const string C_SYSTEM_QUEST_DETAIL = "system_quest_detail";//系统任务详情界面
        
        public const string C_TREASURE_HUNT_MAIN = "treasure_hunt_main";//太空寻宝 - 主界面
        public const string C_TREASURE_HUNT_STATION_LEVEL_DETAIL = "treasure_hunt_station_level_detail";//太空寻宝 - 空间站等级详情界面
        public const string C_TREASURE_HUNT_STATION_UPGRADE = "treasure_hunt_station_upgrade";//太空寻宝 - 太空舱升级界面
        public const string C_TREASURE_HUNT_LAB = "treasure_hunt_lab";//太空寻宝 - 实验室界面
        public const string C_TREASURE_HUNT_LAB_TREASURE_PUT_IN = "treasure_hunt_lab_treasure_put_in";//太空寻宝 - 实验室奇物放入界面
        public const string C_TREASURE_HUNT_SELECT_LAB = "treasure_hunt_select_lab";//太空寻宝 - 选择实验室界面
        public const string C_TREASURE_HUNT_SPECIMEN_ROOM = "treasure_hunt_specimen_room";//太空寻宝 - 标本室界面
        public const string C_TREASURE_HUNT_SPECIMEN_CONVERT = "treasure_hunt_specimen_convert";//太空寻宝 - 标本转换界面
        public const string C_TREASURE_HUNT_COLLECT_ACHIEVE = "treasure_hunt_collect_achieve";//太空寻宝 - 收集成就界面
        public const string C_TREASURE_HUNT_CATALOG_MAIN = "treasure_hunt_catalog_main";//太空寻宝 - 图鉴主界面
        public const string C_TREASURE_HUNT_ORE_CATALOG = "treasure_hunt_ore_catalog";//太空寻宝 - 矿石图鉴界面
        public const string C_TREASURE_HUNT_TREASURE_CATALOG = "treasure_hunt_treasure_catalog";//太空寻宝 - 奇物图鉴界面
        public const string C_TREASURE_HUNT_ORE_DETAIL_INFO = "treasure_hunt_ore_detail_info";//太空寻宝 - 矿石详情界面
        public const string C_TREASURE_HUNT_TREASURE_DETAIL_INFO = "treasure_hunt_treasure_detail_info";//太空寻宝 - 奇物详情界面
        public const string C_TREASURE_HUNT_COMPOSITE_CATALOG = "treasure_hunt_composite_catalog";//太空寻宝 - 组合图鉴界面
        public const string C_TREASURE_HUNT_COMPOSITE_CATALOG_DETAIL = "treasure_hunt_composite_catalog_detail";//太空寻宝 - 组合图鉴详情界面
        public const string C_TREASURE_HUNT_GAME_MAIN = "treasure_hunt_game_main";//太空寻宝 - 游戏主界面
        public const string C_TREASURE_HUNT_SELECT_ENERGY = "treasure_hunt_select_energy";//太空寻宝 - 选择能源界面
        public const string C_TREASURE_HUNT_SELECT_AREA = "treasure_hunt_select_area";//太空寻宝 - 选择区域界面
        public const string C_TREASURE_HUNT_AREA_ORE_TREASURE_DETAIL = "treasure_hunt_area_ore_treasure_detail";//太空寻宝 - 区域矿石奇物详情界面
        public const string C_TREASURE_HUNT_CAPTURE_ORE_RESULT = "treasure_hunt_capture_ore_result";//太空寻宝 - 捕获矿石结果界面
        public const string C_TREASURE_HUNT_CAPTURE_TREASURE_RESULT = "treasure_hunt_capture_treasure_result";//太空寻宝 - 捕获奇物结果界面
        public const string C_TREASURE_HUNT_AKEY_CAPTURE_RESULT = "treasure_hunt_akey_capture_result";//太空寻宝 - 一键捕获结果界面
        public const string C_TREASURE_HUNT_CAPTURE_HARVEST_RESULT = "treasure_hunt_capture_harvest_result";//太空寻宝 - 打捞收获物结果界面
        public const string C_TREASURE_HUNT_ORE_MASS_RANK = "treasure_hunt_ore_mass_rank";//太空寻宝 - 矿石质量排行界面
        public const string C_TREASURE_HUNT_GAME_PLAY_QUIT_CHECK = "treasure_hunt_game_play_quit_check";//太空寻宝 - 游戏中退出确认界面
        public const string C_TREASURE_HUNT_GAME_PLAY_RESUME_COUNT_DOWN = "treasure_hunt_game_play_resume_count_down";//太空寻宝 - 游戏中暂停恢复倒计时界面
        public const string C_TREASURE_HUNT_GAME_PLAY_END_EFFECT = "treasure_hunt_game_play_end_effect";//太空寻宝 - 游戏中结束特效界面
        public const string C_CASH_GIFT_PACK_MAIN = "cash_gift_pack_main";//现金礼包主界面
        public const string C_CASH_GIFT_PACK_DAILY_SCORE_SHOP = "cash_gift_pack_daily_score_shop";//现金礼包每日积分商店界面
        public const string C_GEM_GIFT_PACK_MAIN = "cash_gift_pack_main";//钻石礼包-主界面
        public const string C_PAY_BY_VOUCHER_CONFIRM = "pay_by_voucher_confirm";//使用代金券购买确认弹窗
        public const string C_GRAVE_MAIN = "grave_main";//杰出者大厅主界面
        public const string C_GRAVE_PLAYER_DETAIL = "grave_player_detail";//杰出者详情
        public const string C_GRAVE_CONGRATS = "grave_congrats";//杰出者庆祝
        public const string C_GRAVE_BLESS_GET = "grave_bless_get";//杰出者祝福
        public const string C_GRAVE_BLESS_DETAIL = "grave_bless_detail";//祝福详情界面
        public const string C_GRAVE_HONOR_LOG = "grave_honor_log";//荣誉列表界面
        public const string C_GRAVE_NEW_PROMINENT = "grave_new_prominent";//新晋者列表界面
        public const string C_BAG_ITEM_USE_TIME = "bag_item_use_time";// 背包道具时间减少窗口
        public const string C_ITEM_AUTO_CONVERSION = "item_auto_conversion";//物品自动转换

        public const string C_MARS_POWER_DETAIL = "mars_power_detail";//火星实力详情界面
        public const string C_MARS_MISSION_PREVIEW = "mars_mission_preview";//火星任务预览界面
        public const string C_MARS_GO_TO = "mars_go_to";//前往火星界面
        public const string C_MARS_GO_TO_START_CONFIRM = "mars_go_to_start_confirm";//确认前往火星界面
        public const string C_MARS_GO_TO_LOG_DETAIL = "mars_go_to_log_detail";//前往火星日志详情界面
        public const string C_MARS_ARRIVE_NEW_STAGE_CONFIRM = "mars_arrive_new_stage_confirm";//前往火星到达新节点确认界面
        public const string C_MARS_LANDING_SELECT = "mars_landing_select";//选择火星着陆地点界面
        public const string C_MARS_EXPLORE = "mars_explore";//火星探索界面
        public const string C_MARS_EXPLORE_TEAM_EDIT = "mars_explore_team_edit";//火星探索队伍编辑界面
        public const string C_MARS_EXPLORE_TEAM_HERO_EDIT = "mars_explore_team_hero_edit";//火星探索队伍英雄编辑界面
        public const string C_MARS_EXPLORE_TEAM_REPAIR = "mars_explore_team_repair";//火星探索队伍修理界面
        public const string C_MARS_EXPLORE_LEVEL_DETAIL = "mars_explore_level_detail";//火星探索等级详情界面
        public const string C_MARS_EXPLORE_COLLECTING_DETAIL = "mars_explore_collecting_detail";//火星探索采集中详情界面
        public const string C_MARS_EXPLORE_UPGRADE = "mars_explore_upgrade";//火星探索升级界面
        public const string C_MARS_EXPLORE_PVP_LOG = "mars_explore_pvp_log";//火星探索PVP日志界面
        public const string C_MARS_EXPLORE_PVP_DETAIL = "mars_explore_pvp_detail";//火星探索PVP详情界面
        public const string C_MARS_EXPLORE_BATTLE_INFO = "mars_explore_battle_info";//火星探索战斗信息界面
        public const string C_MARS_EXPLORE_MINE_INFO = "mars_explore_mine_info";//火星探索采集点信息界面
        public const string C_MARS_EXPLORE_MINE_DETAIL_INFO = "mars_explore_mine_detail_info";//火星探索采集点详情界面
        public const string C_MARS_EXPLORE_TEAM_UNLOCK_DESC = "mars_explore_team_unlock_desc";//火星探索队伍解锁说明界面
        public const string C_MARS_EXPLORE_TEAM_SELECT = "mars_explore_team_select";//火星探索队伍选择界面
        public const string C_MARS_EXPLORE_BOSS_INFO = "mars_explore_boss_info";//火星探索Boss信息界面
        public const string C_MARS_EXPLORE_MINE_SHARE = "mars_explore_mine_share";//火星探索采集点分享界面
        public const string C_MARS_BUILDING_COMPLETE_NOW_CHECK = "mars_building_complete_now_check";//火星建筑立即完成确认界面
        public const string C_MARS_BUILD_QUEUE_BUY = "mars_build_queue_buy";//火星建筑队列购买界面
        public const string C_MARS_CASH_GIFT_PACK_MAIN = "mars_cash_gift_pack_main";//火星现金礼包主界面
        public const string C_MARS_BUILD_QUEUE_DETAIL = "mars_build_queue_detail";//火星建筑队列详情界面
        public const string C_MARS_EXPLORE_POS_ITEM_SELECT = "mars_explore_pos_item_select";//火星探索位置事件选择界面

        #region 联盟副本

        public const string C_GUIlD_DUNGEON_MAIN = "guild_dungeon_main";//联盟副本主界面
        public const string C_GUILD_DUNGEON_MAP = "guild_dungeon_map";//联盟副本地图界面
        public const string C_GUIlD_DUNGEON_START = "guild_dungeon_start";//联盟副本开始界面
        public const string C_GUIlD_DUNGEON_UPGRADE = "guild_dungeon_upgrade";//联盟副本升级界面
        public const string C_GUIlD_DUNGEON_RANK = "guild_dungeon_rank";//联盟副本排行榜界面
        public const string C_GUIlD_DUNGEON_BATTLE = "guild_dungeon_battle";//联盟副本战斗界面
        public const string C_GUIlD_DUNGEON_LOG = "guild_dungeon_log";//联盟副本日志界面
        public const string C_GUIlD_DUNGEON_AUTO_OPEN = "guild_dungeon_auto_open";//联盟副本自动开启界面
        public const string C_GUIlD_DUNGEON_SELECT_HERO = "guild_dungeon_select_hero";//联盟副本选择大臣界面

        #endregion

        #region 联盟协作

        public const string C_GUIlD_COOPERATE_MAIN = "guild_cooperate_main";//联盟协作主界面
        public const string C_GUIlD_COOPERATE_ATTR_POINT_DETAIL = "guild_cooperate_attr_point_detail";//联盟协作属性据点详情界面
        public const string C_GUIlD_COOPERATE_LOG = "guild_cooperate_log";//联盟协作日志界面
        public const string C_GUIlD_COOPERATE_REWARD_POINT_DETAIL = "guild_cooperate_reward_point_detail";//联盟协作奖励据点详情界面
        public const string C_GUIlD_COOPERATE_RANK = "guild_cooperate_rank";//联盟协作排行界面
        public const string C_GUIlD_COOPERATE_MAP = "guild_cooperate_map";//联盟协作地图预览界面
        public const string C_GUIlD_COOPERATE_ATTR_POINT_FINISH = "guild_cooperate_attr_point_finish";//联盟协作属性据点完成界面

        #endregion
        public const string C_GUIlD_MARS_HELP = "guild_mars_help";//联盟互助


        #region 火星居民

        public const string C_MARS_RESIDENT = "C_MARS_RESIDENT";//火星居民界面
        public const string C_MARS_INTELLIGENT_CONTROL = "C_MARS_INTELLIGENT_CONTROL";//火星基地智能控制界面
        public const string C_MARS_INTELLIGENT_CONTROL_DETAIL = "C_MARS_INTELLIGENT_CONTROL_DETAIL";//火星基地智能控制详情界面
        public const string C_MARS_POPULAR_WILL = "C_MARS_POPULAR_WILL";//火星基地民意界面
        public const string C_MARS_POPULAR_WILL_REWARD_HELP_DEAL = "C_MARS_POPULAR_WILL_REWARD_HELP_DEAL";//火星基地民意奖励帮助处理界面
        public const string C_MARS_POPULAR_WILL_CHOICE_HELP_DEAL = "C_MARS_POPULAR_WILL_CHOICE_HELP_DEAL";//火星基地民意选择帮助处理界面
        public const string C_MARS_POPULAR_WILL_CHOICE_HELP_DEAL_RESULT = "C_MARS_POPULAR_WILL_CHOICE_HELP_DEAL_RESULT";//火星基地民意选择帮助处理结果界面
        public const string C_MARS_TIME_SPEEDUP = "C_MARS_TIME_SPEEDUP";//火星时间加速界面
        public const string C_MARS_TIME_COMPLETE_NOW_CONFIRM = "C_MARS_TIME_COMPLETE_NOW_CONFIRM";//火星时间立即完成确认界面
        public const string C_MARS_EVENT_DETAIL = "C_MARS_EVENT_DETAIL";//火星基地事件详情界面
        public const string C_MARS_RESIDENT_REPLENISH = "C_MARS_RESIDENT_REPLENISH";//火星居民补充界面
        public const string C_MARS_RESIDENT_REPLENISH_RESULT = "C_MARS_RESIDENT_REPLENISH_RESULT";//火星居民补充结果界面
        public const string C_MARS_POPULAR_GET_TIP = "C_MARS_POPULAR_GET_TIP";//火星基地民意获取提示界面

        #endregion

        #region 火星科技

        public const string C_MARS_TECHNOLOGY_TREE = "C_MARS_TECHNOLOGY_TREE";//火星科技树界面
        public const string C_MARS_TECHNOLOGY_ADD_OVERVIEW = "C_MARS_TECHNOLOGY_ADD_OVERVIEW";//火星科技加成总览界面
        public const string C_MARS_TECHNOLOGY_DETAIL = "C_MARS_TECHNOLOGY_DETAIL";//火星科技详情界面
        public const string C_MARS_TECHNOLOGY_LVL_PROPERTY = "C_MARS_TECHNOLOGY_LVL_PROPERTY";//火星科技等级属性界面

        #endregion
        
        #region VIP

        public const string C_VIP_MAIN = "vip_main";//vip主界面
        public const string C_VIP_LEVEL_PREVIEW = "vip_level_review";//vip等级预览界面
        public const string C_VIP_LEVEL_UPGRADE = "vip_level_upgrade";//vip等级升级界面

        #endregion

        #region 首充礼包

        public const string C_FIRST_RECHARGE_MAIN = "first_recharge_main";//首充礼包主界面

        #endregion

        #region 推送礼包

        public const string C_PUSH_GIFT_PACK_POP = "push_gift_pack_pop";//推送礼包弹出界面
        public const string C_PUSH_GIFT_PACK_LIST = "push_gift_pack_list";//推送礼包列表界面

        #endregion

        #region 商店好评

        public const string C_STORE_REVIEWS_MAIN = "store_reviews_main";//商店评价主界面
        public const string C_STORE_REVIEWS_ROAST = "store_reviews_roast";//商店评价吐槽界面

        #endregion

        #region 限时兑换

        public const string C_RUSH_EXCHANGE_MAIN = "rush_exchange_main";//限时兑换主界面
        

        #endregion
        
        public const string C_ACTIVITY_MERGE_SHOW_PUSH_NOTICE = "activity_merge_show_push_notice";//活动合并展示推送弹窗
        
        public const string C_FUND_TASK_DETAIL = "fund_task_detail";//基金任务详情界面
        public const string C_FUND_ACTIVATE = "fund_activate";//基金激活界面
        public const string C_FUND_PREVIEW = "fund_preview";//基金预览界面
        public const string C_IMPROVE_WAY = "improve_way";//提升途径界面
        
        public const string C_LOVER_COLLECT_SELECT = "lover_collect_select";//情人收集选择界面
        public const string C_LOVER_COLLECT_RESCUE = "lover_collect_rescue";//情人收集营救界面

        public const string C_COMMON_TOOL_TIP_SERVER_LIST = "server_list_tool_tip";//跨服服务器列表详情tooltip
        public const string C_PLAYER_LVL_UP_SUC = "player_level_up_suc";//玩家升级成功界面
    }

    /// <summary>
    /// 排行榜相关
    /// </summary>
    public class UINodeTagConst_Rank
    {
        public const string C_MAIN_RANK_FIXED_LIST_NODE = "rank_fixed_list_node";
        public const string C_MAIN_RANK_FIXED_NODE = "rank_fixed_node";
        public const string C_MAIN_RANK_FIXED_ADDTION_NODE = "rank_fixed_addition_node";
        public const string C_MAIN_GUILD_RANK_FIXED_NODE = "guild_rank_fixed_node";
        public const string C_MAIN_GUILD_RANK_FIXED_ADDTION_NODE = "guild_rank_fixed_addition_node";
    }

    /// <summary>
    /// 好友相关
    /// </summary>
    public class UINodeTagConst_Friends
    {
        public const string C_PAGE_FRIEND_LIST = "page_friend_list";
        public const string C_MAIN_FRIENDS_NODE = "win_friends_main";
        public const string C_ADD_ADD_FRIENDS_NODE = "win_add_friend_main";
        public const string C_ADD_REQUEST_FRIENDS_NODE = "win_apply_friend_main";
        public const string C_ADD_FRIEND_BTNS_TIP = "prefab_friend_more";
        public const string C_ADD_FRIEND_SELECT_NODE = "win_select_friend_main";
        public const string C_ADD_FRIEND_VISIT_NODE = "win_friend_visit_main";

    }

    /// <summary>
    /// 商店相关
    /// </summary>
    public class UINodeTagConst_Shop
    {
        public const string C_MAIN_SHOP_NODE = "win_shop_main";
        public const string C_MAIN_SHOP_NO_TAB_NODE = "win_shop__no_tab_main";
        public const string C_ADD_SHOP_BUY_ITEM_NODE = "shop_buy_item";

    }

    /// <summary>
    /// 子嗣相关
    /// </summary>
    public class UINodeTagConst_Child
    {
        public const string C_MAIN_SCHOOL_NODE = "win_school_main";
        public const string C_MAIN_CHILD_NODE = "win_child_main";
        public const string C_CHILD_NAMING = "win_child_naming";
        public const string C_CHILD_ONE_KEY_EDUCATION_PLUS = "win_pop_child_auto_educate";
        public const string C_CHILD_GET = "win_child_get";
    }

    public class UINodeTagConst_College
    {
        public const string C_MAIN_COLLEGE_NODE = "win_college_main";
    }

    public class UINodeTagConst_Adult
    {
        public const string C_MAIN_ADULT_NODE = "win_adult_main";
    }

    public class UINodeTagConst_PlayerInfo
    {
        public const string C_MAIN_PLAYERINFO_NODE = "win_player_info";
        public const string C_MAIN_PLAYERINFO_DRESS_NODE = "win_player_info_dress";
        public const string C_MAIN_PLAYERINFO_TITLE_NODE = "win_player_info_title";
        public const string C_MAIN_PLAYERINFO_SKIN_NODE = "win_player_info_skin";
        public const string C_MAIN_PLAYERINFO_SKIN_UNLOCK_NODE = "win_player_info_skin_unlock";
        public const string C_MAIN_PLAYERINFO_SKIN_UPGRADE_NODE = "win_player_info_skin_upgrade";
        public const string C_MAIN_PLAYERINFO_COMBO_TITLE_ITEM_DETAIL_NODE = "win_player_info_combo_title_item_detail";
        
        public const string C_ADD_PLAYERINFO_RENAME_NODE = "win_player_rename";
        public const string C_ADD_PLAYERINFO_LV_PREVIEW_NODE = "win_player_info_privilege";
        public const string C_ADD_PLAYER_EARNINGS_DETAIL_NODE = "player_earnings_detail";//赚速详情界面
        public const string C_ADD_PLAYER_HERO_GAIN = "player_hero_gain";//升级活动伙伴界面
        public const string C_PLAYER_ROOM_SKIN = "player_room_skin";//玩家卧室皮肤界面
        public const string C_ADD_PLAYER_REPORT = "win_player_report";//举报弹窗
    }
    /// <summary>
    /// 定义退出控制对象
    /// </summary>
    public class NodeESC_Const
    {
        public const int C_QUEUE_ESC_CONTROLLER_NOTICE = 1;
        //开启全部遮罩的屏蔽
        public const int C_QUEUE_ESC_ALL_INPUT_MASK = 2;

        public const int C_QUEUE_ESC_LOADING = 3;
        //客户端热更资源提示弹窗屏蔽
        public const int C_QUEUE_ESC_RES_HOTFIX_UPDATE = 4;
        //低帧率提示弹窗屏蔽
        public const int C_QUEUE_ESC_LOW_FRAME_RATE = 5;
        //招聘体验的屏蔽
        public const int C_QUEUE_ESC_HIRE_MAIN = 6;
        //创角选择形象的屏蔽
        public const int C_QUEUE_ESC_CREATE_PLAYER_PREFAB = 7;
        //GM命令支付弹窗屏蔽
        public const int C_QUEUE_ESC_GM_PAY = 8;
    }

    /// <summary>
    /// 关卡窗口
    /// </summary>
    public class UINodeTagConst_Chapter
    {
        public const string C_CHAPTER_MAP = "win_chaptermap";//关卡地图界面
        public const string C_CHAPTER = "win_chapter_main";//关卡主界面
        public const string C_CHAPTER_BOSS = "win_chapter_boss";//关卡boss战窗口
        public const string C_CHAPTER_AUTO_SETTING = "win_chapter_auto_setting";//关卡自动设置窗口
        public const string C_CHAPTER_BOSS_INSPIRE = "win_chapter_boss_inspire";//关卡boss战鼓舞界面
        public const string C_CHAPTER_STAGE = "win_chapter_stage";//关卡阶段窗口
        public const string C_CHAPTER_STORY_LIST = "win_story_review_main";//关卡故事列表窗口
        public const string C_CHAPTER_STORY_DETAIL = "win_story_review_detail";//关卡故事详情窗口
        public const string C_CHAPTER_AUTO_FORWARD_REWARD = "win_chapter_auto_forward_reward";//关卡故事详情窗口
        public const string C_CHAPTER_SKIP_TOGGLE_DIALOG = "win_chapter_skip_toggle_dialog";//关卡跳过对话确认窗口

    }

    /// <summary>
    /// 通用事件窗口
    /// </summary>
    public class UINodeTagConst_CommonEvent
    {
        public const string C_COMMON_AWARD_EVENT_NODE = "GGUIWndChapterAwardEvent";//关卡奖励事件窗口
        public const string C_COMMON_CHOICE_EVENT_NODE = "GGUIWndChapterChoiceEvent";//关卡选择事件窗口
        public const string C_COMMON_EVENT_RESULT_NODE = "win_event_completed";//通用事件结果窗口
        public const string C_COMMON_FITTING_EVENT_NODE = "GGUIWndCommonSimpleFittingEvent";//试穿事件窗口
        public const string C_COMMON_AVATAR_SCORE_EVENT_NODE = "GGUIWndCommonAvatarScoreEvent";//avatar评分事件窗口
        public const string C_COMMON_AVATAR_SCORE_EVENT_RESULT_NODE = "GGUIWndCommonAvatarScoreEventResult";//avatar评分事件结果窗口
    }

    /// <summary>
    /// 签到窗口
    /// </summary>
    public class UINodeTagConst_DailyCheck
    {
        public const string C_MAIN_DAILY_CHECK_NODE = "GGUIWndDailyCheckMain";//签到界面

        public const string C_ADD_DAILY_CHECK_GET_REWARD_NODE = "GGUIWndDailyCheckGetReward";//签到奖励

    }

    public class UINodeTagConst_Market
    {
        public const string C_MAIN_MARKET = "market_main";//集市
        public const string C_ADD_MARKET_LV_UP = "market_lv_up";//集市升级
        public const string C_ADD_MARKET_SHOP = "market_shop";//集市店铺

    }

    public class UINodeTagConst_Week
    {
        public const string C_NOTICE_WEEK_CARD = "win_week_card_main_notice";
    }

    /// <summary>
    /// 联盟相关
    /// </summary>
    public class UINodeTagConst_Guild
    {
        public const string C_GUILD_APPLY = "guild_apply";//联盟申请界面
        public const string C_GUILD_CREATE = "guild_create";//联盟创建界面
        public const string C_GUILD_LEVEL_PREVIEW = "guild_level_preview";//联盟等级预览界面
        public const string C_GUILD_MEMBER_LIST = "guild_member_list";//联盟成员列表界面
        public const string C_GUILD_MEMBER_SELECT = "guild_member_select";//联盟成员选择界面
        public const string C_GUILD_NOTICE = "guild_notice";//联盟通知界面
        public const string C_GUILD_MAIN = "guild_main";//联盟主界面
        public const string C_GUILD_SELECT_FLAG = "guild_select_flag";//联盟旗帜选择界面
        public const string C_GUILD_CHANGE_NAME = "guild_change_name";//联盟修改名称界面
        public const string C_GUILD_APPLY_CONDITION = "guild_apply_condition";//联盟申请添加修改界面
        public const string C_GUILD_OTHERS_APPLY_LIST = "guild_others_apply_list";//联盟申请列表界面
        public const string C_GUILD_APPOINT = "guild_appoint";//联盟成员任命管理界面
        public const string C_GUILD_INFO = "guild_info";//联盟信息界面
        public const string C_GUILD_CONSTRUCT = "guild_construct";//联盟建设界面
        public const string C_GUILD_RANK = "guild_rank";//联盟排行界面
        public const string C_GUILD_OTHER_GUILD_INFO = "other_guild_info";//其他联盟信息界面
        
        public const string C_GUILD_ENTRUST = "guild_entrust";//联盟委托界面
        public const string C_GUILD_ENTRUST_RECORD = "guild_entrust_record";//联盟委托记录
        
        public const string C_GUILD_DISPATCH_HERO = "guild_dispatch_hero";//联盟派遣英雄界面
        public const string C_GUILD_LOG = "guild_log";//联盟日志界面
        public const string C_GUILD_GIFT = "guild_gift";//联盟礼物界面
        public const string C_GUILD_BOX = "guild_box";//联盟宝箱
        public const string C_GUILD_BOX_SCORE = "guild_box_score";//联盟宝箱积分详情
        public const string C_GUILD_BOX_ACTIVE_BOX_DETAIL = "guild_box_active_box_detail";//联盟活跃宝箱奖励详情
    }

    /// <summary>
    /// TreasureHunt相关
    /// </summary>
    public class UINodeTagConst_TreasureHunt
    {
        public const string C_TREASURE_HUNT_GAME_PLAY_END_EFFECT = "treasure_hunt_game_play_end_effect";// 太空寻宝游戏结束效果窗口
    }

    /// <summary>
    /// Mars相关
    /// </summary>
    public class UINodeTagConst_Mars
    {
        public const string C_MARS_BUILDING_TIME_BUFF_EXPLAIN = "mars_building_time_buff_explain";// 火星建筑时间增益说明窗口
        public const string C_MARS_BUILDING_UPGRADE = "mars_building_upgrade";// 火星建筑升级窗口
        public const string C_MARS_BUILDING_ENERGY_YIELD_DETAIL = "mars_building_energy_yield_detail";// 火星建筑能量产出详情窗口
        public const string C_MARS_BUILDING_ENERGY_DETAIL = "mars_building_energy_detail";// 火星能源建筑详情窗口
        public const string C_MARS_BUILDING_BUILD = "mars_building_build";// 火星建筑建造窗口
        public const string C_MARS_BUILDING_CONSTRUCTING = "mars_building_constructing";// 火星建筑建造中窗口
        public const string C_MARS_BUILDING_HOME_INFO = "mars_building_home_info";// 火星主基地信息窗口
        public const string C_MARS_BUILDING_HOME_UPGRADE = "mars_building_home_upgrade";// 火星主基地升级窗口
        public const string C_MARS_BUILDING_HOME_UPGRADING = "mars_building_home_upgrading";// 火星主基地升级中窗口
        public const string C_MARS_BUILDING_INFO = "mars_building_info";// 火星建筑信息窗口
        public const string C_MARS_BUILDING_UPGRADING = "mars_building_upgrading";// 火星建筑升级中窗口
        
        #region 火星拓展

        public const string C_MARS_EXPAND_BUILDING_DETAIL = "mars_expand_building_detail";// 火星建筑详情窗口
        public const string C_MARS_EXPAND_BUILDING_MORE_DETAIL = "mars_expand_building_more_detail";// 火星建筑更多详情窗口
        public const string C_MARS_EXPAND_BUILDING_UPGRADE = "mars_expand_building_upgrade";//火星拓展建筑升级窗口

        #endregion
    }
}
