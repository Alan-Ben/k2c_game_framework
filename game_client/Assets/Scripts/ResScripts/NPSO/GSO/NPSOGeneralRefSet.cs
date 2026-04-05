using UnityEngine;
using ALPackage;
using System;
using System.Collections.Generic;
using CommonEnum;
using NPEnum;
using UnityEngine.Serialization;

namespace GOE
{
    [Serializable]
    public class NPGeneralRefObj : _IALBasicRefObj
    {
        public long _refId
        {
            get
            {
                return NPSOGeneralRefSet.generalId;
            }
        }

        public int base_crit_damage_per = 20000; //基础暴击倍率（万分比）
        public int min_crit_damage_per = 5000;//最小暴击伤害倍率（万分比）
        public int melee_dis;//近战距离 小于等于这个值的就显示近战.
        public long surrender_show_time;//用来控制 投降框存在的时间 (单位毫秒)

        /** 地图可视高度判断范围 */
        public short can_view_delta_height = 100;
        /** 地图可行走高度判断范围 */
        public short can_walk_delta_height = 50;

        /** 最低伤害系数（万分比） */
        public long min_base_damage_per = 1500;

        //被攻击方被克制的伤害系数
        public int element_be_restraint_per = 10000;
        //攻击方克制的伤害系数
        public int element_restraint_per = 11500;

        public long default_player_icon;//默认玩家头像
        public long default_player_icon_bgk;//默认玩家头像框
        public long default_player_skin;//默认玩家皮肤
        public long default_player_combo_title_bg;//默认玩家组合称号底框
        public long default_player_room_skin;// 默认玩家房间皮肤
        public long player_combo_title_unlock_center_tip_id;//玩家组合称号解锁center tip id
        public List<NPGTextureIndex> robot_icon_list;//机器人头像 EWCGDifficulty 配三个不同难度的机器人Icon

        public float team_skill_show_time;//指挥官技能展示对象持续展示时间

        public float release_skill_break_show_time = 3f;//技能被打断的展示时长

        public int trigger_max_deep = 8;//被动触发层数上限

        //玩家属性加成
        public NPPlayerPropertyModifier player_property; //玩家属性加成


        //TODO:新增配置往下写
        //eg:
        //public int pre_operation_num;//操作队列同一时间操作对象个数限制

        //Mission相关

        public _NPPlayerConditionSerializeInfo player_move_building_cond;//玩家自由摆放建筑条件
        public int common_use_add_num;//单次增减数量
        public float chat_server_failed_reconnect_delay;//聊天服务器ip获取失败时，要过多久进行重试（单位：秒）（备注：如果小于等于0就不重试）

        public long chat_show_time_condition_ms;//两条消息间隔多久会显示一个时间节点
        public long chat_default_bubble_id;//默认气泡框id

        public NPGSceneIndex show_case_scene_index;//showcase场景资源
        public int city_area_refresh_rate = 200;//乐园刷新频率,

        public List<NPCommonCostItem> player_info_rename_cost_list; //玩家改名消耗
        public WCGIntRange player_info_rename_cost_range; //玩家改名限制区间

        public long avatar_snapshot_gem_unlock_type_id;//钻石解锁时装预设格的times_price表type_id

        public NPGTextureIndex avatar_snapshot_default_icon;//预设默认图标
        public NPGSpriteIndex avatar_snapshot_quality_icon;//预设统一品质底图

        public int quest_num_limit_max; //接受任务的最大数量

        public long build_done_tip_id;//建造完成的提示tipid
        public List<long> mission_expect_tips_list;//关卡战斗预期提示列表15000:9000

        public long actor_dying_limit_hp_per;//角色濒死状态最大血量万分比

        public long building_check_wait_sfx_id;//建筑等待确认播放的特效id
        public long building_check_sfx_id;//建筑确认播放的特效id

        public List<ENPItemType> shield_itemtype_list;//通用奖励弹窗屏蔽的物品大类列表
        public List<NPCommonItem> shield_commonitem_list;//通用奖励弹窗屏蔽的CommonItem列表


        public NPGTextureIndex mini_game_quset_dialog_op_icon;//悬赏任务npc对话开始任务按钮图标

        public int friend_apply_limit;//被好友申请的数量限制
        public int friend_apply_avaiable_secs;//好友申请有效时长
        public int friend_apply_send_margin_secs;//好友发送申请间隔时长


        public float dialogue_talk_speed;//对话打字机效果速度（每个字符的间隔时间，毫秒）

        public int chat_private_send_cd = 3000;//私聊消息间隔时长
        public NPGTextureIndex chat_private_icon;//私聊图标
        public _NPPlayerConditionSerializeInfo chat_unlock_condition;//私聊解锁条件


        public NPGGoIndex party_reward_city_go_index;//乐园的聚会奖励马车形象

        public float drag_camera_move_viewpos_threshold;//拖动到距离边缘多少时会移动相机

        public long machine_share_msg_send_cd;//机关分享消息发送cd（毫秒）

        public NPGTextureIndex space_food_capture_texture_of_tab_all; // 大地图中捕捉窗口的“所有”按钮的图标
        public float teleportMinDistance; // 使用传送功能的最小距离
        public float battle_win_result_delay_time_s = 1f;//战斗胜利延迟弹窗，单位秒

        public long default_click_audio_mixer_group_id = 2;//默认点击音效的混音器组id
        public long city_building_repair_op_id;//乐园建筑修复行为的 id

        public _NPPlayerConditionSerializeInfo player_akey_upgrade_building_cond;//玩家一键升级建筑条件

        public List<NPCommonCostItem> chg_gender_cost_item_list; //修改性别消耗
        public List<NPCommonCostItem> chg_skin_color_cost_item_list; //修改肤色消耗

        public List<_NNPCommonEnumLongInfo<ENPMonsterPeriodType>> mission_mob_layout_item_res_id_list;//关卡怪物布阵怪物item不同类型资源id

