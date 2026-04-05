using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ALPackage;
using Excel;
using System.Data;
using System.Data.OleDb;
using System.Reflection;
using NPEnum;
using System.Text;


namespace GOE
{
	public class NPExportWnd : ALExportWnd
	{
	    private static string EXCEL_LINE_DELIMITER = "\t";//行内分隔符
	    private static string EXCEL_LINE = "\r\n";//行间分隔符

	    private ALInputFieldConfirmItem _m_tiSiftTextItem;
	    private ALComfirmItem _m_ciCopyRefdataItem;
	    private ALComfirmItem _m_ciExportSelectItem;

	    public NPExportWnd()
	        : base()
	    {
	        //装备数据对象
	        //_regMenuItem(new EquipExportMenu());

	        _m_ciCopyRefdataItem = new ALComfirmItem("拷贝refdata数据到__dlserverref,同时复制ai", () =>
	        {
	            //==== 导出给服务器使用的所有表数据
	            FireCreater.dlServerRef();

	            EditorUtility.DisplayDialog("提示", "复制完成！", "确定");
	        });

	        _regMenuItem(_m_ciCopyRefdataItem);
	        _m_tiSiftTextItem = new ALInputFieldConfirmItem("过滤名称(多个用;分隔开)：", "", "全部导出当前过滤的表", (_inputStr) =>
	        {
		        NPExportSettingMgr.instance.exeExportCurSelectCanShow();
		        AssetDatabase.Refresh();
	        }, 150, 30);
	        _regMenuItem(_m_tiSiftTextItem);

            _regMenuItem(_m_ciExportSelectItem);
            _regMenuItem(new NPGeneralRefExportMenu("generalref", _judgeTagCanShow));//常量表
	      
	        //品质表
	        _regMenuItem(
	            new _TNPAutoExportRefMenu<NPQualityRefObj, NPGSOQualityRefSet>
	            (ENPExportSettingEnum.QUALITY, NPGSOQualityRefSet.assetPath, NPGSOQualityRefSet.objName
	                , "quality", "quality 通用品质表", _judgeTagCanShow));
	  
	        //品质额外表
	        _regMenuItem(
	            new _TNPAutoExportRefMenu<NPQualityExtRefObj, NPGSOQualityExtRefSet>
	            (ENPExportSettingEnum.QUALITY_EXT, NPGSOQualityExtRefSet.assetPath, NPGSOQualityExtRefSet.objName
	                , "quality_ext", "quality_ext 品质额外表", _judgeTagCanShow));
            
	        //货币表
	        _regMenuItem(
	            new _TNPAutoExportRefUniformMenu<NPCurrencyResObj, NPSOCurrencyResRefSet>
	            (ENPExportSettingEnum.PLAYER_RES, NPSOCurrencyResRefSet.assetPath, NPSOCurrencyResRefSet.objName
	                , "currency", "currency 货币表", ENPItemType.CURRENCY, _judgeTagCanShow));
	        
	        _regMenuItem(new NPSfxExportMenu("sfx", _judgeTagCanShow));
	        _regMenuItem(new _TNPAutoExportRefMenu<NPSfx3DRefObj, NPSOSfx3DRefSet>
	        (ENPExportSettingEnum.SFX_3D, NPSOSfx3DRefSet.assetPath, NPSOSfx3DRefSet.objName
	            , "sfx_3d", "特效3d信息配置表 (sfx_3d)", _judgeTagCanShow));
            
			//_regMenuItem(new NPExpLevelExportMenu("explevel", _judgeTagCanShow));               //等级经验表

			_regMenuItem(new NPAudioExportMenu("audio_ref voice", _judgeTagCanShow));                 //音效表
			//音效组表
			_regMenuItem(new _TNPAutoExportRefMenu<NPAudioGroupRefObj, NPGSOAudioGroupRefSet>
				(ENPExportSettingEnum.QUALITY_GROUP, NPGSOAudioGroupRefSet.assetPath, NPGSOAudioGroupRefSet.objName
					, "audio_group", "audio_group 音效组表", _judgeTagCanShow));
			
			_regMenuItem(new _TNPAutoExportRefMenu<VoiceKeyRefObj, NPGSOVoiceKeyRefSet>(ENPExportSettingEnum.VOICE_KEY, NPGSOVoiceKeyRefSet.assetPath, NPGSOVoiceKeyRefSet.objName
				, "voice_key", "voice_key 配音对应key表", _judgeTagCanShow));
			
	        //server effect只需要导出服务器数据， 因此处理方式略有不同
	        _regMenuItem(new NPOnlyServerRefExportMenu("server_effect", ENPExportSettingEnum.SERVER_EFFECT, "服务端效果 (Server Effect)", _judgeTagCanShow));   //服务器效果
	        _regMenuItem(new _TNPAutoExportRefMenu<NPRemoteEffectRefObj, NPGSORemoteEffectRefSet>
	            (ENPExportSettingEnum.REMOTE_EFFECT, NPGSORemoteEffectRefSet.assetPath, NPGSORemoteEffectRefSet.objName
	            , "remote_effect", "远程执行效果 (remote_effect)", _judgeTagCanShow));
            
	        _regMenuItem(new LanguageExportMenu("language", _judgeTagCanShow));  //登陆游戏内翻译表 
	        _regMenuItem(new PlatLanguageExportMenu("platlanguage", _judgeTagCanShow));  //登陆游戏内翻译表
	        _regMenuItem(new NPDetectorCharacterExportMenu("detectorcharacter", _judgeTagCanShow));  //非法字符表
	        _regMenuItem(new NPDetectorPlayerNameExportMenu("detectorplayername", _judgeTagCanShow));  //玩家取名非法字符表

	        //引导导出部分
	        _regMenuItem(new _TNPAutoExportRefMenu<NPTutorialRef, NPGSOTutorialRefSet>
	            (ENPExportSettingEnum.TUTORIAL, NPGSOTutorialRefSet.assetPath, NPGSOTutorialRefSet.objName
	            , "tutorial", "引导信息 (tutorial)", _judgeTagCanShow));
	        _regMenuItem(new _TNPAutoExportRefMenu<NPTutorialEdgeRef, NPGSOTutorialEdgeRefSet>
	            (ENPExportSettingEnum.TUTORIAL_EDGE, NPGSOTutorialEdgeRefSet.assetPath, NPGSOTutorialEdgeRefSet.objName
	            , "tutorial_edge", "引导跳转边信息 (tutorial edge)", _judgeTagCanShow));
	        _regMenuItem(new _TNPAutoExportRefMenu<NPSimpleTutorialRefObj, NPGSOSimpleTutorialRefSet>
	        (ENPExportSettingEnum.SIMPLE_TUTORIAL, NPGSOSimpleTutorialRefSet.assetPath, NPGSOSimpleTutorialRefSet.objName
		        , "simple_tutorial", "简易引导 (simple_tutorial)", _judgeTagCanShow));
	        _regMenuItem(new _TNPAutoExportRefMenu<NPSimpleTutorialEdgeRef, NPGSOSimpleTutorialEdgeRefSet>
	        (ENPExportSettingEnum.SIMPLE_TUTORIAL_EDGE, NPGSOSimpleTutorialEdgeRefSet.assetPath, NPGSOSimpleTutorialEdgeRefSet.objName
		        , "simple_tutorial_edge", "简易引导跳转边信息 (simple_tutorial_edge)", _judgeTagCanShow));
	        
	        _regMenuItem(new _TNPAutoExportRefMenu<NPPlayerBuffRefObj, NPGSOPlayerBuffRefSet>
		        (ENPExportSettingEnum.PLAYER_BUFF, NPGSOPlayerBuffRefSet.assetPath, NPGSOPlayerBuffRefSet.objName
			        , "player_buff",  "玩家buff(player_buff)", _judgeTagCanShow));    //玩家buff
	        
	        _regMenuItem(new _TNPAutoExportRefMenu<NPSimpleUnlockRef, NPGSOSimpleUnlockRefSet>
	            (ENPExportSettingEnum.SIMPLE_UNLOCK, NPGSOSimpleUnlockRefSet.assetPath, NPGSOSimpleUnlockRefSet.objName
	            , "simple_unlock", "预制系统解锁信息 (simple_unlock)", _judgeTagCanShow));
	         // _regMenuItem(new NPSysInfoRefExportMenu("sysinforef", _judgeTagCanShow)); //领主等级
        
	        _regMenuItem(new _TNPAutoExportRefUniformMenu<SysInfoRef,GSOSysInfoRefSet>(ENPExportSettingEnum.SYS_INFO,
	            GSOSysInfoRefSet.assetPath, GSOSysInfoRefSet.objName, "sys_info", "系统相关信息表(sys_info)", NPEnum.ENPItemType.SYS_INFO, _judgeTagCanShow));
        
	        _regMenuItem(new NPItemAlterRefExportMenu("itemalter", _judgeTagCanShow));
        
	        _regMenuItem(new NPVersionUpRewardExportMenu("versionupreward", _judgeTagCanShow));
            
            _regMenuItem(new _TNPAutoExportRefMenu<NPLoginWayRefObj, NPGSOLoginWayRefSet>
            (ENPExportSettingEnum.LOGIN_WAY, NPGSOLoginWayRefSet.assetPath, NPGSOLoginWayRefSet.objName
                , "login_way", "login_way 登录方式表", _judgeTagCanShow));

	        _regMenuItem(new NPSceneInfoRefExportMenu("sceneinfo", _judgeTagCanShow));       //主场景信息

	        #region 成就

	        _regMenuItem(
		        new _TNPAutoExportRefMenu<AchieveRefObj, GSOAchieveRefSet>
		        (ENPExportSettingEnum.ACHIEVE, GSOAchieveRefSet.assetPath, GSOAchieveRefSet.objName
			        , "achieve", "成就 (achieve)", _judgeTagCanShow));
	        _regMenuItem(
		        new _TNPAutoExportRefMenu<AchieveStepRefObj, GSOAchieveStepRefSet>
		        (ENPExportSettingEnum.ACHIEVE_STEP, GSOAchieveStepRefSet.assetPath, GSOAchieveStepRefSet.objName
			        , "achieve_step", "成就步骤 (achieve_step)", _judgeTagCanShow));
	        _regMenuItem(
		        new _TNPAutoExportRefMenu<AchievePointStepRefObj, GSOAchievePointStepRefSet>
		        (ENPExportSettingEnum.ACHIEVE_POINT_STEP, GSOAchievePointStepRefSet.assetPath, GSOAchievePointStepRefSet.objName
			        , "achieve_point_step", "成就点数阶段 (achieve_point_step)", _judgeTagCanShow));
	        _regMenuItem(
		        new _TNPAutoExportRefMenu<AchieveTypeRefObj, GSOAchieveTypeRefSet>
		        (ENPExportSettingEnum.ACHIEVE_TYPE, GSOAchieveTypeRefSet.assetPath, GSOAchieveTypeRefSet.objName
			        , "achieve_type", "成就类型 (achieve_type)", _judgeTagCanShow));
	        
	        _regMenuItem(
	            new _TNPAutoExportRefUniformMenu<AchievePointRefObj, GSOAchievePointRefSet>
	            (ENPExportSettingEnum.ACHIEVE_POINT, GSOAchievePointRefSet.assetPath, GSOAchievePointRefSet.objName
	                , "achieve_point", "成就点数表（achieve_point）", ENPItemType.ACHIEVE_POINT, _judgeTagCanShow));
	        

	        #endregion

	        #region 聊天相关

	        _regMenuItem(new NPChatRoomRefExportMenu(_judgeTagCanShow)); // 聊天室配置
	        _regMenuItem(
		        new _TNPAutoExportRefMenu<NPChatNPCRefObj, NPGSOChatNPCRefSet>
		        (ENPExportSettingEnum.CHAT_NPC, NPGSOChatNPCRefSet.assetPath, NPGSOChatNPCRefSet.objName
			        , "chat_npc", "聊天NPC (chat_npc)", _judgeTagCanShow));
	        _regMenuItem(new _TNPAutoExportRefUniformMenu<GChatEmoteGroupRefObj, GSOChatEmoteGroupRefSet>
	        (ENPExportSettingEnum.CHAT_EMOTE_GROUP, GSOChatEmoteGroupRefSet.assetPath, GSOChatEmoteGroupRefSet.objName
		        , "chat_emote_group", "chat_emote_group 聊天表情组",ENPItemType.CHAT_EMOTE_GROUP ,_judgeTagCanShow));
	        _regMenuItem(new _TNPAutoExportRefMenu<GChatEmoteItemRefObj, GSOChatEmoteItemRefSet>
	        (ENPExportSettingEnum.CHAT_EMOTE_ITEM, GSOChatEmoteItemRefSet.assetPath, GSOChatEmoteItemRefSet.objName
		        , "chat_emote_item", "chat_emote_item 聊天表情", _judgeTagCanShow));

	        #endregion
	        
            
            //服务端用任务步骤额外效果表
            _regMenuItem(new NPOnlyServerRefExportMenu("quest_step_extra_trigger", ENPExportSettingEnum.QUEST_STEP_EXTRA_TRIGGER, "quest_step_extra_trigger 任务步骤额外效果表", _judgeTagCanShow));
        
        #region reward 奖励信息导出

	        _regMenuItem(new _TNPAutoExportRefUniformMenu<NPSORewardRefObj, NPSORewardRefSet>(ENPExportSettingEnum.REWARD,
	               NPSORewardRefSet.assetPath, NPSORewardRefSet.objName, "reward", "Reward 表", NPEnum.ENPItemType.REWARD, _judgeTagCanShow));

	        _regMenuItem(new NPOnlyServerRefExportMenu("reward_sub", ENPExportSettingEnum.REWARD_SUB, "reward_sub表 (Reward Sub)", _judgeTagCanShow));

	        //_regMenuItem(new NPOnlyServerRefExportMenu("reward_group", ENPExportSettingEnum.REWARD_GROUP, "reward 奖励集合 (Reward Group)", _judgeTagCanShow));

	        //_regMenuItem(new NPOnlyServerRefExportMenu("reward_item", ENPExportSettingEnum.REWARD_ITEM, "reward 奖励单项信息 (Reward Item)", _judgeTagCanShow));
        #endregion
        
        #region rule 功能规则信息导出

        _regMenuItem(new _TNPAutoExportRefMenu<NPRuleRefObj, NPGSORuleRefSet>(ENPExportSettingEnum.RULE,
	        NPGSORuleRefSet.assetPath, NPGSORuleRefSet.objName, "rule", "Rule 表", _judgeTagCanShow));
        _regMenuItem(new _TNPAutoExportRefMenu<NPRuleSubRefObj, NPGSORuleSubRefSet>(ENPExportSettingEnum.RULE_SUB,
	        NPGSORuleSubRefSet.assetPath, NPGSORuleSubRefSet.objName, "rule_sub", "Rule_Sub 表", _judgeTagCanShow));
		#endregion

        #region 背包相关导出
	        //背包物品表 - 同步到uniform
	        _regMenuItem(new _TNPAutoExportRefUniformMenu<BagItemRefObj, GSOBagItemRefSet>(ENPExportSettingEnum.BAG_ITEM,
	                GSOBagItemRefSet.assetPath, GSOBagItemRefSet.objName, "bag_item", "Bag Item 背包物品表", NPEnum.ENPItemType.BAG_ITEM, _judgeTagCanShow));
	        //背包物品使用表
	        _regMenuItem(new _TNPAutoExportRefMenu<BagItemUseRefObj, GSOBagItemUseRefSet>
	                 (ENPExportSettingEnum.BAG_ITEM_USE, GSOBagItemUseRefSet.assetPath, GSOBagItemUseRefSet.objName
	                  , "bag_item_use", "Bag Item Use 背包使用使用表", _judgeTagCanShow));

	        _regMenuItem(
	            new _TNPAutoExportRefMenu<ItemExchangeRefObj, GSOItemExchangeRefSet>
	            (ENPExportSettingEnum.ITEM_EXCHANGE, GSOItemExchangeRefSet.assetPath, GSOItemExchangeRefSet.objName
	                , "item_exchange", "item_exchange 物品转换表", _judgeTagCanShow));

            _regMenuItem(new NPItemConvertRefExportMenu("item_convert", _judgeTagCanShow));

            _regMenuItem(new _TNPAutoExportRefMenu<BagItemHeroRefObj, GSOBagItemHeroRefSet>
            (ENPExportSettingEnum.BAG_ITEM_HERO, GSOBagItemHeroRefSet.assetPath, GSOBagItemHeroRefSet.objName
	            , "bag_item_hero", "bag_item_hero 背包骑士物品表", _judgeTagCanShow));
            _regMenuItem(new _TNPAutoExportRefMenu<BagItemConsortRefObj, GSOBagItemConsortRefSet>
            (ENPExportSettingEnum.BAG_ITEM_CONSORT, GSOBagItemConsortRefSet.assetPath, GSOBagItemConsortRefSet.objName
                , "bag_item_consort", "bag_item_consort 背包情人物品表", _judgeTagCanShow));

            _regMenuItem(new _TNPAutoExportRefMenu<BagItemDyeRefObj, GSOBagItemDyeRefSet>
            (ENPExportSettingEnum.BAG_ITEM_DYE, GSOBagItemDyeRefSet.assetPath, GSOBagItemDyeRefSet.objName
                , "bag_item_dye", "bag_item_dye 背包染色物品表", _judgeTagCanShow));

            _regMenuItem(new NPOnlyServerRefExportMenu("activity_bag_item", ENPExportSettingEnum.BAG_ITEM_ACTIVITY, "活动道具表 (activity_bag_item)", _judgeTagCanShow));

            #endregion

            #region 玩家信息相关 - 称号 - 称号档位 - 头像 - 头像框 - 气泡框 - 勋章 - 等级

            _regMenuItem(new _TNPAutoExportRefUniformMenu<PlayerIconRefObj, GSOPlayerIconRefSet>(ENPExportSettingEnum.PLAYER_ICON,
	         GSOPlayerIconRefSet.assetPath, GSOPlayerIconRefSet.objName, "player_icon", "Player Icon 玩家头像表", NPEnum.ENPItemType.ICON, _judgeTagCanShow));

	        _regMenuItem(new _TNPAutoExportRefUniformMenu<PlayerIconBgkRefObj, GSOPlayerIconBgkRefSet>(ENPExportSettingEnum.PLAYER_ICON_BGK,
	                GSOPlayerIconBgkRefSet.assetPath, GSOPlayerIconBgkRefSet.objName, "icon_bgk", "Icon Bgk 玩家头像框表", NPEnum.ENPItemType.ICON_BGK, _judgeTagCanShow));

	        _regMenuItem(new _TNPAutoExportRefUniformMenu<NPPlayerBubbleRefObj, NPSOPlayerBubbleRefSet>(ENPExportSettingEnum.PLAYER_BUBBLE,
	                NPSOPlayerBubbleRefSet.assetPath, NPSOPlayerBubbleRefSet.objName, "player_bubble", "Player Bubble 玩家气泡框表", NPEnum.ENPItemType.BUBBLE, _judgeTagCanShow));

	        _regMenuItem(
	                new _TNPAutoExportRefMenu<NPPlayerGenderRefObj, NPSOPlayerGenderRefSet>
	                (ENPExportSettingEnum.PLAYER_GENDER, NPSOPlayerGenderRefSet.assetPath, NPSOPlayerGenderRefSet.objName
	                    , "player_gender", "性别表 (player_gender)", _judgeTagCanShow));
	        _regMenuItem(
		        new _TNPAutoExportRefMenu<NPPlayerPropertyRefObj, NPSOPlayerPropertyRefSet>
		        (ENPExportSettingEnum.PLAYER_PROPERTY, NPSOPlayerPropertyRefSet.assetPath, NPSOPlayerPropertyRefSet.objName
			        , "player_property", "玩家属性表 (player_property)", _judgeTagCanShow));

	        _regMenuItem(new _TNPAutoExportRefMenu<PlayerLvlRefObj, NPSOPlayerLvlRefSet>
	                (ENPExportSettingEnum.PLAYER_LVL, NPSOPlayerLvlRefSet.assetPath, NPSOPlayerLvlRefSet.objName
	                , "player_lvl", "玩家等级信息 (player_lvl)", _judgeTagCanShow));

	        _regMenuItem(new _TNPAutoExportRefMenu<PlayerHeroUnlockShowRefObj, GSOPlayerHeroUnlockShowRefSet>
	        (ENPExportSettingEnum.PLAYER_HERO_UNLOCK_SHOW, GSOPlayerHeroUnlockShowRefSet.assetPath, GSOPlayerHeroUnlockShowRefSet.objName
		        , "player_hero_unlock_show", "玩家大臣解锁表现表 (player_hero_unlock_show)", _judgeTagCanShow));
            #endregion

            #region 孵化

	        //聚会保护罩道具表
	        _regMenuItem(new _TNPAutoExportRefMenu<BagItemPartyRefObj, GSOBagItemPartyRefSet>
	        (ENPExportSettingEnum.BAG_ITEM_PARTY, GSOBagItemPartyRefSet.assetPath, GSOBagItemPartyRefSet.objName
		        , "bag_item_party", "bag_item_party 聚会保护罩道具子表", _judgeTagCanShow));


        #endregion
        

            #region 排行榜

            //排行榜通用表
            _regMenuItem(new _TNPAutoExportRefMenu<NPRankRefObj, NPSORankRefSet>
	            (ENPExportSettingEnum.RANK, NPSORankRefSet.assetPath, NPSORankRefSet.objName
	             , "rank", "Rank 排行榜通用表", _judgeTagCanShow));

	        //排行榜常驻表
	        _regMenuItem(new _TNPAutoExportRefMenu<NPRankFixedRefObj, NPSORankFixedRefSet>
	            (ENPExportSettingEnum.RANK_FIXED, NPSORankFixedRefSet.assetPath, NPSORankFixedRefSet.objName
	             , "rank_fixed", "Rank Fixed 排行榜常驻表", _judgeTagCanShow));

            #endregion

            #region 任务
            //任务表
            _regMenuItem(new _TNPAutoExportRefMenu<QuestRefObj, GSOQuestRefSet>
	            (ENPExportSettingEnum.QUEST, GSOQuestRefSet.assetPath, GSOQuestRefSet.objName
	             , "quest", "Quest 任务表", _judgeTagCanShow));

	        //任务步骤表
	        _regMenuItem(new _TNPAutoExportRefMenu<QuestStepRefObj, GSOQuestStepRefSet>
	            (ENPExportSettingEnum.QUEST_STEP, GSOQuestStepRefSet.assetPath, GSOQuestStepRefSet.objName
	             , "quest_step", "Quest Step 任务步骤表", _judgeTagCanShow));

	        //任务目标表
	        _regMenuItem(new _TNPAutoExportRefMenu<QuestTargetRefObj, GSOQuestTargetRefSet>
	            (ENPExportSettingEnum.QUEST_TARGET, GSOQuestTargetRefSet.assetPath, GSOQuestTargetRefSet.objName
	             , "quest_target", "Quest Target 任务目标表", _judgeTagCanShow));
            
	        //任务章节表
	        // _regMenuItem(new _TNPAutoExportRefMenu<QuestGroupRefObj, GSOQuestGroupRefSet>
	        //     (ENPExportSettingEnum.QUEST_GROUP, GSOQuestGroupRefSet.assetPath, GSOQuestGroupRefSet.objName
	        //      , "quest_group", "Quest Group 任务章节表", _judgeTagCanShow));

	        //日常任务
	        _regMenuItem(new _TNPAutoExportRefMenu<DailyQuestRefObj, GSODailyQuestRefSet>
	             (ENPExportSettingEnum.DAILY_QUEST, GSODailyQuestRefSet.assetPath, GSODailyQuestRefSet.objName
	              , "daily_quest", "Daily Quest 日常任务表", _judgeTagCanShow));

	        //日常任务积分奖励
	        _regMenuItem(new _TNPAutoExportRefMenu<DailyQuestActiveRewardRefObj, GSODailyQuestActiveRewardRefSet>
	            (ENPExportSettingEnum.DAILY_QUEST_REWARD, GSODailyQuestActiveRewardRefSet.assetPath, GSODailyQuestActiveRewardRefSet.objName
	             , "daily_quest_active_reward", "Daily Quest Active Reward 日常任务积分奖励表", _judgeTagCanShow));

			//日常任务刷新表
			_regMenuItem(new NPOnlyServerRefExportMenu("daily_quest_refresh", ENPExportSettingEnum.DAILY_QUEST_REFRESH, "daily_quest_refresh 日常任务刷新表", _judgeTagCanShow));

			//日常任务随机组表
			_regMenuItem(new NPOnlyServerRefExportMenu("daily_quest_random_group", ENPExportSettingEnum.DAILY_QUEST_GROUP, "daily_quest_random_group 日常任务随机组表", _judgeTagCanShow));
            #endregion

            #region 邮件

            _regMenuItem(
	           new _TNPAutoExportRefMenu<GMailRefObj, GSOMailRefSet>
	           (ENPExportSettingEnum.MAIL, GSOMailRefSet.assetPath, GSOMailRefSet.objName
	           , "mail", "邮件表 (mail)", _judgeTagCanShow));
	        _regMenuItem(
	           new _TNPAutoExportRefMenu<GMailTypeRefObj, GSOMailTypeRefSet>
	           (ENPExportSettingEnum.MAIL_TYPE, GSOMailTypeRefSet.assetPath, GSOMailTypeRefSet.objName
	           , "mail_type", "邮件类型表 (mail_type)", _judgeTagCanShow));
	        _regMenuItem(
	           new _TNPAutoExportRefMenu<GMailSenderRefObj, GSOMailSenderRefSet>
	           (ENPExportSettingEnum.MAIL_SENDER, GSOMailSenderRefSet.assetPath, GSOMailSenderRefSet.objName
	           , "mail_sender", "邮件发送者表 (mail_sender)", _judgeTagCanShow));

        #endregion

            #region 伙伴相关表

	        //伙伴主表
	        _regMenuItem(new _TNPAutoExportRefUniformMenu<HeroRefObj, GSOHeroRefSet>(ENPExportSettingEnum.HERO,
	            GSOHeroRefSet.assetPath, GSOHeroRefSet.objName, "hero", "伙伴表（hero）", ENPItemType.HERO, _judgeTagCanShow));

			//伙伴等级表
			_regMenuItem(new _TNPAutoExportRefMenu<HeroLevelRefObj, GSOHeroLevelRefSet>(ENPExportSettingEnum.HERO_LEVEL, 
                GSOHeroLevelRefSet.assetPath, GSOHeroLevelRefSet.objName, "hero_level", "伙伴等级表（hero_level）", _judgeTagCanShow));

			//伙伴觉醒表
			_regMenuItem(new _TNPAutoExportRefMenu<HeroStarRefObj, GSOHeroStarRefSet>(ENPExportSettingEnum.HERO_STAR,
                GSOHeroStarRefSet.assetPath, GSOHeroStarRefSet.objName, "hero_star", "伙伴觉醒表（hero_star）", _judgeTagCanShow));

			//伙伴觉醒技能表
			_regMenuItem(new _TNPAutoExportRefMenu<HeroStarSkillRefObj, GSOHeroStarSkillRefSet>(ENPExportSettingEnum.HERO_STAR_SKILL,
                GSOHeroStarSkillRefSet.assetPath, GSOHeroStarSkillRefSet.objName, "hero_star_skill", "伙伴觉醒技能表（hero_star_skill）", _judgeTagCanShow));

			//伙伴觉醒技能等级表
			_regMenuItem(new _TNPAutoExportRefMenu<HeroStarSkillLevelRefObj, GSOHeroStarSkillLevelRefSet>(ENPExportSettingEnum.HERO_STAR_SKILL_LEVEL,
                GSOHeroStarSkillLevelRefSet.assetPath, GSOHeroStarSkillLevelRefSet.objName, "hero_star_skill_level", "伙伴觉醒技能等级表（hero_star_skill_level）", _judgeTagCanShow));

	        //伙伴皮肤表
	        _regMenuItem(new _TNPAutoExportRefUniformMenu<HeroSkinRefObj, GSOHeroSkinRefSet>(ENPExportSettingEnum.HERO_SKIN,
	            GSOHeroSkinRefSet.assetPath, GSOHeroSkinRefSet.objName, "hero_skin", "伙伴皮肤表（hero_skin）", ENPItemType.HERO_SKIN, _judgeTagCanShow));

			//伙伴皮肤等级表
            _regMenuItem(new _TNPAutoExportRefMenu<HeroSkinLevelRefObj, GSOHeroSkinLevelRefSet>(ENPExportSettingEnum.HERO_SKIN_LEVEL,
                GSOHeroSkinLevelRefSet.assetPath, GSOHeroSkinLevelRefSet.objName, "hero_skin_level", "伙伴皮肤等级表（hero_skin_level）", _judgeTagCanShow));

	        //伙伴阶段表
	        _regMenuItem(new _TNPAutoExportRefMenu<HeroStepRefObj, GSOHeroStepRefSet>(ENPExportSettingEnum.HERO_STEP, 
                GSOHeroStepRefSet.assetPath, GSOHeroStepRefSet.objName, "hero_step", "伙伴阶段表（hero_step）", _judgeTagCanShow));

			//伙伴资质技能表
			_regMenuItem(new _TNPAutoExportRefMenu<HeroTalentSkillRefObj, GSOHeroTalentSkillRefSet>(ENPExportSettingEnum.HERO_TALENT_SKILL,
                GSOHeroTalentSkillRefSet.assetPath, GSOHeroTalentSkillRefSet.objName, "hero_talent_skill", "伙伴资质技能表（hero_talent_skill）", _judgeTagCanShow));

			//伙伴资质技能等级表
			_regMenuItem(new _TNPAutoExportRefMenu<HeroTalentSkillLevelRefObj, GSOHeroTalentSkillLevelRefSet>(ENPExportSettingEnum.HERO_TALENT_SKILL_LEVEL,
                GSOHeroTalentSkillLevelRefSet.assetPath, GSOHeroTalentSkillLevelRefSet.objName, "hero_talent_skill_level", "伙伴资质技能等级表（hero_talent_skill_level）", _judgeTagCanShow));

			//伙伴经营技能表
			_regMenuItem(new _TNPAutoExportRefMenu<HeroBusinessSkillRefObj, GSOHeroBusinessSkillRefSet>(ENPExportSettingEnum.HERO_BUSINESS_SKILL,
                GSOHeroBusinessSkillRefSet.assetPath, GSOHeroBusinessSkillRefSet.objName, "hero_business_skill", "伙伴经营技能表（hero_business_skill）", _judgeTagCanShow));

			//伙伴经营技能升级消耗组表
			_regMenuItem(new _TNPAutoExportRefMenu<HeroBusinessSkillUpgradeRefObj, GSOHeroBusinessSkillUpgradeRefSet>(ENPExportSettingEnum.HERO_BUSINESS_SKILL_UPGRADE,
                GSOHeroBusinessSkillUpgradeRefSet.assetPath, GSOHeroBusinessSkillUpgradeRefSet.objName, "hero_business_skill_upgrade", "伙伴经营技能升级消耗组表（hero_business_skill_upgrade）", _judgeTagCanShow));

			//伙伴光环表
			_regMenuItem(new _TNPAutoExportRefMenu<HeroHaloRefObj, GSOHeroHaloRefSet>(ENPExportSettingEnum.HERO_HALO,
                GSOHeroHaloRefSet.assetPath, GSOHeroHaloRefSet.objName, "hero_halo", "伙伴光环表（hero_halo）", _judgeTagCanShow));

			//伙伴光环等级表
			_regMenuItem(new _TNPAutoExportRefMenu<HeroHaloLevelRefObj, GSOHeroHaloLevelRefSet>(ENPExportSettingEnum.HERO_HALO_LEVEL,
                GSOHeroHaloLevelRefSet.assetPath, GSOHeroHaloLevelRefSet.objName, "hero_halo_level", "伙伴光环等级表（hero_halo_level）", _judgeTagCanShow));

			//伙伴套系表
			_regMenuItem(new _TNPAutoExportRefMenu<HeroSuitRefObj, GSOHeroSuitRefSet>(ENPExportSettingEnum.HERO_HALO_SUIT,
                GSOHeroSuitRefSet.assetPath, GSOHeroSuitRefSet.objName, "hero_halo_suit", "伙伴套系表（hero_halo_suit）", _judgeTagCanShow));

			//伙伴套系技能表
			_regMenuItem(new _TNPAutoExportRefMenu<HeroSuitSkillRefObj, GSOHeroSuitSkillRefSet>(ENPExportSettingEnum.HERO_HALO_SUIT_SKILL,
                GSOHeroSuitSkillRefSet.assetPath, GSOHeroSuitSkillRefSet.objName, "hero_halo_suit_skill", "伙伴套系技能表（hero_halo_suit_skill）", _judgeTagCanShow));

			//伙伴套系技能等级表
			_regMenuItem(new _TNPAutoExportRefMenu<HeroSuitSkillLevelRefObj, GSOHeroSuitSkillLevelRefSet>(ENPExportSettingEnum.HERO_HALO_SUIT_SKILL_LEVEL,
                GSOHeroSuitSkillLevelRefSet.assetPath, GSOHeroSuitSkillLevelRefSet.objName, "hero_halo_suit_skill_level", "伙伴套系技能等级表（hero_halo_suit_skill_level）", _judgeTagCanShow));

			//伙伴配音组表
			_regMenuItem(new _TNPAutoExportRefMenu<HeroVoiceGroupRefObj, GSOHeroVoiceGroupRefSet>(ENPExportSettingEnum.HERO_VOICE_GROUP,
				GSOHeroVoiceGroupRefSet.assetPath, GSOHeroVoiceGroupRefSet.objName, "hero_voice_group", "大臣配音组表（hero_voice_group）", _judgeTagCanShow));

			#endregion

			#region 粒子相关表

			_regMenuItem(new _TNPAutoExportRefMenu<NPParticleRefObj, NPGSOParticleRefSet>
	        (ENPExportSettingEnum.PARTICLE, NPGSOParticleRefSet.assetPath, NPGSOParticleRefSet.objName
	            , "particle", "粒子表 (particle)", _judgeTagCanShow));

        #endregion

			#region 活动

			//活动主表
			_regMenuItem(
				new _TNPAutoExportRefMenu<GActivityMainRefObj, GSOActivityMainRefSet>
				(ENPExportSettingEnum.ACTIVITY_MAIN, GSOActivityMainRefSet.assetPath, GSOActivityMainRefSet.objName
					, "activity_main", "activity_main 活动主表", _judgeTagCanShow));

			//活动事件表
			_regMenuItem(
				new NPOnlyServerRefExportMenu("activity_event",ENPExportSettingEnum.ACTIVITY_EVENT,
					"activity_event 活动事件表"
					, _judgeTagCanShow));

			//活动阶段奖励表
			_regMenuItem(
				new _TNPAutoExportRefMenu<GActivityStepRewardRefObj, GSOActivityStepRewardRefSet>
				(ENPExportSettingEnum.ACTIVITY_STEP_REWARD_STEP, GSOActivityStepRewardRefSet.assetPath, GSOActivityStepRewardRefSet.objName
					, "activity_step_reward", "activity_step_reward 活动阶段奖励表", _judgeTagCanShow));

			//活动排行奖励表
			_regMenuItem(
				new _TNPAutoExportRefMenu<GActivityRankRewardRefObj, GSOActivityRankRewardRefSet>
				(ENPExportSettingEnum.ACTIVITY_RANK_REWARD, GSOActivityRankRewardRefSet.assetPath, GSOActivityRankRewardRefSet.objName
					, "activity_rank_reward", "activity_rank_reward 活动排行奖励表", _judgeTagCanShow));

			//活动计划表
            _regMenuItem(new NPOnlyServerRefExportMenu("activity_plan", ENPExportSettingEnum.ACTIVITY_PLAN, "活动计划表（activity_plan）", _judgeTagCanShow));

            #endregion

            #region 阶段奖励

            //阶段奖励设置表
            _regMenuItem(new _TNPAutoExportRefMenu<ActivityStepRewardSetRefObj, GSOActivityStepRewardSetRefSet>
            (ENPExportSettingEnum.STEP_REWARD_SET, GSOActivityStepRewardSetRefSet.assetPath, GSOActivityStepRewardSetRefSet.objName
	            , "step_reward_set", "阶段奖励设置表（step_reward_set）", _judgeTagCanShow));
			
			//阶段奖励设置事件任务表
            _regMenuItem(new _TNPAutoExportRefMenu<ActivityStepRewardSetEventTaskRefObj, GSOActivityStepRewardSetEventTaskRefSet>
            (ENPExportSettingEnum.STEP_REWARD_SET_EVENT_TASK, GSOActivityStepRewardSetEventTaskRefSet.assetPath, GSOActivityStepRewardSetEventTaskRefSet.objName
	            , "step_reward_set_event_task", "阶段奖励设置事件任务表（step_reward_set_event_task）", _judgeTagCanShow));
			
			#endregion
			
			//通用属性表
            _regMenuItem(new _TNPAutoExportRefMenu<BasicAttrRefObj, GSOBasicAttrRefSet>(ENPExportSettingEnum.BASIC_ATTR, 
                GSOBasicAttrRefSet.assetPath, GSOBasicAttrRefSet.objName, "basic_attr", "通用属性表（basic_attr）", _judgeTagCanShow));

            //价格递增表
	        _regMenuItem(
	            new _TNPAutoExportRefMenu<TimesPriceRefObj, NPGSOTimesPriceRefSet>
	            (ENPExportSettingEnum.TIMES_PRICE, NPGSOTimesPriceRefSet.assetPath, NPGSOTimesPriceRefSet.objName
	                , "times_price", "times_price 价格递增表", _judgeTagCanShow));

	        //UI资源路径表
	        _regMenuItem(
	            new _TNPAutoExportSQLiteMenuItem<NPUIResPathRefObj>
	            (ENPExportSettingEnum.UI_RES_PATH, NPUIResPathRefObj.objName, NPUIResPathRefObj.tableName, NPUIResPathRefObj.assetPath
	                , "ui_res_path", "UI资源路径表（ui_res_path）", _judgeTagCanShow));

	        _regMenuItem(new _TNPAutoExportRefUniformMenu<ItemDefRefObj, GSOItemDefRefSet>(ENPExportSettingEnum.ITEM_DEF,
	            GSOItemDefRefSet.assetPath, GSOItemDefRefSet.objName, "item_def", "item_def 表", NPEnum.ENPItemType.ITEM_DEF, _judgeTagCanShow));

	        //对应的编码段的字符长度
            _regMenuItem(new NPUnicodeLengthCheckExportMenu("unicode_length_check", _judgeTagCanShow));

	        //上浮提示表
	        _regMenuItem(new _TNPAutoExportRefMenu<NPCenterTipsRefObj, NPGSOCenterTipsRefSet>
	        (ENPExportSettingEnum.CENTER_TIPS, NPGSOCenterTipsRefSet.assetPath, NPGSOCenterTipsRefSet.objName
	            , "center_tips", "上浮提示表 (center_tips)", _judgeTagCanShow));

	        //LAZY_CD主表
	        _regMenuItem(new _TNPAutoExportRefUniformMenu<NPLazyCDRefObj, NPSOLazyCDRefSet>(ENPExportSettingEnum.LAZY_CD,
	            NPSOLazyCDRefSet.assetPath, NPSOLazyCDRefSet.objName, "player_lazy_cd", "player_lazy_cd cd表", NPEnum.ENPItemType.LAZY_CD, _judgeTagCanShow));

	        //图标提示工具类
	        _regMenuItem(new _TNPAutoExportRefUniformMenu<NPToolTipItemRefObj, NPSOToolTipItemRefSet>(ENPExportSettingEnum.TOOL_TIP_ITEM,
	            NPSOToolTipItemRefSet.assetPath, NPSOToolTipItemRefSet.objName, "tooltip_item", "tooltip_item 图标提示工具", NPEnum.ENPItemType.TOOL_TIP_ITEM, _judgeTagCanShow));

	        //队列上浮提示时间表
	        _regMenuItem(new _TNPAutoExportRefMenu<QueueDealerTipsRefObj, GSOQueueDealerTipsRefSet>
	        (ENPExportSettingEnum.QUEUE_DEALER_TIPS, GSOQueueDealerTipsRefSet.assetPath, GSOQueueDealerTipsRefSet.objName
	            , "queue_dealer_tips", "队列处理提示表 (queue_dealer_tips)", _judgeTagCanShow));
            
	        //获取途径表
	        _regMenuItem(new _TNPAutoExportRefMenu<NPAccessRefObj, NPGSOAccessRefSet>
	            (ENPExportSettingEnum.ACCESS, NPGSOAccessRefSet.assetPath, NPGSOAccessRefSet.objName
	            , "access", "获取途径表（access）", _judgeTagCanShow));

	        //入口点表
	        _regMenuItem(new _TNPAutoExportRefMenu<EntryPointRefObj, GSOEntryPointRefSet>
	        (ENPExportSettingEnum.ENTRY_POINT, GSOEntryPointRefSet.assetPath, GSOEntryPointRefSet.objName
		        , "entry_point", "入口点表（entry_point）", _judgeTagCanShow));
	        
	        //界面跳转类型和系统功能的映射表
	        _regMenuItem(new _TNPAutoExportRefMenu<NPMainNodeToFunctionTypeRefObj, NPGSOMainNodeToFunctionTypeRefSet>
	            (ENPExportSettingEnum.MAIN_NODE_TO_FUNCTION_TYPE, NPGSOMainNodeToFunctionTypeRefSet.assetPath, NPGSOMainNodeToFunctionTypeRefSet.objName
	            , "main_node_to_function_type", "界面跳转类型和系统功能的映射表（main_node_to_function_type）", _judgeTagCanShow));

	        //功能解锁表
	        _regMenuItem(new _TNPAutoExportRefMenu<FunctionUnlockRefObj, GSOFunctionUnlockRefSet>
	            (ENPExportSettingEnum.FUNCTION_UNLOCK, GSOFunctionUnlockRefSet.assetPath, GSOFunctionUnlockRefSet.objName
	            , "func_unlock", "功能解锁表（func_unlock）", _judgeTagCanShow));

	        //语言翻译替换表
	        _regMenuItem(
	            new _TNPAutoExportSQLiteMenuItem<NPLanguageArgsRefObj>
	            (ENPExportSettingEnum.LAN_ARGS, NPLanguageArgsRefObj.objName, NPLanguageArgsRefObj.tableName, NPLanguageArgsRefObj.assetPath
	                , "lan_args", "语言翻译替换表（lan_args）", _judgeTagCanShow));

			//固定时间恢复的CD表
			_regMenuItem(new _TNPAutoExportRefUniformMenu<NPFixedCDRefObj, NPGSOFixedCDRefSet>(ENPExportSettingEnum.FIXED_CD,
				NPGSOFixedCDRefSet.assetPath, NPGSOFixedCDRefSet.objName, "player_fixed_cd", "固定时间恢复的CD表（player_fixed_cd）", NPEnum.ENPItemType.FIXED_CD, _judgeTagCanShow));
            
        
     

			#region 对话

			//对话主表
			_regMenuItem(new _TNPAutoExportRefMenu<NPDialogueRefObj, NPGSODialogueRefSet>
				(ENPExportSettingEnum.DIALOGUE, NPGSODialogueRefSet.assetPath, NPGSODialogueRefSet.objName
				, "dialogue", "对话主表（dialogue）", _judgeTagCanShow));

			//对话句子表
			_regMenuItem(
				new _TNPAutoExportSQLiteMenuItem<NPDialogueSentenceRefObj>
				(ENPExportSettingEnum.DIALOGUE_SENTENCE, NPDialogueSentenceRefObj.objName, NPDialogueSentenceRefObj.tableName, NPDialogueSentenceRefObj.assetPath
					, "dialogue_sentence", "对话句子表（dialogue_sentence）", _judgeTagCanShow));
			//对话主表
			_regMenuItem(new _TNPAutoExportRefMenu<NPDialogueResponseOptionRefObj, NPGSODialogueResponseOptionRefSet>
			(ENPExportSettingEnum.DIALOGUE_RESPONESE_OPTION, NPGSODialogueResponseOptionRefSet.assetPath, NPGSODialogueResponseOptionRefSet.objName
				, "response_option", "对话选项（response_option）", _judgeTagCanShow));

            #endregion

            #region 商店相关表
            _regMenuItem(new _TNPAutoExportRefMenu<NPShopRefObj, NPSOShopRefSet>
				 (ENPExportSettingEnum.SHOP, NPSOShopRefSet.assetPath, NPSOShopRefSet.objName
				 , "shop", "Shop 商店表", _judgeTagCanShow));

            _regMenuItem(new _TNPAutoExportRefMenu<NPShopItemRefObj, NPSOShopItemRefSet>
            (ENPExportSettingEnum.SHOP_ITEM, NPSOShopItemRefSet.assetPath, NPSOShopItemRefSet.objName
	            , "shop_item", "Shop Item 商店商品表", _judgeTagCanShow));

            _regMenuItem(new _TNPAutoExportRefMenu<NPShopItemGroupRefObj, NPSOShopItemGroupRefSet>
            (ENPExportSettingEnum.SHOP_ITEM_GROUP, NPSOShopItemGroupRefSet.assetPath, NPSOShopItemGroupRefSet.objName
	            , "shop_item_group", "shop_item_group 商店商品组表", _judgeTagCanShow));

            _regMenuItem(new _TNPAutoExportRefMenu<NPShopItemDiscountRefObj, NPSOShopItemDiscountRefSet>
				(ENPExportSettingEnum.SHOP_ITEM_DISCOUNT, NPSOShopItemDiscountRefSet.assetPath, NPSOShopItemDiscountRefSet.objName
                , "shop_item_discount", "Shop Item Discount 商品打折表", _judgeTagCanShow));

			#endregion

			#region  游历表

			_regMenuItem(new _TNPAutoExportRefMenu<TravelPosRefObj, GSOTravelPosRefSet>
			(ENPExportSettingEnum.TRAVEL_POS, GSOTravelPosRefSet.assetPath, GSOTravelPosRefSet.objName
				, "travel_pos", "travel_pos 游历地点表", _judgeTagCanShow));

			//游历妃子表
			_regMenuItem(new _TNPAutoExportRefMenu<TravelConsortRefObj, GSOTravelConsortRefSet>
			(ENPExportSettingEnum.TRAVEL_CONSORT, GSOTravelConsortRefSet.assetPath, GSOTravelConsortRefSet.objName
				, "travel_consort", "游历妃子表（travel_consort）", _judgeTagCanShow));

			//游历事件类型表
			_regMenuItem(new _TNPAutoExportRefMenu<TravelEventTypeRefObj, GSOTravelEventTypeRefSet>
			(ENPExportSettingEnum.TRAVEL_EVENT_TYPE, GSOTravelEventTypeRefSet.assetPath, GSOTravelEventTypeRefSet.objName
				, "travel_event_type", "游历事件类型表（travel_event_type）", _judgeTagCanShow));

			//游历事件表
			_regMenuItem(new _TNPAutoExportSQLiteMenuItem<TravelEventRefObj>
			(ENPExportSettingEnum.TRAVEL_EVENT, TravelEventRefObj.objName, TravelEventRefObj.tableName, TravelEventRefObj.assetPath
			, "travel_event", "游历事件表（travel_event）", _judgeTagCanShow));

			//游历单次事件表
			_regMenuItem(new _TNPAutoExportRefMenu<TravelEventOnceRefObj, GSOTravelEventOnceRefSet>
			(ENPExportSettingEnum.TRAVEL_EVENT_ONCE, GSOTravelEventOnceRefSet.assetPath, GSOTravelEventOnceRefSet.objName
				, "travel_event_once", "游历单次事件表（travel_event_once）", _judgeTagCanShow));

			//游历妃子好感度事件表
			_regMenuItem(new _TNPAutoExportRefMenu<TravelEventConsortLikeRefObj, GSOTravelEventConsortLikeRefSet>
			(ENPExportSettingEnum.TRAVEL_EVENT_CONSORT_LIKE, GSOTravelEventConsortLikeRefSet.assetPath, GSOTravelEventConsortLikeRefSet.objName
				, "travel_event_consort_like", "游历妃子好感度事件表（travel_event_consort_like）", _judgeTagCanShow));

			//游历妃子亲密度事件表
			_regMenuItem(new _TNPAutoExportRefMenu<TravelEventConsortIntimacyRefObj, GSOTravelEventConsortIntimacyRefSet>
			(ENPExportSettingEnum.TRAVEL_EVENT_CONSORT_INTIMACY, GSOTravelEventConsortIntimacyRefSet.assetPath, GSOTravelEventConsortIntimacyRefSet.objName
				, "travel_event_consort_intimacy", "游历妃子亲密度事件表（travel_event_consort_intimacy）", _judgeTagCanShow));

			//游历妃子酒馆事件表
			_regMenuItem(new _TNPAutoExportRefMenu<TravelEventConsortBarRefObj, GSOTravelEventConsortBarRefSet>
			(ENPExportSettingEnum.TRAVEL_EVENT_CONSORT_BAR, GSOTravelEventConsortBarRefSet.assetPath, GSOTravelEventConsortBarRefSet.objName
				, "travel_event_consort_bar", "游历妃子酒馆事件表（travel_event_consort_bar）", _judgeTagCanShow));

			//游历妃子酒馆事件消耗表
			_regMenuItem(new _TNPAutoExportRefMenu<TravelEventConsortBarCostRefObj, GSOTravelEventConsortBarCostRefSet>
			(ENPExportSettingEnum.TRAVEL_EVENT_CONSORT_BAR_COST, GSOTravelEventConsortBarCostRefSet.assetPath, GSOTravelEventConsortBarCostRefSet.objName
				, "travel_event_consort_bar_cost", "游历妃子酒馆事件消耗表（travel_event_consort_bar_cost）", _judgeTagCanShow));

			//游历交换事件表
			_regMenuItem(new _TNPAutoExportRefMenu<TravelEventChangeRefObj, GSOTravelEventChangeRefSet>
			(ENPExportSettingEnum.TRAVEL_EVENT_CHANGE, GSOTravelEventChangeRefSet.assetPath, GSOTravelEventChangeRefSet.objName
				, "travel_event_change", "游历交换事件表（travel_event_change）", _judgeTagCanShow));

			//游历邀约事件表
			_regMenuItem(new _TNPAutoExportRefMenu<TravelEventInvitationRefObj, GSOTravelEventInvitationRefSet>
			(ENPExportSettingEnum.TRAVEL_EVENT_INVITATION, GSOTravelEventInvitationRefSet.assetPath, GSOTravelEventInvitationRefSet.objName
				, "travel_event_invitation", "游历邀约事件表（travel_event_invitation）", _judgeTagCanShow));

			//游历卷王事件表
			_regMenuItem(new _TNPAutoExportRefMenu<TravelEventGiftedRefObj, GSOTravelEventGiftedRefSet>
			(ENPExportSettingEnum.TRAVEL_EVENT_GIFTDE, GSOTravelEventGiftedRefSet.assetPath, GSOTravelEventGiftedRefSet.objName
				, "travel_event_giftde", "游历卷王事件表（travel_event_giftde）", _judgeTagCanShow));

			//游历博彩事件表
			_regMenuItem(new _TNPAutoExportRefMenu<TravelEventGambleRefObj, GSOTravelEventGambleRefSet>
			(ENPExportSettingEnum.TRAVEL_EVENT_GAMBLE, GSOTravelEventGambleRefSet.assetPath, GSOTravelEventGambleRefSet.objName
				, "travel_event_gamble", "游历博彩事件表（travel_event_gamble）", _judgeTagCanShow));

			//游历大臣实力事件表
			_regMenuItem(new _TNPAutoExportRefMenu<TravelEventAddPowerRefObj, GSOTravelEventAddPowerRefSet>
			(ENPExportSettingEnum.TRAVEL_EVENT_ADD_POWER, GSOTravelEventAddPowerRefSet.assetPath, GSOTravelEventAddPowerRefSet.objName
				, "travel_event_add_power", "游历大臣实力事件表（travel_event_add_power）", _judgeTagCanShow));

			//游历事件一键表
			_regMenuItem(new _TNPAutoExportRefMenu<TravelEventAkeyRefObj, GSOTravelEventAkeyRefSet>
			(ENPExportSettingEnum.TRAVEL_EVENT_AKEY, GSOTravelEventAkeyRefSet.assetPath, GSOTravelEventAkeyRefSet.objName
				, "travel_event_akey", "游历事件一键表（travel_event_akey）", _judgeTagCanShow));
			
			#endregion

			#region NPC相关表

			//NPC主表
			_regMenuItem(
			new _TNPAutoExportSQLiteMenuItem<NPNPCRefObj>
				(ENPExportSettingEnum.NPC, NPNPCRefObj.objName, NPNPCRefObj.tableName, NPNPCRefObj.assetPath
					, "npc", "NPC表（npc）", _judgeTagCanShow));

			//NPC-Actor类型子表
			_regMenuItem(
			new _TNPAutoExportSQLiteMenuItem<NPNPCActorRefObj>
				(ENPExportSettingEnum.NPC_ACTOR, NPNPCActorRefObj.objName, NPNPCActorRefObj.tableName, NPNPCActorRefObj.assetPath
					, "npc_actor", "NPC-Actor类型子表（npc_actor）", _judgeTagCanShow));

			//NPC-独立模型子表
			_regMenuItem(
			new _TNPAutoExportSQLiteMenuItem<NPNPCGoRefObj>
				(ENPExportSettingEnum.NPC_GO, NPNPCGoRefObj.objName, NPNPCGoRefObj.tableName, NPNPCGoRefObj.assetPath
					, "npc_go", "NPC-独立模型子表（npc_go）", _judgeTagCanShow));

			#endregion

			//玩家起名表
			_regMenuItem(new NPPlayerNameRefExportMenu("player_name", _judgeTagCanShow));

            //子嗣起名表
            _regMenuItem(new ChildNameRefExportMenu("child_name", _judgeTagCanShow));

            //玩家形象表
            _regMenuItem(new _TNPAutoExportRefMenu<NPPlayerPrefabRefObj, NPSOPlayerPrefabRefSet>
				(ENPExportSettingEnum.PLAYER_PREFAB, NPSOPlayerPrefabRefSet.assetPath, NPSOPlayerPrefabRefSet.objName
				   , "player_prefab", "默认形象表（player_prefab）", _judgeTagCanShow));

            //红点表
            _regMenuItem(new _TNPAutoExportRefMenu<NPRedTipRefObj, NPGSORedTipRefSet>
			(ENPExportSettingEnum.RED, NPGSORedTipRefSet.assetPath, NPGSORedTipRefSet.objName
				, "red", "红点表（red）", _judgeTagCanShow));

            //showcase模型额外表现表
            _regMenuItem(new _TNPAutoExportRefMenu<NPShowCaseActorBehaviorRefObj, NPGSOShowCaseActorBehaviorRefSet>
            (ENPExportSettingEnum.SHOW_CASE_ACTOR_BEHAVIOR, NPGSOShowCaseActorBehaviorRefSet.assetPath, NPGSOShowCaseActorBehaviorRefSet.objName
	            , "show_case_actor_behavior", "showcase模型额外表现表（show_case_actor_behavior）", _judgeTagCanShow));
            
			//动画附加物表
			_regMenuItem(new _TNPAutoExportRefMenu<AttachmentItemRefObj, GSOAttachmentItemRefSet>
			(ENPExportSettingEnum.ATTACHMENT_ITEM, GSOAttachmentItemRefSet.assetPath, GSOAttachmentItemRefSet.objName
				, "attachment_item", "附加物表（attachment_item）", _judgeTagCanShow));

			//登入大区表
			_regMenuItem(new _TNPAutoExportRefMenu<NPLoginAreaRefObj, NPGSOLoginAreaRefSet>
			(ENPExportSettingEnum.LOGIN_AREA, NPGSOLoginAreaRefSet.assetPath, NPGSOLoginAreaRefSet.objName
				, "login_area", "login_area 登入大区表", _judgeTagCanShow));
            
            #region 通用分享

            //share表
            _regMenuItem(new _TNPAutoExportRefMenu<NPSOShareRefObj, NPSOShareRefSet>
            (ENPExportSettingEnum.SHARE, NPSOShareRefSet.assetPath, NPSOShareRefSet.objName
                , "share", "share 通用分享表", _judgeTagCanShow));

            //通用宝箱表
            _regMenuItem(new _TNPAutoExportRefMenu<NPSOCommonBoxRefObj, NPSOCommonBoxRefSet>
            (ENPExportSettingEnum.COMMON_BOX, NPSOCommonBoxRefSet.assetPath, NPSOCommonBoxRefSet.objName
                , "box_comm", "通用宝箱表（box_comm）", _judgeTagCanShow));

			#endregion
			
                
		    //goto效果表
			_regMenuItem(new _TNPAutoExportRefMenu<NPSOEffectGotoRefObj, NPSOEffectGotoRefSet>
            (ENPExportSettingEnum.EFFECT_GOTO, NPSOEffectGotoRefSet.assetPath, NPSOEffectGotoRefSet.objName
                , "effect_goto", "GoTo效果表（effect_goto）", _judgeTagCanShow));

			//本地推送表
            _regMenuItem(new _TNPAutoExportRefMenu<LocalPushRefObj, GSOLocalPushRefSet>
            (ENPExportSettingEnum.LOCAL_PUSH, GSOLocalPushRefSet.assetPath, GSOLocalPushRefSet.objName
                , "local_push", "本地推送表（local_push）", _judgeTagCanShow));

			//主城推送弹窗表
			_regMenuItem(new _TNPAutoExportRefMenu<MainCityPushNoticeRefObj, GSOMainCityPushNoticeRefSet>
			(ENPExportSettingEnum.MAIN_CITY_PUSH_NOTICE, GSOMainCityPushNoticeRefSet.assetPath, GSOMainCityPushNoticeRefSet.objName
				, "main_city_push_notice", "主城推送弹窗表（main_city_push_notice）", _judgeTagCanShow));
			
			//活动合并展示推送表
			_regMenuItem(new _TNPAutoExportRefMenu<PushNoticeActivityMergeRefObj, GSOPushNoticeActivityMergeRefSet>
			(ENPExportSettingEnum.PUSH_NOTICE_ACTIVITY_MERGE, GSOPushNoticeActivityMergeRefSet.assetPath, GSOPushNoticeActivityMergeRefSet.objName
				, "push_notice_activity_merge", "活动合并展示推送表（push_notice_activity_merge）", _judgeTagCanShow));

			#region 批量战斗相关表

			_regMenuItem(new NPOnlyServerRefExportMenu("batch_mission", ENPExportSettingEnum.BATCH_MISSION, "批量战斗关卡表（batch_mission）", _judgeTagCanShow));
			_regMenuItem(new NPOnlyServerRefExportMenu("batch_pvp", ENPExportSettingEnum.BATCH_PVP, "批量战斗PVP表（batch_pvp）", _judgeTagCanShow));
			_regMenuItem(new NPOnlyServerRefExportMenu("batch_pet_group", ENPExportSettingEnum.BATCH_PET_GROUP, "批量战斗宠物组表（batch_pet_group）", _judgeTagCanShow));
			_regMenuItem(new NPOnlyServerRefExportMenu("batch_pet_property", ENPExportSettingEnum.BATCH_PET_PROPERTY, "批量战斗宠物属性表（batch_pet_property）", _judgeTagCanShow));
			_regMenuItem(new NPOnlyServerRefExportMenu("batch_pet_template", ENPExportSettingEnum.BATCH_PET_TEMPLATE, "批量战斗宠物技能表（batch_pet_template）", _judgeTagCanShow));

            #endregion

			#region 情人相关
			
			_regMenuItem(new _TNPAutoExportRefUniformMenu<GConsortRefObj,GSOConsortRefSet>(ENPExportSettingEnum.CONSORT,
				GSOConsortRefSet.assetPath, GSOConsortRefSet.objName, "consort", "情人表 (consort)", NPEnum.ENPItemType.CONSORT, _judgeTagCanShow));
			
			_regMenuItem(new _TNPAutoExportRefMenu<ConsortStoryRefObj, GSOConsortStoryRefSet>
				(ENPExportSettingEnum.CONSORT_STORY, GSOConsortStoryRefSet.assetPath, GSOConsortStoryRefSet.objName
					, "consort_story", "妃子故事表 consort_story", _judgeTagCanShow));

			_regMenuItem(new _TNPAutoExportRefMenu<ConsortStoryBgRefObj, GSOConsortStoryBgRefSet>
				(ENPExportSettingEnum.CONSORT_STORY_BG, GSOConsortStoryBgRefSet.assetPath, GSOConsortStoryBgRefSet.objName
					, "consort_story_bg", "妃子故事背景表 consort_story_bg", _judgeTagCanShow));
			
			_regMenuItem(new _TNPAutoExportRefMenu<ConsortFettersLvlRefObj, GSOConsortFettersLvlRefSet>
				(ENPExportSettingEnum.CONSORT_FETTERS_LVL, GSOConsortFettersLvlRefSet.assetPath, GSOConsortFettersLvlRefSet.objName
					, "consort_fetters_lvl", "妃子羁绊等级表 consort_fetters_lvl", _judgeTagCanShow));
			
			_regMenuItem(new _TNPAutoExportRefMenu<ConsortFettersSkillRefObj, GSOConsortFettersSkillRefSet>
			(ENPExportSettingEnum.CONSORT_FETTERS_SKILL, GSOConsortFettersSkillRefSet.assetPath, GSOConsortFettersSkillRefSet.objName
				, "consort_fetters_skill", "家人羁绊技能表 consort_fetters_skill", _judgeTagCanShow));
			
			_regMenuItem(new _TNPAutoExportRefMenu<ConsortFettersSkillLvlRefObj, GSOConsortFettersSkillLvlRefSet>
			(ENPExportSettingEnum.CONSORT_FETTERS_SKILL_LVL, GSOConsortFettersSkillLvlRefSet.assetPath, GSOConsortFettersSkillLvlRefSet.objName
				, "consort_fetters_skill_lvl", "家人羁绊技能等级 consort_fetters_skill_lvl", _judgeTagCanShow));
			
			_regMenuItem(new _TNPAutoExportRefMenu<ConsortBusinessSkillRefObj, GSOConsortBusinessSkillRefSet>
			(ENPExportSettingEnum.CONSORT_BUSINESS_SKILL, GSOConsortBusinessSkillRefSet.assetPath, GSOConsortBusinessSkillRefSet.objName
				, "consort_business_skill", "家人经营技能 consort_business_skill", _judgeTagCanShow));
			
			_regMenuItem(new _TNPAutoExportRefMenu<ConsortBusinessPotentialRefObj, GSOConsortBusinessPotentialRefSet>
			(ENPExportSettingEnum.CONSORT_BUSINESS_POTENTIAL, GSOConsortBusinessPotentialRefSet.assetPath, GSOConsortBusinessPotentialRefSet.objName
				, "consort_business_potential", "家人经营潜力 consort_business_potential", _judgeTagCanShow));
			
			_regMenuItem(new _TNPAutoExportRefMenu<ConsortBlessSkillRefObj, GSOConsortBlessSkillRefSet>
			(ENPExportSettingEnum.CONSORT_BLESS_SKILL, GSOConsortBlessSkillRefSet.assetPath, GSOConsortBlessSkillRefSet.objName
				, "consort_bless_skill", "家人加护技能 consort_bless_skill", _judgeTagCanShow));
			
			_regMenuItem(new _TNPAutoExportRefMenu<ConsortBlessSkillLvlRefObj, GSOConsortBlessSkillLvlRefSet>
			(ENPExportSettingEnum.CONSORT_BLESS_SKILL_LVL, GSOConsortBlessSkillLvlRefSet.assetPath, GSOConsortBlessSkillLvlRefSet.objName
				, "consort_bless_skill_lvl", "家人加护技能等级 consort_bless_skill_lvl", _judgeTagCanShow));
			
			_regMenuItem(new _TNPAutoExportRefUniformMenu<GConsortSkinRefObj,GSOConsortSkinRefSet>(ENPExportSettingEnum.CONSORT_SKIN,
				GSOConsortSkinRefSet.assetPath, GSOConsortSkinRefSet.objName, "consort_skin", "情人皮肤表 (consort_skin)", NPEnum.ENPItemType.CONSORT_SKIN, _judgeTagCanShow));

			_regMenuItem(new _TNPAutoExportRefMenu<GConsortSkinLvlRefObj, GSOConsortSkinLvlRefSet>
			(ENPExportSettingEnum.CONSORT_SKIN_LVL, GSOConsortSkinLvlRefSet.assetPath, GSOConsortSkinLvlRefSet.objName
				, "consort_skin_lvl", "情人皮肤等级表（consort_skin_lvl）", _judgeTagCanShow));
			
			_regMenuItem(new _TNPAutoExportRefMenu<ConsortTravelRefObj, GSOConsortTravelRefSet>
			(ENPExportSettingEnum.CONSORT_TRAVEL, GSOConsortTravelRefSet.assetPath, GSOConsortTravelRefSet.objName
				, "consort_travel", "妃子旅游表（consort_travel）", _judgeTagCanShow));
			
			_regMenuItem(new _TNPAutoExportRefMenu<ConsortHaloLvlRefObj, GSOConsortHaloLvlRefSet>
			(ENPExportSettingEnum.CONSORT_HALO_LVL, GSOConsortHaloLvlRefSet.assetPath, GSOConsortHaloLvlRefSet.objName
				, "consort_halo_lvl", "妃子星辉等级表（consort_halo_lvl）", _judgeTagCanShow));
			
			_regMenuItem(new _TNPAutoExportRefMenu<ConsortHaloSkillRefObj, GSOConsortHaloSkillRefSet>
			(ENPExportSettingEnum.CONSORT_HALO_SKILL, GSOConsortHaloSkillRefSet.assetPath, GSOConsortHaloSkillRefSet.objName
				, "consort_halo_skill", "妃子星辉技能表（consort_halo_skill）", _judgeTagCanShow));
			
			_regMenuItem(new _TNPAutoExportRefMenu<ConsortHaloSkillLvlRefObj, GSOConsortHaloSkillLvlRefSet>
			(ENPExportSettingEnum.CONSORT_HALO_SKILL_LVL, GSOConsortHaloSkillLvlRefSet.assetPath, GSOConsortHaloSkillLvlRefSet.objName
				, "consort_halo_skill_lvl", "妃子星辉技能等级表（consort_halo_skill_lvl）", _judgeTagCanShow));
			
			_regMenuItem(new _TNPAutoExportRefMenu<ConsortVoiceGroupRefObj, GSOConsortVoiceGroupRefSet>
				(ENPExportSettingEnum.CONSORT_VOICE_GROUP, GSOConsortVoiceGroupRefSet.assetPath, GSOConsortVoiceGroupRefSet.objName, 
					"consort_voice_group", "妃子配音组表（consort_voice_group）", _judgeTagCanShow));

			_regMenuItem(new _TNPAutoExportRefUniformMenu<ConsortCGRefObj,GSOConsortCGRefSet>
				(ENPExportSettingEnum.CONSORT_CG, GSOConsortCGRefSet.assetPath, GSOConsortCGRefSet.objName,
					"consort_cg", "妃子cg表 (consort_cg)", NPEnum.ENPItemType.CONSORT_CG, _judgeTagCanShow));

			//妃子ai角色码表
			_regMenuItem(new NPOnlyServerRefExportMenu("consort_chat_ai_code", ENPExportSettingEnum.CONSORT_CHAT_AI_CODE,
				"妃子ai角色码表（consort_chat_ai_code）", _judgeTagCanShow));
			#endregion
            
			// 加成概率表导出
			_regMenuItem(new ProAddExportMenu("pro_add", _judgeTagCanShow));
			// 操作消耗表导出
			_regMenuItem(new OpCostExportMenu("op_cost", _judgeTagCanShow));
			
            #region 子嗣相关
            _regMenuItem(new _TNPAutoExportRefMenu<ChildAttrRefObj, GSOChildAttrRefSet>
            (ENPExportSettingEnum.CHILD_ATTR, GSOChildAttrRefSet.assetPath, GSOChildAttrRefSet.objName
	            , "child_attr", "子嗣相性表（child_attr）", _judgeTagCanShow));
            _regMenuItem(new _TNPAutoExportRefMenu<ChildCareerRefObj, GSOChildCareerRefSet>
            (ENPExportSettingEnum.CHILD_CAREER, GSOChildCareerRefSet.assetPath, GSOChildCareerRefSet.objName
	            , "child_career", "子嗣职业表（child_career）", _judgeTagCanShow));
            _regMenuItem(new _TNPAutoExportRefMenu<ChildInitResRefObj, GSOChildInitResRefSet>
            (ENPExportSettingEnum.CHILD_INIT_RES, GSOChildInitResRefSet.assetPath, GSOChildInitResRefSet.objName
                , "child_init_res", "子嗣初始资源表（child_init_res）", _judgeTagCanShow));
            _regMenuItem(new _TNPAutoExportRefMenu<ChildQualityRefObj, GSOChildQualityRefSet>
			(ENPExportSettingEnum.CHILD_QUALITY, GSOChildQualityRefSet.assetPath, GSOChildQualityRefSet.objName
                , "child_quality", "子嗣天资表（child_quality）", _judgeTagCanShow));
            _regMenuItem(new _TNPAutoExportRefMenu<ChildResRefObj, GSOChildResRefSet>
			(ENPExportSettingEnum.CHILD_RES, GSOChildResRefSet.assetPath, GSOChildResRefSet.objName
                , "child_res", "子嗣形象资源表（child_res）", _judgeTagCanShow));
            _regMenuItem(new _TNPAutoExportRefMenu<ChildSeatRefObj, GSOChildSeatRefSet>
            (ENPExportSettingEnum.CHILD_SEAT, GSOChildSeatRefSet.assetPath, GSOChildSeatRefSet.objName
	            , "child_seat", "子嗣席位表（child_seat）", _judgeTagCanShow));
            #endregion

            #region 关卡表
            
            _regMenuItem(new _TNPAutoExportSQLiteMenuItem<ChapterRefObj>
            (ENPExportSettingEnum.CHAPTER, ChapterRefObj.objName, ChapterRefObj.tableName, ChapterRefObj.assetPath
	            , "chapter", "关卡表 (chapter)", _judgeTagCanShow));

            _regMenuItem(new _TNPAutoExportRefMenu<ChapterCostRefObj, GSOChapterCostRefSet>
            (ENPExportSettingEnum.CHAPTER_COST, GSOChapterCostRefSet.assetPath, GSOChapterCostRefSet.objName
	            , "chapter_cost", "关卡金币消耗倍率配表（chapter_cost）", _judgeTagCanShow));
            
            _regMenuItem(new _TNPAutoExportRefMenu<ChapterBuildUnlockRefObj, GSOChapterBuildUnlockRefSet>
            (ENPExportSettingEnum.CHAPTER_BUILD_UNLOCK, GSOChapterBuildUnlockRefSet.assetPath, GSOChapterBuildUnlockRefSet.objName
	            , "chapter_build_unlock", "关卡建筑解锁子表（chapter_build_unlock）", _judgeTagCanShow));
            
            _regMenuItem(new _TNPAutoExportRefMenu<ChapterBossStyleRefObj, ChapterBossStyleRefSet>
            (ENPExportSettingEnum.CHAPTER_BOSS_STYLE, ChapterBossStyleRefSet.assetPath, ChapterBossStyleRefSet.objName
	            , "chapter_boss_style", "boss样式表（chapter_boss_style）", _judgeTagCanShow));
            
            _regMenuItem(new _TNPAutoExportRefMenu<ChapterNodeStyleRefObj, ChapterNodeStyleRefSet>
            (ENPExportSettingEnum.CHAPTER_NODE_STYLE, ChapterNodeStyleRefSet.assetPath, ChapterNodeStyleRefSet.objName
	            , "chapter_node_style", "关卡地图样式表（chapter_node_style）", _judgeTagCanShow));
            
			//关卡事件表
			_regMenuItem(new _TNPAutoExportRefMenu<ChapterEventRefObj, GSOChapterEventRefSet>
			(ENPExportSettingEnum.CHAPTER_EVENT, GSOChapterEventRefSet.assetPath, GSOChapterEventRefSet.objName
				, "chapter_event", "关卡事件表（chapter_event）", _judgeTagCanShow));

			//关卡事件奖励表
			_regMenuItem(new _TNPAutoExportRefMenu<ChapterEventRewardRefObj, GSOChapterEventRewardRefSet>
			(ENPExportSettingEnum.CHAPTER_EVENT_REWARD, GSOChapterEventRewardRefSet.assetPath, GSOChapterEventRewardRefSet.objName
				, "chapter_event_reward", "关卡事件奖励表（chapter_event_reward）", _judgeTagCanShow));

			//关卡选择事件表
			_regMenuItem(new _TNPAutoExportRefMenu<ChapterEventChoiceRefObj, GSOChapterEventChoiceRefSet>
			(ENPExportSettingEnum.CHAPTER_EVENT_CHOICE, GSOChapterEventChoiceRefSet.assetPath, GSOChapterEventChoiceRefSet.objName
				, "chapter_event_choice", "关卡选择事件表（chapter_event_choice）", _judgeTagCanShow));

			//关卡选择事件选项表
			_regMenuItem(new _TNPAutoExportRefMenu<ChapterEventChoiceOptionRefObj, GSOChapterEventChoiceOptionRefSet>
			(ENPExportSettingEnum.CHAPTER_EVENT_CHOICE_OPTION, GSOChapterEventChoiceOptionRefSet.assetPath, GSOChapterEventChoiceOptionRefSet.objName
				, "chapter_event_choice_option", "关卡选择事件选项表（chapter_event_choice_option）", _judgeTagCanShow));

			//关卡派遣事件表
			_regMenuItem(new _TNPAutoExportRefMenu<ChapterEventDispatchRefObj, GSOChapterEventDispatchRefSet>
			(ENPExportSettingEnum.CHAPTER_EVENT_DISPATCH, GSOChapterEventDispatchRefSet.assetPath, GSOChapterEventDispatchRefSet.objName
				, "chapter_event_dispatch", "关卡派遣事件表（chapter_event_dispatch）", _judgeTagCanShow));

			//关卡派遣事件展示表
			_regMenuItem(new _TNPAutoExportRefMenu<ChapterEventDispatchShowRefObj, GSOChapterEventDispatchShowRefSet>
			(ENPExportSettingEnum.CHAPTER_EVENT_DISPATCH_SHOW, GSOChapterEventDispatchShowRefSet.assetPath, GSOChapterEventDispatchShowRefSet.objName
				, "chapter_event_dispatch_show", "关卡派遣事件展示表（chapter_event_dispatch_show）", _judgeTagCanShow));

			//关卡派遣事件条件表
			_regMenuItem(new _TNPAutoExportRefMenu<ChapterEventDispatchCondRefObj, GSOChapterEventDispatchCondRefSet>
			(ENPExportSettingEnum.CHAPTER_EVENT_DISPATCH_COND, GSOChapterEventDispatchCondRefSet.assetPath, GSOChapterEventDispatchCondRefSet.objName
				, "chapter_event_dispatch_cond", "关卡派遣事件条件表（chapter_event_dispatch_cond）", _judgeTagCanShow));
			//关卡派遣事件奖励表
			_regMenuItem(new NPOnlyServerRefExportMenu("chapter_event_dispatch_reward", ENPExportSettingEnum.SERVER_EFFECT, "关卡派遣事件奖励表 (chapter_event_dispatch_reward)", _judgeTagCanShow));   //关卡派遣事件奖励表

			//关卡节点表
			_regMenuItem(new _TNPAutoExportRefMenu<ChapterStageRefObj, GSOChapterStageRefSet>
			(ENPExportSettingEnum.CHAPTER_STAGE, GSOChapterStageRefSet.assetPath, GSOChapterStageRefSet.objName
				, "chapter_stage", "关卡 节-Stage 表（chapter_stage）", _judgeTagCanShow));
			
			//关卡节剧情表
			_regMenuItem(new _TNPAutoExportRefMenu<ChapterStagePlotRefObj, GSOChapterStagePlotRefSet>
			(ENPExportSettingEnum.CHAPTER_STAGE_PLOT, GSOChapterStagePlotRefSet.assetPath, GSOChapterStagePlotRefSet.objName
				, "chapter_stage_plot", "关卡节剧情表（chapter_stage_plot）", _judgeTagCanShow));
			
			//关卡故事表
			_regMenuItem(new _TNPAutoExportRefMenu<ChapterStoryRefObj, GSOChapterStoryRefSet>
			(ENPExportSettingEnum.CHAPTER_STORY, GSOChapterStoryRefSet.assetPath, GSOChapterStoryRefSet.objName
				, "chapter_story", "关卡故事表（chapter_story）", _judgeTagCanShow));
            #endregion

            #region 宴会

            _regMenuItem(new _TNPAutoExportRefMenu<GDinnerTypeRefObj, GSODinnerTypeRefSet>
            (ENPExportSettingEnum.DINNER_TYPE, GSODinnerTypeRefSet.assetPath, GSODinnerTypeRefSet.objName
	            , "dinner_type", "宴会类型（dinner_type）", _judgeTagCanShow));
            _regMenuItem(new _TNPAutoExportRefMenu<GDinnerJoinCostRefObj, GSODinnerJoinCostRefSet>
            (ENPExportSettingEnum.DINNER_JOIN_COST, GSODinnerJoinCostRefSet.assetPath, GSODinnerJoinCostRefSet.objName
	            , "dinner_join_cost", "宴会参与消耗（dinner_join_cost）", _judgeTagCanShow));
            //宴会凭证表
            _regMenuItem(new _TNPAutoExportRefMenu<DinnerPermitRefObj, GSODinnerPermitRefSet>
            (ENPExportSettingEnum.DINNER_PERMIT, GSODinnerPermitRefSet.assetPath, GSODinnerPermitRefSet.objName
	            , "dinner_permit", "宴会凭证表（dinner_permit）", _judgeTagCanShow));

            #endregion

            #region 通用事件

            _regMenuItem(new _TNPAutoExportSQLiteMenuItem<CommonEventAwardRefObj>
	            (ENPExportSettingEnum.COMMON_EVENT_AWARD, CommonEventAwardRefObj.objName, CommonEventAwardRefObj.tableName, CommonEventAwardRefObj.assetPath
		            , "common_event_award", "通用事件-奖励事件实例表 (common_event_award)", _judgeTagCanShow));
            
            _regMenuItem(new _TNPAutoExportRefMenu<CommonEventAwardShowRefObj, GSOCommonEventAwardShowRefSet>
            (ENPExportSettingEnum.COMMON_EVENT_AWARD_SHOW, GSOCommonEventAwardShowRefSet.assetPath, GSOCommonEventAwardShowRefSet.objName
	            , "common_event_award_show", "通用事件-奖励事件表现表 (common_event_award_show)", _judgeTagCanShow));
            
            _regMenuItem(new _TNPAutoExportRefMenu<CommonEventChoiceOptionRefObj, GSOCommonEventChoiceOptionRefSet>
            (ENPExportSettingEnum.COMMON_EVENT_CHOICE_OPTION, GSOCommonEventChoiceOptionRefSet.assetPath, GSOCommonEventChoiceOptionRefSet.objName
	            , "common_event_choice_option", "通用事件-选择事件选项表 (common_event_choice_option)", _judgeTagCanShow));
            
            _regMenuItem(new _TNPAutoExportSQLiteMenuItem<CommonEventChoiceRefObj>
            (ENPExportSettingEnum.COMMON_EVENT_CHOICE, CommonEventChoiceRefObj.objName, CommonEventChoiceRefObj.tableName, CommonEventChoiceRefObj.assetPath
	            , "common_event_choice", "通用事件-选择事件实例表 (common_event_choice)", _judgeTagCanShow));
            
            _regMenuItem(new _TNPAutoExportRefMenu<CommonEventChoiceShowRefObj, GSOCommonEventChoiceShowRefSet>
            (ENPExportSettingEnum.COMMON_EVENT_CHOICE_SHOW, GSOCommonEventChoiceShowRefSet.assetPath, GSOCommonEventChoiceShowRefSet.objName
	            , "common_event_choice_show", "通用事件-选择事件展示表 (common_event_choice_show)", _judgeTagCanShow));
            
            _regMenuItem(new _TNPAutoExportSQLiteMenuItem<CommonEventDialogRefObj>
            (ENPExportSettingEnum.COMMON_EVENT_DIALOG, CommonEventDialogRefObj.objName, CommonEventDialogRefObj.tableName, CommonEventDialogRefObj.assetPath
	            , "common_event_dialog", "通用事件-对话事件实例表 (common_event_dialog)", _judgeTagCanShow));
            
            _regMenuItem(new _TNPAutoExportRefMenu<CommonEventDispatchCondRefObj, GSOCommonEventDispatchCondRefSet>
            (ENPExportSettingEnum.COMMON_EVENT_DISPATCH_COND, GSOCommonEventDispatchCondRefSet.assetPath, GSOCommonEventDispatchCondRefSet.objName
	            , "common_event_dispatch_cond", "通用事件-派遣事件条件表 (common_event_dispatch_cond)", _judgeTagCanShow));
            
            _regMenuItem(new _TNPAutoExportSQLiteMenuItem<CommonEventDispatchRefObj>
            (ENPExportSettingEnum.COMMON_EVENT_DISPATCH, CommonEventDispatchRefObj.objName, CommonEventDispatchRefObj.tableName, CommonEventDispatchRefObj.assetPath
	            , "common_event_dispatch", "通用事件-派遣事件实例表 (common_event_dispatch)", _judgeTagCanShow));
            
            _regMenuItem(new _TNPAutoExportRefMenu<CommonEventDispatchResultRefObj, GSOCommonEventDispatchResultRefSet>
            (ENPExportSettingEnum.COMMON_EVENT_DISPATCH_RESULT, GSOCommonEventDispatchResultRefSet.assetPath, GSOCommonEventDispatchResultRefSet.objName
	            , "common_event_dispatch_result", "通用事件-派遣事件结果表 (common_event_dispatch_result)", _judgeTagCanShow));
            
            _regMenuItem(new _TNPAutoExportRefMenu<CommonEventDispatchShowRefObj, GSOCommonEventDispatchShowRefSet>
            (ENPExportSettingEnum.COMMON_EVENT_DISPATCH_SHOW, GSOCommonEventDispatchShowRefSet.assetPath, GSOCommonEventDispatchShowRefSet.objName
	            , "common_event_dispatch_show", "通用事件-派遣事件问题表 (common_event_dispatch_show)", _judgeTagCanShow));
            
            _regMenuItem(new _TNPAutoExportSQLiteMenuItem<CommonEventRefObj>
	            (ENPExportSettingEnum.COMMON_EVENT, CommonEventRefObj.objName, CommonEventRefObj.tableName, CommonEventRefObj.assetPath
		            , "common_event", "通用事件主表 (common_event)", _judgeTagCanShow));
            
            _regMenuItem(
	            new _TNPAutoExportSQLiteMenuItem<CommonEventRewardRefObj>
	            (ENPExportSettingEnum.COMMON_EVENT_REWARD, CommonEventRewardRefObj.objName, CommonEventRewardRefObj.tableName, CommonEventRewardRefObj.assetPath
		            , "common_event_reward", "通用事件-事件奖励 (common_event_reward)", _judgeTagCanShow));

            _regMenuItem(new _TNPAutoExportSQLiteMenuItem<CommonEventPlotDialogRefObj>
            (ENPExportSettingEnum.COMMON_EVENT_PLOT_DIALOG, CommonEventPlotDialogRefObj.objName, CommonEventPlotDialogRefObj.tableName, CommonEventPlotDialogRefObj.assetPath
                , "common_event_plot_dialog", "通用事件-剧情对话事件实例表 (common_event_plot_dialog)", _judgeTagCanShow));

            _regMenuItem(new _TNPAutoExportSQLiteMenuItem<CommonEventMiniGameRefObj>
            (ENPExportSettingEnum.COMMON_EVENT_MINI_GAME, CommonEventMiniGameRefObj.objName, CommonEventMiniGameRefObj.tableName, CommonEventMiniGameRefObj.assetPath
	            , "common_event_mini_game", "通用事件-小游戏事件实例表 (common_event_mini_game)", _judgeTagCanShow));
            
            _regMenuItem(new _TNPAutoExportRefMenu<CommonEventMiniGameShowRefObj, GSOCommonEventMiniGameShowRefSet>
            (ENPExportSettingEnum.COMMON_EVENT_MINI_GAME_SHOW, GSOCommonEventMiniGameShowRefSet.assetPath, GSOCommonEventMiniGameShowRefSet.objName
	            , "common_event_mini_game_show", "通用事件-小游戏事件展示表 (common_event_mini_game_show)", _judgeTagCanShow));
            
			#endregion

			#region 政务

			_regMenuItem(new _TNPAutoExportRefMenu<AnecdoteEventChoiceOptionRefObj, GSOAnecdoteEventChoiceOptionRefSet>
			(ENPExportSettingEnum.ANECDOTE_EVENT_CHOICE_OPTION, GSOAnecdoteEventChoiceOptionRefSet.assetPath, GSOAnecdoteEventChoiceOptionRefSet.objName
				, "anecdote_event_choice_option", "经营选择事件选择支表（anecdote_event_choice_option）", _judgeTagCanShow));
			_regMenuItem(new _TNPAutoExportRefMenu<AnecdoteEventChoiceRefObj, GSOAnecdoteEventChoiceRefSet>
			(ENPExportSettingEnum.ANECDOTE_EVENT_CHOICE, GSOAnecdoteEventChoiceRefSet.assetPath, GSOAnecdoteEventChoiceRefSet.objName
				, "anecdote_event_choice", "经营选择事件表（anecdote_event_choice）", _judgeTagCanShow));
			_regMenuItem(new _TNPAutoExportRefMenu<AnecdoteEventEarningsRefObj, GSOAnecdoteEventEarningsRefSet>
			(ENPExportSettingEnum.ANECDOTE_EVENT_EARNINGS, GSOAnecdoteEventEarningsRefSet.assetPath, GSOAnecdoteEventEarningsRefSet.objName
				, "anecdote_event_earnings", "经营赚速事件表（anecdote_event_earnings）", _judgeTagCanShow));
			_regMenuItem(new _TNPAutoExportRefMenu<AnecdoteEventRefObj, GSOAnecdoteEventRefSet>
            (ENPExportSettingEnum.ANECDOTE_EVENT, GSOAnecdoteEventRefSet.assetPath, GSOAnecdoteEventRefSet.objName
                , "anecdote_event", "经营事件表（anecdote_event）", _judgeTagCanShow));
			_regMenuItem(new _TNPAutoExportRefMenu<AnecdoteEventRewardRefObj, GSOAnecdoteEventRewardRefSet>
			(ENPExportSettingEnum.ANECDOTE_EVENT_REWARD, GSOAnecdoteEventRewardRefSet.assetPath, GSOAnecdoteEventRewardRefSet.objName
				, "anecdote_event_reward", "经营奖励事件表（anecdote_event_reward）", _judgeTagCanShow));
			_regMenuItem(new _TNPAutoExportRefMenu<AnecdotePosRefObj, GSOAnecdotePosRefSet>
			(ENPExportSettingEnum.ANECDOTE_POS, GSOAnecdotePosRefSet.assetPath, GSOAnecdotePosRefSet.objName
				, "anecdote_pos", "经营事件位置表（anecdote_pos）", _judgeTagCanShow));

			#endregion

			#region 小游戏表

			_regMenuItem(new _TNPAutoExportSQLiteMenuItem<MiniGameMainRefObj>
			(ENPExportSettingEnum.MINI_GAME_MAIN, MiniGameMainRefObj.objName, MiniGameMainRefObj.tableName, MiniGameMainRefObj.assetPath
				, "mini_game_main", "小游戏主表 (mini_game_main)", _judgeTagCanShow));

			_regMenuItem(new _TNPAutoExportRefMenu<PuzzleGameRefObj, GSOPuzzleGameRefSet>
			(ENPExportSettingEnum.PUZZLE_GAME, GSOPuzzleGameRefSet.assetPath, GSOPuzzleGameRefSet.objName
				, "puzzle_game", "拼图游戏表 (puzzle_game)", _judgeTagCanShow));
			
			_regMenuItem(new _TNPAutoExportRefMenu<FindThingsGameRefObj, GSOFindThingsGameRefSet>
			(ENPExportSettingEnum.FIND_THINGS_GAME, GSOFindThingsGameRefSet.assetPath, GSOFindThingsGameRefSet.objName
				, "find_things_game", "找东西游戏表 (find_things_game)", _judgeTagCanShow));
			
			_regMenuItem(new _TNPAutoExportRefMenu<TakeThingsSequentiallyGameRefObj, GSOTakeThingsSequentiallyGameRefSet>
			(ENPExportSettingEnum.TAKE_THINGS_SEQUENTIALLY_GAME, GSOTakeThingsSequentiallyGameRefSet.assetPath, GSOTakeThingsSequentiallyGameRefSet.objName
				, "take_things_sequentially_game", "顺序取东西游戏表 (take_things_sequentially_game)", _judgeTagCanShow));
			
			_regMenuItem(new _TNPAutoExportRefMenu<QTEGameRefObj, GSOQTEGameRefSet>
			(ENPExportSettingEnum.QTE_GAME, GSOQTEGameRefSet.assetPath, GSOQTEGameRefSet.objName
				, "qte_game", "qte游戏表 (qte_game)", _judgeTagCanShow));
			
			_regMenuItem(new _TNPAutoExportRefMenu<QteClickOpportunityGameRefObj, GSOQteClickOpportunityGameRefSet>
			(ENPExportSettingEnum.QTE_CLICK_OPPORTUNITY_GAME, GSOQteClickOpportunityGameRefSet.assetPath, GSOQteClickOpportunityGameRefSet.objName
				, "qte_click_opportunity_game", "Qte点击时机小游戏表 (qte_click_opportunity_game)", _judgeTagCanShow));
			
			_regMenuItem(new _TNPAutoExportRefMenu<DragBoxGameRefObj, GSODragBoxGameRefSet>
			(ENPExportSettingEnum.DRAG_BOX_GAME, GSODragBoxGameRefSet.assetPath, GSODragBoxGameRefSet.objName
				, "drag_box_game", "拖箱子游戏表 (drag_box_game)", _judgeTagCanShow));
			
			#endregion

			#region 漫画

			_regMenuItem(new _TNPAutoExportRefMenu<SimpleComicRefObj, GSOCSimpleComicRefSet>
			(ENPExportSettingEnum.SIMPLE_COMIC, GSOCSimpleComicRefSet.assetPath, GSOCSimpleComicRefSet.objName
				, "simple_comic", "简易漫画表（simple_comic）", _judgeTagCanShow));

            #endregion

			#region 跑马灯

			_regMenuItem(new _TNPAutoExportRefMenu<MarqueeRefObj, GSOMarqueeRefSet>
            (ENPExportSettingEnum.MARQUEE, GSOMarqueeRefSet.assetPath, GSOMarqueeRefSet.objName
                , "marquee", "跑马灯表（marquee）", _judgeTagCanShow));

			#endregion

			#region Q版形象

			//Q版形象表
			_regMenuItem(new _TNPAutoExportRefUniformMenu<CuteActorRefObj, GSOCuteActorRefSet>(ENPExportSettingEnum.CUTE_ACTOR,
				GSOCuteActorRefSet.assetPath, GSOCuteActorRefSet.objName, "cute_actor", "cute_actor表", NPEnum.ENPItemType.CUTE_ACTOR, _judgeTagCanShow));

			#endregion

			#region 猫咪气泡表

			//猫咪气泡表
			_regMenuItem(new _TNPAutoExportRefMenu<CatBubbleRefObj, GSOCatBubbleRefSet>
            (ENPExportSettingEnum.CAT_BUBBLE, GSOCatBubbleRefSet.assetPath, GSOCatBubbleRefSet.objName
                , "cat_bubble", "猫咪气泡表（cat_bubble）", _judgeTagCanShow));

			#endregion

			#region 国家大区表

			//国家大区表
			_regMenuItem(new _TNPAutoExportRefMenu<CountryAreaRefObj, PSOCountryAreaRefSet>
            (ENPExportSettingEnum.COUNTRY_AREA, PSOCountryAreaRefSet.assetPath, PSOCountryAreaRefSet.objName
                , "country_area", "国家大区表（country_area）", _judgeTagCanShow));

			#endregion

			#region 运营公告相关表

			_regMenuItem(new _TNPAutoExportRefMenu<AnnouncementBannerRefObj, GSOAnnouncementBannerRefSet>
            (ENPExportSettingEnum.ANNOUNCEMENT_BANNER, GSOAnnouncementBannerRefSet.assetPath, GSOAnnouncementBannerRefSet.objName
                , "announcement_banner", "运营公告海报图表（announcement_banner）", _judgeTagCanShow));

			_regMenuItem(new _TNPAutoExportRefMenu<AnnouncementJumpRefObj, GSOAnnouncementJumpRefSet>
            (ENPExportSettingEnum.ANNOUNCEMENT_JUMP, GSOAnnouncementJumpRefSet.assetPath, GSOAnnouncementJumpRefSet.objName
                , "announcement_jump", "运营公告游戏内跳转表（announcement_jump）", _judgeTagCanShow));

			_regMenuItem(new _TNPAutoExportRefMenu<AnnouncementTabRefObj, GSOAnnouncementTabRefSet>
            (ENPExportSettingEnum.ANNOUNCEMENT_TAB, GSOAnnouncementTabRefSet.assetPath, GSOAnnouncementTabRefSet.objName
                , "announcement_tab", "运营公告页签图表（announcement_tab）", _judgeTagCanShow));

			#endregion

            #region 增量包表

            //增量包表
            _regMenuItem(new _TNPAutoExportRefMenu<AddPackRefObj, GSOAddPackRefSet>
            (ENPExportSettingEnum.ADD_PACK, GSOAddPackRefSet.assetPath, GSOAddPackRefSet.objName
	            , "add_pack", "增量包表（add_pack）", _judgeTagCanShow));
            //增量包路径表
            _regMenuItem(new _TNPAutoExportRefMenu<AddPackPathsRefObj, GSOAddPackPathsRefSet>
            (ENPExportSettingEnum.ADD_PACK_PATHS, GSOAddPackPathsRefSet.assetPath, GSOAddPackPathsRefSet.objName
	            , "add_pack_paths", "增量包路径表（add_pack_paths）", _judgeTagCanShow));

			#endregion
			
			#region 建筑
			
			_regMenuItem(new _TNPAutoExportRefMenu<BuildingRefObj, GSOBuildingRefSet>
			(ENPExportSettingEnum.BUILDING, GSOBuildingRefSet.assetPath, GSOBuildingRefSet.objName
				, "building", "建筑表（building）", _judgeTagCanShow));
			_regMenuItem(new _TNPAutoExportRefMenu<BusinessBuildingRefObj, GSOBusinessBuildingRefSet>
			(ENPExportSettingEnum.BUSINESS_BUILDING, GSOBusinessBuildingRefSet.assetPath, GSOBusinessBuildingRefSet.objName
				, "business_building", "经营建筑表（business_building）", _judgeTagCanShow));
			_regMenuItem(new _TNPAutoExportRefMenu<BusinessBuildingHireCostRefObj, GSOBusinessBuildingHireCostRefSet>
			(ENPExportSettingEnum.BUSINESS_BUILDING_HIRE_COST, GSOBusinessBuildingHireCostRefSet.assetPath, GSOBusinessBuildingHireCostRefSet.objName
				, "business_building_hire_cost", "经营建筑雇佣花费表（business_building_hire_cost）", _judgeTagCanShow));
			_regMenuItem(new _TNPAutoExportRefMenu<BusinessBuildingLevelRefObj, GSOBusinessBuildingLevelRefSet>
			(ENPExportSettingEnum.BUSINESS_BUILDING_LEVEL, GSOBusinessBuildingLevelRefSet.assetPath, GSOBusinessBuildingLevelRefSet.objName
				, "business_building_level", "经营建筑等级表（business_building_level）", _judgeTagCanShow));
			_regMenuItem(new _TNPAutoExportRefMenu<BusinessBuildingVideoGroupRefObj, GSOBusinessBuildingVideoGroupRefSet>
			(ENPExportSettingEnum.BUSINESS_BUILDING_VIDEO_GROUP, GSOBusinessBuildingVideoGroupRefSet.assetPath, GSOBusinessBuildingVideoGroupRefSet.objName
				, "business_building_video_group", "经营建筑视频表（business_building_video_group）", _judgeTagCanShow));
			_regMenuItem(new _TNPAutoExportRefMenu<FarmingBuildingRefObj, GSOFarmingBuildingRefSet>
			(ENPExportSettingEnum.FARMING_BUILDING, GSOFarmingBuildingRefSet.assetPath, GSOFarmingBuildingRefSet.objName
				, "farming_building", "农业建筑表（farming_building）", _judgeTagCanShow));
			_regMenuItem(new _TNPAutoExportRefMenu<FarmingBuildingLevelRefObj, GSOFarmingBuildingLevelRefSet>
			(ENPExportSettingEnum.FARMING_BUILDING_LEVEL, GSOFarmingBuildingLevelRefSet.assetPath, GSOFarmingBuildingLevelRefSet.objName
				, "farming_building_level", "农业建筑等级表（farming_building_level）", _judgeTagCanShow));
			_regMenuItem(new _TNPAutoExportRefMenu<BusinessBuildingDevelopRefObj, GSOBusinessBuildingDevelopRefSet>
			(ENPExportSettingEnum.BUSINESS_BUILDING_DEVELOP, GSOBusinessBuildingDevelopRefSet.assetPath, GSOBusinessBuildingDevelopRefSet.objName
				, "business_building_develop", "经营建筑发展表（business_building_develop）", _judgeTagCanShow));
			_regMenuItem(new _TNPAutoExportRefMenu<BusinessBuildingProductRefObj, GSOBusinessBuildingProductRefSet>
			(ENPExportSettingEnum.BUSINESS_BUILDING_PRODUCT, GSOBusinessBuildingProductRefSet.assetPath, GSOBusinessBuildingProductRefSet.objName
				, "business_building_product", "经营建筑产品表（business_building_product）", _judgeTagCanShow));

			#endregion

			#region 晚间副本

			// 晚间副本排行榜奖励表
			_regMenuItem(new _TNPAutoExportRefMenu<EveningDungeonRankRewardRefObj, GSOEveningDungeonRankRewardRefSet>
			(ENPExportSettingEnum.EVENING_DUNGEON_RANK_REWARD, GSOEveningDungeonRankRewardRefSet.assetPath, GSOEveningDungeonRankRewardRefSet.objName
				, "evening_dungeon_rank_reward", "evening_dungeon_rank_reward 晚间副本排行榜奖励表", _judgeTagCanShow));
			
			_regMenuItem(new NPOnlyServerRefExportMenu("evening_dungeon_damage_ratio", ENPExportSettingEnum.EVENING_DUNGEON_DAMAGE_RATIO, "evening_dungeon_damage_ratio 晚间副本伤害比率表", _judgeTagCanShow));
			_regMenuItem(new NPOnlyServerRefExportMenu("evening_dungeon_boss_blood_add", ENPExportSettingEnum.EVENING_DUNGEON_BOSS_BLOOD_ADD, "evening_dungeon_boss_blood_add 晚间副本boss血量增加表", _judgeTagCanShow));

			#endregion

			#region 开服七日

			_regMenuItem(new _TNPAutoExportRefMenu<SevenDayGoalsTaskRefObj, GSOSevenDayGoalsTaskRefSet>
			(ENPExportSettingEnum.SEVEN_DAY_GOALS_TASK, GSOSevenDayGoalsTaskRefSet.assetPath, GSOSevenDayGoalsTaskRefSet.objName
				, "seven_day_goals_task", "开服七日任务模板表（seven_day_goals_task）", _judgeTagCanShow));
			_regMenuItem(new _TNPAutoExportRefMenu<SevenDayGoalsTaskRewardRefObj, GSOSevenDayGoalsTaskRewardRefSet>
			(ENPExportSettingEnum.SEVEN_DAY_GOALS_TASK_REWARD, GSOSevenDayGoalsTaskRewardRefSet.assetPath, GSOSevenDayGoalsTaskRewardRefSet.objName
				, "seven_day_goals_task_reward", "开服七日每天的任务表（seven_day_goals_task_reward）", _judgeTagCanShow));
			_regMenuItem(new _TNPAutoExportRefMenu<SevenDayGoalsStepRewardRefObj, GSOSevenDayGoalsStepRewardRefSet>
			(ENPExportSettingEnum.SEVEN_DAY_GOALS_STEP_REWARD, GSOSevenDayGoalsStepRewardRefSet.assetPath, GSOSevenDayGoalsStepRewardRefSet.objName
				, "seven_day_goals_step_reward", "开服七日分数奖励表（seven_day_goals_step_reward）", _judgeTagCanShow));
			_regMenuItem(new _TNPAutoExportRefMenu<SevenDayGoalsGiftPackRefObj, GSOSevenDayGoalsGiftPackRefSet>
			(ENPExportSettingEnum.SEVEN_DAY_GOALS_GIFT_PACK, GSOSevenDayGoalsGiftPackRefSet.assetPath, GSOSevenDayGoalsGiftPackRefSet.objName
				, "seven_day_goals_gift_pack", "开服七日礼包表（seven_day_goals_gift_pack）", _judgeTagCanShow));   

			#endregion
			
			//>>>>>>>>>> AUTO GENERATE START <<<<<<<<<<

			//联盟等级表
			_regMenuItem(new _TNPAutoExportRefMenu<GuildLevelRefObj, GSOGuildLevelRefSet>
			(ENPExportSettingEnum.GUILD_LEVEL, GSOGuildLevelRefSet.assetPath, GSOGuildLevelRefSet.objName
				, "guild_level", "联盟等级表（guild_level）", _judgeTagCanShow));

			//联盟职位表
			_regMenuItem(new _TNPAutoExportRefMenu<GuildPositionRefObj, GSOGuildPositionRefSet>
			(ENPExportSettingEnum.GUILD_POSITION, GSOGuildPositionRefSet.assetPath, GSOGuildPositionRefSet.objName
				, "guild_position", "联盟职位表（guild_position）", _judgeTagCanShow));

			//联盟旗帜表
			_regMenuItem(new _TNPAutoExportRefMenu<GuildFlagRefObj, GSOGuildFlagRefSet>
			(ENPExportSettingEnum.GUILD_FLAG, GSOGuildFlagRefSet.assetPath, GSOGuildFlagRefSet.objName
				, "guild_flag", "联盟旗帜表（guild_flag）", _judgeTagCanShow));

			//联盟建设表
			_regMenuItem(new _TNPAutoExportRefMenu<GuildConstructRefObj, GSOGuildConstructRefSet>
			(ENPExportSettingEnum.GUILD_CONSTRUCT, GSOGuildConstructRefSet.assetPath, GSOGuildConstructRefSet.objName
				, "guild_construct", "联盟建设表（guild_construct）", _judgeTagCanShow));

			//联盟日志表
			_regMenuItem(new _TNPAutoExportRefMenu<GuildLogRefObj, GSOGuildLogRefSet>
			(ENPExportSettingEnum.GUILD_LOG, GSOGuildLogRefSet.assetPath, GSOGuildLogRefSet.objName
				, "guild_log", "联盟日志表（guild_log）", _judgeTagCanShow));
			
			//加盟限制表
			_regMenuItem(new _TNPAutoExportRefMenu<GuildJoinLimitRefObj, GSOGuildJoinLimitRefSet>
			(ENPExportSettingEnum.GUILD_JOIN_LIMIT, GSOGuildJoinLimitRefSet.assetPath, GSOGuildJoinLimitRefSet.objName
				, "guild_join_limit", "加盟限制表（guild_join_limit）", _judgeTagCanShow));

			//联盟捐赠进度奖励表
			_regMenuItem(new _TNPAutoExportRefMenu<GuildConstructRewardRefObj, GSOGuildConstructRewardRefSet>
			(ENPExportSettingEnum.GUILD_CONSTRUCT_REWARD, GSOGuildConstructRewardRefSet.assetPath, GSOGuildConstructRewardRefSet.objName
				, "guild_construct_reward", "联盟捐赠进度奖励表（GuildConstructRewardRefObj）", _judgeTagCanShow));

			//联盟杂物委托表
			_regMenuItem(new _TNPAutoExportRefMenu<GuildRandomEntrustRefObj, GSOGuildRandomEntrustRefSet>
			(ENPExportSettingEnum.GUILD_RANDOM_ENTRUST, GSOGuildRandomEntrustRefSet.assetPath, GSOGuildRandomEntrustRefSet.objName
				, "guild_random_entrust", "联盟杂物委托表（GuildRandomEntrustRefObj）", _judgeTagCanShow));

			//联盟杂物委托品质表
			_regMenuItem(new _TNPAutoExportRefMenu<GuildRandomEntrustQualityRefObj, GSOGuildRandomEntrustQualityRefSet>
			(ENPExportSettingEnum.GUILD_RANDOM_ENTRUST_QUALITY, GSOGuildRandomEntrustQualityRefSet.assetPath, GSOGuildRandomEntrustQualityRefSet.objName
				, "guild_random_entrust_quality", "联盟杂物委托品质表（guild_random_entrust_quality）", _judgeTagCanShow));
            
			//藏品表
			_regMenuItem(new _TNPAutoExportRefUniformMenu<EquipRefObj, GSOEquipRefSet>
			(ENPExportSettingEnum.EQUIP, GSOEquipRefSet.assetPath, GSOEquipRefSet.objName
				, "equip", "藏品表（equip）", ENPItemType.EQUIP, _judgeTagCanShow));

			//弹幕组随机表
			_regMenuItem(new _TNPAutoExportRefMenu<CommentRandomGroupRefObj, GSOCommentRandomGroupRefSet>
			(ENPExportSettingEnum.COMMENT_RANDOM_GROUP, GSOCommentRandomGroupRefSet.assetPath, GSOCommentRandomGroupRefSet.objName
				, "comment_random_group", "弹幕组随机表（comment_random_group）", _judgeTagCanShow));

			//弹幕组随机表
			_regMenuItem(new _TNPAutoExportRefMenu<CommentPrefabRandomRefObj, GSOCommentPrefabRandomRefSet>
			(ENPExportSettingEnum.COMMENT_PREFAB_RANDOM, GSOCommentPrefabRandomRefSet.assetPath, GSOCommentPrefabRandomRefSet.objName
				, "comment_prefab_random", "弹幕组随机表（comment_prefab_random）", _judgeTagCanShow));
            
			//弹幕名字随机表
			_regMenuItem(new _TNPAutoExportRefMenu<CommentNameRandomRefObj, GSOCommentNameRandomRefSet>
			(ENPExportSettingEnum.COMMENT_NAME_RANDOM, GSOCommentNameRandomRefSet.assetPath, GSOCommentNameRandomRefSet.objName
				, "comment_name_random", "弹幕名字随机表（comment_name_random）", _judgeTagCanShow));

			//弹幕头像随机表
			_regMenuItem(new _TNPAutoExportRefMenu<CommentIconRandomRefObj, GSOCommentIconRandomRefSet>
			(ENPExportSettingEnum.COMMENT_ICON_RANDOM, GSOCommentIconRandomRefSet.assetPath, GSOCommentIconRandomRefSet.objName
				, "comment_icon_random", "弹幕头像随机表（comment_icon_random）", _judgeTagCanShow));

			#region 抽卡表

			//抽卡卡池表
			_regMenuItem(new _TNPAutoExportRefMenu<GachaPoolRefObj, GSOGachaPoolRefSet>
			(ENPExportSettingEnum.GACHA_POOL, GSOGachaPoolRefSet.assetPath, GSOGachaPoolRefSet.objName
				, "gacha_pool", "抽卡卡池表（gacha_pool）", _judgeTagCanShow));

			//抽卡道具表
			_regMenuItem(new _TNPAutoExportRefMenu<GachaItemRefObj, GSOGachaItemRefSet>
			(ENPExportSettingEnum.GACHA_ITEM, GSOGachaItemRefSet.assetPath, GSOGachaItemRefSet.objName
				, "gacha_item", "抽卡道具表（gacha_item）", _judgeTagCanShow));
			
			//抽卡道具展示信息表
			_regMenuItem(new _TNPAutoExportRefMenu<GachaItemShowInfoRefObj, GSOGachaItemShowInfoRefSet>
			(ENPExportSettingEnum.GACHA_ITEM_SHOW_INFO, GSOGachaItemShowInfoRefSet.assetPath, GSOGachaItemShowInfoRefSet.objName
				, "gacha_item_show_info", "抽卡道具展示信息表（gacha_item_show_info）", _judgeTagCanShow));
			
			_regMenuItem(new NPOnlyServerRefExportMenu("gacha_pool_step", ENPExportSettingEnum.GACHA_POOL_STEP, "抽卡阶段表 (gacha_pool_step)", _judgeTagCanShow));
			_regMenuItem(new NPOnlyServerRefExportMenu("gacha_quality_weight", ENPExportSettingEnum.GACHA_QUALITY_WEIGHT, "抽卡品质权重表 (gacha_quality_weight)", _judgeTagCanShow));

			//抽卡保底表
			_regMenuItem(new _TNPAutoExportRefMenu<GachaGuaranteeRefObj, GSOGachaGuaranteeRefSet>
			(ENPExportSettingEnum.GACHA_GUARANTEE, GSOGachaGuaranteeRefSet.assetPath, GSOGachaGuaranteeRefSet.objName
				, "gacha_guarantee", "抽卡保底表（gacha_guarantee）", _judgeTagCanShow));
			
			#endregion

			#region 招募表

			//招募表
			_regMenuItem(new _TNPAutoExportRefMenu<RecruitRefObj, GSORecruitRefSet>
			(ENPExportSettingEnum.RECRUIT, GSORecruitRefSet.assetPath, GSORecruitRefSet.objName
				, "recruit", "招募表（recruit）", _judgeTagCanShow));

			//招募商店表
			_regMenuItem(new _TNPAutoExportRefMenu<RecruitShopRefObj, GSORecruitShopRefSet>
			(ENPExportSettingEnum.RECRUIT_SHOP, GSORecruitShopRefSet.assetPath, GSORecruitShopRefSet.objName
				, "recruit_shop", "招募商店表（recruit_shop）", _judgeTagCanShow));

			#endregion
			
			//竞技场指定谈判道具表
			_regMenuItem(new _TNPAutoExportRefMenu<ArenaSelectAttackConsumeRefObj, GSOArenaSelectAttackConsumeRefSet>
			(ENPExportSettingEnum.ARENA_SELECT_ATTACK_CONSUME, GSOArenaSelectAttackConsumeRefSet.assetPath, GSOArenaSelectAttackConsumeRefSet.objName
				, "arena_select_attack_consume", "竞技场指定谈判道具表（arena_select_attack_consume）", _judgeTagCanShow));

			//竞技场贸易站等级表
			_regMenuItem(new _TNPAutoExportRefMenu<ArenaStationLevelRefObj, GSOArenaStationLevelRefSet>
			(ENPExportSettingEnum.ARENA_STATION_LEVEL, GSOArenaStationLevelRefSet.assetPath, GSOArenaStationLevelRefSet.objName
				, "arena_station_level", "竞技场贸易站等级表（arena_station_level）", _judgeTagCanShow));

			//竞技场临时增益表
			_regMenuItem(new _TNPAutoExportRefMenu<ArenaBuffRefObj, GSOArenaBuffRefSet>
			(ENPExportSettingEnum.ARENA_BUFF, GSOArenaBuffRefSet.assetPath, GSOArenaBuffRefSet.objName
				, "arena_buff", "竞技场临时增益表（arena_buff）", _judgeTagCanShow));

			//竞技场轮次奖励表
			_regMenuItem(new _TNPAutoExportRefMenu<ArenaRoundRewardRefObj, GSOArenaRoundRewardRefSet>
			(ENPExportSettingEnum.ARENA_ROUND_REWARD, GSOArenaRoundRewardRefSet.assetPath, GSOArenaRoundRewardRefSet.objName
				, "arena_round_reward", "竞技场轮次奖励表（arena_round_reward）", _judgeTagCanShow));

			//竞技场最终奖励表
			_regMenuItem(new _TNPAutoExportRefMenu<ArenaFinalRewardRefObj, GSOArenaFinalRewardRefSet>
			(ENPExportSettingEnum.ARENA_FINAL_REWARD, GSOArenaFinalRewardRefSet.assetPath, GSOArenaFinalRewardRefSet.objName
				, "arena_final_reward", "竞技场最终奖励表（arena_final_reward）", _judgeTagCanShow));

            //竞技场机器人随机表
            _regMenuItem(new NPOnlyServerRefExportMenu("arena_bot_random", ENPExportSettingEnum.ARENA_BOT_RANDOM,
                "竞技场机器人随机表（arena_bot_random）", _judgeTagCanShow));

            //竞技场机器人模板表
            _regMenuItem(new NPOnlyServerRefExportMenu("arena_bot_template", ENPExportSettingEnum.ARENA_BOT_TEMPLATE,
                "竞技场机器人模板表 （arena_bot_template）", _judgeTagCanShow));

            _regMenuItem(new NPOnlyServerRefExportMenu("rank_gift_pack", ENPExportSettingEnum.RANK_GIFT_PACK, "rank_gift_pack 特卖礼包", _judgeTagCanShow));

            
            #region 阶段目标
            
            _regMenuItem(new _TNPAutoExportRefMenu<StageGoalBigStepRefObj, GSOStageGoalBigStepRefSet>
	        (ENPExportSettingEnum.STAGE_GOAL_BIG_STEP, GSOStageGoalBigStepRefSet.assetPath, GSOStageGoalBigStepRefSet.objName
		        , "stage_goal_big_step", "阶段目标大阶段表（stage_goal_big_step）", _judgeTagCanShow));


            _regMenuItem(new _TNPAutoExportRefMenu<StageGoalRefObj, GSOStageGoalRefSet>
			(ENPExportSettingEnum.STAGE_GOAL, GSOStageGoalRefSet.assetPath, GSOStageGoalRefSet.objName
				, "stage_goal", "阶段表（stage_goal）", _judgeTagCanShow));

			_regMenuItem(new _TNPAutoExportRefMenu<StageGoalTaskRefObj, GSOStageGoalTaskRefSet>
			(ENPExportSettingEnum.STAGE_GOAL_TASK, GSOStageGoalTaskRefSet.assetPath, GSOStageGoalTaskRefSet.objName
				, "stage_goal_task", "阶段目标表（stage_goal_task）", _judgeTagCanShow));

			#endregion

			#region 午间副本
			
			//午间副本Boss波次表
			_regMenuItem(new _TNPAutoExportRefMenu<MiddayDungeonWaveRefObj, GSOMiddayDungeonWaveRefSet>
			(ENPExportSettingEnum.MIDDAY_DUNGEON_WAVE, GSOMiddayDungeonWaveRefSet.assetPath, GSOMiddayDungeonWaveRefSet.objName
				, "midday_dungeon_wave", "午间副本Boss波次表（midday_dungeon_wave）", _judgeTagCanShow));
			
			//开服天数相关
			_regMenuItem(new NPOnlyServerRefExportMenu("server_start_days",ENPExportSettingEnum.SERVER_START_DAYS,
				"server_start_days 午间副本开服天数", _judgeTagCanShow));

			#endregion
			
			//通用目标奖励表
			_regMenuItem(new _TNPAutoExportRefMenu<CommonTargetRewardRefObj, GSOCommonTargetRewardRefSet>
			(ENPExportSettingEnum.COMMON_TARGET_REWARD, GSOCommonTargetRewardRefSet.assetPath, GSOCommonTargetRewardRefSet.objName
				, "common_target_reward", "通用目标奖励表（common_target_reward）", _judgeTagCanShow));
			
			//通用刷新表
			_regMenuItem(new _TNPAutoExportRefMenu<CommonRefreshRefObj, GSOCommonRefreshRefSet>
			(ENPExportSettingEnum.COMMON_REFRESH, GSOCommonRefreshRefSet.assetPath, GSOCommonRefreshRefSet.objName
				, "common_refresh", "通用刷新表（common_refresh）", _judgeTagCanShow));

			//活动限时冲榜表
			_regMenuItem(new _TNPAutoExportRefMenu<ActivityRankRushRefObj, GSOActivityRankRushRefSet>
			(ENPExportSettingEnum.ACTIVITY_RANK_RUSH, GSOActivityRankRushRefSet.assetPath, GSOActivityRankRushRefSet.objName
				, "activity_rank_rush", "活动限时冲榜表（activity_rank_rush）", _judgeTagCanShow));
			//爬塔表
			_regMenuItem(new _TNPAutoExportRefMenu<TowerChapterRefObj, GSOTowerChapterRefSet>
			(ENPExportSettingEnum.TOWER_CHAPTER, GSOTowerChapterRefSet.assetPath, GSOTowerChapterRefSet.objName
				, "tower_chapter", "爬塔表（tower_chapter）", _judgeTagCanShow));

			//爬塔关卡阶段表
			_regMenuItem(new _TNPAutoExportRefMenu<TowerChapterStageRefObj, GSOTowerChapterStageRefSet>
			(ENPExportSettingEnum.TOWER_CHAPTER_STAGE, GSOTowerChapterStageRefSet.assetPath, GSOTowerChapterStageRefSet.objName
				, "tower_chapter_stage", "爬塔关卡阶段表（tower_chapter_stage）", _judgeTagCanShow));

			//爬塔调整展示阶梯表
			_regMenuItem(new _TNPAutoExportRefMenu<TowerChapterStageShowRefObj, GSOTowerChapterStageShowRefSet>
			(ENPExportSettingEnum.TOWER_CHAPTER_STAGE_SHOW, GSOTowerChapterStageShowRefSet.assetPath, GSOTowerChapterStageShowRefSet.objName
				, "tower_chapter_stage_show", "爬塔调整展示阶梯表（tower_chapter_stage_show）", _judgeTagCanShow));

			//玩家皮肤表
			_regMenuItem(new _TNPAutoExportRefUniformMenu<PlayerSkinRefObj, GSOPlayerSkinRefSet>
			(ENPExportSettingEnum.PLAYER_SKIN, GSOPlayerSkinRefSet.assetPath, GSOPlayerSkinRefSet.objName
				, "player_skin", "玩家皮肤表（player_skin）", ENPItemType.PLAYER_SKIN, _judgeTagCanShow));

			//玩家皮肤等级表
			_regMenuItem(new _TNPAutoExportRefMenu<PlayerSkinLevelRefObj, GSOPlayerSkinLevelRefSet>
			(ENPExportSettingEnum.PLAYER_SKIN_LEVEL, GSOPlayerSkinLevelRefSet.assetPath, GSOPlayerSkinLevelRefSet.objName
				, "player_skin_level", "玩家皮肤等级表（player_skin_level）", _judgeTagCanShow));

			//玩家称号表
			_regMenuItem(new _TNPAutoExportRefUniformMenu<PlayerTitleRefObj, GSOPlayerTitleRefSet>
			(ENPExportSettingEnum.PLAYER_TITLE, GSOPlayerTitleRefSet.assetPath, GSOPlayerTitleRefSet.objName
				, "player_title", "玩家称号表（player_title）", ENPItemType.TITLE, _judgeTagCanShow));

			//玩家限时称号分组表
			_regMenuItem(new _TNPAutoExportRefMenu<PlayerTitleLimitGroupRefObj, GSOPlayerTitleLimitGroupRefSet>
			(ENPExportSettingEnum.PLAYER_TITLE_LIMIT_GROUP, GSOPlayerTitleLimitGroupRefSet.assetPath, GSOPlayerTitleLimitGroupRefSet.objName
				, "player_title_limit_group", "玩家限时称号分组表（player_title_limit_group）", _judgeTagCanShow));

			//玩家组合称号前缀表
			_regMenuItem(new _TNPAutoExportRefUniformMenu<PlayerTitlePrefixRefObj, GSOPlayerTitlePrefixRefSet>
			(ENPExportSettingEnum.PLAYER_TITLE_PREFIX, GSOPlayerTitlePrefixRefSet.assetPath, GSOPlayerTitlePrefixRefSet.objName
				, "player_title_prefix", "玩家组合称号前缀表（player_title_prefix）", ENPItemType.TITLE_PRE, _judgeTagCanShow));

			//玩家组合称号后缀表
			_regMenuItem(new _TNPAutoExportRefUniformMenu<PlayerTitleSuffixRefObj, GSOPlayerTitleSuffixRefSet>
			(ENPExportSettingEnum.PLAYER_TITLE_SUFFIX, GSOPlayerTitleSuffixRefSet.assetPath, GSOPlayerTitleSuffixRefSet.objName
				, "player_title_suffix", "玩家组合称号后缀表（player_title_suffix）", ENPItemType.TITLE_SFX, _judgeTagCanShow));

			//玩家组合称号底框表
			_regMenuItem(new _TNPAutoExportRefUniformMenu<PlayerTitleBgRefObj, GSOPlayerTitleBgRefSet>
			(ENPExportSettingEnum.PLAYER_TITLE_BG, GSOPlayerTitleBgRefSet.assetPath, GSOPlayerTitleBgRefSet.objName
				, "player_title_bg", "玩家组合称号底框表（player_title_bg）", ENPItemType.TITLE_BG, _judgeTagCanShow));

			//千万目标奖励
			_regMenuItem(new _TNPAutoExportRefMenu<EarningGoalRewardRefObj, GSOEarningGoalRewardRefSet>
			(ENPExportSettingEnum.EARNING_GOAL_REWARD, GSOEarningGoalRewardRefSet.assetPath, GSOEarningGoalRewardRefSet.objName
				, "earning_goal_reward", "千万目标奖励（earning_goal_reward）", _judgeTagCanShow));

			//千万目标荣耀奖励表
			_regMenuItem(new _TNPAutoExportRefMenu<EarningGoalHonorRewardRefObj, GSOEarningGoalHonorRewardRefSet>
			(ENPExportSettingEnum.EARNING_GOAL_HONOR_REWARD, GSOEarningGoalHonorRewardRefSet.assetPath, GSOEarningGoalHonorRewardRefSet.objName
				, "earning_goal_honor_reward", "千万目标荣耀奖励表（earning_goal_honor_reward）", _judgeTagCanShow));

			//七日登录
			_regMenuItem(new _TNPAutoExportRefMenu<SevenDayLoginRefObj, GSOSevenDayLoginRefSet>
			(ENPExportSettingEnum.SEVEN_DAY_LOGIN, GSOSevenDayLoginRefSet.assetPath, GSOSevenDayLoginRefSet.objName
				, "seven_day_login", "七日登录（seven_day_login）", _judgeTagCanShow));

			//商店总表
			_regMenuItem(new _TNPAutoExportRefMenu<ShopMainRefObj, GSOShopMainRefSet>
			(ENPExportSettingEnum.SHOP_MAIN, GSOShopMainRefSet.assetPath, GSOShopMainRefSet.objName
				, "shop_main", "商店总表（shop_main）", _judgeTagCanShow));

			//活动商店表
			_regMenuItem(new _TNPAutoExportRefMenu<ActivityShopRefObj, GSOActivityShopRefSet>
			(ENPExportSettingEnum.ACTIVITY_SHOP, GSOActivityShopRefSet.assetPath, GSOActivityShopRefSet.objName
				, "activity_shop", "活动商店表（activity_shop）", _judgeTagCanShow));

			//活动商店道具表
			_regMenuItem(new _TNPAutoExportRefMenu<ActivityShopItemRefObj, GSOActivityShopItemRefSet>
			(ENPExportSettingEnum.ACTIVITY_SHOP_ITEM, GSOActivityShopItemRefSet.assetPath, GSOActivityShopItemRefSet.objName
				, "activity_shop_item", "活动商店道具表（activity_shop_item）", _judgeTagCanShow));

			//钻石礼包组表
			_regMenuItem(new _TNPAutoExportRefMenu<CrystalGiftPackGroupRefObj, GSOCrystalGiftPackGroupRefSet>
			(ENPExportSettingEnum.CRYSTAL_GIFT_PACK_GROUP, GSOCrystalGiftPackGroupRefSet.assetPath, GSOCrystalGiftPackGroupRefSet.objName
				, "crystal_gift_pack_group", "钻石礼包组表（crystal_gift_pack_group）", _judgeTagCanShow));

			//钻石礼包表
			_regMenuItem(new _TNPAutoExportRefMenu<CrystalGiftPackRefObj, GSOCrystalGiftPackRefSet>
			(ENPExportSettingEnum.CRYSTAL_GIFT_PACK, GSOCrystalGiftPackRefSet.assetPath, GSOCrystalGiftPackRefSet.objName
				, "crystal_gift_pack", "钻石礼包表（crystal_gift_pack）", _judgeTagCanShow));

			//活动兑换卷表
			_regMenuItem(new _TNPAutoExportRefUniformMenu<ActivityCurrencyRefObj, GSOActivityCurrencyRefSet>
			(ENPExportSettingEnum.ACTIVITY_CURRENCY, GSOActivityCurrencyRefSet.assetPath, GSOActivityCurrencyRefSet.objName
				, "activity_currency", "活动兑换卷表（activity_currency）", ENPItemType.ACTIVITY_CURRENCY, _judgeTagCanShow));

			//活动换皮表
			_regMenuItem(new _TNPAutoExportRefMenu<ActivityPrefabSkinRefObj, GSOActivityPrefabSkinRefSet>
			(ENPExportSettingEnum.ACTIVITY_PREFAB_SKIN, GSOActivityPrefabSkinRefSet.assetPath, GSOActivityPrefabSkinRefSet.objName
				, "activity_prefab_skin", "活动换皮表（activity_prefab_skin）", _judgeTagCanShow));

            //万能活动表
            _regMenuItem(new ActivityRefExportMenu("万能活动表 (regular_event)", ENPExportSettingEnum.REGULAR_EVENT, _judgeTagCanShow,
                "regular_event_shop", "regular_event_shop_item"));

			//创角预设表
			_regMenuItem(new _TNPAutoExportRefMenu<PlayerCreatPlayerPrefabRefObj, GSOPlayerCreatPlayerPrefabRefSet>
			(ENPExportSettingEnum.PLAYER_CREATE_PLAYER_PREFAB, GSOPlayerCreatPlayerPrefabRefSet.assetPath, GSOPlayerCreatPlayerPrefabRefSet.objName
				, "player_create_player_prefab", "创角预设表（player_create_player_prefab）", _judgeTagCanShow));

            // 三消活动表
            _regMenuItem(new ActivityRefExportMenu("三消活动表 (activity_tilematch)", ENPExportSettingEnum.TILEMATCH_ACTIVITY, _judgeTagCanShow,
	            "tilematch_other", "tilematch_mode", "tilematch_block", "tilematch_block_show", "tilematch_link", "tilematch_task", "tilematch_step_reward", "tilematch_jackpot_group"));

            // 2048 活动表
            _regMenuItem(new ActivityRefExportMenu("2048 活动表 (activity_nummerge)", ENPExportSettingEnum.NUMMERGE_ACTIVITY, _judgeTagCanShow,
	            "num_merge_other", "num_merge_mode", "num_merge_block", "num_merge_box"));
            
			//活动中心表
			_regMenuItem(new _TNPAutoExportRefMenu<ActivityCenterRefObj, GSOActivityCenterRefSet>
			(ENPExportSettingEnum.ACTIVITY_CENTER, GSOActivityCenterRefSet.assetPath, GSOActivityCenterRefSet.objName
				, "activity_center", "活动中心表（activity_center）", _judgeTagCanShow));
            
			//旅店等级表
			_regMenuItem(new _TNPAutoExportRefMenu<InnLevelRefObj, GSOInnLevelRefSet>
			(ENPExportSettingEnum.INN_LEVEL, GSOInnLevelRefSet.assetPath, GSOInnLevelRefSet.objName
				, "inn_level", "旅店等级表（inn_level）", _judgeTagCanShow));

			//旅店奖牌等级表
			_regMenuItem(new _TNPAutoExportRefMenu<InnMedalLevelRefObj, GSOInnMedalLevelRefSet>
			(ENPExportSettingEnum.INN_MEDAL_LEVEL, GSOInnMedalLevelRefSet.assetPath, GSOInnMedalLevelRefSet.objName
				, "inn_medal_level", "旅店奖牌等级表（inn_medal_level）", _judgeTagCanShow));

			//旅店设施表
			_regMenuItem(new _TNPAutoExportRefMenu<InnStationRefObj, GSOInnStationRefSet>
			(ENPExportSettingEnum.INN_STATION, GSOInnStationRefSet.assetPath, GSOInnStationRefSet.objName
				, "inn_station", "旅店设施表（inn_station）", _judgeTagCanShow));

			//旅店设施等级表
			_regMenuItem(new _TNPAutoExportRefMenu<InnStationLevelRefObj, GSOInnStationLevelRefSet>
			(ENPExportSettingEnum.INN_STATION_LEVEL, GSOInnStationLevelRefSet.assetPath, GSOInnStationLevelRefSet.objName
				, "inn_station_level", "旅店设施等级表（inn_station_level）", _judgeTagCanShow));

			//旅店菜品表
			_regMenuItem(new _TNPAutoExportRefMenu<InnDishRefObj, GSOInnDishRefSet>
			(ENPExportSettingEnum.INN_DISH, GSOInnDishRefSet.assetPath, GSOInnDishRefSet.objName
				, "inn_dish", "旅店菜品表（inn_dish）", _judgeTagCanShow));

			//旅店菜品等级表
			_regMenuItem(new _TNPAutoExportRefMenu<InnDishLevelRefObj, GSOInnDishLevelRefSet>
			(ENPExportSettingEnum.INN_DISH_LEVEL, GSOInnDishLevelRefSet.assetPath, GSOInnDishLevelRefSet.objName
				, "inn_dish_level", "旅店菜品等级表（inn_dish_level）", _judgeTagCanShow));

			//旅店客人表
			_regMenuItem(new _TNPAutoExportRefMenu<InnGuestRefObj, GSOInnGuestRefSet>
			(ENPExportSettingEnum.INN_GUEST, GSOInnGuestRefSet.assetPath, GSOInnGuestRefSet.objName
				, "inn_guest", "旅店客人表（inn_guest）", _judgeTagCanShow));

			//倒计时事件表
			_regMenuItem(new _TNPAutoExportRefMenu<CountdownEventRefObj, GSOCountdownEventRefSet>
			(ENPExportSettingEnum.COUNTDOWN_EVENT, GSOCountdownEventRefSet.assetPath, GSOCountdownEventRefSet.objName
				, "countdown_event", "倒计时事件表（countdown_event）", _judgeTagCanShow));

			//妃子预设对话表
			_regMenuItem(new _TNPAutoExportRefMenu<ConsortChatDialogueRefObj, GSOConsortChatDialogueRefSet>
			(ENPExportSettingEnum.CONSORT_CHAT_DIALOGUE, GSOConsortChatDialogueRefSet.assetPath, GSOConsortChatDialogueRefSet.objName
				, "consort_chat_dialogue", "妃子预设对话表（consort_chat_dialogue）", _judgeTagCanShow));

			//妃子预设句子表
			_regMenuItem(new _TNPAutoExportRefMenu<ConsortChatDialogueSentenceRefObj, GSOConsortChatDialogueSentenceRefSet>
			(ENPExportSettingEnum.CONSORT_CHAT_DIALOGUE_SENTENCE, GSOConsortChatDialogueSentenceRefSet.assetPath, GSOConsortChatDialogueSentenceRefSet.objName
				, "consort_chat_dialogue_sentence", "妃子预设句子表（consort_chat_dialogue_sentence）", _judgeTagCanShow));

			//妃子AI对话表
			_regMenuItem(new _TNPAutoExportRefMenu<ConsortChatAIRefObj, GSOConsortChatAIRefSet>
			(ENPExportSettingEnum.CONSORT_CHAT_AI, GSOConsortChatAIRefSet.assetPath, GSOConsortChatAIRefSet.objName
				, "consort_chat_ai", "妃子AI对话表（consort_chat_ai）", _judgeTagCanShow));

			//妃子朋友圈表
			_regMenuItem(new _TNPAutoExportRefMenu<ConsortChatMomentsRefObj, GSOConsortChatMomentsRefSet>
			(ENPExportSettingEnum.CONSORT_CHAT_MOMENTS, GSOConsortChatMomentsRefSet.assetPath, GSOConsortChatMomentsRefSet.objName
				, "consort_chat_moments", "妃子朋友圈表（consort_chat_moments）", _judgeTagCanShow));

			//妃子朋友圈照片表
			_regMenuItem(new _TNPAutoExportRefMenu<ConsortChatImageGroupRefObj, GSOConsortChatImageGroupRefSet>
			(ENPExportSettingEnum.CONSORT_CHAT_IMAGE_GROUP, GSOConsortChatImageGroupRefSet.assetPath, GSOConsortChatImageGroupRefSet.objName
				, "consort_chat_image_group", "妃子朋友圈照片表（consort_chat_image_group）", _judgeTagCanShow));

			//旅店特殊客人表
			_regMenuItem(new _TNPAutoExportRefMenu<InnSpecialGuestRefObj, GSOInnSpecialGuestRefSet>
			(ENPExportSettingEnum.INN_SPECIAL_GUEST, GSOInnSpecialGuestRefSet.assetPath, GSOInnSpecialGuestRefSet.objName
				, "inn_special_guest", "旅店特殊客人表（inn_special_guest）", _judgeTagCanShow));

			//旅店菜谱表
			_regMenuItem(new _TNPAutoExportRefUniformMenu<InnRecipeRefObj, GSOInnRecipeRefSet>
			(ENPExportSettingEnum.INN_RECIPE, GSOInnRecipeRefSet.assetPath, GSOInnRecipeRefSet.objName
				, "inn_recipe", "旅店菜谱表（inn_recipe）", ENPItemType.INN_RECIPE, _judgeTagCanShow));

			//博物馆表
			_regMenuItem(new _TNPAutoExportRefUniformMenu<MuseumItemRefObj, GSOMuseumItemRefSet>
			(ENPExportSettingEnum.MUSEUM_ITEM, GSOMuseumItemRefSet.assetPath, GSOMuseumItemRefSet.objName
				, "museum_item", "博物馆表（museum_item）", ENPItemType.MUSEUM_ITEM, _judgeTagCanShow));

			//博物馆表
			_regMenuItem(new _TNPAutoExportRefMenu<MuseumItemLevelRefObj, GSOMuseumItemLevelRefSet>
			(ENPExportSettingEnum.MUSEUM_ITEM_LEVEL, GSOMuseumItemLevelRefSet.assetPath, GSOMuseumItemLevelRefSet.objName
				, "museum_item_level", "博物馆表（museum_item_level）", _judgeTagCanShow));

			//博物馆表
			_regMenuItem(new _TNPAutoExportRefMenu<MuseumItemUpgradeCostRefObj, GSOMuseumItemUpgradeCostRefSet>
			(ENPExportSettingEnum.MUSEUM_ITEM_UPGRADE_COST, GSOMuseumItemUpgradeCostRefSet.assetPath, GSOMuseumItemUpgradeCostRefSet.objName
				, "museum_item_upgrade_cost", "博物馆表（museum_item_upgrade_cost）", _judgeTagCanShow));

			//招聘体验表
			_regMenuItem(new _TNPAutoExportRefMenu<HireRefObj, GSOHireRefSet>
			(ENPExportSettingEnum.HIRE, GSOHireRefSet.assetPath, GSOHireRefSet.objName
				, "hire", "招聘体验表（hire）", _judgeTagCanShow));
			//爬塔研究表
			_regMenuItem(new _TNPAutoExportRefMenu<TowerResearchRefObj, GSOTowerResearchRefSet>
			(ENPExportSettingEnum.TOWER_RESEARCH, GSOTowerResearchRefSet.assetPath, GSOTowerResearchRefSet.objName
				, "tower_research", "爬塔研究表（tower_research）", _judgeTagCanShow));

			//系统任务表
			_regMenuItem(new _TNPAutoExportRefMenu<SystemQuestRefObj, GSOSystemQuestRefSet>
			(ENPExportSettingEnum.SYSTEM_QUEST, GSOSystemQuestRefSet.assetPath, GSOSystemQuestRefSet.objName
				, "system_quest", "系统任务表（system_quest）", _judgeTagCanShow));

			//礼包表
			_regMenuItem(new _TNPAutoExportRefMenu<GiftPackRefObj, GSOGiftPackRefSet>
			(ENPExportSettingEnum.GIFT_PACK, GSOGiftPackRefSet.assetPath, GSOGiftPackRefSet.objName
				, "gift_pack", "礼包表（gift_pack）", _judgeTagCanShow));

			//礼包组表
			_regMenuItem(new _TNPAutoExportRefMenu<GiftPackGroupRefObj, GSOGiftPackGroupRefSet>
			(ENPExportSettingEnum.GIFT_PACK_GROUP, GSOGiftPackGroupRefSet.assetPath, GSOGiftPackGroupRefSet.objName
				, "gift_pack_group", "礼包组表（gift_pack_group）", _judgeTagCanShow));

			//支付档位表
			_regMenuItem(new _TNPAutoExportRefUniformMenu<PayRefObj, GSOPayRefSet>
			(ENPExportSettingEnum.PAY, GSOPayRefSet.assetPath, GSOPayRefSet.objName
				, "pay", "支付档位表（pay）", ENPItemType.PAY, _judgeTagCanShow));
				
			//太空寻宝 - 矿石表
			_regMenuItem(new _TNPAutoExportRefMenu<TreasureHuntOreRefObj, GSOTreasureHuntOreRefSet>
			(ENPExportSettingEnum.TREASURE_HUNT_ORE, GSOTreasureHuntOreRefSet.assetPath, GSOTreasureHuntOreRefSet.objName
				, "treasure_hunt_ore", "太空寻宝 - 矿石表（treasure_hunt_ore）", _judgeTagCanShow));

			//太空寻宝 - 奇物表
			_regMenuItem(new _TNPAutoExportRefMenu<TreasureHuntTreasureRefObj, GSOTreasureHuntTreasureRefSet>
			(ENPExportSettingEnum.TREASURE_HUNT_TREASURE, GSOTreasureHuntTreasureRefSet.assetPath, GSOTreasureHuntTreasureRefSet.objName
				, "treasure_hunt_treasure", "太空寻宝 - 奇物表（treasure_hunt_treasure）", _judgeTagCanShow));

			//太空寻宝 - 实验室表
			_regMenuItem(new _TNPAutoExportRefMenu<TreasureHuntLabRefObj, GSOTreasureHuntLabRefSet>
			(ENPExportSettingEnum.TREASURE_HUNT_LAB, GSOTreasureHuntLabRefSet.assetPath, GSOTreasureHuntLabRefSet.objName
				, "treasure_hunt_lab", "太空寻宝 - 实验室表（treasure_hunt_lab）", _judgeTagCanShow));

			//太空寻宝 - 太空区域表
			_regMenuItem(new _TNPAutoExportRefMenu<TreasureHuntAreaRefObj, GSOTreasureHuntAreaRefSet>
			(ENPExportSettingEnum.TREASURE_HUNT_AREA, GSOTreasureHuntAreaRefSet.assetPath, GSOTreasureHuntAreaRefSet.objName
				, "treasure_hunt_area", "太空寻宝 - 太空区域表（treasure_hunt_area）", _judgeTagCanShow));

			//太空寻宝 - 图鉴页签表
			_regMenuItem(new _TNPAutoExportRefMenu<TreasureHuntCatalogTabRefObj, GSOTreasureHuntCatalogTabRefSet>
			(ENPExportSettingEnum.TREASURE_HUNT_CATALOG_TAB, GSOTreasureHuntCatalogTabRefSet.assetPath, GSOTreasureHuntCatalogTabRefSet.objName
				, "treasure_hunt_catalog_tab", "太空寻宝 - 图鉴页签表（treasure_hunt_catalog_tab）", _judgeTagCanShow));

			//太空寻宝 - 太空舱等级表
			_regMenuItem(new _TNPAutoExportRefMenu<TreasureHuntStationLvlRefObj, GSOTreasureHuntStationLvlRefSet>
			(ENPExportSettingEnum.TREASURE_HUNT_STATION_LVL, GSOTreasureHuntStationLvlRefSet.assetPath, GSOTreasureHuntStationLvlRefSet.objName
				, "treasure_hunt_station_lvl", "太空寻宝 - 太空舱等级表（treasure_hunt_station_lvl）", _judgeTagCanShow));

			//太空寻宝 - 技能表
			_regMenuItem(new _TNPAutoExportRefMenu<TreasureHuntSkillRefObj, GSOTreasureHuntSkillRefSet>
			(ENPExportSettingEnum.TREASURE_HUNT_SKILL, GSOTreasureHuntSkillRefSet.assetPath, GSOTreasureHuntSkillRefSet.objName
				, "treasure_hunt_skill", "太空寻宝 - 技能表（treasure_hunt_skill）", _judgeTagCanShow));
			//太空寻宝 - 技能等级表
			_regMenuItem(new _TNPAutoExportRefMenu<TreasureHuntSkillLevelRefObj, GSOTreasureHuntSkillLevelRefSet>
			(ENPExportSettingEnum.TREASURE_HUNT_SKILL_LEVEL, GSOTreasureHuntSkillLevelRefSet.assetPath, GSOTreasureHuntSkillLevelRefSet.objName
				, "treasure_hunt_skill_level", "太空寻宝 - 技能等级表（treasure_hunt_skill_level）", _judgeTagCanShow));

			//太空寻宝 - 组合图鉴表
			_regMenuItem(new _TNPAutoExportRefMenu<TreasureHuntCompositeCatalogRefObj, GSOTreasureHuntCompositeCatalogRefSet>
			(ENPExportSettingEnum.TREASURE_HUNT_COMPOSITE_CATALOG, GSOTreasureHuntCompositeCatalogRefSet.assetPath, GSOTreasureHuntCompositeCatalogRefSet.objName
				, "treasure_hunt_composite_catalog", "太空寻宝 - 组合图鉴表（treasure_hunt_composite_catalog）", _judgeTagCanShow));

			//杰出者大厅
			_regMenuItem(new _TNPAutoExportRefMenu<GraveMainRefObj, GSOGraveMainRefSet>
			(ENPExportSettingEnum.GRAVE_MAIN, GSOGraveMainRefSet.assetPath, GSOGraveMainRefSet.objName
				, "grave_main", "杰出者大厅（grave_main）", _judgeTagCanShow));

			//杰出者类型
			_regMenuItem(new _TNPAutoExportRefMenu<GraveTypeRefObj, GSOGraveTypeRefSet>
			(ENPExportSettingEnum.GRAVE_TYPE, GSOGraveTypeRefSet.assetPath, GSOGraveTypeRefSet.objName
				, "grave_type", "杰出者类型（grave_type）", _judgeTagCanShow));

			//玩家buff事件表
			_regMenuItem(new _TNPAutoExportRefMenu<PlayerBuffEventRefObj, GSOPlayerBuffEventRefSet>
			(ENPExportSettingEnum.PLAYER_BUFF_EVENT, GSOPlayerBuffEventRefSet.assetPath, GSOPlayerBuffEventRefSet.objName
				, "player_buff_event", "玩家buff事件表（player_buff_event）", _judgeTagCanShow));

			//寻宝区域距离表
			_regMenuItem(new _TNPAutoExportRefMenu<TreasureHuntAreaDistanceRefObj, GSOTreasureHuntAreaDistanceRefSet>
			(ENPExportSettingEnum.TREASURE_HUNT_AREA_DISTANCE, GSOTreasureHuntAreaDistanceRefSet.assetPath, GSOTreasureHuntAreaDistanceRefSet.objName
				, "treasure_hunt_area_distance", "寻宝区域距离表（treasure_hunt_area_distance）", _judgeTagCanShow));
			//联盟PVE副本
			_regMenuItem(new _TNPAutoExportRefMenu<GuildDungeonRefObj, GSOGuildDungeonRefSet>
			(ENPExportSettingEnum.GUILD_DUNGEON, GSOGuildDungeonRefSet.assetPath, GSOGuildDungeonRefSet.objName
				, "guild_dungeon", "联盟PVE副本（guild_dungeon）", _judgeTagCanShow));

			//联盟PVE副本等级表
			_regMenuItem(new _TNPAutoExportRefMenu<GuildDungeonLvlRefObj, GSOGuildDungeonLvlRefSet>
			(ENPExportSettingEnum.GUILD_DUNGEON_LVL, GSOGuildDungeonLvlRefSet.assetPath, GSOGuildDungeonLvlRefSet.objName
				, "guild_dungeon_lvl", "联盟PVE副本等级表（guild_dungeon_lvl）", _judgeTagCanShow));

			//联盟PVE副本怪物表
			_regMenuItem(new _TNPAutoExportRefMenu<GuildDungeonMonsterRefObj, GSOGuildDungeonMonsterRefSet>
			(ENPExportSettingEnum.GUILD_DUNGEON_MONSTER, GSOGuildDungeonMonsterRefSet.assetPath, GSOGuildDungeonMonsterRefSet.objName
				, "guild_dungeon_monster", "联盟PVE副本怪物表（guild_dungeon_monster）", _judgeTagCanShow));

			//联盟PVE副本怪物展示
			_regMenuItem(new _TNPAutoExportRefMenu<GuildDungeonMonsterShowRefObj, GSOGuildDungeonMonsterShowRefSet>
			(ENPExportSettingEnum.GUILD_DUNGEON_MONSTER_SHOW, GSOGuildDungeonMonsterShowRefSet.assetPath, GSOGuildDungeonMonsterShowRefSet.objName
				, "guild_dungeon_monster_show", "联盟PVE副本怪物展示（guild_dungeon_monster_show）", _judgeTagCanShow));

			//系统任务组表
			_regMenuItem(new _TNPAutoExportRefMenu<SystemQuestGroupRefObj, GSOSystemQuestGroupRefSet>
			(ENPExportSettingEnum.SYSTEM_QUEST_GROUP, GSOSystemQuestGroupRefSet.assetPath, GSOSystemQuestGroupRefSet.objName
				, "system_quest_group", "系统任务组表（system_quest_group）", _judgeTagCanShow));

			//火星所有建筑表
			_regMenuItem(new _TNPAutoExportRefMenu<MarsAllBuildingRefObj, GSOMarsAllBuildingRefSet>
			(ENPExportSettingEnum.MARS_ALL_BUILDING, GSOMarsAllBuildingRefSet.assetPath, GSOMarsAllBuildingRefSet.objName
				, "mars_all_building", "火星所有建筑表（mars_all_building）", _judgeTagCanShow));
			
			//火星建筑
			_regMenuItem(new _TNPAutoExportRefMenu<MarsBuildingRefObj, GSOMarsBuildingRefSet>
			(ENPExportSettingEnum.MARS_BUILDING, GSOMarsBuildingRefSet.assetPath, GSOMarsBuildingRefSet.objName
				, "mars_building", "火星建筑（mars_building）", _judgeTagCanShow));

            //火星建筑等级
            _regMenuItem(new _TNPAutoExportRefMenu<MarsBuildingLevelRefObj, GSOMarsBuildingLevelRefSet>
            (ENPExportSettingEnum.MARS_BUILDING_LEVEL, GSOMarsBuildingLevelRefSet.assetPath, GSOMarsBuildingLevelRefSet.objName
                , "mars_building_level", "火星建筑等级（mars_building_level）", _judgeTagCanShow));

            //火星主基地等级
            _regMenuItem(new _TNPAutoExportRefMenu<MarsBuildingHomeLevelRefObj, GSOMarsBuildingHomeLevelRefSet>
			(ENPExportSettingEnum.MARS_BUILDING_HOME_LEVEL, GSOMarsBuildingHomeLevelRefSet.assetPath, GSOMarsBuildingHomeLevelRefSet.objName
				, "mars_building_home_level", "火星主基地等级（mars_building_home_level）", _judgeTagCanShow));

			//火星建筑部件归属
			_regMenuItem(new _TNPAutoExportRefMenu<MarsBuildingEquipmentBelongRefObj, GSOMarsBuildingEquipmentBelongRefSet>
			(ENPExportSettingEnum.MARS_BUILDING_EQUIPMENT_BELONG, GSOMarsBuildingEquipmentBelongRefSet.assetPath, GSOMarsBuildingEquipmentBelongRefSet.objName
				, "mars_building_equipment_belong", "火星建筑部件归属（mars_building_equipment_belong）", _judgeTagCanShow));

			//火星建筑派遣等级
			_regMenuItem(new _TNPAutoExportRefMenu<MarsBuildingSettleLevelRefObj, GSOMarsBuildingSettleLevelRefSet>
			(ENPExportSettingEnum.MARS_BUILDING_SETTLE_LEVEL, GSOMarsBuildingSettleLevelRefSet.assetPath, GSOMarsBuildingSettleLevelRefSet.objName
				, "mars_building_settle_level", "火星建筑派遣等级（mars_building_settle_level）", _judgeTagCanShow));

			//火星部件
			_regMenuItem(new _TNPAutoExportRefMenu<MarsEquipmentRefObj, GSOMarsEquipmentRefSet>
			(ENPExportSettingEnum.MARS_EQUIPMENT, GSOMarsEquipmentRefSet.assetPath, GSOMarsEquipmentRefSet.objName
				, "mars_equipment", "火星部件（mars_equipment）", _judgeTagCanShow));

			//火星部件等级
			_regMenuItem(new _TNPAutoExportRefMenu<MarsEquipmentLevelRefObj, GSOMarsEquipmentLevelRefSet>
			(ENPExportSettingEnum.MARS_EQUIPMENT_LEVEL, GSOMarsEquipmentLevelRefSet.assetPath, GSOMarsEquipmentLevelRefSet.objName
				, "mars_equipment_level", "火星部件等级（mars_equipment_level）", _judgeTagCanShow));

			//火星部件能源等级
			_regMenuItem(new _TNPAutoExportRefMenu<MarsEquipmentEnergyLevelRefObj, GSOMarsEquipmentEnergyLevelRefSet>
			(ENPExportSettingEnum.MARS_EQUIPMENT_ENERGY_LEVEL, GSOMarsEquipmentEnergyLevelRefSet.assetPath, GSOMarsEquipmentEnergyLevelRefSet.objName
				, "mars_equipment_energy_level", "火星部件能源等级（mars_equipment_energy_level）", _judgeTagCanShow));

			//火星部件生活等级
			_regMenuItem(new _TNPAutoExportRefMenu<MarsEquipmentLivingLevelRefObj, GSOMarsEquipmentLivingLevelRefSet>
			(ENPExportSettingEnum.MARS_EQUIPMENT_LIVING_LEVEL, GSOMarsEquipmentLivingLevelRefSet.assetPath, GSOMarsEquipmentLivingLevelRefSet.objName
				, "mars_equipment_living_level", "火星部件生活等级（mars_equipment_living_level）", _judgeTagCanShow));

			//火星部件食物等级
			_regMenuItem(new _TNPAutoExportRefMenu<MarsEquipmentFoodLevelRefObj, GSOMarsEquipmentFoodLevelRefSet>
			(ENPExportSettingEnum.MARS_EQUIPMENT_FOOD_LEVEL, GSOMarsEquipmentFoodLevelRefSet.assetPath, GSOMarsEquipmentFoodLevelRefSet.objName
				, "mars_equipment_food_level", "火星部件食物等级（mars_equipment_food_level）", _judgeTagCanShow));

			//火星部件医院等级
			_regMenuItem(new _TNPAutoExportRefMenu<MarsEquipmentHospitalLevelRefObj, GSOMarsEquipmentHospitalLevelRefSet>
			(ENPExportSettingEnum.MARS_EQUIPMENT_HOSPITAL_LEVEL, GSOMarsEquipmentHospitalLevelRefSet.assetPath, GSOMarsEquipmentHospitalLevelRefSet.objName
				, "mars_equipment_hospital_level", "火星部件医院等级（mars_equipment_hospital_level）", _judgeTagCanShow));

			//背包物品时间减少
			_regMenuItem(new _TNPAutoExportRefMenu<MarsBagItemTimeReduceRefObj, GSOMasrBagItemTimeReduceRefSet>
			(ENPExportSettingEnum.MARS_BAG_ITEM_TIME_REDUCE, GSOMasrBagItemTimeReduceRefSet.assetPath, GSOMasrBagItemTimeReduceRefSet.objName
				, "mars_bag_item_time_reduce", "火星背包物品时间减少（mars_bag_item_time_reduce）", _judgeTagCanShow));
				
			//火星航行表
			_regMenuItem(new _TNPAutoExportRefMenu<MarsGoRouteRefObj, GSOMarsGoRouteRefSet>
			(ENPExportSettingEnum.MARS_GO_ROUTE, GSOMarsGoRouteRefSet.assetPath, GSOMarsGoRouteRefSet.objName
				, "mars_go_route", "火星航行表（mars_go_route）", _judgeTagCanShow));

			//火星航行日志表
			_regMenuItem(new _TNPAutoExportRefMenu<MarsGoRouteLogRefObj, GSOMarsGoRouteLogRefSet>
			(ENPExportSettingEnum.MARS_GO_ROUTE_LOG, GSOMarsGoRouteLogRefSet.assetPath, GSOMarsGoRouteLogRefSet.objName
				, "mars_go_route_log", "火星航行日志表（mars_go_route_log）", _judgeTagCanShow));

			//火星智能控制表
			_regMenuItem(new _TNPAutoExportRefMenu<MarsIntelligentControlRefObj, GSOMarsIntelligentControlRefSet>
			(ENPExportSettingEnum.MARS_INTELLIGENT_CONTROL, GSOMarsIntelligentControlRefSet.assetPath, GSOMarsIntelligentControlRefSet.objName
				, "mars_intelligent_control", "火星智能控制表（mars_intelligent_control）", _judgeTagCanShow));
			
			//火星满意度表
			_regMenuItem(new _TNPAutoExportRefMenu<MarsSatisfactionDegreeRefObj, GSOMarsSatisfactionDegreeRefSet>
			(ENPExportSettingEnum.MARS_SATISFACTION_DEGREE, GSOMarsSatisfactionDegreeRefSet.assetPath, GSOMarsSatisfactionDegreeRefSet.objName
				, "mars_satisfaction_degree", "火星满意度表（mars_satisfaction_degree）", _judgeTagCanShow));

			//火星人民信件表
			_regMenuItem(new _TNPAutoExportSQLiteMenuItem<MarsPeopleLetterRefObj>
			(ENPExportSettingEnum.MARS_PEOPLE_LETTER, MarsPeopleLetterRefObj.objName, MarsPeopleLetterRefObj.tableName, MarsPeopleLetterRefObj.assetPath
				, "mars_people_letter", "火星人民信件表（mars_people_letter）", _judgeTagCanShow));

			//火星居民求助表
			_regMenuItem(new _TNPAutoExportRefMenu<MarsPeopleHelpRefObj, GSOMarsPeopleHelpRefSet>
			(ENPExportSettingEnum.MARS_PEOPLE_HELP, GSOMarsPeopleHelpRefSet.assetPath, GSOMarsPeopleHelpRefSet.objName
				, "mars_people_help", "火星居民求助表（mars_people_help）", _judgeTagCanShow));

			//火星居民选择求助表
			_regMenuItem(new _TNPAutoExportRefMenu<MarsPeopleChoiceHelpRefObj, GSOMarsPeopleChoiceHelpRefSet>
			(ENPExportSettingEnum.MARS_PEOPLE_CHOICE_HELP, GSOMarsPeopleChoiceHelpRefSet.assetPath, GSOMarsPeopleChoiceHelpRefSet.objName
				, "mars_people_choice_help", "火星居民选择求助表（mars_people_choice_help）", _judgeTagCanShow));

			//火星居民奖励求助表
			_regMenuItem(new _TNPAutoExportRefMenu<MarsPeopleRewardHelpRefObj, GSOMarsPeopleRewardHelpRefSet>
			(ENPExportSettingEnum.MARS_PEOPLE_REWARD_HELP, GSOMarsPeopleRewardHelpRefSet.assetPath, GSOMarsPeopleRewardHelpRefSet.objName
				, "mars_people_reward_help", "火星居民奖励求助表（mars_people_reward_help）", _judgeTagCanShow));

			//火星基地事件表
			_regMenuItem(new _TNPAutoExportRefMenu<MarsEventRefObj, GSOMarsEventRefSet>
			(ENPExportSettingEnum.MARS_EVENT, GSOMarsEventRefSet.assetPath, GSOMarsEventRefSet.objName
				, "mars_event", "火星基地事件表（mars_event）", _judgeTagCanShow));

			// 火星基地事件触发概率表
			_regMenuItem(new NPOnlyServerRefExportMenu("mars_event_trigger_per", ENPExportSettingEnum.MARS_EVENT_TRIGGER_PER, "mars_event_trigger_per 火星基地事件触发概率表", _judgeTagCanShow));
				
			//火星移民表
			_regMenuItem(new _TNPAutoExportRefMenu<MarsImmigrationRefObj, GSOMarsImmigrationRefSet>
			(ENPExportSettingEnum.MARS_IMMIGRATION, GSOMarsImmigrationRefSet.assetPath, GSOMarsImmigrationRefSet.objName
				, "mars_immigration", "火星移民表（mars_immigration）", _judgeTagCanShow));

			// 火星移民概率表（仅服务端）
			_regMenuItem(new NPOnlyServerRefExportMenu("mars_immigration_per", ENPExportSettingEnum.MARS_IMMIGRATION_PER, "mars_immigration_per 火星移民概率表", _judgeTagCanShow));
			
			//妃子朋友圈背景组表
			_regMenuItem(new _TNPAutoExportRefMenu<ConsortMomentsBgGroupRefObj, GSOConsortMomentsBgGroupRefSet>
			(ENPExportSettingEnum.CONSORT_MOMENTS_BG_GROUP, GSOConsortMomentsBgGroupRefSet.assetPath, GSOConsortMomentsBgGroupRefSet.objName
				, "consort_moments_bg_group", "妃子朋友圈背景组表（consort_moments_bg_group）", _judgeTagCanShow));

			//妃子朋友圈妃子图片组表
			_regMenuItem(new _TNPAutoExportRefMenu<ConsortMomentsConsortGroupRefObj, GSOConsortMomentsConsortGroupRefSet>
			(ENPExportSettingEnum.CONSORT_MOMENTS_CONSORT_GROUP, GSOConsortMomentsConsortGroupRefSet.assetPath, GSOConsortMomentsConsortGroupRefSet.objName
				, "consort_moments_consort_group", "妃子朋友圈妃子图片组表（consort_moments_consort_group）", _judgeTagCanShow));

			//妃子朋友圈妃子图片表
			_regMenuItem(new _TNPAutoExportRefMenu<ConsortMomentsConsortRefObj, GSOConsortMomentsConsortRefSet>
			(ENPExportSettingEnum.CONSORT_MOMENTS_CONSORT, GSOConsortMomentsConsortRefSet.assetPath, GSOConsortMomentsConsortRefSet.objName
				, "consort_moments_consort", "妃子朋友圈妃子图片表（consort_moments_consort）", _judgeTagCanShow));

			//妃子朋友圈背景图片表
			_regMenuItem(new _TNPAutoExportRefMenu<ConsortMomentsBgRefObj, GSOConsortMomentsBgRefSet>
			(ENPExportSettingEnum.CONSORT_MOMENTS_BG, GSOConsortMomentsBgRefSet.assetPath, GSOConsortMomentsBgRefSet.objName
				, "consort_moments_bg", "妃子朋友圈背景图片表（consort_moments_bg）", _judgeTagCanShow));

			//公会协作区域表
			_regMenuItem(new _TNPAutoExportRefMenu<GuildCooperateAreaRefObj, GSOGuildCooperateAreaRefSet>
			(ENPExportSettingEnum.GUILD_COOPERATE_AREA, GSOGuildCooperateAreaRefSet.assetPath, GSOGuildCooperateAreaRefSet.objName
				, "guild_cooperate_area", "公会协作区域表（guild_cooperate_area）", _judgeTagCanShow));

			//公会协作据点表
			_regMenuItem(new _TNPAutoExportRefMenu<GuildCooperateAreaPosRefObj, GSOGuildCooperateAreaPosRefSet>
			(ENPExportSettingEnum.GUILD_COOPERATE_AREA_POS, GSOGuildCooperateAreaPosRefSet.assetPath, GSOGuildCooperateAreaPosRefSet.objName
				, "guild_cooperate_area_pos", "公会协作据点表（guild_cooperate_area_pos）", _judgeTagCanShow));
				
			//太空寻宝奇物产出表
			_regMenuItem(new _TNPAutoExportRefMenu<TreasureHuntTreasureOutputRefObj, GSOTreasureHuntTreasureOutputRefSet>
			(ENPExportSettingEnum.TREASURE_HUNT_TREASURE_OUTPUT, GSOTreasureHuntTreasureOutputRefSet.assetPath, GSOTreasureHuntTreasureOutputRefSet.objName
				, "treasure_hunt_treasure_output", "太空寻宝奇物产出表（treasure_hunt_treasure_output）", _judgeTagCanShow));

			//红点监听表
			_regMenuItem(new _TNPAutoExportRefMenu<RedMonitorRefObj, GSORedMonitorRefSet>
			(ENPExportSettingEnum.RED_MONITOR, GSORedMonitorRefSet.assetPath, GSORedMonitorRefSet.objName
				, "red_monitor", "红点监听表（red_monitor）", _judgeTagCanShow));
				
			//火星建筑建造条件表
			_regMenuItem(new _TNPAutoExportRefMenu<MarsBuildingConditionRefObj, GSOMarsBuildingConditionRefSet>
			(ENPExportSettingEnum.MARS_BUILDING_CONDITION, GSOMarsBuildingConditionRefSet.assetPath, GSOMarsBuildingConditionRefSet.objName
				, "mars_building_condition", "火星建筑建造条件表（mars_building_condition）", _judgeTagCanShow));

			//子嗣配音组表
			_regMenuItem(new _TNPAutoExportRefMenu<ChildVoiceGroupRefObj, GSOChildVoiceGroupRefSet>
			(ENPExportSettingEnum.CHILD_VOICE_GROUP, GSOChildVoiceGroupRefSet.assetPath, GSOChildVoiceGroupRefSet.objName
				, "child_voice_group", "子嗣配音组表（child_voice_group）", _judgeTagCanShow));

            //邮件计划表
            _regMenuItem(new NPOnlyServerRefExportMenu("mail_plan", ENPExportSettingEnum.MAIL_PLAN, "邮件计划表（mail_plan）", _judgeTagCanShow));

			//特殊客人选择表
			_regMenuItem(new _TNPAutoExportRefMenu<InnSpecialGuestChoiceRefObj, GSOInnSpecialGuestChoiceRefSet>
			(ENPExportSettingEnum.INN_SPECIAL_GUEST_CHOICE, GSOInnSpecialGuestChoiceRefSet.assetPath, GSOInnSpecialGuestChoiceRefSet.objName
				, "inn_special_guest_choice", "特殊客人选择表（inn_special_guest_choice）", _judgeTagCanShow));

			//特殊客人选择选项表
			_regMenuItem(new _TNPAutoExportRefMenu<InnSpecialGuestChoiceOptionRefObj, GSOInnSpecialGuestChoiceOptionRefSet>
			(ENPExportSettingEnum.INN_SPECIAL_GUEST_CHOICE_OPTION, GSOInnSpecialGuestChoiceOptionRefSet.assetPath, GSOInnSpecialGuestChoiceOptionRefSet.objName
				, "inn_special_guest_choice_option", "特殊客人选择选项表（inn_special_guest_choice_option）", _judgeTagCanShow));

            //VIP表
			_regMenuItem(new _TNPAutoExportRefMenu<VipRefObj, GSOVipRefSet>
			(ENPExportSettingEnum.VIP, GSOVipRefSet.assetPath, GSOVipRefSet.objName
				, "vip", "VIP表（vip）", _judgeTagCanShow));

			//火星探索等级表
			_regMenuItem(new _TNPAutoExportRefMenu<MarsExploreLvlRefObj, GSOMarsExploreLvlRefSet>
			(ENPExportSettingEnum.MARS_EXPLORE_LVL, GSOMarsExploreLvlRefSet.assetPath, GSOMarsExploreLvlRefSet.objName
				, "mars_explore_lvl", "火星探索等级表（mars_explore_lvl）", _judgeTagCanShow));

			//火星探索位置表
			_regMenuItem(new _TNPAutoExportRefMenu<MarsExplorePosRefObj, GSOMarsExplorePosRefSet>
			(ENPExportSettingEnum.MARS_EXPLORE_POS, GSOMarsExplorePosRefSet.assetPath, GSOMarsExplorePosRefSet.objName
				, "mars_explore_pos", "火星探索位置表（mars_explore_pos）", _judgeTagCanShow));

			//火星探索事件表
			_regMenuItem(new _TNPAutoExportRefMenu<MarsExploreEventRefObj, GSOMarsExploreEventRefSet>
			(ENPExportSettingEnum.MARS_EXPLORE_EVENT, GSOMarsExploreEventRefSet.assetPath, GSOMarsExploreEventRefSet.objName
				, "mars_explore_event", "火星探索事件表（mars_explore_event）", _judgeTagCanShow));

			//火星探索战斗事件表
			_regMenuItem(new _TNPAutoExportRefMenu<MarsExploreEventBattleRefObj, GSOMarsExploreEventBattleRefSet>
			(ENPExportSettingEnum.MARS_EXPLORE_EVENT_BATTLE, GSOMarsExploreEventBattleRefSet.assetPath, GSOMarsExploreEventBattleRefSet.objName
				, "mars_explore_event_battle", "火星探索战斗事件表（mars_explore_event_battle）", _judgeTagCanShow));

			//火星探索Boss事件表
			_regMenuItem(new _TNPAutoExportRefMenu<MarsExploreEventBossRefObj, GSOMarsExploreEventBossRefSet>
			(ENPExportSettingEnum.MARS_EXPLORE_EVENT_BOSS, GSOMarsExploreEventBossRefSet.assetPath, GSOMarsExploreEventBossRefSet.objName
				, "mars_explore_event_boss", "火星探索Boss事件表（mars_explore_event_boss）", _judgeTagCanShow));

			//火星探索队伍表
			_regMenuItem(new _TNPAutoExportRefMenu<MarsExploreTeamRefObj, GSOMarsExploreTeamRefSet>
			(ENPExportSettingEnum.MARS_EXPLORE_TEAM, GSOMarsExploreTeamRefSet.assetPath, GSOMarsExploreTeamRefSet.objName
				, "mars_explore_team", "火星探索队伍表（mars_explore_team）", _judgeTagCanShow));

			//火星科技表
			_regMenuItem(new _TNPAutoExportRefMenu<MarsTechnologyRefObj, GSOMarsTechnologyRefSet>
			(ENPExportSettingEnum.MARS_TECHNOLOGY, GSOMarsTechnologyRefSet.assetPath, GSOMarsTechnologyRefSet.objName
				, "mars_technology", "火星科技表（mars_technology）", _judgeTagCanShow));

			//火星科技等级表
			_regMenuItem(new _TNPAutoExportRefMenu<MarsTechnologyLevelRefObj, GSOMarsTechnologyLevelRefSet>
			(ENPExportSettingEnum.MARS_TECHNOLOGY_LEVEL, GSOMarsTechnologyLevelRefSet.assetPath, GSOMarsTechnologyLevelRefSet.objName
				, "mars_technology_level", "火星科技等级表（mars_technology_level）", _judgeTagCanShow));

            //礼包额外获得表
			_regMenuItem(new _TNPAutoExportRefMenu<GiftPackExtraGainRefObj, GSOGiftPackExtraGainRefSet>
			(ENPExportSettingEnum.GIFT_PACK_EXTRA_GAIN, GSOGiftPackExtraGainRefSet.assetPath, GSOGiftPackExtraGainRefSet.objName
				, "gift_pack_extra_gain", "礼包额外获得表（gift_pack_extra_gain）", _judgeTagCanShow));

			//首充天数表
			_regMenuItem(new _TNPAutoExportRefMenu<FirstRechargeDayRefObj, GSOFirstRechargeDayRefSet>
			(ENPExportSettingEnum.FIRST_RECHARGE_DAY, GSOFirstRechargeDayRefSet.assetPath, GSOFirstRechargeDayRefSet.objName
				, "first_recharge_day", "首充天数表（first_recharge_day）", _judgeTagCanShow));

            //充值返利组表
			_regMenuItem(new _TNPAutoExportRefMenu<RechargeRebateGroupRefObj, GSORechargeRebateGroupRefSet>
			(ENPExportSettingEnum.RECHARGE_REBATE_GROUP, GSORechargeRebateGroupRefSet.assetPath, GSORechargeRebateGroupRefSet.objName
				, "recharge_rebate_group", "充值返利组表（recharge_rebate_group）", _judgeTagCanShow));

			//充值返利阶段表
			_regMenuItem(new _TNPAutoExportRefMenu<RechargeRebateStepRefObj, GSORechargeRebateStepRefSet>
			(ENPExportSettingEnum.RECHARGE_REBATE_STEP, GSORechargeRebateStepRefSet.assetPath, GSORechargeRebateStepRefSet.objName
				, "recharge_rebate_step", "充值返利阶段表（recharge_rebate_step）", _judgeTagCanShow));
				
			//火星科技类型表
			_regMenuItem(new _TNPAutoExportRefMenu<MarsTechnologyTypeRefObj, GSOMarsTechnologyTypeRefSet>
			(ENPExportSettingEnum.MARS_TECHNOLOGY_TYPE, GSOMarsTechnologyTypeRefSet.assetPath, GSOMarsTechnologyTypeRefSet.objName
				, "mars_technology_type", "火星科技类型表（mars_technology_type）", _judgeTagCanShow));

			// 玩家属性展示表PlayerPropertyShowRefObj
			_regMenuItem(new _TNPAutoExportRefMenu<PlayerPropertyShowRefObj, GSOPlayerPropertyShowRefSet>
			(ENPExportSettingEnum.PLAYER_PROPERTY_SHOW, GSOPlayerPropertyShowRefSet.assetPath, GSOPlayerPropertyShowRefSet.objName
				, "player_property_show", "玩家属性展示表（player_property_show）", _judgeTagCanShow));
			
			// 火星属性展示表MarsPropertyShowRefObj
			_regMenuItem(new _TNPAutoExportRefMenu<MarsPropertyShowRefObj, GSOMarsPropertyShowRefSet>
			(ENPExportSettingEnum.MARS_PROPERTY_SHOW, GSOMarsPropertyShowRefSet.assetPath, GSOMarsPropertyShowRefSet.objName
				, "mars_property_show", "火星属性展示表（mars_property_show）", _judgeTagCanShow));
			
			//火星探索矿点表
			_regMenuItem(new _TNPAutoExportRefMenu<MarsExploreMineRefObj, GSOMarsExploreMineRefSet>
			(ENPExportSettingEnum.MARS_EXPLORE_MINE, GSOMarsExploreMineRefSet.assetPath, GSOMarsExploreMineRefSet.objName
				, "mars_explore_mine", "火星探索矿点表（mars_explore_mine）", _judgeTagCanShow));

			//火星道具时间类型表
			_regMenuItem(new _TNPAutoExportRefMenu<MarsBagItemTimeTypeRefObj, GSOMarsBagItemTimeTypeRefSet>
			(ENPExportSettingEnum.MARS_BAG_ITEM_TIME_TYPE, GSOMarsBagItemTimeTypeRefSet.assetPath, GSOMarsBagItemTimeTypeRefSet.objName
				, "mars_bag_item_time_type", "火星道具时间类型表（mars_bag_item_time_type）", _judgeTagCanShow));
				
			//火星探索收集奖励表
			_regMenuItem(new _TNPAutoExportRefMenu<MarsExploreCollectBonusRefObj, GSOMarsExploreCollectBonusRefSet>
			(ENPExportSettingEnum.MARS_EXPLORE_COLLECT_BONUS, GSOMarsExploreCollectBonusRefSet.assetPath, GSOMarsExploreCollectBonusRefSet.objName
				, "mars_explore_collect_bonus", "火星探索收集奖励表（mars_explore_collect_bonus）", _judgeTagCanShow));

            //权益卡表
			_regMenuItem(new _TNPAutoExportRefUniformMenu<PrivilegeCardRefObj, GSOPrivilegeCardRefSet>
			(ENPExportSettingEnum.PRIVILEGE_CARD, GSOPrivilegeCardRefSet.assetPath, GSOPrivilegeCardRefSet.objName
				, "privilege_card", "权益卡表（privilege_card）", ENPItemType.PRIVILEGE_CARD, _judgeTagCanShow));

			//玩家权限表
			_regMenuItem(new _TNPAutoExportRefMenu<PlayerPermissionsRefObj, GSOPlayerPermissionsRefSet>
			(ENPExportSettingEnum.PLAYER_PERMISSIONS, GSOPlayerPermissionsRefSet.assetPath, GSOPlayerPermissionsRefSet.objName
				, "player_permissions", "玩家权限表（player_permissions）", _judgeTagCanShow));

			//推送礼包组表
			_regMenuItem(new _TNPAutoExportRefMenu<PushGiftGroupRefObj, GSOPushGiftGroupRefSet>
			(ENPExportSettingEnum.PUSH_GIFT_GROUP, GSOPushGiftGroupRefSet.assetPath, GSOPushGiftGroupRefSet.objName
				, "push_gift_group", "推送礼包组表（push_gift_group）", _judgeTagCanShow));

			//推送礼包表
			_regMenuItem(new _TNPAutoExportRefMenu<PushGiftPackRefObj, GSOPushGiftPackRefSet>
			(ENPExportSettingEnum.PUSH_GIFT_PACK, GSOPushGiftPackRefSet.assetPath, GSOPushGiftPackRefSet.objName
				, "push_gift_pack", "推送礼包表（push_gift_pack）", _judgeTagCanShow));

			//缺少物品触发推送礼包表
			_regMenuItem(new _TNPAutoExportRefMenu<PushGiftItemTriggerRefObj, GSOPushGiftItemTriggerRefSet>
			(ENPExportSettingEnum.PUSH_GIFT_ITEM_TRIGGER, GSOPushGiftItemTriggerRefSet.assetPath, GSOPushGiftItemTriggerRefSet.objName
				, "push_gift_item_trigger", "缺少物品触发推送礼包表（push_gift_item_trigger）", _judgeTagCanShow));

			//联盟宝箱
			_regMenuItem(new _TNPAutoExportRefUniformMenu<GuildBoxRefObj, GSOGuildBoxRefSet>
			(ENPExportSettingEnum.GUILD_BOX, GSOGuildBoxRefSet.assetPath, GSOGuildBoxRefSet.objName
				, "guild_box", "联盟宝箱（guild_box）", ENPItemType.GUILD_BOX, _judgeTagCanShow));

			//玩家永久加成表
			_regMenuItem(new _TNPAutoExportRefUniformMenu<PlayerForeverAddRefObj, GSOPlayerForeverAddRefSet>
			(ENPExportSettingEnum.PLAYER_FOREVER_ADD, GSOPlayerForeverAddRefSet.assetPath, GSOPlayerForeverAddRefSet.objName
				, "player_forever_add", "玩家永久加成表（player_forever_add）", ENPItemType.FOREVER_ADD, _judgeTagCanShow));

            //联盟宝箱事件表
            _regMenuItem(new NPOnlyServerRefExportMenu("guild_box_event", ENPExportSettingEnum.GUILD_BOX_EVENT, "联盟宝箱事件表（guild_box_event）", _judgeTagCanShow));

            //午间副本宝箱
			_regMenuItem(new _TNPAutoExportRefMenu<MiddayDungeonBoxRefObj, GSOMiddayDungeonBoxRefSet>
			(ENPExportSettingEnum.MIDDAY_DUNGEON_BOX, GSOMiddayDungeonBoxRefSet.assetPath, GSOMiddayDungeonBoxRefSet.objName
				, "midday_dungeon_box", "午间副本宝箱（midday_dungeon_box）", _judgeTagCanShow));

			//聊天系统通知表
			_regMenuItem(new _TNPAutoExportRefMenu<ChatSystemLogRefObj, GSOChatSystemLogRefSet>
			(ENPExportSettingEnum.CHAT_SYSTEM_LOG, GSOChatSystemLogRefSet.assetPath, GSOChatSystemLogRefSet.objName
				, "chat_system_log", "聊天系统通知表（chat_system_log）", _judgeTagCanShow));

			//限时兑换
			_regMenuItem(new _TNPAutoExportRefMenu<RushExchangeRefObj, GSORushExchangeRefSet>
			(ENPExportSettingEnum.RUSH_EXCHANGE, GSORushExchangeRefSet.assetPath, GSORushExchangeRefSet.objName
				, "rush_exchange", "限时兑换（rush_exchange）", _judgeTagCanShow));

			//限时兑换物品价值
			_regMenuItem(new _TNPAutoExportRefMenu<RushExchangeItemRefObj, GSORushExchangeItemRefSet>
			(ENPExportSettingEnum.RUSH_EXCHANGE_ITEM, GSORushExchangeItemRefSet.assetPath, GSORushExchangeItemRefSet.objName
				, "rush_exchange_item", "限时兑换物品价值（rush_exchange_item）", _judgeTagCanShow));

			//限时兑换组
			_regMenuItem(new _TNPAutoExportRefMenu<RushExchangeGroupRefObj, GSORushExchangeGroupRefSet>
			(ENPExportSettingEnum.RUSH_EXCHANGE_GROUP, GSORushExchangeGroupRefSet.assetPath, GSORushExchangeGroupRefSet.objName
				, "rush_exchange_group", "限时兑换组（rush_exchange_group）", _judgeTagCanShow));

			//玩家卧室皮肤表
			_regMenuItem(new _TNPAutoExportRefUniformMenu<PlayerRoomSkinRefObj, GSOPlayerRoomSkinRefSet>
			(ENPExportSettingEnum.PLAYER_ROOM_SKIN, GSOPlayerRoomSkinRefSet.assetPath, GSOPlayerRoomSkinRefSet.objName
				, "player_room_skin", "玩家卧室皮肤表（player_room_skin）", ENPItemType.ROOM_SKIN, _judgeTagCanShow));

			//活动组队表
			_regMenuItem(new _TNPAutoExportRefMenu<ActivityTeamRefObj, GSOActivityTeamRefSet>
			(ENPExportSettingEnum.ACTIVITY_TEAM, GSOActivityTeamRefSet.assetPath, GSOActivityTeamRefSet.objName
				, "activity_team", "活动组队表（activity_team）", _judgeTagCanShow));

			//通用表现组主表
			_regMenuItem(new _TNPAutoExportRefMenu<PerformGroupRefObj, GSOPerformGroupRefSet>
			(ENPExportSettingEnum.PERFORM_GROUP, GSOPerformGroupRefSet.assetPath, GSOPerformGroupRefSet.objName
				, "perform_group", "通用表现组主表（perform_group）", _judgeTagCanShow));

			//通用表现组子项表
			_regMenuItem(new _TNPAutoExportRefMenu<PerformGroupItemRefObj, GSOPerformGroupItemRefSet>
			(ENPExportSettingEnum.PERFORM_GROUP_ITEM, GSOPerformGroupItemRefSet.assetPath, GSOPerformGroupItemRefSet.objName
				, "perform_group_item", "通用表现组子项表（perform_group_item）", _judgeTagCanShow));

			//通用表现组对话类型子表
			_regMenuItem(new _TNPAutoExportRefMenu<PerformGroupDialogueRefObj, GSOPerformGroupDialogueRefSet>
			(ENPExportSettingEnum.PERFORM_GROUP_DIALOGUE, GSOPerformGroupDialogueRefSet.assetPath, GSOPerformGroupDialogueRefSet.objName
				, "perform_group_dialogue", "通用表现组对话类型子表（perform_group_dialogue）", _judgeTagCanShow));

			//>>>>>>>>>> AUTO GENERATE END   <<<<<<<<<<
			//基金主表
			_regMenuItem(new _TNPAutoExportRefMenu<ActivityFundRefObj, GSOActivityFundRefSet>
			(ENPExportSettingEnum.ACTIVITY_FUND, GSOActivityFundRefSet.assetPath, GSOActivityFundRefSet.objName
				, "activity_fund", "基金主表（activity_fund）", _judgeTagCanShow));

			//基金等级表
			_regMenuItem(new _TNPAutoExportRefMenu<ActivityFundLevelRefObj, GSOActivityFundLevelRefSet>
			(ENPExportSettingEnum.ACTIVITY_FUND_LEVEL, GSOActivityFundLevelRefSet.assetPath, GSOActivityFundLevelRefSet.objName
				, "activity_fund_level", "基金等级表（activity_fund_level）", _judgeTagCanShow));

			//基金阶段表
			_regMenuItem(new _TNPAutoExportRefMenu<ActivityFundStepRefObj, GSOActivityFundStepRefSet>
			(ENPExportSettingEnum.ACTIVITY_FUND_STEP, GSOActivityFundStepRefSet.assetPath, GSOActivityFundStepRefSet.objName
				, "activity_fund_step", "基金阶段表（activity_fund_step）", _judgeTagCanShow));

			//基金任务表
			_regMenuItem(new _TNPAutoExportRefMenu<ActivityFundTaskRefObj, GSOActivityFundTaskRefSet>
			(ENPExportSettingEnum.ACTIVITY_FUND_TASK, GSOActivityFundTaskRefSet.assetPath, GSOActivityFundTaskRefSet.objName
				, "activity_fund_task", "基金任务表（activity_fund_task）", _judgeTagCanShow));

            //>>>>>>>>>> AUTO GENERATE END   <<<<<<<<<<

            if (_m_eaiExportAllItem != null)
	            _m_eaiExportAllItem.regExportFunc(()=>
	            {
	                NPExportSettingMgr.instance.exeExportAllSelect();
	                AssetDatabase.Refresh();
	            });

	        if (_m_saiSelectAllItem != null)
	            _m_saiSelectAllItem.regSelectFunc(NPExportSettingMgr.instance.setIsSelectAll);

	        NPExportSettingMgr.instance.onSelectAllChg += _onSelectAllChg;
	        NPExportSettingMgr.instance.onSubExportMenuSelectChg += _onSubExportMenuSelectChg;
	    }

