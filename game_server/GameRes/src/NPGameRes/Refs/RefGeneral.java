package NPGameRes.Refs;

import Common.ArenaEnum.EArenaBuffType;
import Common.HeroObj.Hero_ArenaShowList;
import Common.MarsEnum.EMarsExploreEventType;
import Common.MarsEnum.EMarsPeopleHelpType;
import Common.TravelEnum.ETravelEventType;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.CommonObj.NPCommonItem;
import NPCommon.CommonObj.NPRefreshTimeObj;
import NPCommon.Game.*;
import NPCommon.Game.RangeWeight.RangeRandomWeightList;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefSingleContainer;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Pair.*;
import NPCommon.Util.WCGResCommon;
import NPEnum.ENPTimeRefreshType;
import NPEnum.EQuality;
import NPGameRes.GameObjs.Arena.ArenaHeroObjList;
import NPGameRes.GameObjs.CommonObj.LevelObj._TLevelMapMgr;
import NPGameRes.GameObjs.NPPlayerProperty.NPPlayerPropertyModifier;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;
import NPGameRes.GameObjs.Tower.TowerStageRefObj;
import NPGameRes.Refs.Arena.RefArenaBuff;
import NPGameRes.Refs.Dinner.RefDinnerJoinCost;
import NPGameRes.Refs.Guild.RefGuildBoxEvent;
import NPGameRes.Refs.Mars.RefMarsExploreEventBattle;
import NPGameRes.Refs.Mars.RefMarsExploreMine;
import NPGameRes.Refs.Mars.RefMarsGoRoute;
import NPGameRes.Refs.Mars.RefMarsImmigrationPer;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

@RefTable(ignore = true)
public class RefGeneral extends RefBase
{
    private static RefGeneralMgr _g_mgr = new RefGeneralMgr();

    public static RefGeneralMgr getMgr()
    {
        return _g_mgr;
    }

    public static RefGeneral Ref()
    {
        return getMgr().getRef();
    }

    public static class RefGeneralMgr extends RefSingleContainer<RefGeneral>
    {
        protected RefGeneralMgr()
        {
            super();

            //设置新的值
            initPut(new RefGeneral());
        }
    }

    /// ///////////////////////////