        public int bag_auto_use_max_count;//背包使用道具的时候默认直接使用的最大数量
        #region 签到
        public int daily_check_timeout;//【每日签到】超过x天登录更换问候语
        public int daily_check_dessert_num;//【每日签到】甜品数量
        public int daily_check_extra_dessert_num;//【每日签到】超过x天后额外甜品数量
        public long daily_check_simple_unlock_id;//【每日签到】签到解锁条件

        #endregion

        #region 妃子

        public long consort_rand_call_cd; // cd配表ID，妃子随机宠幸消耗的CD
        public long consort_a_key_call_simple_unlock_id; //【情人】一键约会解锁simple_unlock ID
        public NPCommonItem consort_bless_point_item;//【家人】加护点item(用于展示加护点不足时, 点击跳转途径)
        public NPCommonItem consort_charm_item;//【情人】魅力值对应道具
        public NPCommonItem consort_intimacy_item;//【情人】亲密度对应道具
        public NPCommonItem consort_like_item;//【情人】好感度对应道具

        public long consort_bless_skill_simple_unlock_id;//【家人】妃子加护技能解锁simple_unlock ID

        #endregion

        public NPCommonCostItem consort_call_cost_item; // 【情人】指定妃子宠幸消耗的道具
        public long consort_call_cost_gem; // 【情人】指定问候消耗的钻石数量
        public List<long> consort_call_gem_discount_list; // 【情人】指定问候消耗钻石打折列表

        #region 子嗣

        public NPCommonItem child_seat_recover_item; // 回复 1 点脑力消耗的道具
        public long child_seat_recover_S; // 回复 1 点脑力所需的时间（秒）
        public long child_scene_id; // 子嗣场景 id
        public NPGGoIndex child_classroom_res_index;
        public string child_default_classroom_video_name; // 默认教室视频名
        public long child_one_key_naming_unlock_id; // 一键取名的解锁条件
        public long child_one_key_educate_unlock_id; // 一键上课的解锁条件
        public long child_one_key_educate_plus_unlock_id; // 进阶一键上课的解锁条件
        public long child_one_key_graduate_unlock_id; // 一键毕业的解锁条件
        public int child_name_length_limit;
        public float child_step_up_show_delayS; // 升学时的模型切换和弹窗展示相关的延迟时间
        public float child_one_key_educate_spaceS; // 一键上课的间隔时间
        public int unmarry_adult_limit;//未婚子嗣数量上限
        public int married_adult_limit;//已婚子嗣数量上限
        public long child_add_per;
        public long child_cal_unit_per;
        public long child_train_add_per;
        public long child_seat_energy_red_tip_threshold;//脑力值达到最大脑力值指定万分比时显示红点

        #endregion

        #region 成年子嗣
        public NPCommonCostItem child_marry_server_refresh_cost_item; //全服联姻刷新消耗物品
        public WCGIntRange child_marry_earnings_lower_limit_percentage_range;//联姻收益下限百分比范围
        public int child_marry_earnings_lower_limit_adjustment_percentage;//联姻收益下限百分比调整值
        
        #endregion

        public long hero_level_upgrade_ten_times_simple_unlock_id;//伙伴等级连升十级条件simple_unlock id
        public long hero_talent_upgrade_ten_times_simple_unlock_id;//伙伴资质连升十级条件simple_unlock id
        public NPGTextureIndex hero_all_attr_icon;//骑士全属性单独图标
        public long levy_food_gap_sec = 5; //粮食征收上浮提示时间间隔（s）-客户端
        public long levy_offline_show_min;//离线超过这个时长才会弹出离线奖励弹窗
        public long levy_soldier_big_crit_sfx_id;//征收士兵暴击拖尾特效id
        public long levy_soldier_big_txt_sfx_id;//征收士兵暴击文字放大特效id

        public NPGTextureIndex hero_random_attr_icon;//骑士随机属性图标

        #region 大学
        public int college_init_slot_count;//初始席位
        public long college_add_slot_time_price_id;//席位扩增time_price_id
        public int college_extra_slot_limit_count;//【大学】额外解锁席位上限
        #endregion

        #region 关卡

        // public List<long> chapter_pve_loss_degree; //pve兵力折损程度"微小","一般","严重"
        public WCGLongRange chapter_pve_loss_percent_limit; //pve兵力折损比例上下限（万分比）
                                                            // public int chapter_pve_consume_target_ratio;//pve每次消耗目标值比例/万分比

        public NPCommonCostItem chapter_hero_revive_item; //回复骑士出战次数道具
        public int chapter_hero_free_count; //骑士每日默认出战次数上限
        public int chapter_hero_item_revive_limitation_count; //每日道具复活次数上限
        public long chapter_boss_level_scene_id; // Boss战场景 id
        public long chapter_game_scene_id; // 关卡游玩场景 id
        public int chapter_max_suspended_event_count; // 关卡最大挂起事件的数量
        public float chapter_game_player_move_delay; // 玩家移动延迟
        public float chapter_game_dice_simulation_step_time; // 骰子物理模拟的单步时间
        public int chapter_game_dice_simulation_max_step_count; // 骰子物理模拟的最大数量
        public float chapter_game_dice_destroy_delay; // 骰子延迟多久销毁
        public NPGGoIndex chapter_game_player_go_index; // 玩家对象的 GoIndex
        public NPGGoIndex chapter_game_dice_go_index; // 骰子对象的 GoIndex
        public NPGGoIndex chapter_game_cat_go_index; // 关卡跟随玩家的猫GoIndex

        public long chapter_a_key_deal_simple_unlock_id;//【关卡】一键处理事件simpleUnlock id
        public long chapter_boss_fight_auto_attack_simple_unlock_id;//【关卡】boss战自动攻击simpleUnlock id
        public long chapter_arrive_blank_block_sfx_id;//【关卡】到达空白格时播放的特效id
        public NPGGoIndex chapter_blank_block_go_index;//【关卡】空白格子上要加载的GoIndex
        public Vector3 chapter_blank_block_go_offset;//【关卡】空白格子上要加载的GoIndex的偏移
        public bool chapter_blank_block_go_face_to_path;//【关卡】空白格子上要加载的GoIndex是否朝向路径
        public long chapter_gain_extra_reward_block_sfx_id;//【关卡】获取额外奖励时格子上播放的特效id