	    /** 判断标记是否可视 */
	    protected bool _judgeTagCanShow(string _tag,string _excelFileName)
	    {
	        if (null == _m_tiSiftTextItem || string.IsNullOrEmpty(_m_tiSiftTextItem.inputText))
	            return true;

	        //按照分号分隔不同标记，判断标记或文件名是否包含
	        string[] strs = _m_tiSiftTextItem.inputText.Split(new string[] { ";" }, StringSplitOptions.None);

	        for (int i = 0; i < strs.Length; i++)
	        {
	            if (!string.IsNullOrEmpty(strs[i]) && (_tag.ToLowerInvariant().Contains(strs[i].ToLowerInvariant()) || _excelFileName.ToLowerInvariant().Contains(strs[i].ToLowerInvariant())))
	                return true;
	        }

	        return false;
	    }

	    private void _onSelectAllChg(bool _isOn)
	    {
	        _m_saiSelectAllItem.setToggleValue(_isOn);
	    }

	    //子导出菜单选择toggle改变事件处理
	    private void _onSubExportMenuSelectChg(ENPExportSettingEnum _type, bool _isOn)
	    {

	    }



	    [MenuItem("NPAssets/数据配表导出/打开导出窗口 %#J")]
	    static void ResExportWnd()
	    {
	        //创建窗口
	        Rect wr = new Rect(100, 100, 800, 800);
	        ALExportWnd.showExportWnd<NPExportWnd>(wr, "NP Excel资源导出窗口");
	        NPExportSettingMgr.instance.resetData();
	        NPExportSettingMgr.instance.loadLocalSave();
	    }