    @Override
    public RefGeneralMgr getStaticContainer()
    {
        return _g_mgr;
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefGeneralMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefGeneral newRef = (RefGeneral) _newRef;
        player_property = newRef.player_property;
        player_init_give_item = newRef.player_init_give_item;
        player_init_mail_id = newRef.player_init_mail_id;
        player_init_mail_give_item = newRef.player_init_mail_give_item;
        mail_max_lock_num = newRef.mail_max_lock_num;
        mail_max_num = newRef.mail_max_num;
        player_info_rename_cost_list = newRef.player_info_rename_cost_list;
        friend_apply_limit = newRef.friend_apply_limit;
        friend_apply_avaiable_secs = newRef.friend_apply_avaiable_secs;
        hero_level_up_ten_times_condition = newRef.hero_level_up_ten_times_condition;
        equip_upgrade_ten_times_simple_unlock_id = newRef.equip_upgrade_ten_times_simple_unlock_id;
        equip_normal_cost_group_id = newRef.equip_normal_cost_group_id;
        equip_advance_cost = newRef.equip_advance_cost;
        equip_normal_add_pro_group_id = newRef.equip_normal_add_pro_group_id;
        equip_advance_add_pro_group_id = newRef.equip_advance_add_pro_group_id;
        consort_rand_call_cd = newRef.consort_rand_call_cd;
        consort_call_rand_cg_per = newRef.consort_call_rand_cg_per;
        consort_call_rand_per = newRef.consort_call_rand_per;
        consort_call_gem_reset = newRef.consort_call_gem_reset;
        consort_reshape_skill_max_count = newRef.consort_reshape_skill_max_count;
        consort_call_new_cg_fixed_cd = newRef.consort_call_new_cg_fixed_cd;
        child_cal_unit_per = newRef.child_cal_unit_per;
        child_add_per = newRef.child_add_per;
        child_train_add_per = newRef.child_train_add_per;
        child_seat_recover_item = newRef.child_seat_recover_item;
        child_seat_recover_S = newRef.child_seat_recover_S;
        unmarry_adult_limit = newRef.unmarry_adult_limit;
        married_adult_limit = newRef.married_adult_limit;
        marry_apply_expired_S = newRef.marry_apply_expired_S;
        child_married_bonus_add_per = newRef.child_married_bonus_add_per;
        child_married_gift_mail_id = newRef.child_married_gift_mail_id;
        giftde_child_dinner_permit_per = newRef.giftde_child_dinner_permit_per;
        unlock_chapter_simple_unlock_id = newRef.unlock_chapter_simple_unlock_id;
        gold_inspire_increase_power_ratio_per = newRef.gold_inspire_increase_power_ratio_per;
        crystal_inspire_increase_power_ratio_per = newRef.crystal_inspire_increase_power_ratio_per;
        item_inspire_increase_power_ratio_per = newRef.item_inspire_increase_power_ratio_per;
        gold_inspire_calculate_ratio_time_price_id = newRef.gold_inspire_calculate_ratio_time_price_id;
        crystal_inspire_fixed_cost = newRef.crystal_inspire_fixed_cost;
        item_inspire_cost = newRef.item_inspire_cost;
        forward_gold_cost_ratio_range = newRef.forward_gold_cost_ratio_range;
        critical_hit_coefficient = newRef.critical_hit_coefficient;
        anecdote_first_event = newRef.anecdote_first_event;
        anecdote_lazy_cd = newRef.anecdote_lazy_cd;
        anecdote_pos_group_max_event_count = newRef.anecdote_pos_group_max_event_count;
        quest_init_open_list = newRef.quest_init_open_list;
        daily_quest_one_key_finish_condition = newRef.daily_quest_one_key_finish_condition;
        default_quest_id = newRef.default_quest_id;
        daily_check_refresh_clock = newRef.daily_check_refresh_clock;
        daily_check_once_reward_id = newRef.daily_check_once_reward_id;
        daily_check_dessert_num = newRef.daily_check_dessert_num;
        daily_check_timeout = newRef.daily_check_timeout;
        daily_check_extra_dessert_num = newRef.daily_check_extra_dessert_num;
        daily_check_default_consort_id = newRef.daily_check_default_consort_id;
        daily_check_step_show_num = newRef.daily_check_step_show_num;
        player_title_available_time_max = newRef.player_title_available_time_max;
        travel_cost_lazycd_id = newRef.travel_cost_lazycd_id;
        travel_one_key_simple_unlock_id = newRef.travel_one_key_simple_unlock_id;
        travel_default_event_type = newRef.travel_default_event_type;
        travel_akey_event_limit = newRef.travel_akey_event_limit;
        early_travel_trigger_event_list = newRef.early_travel_trigger_event_list;
        market_akey_simple_unlock_id = newRef.market_akey_simple_unlock_id;
        market_reset = newRef.market_reset;
        market_reset_num = newRef.market_reset_num;
        market_shop_cd_sec = newRef.market_shop_cd_sec;
        market_up_cost_item = newRef.market_up_cost_item;
        market_shop_restore_cd_costitem = newRef.market_shop_restore_cd_costitem;
        marquee_limit = newRef.marquee_limit;
        marquee_gacha_draw_item_marquee_id = newRef.marquee_gacha_draw_item_marquee_id;
        marquee_gain_ur_museum_item_marquee_id = newRef.marquee_gain_ur_museum_item_marquee_id;
        marquee_gain_ur_hero_marquee_id = newRef.marquee_gain_ur_hero_marquee_id;
        marquee_gain_ur_consort_marquee_id = newRef.marquee_gain_ur_consort_marquee_id;
        marquee_first_reach_big_stage_marquee_id = newRef.marquee_first_reach_big_stage_marquee_id;
        friend_recommend_num = newRef.friend_recommend_num;
        friend_recommend_power_interval = newRef.friend_recommend_power_interval;
        friend_recommend_lvl_interval = newRef.friend_recommend_lvl_interval;
        receive_like_score = newRef.receive_like_score;
        like_range = newRef.like_range;
        rank_show_max_num = newRef.rank_show_max_num;
        rank_fixed_akey_simple_unlock_id = newRef.rank_fixed_akey_simple_unlock_id;
        activity_step_reward_mail_id = newRef.activity_step_reward_mail_id;
        activity_rank_reward_mail_id = newRef.activity_rank_reward_mail_id;
        bag_auto_use_max_count = newRef.bag_auto_use_max_count;
        batch_use_item_max_count = newRef.batch_use_item_max_count;
        activity_bag_item_expired_mail_id = newRef.activity_bag_item_expired_mail_id;
        visit_other_player_gain_item_list = newRef.visit_other_player_gain_item_list;
        week_card_offline_hosting_max_profit_time_sec = newRef.week_card_offline_hosting_max_profit_time_sec;
        week_card_offline_active_min_time_sec = newRef.week_card_offline_active_min_time_sec;
        week_card_free_trial_time_sec = newRef.week_card_free_trial_time_sec;
        week_card_func_simple_unlock_id = newRef.week_card_func_simple_unlock_id;
        shield_cid_limit = newRef.shield_cid_limit;
        item_expired_mail_id = newRef.item_expired_mail_id;
        questionnaire_mail_id = newRef.questionnaire_mail_id;
        collect_likes_fixed_id = newRef.collect_likes_fixed_id;
        guild_change_name_cost = newRef.guild_change_name_cost;
        guild_name_length_limit = newRef.guild_name_length_limit;
        guild_simple_name_length_limit = newRef.guild_simple_name_length_limit;
        guild_declaration_length_limit = newRef.guild_declaration_length_limit;
        guild_announcement_length_limit = newRef.guild_announcement_length_limit;
        guild_create_cost = newRef.guild_create_cost;
        guild_first_time_join_reward_list = newRef.guild_first_time_join_reward_list;
        guild_change_flag_cost = newRef.guild_change_flag_cost;
        guild_free_join_cd_num = newRef.guild_free_join_cd_num;
        guild_join_cd_hours = newRef.guild_join_cd_hours;
        guild_join_request_limit_num = newRef.guild_join_request_limit_num;
        guild_leader_proactive_transfer_cd_hours = newRef.guild_leader_proactive_transfer_cd_hours;
        guild_leader_transfer_target_online_within_hours = newRef.guild_leader_transfer_target_online_within_hours;
        guild_leader_trigger_passive_transfer_offline_beyond_hours = newRef.guild_leader_trigger_passive_transfer_offline_beyond_hours;
        guild_leader_impeach_offline_beyond_hours = newRef.guild_leader_impeach_offline_beyond_hours;
        guild_leader_impeach_message_available_within_hours = newRef.guild_leader_impeach_message_available_within_hours;
        guild_upgrade_mail_id = newRef.guild_upgrade_mail_id;
        guild_first_time_join_success_mail_id = newRef.guild_first_time_join_success_mail_id;
        guild_join_success_mail_id = newRef.guild_join_success_mail_id;
        guild_passive_exit_mail_id = newRef.guild_passive_exit_mail_id;
        guild_position_change_mail_id = newRef.guild_position_change_mail_id;
        guild_broadcast_message_daily_limit_fixcd_id = newRef.guild_broadcast_message_daily_limit_fixcd_id;
        guild_broadcast_message_cost = newRef.guild_broadcast_message_cost;
        guild_broadcast_message_length_limit = newRef.guild_broadcast_message_length_limit;
        guild_rank_id = newRef.guild_rank_id;
        guild_impeach_succ_need_percent = newRef.guild_impeach_succ_need_percent;
        guild_log_chat_npc_id = newRef.guild_log_chat_npc_id;
        guild_open_recruit_gap_sec = newRef.guild_open_recruit_gap_sec;
        guild_entrust_reward_mail_id = newRef.guild_entrust_reward_mail_id;
        deal_entrust_lazy_cd_id = newRef.deal_entrust_lazy_cd_id;
        guild_entrust_crit_mul_weight = newRef.guild_entrust_crit_mul_weight;
        each_attr_can_dispatch_hero_num = newRef.each_attr_can_dispatch_hero_num;
        guild_exp_common_item = newRef.guild_exp_common_item;
        guild_wealth_common_item = newRef.guild_wealth_common_item;
        personal_contribution_common_item = newRef.personal_contribution_common_item;
        guild_rank_fixed_id = newRef.guild_rank_fixed_id;
        guild_box_claim_fixed_cd_id = newRef.guild_box_claim_fixed_cd_id;
        guild_box_effect_secs = newRef.guild_box_effect_secs;
        guild_box_active_effect_secs = newRef.guild_box_active_effect_secs;
        guild_cooperate_dispatch_time_reset_cost = newRef.guild_cooperate_dispatch_time_reset_cost;
        guild_cooperate_dispatch_time_reset_limit_fix_cd_id = newRef.guild_cooperate_dispatch_time_reset_limit_fix_cd_id;
        guild_cooperate_dispatch_property_point_hp_per = newRef.guild_cooperate_dispatch_property_point_hp_per;
        guild_cooperate_dispatch_guild_coin_reward_min = newRef.guild_cooperate_dispatch_guild_coin_reward_min;
        guild_cooperate_dispatch_guild_devote_reward_min = newRef.guild_cooperate_dispatch_guild_devote_reward_min;
        guild_cooperate_dispatch_can_gain_guild_devote_time_fix_cd_id = newRef.guild_cooperate_dispatch_can_gain_guild_devote_time_fix_cd_id;
        guild_cooperate_dispatch_can_gain_guild_coin_time_fix_cd_id = newRef.guild_cooperate_dispatch_can_gain_guild_coin_time_fix_cd_id;
        guild_cooperate_refresh_time = newRef.guild_cooperate_refresh_time;
        guild_cooperate_dispatch_same_property_add_per = newRef.guild_cooperate_dispatch_same_property_add_per;
        guild_cooperate_log_max_count = newRef.guild_cooperate_log_max_count;
        guild_cooperate_construction_common_item = newRef.guild_cooperate_construction_common_item;
        business_building_res_max_timeS = newRef.business_building_res_max_timeS;
        farming_building_click_spaceMS = newRef.farming_building_click_spaceMS;
        farming_building_trigger_multiple_daily_limit = newRef.farming_building_trigger_multiple_daily_limit;
        dinner_deleting_contact_record_deadline = newRef.dinner_deleting_contact_record_deadline;
        dinner_open_record_save_limit = newRef.dinner_open_record_save_limit;
        dinner_npc_join_cost_id = newRef.dinner_npc_join_cost_id;
        dinner_owner_settle_mail_id = newRef.dinner_owner_settle_mail_id;
        arena_initial_choose_buff_list = newRef.arena_initial_choose_buff_list;
        arena_choose_buff_list = newRef.arena_choose_buff_list;
        arena_random_attack_free_limit = newRef.arena_random_attack_free_limit;
        arena_random_attack_crystal_buy_limit = newRef.arena_random_attack_crystal_buy_limit;
        arena_random_attack_crystal_buy_ratio = newRef.arena_random_attack_crystal_buy_ratio;
        arena_random_attack_crystal_buy_time_price_id = newRef.arena_random_attack_crystal_buy_time_price_id;
        arena_random_attack_match_interval = newRef.arena_random_attack_match_interval;
        arena_select_attack_daily_limit = newRef.arena_select_attack_daily_limit;
        arena_celebrity_rank_up_need_defeat_hero = newRef.arena_celebrity_rank_up_need_defeat_hero;
        arena_celebrity_rank_limit_num = newRef.arena_celebrity_rank_limit_num;
        arena_one_key_attack_simple_unlock_id = newRef.arena_one_key_attack_simple_unlock_id;
        arena_rank_fixed_id = newRef.arena_rank_fixed_id;
        arena_station_output_item = newRef.arena_station_output_item;
        arena_gain_influence_for_defeating_each_hero = newRef.arena_gain_influence_for_defeating_each_hero;
        arena_deduct_influence_for_each_hero_defeated = newRef.arena_deduct_influence_for_each_hero_defeated;
        arena_gain_coin_for_defeating_each_hero = newRef.arena_gain_coin_for_defeating_each_hero;
        arena_initial_influence = newRef.arena_initial_influence;
        arena_battle_report_limit = newRef.arena_battle_report_limit;
        arena_fight_back_limit = newRef.arena_fight_back_limit;
        arena_bot_hero_list = newRef.arena_bot_hero_list;
        arena_first_experience_bot_template_id = newRef.arena_first_experience_bot_template_id;
        unlock_arena_when_hero_num_equals = newRef.unlock_arena_when_hero_num_equals;
        arena_station_privilege_collect_limit_time_sec = newRef.arena_station_privilege_collect_limit_time_sec;
        arena_station_privilege_collect_limit_num = newRef.arena_station_privilege_collect_limit_num;
        arena_station_privilege_permissions_id = newRef.arena_station_privilege_permissions_id;
        arena_us_hero_list = newRef.arena_us_hero_list;
        playerLvl_daily_reward_mail_id = newRef.playerLvl_daily_reward_mail_id;
        seven_days_login_reward_timeprice_id = newRef.seven_days_login_reward_timeprice_id;
        mail_plan_mail_id = newRef.mail_plan_mail_id;
        tower_coin_daily_reward_mail_id = newRef.tower_coin_daily_reward_mail_id;
        tower_unlock_simple_unlock_id = newRef.tower_unlock_simple_unlock_id;
        tower_defence_report_limit = newRef.tower_defence_report_limit;
        tower_defence_report_time_limit = newRef.tower_defence_report_time_limit;
        activity_default_settle_duration_sec = newRef.activity_default_settle_duration_sec;
        midday_dungeon_preview_fight_time = newRef.midday_dungeon_preview_fight_time;
        midday_dungeon_start_fight_time = newRef.midday_dungeon_start_fight_time;
        midday_dungeon_end_fight_time = newRef.midday_dungeon_end_fight_time;
        midday_dungeon_rank_fixed_id = newRef.midday_dungeon_rank_fixed_id;
        midday_dungeon_can_borrow_guild_hero_num_fixed_cd_id = newRef.midday_dungeon_can_borrow_guild_hero_num_fixed_cd_id;
        midday_dungeon_auto_fight_simple_unlock_id = newRef.midday_dungeon_auto_fight_simple_unlock_id;
        midday_dungeon_gold_profit_ratio = newRef.midday_dungeon_gold_profit_ratio;
        midday_dungeon_server_round_drop_box_limit = newRef.midday_dungeon_server_round_drop_box_limit;
        evening_dungeon_preview_fight_time = newRef.evening_dungeon_preview_fight_time;
        evening_dungeon_start_fight_time = newRef.evening_dungeon_start_fight_time;
        evening_dungeon_end_fight_time = newRef.evening_dungeon_end_fight_time;
        evening_dungeon_close_fight_time = newRef.evening_dungeon_close_fight_time;
        evening_dungeon_boss_respawn_times_limit = newRef.evening_dungeon_boss_respawn_times_limit;
        evening_dungeon_boss_respawn_sec = newRef.evening_dungeon_boss_respawn_sec;
        evening_dungeon_boss_initial_blood = newRef.evening_dungeon_boss_initial_blood;
        evening_dungeon_boss_respawn_reference_times = newRef.evening_dungeon_boss_respawn_reference_times;
        evening_dungeon_auto_fight_simple_unlock_id = newRef.evening_dungeon_auto_fight_simple_unlock_id;
        evening_dungeon_day_first_kill_boss_reward = newRef.evening_dungeon_day_first_kill_boss_reward;
        evening_dungeon_day_kill_boss_reward = newRef.evening_dungeon_day_kill_boss_reward;
        evening_dungeon_rank_reward_mail_id = newRef.evening_dungeon_rank_reward_mail_id;
        evening_dungeon_gold_profit_ratio = newRef.evening_dungeon_gold_profit_ratio;
        evening_dungeon_rank_id = newRef.evening_dungeon_rank_id;
        earning_rank_id = newRef.earning_rank_id;
        earning_goal_mail_id = newRef.earning_goal_mail_id;
        seven_day_goals_mail_id = newRef.seven_day_goals_mail_id;
        consort_chat_ai_send_time_price_id = newRef.consort_chat_ai_send_time_price_id;
        consort_chat_ai_send_fixed_cd_id = newRef.consort_chat_ai_send_fixed_cd_id;
        consort_chat_moment_daily_max_count = newRef.consort_chat_moment_daily_max_count;
        consort_chat_moment_reply_daily_max_count = newRef.consort_chat_moment_reply_daily_max_count;
        consort_ai_chat_default_language = newRef.consort_ai_chat_default_language;
        consort_chat_ai_consort_initiate_msg_daily_max_count = newRef.consort_chat_ai_consort_initiate_msg_daily_max_count;
        consort_chat_consort_initiate_msg_offline_max_count = newRef.consort_chat_consort_initiate_msg_offline_max_count;
        consort_chat_ai_evaluate_reply_daily_max_count = newRef.consort_chat_ai_evaluate_reply_daily_max_count;
        consort_call_gain_child_simple_unlock_id = newRef.consort_call_gain_child_simple_unlock_id;
        inn_receive_guest_lazy_cd_id = newRef.inn_receive_guest_lazy_cd_id;
        inn_one_key_receive_guest_unlock_id = newRef.inn_one_key_receive_guest_unlock_id;
        inn_receive_cost_sec = newRef.inn_receive_cost_sec;
        inn_receive_gain_station_blueprint_wei = newRef.inn_receive_gain_station_blueprint_wei;
        inn_gain_station_blueprint_limit_interval = newRef.inn_gain_station_blueprint_limit_interval;
        inn_interval_gain_station_blueprint_limit = newRef.inn_interval_gain_station_blueprint_limit;
        inn_station_blueprint_item = newRef.inn_station_blueprint_item;
        inn_affection_item = newRef.inn_affection_item;
        inn_inline_guest_num_limit = newRef.inn_inline_guest_num_limit;
        inn_init_station_list = newRef.inn_init_station_list;
        inn_init_dish_list = newRef.inn_init_dish_list;
        treasure_hunt_lazy_cd_id = newRef.treasure_hunt_lazy_cd_id;
        treasure_hunt_premium_energy_common_item = newRef.treasure_hunt_premium_energy_common_item;
        treasure_hunt_advanced_energy_common_item = newRef.treasure_hunt_advanced_energy_common_item;
        treasure_hunt_akey_pickup_simple_unlock_id = newRef.treasure_hunt_akey_pickup_simple_unlock_id;
        treasure_hunt_instant_pickup_simple_unlock_id = newRef.treasure_hunt_instant_pickup_simple_unlock_id;
        treasure_hunt_akey_consume_energy_limit = newRef.treasure_hunt_akey_consume_energy_limit;
        treasure_hunt_advanced_energy_add_ore_quality_weight = newRef.treasure_hunt_advanced_energy_add_ore_quality_weight;
        treasure_hunt_advanced_ore_min_grade = newRef.treasure_hunt_advanced_ore_min_grade;
        treasure_hunt_pending_ore_num_limit = newRef.treasure_hunt_pending_ore_num_limit;
        treasure_hunt_premium_energy_num_limit = newRef.treasure_hunt_premium_energy_num_limit;
        treasure_hunt_treasure_guarantee_time = newRef.treasure_hunt_treasure_guarantee_time;
        treasure_hunt_ore_guarantee_quality = newRef.treasure_hunt_ore_guarantee_quality;
        treasure_hunt_ore_guarantee_min_time = newRef.treasure_hunt_ore_guarantee_min_time;
        treasure_hunt_ore_guarantee_min_weight = newRef.treasure_hunt_ore_guarantee_min_weight;
        treasure_hunt_ore_guarantee_max_time = newRef.treasure_hunt_ore_guarantee_max_time;
        treasure_hunt_ore_guarantee_max_weight = newRef.treasure_hunt_ore_guarantee_max_weight;
        treasure_hunt_treasure_replace_common_item = newRef.treasure_hunt_treasure_replace_common_item;
        grave_congratulate_show_time_ts = newRef.grave_congratulate_show_time_ts;
        grave_celebrate_reward_id = newRef.grave_celebrate_reward_id;
        grave_congratulate_reward_id = newRef.grave_congratulate_reward_id;
        grave_celebrate_fixed_cd_id = newRef.grave_celebrate_fixed_cd_id;
        grave_buff_list = newRef.grave_buff_list;
        grave_title_record_limit = newRef.grave_title_record_limit;
        grave_arena_round_reward_buff = newRef.grave_arena_round_reward_buff;
        grave_arena_round_reward_multi = newRef.grave_arena_round_reward_multi;
        grave_congratulate_reward_gain_fixed_cd = newRef.grave_congratulate_reward_gain_fixed_cd;
        guild_dungeon_allow_hour_range = newRef.guild_dungeon_allow_hour_range;
        guild_dungeon_auto_settle_time = newRef.guild_dungeon_auto_settle_time;
        guild_dungeon_reward_mail_id = newRef.guild_dungeon_reward_mail_id;
        guild_dungeon_log_limit = newRef.guild_dungeon_log_limit;
        guild_dungeon_recover_hero_cost = newRef.guild_dungeon_recover_hero_cost;
        guild_dungeon_recover_hero_limit = newRef.guild_dungeon_recover_hero_limit;
        guild_dungeon_attack_contri_v = newRef.guild_dungeon_attack_contri_v;
        guild_dungeon_attack_contri_limit = newRef.guild_dungeon_attack_contri_limit;
        guild_dungeon_tidedrop_max = newRef.guild_dungeon_tidedrop_max;
        guild_dungeon_tidedrop_divisor = newRef.guild_dungeon_tidedrop_divisor;
        guild_dungeon_tidedrop_power = newRef.guild_dungeon_tidedrop_power;
        guild_dungeon_tidedrop_constant = newRef.guild_dungeon_tidedrop_constant;
        mars_route_to_close_secs = newRef.mars_route_to_close_secs;
        mars_satisfaction_degree_init_per = newRef.mars_satisfaction_degree_init_per;
        mars_immigration_daily_max_num = newRef.mars_immigration_daily_max_num;
        mars_resident_npc_id_list = newRef.mars_resident_npc_id_list;
        mars_letter_limit = newRef.mars_letter_limit;
        mars_resident_dispatch_default_num = newRef.mars_resident_dispatch_default_num;
        mars_satisfaction_value_common_item = newRef.mars_satisfaction_value_common_item;
        mars_event_trigger_time = newRef.mars_event_trigger_time;
        mars_letter_refresh_time = newRef.mars_letter_refresh_time;
        mars_letter_refresh_num_once = newRef.mars_letter_refresh_num_once;
        mars_daily_help_refresh_num = newRef.mars_daily_help_refresh_num;
        mars_daily_help_refresh_time = newRef.mars_daily_help_refresh_time;
        mars_help_limit = newRef.mars_help_limit;
        mars_building_home_id = newRef.mars_building_home_id;
        mars_building_sec_to_diamond_ratio = newRef.mars_building_sec_to_diamond_ratio;
        mars_building_slot_people_count = newRef.mars_building_slot_people_count;
        mars_building_energy_output_item = newRef.mars_building_energy_output_item;
        mars_building_oxygen_coefficient = newRef.mars_building_oxygen_coefficient;
        mars_building_satiety_coefficient = newRef.mars_building_satiety_coefficient;
        mars_building_sleep_coefficient = newRef.mars_building_sleep_coefficient;
        mars_building_comfort_coefficient = newRef.mars_building_comfort_coefficient;
        mars_building_mood_coefficient = newRef.mars_building_mood_coefficient;
        mars_building_oxygen_yield_per_consume = newRef.mars_building_oxygen_yield_per_consume;
        mars_building_satiety_yield_per_consume = newRef.mars_building_satiety_yield_per_consume;
        mars_building_sleep_yield_per_consume = newRef.mars_building_sleep_yield_per_consume;
        mars_building_comfort_yield_per_consume = newRef.mars_building_comfort_yield_per_consume;
        mars_building_mood_yield_per_consume = newRef.mars_building_mood_yield_per_consume;
        mars_building_people_cure_rate = newRef.mars_building_people_cure_rate;
        mars_building_cure_time_gap_sec = newRef.mars_building_cure_time_gap_sec;
        mars_building_oxygen_index_max = newRef.mars_building_oxygen_index_max;
        mars_building_satiety_index_max = newRef.mars_building_satiety_index_max;
        mars_building_sleep_index_max = newRef.mars_building_sleep_index_max;
        mars_building_comfort_index_max = newRef.mars_building_comfort_index_max;
        mars_building_mood_index_max = newRef.mars_building_mood_index_max;
        mars_satisfaction_degree_reward_mail_id = newRef.mars_satisfaction_degree_reward_mail_id;
        mars_satisfaction_degree_reward_mail_simple_unlock_id = newRef.mars_satisfaction_degree_reward_mail_simple_unlock_id;
        mars_building_temp_queue_time_price_type = newRef.mars_building_temp_queue_time_price_type;
        mars_temp_building_queue_gain_buff_item = newRef.mars_temp_building_queue_gain_buff_item;
        mars_go_to_finish_marquee_id = newRef.mars_go_to_finish_marquee_id;
        mars_building_people_id = newRef.mars_building_people_id;
        mars_explore_refresh_event_type_wei_list = newRef.mars_explore_refresh_event_type_wei_list;
        mars_explore_daily_refresh_special_event_num = newRef.mars_explore_daily_refresh_special_event_num;
        mars_explore_cd = newRef.mars_explore_cd;
        mars_explore_boss_pos_list = newRef.mars_explore_boss_pos_list;
        mars_explore_boss_unlock_lvl = newRef.mars_explore_boss_unlock_lvl;
        mars_explore_daily_refresh_fixed_cd = newRef.mars_explore_daily_refresh_fixed_cd;
        mars_explore_team_hero_max_num = newRef.mars_explore_team_hero_max_num;
        mars_explore_team_loss_min_per = newRef.mars_explore_team_loss_min_per;
        mars_explore_team_loss_max_per = newRef.mars_explore_team_loss_max_per;
        mars_explore_team_repair_unit_ms = newRef.mars_explore_team_repair_unit_ms;
        mars_explore_team_repair_unit_cost_list = newRef.mars_explore_team_repair_unit_cost_list;
        repair_power_basic = newRef.repair_power_basic;
        mars_home_output_remain_max_time_s = newRef.mars_home_output_remain_max_time_s;
        mars_explore_team_base_cost_per = newRef.mars_explore_team_base_cost_per;
        mars_explore_team_fail_cost_per = newRef.mars_explore_team_fail_cost_per;
        mars_explore_team_win_cost_per = newRef.mars_explore_team_win_cost_per;
        mars_explore_team_win_red_per = newRef.mars_explore_team_win_red_per;
        mars_explore_team_fail_cost_per_max = newRef.mars_explore_team_fail_cost_per_max;
        mars_explore_pvp_record_save_limit = newRef.mars_explore_pvp_record_save_limit;
        mars_power_rank_id = newRef.mars_power_rank_id;
        mars_go_route_arrive_reduce_list = newRef.mars_go_route_arrive_reduce_list;
        mars_explore_team_power_coef = newRef.mars_explore_team_power_coef;
        mars_mine_secs = newRef.mars_mine_secs;
        mars_mine_mine_new_per = newRef.mars_mine_mine_new_per;
        mars_mine_num_min = newRef.mars_mine_num_min;
        mars_mine_num_max = newRef.mars_mine_num_max;
        stage_goal_first_reach_detail_list_show_count = newRef.stage_goal_first_reach_detail_list_show_count;
        earnings_marquee_config = newRef.earnings_marquee_config;
        earnings_marquee_ref_id = newRef.earnings_marquee_ref_id;
        guild_mars_help_deal_reward_item = newRef.guild_mars_help_deal_reward_item;
        guild_mars_help_deal_reward_fixed_cd_id = newRef.guild_mars_help_deal_reward_fixed_cd_id;
        store_reviews_trigger_common_item = newRef.store_reviews_trigger_common_item;
        store_reviews_reward_mail_id = newRef.store_reviews_reward_mail_id;
        store_reviews_reward_item_list = newRef.store_reviews_reward_item_list;
        mars_explore_lead_soldier_cut_coefficient = newRef.mars_explore_lead_soldier_cut_coefficient;
        mars_mine_guild_share_limit = newRef.mars_mine_guild_share_limit;
        guild_box_dispatch_mail_id = newRef.guild_box_dispatch_mail_id;
        guild_mars_help_auto_deal_buff_id = newRef.guild_mars_help_auto_deal_buff_id;
        rush_exchange_done_need_wait_sec = newRef.rush_exchange_done_need_wait_sec;
        rush_exchange_refresh_sec = newRef.rush_exchange_refresh_sec;
        rush_exchange_day_can_exchange_times = newRef.rush_exchange_day_can_exchange_times;
        midday_dungeon_system_log_id = newRef.midday_dungeon_system_log_id;
        evening_dungeon_system_log_id = newRef.evening_dungeon_system_log_id;
        recruit_gain_hero_or_consort_marquee_id = newRef.recruit_gain_hero_or_consort_marquee_id;
        mars_go_route_done_box_id = newRef.mars_go_route_done_box_id;
        mars_go_route_done_chat_system_log = newRef.mars_go_route_done_chat_system_log;
        arrive_space_box_id = newRef.arrive_space_box_id;
        arrive_space_chat_system_log = newRef.arrive_space_chat_system_log;
        default_player_room_skin = newRef.default_player_room_skin;
        activity_team_apply_limit = newRef.activity_team_apply_limit;
        order_send_item_mail_id = newRef.order_send_item_mail_id;
        voucher_item_bag_item_id = newRef.voucher_item_bag_item_id;
        lover_collect_lover_ids = newRef.lover_collect_lover_ids;
        lover_collect_need_earn_speed = newRef.lover_collect_need_earn_speed;
    }