        #endregion

        public NPCommonCostItem consort_call_recharge_cost;//【情人】恢复妃子宠幸精力消耗的道具

        public long arena_hero_atk_cal_value;//骑士攻击力计算系数
        public NPCommonAssetPathInfo player_avatar_asset_path;//玩家的 Avatar 资源路径
        public long player_avatar_edit_scene_id;//编辑玩家 Avatar 的场景 id
        public long dinner_list_show_num;//每页显示宴会数量

        public int shop_item_batch_buy_cond_count; // 【商店】批量购买的条件数量

        public long chapter_can_esc_unlock_id = 11011; // 关卡允许esc的条件id
        public long consort_entry_scene_id = 11000; // 妃子寝宫场景 id
        public long hero_battle_entry_scene_id = 11000; // 骑士挑战场景 id
        public long avatar_stage_display_show = 2010; // 玩家形象舞台独立展示场景id

        #region 宴会

        public long dinner_rank_fixed_id;//宴会排行榜id
        public long dinner_entry_scene_id = 2001; // 宴会入口场景 id
        public int dinner_server_share_cd = 120000;//全服聊天分享cd ms
        public int dinner_guild_share_cd = 120000;//联盟聊天分享cd ms


        #endregion
        public long space_station_entry_scene_id = 1010; // 空间站入口 id
        
        #region Mars
        public long mars_entry_scene_id = 1020; // 火星入口 id
        public long mars_building_sec_to_diamond_ratio = 60; // 【火星基地】钻石兑换比 1 钻石兑换多少（秒）
        public long mars_building_build_guild_help_sec_reduce = 60; // 【火星基地】联盟成员帮助 1 次，减少的时间（秒）
        public _NPPlayerConditionSerializeInfo mars_building_another_build_num_unlock_condition; // 【火星基地】修建队列2永久解锁条件
        public int mars_building_slot_people_count; // 【火星基地】派遣槽位容纳人数
        public NPCommonItem mars_building_energy_output_item; // 【火星基地】能源建筑产出的资源类型
        public long mars_building_home_id; // 【火星基地】主基地建筑 ID
        public NPGTextureIndex mars_equipment_energy_max_storage_icon;
        public NPGTextureIndex mars_equipment_food_satiety_yield_icon;
        public NPGTextureIndex mars_equipment_hospital_cure_rate_icon;
        public NPGTextureIndex mars_equipment_living_comfort_yield_icon;
        public NPGTextureIndex mars_equipment_living_mood_yield_icon;
        public NPGTextureIndex mars_equipment_living_sleep_yield_icon;
        public NPGTextureIndex mars_equipment_living_people_num_limit_icon;
        public NPGTextureIndex mars_equipment_hospital_cure_num_range_icon;
        public NPGGoIndex mars_equipment_upgrade_effect_go;
        public long mars_building_oxygen_coefficient;
        public long mars_building_satiety_coefficient;
        public long mars_building_sleep_coefficient;
        public long mars_building_comfort_coefficient;
        public long mars_building_mood_coefficient;
        public long mars_building_people_cure_rate;
        public long mars_building_oxygen_yield_per_consume;
        public long mars_building_satiety_yield_per_consume;
        public long mars_building_sleep_yield_per_consume;
        public long mars_building_comfort_yield_per_consume;
        public long mars_building_mood_yield_per_consume;
        public long mars_home_output_pop_min_time_s;    //火星基地弹出资源收集气泡的最小时间间隔
        public long mars_home_output_red_time_s;        //火星基地弹出资源收集红点的时间间隔
        public int mars_explore_team_hero_max_num;
        public WCGIntRange mars_explore_team_name_range;
        public long mars_explore_cd;
        public NPGGoIndex mars_explore_team_scene_go_index;
        public NPGGoIndex mars_explore_team_path_line_scene_go_index;
        public long mars_building_temp_queue_time_price_type;
        public int mars_explore_pvp_record_save_limit;
        public int mars_building_queue_max_count;
        public NPCommonCostItem mars_temp_building_queue_gain_buff_item;
        public long mars_building_oxygen_index_max;
        public long mars_building_satiety_index_max;
        public long mars_building_sleep_index_max;
        public long mars_building_comfort_index_max;
        public long mars_building_mood_index_max;
        public float mars_can_get_energy_progress_threshold;//【火星基地】可领取能源进度比例(场景中能源建筑会显示领取按钮)
        public long mars_explore_lead_soldier_cut_coefficient;
        public int mars_mine_guild_share_limit;
        public long mars_building_queue_forever_add_id;
        public long mars_explore_team_repair_unit_ms;
        public List<NPCommonCostItem> mars_explore_team_repair_unit_cost_list;
        public int repair_power_basic;      //单个兵力维修成本对应的单兵实力基数，以此计算实力提升后的单兵维修成本（计算维修成本的时候不带百分比加成计算）
        public long mars_building_upgrade_audio_id;
        public long mars_building_build_audio_id;
        public long mars_explore_share_mine_to_guild_chat_interval_ms;
        #endregion

        public int mail_max_lock_num;//邮件最大收藏数量
        public int mail_max_num;//最大邮件数量
        public long default_mail_refid;//【邮件】默认运营邮件ID

        public _NPPlayerConditionSerializeInfo daily_quest_one_key_finish_condition;//每日任务一键领取条件

        public NPCommonItem restore_anecdote_item;//恢复政务次数的道具
        public long anecdote_one_key_simple_unlock_id;//政务解锁一键完成simple_unlock ID
        public NPGTextureIndex anecdote_cd_entrance_bg_image;//政务倒计时入口背景图
        public NPGTextureIndex anecdote_cd_entrance_icon;//政务倒计时入口图标
        public long anecdote_lazy_cd;//政务事件使用的CD，等同当前可以处理的政务事件数量
        public NPGGoIndex consort_default_detail_bg;//妃子默认详情背景图
        public long consort_ten_gift_simple_unlock_id; //【情人】十连送礼解锁条件


        #region 通用事件

        public long commonevent_dispatchevent_akey_dispatch_simple_unlock_id;//【通用事件】派遣事件 一键派遣解锁SimpleUnlock ID