	    /***********
	     * excel读取的相关处理文件
	     **/
	    public class NPExcelReadInfo
	    {
	        private FileStream _m_fFileStream;
	        private string _m_sSrcPath;
	        private string _m_sTmpFilePath;
	        private IExcelDataReader _m_iExcelReader;

	        public NPExcelReadInfo()
	        {
	            _m_fFileStream = null;
	            _m_sSrcPath = string.Empty;
	            _m_sTmpFilePath = string.Empty;
	            _m_iExcelReader = null;
	        }

	        public string srcPath { get { return _m_sSrcPath; } }
	        public IExcelDataReader excelReader { get { return _m_iExcelReader; } }
	        public void init(string _path)
	        {
	            _m_sSrcPath = _path;
	            string exitension = Path.GetExtension(_path);
	            //开启文件并进行读取
	            _m_sTmpFilePath = _path.Replace(exitension, ".tmpX");
	            //拷贝一个文件，避免开启excel时无法读取的问题
	            File.Copy(_path, _m_sTmpFilePath, true);
	            //开启文件并进行读取
	            _m_fFileStream = File.OpenRead(_m_sTmpFilePath);
	            _m_iExcelReader = ExcelReaderFactory.CreateOpenXmlReader(_m_fFileStream);
	            Debug.Log("path: " + _path);
	        }

