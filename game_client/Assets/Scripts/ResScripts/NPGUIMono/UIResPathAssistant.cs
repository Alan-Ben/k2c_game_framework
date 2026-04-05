using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// UI资源路径获取工具类
    /// </summary>
    public class UIResPathAssistant
    {

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        //资源路径，程序开发时候用，正式环境不走这里
        private static Dictionary<long, NPCommonAssetPathInfo> _m_uiPathDict = new Dictionary<long, NPCommonAssetPathInfo>()
        {
            #region Common
            {UIResPathConst.WIN_TOOL_TIP_TEXT, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "win_tool_tip_text_vertical")},
            {UIResPathConst.WIN_TOOL_TIP_TEXT_FOLLOW, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "win_tool_tip_text_horizontal")},
            {UIResPathConst.WIN_TOOL_TIP_TITLE_TEXT, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "win_tool_tip_text_normal")},
            {UIResPathConst.WIN_TOOL_TIP_ITEM_DETAIL_BTN, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "win_tool_tip_name_num_des_btn")},
            {UIResPathConst.WIN_TOOL_TIP_ITEM_DRESS, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "win_tool_tip_icon_detail")},

            {UIResPathConst.WIN_COMMON_RESOURCES_TIP, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "win_common_resources_tip")},
            {UIResPathConst.WIN_GET_ITEM, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "win_get_item")},
            {UIResPathConst.WIN_COMMON_RESOURCES_WITHOUTBAG_TIP, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "win_common_resources_withoutbag_tip")},
            {UIResPathConst.WIN_MUSEUM_FARGMENT_AUTO_COMPOSE, new NPCommonAssetPathInfo("gui/museum.unity3d", "win_museum_fargment_auto_compose")},
            {UIResPathConst.WIN_COMMON_COST_TOGGLE_DIALOG, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "win_common_cost_toggle_dialog")},
            {UIResPathConst.WIN_COMMON_TOGGLE_DIALOG, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "win_common_toggle_dialog")},
            {UIResPathConst.WIN_COMMON_ACCESS_WAYS, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "win_common_access_ways")},
            {UIResPathConst.WIN_COMMON_PLAYER_BUSINESS_CARD_TIP, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "win_common_player_business_card_tip")},
            {UIResPathConst.WIN_LOADING_MIST, new NPCommonAssetPathInfo("gui/common_ui.unity3d", "win_loading_mist")},
            {UIResPathConst.WIN_COMMON_BACK, new NPCommonAssetPathInfo("gui/common_gui.unity3d", "win_common_back")},
            {UIResPathConst.WIN_BURST_PARTICLE, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "win_burst_particle")},
            {UIResPathConst.WIN_CENTER_TIPS, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "center_tips")},
            {UIResPathConst.WIN_COMMON_TOGGLE_FORCE_DIALOG, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "win_common_toggle_force_dialog")},
            {UIResPathConst.WIN_COMMON_COST_DIALOG, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "win_common_cost_dialog")},
            {UIResPathConst.WIN_COMMON_REWARD_PREVIEW, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "win_common_reward_preview")},
            {UIResPathConst.WIN_COMMON_REWARD_PREVIEW_TOOL_TIP, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "win_tool_tip_reward_preview")},
            {UIResPathConst.WIN_COMMON_SCENE_DRAG_TIP, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "win_scene_drag_tip")},

            {UIResPathConst.C_FLOURISH_TIP_RES_ID, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "player_flourish_tips")},

            {UIResPathConst.C_ACTIVITY_CENTER_RES_ID, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "win_activity_hall")},
            {UIResPathConst.C_DEFAULT_RESBAR_RES_ID, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "win_resbar_1")},
            
            {UIResPathConst.WIN_COMMON_SHARE_MAIN, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "win_common_share")},
            {UIResPathConst.PREFAB_COMMON_SHARE_FRIEND_PAGR, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "prefab_common_share_friend_page")},

            {UIResPathConst.WIN_GET_SPECIAL_ITEM, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "win_get_showoffitem")},
            {UIResPathConst.WIN_COMMON_SKIP, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "win_common_skip")},
            {UIResPathConst.WIN_SCREEN_SFX, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "win_screen_sfx")},

            {UIResPathConst.WIN_TUTORIAL_MOVE_MASK, new NPCommonAssetPathInfo("gui/tutorial_gui.unity3d", "guide_movementmask")},
                
            {UIResPathConst.PREFAB_COMMON_ACCESS_WAYS_COMBINED_ITEM, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "prefab_common_access_ways_item3")},
            {UIResPathConst.WIN_COMMON_PLAYER_AVATAR_DOWNLOAD_TIP, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "win_res_download_clothes_doing")},
            {UIResPathConst.WIN_COMMON_ADD_PACK, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "win_add_pack")},
            {UIResPathConst.WIN_COMMON_LOW_FRAME_RATE, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "win_common_toggle_lowframetip")},
            
            {UIResPathConst.C_COMMON_EVENT_COMPLETED_RES_ID, new NPCommonAssetPathInfo("gui/event.unity3d", "win_event_completed")},
            {UIResPathConst.C_COMMON_EVENT_AWARD_EVENT_RES_ID, new NPCommonAssetPathInfo("gui/event.unity3d", "win_event_reward")},
            {UIResPathConst.C_COMMON_EVENT_CHOICE_EVENT_RES_ID, new NPCommonAssetPathInfo("gui/event.unity3d", "win_event_select")},
            {UIResPathConst.C_COMMON_EVENT_DISPATCH_EVENT_RES_ID, new NPCommonAssetPathInfo("gui/event.unity3d", "win_event_knight_choise")},
            // {UIResPathConst.C_COMMON_EVENT_DISPATCH_EVENT_SELECT_HERO_RES_ID, new NPCommonAssetPathInfo("gui/event.unity3d", "win_event_knight_choise")},
            {UIResPathConst.C_COMMON_EVENT_FITTING_EVENT_RES_ID, new NPCommonAssetPathInfo("gui/event.unity3d", "win_event_fitting")},
            {UIResPathConst.C_OFFLINE_GOLD_EARNINGS, new NPCommonAssetPathInfo("gui/city_building.unity3d", "win_city_offline_income")},
            {UIResPathConst.C_MAIN_FUNCTION_SELECT, new NPCommonAssetPathInfo("gui/main.unity3d", "win_main_tab")},
            {UIResPathConst.WIN_COMMON_SYSTEM_QUEST_DETAIL, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "win_com_gameplay_quest_main")},
            {55, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "win_common_random_box_info")},//奖励概率详情
            {56, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "win_common_improve_way")},//通用提升途径弹窗

            #endregion
                
            #region 伙伴 1000-1099
            
            {1000, new NPCommonAssetPathInfo("gui/hero.unity3d", "win_hero_main")},
            {1001, new NPCommonAssetPathInfo("gui/hero.unity3d", "win_hero_get")},
            {1002, new NPCommonAssetPathInfo("gui/hero.unity3d", "win_hero_info")},
            {1003, new NPCommonAssetPathInfo("gui/hero.unity3d", "page_hero_detail")},
            {1004, new NPCommonAssetPathInfo("gui/hero.unity3d", "win_pop_hero_limit_break")},
            {1005, new NPCommonAssetPathInfo("gui/hero.unity3d", "win_pop_hero_limit_break_successful")},
            {1006, new NPCommonAssetPathInfo("gui/hero.unity3d", "page_hero_business")},
            {1007, new NPCommonAssetPathInfo("gui/hero.unity3d", "win_pop_hero_business_successful")},
            {1008, new NPCommonAssetPathInfo("gui/hero.unity3d", "page_hero_talent")},
            {1009, new NPCommonAssetPathInfo("gui/hero.unity3d", "win_hero_talent_dec_tip")},
            {1010, new NPCommonAssetPathInfo("gui/hero.unity3d", "page_hero_star")},
            {1011, new NPCommonAssetPathInfo("gui/hero.unity3d", "win_pop_hero_awaken")},
            {1012, new NPCommonAssetPathInfo("gui/hero.unity3d", "page_hero_halo")},
            {1013, new NPCommonAssetPathInfo("gui/hero.unity3d", "win_pop_hero_halo")},
            {1014, new NPCommonAssetPathInfo("gui/hero.unity3d", "page_hero_info_memoir")},
            {1015, new NPCommonAssetPathInfo("gui/hero.unity3d", "page_hero_consort")},
            {1016, new NPCommonAssetPathInfo("gui/hero.unity3d", "win_pop_hero_equip")},
            {1017, new NPCommonAssetPathInfo("gui/hero.unity3d", "win_pop_hero_equip_replace")},
            {1018, new NPCommonAssetPathInfo("gui/hero.unity3d", "page_pop_hero_nature_pow")},
            {1019, new NPCommonAssetPathInfo("gui/hero.unity3d", "page_pop_hero_nature_suit")},
            {1020, new NPCommonAssetPathInfo("gui/hero.unity3d", "win_hero_lock_info")},
            {1021, new NPCommonAssetPathInfo("gui/hero.unity3d", "page_hero_lock_info_memoir")},
            {1022, new NPCommonAssetPathInfo("gui/hero.unity3d", "page_hero_lock_info_skill")},
            {1023, new NPCommonAssetPathInfo("gui/hero.unity3d", "win_hero_nature")},
            {1024, new NPCommonAssetPathInfo("gui/hero.unity3d", "win_hero_skin")},
            {1025, new NPCommonAssetPathInfo("gui/hero.unity3d", "win_hero_get_tip")},
            {1026, new NPCommonAssetPathInfo("gui/hero.unity3d", "prefab_hero_list_notowned_bar")},
            {1027, new NPCommonAssetPathInfo("gui/hero.unity3d", "prefab_hero_star_skill_info_tip")},
            {1028, new NPCommonAssetPathInfo("gui/hero.unity3d", "page_hero_skin_win")},
            {1029, new NPCommonAssetPathInfo("gui/hero.unity3d", "win_hero_star_talent_tip")},
            {1030, new NPCommonAssetPathInfo("gui/hero.unity3d", "prefab_hero_list_owned_bar")},

            #endregion


            #region 城建 1100 -1199

            {1100, new NPCommonAssetPathInfo("gui/city_building.unity3d", "win_city_business_building_main")},
            {1101, new NPCommonAssetPathInfo("gui/city_building.unity3d", "win_city_business_building_main_add")},
            {1102, new NPCommonAssetPathInfo("gui/city_building.unity3d", "prefab_city_business_building_entrance")},
            {1103, new NPCommonAssetPathInfo("gui/city_building.unity3d", "win_city_business_building_appoint")},
            {1104, new NPCommonAssetPathInfo("gui/city_building.unity3d", "win_city_business_building_upgrade")},
            {1105, new NPCommonAssetPathInfo("gui/city_building.unity3d", "win_city_business_building_upgrade_success")},
            {1106, new NPCommonAssetPathInfo("gui/city_building.unity3d", "win_city_farming_upgrade")},
            {1107, new NPCommonAssetPathInfo("gui/city_building.unity3d", "win_city_farming_upgrade_success")},
            {1108, new NPCommonAssetPathInfo("gui/city_building.unity3d", "win_city_business_building_build")},
            {1109, new NPCommonAssetPathInfo("gui/city_building.unity3d", "win_city_business_building_build_success")},
            {1110, new NPCommonAssetPathInfo("gui/city_building.unity3d", "prefab_city_building_build_tip")},
            {1111, new NPCommonAssetPathInfo("gui/city_building.unity3d", "prefab_city_building_unbuilt_nonsense")},
            {1112, new NPCommonAssetPathInfo("gui/city_building.unity3d", "prefab_city_building_farming_building_name")},
            {1113, new NPCommonAssetPathInfo("gui/city_building.unity3d", "prefab_city_building_coin_earning_tip")},
            {1114, new NPCommonAssetPathInfo("gui/city_building.unity3d", "prefab_city_building_click_coin_earning_tip")},
            {1115, new NPCommonAssetPathInfo("gui/city_building.unity3d", "win_city_business_building_change_hero_tip")},
            {1116, new NPCommonAssetPathInfo("gui/city_building.unity3d", "prefab_city_stage_goal_building_entrance")},
            {1117, new NPCommonAssetPathInfo("gui/city_building.unity3d", "win_city_business_building_business")},
            {1118, new NPCommonAssetPathInfo("gui/city_building.unity3d", "prefab_city_business_building_business_develop")},
            {1119, new NPCommonAssetPathInfo("gui/city_building.unity3d", "prefab_city_business_building_business_product")},
            {1120, new NPCommonAssetPathInfo("gui/city_building.unity3d", "win_city_business_building_product_unlock_success")},
            {1198, new NPCommonAssetPathInfo("gui/city_building.unity3d", "win_building_main")},
            {1199, new NPCommonAssetPathInfo("gui/city_building.unity3d", "win_building_follow")},
            {1197, new NPCommonAssetPathInfo("gui/city_building.unity3d", "win_home_entry_follow")},

            #endregion
                
            #region 子嗣 1200-1299
            {1201, new NPCommonAssetPathInfo("gui/child.unity3d", "win_child_grow_room")},
            {1202, new NPCommonAssetPathInfo("gui/child.unity3d", "win_child_get")},
            {1203, new NPCommonAssetPathInfo("gui/child.unity3d", "prefab_pop_child_detail")},
            {1204, new NPCommonAssetPathInfo("gui/child.unity3d", "win_child_pop_graduate")},
            {1205, new NPCommonAssetPathInfo("gui/child.unity3d", "win_child_pop_graduate_multi")},
            {1206, new NPCommonAssetPathInfo("gui/child.unity3d", "win_child_pop_graduate_single")},
            {1207, new NPCommonAssetPathInfo("gui/child.unity3d", "win_child_pop_name_change")},
            {1208, new NPCommonAssetPathInfo("gui/child.unity3d", "win_child_pop_restore_EP")},
            {1209, new NPCommonAssetPathInfo("gui/child.unity3d", "win_child_pop_stage_up")},
            {1210, new NPCommonAssetPathInfo("gui/child.unity3d", "win_child_pop_stage_up_result")},
            {1211, new NPCommonAssetPathInfo("gui/child.unity3d", "prefab_pop_child_auto_educate")},
            {1212, new NPCommonAssetPathInfo("gui/child.unity3d", "prefab_pop_child_graduate_detail")},
            {1232, new NPCommonAssetPathInfo("gui/child.unity3d", "win_child_main")},
            #endregion

            #region 聊天
           
            {1300, new NPCommonAssetPathInfo("gui/chat.unity3d", "win_chat_main")},
            {1311, new NPCommonAssetPathInfo("gui/chat.unity3d", "page_chat_list")},
            {1312, new NPCommonAssetPathInfo("gui/chat.unity3d", "page_private_chat_list")},
            {1313, new NPCommonAssetPathInfo("gui/chat.unity3d", "page_friend_list")},
            {1314, new NPCommonAssetPathInfo("gui/chat.unity3d", "prefab_friend_group_bar")},
            {1315, new NPCommonAssetPathInfo("gui/chat.unity3d", "win_chat_pop_edit_group")},
            {1316, new NPCommonAssetPathInfo("gui/chat.unity3d", "win_chat_pop_add_friend")},
            {1317, new NPCommonAssetPathInfo("gui/chat.unity3d", "page_friend_recommend")},
            {1318, new NPCommonAssetPathInfo("gui/chat.unity3d", "page_friend_request")},
            {1319, new NPCommonAssetPathInfo("gui/chat.unity3d", "win_chat_pop_hero_share")},
            {1320, new NPCommonAssetPathInfo("gui/chat.unity3d", "win_chat_hero_info")},
            {1321, new NPCommonAssetPathInfo("gui/chat.unity3d", "win_chat_pop_share_consort")},
            {1322, new NPCommonAssetPathInfo("gui/chat.unity3d", "win_chat_consort_details")},
            {1323, new NPCommonAssetPathInfo("gui/chat.unity3d", "win_chat_pop_share_child")},
            {1324, new NPCommonAssetPathInfo("gui/chat.unity3d", "win_chat_child_details")},
            {1325, new NPCommonAssetPathInfo("gui/chat.unity3d", "win_chat_pop_add_group")},
            {1350, new NPCommonAssetPathInfo("gui/chat.unity3d", "prefab_chat_msg_item_dinner_R")},
            {1351, new NPCommonAssetPathInfo("gui/chat.unity3d", "prefab_chat_msg_item_dinner_L")},
            {1352, new NPCommonAssetPathInfo("gui/chat.unity3d", "prefab_chat_msg_item_adult_marry_R")},
            {1353, new NPCommonAssetPathInfo("gui/chat.unity3d", "prefab_chat_msg_item_adult_marry_L")},
            {1354, new NPCommonAssetPathInfo("gui/chat.unity3d", "prefab_chat_msg_item_hero_R")},
            {1355, new NPCommonAssetPathInfo("gui/chat.unity3d", "prefab_chat_msg_item_hero_L")},
            {1356, new NPCommonAssetPathInfo("gui/chat.unity3d", "prefab_chat_msg_item_emote_R")},
            {1357, new NPCommonAssetPathInfo("gui/chat.unity3d", "prefab_chat_msg_item_emote_L")},
            {1358, new NPCommonAssetPathInfo("gui/chat.unity3d", "win_pop_friend_list")},
            {1359, new NPCommonAssetPathInfo("gui/chat.unity3d", "prefab_chat_msg_item_share_clothes_R")},
            {1360, new NPCommonAssetPathInfo("gui/chat.unity3d", "prefab_chat_msg_item_share_clothes_L")},
            {1361, new NPCommonAssetPathInfo("gui/chat.unity3d", "win_garden_visit_main")},
            {1362, new NPCommonAssetPathInfo("gui/chat.unity3d", "win_pop_block_friend_list")},
            {1363, new NPCommonAssetPathInfo("gui/guild.unity3d", "prefab_chat_guild_system_notifications")},
            {1364, new NPCommonAssetPathInfo("gui/guild.unity3d", "prefab_chat_msg_item_guild_notice_L")},
            {1365, new NPCommonAssetPathInfo("gui/guild.unity3d", "prefab_chat_msg_item_guild_notice_R")},
            {1366, new NPCommonAssetPathInfo("gui/guild.unity3d", "prefab_chat_msg_item_guild_recruit_L")},
            {1367, new NPCommonAssetPathInfo("gui/guild.unity3d", "prefab_chat_msg_item_guild_recruit_R")},
            {1368, new NPCommonAssetPathInfo("gui/chat.unity3d", "prefab_chat_msg_item_box_L")},
            {1369, new NPCommonAssetPathInfo("gui/chat.unity3d", "prefab_chat_msg_item_box_R")},
            {1370, new NPCommonAssetPathInfo("gui/chat.unity3d", "win_chat_pop_share_CG")},//聊天分享-CG分享弹窗
            {1371, new NPCommonAssetPathInfo("gui/chat.unity3d", "win_chat_CG_details")},//聊天分享-CG分享详情
            {1372, new NPCommonAssetPathInfo("gui/chat.unity3d", "prefab_chat_msg_item_CG_L")},//聊天-CG分享（L）
            {1373, new NPCommonAssetPathInfo("gui/chat.unity3d", "prefab_chat_msg_item_CG_R")},//聊天-CG分享（R）
            {1374, new NPCommonAssetPathInfo("gui/chat.unity3d", "win_pop_chat_marry_union")},//聊天-学徒结伴申请弹窗
            {1375, new NPCommonAssetPathInfo("gui/chat.unity3d", "prefab_chat_msg_item_evening_dungeon_box_L")},//聊天-晚间副本宝箱（L）
            {1376, new NPCommonAssetPathInfo("gui/chat.unity3d", "prefab_chat_msg_item_evening_dungeon_box_R")},//聊天-晚间副本宝箱（R）

            {1377, new NPCommonAssetPathInfo("gui/chat.unity3d", "prefab_chat_msg_item_system_log")},// 系统log
            {1381, new NPCommonAssetPathInfo("gui/chat.unity3d", "prefab_chat_msg_item_common_box")},// 通用宝箱
            {1385, new NPCommonAssetPathInfo("gui/chat.unity3d", "prefab_chat_msg_item_system_log_earning_goal")},// 通用宝箱
            {1386, new NPCommonAssetPathInfo("gui/chat.unity3d", "prefab_chat_msg_item_share_mars_explore_mine_L")},// 聊天-矿分享（L）
            {1387, new NPCommonAssetPathInfo("gui/chat.unity3d", "prefab_chat_msg_item_share_mars_explore_mine_R")},// 聊天-矿分享（R）
            {1388, new NPCommonAssetPathInfo("gui/chat.unity3d", "win_pop_chat_complain")},// 聊天-举报弹窗
            
            #endregion

            #region 乐园主界面 1500 -1599 

            {1501, new NPCommonAssetPathInfo("gui/main.unity3d", "win_main_bar")},
            {1510, new NPCommonAssetPathInfo("gui/main.unity3d", "win_main_home")},
            {1511, new NPCommonAssetPathInfo("gui/main.unity3d", "win_main_room")},
            {1512, new NPCommonAssetPathInfo("gui/main.unity3d", "win_hero_battle_entry")},
            {1514, new NPCommonAssetPathInfo("gui/main.unity3d", "win_main_room")},
            {1515, new NPCommonAssetPathInfo("gui/main.unity3d", "win_main_power_info_tip")},
            {1516, new NPCommonAssetPathInfo("gui/main.unity3d", "win_main_wars")},
            {1517, new NPCommonAssetPathInfo("gui/main.unity3d", "win_main_child")},

            #endregion
            
            #region 情人系统 1400 -1499
            
            {1401, new NPCommonAssetPathInfo("gui/consort.unity3d", "win_consort_main")},
            {1402, new NPCommonAssetPathInfo("gui/consort.unity3d", "win_consort_enter")},
            {1403, new NPCommonAssetPathInfo("gui/consort.unity3d", "win_consort_details_lock")},
            {1404, new NPCommonAssetPathInfo("gui/consort.unity3d", "prefab_consort_list_bar")},
            {1405, new NPCommonAssetPathInfo("gui/consort.unity3d", "win_consort_details_info")},
            {1406, new NPCommonAssetPathInfo("gui/consort.unity3d", "win_consort_details_showcase")},
            {1407, new NPCommonAssetPathInfo("gui/consort.unity3d", "win_consort_details_op_wnd")},
            {1408, new NPCommonAssetPathInfo("gui/consort.unity3d", "win_consort_CG_main")},
            {1409, new NPCommonAssetPathInfo("gui/consort.unity3d", "win_consort_CG_detail")},
            {1410, new NPCommonAssetPathInfo("gui/consort.unity3d", "win_consort_skin")},
            {1411, new NPCommonAssetPathInfo("gui/consort.unity3d", "win_consort_random_date_reward")},//随机邀约奖励窗口
            {1412, new NPCommonAssetPathInfo("gui/consort.unity3d", "win_consort_onekey_date_reward")},//一键邀约奖励窗口
            {1413, new NPCommonAssetPathInfo("gui/consort.unity3d", "win_consort_skin_unlock")},//妃子皮肤解锁弹窗
            {1414, new NPCommonAssetPathInfo("gui/consort.unity3d", "win_pop_consort_stella_current")},//妃子星辉效果弹窗
            {1415, new NPCommonAssetPathInfo("gui/consort.unity3d", "win_pop_consort_stella_levelup")},//妃子星辉升级结果弹窗
            {1416, new NPCommonAssetPathInfo("gui/consort.unity3d", "win_pop_consort_bond_level_up")},//妃子提升羁绊窗口
            {1417, new NPCommonAssetPathInfo("gui/consort.unity3d", "win_consort_get")},//获取妃子窗口
            {1418, new NPCommonAssetPathInfo("gui/consort.unity3d", "win_pop_consort_story_unlock")},//故事解锁弹窗
            {1419, new NPCommonAssetPathInfo("gui/consort.unity3d", "win_pop_consort_skill_unlock")},//经营技能解锁弹窗

            {1422, new NPCommonAssetPathInfo("gui/consort.unity3d", "win_pop_consort_date_effect_detail")},//家人-指定邀约效果详情浮窗
            {1423, new NPCommonAssetPathInfo("gui/consort.unity3d", "win_pop_consort_child_effect_detail")},//家人-获得卷王效果详情浮窗
            
            {1424, new NPCommonAssetPathInfo("gui/consort.unity3d", "win_consort_info_page")},//家人-详情页面简介窗口
            {1425, new NPCommonAssetPathInfo("gui/consort.unity3d", "win_consort_story_page")},//家人-故事窗口

            {1426, new NPCommonAssetPathInfo("gui/consort.unity3d", "win_consort_random_date_calling")},//家人-随机邀约表现过程弹窗
            {1427, new NPCommonAssetPathInfo("gui/consort.unity3d", "win_consort_onekey_date_traveling")},//家人-指定邀约表现过程弹窗
            {1428, new NPCommonAssetPathInfo("gui/consort.unity3d", "win_pop_consort_entrance_seclect")},//家人-入口自定义选择弹窗
            {1429, new NPCommonAssetPathInfo("gui/consort.unity3d", "win_consort_CG_get")},//家人-获取CG弹窗
            {1430, new NPCommonAssetPathInfo("gui/consort.unity3d", "win_pop_consort_CG_share")},//家人-CG分享确认弹窗
            
            {1431, new NPCommonAssetPathInfo("gui/consort.unity3d", "win_room_scene")},//家人-豪宅主界面
            {1432, new NPCommonAssetPathInfo("gui/consort.unity3d", "win_room_background")},//家人-豪宅皮肤界面
            
            // 1450后的是可能配置在UI上的, 代码别直接写id
            {1450, new NPCommonAssetPathInfo("gui/consort.unity3d", "prefab_consort_info_page")},//妃子详情页面简介page
            {1451, new NPCommonAssetPathInfo("gui/consort.unity3d", "prefab_consort_stella_page")},//妃子详情页面星辉page
            {1452, new NPCommonAssetPathInfo("gui/consort.unity3d", "prefab_consort_bond_page")},//妃子详情页面羁绊page
            {1453, new NPCommonAssetPathInfo("gui/consort.unity3d", "prefab_consort_skill_page")},//妃子详情页面经营page
            {1454, new NPCommonAssetPathInfo("gui/consort.unity3d", "prefab_consort_relation_page")},//妃子详情页面加护page
            {1455, new NPCommonAssetPathInfo("gui/consort.unity3d", "win_consort_interaction")},//妃子详情页面互动page

            {1460, new NPCommonAssetPathInfo("gui/consort.unity3d", "prefab_consort_gift_page")},
            {1461, new NPCommonAssetPathInfo("gui/consort.unity3d", "prefab_consort_story_page")},
            {1462, new NPCommonAssetPathInfo("gui/consort.unity3d", "prefab_consort_travel_page")},
            {1463, new NPCommonAssetPathInfo("gui/consort.unity3d", "go_story_title_bar")},
            
            {1470, new NPCommonAssetPathInfo("gui/consort.unity3d", "win_pop_consort_lock_bond_list")},
            {1471, new NPCommonAssetPathInfo("gui/consort.unity3d", "win_pop_consort_bond_list")},
            {1472, new NPCommonAssetPathInfo("gui/consort.unity3d", "win_pop_consort_student_list")},

            
            #endregion
                
            #region 征收 1600-1699
            
            {1602, new NPCommonAssetPathInfo("gui/levy.unity3d", "win_levy_silver")},
            {1603, new NPCommonAssetPathInfo("gui/levy.unity3d", "win_levy_soldier")},
            {1604, new NPCommonAssetPathInfo("gui/levy.unity3d", "win_levy_food")},
            {1605, new NPCommonAssetPathInfo("gui/levy.unity3d", "levy_silver_follow_item")},
            {1606, new NPCommonAssetPathInfo("gui/levy.unity3d", "levy_food_follow_item")},
            {1607, new NPCommonAssetPathInfo("gui/levy.unity3d", "levy_soldier_follow_item")},
            {1612, new NPCommonAssetPathInfo("gui/levy.unity3d", "win_levy_silver_offline")},

            #endregion

            #region 玩家信息 1700-1799
            {1700, new NPCommonAssetPathInfo("gui/player_info.unity3d", "win_Player_main")},
            {1701, new NPCommonAssetPathInfo("gui/player_info.unity3d", "win_player_info_dress_title")},
            {1702, new NPCommonAssetPathInfo("gui/player_info.unity3d", "win_player_info_dress_main")},
            {1703, new NPCommonAssetPathInfo("gui/player_info.unity3d", "win_player_info_dress_icon_tab")},
            {1704, new NPCommonAssetPathInfo("gui/player_info.unity3d", "win_player_info_dress_icon_bgk_tab")},
            {1705, new NPCommonAssetPathInfo("gui/player_info.unity3d", "win_player_info_dress_icon_bubble_tab")},
            {1706, new NPCommonAssetPathInfo("gui/player_info.unity3d", "win_player_rename")},
            {1707, new NPCommonAssetPathInfo("gui/player_info.unity3d", "win_player_info_privilege")},
            {1708, new NPCommonAssetPathInfo("gui/player_info.unity3d", "win_player_info_title_explain")},
            {1709, new NPCommonAssetPathInfo("gui/player_info.unity3d", "win_player_info_other")},
            {1710, new NPCommonAssetPathInfo("gui/player_info.unity3d", "win_player_upgrade_success")},
            {1715, new NPCommonAssetPathInfo("gui/player_info.unity3d", "win_player_info_hero_join")},
            {1716, new NPCommonAssetPathInfo("gui/player_info.unity3d", "win_player_daily_reward_preview")},
            {1717, new NPCommonAssetPathInfo("gui/player_info.unity3d", "win_player_info_title_main")},
            {1721, new NPCommonAssetPathInfo("gui/player_info.unity3d", "prefab_player_info_limit_title_list_bar")},
            {1722, new NPCommonAssetPathInfo("gui/player_info.unity3d", "prefab_player_info_title_info_tip")},
            {1723, new NPCommonAssetPathInfo("gui/player_info.unity3d", "win_player_info_skin_main")},
            {1724, new NPCommonAssetPathInfo("gui/player_info.unity3d", "win_player_info_get_skin")},
            {1725, new NPCommonAssetPathInfo("gui/player_info.unity3d", "win_player_info_skin_upgrade_success")},
            {1726, new NPCommonAssetPathInfo("gui/player_info.unity3d", "prefab_player_info_icon_type_item")},
            {1750, new NPCommonAssetPathInfo("gui/player_info.unity3d", "win_player_set_up_push")},

	        #endregion
             
            #region 藏品 1800-1899

            {1800, new NPCommonAssetPathInfo("gui/equip.unity3d", "win_equip_main")},
            {1801, new NPCommonAssetPathInfo("gui/equip.unity3d", "win_equip_info")},
            {1802, new NPCommonAssetPathInfo("gui/equip.unity3d", "win_equip_info_page")},
            {1803, new NPCommonAssetPathInfo("gui/equip.unity3d", "win_equip_lock_info")},
            {1804, new NPCommonAssetPathInfo("gui/equip.unity3d", "page_equip_upgrade")},
            {1805, new NPCommonAssetPathInfo("gui/equip.unity3d", "page_equip_skill")},
            {1806, new NPCommonAssetPathInfo("gui/equip.unity3d", "win_equip_melt")},
            {1807, new NPCommonAssetPathInfo("gui/equip.unity3d", "win_equip_melt_details_confirm")},
            {1808, new NPCommonAssetPathInfo("gui/equip.unity3d", "win_equip_get")},

	        #endregion

            #region 背包 1900-1999

            {1905, new NPCommonAssetPathInfo("gui/bag.unity3d", "win_bag")},
            {1906, new NPCommonAssetPathInfo("gui/bag.unity3d", "win_bag_item_pop_detail_simple")},
            {1907, new NPCommonAssetPathInfo("gui/bag.unity3d", "win_bag_item_pop_use_simple")},
            {1908, new NPCommonAssetPathInfo("gui/bag.unity3d", "win_bag_item_use")},
            {1909, new NPCommonAssetPathInfo("gui/bag.unity3d", "win_bag_item_use_percent")},
            {1910, new NPCommonAssetPathInfo("gui/bag.unity3d", "win_bag_item_use_select")},
            {1911, new NPCommonAssetPathInfo("gui/bag.unity3d", "win_bag_item_pop_convert_simple")},
            {1912, new NPCommonAssetPathInfo("gui/bag.unity3d", "win_bag_item_convert")},
            {1913, new NPCommonAssetPathInfo("gui/bag.unity3d", "prefab_bag_item_sort_title")},
            {1914, new NPCommonAssetPathInfo("gui/bag.unity3d", "win_bag_item_auto_convert")},
            {1915, new NPCommonAssetPathInfo("gui/bag.unity3d", "win_bag_item_knight")},
            {1916, new NPCommonAssetPathInfo("gui/bag.unity3d", "win_bag_goods")},
            {1917, new NPCommonAssetPathInfo("gui/bag.unity3d", "win_bag_attr_up")},
            {1918, new NPCommonAssetPathInfo("gui/bag.unity3d", "win_bag_item_simple_combine")},
            {1920, new NPCommonAssetPathInfo("gui/bag.unity3d", "win_bag_item_use_show_real_gain_item")},
            {1921, new NPCommonAssetPathInfo("gui/bag.unity3d", "win_bag_item_consort")},
            {1922, new NPCommonAssetPathInfo("gui/bag.unity3d", "win_bag_convert")},
            {1923, new NPCommonAssetPathInfo("gui/bag.unity3d", "win_bag_item_pop_convertUse_simple")},

            #endregion
            #region 关卡 2000-2099

            {2000, new NPCommonAssetPathInfo("gui/avatar_gacha.unity3d", "win_avatar_gacha_main")},
            {2001, new NPCommonAssetPathInfo("gui/avatar_gacha.unity3d", "prefab_avatar_gacha_pool_item")},
            {2002, new NPCommonAssetPathInfo("gui/avatar_gacha.unity3d", "win_avatar_gacha_clothes_unit_gather")},
            {2003, new NPCommonAssetPathInfo("gui/avatar_gacha.unity3d", "win_avatar_content_unit_grid_bar")},
            {2004, new NPCommonAssetPathInfo("gui/avatar_gacha.unity3d", "win_avatar_gacha_draw_main")},
            {2011, new NPCommonAssetPathInfo("gui/avatar_gacha.unity3d", "win_avatar_gacha_Info_1")},
            {2012, new NPCommonAssetPathInfo("gui/avatar_gacha.unity3d", "win_avatar_gacha_Info_2")},
            {2016, new NPCommonAssetPathInfo("gui/avatar_gacha.unity3d", "win_acatat_gacha_pop_inform")},
            {2017, new NPCommonAssetPathInfo("gui/avatar_gacha.unity3d", "page_clothes_item_list")},
            {2018, new NPCommonAssetPathInfo("gui/avatar_gacha.unity3d", "page_records")},

            #endregion

            #region 关卡 2100-2199

            {2100, new NPCommonAssetPathInfo("gui/chapter.unity3d", "win_chapter_main_v2")}, // 关卡游戏界面
            {2102, new NPCommonAssetPathInfo("gui/chapter.unity3d", "win_chapter_battle_boss")}, // boss战斗页面
            {2103, new NPCommonAssetPathInfo("gui/chapter.unity3d", "win_chapter_boost_tutelage")}, // 关卡自动前期设置界面
            {2104, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "top_tips_chapter_plot")}, // 关卡自动前期设置界面
            {2106, new NPCommonAssetPathInfo("gui/chapter.unity3d", "win_chapter_main_video")}, // 关卡游戏界面视频窗口
            {2107, new NPCommonAssetPathInfo("gui/chapter.unity3d", "win_common_toggle_chapter_auto")}, // 关卡跳过对话确认窗口

            {2111, new NPCommonAssetPathInfo("gui/chapter.unity3d", "win_chatper_event_reward")}, //关卡奖励事件
            {2112, new NPCommonAssetPathInfo("gui/chapter.unity3d", "win_chatper_event_choice")}, //关卡选项事件
            {2113, new NPCommonAssetPathInfo("gui/chapter.unity3d", "win_chatper_event_end")}, //关卡事件结果
            {2114, new NPCommonAssetPathInfo("gui/chapter.unity3d", "win_chatper_event_hero")}, //关卡派遣事件
            {2115, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "top_tips_chapter_event")}, //关卡事件提示窗口
            {2117, new NPCommonAssetPathInfo("gui/chapter.unity3d", "win_story_review_main")}, //关卡总览-故事总览界面
            {2118, new NPCommonAssetPathInfo("gui/chapter.unity3d", "win_story_review_details")}, //关卡总览-剧情详情界面
            {2130, new NPCommonAssetPathInfo("gui/chapter.unity3d", "win_chapter_map_main")}, // 关卡地图
            {2131, new NPCommonAssetPathInfo("gui/chapter.unity3d", "win_chapter_auto_forward_reward")}, // 自动前进奖励
            #endregion
            
            #region 召唤2200-2299

            {2201, new NPCommonAssetPathInfo("gui/summon.unity3d", "win_summon_main")}, // 精灵之泉（抽卡）-主界面
            {2202, new NPCommonAssetPathInfo("gui/summon.unity3d", "win_summon_get")}, // 精灵之泉（抽卡）-获得界面
            {2203, new NPCommonAssetPathInfo("gui/summon.unity3d", "win_summon_count")}, // 精灵之泉（抽卡）-额外奖励弹窗
            {2204, new NPCommonAssetPathInfo("gui/summon.unity3d", "win_summon_probability")}, // 精灵之泉（抽卡）-概率展示弹窗
            {2205, new NPCommonAssetPathInfo("gui/summon.unity3d", "win_summon_turntable")}, // 精灵之泉（抽卡）- 表现流程

            #endregion
            
            #region 登录 2300-2399
            
            {2301, new NPCommonAssetPathInfo("gui/login_gui.unity3d", "win_login_game")},
            {2302, new NPCommonAssetPathInfo("gui/login_gui.unity3d", "win_login_choose_server")},
            {2304, new NPCommonAssetPathInfo("gui/login_gui.unity3d", "win_login_account_way")},
            {2306, new NPCommonAssetPathInfo("gui/login_gui.unity3d", "win_use_account")},
            {2307, new NPCommonAssetPathInfo("gui/login_gui.unity3d", "account_grid")},
            {2310, new NPCommonAssetPathInfo("gui/login_gui.unity3d", "wnd_queue")},

            #endregion

            #region 任务 2400-2499

            {2401, new NPCommonAssetPathInfo("gui/quest.unity3d", "win_quest_main")},
            {2402, new NPCommonAssetPathInfo("gui/quest.unity3d", "win_quest_strong")},
            {2403, new NPCommonAssetPathInfo("gui/quest.unity3d", "win_quest_complete_tips")},
            {2404, new NPCommonAssetPathInfo("gui/quest.unity3d", "win_quest_entry_tip")},
            {2405, new NPCommonAssetPathInfo("gui/quest.unity3d", "page_quest_task")},

            #endregion

            #region 成年子嗣 2500~2599
            {2501, new NPCommonAssetPathInfo("gui/child_marry.unity3d", "win_Marry_main")},
            {2502, new NPCommonAssetPathInfo("gui/child_marry.unity3d", "win_pop_Marry_leaderboard")},
            {2503, new NPCommonAssetPathInfo("gui/child_marry.unity3d", "win_pop_Marry_leaderboard_student_detail")},
            {2504, new NPCommonAssetPathInfo("gui/child_marry.unity3d", "win_pop_Marry_request")},
            {2505, new NPCommonAssetPathInfo("gui/child_marry.unity3d", "win_pop_Marry_succ")},
            {2507, new NPCommonAssetPathInfo("gui/child_marry.unity3d", "win_pop_Marry_union")},
            {2508, new NPCommonAssetPathInfo("gui/child_marry.unity3d", "page_Marry_union_designate")},
            {2509, new NPCommonAssetPathInfo("gui/child_marry.unity3d", "page_Marry_union_server")},
            {2510, new NPCommonAssetPathInfo("gui/child_marry.unity3d", "win_pop_Marry_child_select")},
            {2511, new NPCommonAssetPathInfo("gui/child_marry.unity3d", "page_unmarry")},
            {2512, new NPCommonAssetPathInfo("gui/child_marry.unity3d", "page_married")},
            {2521, new NPCommonAssetPathInfo("gui/child_marry.unity3d", "win_pop_Marry_child_upper_limit_set")},
            #endregion

            #region 好友 2600~2699
             {2601, new NPCommonAssetPathInfo("gui/friend.unity3d", "win_friend_main")},
             {2602, new NPCommonAssetPathInfo("gui/friend.unity3d", "win_add_friend_main")},
             {2603, new NPCommonAssetPathInfo("gui/friend.unity3d", "win_apply_friend_main")},
             {2604, new NPCommonAssetPathInfo("gui/friend.unity3d", "prefab_friend_more")},


	        #endregion

            #region 招募 2700~2799
            
            {2700, new NPCommonAssetPathInfo("gui/recruit.unity3d", "win_recruit_main")},//招募-主界面
            {2701, new NPCommonAssetPathInfo("gui/recruit.unity3d", "win_recruit_hero_main")},//招募-兑换伙伴界面
            {2702, new NPCommonAssetPathInfo("gui/recruit.unity3d", "win_recruit_consort_main")},//招募-兑换家人界面
            {2703, new NPCommonAssetPathInfo("gui/recruit.unity3d", "win_recruit_set_1_main")},//招募-第三类兑换界面
            {2704, new NPCommonAssetPathInfo("gui/recruit.unity3d", "win_recruit_hero_info")},//招募-伙伴兑换详情界面
            {2705, new NPCommonAssetPathInfo("gui/recruit.unity3d", "win_recruit_consort_info")},//招募-家人兑换详情界面

            #endregion

            #region 上浮提示 2800-2899

            {2800, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "center_tips_item_text")},
            {2801, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "center_tips_item_text_num")},
            {2802, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "center_tips_item_icon_text")},
            {2805, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "center_tips_item_icon_text_text_small")},
            {2806, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "win_power_up_tip")},
            {2808, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "prefab_center_tips_rank_L")},
            {2834, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "win_MarsPowerUp_tip")},

            #endregion
                
            #region 宴会 2900~2999
            {2901, new NPCommonAssetPathInfo("gui/dinner.unity3d", "win_dinner_main")},
            {2902, new NPCommonAssetPathInfo("gui/dinner.unity3d", "win_dinner_host_select_type")},
            {2903, new NPCommonAssetPathInfo("gui/dinner.unity3d", "win_dinner_host_main")},
            {2904, new NPCommonAssetPathInfo("gui/dinner.unity3d", "win_dinner_host_end_result")},
            {2905, new NPCommonAssetPathInfo("gui/dinner.unity3d", "win_dinner_invite_main")},
            {2906, new NPCommonAssetPathInfo("gui/dinner.unity3d", "page_dinner_invite_recommend")},
            {2907, new NPCommonAssetPathInfo("gui/dinner.unity3d", "page_dinner_invite_friend")},
            {2908, new NPCommonAssetPathInfo("gui/dinner.unity3d", "page_dinner_invite_guild")},
            {2909, new NPCommonAssetPathInfo("gui/dinner.unity3d", "win_dinner_join_way")},
            {2910, new NPCommonAssetPathInfo("gui/dinner.unity3d", "win_dinner_join_way_tip")},
            {2911, new NPCommonAssetPathInfo("gui/dinner.unity3d", "win_dinner_join_reward")},
            {2912, new NPCommonAssetPathInfo("gui/dinner.unity3d", "win_dinner_join_records")},
            {2913, new NPCommonAssetPathInfo("gui/dinner.unity3d", "win_dinner_host_player_follow")},
            {2914, new NPCommonAssetPathInfo("gui/dinner.unity3d", "win_dinner_rank")},
            {2915, new NPCommonAssetPathInfo("gui/dinner.unity3d", "win_dinner_host_records")},
            {2916, new NPCommonAssetPathInfo("gui/dinner.unity3d", "page_dinner_host_records_past")},
            {2917, new NPCommonAssetPathInfo("gui/dinner.unity3d", "page_dinner_host_records_reciprocal")},
            {2918, new NPCommonAssetPathInfo("gui/dinner.unity3d", "win_dinner_attend")},
            {2919, new NPCommonAssetPathInfo("gui/dinner.unity3d", "prefab_dinner_host_select_props_item")},
            {2920, new NPCommonAssetPathInfo("gui/dinner.unity3d", "prefab_dinner_host_select_consort_item")},
            {2921, new NPCommonAssetPathInfo("gui/dinner.unity3d", "prefab_dinner_host_select_celebration_item")},
            {2922, new NPCommonAssetPathInfo("gui/dinner.unity3d", "win_dinner_invitation_received")},
            {2923, new NPCommonAssetPathInfo("gui/dinner.unity3d", "prefab_dinner_host_select_high_props_item")},
            {2924, new NPCommonAssetPathInfo("gui/dinner.unity3d", "win_dinner_detail_log_popup")}, //会-宴会参与详情弹窗
            {2925, new NPCommonAssetPathInfo("gui/dinner.unity3d", "prefab_dinner_host_select_consort_title_item")}, //bar
            {2926, new NPCommonAssetPathInfo("gui/dinner.unity3d", "win_dinner_host_success")},
            {2927, new NPCommonAssetPathInfo("gui/dinner.unity3d", "win_dinner_invitation_received_child")},
            {2928, new NPCommonAssetPathInfo("gui/dinner.unity3d", "prefab_dinner_host_select_child_item")},

            #endregion

            #region 成就 3000~3099

            {3000, new NPCommonAssetPathInfo("gui/achieve.unity3d", "win_achieve_main")},
            {3002, new NPCommonAssetPathInfo("gui/achieve.unity3d", "win_achieve_detail")},
            {3005, new NPCommonAssetPathInfo("gui/achieve.unity3d", "prefab_achieve_page")},
            {3006, new NPCommonAssetPathInfo("gui/achieve.unity3d", "win_achieve_preview")},

	        #endregion

            #region 商店 3100~3199
            {3100, new NPCommonAssetPathInfo("gui/shop.unity3d", "win_shop_main")},
            {3101, new NPCommonAssetPathInfo("gui/shop.unity3d", "win_shop_no_tab_main")},
            {3102, new NPCommonAssetPathInfo("gui/shop.unity3d", "win_shop_pop_purchase")},
            {3103, new NPCommonAssetPathInfo("gui/shop.unity3d", "page_shop_general_1")},
            {3104, new NPCommonAssetPathInfo("gui/shop.unity3d", "page_shop_general_2")},
            {3111, new NPCommonAssetPathInfo("gui/shop.unity3d", "prefab_shop_discount_10%")},
            {3112, new NPCommonAssetPathInfo("gui/shop.unity3d", "prefab_shop_discount_20%")},
            {3113, new NPCommonAssetPathInfo("gui/shop.unity3d", "prefab_shop_discount_30%")},
            {3114, new NPCommonAssetPathInfo("gui/shop.unity3d", "prefab_shop_discount_40%")},
            {3115, new NPCommonAssetPathInfo("gui/shop.unity3d", "prefab_shop_discount_50%")},
            {3116, new NPCommonAssetPathInfo("gui/shop.unity3d", "prefab_shop_discount_60%")},
            {3117, new NPCommonAssetPathInfo("gui/shop.unity3d", "prefab_shop_discount_70%")},
            {3118, new NPCommonAssetPathInfo("gui/shop.unity3d", "prefab_shop_discount_80%")},
            {3119, new NPCommonAssetPathInfo("gui/shop.unity3d", "prefab_shop_discount_90%")},
            {3121, new NPCommonAssetPathInfo("gui/shop.unity3d", "prefab_shop_recommended")},
            #endregion

            #region 邮件 3200~3299

            {3200, new NPCommonAssetPathInfo("gui/mail.unity3d", "win_mail_main")},
            {3201, new NPCommonAssetPathInfo("gui/mail.unity3d", "win_mail_detail")},
            {3202, new NPCommonAssetPathInfo("gui/mail.unity3d", "win_mail_detail_expired")},
            {3203, new NPCommonAssetPathInfo("gui/mail.unity3d", "win_mail_detail_birthday")},
            {3204, new NPCommonAssetPathInfo("gui/mail.unity3d", "prefab_mail_list_read_item")},
            {3205, new NPCommonAssetPathInfo("gui/mail.unity3d", "prefab_mail_list_gift_item")},
            {3206, new NPCommonAssetPathInfo("gui/mail.unity3d", "prefab_mail_list_birthday_item")},
            {3207, new NPCommonAssetPathInfo("gui/mail.unity3d", "prefab_mail_list_expired_item")},
            {3208, new NPCommonAssetPathInfo("gui/mail.unity3d", "prefab_mail_list_activitygift_item")},
            {3209, new NPCommonAssetPathInfo("gui/mail.unity3d", "prefab_mail_list_hero_item")},
            {3210, new NPCommonAssetPathInfo("gui/mail.unity3d", "win_mail_detail_hero")},

            #endregion

            #region 每日任务 3300~3399

            {3300, new NPCommonAssetPathInfo("gui/daily_quest.unity3d", "win_daily_quest_main")},
            {3301, new NPCommonAssetPathInfo("gui/daily_quest.unity3d", "win_daily_quest_reward_preview")},

            #endregion

            #region 政务 3400~3499
            
            {3400, new NPCommonAssetPathInfo("gui/anecdote.unity3d", "win_anecdote_choice_entry_special")},
            {3401, new NPCommonAssetPathInfo("gui/anecdote.unity3d", "win_anecdote_choice_entry_normal")},
            {3402, new NPCommonAssetPathInfo("gui/anecdote.unity3d", "win_anecdote_choice_entry_earnings")},
            {3403, new NPCommonAssetPathInfo("gui/anecdote.unity3d", "win_anecdote_choice_entry_done")},
            {3404, new NPCommonAssetPathInfo("gui/anecdote.unity3d", "win_anecdote_choice_event")},
            {3405, new NPCommonAssetPathInfo("gui/anecdote.unity3d", "win_anecdote_choice_event_result")},
            {3406, new NPCommonAssetPathInfo("gui/anecdote.unity3d", "win_anecdote_earnings_event")},
            {3407, new NPCommonAssetPathInfo("gui/anecdote.unity3d", "win_anecdote_special_event_start")},
            {3408, new NPCommonAssetPathInfo("gui/anecdote.unity3d", "win_anecdote_special_event_continue")},
            {3409, new NPCommonAssetPathInfo("gui/anecdote.unity3d", "win_anecdote_special_event_end")},

            #endregion

            #region 签到 3500~3599
            {3501, new NPCommonAssetPathInfo("gui/daily_check.unity3d", "win_daily_check_page")},
            {3502, new NPCommonAssetPathInfo("gui/daily_check.unity3d", "win_daily_check_main")},
            {3503, new NPCommonAssetPathInfo("gui/daily_check.unity3d", "win_daily_check_get_reward")},

            
            #endregion

            #region 游历 3600~3699

            {3601, new NPCommonAssetPathInfo("gui/travel.unity3d", "win_travel_main")},//游历主窗口
            {3602, new NPCommonAssetPathInfo("gui/travel.unity3d", "win_pop_travel_all_consort_list")},//游历妃子列表
            
            {3603, new NPCommonAssetPathInfo("gui/travel.unity3d", "win_travel_common_result")},//通用事件结果弹窗
            
            {3604, new NPCommonAssetPathInfo("gui/travel.unity3d", "win_pop_travel_consort_list")},//邀约事件选择妃子窗口
            {3605, new NPCommonAssetPathInfo("gui/travel.unity3d", "win_travel_result_invite")},//邀约事件结果窗口
            
            {3606, new NPCommonAssetPathInfo("gui/travel.unity3d", "win_travel_result_child")},//卷王事件结果窗口
            
            {3607, new NPCommonAssetPathInfo("gui/travel.unity3d", "win_travel_result_stamina")},//交换事件
            
            {3608, new NPCommonAssetPathInfo("gui/travel.unity3d", "win_pop_travel_hero_list")},//增加大臣实力事件窗口
            {3609, new NPCommonAssetPathInfo("gui/travel.unity3d", "win_travel_result_fellow")},//增加大臣实力事件结果窗口

            {3610, new NPCommonAssetPathInfo("gui/travel.unity3d", "win_travel_bar_consort_list")},//妃子酒馆事件选择妃子窗口
            {3611, new NPCommonAssetPathInfo("gui/travel.unity3d", "win_travel_bond_normal")},//妃子好感度增加提示窗口
            {3612, new NPCommonAssetPathInfo("gui/travel.unity3d", "win_travel_consort_event_result")},//妃子事件结果窗口

            {3613, new NPCommonAssetPathInfo("gui/travel.unity3d", "win_travel_bond_stageup")},//妃子好感度阶段变化提示窗口

            {3614, new NPCommonAssetPathInfo("gui/travel.unity3d", "prefab_travel_consort_stamina_cost_item")},//妃子交换事件对话 - 交换按钮
            {3615, new NPCommonAssetPathInfo("gui/travel.unity3d", "prefab_travel_consort_stamina_item")},//妃子交换事件对话 - 不交换按钮
            
            {3616, new NPCommonAssetPathInfo("gui/travel.unity3d", "win_travel_onekey")},//妃子一键游历结果弹窗
            {3617, new NPCommonAssetPathInfo("gui/travel.unity3d", "win_travel_skip")},//游历跳过窗口
            
            {3618, new NPCommonAssetPathInfo("gui/travel.unity3d", "win_pop_traveling")},//游历过场表现窗口
            
            {3632, new NPCommonAssetPathInfo("gui/travel.unity3d", "win_travel_bg_show")},//游历-事件背景放置窗口

            {3633, new NPCommonAssetPathInfo("gui/travel.unity3d", "win_travel_gamble_event_bet")},//游历-押注选择钻石数量
            {3634, new NPCommonAssetPathInfo("gui/travel.unity3d", "win_travel_gamble_event_choose")}, // 游历-押注选择颜色
            {3635, new NPCommonAssetPathInfo("gui/travel.unity3d", "win_travel_gamble_event_result")}, //游历-博彩结果弹窗
            
            #endregion

            #region 骑士推荐 3700~3799
            
            {3700, new NPCommonAssetPathInfo("gui/hero_recommend.unity3d", "win_hero_recommend_details")},
            {3701, new NPCommonAssetPathInfo("gui/hero_recommend.unity3d", "prefab_hero_recommend_entrance")},

            #endregion

            #region 创角 3800~3899

            {3800, new NPCommonAssetPathInfo("gui/create.unity3d", "win_create_main")},

            #endregion

            #region 限时冲榜 3900~3999

            {3900, new NPCommonAssetPathInfo("gui/rank_rush.unity3d", "win_limited_activity_main")},
            {3901, new NPCommonAssetPathInfo("gui/rank_rush.unity3d", "win_rank_rush_main")},
            {3902, new NPCommonAssetPathInfo("gui/rank_rush.unity3d", "win_rank_rush_details")},
            {3903, new NPCommonAssetPathInfo("gui/rank_rush.unity3d", "page_rank_rush_details_award_list")},
            {3904, new NPCommonAssetPathInfo("gui/rank_rush.unity3d", "page_rank_rush_details_rank_list")},
            {3905, new NPCommonAssetPathInfo("gui/rank_rush.unity3d", "win_rank_upgrade_access")},
            {3906, new NPCommonAssetPathInfo("gui/rank_rush.unity3d", "win_step_reward_main")},	//限时任务-主界面
            {3908, new NPCommonAssetPathInfo("gui/rank_rush.unity3d", "win_rank_rush_multiple_details")},	//多个冲榜详情界面
            {3909, new NPCommonAssetPathInfo("gui/rank_rush.unity3d", "win_rank_rush_guild_details")},//联盟冲榜详情界面
            {3912, new NPCommonAssetPathInfo("gui/rank_rush.unity3d", "prefab_rank_rush_guild_info_tip")},//联盟冲榜成员积分详情界面
            #endregion

            #region 跑马灯 4000~4099

            {4000, new NPCommonAssetPathInfo("gui/marquee.unity3d", "win_marquee_main")},

            #endregion

            #region 功能预告 4100~4199

            {4100, new NPCommonAssetPathInfo("gui/func_preview.unity3d", "page_func_preview")},
            {4101, new NPCommonAssetPathInfo("gui/func_preview.unity3d", "win_func_preview_put_up")},

            #endregion

            #region 排行榜

            
            {4301, new NPCommonAssetPathInfo("gui/rank.unity3d", "win_rank_details")},
            {4331, new NPCommonAssetPathInfo("gui/rank.unity3d", "win_rank_details_no_like")},


            #endregion

            #region 国力目标 4400-4499


            {4400, new NPCommonAssetPathInfo("gui/gdp_goals.unity3d", "win_GDP_goals_hero_tip")},
            {4401, new NPCommonAssetPathInfo("gui/gdp_goals.unity3d", "win_GDP_goals_consort_tip")},
            {4402, new NPCommonAssetPathInfo("gui/gdp_goals.unity3d", "win_GDP_goals_win")},
            {4410, new NPCommonAssetPathInfo("gui/gdp_goals.unity3d", "page_GDP_consort_info")},
            {4420, new NPCommonAssetPathInfo("gui/gdp_goals.unity3d", "page_GDP_hero_info")},

            #endregion
            #region 国力目标 4500-4599

            
            {4500, new NPCommonAssetPathInfo("gui/week_card.unity3d", "win_week_card_main")},
            {4501, new NPCommonAssetPathInfo("gui/week_card.unity3d", "win_week_card_offline_reward")},
            {4502, new NPCommonAssetPathInfo("gui/week_card.unity3d", "win_week_card_college_hero_choice")},
            {4503, new NPCommonAssetPathInfo("gui/week_card.unity3d", "win_week_card_appoint")},
            {4504, new NPCommonAssetPathInfo("gui/week_card.unity3d", "prefab_week_card_appoint_item_normal")},
            {4505, new NPCommonAssetPathInfo("gui/week_card.unity3d", "prefab_week_card_appoint_item_college")},
            {4506, new NPCommonAssetPathInfo("gui/week_card.unity3d", "win_week_card_appoint_choose_npc")},

            #endregion

            #region 问卷调查 4600~4699
            
            {4600, new NPCommonAssetPathInfo("gui/questionnaire.unity3d", "win_questionnaire")},

            #endregion

            #region 运营公告 4700~4799
            
            {4700, new NPCommonAssetPathInfo("gui/announcement.unity3d", "win_announcement")},

            #endregion

            #region avatar评分 4800~4899
            
            {4800, new NPCommonAssetPathInfo("gui/avatar_score.unity3d", "win_avatar_score_main")},
            {4801, new NPCommonAssetPathInfo("gui/avatar_score.unity3d", "win_avatar_score_award")},

            #endregion

            #region 联盟 4900~4999
            
            {4900, new NPCommonAssetPathInfo("gui/guild.unity3d", "win_guild_apply")},
            {4901, new NPCommonAssetPathInfo("gui/guild.unity3d", "win_guild_apply_create")},
            {4902, new NPCommonAssetPathInfo("gui/guild.unity3d", "win_guild_apply_create_flag")},
            {4903, new NPCommonAssetPathInfo("gui/guild.unity3d", "win_guild_main")},
            {4904, new NPCommonAssetPathInfo("gui/guild.unity3d", "win_guild_level_preview")},
            {4905, new NPCommonAssetPathInfo("gui/guild.unity3d", "win_guild_info")},
            {4906, new NPCommonAssetPathInfo("gui/guild.unity3d", "win_guild_revise_guild_name")},
            {4907, new NPCommonAssetPathInfo("gui/guild.unity3d", "win_guild_lay_off")},
            {4908, new NPCommonAssetPathInfo("gui/guild.unity3d", "win_guild_apply_list")},
            {4909, new NPCommonAssetPathInfo("gui/guild.unity3d", "win_guild_apply_rank_not_join")},
            {4910, new NPCommonAssetPathInfo("gui/guild.unity3d", "win_guild_apply_info")},
            {4911, new NPCommonAssetPathInfo("gui/guild.unity3d", "win_guild_member_list")},
            {4912, new NPCommonAssetPathInfo("gui/guild.unity3d", "win_guild_notice")},
            {4913, new NPCommonAssetPathInfo("gui/guild.unity3d", "win_guild_notice_list")},
            {4914, new NPCommonAssetPathInfo("gui/guild.unity3d", "win_guild_member_list_manage")},
            {4915, new NPCommonAssetPathInfo("gui/guild.unity3d", "win_guild_build")},
			{4916, new NPCommonAssetPathInfo("gui/guild.unity3d", "win_guild_rank")},
            
            {4917, new NPCommonAssetPathInfo("gui/guild.unity3d", "win_guild_build_main")},
            {4918, new NPCommonAssetPathInfo("gui/guild.unity3d", "win_guild_event_main")},
            {4919, new NPCommonAssetPathInfo("gui/guild.unity3d", "win_guild_event_main_reward_preview")},
            {4920, new NPCommonAssetPathInfo("gui/guild.unity3d", "win_guild_event_entrustment_record")},

            {4921, new NPCommonAssetPathInfo("gui/guild.unity3d", "win_guild_gift_main")},
            {4922, new NPCommonAssetPathInfo("gui/guild.unity3d", "win_guild_gift_list")},
            {4923, new NPCommonAssetPathInfo("gui/guild.unity3d", "win_guild_gift_record")},

            {4924, new NPCommonAssetPathInfo("gui/guild.unity3d", "win_guild_station_main")},
            {4925, new NPCommonAssetPathInfo("gui/guild.unity3d", "win_guild_station_choose_hero")},
            
            {4926, new NPCommonAssetPathInfo("gui/guild.unity3d", "win_guild_build_add_tip")},
            {4927, new NPCommonAssetPathInfo("gui/guild.unity3d", "prefab_guild_build_box_preview_tip")},
            {4928, new NPCommonAssetPathInfo("gui/guild.unity3d", "win_guild_build_extra_tip")},
            {4929, new NPCommonAssetPathInfo("gui/guild.unity3d", "win_guild_build_item_special_tip")},
            {4930, new NPCommonAssetPathInfo("gui/guild.unity3d", "win_guild_history")},
            {4931, new NPCommonAssetPathInfo("gui/guild.unity3d", "prefab_guild_history_time_item")},

            {4932, new NPCommonAssetPathInfo("gui/guild.unity3d", "win_guild_cooperate_main")},
            {4933, new NPCommonAssetPathInfo("gui/guild.unity3d", "win_guild_cooperate_area")},
            {4934, new NPCommonAssetPathInfo("gui/guild.unity3d", "win_guild_cooperate_reward_building")},
            {4935, new NPCommonAssetPathInfo("gui/guild.unity3d", "win_guild_cooperate_attribute_building")},
            {4936, new NPCommonAssetPathInfo("gui/guild.unity3d", "win_guild_cooperate_attribute_building_success")},
            {4937, new NPCommonAssetPathInfo("gui/guild.unity3d", "win_guild_cooperate_history")},
            {4938, new NPCommonAssetPathInfo("gui/guild.unity3d", "win_guild_cooperate_rank")},
            
            {4950, new NPCommonAssetPathInfo("gui/guild.unity3d", "win_guild_assist")}, //公会互助
            #endregion

            #region 阶段目标 5000~5099
            {5001, new NPCommonAssetPathInfo("gui/stage_goal.unity3d", "page_stage_goal_overview")},
            {5002, new NPCommonAssetPathInfo("gui/stage_goal.unity3d", "page_stage_goal_task")},
            {5003, new NPCommonAssetPathInfo("gui/stage_goal.unity3d", "win_stage_goal_big_stage_unlock")},
            {5004, new NPCommonAssetPathInfo("gui/stage_goal.unity3d", "win_stage_goal_small_stage_unlock")},
            {5005, new NPCommonAssetPathInfo("gui/stage_goal.unity3d", "prefab_stage_goal_overview_current")},
            {5006, new NPCommonAssetPathInfo("gui/stage_goal.unity3d", "prefab_stage_goal_overview_end")},
            {5008, new NPCommonAssetPathInfo("gui/stage_goal.unity3d", "win_stage_goal_task_info")},
            {5009, new NPCommonAssetPathInfo("gui/stage_goal.unity3d", "win_stage_goal_big_stage_complete")},
            {5010, new NPCommonAssetPathInfo("gui/stage_goal.unity3d", "win_stage_goal_small_stage_complete")},
            {5016, new NPCommonAssetPathInfo("gui/stage_goal.unity3d", "win_stage_goal_rank")},
            {5017, new NPCommonAssetPathInfo("gui/stage_goal.unity3d", "win_stage_goal_rank_info")},
            #endregion

            #region 竞技场 5200~5299

            {5200, new NPCommonAssetPathInfo("gui/arena.unity3d", "win_arena_main")},
            {5201, new NPCommonAssetPathInfo("gui/arena.unity3d", "win_arena_trading_post_upgrade")},
            {5202, new NPCommonAssetPathInfo("gui/arena.unity3d", "win_arena_trading_post_upgrade_success")},
            {5203, new NPCommonAssetPathInfo("gui/arena.unity3d", "win_arena_rank")},
            {5204, new NPCommonAssetPathInfo("gui/arena.unity3d", "win_arena_main_add_battle_num")},
            {5205, new NPCommonAssetPathInfo("gui/arena.unity3d", "win_arena_report")},
            {5206, new NPCommonAssetPathInfo("gui/arena.unity3d", "win_arena_setting")},
            {5207, new NPCommonAssetPathInfo("gui/arena.unity3d", "win_arena_battle_preparing")},
            {5208, new NPCommonAssetPathInfo("gui/arena.unity3d", "win_arena_battle_choose_hero")},
            {5209, new NPCommonAssetPathInfo("gui/arena.unity3d", "win_arena_battle")},
            {5210, new NPCommonAssetPathInfo("gui/arena.unity3d", "win_arena_auto_battle_setting")},
            {5211, new NPCommonAssetPathInfo("gui/arena.unity3d", "win_arena_battle_buy_buff")},
            {5212, new NPCommonAssetPathInfo("gui/arena.unity3d", "win_arena_battle_show")},
            {5213, new NPCommonAssetPathInfo("gui/arena.unity3d", "win_arena_battle_result")},
            {5214, new NPCommonAssetPathInfo("gui/arena.unity3d", "win_arena_battle_result_win")},
            {5215, new NPCommonAssetPathInfo("gui/arena.unity3d", "win_arena_battle_result_fail")},
            {5216, new NPCommonAssetPathInfo("gui/arena.unity3d", "win_arena_battle_result_winning_streak_reward")},
            {5217, new NPCommonAssetPathInfo("gui/arena.unity3d", "win_arena_battle_hero_attribute_up")},
            {5218, new NPCommonAssetPathInfo("gui/arena.unity3d", "win_arena_trading_post_gold_info")},
            {5219, new NPCommonAssetPathInfo("gui/arena.unity3d", "win_arena_battle_choose_hero_info")},

            #endregion
            
            #region 爬塔 5300~5399

            {5300, new NPCommonAssetPathInfo("gui/tower.unity3d", "win_tower_main")},//爬塔-主界面
            {5301, new NPCommonAssetPathInfo("gui/tower.unity3d", "win_tower_chapter")},//爬塔-章节列表详情
            {5302, new NPCommonAssetPathInfo("gui/tower.unity3d", "win_tower_challenge")},//爬塔-可挑战关卡界面
            {5303, new NPCommonAssetPathInfo("gui/tower.unity3d", "win_tower_battle")},//爬塔-战斗界面
            {5304, new NPCommonAssetPathInfo("gui/tower.unity3d", "win_Tower_battle_suc")},//爬塔-战斗胜利
            {5305, new NPCommonAssetPathInfo("gui/tower.unity3d", "win_tower_battle_fail")},//爬塔-战斗失败
            {5306, new NPCommonAssetPathInfo("gui/tower.unity3d", "win_tower_log")},//爬塔-战报弹窗
            {5307, new NPCommonAssetPathInfo("gui/tower.unity3d", "prefab_follow_tower_chapter_name")},//爬塔-主界面章节跟随UI
            {5308, new NPCommonAssetPathInfo("gui/tower.unity3d", "prefab_tower_battle_bubbles_L")},       //爬塔-支援气泡-左
            {5309, new NPCommonAssetPathInfo("gui/tower.unity3d", "prefab_tower_battle_bubbles_R")},       //爬塔-支援气泡-右
            {5310, new NPCommonAssetPathInfo("gui/tower.unity3d", "win_tower_battle_cutscene")},       //爬塔-过场弹窗
            {5311, new NPCommonAssetPathInfo("gui/tower.unity3d", "prefab_tower_battle_power_num")},       //爬塔-攻击数值
            {5312, new NPCommonAssetPathInfo("gui/tower.unity3d", "win_tower_unlock_chapter")},        //爬塔-章节解锁
            {5313, new NPCommonAssetPathInfo("gui/tower.unity3d", "win_tower_detail_tip")},        //爬塔-当前爬塔状态详情
            {5314, new NPCommonAssetPathInfo("gui/tower.unity3d", "win_tower_battle_PVE")},        //爬塔-战斗界面-PVE
            {5315, new NPCommonAssetPathInfo("gui/tower.unity3d", "win_tower_battle_PVP")},        //爬塔-战斗界面-PVP
            {5316, new NPCommonAssetPathInfo("gui/tower.unity3d", "win_tower_research_popup")},        //爬塔-科技弹窗
            {5317, new NPCommonAssetPathInfo("gui/tower.unity3d", "win_tower_research_reward_tip")},        //爬塔-科技奖励tip
            {5318, new NPCommonAssetPathInfo("gui/tower.unity3d", "prefab_tower_chapter_item_info_tip")},        //爬塔-科技奖励tip
            #endregion

            #region 午间副本 5400~
            {5400, new NPCommonAssetPathInfo("gui/midday_dungeon.unity3d", "win_midday_dungeon_enter")},//副本主界面	 
            {5401, new NPCommonAssetPathInfo("gui/midday_dungeon.unity3d", "win_midday_dungeon_battle")},//午间副本-战斗界面
            {5402, new NPCommonAssetPathInfo("gui/midday_dungeon.unity3d", "win_midday_dungeon_box_get")},//午间副本-宝箱获得
            {5403, new NPCommonAssetPathInfo("gui/midday_dungeon.unity3d", "win_midday_dungeon_rank_details")},//午间副本-积分排名
            {5404, new NPCommonAssetPathInfo("gui/midday_dungeon.unity3d", "win_midday_dungeon_result")},//午间副本-战斗结算
            {5405, new NPCommonAssetPathInfo("gui/midday_dungeon.unity3d", "win_midday_dungeon_select_hero")},//午间副本-伙伴选择
            {5406, new NPCommonAssetPathInfo("gui/midday_dungeon.unity3d", "win_pop_midday_dungeon_box_record")},//午间副本-宝箱详情
            
            #endregion

            
            #region 晚间活动 5500

            {5500, new NPCommonAssetPathInfo("gui/evening_dungeon.unity3d", "win_evening_dungeon_battle")},//晚间副本-战斗界面
            {5501, new NPCommonAssetPathInfo("gui/evening_dungeon.unity3d", "win_evening_dungeon_killed")},//晚间副本-最后一击
            {5502, new NPCommonAssetPathInfo("gui/evening_dungeon.unity3d", "win_pop_evening_dungeon_rank")},//晚间副本-排行窗口
            {5503, new NPCommonAssetPathInfo("gui/evening_dungeon.unity3d", "win_pop_evening_dungeon_select_hero")},//晚间副本-伙伴选择
            {5504, new NPCommonAssetPathInfo("gui/evening_dungeon.unity3d", "win_evening_dungeon_rank_details")},//晚间副本-排行详情界面
            {5505, new NPCommonAssetPathInfo("gui/evening_dungeon.unity3d", "win_evening_dungeon_result")},//晚间副本-战斗结算
            {5507, new NPCommonAssetPathInfo("gui/evening_dungeon.unity3d", "win_pop_evening_dungeon_last_hit")},//晚间副本-尾刀记录弹窗
            {5508, new NPCommonAssetPathInfo("gui/evening_dungeon.unity3d", "win_evening_dungeon_enter")},//晚间副本-主界面
            {5509, new NPCommonAssetPathInfo("gui/evening_dungeon.unity3d", "win_dungeon_main")},//副本活动主界面
            {5510, new NPCommonAssetPathInfo("gui/evening_dungeon.unity3d", "win_dungeon_rank_main")},//副本活动排行榜界面

            #endregion

            #region 七日登录 5600

            
            {5600, new NPCommonAssetPathInfo("gui/seven_days.unity3d", "win_seven_days_main")}, //七日登录-主界面

            #endregion

            #region 赚速目标 5700
            {5700, new NPCommonAssetPathInfo("gui/earning_goal.unity3d", "win_earning_goal_main")},// 赚速目标-主界面
            {5701, new NPCommonAssetPathInfo("gui/earning_goal.unity3d", "win_earning_goal_reward_popup")},// 赚速目标-全民弹窗
            {5702, new NPCommonAssetPathInfo("gui/earning_goal.unity3d", "page_earning_goal_reward_global")},// 赚速目标-全民弹窗-全民奖励
            {5703, new NPCommonAssetPathInfo("gui/earning_goal.unity3d", "page_earning_goal_reward_honor")},// 赚速目标-全民弹窗-荣誉奖励
            {5704, new NPCommonAssetPathInfo("gui/earning_goal.unity3d", "win_earning_goal_self_reward")},// 赚速目标-个人奖励
            {5705, new NPCommonAssetPathInfo("gui/earning_goal.unity3d", "page_earning_goal_self_reward")},// 赚速目标-个人奖励 (页面形式)
            #endregion
            
            #region 七日目标 5800
            
            {5800, new NPCommonAssetPathInfo("gui/seven_day_goals.unity3d", "win_seven_day_goals_main")},
            {5801, new NPCommonAssetPathInfo("gui/seven_day_goals.unity3d", "win_seven_day_goals_daily_task")},
            {5802, new NPCommonAssetPathInfo("gui/seven_day_goals.unity3d", "win_seven_day_goals_daily_packages")},
            
            #endregion

            #region 活动通用 6000
            
            {6000, new NPCommonAssetPathInfo("gui/activity_common.unity3d", "win_activity_exchange_shop")},
            {6001, new NPCommonAssetPathInfo("gui/activity_common.unity3d", "win_activity_gift_pack_shop")},
            {6002, new NPCommonAssetPathInfo("gui/activity_common.unity3d", "win_activity_gift_pack_buy")},
            {6005, new NPCommonAssetPathInfo("gui/activity_common.unity3d", "win_activity_achieve_main")},
            {6006, new NPCommonAssetPathInfo("gui/activity_common.unity3d", "win_pop_activity_achieve_detail")},
            
            #endregion

            #region 情人互动 6200
            {6200, new NPCommonAssetPathInfo("gui/consort_chat.unity3d", "win_consort_chat_main")},//情人互动-主界面
            {6201, new NPCommonAssetPathInfo("gui/consort_chat.unity3d", "page_consort_chat_message")},//情人互动-聊天页签
            {6202, new NPCommonAssetPathInfo("gui/consort_chat.unity3d", "page_consort_chat_monents")},//情人互动-朋友圈页签
            {6203, new NPCommonAssetPathInfo("gui/consort_chat.unity3d", "win_consort_chat_message_details")},//情人互动-聊天详情界面
            {6204, new NPCommonAssetPathInfo("gui/consort_chat.unity3d", "win_consort_chat_picture_details")},//情人互动-图片详情查看界面
            {6205, new NPCommonAssetPathInfo("gui/consort_chat.unity3d", "prefab_consort_chat_msg_item_text_L")},//情人互动-聊天气泡-对方
            {6206, new NPCommonAssetPathInfo("gui/consort_chat.unity3d", "prefab_consort_chat_msg_item_text_R")},//情人互动-聊天气泡-我方
            {6207, new NPCommonAssetPathInfo("gui/consort_chat.unity3d", "prefab_consort_chat_msg_item_image_L")},//情人互动-聊天气泡-对方-图片
            {6208, new NPCommonAssetPathInfo("gui/consort_chat.unity3d", "prefab_consort_chat_reward_item")},//情人互动-聊天气泡-奖励
            {6209, new NPCommonAssetPathInfo("gui/consort_chat.unity3d", "prefab_consort_chat_monents_item")},//情人互动-朋友圈item
            {6210, new NPCommonAssetPathInfo("gui/consort_chat.unity3d", "prefab_consort_chat_img_group")},//情人互动-朋友圈item
            {6212, new NPCommonAssetPathInfo("gui/consort_chat.unity3d", "win_consort_chat_new_moments_list")},//情人朋友圈-互动列表
            {6213, new NPCommonAssetPathInfo("gui/consort_chat.unity3d", "win_consort_chat_new_moments_detail")},//情人朋友圈-朋友圈详情

            

            #endregion

            #region 旅店 6400

            {6400, new NPCommonAssetPathInfo("gui/inn.unity3d", "win_inn_main")},//旅店主界面
            {6401, new NPCommonAssetPathInfo("gui/inn.unity3d", "win_pop_inn_earnings_details")},//收益详情弹窗
            {6402, new NPCommonAssetPathInfo("gui/inn.unity3d", "win_pop_inn_guest_list")},//客人列表
            {6403, new NPCommonAssetPathInfo("gui/inn.unity3d", "prefab_inn_normal_guests_page")},//普通客人页签
            {6404, new NPCommonAssetPathInfo("gui/inn.unity3d", "prefab_inn_special_guests_page")},//特殊客人页签
            {6405, new NPCommonAssetPathInfo("gui/inn.unity3d", "win_inn_guest_get")},//客人获得弹窗
            {6406, new NPCommonAssetPathInfo("gui/inn.unity3d", "win_pop_inn_normal_guest_details")},//客人详情弹窗
            {6407, new NPCommonAssetPathInfo("gui/inn.unity3d", "win_pop_inn_medal")},//旅店奖牌弹窗
            {6409, new NPCommonAssetPathInfo("gui/inn.unity3d", "win_pop_inn_menu")},//菜单列表弹窗
            {6410, new NPCommonAssetPathInfo("gui/inn.unity3d", "prefab_inn_menu_page")},//菜品列表页签
            {6411, new NPCommonAssetPathInfo("gui/inn.unity3d", "prefab_inn_recipes_page")},//研制菜品页签
            {6412, new NPCommonAssetPathInfo("gui/inn.unity3d", "win_pop_inn_menu_details")},//菜品详情弹窗
            {6413, new NPCommonAssetPathInfo("gui/inn.unity3d", "win_pop_inn_recipes_details")},//菜品未解锁详情弹窗
            {6414, new NPCommonAssetPathInfo("gui/inn.unity3d", "win_pop_inn_recipes_get")},//菜品研制成功弹窗
            {6415, new NPCommonAssetPathInfo("gui/inn.unity3d", "win_pop_inn_recipes_get_cutscene")},//菜品研制过程banner
            {6416, new NPCommonAssetPathInfo("gui/inn.unity3d", "win_pop_inn_rating")},//旅店等级弹窗
            {6417, new NPCommonAssetPathInfo("gui/inn.unity3d", "win_pop_inn_rating_info_list")},//旅店等级预览弹窗
            {6418, new NPCommonAssetPathInfo("gui/inn.unity3d", "win_pop_inn_station_main")},//旅店设施列表
            {6419, new NPCommonAssetPathInfo("gui/inn.unity3d", "win_pop_inn_station_details")},//旅店设施详情
            {6420, new NPCommonAssetPathInfo("gui/inn.unity3d", "win_pop_inn_station_get")},//旅店设施建造成功弹窗
            {6421, new NPCommonAssetPathInfo("gui/inn.unity3d", "win_pop_inn_station_upgrade_succ")},//旅店设施升级成功弹窗
            {6422, new NPCommonAssetPathInfo("gui/inn.unity3d", "win_inn_treasure_main_list")},//旅店珍宝列表界面
            {6423, new NPCommonAssetPathInfo("gui/inn.unity3d", "win_inn_treasure_details")},//旅店珍宝详情界面
            {6424, new NPCommonAssetPathInfo("gui/inn.unity3d", "win_pop_inn_treasure_get")},//旅店珍宝解锁成功弹窗
            {6425, new NPCommonAssetPathInfo("gui/inn.unity3d", "win_pop_inn_treasure_story")},//旅店珍宝故事详情弹窗
            {6427, new NPCommonAssetPathInfo("gui/inn.unity3d", "win_pop_inn_station_build")},
            {6428, new NPCommonAssetPathInfo("gui/inn.unity3d", "win_pop_inn_rating_upgrade_succ")},
            {6429, new NPCommonAssetPathInfo("gui/inn.unity3d", "win_pop_inn_special_guest_details")},
            {6430, new NPCommonAssetPathInfo("gui/inn.unity3d", "win_inn_hud")},
            {6431, new NPCommonAssetPathInfo("gui/inn.unity3d", "win_inn_station_build_btn")},
            {6432, new NPCommonAssetPathInfo("gui/inn.unity3d", "win_inn_guest_banner")}, //特殊客人提示banner
            {6433, new NPCommonAssetPathInfo("gui/inn.unity3d", "win_inn_guest_cheer")}, //特殊客人欢呼界面
            {6434, new NPCommonAssetPathInfo("gui/inn.unity3d", "win_inn_treasure_banner")}, //获得珍宝的提示
            {6435, new NPCommonAssetPathInfo("gui/inn.unity3d", "win_inn_special_guest_hud")}, //特殊客人头顶的 hud
            {6436, new NPCommonAssetPathInfo("gui/inn.unity3d", "win_inn_normal_guest_get")},
            {6437, new NPCommonAssetPathInfo("gui/inn.unity3d", "win_inn_guest_get_follow_tip")},
            {6438, new NPCommonAssetPathInfo("gui/inn.unity3d", "win_inn_normal_guest_tip")},
            {6439, new NPCommonAssetPathInfo("gui/inn.unity3d", "prefab_inn_reward_tip")},
            {6440, new NPCommonAssetPathInfo("gui/inn.unity3d", "prefab_inn_guest_serve_follow")},
            {6441, new NPCommonAssetPathInfo("gui/inn.unity3d", "prefab_inn_cash_counter_follow")},

            #endregion

            #region 招聘体验 6500

            {6500, new NPCommonAssetPathInfo("gui/hire.unity3d", "win_hire_main")},//招聘体验-主界面

            #endregion

            #region 充值礼包 6700

            {6700, new NPCommonAssetPathInfo("gui/gift_pack.unity3d", "win_cash_gift_pack_main")},//充值礼包-主界面
            {6705, new NPCommonAssetPathInfo("gui/gift_pack.unity3d", "win_cash_gift_pack_daily_exchange_shop")},//充值礼包-每日商店兑换弹窗
            {6706, new NPCommonAssetPathInfo("gui/gift_pack.unity3d", "win_cash_gift_pack_cost_dialog")},//充值礼包-代金币购买二次确认弹窗
            {6751, new NPCommonAssetPathInfo("gui/gift_pack.unity3d", "win_gem_gift_pack_main")},//钻石礼包-主界面
            {6761, new NPCommonAssetPathInfo("gui/gift_pack.unity3d", "win_mars_cash_gift_pack_main")},//充值礼包-火星
            {6762, new NPCommonAssetPathInfo("gui/gift_pack.unity3d", "page_cash_gift_pack_single_construction_queue")},//建造队列礼包-火星
            {6763, new NPCommonAssetPathInfo("gui/gift_pack.unity3d", "page_cash_gift_pack_single_army_queue")},//行军队列礼包-火星
            
            #endregion
			
            #region 太空寻宝 6800~6899

            {6800, new NPCommonAssetPathInfo("gui/treasure_hunt.unity3d", "win_treasure_hunt_main")},//太空寻宝-主界面
            {6801, new NPCommonAssetPathInfo("gui/treasure_hunt.unity3d", "win_treasure_hunt_get_pop_info_tip")},//太空寻宝-能量产出详情弹窗
            {6802, new NPCommonAssetPathInfo("gui/treasure_hunt.unity3d", "win_treasure_hunt_upgrade")},//太空寻宝-太空舱等级详情弹窗
            {6803, new NPCommonAssetPathInfo("gui/treasure_hunt.unity3d", "win_treasure_hunt_laboratory_main")},//太空寻宝-实验室主界面
            {6804, new NPCommonAssetPathInfo("gui/treasure_hunt.unity3d", "win_treasure_hunt_laboratory_switch")},//太空寻宝-实验室切换
            {6805, new NPCommonAssetPathInfo("gui/treasure_hunt.unity3d", "win_treasure_hunt_specimen_room_main")},//太空寻宝-标本室主界面
            {6806, new NPCommonAssetPathInfo("gui/treasure_hunt.unity3d", "win_treasure_hunt_specimen_room_result")},//太空寻宝-标本室转化结果界面
            {6807, new NPCommonAssetPathInfo("gui/treasure_hunt.unity3d", "win_treasure_hunt_game_main")},//太空寻宝-游戏主界面
            {6808, new NPCommonAssetPathInfo("gui/treasure_hunt.unity3d", "win_treasure_hunt_game_place")},//太空寻宝-太空区域界面
            {6809, new NPCommonAssetPathInfo("gui/treasure_hunt.unity3d", "win_treasure_hunt_put_in_success")},//太空寻宝-实验室放入成功
            {6810, new NPCommonAssetPathInfo("gui/treasure_hunt.unity3d", "win_treasure_hunt_game_place_info")},//太空寻宝-太空区域信息
            {6811, new NPCommonAssetPathInfo("gui/treasure_hunt.unity3d", "win_treasure_hunt_game_main_replace_pop")},//太空寻宝-选择能源页面
            {6812, new NPCommonAssetPathInfo("gui/treasure_hunt.unity3d", "win_treasure_hunt_game_get_ore_success")},//太空寻宝-获得矿石弹窗
            {6813, new NPCommonAssetPathInfo("gui/treasure_hunt.unity3d", "win_treasure_hunt_game_get_strange_success")},//太空寻宝-获得奇异物品弹窗
            {6814, new NPCommonAssetPathInfo("gui/treasure_hunt.unity3d", "win_treasure_hunt_game_get_ore_most_success")},//太空寻宝-一键探索成功弹窗
            {6815, new NPCommonAssetPathInfo("gui/treasure_hunt.unity3d", "win_treasure_hunt_material")},//太空寻宝-材料室界面
            {6816, new NPCommonAssetPathInfo("gui/treasure_hunt.unity3d", "win_treasure_hunt_Illustrations_main")},//太空寻宝-图鉴主界面
            {6817, new NPCommonAssetPathInfo("gui/treasure_hunt.unity3d", "win_treasure_hunt_Illustrations_ore_main")},//太空寻宝-矿石图鉴
            {6818, new NPCommonAssetPathInfo("gui/treasure_hunt.unity3d", "win_treasure_hunt_Illustrations_strange_thing_main")},//太空寻宝-奇异图鉴
            {6819, new NPCommonAssetPathInfo("gui/treasure_hunt.unity3d", "win_treasure_hunt_Illustrations_ore_info")},//太空寻宝-矿石图鉴详细信息
            {6820, new NPCommonAssetPathInfo("gui/treasure_hunt.unity3d", "win_treasure_hunt_Illustrations_strange_thing_info")},//太空寻宝-奇物图鉴详细信息
            {6821, new NPCommonAssetPathInfo("gui/treasure_hunt.unity3d", "win_treasure_hunt_Illustrations_combination_main")},//太空寻宝-组合图鉴
            {6822, new NPCommonAssetPathInfo("gui/treasure_hunt.unity3d", "win_treasure_hunt_Illustrations_combination_info")},//太空寻宝-组合图鉴详情
            {6823, new NPCommonAssetPathInfo("gui/treasure_hunt.unity3d", "win_treasure_hunt_Illustrations_ore_weight")},//太空寻宝-矿石重量排行
            {6824, new NPCommonAssetPathInfo("gui/treasure_hunt.unity3d", "win_treasure_hunt_collect_achieve_main")},//太空寻宝-收集成就
            {6825, new NPCommonAssetPathInfo("gui/treasure_hunt.unity3d", "win_treasure_hunt_gameplay_main")},//太空寻宝-游戏玩法
            {6826, new NPCommonAssetPathInfo("gui/treasure_hunt.unity3d", "win_treasure_hunt_gameplay_gaming_tip")},//太空寻宝-游戏阶段提示
            {6827, new NPCommonAssetPathInfo("gui/treasure_hunt.unity3d", "win_treasure_hunt_gameplay_runUp_tip")},//太空寻宝-助跑阶段提示
            {6828, new NPCommonAssetPathInfo("gui/treasure_hunt.unity3d", "win_treasure_hunt_gameplay_quit_tip")},//太空寻宝-助跑阶段提示
            {6829, new NPCommonAssetPathInfo("gui/treasure_hunt.unity3d", "win_treasure_hunt_gameplay_resume_countdown")},//太空寻宝-助跑阶段提示
            {6830, new NPCommonAssetPathInfo("gui/treasure_hunt.unity3d", "win_treasure_hunt_gameplay_end_effect")},//太空寻宝-结束特效
            {6831, new NPCommonAssetPathInfo("gui/treasure_hunt.unity3d", "win_treasure_hunt_common_get")}, // 太空寻宝 - 通用获得弹窗
            {6832, new NPCommonAssetPathInfo("gui/treasure_hunt.unity3d", "win_treasure_hunt_upgrade_success")}, // 太空寻宝 - 太空舱升级弹窗

            #endregion
            
            #region 公会副本 6900-6909
            
            {6900, new NPCommonAssetPathInfo("gui/guild_dungeon.unity3d", "win_guild_dungeon_main")}, //公会副本-主界面
            {6901, new NPCommonAssetPathInfo("gui/guild_dungeon.unity3d", "win_pop_guild_dungeon_opening_cost")}, //公会副本-开启消耗选择弹窗
            {6902, new NPCommonAssetPathInfo("gui/guild_dungeon.unity3d", "win_pop_guild_dungeon_auto_open")}, //公会副本-自动开启弹窗
            {6903, new NPCommonAssetPathInfo("gui/guild_dungeon.unity3d", "win_pop_guild_dungeon_upgrade")}, //公会副本-副本升级弹窗
            {6904, new NPCommonAssetPathInfo("gui/guild_dungeon.unity3d", "win_pop_guild_dungeon_upgrade_succ")}, //公会副本-副本升级成功弹窗
            {6905, new NPCommonAssetPathInfo("gui/guild_dungeon.unity3d", "win_guild_dungeon_battle_main")}, //公会副本-战斗界面
            {6906, new NPCommonAssetPathInfo("gui/guild_dungeon.unity3d", "win_pop_guild_dungeon_select_hero")}, //公会副本-大臣选择弹窗
            {6907, new NPCommonAssetPathInfo("gui/guild_dungeon.unity3d", "win_pop_guild_dungeon_diary")}, //公会副本-战斗日志弹窗
            {6908, new NPCommonAssetPathInfo("gui/guild_dungeon.unity3d", "win_pop_guild_dungeon_result")}, //公会副本-战斗结算弹窗
            {6909, new NPCommonAssetPathInfo("gui/guild_dungeon.unity3d", "win_guild_dungeon_map_main")}, //公会副本-地图界面
            {6910, new NPCommonAssetPathInfo("gui/guild_dungeon.unity3d", "win_pop_guild_dungeon_leaderboard")}, //公会副本-积分排行榜弹窗
            {6911, new NPCommonAssetPathInfo("gui/guild_dungeon.unity3d", "win_guild_dungeon_map_monster_follow")}, //公会副本 地图怪物跟随UI
            {6912, new NPCommonAssetPathInfo("gui/guild_dungeon.unity3d", "win_guild_dungeon_map_boss_follow")}, //公会副本 地图怪物跟随UI

            #endregion

            #region 推送礼包 8500~8600

            {8500, new NPCommonAssetPathInfo("gui/push_gift.unity3d", "win_push_gift_frame")}, //推送礼包-框架界面
            {8501, new NPCommonAssetPathInfo("gui/push_gift.unity3d", "win_push_gift_single")}, //推送礼包-单个礼包加载界面
            {8502, new NPCommonAssetPathInfo("gui/push_gift.unity3d", "prefab_push_gift_common")}, //推送礼包-通用单礼包弹窗
            {8503, new NPCommonAssetPathInfo("gui/push_gift.unity3d", "prefab_push_gift_player_level")}, //推送礼包-玩家等级礼包弹窗
            {8504, new NPCommonAssetPathInfo("gui/push_gift.unity3d", "prefab_push_gift_consort")}, //推送礼包-情人礼包弹窗

            #endregion

            {8800, new NPCommonAssetPathInfo("gui/push_notice.unity3d", "win_push_notice_activity_list")},
            
            #region 小游戏

            {10000, new NPCommonAssetPathInfo("gui/game/minigame.unity3d", "win_minigame_common")},
            {10100, new NPCommonAssetPathInfo("gui/game/search.unity3d", "win_game_2_search_1")},
            {10200, new NPCommonAssetPathInfo("gui/game/take_things.unity3d", "win_game_3_take_things")},
            {10300, new NPCommonAssetPathInfo("gui/game/puzzle.unity3d", "win_game_1_puzzle_1")},
            {10500, new NPCommonAssetPathInfo("gui/game/qte2.unity3d", "win_game_5_qte2")},
            {10600, new NPCommonAssetPathInfo("gui/game/dragbox.unity3d", "win_game_6_dragbox")},

            #endregion
            
            #region 对话 22000-22999

            {22000, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "win_dialogue_main")},
            {22001, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "win_dialog_box_other")},
            {22002, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "win_dialog_box_self")},
            {22007, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "win_dialogue_retrospect")},

            #endregion

            #region 漫画

            {22400, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "win_simple_comic")},
            {22401, new NPCommonAssetPathInfo("gui/game_gui.unity3d", "win_simple_comic_sub_wnd")},

            #endregion

            #region 开篇剧情

            {100000, new NPCommonAssetPathInfo("gui/video.unity3d", "win_intro_story_play")},

            #endregion

            #region 大地图 遗留

            //{2200, new NPCommonAssetPathInfo("gui/space.unity3d", "win_space")},
            //{2201, new NPCommonAssetPathInfo("gui/space.unity3d", "win_space_bar")},
            //{2202, new NPCommonAssetPathInfo("gui/space.unity3d", "win_space_hud")},
            //{2203, new NPCommonAssetPathInfo("gui/space.unity3d", "win_space_walk_confirm")},
            //{2204, new NPCommonAssetPathInfo("gui/space.unity3d", "win_space_player_info")},
            //{2205, new NPCommonAssetPathInfo("gui/space.unity3d", "win_space_player_locate")},
            //{2206, new NPCommonAssetPathInfo("gui/space.unity3d", "win_space_common_choose")},
            //{2207, new NPCommonAssetPathInfo("gui/space.unity3d", "win_space_common_interact_panel")},
            //{2208, new NPCommonAssetPathInfo("gui/space.unity3d", "prefab_space_player_choose_item")},
            //{2209, new NPCommonAssetPathInfo("gui/space.unity3d", "win_space_common_follow_item")},
            //{2210, new NPCommonAssetPathInfo("gui/space.unity3d", "prefab_space_minimap_item_player")},
            //{2211, new NPCommonAssetPathInfo("gui/space.unity3d", "win_space_map")},
            //{2212, new NPCommonAssetPathInfo("gui/space.unity3d", "prefab_space_minimap_item")},
            //{2213, new NPCommonAssetPathInfo("gui/space.unity3d", "win_space_map_teleport_confirm")},
            //{2214, new NPCommonAssetPathInfo("gui/space.unity3d", "win_space_other_player_info")},
            //{2215, new NPCommonAssetPathInfo("gui/space.unity3d", "win_space_pet_feeding")},
            //{2216, new NPCommonAssetPathInfo("gui/space.unity3d", "win_space_machine")},
            //{2217, new NPCommonAssetPathInfo("gui/space.unity3d", "win_space_map_diggings_list")},
            //{2218, new NPCommonAssetPathInfo("gui/space.unity3d", "win_diggings_battle_result")},
            //{2219, new NPCommonAssetPathInfo("gui/space.unity3d", "win_space_common_tip_get")},
            //{2220, new NPCommonAssetPathInfo("gui/space.unity3d", "win_space_common_tip_cost")},
            //{2221, new NPCommonAssetPathInfo("gui/space.unity3d", "win_space_machine_tip")},
            //{2222, new NPCommonAssetPathInfo("gui/space.unity3d", "win_space_npc_dialog")},
            //{2223, new NPCommonAssetPathInfo("gui/space.unity3d", "win_space_diggings_follow_item")},
            //{2224, new NPCommonAssetPathInfo("gui/space.unity3d", "win_space_interactive")},
            //{2225, new NPCommonAssetPathInfo("gui/space.unity3d", "win_space_interactive_progress")},
            //{2226, new NPCommonAssetPathInfo("gui/space.unity3d", "win_space_party_follow_item")},
            //{2227, new NPCommonAssetPathInfo("gui/space.unity3d", "win_space_arena_follow_item")},
            //{2228, new NPCommonAssetPathInfo("gui/space.unity3d", "win_space_border_tip")},

            //{2230, new NPCommonAssetPathInfo("gui/space.unity3d", "win_space_map_diggings_nomal_confirm")},
            //{2231, new NPCommonAssetPathInfo("gui/space.unity3d", "win_space_map_diggings_high_confirm")},
            //{2232, new NPCommonAssetPathInfo("gui/space.unity3d", "win_diggings_attack")},

            //{2233, new NPCommonAssetPathInfo("gui/space.unity3d", "win_space_pet_capture")},
            //{2234, new NPCommonAssetPathInfo("gui/space.unity3d", "win_space_captrue_show")},
            //{2235, new NPCommonAssetPathInfo("gui/space.unity3d", "win_space_reward_get")},


            //{2240, new NPCommonAssetPathInfo("gui/space.unity3d", "win_space_npc_battle")},
            //{2241, new NPCommonAssetPathInfo("gui/space.unity3d", "win_battle_npc_pass_end")},
            //{2242, new NPCommonAssetPathInfo("gui/space.unity3d", "win_battle_npc_pass_victory")},
            //{2243, new NPCommonAssetPathInfo("gui/space.unity3d", "win_battle_npc_pass_fail")},

            //{2260, new NPCommonAssetPathInfo("gui/space.unity3d", "win_small_map_fitter")},
            //{2261, new NPCommonAssetPathInfo("gui/space.unity3d", "win_mini_map_fitter")},

            #endregion

            #region Grave 杰出者大厅

            {6600, new NPCommonAssetPathInfo("gui/grave.unity3d", "win_grave_main")}, //杰出者大厅-主界面
            {6601, new NPCommonAssetPathInfo("gui/grave.unity3d", "win_grave_player_detail")}, //杰出者大厅-玩家详情界面
            {6602, new NPCommonAssetPathInfo("gui/grave.unity3d", "win_pop_grave_blessing_details")}, //杰出者大厅-杰出者祝福弹窗
            {6603, new NPCommonAssetPathInfo("gui/grave.unity3d", "win_pop_grave_blessing_get")}, //杰出者大厅-杰出者祝福获得弹窗
            {6604, new NPCommonAssetPathInfo("gui/grave.unity3d", "win_pop_grave_new_prominent")}, //杰出者大厅-新晋杰出者弹窗
            {6605, new NPCommonAssetPathInfo("gui/grave.unity3d", "win_pop_grave_congrats_succ")}, //杰出者大厅-膜拜成功弹窗
            {6606, new NPCommonAssetPathInfo("gui/grave.unity3d", "win_pop_grave_records")}, //杰出者大厅-榜单历史记录弹窗
            {6607, new NPCommonAssetPathInfo("gui/grave.unity3d", "win_grave_person_item")}, //杰出者大厅-榜单历史记录弹窗

            #endregion

            #region 火星 7000~7099

            {7000, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_pop_mars_start_mission")}, //前往火星-火星任务弹窗
            {7001, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_start_to_mars")}, //前往火星-火星任务开启界面
            {7002, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_way_to_mars")}, //前往火星-火星任务进行中界面
            {7003, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_pop_mars_journey_log")}, //前往火星-航线日志弹窗
            {7004, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_mars_landing_choose")}, //前往火星-登陆交互界面
            {7005, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_pop_mars_new_stage")}, //前往火星-到达新节点确认界面

            #endregion
            
            #region 火星基地 7100~7199
            {7100, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_Mars_hud")},
            {7101, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_Mars_main_scene")},
            {7102, new NPCommonAssetPathInfo("gui/mars.unity3d", "prefab_Mars_building_build_tip")},
            {7103, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_pop_Mars_build_conditions")},
            {7104, new NPCommonAssetPathInfo("gui/mars.unity3d", "prefab_Mars_building_constructing_tip")},
            {7105, new NPCommonAssetPathInfo("gui/mars.unity3d", "prefab_Mars_building_confirm_tip")},
            {7106, new NPCommonAssetPathInfo("gui/mars.unity3d", "prefab_Mars_building_name_tip")},
            {7107, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_pop_Mars_building_details")},
            {7108, new NPCommonAssetPathInfo("gui/mars.unity3d", "prefab_Mars_building_details_equipment_page")},
            {7109, new NPCommonAssetPathInfo("gui/mars.unity3d", "prefab_Mars_building_details_poineers_page")},
            {7110, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_pop_Mars_upgrade_conditions")},
            {7111, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_pop_Mars_build")},
            {7112, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_pop_Mars_upgrade")},
            {7113, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_pop_Mars_speed_up_access_ways")},
            {7114, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_pop_Mars_home_upgrade")},
            {7115, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_pop_Mars_home_upgrade_conditions")},
            {7116, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_pop_Mars_main_building_details")},
            {7117, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_pop_Mars_energy_produce")},
            {7118, new NPCommonAssetPathInfo("gui/mars.unity3d", "prefab_Mars_building_energy_tip")},
            {7119, new NPCommonAssetPathInfo("gui/mars.unity3d", "prefab_Mars_building_level_tip")},
            {7120, new NPCommonAssetPathInfo("gui/mars.unity3d", "prefab_Mars_building_immigrant_tip")},
            {7121, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_pop_Mars_building_complete_now_check")},
            {7122, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_pop_Mars_speed_up")},
            {7123, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_pop_Mars_build_queue_buy")},
            {7124, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_Mars_power_info_tip")},
            {7125, new NPCommonAssetPathInfo("gui/mars.unity3d", "prefab_Mars_building_name_center_tip")},
            {7126, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_pop_Mars_build_queue_state")},
            {7128, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_pop_Mars_energy_build_detail_info")},
                    
            {7193, new NPCommonAssetPathInfo("gui/mars.unity3d", "prefab_Mars_building_red_tip")},
            
            #endregion

            #region 火星居民 7200~7299

            {7200, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_pop_mars_people_state")}, //火星居民 - 状态界面
            {7201, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_pop_mars_people_recruit")}, //火星居民 - 居民补充申请弹窗
            {7202, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_pop_mars_people_arrived")}, //火星居民 - 居民到达窗口
            {7203, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_mars_ai")}, //火星居民 - ai智控界面
            {7204, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_pop_mars_ai_detail")}, //火星居民 - ai智控详情弹窗
            {7205, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_mars_people_center")}, //火星居民 - 民意信箱界面
            {7206, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_pop_mars_sos_detail_select")}, //火星居民 - 选项求助弹窗
            {7207, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_pop_mars_sos_detail_select_done")}, //火星居民 - 选项求助结果弹窗
            {7208, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_pop_mars_sos_detail_reward")}, //火星居民 - 奖励求助弹窗
            {7209, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_pop_mars_event_detail")}, //火星居民 - 事件详情弹窗
            {7210, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_pop_mars_people_get_tip")}, //火星居民 - 事件详情弹窗

            #endregion

            #region 火星拓展 7300~7399

            {7300, new NPCommonAssetPathInfo("gui/mars.unity3d", "prefab_Mars_armory_hud")},//火星拓展-兵工厂hud
            {7301, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_pop_Mars_armory_develop_details")},//火星拓展-兵工厂升级弹窗
            {7302, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_pop_Mars_armory_info_details")},//火星拓展-兵工厂详情弹窗
            {7303, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_pop_Mars_armory_more_info_details")},//火星拓展-兵工厂更多详情弹窗
            {7304, new NPCommonAssetPathInfo("gui/mars.unity3d", "prefab_Mars_tech_hud")},//火星拓展-科研室普通状态操作hud
            {7305, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_Mars_tech_building_main")},//火星拓展-科研室主界面
            {7306, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_pop_Mars_tech_bouns_overview")},//火星拓展-科技加成总览弹窗
            {7307, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_pop_Mars_tech_item_details")},//火星拓展-科技研究详情弹窗
            {7308, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_pop_Mars_tech_level_overview")},//火星拓展-科技等级效果预览弹窗
            {7309, new NPCommonAssetPathInfo("gui/mars.unity3d", "prefab_Mars_maintenance_hud")},//火星拓展-维修室hud
            {7310, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_Mars_maintenance_building_main")},//火星拓展-维修室主界面
            {7311, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_Mars_maintenance_team_appoint")},//火星拓展-队伍编辑弹窗
            {7312, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_Mars_maintenance_team_rename")},//火星拓展-队伍重命名弹窗
            {7313, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_pop_Mars_maintenance_confirm")},//火星拓展-维修确认弹窗
            {7314, new NPCommonAssetPathInfo("gui/mars.unity3d", "prefab_Mars_tech_state_hud")},//火星拓展-科研室状态hud
            {7315, new NPCommonAssetPathInfo("gui/mars.unity3d", "prefab_Mars_maintenance_tip_hud")},//火星拓展-维修室提示hud
            {7316, new NPCommonAssetPathInfo("gui/mars.unity3d", "prefab_Mars_tech_upgrading_hud")},//火星拓展-科研室升级中状态操作hud
            {7317, new NPCommonAssetPathInfo("gui/mars.unity3d", "prefab_Mars_assist_hud")},//火星拓展-互助建筑hud
            {7318, new NPCommonAssetPathInfo("gui/mars.unity3d", "prefab_Mars_assist_state_hud")},//火星拓展-有盟友需要帮助建筑hud
            {7319, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_pop_Mars_assist_develop_details")},//火星拓展-兵工厂升级弹窗
            {7320, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_pop_Mars_assist_info_details")},//火星拓展-兵工厂详情弹窗
            {7321, new NPCommonAssetPathInfo("gui/mars.unity3d", "prefab_Mars_maintenance_repair_time_hud")},//火星拓展-维修室维修时间hud
            
            #endregion
            
            #region 火星探索 7400~7499
            
            {7400, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_mars_explore_main")}, // 火星探索-主界面窗口
            {7401, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_pop_mars_explore_lv_info")}, // 火星探索-主界面-等级详情弹窗
            {7402, new NPCommonAssetPathInfo("gui/mars.unity3d", "prefab_mars_explore_event_bubbles")}, // 火星探索-事件泡泡
            {7403, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_pop_mars_explore_event_team_select")}, // 火星探索-选择派遣队伍
            {7404, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_pop_mars_explore_event_info_info")}, // 火星探索-事件详情的详情弹窗
            {7405, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_pop_mars_explore_resource_get")}, // 火星探索-采集奖励弹窗
            {7406, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_pop_mars_explore_upgrade")}, // 火星探索-探索等级升级弹窗
            {7407, new NPCommonAssetPathInfo("gui/mars.unity3d", "prefab_mars_explore_event_car")}, // 火星探索-行驶的车
            {7408, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_pop_mars_explore_team_unlock")}, // 火星探索-解锁探索队伍途径弹窗
            {7409, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_mars_explore_hud")}, // 火星探索-hud
            {7410, new NPCommonAssetPathInfo("gui/mars.unity3d", "prefab_mars_explore_mine_bubbles")}, // 火星探索-矿的气泡
            {7411, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_pop_mars_explore_event_resource_info")}, // 火星探索-事件-资源弹窗
            {7412, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_pop_mars_explore_event_fighting_info")}, // 火星探索-事件-战斗弹窗
            {7413, new NPCommonAssetPathInfo("gui/mars.unity3d", "prefab_mars_explore_home_base_hud")}, // 火星探索-事件-战斗弹窗
            {7414, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_pop_mars_explore_pvp_log")}, // 火星探索-事件-战斗弹窗
            {7415, new NPCommonAssetPathInfo("gui/mars.unity3d", "prefab_mars_explore_boss_event_bubbles")}, // 火星探索-事件-战斗弹窗
            {7416, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_Mars_mine_share_main")}, // 火星探索-火星分享弹窗
            {7417, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_pop_mars_explore_log_details")},
            {7418, new NPCommonAssetPathInfo("gui/mars.unity3d", "win_pop_mars_explore_event_list")},
            {7419, new NPCommonAssetPathInfo("gui/mars.unity3d", "prefab_mars_explore_event_list_battle_item")},
            {7420, new NPCommonAssetPathInfo("gui/mars.unity3d", "prefab_mars_explore_event_list_boss_item")},
            {7421, new NPCommonAssetPathInfo("gui/mars.unity3d", "prefab_mars_explore_event_list_mine_item")},
            #endregion
            
            #region 首充礼包 8000~8009
            
            {8000, new NPCommonAssetPathInfo("gui/first_recharge.unity3d", "win_first_recharge_win")},//首充礼包

            #endregion

            #region 商店好评 8010~8020

            {8010, new NPCommonAssetPathInfo("gui/appscore.unity3d", "win_appscore")},//好评引导窗口
            {8011, new NPCommonAssetPathInfo("gui/appscore.unity3d", "win_appscore_diss")},//评价窗口

            #endregion

            #region VIP 8100~8199
            
            {8100, new NPCommonAssetPathInfo("gui/vip.unity3d", "win_vip_main")},//VIP主界面
            {8101, new NPCommonAssetPathInfo("gui/vip.unity3d", "win_vip_info")},//VIP-充值详情弹窗
            {8102, new NPCommonAssetPathInfo("gui/vip.unity3d", "win_vip_get")},//VIP-等级升级弹窗

            #endregion

            #region 联盟宝箱 8700~8799

            {8702, new NPCommonAssetPathInfo("gui/guild_box.unity3d", "win_guild_gift_main")},//
            {8703, new NPCommonAssetPathInfo("gui/guild_box.unity3d", "win_guild_gift_main_points_info")},//
            {8704, new NPCommonAssetPathInfo("gui/guild_box.unity3d", "win_guild_gift_main_box_info")},//
            {8705, new NPCommonAssetPathInfo("gui/guild_box.unity3d", "prefab_guild_gift_main_pay")},//
            {8706, new NPCommonAssetPathInfo("gui/guild_box.unity3d", "prefab_guild_gift_main_free")},//


            #endregion
            
            #region 联盟宝箱 8900~8999

            {8900, new NPCommonAssetPathInfo("gui/rank_gift_pack.unity3d", "win_rank_gift_pack_main")},//
            {8901, new NPCommonAssetPathInfo("gui/rank_gift_pack.unity3d", "prefab_rank_gift_pack_city_entrance")},//
            #endregion
            #region 限时兑换 9000- 9099

            {9000, new NPCommonAssetPathInfo("gui/rush_exchange.unity3d", "win_rush_exchange_win")},//
            {9001, new NPCommonAssetPathInfo("gui/rush_exchange.unity3d", "win_rush_exchange_bubble")},//
            
            #endregion
            #region 情人收集 9100~9199
            {9100, new NPCommonAssetPathInfo("gui/consort_rescue.unity3d", "win_consort_rescue_choose")},
            {9101, new NPCommonAssetPathInfo("gui/consort_rescue.unity3d", "win_consort_rescue_main")},
            #endregion
        };
#endif

        /// <summary>
        /// 获取assetPath
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public static NPCommonAssetPathInfo getAssetInfo(long _id)
        {
#if NP_GAME
            NPUIResPathRefObj uiResPathRefObj = GRefdataCoreMgr.instance.uiResPathRefCore.getRef(_id);
            if (null == uiResPathRefObj)
            {

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                NPCommonAssetPathInfo commonAssetPathInfo = null;
                if (_m_uiPathDict.TryGetValue(_id, out commonAssetPathInfo))
                {
                    return commonAssetPathInfo;
                }
#endif
                Debug.LogError($"ui_res_path配置缺失，Id:{_id}");
                return null;
            }
            else
            {
                return uiResPathRefObj;
            }
#else
            return null;
#endif
        }

        /// <summary>
        /// 获取assetPath
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public static string getAssetPath(long _id)
        {
#if NP_GAME
            NPUIResPathRefObj uiResPathRefObj = GRefdataCoreMgr.instance.uiResPathRefCore.getRef(_id);
            if (null == uiResPathRefObj)
            {

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                NPCommonAssetPathInfo commonAssetPathInfo = null;
                if (_m_uiPathDict.TryGetValue(_id, out commonAssetPathInfo))
                {
                    return commonAssetPathInfo.asset_path;
                }
#endif

                Debug.LogError($"ui_res_path配置缺失，Id:{_id}");
                return String.Empty;
            }
            else
            {
                return uiResPathRefObj.asset_path;
            }
#else
            return String.Empty;
#endif
        }

        /// <summary>
        /// 获取objName
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public static string getObjName(long _id)
        {
#if NP_GAME
            NPUIResPathRefObj uiResPathRefObj = GRefdataCoreMgr.instance.uiResPathRefCore.getRef(_id);
            if (null == uiResPathRefObj)
            {

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                NPCommonAssetPathInfo commonAssetPathInfo = null;
                if (_m_uiPathDict.TryGetValue(_id, out commonAssetPathInfo))
                {
                    return commonAssetPathInfo.obj_name;
                }
#endif
                Debug.LogError($"ui_res_path配置缺失，Id:{_id}");
                return String.Empty;
            }
            else
            {
                return uiResPathRefObj.obj_name;
            }
#else
           return String.Empty;
#endif
        }

        /// <summary>
        /// 校验资源，判断程序定义的跟策划配置的是否对的上
        /// </summary>
        public static void CheckRes()
        {
#if NP_GAME
#if UNITY_EDITOR
            foreach (long id in _m_uiPathDict.Keys)
            {
                NPUIResPathRefObj npuiResPathRefObj = GRefdataCoreMgr.instance.uiResPathRefCore.getRef(id);
                if (null == npuiResPathRefObj)
                {
                    NPCommonAssetPathInfo commonAssetPathInfo = null;
                    if (_m_uiPathDict.TryGetValue(id, out commonAssetPathInfo))
                    {
                        Debug.LogError($"ui_res_path配置缺失，Id:{id};路径:{commonAssetPathInfo.ToString()}");
                    }
                    else
                    {
                        Debug.LogError($"ui_res_path配置缺失，Id:{id}");
                    }
                }
            }
#endif
#endif
        }
    }
}