        #endregion

        #region 游历

        public long travel_scene_id;//【游历】游历场景id
        public long travel_cost_lazycd_id;//【游历】游历消耗的lazycd_id
        public int travel_show_lazycd_red_count = 5;//【游历】游历显示体力红点需要的体力点数
        public long travel_one_key_simple_unlock_id;//【游历】一键游历解锁条件
        // public long travel_meet_unget_consort_interlude_wnd_res_id;//【游历】遇见未获取妃子过程窗口资源id
        // public float travel_meet_unget_consort_interlude_wnd_show_time;//【游历】遇见未获取妃子过程窗口展示时间
        public long travel_pos_unlock_center_tip_id;//【游历】游历地点解锁center_tip_id

        #endregion

        public long achieve_point_particle_id;//成就点数粒子表id
        public long daily_quest_sore_particle_id;//每日任务积分粒子表id

        public long currency_particle_id;//货币类型粒子表id
        public long bag_item_particle_id;//背包道具类型粒子表id
        public List<ParticleNumRangeInfo> currency_particle_num_range_list;//货币资源类型需要展示的粒子数量区间列表

        public long hero_attr_chg_center_tip_id;//骑士属性变化center tip id
        public long daily_quest_finish_center_tip_id;//每日任务完成center tip id

        public long auto_show_quest_simple_unlock_id;//回到主城界面需要自动展示主线任务弹窗的条件id

        public List<NPGGoIndex> anecdote_npc_go_index_list;//政务场景人物列表

        #region 集市
        public long market_akey_simple_unlock_id;//【集市】一键经营条件解锁simple_unlockID
        public int market_reset_num;//【集市】重置次数
        public long market_shop_cd_sec;//【集市】恢复一点经营次数所需时间
        public NPCommonItem market_up_cost_item;//【集市】升级消耗物品
        public long market_scene_id = 11000; // 【集市】场景 id
        public NPCommonCostItem market_shop_restore_cd_costitem;//【集市】 消耗道具直接经营
        public List<NPCommonKeyValueInfo> market_crit_sfx_id_list;//集市暴击倍数特效id列表
        #endregion

        public long college_scene_id = 11000; // 【大学】场景 id
        public float dialog_wnd_blue_bk_alpha = 0.9f;//对话窗口压暗背景参数，越小越暗
        public List<ENPItemType> item_show_shield_type_list;//道具展示需要屏蔽的类型列表
        public Color item_not_enough_color;

        public int chat_share_send_margin_secs;//聊天分享（骑士，子嗣，妃子等）间隔时长
        public int friend_group_name_char_limit;//好友分组名字字符长度限制
        public int chat_private_up_to_top_limit;//私聊置顶数量限制
        public long func_unlock_fly_sfx_id;//场景入口功能解锁飞行特效id
        public int rank_show_max_num = 100;
        public List<string> rank_like_suc_random_str_list; // 【排行榜】点赞成功随机展示的文本
        public long power_rank_fixed_id = 101;//国力的常驻排行榜id
        public List<long> make_face_show_suit_id_list;//捏脸时试穿的套装id列表

        public _NPPlayerConditionSerializeInfo entrance_show_hand_guide_cond;//入口手指引导展示条件
        public long anecdote_hand_guide_ui_res_id;//政务入口手指引导资源id
        public long hero_recommend_hand_guide_ui_res_id;//骑士推荐入口手指引导资源id

        public long player_default_cute_actor_id = 1001;//玩家默认Q版形象id
        public long friend_visit_simple_unlock_id;//庭院拜访解锁id
        public long friend_visit_particle_id;//好友拜访粒子表id

        public long first_call_consort_invitation_bg_id;//第一次约会专属背景配置id

        public float cat_bubble_refresh_time;//猫咪气泡文本刷新间隔
        public List<string> cat_bubble_default_list;//猫咪气泡默认文本列表
        public _NNPCommonLongEnumInfo<EWeekCardNPCType> week_card_assign_default_td_show;//默认执政官形象

        public int shield_cid_limit;//屏蔽玩家数量上限

        public long levy_silver_particle_id = 12403;//征收金币粒子表id
        public long levy_soldier_particle_id = 12401;//普通征收面包粒子表id
        public long levy_soldier_big_particle_id = 12402;//暴击征收面包粒子表id
        public NPGTextureIndex reward_preview_default_not_get_box_icon;//奖励预览弹窗未领取状态宝箱默认图标
        public NPGTextureIndex reward_preview_default_already_get_box_icon;//奖励预览弹窗已领取状态宝箱默认图标

        public int check_low_frame_rate_duration_time_sec;//检测低帧率持续时间秒
        public int check_low_frame_rate_value;//检测低帧率帧数下限

        public int check_city_screen_click_hide_ani_time_sec = 2;//检测主界面屏幕点击隐藏动画时间秒

        public float levy_silver_red_tip_show_percent = 10f;//征收银币红点显示百分比- 超过这个百分比显示红点

        public long create_player_edit_scene_id = 2;//创角的场景 id
        public long avatar_score_edit_scene_id;//avatar评分事件的场景 id

        public _NPPlayerConditionSerializeInfo dialogue_set_self_name_cond;//设置玩家对话名称显示为自己名字的条件，条件没通过显示“你”
        public long collect_likes_fixed_id = 101;//每日点赞限制次数的fixedCD_id

        public NPCommonCostItem guild_change_name_cost;//修改联盟名称消耗
        public WCGIntRange guild_name_length_limit;//联盟名称字符长度限制
        public WCGIntRange guild_simple_name_length_limit;//联盟简称字符长度限制
        public WCGIntRange guild_declaration_length_limit;//联盟宣言字符长度限制
        public WCGIntRange guild_announcement_length_limit;//联盟公告字符长度限制
        public NPCommonCostItem guild_create_cost;//创建联盟消耗
        public List<NPCommonCostItem> guild_first_time_join_reward_list;//首次加入联盟奖励
        public NPCommonCostItem guild_change_flag_cost;//修改联盟旗帜消耗
        public int guild_leader_impeach_offline_beyond_hours;//盟主被弹劾需要离线超过x小时
        public int guild_leader_impeach_message_available_within_hours;//盟主弹劾信息有效期
        public long guild_broadcast_message_daily_limit_fixcd_id;//联盟群发消息每日次数fixCdId
        public NPCommonCostItem guild_broadcast_message_cost;//联盟群发次数用完后消耗
        public WCGIntRange guild_broadcast_message_length_limit;//联盟群发字符长度限制
        public long guild_impeach_succ_need_percent;//联盟弹劾盟主成功所需成员百分比
        public int guild_join_request_limit_num;//玩家入盟请求上限条数
        public long guild_main_scene_id;//联盟主界面场景id
        public long guild_open_recruit_gap_sec;//联盟公开招募间隔时间（秒）