	        /*********
	         * 释放相关资源
	         **/
	        public void discard()
	        {
	            if (null == _m_fFileStream)
	                return;

	            //释放文件资源
	            _m_fFileStream.Close();
	            _m_fFileStream.Dispose();
	            _m_iExcelReader.Dispose();
	            _m_fFileStream = null;
	            _m_iExcelReader = null;

	            //删除拷贝的文件
	            File.Delete(_m_sTmpFilePath);
	            _m_sSrcPath = string.Empty;
	            _m_sTmpFilePath = string.Empty;
	        }
	    }

	    /******************
	     * 读取excel文件的具体函数，带入的回调为对每个格子进行处理的处理函数
	     * 回调带入的参数为 定义结构体对象，格子列名称，格子内容
	     **/
	    public static NPExcelReadInfo readExcel(string _path)
	    {
	        NPExcelReadInfo excelInfo = new NPExcelReadInfo();
	        excelInfo.init(_path);

	        return excelInfo;
	    }

	    /**************
	     * 读取操作类
	     **/
	    public class NPExcelReadingFuncInfo<T> : _ANPExcelReadingBasicClass
	    {
	        public List<T> dataList;
	        public Func<T> createDelegate;
	        public Action<T, int, Dictionary<string, string>> readDelegate;

	        public NPExcelReadingFuncInfo(string _sheetName, ENPExportSettingEnum _exportEnum, Func<T> _createDelegate, Action<T, int, Dictionary<string, string>> _readDelegate, bool _exportTxt = true)
	            : base(_sheetName, _exportEnum, _exportTxt, new List<T>())
	        {
	            //将基类数据转化过来
	            dataList = (List<T>)recList;
	            exportEnum = _exportEnum;
	            createDelegate = _createDelegate;
	            readDelegate = _readDelegate;
	            exportTxt = _exportTxt;
	        }