    /**********
     * 获取对象数据Id，尽量唯一
     *
     * @author alzq.z
     * @time 2019年4月3日 下午11:35:24
     */
    @Override
    public long Id()
    {
        return 0;
    }

    //general_process覆盖完成后调用
    public boolean NewAssert()
    {
        return true;
    }

    /// /////////////////////////// NP留存配置 //////////////////////////////

    //玩家初始化相关
    public NPPlayerPropertyModifier player_property;//玩家属性
    public ArrayList<NPCommonCostItem> player_init_give_item = WCGResCommon.parseList(NPCommonCostItem.class, "CURRENCY:2:100");//玩家初始给与物品
    public long player_init_mail_id = 1000;//玩家初始化收到的邮件id
    public ArrayList<NPCommonCostItem> player_init_mail_give_item = new ArrayList<>();//玩家初始化收到的邮件奖励

    public int mail_max_lock_num = 20;//邮件最大收藏数量
    public int mail_max_num = 100;//最大邮件数量

    //玩家重命名消耗，列表每一个消耗表示不同的消耗优先级，其中一个消耗满足即可成功改名
    public ArrayList<NPCommonCostItem> player_info_rename_cost_list;

    //好友系统相关
    public int friend_apply_limit = 50;//玩家好友申请保留数量上限
    public int friend_apply_avaiable_secs = 3600;//好友申请有效时长秒数