        public long guild_entrust_reward_mail_id;//发放杂物委托奖励邮件id
        public List<int> send_active_box_required_active_stamp_num;//获得活跃宝箱所需印章数量
        public int week_draw_active_box_limit;//每周可领取活跃宝箱数量上限
        public List<NPCommonCostItem> active_box_reward_list;//活跃宝箱奖励列表
        public int guild_great_reward_progress_per_active_box;//发放一个活跃宝箱给工会大礼增加的进度
        public List<NPCommonCostItem> guild_great_reward_reward_item_list;//工会大礼奖励列表
        public int guild_great_reward_need_point;//工会大礼所需累计进度
        public int weekly_draw_guild_great_reward_limit;//工会大礼的每周领取上限次数
        public long deal_entrust_lazy_cd_id;//处理委托的CD id
        public int each_attr_can_dispatch_hero_num;//每个相性可派遣大臣数量
        public long active_box_valid_time_sec;//活跃宝箱有效时间（秒）
        public long guild_great_reward_valid_time_sec;//工会大礼有效时间（秒）
        public NPCommonItem guild_exp_common_item;//联盟经验common_item
        public NPCommonItem guild_wealth_common_item;//联盟财富common_item
        public NPCommonItem personal_contribution_common_item;//个人贡献common_item
        public NPCommonItem personal_guild_coin_common_item;//个人联盟币common_item
        public NPCommonItem guild_cooperate_construction_common_item;//联盟协作建设值common_item
        public int guild_log_max_show_count;//公会日志最大展示数量
        public long guild_rank_fixed_id;//公会常驻排行榜id

        public long building_scene_id; // 建筑场景id
        public long building_effect_scene_id; // 建筑表现效果场景id
        public long farming_building_click_spaceMS; // 农业建筑点击间隔时间
        public long business_building_hero_settle_unlock_id;
        public _NPPlayerConditionSerializeInfo building_product_unlock_condition;//建筑研究解锁条件

        public float chapter_idle_forward_wait_delay_time_s = 0.1f;//关卡前进idle状态等待延迟，秒
        public float chapter_normal_foward_delay_time_s = 1f;//关卡单次前进延迟时间，秒
        public float chapter_quick_forward_delay_time_s = 0.5f;//关卡快速前进延迟时间，秒
        public float chapter_node_move_effect_delay_time_s = 0.5f;//关卡前进节点移动表现时间

        public float chapter_auto_forward_delay_time_s = 1f;//关卡自动前进等待时间
        public float chapter_auto_boss_delay_time_s = 3f;//关卡bosss前进等待时间

        
        public int gold_inspire_increase_power_ratio_per;//金币鼓舞增加战力比例
        public int crystal_inspire_increase_power_ratio_per;//水晶鼓舞增加战力比例
        public int item_inspire_increase_power_ratio_per;//道具鼓舞增加战力比例
        public long gold_inspire_calculate_ratio_time_price_id;//金币鼓舞计算战力比例time_price_id
        public NPCommonCostItem crystal_inspire_fixed_cost;//水晶鼓舞固定消耗
        public NPCommonCostItem item_inspire_cost;//道具鼓舞消耗
        public WCGPairInt forward_gold_cost_ratio_range;//前进金币消耗比例范围
        public NPGGoIndex default_player_go_index;//默认玩家形象
        public int auto_forward_gold_inspire_count_limit;//自动前进金币鼓舞次数限制
        public int auto_forward_crystal_inspire_count_limit;//自动前进水晶鼓舞次数限制
        public int auto_forward_item_inspire_count_limit;//自动前进道具鼓舞次数限制

        public long national_power_target_simple_unlock_id;//国力目标simple_unlock id
        
        #region 藏品

        public long equip_one_key_rebuild_simple_unlock_id;//藏品一键重塑simple_unlock_id
        public long equip_upgrade_ten_times_simple_unlock_id;//藏品十连升级simple_unlock_id
        public long equip_normal_cost_group_id;//藏品普通重塑消耗组id
        public NPCommonCostItem equip_advance_cost;//藏品高级重塑消耗道具
        public long equip_normal_add_pro_group_id;//藏品普通重塑加成概率组ID
        public long equip_advance_add_pro_group_id;//藏品高级重塑加成概率组ID
        public long equip_add_pro_per_max_value;//藏品加成效果最大值万分比
        public NPCommonItem earnings_show_item;//赚速展示用道具

        #endregion

        public long summon_gacha_pool_id;//召唤系统的抽卡卡池

        public long recruit_scene_id;//招募场景id

        #region 竞技场

