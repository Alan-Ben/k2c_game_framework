namespace GOE
{
    /// <summary>
    /// 翻译KEY常量
    /// </summary>
    public class TransKeyConst
    {

        #region 平台翻译

        public const string init_forceUpdateTitle_none = "#1_init_forceUpdateTitle_none";//版本更新
        public const string init_updateGoTo_none = "#1_init_updateGoTo_none";//前往
        public const string init_suggestUpdateTitle_none = "#1_init_suggestUpdateTitle_none";//推荐更新
        public const string init_suggestUpdateTip_versionStr = "#1_init_suggestUpdateTip_versionStr";//有可更新版本（版本号{0}），是否前往更新？
        public const string init_forceUpdateTip_versionStr = "#1_init_forceUpdateTip_versionStr";//当前游戏运行版本过低，为保障您更好的体验，请前往更新版本至{0}。
        public const string init_gameres_fail_str = "#1_init_gameres_fail_str";//初始化游戏资源{0}失败!
        public const string init_login_fail_str = "#1_init_login_fail_str";//初始化登入{0}失败，是否重新登入!
        
        public const string init_cdn_not_found_none = "#1_init_cdn_not_found_none";//找不到对应版本号的CDN
        public const string init_invalid_platform_data_none = "#1_init_invalid_platform_data_none";//Invalid Platform Data
        
        // 仅内部测试或作弊
        public const string sdk_gm_test_purchase_str = "#1_sdk_gm_test_purchase_str";//是否使用GM命令测试购买?\n(GIFT_PACK:{0})
        public const string cheat_enter_command_none = "#1_cheat_enter_command_none";//Please enter your cheat command
        
        // 仅Editor
        public const string init_us_protocol_timeout_none = "#1_init_us_protocol_timeout_none";//发生进入US协议后，10秒了还没收到GS的001_004协议，找服务端排查！！！
        public const string init_comp_not_inited_str = "#1_init_comp_not_inited_str";//未初始化的组件: {0}
        public const string init_res_download_fail_num = "#1_init_res_download_fail_num";//ResDownloadFail: {0}
        public const string init_video_download_fail_num = "#1_init_video_download_fail_num";//videoDownloadFail: {0}
        public const string injectfix_patch_load_fail_str = "#1_injectfix_patch_load_fail_str";//InjectFix补丁加载失败 ,确认是否已经Inject{0}。
        
        public const string login_reqEnterServer = "#1_login_reqEnterServer";//连接服务器中
        public const string login_queue_ing = "#1_login_queue_ing";//您正在排队中，您的前面有{0}人，预计等待时间{1}
        public const string login_queue_end = "#1_login_queue_end";//即将进入梦想大陆
        public const string login_selfServerGroup = "#1_login_selfServerGroup";//我的服务器
        public const string login_serverSelect = "#1_login_serverSelect";//确认选择“{0}”服务器吗？
        public const string login_queueChgServer = "#1_login_queueChgServer";//您正在排队中，前方还有{0}人，预计时间{1}，是否要放弃排队切换到服务器：{2}？
        public const string login_serverFreeze = "#1_login_serverFreeze";//您的账号已被冻结，{0}后方可解除
        public const string login_chgServer = "#1_login_chgServer";//切换服务器
        public const string login_res_downloading = "#1_login_res_downloading";//下载资源中…
        public const string login_expand_res_fail = "#1_login_expand_res_fail";//资源获取失败,请尝试重新登入
        public const string login_disconnect_tip = "#1_login_disconnect_tip";//失去连接，请再次尝试！
        public const string login_confirm = "#1_login_confirm";//重登
        public const string login_loginFailedTipContent_cdn = "#1_cdn_fail_connect"; //cdn链接失败，请检查网络后重试
        public const string login_loginFailedTipContent_none = "#1_login_loginFailedTipContent_none";//很抱歉登录失败，请确认网络正常后重试~
        public const string login_loginFailedTipBtn_none = "#1_login_loginFailedTipBtn_none";//重试
        public const string login_loginFailedTipTitle_none = "#1_login_loginFailedTipTitle_none";//登录失败
        public const string login_signing_none = "#1_login_signing_none";//登录中
        public const string login_signsucc_none = "#1_login_signsucc_none";//登录成功！
        public const string sys_tip_title_none = "#1_sys_tip_title_none";//提示
        public const string dataerr_synching = "#1_dataerr_synching";//噢，御星师，发现资源不同步，请检查或者联系GM！
        public const string memory_not_enough_clear = "#1_memory_not_enough_clear";//存储空间不足，请尝试清理！
        public const string memory_confirm = "#1_memory_confirm";//清除
        public const string update_resource = "#1_update_resource";//有{0}资源等待更新
        public const string update_resource_nonwifi = "#1_update_resource_nonwifi";//有{0}资源等待更新，推荐使用WLAN更新，检测到您未使用WLAN，是否继续更新？
        public const string string_length_less_min = "#1_string_length_less_min";//小于最小长度
        public const string string_length_more_max = "#1_string_length_more_max";//大于最大长度
        public const string reconnect_tip = "#1_reconnect_tip";//与大陆失去连接，请再次尝试！
        public const string reconnect_confirm = "#1_reconnect_confirm";//确定(重连)
        public const string relogin_fail_tip = "#1_relogin_fail_tip";//重登失败
        public const string be_device_kick = "#1_be_device_kick";//噢！御星师，请留心您的账号已在其他设备登录
        public const string update_confirm = "#1_update_confirm";//确定
        public const string loading_allServerMaintain_none = "#1_loading_allServerMaintain_none";//全服维护中
        public const string policy_title_none = "#1_policy_title_none";//友情提示
        public const string policy_content_none = "#1_policy_content_none";//进入游戏前，请先阅读<a href=https://www.mjyx.com/en/service>《服务条款》</a>以及<a href=https://www.mjyx.com/en/policy>《隐私政策》</a>，感谢您的支持，祝您游戏愉快
        public const string policy_agree_none = "#1_policy_agree_none";//同意
        public const string quit_game = "#1_quit_game";//是否退出游戏
        public const string play_more = "#1_play_more";//再玩一会
        public const string quit = "#1_quit";//退出
        public const string ok = "#1_ok";//确定
        public const string confirm = "#1_confirm";//确定
        public const string cancel = "#1_cancel";//取消

        #endregion



        #region Common

        public const string common_sysUnOpen_none = "#1_common_sysUnOpen_none";//敬请期待
        public const string common_useNum_num_num = "#1_common_useNum_num_num";//{0}/{1}
        public const string common_useNum2_num_num = "#1_common_useNum2_num_num";//({0}/{1})
        public const string common_itemNotEnough_itemStr = "#1_common_itemNotEnough_itemStr";//{0}不足
        public const string common_percentage_num = "#1_common_percentage_num";//{0}%
        public const string common_propAddPer_num = "#1_common_propAddPer_num";//(+{0}%)
        public const string common_propAdd_num = "#1_common_propAdd_num";//(+{0})
        public const string common_propReducedPer_num = "#1_common_propReducedPer_num";//(-{0}%)
        public const string common_propReduced_num = "#1_common_propReduced_num";//(-{0})
        public const string common_addPropPer_num = "#1_common_addPropPer_num";//+{0}%
        public const string common_reducedPropPer_num = "#1_common_reducedPropPer_num";//-{0}%
        public const string common_add_num = "#1_common_add_num";//+{0}
        public const string common_somethingAdd_str_num = "#1_common_somethingAdd_str_num";//{0}+{1}
        public const string common_reduced_num = "#1_common_reduced_num";//-{0}
        public const string common_resource_haveNum = "#1_common_resource_haveNum";//拥有：{0}
        public const string common_resource_production_tip = "#1_common_resource_production_tip";//来源：{0}
        public const string common_parentheses_str = "#1_common_parentheses_str";//({0})
        public const string common_reward_preview_title = "#1_common_reward_preview_title";//概率获得以下奖励
        public const string common_value = "#1_common_value";//{0}
        public const string common_strDotStr = "#1_common_strDotStr_str_str";//{0}.{1}
        public const string common_getreward_tip = "#1_common_getreward_tip";//获得奖励
        public const string no_longer_tips_today_none = "#1_no_longer_tips_today_none";//今日不再提醒
        public const string confirm_delete_account = "#1_confirm_delete_account";//是否确认删除账号？
        public const string enter_ui_main_node_fail = "#1_enter_ui_main_node_fail";//跳转失败
        public const string login_username_empty_tip_none = "#1_login_username_empty_tip_none";//请输入用户名
        public const string login_password_empty_tip_none = "#1_login_password_empty_tip_none";//请输入密码
        public const string login_serverId_empty_tip_none = "#1_login_serverId_empty_tip_none";//请输入服务器IP
        public const string login_serverPort_empty_tip_none = "#1_login_serverPort_empty_tip_none";//请输入服务器端口
        public const string login_serverPort_number_none = "#1_login_serverPort_number_none";//请输入服务器端口号
        public const string login_serverID_number_none = "#1_login_serverID_number_none";//请输入服务器端口号
        public const string login_playerinfo_init_fail_none = "#1_login_playerinfo_init_fail_none";//用户信息初始化失败！
        public const string login_init_hotfix_component_failed = "#1_login_init_hotfix_component_failed";//初始化热更数据组件失败
        public const string server_refdata_version_different = "#1_server_refdata_version_different";//配表版本不一致
        public const string dialogue_skip_confirm_desc_none = "#1_dialogue_skip_confirm_desc_none";//确定跳过剧情吗？
        public const string dialogue_skip_confirm_title_none = "#1_dialogue_skip_confirm_title_none";//跳过剧情
        public const string common_nationPower_value = "#1_common_nationPower_value";//国力：{0}
        public const string common_level_num = "#1_common_level_num";//Lv.{0}
        public const string common_ownItemNum_num = "#1_common_ownItemNum_num";//拥有道具：{0}
        public const string common_closeCountDown_tip = "#1_common_closeCountDown_tip";//{0}后，自动关闭
        public const string common_words_limit_tip = "#2_common_words_limit_tip";//({0}/{1})
        public const string common_pageNum_num_num = "#1_common_pageNum_num_num";//{0}/{1}
        public const string common_str_colon_str = "#1_common_str_colon_str";// {0}:{1}
        public const string common_multiple_num = "#1_common_multiple_num";//x{0}
        public const string common_optimize_none = "#1_common_optimize_none";//优化
        public const string common_lowFrameRateTipDesc_none = "#1_common_lowFrameRateTipDesc_none";//女王姐，目前设备有点不流畅了，是否进行画质优化下~（<size=32>后续还可在设置中进行调整哈</size>）
        public const string common_you_none = "#1_common_you_none";//你
        public const string common_ID_num = "#1_common_ID_num";//ID:{0}
        public const string common_currentTotalNum_num_num = "#1_common_currentTotalNum_num_num";//{0}/{1}
        public const string common_default_none = "#1_common_default_none";//默认
        public const string common_level2_num = "#1_common_level2_num";//{0}级
        public const string common_quality_str = "#1_common_quality_str";//品质：{0}
        public const string common_earningSpeed_num = "#1_common_earningSpeed_num";//村庄收益：{0}/秒
        public const string common_levelAndLevelName_num_str = "#1_common_levelAndLevelName_num_str";//{0} {1}
        public const string common_time_str = "#1_common_time_str";//时间：{0}
        public const string common_interval_num_num = "#1_common_interval_num_num";//{0}~{1}
        public const string common_interval2_num_num = "#1_common_interval2_num_num";//{0}-{1}
        public const string common_noGainWayTip_none = "#1_common_noGainWayTip_none";//暂无获取途径
        public const string common_max_none = "#1_common_max_none";//Max
        public const string common_activity_notStart_none = "#1_common_activity_notStart_none";//活动未开启
        public const string common_activity_alreadyEnd_none = "#1_common_activity_alreadyEnd_none";//活动已结束
        public const string common_twoParam_str_str = "#1_common_twoParam_str_str";//{0}{1}
        public const string common_twoParamWithSpace_str_str = "#1_common_twoParamWithSpace_str_str";//{0} {1}
        public const string common_leftTime_str = "#1_common_leftTime_str";//剩余时间：{0}
        public const string common_VIPExp_str = "#1_common_VIPExp_str";//VIP经验 {0}
        public const string common_notObtained_str = "#1_common_notObtained_str";//{0}未获得

        #endregion

        #region 设置

        public const string setting_openMainLocalPush_none = "#1_setting_openMainLocalPush_none";//您已开启信息推送功能
        public const string setting_closeMainLocalPush_none = "#1_setting_closeMainLocalPush_none";//您已关闭信息推送功能
        public const string setting_openLocalPushItem_name = "#1_setting_openLocalPushItem_name";//您已开启{0}通知
        public const string setting_closeLocalPushItem_name = "#1_setting_closeLocalPushItem_name";//您已关闭{0}通知
        public const string setting_canNotOpenLocalPushTip_none = "#1_setting_canNotOpenLocalPushTip_none";//开启该通知需要先开启所有信息推送功能
        public const string setting_qualitySetupPicVeryLow_desc = "#2_setting_qualitySetupPicVeryLow_desc";//最低画质
        public const string setting_qualitySetupPicLow_desc = "#2_setting_qualitySetupPicLow_desc";//低画质
        public const string setting_qualitySetupPicNormal_desc = "#2_setting_qualitySetupPicNormal_desc";//中画质
        public const string setting_qualitySetupPicHigh_desc = "#2_setting_qualitySetupPicHigh_desc";//高画质
        public const string setting_qualitySetupPicUltra_desc = "#2_setting_qualitySetupPicUltra_desc";//最高画质
        public const string setting_switchQualityConfirmDesc_str_str = "#1_setting_switchQualityConfirmDesc_str_str";//是否确认从{0}切换到{1}？
        public const string setting_change_server_tip = "#1_setting_change_server";//是否返回登入界面切换服务器
        public const string audio_openBgAudioTip_none = "#1_audio_openBgAudioTip_none";//已开启配乐音量
        public const string audio_closeBgAudioTip_none = "#1_audio_closeBgAudioTip_none";//已关闭配乐音量
        public const string audio_openAudioTip_none = "#1_audio_openAudioTip_none";//已开启音效音量
        public const string audio_closeAudioTip_none = "#1_audio_closeAudioTip_none";//已关闭音效音量
        public const string audio_openVoiceTip_none = "#1_audio_openVoiceTip_none";//配音开启成功
        public const string audio_closeVoiceTip_none = "#1_audio_closeVoiceTip_none";//配音关闭成功
        public const string account_typeCurLogin_str = "#1_account_typeCurLogin_str";//{0}（当前已登录）
        public const string account_isCurLoginType_none = "#1_account_isCurLoginType_none";//已是当前登录方式
        public const string account_alreadyBind_none = "#1_account_alreadyBind_none";//已绑定
        public const string account_bindSucceed_none = "#1_account_bindSucceed_none";//绑定成功
        public const string account_bindFailedCode_num = "#1_account_bindFailedCode_num";//绑定失败（{0}）
        public const string account_switchFailedCode_num = "#1_account_switchFailedCode_num";//切换账号失败（{0}）
        public const string account_accountCurLogin_str = "#1_account_accountCurLogin_str";//{0}已登录
        public const string account_accountNotLogin_str = "#1_account_accountNotLogin_str";//{0}登录
        public const string account_loginTypeBind_str = "#1_account_loginTypeBind_str";//{0}已绑定
        public const string account_loginTypeUnBind_str = "#1_account_loginTypeUnBind_str";//绑定{0}

        #endregion

        #region 时间

        public const string chat_yesterday_timeStr = "#1_chat_yesterday_timeStr";//昨日 {0}
        public const string chat_weekTime_weekStr_timeStr = "#1_chat_weekTime_weekStr_timeStr";//{0} {1}
        public const string time_forever = "#1_time_forever_none";//永久
        public const string time_year_num = "#1_time_year_num";//{0}年
        public const string time_month_num = "#1_time_month_num";//{0}个月
        public const string time_hour_num = "#1_time_hour_num";//{0}时
        public const string time_day_num = "#1_time_day_num";//{0}天
        public const string time_minute_num = "#1_time_minute_num";//{0}分
        public const string time_lessOneMin_num = "#1_time_lessOneMin_num";//{0}秒
        public const string time_minSec_num = "#1_time_minSec_num";//{0}分{1}秒
        public const string timestamp_h_m = "#1_timestamp_h_m";//{0:D2}:{1:D2}
        public const string timestamp_h_m_s = "#1_timestamp_h_m_s";//{0:D2}:{1:D2}:{2:D2}
        public const string timestamp_m_d_h_m = "#1_timestamp_m_d_h_m";//{0:D2}/{1:D2} {2:D2}:{3:D2}
        public const string timestamp_m_d_y_h_m = "#1_timestamp_m_d_y_h_m";//{0}/{1:D2}/{2:D2} {3:D2}:{4:D2}
        public const string timestamp_m_d_y_h_m_s = "#1_timestamp_m_d_y_h_m_s";//{0}/{1:D2}/{2:D2} {3:D2}:{4:D2}:{5:D2}
        public const string time_duration = "#1_time_duration{0}-{1}(UTC{2})";//{0} - {1} GMT{2}
        public const string time_duration_cd = "#1_time_duration{0}-{1}(UTC{2})(CD{3})";//{0} - {1} GMT{2} （活动结束：{3}）
        public const string timestamp_y_m_d = "#1_timestamp_y_m_d";//{0}/{1:D2}/{2:D2}
        public const string timestamp_m_d = "#1_timestamp_m_d";//{0:D2}/{1:D2}
        public const string time_lessOneDay_num = "#1_time_lessOneDay_num";//{0}时{1}分
        public const string time_lessOneMinute_none = "#1_time_lessOneMinute_none";//1分钟内
        public const string time_moreOneDay_num = "#1_time_moreOneDay_num";//{0}天{1}时
        public const string time_duration_countdown = "#1_time_duration_countdown";//活动结束：{0}
        public const string monday = "#1_monday";//周一
        public const string tuesday = "#1_tuesday";//周二
        public const string wednesday = "#1_wednesday";//周三
        public const string thursday = "#1_thursday";//周四
        public const string friday = "#1_friday";//周五
        public const string saturday = "#1_saturday";//周六
        public const string sunday = "#1_sunday";//周日
        public const string month_abbr_Jan = "#1_month_abbr_Jan";//01
        public const string month_abbr_Feb = "#1_month_abbr_Feb";//02
        public const string month_abbr_Mar = "#1_month_abbr_Mar";//03
        public const string month_abbr_Apr = "#1_month_abbr_Apr";//04
        public const string month_abbr_May = "#1_month_abbr_May";//05
        public const string month_abbr_Jun = "#1_month_abbr_Jun";//06
        public const string month_abbr_Jul = "#1_month_abbr_Jul";//07
        public const string month_abbr_Aug = "#1_month_abbr_Aug";//08
        public const string month_abbr_Sept = "#1_month_abbr_Sept";//09
        public const string month_abbr_Oct = "#1_month_abbr_Oct";//10
        public const string month_abbr_Nov = "#1_month_abbr_Nov";//11
        public const string month_abbr_Dec = "#1_month_abbr_Dec";//12

        #endregion

        #region 活动

        public const string activity_activityShopBuyDailyLimit_num = "#1_activity_activityShopBuyDailyLimit_num";//每日限购 {0}
        public const string activity_activityShopBuyLimit_num = "#1_activity_activityShopBuyLimit_num";//限购 {0}
        public const string activity_itemSellOutTip_none = "#1_activity_itemSellOutTip_none";//售罄
        public const string activity_crystalGiftPackBuyConfirm_str = "#1_activity_crystalGiftPackBuyConfirm_str";//是否消耗{0}个{1}购买{2}个{3}
        public const string activity_crystalGiftPackTabName_none = "#1_activity_crystalGiftPackTabName_none";//钻石礼包
        public const string activity_playing_str = "#1_activity_playing_str";//进行中：{0}
        public const string activity_settling_str = "#1_activity_settling_str";//结算中：{0}
        public const string activity_gettingReward_str = "#1_activity_gettingReward_str";//领奖中：{0}
        public const string activity_stateUpdateTip_none = "#1_activity_stateUpdateTip_none";//活动状态更新

        #endregion

        #region 伙伴

        public const string hero_power_none = "#1_hero_power_none";//实力
        public const string hero_talent_none = "#1_hero_talent_none";//资质
        public const string hero_star_none = "#1_hero_star_none";//觉醒
        public const string hero_talentAddValue_num = "#1_hero_talentAddValue_num";//资质+{0}
        public const string hero_talentSkillIsMaxLevel_none = "#1_hero_talentSkillIsMaxLevel_none";//此技能已提升至满级
        public const string hero_talentSkillNameLevel_name_curLevel_maxLevel = "#1_hero_talentSkillNameLevel_name_curLevel_maxLevel";//{0}{1}/{2}
        public const string hero_talentValueAndNextValue_num_num = "#1_hero_talentValueAndNextValue_num_num";//资质+{0}（下级+{1}）
        public const string hero_talentUpgradeCostItemName_name = "#1_hero_talentUpgradeCostItemName_name";//使用{0}
        public const string hero_nextLevelAdd_num = "#1_hero_nextLevelAdd_num";//（下级+{0}）
        public const string hero_businessSkillUnlockCondDesc_name_level = "#1_hero_businessSkillUnlockCondDesc_name_level";//{0}Lv.{1}解锁
        public const string hero_businessSkillAutoUnlockDesc_name_level = "#1_hero_businessSkillAutoUnlockDesc_name_level";//{0}达到等级{1}，解锁了新的经营效果
        public const string hero_talentSkillLevelName_level_name = "#1_hero_talentSkillLevelName_level_name";//Lv.{0} {1}
        public const string hero_starSkillLevelName_level_name = "#1_hero_starSkillLevelName_level_name";//Lv.{0} {1}
        public const string hero_starSkillLevelDesc_num_str = "#1_hero_starSkillLevelDesc_num_str";//Lv.{0}:{1}
        public const string hero_starSkillDescSplitJoint_str_str = "#1_hero_starSkillDescSplitJoint_str_str";//{0}{1}
        public const string hero_powerAddValue_num = "#1_hero_powerAddValue_num";//基础值+{0}
        public const string hero_powerAddValuePer_num = "#1_hero_powerAddValuePer_num";//系数+{0}%
        public const string hero_upgradeHaloLevel_num = "#1_hero_upgradeHaloLevel_num";//提升星辉 {0}级
        public const string hero_accessWayDesc_str = "#1_hero_accessWayDesc_str";//获取途径：{0}
        public const string hero_heroTitle_str = "#1_hero_heroTitle_str";//伙伴称号：{0}
        public const string hero_consortTitle_str = "#1_hero_consortTitle_str";//家人称号：{0}
        public const string hero_blessHeroDesc_str_str = "#1_hero_blessHeroDesc_str_str";//{0}给予{1}的加护
        public const string hero_blessConsortNotGet_str = "#1_hero_blessConsortNotGet_str";//{0}（暂未获取）
        public const string hero_selfPowerAddValue_num = "#1_hero_selfPowerAddValue_num";//自身实力+{0}
        public const string hero_suitSkillNameLevel_name_num_num = "#1_hero_suitSkillNameLevel_name_num_num";//{0} {1}/{2}
        public const string hero_needActivePreHaloLevelTip_none = "#1_hero_needActivePreHaloLevelTip_none";//请先激活前置技能
        public const string hero_ownNum_num_num = "#1_hero_ownNum_num_num";//{0}/{1}
        public const string hero_heroName_str = "#1_hero_heroName_str";//伙伴名称：{0}
        public const string heroMars_teamPowerAdd_str = "#1_heroMars_teamPowerAdd_str";//所属火星探索队伍实力+{0}%

        #endregion

        #region 抽卡

        public const string avatar_gacha_unit_count = "#1_avatar_gacha_unit_count_num";//{0}/{1}
        public const string avatar_gacha_exchange_count = "#1_avatar_gacha_exchange_count_num";//有{0}个unit已经转换
        public const string avatar_gacha_unit_quality_title = "#1_avatar_gacha_unit_quality_title_num";//{0}星散件
        public const string avatar_gacha_suit_process_count = "#1_avatar_gacha_suit_process_count_str";//{0}收集进度
        public const string avatar_gacha_guarantee_tips = "#1_avatar_gacha_guarantee_tips";//玩家必在{0}次以内，获得一件未获得的{1}星套装部件（直至集齐全部{2}星套装）
        

        #endregion

        #region 情人

        public const string consort_skill_lvl_num = "#1_consort_skill_lvl_num";//技能lv.{0}
        public const string consort_skill_cost_count_num = "#1_consort_skill_cost_count_num";//技能消耗势力值：{0}
        public const string consort_skill_point_num = "#1_consort_skill_point_num";//势力值：{0}
        public const string consort_pos_attr_num = "#1_consort_pos_attr_num";//子嗣属性+{0}
        public const string consort_randcall_succ = "#1_consort_randcall_succ_none";//约会成功
        public const string consort_randcall_succ_child_str = "#1_consort_randcall_succ_child_str";//随机宠幸成功：{0},子嗣实例id：{1}
        public const string consort_randcall_succ_nochild_str = "#1_consort_randcall_succ_nochild_str";//随机宠幸成功：{0},没有生子嗣
        public const string consort_count_num = "#1_consort_count_num";//情人数量：{0}
        public const string consort_intimacy_attr_add_num = "#1_consort_intimacy_attr_add_num";//所有子嗣属性加成+{0}
        public const string consort_pos_count_num = "#1_consort_pos_count_num";//数量 {0}/{1}
        public const string consort_charm_num = "#1_consort_charm_num";//魅力{0}
        public const string consort_intimacy_num = "#1_consort_intimacy_num";//亲密度{0}
        public const string consort_halo_count_num = "#1_consort_halo_count_num";//{0}/{1}
        public const string consort_halo_basic_add_num = "#1_consort_halo_basic_add_num";//基础加成 +{0}
        public const string consort_halo_title_str = "#1_consort_halo_title_str";//实际加成 +{0}
        public const string consort_intimacy_halo_addattr_num = "#1_consort_intimacy_halo_addattr_num";//亲密光环对子嗣总价成：+{0}
        public const string consort_call_succ_child_str = "#1_consort_call_succ_child_str";//宠幸成功：{0},子嗣实例id：{1}
        public const string consort_call_succ_nochild_str = "#1_consort_call_succ_nochild_str";//宠幸成功：{0},没有生子嗣
        public const string consort_son_count = "#1_consort_son_count";//子嗣数量{0}
        public const string consort_son_attr = "#1_consort_son_attr";//子嗣总属性+{0}
        public const string consort_intimacy_chg_str = "#1_consort_intimacy_chg_str";//亲密度变化+{0}
        public const string consort_charm_chg_str = "#1_consort_charm_chg_str";//魅力值变化+{0}
        public const string consort_skillpoint_chg_str = "#1_consort_skillpoint_chg_str";//势力值变化+{0}
        public const string consort_like_chg_str = "#1_consort_like_chg_str";//好感度变化+{0}
        public const string consort_intimacy_critica_chg_str = "#1_consort_intimacy_critica_chg_str";//亲密度变化+{0} x{1}
        public const string consort_charm_critica_chg_str = "#1_consort_charm_critica_chg_str";//魅力值变化+{0} x{1}
        public const string consort_skillpoint_critica_chg_str = "#1_consort_skillpoint_critica_chg_str";//势力值变化+{0} x{1}
        public const string consort_like_critica_chg_str = "#1_consort_like_chg_critica_str";//好感度变化+{0} x{1}
        public const string consort_pos_count_max_none = "#1_consort_pos_count_max_none";//妃位人数满了
        public const string consort_skill_lock_tip = "#1_consort_skill_lock_tip";//技能未解锁
        public const string consort_call_cost_count_num = "#1_consort_call_cost_count_num";//x{0}
        public const string consort_common_process = "#1_consort_common_process_num";//{0}/{1}
        public const string consort_skill_attr = "#1_consort_skill_attr_num";//+{0}
        public const string consort_skill_unlock_intimacy_limit = "#1_consort_skill_unlock_intimacy_limit_num";//{0}/{1}  技能未解锁需求的亲密度
        public const string consort_pos_condition_disable_none = "#1_consort_pos_condition_disable_none";//不满足升品条件
        public const string consort_intimacy_range_str_str = "#1_consort_intimacy_range_num_num";//亲密度{0}-{1}
        public const string consort_call_count = "#1_consort_call_count_num";//约会次数：{0}
        public const string consort_call_skill_point_count = "#1_consort_call_skill_point_count_num";//势力值+{0} （一键约会结果弹窗）
        public const string consort_unlock_count_num = "#1_consort_unlock_count_num";//已获得妃子：{0}
        public const string consort_lock_count_num = "#1_consort_lock_count_num";//未获得妃子：{0}
        public const string consort_energy_count_num = "#1_consort_energy_count_num";//{0}/{1}   精力数量
        public const string consort_gem_call_discount = "#1_consort_gem_call_discount_num";//%{0}
        public const string consort_onekey_call_child_limit_tip = "#1_consort_onekey_call_child_limit_tip_none";//子嗣席位已满，无法获得更多更多子嗣，是否据徐约会?
        public const string consort_pos_attr_tip_title = "#1_consort_pos_attr_tip_title";//子嗣加成
        public const string consort_pos_attr_tip_content = "#1_consort_pos_attr_tip_content";//子嗣升级属性+{0}
        public const string consort_pos_lvlUpDesc_tip_title = "#1_consort_pos_lvlUpDesc_tip_title";//升温条件
        public const string consort_pos_lvlUpDesc_tip_content = "#1_consort_pos_lvlUpDesc_tip_content";//亲密值达到{0}，且魅力值达到{1}的知己可提升到{2}

        public const string consort_accessWayDesc_str = "#1_consort_accessWayDesc_str";//获取途径：{0}
        public const string consort_fetter_cannotLevelUp_none = "#1_consort_fetter_cannot_level_up_none";//不满足提升关系条件
        public const string consort_fetter_levelUpSuccTip_str1 = "#1_consort_fetter_levelUpSuccTip_str1";//关系已提升至{0}
        public const string consort_fetter_levelUpMaxLevelTip_none = "#1_consort_fetter_levelUpMaxLevelTip_none";//已达到最高等级
        public const string consort_business_skillEffectDesc_str_num = "#1_consort_business_skillEffectDesc_str_num";//{0}类建筑收益+{1}%
        public const string consort_business_skillEffectDescAllAttr_num = "#1_consort_business_skillEffectDescAllAttr_num";//所有公司收益+{0}%
        public const string consort_business_skillUnlockTip_num = "#1_consort_business_skillUnlockTip_num";//亲密度达到{0}, 领悟了新的经营技能
        public const string consort_skill_unlock_tip_num = "#1_consort_skill_unlock_tip_num";//再提升{0}亲密度可解锁该技能
        public const string consort_relate_next_level_desc = "#1_consort_relate_next_level_desc";//(下级+{0})
        public const string consort_skill_rate_title = "#1_consort_skill_rate_title";//成功率:{0}
        
        public const string consort_skin_onUnlockSkinConsortNotGotTip_str = "#1_consort_skin_onUnlockSkinConsortNotGotTip_str";//解锁该皮肤需要先获得妃子{0}
        public const string consort_skin_onUnlockSkinAlreadyGotTip_none = "#1_consort_skin_onUnlockSkinAlreadyGotTip_none";//已获取该妃子皮肤
        public const string consort_cgShareConfirmDesc_none = "#1_consort_cgShareConfirmDesc_none";//是否将{0}分享至聊天

        #endregion

        #region ConsortChat 情人互动
        public const string consort_chat_moment_player_high_light = "#1_consort_chat_moment_player_high_light";//<Color :>{0}
        public const string consort_chat_moment_reply = "#1_consort_chat_moment_reply";//回复{0} : {1}
        public const string consort_chat_msg_loading_mini_content = "#1_consort_chat_msg_loading_mini_content";//输入中。。。
        public const string consort_chat_moment_like_player_splitter = "#1_consort_chat_moment_like_player_splitter"; // ,
        public const string consort_chat_moment_send_time_str = "#1_consort_chat_moment_send_time_str";//{0}前
        public const string consort_chat_moment_already_comment_tip = "#1_consort_chat_moment_already_comment_tip";//已经评论过了
        public const string consort_chat_msg_dialogue_recent_desc = "#1_consort_chat_msg_dialogue_recent_desc";//最近
        public const string consort_chat_msg_dialogue_end_desc = "#1_consort_chat_msg_dialogue_end_desc";//本次对话结束
        public const string consort_chat_input_empty_tips = "#1_consort_chat_input_empty_tips";// 输入内容为空
        public const string consort_chat_moment_content_ai_req_prompt_str_str = "#1_consort_chat_moment_content_ai_req_prompt_str_str"; // AI朋友圈内容prompt {0:背景prompt} {1:角色prompt}
        public const string consort_chat_unlock_ai_chat_tip = "#1_consort_chat_unlock_ai_chat_tip";// {0}好感度到达{1}可解锁自由聊天
        public const string consort_chat_moment_consort_interaction_tip = "#1_consort_chat_moment_consort_interaction_tip";//{0}条@消息
        public const string consort_chat_moment_ai_comment_prompt_str_str = "#1_consort_chat_moment_ai_comment_prompt_str_str"; // 朋友的朋友圈内容为{0:朋友圈内容}，你会如何评论 


        #endregion

        #region 背包

        public const string bag_itemNotEnough_none = "#1_bag_itemNotEnough_none";
        public const string bag_notSelect_tip = "#1_bag_notSelect_tip";
        public const string bag_combineNotEnough_none = "#1_bag_combineNotEnough_none";
        public const string bag_select_tip = "#1_bag_select_tip";
        public const string bag_itemUseCount_num_num = "#1_bag_itemUseCount_num_num";//{0}/{1}
        public const string bag_select_num_num = "#1_bag_select_num_num";
        public const string bagItemUse_heroLv_str = "#1_bagItemUse_heroLv_str";//lv.{0}
        public const string bag_item_select_too_much_none = "#1_bag_item_select_too_much_none";
        public const string bag_combineNum_num_num = "#1_bag_combineNum_num_num";
        public const string bag_randomTitle_str = "#1_bag_randomTitle_str";//本次共使用{0}
        public const string bag_noHeroTip_str = "#1_bag_noHeroTip_str";//使用骑士道具时没有骑士弹出tip
        public const string bag_noConsortTip_str = "#1_bag_noConsortTip_str";//使用妃子道具时没有妃子弹出tip
        public const string bag_useTitle_none = "#2_bag_use_title1";//道具使用弹窗标题
        public const string bag_convert_item_num = "#1_bag_convert_item_num";//数量：{0}/{1}
        public const string bag_consortAddValueType_str = "#1_bag_consortAddValueType_str";//家人{0}提升
        public const string bag_heroAddValueType_str = "#1_bag_heroAddValueType_str";//伙伴{0}提升
        public const string bag_heroRandomUseFail_str = "#1_bag_heroRandomUseFail_str";//道具使用失败，未拥有{0}特长伙伴
        public const string bag_noImproveItemCanUse_none = "#1_bag_noImproveItemCanUse_none";//暂无提升道具
        public const string bag_useAllItem_none = "#1_bag_useAllItem_none";//全部使用

        #endregion



        #region 子嗣

        public const string child_oneKeyNamingTip_none = "#1_child_oneKeyNamingTip_none"; // 勾选后点击取名，将同时为所有需要的学生进行默认取名
        public const string child_emptyRoomToConsortTip_none = "#1_child_emptyRoomToConsortTip_none"; // 当前教室中暂无学生，是否前往邀约家人？
        public const string child_graduationDesc_name = "#1_child_graduationDesc_name"; // {0}从高级学衔毕业啦！....
        public const string child_stepUpDesc_name = "#1_child_step{0}UpDesc_name"; // {0}升学啦！....
        public const string child_illegalNameTip_none = "#1_child_illegalNameTip_none"; // 命名含有非法字符
        public const string child_educatingEnergyEmpty_none = "#1_child_educatingEnergyEmpty_none"; // 脑力不足
        public const string child_energyIsFullTip_none = "#1_child_energyIsFullTip_none"; // 无需使用脑力糖果
        public const string child_room_EP_num = "#1_child_room_EP_num"; // 脑力值：{0}/{1}
        public const string child_room_EP_countdown_desc = "#1_child_room_EP_countdown_desc"; // 脑力值：{0}
        public const string child_EP_current_num = "#1_child_EP_current_num";
        public const string child_multiGraduationChildNum_num = "#1_child_multiGraduationChildNum_num"; // 共计毕业人数：{0}
        public const string child_multiGraduationTotalEarnings_num = "#1_child_multiGraduationTotalEarnings_num"; // 共计收益：{0}
        public const string child_energyRecoverSuccess_none = "#1_child_energyRecoverSuccess_none"; // 脑力值已补充
        public const string child_energyRecoverNum_num = "#1_child_energyRecoverNum_num"; // 脑力值+{0}
        public const string child_engagedTargetRefuseAllRequest_none = "#1_child_engagedTargetRefuseAllRequest_none"; // 对方已设置拒绝所有邀约
        public const string child_shareChildAlreadyMarried_none = "#1_child_shareChildAlreadyMarried_none"; // 该学徒已组队

        #endregion



        #region 大学

        public const string college_mulSelectIsMax_none = "#1_college_mulSelectIsMax_none";//进修名额已满，请进行调整
        public const string college_isAllLearning_none = "#1_college_isAllLearning_none";
        public const string college_collegeNum_num_num = "#1_college_collegeNum_num_num";//大学席位:{0}/{1}
        public const string college_mulChooseNum_num_num = "#1_college_mulChooseNum_num_num";//大学席位:{0}/{1}
        public const string college_studying_none = "#2_college_studying_none";//大学进修中
        public const string college_extraPosIsMax_none = "#1_college_extraPosIsMax_none";//大学位置达到上限

        #endregion



        #region 关卡
        public const string chapter_power_less_than_boss = "#1_chapter_power_less_than_boss";//实力不足，无法挑战BOSS
        public const string chapter_click_power_less_than_boss = "#1_chapter_click_power_less_than_boss";//实力不足，无法挑战BOSS
        public const string chapter_canNotFind_none = "#1_chapter_canNotFind_none"; //找不到关卡id显示的key
        public const string chapter_power_pass_boss = "#1_chapter_power_pass_boss"; //当前战力已超过boss
        public const string chapter_event_dispatch_no_hero_tip = "#1_chapter_event_dispatch_no_hero_tip"; //请选择大臣
        public const string chapter_event_dispatch_hero_already_max_tip = "#1_chapter_event_dispatch_hero_already_max_tip"; //派遣人数已满
        public const string chapter_currentTotalNum_num_num = "#1_chapter_currentTotalNum_num_num"; //{0} / {1}
        public const string chapter_forward_cost_add = "#1_chapter_forward_cost_add"; //当前顾问实力为{0}，关卡消耗增加 <size=38><color=#ff137b>{1}%</color></size>
        public const string chapter_forward_cost_reduce = "#1_chapter_forward_cost_reduce"; //当前顾问实力为{0}，关卡消耗减少 <size=38><color=#06fff6>{1}%</color></size>
        public const string chapterMax_tip = "#1_chapterMax_tip"; //恭喜您完成了所有关卡！更多精彩内容，敬请期待。
        public const string chapter_quick_forward_wnd_title = "#1_chapter_quick_forward_wnd_title";//窗口快速前进提示窗口标题
        public const string chapter_quick_forward_wnd_desc = "#1_chapter_quick_forward_wnd_desc";//窗口快速前进提示窗口描述

        #endregion



        #region 征收

        public const string levy_soldier_curCount_num_num = "#1_levy_soldier_curCount_num_num";//当前烘焙次数{0}/{1}
        public const string levy_soldier_countToMaxTime_num = "#1_levy_soldier_countToMaxTime_num";//烘焙累计满剩余时间 {0}
        public const string levy_silver_curCount_num_num = "#1_levy_silver_curCount_num_num";//当前银币累计{0}/{1}
        public const string levy_silver_countToMaxTime_num = "#1_levy_silver_countToMaxTime_num";//银币累计满剩余时间 {0}
        public const string levy_solider_crit_base_num = "#1_levy_solider_crit_base_num";//暴击基础值：{0}
        public const string levy_solider_crit_big_num = "#1_levy_solider_crit_big_num";//暴击倍数：{0}
        #endregion



        #region 建筑
        public const string building_earningTip_num = "#1_building_earningTip_num";
        public const string building_hudName_str_num = "#1_building_hudName_str_num"; // {0} {1}级
        public const string building_businessBuildingTitle_name_level = "#1_building_businessBuildingTitle_name_level"; // {0}·{1}级
        public const string building_businessBuildingTotalEarningsPerS_num = "#1_building_businessBuildingTotalEarningsPerS_num"; // {0}/秒
        public const string building_businessBuildingEmployeeEarningsPerPerson_num = "#1_building_businessBuildingEmployeeEarningsPerPerson_num"; // {0}/人
        public const string building_businessBuildingEmployeeNum_num_num = "#1_building_businessBuildingEmployeeNum_num_num"; // 员工数量：{0}/{1}
        public const string building_businessBuildingEarningsBonus_num = "#1_building_businessBuildingEarningsBonus_num"; // 收益倍率：{0}%
        public const string building_businessBuildingNextEarningsBonus_num = "#1_building_businessBuildingNextEarningsBonus_num"; // (下一等级{0}%)
        public const string building_businessBuildingHeroSlotFull_none = "#1_building_businessBuildingHeroSlotFull_none"; // 驻扎槽位已满
        public const string building_businessBuildingAvailableHeroBarTitle_name = "#1_building_businessBuildingAvailableHeroBarTitle_name"; // 可委任到{0}的伙伴
        public const string building_businessBuildingNotGainHeroBarTitle_name = "#1_building_businessBuildingNotGainHeroBarTitle_name"; // 未获得的的委任到{0}的伙伴
        public const string building_businessBuildingHeroAlreadyJoinsAnother_name = "#1_building_businessBuildingHeroAlreadyJoinsAnother_name"; // 已经委任到{0}中
        public const string building_businessBuildingDetailTotalEarningsPerS_num = "#1_building_businessBuildingDetailTotalEarningsPerS_num"; // 收益：{0}/秒
        public const string building_infoPowerNum_num1 = "#1_building_infoPowerNum_num1"; // {0}/秒
        public const string building_businessBuildingDetailBaseEarningsPerS_num = "#1_building_businessBuildingDetailBaseEarningsPerS_num"; // 基础收益：{0}/秒
        public const string building_businessBuildingDetailHeroEarningsPerS_num = "#1_building_businessBuildingDetailHeroEarningsPerS_num"; // 伙伴经营能力：{0}/秒
        public const string building_businessBuildingDetailTotalEarningsGoldPerS_num = "#1_building_businessBuildingDetailTotalEarningsGoldPerS_num"; // 每秒产出金币{0} 
        public const string building_businessBuildingDetailEmployeeEarningsPerS_num = "#1_building_businessBuildingDetailEmployeeEarningsPerS_num"; // 员工总收益：{0}/秒
        public const string building_addInnEarningsPerS_num = "#1_building_addInnEarningsPerS_num"; // 太空运输加成：{0}/秒
        public const string building_addTreasureHuntPerS_num = "#1_building_addTreasureHuntPerS_num"; // 太空打捞加成：{0}/秒
        public const string building_businessBuildingDetailEarningsBonus_num = "#1_building_businessBuildingDetailEarningsBonus_num"; // 收益加成：{0}%
        public const string building_businessBuildingDetailPlacedHeroBonus_num = "#1_building_businessBuildingDetailPlacedHeroBonus_num"; // 委任伙伴+{0}%
        public const string building_businessBuildingDetailUpgradeBonus_num = "#1_building_businessBuildingDetailUpgradeBonus_num"; // 店铺升级+{0}%
        public const string building_businessBuildingDetailTowerEarningsPers_num = "#1_building_businessBuildingDetailTowerEarningsPers_num"; // 迷宫加成+{0}%
        public const string building_businessBuildingDetailMonthCardEarningsPers_num = "#1_building_businessBuildingDetailMonthCardEarningsPers_num"; // 月卡加成+{0}%
        public const string building_businessBuildingDetailYearCardEarningsPers_num = "#1_building_businessBuildingDetailYearCardEarningsPers_num"; // 年卡加成+{0}%
        public const string building_businessBuildingHeroBonus_num = "#1_building_businessBuildingHeroBonus_num"; // 委任加成+{0}%
        public const string building_businessBuildingHeroSlotUnlockTip_num = "#1_building_businessBuildingHeroSlotUnlockTip_num"; // 招聘{0}名员工解锁
        public const string building_earningsPerPerson_num = "#1_building_earningsPerPerson_num"; // {0}/人
        public const string building_getEmployeeCount_str_num = "#1_building_getEmployeeCount_str_num"; //{0}员工+{1}
        public const string building_videoUnlockTip_level = "#1_building_videoUnlockTip_level";// lv.{0} unlock
        public const string building_businessBuildingPageLevel_level = "#1_building_businessBuildingPageLevel_level";// 当前等级：LV{0}
        public const string building_businessBuildingPageEmployee_num = "#1_building_businessBuildingPageEmployee_num";// 员工数量：{0}
        public const string building_businessBuildingDevelopUnlockTip_level = "#1_building_businessBuildingDevelopUnlockTip_level";// 建筑达到{0}级
        public const string building_clickEarningMutipleValueTip_num = "#1_building_clickEarningMutipleValueTip_num";//暴击X{0}
        public const string building_employeeNumReachLimitTip_none = "#1_building_employeeNumReachLimitTip_none";//当前员工人数已达到上限，提升建筑等级可继续招募
        public const string building_buildSuccessTitle_name = "#1_building_buildSuccessTitle_name";//【{0}】建造成功
        public const string building_upgradeSuccessTitle_name = "#1_building_upgradeSuccessTitle_name";//【{0}】建造成功
        #endregion


        #region 任务

        public const string quest_questProgress_str = "#1_quest_questProgress_str";//任务进度：{0}
        public const string funcUnlock_unlockName_name = "#1_funcUnlock_unlockName_name";//解锁：{0}
        public const string funcUnlock_unlockCond_str = "#1_funcUnlock_unlockCond_str";//解锁条件：{0}
        public const string quest_nameAndProgress_str_str = "#1_quest_nameAndProgress_str_str";//{0}{1}
        public const string funcUnlock_nextUnlock_str = "#1_funcUnlock_nextUnlock_str";//即将解锁{0}

        #endregion

        #region 成年子嗣
        
        public const string adult_totalAdultCount_num = "#1_adult_totalAdultCount_num";//总毕业人数：{0}
        public const string adult_totalAdultEarnings_num = "#1_adult_totalAdultEarnings_num";//毕业人数：{0}
        public const string adult_marriedAdultMaxShowCount_num = "#1_adult_marriedAdultMaxShowCount_num";//只保留最新{0}个已联谊毕业生信息
        public const string adult_noSuitableAdultToMarry_none = "#1_adult_noSuitableAdultToMarry_none";//暂无合适的子嗣可联谊
        public const string adult_marriedAdultRarnings_num = "#1_adult_marriedAdultRarnings_num";//联谊收益：{0}
        public const string adult_graduate_limited_num = "#1_adult_graduate_limited_num";//未联谊的毕业生最多容纳{0}人
        // public const string adult_totalUnmarriedAdultNum_num = "#1_adult_totalUnmarriedAdultNum_num";//未婚子嗣：{0}
        
        #endregion

        #region 捏脸

        public const string make_face_import_empty_tip_none = "#1_make_face_import_empty_tip_none";//请输入捏脸数据
        public const string make_face_import_error_none = "#1_make_face_import_error_none";//数据出现错误
        public const string make_face_choice_prefab_title_none = "#1_make_face_choice_prefab_title_none";//选择形象
        public const string make_face_change_prefab_title_none = "#1_make_face_change_prefab_title_none";//修改形象
        public const string make_face_upload_tip_none = "#1_make_face_upload_tip_none";//您是否确定使用该形象进入游戏？
        public const string make_face_can_upload_tip_desc_none = "#1_make_face_can_upload_tip_desc_none";//捏脸尚未保存，是否退出当前界面？
        public const string make_face_copy_succ = "#1_make_face_copy_succ_none";//复制捏脸数据成功
        public const string make_face_enter_editing_title_none = "#1_make_face_enter_editing_title_none";//友情提示
        public const string make_face_enter_editing_content_none = "#1_make_face_enter_editing_content_none";//您的操作尚未保存，未保存搭配直接前往捏脸操作将会失去您的搭配，是否前往？
        

        #endregion

        #region 衣橱
        public const string clothes_suitRewardNoDes_tip = "#1_clothes_suitRewardNoDes_tip";//奖励预览弹窗标题
        public const string clothes_suitRewardGotDes_tip = "#1_clothes_suitRewardGotDes_tip";//套装已领取时的预览弹窗界面的描述展示文本
        public const string clothes_suit_reward_preview_title = "#1_clothes_suit_reward_preview_title_none";//奖励预览弹窗标题
        public const string clothes_customSuitDefaultName_num = "#1_clothes_customSuitDefaultName_num";// 自定义套装{0}
        public const string clothes_bgSaveComplete_none = "#1_clothes_bgSaveComplete_none";// 背景保存完成
        public const string clothes_poseSaveComplete_none = "#1_clothes_poseSaveComplete_none";// 姿势保存完成
        public const string clothes_handPoseSaveComplete_none = "#1_clothes_handPoseSaveComplete_none";// 手势保存完成
        public const string clothes_actionSaveComplete_none = "#1_clothes_actionSaveComplete_none";// 动作保存完成
        public const string clothes_actionIntervalSaveComplete_none = "#1_clothes_actionIntervalSaveComplete_none"; // 动作间隔保存成功
        public const string clothes_no_chg_none = "#1_clothes_no_chg_none";// 您当前未修改任何内容
        public const string clothes_unitSaveComplete_none = "#1_clothes_unitSaveComplete_none";// 时装保存完成
        public const string clothes_unitSearchFailed_none = "#1_clothes_unitSearchFailed_none";// 时装搜索失败
        public const string clothes_handbookLevel_num = "#1_clothes_handbookLevel_num";// 收集度：LV.{0}
        public const string clothes_bgEditCancelConfirm_none = "#1_clothes_bgEditCancelConfirm_none";// 是否保存当前所有修改
        public const string clothes_handbookLevelProgress_num_num = "#1_clothes_handbookLevelProgress_num_num";// {0}/{1}
        public const string clothes_handbookTotalExp_num = "#1_clothes_handbookTotalExp_num";// 总积分：{0}
        public const string clothes_handbookPowerBonus_num = "#1_clothes_handbookPowerBonus_num";// 总国力：{0}
        public const string clothes_handbookRewardLevel_num = "#1_clothes_handbookRewardLevel_num";// 等级 {0}
        public const string clothes_suitTypeCollectionProgress_num_num = "#1_clothes_suitTypeCollectionProgress_num_num";// 套装 {0}/{1}
        public const string clothes_suitTypeCollectionProgressSimple_num_num = "#1_clothes_suitTypeCollectionProgressSimple_num_num";// {0}/{1}
        public const string clothes_suitDetailCollectionProgress_num_num = "#1_clothes_suitDetailCollectionProgress_num_num";// ({0}/{1})
        public const string clothes_editCancelConfirm_none = "#1_clothes_editCancelConfirm_none";// 有操作尚未保存，是否退出当前界面？
        public const string clothes_unitEditChgConfirm_none = "#1_clothes_unitEditChgConfirm_none";// 时装尚未保存，是否切换？
        public const string clothes_suit_menu_progress_num_num = "#1_clothes_suit_menu_progress_num_num";// 套装 {0}/{1}
        public const string clothes_suit_dir_menu_name = "#1_clothes_suit_dir_menu_name_none";// 套装
        public const string clothes_shareClothesContent_none = "#1_clothes_shareClothesContent_none";//您是否要分享当前穿搭？
        public const string clothes_shareClothesTitle_none = "#1_clothes_shareClothesTitle_none";//分享穿搭
        public const string clothes_shareSucceedTip_none = "#1_clothes_shareSucceedTip_none";//穿搭分享成功
        public const string clothes_shareNotHaveTip_none = "#1_clothes_shareNotHaveTip_none";//您有部分时装未拥有，无法全部同款穿搭
        public const string clothes_shareColorDiffenceTip_none = "#1_clothes_shareColorDiffenceTip_none";//您有部分时装颜色不同
        public const string clothes_canNotShareClothesTip_none = "#1_clothes_canNotShareClothesTip_none";//聊天系统还未解锁，暂时无法分享搭配
        public const string clothes_actionSelectedUnavailable_none = "#1_clothes_actionSelectedUnavailable_none";//当前动作不可用
        public const string clothes_handPoseSelectedUnavailable_none = "#1_clothes_handPoseSelectedUnavailable_none";//当前手势不可用
        public const string clothes_poseSelectedUnavailable_none = "#1_clothes_poseSelectedUnavailable_none";//当前姿势不可用
        public const string clothes_actionSelectionTip_num_num = "#1_clothes_actionSelectionTip_num_num";//当前选择的动作：{0}/{1}
        public const string clothes_actionSelectedMaxCount_none = "#1_clothes_actionSelectedMaxCount_none";//当前选择的动作已达上限
        public const string clothes_handbookTargetExp_num = "#1_clothes_handbookTargetExp_num";// 所需积分：{0}
        public const string clothes_handbookCurExp_num = "#1_clothes_handbookCurExp_num";// 当前积分：{0}
        public const string clothes_suitSld_title = "#1_clothes_suitSld_title";//<size=68><color=#885e5e>{0}</color></size>/{1}
        public const string clothes_share_count_num = "#1_clothes_share_count_num";// 次数：{0}/{1}
        public const string clothes_saveDyeComplete_none = "#1_clothes_saveDyeComplete_none";// 染色保存完成
        public const string clothes_saveDyeNoChange_none = "#1_clothes_saveDyeNoChange_none";// 染色未发生变化
        public const string clothes_saveDyePaletteLocked_none = "#1_clothes_saveDyePaletteLocked_none";// 调色板未解锁
        public const string clothes_dyeEditDataAbandonTip_none = "#1_clothes_dyeEditDataAbandonTip_none";// 染色数据尚未保存，是否放弃编辑
        public const string clothes_dyeDefaultPaletteName_none = "#1_clothes_dyeDefaultPaletteName_none";// 默认调色板
        public const string clothes_paletteUnlockTipTitle_none = "#1_clothes_paletteUnlockTipTitle_none";// 解锁提示
        public const string clothes_paletteUnlockTipContent_name_item = "#1_clothes_paletteUnlockTipContent_name_item";// {0}还未解锁，重复抽取该时装有机率随机解锁该色盘，或者使用{1}解锁。是否使用{1}解锁？

        #endregion

        #region 宴会
        public const string dinner_common_score = "#1_dinner_score";// 积分{0}
        public const string dinner_common_joiner_count = "#1_dinner_joiner_count";// <size >{0}</size>/{1}
        public const string dinner_common_bejoined_dinner_num = "#1_dinner_common_join_my_dinner_num";// 参加过我的宴会次数{0}
        public const string dinner_common_join_player_dinner_num = "#1_dinner_common_join_player_dinner_num";// 我参加过对方的宴会次数{0}
        public const string dinner_main_score = "#1_dinner_main_score";// 积分{0}
        public const string dinner_permit_cd = "#1_dinner_permit_cd";// 宴会将在{0}后过期
        public const string dinner_join_cost_coin = "#1_dinner_join_cost_coin_mum";// 宴会币+{0}
        public const string dinner_join_cost_score = "#1_dinner_join_cost_score_num";// 宴会积分+{0}
        public const string dinner_join_cost_count = "#1_dinner_join_cost_count_num";// ({0}/{1})
        public const string dinner_join_cost_tooltip_basic = "#1_dinner_join_cost_tooltip_basic";// 基础人气:{0}
        public const string dinner_join_cost_tooltip_rank_bonus = "#1_dinner_join_cost_tooltip_rank_bonus";// 评级加成:+{0}%
        public const string dinner_join_cost_tooltip_fellow_talent = "#1_dinner_join_cost_tooltip_fellow_talent";// 伙伴觉醒:+{0}%
        public const string dinner_join_cost_tooltip_family_relation_ship = "#1_dinner_join_cost_tooltip_family_relation_ship";// 家人关系:+{0}%
        public const string dinner_start_log_item_consort_desc = "#1_dinner_start_log_item_consort_desc_str";// {0}的妃子宴会
        public const string dinner_create_seat_count = "#1_dinner_create_seat_count";// 席位：{0}
        public const string dinner_join_add_score = "#1_dinner_join_add_score_num";// 赴宴积分：{0}%
        public const string dinner_guest_log_coin = "#1_dinner_guest_log_coin_str";// 宴会币：{0}
        public const string dinner_guest_log_gift = "#1_dinner_guest_log_gift_str";// 礼物：{0}
        public const string dinner_result_score = "#1_dinner_result_score";// 人气：{0}(+{1}%)
        public const string dinner_guest_hero_name = "#1_dinner_guest_hero_name";// 大臣参加宴会的名字：(Fellow){0}
        public const string dinner_cost_count_out_none = "#1_dinner_cost_count_out_none";// 这个方式的赴宴次数已用完
        public const string dinner_chat_invite_title = "#1_dinner_chat_invite_title_str";// {0}点击进入
        public const string dinner_chat_invite_desc = "#1_dinner_chat_invite_desc_str";// 我的城堡正在举办{0}，期待你的到来
        public const string dinner_chat_seat_count = "#1_dinner_chat_seat_count_num";// 宾客席位{0}/{1}
        public const string dinner_invite_cding_tip = "#1_dinner_invite_cding_tip_str";// 还需要 {0}可继续邀请
        public const string dinner_is_over_tip = "#1_dinner_is_over_none";// 宴会已经结束
        public const string dinner_player_join_my_dinner_tip = "#1_dinner_player_join_my_dinner_tip";// {0}带着{1}前来参加您的{2}
        public const string dinner_permit_get_go_to_create_tip = "#1_dinner_permit_get_go_to_create_tip";// 当前正在举办宴会，需等待宴会结束才能举办新的宴会
        public const string dinner_joiner_is_my_hero_tip = "#1_dinner_joiner_is_my_hero_tip";// 该角色为你的伙伴
        public const string dinner_detail_log_create_info_str = "#1_dinner_detail_log_create_info_str";// {0玩家}创建了{1类型}宴会
        public const string dinner_detail_log_join_info_str = "#1_dinner_detail_log_join_info_str";// {0玩家}带着{1加入方式}为宴会增加了{2}人气
        public const string dinner_bar_celebration_name = "#1_dinner_bar_celebration_name";// 庆典bar
        public const string dinner_bar_party_name = "#1_dinner_bar_party_name";// 聚会bar
        public const string dinner_detail_timestamp_h_m_s = "#1_dinner_detail_timestamp_h_m_s";//{0:D2}:{1:D2}:{2:D2}
        public const string dinner_enterTimestamp_h_m_s = "#1_dinner_enterTimestamp_h_m_s";//{0:D2}:{1:D2}:{2:D2}
        public const string dinner_seatNum_num_num = "#1_dinner_seatNum_num_num";//{0}/{1} 席位
        public const string dinner_player_has_join_tip =  "#1_dinner_player_has_join_tip";//该玩家已加入宴会
        public const string dinner_share_server_suc_tip = "#1_dinner_share_server_suc_tip";//您已成功分享至全服
        public const string dinner_share_guild_suc_tip = "#1_dinner_share_guild_suc_tip";//您已成功分享至联盟
        public const string dinner_share_fail_tip = "#1_dinner_share_fail_tip";//【{0:cd时长}后可再次分享
        #endregion

        #region 好友
        public const string friend_shield_count = "#1_friend_shield_count_num";//当前人数{0}/{1}
        public const string friend_shield_isShield_none = "#1_friend_shield_isShield_none";//屏蔽的对象已经在屏蔽列表中
        public const string friends_send_isShield_none = "#1_friends_send_isShield_none";//您已屏蔽对方，添加好友需要取消屏蔽操作
        public const string friend_shield_content_none = "#1_friend_shield_content_none";//是否屏蔽玩家{0}
        public const string friend_shield_title_none = "#1_friend_shield_title_none";//屏蔽提示标题
        public const string friend_unshield_content_none = "#1_friend_unshield_content_none";//是否取消屏蔽玩家{0}
        public const string friend_unshield_title_none = "#1_friend_unshield_title_none";//取消屏蔽提示标题
        public const string friend_shield_cid_limit = "#1_friend_shield_cid_limit_tip";//屏蔽列表已达上限
        public const string friend_editgroup_move_none_tip = "#1_friend_editgroup_move_none_tip";//未选择好友移动分组
        public const string friend_editgroup_rename_tip = "#1_friend_editgroup_rename_tip";// 改名成功
        public const string friends_group_need_add_tip = "#1_friends_group_need_add_tip_none";// 请新增好友分组
        public const string friends_group_name_char_count = "#1_friends_group_name_char_count_num";// 字符：{0}/{1}
        public const string friends_group_name_char_count_max = "#1_friends_group_name_char_count_max_none";// 输入的字符超过上限，请修改名称
        public const string friends_offline_time_str = "#1_friends_offline_time_str";// 离线{0}"
        public const string friends_custom_group_count_max = "#1_friends_custom_group_count_max_none";//自定义分组已达上限
        public const string friends_group_name_noChg = "#1_friends_group_name_noChg_none";//分组名称未修改
        public const string friends_del_friend_tip_desc = "#1_friends_del_friend_tip_desc_none";//是否删除该好友？
        public const string friends_group_del_tip_desc = "#1_friends_group_del_tip_desc_none";//删除该分组后将不可恢复，您是否要删除该分组？
        public const string friends_custom_group_player_count_limit = "#1_friends_group_count_limit_num";//好友数量限制{0}/{1}
        public const string friends_group_count_max = "#1_friends_group_count_max_none";//好友分组已达上限
        public const string friends_group_count_limit = "#1_friends_group_count_limit_num";//您最多可创建{0}个分组
        public const string friends_is_frend_tip_str = "#1_friends_is_frend_tip_str";//您和{0}已是好友
        public const string friends_agree_add_str = "#1_friends_agree_add_str";//同意{0}好友申请提示
        public const string friends_refuse_add_str = "#1_friends_refuse_add_str"; //拒绝{0}好友申请提示
        public const string friends_allAgree_none = "#1_friends_allAgree_none";//您已一键通过所有好友申请
        public const string friends_allRefuse_none = "#1_friends_allRefuse_none";//您已一键忽略所有好友申请
        public const string friends_default_group_cannot_chg_name = "#1_friends_default_group_cannot_chg_name_none";//默认分组不能改名
        public const string friends_default_group_cannot_remove = "#1_friends_default_group_cannot_remove_none";//默认分组不能删除
        public const string friends_group_name_empty_tip = "#1_friends_group_name_empty_tip_none";//分组名字不能为空
        public const string friends_group_default_name = "#1_friends_group_default_name_none";//默认好友分组
        public const string friends_count_num = "#1_friends_count_num";//好友{0}/{1}
        public const string friends_group_edi_friend_count_num = "#1_friends_group_edi_friend_count_num";//好友{0}/{1}
        public const string friends_agree_add_none = "#1_friends_agree_add_none";//同意好友申请提示
        public const string friends_refuse_add_none = "#1_friends_refuse_add_none"; //拒绝好友申请提示
        public const string friends_inputId_none = "#1_friends_inputId_none"; //好友搜索cid输入为空提示
        public const string friends_inputIdError_none = "#1_friends_inputIdError_none"; //好友搜索cid输入错误
        public const string friends_send_self_none = "#1_friends_send_self_none";//您无法添加自己为好友
        public const string friends_send_isFriend_none = "#1_friends_send_isFriend_none";//你们已经是好友，不可重复添加
        public const string friends_send_suc_none = "#1_friends_send_suc_none";//好友申请已发送
        public const string friends_delete_suc_none = "#1_friends_delete_suc_none";//删除好友成功
        public const string friends_add_suc_str = "#1_friends_add_suc_str";//添加好友成功
        public const string friends_other_playerInfo_str = "#1_friends_other_playerInfo_str";//其他玩家空间名称{0}(其他玩家名称)
        public const string friends_sure_delete_str = "#1_friends_sure_delete_str";//您确认要删除{0}吗
        public const string friends_lastLoginTime_str = "#1_friends_lastLoginTime_stre";//{0}前登录
        public const string friends_max_day_num = "#1_friends_max_day_num";//已超过{0}天未登录
        public const string friends_refuseContent_none = "#1_friends_refuseContent_none";//一键拒绝所有好友申请内容
        public const string friends_request_count_num_num = "#1_friends_request_count_num_num";//申请人数{0}/{1}  
        public const string friends_countIsMax_none = "#1_friends_countIsMax_none";//好友数量已达上限
        public const string friends_sendRequestIsInMargin_num = "#1_friends_sendRequestIsInMargin_num";//{0}时间内不能重复发送好友申请
        public const string friends_visit_get_reward_none = "#1_friends_visit_get_reward_none";//好友拜访奖励领取提示

        #endregion

        #region 日常任务

        public const string dailyQuest_activeScoreRewardUnlockDesc_score = "#1_dailyQuest_activeScoreRewardUnlockDesc_score";//解锁条件：{0}日常任务积分
        public const string dailyQuest_activeRewardAlreadyGet_none = "#1_dailyQuest_activeRewardAlreadyGet_none";//当前宝箱已被领取
        public const string dailyQuest_currentActiveScore_score = "#1_dailyQuest_currentActiveScore_score";//当前积分：{0}
        public const string dailyQuest_leftResetTime_time = "#1_dailyQuest_leftResetTime_time";//重置时间：{0}
        public const string dailyQuest_rewardPreview_num = "#1_dailyQuest_rewardPreview_num";//奖励预览标题，带一个积分参数

        #endregion

        #region 政务

        public const string anecdote_eventChoiceNoSelect_none = "#1_anecdote_eventChoiceNoSelect_none"; // 没有选中选项
        public const string anecdote_earnings_num = "#1_anecdote_earnings_num";
        public const string anecdote_earnings_goal_num = "#1_anecdote_earnings_goal_num";
        public const string anecdote_cantFindHeroRewardEvent_none = "#1_anecdote_cantFindHeroRewardEvent_none"; // 找不到大臣奖励事件

        #endregion

        #region 游历

        public const string travel_mess_value_num_num = "#1_travel_mess_value_num_num";//情报值：{0}/{1}
        public const string travel_intimacy_num = "#1_travel_intimacy_num";//亲密度：{0}
        public const string travel_mess_count_num = "#1_travel_mess_count_num";//情报数量：{0}
        public const string travel_mess_exchange_need_num = "#1_travel_mess_exchange_need_num";//当前还需{0}点情报值
        public const string travel_mess_item_title = "#1_travel_mess_item_title_str";//{0}的情报
        public const string travel_result_consort_item = "#1_travel_result_consort_item_str";//遇见{0}
        public const string travel_add_mess_point_num = "#1_travel_add_mess_point_num";// 情报值+{0}
        public const string travel_mess_value_chg_tip = "#1_travel_mess_value_chg_tip_num";// 增加{0}点情报值
        public const string travel_akey_travel_count = "#1_travel_akey_travel_count_num";// 游历次数：{0}
        public const string travel_akey_travel_add_mess_point = "#1_travel_akey_travel_add_mess_point_num";// 情报值+{0}
        public const string travel_consort_event_desc = "#1_travel_consort_event_desc_str";// 在{0}遇到了熟悉的身影
        public const string travel_addPowerEventNotSelectHeroTip = "#1_travel_addPowerEventNotSelectHeroTip_none";// 请选择大臣
        public const string travel_clickMainCityPosTip_none = "#1_travel_clickMainCityPosTip_none";//已准备就绪，点击出游即可进行游历
        public const string travel_invitationEventSimpleResult_str = "#1_travel_invitationEventSimpleResult_str";// 邀约成功，下次约会必定遇见{0}
        public const string travel_heroAddPowerTip_str_num = "#1_travel_heroAddPowerTip_str_num";// {0}伙伴实力+{1}
        public const string travel_gambleEventAnteTip_num = "#1_travel_gambleEventAnteTip_num";// 成功押注{0}钻
        public const string travel_gambleEventWaivePopWndTip = "#1_travel_gambleEventWaivePopWndTip_none";//是否放弃本次押注
        public const string travel_gamble_result_win_gain_num = "#1_travel_gamble_result_gain_num";//gain: +{0}
        public const string travel_gamble_result_lose_loss_num = "#1_travel_gamble_result_loss_num";//loss: -{0}
        public const string travel_gamble_result_jakpot_gain_num = "#1_travel_gamble_result_jakpot_gain_num";//jakpot: +{0}


        #endregion

        #region 通用事件

        public const string commonEvent_dispatchEventSelectHeroMax_none = "#1_commonEvent_dispatchEventSelectHeroMax_none";//派遣大臣数量已达上限
        public const string commonEvent_dispatchEventNotAllCondEnableTip_none = "#1_commonEvent_dispatchEventNotAllCondEnableTip_none";//未满足所有条件，是否依旧执行
        public const string commonEvent_dispatchEventAkeyFunctionLock_none = "#1_commonEvent_dispatchEventAkeyFunctionLock_none";// 一键派遣功能未解锁
        public const string commonEvent_dispatchEventSelectHeroTip_none = "#1_commonEvent_dispatchEventSelectHeroTip_none"; // 已经点开大臣列表，再次点击+号，要求玩家选择大臣的提示

        #endregion

        #region 玩家信息

        public const string playerinfo_unUseMsg_des = "#1_playerinfo_unUseMsg_none"; //卸下二次确认弹窗文本
        public const string playerinfo_renameSame_str = "#1_playerinfo_renameSame_str"; //名字没有改动提示
        public const string playerinfo_renameEmpty_str = "#1_playerinfo_renameEmpty_str";//名字为空提示
        public const string playerInfo_renameSuc_str = "#1_playerInfo_renameSuc_str"; //改名成功提示
        public const string playerInfo_setnameSuc_str = "#1_playerInfo_setSuc_str"; //取名成功提示
        public const string playerinfo_renameRange_num_num = "#1_playerinfo_renameRange_num_num";//不在命名区间提示 {0} {1}
        public const string playerinfo_vip_num = "#1_playerinfo_vip_num";//vip{0}
        public const string playerinfo_noalliancename_des = "#2_playerinfo_noalliancename_des"; //没有联盟显示的文本
        public const string playerinfo_renameIllegal_none = "#1_playerinfo_renameIllegal_none";//名字中包含屏蔽的提示
        public const string playerInfo_copySuc_str = "#1_playerInfo_copySuc_str";//复制成功
        public const string playerInfo_playerNoChange_none = "#1_playerInfo_playerNoChange_none"; //形象没有变化提示
        public const string playerInfo_chgSkinColorSuc_none = "#1_playerInfo_chgSkinColorSuc_none";//修改肤色成功
        public const string playerInfo_chgGenderSuc_none = "#1_playerInfo_chgGenderSuc_none";//修改玩家性别成功
        public const string playerInfo_allIntimacy_num = "#1_playerInfo_allIntimacy_num";//总亲密度文本显示
        public const string playerInfo_allNationPower_num = "#1_playerInfo_allNationPower_num";//总国力文本显示  {0}/秒
        public const string playerInfo_allFight_num = "#1_playerInfo_allFight_num";//总战力文本显示
        public const string playerInfo_expProgress_num_num = "#1_playerInfo_expProgress_num_num";//{0}/{1}
        public const string playerInfo_normalIconTabName_none = "#1_playerInfo_normalIconTabName_none";//默认
        public const string playerInfo_consortIconTabName_none = "#1_playerInfo_consortIconTabName_none";//恋人
        public const string playerInfo_heroIconTabName_none = "#1_playerInfo_heroIconTabName_none";//骑士
        public const string playerInfo_allIconTabName_none = "#1_playerInfo_allIconTabName_none";//所有

        public const string playerInfo_serverName_str = "#1_playerInfo_serverName_str";//服务器:{0}
        
        public const string playerInfo_offlineGoldTime_num = "#1_playerInfo_offlineGoldTime_num";//离线时间: {0}
        public const string playerInfo_offlineGoldMaxTime_num = "#1_playerInfo_offlineGoldMaxTime_num";//离线收益上限: {0}
        
        public static string playerInfo_dailyReward_none = "#1_playerInfo_dailyReward_none";//每日奖励
        public static string playerInfo_childEducateHeroExp_none = "#1_playerInfo_childEducateHeroExp_none";//子嗣上课获得大臣经验
        public static string main_earningsShow_num = "#1_main_earningsShow_num";//{0}

        public const string playerInfo_comboTitlePrefix_none = "#1_playerInfo_comboTitlePrefix_none";//称号前缀
        public const string playerInfo_comboTitleSuffix_none = "#1_playerInfo_comboTitleSuffix_none";//称号后缀
        public const string playerInfo_comboTitleBg_none = "#1_playerInfo_comboTitleBg_none";//称号底图
        public const string playerInfo_gainTime_str = "#1_playerInfo_gainTime_str";//获得时间：{0}
        public const string playerInfo_gainTitlePrefix_str = "#1_playerInfo_gainTitlePrefix_str";//获得前缀称号：{0}
        public const string playerInfo_gainTitleSuffix_str = "#1_playerInfo_gainTitleSuffix_str";//获得后缀称号：{0}
        public const string playerInfo_gainTitleBg_str = "#1_playerInfo_gainTitleBg_str";//获得称号底框：{0}
        public const string playerInfo_normalIconTitle_none = "#1_playerInfo_normalIconTitle_none";//普通
        public const string playerInfo_consortIconTitle_none = "#1_playerInfo_consortIconTitle_none";//家人
        public const string playerInfo_heroIconTitle_none = "#1_playerInfo_heroIconTitle_none";//伙伴
        public const string playerInfo_icon_none = "#1_playerInfo_icon_none";//头像
        public const string playerInfo_iconBgk_none = "#1_playerInfo_iconBgk_none";//头像框
        public const string playerInfo_bubble_none = "#1_playerInfo_bubble_none";//气泡框
        public const string playerInfo_serverNamePlayerName_str_str = "#1_playerInfo_serverNamePlayerName_str_str";//[{0}]{1}       

        #endregion

        #region 成就

        public const string achieve_rewardPreview_num = "#1_achieve_rewardPreview_num";//成就预览标题，带一个积分参数
        public const string achieve_canGetRewardProcess_num_num = "#1_achieve_canGetRewardProcess_num_num";//（可领取时进度富文本）{0}/{1}
        public const string achieve_canNotGetRewardProcess_num_num = "#1_achieve_canNotGetRewardProcess_num_num";//（不可领取时进度富文本）{0}/{1}

        #endregion

        #region 骑士推荐

        public const string heroRecommend_selectHeroBtnDesc_name = "#1_heroRecommend_selectHeroBtnDesc_name";//选择骑士：{0}
        public const string heroRecommend_noCountTip_none = "#1_heroRecommend_noCountTip_none";//当前没有推荐次数

        #endregion

        #region 新手流程

        public const string create_need_to_make_face = "#1_create_need_to_make_face";// 创角的是否去捏脸提示
        public const string create_game_start = "#1_create_game_start";// 直接开始游戏
        public const string create_make_face = "#1_create_make_face";// 捏脸


        #endregion

        #region 跑马灯

        public const string marquee_deleteAllConfirmDesc_none = "#1_marquee_deleteAllConfirmDesc_none";//是否删除所有跑马灯信息？

        #endregion

        #region 通用获得物品奖励

        public const string getItem_iconTypeName_none = "#1_getItem_iconTypeName_none";//头像
        public const string getItem_iconBgkTypeName_none = "#1_getItem_iconBgkTypeName_none";//头像框
        public const string getItem_bubbleTypeName_none = "#1_getItem_bubbleTypeName_none";//气泡框
        public const string getItem_titleTypeName_none = "#1_getItem_titleTypeName_none";//称号
        public const string getItem_expiredTime_time = "#1_getItem_expiredTime_time";//有效期：{0}

        #endregion

        #region 周卡

        public const string week_card_type_name = "#1_week_card_type_name_{0}"; //周卡委派名字,参数替换为EWeekCardSettleType的枚举子字符串，替换后座位语言key
        public const string week_card_type_desc = "#1_week_card_type_desc_{0}"; //周卡委派描述,参数替换为EWeekCardSettleType的枚举子字符串，替换后座位语言key
        public const string week_card_college_hero_count = "#1_week_card_college_hero_count"; //{0}/{1}
        public const string week_card_selected_hero_max = "#1_week_card_selected_hero_max"; //选择的骑士数量已达上限
        public const string week_card_settle_LEVY_SILVER = "#1_week_card_settle_LEVY_SILVER"; //共计征收金币{0}次
        public const string week_card_settle_LEVY_SOLIDER = "#1_week_card_settle_LEVY_SOLIDER"; //共计征收面包{0}次
        public const string week_card_settle_ANECDOTE = "#1_week_card_settle_ANECDOTE"; //共计处理趣闻{0}次
        public const string week_card_settle_CONSORT_RND_CALL = "#1_week_card_settle_CONSORT_RND_CALL"; //共计倾诉知己{0}次
        public const string week_card_settle_CONSORT_RND_CALL_skill_point = "#1_week_card_settle_CONSORT_RND_CALL_skill_point"; //{0}势力值+{1}
        public const string week_card_settle_CONSORT_RND_CALL_child = "#1_week_card_settle_CONSORT_RND_CALL_child"; //触发遇见学徒{0}次
        public const string week_card_settle_TRAVEL = "#1_week_card_settle_TRAVEL"; //共计游历{0}次
        public const string week_card_settle_TRAVEL_intimacy = "#1_week_card_settle_TRAVEL_intimacy"; //{0}亲密度+{1}
        public const string week_card_settle_TRAVEL_like = "#1_week_card_settle_TRAVEL_like"; //{0}好感度+{1}
        public const string week_card_settle_CHILD_TRAIN = "#1_week_card_settle_CHILD_TRAIN"; //共计培养学徒{0}次
        public const string week_card_settle_COLLEGE_STUDY = "#1_week_card_settle_COLLEGE_STUDY"; //共计骑士学习{0}次
        public const string week_card_hero_no_selected_none = "#1_week_card_hero_no_selected_none"; //更换形象没有选中一个形象
        public const string week_card_mainPay_tip = "#1_week_card_mainPay_tip"; //{0}后失效
        public const string week_card_payCardTip_title = "#1_week_card_payCardTip_title"; //友情提示
        public const string week_card_payCardTip_des = "#1_week_card_payCardTip_des"; //周卡已激活，已开启委派功能，是否理解前往设置？
        public const string week_card_chg_tdShow_succ = "#1_week_card_chg_tdShow_succ"; //更换成功
        public const string week_card_selected_hero_count = "#1_week_card_selected_hero_count_num"; //进修人数：{0}/{1}

        #endregion

        #region 集市

        public const string market_canGetCurCount_num_num = "#1_market_canGetCurCount_num_num"; //可领取时 : 集市店铺当前可领取次数{0}_{1}
        public const string market_noGetCurCount_num_num = "#1_market_noGetCurCount_num_num"; //不可领取时 : 集市店铺当前可领取次数{0}_{1}
        public const string market_countDownTime_num = "#1_market_countDownTime_num"; //重置倒计时:{0}

        public const string market_rewardCritPer_num = "#1_market_rewardCritPer_num"; //奖励翻倍概率: {0}%
        public const string market_rewardCritPerAfter_num = "#1_market_rewardCritPerAfter_num"; //{0}%
        public const string market_rewardCritCount_num = "#1_market_rewardCritCount_num"; //翻倍倍数: {0}
        public const string market_rewardCritCountAfter_num = "#1_market_rewardCritCountAfter_num"; //{0}

        public const string market_bigCrit_none = "#1_market_bigCrit_none"; //天赐良机时，你将有机会触发N倍奖励
        public const string market_recoverTime_num = "#1_market_recoverTime_num"; //忙碌中: {0}
        public const string market_inCD_none = "#1_market_inCD_none"; // 冷却cd中
        public const string market_shopLvUpSuc_none = "#1_market_shopLvUpSuc_none"; // 店铺升级成功
        public const string market_shopIsLock_str = "#1_market_shopIsLock_str"; // {0}店铺未开张，无法获得奖励
        public const string market_noShopHasCount_none = "#1_market_noShopHasCount_none"; // 当前没有可直接经营的店铺
        public const string market_shopCostItemGetRewardTipContent_none = "#1_market_shopCostItemGetRewardTipContent_none"; //(集市店铺消耗道具领取奖励提示内容)
        public const string market_shopMaxLevelMultipleDesc_num = "#1_market_shopMaxLevelMultipleDesc_num"; //当前满级翻倍倍数: {0}
        public const string market_shopMaxLevelProbabilityDesc_num = "#1_market_shopMaxLevelProbabilityDesc_num"; //当前满级翻倍概率: {0}%


        #endregion

        #region CDN

        public const string cdn_forceUpdateClientHotfixTip_none = "#1_cdn_forceUpdateClientHotfixTip_none";//发现游戏更新，请重启游戏
        public const string cdn_updateImImmediately_none = "#1_cdn_updateImImmediately_none";//立即更新
        public const string cdn_promptUpdateClientHotfixTip_none = "#1_cdn_promptUpdateClientHotfixTip_none";//发现游戏更新，是否立即重启
        public const string cdn_promptUpdateHotfixCancel_none = "#1_cdn_promptUpdateHotfixCancel_none";//稍后再说

        #endregion

        #region 运营公告

        public const string announcement_contactCustomerServiceTip_none = "#1_announcement_contactCustomerServiceTip_none";//联系客服了解更多!

        #endregion

        #region 问卷调查

        public const string questionnaire_questionnaireIsEnd_none = "#1_questionnaire_questionnaireIsEnd_none";//问卷调查已结束

        #endregion

        #region avatar评分

        public const string avatarScore_stepRewardPreviewTitle_score = "#1_avatarScore_stepRewardPreviewTitle_score";//总星数达到{0}可获得该阶段奖励
        public const string avatarScore_finishEventCondition_none = "#1_avatarScore_finishEventCondition_none";//需要穿戴更多单品达到第一目标阶段，可完成事件
        public const string avatarScore_unitCanNotChange_none = "#1_avatarScore_unitCanNotChange_none";//与事件穿着冲突，不可替换

        #endregion

        #region 收集点赞

        public const string collect_likes_count_max = "#1_collect_likes_count_max_none"; //今日点赞次数已达上限
        public const string collect_likes_has_likeed = "#1_collect_likes_has_likeed_none"; //今天已经给她点过赞了噢
        public const string collect_likes_share_has_likeed = "#1_collect_likes_share_has_likeed_none"; //已点赞过该分享

        #endregion

        #region 联盟

        public const string guild_inputNameUnderLimit_none = "#1_guild_inputNameUnderLimit_none"; //名称过短
        public const string guild_inputNameOverLimit_none = "#1_guild_inputNameOverLimit_none"; //名称过长
        public const string guild_inputNameIllegal_none = "#1_guild_inputNameIllegal_none"; //名称包含特殊字符
        public const string guild_inputAbbreviationUnderLimit_none = "#1_guild_inputAbbreviationUnderLimit_none"; //简称过短
        public const string guild_inputAbbreviationOverLimit_none = "#1_guild_inputAbbreviationOverLimit_none"; //简称过长
        public const string guild_inputAbbreviationIllegal_none = "#1_guild_inputAbbreviationIllegal_none"; //简称包含特殊字符
        public const string guild_inputDeclarationUnderLimit_none = "#1_guild_inputDeclarationUnderLimit_none"; //宣言过短
        public const string guild_inputDeclarationOverLimit_none = "#1_guild_inputDeclarationOverLimit_none"; //宣言过长
        public const string guild_inputDeclarationIllegal_none = "#1_guild_inputDeclarationIllegal_none"; //宣言包含特殊字符
        public const string guild_inputAnnouncementUnderLimit_none = "#1_guild_inputAnnouncementUnderLimit_none"; //公告过短
        public const string guild_inputAnnouncementOverLimit_none = "#1_guild_inputAnnouncementOverLimit_none"; //公告过长
        public const string guild_applymentCding_tip = "#1_guild_applymentCding_tip"; //{0}后，才可再次加入联盟
        public const string guild_showName_simpleName_name = "#1_guild_showName_simpleName_name"; //({0}){1}
        public const string guild_guildLevel_level = "#1_guild_guildLevel_level"; //{0}级联盟
        public const string guild_sevenDayContribute_num = "#1_guild_sevenDayContribute_num"; //七日贡献度：{0}
        public const string guild_guildContribute_num = "#1_guild_guildContribute_num"; //联盟贡献度：{0}
        public const string guild_dontHavePermission_none = "#1_guild_dontHavePermission_none"; //您没有权限
        public const string guild_broadcastMessageMemberCount_count_count = "#1_guild_broadcastMessageMemberCount_count_count"; //通知的成员：{0}/{1}
        public const string guild_messageIsEmpty_none = "#1_guild_messageIsEmpty_none"; //内容为空
        public const string guild_messageUnderLimit_none = "#1_guild_messageUnderLimit_none"; //内容过短
        public const string guild_messageOverLimit_none = "#1_guild_messageOverLimit_none"; //内容过长
        public const string guild_messageIllegal_none = "#1_guild_messageIllegal_none"; //内容包含特殊字符
        public const string guild_todayNoticeLeftCount_count = "#1_guild_todayNoticeLeftCount_count"; //今日剩余次数：{0}
        public const string guild_noticeNoSelectMember_none = "#1_guild_noticeNoSelectMember_none"; //未选择成员
        public const string guild_quitGuildConfirm_none = "#1_guild_quitGuildConfirm_none"; //是否要退出联盟？
        public const string guild_leaderName_name = "#1_guild_leaderName_name"; //盟主：{0}
        public const string guild_chgInfoSuc_none = "#1_guild_chgInfoSuc_none"; //修改成功
        public const string guild_canNotDismissGuild_none = "#1_guild_canNotDismissGuild_none"; //当前无法遣散联盟，请领主大人先踢出其他联盟成员。
        public const string guild_dismissGuildConfirmDesc_none = "#1_guild_dismissGuildConfirmDesc_none"; //(遣散二次确认弹窗描述)
        public const string guild_joinLimit_freeJoin_none = "#1_guild_joinLimit_freeJoin_none"; //无需审批加入
        public const string guild_joinLimit_approvalJoin_none = "#1_guild_joinLimit_approvalJoin_none"; //允许审批加入
        public const string guild_joinLimit_declineJoin_none = "#1_guild_joinLimit_declineJoin_none"; //拒绝任何人加入
        public const string guild_guildMemberCountReachedLimit_none = "#1_guild_guildMemberCountReachedLimit_none"; //联盟人数达到上限
        public const string guild_noJoinRequest_none = "#1_guild_noJoinRequest_none"; //没有入盟申请
        public const string guild_oneKeyRefuseAllRequest_none = "#1_guild_oneKeyRefuseAllRequest_none"; //已一键拒绝所有申请
        public const string guild_oneKeyAgreeAllRequest_none = "#1_guild_oneKeyAgreeAllRequest_none"; //已一键同意所有申请
        public const string guild_oneKeyAgreeMemberLimit_none = "#1_guild_oneKeyAgreeMemberLimit_none"; //联盟人数达到上限
        public const string guild_currentPosition_name = "#1_guild_currentPosition_name"; //现职位：{0}
        public const string guild_positionNameWithCount_name_curCount_totalCount = "#1_guild_positionNameWithCount_name_curCount_totalCount"; //{0}({1}/{2})
        public const string guild_appointSucceed_none = "#1_guild_appointSucceed_none"; //任命成功
        public const string guild_transferLeaderDesc_none = "#1_guild_transferLeaderDesc_none"; //（转让盟主二次确认弹窗描述）
        public const string guild_transferLeaderSucceed_none = "#1_guild_transferLeaderSucceed_none"; //转让成功
        public const string guild_kickOutDesc_none = "#1_guild_kickOutDesc_none"; //（踢出成员二次确认弹窗描述）
        public const string guild_kickOutSucceed_none = "#1_guild_kickOutSucceed_none"; //踢出成功
        public const string guild_alreadyInAGuildTip_none = "#1_guild_alreadyInAGuildTip_none";//已经在一个联盟中 
        public const string guild_applyingJoinTip_none = "#1_guild_applyingJoinTip_none";//已申请加入联盟 
        public const string guild_guildDeclineJoinTip_none = "#1_guild_guildDeclineJoinTip_none";//联盟拒绝任何人加入 
        public const string guild_reqJoinGuildMemberFullTip_none = "#1_guild_reqJoinGuildMemberFullTip_none";//该联盟人数已达到上限
        public const string guild_joinGuildInCDTip_str1 = "#1_guild_joinGuildInCDTip_str1";//还剩{0}可加入联盟
        public const string guild_reqJoinGuildNumMax_none = "#1_guild_reqJoinGuildNumMax_none";//您申请加入的联盟数量已达到上限
        public const string guild_reqJoinGuildSuccTip_none = "#1_guild_reqJoinGuildSuccTip_none";//加入联盟成功
        public const string guild_reqApplyJoinGuildSuccTip_none = "#1_guild_guild_reqApplyJoinGuildSuccTip_none";//申请加入联盟成功提示
        public const string guild_cancelApplyJoinGuildTip_none = "#1_guild_cancelApplyJoinGuildTip_none";//取消申请加入联盟提示
        public const string guild_contributeNotEnough_amount = "#1_guild_contributeNotEnough_amount";//贡献度不足，需要{0}贡献度
        public const string guild_applymentCondition_desc2 = "#1_guild_applymentCondition_desc2";//未设置申请条件，需要审批加入
        public const string guild_applymentCondition_desc3 = "#1_guild_applymentCondition_desc3";//未设置申请条件，无需审批加入
        public const string guild_applymentCondition_desc4 = "#1_guild_applymentCondition_desc4";//该联盟拒绝任何人加入
        public const string guild_leaveGuildTip_none = "#1_guild_leaveGuildTip_none";//（被移除出联盟的二次确认提示）
        public const string guild_createGuildSucceed_none = "#1_guild_createGuildSucceed_none";//创建联盟成功
        public const string guild_todayGoldConstructCount_num_num = "#1_guild_todayGoldConstructCount_num_num";//今日联盟金币建设总次数：{0}/{1}
        public const string guild_todayItemConstructCount_num_num = "#1_guild_todayItemConstructCount_num_num";//今日联盟道具建设总次数：{0}/{1}
        public const string guild_constructSucceed_none = "#1_guild_constructSucceed_none";//捐献成功
        public const string guild_inImpeachCD_cdTime = "#1_guild_inImpeachCD_cdTime";//弹劾中\n{0}
        public const string guild_impeachConfirmTip_num = "#1_guild_impeachConfirmTip_num";//盟主已超过{0}小时未上线，是否弹劾盟主？
        public const string guild_impeachSucceed_none = "#1_guild_impeachSucceed_none";//弹劾成功
        public const string guild_leaderImpeachTip_none = "#1_guild_leaderImpeachTip_none";//您多日未上线，联盟疏于管理，联盟成员有所不满，是否继续兼任盟主职位？
        public const string guild_resignationPosition_none = "#1_guild_resignationPosition_none";//卸任职位
        public const string guild_serveAsLeader_none = "#1_guild_serveAsLeader_none";//担任盟主
        public const string guild_recruitMiniChatDesc_str1 = "#1_guild_recruitMiniChatDesc_str1";//诚招盟友！壮大联盟{0}需要您的加入
        public const string guild_recruitChatDesc_str1 = "#1_guild_recruitChatDesc_str1";//诚招盟友！壮大联盟{0}需要您的加入
        public const string guild_dissolveGuildSuc_none = "#1_guild_dissolveGuildSuc_none";//联盟已解散
        public const string guild_nameNotChg_none = "#1_guild_nameNotChg_none";//未修改
        public const string guild_appointConditionDesc_num_num = "#1_guild_appointConditionDesc_num_num";//任命条件:只能任命最近三天内上线的联盟成员。精英≥{0}贡献度,副盟主≥{1}贡献度。
        public const string guild_memberIsNotDeputyLeader_none = "#1_guild_memberIsNotDeputyLeader_none";//该成员不是副盟主
        public const string guild_alreadyImpeach_none = "#1_guild_alreadyImpeach_none";//已弹劾 
        public const string guild_positionMemberReachLimit_none = "#1_guild_positionMemberReachLimit_none";//联盟职位人数已达上限
        public const string guild_logShowMaxCountDesc_num = "#1_guild_logShowMaxCountDesc_num";//最多保留{0}条
        public const string guild_logContent_time_string = "#1_guild_logContent_time_string";//{0}    {1}
        public const string guild_autoMarsHelpLeftTime_str = "#1_guild_autoMarsHelpLeftTime_str";//剩余时间：{0}
        public const string guild_todayAutoMarsHelpCount_num = "#1_guild_todayAutoMarsHelpCount_num";//今日已自动帮助：{0}次
        public const string guild_autoMarsHelpTipDesc_none = "#1_guild_autoMarsHelpTipDesc_none";//(自动帮助特权描述tip)
        public const string guild_entrustMultipleTip_num = "#1_guild_entrustMultipleTip_num";//暴击！x {0}

        #endregion

        #region 藏品

        public const string equip_greedAndBelow_none = "#1_equip_greedAndBelow_none"; //绿色及以下
        public const string equip_blueAndBelow_none = "#1_equip_blueAndBelow_none"; //蓝色及以下
        public const string equip_purpleAndBelow_none = "#1_equip_purpleAndBelow_none"; //紫色及以下
        public const string equip_orangeAndBelow_none = "#1_equip_orangeAndBelow_none"; //橙色及以下
        public const string equip_equipByHero_name = "#1_equip_equipByHero_name"; //佩戴者：{0}
        public const string equip_goToHeroDesc_none = "#1_equip_goToHeroDesc_none"; //当前无人佩戴，是否要前往伙伴界面进行操作？
        public const string equip_talentValue_num = "#1_equip_talentValue_num"; //资质{0}
        public const string equip_skillAttrName_str = "#1_equip_skillAttrName_str"; //{0}相性
        public const string equip_skillRebuildPropProbability_num_num = "#1_equip_skillRebuildPropProbability_num_num"; //{0}%-{1}%
        public const string equip_pendingIsBetterDesc_none = "#1_equip_pendingIsBetterDesc_none"; //重铸产生了更好的加成属性，是否将其设置为新属性？
        public const string equip_noHeroSkillRebuildDesc_none = "#1_equip_noHeroSkillRebuildDesc_none";//这个藏品还没有被装备，你确定要重塑它吗？
        public const string equip_curSkillIsBetterDesc_none = "#1_equip_curSkillIsBetterDesc_none";//你当前的技能效果更好，确认替换它吗？
        public const string equip_levelName_num_str = "#1_equip_levelName_num_str";//Lv.{0} {1}
        public const string equip_replaceHeroDesc_str_str_str = "#1_equip_replaceHeroDesc_str_str_str";//此藏品已被{0}装备，是否从{0}身上卸下此藏品，改为装备给{1}
        public const string equip_equipLevel_num = "#1_equip_equipLevel_num";//等级 {0}
        public const string equip_initTalentValue_num = "#1_equip_initTalentValue_num";//资质：{0}
        public const string equip_initSkillCount_num = "#1_equip_initSkillCount_num";//初始技能数：{0}
        public const string equip_lockCanNotRecycle_none = "#1_equip_lockCanNotRecycle_none";//藏品锁定中无法分解
        public const string equip_wearCanNotRecycle_none = "#1_equip_wearCanNotRecycle_none";//藏品穿戴中无法分解

        #endregion

        #region 招募

        public const string recruit_exchangeSecondCheckWndTitle_none = "#1_recruit_exchangeSecondCheckPopWndTitle_none";//招募 - 兑换二次确认弹窗标题
        public const string recruit_exchangeSecondCheckWndContent_none = "#1_recruit_exchangeSecondCheckWndContent_none";//招募 - 兑换二次确认弹窗内容

        #endregion

        #region 召唤

        public const string summon_publicRollRecordDesc_str2_num1 = "#1_summon_publicRollRecordDesc_str2";//恭喜{0}获得了{1}*{2}

        #endregion
		
        #region 竞技场

        public const string arena_restoreSelectCount_num_num = "#1_arena_restoreSelectCount_num_num";//数量：{0}/{1}
        public const string arena_restoreAlreadyBuyCount_num_num = "#1_arena_restoreAlreadyBuyCount_num_num";//（今日已购买{0}/{1}次）
        public const string arena_stationResIsEmpty_none = "#1_arena_stationResIsEmpty_none";//贸易站内空空如也
        public const string arena_stationHarvestRatio_num = "#1_arena_stationHarvestRatio_num";//贸易站收成比例：{0}倍
        public const string arena_stationRemoveLimitTimeDesc_none = "#1_arena_stationRemoveLimitTimeDesc_none";//年卡特权用户将移除存储上限
        public const string arena_stationUpgradeNeedGetResFirst_none = "#1_arena_stationUpgradeNeedGetResFirst_none";//贸易站存在资源，请先领取再进行升级
        public const string arena_buyRandomAttackCountReachLimit_none = "#1_arena_buyRandomAttackCountReachLimit_none";//今日购买次数已达上限
        public const string arena_buyRandomAttackCountSuc_none = "#1_arena_buyRandomAttackCountSuc_none";//谈判次数购买成功
        public const string arena_stationLevel_num = "#1_arena_stationLevel_num";//贸易站 Lv.{0}
        public const string arena_randomAttackCount_num_num = "#1_arena_randomAttackCount_num_num";//剩余次数：{0}/{1}
        public const string arena_defeatMyHeroCount_num = "#1_arena_defeatMyHeroCount_num";//击败我方{0}名伙伴
        public const string arena_myInfluenceChg_num = "#1_arena_myInfluenceChg_num";//我方商会影响力：{0}
        public const string arena_reportPassTime_str = "#1_arena_reportPassTime_str";//{0}前
        public const string arena_selectAttackCountReachLimit_none = "#1_arena_selectAttackCountReachLimit_none";//指定谈判已达上限
        public const string arena_celebrityDefeatHeroDesc_str_str_num = "#1_arena_celebrityDefeatHeroDesc_str_str_num";//{0}击败{1}的{2}名伙伴
        public const string arena_getOnTheCelebrityListCondition_num = "#1_arena_getOnTheCelebrityListCondition_num";//上榜条件：击败{0}名以上伙伴
        public const string arena_lastSelectCelebrityName_str = "#1_arena_lastSelectCelebrityName_str";//上次谈判：{0}
        public const string arena_lastSelectCelebrityNone_none = "#1_arena_lastSelectCelebrityNone_none";//上次谈判：暂无
        public const string arena_selfRank_num = "#1_arena_selfRank_num";//我的排名：{0}
        public const string arena_selfInfluence_num = "#1_arena_selfInfluence_num";//我的商会影响力：{0}
        public const string arena_influence_num = "#1_arena_influence_num";//商会影响力：{0}
        public const string arena_battleOpponentHeroCount_num_num = "#1_arena_battleOpponentHeroCount_num_num";//伙伴数量 {0}/{1}
        public const string arena_startBattleNeedSelectHero_none = "#1_arena_startBattleNeedSelectHero_none";//请选择伙伴
        public const string arena_battleHeroPower_num = "#1_arena_battleHeroPower_num";//实力：{0}
        public const string arena_battleHeroTemporaryAddPowerPer_num = "#1_arena_battleHeroTemporaryAddPowerPer_num";//当前临时实力加成：{0}%
        public const string arena_battleBuyBuffSuc_none = "#1_arena_battleBuyBuffSuc_none";//临时增益购买成功
        public const string arena_battleHadBuyBuffTip_none = "#1_arena_battleHadBuyBuffTip_none";//本回合已购买临时增益，去跟对手谈判吧
        public const string arena_battleRoundLostDesc_str = "#1_arena_battleRoundLostDesc_str";//很遗憾，您的伙伴【{0}】血量为0，无法继续谈判！
        public const string arena_battleRoundWinNoReachRewardDesc_num_num = "#1_arena_battleRoundWinNoReachRewardDesc_num_num";//恭喜您获得{0}连胜，再连胜{1}场可获得连胜奖励
        public const string arena_battleRoundWinReachRewardDesc_num = "#1_arena_battleRoundWinReachRewardDesc_num";//恭喜您获得{0}连胜，可获得连胜奖励
        public const string arena_battleFinalResultTitle_none = "#1_arena_battleFinalResultTitle_none";//结算详情
        public const string arena_battleFinalResultAllDefeatTitle_none = "#1_arena_battleFinalResultAllDefeatTitle_none";//大获全胜
        public const string arena_battleFinalResultWinDesc_str_str_num = "#1_arena_battleFinalResultWinDesc_str_str_num";//恭喜，您的伙伴【{0}】击败了【{1}】的{2}位伙伴
        public const string arena_battleFinalResultLostDesc_str_str = "#1_arena_battleFinalResultLostDesc_str_str";//很遗憾，您的伙伴【{0}】未能击败【{1}】的伙伴
        public const string arena_battleFinishAddHeroPowerDesc_num_num_num = "#1_arena_battleFinishAddHeroPowerDesc_num_num_num";//实力提升={0}(身份效果)X{1}(击败伙伴数)X{2}(指名谈判)
        public const string arena_stationOutputSpeed_num = "#1_arena_stationOutputSpeed_num";//{0}/秒
        public const string arena_curBlood_num_num = "#1_arena_curBlood_num_num";//当前血量：{0}/{1}
        public const string arena_needChooseInitBuffTip_none = "#1_arena_needChooseInitBuffTip_none";//请先选择初始增益
        public const string arena_rankNow_num = "#1_arena_rankNow_num";//排名：{0}
        public const string arena_randomTypeBattle_none = "#1_arena_randomTypeBattle_none";//随机谈判
        public const string arena_selectTypeBattle_none = "#1_arena_selectTypeBattle_none";//指定谈判

        #endregion

        #region 阶段奖励
        public const string stage_goal_process_percent = "#1_stage_goal_process_percent_num_num"; //阶段奖励进度显示key {0}/{1}
        public const string stage_goal_taskNum_num = "#1_stage_goal_taskNum_num"; //第{0}阶段
        public const string stage_goal_taskNumAndName_num_name = "#1_stage_goal_taskNumAndName_num_name"; //第{0}阶段：{1}
        public const string stage_goal_taskGetReward_num_num = "#1_stage_goal_taskGetReward_num_num"; //完成阶段{0}-{1}
        public const string stage_goal_taskDone_none = "#1_stage_goal_taskDone_none"; //该任务已完成
        public const string stage_goal_subTaskProgressPercent_num = "#1_stage_goal_subTaskProgressPercent_num"; //{0}%
        public const string stage_goal_nextStageGoalUnlockCD_num_num_num = "#1_stage_goal_nextStageGoalUnlockCD_num_num_num"; //距离解锁剩余{0}:{1}:{2}
        public const string stage_goal_nextStegeNeedServerDayDesc_str_num = "#1_stage_goal_nextStegeNeedServerDayDesc_str_num"; //下一阶段{0}需要服务器开放天数达到{1}天后解锁
        #endregion

        #region 限时冲榜

        public const string rankRush_playing_str = "#1_rankRush_playing_str";//进行中：{0}
        public const string rankRush_settling_str = "#1_rankRush_settling_str";//结算中：{0}
        public const string rankRush_gettingReward_str = "#1_rankRush_gettingReward_str";//领奖中：{0}
        public const string rankRush_detailShowPlaying_str = "#1_rankRush_detailShowPlaying_str";//进行中：{0}
        public const string rankRush_detailShowSettling_str = "#1_rankRush_detailShowSettling_str";//结算中：{0}
        public const string rankRush_detailShowGettingReward_str = "#1_rankRush_detailShowGettingReward_str";//领奖中：{0}
        public const string rankRush_myRank_str = "#1_rankRush_myRank_str";//{0}
        public const string rankRush_myScore_str_num = "#1_rankRush_myScore_str_num";//{0}：{1}
        public const string rankRush_notOnTheRankingList_none = "#1_rankRush_notOnTheRankingList_none";//未上榜
        public const string rankRush_buttonReceiveName_none = "#1_rankRush_buttonReceiveName_none";//领取
        public const string rankRush_buttonAlreadyReceiveName_none = "#1_rankRush_buttonAlreadyReceiveName_none";//已领取
        public const string rankRush_rewardRanking_num = "#1_rankRush_rewardRanking_num";//第{0}名
        public const string rankRush_rewardRankingInterval_num_num = "#1_rankRush_rewardRankingInterval_num_num";//第{0}-{1}名
        public const string rankRush_participationAward_none = "#1_rankRush_participationAward_none";//参与奖
        public const string rankRush_activityAlreadyClose_none = "#1_rankRush_activityAlreadyClose_none";//活动已结束
        public const string rankRush_isNotAwardTime_none = "#1_rankRush_isNotAwardTime_none";//未到领奖时间
        public const string rankRush_notSatisfiedGetReward_none = "#1_rankRush_notSatisfiedGetReward_none";//未满足领奖资格
        public const string rankRush_notJoinGuild_none = "#1_rankRush_notJoinGuild_none";//未加入联盟
        public const string rankRush_chatBoxFirstDesc_name_str = "#1_rankRush_chatBoxFirstDesc_name_str";//恭喜{0}在{1}中获得第一名
        public const string rankRush_chatBoxGuildAlreadyDissolve_none = "#1_rankRush_chatBoxGuildAlreadyDissolve_none";//***联盟(已解散）
        public const string rankRush_serverList_str = "#1_rankRush_serverList_str";//参与区服：{0}

        // 阶段任务冲榜活动
        public const string stepTaskRushRank_rewardPoints_num = "#1_stepTaskRushRank_rewardPoints_str";//Reward:{0} Points
        public const string stepTaskRushRank_totalPoints_num = "#1_stepTaskRushRank_totalPoints_str";//Obtained:{0} Points
        public const string stepTaskRushRank_myRank_str = "#1_stepTaskRushRank_myRank_str";//My Rank:{0}

        #endregion

        #region 爬塔 Tower
        
        public const string tower_suc_up_level_num_desc = "#1_tower_suc_up_level_num_desc";//你提升了多少关{0}
        public const string tower_common_total_level_desc = "#1_tower_common_total_level_desc";//{0}F :不是总level而是章节的level
        public const string tower_daily_coin_get_num = "#1_tower_daily_coin_get_num";//+{0}/天
        public const string tower_earn_bonus_add_num = "#1_tower_earn_bonus_add_num";//+{0}%
        public const string tower_challengeEarningAddPer_num = "#1_tower_challengeEarningAddPer_num";//+{0}%
        public const string tower_log_defend_fail_desc = "#1_tower_log_defend_fail_desc";//击败了你的守卫{0}，挑战进度下降{1}层
        public const string tower_log_defend_suc_desc = "#1_tower_log_defend_suc_desc";//没能击败你的守卫{0}，挑战进度不变
        public const string tower_research_title_process_str = "#1_tower_research_title_process_str";//科技研究（+{0}%）
        public const string tower_research_active_need_pre_chapter_tip = "#1_tower_research_active_need_pre_chapter_tip";//请先激活上一层所有科技效果
        public const string tower_suc_research_bonus_up_desc = "#1_tower_suc_research_bonus_up_desc";//科技研究提升可解锁赚速{0}%
        public const string tower_suc_next_research_pos_desc = "#1_tower_suc_next_research_pos_desc";//下一个科技点在{0}
        public const string tower_research_level_name = "#1_tower_research_level_name";//{0}0KM
        public const string tower_research_item_addPropPer_num = "#1_tower_research_item_addPropPer_num";//+{0}%
        public const string tower_research_active_desc = "#1_tower_research_active_desc";//{0}研究，对建筑总收益提升至<color =FFFFFFFF><size=50>{1}%</size>
        public const string tower_research_notactive_desc = "#1_tower_research_notactive_desc";//{0}研究，对建筑总收益提升至<color = FF0000FF><size=50>{1}%</size>

        #endregion

        #region 午间副本 MiddayDungeon
        
        public const string midday_dungeon_select_a_hero_tip = "#1_midday_dungeon_select_a_hero_tip";//选择1个伙伴 提示
        public const string midday_dungeon_no_left_borrow_guid_hero_times = "#1_midday_dungeon_no_left_borrow_guid_hero_times";//无剩余借用次数
        public const string midday_dungeon_mini_box_no_box_desc = "#1_midday_dungeon_mini_box_no_box_desc";//传闻击败魔物后有时会法线宝箱
        public const string midday_dungeon_mini_box_info_desc = "#1_midday_dungeon_mini_box_info_desc";//{0}:这是我再矿区清剿时法线的宝箱
        public const string midday_dungeon_chat_box_mini_content = "#1_midday_dungeon_chat_box_mini_content";// 这是我再矿区清剿时法线的宝箱
        public const string midday_dungeon_no_hero_can_fight_tip = "#1_midday_dungeon_no_hero_can_fight_tip";//没有大臣可处长 提示
        public const string midday_dungeon_activity_no_open_tip = "#1_midday_dungeon_activity_no_open_tip";//活动未开启提示
        public const string midday_dungeon_fight_process_num = "#1_midday_dungeon_fight_process_num";//进度：{0}%
        public const string midday_dungeon_result_point_num = "#1_midday_dungeon_result_point_num"; //积分：{0}
        public const string midday_dungeon_boss_has_clear_tip = "#1_midday_dungeon_boss_has_clear_tip";//你已处理完所有事件
        public const string midddayDungeon_myRank_str = "#1_midddayDungeon_myRank_str";//我的排名：{0}
        public const string midddayDungeon_myScore_str_num = "#1_midddayDungeon_myScore_str_num";//{0}：{1}当前积分值
        public const string midday_dungeon_end_time_cd_desc = "#1_midday_dungeon_end_time_cd_desc";//活动结束倒计时：{0}
        public const string midday_dungeon_select_hero_no_fight_times_tip = "#1_midday_dungeon_select_hero_no_fight_times_tip";//该大臣无出战次数
        public const string midddayDungeon_guildAssit_times_str = "#1_midddayDungeon_guildAssit_times_str";//公会协助次数：{0}/{1}
        public const string midday_dungeon_playDuration_str_str = "#1_midday_dungeon_playDuration_str_str";//{0}-{1}(GMT {2})
        public const string midday_dungeon_box_invalid_tip = "#1_midday_dungeon_box_invalid_tip";//宝箱已失效
        public const string midday_dungeon_box_reach_max_count = "#1_midday_dungeon_box_reach_max_count";//{今日已达领取上限{0}次
        public const string midday_dungeon_box_has_draw_count = "#1_midday_dungeon_box_has_draw_count";//宝箱已领取次数{0}/{1}次
        public const string midday_dungeon_player_can_draw_count = "#1_midday_dungeon_player_can_draw_count";//玩家可领取宝箱次数{0}/{1}次
        #endregion
        
        #region 晚间活动 EveningDungeon

        public const string eveningDungeon_heroLeftFightCount_num = "#1_eveningDungeon_heroLeftFightCount_num";//剩余出战次数：{0}
        public const string eveningDungeon_heroFightCountMaxTip_none = "#1_eveningDungeon_heroFightCountMaxTip_none";//该大臣不可出战
        public const string eveningDungeon_bossRevivedCount_num = "#1_eveningDungeon_bossRevivedCount_num";// (复活次数：{0})
        public const string eveningDungeon_noHeroCanFightTip_none = "#1_eveningDungeon_noHeroCanFightTip_none";//当前无可出战大臣
        public const string eveningDungeon_notYetSelectedHeroTip_none = "#1_eveningDungeon_notYetSelectedHeroTip_none";//请选择出战大臣
        public const string eveningDungeon_heroFightCountUseUpTip_none = "#1_eveningDungeon_heroFightCountUseUpTip_none";//出战次数用尽
        public const string eveningDungeon_myRank_str = "#1_eveningDungeon_myRank_str";//我的排名：{0}
        public const string eveningDungeon_notOnTheRankingList_none = "#1_eveningDungeon_notOnTheRankingList_none";//未上榜
        public const string eveningDungeon_myScore_str_num = "#1_eveningDungeon_myScore_str_num";//造成伤害：{0}
        public const string eveningDungeon_playing_str = "#1_eveningDungeon_playing_str";//进行中：{0}
        public const string eveningDungeon_settling_str = "#1_eveningDungeon_settling_str";//结算中：{0}
        public const string eveningDungeon_preparing_str = "#1_eveningDungeon_preparing_str";//准备中：{0}
        public const string eveningDungeon_closing_str = "#1_eveningDungeon_closing_str";//关闭中：{0}
        public const string eveningDungeon_rankInfoDesc_str_str = "#1_eveningDungeon_rankInfoDesc_str_str";//{0}对魔晶巨像造成了{1}伤害
        public const string eveningDungeon_playDuration_str_str = "#1_eveningDungeon_playDuration_str_str";//魔像开采:{0}-{1}(GMT {2})
        public const string eveningDungeon_chat_box_mini_content = "#1_eveningDungeon_chat_box_mini_content";//这是我在晚间活动时发现的宝箱

        #endregion

        #region 七日目标

        public const string sevenDayGoals_day_num = "#1_sevenDayGoals_day_num";//第{0}天
        public const string sevenDayGoals_unlockDaysLeft_num = "#1_sevenDayGoals_unlockDaysLeft_num";//还有{0}天解锁

        #endregion
        
        #region 千万目标 EarningGoal
        public const string earning_goal_main_earning_goal_desc = "#1_earning_goal_main_earning_goal_desc";//First:{0}
        public const string earning_goal_global_item_title = "#1_earning_goal_global_item_title";//全服赚速率先到达{0}/s
        public const string earning_goal_honor_item_title = "#1_earning_goal_honor_item_title";//全服赚速率先到达{0}/s
        public const string earning_goal_honor_process_num_num = "#1_earning_goal_honor_process_num_num";//{0}/{1}
        public const string earning_goal_timestamp_h_m_s = "#1_earning_goal_timestamp_h_m_s";//{0:D2}:{1:D2}:{2:D2}
        #endregion

        #region 旅店 Inn

        public const string inn_stationLevelAndName_level_name = "#1_inn_stationLevelAndName_level_name";//Lv.{0} {1}
        public const string inn_normalGuestCollectTip_num_num = "#1_inn_normalGuestCollectTip_num_num";//已解锁客人：{0}/{1}
        public const string inn_specialGuestCollectTip_num_num = "#1_inn_specialGuestCollectTip_num_num";//已解锁客人：{0}/{1}
        public const string inn_normalGuestEffect_num = "#1_inn_normalGuestEffect_num";//熟练度加成：{0}%
        public const string inn_normalGuestEffect_none = "#1_inn_normalGuestEffect_none";//无
        public const string inn_stationUnlockTip_name = "#1_inn_stationUnlockTip_name";//在{0}建造后解锁
        public const string inn_stationBuildGuestServeRequire_num_num = "#1_inn_stationBuildGuestServeRequire_num";//{0}/{1}
        public const string inn_stationPopularityGain_num = "#1_inn_stationPopularityGain_num";//{0}/接待
        public const string inn_stationFinesseGain_num = "#1_inn_stationFinesseGain_num";//{0}/接待
        public const string inn_stationBuildSuccessTitle_name = "#1_inn_stationBuildSuccessTitle_name";//{0}建造成功
        public const string inn_stationNameAndLevel_name_level = "#1_inn_stationNameAndLevel_name_level";//{0}{1}级
        public const string inn_dishUnlockProgress_num_num = "#1_inn_dishUnlockProgress_num_num";//菜品解锁进度：{0}/{1}
        public const string inn_dishNum_num = "#1_inn_dishNum_num";//No.{0}
        public const string inn_dishUpgradeSuccess_none = "#1_inn_dishUpgradeSuccess_none";//升级成功
        public const string inn_dishCantUpgrade_name = "#1_inn_dishCantUpgrade_name";//{0}熟练度不足
        public const string inn_medalBuildingCurBonusDesc_value = "#1_inn_medalBuildingCurBonusDesc_value";//所有建筑员工基础收益+{0}
        public const string inn_medalBuildingNextBonusDesc_value = "#1_inn_medalBuildingNextBonusDesc_value";//(下一级+{0})
        public const string inn_medalCanUpgradeCount_num = "#1_inn_medalCanUpgradeCount_num";//可升级次数：{0}
        public const string inn_nothingToGetInCashRegister_none = "#1_inn_nothingToGetInCashRegister_none";//暂无收益
        public const string inn_dishNoRecipeTip_none = "#1_inn_dishNoRecipeTip_none";//您还未获取该菜谱
        public const string inn_serveTheSpcialGuestFirst_none = "#1_inn_serveTheSpcialGuestFirst_none";//请先迎接特殊客人
        public const string inn_dishNotUnlock_none = "#1_inn_dishNotUnlock_none";//菜品未获得
        public const string inn_normalGuestUnlockEffect_value = "#1_inn_normalGuestUnlockEffect_value";//解锁后熟练度加成：{0}%
        public const string inn_noEnoughRewardInCashRegister_none = "#1_inn_noEnoughRewardInCashRegister_none";//收银台内没有足够的奖励
        public const string inn_needMoreGuestTip_none = "#1_inn_needMoreGuestTip_none";//迎宾更多人后可解锁
        public const string inn_currentGuestNumAndTarget_num_num = "#1_inn_currentGuestNumAndTarget_num_num";//总接待客人数量：{0}/{1}
        public const string inn_unlockStationUpgradeTip_none = "#1_inn_unlockStationUpgradeTip_none";//建造所有设施后可升级
        public const string inn_dishBuildingtype_name = "#1_inn_dishBuildingtype_name";//{0}公司收益加成
        public const string inn_specialGuestNoSelectOption_none = "#1_inn_specialGuestNoSelectOption_none";//未选择选项
        public const string inn_museumItemUpgradeSuc_name = "#1_inn_museumItemUpgradeSuc_name";//{0}升级成功

        #endregion
        
        #region 博物馆

        public const string museum_collectProgress_num_num = "#1_museum_collectProgress_num_num"; //收集进度：{0}/{1}
        public const string museum_itemObtainTime_date = "#1_museum_itemObtainTime_date"; //获得时间：{0}

        #endregion

        #region 太空寻宝

        public const string treasureHunt_noTreasureCanPutInLabTip_none = "#1_treasurehunt_notreasurecanputinlabtip_none";//您当前没有可放入的奇物
        public const string treasureHunt_labLockCannotPutInTip_str = "#1_treasurehunt_lablockcannotputintip_str";// {0}实验室还未解锁，无法放入
        public const string treasureHunt_oreMass_num = "#1_treasurehunt_oremass_num"; //矿石质量：{0}g
        public const string treasureHunt_serverMaxOreMass_num = "#1_treasurehunt_servermaxoremass_num"; //本服最大质量：{0}g
        public const string treasureHunt_pendingOreMaxTip_none = "#1_treasureHunt_pendingOreMaxTip_none"; //矿石已达上限无法进行寻宝，请先前往标本间处理
        public const string treasureHunt_akeyExploreTotalCount_num = "#1_treasurehunt_akeyexploretotalcount_num"; //一键探索总次数：{0}
        public const string treasureHunt_oreDetailOreGainNum_num = "#1_treasurehunt_oredetailoregainnum_num"; // 获取数量:{0}
        public const string treasureHunt_oreMassRankTitle_str = "#1_treasurehunt_oremassranktitle_str"; // {0}矿石质量排名
        public const string treasureHunt_oreDetailOreMassRecord_num = "#1_treasurehunt_oredetailoremassrecord_num"; // 质量记录:{0}g
        public const string treasureHunt_oreDetailOreFirstGainTime_str = "#1_treasurehunt_oredetailorefirstgaintime_str"; // 首次获取时间:{0}
        public const string treasureHunt_treasureDetailGainTime_str = "#1_treasurehunt_treasuredetailgaintime_str"; //获取时间：{0}
        public const string treasureHunt_compositeCatalogCompletedTime_str = "#1_treasurehunt_compositecatalogcompletedtime_str"; //组合完成时间：{0}
        public const string treasureHunt_skillActiveSuccTip_none = "#1_treasurehunt_skillactivesucctip_none"; //激活成功
        public const string treasureHunt_skillUpgradeSuccTip_none = "#1_treasurehunt_skillupgradesucctip_none"; //升级成功
        public const string treasureHunt_gameDistance_value = "#1_treasurehunt_gamedistance_value"; //飞行距离{0}米
        public const string treasureHunt_autoDistance_value = "#1_treasurehunt_autodistance_value"; //助跑距离：{0}米
        public const string treasureHunt_maxDistance_value = "#1_treasurehunt_maxdistance_value"; //飞行距离：{0}米
        public const string treasureHunt_protectTime_value = "#1_treasurehunt_protecttime_value"; //保护次数：{0}
        public const string treasureHunt_rewardNum_value = "#1_treasurehunt_rewardnum_value"; //奖励个数：{0}
        public const string treasureHunt_runUpTime_value = "#1_treasureHunt_runUpTime_value";//剩余助跑时间：{0}
        public const string treasureHunt_gameRewardTip_distance = "#1_treasureHunt_gameRewardTip_distance";//距离下一个宝箱：{0}米
        public const string treasureHunt_distanceProgressTarget_value = "#1_treasureHunt_distanceProgressTarget_value";// /{0}
        public const string treasureHunt_commonDistance_value = "#1_treasureHunt_commonDistance_value";//{0}米

        #endregion
        
        #region 杰出者大厅
        
        public const string grave_honor_log_title_num = "#1_grave_honor_log_title_num";//第{0}位杰出者
        public const string grave_new_honor_log_title = "#1_grave_new_honor_log_title";//新晋杰出者
        public const string grave_bless_on_desc = "#1_grave_bless_on_desc";//<color=#F9EA53>{0}</color>{1}（{2}/{3}） 0：描述 1：剩余次数 2：总次数
        public const string grave_bless_off_desc = "#1_grave_bless_off_desc";// <color=#F9EA53>{0}</color>{1} 0：描述
        public const string grave_congratulate_in_cd_tip = "#1_grave_congratulate_in_cd_tip";//今日已祝贺，请明日再来
        public const string grave_congratulate_no_player_achieve_tip = "#1_grave_congratulate_no_player_achieve_tip";//暂无杰出者
        public const string grave_bless_get_desc = "#1_grave_bless_get_desc";//{0}（今日剩余次数{1}/{2}） 0：描述 1：剩余次数 2：总次数

        #endregion

        #region 礼包

        public const string giftPack_extraGemCount_num = "#1_giftPack_extraGemCount_num"; //{0}额外赠送
        public const string giftPack_buyLimit_num = "#1_giftPack_buyLimit_num";//限购 {0}
        public const string giftPack_leftTime_num_num_num = "#1_giftPack_leftTime_num_num_num";//{0}:{1}:{2}

        #endregion

        #region 支付

        public const string pay_cancel_none = "#1_pay_cancel_none";//支付取消
        public const string pay_failed_num = "#1_pay_failed_num";//支付失败，错误码：{0}
        public const string pay_useVoucherDesc_none = "#1_pay_useVoucherDesc_none";//您拥有相同面额的代金券，是否直接使用代金券购买，无需支付现金？

        #endregion

        #region VIP

        public const string vip_unlockNewRewardTip_num = "#1_vip_unlockNewRewardTip_num";//获取{0}VIP经验，即可邀请
        public const string vip_nextLevelNeedExp_num_num = "#1_vip_nextLevelNeedExp_num_num";//还差{0}VIP经验即可到达VIP{1}
        public const string vip_upgradeVIPDesc_num = "#1_vip_upgradeVIPDesc_num";//再获得{0}充值经验可达到下一个VIP档位
        public const string vip_rewardTitle_num = "#1_vip_rewardTitle_num";//VIP{0}奖励
        public const string vip_exceedMaxLevelDesc_none = "#1_vip_exceedMaxLevelDesc_none";//(超过VIP上限等级的描述)
        public const string vip_privilegeDescTitle_num = "#1_vip_privilegeDescTitle_num";//VIP {0} 特权
        public const string vip_tabVIPLevel_num = "#1_vip_tabVIPLevel_num";//VIP {0}

        #endregion

        #region 首充礼包

        public const string firstRecharge_getRewardCD_num_num_num = "#1_firstRecharge_getRewardCD_num_num_num"; //还需{0}:{1}:{2}才可领取
        public const string firstRecharge_hadGetReward_none = "#1_firstRecharge_hadGetReward_none"; //礼包已领取
        public const string firstRecharge_canGetRewardLeftDay_num = "#1_firstRecharge_canGetRewardLeftDay_num"; //购买礼包后第{0}天可领取
        public const string firstRecharge_giftPackProfit_num = "#1_firstRecharge_giftPackProfit_num"; //{0}%价值

        #endregion

        #region 公会PVE副本

        public const string guild_dungeon_monster_level_name = "#1_guild_dungeon_monster_level_name";//{0}(危险度：{1}) 0：名称 1：等级
        public const string guild_dungeon_lock_tip = "#1_guild_dungeon_lock_tip";//公会副本未解锁
        public const string guild_dungeon_startSecondCheckWndTitle_none = "#1_guild_dungeon_startSecondCheckWndTitle_none";//开启联盟pve副本二次确认弹窗标题
        public const string guild_dungeon_startSecondCheckWndContent_none = "#1_guild_dungeon_startSecondCheckWndContent_none";//是否要消耗{0}{1}开启{2}吗////0：消耗的资源数量，1：消耗的资源名称，2：开启的副本名称
        public const string guild_dungeon_upgradeSecondCheckWndTitle_none = "#1_guild_dungeon_upgradeSecondCheckWndTitle_none";//升级联盟pve副本二次确认弹窗标题
        public const string guild_dungeon_upgradeSecondCheckWndContent_none = "#1_guild_dungeon_upgradeSecondCheckWndContent_none";//是否消耗{0}{1}升级副本，升级后下次开启副本将会提高怪物的血量和产出的奖励！//0：消耗的资源名称1：数量
        public const string guild_dungeon_autoStart_set_suc_tip = "#1_guild_dungeon_autoStart_set_suc_tip";//设置自动开启成功
        
        public const string guild_dungeon_log_attack_desc_string_string_num = "#1_guild_dungeon_log_attack_desc_string_string_num";//{0}对{1}造成了{2}伤害
        public const string guild_dungeon_log_kill_desc_string_string = "#1_guild_dungeon_log_kill_desc_string_string";//{0}击败了{1}
        public const string guild_dungeon_log_start_desc_string_string_string = "#1_guild_dungeon_log_start_desc_string_string_string";//{0}使用{1}开启了{2} 
        public const string guild_dungeon_log_start_cost_desc_num_name = "#1_guild_dungeon_log_start_cost_desc_num_name";//{0}{1} 0：消耗数量 1：消耗物品名称
        public const string guild_dungeon_unlockCondition_num = "#1_guild_dungeon_unlockCondition_num";//公会{0}级解锁
        public const string guild_dungeon_preMonsterHasNotDefeat_tip = "#1_guild_dungeon_preMonsterHasNotDefeat_tip";//前置怪物未击败
        public const string guild_dungeon_monsterHasDefeat_tip = "#1_guild_dungeon_monsterHasDefeat_tip";//当前怪物已击败
        public const string guild_dungeon_noHeroCanFight_tip = "#1_guild_dungeon_noHeroCanFight_tip";//没有可战斗英雄
        public const string guild_dungeon_recoverHeroFightWndTitle_none = "#1_guild_dungeon_recoverHeroFightWndTitle_none";//恢复英雄战斗二次确认弹窗标题
        public const string guild_dungeon_recoverHeroFightWndContent_none = "#1_guild_dungeon_recoverHeroFightWndContent_none";//是否消耗{0}个{1}，恢复{2}//0：消耗的恢复道具数量，1：消耗的恢复道具名称，2：恢复的大臣名称
        public const string guild_dungeon_level_name_num = "#1_guild_dungeon_level_name_num";//(lv：{0}) 0：等级
        public const string guild_dungeon_no_permit_to_optn_tip = "#1_guild_dungeon_no_permit_to_optn_tip";//无权限开启副本提示
        public const string guild_dungeon_aotu_settle_time = "#1_guild_dungeon_aotu_settle_time"; // 刷新：{0}
        public const string guild_dungeon_auto_open_cost_desc_numnum = "#1_guild_dungeon_auto_open_cost_desc_numnum"; // 自动开启消耗：{0:联盟副本当前拥有得财富}/{1:当前勾选得自动开启得副本总消耗}
        public const string guild_dungeon_enter_tag_mode_tip = "#1_guild_dungeon_enter_tag_mode_tip";//点击怪物头像进行标记
        public const string guild_dungeon_monsterBeDefeatByOthers_tip = "#1_guild_dungeon_monsterBeDefeatByOthers_tip";//怪物被其他玩家击败

        #endregion

        #region 公会协作

        public const string guildCooperate_resetTimeCountDown_str = "#1_guildCooperate_resetTimeCountDown_str";//重置倒计时：{0}
        public const string guildCooperate_useItemRecoverHeroLeftCount_num_num = "#1_guildCooperate_useItemRecoverHeroLeftCount_num_num";//道具恢复：{0}/{1}
        public const string guildCooperate_canNotUseItemRecoverHero_none = "#1_guildCooperate_canNotUseItemRecoverHero_none";//今日无法使用道具恢复
        public const string guildCooperate_heroConstructionValue_num = "#1_guildCooperate_heroConstructionValue_num";//建设值：{0}
        public const string guildCooperate_logDesc_time_name_str_str_num = "#1_guildCooperate_logDesc_time_name_str_str_num";//{0} {1}增加了{2}-{3}的{4}点建设值
        public const string guildCooperate_canNotGetRewardTip_none = "#1_guildCooperate_canNotGetRewardTip_none";//需要将周围属性据点全部建设完成才可领取
        public const string guildCooperate_resetNoticeTitle_none = "#1_guildCooperate_resetNoticeTitle_none";//活动结束通知
        public const string guildCooperate_resetNoticeContent_none = "#1_guildCooperate_resetNoticeContent_none";//这一轮的公会协作任务已结束，新一轮的公会协作已开启。请重新进入该区域
        public const string guildCooperate_dispatchHeroDesc_str = "#1_guildCooperate_dispatchHeroDesc_str";//派遣伙伴的{0}相性属性和等级越高，可增加更多的建设值
        public const string guildCooperate_recoverHeroSuc_none = "#1_guildCooperate_recoverHeroSuc_none";//恢复次数成功

        #endregion

        #region 公会宝箱

        public const string guild_box_anonymity_name = "#1_guild_box_anonymity_name";//匿名默认名字
        public const string guild_box_canNotCollectAllTip_none = "#1_guild_box_canNotCollectAllTip_none";//无可领取宝箱
        public const string guild_box_dailyFreeBoxNum_num_num = "#1_guild_box_dailyFreeBoxNum_num_num";//每日免费赠礼宝箱上限：{0}/{1}

        #endregion

        #region 火星互助

        public const string guild_mars_help_build_desc = "#1_guild_mars_help_build_desc";//请帮我建造{0：等级}的{1：建筑名}
        public const string guild_mars_help_tech_desc = "#1_guild_mars_help_tech_desc";//请帮我研究{0：等级}的{1：研究名字}
        public const string guild_mars_help_team_repair_desc = "#1_guild_mars_help_team_repair_desc";//请帮我治疗伤兵
        public const string guild_mars_help_reduce_time = "#1_guild_mars_help_reduce_time";//已减少：{0：时间}
        public const string guild_mars_help_my_help_be_deal_tip = "#1_guild_mars_help_my_help_be_deal_tip";//{0：玩家名字}为{1：等级}级{2：名字}提供了加速助力{3：已处理次数}/{4：总次数}
        public const string guild_mars_help_my_team_help_be_deal_tip = "#1_guild_mars_help_my_team_help_be_deal_tip";//{0：玩家名字}为{2：队伍名字}提供了加速助力{3：已处理次数}/{4：总次数}
        public const string guild_mars_help_auto_my_help_be_deal_tip = "#1_guild_mars_help_auto_my_help_be_deal_tip";// {0：玩家名字}使用月卡特权自动为{1：等级}级{2：名字}提供了加速助力{3：已处理次数}/{4：总次数}
        public const string guild_mars_help_auto_my_team_help_be_deal_tip = "#1_guild_mars_help_auto_my_team_help_be_deal_tip";//{0：玩家名字}使用月卡特权自动为{2：队伍名字}提供了加速助力{3：已处理次数}/{4：总次数}
        public const string guild_mars_help_deal_help_tip = "#1_guild_mars_help_deal_help_tip";//我们十分感谢您的帮助 
        public const string guild_mars_help_send_help_suc_tip = "#1_guild_mars_help_send_help_suc_tip";//已向联盟发起求助
        
        #endregion

        #region 充值返利

        public const string rechargeRebate_stepVipExpProgress_num_num = "#1_rechargeRebate_stepVipExpProgress_num_num";//VIP Exp:{0}/{1}
        public const string rechargeRebate_stepDayProgress_num_num = "#1_rechargeRebate_stepDayProgress_num_num";//充值天数：{0}/{1}

        #endregion

        #region 前往火星

        public const string marsGoTo_totalNavigationTime_str = "#1_marsGoTo_totalNavigationTime_str";//累计航行：{0}
        public const string marsGoTo_navigationTime_str = "#1_marsGoTo_navigationTime_str";//航行：{0}
        public const string marsGoTo_landingNeedSelectArea_none = "#1_marsGoTo_landingNeedSelectArea_none";//请先选择登录地点
        public const string marsGoTo_speedUpDesc_num = "#1_marsGoTo_speedUpDesc_num";//航线加速{0}%
        public const string marsGoTo_speedUpTip_none = "#1_marsGoTo_speedUpTip_none";//众多火星先驱维护的航线，为后来者提供便利~
        public const string marsGoTo_arriveMarsTotalTime_str = "#1_marsGoTo_arriveMarsTotalTime_str";//抵达火星需要{0}
        public const string marsGoto_msgSendTimeDesc_time_str_str = "#1_marsGoto_msgSendTimeDesc_time_str_str";//{0} {1}{2}

        #endregion

        
        #region 火星基地

        public const string mars_levelAndName_level_name = "#1_mars_levelAndName_level_name";//Lv.{0} {1}
        public const string mars_intelligentControlCoolingDown_str = "#1_mars_intelligentControlCoolingDown_str";//冷却中:{0}
        public const string mars_intelligentControlCoolingTime_str = "#1_mars_intelligentControlCoolingTime_str";//冷却时间:{0}
        public const string mars_intelligentControlEffective_str = "#1_mars_intelligentControlEffective_str";//生效中:{0}
        public const string mars_energyConsumePerMin_num = "#1_mars_energyConsumePerMin_num";//每分钟消耗能量:{0}
        public const string mars_energyYieldPerMin_num = "#1_mars_energyYieldPerMin_num";//每分钟产出能量:{0}
        public const string mars_building_equipment_not_full_level = "#1_mars_building_equipment_not_full_level";//请先将所有设备升级至最高等级
        public const string mars_buildingCantBuildTip_none = "#1_mars_buildingCantBuildTip_none";//建造条件未达成
        public const string mars_buildingCantUpgradeTip_none = "#1_mars_buildingCantUpgradeTip_none";//升级条件未达成
        public const string mars_buildingEquipmentLevelLimitTip_none = "#1_mars_buildingEquipmentLevelLimitTip_none";//请先提升建筑等级
        public const string mars_commonPerMin_num = "#1_mars_commonPerMin_num";//{0}/min
        public const string mars_propertyNameEnergyOutput_none = "#1_mars_propertyNameEnergyOutput_none";//能量产出
        public const string mars_propertyNameEnergyMaxStorage_none = "#1_mars_propertyNameEnergyMaxStorage_none";//能量储存上限
        public const string mars_propertyNameEnergyConsume_none = "#1_mars_propertyNameEnergyConsume_none";//能量消耗
        public const string mars_propertyNameSatietyYield_none = "#1_mars_propertyNameSatietyYield_none";//饱食度产出
        public const string mars_propertyNameCureRate_none = "#1_mars_propertyNameCureRate_none";//治愈速度
        public const string mars_propertyNameCureNumRange_none = "#1_mars_propertyNameCureNumRange_none";//治愈人数范围
        public const string mars_propertyNameComfortYield_none = "#1_mars_propertyNameComfortYield_none";//舒适度产出
        public const string mars_propertyNameMoodYield_none = "#1_mars_propertyNameMoodYield_none";//心情产出
        public const string mars_propertyNameSleepYield_none = "#1_mars_propertyNameSleepYield_none";//睡眠度产出
        public const string mars_propertyNameLivingPeopleNumLimit_none = "#1_mars_propertyNameLivingPeopleNumLimit_none";//居住人数上限
        public const string mars_speedUpWndTitle_str = "#1_mars_speedUpWndTitle_str";//{0}加速
        public const string mars_speedUpWndNotSelectedItem_none = "#1_mars_speedUpWndNotSelectedItem_none";//未选择加速道具
        public const string mars_speedUpWndSelectedItemNotEnough_none = "#1_mars_speedUpWndSelectedItemNotEnough_none";//加速道具数量不足
        public const string mars_speedUpWndReduceTimeOverflowPopWndTitle_none = "#1_mars_speedUpWndReduceTimeOverflowPopWndTitle_none";//加速时间溢出弹窗标题
        public const string mars_speedUpWndReduceTimeOverflowPopWndContent_none = "#1_mars_speedUpWndReduceTimeOverflowPopWndContent_none";//本次加速有时间溢出，溢出不返还，是否确认消耗道具进行加速
        public const string mars_speedUpWndUsedGeneralItemPopWndTitle_none = "#1_mars_speedUpWndUsedGeneralItemPopWndTitle_none";//使用了通用加速道具弹窗标题
        public const string mars_speedUpWndUsedGeneralItemPopWndContent_none = "#1_mars_speedUpWndUsedGeneralItemPopWndContent_none";//是否确认使用通用加速道具
        public const string mars_timeCompleteNowConfirmTip_str = "#1_mars_timeCompleteNowConfirmTip_str";//消耗:【{0}】来立即完成
        public const string mars_timeCompleteNowRemainTime_str = "#1_mars_timeCompleteNowRemainTime_str";//剩余时间: {0}
        public const string mars_propertyDescEnergyOutput_none = "#1_mars_propertyDescEnergyOutput_none";//能量产出
        public const string mars_propertyDescEnergyMaxStorage_none = "#1_mars_propertyDescEnergyMaxStorage_none";//能量储存上限
        public const string mars_propertyDescEnergyConsume_none = "#1_mars_propertyDescEnergyConsume_none";//能量消耗
        public const string mars_propertyDescSatietyYield_none = "#1_mars_propertyDescSatietyYield_none";//饱食度产出
        public const string mars_propertyDescCureRate_none = "#1_mars_propertyDescCureRate_none";//治愈速度
        public const string mars_propertyDescCureNumRange_none = "#1_mars_propertyDescCureNumRange_none";//治愈人数范围
        public const string mars_propertyDescComfortYield_none = "#1_mars_propertyDescComfortYield_none";//舒适度产出
        public const string mars_propertyDescMoodYield_none = "#1_mars_propertyDescMoodYield_none";//心情产出
        public const string mars_propertyDescSleepYield_none = "#1_mars_propertyDescSleepYield_none";//睡眠度产出
        public const string mars_propertyDescLivingPeopleNumLimit_none = "#1_mars_propertyDescLivingPeopleNumLimit_none";//居住人数上限
        public const string mars_buildingConstructedComplete_name = "#1_mars_buildingConstructedComplete_name";//{0}建造完成
        public const string mars_buildingUpgradeComplete_name_level = "#1_mars_buildingUpgradeComplete_name_level";//{0}达到等级{1}
        public const string mars_buildingQueueUpgradingDesc_name = "#1_mars_buildingQueueUpgradingDesc_name";//{0}正在升级中
        public const string mars_buildingQueueConstructingDesc_name = "#1_mars_buildingQueueConstructingDesc_name";//{0}正在建造中
        public const string mars_buildingQueueName_index = "#1_mars_buildingQueueName_index";//建造队列{0}
        public const string mars_upgradableBuildingNotExistTip_none = "#1_mars_upgradableBuildingNotExistTip_none";//没有可升级的建筑
        public const string mars_tempBuildQueueTime_day = "#1_mars_tempBuildQueueTime_day";
        public const string mars_tryConstructOtherBuildingTip_none = "#1_mars_tryConstructOtherBuildingTip_none";
        public const string mars_noSettlableBuildingTip_none = "#1_mars_noSettlableBuildingTip_none";//没有可入住的建筑

        #endregion

        #region 火星居民

        public const string mars_resident_canContainNum_num_num = "#1_mars_resident_containNum_num_num";//可容纳居民数量: {0}/{1}
        public const string mars_resident_canDispatchNum_num_num = "#1_mars_resident_dispatchNum_num_num";//可派遣数量: {0}/{1}
        public const string mars_resident_replenishingTip_str = "#1_mars_resident_replenishingTip_str";//移民飞船{0}后即将抵达，请耐心等待...
        public const string mars_resident_residentGap_num_num = "#1_mars_resident_residentGap_num_num";//居民缺口: {0}~{1}
        public const string mars_resident_replenishLeftCount_num = "#1_mars_resident_replenishLeftCount_num";//今日剩余请求补给次数: {0}
        public const string mars_resident_replenishCountLimitTip_none = "#1_mars_resident_replenishCountLimitTip_none";//补给已完成，新补给筹备中，请明日再申请..
        public const string mars_resident_replenishPeopleLimitTip_none = "#1_mars_resident_replenishPeopleLimitTip_none";//人口已抵达上限，本次补给不再有新居民补充，是否确认本次补给？
        
        #endregion

        #region 火星科技

        public const string mars_technology_levelConditionDesc_str_num = "#1_mars_technology_levelConditionDesc_str_num";// {0} 等级{1}
        public const string mars_technology_cancelUpgradeWarningTip_str = "#1_mars_technology_cancelUpgradeWarningTip_str";//是否取消{0}的研究？(取消研究只返还....)
        public const string mars_powerSimpleName_none = "#1_mars_powerSimpleName_none";//实力
        public const string mars_technology_hasTechnologyResearchingTip_none = "#1_mars_technology_hasTechnologyResearchingTip_none";//已有科技在研究中，无法进行新的科技研究
        public const string mars_technology_upgradeOriginalTime_str = "#1_mars_technology_upgradeOriginalTime";//原始时间:{0}
        public const string mars_technology_buildingUpgradeingCannotResearchTip_none = "#1_mars_technology_buildingUpgradeingCannotResearchTip_none";//科研所升级中，无法进行科技研究
        public const string mars_technology_buildingUnbuiltTip_none = "#1_mars_technology_buildingUnbuiltTip_none";//科研所未建造
        
        #endregion
        
        #region 火星探索
        public const string mars_exploreLevelUpNeed_num = "#1_mars_exploreLevelUpNeed_num";//再探索{0}次后可提升火星探索中心等级
        public const string mars_exploreLevelUpDesc_num_num = "#1_mars_exploreLevelUpDesc_num_num";//升级条件：完成{1}项情报（{0}/{1}）
        public const string mars_exploreHomeBaseName_playerName = "#1_mars_exploreHomeBaseName_playerName";//{0}的火星基地
        public const string mars_exploreEventMaxTip_none = "#1_mars_exploreEventMaxTip_none";//未处理的情报太多了，别搞~
        public const string mars_exploreDistance_num = "#1_mars_exploreDistance_num";//距离:{0}km
        public const string mars_exploreTimeTake_num = "#1_mars_exploreTimeTake_num";//耗时:{0}h
        public const string mars_exploreMineResourceAmount_num = "#1_mars_exploreMineResourceAmount_num";//资源量:{0}
        public const string mars_exploreMineRemainResource_num = "#1_mars_exploreMineRemainResource_num";//剩余资源:{0}
        public const string mars_explorePvPLogNum_num = "#1_mars_explorePvPLogNum_num";//最多保留{0}探索日志
        public const string mars_explore_resDefSuc = "#1_mars_explore_resDefSuc";
        public const string mars_explore_resDefFail = "#1_mars_explore_resDefFail";
        public const string mars_explore_resAtkSuc = "#1_mars_explore_resAtkSuc";
        public const string mars_explore_resAtkFail = "#1_mars_explore_resAtkFail";
        public const string mars_exploreLog_guildAtkOther_desc = "#1_mars_exploreLog_guildAtkOther_desc";//盟友抢夺战报描述 {0}敌方名称 {1}盟友名称
        public const string mars_exploreLog_otherAtkGuild_desc = "#1_mars_exploreLog_otherAtkGuild_desc";//盟友防守战报描述 {0}敌方名称 {1}盟友名称
        public const string mars_explore_resAtkSucTitle = "#1_mars_explore_resAtkSucTitle";
        public const string mars_explore_resAtkFailTitle = "#1_mars_explore_resAtkFailTitle";
        public const string mars_explore_resDefSucTitle = "#1_mars_explore_resDefSucTitle";
        public const string mars_explore_resDefFailTitle = "#1_mars_explore_resDefFailTitle";
        public const string mars_explore_eventBattleSucTitle = "#1_mars_explore_eventBattleSucTitle";
        public const string mars_explore_eventBattleFailTitle = "#1_mars_explore_eventBattleFailTitle";
        public const string mars_explore_eventSuc = "#1_mars_explore_eventSuc";
        public const string mars_explore_eventFail = "#1_mars_explore_eventFail";
        public const string mars_explore_bossBattleSucTitle = "#1_mars_explore_bossBattleSucTitle";
        public const string mars_explore_bossBattleFailTitle = "#1_mars_explore_bossBattleFailTitle";
        public const string mars_explore_mineCollectCompleteTitle = "#1_mars_explore_mineCollectCompleteTitle";
        public const string mars_explore_resEnd = "#1_mars_explore_resEnd";
        public const string mars_exploreMineOtherPlayerOccupyTip_none = "#1_mars_exploreMineOtherPlayerOccupyTip_none";//该矿点已被其他玩家占领
        public const string mars_exploreMineOtherPlayerForwardTip_none = "#1_mars_exploreMineOtherPlayerForwardTip_none";//该矿点已被其他玩家前往采集
        public const string mars_exploreMinShareMaxCount_num = "#1_mars_exploreMinShareMaxCount_num";//分享数量最多保留最新{0}条
        public const string mars_exploreMineOccupiedByAlliance_none = "#1_mars_exploreMineOccupiedByAlliance_none";//已有同盟玩家驻守，无法前往
        public const string mars_exploreMineShareFinderName_name = "#1_mars_exploreMineShareFinderName_name";//发现者:{0}
        public const string mars_exploreMineShareOccupierName_name = "#1_mars_exploreMineShareOccupierName_name";//占领者:{0}
        public const string mars_exploreMineOccupiedBySelf_none = "#1_mars_exploreMineOccupiedBySelf_none";
        public const string mars_exploreTeamRepairComplete_name = "#1_mars_exploreTeamRepairComplete_name";//{0}修复完成
        public const string mars_exploreMineOccupantName = "#1_mars_exploreMineOccupantName";//【0】{1}
        public const string mars_exploreAlreadySendingTeamTip_none = "#1_mars_exploreAlreadySendingTeamTip_none";//已有队伍在前往该点
        public const string mars_exploreMineHasCollectResource_num = "#1_mars_exploreMineHasCollectResource_num";//已采集资源:{0}
        public const string mars_exploreMineCollectSpeed_num_num = "#1_mars_exploreMineCollectSpeed_num";//采集速度:{0}/h+{1}/h
        public const string mars_exploreMineTeamTroopNum_num = "#1_mars_exploreMineTeamTroopNum_num";//队伍带兵量:{0}
        public const string mars_exploreTeamRepairCancelConfirmDesc_none = "#1_mars_exploreTeamRepairCancelConfirmDesc_none";//是否取消治疗?
        public const string mars_exploreTeamRepairCancelConfirmTitle_none = "#1_mars_exploreTeamRepairCancelConfirmTitle_none";//取消治疗
        public const string mars_explore_attackDangerousTipContent_none = "#1_mars_explore_attackDangerousTipContent_none"; //探索有风险，是否继续派遣队伍? 
        public const string mars_explore_attackHighRiskyTipContent_none = "#1_mars_explore_attackHighRiskyTipContent_none"; //探索有高风险，是否继续派遣队伍?
        public const string mars_explore_guildMineUnderAttack_guildName_attackerName_name = "#1_mars_explore_guildMineUnderAttack_guildName_attackerName_name";//您的同盟矿点正在被{0}的{1}攻击
        public const string mars_exploreMineCollectSpeed_speed = "#1_mars_exploreMineCollectSpeed_speed";//{0}/min
        public const string mars_exploreMineChatShareNoGuildTip_none = "#1_mars_exploreMineChatShareNoGuildTip_none";//未加入联盟
        public const string mars_exploreMineChatShareSuccessTip_none = "#1_mars_exploreMineChatShareSuccessTip_none";//分享成功
        public const string mars_exploreMineChatShareTooFrequentTip_time = "#1_mars_exploreMineChatShareTooFrequentTip_time";//分享过于频繁，请{0}秒稍后再试
        
        #endregion

        #region 服务器

        public const string server_isUnderMaintenance_none = "#1_server_isUnderMaintenance_none";//服务器维护中
        public const string server_inCurServer_none = "#1_server_inCurServer_none";//已在当前服务器
        public const string server_switchAreaTip_none = "#1_server_switchAreaTip_none";//切换其他区服可能会导致游戏内时间变化以及网络延迟，确认要切换吗？

        #endregion

        #region 权益卡

        public const string privilegeCard_activateLeftTime_str = "#1_privilegeCard_activateLeftTime_str";//已激活：剩余{0}
        public const string privilegeCard_alreadyGetRewardToday_none = "#1_privilegeCard_alreadyGetRewardToday_none";//今日奖励已领取

        #endregion

        #region 商店好评

        public const string stroreReviews_finishTip_none = "#1_stroreReviews_finishTip_none";//评价完成

        #endregion
        
        #region 基金
        
        public const string fund_goal_value = "#1_fund_goal_value";//目标：{0}
        public const string fund_briefGoal_value = "#1_fund_briefGoal_value";//目标：{0}
        public const string fund_taskRefreshTime_time = "#1_fund_taskRefreshTime_time";//任务刷新时间：{0}
        public const string fund_taskNameWithTimes_name_value_value = "#1_fund_taskNameWithTimes_name_value_value";//{0}（{1}/{2}）
        public const string fund_activateExpAdd_num = "#1_fund_activateExpAdd_num";//VIP exp +{0}
        public const string fund_noRewardCanDraw_none = "#1_fund_noRewardCanDraw_none";//暂无可领取奖励
        
        #endregion

        #region 限时兑换

        public const string rush_exchange_refresh_time_cd_desc = "#1_rush_exchange_refresh_time_cd_desc";//刷新倒计时：{0}
        public const string rush_exchange_exchanging_time_cd_desc = "#1_rush_exchange_exchanging_time_cd_desc"; //兑换倒计时：{0}
        public const string rush_exchange_wait_time_cd_desc = "#1_rush_exchange_wait_time_cd_desc"; //下一批货物还在等待中：{0}
        public const string rush_exchange_gem_count_desc= "#1_rush_exchange_gem_count_desc"; //需要钻石：{0}
        public const string rush_exchange_use_gem_confim_tip = "#1_rush_exchange_use_gem_confim_tip"; // 是否消耗{0}钻石收集齐所有资源？


        #endregion

        #region 聊天

        public const string chat_getBoxRewardInvalid_none = "#1_chat_getBoxRewardInvalid_none";//已失效
        public const string chat_getBoxRewardLimit_none = "#1_chat_getBoxRewardLimit_none";//达到上限
        public const string chat_getBoxRewardEmpty_none = "#1_chat_getBoxRewardEmpty_none";//已领完
        public const string chat_getBoxRewardGain_none = "#1_chat_getBoxRewardGain_none";//已领取
        public const string chat_Npc_name_System = "#3_chat_Npc_name_System";//系统
        public const string chat_forbidChatTip_str = "#1_chat_forbidChatTip_str";//您已被禁言（{0}）
        public const string chat_forbidChatLeftTime_str = "#1_chat_forbidChatLeftTime_str";//剩余{0}
        public const string chat_forbidChatForever_none = "#1_chat_forbidChatForever_none";//永久
        public const string chat_reportInputEmptyTip_none = "#1_chat_reportInputEmptyTip_none";//请填写举报说明
        public const string chat_reportSendSuc_none = "#1_chat_reportSendSuc_none";//举报发送成功！
        public const string chat_alreadyReport_none = "#1_chat_alreadyReport_none";//您已举报该玩家，客服将尽快处理！

        #endregion

        #region 旧系统KEY

        public const string shop_buyNumMax_none = "#1_shop_buyNumMax_none"; //限购次数已经用完
        public const string shop_buy_noEnough_none = "#1_shop_buy_noEnough_none"; //玩家可购数量不足
        public const string shop_friendsCountIsMax_none = "#1_shop_friendsCountIsMax_none";
        public const string shop_costRefreshSuc_none = "#1_shop_costRefreshSuc_none";//付费刷新成功
        public const string shop_allAgree_none = "#1_shop_allAgree_none";
        public const string shop_allRefuse_none = "#1_shop_allRefuse_none";
        public const string shop_cdIsMax_none = "#1_shop_cdIsMax_none";
        public const string shop_freeRefreshIsMax_none = "#1_shop_freeRefreshIsMax_none";//免费刷新次数达到上限
        public const string shop_costRefreshIsMax_none = "#1_shop_costRefreshIsMax_none";//付费刷新次数达到上限
        public const string shop_refresh_noEnough_none = "#1_shop_refresh_noEnough_none";//付费刷新消耗不足
        public const string shop_buyItem_title_none = "#1_shop_buyItem_title_none";//钻石购买二次确认弹窗标题
        public const string shop_buyItem_desc_num_str_num_str = "#1_shop_buyItem_desc_num_str_num_str";//钻石购买二次确认弹窗内容，是否消耗{0}个{1}购买{2}个{3}
        public const string shop_buyLimitCout_num_num = "#1_shop_buyLimitCout_num_num";//剩余购买次数{0}/{1}
        public const string shop_freeRefresh_desc_num = "#1_shop_freeRefresh_desc_num";//商店免费刷新二次确认弹窗内容，是否免费刷新商店？剩余刷新次数{0}
        public const string shop_freeRefresh_title_none = "#1_shop_freeRefresh_title_none";//商店免费刷新二次确认弹窗标题
        public const string shop_costRefresh_desc_num_str_num = "#1_shop_costRefresh_desc_num_str_num";//商店付费刷新二次确认弹窗内容，是否消耗{0}个{1}是刷新商店？剩余刷新次数{2}
        public const string shop_costRefresh_title_none = "#1_shop_costRefresh_title_none";//商店付费刷新二次确认弹窗标题
        public const string shop_autoRefresh_none = "#1_shop_autoRefresh_none";//商店自动刷新弹窗内容
        public const string shop_allAutoRefresh_none = "#1_shop_allAutoRefresh_none";//所有商店均已刷新
        public const string shop_buy_succ = "#1_shop_buy_succ_none";//购买成功
        public const string shop_free_refresh_count = "#1_shop_free_refresh_count_num";//次数：{0}/{1}
        public const string shop_num_title_count = "#2_shop_num_title2";//x {0}

        // public const string shop_refresh_str = "#1_shop_refresh_str";//系统刷新倒计时{0}
        // public const string shop_freeRefreshTime_des = "#2_shop_freeRefreshTime_des";//免费刷新倒计时{0}
        // public const string shop_freeRefreshTimeToMax_des = "#2_shop_freeRefreshTimeToMax_des";
        public const string quest_count_max_none = "#1_quest_count_max_none";
        public const string quest_same_none = "#1_quest_same_none";
        public const string quest_doneCountMax_none = "#1_quest_doneCountMax_none";
        public const string quest_noEnoughCostItem_none = "#1_quest_noEnoughCostItem_none";
        public const string quest_getQuest_suc = "#1_quest_getQuest_suc";
        public const string quest_followSuc_str = "#1_quest_followSuc_str";
        public const string quest_follow_str_num_num = "#1_quest_follow_str_num_num";
        public const string quest_followFail_str = "#1_quest_followFail_str";
        public const string quest_progress_num_num = "#1_quest_progress_num_num";
        public const string quest_name_str = "#1_quest_name_str";
        public const string quest_drop_none = "#1_quest_drop_none";
        public const string quest_drop_cancel_none = "#1_quest_drop_cancel_none";
        public const string quest_drop_sure_none = "#1_quest_drop_sure_none";

        public const string mission_layout_not_pet = "#1_mission_layout_not_pet";
        public const string mission_EnemyNum_num = "#1_mission_EnemyNum_num";
        public const string mission_enter_lineup_none = "#1_mission_enter_lineup_none";

        public const string chat_playerCardPower_num = "#1_chat_playerCardPower_num";//国力
        public const string chat_playerCard_allianceStr = "#1_chat_playerCard_allianceStr";//玩家卡片中有联盟显示的key
        public const string chat_playerCard_allianceName_none = "#1_chat_playerCard_allianceName_none"; //玩家卡片中没有联盟显示的key
        public const string chat_boxLeftCount_num_num = "#1_chat_boxLeftCount_num_num";
        public const string chat_miniShowSender_name = "#1_chat_miniShowSender_name";
        public const string chat_sendInCd_seconds = "#1_chat_sendInCd_seconds";
        public const string chat_cdShow_seconds = "#1_chat_cdShow_seconds";
        public const string chat_lock_str = "#1_chat_lock_str";
        public const string chat_inputTextNull_none = "#1_chat_inputTextNull_none";
        public const string chat_share_child_str = "#1_chat_share_child_str";//我的子嗣：{0}
        public const string chat_share_consort_str = "#1_chat_share_consort_str";//我的妃子：{0}
        public const string chat_share_consortCG_str = "#1_chat_share_consortCG_str";//我的妃子CG：{0}
        public const string chat_share_hero_str = "#1_chat_share_hero_str";//我的骑士：{0}
        public const string chat_share_mars_explore_mine_str = "#1_chat_share_mars_explore_mine_str";//火星矿点：{0}
        public const string chat_share_clothes_none = "#1_chat_share_clothes_none";//分享穿搭
        public const string chat_emote_group_lock_send_tip = "#1_chat_emote_group_lock_send_tip";//表情包未解锁，不能发送该表情
        public const string chat_channle_no_selected = "#1_chat_channle_no_selected_none";//您尚未选择需要删除的频道
        public const string chat_shareHeroIsInMargin_num = "#1_chat_shareHeroIsInMargin_num";//{0}时间内不能重复分享骑士
        public const string chat_shareConsortIsInMargin_num = "#1_chat_shareConsortIsInMargin_num";//{0}时间内不能重复分享妃子
        public const string chat_shareConsortCGIsInMargin_num = "#1_chat_shareConsortCGIsInMargin_num";//{0}时间内不能重复分享妃子CG
        public const string chat_shareChildIsInMargin_num = "#1_chat_shareChildIsInMargin_num";//{0}时间内不能重复分享子嗣
        public const string chat_shareClothesIsInMargin_num = "#1_chat_shareClothesIsInMargin_num";//{0}时间内不能重复分享穿搭
        public const string chat_channle_msg_time_str = "#1_chat_channle_msg_time_str";//{0}前消息
        public const string chat_channle_msg_time_less_min_none = "#1_chat_channle_msg_time_less_min_none";//1分钟内消息
        public const string chat_private_up_to_top_max = "#1_chat_private_up_to_top_max_none";//私聊置顶数量已达上限
        public const string chat_share_hero_level_num = "#1_chat_share_hero_level_num";// 分享的骑士等级显示， 等级：{0}
        public const string chat_shareSuccess_none = "#1_chat_shareSuccess_none";// 分享成功
        public const string chat_shareNotJoinGuild_none = "#1_chat_shareNotJoinGuild_none";// 未加入联盟，无法分享到联盟频道

        public const string playerID_num = "#1_playerID_num";//玩家cid {0}
        public const string player_lazyCd_num = "#1_player_lazyCd_num"; //{0}/{1}

        public const string account_no_record_none = "#1_account_no_record_none";

        public const string machine_curJoinSuc_none = "#1_machine_curJoinSuc_none";
        public const string machine_finishJoinFailed_none = "#1_machine_finishJoinFailed_none";
        public const string machine_share_cd_tip_str = "#1_machine_share_cd_tip_str";
        public const string machine_share_suc_tip_none = "#1_machine_share_suc_tip_none";
        public const string machine_closeCDTime_timeStr = "#1_machine_closeCDTime_timeStr";
        public const string machine_share_title_none = "#1_machine_share_title_none";
        public const string machine_canNotGetRewardTip_none = "#1_machine_canNotGetRewardTip_none";
        public const string machine_share_banner_expired_tip_none = "#1_machine_share_banner_expired_tip_none";
        public const string machine_share_goto_confirm_title_none = "#1_machine_share_goto_confirm_title_none";
        public const string machine_share_goto_confirm_desc_none = "#1_machine_share_goto_confirm_desc_none";
        public const string machine_share_minichat_content_none = "#1_machine_share_minichat_content_none";

        public const string treasure_map_goto_desc_str_num_num = "#1_treasure_map_goto_desc_str_num_num";
        public const string treasure_map_desc_str_num_num = "#1_treasure_map_desc_str_num_num";

        public const string space_teleport_confirm_str = "#1_space_teleport_confirm_str";
        public const string space_coordinates_num_num = "#1_space_coordinates_num_num";
        public const string space_machine_unlock_remain_desc_none = "#1_space_machine_unlock_remain_desc_none";
        public const string space_leftExcavateStoneCount_num_num = "#1_space_leftExcavateStoneCount_num_num";
        public const string space_partyOpFightAskContent_str = "#1_space_partyOpFightAskContent_str";
        public const string space_partyOpFightAskTitle_none = "#1_space_partyOpFightAskTitle_none";
        public const string space_partyOpKickTip_none = "#1_space_partyOpKickTip_none";
        public const string space_partyOpKickItemTip_none = "#1_space_partyOpKickItemTip_none";
        public const string space_partyOpKickAsk_str = "#1_space_partyOpKickAsk_str";
        public const string space_partyOpKickAskTitle_none = "#1_space_partyOpKickAskTitle_none";
        public const string space_machineTodayGetRewardCount_num_num = "#1_space_machineTodayGetRewardCount_num_num";
        public const string space_distanceToTarget_num = "#1_space_distanceToTarget_num";
        public const string space_party_self_name_str = "#1_space_party_self_name_str";
        public const string space_party_none_name_str = "#1_space_party_none_name_str";
        public const string space_partyInfo_playerCount_num = "#1_space_partyInfo_playerCount_num";
        public const string space_partyNone_str = "#1_space_partyNone_str";
        public const string space_party_create_tip = "#1_space_party_create_tip";
        public const string space_cantAutoMoveToTarget_none = "#1_space_cantAutoMoveToTarget_none";
        public const string space_no_select_food_tip_none = "#1_space_no_select_food_tip_none";
        public const string space_partyOpFightTip_none = "#1_space_partyOpFightTip_none";

        public const string arena_rank_num = "#1_arena_rank_num";
        public const string arena_rank_range_num_num = "#1_arena_rank_range_num_num";

        public const string party_partyMain_addSeatAskTitle_str = "#1_party_partyMain_addSeatAskTitle_str";
        public const string party_partyMain_addSeatAskDesc_str = "#1_party_partyMain_addSeatAskDesc_str";
        public const string party_partyMain_addSeatTip_none = "#1_party_partyMain_addSeatTip_none";
        public const string party_owner_reward_desc_str = "#1_party_owner_reward_desc_str";
        public const string party_joiner_be_kick_desc_str = "#1_party_joiner_be_kick_desc_str";
        public const string party_joiner_be_attack_desc_str = "#1_party_joiner_be_attack_desc_str";
        public const string party_joiner_reward_desc_str = "#1_party_joiner_reward_desc_str";
        public const string party_daily_remain_time_str = "#1_party_daily_remain_time_str";
        public const string party_player_happy_value_in_party_num = "#1_party_player_happy_value_in_party_num";
        public const string party_infoItemJoinTime_timeStr = "#1_party_infoItemJoinTime_timeStr";
        public const string party_detail_kickAsk_none = "#1_party_detail_kickAsk_none";
        public const string party_detail_kickTip_non = "#1_party_detail_kickTip_non";
        public const string party_share_title_none = "#1_party_share_title_none";
        public const string party_share_suc_tip_none = "#1_party_share_suc_tip_none";
        public const string party_detail_joinTitle_none = "#1_party_detail_joinTitle_none";
        public const string party_detail_joinDesc_none = "#1_party_detail_joinDesc_none";
        public const string party_reward_preview_count_num = "#1_party_reward_preview_count_num";
        public const string party_invite_cd_tip_str = "#1_party_invite_cd_tip_str";
        public const string party_invite_suc_tip_none = "#1_party_invite_suc_tip_none";
        public const string party_partyMainTitle_str = "#1_party_partyMainTitle_str";
        public const string party_partyMainCount_none = "#1_party_partyMainCount_none";
        public const string party_partyMain_remainTime_str = "#1_party_partyMain_remainTime_str";
        public const string party_beAttackContent_str = "#1_party_beAttackContent_str";
        public const string party_endContent_none = "#1_party_endContent_none";
        public const string party_endTitle_none = "#1_party_endTitle_none";
        public const string party_beAttackTitle_none = "#1_party_beAttackTitle_none";
        public const string party_beKickContent_none = "#1_party_beKickContent_none";
        public const string party_beKickTitle_none = "#1_party_beKickTitle_none";
        public const string party_partyList_selfFlourish_num = "#1_party_partyList_selfFlourish_num";
        public const string party_partyList_otherFlourish_num = "#1_party_partyList_otherFlourish_num";
        public const string party_info_friendPartyName_str = "#1_party_info_friendPartyName_str";
        public const string party_info_partyName_str = "#1_party_info_partyName_str";
        public const string party_info_playerCount_num = "#1_party_info_playerCount_num";
        public const string party_partyDetail_otherTitle_str = "#1_party_partyDetail_otherTitle_str";
        public const string party_partyDetail_selfTitle_str = "#1_party_partyDetail_selfTitle_str";
        public const string party_partyDetail_selfJoinTitle_str = "#1_party_partyDetail_selfJoinTitle_str";
        public const string party_partyDetail_playerCount_num = "#1_party_partyDetail_playerCount_num";
        public const string party_remain_time_str = "#1_party_remain_time_str";
        public const string party_daily_time_out_str = "#1_party_daily_time_out_str";
        public const string party_partyDetail_joinedTime_str = "#1_party_partyDetail_joinedTime_str";
        public const string party_create_daily_time_str = "#1_party_create_daily_time_str";
        public const string party_create_charLimit_num = "#1_party_create_charLimit_num";
        public const string party_create_null_none = "#1_party_create_null_none";
        public const string party_defaultDeclaration_none = "#1_party_defaultDeclaration_none";
        public const string party_create_illegalTip_none = "#1_party_create_illegalTip_none";
        public const string party_daily_time_desc_none = "#1_party_daily_time_desc_none";
        public const string party_partyList_playerCount_num = "#1_party_partyList_playerCount_num";
        public const string party_partyList_remainTime_str = "#1_party_partyList_remainTime_str";
        public const string party_happy_value_speed_desc_str = "#1_party_happy_value_speed_desc_str";
        public const string party_happy_value_time_out_desc_str = "#1_party_happy_value_time_out_desc_str";
        public const string party_create_enter_none = "#1_party_create_enter_none";
        public const string party_create_use_str = "#1_party_create_use_str";
        public const string party_share_chat_content_str = "#1_party_share_chat_content_str";
        public const string party_in_party_fight_none = "#1_party_in_party_fight_none";
        public const string party_partyMain_useProtectTitle_none = "#1_party_partyMain_useProtectTitle_none";
        public const string party_partyMain_useProtectDesc_none = "#1_party_partyMain_useProtectDesc_none";
        public const string party_detail_fightTip_none = "#1_party_detail_fightTip_none";
        public const string party_partyList_enterPartyAsk_none = "#1_party_partyList_enterPartyAsk_none";
        public const string party_partyDetail_hotStr_num = "#1_party_partyDetail_hotStr_num";
        public const string party_stat_disable = "#1_party_stat_disable";
        public const string party_partyMain_joinAskTitle_none = "#1_party_partyMain_joinAskTitle_none";
        public const string party_partyMain_joinAskDesc_none = "#1_party_partyMain_joinAskDesc_none";

        public const string active_code_error = "#1_active_code_error";

        public const string hintMap_remainingMines_num = "#1_hintMap_remainingMines_num";
        public const string hintMap_arenaRank_num = "#1_hintMap_arenaRank_num";
        public const string hintMap_teleportCheckText_name = "#1_hintMap_teleportCheckText_name";
        public const string hintMap_itemTeleportTitle_none = "#1_hintMap_itemTeleportTitle_none";
        public const string hintMap_itemTeleportConfirm_none = "#1_hintMap_itemTeleportConfirm_none";

        public const string dailyCheck_totalTime_num_num = "#1_dailyCheck_totalTime_num_num";//签到次数: {0}/{1}-签到界面使用
        public const string dailyCheck_mainTotalTime_num_num = "#1_dailyCheck_mainTotalTime_num_num";//签到次数: {0}/{1}-主界面使用

        public const string dailyCheck_totalReward_num = "#1_dailyCheck_totalReward_num";//累计签到奖励: {0}天
        public const string dailyCheck_getRewardDays_num = "#1_dailyCheck_getRewardDays_num";//签到奖励弹窗显示的签到天数: {0}天

        public const string rankCommon_canLikeCount_num = "#1_rankCommon_canLikeCount_num";
        public const string rankCommon_selfRank_num = "#1_rankCommon_selfRank_num";
        public const string rankCommon_selfScore_num = "#1_rankCommon_selfScore_num";
        public const string rankCommon_score_num = "#1_rankCommon_score_num";
        public const string rankCommon_canLikeCountMax_none = "#1_rankCommon_canLikeCountMax_none";
        public const string commonRank_personalRank_title1 = "#2_commonRank_personalRank_title1";
        public const string commonRank_dayRank_title1 = "#2_commonRank_dayRank_title1";

        public const string mail_mail_count_num = "#1_mail_mailCount_num";//邮件：{0}/{1}
        public const string mail_mail_lock_count_num = "#1_mail_mail_lock_count_num";//收藏：{0}/{1}
        public const string mail_sender_str = "#1_mail_sender_str";//发件人：{0}
        public const string mail_sender_sys_none = "#1_mail_sender_sys_none";//系统
        public const string mail_exceedDel_none = "#1_mail_exceedDel_none";//已经过期邮件自动删除
        public const string mail_hasToReadEnd_none = "#1_mail_hasToReadEnd_none"; //必读需要读完才能删除
        public const string mail_lockTip_none = "#1_mail_lockTip_none";//该信件为收藏邮件，若需要删除邮件请先取消收藏
        public const string mail_delMail_none = "#1_mail_delMail_none";//删除邮件提示内容
        public const string mail_lockMax_none = "#1_mail_lockMax_none";//已达最大收藏数量
        public const string mail_delTip_none = "#1_mail_delTip_none";//一键删除提示内容
        public const string mail_not_akey_delTip_none = "#1_mail_not_akey_delTip_none";//暂无邮件可删除
        public const string mail_not_akey_dealTip_none = "#1_mail_not_akey_dealTip_none";//暂无可一键完成邮件

        public const string setting_switchLanguageTo_str = "#1_setting_switchLanguageTo_str";
        public const string setting_switchVoiceLanguageTo_str = "#1_setting_switchVoiceLanguageTo_str";

        public const string flourish_power_num = "#1_flourish_power_num";
        public const string common_playerIconPower_num = "#1_common_playerIconPower_num";//总国力文本显示

        public const string lazy_cd_max_desc = "#1_lazy_cd_max_desc";
        public const string lazy_cd_duration_desc = "#1_lazy_cd_duration_desc";
        public const string lazy_cd_next_remain_desc = "#1_lazy_cd_next_remain_desc";
        public const string lazy_cd_max_remain_desc = "#1_lazy_cd_max_remain_desc";

        public const string achieve_getStepCondition_num = "#1_achieve_getStepCondition_num";
        public const string achieve_process_num = "#1_achieve_process_num";
        public const string achieve_allDone2_none = "#1_achieve_allDone2_none";
        public const string achieve_noFullDesc_num = "#1_achieve_noFullDesc_num";
        public const string achieve_item_process_num = "#1_achieve_item_process_num";

        public const string avatar_snapshot_unlock_cost_desc_num = "#1_avatar_snapshot_unlock_cost_desc_num";
        public const string avatarSnapshot_defaultName_num = "#1_avatarSnapshot_defaultName_num";
        public const string avatar_no_chg_tip_none = "#1_avatar_no_chg_tip_none";
        public const string avatar_save_suc_none = "#1_avatar_save_suc_none";
        public const string avatar_quit_without_save_tip_title_none = "#1_avatar_quit_without_save_tip_title_none";
        public const string avatar_quit_without_save_tip_desc_none = "#1_avatar_quit_without_save_tip_desc_none";
        public const string avatar_has_suit_tip_none = "#1_avatar_has_suit_tip_none";
        public const string avatar_snapshot_unlock_title_none = "#1_avatar_snapshot_unlock_title_none";
        public const string avatar_snapshot_unlock_desc_none = "#1_avatar_snapshot_unlock_desc_none";

        public const string playerDress_noGet_str = "#1_playerDress_noGet_str"; //未获得提示
        public const string playerDress_expired_str = "#1_playerDress_expired_str"; //已过期 
        public const string playerDress_useSuc_str = "#1_playerDress_useSuc_str";//穿戴成功
        public const string playerDress_unUseSuc_str = "#1_playerDress_unUseSuc_str";//卸下成功

        public const string playerDress_alwaysEnable_str = "#1_playerDress_alwaysEnable_str"; //永久获得
        public const string playerDress_isUsing_str = "#1_playerDress_isUsing_str";//使用中
        public const string playerDress_lock_str = "#1_playerDress_lock_str";//未获得

        public const string mini_map_party_confirm_desc = "#1_mini_map_party_confirm_desc";
        public const string mini_map_digging_confirm_desc = "#1_mini_map_digging_confirm_desc";
        public const string mini_map_arena_confirm_desc = "#1_mini_map_arena_confirm_desc";

        public const string skill_res_not_enough = "#1_skill_res_not_enough";
        public const string lack_gold = "#1_lack_gold";
        public const string pos_unable_create_building = "#1_pos_unable_create_building";
        public const string meter_per_second = "#1_meter_per_second";
        public const string timeForever = "#1_TimeForever";
        public const string enter_active_code = "#1_enter_active_code";
        public const string layout_up_limit = "#1_layout_up_limit";
        public const string skill_in_cd = "#1_skill_in_cd";
        public const string unable_to_search_target = "#1_unable_to_search_target";
        public const string search_target_faild = "#1_search_target_faild";
        public const string effect_openHighFrameConfirm_none = "#1_effect_openHighFrameConfirm_none";
        public const string share_in_party_none = "#1_share_in_party_none";
        public const string share_not_in_party_none = "#1_share_not_in_party_none";
        public const string hero_skill_cast_range_error = "#1_hero_skill_cast_range_error";

        public const string common_expiredTimeDesc_none = "#1_common_expiredTimeDesc_none"; //有效期时间说明文本
        public const string common_lvDesc_none = "#1_common_lvDesc_none"; //等级说明文本


        public const string rank_fixed_likeSucTitle_str_num = "#1_rank_fixed_likeSucTitle_str_num"; // 点赞成功界面标题 {排行榜名称} {排名}
        public const string rank_fixed_likeSucLikeScore_num = "#1_rank_fixed_likeSucLikeScore_num"; // 点赞成功界面点赞积分key {积分}
        public const string bag_itemUseTime_title = "#1_bag_itemUseTime_title";// 背包道具时间减少窗口标题
        public const string bag_itemUseTime_timeReduce_value = "#1_bag_itemUseTime_timeReduce_value";// 背包道具时间减少数值
        public const string bag_itemUseTime_reduceTime_value = "#1_bag_itemUseTime_reduceTime_value";// 背包道具单个减少时间数值

        public const string loverCollect_needEarnDiff_num = "#1_loverCollect_needEarnDiff_num";//距离目标还差：{0}
        public const string loverCollect_earnProgress_num_num = "#1_loverCollect_earnProgress_num_num";//当前进度：{0}/{1}

        #endregion
    }
}