    //============================================GOE================================================
    
    //系统全局参数（服务端自行定制）
    @RefField(isIgnore = true)
    public int paramLimit = 99999;
    
    //大臣系统
    public NPPlayerConditionGroupObj hero_level_up_ten_times_condition; //大臣一键提升十级操作的条件

    //藏品系统
    public long equip_upgrade_ten_times_simple_unlock_id; //十连升级simple_unlock_id
    public int equip_normal_cost_group_id; //普通重塑消耗组id
    public NPCommonCostItem equip_advance_cost; //高级重塑消耗
    public int equip_normal_add_pro_group_id; //普通重塑加成概率组ID
    public int equip_advance_add_pro_group_id; //高级重塑加成概率组ID

    //情人系统
    public int consort_rand_call_cd = 0; //cd配表ID，情人随机宠幸消耗的CD
    public WCGPairInt consort_call_rand_cg_per; //【家人】随机邀约触发cg权重，没有cg;有cg
    @RefField(isIgnore = true)
    public WeightValueList<Boolean> consortCallRandCgPer = new WeightValueList<>(); //【家人】随机邀约触发cg权重，没有cg;有cg
    public ArrayList<Integer> consort_call_rand_per = new ArrayList<>(); //【家人】随机邀约生子权重权重，无子嗣:有子嗣:有子嗣且是卷王
    @RefField(isIgnore = true)
    public WeightValueList<EChildBirthRes> consortCallRandPerWeight = new WeightValueList<>(); //【家人】随机邀约生子权重权重，无子嗣:有子嗣:有子嗣且是卷王
    public NPRefreshTimeObj consort_call_gem_reset = new NPRefreshTimeObj();//【情人】指定问候打折次数重置时间
    public int consort_reshape_skill_max_count = 10;//强制玩家前x次必定达不到25%

    //GOB-9080【优化-1】家人CG获得概率配置优化 https://www.teambition.com/task/69840f4acb7f75f7ad242b24
    public long consort_call_new_cg_fixed_cd;
    
    //子嗣系统
    public int child_cal_unit_per = 1000;//计算收益的单位万分比
    public int child_add_per = 10000;//计算收益基础倍数(万分比)
    public int child_train_add_per = 50000;//培养阶段的计算收益倍数(万分比)
    public NPCommonItem child_seat_recover_item;//回复脑力的道具
    public int child_seat_recover_S = 3600; //回复1点脑力所需的时间（秒）
    public int unmarry_adult_limit = 20; //毕业子嗣上限
    public int married_adult_limit = 50; //已联谊子嗣保留数据上限
    public int marry_apply_expired_S = 86400;//【子嗣】联姻申请时限(s)	
    public int child_married_bonus_add_per = 50000;//已婚子嗣带来的加成倍数上限
    public long child_married_gift_mail_id = 104;//【子嗣】联姻礼物下发邮件id
    public int giftde_child_dinner_permit_per = 0;//卷王子嗣毕业后获得宴会凭证的概率（万分比）

    //关卡系统
    public long unlock_chapter_simple_unlock_id;//解锁关卡条件simple_unlock
    public int gold_inspire_increase_power_ratio_per;//金币鼓舞每次增加战力比例万分比
    public int crystal_inspire_increase_power_ratio_per;//钻石鼓舞每次增加战力比例万分比
    public int item_inspire_increase_power_ratio_per;//道具鼓舞每次增加战力比例万分比
    public int gold_inspire_calculate_ratio_time_price_id;//金币鼓舞计算比例time_price_id
    public NPCommonCostItem crystal_inspire_fixed_cost;//钻石鼓舞固定消耗
    public NPCommonCostItem item_inspire_cost;//道具鼓舞cost_item
    public WCGPairInt forward_gold_cost_ratio_range;//前进消耗金币比例范围
    public int critical_hit_coefficient;//暴击系数（万分比）

    //政务
    public long anecdote_first_event = 108;//政务首次事件ID
    public int anecdote_lazy_cd = 0;//政务事件使用的CD，等同当前可以处理的政务事件数量
    public WCGPairIntList anecdote_pos_group_max_event_count;//各pos组的事件数量上限（格式：组id:上限;组id1:上限1），-1表示不限制

    //任务组件
    public ArrayList<Long> quest_init_open_list = new ArrayList<>(); //玩家默认开启的任务列表
    public NPPlayerConditionGroupObj daily_quest_one_key_finish_condition;//日常任务一键完成解锁条件
    
    public long default_quest_id; //【主线任务】条件判断找不到任务id时的默认id	