	        protected override object _createObj()
	        {
	            if (null == createDelegate)
	                return null;

	            return createDelegate();
	        }
	        protected override void _readFunc(object _obj, int _lineIdx, Dictionary<string, string> _lineInfo)
	        {
	            if (null == readDelegate)
	                return;

	            readDelegate((T)_obj, _lineIdx, _lineInfo);
	        }
	    }
	    public abstract class _ANPExcelReadingBasicClass
	    {
	        public string sheetName;
	        public ENPExportSettingEnum exportEnum;
	        public bool exportTxt;
	        public System.Collections.IList recList;

	        public _ANPExcelReadingBasicClass(string _sheetName, ENPExportSettingEnum _exportEnum, bool _exportTxt, System.Collections.IList _recList)
	        {
	            sheetName = _sheetName;
	            exportEnum = _exportEnum;
	            exportTxt = _exportTxt;
	            recList = _recList;
	        }

	        /*********
	         * 处理读取操作
	         **/
	        public object dealReadingFunc(int _lineIdx, Dictionary<string, string> _lineInfo)
	        {
	            object newObj = _createObj();
	            _readFunc(newObj, _lineIdx, _lineInfo);

	            return newObj;
	        }

	        protected abstract object _createObj();
	        protected abstract void _readFunc(object _obj, int _lineIdx, Dictionary<string, string> _lineInfo);
	    }
	    public class NPExcelReadingDealObj
	    {
	        private Dictionary<string, _ANPExcelReadingBasicClass> _m_dicDealDic;