        public List<long> arena_initial_choose_buff_list;//竞技场初始可选择临时增益
        public List<long> arena_choose_buff_list;//竞技场可选择临时增益
        public int arena_random_attack_free_limit;//竞技场随机挑战免费次数
        public int arena_random_attack_crystal_buy_limit;//竞技场随机挑战钻石购买次数上限 每日
        public int arena_random_attack_crystal_buy_ratio;//竞技场随机挑战钻石购买计算系数 每增加N个伙伴可额外获得1次购买次数的机会
        public long arena_random_attack_crystal_buy_time_price_id;//竞技场随机挑战钻石购买TimePriceId
        public int arena_select_attack_daily_limit;//竞技场指定挑战每日上限
        public int arena_celebrity_rank_up_need_defeat_hero;//竞技场登上名人榜需要击败对方伙伴数量
        public int arena_celebrity_rank_limit_num;//竞技场名人榜上限
        public long arena_convenient_setting_simple_unlock_id;//竞技场便捷设置SimpleUnlockId
        public long arena_one_key_attack_simple_unlock_id;//竞技场一键谈判SimpleUnlockId
        public long arena_rank_fixed_id;//竞技场排行榜id
        public NPCommonItem arena_station_output_item;//竞技场贸易站产出物品（commonitem）
        public long arena_collection_resource_interval_sec;//竞技场贸易站收集资源间隔时间（秒）
        public int arena_gain_influence_for_defeating_each_hero;//竞技场每击败1名伙伴数量获得的商会影响力
        public int arena_deduct_influence_for_each_hero_defeated;//竞技场每被击败1名伙伴数量扣除的商会影响力
        public int arena_gain_coin_for_defeating_each_hero;//竞技场每击败1名伙伴数量获得的商会硬币数
        public NPCommonItem arena_influence_sys_info;//竞技场商会影响力系统信息item
        public long arena_bot_icon_id;//竞技场机器人头像id
        public long arena_bot_icon_bgk_id;//竞技场机器人头像框id
        public string arena_bot_show_rank;//竞技场机器人显示排名
        public List<long> arena_bot_level_list;//竞技场机器人等级列表
        public NPGGoIndex arena_battle_video_go_index;//竞技场战斗视频GoIndex
        public List<string> arena_battle_video_win_ani_name_list;//竞技场战斗视频胜利动画名列表
        public List<string> arena_battle_video_lost_ani_name_list;//竞技场战斗视频失败动画名列表
        public long arena_station_privilege_collect_limit_time_sec;//竞技场贸易站特权卡累积时间上限（秒）
        public long arena_station_privilege_collect_limit_num;//竞技场贸易站特权卡累积数量上限
        public long arena_station_privilege_permissions_id;//竞技场贸易站特权权限id
        public long guild_mars_help_auto_deal_buff_id;//联盟互助-自动互助buff_id

        #endregion

        #region 爬塔

        public long tower_scene_id;// 爬塔场景id
        public long tower_unlock_simple_unlock_id;// 爬塔解锁simple_unlock_id
        public long tower_accelerate_battle_simple_unlock_id;// 爬塔加速战斗simple_unlock_id
        public long tower_skip_battle_simple_unlock_id;// 爬塔跳过战斗simple_unlock_id



        #endregion

        #region 午间副本
        public NPTimeRefreshInfo midday_dungeon_preview_fight_time;// 【午间副本】预告时间
        public NPTimeRefreshInfo midday_dungeon_start_fight_time;// 【午间副本】开始时间
        public NPTimeRefreshInfo midday_dungeon_end_fight_time;// 【午间副本】结束时间
        public long midday_dungeon_rank_fixed_id;//午间副本排行榜id
        public long midday_dungeon_can_borrow_guild_hero_num_fixed_cd_id;//午间副本可借用公会伙伴数量固定cd id
        public long midday_dungeon_auto_fight_simple_unlock_id;//午间副本自动战斗解锁id

        public List<string> midday_dungeon_battle_bubble_defeat_list; //午间副本战斗气泡-击败
        public List<string> midday_dungeon_battle_bubble_no_defeat_list; //午间副本战斗气泡-未击败
        #endregion

        #region 系统解锁

        public long building_to_wall_street_tutorial_edge_id;//主城到华尔街的tutorial edge id
        public long room_to_building_tutorial_edge_id;//卧室到主城的tutorial edge id
        public long room_to_wall_street_tutorial_edge_id;//卧室到华尔街的tutorial edge id
        public long wall_street_to_building_tutorial_edge_id;//华尔街到主城的tutorial edge id

        #endregion

        public NPCommonItem power_sys_info;//实力系统信息item

        public long video_bg_audio_res_id;//视频用的背景音效资源id
        public long video_effect_audio_res_id;//视频用的音效资源id

        #region 晚间活动

        public NPTimeRefreshInfo evening_dungeon_preview_fight_time;//【晚间副本】预告时间
        public NPTimeRefreshInfo evening_dungeon_start_fight_time;//【晚间副本】开始时间
        public NPTimeRefreshInfo evening_dungeon_end_fight_time;//【晚间副本】结束时间
        public NPTimeRefreshInfo evening_dungeon_close_fight_time;//【晚间副本】关闭时间
        public int evening_dungeon_boss_respawn_times_limit;//【晚间副本】单场boss复活上限次数
        public long evening_dungeon_boss_respawn_sec;//【晚间副本】boss复活时长（秒）
        public long evening_dungeon_auto_fight_simple_unlock_id;//【晚间副本】自动战斗解锁条件id
        public List<NPCommonCostItem> evening_dungeon_day_first_kill_boss_reward;//【晚间副本】首次击杀boss奖励
        public List<NPCommonCostItem> evening_dungeon_day_kill_boss_reward;//【晚间副本】非首次击杀boss奖励
        public long evening_dungeon_show_combine_bag_item_id;//【晚间副本】合成展示的背包道具id

        #endregion

        public long earning_goal_achieve_id;//【千万目标】活动对应的成就id

        #region ConsortChat