    //每日签到
    public NPRefreshTimeObj daily_check_refresh_clock;//每日签到刷新规则
    public long daily_check_once_reward_id;//每日签到奖励(reward_id)
    public int daily_check_dessert_num;//甜品数量
    public int daily_check_timeout;//超过x天登录更换问候语
    public int daily_check_extra_dessert_num;//超过x天后额外甜品数量
    public long daily_check_default_consort_id;//每日签到默认情人id
    public int daily_check_step_show_num = 5;//【每日签到】每日签到阶段展示数量

    //称号
    public int player_title_available_time_max;//玩家称号有效时间最多累加的上限时间

    //游历
    public long travel_cost_lazycd_id;//【游历】游历消耗的lazycd_id
    public long travel_one_key_simple_unlock_id;//【游历】一键游历解锁条件
    public ETravelEventType travel_default_event_type = ETravelEventType.NONE;//【游历】游历无可用事件时的默认事件类型
    public int travel_akey_event_limit = 10;//【游历】一键游历事件上限
    public ArrayList<WCGPairLong> early_travel_trigger_event_list = new ArrayList<>();//【游历】前期游历触发的事件列表(格式 事件id:位置id，会依次先触发完配置的事件,全部触发完后才会走随机)

    //集市
    public long market_akey_simple_unlock_id;//【集市】一键经营条件解锁simple_unlockID
    public NPRefreshTimeObj market_reset = new NPRefreshTimeObj();//【集市】重置时间-时间点(ENPTimeRefreshType)
    public int market_reset_num;//【集市】重置次数
    public int market_shop_cd_sec;//【集市】回复一点cd的时间
    public NPCommonItem market_up_cost_item = new NPCommonItem();//【集市】升级消耗物品
    public NPCommonCostItem market_shop_restore_cd_costitem = new NPCommonCostItem();//【集市】店铺恢复一次经营消耗的道具

    //跑马灯
    public int marquee_limit = 30;//每个窗口展示跑马灯数量上限
    public long marquee_gacha_draw_item_marquee_id;//抽卡获得道具跑马灯id
    public long marquee_gain_ur_museum_item_marquee_id;//开盲盒或抽奖获得UR藏品跑马灯id
    public long marquee_gain_ur_hero_marquee_id;//获得UR顾问跑马灯id
    public long marquee_gain_ur_consort_marquee_id;//获得UR情人跑马灯id
    public long marquee_first_reach_big_stage_marquee_id;//全服首位到达新大阶段跑马灯id

    //好友推荐
    public int friend_recommend_num = 10;//【好友推荐】数量
    public WCGPairInt friend_recommend_power_interval = new WCGPairInt(8000, 12000);//【好友推荐】国力区间
    public WCGPairInt friend_recommend_lvl_interval = new WCGPairInt(2, 2);//【好友推荐】等级区间

    //常驻排行榜
    public long receive_like_score;//单次点赞获得点赞积分
    public RangeRandomWeightList like_range;//点赞随机区间：权重
    public int rank_show_max_num;//排行榜最多显示人数
    public long rank_fixed_akey_simple_unlock_id;//一键点赞解锁条件

    public long activity_step_reward_mail_id;//活动阶段奖励补发邮件id
    public long activity_rank_reward_mail_id;//活动排行奖励补发邮件id

    //背包道具使用相关
    public int bag_auto_use_max_count;//背包批量使用道具的时候默认直接使用的最大数量
    public int batch_use_item_max_count = 999;//背包批量使用道具最大数量
    public int activity_bag_item_expired_mail_id;//活动道具过期邮件

    //GOD-4144 【优化-1】庭园拜访 https://www.teambition.com/task/6645a901b4fb92ca05fb7348
    public ArrayList<NPCommonCostItem> visit_other_player_gain_item_list = new ArrayList<>();

    //周卡相关
    public int week_card_offline_hosting_max_profit_time_sec;//离线托管最大收益时间（秒）
    public int week_card_offline_active_min_time_sec;//周卡在玩家离线超过x秒后才会结算
    public int week_card_free_trial_time_sec;//周卡免费试用时长（秒）
    public long week_card_func_simple_unlock_id;//周卡功能解锁条件

    //GOD-3458 【优化-0】聊天-聊天频道增加屏蔽功能 https://www.teambition.com/task/66272546d52f89720d411a46
    public int shield_cid_limit = 100;//允许屏蔽的玩家数量上限

    public long item_expired_mail_id;//在存在有效期的item过期时发放邮件通知
    public long questionnaire_mail_id;//问卷奖励补发邮件id
    public long collect_likes_fixed_id;//每日点赞限制次数的fixedCD_id

    //联盟相关
    public List<NPCommonCostItem> guild_change_name_cost;//修改联盟名称消耗
    public WCGPairInt guild_name_length_limit;//联盟名称字符长度限制
    public WCGPairInt guild_simple_name_length_limit;//联盟简称字符长度限制
    public WCGPairInt guild_declaration_length_limit;//联盟宣言字符长度限制
    public WCGPairInt guild_announcement_length_limit;//联盟公告字符长度限制
    public List<NPCommonCostItem> guild_create_cost;//创建联盟消耗
    public List<NPCommonCostItem> guild_first_time_join_reward_list;//首次加入联盟奖励
    public List<NPCommonCostItem> guild_change_flag_cost;//修改联盟旗帜消耗
    public int guild_free_join_cd_num;//免cd加入联盟次数
    public int guild_join_cd_hours;//退盟后加入联盟cd（小时）
    public int guild_join_request_limit_num;//玩家入盟请求上限条数
    public int guild_leader_proactive_transfer_cd_hours;//盟主主动转让CD（小时）
    public int guild_leader_transfer_target_online_within_hours;//盟主转让目标在线时间在x小时以内
    public int guild_leader_trigger_passive_transfer_offline_beyond_hours;//盟主触发被动转让离线时间需超过x小时
    public int guild_leader_impeach_offline_beyond_hours;//盟主被弹劾需要离线超过x小时
    public int guild_leader_impeach_message_available_within_hours;//盟主弹劾信息有效期
    public long guild_upgrade_mail_id;//联盟升级奖励邮件id
    public long guild_first_time_join_success_mail_id;//首次加入联盟奖励邮件id
    public long guild_join_success_mail_id;//加入联盟成功通知邮件id
    public long guild_passive_exit_mail_id;//被动退出联盟通知邮件id
    public long guild_position_change_mail_id;//联盟职位变更通知邮件id
    public long guild_broadcast_message_daily_limit_fixcd_id;//联盟群发消息每日免费次数fixCdId
    public List<NPCommonCostItem> guild_broadcast_message_cost;//联盟群发次数用完后消耗
    public WCGPairInt guild_broadcast_message_length_limit;//联盟群发字符长度限制
    public long guild_rank_id;//联盟排行榜id（rank表）
    public long guild_impeach_succ_need_percent;//联盟弹劾盟主成功所需成员百分比
    public long guild_log_chat_npc_id;//联盟日志聊天npcid
    public int guild_open_recruit_gap_sec;//联盟公开招募间隔时间（秒）
public long guild_entrust_reward_mail_id;//发放杂物委托奖励邮件id
    public int deal_entrust_lazy_cd_id;//杂物委托处理lazy_cd
    public WeightIntValueList guild_entrust_crit_mul_weight = new WeightIntValueList();//联盟委托暴击倍数权重，格式：倍数:权重;倍数:权重（如2:5000;3:3000;5:1000）
    public int each_attr_can_dispatch_hero_num;//每个相性可派遣大臣数量
    public NPCommonItem guild_exp_common_item = new NPCommonItem();//联盟经验common_item
    public NPCommonItem guild_wealth_common_item = new NPCommonItem();//联盟财富common_item
    public NPCommonItem personal_contribution_common_item = new NPCommonItem();//【联盟】个人贡献common_item
    public long guild_rank_fixed_id;//联盟常驻排行榜对应rank_fixed_id
    
    //联盟宝箱
    public long guild_box_claim_fixed_cd_id;//【联盟宝箱】可领取免费宝箱的最大次数
    public int guild_box_effect_secs = 60;//【联盟宝箱】宝箱有效时长（秒）
    public int guild_box_active_effect_secs = 60;//【联盟宝箱】活跃宝箱有效时长（秒），默认7天

    //公会协助
    public NPCommonCostItem guild_cooperate_dispatch_time_reset_cost;//公会协助派遣次数重置消耗
    public long guild_cooperate_dispatch_time_reset_limit_fix_cd_id;//公会协助派遣次数重置上限fixCdId
    public int guild_cooperate_dispatch_property_point_hp_per;//【公会协助】公会币和公会贡献奖励产出判定的据点血量万分比
    public long guild_cooperate_dispatch_guild_coin_reward_min;//【公会协助】玩家建设值未达到据点血量n%时的最低公会币奖励
    public int guild_cooperate_dispatch_guild_devote_reward_min;//【公会协助】玩家建设值未达到据点血量n%时的最低公会贡献奖励
    public long guild_cooperate_dispatch_can_gain_guild_devote_time_fix_cd_id;//公会协助派遣可获得工会贡献次数fixCdId
    public long guild_cooperate_dispatch_can_gain_guild_coin_time_fix_cd_id;//公会协作派遣可获得公会币次数fixCd
    public NPRefreshTimeObj guild_cooperate_refresh_time;//公会协助重置时间
    public int guild_cooperate_dispatch_same_property_add_per;//公会协助派遣同属性加成万分比
    public int guild_cooperate_log_max_count = 1000;//公会协助日志最大保存数量
    public NPCommonItem guild_cooperate_construction_common_item = new NPCommonItem();//联盟协作建设值common_item

    //建筑相关
    public int business_building_res_max_timeS;//建筑每秒收益的最大时长（秒）
    public long farming_building_click_spaceMS;//点击农田建筑获取收益的时间间隔（毫秒）
    public int farming_building_trigger_multiple_daily_limit;//农田建筑触发暴击倍数每日次数上限