	        public NPExcelReadingDealObj()
	        {
	            _m_dicDealDic = new Dictionary<string, _ANPExcelReadingBasicClass>();
	        }

	        //添加处理对象
	        public void addDealObj(_ANPExcelReadingBasicClass _dealObj)
	        {
	            if (null == _dealObj)
	                return;

	            if (_m_dicDealDic.ContainsKey(_dealObj.sheetName.ToLowerInvariant()))
	            {
	                UnityEngine.Debug.LogError("Multiplie deal sheet: " + _dealObj.sheetName);
	                return;
	            }

	            _m_dicDealDic.Add(_dealObj.sheetName.ToLowerInvariant(), _dealObj);
	        }
	        /**********
	         * 获取对应页签的处理对象
	         **/
	        public _ANPExcelReadingBasicClass getDealObj(string _sheetName)
	        {
	            if (!_m_dicDealDic.ContainsKey(_sheetName.ToLowerInvariant()))
	                return null;

	            return _m_dicDealDic[_sheetName.ToLowerInvariant()];
	        }
	    }

	    /******************
	     * 读取excel文件的具体函数，带入的回调为对每个格子进行处理的处理函数
	     * 回调带入的参数为 定义结构体对象，格子列名称，格子内容
	     **/
	    public static void readXls(string _path, NPExcelReadingDealObj _readingDealObj)
	    {
	        //判断带入的函数对象是否有效
	        if (null == _readingDealObj)
	            return;

	        string exitension = Path.GetExtension(_path);

	        //开启文件并进行读取
	        string tmpPath = _path.Replace(exitension, ".tmpX");
	        //拷贝一个文件，避免开启excel时无法读取的问题
	        File.Copy(_path, tmpPath, true);
	        //开启文件并进行读取
	        FileStream stream = File.OpenRead(tmpPath);
	        long s = ALCommon.getNowTimeMill();
	        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
	        UnityEngine.Debug.Log("read file spend ms: " + (ALCommon.getNowTimeMill() - s));
	        s = ALCommon.getNowTimeMill();
	        Debug.Log("path: " + _path);

	        do
	        {
	            //判断sheet name是否有读取对象
	            //获取处理对象
	            _ANPExcelReadingBasicClass readingObj = _readingDealObj.getDealObj(excelReader.Name);
	            if (null == readingObj)
	                continue;

	            //存储列名称,用于分析数据并进行读取
	            List<string> columnNameList = new List<string>();
	            int lineIdx = 0;
	            //对表进行处理
	            while (excelReader.Read())
	            {
	                if (1 == lineIdx)
	                {
	                    //取列名
	                    for (int i = 0; i < excelReader.FieldCount; i++)
	                    {
	                        //添加列名称, 第一行不能有空列，空列意味着隔断
	                        if (excelReader.IsDBNull(i))
	                        {
	                            break;
	                        }
	                        else
	                        {
	                            columnNameList.Add(excelReader.GetString(i));
	                        }

	                    }
	                }
	                else if (lineIdx > 1)
	                {
	                    //进行行处理
	                    Dictionary<string, string> lineData = new Dictionary<string, string>();
	                    int keyCount = columnNameList.Count;

	                    //记录本行是否有效
	                    bool isLineEnable = false;

	                    //当前行中的列数目，每列值进行处理
	                    for (int i = 0; i < excelReader.FieldCount; i++)
	                    {
	                        if (i >= keyCount)
	                            continue;

	                        if (excelReader.IsDBNull(i))
	                        {
	                            lineData[columnNameList[i].ToLowerInvariant()] = "";
	                            continue;
	                        }

	                        //获取对应值，进行后续处理
	                        string value = excelReader.GetString(i);
	                        if (i == 1 && (null != value && value.StartsWith("--")))
	                        {
	                            //设置本行无效
	                            isLineEnable = false;
	                            break;
	                        }

	                        try
	                        {
	                            //处理读取操作
	                            lineData[columnNameList[i].ToLowerInvariant()] = value;
	                            //设置本行有效
	                            isLineEnable = true;

	                        }
	                        catch (Exception ex)
	                        {
	                            Debug.LogError(ex.Message + " Error Position:" + "[" + (i + 1) + "," + (lineIdx + 1) + "]" + "----name:" + columnNameList[i] + "   value:" + value);
	                        }
	                    }

	                    //添加到结果集合
	                    if (isLineEnable)
	                    {
	                        //创建新的接受数据节点
	                        object dataObj = readingObj.dealReadingFunc(lineIdx, lineData);
	                        readingObj.recList.Add(dataObj);
	                    }
	                }

	                //增加行号
	                lineIdx++;
	            }

	            //导出Txt
	            if (readingObj.exportTxt)
	                WriteToTxtFile(_path, excelReader.Name.ToLowerInvariant(), readingObj.exportEnum);

	            if (ALCommon.getNowTimeMill() - s > 1000)
	                UnityEngine.Debug.LogWarning("read sheet: " + excelReader.Name + " spend ms: " + (ALCommon.getNowTimeMill() - s));
	            else
	                UnityEngine.Debug.Log("read sheet: " + excelReader.Name + " spend ms: " + (ALCommon.getNowTimeMill() - s));
	            s = ALCommon.getNowTimeMill();
	        } while (excelReader.NextResult());

	        //释放文件资源
	        stream.Close();
	        stream.Dispose();
	        excelReader.Dispose();
	        stream = null;
	        excelReader = null;

	        //删除拷贝的文件
	        File.Delete(tmpPath);
	    }