        public float consort_chat_preset_reply_delay_time = 3;//【妃子聊天】预设回复延迟时间
        public float consort_chat_preset_reply_typing_time = 2;//【妃子聊天】预设回复输入中显示时间
        public WCGIntRange consort_chat_moment_appear_random_time = new WCGIntRange(5 * 60, 60 * 30); //妃子朋友圈下一条出现的随机时间范围
        public int consort_chat_moment_daily_max_count = 5; // 朋友圈每天最大条数
        public WCGIntRange consort_chat_moment_consort_like_random_count = new WCGIntRange(0, 3); //朋友圈妃子点赞随机数量范围
        public long consort_chat_ai_send_time_price_id; // AI聊天发送time price id
        public long consort_chat_ai_send_fixed_cd_id; // AI聊天发送固定cd id
        public int consort_chat_ai_send_max_msg_count = 10; // 请求ai聊天带上的历史最大消息数
        public int consort_chat_ai_one_msg_max_word_count = 200; // 请求ai聊天带上的历史最大消息数
        public List<string> consort_chat_ai_error_reply_list;//ai 聊天出错 回复文本随机列表
        public List<string> consort_chat_ai_moment_error_reply_list;//ai 朋友圈聊天出错 回复文本随机列表
        public WCGIntRange consort_chat_moment_reply_delay_random_time = new WCGIntRange(1 * 60, 5 * 60); //妃子朋友圈延迟回复的随机时间范围
        public WCGIntRange consort_chat_moment_image_random_count = new WCGIntRange(1, 3); //朋友圈图片随机数量范围
        public List<string> consort_chat_ai_moment_content_error_list;//ai 朋友圈 内容出错文本随机列表
        public WCGIntRange consort_chat_moment_offline_appear_random_time = new WCGIntRange(1 * 60 * 60 , 3 * 60 * 60); //妃子朋友圈下一条出现的随机时间范围
        public int consort_chat_moment_offline_max_count = 5; // 朋友圈离线最大条数
        

        public WCGIntRange consort_chat_consort_initiate_msg_appear_random_time = new WCGIntRange(5 * 60, 60 * 30); //妃子主动发起聊天下一条出现的随机时间范围
        public int consort_chat_ai_consort_initiate_msg_daily_max_count = 5; // 妃子主动发起聊天每天最大条数
        public WCGIntRange consort_chat_consort_initiate_msg_offline_appear_random_time = new WCGIntRange(1 * 60 * 60 , 3 * 60 * 60); //妃子主动发起聊天下一条出现的随机时间范围
        public int consort_chat_consort_initiate_msg_offline_max_count = 5; // 妃子主动发起聊天离线最大条数
        public WCGIntRange consort_chat_moment_consort_ai_reply_count = new WCGIntRange(0, 1); //朋友圈玩家未评论的时候可以随机多少个妃子ai评论
        public WCGIntRange consort_chat_moment_consort_ai_reply_after_player_count = new WCGIntRange(0, 0); //玩家回复朋友圈后增加的评论随机数量

        #endregion

        public long inn_scene_id;//旅店场景 id
        public long inn_receive_guest_lazy_cd_id;//接待客人lazycd id
        public long inn_one_key_receive_guest_unlock_id;//一键接待客人解锁条件id
        public long inn_receive_guest_cd_red_tip_threshold;//体力大于该值时，显示红点提示
        public float inn_receive_cost_sec; //接待客人消耗时间（秒）
        public NPGTextureIndex inn_popularity_icon; //旅店人气图标
        public NPGTextureIndex inn_dish_finesse_icon;

        public int farming_building_trigger_multiple_daily_limit;//农田建筑触发暴击倍数每日次数上限
        public _NPPlayerConditionSerializeInfo show_offline_gold_earnings_cond;//展示离线金币收益弹窗的条件

        public int batch_use_item_max_count;//批量使用道具最大数量
        public List<string> can_use_web_recharge_country_list;//可以使用网页充值的国家列表

        public long default_quest_id;//默认主线任务ID
        public long mars_explore_team_power_coef;//火星探索队伍战力系数GOB-8319 战斗队伍实力显示部分全部要除以一个系数，放在General表中

        public NPCommonItem activity_fund_battle_pass_exp_item; //【基金】战令经验对应的展示道具
        public string web_recharge_url;//网页充值地址

        public List<long> lover_collect_lover_ids; // 情人收集可选情人 id 列表
        public long lover_collect_need_earn_speed; // 领取情人所需的赚速值
        public long count_down_event_have_cd_minimum_reset_count;//倒计时事件有倒计时的最小重置次数，超过这个次数就不再显示倒计时
        public long lover_collect_reach_target_tip_id; // 赚速达到领取情人条件时的提示id
        public long seven_day_goals_push_notice_simple_unlock_id;//七日任务推送弹窗simpleUnlockId
        public long player_report_cd_time_hour;//同一玩家举报冷却时间（小时）

        #region 杰出者大厅

        public long grave_celebrate_reward_id;//【杰出者大厅】膜拜奖励id
        public List<string> grave_celebrate_text_list;//【杰出者大厅】膜拜随机文本
        public long grave_congratulate_show_time_ts;//【杰出者大厅】新晋杰出者展示时长(秒)
        public long grave_congratulate_reward_id;//【杰出者大厅】新晋杰出者膜拜奖励idui
        public List<string> grave_congratulate_text_list;//【杰出者大厅】新晋杰出者膜拜随机文本
        public List<CommonLongIntInfo> grave_buff_list;//【杰出者大厅】buff列表 long: buff id, int: buff层树
        public long grave_celebrate_fixed_cd_id; // 【杰出者大厅】膜拜次数CD	
        public long grave_congratulate_reward_gain_fixed_cd; //【杰出者大厅】新晋杰出者膜拜次数CD
        #endregion


        #region 太空寻宝

        public NPCommonItem treasure_hunt_ore_normal_skill_point_item;//【太空寻宝】矿石普通技能点common_item
        public NPCommonItem treasure_hunt_ore_advanced_skill_point_item;//【太空寻宝】矿石高级技能点common_item
        public long treasure_hunt_lazy_cd_id;//【太空寻宝】挂机体力对应lazy_cd
        public NPCommonItem treasure_hunt_premium_energy_common_item;//【太空寻宝】普通能源对应common_item
        public NPCommonItem treasure_hunt_advanced_energy_common_item;//【太空寻宝】高级能源对应common_item
        public long treasure_hunt_akey_pickup_simple_unlock_id;//【太空寻宝】一键拾取功能的解锁条件simple_unlock id
        public long treasure_hunt_akey_consume_energy_limit;//【太空寻宝】一键拾取功能的拾取次数上限
        public long treasure_hunt_instant_pickup_simple_unlock_id; //【太空寻宝】快速跳过单次拾取的解锁条件simple_unlock id
        public int treasure_hunt_advanced_ore_min_grade;//【太空寻宝】高级矿石需要的最低档位索引（从0开始）
        public int treasure_hunt_pending_ore_num_limit;// 【太空寻宝】待处理矿石数量上限
        public int treasure_hunt_premium_energy_num_limit;//【太空寻宝】普通能源数量上限 领取的时候检查，当前已超过则不可领
        public int treasure_hunt_output_basic_num;//【太空寻宝】钻石产出基础值
        public long treasure_hunt_game_scene_id;//【太空寻宝】游戏场景id