    //宴会相关
    public int dinner_deleting_contact_record_deadline = 30;//互宴记录n天没有来往删除
    public int dinner_open_record_save_limit = 50;//创建宴会记录保存最大数量
    public long dinner_npc_join_cost_id;//NPC赴宴的消耗配置ID
    @RefField(isIgnore = true)
    public RefDinnerJoinCost dinnerNpcJoinCostRef;
    public long dinner_owner_settle_mail_id;//开宴玩家结算邮件id

    //竞技场相关
    public List<Long> arena_initial_choose_buff_list;//竞技场初始可选择临时增益
    public List<Long> arena_choose_buff_list;//竞技场可选择临时增益
    @RefField(isIgnore = true)
    public RefArenaBuff[] arenaChooseBuffListRef = new RefArenaBuff[EArenaBuffType.EArenaBuffType_Length];//竞技场可选择临时增益
    public int arena_random_attack_free_limit;//竞技场随机挑战免费次数
    public int arena_random_attack_crystal_buy_limit;//竞技场随机挑战钻石购买次数上限 每日
    public int arena_random_attack_crystal_buy_ratio;//竞技场随机挑战钻石购买计算系数 每增加N个伙伴可额外获得1次购买次数的机会
    public int arena_random_attack_crystal_buy_time_price_id;//竞技场随机挑战钻石购买TimePriceId
    public WCGPairInt arena_random_attack_match_interval;//竞技场随机挑战匹配范围
    public int arena_select_attack_daily_limit;//竞技场指定挑战每日上限
    public int arena_celebrity_rank_up_need_defeat_hero;//竞技场登上名人榜需要击败对方伙伴数量
    public int arena_celebrity_rank_limit_num;//竞技场名人榜上限
    public long arena_one_key_attack_simple_unlock_id;//竞技场一键谈判SimpleUnlockId
    public long arena_rank_fixed_id;//竞技场排行榜id
    public NPCommonItem arena_station_output_item;//竞技场贸易站产出物品（commonitem）
    public int arena_gain_influence_for_defeating_each_hero;//竞技场每击败1名伙伴数量获得的商会影响力
    public int arena_deduct_influence_for_each_hero_defeated;//竞技场每被击败1名伙伴数量扣除的商会影响力
    public int arena_gain_coin_for_defeating_each_hero;//竞技场每击败1名伙伴数量获得的商会硬币数
    public int arena_initial_influence;//竞技场商会影响力起始值
    public int arena_battle_report_limit;//竞技场战报数量上限
    public int arena_fight_back_limit;//竞技场反击数量上限
    public List<Long> arena_bot_hero_list;//竞技场机器人大臣列表
    public long arena_first_experience_bot_template_id;//竞技场首次体验机器人模板id
    public int unlock_arena_when_hero_num_equals;//拥有伙伴数量达到N个解锁竞技场
    
    public int arena_station_privilege_collect_limit_time_sec = 86400;//竞技场贸易站有特权卡时累积时间上限（秒）
    public long arena_station_privilege_collect_limit_num = 999999999999L;//竞技场贸易站有特权卡时累积数量上限
    public long arena_station_privilege_permissions_id;//竞技场贸易站特权权限id

    //GOB-9329 【优化-0】增加触发引导-----竞技场使用谈判通告效果支持 https://www.teambition.com/task/69a7fa23d799eba16c0d1e77
    //为系统用户构造大臣列表，用于竞技场引导时使用
    public ArenaHeroObjList arena_us_hero_list = new ArenaHeroObjList();
    @RefField(isIgnore = true)
    public Hero_ArenaShowList arenaUsHeroList = new Hero_ArenaShowList();

    //玩家相关
    public long playerLvl_daily_reward_mail_id;//补发玩家升级未领取评级奖励邮件id
    public int seven_days_login_reward_timeprice_id;//七日登录奖励timepriceId
    public long mail_plan_mail_id;//邮件计划奖励邮件id

    //爬塔相关
    public long tower_coin_daily_reward_mail_id = 103;//【迷宫】每日迷宫币奖励邮件id	
    public long tower_unlock_simple_unlock_id = 21011;//【迷宫】迷宫入口解锁simple_unlock

    public int tower_defence_report_limit = 50;//防守战报存储上限（数量）
    public int tower_defence_report_time_limit = 7;//防守战报存储上限（天数）

    //活动相关
    public int activity_default_settle_duration_sec = 3600;//活动默认结算期时长

    //午间副本相关
    public NPRefreshTimeObj midday_dungeon_preview_fight_time = new NPRefreshTimeObj();//【午间副本】预告时间
    public NPRefreshTimeObj midday_dungeon_start_fight_time = new NPRefreshTimeObj();//【午间副本】开始时间
    public NPRefreshTimeObj midday_dungeon_end_fight_time = new NPRefreshTimeObj();//【午间副本】结束时间
    public long midday_dungeon_rank_fixed_id;//【午间副本】常驻排行榜id
    public long midday_dungeon_can_borrow_guild_hero_num_fixed_cd_id;//【午间副本】可以借用联盟派遣大臣数量（fixedCdId）
    public long midday_dungeon_auto_fight_simple_unlock_id;//【午间副本】自动战斗功能解锁条件id
    public int midday_dungeon_gold_profit_ratio;//【午间副本】金币奖励收益倍数（万分比）
    public int midday_dungeon_server_round_drop_box_limit;//【午间副本】全服单场掉落宝箱次数上限

    //晚间副本相关
    public NPRefreshTimeObj evening_dungeon_preview_fight_time = new NPRefreshTimeObj();//【晚间副本】预告时间
    public NPRefreshTimeObj evening_dungeon_start_fight_time = new NPRefreshTimeObj();//【晚间副本】开始时间
    public NPRefreshTimeObj evening_dungeon_end_fight_time = new NPRefreshTimeObj();//【晚间副本】结束时间
    public NPRefreshTimeObj evening_dungeon_close_fight_time = new NPRefreshTimeObj();//【晚间副本】关闭时间
    public int evening_dungeon_boss_respawn_times_limit;//【晚间副本】单场boss复活上限次数
    public int evening_dungeon_boss_respawn_sec;//【晚间副本】boss复活时长（秒）
    public long evening_dungeon_boss_initial_blood;//【晚间副本】boss初始血量
    public int evening_dungeon_boss_respawn_reference_times;//【晚间副本】boss复活次数判定值 根据当日boss复活是否超过判定次数, 超过则增加一定比例; 增加比例根据开服天数而不同
    public long evening_dungeon_auto_fight_simple_unlock_id;//【晚间副本】自动战斗解锁条件id
    public List<NPCommonCostItem> evening_dungeon_day_first_kill_boss_reward;//【晚间副本】首次击杀boss奖励
    public List<NPCommonCostItem> evening_dungeon_day_kill_boss_reward;//【晚间副本】非首次击杀boss奖励
    public long evening_dungeon_rank_reward_mail_id;//【晚间副本】排行奖励邮件id
    public int evening_dungeon_gold_profit_ratio;//【晚间副本】金币奖励收益倍数（万分比）
    public int evening_dungeon_rank_id;//【晚间副本】排行版模板id

    //常驻排行榜相关
    public long earning_rank_id;//常驻赚速排行榜id

    public long earning_goal_mail_id;//赚速目标活动奖励补发邮件id
    public long seven_day_goals_mail_id;//七日目标活动奖励补发邮件id

    public int consort_chat_ai_send_time_price_id;//AI聊天发送time price id
    public long consort_chat_ai_send_fixed_cd_id;//AI聊天发送固定cd id
    public long consort_chat_moment_daily_max_count;//朋友圈每天最大条数
    public long consort_chat_moment_reply_daily_max_count;//朋友圈回复每日最大次数
    public String consort_ai_chat_default_language;//妃子ai对话默认语言
    public long consort_chat_ai_consort_initiate_msg_daily_max_count;//妃子ai每日主动发起对话最大次数
    public long consort_chat_consort_initiate_msg_offline_max_count;//妃子ai离线期间主动发起对话最大次数
    public long consort_chat_ai_evaluate_reply_daily_max_count;//AI评价回复每日最大次数

    //GOB-9270【优化-0】情人随机约会-概率获得学徒增加前置条件 https://www.teambition.com/task/69a5360031a5fbf86ef1fab1
    public long consort_call_gain_child_simple_unlock_id;

    //旅店相关
    public int inn_receive_guest_lazy_cd_id;//迎宾LazyCdId
    public long inn_one_key_receive_guest_unlock_id;//一键迎宾的SimpleUnlockId
    public int inn_receive_cost_sec;//迎宾消耗时间 秒
    public int inn_receive_gain_station_blueprint_wei = 5000;//迎宾获得厨具图纸概率(万分比)
    public int inn_gain_station_blueprint_limit_interval = 100;//迎宾图纸获得上限对应区间值
    public int inn_interval_gain_station_blueprint_limit = 50;//迎宾数量区间内图纸获得上限
    public NPCommonItem inn_station_blueprint_item;//厨具图纸对应道具
    public NPCommonItem inn_affection_item;//心意值对应道具
    public long inn_inline_guest_num_limit = 5000;//旅店队列中的客人数量上限
    public ArrayList<Long> inn_init_station_list = new ArrayList<>();//初始获得的设施列表
    public ArrayList<Long> inn_init_dish_list = new ArrayList<>();//初始获得的菜品列表