	    //获取页面列表
	    public static List<string> getAllSheetNameList(string _path)
	    {
	        string exitension = Path.GetExtension(_path);

	        //开启文件并进行读取
	        string tmpPath = _path.Replace(exitension, ".tmpX");
	        //拷贝一个文件，避免开启excel时无法读取的问题
	        File.Copy(_path, tmpPath, true);
	        //开启文件并进行读取
	        FileStream stream = File.OpenRead(tmpPath);
	        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
	        //创建结果队列
	        List<string> resList = new List<string>();

	        do
	        {
	            resList.Add(excelReader.Name);
	        } while (excelReader.NextResult());

	        return resList;
	    }

	    /******************
	     * 读取excel文件的具体函数，带入的回调为对每个格子进行处理的处理函数
	     * 回调带入的参数为 定义结构体对象，格子列名称，格子内容
	     **/
	    public static List<T> readXls<T>(string _path, string _sheetName, ENPExportSettingEnum _exportEnum, Action<T, int, Dictionary<string, string>> _readDelegate, bool _exportTxt = true)
	    {
	        //判断带入的函数对象是否有效
	        if (null == _readDelegate)
	            return null;
	        string exitension = Path.GetExtension(_path);

	        //开启文件并进行读取
	        string tmpPath = _path.Replace(exitension, ".tmpX");
	        //拷贝一个文件，避免开启excel时无法读取的问题
	        File.Copy(_path, tmpPath, true);
	        //开启文件并进行读取
	        FileStream stream = File.OpenRead(tmpPath);
	        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
	        Debug.Log("path: " + _path + "\t\t_sheetName:  " + _sheetName);
	        //创建结果队列
	        List<T> resList = new List<T>();

	        do
	        {
	            //判断sheet name是否data
	            if (excelReader.Name.Equals(_sheetName, StringComparison.OrdinalIgnoreCase))
	            {
	                Debug.Log("Start Read Sheet: " + excelReader.Name);
	                //存储列名称,用于分析数据并进行读取
	                List<string> columnNameList = new List<string>();
	                int lineIdx = 0;
	                //对表进行处理
	                while (excelReader.Read())
	                {
	                    if (1 == lineIdx)
	                    {
	                        //取列名
	                        for (int i = 0; i < excelReader.FieldCount; i++)
	                        {
	                            //添加列名称, 第一行不能有空列，空列意味着隔断
	                            if (excelReader.IsDBNull(i))
	                            {
	                                break;
	                            }
	                            else
	                            {
	                                columnNameList.Add(excelReader.GetString(i));
	                            }

	                        }
	                        //判断是否有字段，不存在columnNameList中，有的话说明配表少了列，报错
	                        //自动根据字段识别
	                        Type TempCls = typeof(T);
	                        //识别变量
	                        System.Reflection.FieldInfo[] fields = TempCls.GetFields();
	                        for (int i = 0; i < fields.Length; i++)
	                        {
	                            FieldInfo field = fields[i];
	                            if (!columnNameList.Contains(field.Name))
	                            {
	                                UnityEngine.Debug.LogError($"表格{_sheetName}缺少列:{field.Name}");
	                            }
	                        }
	                    }
	                    else if (lineIdx > 1)
	                    {

	                        //进行行处理
	                        //创建新的接受数据节点
	                        T dataObj = System.Activator.CreateInstance<T>();

	                        Dictionary<string, string> lineData = new Dictionary<string, string>();
	                        int keyCount = columnNameList.Count;

	                        //记录本行是否有效
	                        bool isLineEnable = false;

	                        //当前行中的列数目，每列值进行处理
	                        for (int i = 0; i < excelReader.FieldCount; i++)
	                        {
	                            if (i >= keyCount)
	                                continue;

	                            if (excelReader.IsDBNull(i))
	                            {
	                                lineData[columnNameList[i].ToLowerInvariant()] = "";
	                                continue;
	                            }

	                            //获取对应值，进行后续处理
	                            string value = excelReader.GetString(i);
	                            if (i == 1 && (null != value && value.StartsWith("--")))
	                            {
	                                //设置本行无效
	                                isLineEnable = false;
	                                break;
	                            }

	                            try
	                            {
	                                //处理读取操作
	                                lineData[columnNameList[i].ToLowerInvariant()] = value;
	                                //设置本行有效
	                                isLineEnable = true;

	                            }
	                            catch (Exception ex)
	                            {
	                                Debug.LogError(ex.Message + " Error Position:" + "[" + (i + 1) + "," + (lineIdx + 1) + "]" + "----name:" + columnNameList[i] + "   value:" + value);
	                            }
	                        }

	                        //添加到结果集合
	                        if (isLineEnable)
	                        {
	                            _readDelegate(dataObj, lineIdx, lineData);
	                            resList.Add(dataObj);
	                        }
	                    }

	                    //增加行号
	                    lineIdx++;
	                }
	            }
	        } while (excelReader.NextResult());

	        //释放文件资源
	        stream.Close();
	        stream.Dispose();
	        excelReader.Dispose();
	        stream = null;
	        excelReader = null;

	        //删除拷贝的文件
	        File.Delete(tmpPath);

	        return resList;
	    }
	    public static List<T> readXls<T>(string _path, string[] _sheetName, ENPExportSettingEnum _exportEnum, Action<T, int, Dictionary<string, string>> _readDelegate, bool _exportTxt = true)
	    {
	        //判断带入的函数对象是否有效
	        if (null == _readDelegate || null == _sheetName || _sheetName.Length <= 0)
	            return null;

	        string exitension = Path.GetExtension(_path);

	        //开启文件并进行读取
	        string tmpPath = _path.Replace(exitension, ".tmpX");
	        //拷贝一个文件，避免开启excel时无法读取的问题
	        File.Copy(_path, tmpPath, true);
	        //开启文件并进行读取
	        FileStream stream = File.OpenRead(tmpPath);
	        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
	        Debug.Log("path: " + _path + "\t\t_sheetName:  " + _sheetName);
	        //创建结果队列
	        List<T> resList = new List<T>();

	        do
	        {
	            string sheetName = excelReader.Name;

	            //判断sheet name是否data
	            bool fix = false;
	            for (int i = 0; i < _sheetName.Length; i++)
	            {
	                string tmpName = _sheetName[i];
	                if (null == tmpName)
	                    continue;

	                if (sheetName.Equals(tmpName, StringComparison.OrdinalIgnoreCase))
	                {
	                    fix = true;
	                    break;
	                }
	            }

	            //判断是否读取
	            if (fix)
	            {
	                Debug.Log("Start Read Sheet: " + excelReader.Name);
	                //存储列名称,用于分析数据并进行读取
	                List<string> columnNameList = new List<string>();
	                int lineIdx = 0;
	                //对表进行处理
	                while (excelReader.Read())
	                {
	                    if (1 == lineIdx)
	                    {
	                        //取列名
	                        for (int i = 0; i < excelReader.FieldCount; i++)
	                        {
	                            //添加列名称, 第一行不能有空列，空列意味着隔断
	                            if (excelReader.IsDBNull(i))
	                            {
	                                break;
	                            }
	                            else
	                            {
	                                columnNameList.Add(excelReader.GetString(i));
	                            }

	                        }
	                        //判断是否有字段，不存在columnNameList中，有的话说明配表少了列，报错
	                        //自动根据字段识别
	                        Type TempCls = typeof(T);
	                        //识别变量
	                        System.Reflection.FieldInfo[] fields = TempCls.GetFields();
	                        for (int i = 0; i < fields.Length; i++)
	                        {
	                            FieldInfo field = fields[i];
	                            if (!columnNameList.Contains(field.Name))
	                            {
	                                UnityEngine.Debug.LogError($"表格{_sheetName}缺少列:{field.Name}");
	                            }
	                        }
	                    }
	                    else if (lineIdx > 1)
	                    {

	                        //进行行处理
	                        //创建新的接受数据节点
	                        T dataObj = System.Activator.CreateInstance<T>();

	                        Dictionary<string, string> lineData = new Dictionary<string, string>();
	                        int keyCount = columnNameList.Count;

	                        //记录本行是否有效
	                        bool isLineEnable = false;

	                        //当前行中的列数目，每列值进行处理
	                        for (int i = 0; i < excelReader.FieldCount; i++)
	                        {
	                            if (i >= keyCount)
	                                continue;

	                            if (excelReader.IsDBNull(i))
	                            {
	                                lineData[columnNameList[i].ToLowerInvariant()] = "";
	                                continue;
	                            }

	                            //获取对应值，进行后续处理
	                            string value = excelReader.GetString(i);
	                            if (i == 1 && (null != value && value.StartsWith("--")))
	                            {
	                                //设置本行无效
	                                isLineEnable = false;
	                                break;
	                            }

	                            try
	                            {
	                                //处理读取操作
	                                lineData[columnNameList[i].ToLowerInvariant()] = value;
	                                //设置本行有效
	                                isLineEnable = true;

	                            }
	                            catch (Exception ex)
	                            {
	                                Debug.LogError(ex.Message + " Error Position:" + "[" + (i + 1) + "," + (lineIdx + 1) + "]" + "----name:" + columnNameList[i] + "   value:" + value);
	                            }
	                        }

	                        //添加到结果集合
	                        if (isLineEnable)
	                        {
	                            _readDelegate(dataObj, lineIdx, lineData);
	                            resList.Add(dataObj);
	                        }
	                    }

	                    //增加行号
	                    lineIdx++;
	                }
	            }
	        } while (excelReader.NextResult());

	        //释放文件资源
	        stream.Close();
	        stream.Dispose();
	        excelReader.Dispose();
	        stream = null;
	        excelReader = null;

	        //删除拷贝的文件
	        File.Delete(tmpPath);

	        return resList;
	    }