        #endregion

        #region guild dungeon 联盟PVE副本 

        public WCGIntRange guild_dungeon_allow_hour_range;//公会副本允许开启的时间（小时）范围
        public NPTimeRefreshInfo guild_dungeon_auto_settle_time;//公会副本每日自动结算时间
        public long guild_dungeon_auto_fight_cond;//公会副本自动战斗条件(simple_unlock)
        public NPCommonCostItem guild_dungeon_recover_hero_cost;//公会副本恢复大臣出战次数的消耗
        public int guild_dungeon_recover_hero_limit;//大臣消耗道具恢复次数上限
        public int guild_dungeon_attack_contri_v;//公会副本攻击获得公会贡献度
        public int guild_dungeon_attack_contri_limit;//公会副本获得公会贡献度的攻击次数上限
        public long guild_dungeon_reward_mail_id;//奖励补发邮件ID

        #endregion
        #region guild Mars help 联盟互助

   
        public NPCommonCostItem guild_mars_help_deal_reward_item;//公会互助每次可获得的奖励
        public long guild_mars_help_deal_reward_fixed_cd_id;//公会互助可获得奖励总次数
       

        #endregion

        #region 联盟宝箱

        public long guild_box_claim_fixed_cd_id;//公会宝箱免费领取次数固定cd id
        public NPCommonItem guild_box_active_point_item;//联盟宝箱活跃点对应道具(sys_info)
        public int guild_box_effect_secs;//联盟宝箱有效时长（秒）

        #endregion

        #region 火星前往

        public List<WCGTripleLong> mars_go_route_arrive_reduce_list;//前往火星时长动态减少规则（人数下限:人数上限:减少万分比）例 1:5:5000;6:10:7500;11:-1:9500
        public List<string> mars_go_to_random_msg_list;//前往火星留言随机列表

        #endregion

        #region 火星居民

        public int mars_resident_dispatch_default_num;// 【火星基地】居民派遣默认数量
        public int mars_immigration_daily_max_num;//【火星基地】火星每日移民最大数量
        public NPCommonItem mars_satisfaction_value_common_item;//【火星基地】满意值道具
        public int mars_letter_limit = 10;//【火星基地】日常信件数量上限
        public int mars_daily_help_limit = 10;//【火星基地】每日求助数量上限
        public int mars_event_tip_show_num_limit = 10;//【火星基地】事件显示tip上限
        public long mars_immigration_show_silver_cost_tip_earning_multiples = 3600;//

        #endregion

        #region 火星扩建

        public long mars_technology_research_complete_tip_id;//【火星基地】 火星科技研究完成tipId
        public long mars_explore_daily_refresh_fixed_cd;//【火星探索】boss事件每日刷新fixed_cd

        #endregion
        
        #region 公会协作

        public NPCommonCostItem guild_cooperate_dispatch_time_reset_cost;//公会协助派遣次数重置消耗
        public long guild_cooperate_dispatch_time_reset_limit_fix_cd_id;//公会协助派遣次数重置上限fixCdId
        public long guild_cooperate_simple_unlock_id;//公会协作开启simple_unlock_id
        public List<WCGPairIntLong> guild_cooperate_differ_attr_count_prefab_res_id_list;//公会协助属性据点数量对应据点预制体id（数量：预制体ID）
        public long guild_cooperate_dispatch_same_property_add_per;//公会协助派遣同属性加成万分比
        public NPTimeRefreshInfo guild_cooperate_refresh_time;//公会协助重置时间
        public float guild_cooperate_reward_point_shortest_distance;//公会协助奖励据点两两之间最短距离，小于该距离的据点组合不予展示

        #endregion

        #region 首充礼包

        public long first_recharge_gift_pack_id;//首充礼包id
        public long first_recharge_simple_unlock_id;//首充礼包解锁simple_unlock_id

        #endregion

        #region 商店好评

        public List<NPCommonItem> item_show_shield_commonitem_list;//道具展示需要屏蔽的道具列表
        public NPCommonItem store_reviews_trigger_common_item;//触发商店好评道具
        public long store_reviews_reward_mail_id;//商店好评奖励邮件id
        public List<NPCommonCostItem> store_reviews_reward_item_list;//商店好评奖励道具列表
        public float store_reviews_trigger_cd_time_hour;//商店好评触发冷却时间（小时）

        #endregion

        #region 冲榜

        public float rank_rush_ranking_chg_tip_delay_show_time_sec;//冲榜排名变化tip延时展示时间（秒）
        public long rank_rush_ranking_chg_tip_show_simple_unlock_id;//冲榜排名变化tip展示条件simple_unlock_id

        #endregion

        #region 系统任务

        public long system_quest_finish_center_tip_id;//系统任务完成center tip id

        #endregion

        #region 限时兑换

        public int rush_exchange_day_can_exchange_times;// 每日可兑换次数
        public int rush_exchange_done_need_wait_sec; // 兑换完成所需等待时间 秒
        public int rush_exchange_refresh_sec; // 兑换组内 配置刷新间隔时间 秒
        public long rush_exchange_entry_simple_unlock_id; // 限时兑换的解锁条件
        #endregion

        #region 聊天系统通知 System Log
        public long midday_dungeon_system_log_id;
        public long evening_dungeon_system_log_id;

        #endregion

        #region 主线任务

        public List<string> can_not_show_main_quest_finish_tip_node_list;//不能展示主线任务完成tip的界面节点列表
        public List<string> can_not_show_main_quest_next_tip_node_list;//不能展示主线任务下一个任务tip的界面节点列表

        #endregion
    }

    public class NPSOGeneralRefSet : _TALSOBasicRefSet<NPGeneralRefObj>
    {
        public const long generalId = 1000;

        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
        public static string objName { get { return "general"; } }
    }
}