    //太空寻宝
    public int treasure_hunt_lazy_cd_id;//挂机体力对应lazy_cd
    public NPCommonItem treasure_hunt_premium_energy_common_item;//普通能源对应common_item
    public NPCommonItem treasure_hunt_advanced_energy_common_item;//高级能源对应common_item
    public int treasure_hunt_akey_pickup_simple_unlock_id;//一键拾取功能的解锁条件
    public int treasure_hunt_instant_pickup_simple_unlock_id;//快速跳过单次拾取的解锁条件simple_unlock id
    public int treasure_hunt_akey_consume_energy_limit;//单次多倍拾取次数上限
    public QualityValueData treasure_hunt_advanced_energy_add_ore_quality_weight = new QualityValueData();//高级能源的矿石品质增加权重列表
    public int treasure_hunt_advanced_ore_min_grade;//高级矿石需要的最低档位索引（从0开始）
    public int treasure_hunt_pending_ore_num_limit;//待处理矿石数量上限
    public int treasure_hunt_premium_energy_num_limit;//普通能源数量上限 领取的时候检查，当前已超过则不可领
    public int treasure_hunt_treasure_guarantee_time;//n次未随机到奇物时触发保底(下次必获得奇物)
    public EQuality treasure_hunt_ore_guarantee_quality;//矿石保底品质
    public int treasure_hunt_ore_guarantee_min_time;//n次未随机到高品质以上矿石时触发保底下限
    public WeightQualityValueList treasure_hunt_ore_guarantee_min_weight;//触发保底下限时的矿石品质随机权重,无视玩法内的概率加成
    public int treasure_hunt_ore_guarantee_max_time;//连续n次随机到高品质以上矿石时触发保底上限
    public WeightQualityValueList treasure_hunt_ore_guarantee_max_weight;//触发保底上限时的矿石品质随机权重,无视玩法内的概率加成
    public List<NPCommonCostItem> treasure_hunt_treasure_replace_common_item;//奇物重复获得时转换的道具奖励

    //杰出者相关
    public int grave_congratulate_show_time_ts = 3600;//【杰出者大厅】新晋杰出者展示时长(秒)
    public long grave_celebrate_reward_id;//【杰出者大厅】膜拜奖励id
    public long grave_congratulate_reward_id;//【杰出者大厅】新晋杰出者膜拜奖励id
    public long grave_celebrate_fixed_cd_id;//【杰出者大厅】膜拜次数CD
    public WCGPairLongList grave_buff_list = new WCGPairLongList();//【杰出者大厅】buff列表
    @RefField(isIgnore = true)
    public ArrayList<Long> graveTitleIdList = new ArrayList<>();//【杰出者大厅】所有称号列表
    @RefField(isIgnore = true)
    public NPRefreshTimeObj graveBuffEndTimeObj = new NPRefreshTimeObj(ENPTimeRefreshType.REF_CLOCK, 0);
    public int grave_title_record_limit = 10;//【杰出者大厅】称号历史记录数量上限
    public long grave_arena_round_reward_buff;//对应buff：谈判连胜后有{0}%的几率额外获得{grave_arena_round_reward_multi}倍连胜宝箱的奖励
    public int grave_arena_round_reward_multi = 1;//搭配 grave_arena_round_reward_buff，用于奖励倍数
    
    public long grave_congratulate_reward_gain_fixed_cd;

    //公会PVE
    public WCGPairInteger guild_dungeon_allow_hour_range = new WCGPairInteger();//公会副本允许开启的时间（小时）范围
    public NPRefreshTimeObj guild_dungeon_auto_settle_time = new NPRefreshTimeObj();//公会副本每日自动结算时间
    public long guild_dungeon_reward_mail_id;//奖励补发邮件ID
    public int guild_dungeon_log_limit = 100;//公会副本日志数量长度
    public NPCommonCostItem guild_dungeon_recover_hero_cost = new NPCommonCostItem();//公会副本恢复大臣出战次数的消耗
    public int guild_dungeon_recover_hero_limit = 1;//大臣消耗道具恢复次数上限
    public int guild_dungeon_attack_contri_v = 1;//公会副本攻击获得公会贡献度
    public int guild_dungeon_attack_contri_limit = 20;//公会副本获得公会贡献度的攻击次数上限
    public int guild_dungeon_tidedrop_max = 400;//【公会副本】公会币掉落公式-最大值
    public int guild_dungeon_tidedrop_divisor = 200;//【公会副本】公会币掉落公式-伤害除数
    public int guild_dungeon_tidedrop_power = 4000;//【公会副本】公会币掉落公式-次方系数
    public int guild_dungeon_tidedrop_constant = 15;//【公会副本】公会币掉落公式-补偿常数

    //火星系统
    public int mars_route_to_close_secs = 10;//【前往火星】距离下一个阶段提前秒数
    public int mars_satisfaction_degree_init_per;//【火星基地】满意度初始万分比
    public int mars_immigration_daily_max_num;//【火星基地】火星每日移民最大数量
    public ArrayList<Long> mars_resident_npc_id_list = new ArrayList<>();//【火星基地】居民npc id列表
    public int mars_letter_limit = 10;//【火星基地】日常信件数量上限
    public int mars_resident_dispatch_default_num = 10;//【火星基地】居民派遣默认数量
    public NPCommonItem mars_satisfaction_value_common_item = new NPCommonItem();//【火星基地】满意值道具
    public int mars_event_trigger_time = 10800;//【火星基地】事件触发时间
    public int mars_letter_refresh_time = 1800;//【火星基地】日常信件刷新时间
    public int mars_letter_refresh_num_once = 1;//【火星基地】日常信件单次刷新数量
    public WCGPairInt mars_daily_help_refresh_num = new WCGPairInt();//【火星基地】每日求助刷新次数(choice:reward)
    public int mars_daily_help_refresh_time = 1800;//【火星基地】每日求助刷新时间
    public int mars_help_limit = 10;//【火星基地】求助数量上限
    public long mars_building_home_id;//【火星基地】主基地的建筑 id
    public int mars_building_sec_to_diamond_ratio = 60;//【火星基地】钻石兑换比 1 钻石兑换多少（秒）
    public int mars_building_slot_people_count = 10;//【火星基地】派遣槽位容纳人数
    public NPCommonItem mars_building_energy_output_item = new NPCommonItem();//【火星基地】能源建筑产出的资源类型
    public int mars_building_oxygen_coefficient = 0;//氧气系数
    public int mars_building_satiety_coefficient = 0;//饱腹系数
    public int mars_building_sleep_coefficient = 0;//睡眠系数
    public int mars_building_comfort_coefficient = 0;//舒适系数
    public int mars_building_mood_coefficient = 0;//心情系数
    public int mars_building_oxygen_yield_per_consume = 0;//每人口消耗氧气值
    public int mars_building_satiety_yield_per_consume = 0;//每人口消耗饱腹值
    public int mars_building_sleep_yield_per_consume = 0;//每人口消耗睡眠值
    public int mars_building_comfort_yield_per_consume = 0;//每人口消耗舒适值
    public int mars_building_mood_yield_per_consume = 0;//每人口消耗心情值
    public int mars_building_people_cure_rate;//每居民治愈率
    public int mars_building_cure_time_gap_sec = 3600;//治愈间隔时间
    public long mars_building_oxygen_index_max;//氧气指数(oxygen_index)
    public long mars_building_satiety_index_max;//饱腹指数(satiety_index)
    public long mars_building_sleep_index_max;//睡眠指数(sleep_index)
    public long mars_building_comfort_index_max;//舒适指数(comfort_index)
    public long mars_building_mood_index_max;//心情指数(mood_index)
    public long mars_satisfaction_degree_reward_mail_id;//火星基地满意度奖励邮件id
    public long mars_satisfaction_degree_reward_mail_simple_unlock_id;//【火星基地】满意度奖励邮件解锁对应的simple_unlockID

    public int mars_building_temp_queue_time_price_type;//【火星基地】火星建筑临时队列time price类型
    public NPCommonCostItem mars_temp_building_queue_gain_buff_item = new NPCommonCostItem();//【火星基地】临时建造队列解锁获得奖励(BUFF-buff配置ID:buff有效时长)

    //GOB-7237【GOB-1】跑马灯-玩家登录火星时显示全服跑马灯-服务端 https://www.teambition.com/task/692d4beb653bf58482f82c8d
    public long mars_go_to_finish_marquee_id;//所有玩家一登录上火星时，全服显示跑马灯消息：【恭喜{0}登录火星，开启新的旅程】{0}：玩家名称

    //GOB-8281 主基地每次升级，默认提供6小时火星币产出，增加general配置
    //https://www.teambition.com/task/69593a6b2641688371a019bb
    //https://www.teambition.com/task/695a7da72626e1eef2ff84c7单据去除本变量
    //public int mars_home_up_gain_hours = 6;//数值：小时
    
    //【GOB-1】引导-居民中心刚解锁时，立即推送一个事件
    //https://www.teambition.com/task/69708ef85cffe652c0b88f0a
    public long mars_building_people_id;//【火星基地】居民中心建筑 id
    
    //火星探索
    public ArrayList<WCGPairMarsEventTypeInt> mars_explore_refresh_event_type_wei_list = new ArrayList<>();//刷新事件类型权重列表
    @RefField(isIgnore = true)
    public WeightValueList<EMarsExploreEventType> marsExploreRefreshEventTypeWeiList = new WeightValueList<>();//刷新事件类型权重列表
    public int mars_explore_daily_refresh_special_event_num;//特级事件每日刷新数量
    public long mars_explore_cd;//单次探索次数恢复CD(lazy_cd)
    public ArrayList<Long> mars_explore_boss_pos_list = new ArrayList<>();//boss事件专用pos id列表
    public int mars_explore_boss_unlock_lvl;//boss事件解锁时需要的探索等级
    public long mars_explore_daily_refresh_fixed_cd;//【火星探索】boss事件每日刷新fixed_cd
    public int mars_explore_team_hero_max_num = 3;//探索队伍最多能容纳大臣数量
    public int mars_explore_team_loss_min_per;//队伍带兵量损失最小数量（万分比）
    public int mars_explore_team_loss_max_per;//队伍带兵量损失最大数量（万分比）
    public int mars_explore_team_repair_unit_ms = 1;//每单位带兵量维修需要的时间（毫秒）
    public ArrayList<NPCommonCostItem> mars_explore_team_repair_unit_cost_list = new ArrayList<>();//每单位带兵量维修需要的消耗列表
    public int repair_power_basic;        //单个兵力维修成本对应的单兵实力基数，以此计算实力提升后的单兵维修成本（计算维修成本的时候不带百分比加成计算）

    public int mars_home_output_remain_max_time_s = 86400;//火星基地资源建筑产出资源最大存储时间（秒）
    