	    /// <summary>
	    /// 自动根据模板类的相关参数反射信息进行智能解析的处理
	    /// </summary>
	    /// <typeparam name="T"></typeparam>
	    /// <param name="_path"></param>
	    /// <param name="_sheetName"></param>
	    /// <param name="_exportEnum"></param>
	    /// <param name="_readDelegate"></param>
	    /// <param name="_exportTxt"></param>
	    /// <returns></returns>
	    public static List<T> autoReadXls<T>(string _path, string _sheetName, ENPExportSettingEnum _exportEnum, bool _exportTxt = true)
	    {
	        string exitension = Path.GetExtension(_path);

	        //开启文件并进行读取
	        string tmpPath = _path.Replace(exitension, ".tmpX");
	        //拷贝一个文件，避免开启excel时无法读取的问题
	        File.Copy(_path, tmpPath, true);
	        //开启文件并进行读取
	        FileStream stream = File.OpenRead(tmpPath);
	        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
	        Debug.Log("path: " + _path + "\t\t_sheetName:  " + _sheetName);
	        //创建结果队列
	        List<T> resList = new List<T>();

	        do
	        {
	            //判断sheet name是否data
	            if (excelReader.Name.Equals(_sheetName, StringComparison.OrdinalIgnoreCase))
	            {
	                Debug.Log("Start Read Sheet: " + excelReader.Name);
	                //存储列名称,用于分析数据并进行读取
	                List<string> columnNameList = new List<string>();
	                int lineIdx = 0;
	                //对表进行处理
	                while (excelReader.Read())
	                {
	                    if (1 == lineIdx)
	                    {
	                        //取列名
	                        for (int i = 0; i < excelReader.FieldCount; i++)
	                        {
	                            //添加列名称, 第一行不能有空列，空列意味着隔断
	                            if (excelReader.IsDBNull(i))
	                            {
	                                break;
	                            }
	                            else
	                            {
	                                columnNameList.Add(excelReader.GetString(i));
	                            }

	                        }
	                        //判断是否有字段，不存在columnNameList中，有的话说明配表少了列，报错
	                        //自动根据字段识别
	                        Type TempCls = typeof(T);
	                        //识别变量
	                        System.Reflection.FieldInfo[] fields = TempCls.GetFields();
	                        for (int i = 0; i < fields.Length; i++)
	                        {
	                            FieldInfo field = fields[i];
	                            ALAutoExportVariableAttr attr = null;
	                            using (IEnumerator<ALAutoExportVariableAttr> attrIEnumerator = field.GetCustomAttributes<ALAutoExportVariableAttr>().GetEnumerator())
	                            {
		                            if (attrIEnumerator.MoveNext())
			                            attr = attrIEnumerator.Current;
	                            }
	                            // 如果是可以为空和忽略的不用检查
	                            if (attr != null && (attr.canBeEmpty || attr.ignoreReading))
		                            continue;
	                            
	                            if (!columnNameList.Contains(field.Name))
	                            {
	                                UnityEngine.Debug.LogError($"表格{_sheetName}缺少列:{field.Name}");
	                            }
	                        }
	                    }
	                    else if (lineIdx > 1)
	                    {
	                        Dictionary<string, string> lineData = new Dictionary<string, string>();
	                        int keyCount = columnNameList.Count;

	                        //记录本行是否有效
	                        bool isLineEnable = false;

	                        //当前行中的列数目，每列值进行处理
	                        for (int i = 0; i < excelReader.FieldCount; i++)
	                        {
	                            if (i >= keyCount)
	                                continue;

	                            if (excelReader.IsDBNull(i))
	                            {
	                                //第一列无数据则当做无效数据
	                                if (i == 0)
	                                {
	                                    //设置本行无效
	                                    isLineEnable = false;
	                                    break;
	                                }

	                                lineData[columnNameList[i].ToLowerInvariant()] = "";
	                                continue;
	                            }

	                            //获取对应值，进行后续处理,如第一行以--开头则不处理
	                            string value = excelReader.GetString(i);
	                            if (i == 1 && (null != value && value.StartsWith("--")))
	                            {
	                                //设置本行无效
	                                isLineEnable = false;
	                                break;
	                            }
	                            //如第一行没有数据也不处理
	                            if (i == 0 && (null == value || value.Length <= 0))
	                            {
	                                //设置本行无效
	                                isLineEnable = false;
	                                break;
	                            }

	                            try
	                            {
	                                //处理读取操作
	                                lineData[columnNameList[i].ToLowerInvariant()] = value;
	                                //设置本行有效
	                                isLineEnable = true;

	                            }
	                            catch (Exception ex)
	                            {
	                                Debug.LogError(ex.Message + " Error Position:" + "[" + (i + 1) + "," + (lineIdx + 1) + "]" + "----name:" + columnNameList[i] + "   value:" + value);
	                            }
	                        }

	                        //添加到结果集合
	                        if (isLineEnable)
	                        {
	                            //使用自动读取函数
	                            T readObj = ALBasicExportFunc.AutoRead<T>(lineData, _sheetName + "第" + lineIdx + "行");
	                            if (null != readObj)
	                                resList.Add(readObj);
	                            else
	                                UnityEngine.Debug.LogError(_sheetName + "第" + lineIdx + "行，读取失败！");
	                        }
	                    }

	                    //增加行号
	                    lineIdx++;
	                }
	            }
	        } while (excelReader.NextResult());

	        //释放文件资源
	        stream.Close();
	        stream.Dispose();
	        excelReader.Dispose();
	        stream = null;
	        excelReader = null;

	        //删除拷贝的文件
	        File.Delete(tmpPath);

	        return resList;
	    }
	    public static List<T> autoReadXls<T>(string _path, string[] _sheetName, ENPExportSettingEnum _exportEnum, bool _exportTxt = true)
	    {
	        //判断带入的函数对象是否有效
	        if (null == _sheetName || _sheetName.Length <= 0)
	            return null;

	        string exitension = Path.GetExtension(_path);

	        //开启文件并进行读取
	        string tmpPath = _path.Replace(exitension, ".tmpX");
	        //拷贝一个文件，避免开启excel时无法读取的问题
	        File.Copy(_path, tmpPath, true);
	        //开启文件并进行读取
	        FileStream stream = File.OpenRead(tmpPath);
	        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
	        Debug.Log("path: " + _path + "\t\t_sheetName:  " + _sheetName);
	        //创建结果队列
	        List<T> resList = new List<T>();

	        do
	        {
	            string sheetName = excelReader.Name;

	            //判断sheet name是否data
	            bool fix = false;
	            for (int i = 0; i < _sheetName.Length; i++)
	            {
	                string tmpName = _sheetName[i];
	                if (null == tmpName)
	                    continue;

	                if (sheetName.Equals(tmpName, StringComparison.OrdinalIgnoreCase))
	                {
	                    fix = true;
	                    break;
	                }
	            }

	            //判断sheet name是否data
	            if (fix)
	            {
	                Debug.Log("Start Read Sheet: " + excelReader.Name);
	                //存储列名称,用于分析数据并进行读取
	                List<string> columnNameList = new List<string>();
	                int lineIdx = 0;
	                //对表进行处理
	                while (excelReader.Read())
	                {
	                    if (1 == lineIdx)
	                    {
	                        //取列名
	                        for (int i = 0; i < excelReader.FieldCount; i++)
	                        {
	                            //添加列名称, 第一行不能有空列，空列意味着隔断
	                            if (excelReader.IsDBNull(i))
	                            {
	                                break;
	                            }
	                            else
	                            {
	                                columnNameList.Add(excelReader.GetString(i));
	                            }

	                        }
	                        //判断是否有字段，不存在columnNameList中，有的话说明配表少了列，报错
	                        //自动根据字段识别
	                        Type TempCls = typeof(T);
	                        //识别变量
	                        System.Reflection.FieldInfo[] fields = TempCls.GetFields();
	                        for (int i = 0; i < fields.Length; i++)
	                        {
		                        FieldInfo field = fields[i];
		                        ALAutoExportVariableAttr attr = null;
		                        using (IEnumerator<ALAutoExportVariableAttr> attrIEnumerator = field.GetCustomAttributes<ALAutoExportVariableAttr>().GetEnumerator())
		                        {
			                        if (attrIEnumerator.MoveNext())
				                        attr = attrIEnumerator.Current;
		                        }
		                        // 如果是可以为空和忽略的不用检查
		                        if (attr != null && (attr.canBeEmpty || attr.ignoreReading))
			                        continue;
	                            
		                        if (!columnNameList.Contains(field.Name))
		                        {
			                        UnityEngine.Debug.LogError($"表格{_sheetName}缺少列:{field.Name}");
		                        }
	                        }
	                    }
	                    else if (lineIdx > 1)
	                    {
	                        Dictionary<string, string> lineData = new Dictionary<string, string>();
	                        int keyCount = columnNameList.Count;

	                        //记录本行是否有效
	                        bool isLineEnable = false;

	                        //当前行中的列数目，每列值进行处理
	                        for (int i = 0; i < excelReader.FieldCount; i++)
	                        {
	                            if (i >= keyCount)
	                                continue;

	                            if (excelReader.IsDBNull(i))
	                            {
	                                //第一列无数据则当做无效数据
	                                if (i == 0)
	                                {
	                                    //设置本行无效
	                                    isLineEnable = false;
	                                    break;
	                                }

	                                lineData[columnNameList[i].ToLowerInvariant()] = "";
	                                continue;
	                            }

	                            //获取对应值，进行后续处理,如第一行以--开头则不处理
	                            string value = excelReader.GetString(i);
	                            if (i == 1 && (null != value && value.StartsWith("--")))
	                            {
	                                //设置本行无效
	                                isLineEnable = false;
	                                break;
	                            }
	                            //如第一行没有数据也不处理
	                            if (i == 0 && (null == value || value.Length <= 0))
	                            {
	                                //设置本行无效
	                                isLineEnable = false;
	                                break;
	                            }

	                            try
	                            {
	                                //处理读取操作
	                                lineData[columnNameList[i].ToLowerInvariant()] = value;
	                                //设置本行有效
	                                isLineEnable = true;

	                            }
	                            catch (Exception ex)
	                            {
	                                Debug.LogError(ex.Message + " Error Position:" + "[" + (i + 1) + "," + (lineIdx + 1) + "]" + "----name:" + columnNameList[i] + "   value:" + value);
	                            }
	                        }

	                        //添加到结果集合
	                        if (isLineEnable)
	                        {
	                            //使用自动读取函数
	                            T readObj = ALBasicExportFunc.AutoRead<T>(lineData, _sheetName + "第" + lineIdx + "行");
	                            if (null != readObj)
	                                resList.Add(readObj);
	                            else
	                                UnityEngine.Debug.LogError(_sheetName + "第" + lineIdx + "行，读取失败！");
	                        }
	                    }

	                    //增加行号
	                    lineIdx++;
	                }
	            }
	        } while (excelReader.NextResult());

	        //释放文件资源
	        stream.Close();
	        stream.Dispose();
	        excelReader.Dispose();
	        stream = null;
	        excelReader = null;

	        //删除拷贝的文件
	        File.Delete(tmpPath);

	        return resList;
	    }


	    /// <summary>
	    /// 自动根据模板类的相关参数反射信息进行智能解析的处理，支持返回两组数据
	    /// </summary>
	    /// <typeparam name="T"></typeparam>
	    /// <param name="_path"></param>
	    /// <param name="_sheetName"></param>
	    /// <param name="_exportEnum"></param>
	    /// <param name="_readDelegate"></param>
	    /// <param name="_exportTxt"></param>
	    /// <returns></returns>
	    public static void autoReadXlsTwoTemp<T, K>(string _path, string _sheetName, ENPExportSettingEnum _exportEnum, List<T> _listT, List<K> _listK, bool _exportTxt = true)
	    {
	        string exitension = Path.GetExtension(_path);

	        //开启文件并进行读取
	        string tmpPath = _path.Replace(exitension, ".tmpX");
	        //拷贝一个文件，避免开启excel时无法读取的问题
	        File.Copy(_path, tmpPath, true);
	        //开启文件并进行读取
	        FileStream stream = File.OpenRead(tmpPath);
	        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
	        Debug.Log("path: " + _path + "\t\t_sheetName:  " + _sheetName);
	        do
	        {
	            //判断sheet name是否data
	            if (excelReader.Name.Equals(_sheetName, StringComparison.OrdinalIgnoreCase))
	            {
	                Debug.Log("Start Read Sheet: " + excelReader.Name);
	                //存储列名称,用于分析数据并进行读取
	                List<string> columnNameList = new List<string>();
	                int lineIdx = 0;
	                //对表进行处理
	                while (excelReader.Read())
	                {
	                    if (1 == lineIdx)
	                    {
	                        //取列名
	                        for (int i = 0; i < excelReader.FieldCount; i++)
	                        {
	                            //添加列名称, 第一行不能有空列，空列意味着隔断
	                            if (excelReader.IsDBNull(i))
	                            {
	                                break;
	                            }
	                            else
	                            {
	                                columnNameList.Add(excelReader.GetString(i));
	                            }

	                        }
	                        //判断是否有字段，不存在columnNameList中，有的话说明配表少了列，报错
	                        //自动根据字段识别
	                        Type TempCls = typeof(T);
	                        //识别变量
	                        System.Reflection.FieldInfo[] fields = TempCls.GetFields();
	                        for (int i = 0; i < fields.Length; i++)
	                        {
		                        FieldInfo field = fields[i];
		                        ALAutoExportVariableAttr attr = null;
		                        using (IEnumerator<ALAutoExportVariableAttr> attrIEnumerator = field.GetCustomAttributes<ALAutoExportVariableAttr>().GetEnumerator())
		                        {
			                        if (attrIEnumerator.MoveNext())
				                        attr = attrIEnumerator.Current;
		                        }
		                        // 如果是可以为空和忽略的不用检查
		                        if (attr != null && (attr.canBeEmpty || attr.ignoreReading))
			                        continue;
	                            
		                        if (!columnNameList.Contains(field.Name))
		                        {
			                        UnityEngine.Debug.LogError($"表格{_sheetName}缺少列:{field.Name}");
		                        }
	                        }
	                    }
	                    else if (lineIdx > 1)
	                    {
	                        Dictionary<string, string> lineData = new Dictionary<string, string>();
	                        int keyCount = columnNameList.Count;

	                        //记录本行是否有效
	                        bool isLineEnable = false;

	                        //当前行中的列数目，每列值进行处理
	                        for (int i = 0; i < excelReader.FieldCount; i++)
	                        {
	                            if (i >= keyCount)
	                                continue;

	                            if (excelReader.IsDBNull(i))
	                            {
	                                //第一列无数据则当做无效数据
	                                if (i == 0)
	                                {
	                                    //设置本行无效
	                                    isLineEnable = false;
	                                    break;
	                                }

	                                lineData[columnNameList[i].ToLowerInvariant()] = "";
	                                continue;
	                            }

	                            //获取对应值，进行后续处理,如第一行以--开头则不处理
	                            string value = excelReader.GetString(i);
	                            if (i == 1 && (null != value && value.StartsWith("--")))
	                            {
	                                //设置本行无效
	                                isLineEnable = false;
	                                break;
	                            }
	                            //如第一行没有数据也不处理
	                            if (i == 0 && (null == value || value.Length <= 0))
	                            {
	                                //设置本行无效
	                                isLineEnable = false;
	                                break;
	                            }

	                            try
	                            {
	                                //处理读取操作
	                                lineData[columnNameList[i].ToLowerInvariant()] = value;
	                                //设置本行有效
	                                isLineEnable = true;

	                            }
	                            catch (Exception ex)
	                            {
	                                Debug.LogError(ex.Message + " Error Position:" + "[" + (i + 1) + "," + (lineIdx + 1) + "]" + "----name:" + columnNameList[i] + "   value:" + value);
	                            }
	                        }

	                        //添加到结果集合
	                        if (isLineEnable)
	                        {
	                            //使用自动读取函数
	                            T readObjT = ALBasicExportFunc.AutoRead<T>(lineData, _sheetName + "第" + lineIdx + "行");
	                            K readObjK = ALBasicExportFunc.AutoRead<K>(lineData, _sheetName + "第" + lineIdx + "行");

	                            if (null != readObjT)
	                                _listT.Add(readObjT);
	                            else
	                                UnityEngine.Debug.LogError(_sheetName + "第" + lineIdx + "行，读取失败！");

	                            if (null != readObjK)
	                                _listK.Add(readObjK);
	                            else
	                                UnityEngine.Debug.LogError(_sheetName + "第" + lineIdx + "行，读取失败！");
	                        }
	                    }

	                    //增加行号
	                    lineIdx++;
	                }
	            }
	        } while (excelReader.NextResult());

	        //释放文件资源
	        stream.Close();
	        stream.Dispose();
	        excelReader.Dispose();
	        stream = null;
	        excelReader = null;

	        //删除拷贝的文件
	        File.Delete(tmpPath);
	    }

	    public DataSet ExcelToDS(string _path, string _sheetName)
	    {

	        string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + _path + ";" + "Extended Properties=Excel 8.0;";
	        OleDbConnection conn = new OleDbConnection(strConn);
	        conn.Open();
	        string strExcel = "";
	        OleDbDataAdapter myCommand = null;
	        DataSet ds = null;
	        strExcel = "select * from [sheet1$]";
	        myCommand = new OleDbDataAdapter(strExcel, strConn);
	        ds = new DataSet();
	        myCommand.Fill(ds, "table1");
	        return ds;
	    }

	    /// <summary>  
	    /// 读取 Excel 需要添加 Excel; System.Data;  
	    /// </summary>  
	    /// <param name="sheet"></param>  
	    /// <returns></returns>  
	    static DataRowCollection ReadExcel(string _path, string _sheetName)
	    {
	        FileStream stream = File.Open(_path, FileMode.Open, FileAccess.Read, FileShare.Read);
	        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

	        DataSet result = excelReader.AsDataSet();
	        //int columns = result.Tables[0].Columns.Count;  
	        //int rows = result.Tables[0].Rows.Count;  
	        return result.Tables[_sheetName].Rows;
	    }

	    //读一个excel里的单个表
	    public static void WriteToTxtFile(string _path, string _sheetName, ENPExportSettingEnum _exportEnum, Action<string> _onDone = null)
	    {

	        //检查对应文件夹位置是否创建完成
	        NPFilePathData pathData = NPFilePathData.getSavePathData(_exportEnum);
	        if (pathData == null)
	        {
	            Debug.LogError("save path data is null, ENPExportSettingEnum:  " + _exportEnum);
	            return;
	        }

	        pathData.fileDirectoryCheck();
	        string filePath = "";
	        if (_exportEnum == ENPExportSettingEnum.G_LANGUAGE || _exportEnum == ENPExportSettingEnum.P_LANGUAGE)
	            filePath = pathData.getAbsolutePath(_sheetName + "_review");
	        else
	            filePath = pathData.getAbsolutePath(_sheetName);
	        //当前TXT文件是否存在，存在就先删除
	        if (File.Exists(filePath))
	        {
	            File.Delete(Path.GetFullPath(filePath));
	        }

	        string exitension = Path.GetExtension(_path);
	        //开启文件并进行读取
	        string tmpPath = _path.Replace(exitension, ".tmpX");
	        //拷贝一个文件，避免开启excel时无法读取的问题
	        File.Copy(_path, tmpPath, true);
	        //开启文件并进行读取
	        FileStream excelfs = File.OpenRead(tmpPath);
	        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(excelfs);

            //重新创建一个文件读写
            FileStream outputFs = new FileStream(filePath, FileMode.CreateNew, FileAccess.ReadWrite);
	        StreamWriter strmWriter = new StreamWriter(outputFs);    //存入到文本文件中

	        do
	        {
	            if (excelReader.Name.Equals(_sheetName, StringComparison.OrdinalIgnoreCase))
	            {
	                //Debug.Log("Read Sheet: " + excelReader.Name);
	                //存储列名称,用于分析数据并进行读取
	                List<string> columnNameList = new List<string>();
	                int lineIdx = 0;
	                //对表进行处理
	                while (excelReader.Read())
	                {
	                    if (1 == lineIdx)
	                    {
	                        //取列名
	                        for (int i = 0; i < excelReader.FieldCount; i++)
	                        {
	                            //添加列名称, 第一行不能有空列，空列意味着隔断
	                            if (excelReader.IsDBNull(i))
	                            {
	                                break;
	                            }
	                            else
	                            {
	                                columnNameList.Add(excelReader.GetString(i));
	                                strmWriter.Write(excelReader.GetString(i));
	                            }
	                            strmWriter.Write(EXCEL_LINE_DELIMITER);//同一行的每一列分隔符
	                        }
	                        strmWriter.Write(EXCEL_LINE);
	                    }
	                    else if (lineIdx > 1)
	                    {
	                        int keyCount = columnNameList.Count;//最大有效列
	                        string zenoV = string.Empty;
	                        bool isSkip = false;
	                        //当前行中的列数目，每列值进行处理
	                        for (int i = 0; i < excelReader.FieldCount; i++)
	                        {
	                            if (i >= keyCount)
	                                continue;

	                            string value = excelReader.GetString(i);
	                            if (i == 0)
	                            {
	                                zenoV = value;
	                                isSkip = string.IsNullOrEmpty(zenoV);
	                                if (isSkip)
	                                    break;

									//如果只有一列，输出字符
                                    if (keyCount == 1)
                                    {
                                        strmWriter.Write(zenoV);
                                        strmWriter.Write(EXCEL_LINE_DELIMITER);
									}
	                            }
	                            else if (i == 1)
	                            {
	                                if (null != value && value.StartsWith("--"))
	                                {
	                                    isSkip = true;
	                                    break;
	                                }

	                                strmWriter.Write(zenoV);
	                                strmWriter.Write(EXCEL_LINE_DELIMITER);//先输出第一列
	                                strmWriter.Write(value);
	                                strmWriter.Write(EXCEL_LINE_DELIMITER);//同一行的每一列分隔符
	                            }
	                            else
	                            {
	                                strmWriter.Write(value);
	                                strmWriter.Write(EXCEL_LINE_DELIMITER);//同一行的每一列分隔符
	                            }
	                        }
	                        if (isSkip)
	                            continue;
	                        strmWriter.Write(EXCEL_LINE);
	                    }
	                    //增加行号
	                    lineIdx++;
	                }
	            }
	        } while (excelReader.NextResult());

            excelReader.Close();
            excelfs.Close();
            excelReader.Dispose();
            excelfs.Dispose();

            //删除拷贝的文件
            File.Delete(tmpPath);

            strmWriter.Close();
	        outputFs.Close();
	        strmWriter.Dispose();
            outputFs.Dispose();

	        // ------------------ 备份TXT  ---------------------
	        //pathData = WCGSOExportDirectory.instance._getBackUpPathData(_exportEnum);
	        //if(pathData == null)
	        //{
	        //    Debug.LogError("backup pathdata is null");
	        //    return;
	        //}
	        ////检查对应文件夹位置是否创建完成
	        //pathData.fileDirectoryCheck();

	        //string backUpPath = pathData.getAbsolutePath(_sheetName);

	        ////当前TXT文件是否存在，存在就先删除
	        //if (File.Exists(backUpPath)) {
	        //    File.Delete(Path.GetFullPath(backUpPath));
	        //}
	        //Debug.LogError(" filePath:  " + filePath + "\tbackUpPath:  " + backUpPath);
	        ////拷贝一个文件
	        //File.Copy(filePath, backUpPath, true);


	        string log = string.Format("excel文件[{0}]的表[{1}]导出到txt文件{2}成功完成.", _path, _sheetName, filePath);
	        Debug.LogWarning(log);
	        if (_onDone != null)
	            _onDone(filePath);
        }
        public static void WriteToTxtFile(string[] _pathList, string _sheetName, ENPExportSettingEnum _exportEnum, Action<string> _onDone = null)
        {

            //检查对应文件夹位置是否创建完成
            NPFilePathData pathData = NPFilePathData.getSavePathData(_exportEnum);
            if (pathData == null)
            {
                Debug.LogError("save path data is null, ENPExportSettingEnum:  " + _exportEnum);
                return;
            }

            pathData.fileDirectoryCheck();
            string filePath = "";
            if (_exportEnum == ENPExportSettingEnum.G_LANGUAGE || _exportEnum == ENPExportSettingEnum.P_LANGUAGE)
                filePath = pathData.getAbsolutePath(_sheetName + "_review");
            else
                filePath = pathData.getAbsolutePath(_sheetName);
            //当前TXT文件是否存在，存在就先删除
            if (File.Exists(filePath))
            {
                File.Delete(Path.GetFullPath(filePath));
            }

            //重新创建一个文件读写
            FileStream ouputfs = new FileStream(filePath, FileMode.CreateNew, FileAccess.ReadWrite);
            StreamWriter strmWriter = new StreamWriter(ouputfs);    //存入到文本文件中
			bool hasExportHeader = false;       //避免不同文件都输出一遍头部，需要标记
			StringBuilder totalFileStr = new StringBuilder();

            //存储列名称,用于分析数据并进行读取,这里只取第一次，因此在前面初始化
            List<string> columnNameList = new List<string>();

            foreach (string excelPath in _pathList)
			{
				string exitension = Path.GetExtension(excelPath);
				//开启文件并进行读取
				string tmpPath = excelPath.Replace(exitension, ".tmpX");
				//拷贝一个文件，避免开启excel时无法读取的问题
				File.Copy(excelPath, tmpPath, true);
				//开启文件并进行读取
				FileStream excelfs = File.OpenRead(tmpPath);
				IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(excelfs);

				if(totalFileStr.Length > 0)
                {
                    totalFileStr.AppendLine();
					totalFileStr.Append(excelPath);
                }
				else
				{
					totalFileStr.Append(excelPath);
                }

                do
				{
					if (excelReader.Name.Equals(_sheetName, StringComparison.OrdinalIgnoreCase))
					{
						//Debug.Log("Read Sheet: " + excelReader.Name);
						int lineIdx = 0;
						//对表进行处理
						while (excelReader.Read())
						{
							if (1 == lineIdx && !hasExportHeader)
							{
								//取列名
								for (int i = 0; i < excelReader.FieldCount; i++)
								{
									//添加列名称, 第一行不能有空列，空列意味着隔断
									if (excelReader.IsDBNull(i))
									{
										break;
									}
									else
									{
										columnNameList.Add(excelReader.GetString(i));
										strmWriter.Write(excelReader.GetString(i));
									}
									strmWriter.Write(EXCEL_LINE_DELIMITER);//同一行的每一列分隔符
								}
								strmWriter.Write(EXCEL_LINE);

								hasExportHeader = true;
                            }
							else if (lineIdx > 1)
							{
								int keyCount = columnNameList.Count;//最大有效列
								string zenoV = string.Empty;
								bool isSkip = false;
								//当前行中的列数目，每列值进行处理
								for (int i = 0; i < excelReader.FieldCount; i++)
								{
									if (i >= keyCount)
										continue;

									string value = excelReader.GetString(i);
									if (i == 0)
									{
										zenoV = value;
										isSkip = string.IsNullOrEmpty(zenoV);
										if (isSkip)
											break;

										//如果只有一列，输出字符
										if (keyCount == 1)
										{
											strmWriter.Write(zenoV);
											strmWriter.Write(EXCEL_LINE_DELIMITER);
										}
									}
									else if (i == 1)
									{
										if (null != value && value.StartsWith("--"))
										{
											isSkip = true;
											break;
										}

										strmWriter.Write(zenoV);
										strmWriter.Write(EXCEL_LINE_DELIMITER);//先输出第一列
										strmWriter.Write(value);
										strmWriter.Write(EXCEL_LINE_DELIMITER);//同一行的每一列分隔符
									}
									else
									{
										strmWriter.Write(value);
										strmWriter.Write(EXCEL_LINE_DELIMITER);//同一行的每一列分隔符
									}
								}
								if (isSkip)
									continue;
								strmWriter.Write(EXCEL_LINE);
							}
							//增加行号
							lineIdx++;
						}
					}
				} while (excelReader.NextResult());

				excelReader.Close();
				excelfs.Close();
				excelReader.Dispose();
				excelfs.Dispose();

                //删除拷贝的文件
                File.Delete(tmpPath);
            }

            strmWriter.Close();
            ouputfs.Close();
            strmWriter.Dispose();
            ouputfs.Dispose();

            // ------------------ 备份TXT  ---------------------
            //pathData = WCGSOExportDirectory.instance._getBackUpPathData(_exportEnum);
            //if(pathData == null)
            //{
            //    Debug.LogError("backup pathdata is null");
            //    return;
            //}
            ////检查对应文件夹位置是否创建完成
            //pathData.fileDirectoryCheck();

            //string backUpPath = pathData.getAbsolutePath(_sheetName);

            ////当前TXT文件是否存在，存在就先删除
            //if (File.Exists(backUpPath)) {
            //    File.Delete(Path.GetFullPath(backUpPath));
            //}
            //Debug.LogError(" filePath:  " + filePath + "\tbackUpPath:  " + backUpPath);
            ////拷贝一个文件
            //File.Copy(filePath, backUpPath, true);


            string log = string.Format("excel文件[{0}]的表[{1}]导出到txt文件{2}成功完成.", totalFileStr.ToString(), _sheetName, filePath);

            Debug.LogWarning(log);
            if (_onDone != null)
                _onDone(filePath);
        }

        //对常量表特殊处理，导出一个文本
        public static void exportGeneralText(string[] _pathList, string _sheetName, ENPExportSettingEnum _exportEnum)
	    {
	        //检查对应文件夹位置是否创建完成
	        NPFilePathData pathData = NPFilePathData.getSavePathData(_exportEnum);
	        if (pathData == null)
	        {
	            Debug.LogError("save path data is null, ENPExportSettingEnum:  " + _exportEnum);
	            return;
	        }

	        pathData.fileDirectoryCheck();
	        string filePath = pathData.getAbsolutePath(_sheetName);
	        //当前TXT文件是否存在，存在就先删除
	        if (File.Exists(filePath))
	        {
	            File.Delete(Path.GetFullPath(filePath));
            }

            //重新创建一个文件读写
            FileStream outputfs = new FileStream(filePath, FileMode.CreateNew, FileAccess.ReadWrite);
            StreamWriter strmWriter = new StreamWriter(outputfs);    //存入到文本文件中
            bool hasExportHeader = false;       //避免不同文件都输出一遍头部，需要标记
            StringBuilder totalFileStr = new StringBuilder();

            //存储列名称,用于分析数据并进行读取,这里只取第一次，因此在前面初始化
            List<string> columnNameList = new List<string>();

			foreach (string excelPath in _pathList)
			{
				string exitension = Path.GetExtension(excelPath);
				//开启文件并进行读取
				string tmpPath = excelPath.Replace(exitension, ".tmpX");
				//拷贝一个文件，避免开启excel时无法读取的问题
				File.Copy(excelPath, tmpPath, true);
				//开启文件并进行读取
				FileStream excelfs = File.OpenRead(tmpPath);
				IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(excelfs);

                if (totalFileStr.Length > 0)
                {
                    totalFileStr.AppendLine();
                    totalFileStr.Append(excelPath);
                }
                else
                {
                    totalFileStr.Append(excelPath);
                }

                do
				{
					if (excelReader.Name.Equals(_sheetName, StringComparison.OrdinalIgnoreCase))
					{
						//Debug.Log("Read Sheet: " + excelReader.Name);
						int lineIdx = 0;
						//对表进行处理
						while (excelReader.Read())
						{
							if (1 == lineIdx && !hasExportHeader)
							{
								//取列名
								for (int i = 0; i < excelReader.FieldCount; i++)
								{
									//添加列名称, 第一行不能有空列，空列意味着隔断
									if (excelReader.IsDBNull(i))
									{
										break;
									}
									else
									{
										columnNameList.Add(excelReader.GetString(i));
										strmWriter.Write(excelReader.GetString(i));
									}
									strmWriter.Write(EXCEL_LINE_DELIMITER);//同一行的每一列分隔符
								}
								strmWriter.Write(EXCEL_LINE);

                                hasExportHeader = true;
                            }
							else if (lineIdx > 1)
							{
								int keyCount = columnNameList.Count;//最大有效列
								string zenoV = string.Empty;
								//当前行中的列数目，每列值进行处理
								for (int i = 0; i < excelReader.FieldCount; i++)
								{
									if (i >= keyCount)
										continue;

									string value = excelReader.GetString(i);
									if (i == 0)
									{
										zenoV = value;
									}
									else if (i == 1)
									{
										if (null != value && value.StartsWith("--"))
											break;

										strmWriter.Write(zenoV);
										strmWriter.Write(EXCEL_LINE_DELIMITER);//先输出第一列
										strmWriter.Write(value);
										strmWriter.Write(EXCEL_LINE_DELIMITER);//同一行的每一列分隔符
									}
									else
									{
										strmWriter.Write(value);
										strmWriter.Write(EXCEL_LINE_DELIMITER);//同一行的每一列分隔符
									}
								}
								strmWriter.Write(EXCEL_LINE);
							}
							//增加行号
							lineIdx++;
						}
					}
				} while (excelReader.NextResult());

				excelReader.Close();
				excelfs.Close();
				excelReader.Dispose();
				excelfs.Dispose();

				//删除拷贝的文件
				File.Delete(tmpPath);
			}

            strmWriter.Close();
	        outputfs.Close();
	        strmWriter.Dispose();
            outputfs.Dispose();

			string log = string.Format("excel文件[{0}]的表[{1}]导出到txt文件{2}成功完成.", totalFileStr.ToString(), _sheetName, filePath);
	        Debug.LogWarning(log);
	    }
	}



}