    public int  mars_explore_team_base_cost_per;//败方基础损失系数
    public int  mars_explore_team_fail_cost_per;//实力碾压对败方损失加成
    public int  mars_explore_team_win_cost_per;//胜方基础损失系数
    public int  mars_explore_team_win_red_per;//胜方损失减免系数
    public int  mars_explore_team_fail_cost_per_max;//败方精兵实际损失系数上限

    public int mars_explore_pvp_record_save_limit = 100;//PVP记录保存最大数量
    public long mars_power_rank_id;//火星实力冲榜id
    //前往火星时长动态减少规则（人数下限:人数上限:减少万分比），first=-1表示无下限，second=-1表示无上限
    public List<WCGTripleLong> mars_go_route_arrive_reduce_list = new ArrayList<>();
    
    //GOB-8319 战斗队伍实力显示部分全部要除以一个系数，放在General表中
    //https://www.teambition.com/task/695a51439ec890718a63db8f
    public int mars_explore_team_power_coef = 1;//该系数不能为0

    //火星矿产
    public int mars_mine_secs = 3600;//矿场有效时间
    public int mars_mine_mine_new_per = 100;//刷新空矿的概率
    public int mars_mine_num_min = 10;//矿产最小数量
    public int mars_mine_num_max = 100;//矿产最大数量

    //阶段目标
    public int stage_goal_first_reach_detail_list_show_count;//阶段目标大阶段首达详情列表展示数量

    //赚速跑马灯
    public WCGPairIntList earnings_marquee_config;//赚速跑马灯配置（格式：赚速:次数;赚速1:次数1）
    public long earnings_marquee_ref_id;//赚速跑马灯配置ID（对应RefMarquee表的ID）

    //公会-火星互助
    public NPCommonCostItem guild_mars_help_deal_reward_item = new NPCommonCostItem();//公会互助每次可获得的奖励
    public long guild_mars_help_deal_reward_fixed_cd_id;//公会互助每日奖励的fixed_cd

    //GOB-7133 4.服务端开发-商店好评
    //https://www.teambition.com/task/6925736e6ea98659a4946915
    public NPCommonItem store_reviews_trigger_common_item = new NPCommonItem();//触发商店好评道具（sys_info）
    public long store_reviews_reward_mail_id;//商店好评奖励邮件id
    public ArrayList<NPCommonCostItem> store_reviews_reward_item_list = new ArrayList<>();//商店好评奖励道具列表
    
    //GOB-7494【优化-0】火星带兵量公式优化----服务端
    //https://www.teambition.com/task/693a64c66cadaceacc1baf5d
    public int mars_explore_lead_soldier_cut_coefficient = 1;//【火星探索】大臣实力换算带兵量削减系数（万分比）
    public int mars_mine_guild_share_limit = 50;//【火星矿产】公会分享次数上限

    //联盟宝箱
    public long guild_box_dispatch_mail_id;//联盟宝箱下发奖励邮件ID
    
    //权益卡 火星系统 公会互助
    public long guild_mars_help_auto_deal_buff_id;//火星自动互助buff_id

    public int rush_exchange_done_need_wait_sec;//兑换完成所需等待时间 秒
    public int rush_exchange_refresh_sec;//兑换组内 配置刷新间隔时间 秒
    public int rush_exchange_day_can_exchange_times;//每日可兑换次数

    public long midday_dungeon_system_log_id;//午间副本聊天频道系统消息id
    public long evening_dungeon_system_log_id;//晚间副本聊天频道系统消息id
    public long recruit_gain_hero_or_consort_marquee_id;//兑换获得顾问或者情人跑马灯id
    public long mars_go_route_done_box_id;//前往火星阶段完成奖励宝箱id
    public long mars_go_route_done_chat_system_log;//前往火星阶段完成系统消息ID
    public long arrive_space_box_id;//解锁空间站奖励宝箱id
    public long arrive_space_chat_system_log;//解锁空间站系统消息ID
    public long default_player_room_skin;//默认玩家房间皮肤

    //活动队伍相关
    public int activity_team_apply_limit = 50;//活动队伍申请人数上限

    //事件id和排行榜列表 对应map
    @RefField(isIgnore = true)
    public Map<Integer,List<Long>> logicEventRankMap = new HashMap<>();
    
    /**
     * 获取随机npc id
     * @return
     */
    public long rndNpcId()
    {
    	ArrayList<Long> list = this.mars_resident_npc_id_list;
    	if(list.isEmpty())
    		return 0;

    	int idx = CommonFunc.randomInt(list.size() - 1);
    	return list.get(idx);
    }

    //订单相关
    public long order_send_item_mail_id;//订单发货邮件id
    public long voucher_item_bag_item_id;//代金券背包道具id

    //情人收集
    public ArrayList<Long> lover_collect_lover_ids = new ArrayList<>();//情人收集可选情人ID列表
    public long lover_collect_need_earn_speed;//情人收集所需赚速

    //爬塔相关
    @RefField(isIgnore = true)
    public List<TowerStageRefObj> towerChapterStageList = new ArrayList<>();
    public void setTowerChapterStageList(List<TowerStageRefObj> _towerChapterStageList)
    {
        towerChapterStageList = _towerChapterStageList;
    }

    //火星-前往火星阶段数据
    @RefField(isIgnore = true)
    private _TLevelMapMgr<RefMarsGoRoute> _m_lmMarsGoRouteMapMgr = new _TLevelMapMgr<RefMarsGoRoute>();
    public void setMarsGoRouteStageMapMgr(_TLevelMapMgr<RefMarsGoRoute> _mgr) {_m_lmMarsGoRouteMapMgr = _mgr;}
    public _TLevelMapMgr<RefMarsGoRoute> getMarsGoRouteStageMapMgr() {return _m_lmMarsGoRouteMapMgr;}

    //火星探索事件地点
    public long rndExploreBossPos()
    {
    	ArrayList<Long> list = this.mars_explore_boss_pos_list;
    	if(list.isEmpty())
    		return 0;
    	
    	int idx = CommonFunc.randomInt(list.size() - 1);
    	return list.get(idx);
    }

    //火星探索-战斗事件数据整理： 品质-权重事件列表
    @RefField(isIgnore = true)
    public HashMap<EQuality, WeightValueList<RefMarsExploreEventBattle>> marsQualityBattleEventMap = new HashMap<>();
    //根据品质获取火星探索战斗事件
    public RefMarsExploreEventBattle randMarsBattleExploreEventBattleRef(EQuality _quality)
    {
    	HashMap<EQuality, WeightValueList<RefMarsExploreEventBattle>> map = this.marsQualityBattleEventMap;
    	WeightValueList<RefMarsExploreEventBattle> obj = map.get(_quality);
    	if(null == obj)
    		return null;
    	
    	return obj.random();
    }

    //火星探索-矿产数据整理： 等级-权重列表
    @RefField(isIgnore = true)
    public HashMap<Integer, WeightValueList<RefMarsExploreMine>> marsLvlMineMap = new HashMap<>();
    //根据品质获取火星探索战斗事件
    public RefMarsExploreMine randMarsLvlMineMap(int _lvl)
    {
    	HashMap<Integer, WeightValueList<RefMarsExploreMine>> map = this.marsLvlMineMap;
    	WeightValueList<RefMarsExploreMine> obj = map.get(_lvl);
    	if(null == obj)
    		return null;
    	
    	return obj.random();
    }

    @RefField(isIgnore = true)
    public HashMap<Integer, WeightValueList<Integer>> proAddMap = new HashMap<>();
    @RefField(isIgnore = true)
    public HashMap<Integer, WeightValueList<Integer>> proAddConsortNotFullMap = new HashMap<>();
    /**
     * 抽取加成万分比（不需要检查指定次数，直接使用全满的数据）
     */
    public int randomProAdd(int _groupId)
    {
		WeightValueList<Integer> list = this.proAddMap.get(_groupId);
        if(null == list)
            return 0;
        
        return list.random();
    }
    /**
     * 抽取加成万分比（需要检查指定次数）
     * @param _count
     * @param _groupId
     * @return
     */
    public int randomConsortSkillProAdd(int _count, int _groupId)
    {
    	if(_count < consort_reshape_skill_max_count)
    	{
    		WeightValueList<Integer> list = this.proAddConsortNotFullMap.get(_groupId);
            if(null == list)
                return 0;
            
            return list.random();
    	}
    	else 
    	{
    		WeightValueList<Integer> list = this.proAddMap.get(_groupId);
            if(null == list)
                return 0;
            
            return list.random();
    	}
    }
    
    @RefField(isIgnore = true)
    public WeightValueList<EMarsPeopleHelpType> marsHelpTypeWeiObj = new WeightValueList<>();
    public EMarsPeopleHelpType randHelpType()
    {
    	WeightValueList<EMarsPeopleHelpType> obj = this.marsHelpTypeWeiObj;
    	return obj.random();
    }
    
    //联盟宝箱事件触发数据管理
    @RefField(isIgnore = true)
    public HashMap<Integer, RefGuildBoxEvent> guildBoxEventMap = new HashMap<>();
    
    //火星移民数量分档处理
    @RefField(isIgnore = true)
    public WeightValueList<RefMarsImmigrationPer> marsMarsImmigrationPerWeiObj = new WeightValueList<>();
    public RefMarsImmigrationPer randMarsImmigrationPer()
    {
    	WeightValueList<RefMarsImmigrationPer> obj = this.marsMarsImmigrationPerWeiObj;
    	return obj.random();
    }
    public long randMarsImmigrationValue(long _value, StringBuilder _sb)
    {
    	if(null != _sb)
    	{
    		_sb.append("\nValue:").append(_value);
    	}
    	
		RefMarsImmigrationPer perRef = RefGeneral.Ref().randMarsImmigrationPer();
		if(null == perRef)
		{
	    	if(null != _sb)
	    	{
	    		_sb.append(", ref not find.");
	    	}
			return 0;
		}

    	if(null != _sb)
    	{
    		_sb.append(", ref:").append(perRef.id);
    	}
		
		return perRef.randNumPer(_value, _sb);
    }
}
