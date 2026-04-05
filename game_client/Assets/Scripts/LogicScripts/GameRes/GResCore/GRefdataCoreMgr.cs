using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    public partial class GRefdataCoreMgr : _AALBasicRefdataCoreMgr
    {
        private static GRefdataCoreMgr _g_instance;
        [NotNull] public static GRefdataCoreMgr instance
        {
            get
            {
                if (_g_instance == null)
                {
                    _g_instance = new GRefdataCoreMgr();
                }
                return _g_instance;
            }
        }

        //带唯一key的refset

        //装备数据
        //public WCGBaseMapRefCore<WCGEquipRefObj> equipMap = new WCGBaseMapRefCore<WCGEquipRefObj>(NPRefdataResCore.instance, WCGSOEquipRefSet.assetPath, WCGSOEquipRefSet.objName);
        //本地推送表
        //public WCGBaseListRefCore<WCGLocalPushRefObj> localPushMap = new WCGBaseListRefCore<WCGLocalPushRefObj>(NPRefdataResCore.instance,WCGSOLocalPushRefSet.assetPath,WCGSOLocalPushRefSet.objName);

        //通用配置表
        public ALBasicListRefCore<NPGeneralRefObj> npGeneralMap = new ALBasicListRefCore<NPGeneralRefObj>(RefdataResCore.instance, NPSOGeneralRefSet.assetPath, NPSOGeneralRefSet.objName);
        public NPGeneralRefObj npGeneral = null;
       
        //语言长度表
        public ALBasicListRefCore<NPUnicodeLengthCheckRefObj> unicodeLengthCheckRefCore = new ALBasicListRefCore<NPUnicodeLengthCheckRefObj>(RefdataResCore.instance, NPGSOUnicodeLengthCheckRefSet.assetPath, NPGSOUnicodeLengthCheckRefSet.objName);


        //背包物品表
        public ALBasicMapRefCore<BagItemRefObj> bagItemCore = new ALBasicMapRefCore<BagItemRefObj>(RefdataResCore.instance, GSOBagItemRefSet.assetPath, GSOBagItemRefSet.objName);
        public ALBasicMapRefCore<ItemDefRefObj> itemDefCore = new ALBasicMapRefCore<ItemDefRefObj>(RefdataResCore.instance, GSOItemDefRefSet.assetPath, GSOItemDefRefSet.objName);
        public ALBasicMapRefCore<ItemExchangeRefObj> itemExchangeCore = new ALBasicMapRefCore<ItemExchangeRefObj>(RefdataResCore.instance, GSOItemExchangeRefSet.assetPath, GSOItemExchangeRefSet.objName);
        //物品兑换表
        public ALBasicListRefCore<ItemConvertRefObj> itemConvertCore = new ALBasicListRefCore<ItemConvertRefObj>(RefdataResCore.instance, GSOItemConvertRefSet.assetPath, GSOItemConvertRefSet.objName);
        //背包物品使用表
        public ALBasicMapRefCore<BagItemUseRefObj> bagItemUseCore = new ALBasicMapRefCore<BagItemUseRefObj>(RefdataResCore.instance, GSOBagItemUseRefSet.assetPath, GSOBagItemUseRefSet.objName);
        public ALBasicMapRefCore<BagItemHeroRefObj> bagItemHeroCore = new ALBasicMapRefCore<BagItemHeroRefObj>(RefdataResCore.instance, GSOBagItemHeroRefSet.assetPath, GSOBagItemHeroRefSet.objName);
        public ALBasicMapRefCore<BagItemConsortRefObj> bagItemConsortCore = new ALBasicMapRefCore<BagItemConsortRefObj>(RefdataResCore.instance, GSOBagItemConsortRefSet.assetPath, GSOBagItemConsortRefSet.objName);
        public ALBasicMapRefCore<BagItemDyeRefObj> bagItemDyeCore = new ALBasicMapRefCore<BagItemDyeRefObj>(RefdataResCore.instance, GSOBagItemDyeRefSet.assetPath, GSOBagItemDyeRefSet.objName);

        //聚会保护站道具子表
        public ALBasicListRefCore<BagItemPartyRefObj> bagItemPartyCore = new ALBasicListRefCore<BagItemPartyRefObj>(RefdataResCore.instance, GSOBagItemPartyRefSet.assetPath, GSOBagItemPartyRefSet.objName);

        //物品替换表
        public ALBasicListRefCore<ItemAlterRefObj> itemAlterCore = new ALBasicListRefCore<ItemAlterRefObj>(RefdataResCore.instance, GSOItemAlterRefSet.assetPath, GSOItemAlterRefSet.objName);
        private ItemAlterMgr _m_iamItemAlterMgr = new ItemAlterMgr();
        //奖励静态表
        public ALBasicMapRefCore<NPSORewardRefObj> rewardMap = new ALBasicMapRefCore<NPSORewardRefObj>(RefdataResCore.instance, NPSORewardRefSet.assetPath, NPSORewardRefSet.objName);
        
        public ALBasicListRefCore<NPRuleRefObj> ruleList = new ALBasicListRefCore<NPRuleRefObj>(RefdataResCore.instance, NPGSORuleRefSet.assetPath, NPGSORuleRefSet.objName);
        public ALBasicListRefCore<NPRuleSubRefObj> ruleSubList = new ALBasicListRefCore<NPRuleSubRefObj>(RefdataResCore.instance, NPGSORuleSubRefSet.assetPath, NPGSORuleSubRefSet.objName);

        
        #region 玩家信息

        //玩家资源表
        public ALBasicListRefCore<NPCurrencyResObj> currencyResCore = new ALBasicListRefCore<NPCurrencyResObj>(RefdataResCore.instance, NPSOCurrencyResRefSet.assetPath, NPSOCurrencyResRefSet.objName);
        //玩家性别表
        public ALBasicListRefCore<NPPlayerGenderRefObj> playerGenderCore = new ALBasicListRefCore<NPPlayerGenderRefObj>(RefdataResCore.instance, NPSOPlayerGenderRefSet.assetPath, NPSOPlayerGenderRefSet.objName);
        //玩家属性表
        public ALBasicMapListRefCore<NPPlayerPropertyRefObj> playerPropertyCore = new ALBasicMapListRefCore<NPPlayerPropertyRefObj>(RefdataResCore.instance, NPSOPlayerPropertyRefSet.assetPath, NPSOPlayerPropertyRefSet.objName);
        //玩家头像框
        public ALBasicMapListRefCore<PlayerIconRefObj> playerIconCore = new ALBasicMapListRefCore<PlayerIconRefObj>(RefdataResCore.instance, GSOPlayerIconRefSet.assetPath, GSOPlayerIconRefSet.objName);
        //玩家头像背景框
        public ALBasicMapListRefCore<PlayerIconBgkRefObj> iconBgkCore = new ALBasicMapListRefCore<PlayerIconBgkRefObj>(RefdataResCore.instance, GSOPlayerIconBgkRefSet.assetPath, GSOPlayerIconBgkRefSet.objName);
        //玩家气泡框
        public ALBasicMapListRefCore<NPPlayerBubbleRefObj> playerBubbleCore = new ALBasicMapListRefCore<NPPlayerBubbleRefObj>(RefdataResCore.instance, NPSOPlayerBubbleRefSet.assetPath, NPSOPlayerBubbleRefSet.objName);
        //玩家等级表
        public ALBasicMapListRefCore<PlayerLvlRefObj> playerLvlCore = new ALBasicMapListRefCore<PlayerLvlRefObj>(RefdataResCore.instance, NPSOPlayerLvlRefSet.assetPath, NPSOPlayerLvlRefSet.objName);
        //玩家大臣解锁表现表
        public ALBasicListRefCore<PlayerHeroUnlockShowRefObj> playerHeroUnlockShowCore = new ALBasicListRefCore<PlayerHeroUnlockShowRefObj>(RefdataResCore.instance, GSOPlayerHeroUnlockShowRefSet.assetPath, GSOPlayerHeroUnlockShowRefSet.objName);

        #endregion

        public ALBasicMapRefCore<NPSfxRefObj> sfxMap = new ALBasicMapRefCore<NPSfxRefObj>(RefdataResCore.instance, NPSOSfxRefSet.assetPath, NPSOSfxRefSet.objName);
        public ALBasicListRefCore<NPSfx3DRefObj> sfx3dList = new ALBasicListRefCore<NPSfx3DRefObj>(RefdataResCore.instance, NPSOSfx3DRefSet.assetPath, NPSOSfx3DRefSet.objName);

        public ALBasicListRefCore<NPQualityRefObj> qualityRefCore = new ALBasicListRefCore<NPQualityRefObj>(RefdataResCore.instance, NPGSOQualityRefSet.assetPath, NPGSOQualityRefSet.objName);
        public ALBasicMapRefCore<NPQualityExtRefObj> qualityExtRefCore = new ALBasicMapRefCore<NPQualityExtRefObj>(RefdataResCore.instance, NPGSOQualityExtRefSet.assetPath, NPGSOQualityExtRefSet.objName);
        
        // 音效资源数据
        public ALBasicMapRefCore<NPAudioRefObj> audioMap = new ALBasicMapRefCore<NPAudioRefObj>(RefdataResCore.instance, NPGSOAudioRefSet.assetPath, NPGSOAudioRefSet.objName);
        public ALBasicListRefCore<NPAudioGroupRefObj> audioGroupRefCore = new ALBasicListRefCore<NPAudioGroupRefObj>(RefdataResCore.instance, NPGSOAudioGroupRefSet.assetPath, NPGSOAudioGroupRefSet.objName);
        public ALBasicMapRefCore<VoiceKeyRefObj> voiceKeyRefCore = new ALBasicMapRefCore<VoiceKeyRefObj>(RefdataResCore.instance, NPGSOVoiceKeyRefSet.assetPath, NPGSOVoiceKeyRefSet.objName);

       
        //非法字符表
        public ALBasicListRefCore<NPDetectorCharacterRefObj> detectorCharacterMap = new ALBasicListRefCore<NPDetectorCharacterRefObj>(RefdataResCore.instance, NPSODetectorCharacterRefSet.assetPath, NPSODetectorCharacterRefSet.objName);
        //玩家取名屏蔽字库
        public ALBasicListRefCore<NPDetectorPlayerNameRefObj> detectorPlayerNameMap = new ALBasicListRefCore<NPDetectorPlayerNameRefObj>(RefdataResCore.instance, NPSODetectorPlayerNameRefSet.assetPath, NPSODetectorPlayerNameRefSet.objName);
        
        //战斗外新手引导
        public ALBasicMapRefCore<NPTutorialRef> tutorialRefCore = new ALBasicMapRefCore<NPTutorialRef>(RefdataResCore.instance, NPGSOTutorialRefSet.assetPath, NPGSOTutorialRefSet.objName);
        public ALBasicListRefCore<NPTutorialEdgeRef> tutorialEdgeRefCore = new ALBasicListRefCore<NPTutorialEdgeRef>(RefdataResCore.instance, NPGSOTutorialEdgeRefSet.assetPath, NPGSOTutorialEdgeRefSet.objName);
        public ALBasicMapRefCore<NPSimpleTutorialRefObj> simpleTutorialRefCore = new ALBasicMapRefCore<NPSimpleTutorialRefObj>(RefdataResCore.instance, NPGSOSimpleTutorialRefSet.assetPath, NPGSOSimpleTutorialRefSet.objName);
        public ALBasicListRefCore<NPSimpleTutorialEdgeRef> simpleTutorialEdgeRefCore = new ALBasicListRefCore<NPSimpleTutorialEdgeRef>(RefdataResCore.instance, NPGSOSimpleTutorialEdgeRefSet.assetPath, NPGSOSimpleTutorialEdgeRefSet.objName);

        //玩家取名屏蔽字库
        public ALBasicMapRefCore<NPRemoteEffectRefObj> remoteEffectMap = new ALBasicMapRefCore<NPRemoteEffectRefObj>(RefdataResCore.instance, NPGSORemoteEffectRefSet.assetPath, NPGSORemoteEffectRefSet.objName);

        //玩家buff表
        public ALBasicMapRefCore<NPPlayerBuffRefObj> playerBuffMap = new ALBasicMapRefCore<NPPlayerBuffRefObj>(RefdataResCore.instance, NPGSOPlayerBuffRefSet.assetPath, NPGSOPlayerBuffRefSet.objName);

        public ALBasicMapRefCore<SysInfoRef> sysInfoRefMap = new ALBasicMapRefCore<SysInfoRef>(RefdataResCore.instance, GSOSysInfoRefSet.assetPath, GSOSysInfoRefSet.objName);
        public ALBasicMapListRefCore<NPSimpleUnlockRef> simpleUnlockMap = new ALBasicMapListRefCore<NPSimpleUnlockRef>(RefdataResCore.instance, NPGSOSimpleUnlockRefSet.assetPath, NPGSOSimpleUnlockRefSet.objName);

        //版本更新表
        public ALBasicListRefCore<NPGVersionUpRewardRefObj> versionUpRewardMap = new ALBasicListRefCore<NPGVersionUpRewardRefObj>(RefdataResCore.instance, NPSOVersionUpRewardRefSet.assetPath, NPSOVersionUpRewardRefSet.objName);
        
        //成就表
        public ALBasicListRefCore<AchievePointRefObj> achievePointRefCore = new ALBasicListRefCore<AchievePointRefObj>(RefdataResCore.instance, GSOAchievePointRefSet.assetPath, GSOAchievePointRefSet.objName);
        public ALBasicMapListRefCore<AchieveRefObj> achieveMap = new ALBasicMapListRefCore<AchieveRefObj>(RefdataResCore.instance, GSOAchieveRefSet.assetPath, GSOAchieveRefSet.objName);
        public ALBasicListRefCore<AchieveStepRefObj> achieveStepList = new ALBasicListRefCore<AchieveStepRefObj>(RefdataResCore.instance, GSOAchieveStepRefSet.assetPath, GSOAchieveStepRefSet.objName);
        public ALBasicListRefCore<AchievePointStepRefObj> achievePointStepList = new ALBasicListRefCore<AchievePointStepRefObj>(RefdataResCore.instance, GSOAchievePointStepRefSet.assetPath, GSOAchievePointStepRefSet.objName);
        public ALBasicMapRefCore<AchieveTypeRefObj> achieveTypeMap = new ALBasicMapRefCore<AchieveTypeRefObj>(RefdataResCore.instance, GSOAchieveTypeRefSet.assetPath, GSOAchieveTypeRefSet.objName);
        
        //登录方式信息
        public ALBasicListRefCore<NPLoginWayRefObj> loginWayList = new ALBasicListRefCore<NPLoginWayRefObj>(RefdataResCore.instance, NPGSOLoginWayRefSet.assetPath, NPGSOLoginWayRefSet.objName);
        //登录大区信息
        public ALBasicListRefCore<NPLoginAreaRefObj> loginAreaRefCore = new ALBasicListRefCore<NPLoginAreaRefObj>(RefdataResCore.instance, NPGSOLoginAreaRefSet.assetPath, NPGSOLoginAreaRefSet.objName);

        //主城信息
        [NotNull] public ALBasicMapRefCore<SceneInfoRefObj> sceneInfoRefCore = new ALBasicMapRefCore<SceneInfoRefObj>(RefdataResCore.instance, NPGSOSceneInfoRefSet.assetPath, NPGSOSceneInfoRefSet.objName);

        //uniform数据存储
        public ALBasicSqliteSetRefCore<UniformItemObj> uniformRefCore = new ALBasicSqliteSetRefCore<UniformItemObj>(GGameSqliteMgr.instance, UniformItemAsset.AssetPath
            , UniformItemAsset.AssetObjName, UniformItemAsset.TableName, "Id");

        //UI资源路径信息
        public ALBasicSqliteSetRefCore<NPUIResPathRefObj> uiResPathRefCore = new ALBasicSqliteSetRefCore<NPUIResPathRefObj>(GGameSqliteMgr.instance, NPUIResPathRefObj.assetPath
            , NPUIResPathRefObj.objName, NPUIResPathRefObj.tableName, "Id");

        //邮件
        public ALBasicMapRefCore<GMailRefObj> mailRefCore = new ALBasicMapRefCore<GMailRefObj>(RefdataResCore.instance, GSOMailRefSet.assetPath, GSOMailRefSet.objName);
        public ALBasicMapRefCore<GMailTypeRefObj> mailTypeRefCore = new ALBasicMapRefCore<GMailTypeRefObj>(RefdataResCore.instance, GSOMailTypeRefSet.assetPath, GSOMailTypeRefSet.objName);
        public ALBasicMapRefCore<GMailSenderRefObj> mailSenderRefCore = new ALBasicMapRefCore<GMailSenderRefObj>(RefdataResCore.instance, GSOMailSenderRefSet.assetPath, GSOMailSenderRefSet.objName);
        
        //聊天
        public ALBasicMapRefCore<NPChatRoomRefObj> chatRoomRefCore = new ALBasicMapRefCore<NPChatRoomRefObj>(RefdataResCore.instance, NPGSOChatRoomRefSet.assetPath, NPGSOChatRoomRefSet.objName);
        public ALBasicMapRefCore<NPChatNPCRefObj> chatNPCRefCore = new ALBasicMapRefCore<NPChatNPCRefObj>(RefdataResCore.instance, NPGSOChatNPCRefSet.assetPath, NPGSOChatNPCRefSet.objName);
        public ALBasicMapRefCore<GChatEmoteGroupRefObj> chatEmoteGroupRefCore = new ALBasicMapRefCore<GChatEmoteGroupRefObj>(RefdataResCore.instance, GSOChatEmoteGroupRefSet.assetPath, GSOChatEmoteGroupRefSet.objName);
        public ALBasicMapRefCore<GChatEmoteItemRefObj> chatEmoteItemRefCore = new ALBasicMapRefCore<GChatEmoteItemRefObj>(RefdataResCore.instance, GSOChatEmoteItemRefSet.assetPath, GSOChatEmoteItemRefSet.objName);

        //伙伴
        public ALBasicMapListRefCore<HeroRefObj> heroRefCore = new ALBasicMapListRefCore<HeroRefObj>(RefdataResCore.instance, GSOHeroRefSet.assetPath, GSOHeroRefSet.objName);
        public ALBasicMapRefCore<HeroLevelRefObj> heroLevelRefCore = new ALBasicMapRefCore<HeroLevelRefObj>(RefdataResCore.instance, GSOHeroLevelRefSet.assetPath, GSOHeroLevelRefSet.objName);
        public ALBasicListRefCore<HeroStarRefObj> heroStarRefCore = new ALBasicListRefCore<HeroStarRefObj>(RefdataResCore.instance, GSOHeroStarRefSet.assetPath, GSOHeroStarRefSet.objName);
        public ALBasicMapRefCore<HeroStarSkillRefObj> heroStarSkillRefCore = new ALBasicMapRefCore<HeroStarSkillRefObj>(RefdataResCore.instance, GSOHeroStarSkillRefSet.assetPath, GSOHeroStarSkillRefSet.objName);
        public ALBasicListRefCore<HeroStarSkillLevelRefObj> heroStarSkillLevelRefCore = new ALBasicListRefCore<HeroStarSkillLevelRefObj>(RefdataResCore.instance, GSOHeroStarSkillLevelRefSet.assetPath, GSOHeroStarSkillLevelRefSet.objName);
        public ALBasicMapRefCore<HeroSkinRefObj> heroSkinRefCore = new ALBasicMapRefCore<HeroSkinRefObj>(RefdataResCore.instance, GSOHeroSkinRefSet.assetPath, GSOHeroSkinRefSet.objName);
        public ALBasicListRefCore<HeroSkinLevelRefObj> heroSkinLevelRefCore = new ALBasicListRefCore<HeroSkinLevelRefObj>(RefdataResCore.instance, GSOHeroSkinLevelRefSet.assetPath, GSOHeroSkinLevelRefSet.objName);
        public ALBasicMapListRefCore<HeroStepRefObj> heroStepRefCore = new ALBasicMapListRefCore<HeroStepRefObj>(RefdataResCore.instance, GSOHeroStepRefSet.assetPath, GSOHeroStepRefSet.objName);
        public ALBasicMapRefCore<HeroTalentSkillRefObj> heroTalentSkillRefCore = new ALBasicMapRefCore<HeroTalentSkillRefObj>(RefdataResCore.instance, GSOHeroTalentSkillRefSet.assetPath, GSOHeroTalentSkillRefSet.objName);
        public ALBasicListRefCore<HeroTalentSkillLevelRefObj> heroTalentSkillLevelRefCore = new ALBasicListRefCore<HeroTalentSkillLevelRefObj>(RefdataResCore.instance, GSOHeroTalentSkillLevelRefSet.assetPath, GSOHeroTalentSkillLevelRefSet.objName);
        public ALBasicMapRefCore<HeroBusinessSkillRefObj> heroBusinessSkillRefCore = new ALBasicMapRefCore<HeroBusinessSkillRefObj>(RefdataResCore.instance, GSOHeroBusinessSkillRefSet.assetPath, GSOHeroBusinessSkillRefSet.objName);
        public ALBasicListRefCore<HeroBusinessSkillUpgradeRefObj> heroBusinessSkillUpgradeRefCore = new ALBasicListRefCore<HeroBusinessSkillUpgradeRefObj>(RefdataResCore.instance, GSOHeroBusinessSkillUpgradeRefSet.assetPath, GSOHeroBusinessSkillUpgradeRefSet.objName);
        public ALBasicMapRefCore<HeroHaloRefObj> heroHaloRefCore = new ALBasicMapRefCore<HeroHaloRefObj>(RefdataResCore.instance, GSOHeroHaloRefSet.assetPath, GSOHeroHaloRefSet.objName);
        public ALBasicListRefCore<HeroHaloLevelRefObj> heroHaloLevelRefCore = new ALBasicListRefCore<HeroHaloLevelRefObj>(RefdataResCore.instance, GSOHeroHaloLevelRefSet.assetPath, GSOHeroHaloLevelRefSet.objName);
        public ALBasicMapRefCore<HeroSuitRefObj> heroHaloSuitRefCore = new ALBasicMapRefCore<HeroSuitRefObj>(RefdataResCore.instance, GSOHeroSuitRefSet.assetPath, GSOHeroSuitRefSet.objName);
        public ALBasicMapRefCore<HeroSuitSkillRefObj> heroHaloSuitSkillRefCore = new ALBasicMapRefCore<HeroSuitSkillRefObj>(RefdataResCore.instance, GSOHeroSuitSkillRefSet.assetPath, GSOHeroSuitSkillRefSet.objName);
        public ALBasicListRefCore<HeroSuitSkillLevelRefObj> heroHaloSuitSkillLevelRefCore = new ALBasicListRefCore<HeroSuitSkillLevelRefObj>(RefdataResCore.instance, GSOHeroSuitSkillLevelRefSet.assetPath, GSOHeroSuitSkillLevelRefSet.objName);
        public ALBasicListRefCore<HeroVoiceGroupRefObj> heroVoiceGroupRefCore = new ALBasicListRefCore<HeroVoiceGroupRefObj>(RefdataResCore.instance, GSOHeroVoiceGroupRefSet.assetPath, GSOHeroVoiceGroupRefSet.objName);


        public ALBasicMapListRefCore<TimesPriceRefObj> timesPriceRefCore = new ALBasicMapListRefCore<TimesPriceRefObj>(RefdataResCore.instance, NPGSOTimesPriceRefSet.assetPath, NPGSOTimesPriceRefSet.objName);


        //粒子
        public ALBasicMapRefCore<NPParticleRefObj> particleMap = new ALBasicMapRefCore<NPParticleRefObj>(RefdataResCore.instance, NPGSOParticleRefSet.assetPath, NPGSOParticleRefSet.objName);

        //上浮提示
        public ALBasicMapRefCore<NPCenterTipsRefObj> tipMap = new ALBasicMapRefCore<NPCenterTipsRefObj>(RefdataResCore.instance, NPGSOCenterTipsRefSet.assetPath, NPGSOCenterTipsRefSet.objName);

        #region 通用排行榜

        public ALBasicListRefCore<NPRankRefObj> rankCommonRefCore = new ALBasicListRefCore<NPRankRefObj>(RefdataResCore.instance, NPSORankRefSet.assetPath, NPSORankRefSet.objName);
        public ALBasicListRefCore<NPRankFixedRefObj> rankFixedRefCore = new ALBasicListRefCore<NPRankFixedRefObj>(RefdataResCore.instance, NPSORankFixedRefSet.assetPath, NPSORankFixedRefSet.objName);

        #endregion

        #region 任务
        public ALBasicMapRefCore<QuestRefObj> questMap = new ALBasicMapRefCore<QuestRefObj>(RefdataResCore.instance, GSOQuestRefSet.assetPath, GSOQuestRefSet.objName);
        public ALBasicMapRefCore<QuestStepRefObj> questStepMap = new ALBasicMapRefCore<QuestStepRefObj>(RefdataResCore.instance, GSOQuestStepRefSet.assetPath, GSOQuestStepRefSet.objName);
        public ALBasicMapRefCore<QuestTargetRefObj> questTargetMap = new ALBasicMapRefCore<QuestTargetRefObj>(RefdataResCore.instance, GSOQuestTargetRefSet.assetPath, GSOQuestTargetRefSet.objName);

        public ALBasicMapRefCore<QuestGroupRefObj> questGroupMap = new ALBasicMapRefCore<QuestGroupRefObj>(RefdataResCore.instance, GSOQuestGroupRefSet.assetPath, GSOQuestGroupRefSet.objName);
        //每日任务相关
        public ALBasicMapRefCore<DailyQuestRefObj> dailyQuestMap = new ALBasicMapRefCore<DailyQuestRefObj>(RefdataResCore.instance, GSODailyQuestRefSet.assetPath, GSODailyQuestRefSet.objName);
        public ALBasicListRefCore<DailyQuestActiveRewardRefObj> dailyQuestRewardListCore = new ALBasicListRefCore<DailyQuestActiveRewardRefObj>(RefdataResCore.instance, GSODailyQuestActiveRewardRefSet.assetPath, GSODailyQuestActiveRewardRefSet.objName);

        #endregion

        //cd数据
        public ALBasicMapRefCore<NPLazyCDRefObj> lazyCdMap = new ALBasicMapRefCore<NPLazyCDRefObj>(RefdataResCore.instance, NPSOLazyCDRefSet.assetPath, NPSOLazyCDRefSet.objName);

        //上浮提示
        public ALBasicMapRefCore<QueueDealerTipsRefObj> queueDealerTipMap = new ALBasicMapRefCore<QueueDealerTipsRefObj>(RefdataResCore.instance, GSOQueueDealerTipsRefSet.assetPath, GSOQueueDealerTipsRefSet.objName);

        //获取途径表
        public ALBasicMapRefCore<NPAccessRefObj> accessRefCore = new ALBasicMapRefCore<NPAccessRefObj>(RefdataResCore.instance, NPGSOAccessRefSet.assetPath, NPGSOAccessRefSet.objName);

        //界面跳转类型和系统功能的映射表
        public ALBasicMapRefCore<NPMainNodeToFunctionTypeRefObj> mainNodeToFunctionTypeRefCore = new ALBasicMapRefCore<NPMainNodeToFunctionTypeRefObj>(RefdataResCore.instance, NPGSOMainNodeToFunctionTypeRefSet.assetPath, NPGSOMainNodeToFunctionTypeRefSet.objName);

        //功能解锁表
        public ALBasicMapRefCore<FunctionUnlockRefObj> funcUnlockRefCore = new ALBasicMapRefCore<FunctionUnlockRefObj>(RefdataResCore.instance, GSOFunctionUnlockRefSet.assetPath, GSOFunctionUnlockRefSet.objName);
        
        //语言翻译替换表
        public ALBasicSqliteStrKeySetRefCore<NPLanguageArgsRefObj> languageArgsRefCore = new ALBasicSqliteStrKeySetRefCore<NPLanguageArgsRefObj>(GGameSqliteMgr.instance, NPLanguageArgsRefObj.assetPath
            , NPLanguageArgsRefObj.objName, NPLanguageArgsRefObj.tableName, "_id");

        //固定时间恢复的CD表
        public ALBasicMapRefCore<NPFixedCDRefObj> fixedCdMap = new ALBasicMapRefCore<NPFixedCDRefObj>(RefdataResCore.instance, NPGSOFixedCDRefSet.assetPath, NPGSOFixedCDRefSet.objName);
        

        #region 对话

        //对话主表
        public ALBasicMapRefCore<NPDialogueRefObj> dialogueMap = new ALBasicMapRefCore<NPDialogueRefObj>(RefdataResCore.instance, NPGSODialogueRefSet.assetPath, NPGSODialogueRefSet.objName);

        //对话句子表
        public ALBasicSqliteSetRefCore<NPDialogueSentenceRefObj> dialogueSentenceRefCore = new ALBasicSqliteSetRefCore<NPDialogueSentenceRefObj>(GGameSqliteMgr.instance, NPDialogueSentenceRefObj.assetPath
            , NPDialogueSentenceRefObj.objName, NPDialogueSentenceRefObj.tableName, "Id");
        //对话选项表
        public ALBasicMapRefCore<NPDialogueResponseOptionRefObj> dialogueResponseOptionRefCore = new ALBasicMapRefCore<NPDialogueResponseOptionRefObj>(RefdataResCore.instance, NPGSODialogueResponseOptionRefSet.assetPath,NPGSODialogueResponseOptionRefSet.objName);

        #endregion

        #region NPC

        //NPC主表
        public ALBasicSqliteSetRefCore<NPNPCRefObj> npcRefCore = new ALBasicSqliteSetRefCore<NPNPCRefObj>(GGameSqliteMgr.instance, NPNPCRefObj.assetPath
            , NPNPCRefObj.objName, NPNPCRefObj.tableName, "Id");

        //NPC-Actor类型子表
        public ALBasicSqliteSetRefCore<NPNPCActorRefObj> npcActorRefCore = new ALBasicSqliteSetRefCore<NPNPCActorRefObj>(GGameSqliteMgr.instance, NPNPCActorRefObj.assetPath
            , NPNPCActorRefObj.objName, NPNPCActorRefObj.tableName, "Id");

        //NPC-独立模型子表
        public ALBasicSqliteSetRefCore<NPNPCGoRefObj> npcGoRefCore = new ALBasicSqliteSetRefCore<NPNPCGoRefObj>(GGameSqliteMgr.instance, NPNPCGoRefObj.assetPath
            , NPNPCGoRefObj.objName, NPNPCGoRefObj.tableName, "Id");

        #endregion

        #region 商店表

        public ALBasicMapRefCore<NPShopRefObj> shopMap = new ALBasicMapRefCore<NPShopRefObj>(RefdataResCore.instance, NPSOShopRefSet.assetPath, NPSOShopRefSet.objName);
        public ALBasicMapRefCore<NPShopItemRefObj> shopItemMap = new ALBasicMapRefCore<NPShopItemRefObj>(RefdataResCore.instance, NPSOShopItemRefSet.assetPath, NPSOShopItemRefSet.objName);
        public ALBasicMapRefCore<NPShopItemGroupRefObj> shopItemGroupMap = new ALBasicMapRefCore<NPShopItemGroupRefObj>(RefdataResCore.instance, NPSOShopItemGroupRefSet.assetPath, NPSOShopItemGroupRefSet.objName);
        public ALBasicMapRefCore<NPShopItemDiscountRefObj> shopItemDiscountMap = new ALBasicMapRefCore<NPShopItemDiscountRefObj>(RefdataResCore.instance, NPSOShopItemDiscountRefSet.assetPath, NPSOShopItemDiscountRefSet.objName);

        #endregion

        #region 游历
        public ALBasicMapRefCore<TravelPosRefObj> travelPosCore = new ALBasicMapRefCore<TravelPosRefObj>(RefdataResCore.instance, GSOTravelPosRefSet.assetPath, GSOTravelPosRefSet.objName);
        //游历妃子表
		public ALBasicListRefCore<TravelConsortRefObj> travelConsortRefCore = new ALBasicListRefCore<TravelConsortRefObj>(RefdataResCore.instance, GSOTravelConsortRefSet.assetPath, GSOTravelConsortRefSet.objName);
		//游历事件类型表
		public ALBasicListRefCore<TravelEventTypeRefObj> travelEventTypeRefCore = new ALBasicListRefCore<TravelEventTypeRefObj>(RefdataResCore.instance, GSOTravelEventTypeRefSet.assetPath, GSOTravelEventTypeRefSet.objName);
		//游历事件表
		public ALBasicSqliteSetRefCore<TravelEventRefObj> travelEventRefCore = new ALBasicSqliteSetRefCore<TravelEventRefObj>(GGameSqliteMgr.instance, TravelEventRefObj.assetPath, TravelEventRefObj.objName, TravelEventRefObj.tableName, "Id");
		//游历单次事件表
		public ALBasicListRefCore<TravelEventOnceRefObj> travelEventOnceRefCore = new ALBasicListRefCore<TravelEventOnceRefObj>(RefdataResCore.instance, GSOTravelEventOnceRefSet.assetPath, GSOTravelEventOnceRefSet.objName);
		//游历妃子好感度事件表
		public ALBasicListRefCore<TravelEventConsortLikeRefObj> travelEventConsortLikeRefCore = new ALBasicListRefCore<TravelEventConsortLikeRefObj>(RefdataResCore.instance, GSOTravelEventConsortLikeRefSet.assetPath, GSOTravelEventConsortLikeRefSet.objName);
		//游历妃子亲密度事件表
		public ALBasicListRefCore<TravelEventConsortIntimacyRefObj> travelEventConsortIntimacyRefCore = new ALBasicListRefCore<TravelEventConsortIntimacyRefObj>(RefdataResCore.instance, GSOTravelEventConsortIntimacyRefSet.assetPath, GSOTravelEventConsortIntimacyRefSet.objName);
		//游历妃子酒馆事件表
		public ALBasicListRefCore<TravelEventConsortBarRefObj> travelEventConsortBarRefCore = new ALBasicListRefCore<TravelEventConsortBarRefObj>(RefdataResCore.instance, GSOTravelEventConsortBarRefSet.assetPath, GSOTravelEventConsortBarRefSet.objName);
		//游历妃子酒馆事件消耗表
		public ALBasicListRefCore<TravelEventConsortBarCostRefObj> travelEventConsortBarCostRefCore = new ALBasicListRefCore<TravelEventConsortBarCostRefObj>(RefdataResCore.instance, GSOTravelEventConsortBarCostRefSet.assetPath, GSOTravelEventConsortBarCostRefSet.objName);
		//游历交换事件表
		public ALBasicListRefCore<TravelEventChangeRefObj> travelEventChangeRefCore = new ALBasicListRefCore<TravelEventChangeRefObj>(RefdataResCore.instance, GSOTravelEventChangeRefSet.assetPath, GSOTravelEventChangeRefSet.objName);
		//游历邀约事件表
		public ALBasicListRefCore<TravelEventInvitationRefObj> travelEventInvitationRefCore = new ALBasicListRefCore<TravelEventInvitationRefObj>(RefdataResCore.instance, GSOTravelEventInvitationRefSet.assetPath, GSOTravelEventInvitationRefSet.objName);
		//游历卷王事件表
		public ALBasicListRefCore<TravelEventGiftedRefObj> travelEventGiftedRefCore = new ALBasicListRefCore<TravelEventGiftedRefObj>(RefdataResCore.instance, GSOTravelEventGiftedRefSet.assetPath, GSOTravelEventGiftedRefSet.objName);
        //游历博彩事件表
        public ALBasicListRefCore<TravelEventGambleRefObj> travelEventGambleRefCore = new ALBasicListRefCore<TravelEventGambleRefObj>(RefdataResCore.instance, GSOTravelEventGambleRefSet.assetPath, GSOTravelEventGambleRefSet.objName);
		//游历大臣实力事件表
		public ALBasicListRefCore<TravelEventAddPowerRefObj> travelEventAddPowerRefCore = new ALBasicListRefCore<TravelEventAddPowerRefObj>(RefdataResCore.instance, GSOTravelEventAddPowerRefSet.assetPath, GSOTravelEventAddPowerRefSet.objName);
		//游历事件一键表
		public ALBasicListRefCore<TravelEventAkeyRefObj> travelEventAkeyRefCore = new ALBasicListRefCore<TravelEventAkeyRefObj>(RefdataResCore.instance, GSOTravelEventAkeyRefSet.assetPath, GSOTravelEventAkeyRefSet.objName);


        #endregion

       
        #region 随机名称 这些表不需要放到list里面，再InitRefLanguage里面单独初始化的

        private MultiLangBasicListRefCore<PlayerNameRefObj> playerNameRefCore = new MultiLangBasicListRefCore<PlayerNameRefObj>(RefdataResCore.instance, GSOPlayerNameRefSet.objName, GSOPlayerNameRefSet.languages);
        private MultiLangBasicListRefCore<ChildNameRefObj> childNameRefCore = new MultiLangBasicListRefCore<ChildNameRefObj>(RefdataResCore.instance, GSOChildNameRefSet.objName, GSOChildNameRefSet.languages);

        #endregion

        public ALBasicMapRefCore<NPRedTipRefObj> redTipRefCore = new ALBasicMapRefCore<NPRedTipRefObj>(RefdataResCore.instance, NPGSORedTipRefSet.assetPath, NPGSORedTipRefSet.objName);
        public ALBasicMapRefCore<AttachmentItemRefObj> attachmentItemRefCore = new ALBasicMapRefCore<AttachmentItemRefObj>(RefdataResCore.instance, GSOAttachmentItemRefSet.assetPath, GSOAttachmentItemRefSet.objName);

        #region 通用宝箱表

        public ALBasicMapListRefCore<NPSOShareRefObj> shareMap = new ALBasicMapListRefCore<NPSOShareRefObj>(RefdataResCore.instance, NPSOShareRefSet.assetPath, NPSOShareRefSet.objName);
        public ALBasicMapListRefCore<NPSOCommonBoxRefObj> commonBoxMap = new ALBasicMapListRefCore<NPSOCommonBoxRefObj>(RefdataResCore.instance, NPSOCommonBoxRefSet.assetPath, NPSOCommonBoxRefSet.objName);

        #endregion

        //showcase模型额外表现表
        public ALBasicMapRefCore<NPShowCaseActorBehaviorRefObj> showCaseActorBehaviorRefCore = new ALBasicMapRefCore<NPShowCaseActorBehaviorRefObj>(RefdataResCore.instance, NPGSOShowCaseActorBehaviorRefSet.assetPath, NPGSOShowCaseActorBehaviorRefSet.objName);

        //创角表
        public ALBasicListRefCore<NPPlayerPrefabRefObj> playerPrefabRefCore = new ALBasicListRefCore<NPPlayerPrefabRefObj>(RefdataResCore.instance, NPSOPlayerPrefabRefSet.assetPath, NPSOPlayerPrefabRefSet.objName);
        
        //GoTo效果表
        public ALBasicMapRefCore<NPSOEffectGotoRefObj> effectGoToRefCore = new ALBasicMapRefCore<NPSOEffectGotoRefObj>(RefdataResCore.instance, NPSOEffectGotoRefSet.assetPath, NPSOEffectGotoRefSet.objName);

        #region 情人
        //情人表
        public ALBasicListRefCore<GConsortRefObj> consortRefCore = new ALBasicListRefCore<GConsortRefObj>(RefdataResCore.instance, GSOConsortRefSet.assetPath, GSOConsortRefSet.objName);
        //家人故事表
        public ALBasicListRefCore<ConsortStoryRefObj> consortStoryRefCore = new ALBasicListRefCore<ConsortStoryRefObj>(RefdataResCore.instance, GSOConsortStoryRefSet.assetPath, GSOConsortStoryRefSet.objName);
        //妃子故事背景表
        public ALBasicListRefCore<ConsortStoryBgRefObj> consortStoryBgRefCore = new ALBasicListRefCore<ConsortStoryBgRefObj>(RefdataResCore.instance, GSOConsortStoryBgRefSet.assetPath, GSOConsortStoryBgRefSet.objName);
        //家人羁绊等级表
        public ALBasicListRefCore<ConsortFettersLvlRefObj> consortFettersLvlRefCore = new ALBasicListRefCore<ConsortFettersLvlRefObj>(RefdataResCore.instance, GSOConsortFettersLvlRefSet.assetPath, GSOConsortFettersLvlRefSet.objName);
        // 家人羁绊技能表
        public ALBasicListRefCore<ConsortFettersSkillRefObj> consortFettersSkillRefCore = new ALBasicListRefCore<ConsortFettersSkillRefObj>(RefdataResCore.instance, GSOConsortFettersSkillRefSet.assetPath, GSOConsortFettersSkillRefSet.objName);
        // 家人羁绊技能等级表
        public ALBasicListRefCore<ConsortFettersSkillLvlRefObj> consortFettersSkillLvlRefCore = new ALBasicListRefCore<ConsortFettersSkillLvlRefObj>(RefdataResCore.instance, GSOConsortFettersSkillLvlRefSet.assetPath, GSOConsortFettersSkillLvlRefSet.objName);
        // 家人经营技能表
        public ALBasicListRefCore<ConsortBusinessSkillRefObj> consortBusinessSkillRefCore = new ALBasicListRefCore<ConsortBusinessSkillRefObj>(RefdataResCore.instance, GSOConsortBusinessSkillRefSet.assetPath, GSOConsortBusinessSkillRefSet.objName);
        // 家人经营潜能表
        public ALBasicListRefCore<ConsortBusinessPotentialRefObj> consortBusinessPotentialRefCore = new ALBasicListRefCore<ConsortBusinessPotentialRefObj>(RefdataResCore.instance, GSOConsortBusinessPotentialRefSet.assetPath, GSOConsortBusinessPotentialRefSet.objName);
        // 家人加护技能表
        public ALBasicListRefCore<ConsortBlessSkillRefObj> consortBlessSkillRefCore = new ALBasicListRefCore<ConsortBlessSkillRefObj>(RefdataResCore.instance, GSOConsortBlessSkillRefSet.assetPath, GSOConsortBlessSkillRefSet.objName);
        // 家人加护技能等级表
        public ALBasicListRefCore<ConsortBlessSkillLvlRefObj> consortBlessSkillLvlRefCore = new ALBasicListRefCore<ConsortBlessSkillLvlRefObj>(RefdataResCore.instance, GSOConsortBlessSkillLvlRefSet.assetPath, GSOConsortBlessSkillLvlRefSet.objName);
        //家人皮肤表
        public ALBasicListRefCore<GConsortSkinRefObj> consortSkinRefCore = new ALBasicListRefCore<GConsortSkinRefObj>(RefdataResCore.instance, GSOConsortSkinRefSet.assetPath, GSOConsortSkinRefSet.objName);
        //家人皮肤等级表
        public ALBasicListRefCore<GConsortSkinLvlRefObj> consortSkinLvlRefCore = new ALBasicListRefCore<GConsortSkinLvlRefObj>(RefdataResCore.instance, GSOConsortSkinLvlRefSet.assetPath, GSOConsortSkinLvlRefSet.objName);
        // 妃子旅游
        public ALBasicListRefCore<ConsortTravelRefObj> consortTravelRefCore = new ALBasicListRefCore<ConsortTravelRefObj>(RefdataResCore.instance, GSOConsortTravelRefSet.assetPath, GSOConsortTravelRefSet.objName);
        // 妃子星辉等级表
        public ALBasicListRefCore<ConsortHaloLvlRefObj> consortHaloLvlRefCore = new ALBasicListRefCore<ConsortHaloLvlRefObj>(RefdataResCore.instance, GSOConsortHaloLvlRefSet.assetPath, GSOConsortHaloLvlRefSet.objName);
        // 妃子星辉技能表
        public ALBasicListRefCore<ConsortHaloSkillRefObj> consortHaloSkillRefCore = new ALBasicListRefCore<ConsortHaloSkillRefObj>(RefdataResCore.instance, GSOConsortHaloSkillRefSet.assetPath, GSOConsortHaloSkillRefSet.objName);
        // 妃子星辉技能等级表
        public ALBasicListRefCore<ConsortHaloSkillLvlRefObj> consortHaloSkillLvlRefCore = new ALBasicListRefCore<ConsortHaloSkillLvlRefObj>(RefdataResCore.instance, GSOConsortHaloSkillLvlRefSet.assetPath, GSOConsortHaloSkillLvlRefSet.objName);
        //妃子配音组表
        public ALBasicListRefCore<ConsortVoiceGroupRefObj> consortVoiceGroupRefCore = new ALBasicListRefCore<ConsortVoiceGroupRefObj>(RefdataResCore.instance, GSOConsortVoiceGroupRefSet.assetPath, GSOConsortVoiceGroupRefSet.objName);
        // 妃子CG表
        public ALBasicListRefCore<ConsortCGRefObj> consortCGRefCore = new ALBasicListRefCore<ConsortCGRefObj>(RefdataResCore.instance, GSOConsortCGRefSet.assetPath, GSOConsortCGRefSet.objName);

        #endregion

        // 加成概率表
        public ALBasicMapRefCore<ProAddGroupRefObj> proAddGroupRefCore = new ALBasicMapRefCore<ProAddGroupRefObj>(RefdataResCore.instance, GSOProAddGroupRefSet.assetPath, GSOProAddGroupRefSet.objName);
        
        // 操作消耗表
        public ALBasicMapRefCore<OpCostGroupRefObj> opCostGroupRefCore = new ALBasicMapRefCore<OpCostGroupRefObj>(RefdataResCore.instance, GSOOpCostGroupRefSet.assetPath, GSOOpCostGroupRefSet.objName);
        
        #region 关卡

        public ALBasicSqliteSetRefCore<ChapterRefObj> chapterRefCore = new ALBasicSqliteSetRefCore<ChapterRefObj>(GGameSqliteMgr.instance, ChapterRefObj.assetPath
            , ChapterRefObj.objName, ChapterRefObj.tableName, "Id");
        public ALBasicListRefCore<ChapterBossStyleRefObj> chapterBossStyleRefCore = new ALBasicListRefCore<ChapterBossStyleRefObj>(RefdataResCore.instance, ChapterBossStyleRefSet.assetPath, ChapterBossStyleRefSet.objName);
        public ALBasicListRefCore<ChapterNodeStyleRefObj> chapterNodeStyleRefCore = new ALBasicListRefCore<ChapterNodeStyleRefObj>(RefdataResCore.instance, ChapterNodeStyleRefSet.assetPath, ChapterNodeStyleRefSet.objName);

        //关卡选择事件选项表
        public ALBasicListRefCore<ChapterCostRefObj> chapterCostRefCore = new ALBasicListRefCore<ChapterCostRefObj>(RefdataResCore.instance, GSOChapterCostRefSet.assetPath, GSOChapterCostRefSet.objName);
        public ALBasicListRefCore<ChapterBuildUnlockRefObj> chapterBuildUnlockRefCore = new ALBasicListRefCore<ChapterBuildUnlockRefObj>(RefdataResCore.instance, GSOChapterBuildUnlockRefSet.assetPath, GSOChapterBuildUnlockRefSet.objName);

        
        //关卡事件表
        public ALBasicListRefCore<ChapterEventRefObj> chapterEventRefCore = new ALBasicListRefCore<ChapterEventRefObj>(RefdataResCore.instance, GSOChapterEventRefSet.assetPath, GSOChapterEventRefSet.objName);
        //关卡事件奖励表
        public ALBasicListRefCore<ChapterEventRewardRefObj> chapterEventRewardRefCore = new ALBasicListRefCore<ChapterEventRewardRefObj>(RefdataResCore.instance, GSOChapterEventRewardRefSet.assetPath, GSOChapterEventRewardRefSet.objName);
        //关卡选择事件表
        public ALBasicListRefCore<ChapterEventChoiceRefObj> chapterEventChoiceRefCore = new ALBasicListRefCore<ChapterEventChoiceRefObj>(RefdataResCore.instance, GSOChapterEventChoiceRefSet.assetPath, GSOChapterEventChoiceRefSet.objName);
        //关卡选择事件选项表
        public ALBasicListRefCore<ChapterEventChoiceOptionRefObj> chapterEventChoiceOptionRefCore = new ALBasicListRefCore<ChapterEventChoiceOptionRefObj>(RefdataResCore.instance, GSOChapterEventChoiceOptionRefSet.assetPath, GSOChapterEventChoiceOptionRefSet.objName);
        //关卡派遣事件表
        public ALBasicListRefCore<ChapterEventDispatchRefObj> chapterEventDispatchRefCore = new ALBasicListRefCore<ChapterEventDispatchRefObj>(RefdataResCore.instance, GSOChapterEventDispatchRefSet.assetPath, GSOChapterEventDispatchRefSet.objName);
        //关卡派遣事件展示表
        public ALBasicListRefCore<ChapterEventDispatchShowRefObj> chapterEventDispatchShowRefCore = new ALBasicListRefCore<ChapterEventDispatchShowRefObj>(RefdataResCore.instance, GSOChapterEventDispatchShowRefSet.assetPath, GSOChapterEventDispatchShowRefSet.objName);
        //关卡派遣事件条件表
        public ALBasicListRefCore<ChapterEventDispatchCondRefObj> chapterEventDispatchCondRefCore = new ALBasicListRefCore<ChapterEventDispatchCondRefObj>(RefdataResCore.instance, GSOChapterEventDispatchCondRefSet.assetPath, GSOChapterEventDispatchCondRefSet.objName);
		// 关卡节点表
        public ALBasicListRefCore<ChapterStageRefObj> chapterStageRefCore = new ALBasicListRefCore<ChapterStageRefObj>(RefdataResCore.instance, GSOChapterStageRefSet.assetPath, GSOChapterStageRefSet.objName);
        // 关卡节剧情表
        public ALBasicListRefCore<ChapterStagePlotRefObj> chapterStagePlotRefCore = new ALBasicListRefCore<ChapterStagePlotRefObj>(RefdataResCore.instance, GSOChapterStagePlotRefSet.assetPath, GSOChapterStagePlotRefSet.objName);
        // 关卡故事表
        public ALBasicListRefCore<ChapterStoryRefObj> chapterStoryRefCore = new ALBasicListRefCore<ChapterStoryRefObj>(RefdataResCore.instance, GSOChapterStoryRefSet.assetPath, GSOChapterStoryRefSet.objName);

        #endregion

        #region 通用事件

        [NotNull] public ALBasicSqliteSetRefCore<CommonEventAwardRefObj> commonEventAwardRefCore = new ALBasicSqliteSetRefCore<CommonEventAwardRefObj>(GGameSqliteMgr.instance, CommonEventAwardRefObj.assetPath
            , CommonEventAwardRefObj.objName, CommonEventAwardRefObj.tableName, "Id");
        [NotNull] public ALBasicListRefCore<CommonEventAwardShowRefObj> commonEventAwardShowRefCore = new ALBasicListRefCore<CommonEventAwardShowRefObj>(RefdataResCore.instance, GSOCommonEventAwardShowRefSet.assetPath, GSOCommonEventAwardShowRefSet.objName);
        [NotNull] public ALBasicListRefCore<CommonEventChoiceOptionRefObj> commonEventChoiceOptionRefCore = new ALBasicListRefCore<CommonEventChoiceOptionRefObj>(RefdataResCore.instance, GSOCommonEventChoiceOptionRefSet.assetPath, GSOCommonEventChoiceOptionRefSet.objName);
        [NotNull] public ALBasicSqliteSetRefCore<CommonEventChoiceRefObj> commonEventChoiceRefCore = new ALBasicSqliteSetRefCore<CommonEventChoiceRefObj>(GGameSqliteMgr.instance, CommonEventChoiceRefObj.assetPath
            , CommonEventChoiceRefObj.objName, CommonEventChoiceRefObj.tableName, "Id");
        [NotNull] public ALBasicListRefCore<CommonEventChoiceShowRefObj> commonEventChoiceShowRefCore = new ALBasicListRefCore<CommonEventChoiceShowRefObj>(RefdataResCore.instance, GSOCommonEventChoiceShowRefSet.assetPath, GSOCommonEventChoiceShowRefSet.objName);
        [NotNull] public ALBasicSqliteSetRefCore<CommonEventDialogRefObj> commonEventDialogRefCore = new ALBasicSqliteSetRefCore<CommonEventDialogRefObj>(GGameSqliteMgr.instance, CommonEventDialogRefObj.assetPath
            , CommonEventDialogRefObj.objName, CommonEventDialogRefObj.tableName, "Id");
        [NotNull] public ALBasicListRefCore<CommonEventDispatchCondRefObj> commonEventDispatchCondRefCore = new ALBasicListRefCore<CommonEventDispatchCondRefObj>(RefdataResCore.instance, GSOCommonEventDispatchCondRefSet.assetPath, GSOCommonEventDispatchCondRefSet.objName);
        [NotNull] public ALBasicSqliteSetRefCore<CommonEventDispatchRefObj> commonEventDispatchRefCore = new ALBasicSqliteSetRefCore<CommonEventDispatchRefObj>(GGameSqliteMgr.instance, CommonEventDispatchRefObj.assetPath
            , CommonEventDispatchRefObj.objName, CommonEventDispatchRefObj.tableName, "Id");
        [NotNull] public ALBasicListRefCore<CommonEventDispatchResultRefObj> commonEventDispatchResultRefCore = new ALBasicListRefCore<CommonEventDispatchResultRefObj>(RefdataResCore.instance, GSOCommonEventDispatchResultRefSet.assetPath, GSOCommonEventDispatchResultRefSet.objName);
        [NotNull] public ALBasicListRefCore<CommonEventDispatchShowRefObj> commonEventDispatchShowRefCore = new ALBasicListRefCore<CommonEventDispatchShowRefObj>(RefdataResCore.instance, GSOCommonEventDispatchShowRefSet.assetPath, GSOCommonEventDispatchShowRefSet.objName);
        [NotNull] public ALBasicSqliteSetRefCore<CommonEventRefObj> commonEventRefCore = new ALBasicSqliteSetRefCore<CommonEventRefObj>(GGameSqliteMgr.instance, CommonEventRefObj.assetPath
            , CommonEventRefObj.objName, CommonEventRefObj.tableName, "Id");
        [NotNull] public ALBasicSqliteSetRefCore<CommonEventRewardRefObj> commonEventRewardRefCore = new ALBasicSqliteSetRefCore<CommonEventRewardRefObj>(GGameSqliteMgr.instance, CommonEventRewardRefObj.assetPath
            , CommonEventRewardRefObj.objName, CommonEventRewardRefObj.tableName, "Id");
        [NotNull] public ALBasicSqliteSetRefCore<CommonEventPlotDialogRefObj> commonEventPlotDialogRefCore = new ALBasicSqliteSetRefCore<CommonEventPlotDialogRefObj>(GGameSqliteMgr.instance, CommonEventPlotDialogRefObj.assetPath
            , CommonEventPlotDialogRefObj.objName, CommonEventPlotDialogRefObj.tableName, "Id");
        [NotNull] public ALBasicSqliteSetRefCore<CommonEventMiniGameRefObj> commonEventMiniGameRefCore = new ALBasicSqliteSetRefCore<CommonEventMiniGameRefObj>(GGameSqliteMgr.instance, CommonEventMiniGameRefObj.assetPath
            , CommonEventMiniGameRefObj.objName, CommonEventMiniGameRefObj.tableName, "Id");
        [NotNull] public ALBasicListRefCore<CommonEventMiniGameShowRefObj> commonEventMiniGameShowRefCore = new ALBasicListRefCore<CommonEventMiniGameShowRefObj>(RefdataResCore.instance, GSOCommonEventMiniGameShowRefSet.assetPath, GSOCommonEventMiniGameShowRefSet.objName);

        #endregion

        public ALBasicListRefCore<LocalPushRefObj> localPushCore = new ALBasicListRefCore<LocalPushRefObj>(RefdataResCore.instance, GSOLocalPushRefSet.assetPath, GSOLocalPushRefSet.objName);
        //主城推送弹窗表
        public ALBasicListRefCore<MainCityPushNoticeRefObj> mainCityPushNoticeRefCore = new ALBasicListRefCore<MainCityPushNoticeRefObj>(RefdataResCore.instance, GSOMainCityPushNoticeRefSet.assetPath, GSOMainCityPushNoticeRefSet.objName);
        // 活动合并展示推送表
        public ALBasicListRefCore<PushNoticeActivityMergeRefObj> pushNoticeActivityMergeRefCore = new ALBasicListRefCore<PushNoticeActivityMergeRefObj>(RefdataResCore.instance, GSOPushNoticeActivityMergeRefSet.assetPath, GSOPushNoticeActivityMergeRefSet.objName);
        
        //猫咪气泡表
        public ALBasicListRefCore<CatBubbleRefObj> catBubbleRefCore = new ALBasicListRefCore<CatBubbleRefObj>(RefdataResCore.instance, GSOCatBubbleRefSet.assetPath, GSOCatBubbleRefSet.objName);
        
        //增量包表
        public ALBasicListRefCore<AddPackRefObj> addPackRefCore = new ALBasicListRefCore<AddPackRefObj>(RefdataResCore.instance, GSOAddPackRefSet.assetPath, GSOAddPackRefSet.objName);
        public ALBasicListRefCore<AddPackPathsRefObj> addPackPathsRefCore = new ALBasicListRefCore<AddPackPathsRefObj>(RefdataResCore.instance, GSOAddPackPathsRefSet.assetPath, GSOAddPackPathsRefSet.objName);

        //通用属性表
        public ALBasicMapRefCore<BasicAttrRefObj> basicAttrRefCore = new ALBasicMapRefCore<BasicAttrRefObj>(RefdataResCore.instance, GSOBasicAttrRefSet.assetPath, GSOBasicAttrRefSet.objName);

        [NotNull]public ALBasicMapRefCore<EntryPointRefObj> entryPointRefCore = new ALBasicMapRefCore<EntryPointRefObj>(RefdataResCore.instance, GSOEntryPointRefSet.assetPath, GSOEntryPointRefSet.objName);
        
        #region 子嗣
        public ALBasicMapRefCore<ChildAttrRefObj> childAttrCore = new ALBasicMapRefCore<ChildAttrRefObj>(RefdataResCore.instance, GSOChildAttrRefSet.assetPath, GSOChildAttrRefSet.objName);
        public ALBasicMapRefCore<ChildCareerRefObj> childCareerCore = new ALBasicMapRefCore<ChildCareerRefObj>(RefdataResCore.instance, GSOChildCareerRefSet.assetPath, GSOChildCareerRefSet.objName);
        public ALBasicMapRefCore<ChildInitResRefObj> childInitResCore = new ALBasicMapRefCore<ChildInitResRefObj>(RefdataResCore.instance, GSOChildInitResRefSet.assetPath, GSOChildInitResRefSet.objName);
        public ALBasicMapRefCore<ChildResRefObj> childResCore = new ALBasicMapRefCore<ChildResRefObj>(RefdataResCore.instance, GSOChildResRefSet.assetPath, GSOChildResRefSet.objName);
        public ALBasicMapRefCore<ChildQualityRefObj> childQualityCore = new ALBasicMapRefCore<ChildQualityRefObj>(RefdataResCore.instance, GSOChildQualityRefSet.assetPath, GSOChildQualityRefSet.objName);
        public ALBasicMapRefCore<ChildSeatRefObj> childSeatCore = new ALBasicMapRefCore<ChildSeatRefObj>(RefdataResCore.instance, GSOChildSeatRefSet.assetPath, GSOChildSeatRefSet.objName);
        #endregion

        #region 宴会

        public ALBasicListRefCore<GDinnerTypeRefObj> dinnerTypeRefCore = new ALBasicListRefCore<GDinnerTypeRefObj>(RefdataResCore.instance, GSODinnerTypeRefSet.assetPath, GSODinnerTypeRefSet.objName);
        public ALBasicListRefCore<GDinnerJoinCostRefObj> dinnerJoinCostRefCore = new ALBasicListRefCore<GDinnerJoinCostRefObj>(RefdataResCore.instance, GSODinnerJoinCostRefSet.assetPath, GSODinnerJoinCostRefSet.objName);
        //宴会凭证表
        public ALBasicMapRefCore<DinnerPermitRefObj> dinnerPermitRefCore = new ALBasicMapRefCore<DinnerPermitRefObj>(RefdataResCore.instance, GSODinnerPermitRefSet.assetPath, GSODinnerPermitRefSet.objName);
        #endregion
        
        #region 经验事件

        public ALBasicListRefCore<AnecdoteEventChoiceOptionRefObj> anecdoteEventChoiceOptionRefCore = new ALBasicListRefCore<AnecdoteEventChoiceOptionRefObj>(RefdataResCore.instance, GSOAnecdoteEventChoiceOptionRefSet.assetPath, GSOAnecdoteEventChoiceOptionRefSet.objName);
        public ALBasicListRefCore<AnecdoteEventChoiceRefObj> anecdoteEventChoiceRefCore = new ALBasicListRefCore<AnecdoteEventChoiceRefObj>(RefdataResCore.instance, GSOAnecdoteEventChoiceRefSet.assetPath, GSOAnecdoteEventChoiceRefSet.objName);
        public ALBasicListRefCore<AnecdoteEventEarningsRefObj> anecdoteEventEarningsRefCore = new ALBasicListRefCore<AnecdoteEventEarningsRefObj>(RefdataResCore.instance, GSOAnecdoteEventEarningsRefSet.assetPath, GSOAnecdoteEventEarningsRefSet.objName);
        public ALBasicListRefCore<AnecdoteEventRefObj> anecdoteEventRefCore = new ALBasicListRefCore<AnecdoteEventRefObj>(RefdataResCore.instance, GSOAnecdoteEventRefSet.assetPath, GSOAnecdoteEventRefSet.objName);
        public ALBasicListRefCore<AnecdoteEventRewardRefObj> anecdoteEventRewardRefCore = new ALBasicListRefCore<AnecdoteEventRewardRefObj>(RefdataResCore.instance, GSOAnecdoteEventRewardRefSet.assetPath, GSOAnecdoteEventRewardRefSet.objName);
        public ALBasicListRefCore<AnecdotePosRefObj> anecdotePosRefCore = new ALBasicListRefCore<AnecdotePosRefObj>(RefdataResCore.instance, GSOAnecdotePosRefSet.assetPath, GSOAnecdotePosRefSet.objName); 

        #endregion

        #region 小游戏

        [NotNull] public ALBasicSqliteSetRefCore<MiniGameMainRefObj> miniGameMainRefCore = new ALBasicSqliteSetRefCore<MiniGameMainRefObj>(GGameSqliteMgr.instance, MiniGameMainRefObj.assetPath
            , MiniGameMainRefObj.objName, MiniGameMainRefObj.tableName, "Id");
        [NotNull] public ALBasicMapRefCore<FindThingsGameRefObj> findThingsGameRefCore = new ALBasicMapRefCore<FindThingsGameRefObj>(RefdataResCore.instance, GSOFindThingsGameRefSet.assetPath, GSOFindThingsGameRefSet.objName);
        [NotNull] public ALBasicMapRefCore<PuzzleGameRefObj> puzzleGameRefCore = new ALBasicMapRefCore<PuzzleGameRefObj>(RefdataResCore.instance, GSOPuzzleGameRefSet.assetPath, GSOPuzzleGameRefSet.objName);
        [NotNull] public ALBasicMapRefCore<TakeThingsSequentiallyGameRefObj> takeThingsSequentiallyGameRefCore = new ALBasicMapRefCore<TakeThingsSequentiallyGameRefObj>(RefdataResCore.instance, GSOTakeThingsSequentiallyGameRefSet.assetPath, GSOTakeThingsSequentiallyGameRefSet.objName);
        [NotNull] public ALBasicMapRefCore<QTEGameRefObj> qteGameRefCore = new ALBasicMapRefCore<QTEGameRefObj>(RefdataResCore.instance, GSOQTEGameRefSet.assetPath, GSOQTEGameRefSet.objName);
        [NotNull] public ALBasicMapRefCore<QteClickOpportunityGameRefObj> qteClickOpportunityGameRefCore = new ALBasicMapRefCore<QteClickOpportunityGameRefObj>(RefdataResCore.instance, GSOQteClickOpportunityGameRefSet.assetPath, GSOQteClickOpportunityGameRefSet.objName);
        [NotNull] public ALBasicMapRefCore<DragBoxGameRefObj> dragBoxGameRefCore = new ALBasicMapRefCore<DragBoxGameRefObj>(RefdataResCore.instance, GSODragBoxGameRefSet.assetPath, GSODragBoxGameRefSet.objName);

        #endregion

        #region 漫画

        [NotNull] public readonly ALBasicMapRefCore<SimpleComicRefObj> simpleCommicRefCore = new (RefdataResCore.instance, GSOCSimpleComicRefSet.assetPath, GSOCSimpleComicRefSet.objName);

        #endregion

        #region 跑马灯

        public ALBasicMapRefCore<MarqueeRefObj> marqueeRefCore = new ALBasicMapRefCore<MarqueeRefObj>(RefdataResCore.instance, GSOMarqueeRefSet.assetPath, GSOMarqueeRefSet.objName);

        #endregion

        #region Q版形象

        public ALBasicMapRefCore<CuteActorRefObj> cuteActorRefCore = new ALBasicMapRefCore<CuteActorRefObj>(RefdataResCore.instance, GSOCuteActorRefSet.assetPath, GSOCuteActorRefSet.objName);

        #endregion

        #region 运营公告

        public ALBasicMapRefCore<AnnouncementBannerRefObj> announcementBannerRefCore = new ALBasicMapRefCore<AnnouncementBannerRefObj>(RefdataResCore.instance, GSOAnnouncementBannerRefSet.assetPath, GSOAnnouncementBannerRefSet.objName);
        public ALBasicMapRefCore<AnnouncementJumpRefObj> announcementJumpRefCore = new ALBasicMapRefCore<AnnouncementJumpRefObj>(RefdataResCore.instance, GSOAnnouncementJumpRefSet.assetPath, GSOAnnouncementJumpRefSet.objName);
        public ALBasicMapRefCore<AnnouncementTabRefObj> announcementTabRefCore = new ALBasicMapRefCore<AnnouncementTabRefObj>(RefdataResCore.instance, GSOAnnouncementTabRefSet.assetPath, GSOAnnouncementTabRefSet.objName);

        #endregion
        
        public ALBasicListRefCore<BuildingRefObj> buildingRefCore = new ALBasicListRefCore<BuildingRefObj>(RefdataResCore.instance, GSOBuildingRefSet.assetPath, GSOBuildingRefSet.objName);
        public ALBasicListRefCore<BusinessBuildingRefObj> businessBuildingRefCore = new ALBasicListRefCore<BusinessBuildingRefObj>(RefdataResCore.instance, GSOBusinessBuildingRefSet.assetPath, GSOBusinessBuildingRefSet.objName);
        public ALBasicListRefCore<BusinessBuildingHireCostRefObj> businessBuildingHireCostRefCore = new ALBasicListRefCore<BusinessBuildingHireCostRefObj>(RefdataResCore.instance, GSOBusinessBuildingHireCostRefSet.assetPath, GSOBusinessBuildingHireCostRefSet.objName);
        public ALBasicListRefCore<BusinessBuildingLevelRefObj> businessBuildingLevelRefCore = new ALBasicListRefCore<BusinessBuildingLevelRefObj>(RefdataResCore.instance, GSOBusinessBuildingLevelRefSet.assetPath, GSOBusinessBuildingLevelRefSet.objName);
        public ALBasicListRefCore<BusinessBuildingVideoGroupRefObj> businessBuildingVideoGroupRefCore = new ALBasicListRefCore<BusinessBuildingVideoGroupRefObj>(RefdataResCore.instance, GSOBusinessBuildingVideoGroupRefSet.assetPath, GSOBusinessBuildingVideoGroupRefSet.objName);
        public ALBasicListRefCore<FarmingBuildingRefObj> farmingBuildingRefCore = new ALBasicListRefCore<FarmingBuildingRefObj>(RefdataResCore.instance, GSOFarmingBuildingRefSet.assetPath, GSOFarmingBuildingRefSet.objName);
        public ALBasicListRefCore<FarmingBuildingLevelRefObj> farmingBuildingLevelRefCore = new ALBasicListRefCore<FarmingBuildingLevelRefObj>(RefdataResCore.instance, GSOFarmingBuildingLevelRefSet.assetPath, GSOFarmingBuildingLevelRefSet.objName);
        public ALBasicListRefCore<BusinessBuildingDevelopRefObj> businessBuildingDevelopRefCore = new ALBasicListRefCore<BusinessBuildingDevelopRefObj>(RefdataResCore.instance, GSOBusinessBuildingDevelopRefSet.assetPath, GSOBusinessBuildingDevelopRefSet.objName);
        public ALBasicListRefCore<BusinessBuildingProductRefObj> businessBuildingProductRefCore = new ALBasicListRefCore<BusinessBuildingProductRefObj>(RefdataResCore.instance, GSOBusinessBuildingProductRefSet.assetPath, GSOBusinessBuildingProductRefSet.objName);

        #region 阶段目标
        public ALBasicListRefCore<StageGoalBigStepRefObj> stageGoalBigStepRefCore = new ALBasicListRefCore<StageGoalBigStepRefObj>(RefdataResCore.instance, GSOStageGoalBigStepRefSet.assetPath, GSOStageGoalBigStepRefSet.objName);
        public ALBasicListRefCore<StageGoalRefObj> stageGoalRefCore = new ALBasicListRefCore<StageGoalRefObj>(RefdataResCore.instance, GSOStageGoalRefSet.assetPath, GSOStageGoalRefSet.objName);
        public ALBasicListRefCore<StageGoalTaskRefObj> stageGoalTaskRefCore = new ALBasicListRefCore<StageGoalTaskRefObj>(RefdataResCore.instance, GSOStageGoalTaskRefSet.assetPath, GSOStageGoalTaskRefSet.objName);
        #endregion
        
        // 晚间副本排行奖励表
        public ALBasicListRefCore<EveningDungeonRankRewardRefObj> eveningDungeonRankRewardRefCore = new ALBasicListRefCore<EveningDungeonRankRewardRefObj>(RefdataResCore.instance, GSOEveningDungeonRankRewardRefSet.assetPath, GSOEveningDungeonRankRewardRefSet.objName);

        #region 开服七天

        public ALBasicListRefCore<SevenDayGoalsTaskRefObj> sevenDayGoalsTaskRefCore = new ALBasicListRefCore<SevenDayGoalsTaskRefObj>(RefdataResCore.instance, GSOSevenDayGoalsTaskRefSet.assetPath, GSOSevenDayGoalsTaskRefSet.objName);
        public ALBasicListRefCore<SevenDayGoalsTaskRewardRefObj> sevenDayGoalsTaskRewardRefCore = new ALBasicListRefCore<SevenDayGoalsTaskRewardRefObj>(RefdataResCore.instance, GSOSevenDayGoalsTaskRewardRefSet.assetPath, GSOSevenDayGoalsTaskRewardRefSet.objName);
        public ALBasicListRefCore<SevenDayGoalsStepRewardRefObj> sevenDayGoalsStepRewardRefCore = new ALBasicListRefCore<SevenDayGoalsStepRewardRefObj>(RefdataResCore.instance, GSOSevenDayGoalsStepRewardRefSet.assetPath, GSOSevenDayGoalsStepRewardRefSet.objName);
        public ALBasicListRefCore<SevenDayGoalsGiftPackRefObj> sevenDayGoalsGiftPackRefCore = new ALBasicListRefCore<SevenDayGoalsGiftPackRefObj>(RefdataResCore.instance, GSOSevenDayGoalsGiftPackRefSet.assetPath, GSOSevenDayGoalsGiftPackRefSet.objName);

        #endregion

        //>>>>>>>>>> AUTO GENERATE START <<<<<<<<<<
		//联盟等级表
		public ALBasicMapListRefCore<GuildLevelRefObj> guildLevelRefCore = new ALBasicMapListRefCore<GuildLevelRefObj>(RefdataResCore.instance, GSOGuildLevelRefSet.assetPath, GSOGuildLevelRefSet.objName);
		//联盟职位表
		public ALBasicMapRefCore<GuildPositionRefObj> guildPositionRefCore = new ALBasicMapRefCore<GuildPositionRefObj>(RefdataResCore.instance, GSOGuildPositionRefSet.assetPath, GSOGuildPositionRefSet.objName);
		//联盟旗帜表
		public ALBasicMapListRefCore<GuildFlagRefObj> guildFlagRefCore = new ALBasicMapListRefCore<GuildFlagRefObj>(RefdataResCore.instance, GSOGuildFlagRefSet.assetPath, GSOGuildFlagRefSet.objName);
		//联盟建设表
		public ALBasicListRefCore<GuildConstructRefObj> guildConstructRefCore = new ALBasicListRefCore<GuildConstructRefObj>(RefdataResCore.instance, GSOGuildConstructRefSet.assetPath, GSOGuildConstructRefSet.objName);
		//联盟日志表
		public ALBasicMapRefCore<GuildLogRefObj> guildLogRefCore = new ALBasicMapRefCore<GuildLogRefObj>(RefdataResCore.instance, GSOGuildLogRefSet.assetPath, GSOGuildLogRefSet.objName);
        // 入盟限制表
        public ALBasicListRefCore<GuildJoinLimitRefObj> guildJoinLimitRefCore = new ALBasicListRefCore<GuildJoinLimitRefObj>(RefdataResCore.instance, GSOGuildJoinLimitRefSet.assetPath, GSOGuildJoinLimitRefSet.objName);
        //联盟捐赠进度奖励表
        public ALBasicListRefCore<GuildConstructRewardRefObj> guildConstructRewardRefCore = new ALBasicListRefCore<GuildConstructRewardRefObj>(RefdataResCore.instance, GSOGuildConstructRewardRefSet.assetPath, GSOGuildConstructRewardRefSet.objName);
        //联盟杂物委托表
        public ALBasicListRefCore<GuildRandomEntrustRefObj> guildRandomEntrustRefCore = new ALBasicListRefCore<GuildRandomEntrustRefObj>(RefdataResCore.instance, GSOGuildRandomEntrustRefSet.assetPath, GSOGuildRandomEntrustRefSet.objName);
        //联盟杂物委托品质表
        public ALBasicListRefCore<GuildRandomEntrustQualityRefObj> guildRandomEntrustQualityRefCore = new ALBasicListRefCore<GuildRandomEntrustQualityRefObj>(RefdataResCore.instance, GSOGuildRandomEntrustQualityRefSet.assetPath, GSOGuildRandomEntrustQualityRefSet.objName);
        
        //藏品表
		public ALBasicMapListRefCore<EquipRefObj> equipRefCore = new ALBasicMapListRefCore<EquipRefObj>(RefdataResCore.instance, GSOEquipRefSet.assetPath, GSOEquipRefSet.objName);
		//弹幕组随机表
		public ALBasicListRefCore<CommentRandomGroupRefObj> commentRandomGroupRefCore = new ALBasicListRefCore<CommentRandomGroupRefObj>(RefdataResCore.instance, GSOCommentRandomGroupRefSet.assetPath, GSOCommentRandomGroupRefSet.objName);
    	//弹幕组随机表
		public ALBasicListRefCore<CommentPrefabRandomRefObj> commentPrefabRandomRefCore = new ALBasicListRefCore<CommentPrefabRandomRefObj>(RefdataResCore.instance, GSOCommentPrefabRandomRefSet.assetPath, GSOCommentPrefabRandomRefSet.objName);
		//弹幕名字随机表
		public ALBasicListRefCore<CommentNameRandomRefObj> commentNameRandomRefCore = new ALBasicListRefCore<CommentNameRandomRefObj>(RefdataResCore.instance, GSOCommentNameRandomRefSet.assetPath, GSOCommentNameRandomRefSet.objName);
		//弹幕头像随机表
		public ALBasicListRefCore<CommentIconRandomRefObj> commentIconRandomRefCore = new ALBasicListRefCore<CommentIconRandomRefObj>(RefdataResCore.instance, GSOCommentIconRandomRefSet.assetPath, GSOCommentIconRandomRefSet.objName);

        #region 抽卡表

        //抽卡卡池表
        public ALBasicListRefCore<GachaPoolRefObj> gachaPoolRefCore = new ALBasicListRefCore<GachaPoolRefObj>(RefdataResCore.instance, GSOGachaPoolRefSet.assetPath, GSOGachaPoolRefSet.objName);
        //抽卡道具表
        public ALBasicListRefCore<GachaItemRefObj> gachaItemRefCore = new ALBasicListRefCore<GachaItemRefObj>(RefdataResCore.instance, GSOGachaItemRefSet.assetPath, GSOGachaItemRefSet.objName);
        //抽卡道具展示信息表
        public ALBasicListRefCore<GachaItemShowInfoRefObj> gachaItemShowInfoRefCore = new ALBasicListRefCore<GachaItemShowInfoRefObj>(RefdataResCore.instance, GSOGachaItemShowInfoRefSet.assetPath, GSOGachaItemShowInfoRefSet.objName);
        //抽卡保底表
        public ALBasicListRefCore<GachaGuaranteeRefObj> gachaGuaranteeRefCore = new ALBasicListRefCore<GachaGuaranteeRefObj>(RefdataResCore.instance, GSOGachaGuaranteeRefSet.assetPath, GSOGachaGuaranteeRefSet.objName);

        #endregion

        #region 招募表

        //招募表
        public ALBasicListRefCore<RecruitRefObj> recruitRefCore = new ALBasicListRefCore<RecruitRefObj>(RefdataResCore.instance, GSORecruitRefSet.assetPath, GSORecruitRefSet.objName);
        //招募商店表
        public ALBasicListRefCore<RecruitShopRefObj> recruitShopRefCore = new ALBasicListRefCore<RecruitShopRefObj>(RefdataResCore.instance, GSORecruitShopRefSet.assetPath, GSORecruitShopRefSet.objName);

        #endregion
        
		//竞技场指定谈判道具表
		public ALBasicListRefCore<ArenaSelectAttackConsumeRefObj> arenaSelectAttackConsumeRefCore = new ALBasicListRefCore<ArenaSelectAttackConsumeRefObj>(RefdataResCore.instance, GSOArenaSelectAttackConsumeRefSet.assetPath, GSOArenaSelectAttackConsumeRefSet.objName);
		//竞技场贸易站等级表
		public ALBasicMapRefCore<ArenaStationLevelRefObj> arenaStationLevelRefCore = new ALBasicMapRefCore<ArenaStationLevelRefObj>(RefdataResCore.instance, GSOArenaStationLevelRefSet.assetPath, GSOArenaStationLevelRefSet.objName);
		//竞技场临时增益表
		public ALBasicMapRefCore<ArenaBuffRefObj> arenaBuffRefCore = new ALBasicMapRefCore<ArenaBuffRefObj>(RefdataResCore.instance, GSOArenaBuffRefSet.assetPath, GSOArenaBuffRefSet.objName);
		//竞技场轮次奖励表
		public ALBasicMapListRefCore<ArenaRoundRewardRefObj> arenaRoundRewardRefCore = new ALBasicMapListRefCore<ArenaRoundRewardRefObj>(RefdataResCore.instance, GSOArenaRoundRewardRefSet.assetPath, GSOArenaRoundRewardRefSet.objName);
		//竞技场最终奖励表
		public ALBasicMapRefCore<ArenaFinalRewardRefObj> arenaFinalRewardRefCore = new ALBasicMapRefCore<ArenaFinalRewardRefObj>(RefdataResCore.instance, GSOArenaFinalRewardRefSet.assetPath, GSOArenaFinalRewardRefSet.objName);
		//通用目标奖励表
		public ALBasicMapListRefCore<CommonTargetRewardRefObj> commonTargetRewardRefCore = new ALBasicMapListRefCore<CommonTargetRewardRefObj>(RefdataResCore.instance, GSOCommonTargetRewardRefSet.assetPath, GSOCommonTargetRewardRefSet.objName);
        //通用刷新表
        public ALBasicMapRefCore<CommonRefreshRefObj> commonRefreshRefCore = new ALBasicMapRefCore<CommonRefreshRefObj>(RefdataResCore.instance, GSOCommonRefreshRefSet.assetPath, GSOCommonRefreshRefSet.objName);
		//通用活动表
		public ALBasicMapRefCore<GActivityMainRefObj> activityMainRefCore = new ALBasicMapRefCore<GActivityMainRefObj>(RefdataResCore.instance, GSOActivityMainRefSet.assetPath, GSOActivityMainRefSet.objName);
        //通用活动排行奖励表
        public ALBasicListRefCore<GActivityRankRewardRefObj> activityRankRewardRefCore = new ALBasicListRefCore<GActivityRankRewardRefObj>(RefdataResCore.instance, GSOActivityRankRewardRefSet.assetPath, GSOActivityRankRewardRefSet.objName);
        //通用活动阶段奖励表
        public ALBasicListRefCore<GActivityStepRewardRefObj> activityStepRewardRefCore = new ALBasicListRefCore<GActivityStepRewardRefObj>(RefdataResCore.instance, GSOActivityStepRewardRefSet.assetPath, GSOActivityStepRewardRefSet.objName);
		//活动限时冲榜表
		public ALBasicListRefCore<ActivityRankRushRefObj> activityRankRushRefCore = new ALBasicListRefCore<ActivityRankRushRefObj>(RefdataResCore.instance, GSOActivityRankRushRefSet.assetPath, GSOActivityRankRushRefSet.objName);
		//爬塔表
		public ALBasicListRefCore<TowerChapterRefObj> towerChapterRefCore = new ALBasicListRefCore<TowerChapterRefObj>(RefdataResCore.instance, GSOTowerChapterRefSet.assetPath, GSOTowerChapterRefSet.objName);
		//爬塔关卡阶段表
		public ALBasicListRefCore<TowerChapterStageRefObj> towerChapterStageRefCore = new ALBasicListRefCore<TowerChapterStageRefObj>(RefdataResCore.instance, GSOTowerChapterStageRefSet.assetPath, GSOTowerChapterStageRefSet.objName);
		//爬塔调整展示阶梯表
		public ALBasicListRefCore<TowerChapterStageShowRefObj> towerChapterStageShowRefCore = new ALBasicListRefCore<TowerChapterStageShowRefObj>(RefdataResCore.instance, GSOTowerChapterStageShowRefSet.assetPath, GSOTowerChapterStageShowRefSet.objName);
		//玩家皮肤表
		public ALBasicMapListRefCore<PlayerSkinRefObj> playerSkinRefCore = new ALBasicMapListRefCore<PlayerSkinRefObj>(RefdataResCore.instance, GSOPlayerSkinRefSet.assetPath, GSOPlayerSkinRefSet.objName);
		//玩家皮肤等级表
		public ALBasicMapListRefCore<PlayerSkinLevelRefObj> playerSkinLevelRefCore = new ALBasicMapListRefCore<PlayerSkinLevelRefObj>(RefdataResCore.instance, GSOPlayerSkinLevelRefSet.assetPath, GSOPlayerSkinLevelRefSet.objName);
		//玩家称号表
		public ALBasicMapListRefCore<PlayerTitleRefObj> playerTitleRefCore = new ALBasicMapListRefCore<PlayerTitleRefObj>(RefdataResCore.instance, GSOPlayerTitleRefSet.assetPath, GSOPlayerTitleRefSet.objName);
		//玩家限时称号分组表
		public ALBasicMapRefCore<PlayerTitleLimitGroupRefObj> playerTitleLimitGroupRefCore = new ALBasicMapRefCore<PlayerTitleLimitGroupRefObj>(RefdataResCore.instance, GSOPlayerTitleLimitGroupRefSet.assetPath, GSOPlayerTitleLimitGroupRefSet.objName);
		//玩家组合称号前缀表
		public ALBasicMapListRefCore<PlayerTitlePrefixRefObj> playerTitlePrefixRefCore = new ALBasicMapListRefCore<PlayerTitlePrefixRefObj>(RefdataResCore.instance, GSOPlayerTitlePrefixRefSet.assetPath, GSOPlayerTitlePrefixRefSet.objName);
		//玩家组合称号后缀表
		public ALBasicMapListRefCore<PlayerTitleSuffixRefObj> playerTitleSuffixRefCore = new ALBasicMapListRefCore<PlayerTitleSuffixRefObj>(RefdataResCore.instance, GSOPlayerTitleSuffixRefSet.assetPath, GSOPlayerTitleSuffixRefSet.objName);
		//玩家组合称号底框表
		public ALBasicMapListRefCore<PlayerTitleBgRefObj> playerTitleBgRefCore = new ALBasicMapListRefCore<PlayerTitleBgRefObj>(RefdataResCore.instance, GSOPlayerTitleBgRefSet.assetPath, GSOPlayerTitleBgRefSet.objName);
		//午间副本Boss波次表
		public ALBasicListRefCore<MiddayDungeonWaveRefObj> middayDungeonWaveRefCore = new ALBasicListRefCore<MiddayDungeonWaveRefObj>(RefdataResCore.instance, GSOMiddayDungeonWaveRefSet.assetPath, GSOMiddayDungeonWaveRefSet.objName);
		//阶段奖励设置表
		public ALBasicListRefCore<ActivityStepRewardSetRefObj> stepRewardSetRefCore = new ALBasicListRefCore<ActivityStepRewardSetRefObj>(RefdataResCore.instance, GSOActivityStepRewardSetRefSet.assetPath, GSOActivityStepRewardSetRefSet.objName);
		//阶段奖励设置事件任务表
		public ALBasicListRefCore<ActivityStepRewardSetEventTaskRefObj> stepRewardSetEventTaskRefCore = new ALBasicListRefCore<ActivityStepRewardSetEventTaskRefObj>(RefdataResCore.instance, GSOActivityStepRewardSetEventTaskRefSet.assetPath, GSOActivityStepRewardSetEventTaskRefSet.objName);
		//商店总表
		public ALBasicMapRefCore<ShopMainRefObj> shopMainRefCore = new ALBasicMapRefCore<ShopMainRefObj>(RefdataResCore.instance, GSOShopMainRefSet.assetPath, GSOShopMainRefSet.objName);
		//千万目标奖励
		public ALBasicListRefCore<EarningGoalRewardRefObj> earningGoalRewardRefCore = new ALBasicListRefCore<EarningGoalRewardRefObj>(RefdataResCore.instance, GSOEarningGoalRewardRefSet.assetPath, GSOEarningGoalRewardRefSet.objName);
		//千万目标荣耀奖励表
		public ALBasicListRefCore<EarningGoalHonorRewardRefObj> earningGoalHonorRewardRefCore = new ALBasicListRefCore<EarningGoalHonorRewardRefObj>(RefdataResCore.instance, GSOEarningGoalHonorRewardRefSet.assetPath, GSOEarningGoalHonorRewardRefSet.objName);
		//七日登录
		public ALBasicListRefCore<SevenDayLoginRefObj> sevenDayLoginRefCore = new ALBasicListRefCore<SevenDayLoginRefObj>(RefdataResCore.instance, GSOSevenDayLoginRefSet.assetPath, GSOSevenDayLoginRefSet.objName);
		//活动商店表
		public ALBasicMapRefCore<ActivityShopRefObj> activityShopRefCore = new ALBasicMapRefCore<ActivityShopRefObj>(RefdataResCore.instance, GSOActivityShopRefSet.assetPath, GSOActivityShopRefSet.objName);
		//活动商店道具表
		public ALBasicMapListRefCore<ActivityShopItemRefObj> activityShopItemRefCore = new ALBasicMapListRefCore<ActivityShopItemRefObj>(RefdataResCore.instance, GSOActivityShopItemRefSet.assetPath, GSOActivityShopItemRefSet.objName);
		//钻石礼包组表
		public ALBasicMapRefCore<CrystalGiftPackGroupRefObj> crystalGiftPackGroupRefCore = new ALBasicMapRefCore<CrystalGiftPackGroupRefObj>(RefdataResCore.instance, GSOCrystalGiftPackGroupRefSet.assetPath, GSOCrystalGiftPackGroupRefSet.objName);
		//钻石礼包表
		public ALBasicMapListRefCore<CrystalGiftPackRefObj> crystalGiftPackRefCore = new ALBasicMapListRefCore<CrystalGiftPackRefObj>(RefdataResCore.instance, GSOCrystalGiftPackRefSet.assetPath, GSOCrystalGiftPackRefSet.objName);
		//活动兑换卷表
		public ALBasicMapRefCore<ActivityCurrencyRefObj> activityCurrencyRefCore = new ALBasicMapRefCore<ActivityCurrencyRefObj>(RefdataResCore.instance, GSOActivityCurrencyRefSet.assetPath, GSOActivityCurrencyRefSet.objName);
		//活动换皮表
		public ALBasicListRefCore<ActivityPrefabSkinRefObj> activityPrefabSkinRefCore = new ALBasicListRefCore<ActivityPrefabSkinRefObj>(RefdataResCore.instance, GSOActivityPrefabSkinRefSet.assetPath, GSOActivityPrefabSkinRefSet.objName);
		//创角预设表
		public ALBasicListRefCore<PlayerCreatPlayerPrefabRefObj> playerCreatPlayerPrefabRefCore = new ALBasicListRefCore<PlayerCreatPlayerPrefabRefObj>(RefdataResCore.instance, GSOPlayerCreatPlayerPrefabRefSet.assetPath, GSOPlayerCreatPlayerPrefabRefSet.objName);
		//活动中心表
		public ALBasicMapListRefCore<ActivityCenterRefObj> activityCenterRefCore = new ALBasicMapListRefCore<ActivityCenterRefObj>(RefdataResCore.instance, GSOActivityCenterRefSet.assetPath, GSOActivityCenterRefSet.objName);
		//倒计时事件表
		public ALBasicMapRefCore<CountdownEventRefObj> countdownEventRefCore = new ALBasicMapRefCore<CountdownEventRefObj>(RefdataResCore.instance, GSOCountdownEventRefSet.assetPath, GSOCountdownEventRefSet.objName);
		//妃子预设对话表
		public ALBasicListRefCore<ConsortChatDialogueRefObj> consortChatDialogueRefCore = new ALBasicListRefCore<ConsortChatDialogueRefObj>(RefdataResCore.instance, GSOConsortChatDialogueRefSet.assetPath, GSOConsortChatDialogueRefSet.objName);
		//妃子预设句子表
		public ALBasicListRefCore<ConsortChatDialogueSentenceRefObj> consortChatDialogueSentenceRefCore = new ALBasicListRefCore<ConsortChatDialogueSentenceRefObj>(RefdataResCore.instance, GSOConsortChatDialogueSentenceRefSet.assetPath, GSOConsortChatDialogueSentenceRefSet.objName);
		//妃子AI对话表
		public ALBasicListRefCore<ConsortChatAIRefObj> consortChatAIRefCore = new ALBasicListRefCore<ConsortChatAIRefObj>(RefdataResCore.instance, GSOConsortChatAIRefSet.assetPath, GSOConsortChatAIRefSet.objName);
		//妃子朋友圈表
		public ALBasicListRefCore<ConsortChatMomentsRefObj> consortChatMomentsRefCore = new ALBasicListRefCore<ConsortChatMomentsRefObj>(RefdataResCore.instance, GSOConsortChatMomentsRefSet.assetPath, GSOConsortChatMomentsRefSet.objName);
		//妃子朋友圈照片表
		public ALBasicListRefCore<ConsortChatImageGroupRefObj> consortChatImageGroupRefCore = new ALBasicListRefCore<ConsortChatImageGroupRefObj>(RefdataResCore.instance, GSOConsortChatImageGroupRefSet.assetPath, GSOConsortChatImageGroupRefSet.objName);
		//旅店等级表
		public ALBasicListRefCore<InnLevelRefObj> innLevelRefCore = new ALBasicListRefCore<InnLevelRefObj>(RefdataResCore.instance, GSOInnLevelRefSet.assetPath, GSOInnLevelRefSet.objName);
		//旅店奖牌等级表
		public ALBasicListRefCore<InnMedalLevelRefObj> innMedalLevelRefCore = new ALBasicListRefCore<InnMedalLevelRefObj>(RefdataResCore.instance, GSOInnMedalLevelRefSet.assetPath, GSOInnMedalLevelRefSet.objName);
		//旅店设施表
		public ALBasicListRefCore<InnStationRefObj> innStationRefCore = new ALBasicListRefCore<InnStationRefObj>(RefdataResCore.instance, GSOInnStationRefSet.assetPath, GSOInnStationRefSet.objName);
		//旅店设施等级表
		public ALBasicListRefCore<InnStationLevelRefObj> innStationLevelRefCore = new ALBasicListRefCore<InnStationLevelRefObj>(RefdataResCore.instance, GSOInnStationLevelRefSet.assetPath, GSOInnStationLevelRefSet.objName);
		//旅店菜品表
		public ALBasicListRefCore<InnDishRefObj> innDishRefCore = new ALBasicListRefCore<InnDishRefObj>(RefdataResCore.instance, GSOInnDishRefSet.assetPath, GSOInnDishRefSet.objName);
		//旅店菜品等级表
		public ALBasicListRefCore<InnDishLevelRefObj> innDishLevelRefCore = new ALBasicListRefCore<InnDishLevelRefObj>(RefdataResCore.instance, GSOInnDishLevelRefSet.assetPath, GSOInnDishLevelRefSet.objName);
		//旅店客人表
		public ALBasicListRefCore<InnGuestRefObj> innGuestRefCore = new ALBasicListRefCore<InnGuestRefObj>(RefdataResCore.instance, GSOInnGuestRefSet.assetPath, GSOInnGuestRefSet.objName);
		//旅店特殊客人表
		public ALBasicListRefCore<InnSpecialGuestRefObj> innSpecialGuestRefCore = new ALBasicListRefCore<InnSpecialGuestRefObj>(RefdataResCore.instance, GSOInnSpecialGuestRefSet.assetPath, GSOInnSpecialGuestRefSet.objName);
		//旅店表
		public ALBasicListRefCore<InnRecipeRefObj> innRecipeRefCore = new ALBasicListRefCore<InnRecipeRefObj>(RefdataResCore.instance, GSOInnRecipeRefSet.assetPath, GSOInnRecipeRefSet.objName);
		//博物馆表
		public ALBasicListRefCore<MuseumItemRefObj> museumItemRefCore = new ALBasicListRefCore<MuseumItemRefObj>(RefdataResCore.instance, GSOMuseumItemRefSet.assetPath, GSOMuseumItemRefSet.objName);
		//博物馆表
		public ALBasicListRefCore<MuseumItemLevelRefObj> museumItemLevelRefCore = new ALBasicListRefCore<MuseumItemLevelRefObj>(RefdataResCore.instance, GSOMuseumItemLevelRefSet.assetPath, GSOMuseumItemLevelRefSet.objName);
		//博物馆表
		public ALBasicListRefCore<MuseumItemUpgradeCostRefObj> museumItemUpgradeCostRefCore = new ALBasicListRefCore<MuseumItemUpgradeCostRefObj>(RefdataResCore.instance, GSOMuseumItemUpgradeCostRefSet.assetPath, GSOMuseumItemUpgradeCostRefSet.objName);
		//招聘体验表
		public ALBasicMapListRefCore<HireRefObj> hireRefCore = new ALBasicMapListRefCore<HireRefObj>(RefdataResCore.instance, GSOHireRefSet.assetPath, GSOHireRefSet.objName);
		//系统任务表
		public ALBasicMapListRefCore<SystemQuestRefObj> systemQuestRefCore = new ALBasicMapListRefCore<SystemQuestRefObj>(RefdataResCore.instance, GSOSystemQuestRefSet.assetPath, GSOSystemQuestRefSet.objName);
        //系统任务组表
        public ALBasicMapListRefCore<SystemQuestGroupRefObj> systemQuestGroupRefCore = new ALBasicMapListRefCore<SystemQuestGroupRefObj>(RefdataResCore.instance, GSOSystemQuestGroupRefSet.assetPath, GSOSystemQuestGroupRefSet.objName);
		//爬塔研究表
		public ALBasicListRefCore<TowerResearchRefObj> towerResearchRefCore = new ALBasicListRefCore<TowerResearchRefObj>(RefdataResCore.instance, GSOTowerResearchRefSet.assetPath, GSOTowerResearchRefSet.objName);
		//礼包表
		public ALBasicMapRefCore<GiftPackRefObj> giftPackRefCore = new ALBasicMapRefCore<GiftPackRefObj>(RefdataResCore.instance, GSOGiftPackRefSet.assetPath, GSOGiftPackRefSet.objName);
		//礼包组表
		public ALBasicMapListRefCore<GiftPackGroupRefObj> giftPackGroupRefCore = new ALBasicMapListRefCore<GiftPackGroupRefObj>(RefdataResCore.instance, GSOGiftPackGroupRefSet.assetPath, GSOGiftPackGroupRefSet.objName);
		//支付档位表
		public ALBasicMapRefCore<PayRefObj> payRefCore = new ALBasicMapRefCore<PayRefObj>(RefdataResCore.instance, GSOPayRefSet.assetPath, GSOPayRefSet.objName);
		//太空寻宝 - 矿石表
		public ALBasicListRefCore<TreasureHuntOreRefObj> treasureHuntOreRefCore = new ALBasicListRefCore<TreasureHuntOreRefObj>(RefdataResCore.instance, GSOTreasureHuntOreRefSet.assetPath, GSOTreasureHuntOreRefSet.objName);
		//太空寻宝 - 奇物表
		public ALBasicListRefCore<TreasureHuntTreasureRefObj> treasureHuntTreasureRefCore = new ALBasicListRefCore<TreasureHuntTreasureRefObj>(RefdataResCore.instance, GSOTreasureHuntTreasureRefSet.assetPath, GSOTreasureHuntTreasureRefSet.objName);
		//太空寻宝 - 实验室表
		public ALBasicListRefCore<TreasureHuntLabRefObj> treasureHuntLabRefCore = new ALBasicListRefCore<TreasureHuntLabRefObj>(RefdataResCore.instance, GSOTreasureHuntLabRefSet.assetPath, GSOTreasureHuntLabRefSet.objName);
		//太空寻宝 - 太空区域表
		public ALBasicListRefCore<TreasureHuntAreaRefObj> treasureHuntAreaRefCore = new ALBasicListRefCore<TreasureHuntAreaRefObj>(RefdataResCore.instance, GSOTreasureHuntAreaRefSet.assetPath, GSOTreasureHuntAreaRefSet.objName);
		//太空寻宝 - 图鉴页签表
		public ALBasicListRefCore<TreasureHuntCatalogTabRefObj> treasureHuntCatalogTabRefCore = new ALBasicListRefCore<TreasureHuntCatalogTabRefObj>(RefdataResCore.instance, GSOTreasureHuntCatalogTabRefSet.assetPath, GSOTreasureHuntCatalogTabRefSet.objName);
		//太空寻宝 - 太空舱等级表
		public ALBasicListRefCore<TreasureHuntStationLvlRefObj> treasureHuntStationLvlRefCore = new ALBasicListRefCore<TreasureHuntStationLvlRefObj>(RefdataResCore.instance, GSOTreasureHuntStationLvlRefSet.assetPath, GSOTreasureHuntStationLvlRefSet.objName);
		//太空寻宝 - 技能表
		public ALBasicListRefCore<TreasureHuntSkillRefObj> treasureHuntSkillRefCore = new ALBasicListRefCore<TreasureHuntSkillRefObj>(RefdataResCore.instance, GSOTreasureHuntSkillRefSet.assetPath, GSOTreasureHuntSkillRefSet.objName);
		//太空寻宝 - 技能等级表
		public ALBasicListRefCore<TreasureHuntSkillLevelRefObj> treasureHuntSkillLevelRefCore = new ALBasicListRefCore<TreasureHuntSkillLevelRefObj>(RefdataResCore.instance, GSOTreasureHuntSkillLevelRefSet.assetPath, GSOTreasureHuntSkillLevelRefSet.objName);
		//太空寻宝 - 组合图鉴表
		public ALBasicListRefCore<TreasureHuntCompositeCatalogRefObj> treasureHuntCompositeCatalogRefCore = new ALBasicListRefCore<TreasureHuntCompositeCatalogRefObj>(RefdataResCore.instance, GSOTreasureHuntCompositeCatalogRefSet.assetPath, GSOTreasureHuntCompositeCatalogRefSet.objName);
		//杰出者大厅
		public ALBasicListRefCore<GraveMainRefObj> graveMainRefCore = new ALBasicListRefCore<GraveMainRefObj>(RefdataResCore.instance, GSOGraveMainRefSet.assetPath, GSOGraveMainRefSet.objName);
		//杰出者大厅
		public ALBasicListRefCore<GraveTypeRefObj> graveTypeRefCore = new ALBasicListRefCore<GraveTypeRefObj>(RefdataResCore.instance, GSOGraveTypeRefSet.assetPath, GSOGraveTypeRefSet.objName);
		//玩家buff事件表
		public ALBasicListRefCore<PlayerBuffEventRefObj> playerBuffEventRefCore = new ALBasicListRefCore<PlayerBuffEventRefObj>(RefdataResCore.instance, GSOPlayerBuffEventRefSet.assetPath, GSOPlayerBuffEventRefSet.objName);
		//寻宝区域距离表
		public ALBasicListRefCore<TreasureHuntAreaDistanceRefObj> treasureHuntAreaDistanceRefCore = new ALBasicListRefCore<TreasureHuntAreaDistanceRefObj>(RefdataResCore.instance, GSOTreasureHuntAreaDistanceRefSet.assetPath, GSOTreasureHuntAreaDistanceRefSet.objName);
		//联盟PVE副本
		public ALBasicListRefCore<GuildDungeonRefObj> guildDungeonRefCore = new ALBasicListRefCore<GuildDungeonRefObj>(RefdataResCore.instance, GSOGuildDungeonRefSet.assetPath, GSOGuildDungeonRefSet.objName);
		//联盟PVE副本等级表
		public ALBasicListRefCore<GuildDungeonLvlRefObj> guildDungeonLvlRefCore = new ALBasicListRefCore<GuildDungeonLvlRefObj>(RefdataResCore.instance, GSOGuildDungeonLvlRefSet.assetPath, GSOGuildDungeonLvlRefSet.objName);
		//联盟PVE副本怪物表
		public ALBasicListRefCore<GuildDungeonMonsterRefObj> guildDungeonMonsterRefCore = new ALBasicListRefCore<GuildDungeonMonsterRefObj>(RefdataResCore.instance, GSOGuildDungeonMonsterRefSet.assetPath, GSOGuildDungeonMonsterRefSet.objName);
		//联盟PVE副本怪物展示
		public ALBasicListRefCore<GuildDungeonMonsterShowRefObj> guildDungeonMonsterShowRefCore = new ALBasicListRefCore<GuildDungeonMonsterShowRefObj>(RefdataResCore.instance, GSOGuildDungeonMonsterShowRefSet.assetPath, GSOGuildDungeonMonsterShowRefSet.objName);
		//火星所有建筑表
		public ALBasicListRefCore<MarsAllBuildingRefObj> marsAllBuildingRefCore = new ALBasicListRefCore<MarsAllBuildingRefObj>(RefdataResCore.instance, GSOMarsAllBuildingRefSet.assetPath, GSOMarsAllBuildingRefSet.objName);
		//火星建筑
		public ALBasicListRefCore<MarsBuildingRefObj> marsBuildingRefCore = new ALBasicListRefCore<MarsBuildingRefObj>(RefdataResCore.instance, GSOMarsBuildingRefSet.assetPath, GSOMarsBuildingRefSet.objName);
		//火星建筑等级
		public ALBasicListRefCore<MarsBuildingLevelRefObj> marsBuildingLevelRefCore = new ALBasicListRefCore<MarsBuildingLevelRefObj>(RefdataResCore.instance, GSOMarsBuildingLevelRefSet.assetPath, GSOMarsBuildingLevelRefSet.objName);
		//火星主基地等级
		public ALBasicListRefCore<MarsBuildingHomeLevelRefObj> marsBuildingHomeLevelRefCore = new ALBasicListRefCore<MarsBuildingHomeLevelRefObj>(RefdataResCore.instance, GSOMarsBuildingHomeLevelRefSet.assetPath, GSOMarsBuildingHomeLevelRefSet.objName);
        //火星建筑部件归属
        public ALBasicListRefCore<MarsBuildingEquipmentBelongRefObj> marsBuildingEquipmentBelongRefCore = new ALBasicListRefCore<MarsBuildingEquipmentBelongRefObj>(RefdataResCore.instance, GSOMarsBuildingEquipmentBelongRefSet.assetPath, GSOMarsBuildingEquipmentBelongRefSet.objName);
		//火星建筑派遣等级
		public ALBasicListRefCore<MarsBuildingSettleLevelRefObj> marsBuildingSettleLevelRefCore = new ALBasicListRefCore<MarsBuildingSettleLevelRefObj>(RefdataResCore.instance, GSOMarsBuildingSettleLevelRefSet.assetPath, GSOMarsBuildingSettleLevelRefSet.objName);
		//火星部件
		public ALBasicListRefCore<MarsEquipmentRefObj> marsEquipmentRefCore = new ALBasicListRefCore<MarsEquipmentRefObj>(RefdataResCore.instance, GSOMarsEquipmentRefSet.assetPath, GSOMarsEquipmentRefSet.objName);
		//火星部件等级
		public ALBasicListRefCore<MarsEquipmentLevelRefObj> marsEquipmentLevelRefCore = new ALBasicListRefCore<MarsEquipmentLevelRefObj>(RefdataResCore.instance, GSOMarsEquipmentLevelRefSet.assetPath, GSOMarsEquipmentLevelRefSet.objName);
		//火星部件能源等级
		public ALBasicListRefCore<MarsEquipmentEnergyLevelRefObj> marsEquipmentEnergyLevelRefCore = new ALBasicListRefCore<MarsEquipmentEnergyLevelRefObj>(RefdataResCore.instance, GSOMarsEquipmentEnergyLevelRefSet.assetPath, GSOMarsEquipmentEnergyLevelRefSet.objName);
		//火星部件生活等级
		public ALBasicListRefCore<MarsEquipmentLivingLevelRefObj> marsEquipmentLivingLevelRefCore = new ALBasicListRefCore<MarsEquipmentLivingLevelRefObj>(RefdataResCore.instance, GSOMarsEquipmentLivingLevelRefSet.assetPath, GSOMarsEquipmentLivingLevelRefSet.objName);
		//火星部件食物等级
		public ALBasicListRefCore<MarsEquipmentFoodLevelRefObj> marsEquipmentFoodLevelRefCore = new ALBasicListRefCore<MarsEquipmentFoodLevelRefObj>(RefdataResCore.instance, GSOMarsEquipmentFoodLevelRefSet.assetPath, GSOMarsEquipmentFoodLevelRefSet.objName);
		//火星部件医院等级
		public ALBasicListRefCore<MarsEquipmentHospitalLevelRefObj> marsEquipmentHospitalLevelRefCore = new ALBasicListRefCore<MarsEquipmentHospitalLevelRefObj>(RefdataResCore.instance, GSOMarsEquipmentHospitalLevelRefSet.assetPath, GSOMarsEquipmentHospitalLevelRefSet.objName);
		//背包物品时间减少
		public ALBasicListRefCore<MarsBagItemTimeReduceRefObj> marsBagItemTimeReduceRefCore = new ALBasicListRefCore<MarsBagItemTimeReduceRefObj>(RefdataResCore.instance, GSOMasrBagItemTimeReduceRefSet.assetPath, GSOMasrBagItemTimeReduceRefSet.objName);
		//火星航行表
		public ALBasicMapListRefCore<MarsGoRouteRefObj> marsGoRouteRefCore = new ALBasicMapListRefCore<MarsGoRouteRefObj>(RefdataResCore.instance, GSOMarsGoRouteRefSet.assetPath, GSOMarsGoRouteRefSet.objName);
		//火星航行日志表
		public ALBasicMapRefCore<MarsGoRouteLogRefObj> marsGoRouteLogRefCore = new ALBasicMapRefCore<MarsGoRouteLogRefObj>(RefdataResCore.instance, GSOMarsGoRouteLogRefSet.assetPath, GSOMarsGoRouteLogRefSet.objName);
		//妃子朋友圈背景组表
		public ALBasicListRefCore<ConsortMomentsBgGroupRefObj> consortMomentsBgGroupRefCore = new ALBasicListRefCore<ConsortMomentsBgGroupRefObj>(RefdataResCore.instance, GSOConsortMomentsBgGroupRefSet.assetPath, GSOConsortMomentsBgGroupRefSet.objName);
		//妃子朋友圈妃子图片组表
		public ALBasicListRefCore<ConsortMomentsConsortGroupRefObj> consortMomentsConsortGroupRefCore = new ALBasicListRefCore<ConsortMomentsConsortGroupRefObj>(RefdataResCore.instance, GSOConsortMomentsConsortGroupRefSet.assetPath, GSOConsortMomentsConsortGroupRefSet.objName);
		//妃子朋友圈妃子图片表
		public ALBasicListRefCore<ConsortMomentsConsortRefObj> consortMomentsConsortRefCore = new ALBasicListRefCore<ConsortMomentsConsortRefObj>(RefdataResCore.instance, GSOConsortMomentsConsortRefSet.assetPath, GSOConsortMomentsConsortRefSet.objName);
		//妃子朋友圈背景图片表
		public ALBasicListRefCore<ConsortMomentsBgRefObj> consortMomentsBgRefCore = new ALBasicListRefCore<ConsortMomentsBgRefObj>(RefdataResCore.instance, GSOConsortMomentsBgRefSet.assetPath, GSOConsortMomentsBgRefSet.objName);
		//火星智能控制表
		public ALBasicListRefCore<MarsIntelligentControlRefObj> marsIntelligentControlRefCore = new ALBasicListRefCore<MarsIntelligentControlRefObj>(RefdataResCore.instance, GSOMarsIntelligentControlRefSet.assetPath, GSOMarsIntelligentControlRefSet.objName);
		//火星满意度表
		public ALBasicListRefCore<MarsSatisfactionDegreeRefObj> marsSatisfactionDegreeRefCore = new ALBasicListRefCore<MarsSatisfactionDegreeRefObj>(RefdataResCore.instance, GSOMarsSatisfactionDegreeRefSet.assetPath, GSOMarsSatisfactionDegreeRefSet.objName);
		//火星人民信件表
		public ALBasicSqliteSetRefCore<MarsPeopleLetterRefObj> marsPeopleLetterRefCore = new ALBasicSqliteSetRefCore<MarsPeopleLetterRefObj>(GGameSqliteMgr.instance, MarsPeopleLetterRefObj.assetPath, MarsPeopleLetterRefObj.objName, MarsPeopleLetterRefObj.tableName, "Id");
		//火星居民求助表
		public ALBasicListRefCore<MarsPeopleHelpRefObj> marsPeopleHelpRefCore = new ALBasicListRefCore<MarsPeopleHelpRefObj>(RefdataResCore.instance, GSOMarsPeopleHelpRefSet.assetPath, GSOMarsPeopleHelpRefSet.objName);
		//火星居民选择求助表
		public ALBasicListRefCore<MarsPeopleChoiceHelpRefObj> marsPeopleChoiceHelpRefCore = new ALBasicListRefCore<MarsPeopleChoiceHelpRefObj>(RefdataResCore.instance, GSOMarsPeopleChoiceHelpRefSet.assetPath, GSOMarsPeopleChoiceHelpRefSet.objName);
		//火星居民奖励求助表
		public ALBasicListRefCore<MarsPeopleRewardHelpRefObj> marsPeopleRewardHelpRefCore = new ALBasicListRefCore<MarsPeopleRewardHelpRefObj>(RefdataResCore.instance, GSOMarsPeopleRewardHelpRefSet.assetPath, GSOMarsPeopleRewardHelpRefSet.objName);
		//火星基地事件表
		public ALBasicListRefCore<MarsEventRefObj> marsEventRefCore = new ALBasicListRefCore<MarsEventRefObj>(RefdataResCore.instance, GSOMarsEventRefSet.assetPath, GSOMarsEventRefSet.objName);
		//火星移民表
		public ALBasicListRefCore<MarsImmigrationRefObj> marsImmigrationRefCore = new ALBasicListRefCore<MarsImmigrationRefObj>(RefdataResCore.instance, GSOMarsImmigrationRefSet.assetPath, GSOMarsImmigrationRefSet.objName);
		//公会协作区域表
		public ALBasicMapRefCore<GuildCooperateAreaRefObj> guildCooperateAreaRefCore = new ALBasicMapRefCore<GuildCooperateAreaRefObj>(RefdataResCore.instance, GSOGuildCooperateAreaRefSet.assetPath, GSOGuildCooperateAreaRefSet.objName);
		//公会协作据点表
		public ALBasicMapRefCore<GuildCooperateAreaPosRefObj> guildCooperateAreaPosRefCore = new ALBasicMapRefCore<GuildCooperateAreaPosRefObj>(RefdataResCore.instance, GSOGuildCooperateAreaPosRefSet.assetPath, GSOGuildCooperateAreaPosRefSet.objName);
		//太空寻宝奇物产出表
		public ALBasicListRefCore<TreasureHuntTreasureOutputRefObj> treasureHuntTreasureOutputRefCore = new ALBasicListRefCore<TreasureHuntTreasureOutputRefObj>(RefdataResCore.instance, GSOTreasureHuntTreasureOutputRefSet.assetPath, GSOTreasureHuntTreasureOutputRefSet.objName);
		//红点监听表
		public ALBasicMapListRefCore<RedMonitorRefObj> redMonitorRefCore = new ALBasicMapListRefCore<RedMonitorRefObj>(RefdataResCore.instance, GSORedMonitorRefSet.assetPath, GSORedMonitorRefSet.objName);
		//火星建筑建造条件表
		public ALBasicListRefCore<MarsBuildingConditionRefObj> marsBuildingConditionRefCore = new ALBasicListRefCore<MarsBuildingConditionRefObj>(RefdataResCore.instance, GSOMarsBuildingConditionRefSet.assetPath, GSOMarsBuildingConditionRefSet.objName);
		//子嗣配音组表
		public ALBasicListRefCore<ChildVoiceGroupRefObj> childVoiceGroupRefCore = new ALBasicListRefCore<ChildVoiceGroupRefObj>(RefdataResCore.instance, GSOChildVoiceGroupRefSet.assetPath, GSOChildVoiceGroupRefSet.objName);
		//特殊客人选择表
		public ALBasicListRefCore<InnSpecialGuestChoiceRefObj> innSpecialGuestChoiceRefCore = new ALBasicListRefCore<InnSpecialGuestChoiceRefObj>(RefdataResCore.instance, GSOInnSpecialGuestChoiceRefSet.assetPath, GSOInnSpecialGuestChoiceRefSet.objName);
		//特殊客人选择选项表
		public ALBasicListRefCore<InnSpecialGuestChoiceOptionRefObj> innSpecialGuestChoiceOptionRefCore = new ALBasicListRefCore<InnSpecialGuestChoiceOptionRefObj>(RefdataResCore.instance, GSOInnSpecialGuestChoiceOptionRefSet.assetPath, GSOInnSpecialGuestChoiceOptionRefSet.objName);
		//VIP表
		public ALBasicMapListRefCore<VipRefObj> vipRefCore = new ALBasicMapListRefCore<VipRefObj>(RefdataResCore.instance, GSOVipRefSet.assetPath, GSOVipRefSet.objName);
		//火星探索等级表
		public ALBasicListRefCore<MarsExploreLvlRefObj> marsExploreLvlRefCore = new ALBasicListRefCore<MarsExploreLvlRefObj>(RefdataResCore.instance, GSOMarsExploreLvlRefSet.assetPath, GSOMarsExploreLvlRefSet.objName);
		//火星探索位置表
		public ALBasicListRefCore<MarsExplorePosRefObj> marsExplorePosRefCore = new ALBasicListRefCore<MarsExplorePosRefObj>(RefdataResCore.instance, GSOMarsExplorePosRefSet.assetPath, GSOMarsExplorePosRefSet.objName);
		//火星探索事件表
		public ALBasicListRefCore<MarsExploreEventRefObj> marsExploreEventRefCore = new ALBasicListRefCore<MarsExploreEventRefObj>(RefdataResCore.instance, GSOMarsExploreEventRefSet.assetPath, GSOMarsExploreEventRefSet.objName);
		//火星探索战斗事件表
		public ALBasicListRefCore<MarsExploreEventBattleRefObj> marsExploreEventBattleRefCore = new ALBasicListRefCore<MarsExploreEventBattleRefObj>(RefdataResCore.instance, GSOMarsExploreEventBattleRefSet.assetPath, GSOMarsExploreEventBattleRefSet.objName);
		//火星探索PVE事件表
		public ALBasicListRefCore<MarsExploreEventBossRefObj> marsExploreEventBossRefCore = new ALBasicListRefCore<MarsExploreEventBossRefObj>(RefdataResCore.instance, GSOMarsExploreEventBossRefSet.assetPath, GSOMarsExploreEventBossRefSet.objName);
		//火星探索队伍表
		public ALBasicListRefCore<MarsExploreTeamRefObj> marsExploreTeamRefCore = new ALBasicListRefCore<MarsExploreTeamRefObj>(RefdataResCore.instance, GSOMarsExploreTeamRefSet.assetPath, GSOMarsExploreTeamRefSet.objName);
		//火星科技表
		public ALBasicListRefCore<MarsTechnologyRefObj> marsTechnologyRefCore = new ALBasicListRefCore<MarsTechnologyRefObj>(RefdataResCore.instance, GSOMarsTechnologyRefSet.assetPath, GSOMarsTechnologyRefSet.objName);
		//火星科技等级表
		public ALBasicMapRefCore<MarsTechnologyLevelRefObj> marsTechnologyLevelRefCore = new ALBasicMapRefCore<MarsTechnologyLevelRefObj>(RefdataResCore.instance, GSOMarsTechnologyLevelRefSet.assetPath, GSOMarsTechnologyLevelRefSet.objName);
		//礼包额外获得表
		public ALBasicListRefCore<GiftPackExtraGainRefObj> giftPackExtraGainRefCore = new ALBasicListRefCore<GiftPackExtraGainRefObj>(RefdataResCore.instance, GSOGiftPackExtraGainRefSet.assetPath, GSOGiftPackExtraGainRefSet.objName);
		//首充天数表
		public ALBasicMapListRefCore<FirstRechargeDayRefObj> firstRechargeDayRefCore = new ALBasicMapListRefCore<FirstRechargeDayRefObj>(RefdataResCore.instance, GSOFirstRechargeDayRefSet.assetPath, GSOFirstRechargeDayRefSet.objName);
		//充值返利组表
		public ALBasicMapListRefCore<RechargeRebateGroupRefObj> rechargeRebateGroupRefCore = new ALBasicMapListRefCore<RechargeRebateGroupRefObj>(RefdataResCore.instance, GSORechargeRebateGroupRefSet.assetPath, GSORechargeRebateGroupRefSet.objName);
		//充值返利阶段表
		public ALBasicMapListRefCore<RechargeRebateStepRefObj> rechargeRebateStepRefCore = new ALBasicMapListRefCore<RechargeRebateStepRefObj>(RefdataResCore.instance, GSORechargeRebateStepRefSet.assetPath, GSORechargeRebateStepRefSet.objName);
		//火星科技类型表
		public ALBasicListRefCore<MarsTechnologyTypeRefObj> marsTechnologyTypeRefCore = new ALBasicListRefCore<MarsTechnologyTypeRefObj>(RefdataResCore.instance, GSOMarsTechnologyTypeRefSet.assetPath, GSOMarsTechnologyTypeRefSet.objName);
		//玩家属性展示表
		public ALBasicListRefCore<PlayerPropertyShowRefObj> playerPropertyShowRefCore = new ALBasicListRefCore<PlayerPropertyShowRefObj>(RefdataResCore.instance, GSOPlayerPropertyShowRefSet.assetPath, GSOPlayerPropertyShowRefSet.objName);
		// 火星属性展示表
		public ALBasicListRefCore<MarsPropertyShowRefObj> marsPropertyShowRefCore = new ALBasicListRefCore<MarsPropertyShowRefObj>(RefdataResCore.instance, GSOMarsPropertyShowRefSet.assetPath, GSOMarsPropertyShowRefSet.objName);
		//火星探索矿点表
		public ALBasicListRefCore<MarsExploreMineRefObj> marsExploreMineRefCore = new ALBasicListRefCore<MarsExploreMineRefObj>(RefdataResCore.instance, GSOMarsExploreMineRefSet.assetPath, GSOMarsExploreMineRefSet.objName);
		//火星探索收集奖励表
		public ALBasicListRefCore<MarsExploreCollectBonusRefObj> marsExploreCollectBonusRefCore = new ALBasicListRefCore<MarsExploreCollectBonusRefObj>(RefdataResCore.instance, GSOMarsExploreCollectBonusRefSet.assetPath, GSOMarsExploreCollectBonusRefSet.objName);
		//火星道具时间类型表
		public ALBasicListRefCore<MarsBagItemTimeTypeRefObj> marsBagItemTimeTypeRefCore = new ALBasicListRefCore<MarsBagItemTimeTypeRefObj>(RefdataResCore.instance, GSOMarsBagItemTimeTypeRefSet.assetPath, GSOMarsBagItemTimeTypeRefSet.objName);
		//权益卡表
		public ALBasicMapRefCore<PrivilegeCardRefObj> privilegeCardRefCore = new ALBasicMapRefCore<PrivilegeCardRefObj>(RefdataResCore.instance, GSOPrivilegeCardRefSet.assetPath, GSOPrivilegeCardRefSet.objName);
		//玩家权限表
		public ALBasicMapRefCore<PlayerPermissionsRefObj> playerPermissionsRefCore = new ALBasicMapRefCore<PlayerPermissionsRefObj>(RefdataResCore.instance, GSOPlayerPermissionsRefSet.assetPath, GSOPlayerPermissionsRefSet.objName);
		//推送礼包组表
		public ALBasicListRefCore<PushGiftGroupRefObj> pushGiftGroupRefCore = new ALBasicListRefCore<PushGiftGroupRefObj>(RefdataResCore.instance, GSOPushGiftGroupRefSet.assetPath, GSOPushGiftGroupRefSet.objName);
		//推送礼包表
		public ALBasicMapRefCore<PushGiftPackRefObj> pushGiftPackRefCore = new ALBasicMapRefCore<PushGiftPackRefObj>(RefdataResCore.instance, GSOPushGiftPackRefSet.assetPath, GSOPushGiftPackRefSet.objName);
		//缺少物品触发推送礼包表
		public ALBasicListRefCore<PushGiftItemTriggerRefObj> pushGiftItemTriggerRefCore = new ALBasicListRefCore<PushGiftItemTriggerRefObj>(RefdataResCore.instance, GSOPushGiftItemTriggerRefSet.assetPath, GSOPushGiftItemTriggerRefSet.objName);

		//联盟宝箱
		public ALBasicListRefCore<GuildBoxRefObj> guildBoxRefCore = new ALBasicListRefCore<GuildBoxRefObj>(RefdataResCore.instance, GSOGuildBoxRefSet.assetPath, GSOGuildBoxRefSet.objName);
		//玩家永久加成表
		public ALBasicMapRefCore<PlayerForeverAddRefObj> playerForeverAddRefCore = new ALBasicMapRefCore<PlayerForeverAddRefObj>(RefdataResCore.instance, GSOPlayerForeverAddRefSet.assetPath, GSOPlayerForeverAddRefSet.objName);
		//午间副本宝箱
		public ALBasicListRefCore<MiddayDungeonBoxRefObj> middayDungeonBoxRefCore = new ALBasicListRefCore<MiddayDungeonBoxRefObj>(RefdataResCore.instance, GSOMiddayDungeonBoxRefSet.assetPath, GSOMiddayDungeonBoxRefSet.objName);
		//基金主表
		public ALBasicListRefCore<ActivityFundRefObj> activityFundRefCore = new ALBasicListRefCore<ActivityFundRefObj>(RefdataResCore.instance, GSOActivityFundRefSet.assetPath, GSOActivityFundRefSet.objName);
		//基金等级表
		public ALBasicListRefCore<ActivityFundLevelRefObj> activityFundLevelRefCore = new ALBasicListRefCore<ActivityFundLevelRefObj>(RefdataResCore.instance, GSOActivityFundLevelRefSet.assetPath, GSOActivityFundLevelRefSet.objName);
		//基金阶段表
		public ALBasicListRefCore<ActivityFundStepRefObj> activityFundStepRefCore = new ALBasicListRefCore<ActivityFundStepRefObj>(RefdataResCore.instance, GSOActivityFundStepRefSet.assetPath, GSOActivityFundStepRefSet.objName);
		//基金任务表
		public ALBasicListRefCore<ActivityFundTaskRefObj> activityFundTaskRefCore = new ALBasicListRefCore<ActivityFundTaskRefObj>(RefdataResCore.instance, GSOActivityFundTaskRefSet.assetPath, GSOActivityFundTaskRefSet.objName);
		//聊天系统通知表
		public ALBasicListRefCore<ChatSystemLogRefObj> chatSystemLogRefCore = new ALBasicListRefCore<ChatSystemLogRefObj>(RefdataResCore.instance, GSOChatSystemLogRefSet.assetPath, GSOChatSystemLogRefSet.objName);
		//限时兑换
		public ALBasicListRefCore<RushExchangeRefObj> rushExchangeRefCore = new ALBasicListRefCore<RushExchangeRefObj>(RefdataResCore.instance, GSORushExchangeRefSet.assetPath, GSORushExchangeRefSet.objName);
		//限时兑换物品价值
		public ALBasicListRefCore<RushExchangeItemRefObj> rushExchangeItemRefCore = new ALBasicListRefCore<RushExchangeItemRefObj>(RefdataResCore.instance, GSORushExchangeItemRefSet.assetPath, GSORushExchangeItemRefSet.objName);
		//限时兑换组
		public ALBasicListRefCore<RushExchangeGroupRefObj> rushExchangeGroupRefCore = new ALBasicListRefCore<RushExchangeGroupRefObj>(RefdataResCore.instance, GSORushExchangeGroupRefSet.assetPath, GSORushExchangeGroupRefSet.objName);
		//玩家卧室皮肤表
		public ALBasicListRefCore<PlayerRoomSkinRefObj> playerRoomSkinRefCore = new ALBasicListRefCore<PlayerRoomSkinRefObj>(RefdataResCore.instance, GSOPlayerRoomSkinRefSet.assetPath, GSOPlayerRoomSkinRefSet.objName);
	
		//活动组队表
		public ALBasicListRefCore<ActivityTeamRefObj> activityTeamRefCore = new ALBasicListRefCore<ActivityTeamRefObj>(RefdataResCore.instance, GSOActivityTeamRefSet.assetPath, GSOActivityTeamRefSet.objName);
		//通用表现组主表
		public ALBasicListRefCore<PerformGroupRefObj> performGroupRefCore = new ALBasicListRefCore<PerformGroupRefObj>(RefdataResCore.instance, GSOPerformGroupRefSet.assetPath, GSOPerformGroupRefSet.objName);
		//通用表现组子项表
		public ALBasicListRefCore<PerformGroupItemRefObj> performGroupItemRefCore = new ALBasicListRefCore<PerformGroupItemRefObj>(RefdataResCore.instance, GSOPerformGroupItemRefSet.assetPath, GSOPerformGroupItemRefSet.objName);
		//通用表现组对话类型子表
		public ALBasicListRefCore<PerformGroupDialogueRefObj> performGroupDialogueRefCore = new ALBasicListRefCore<PerformGroupDialogueRefObj>(RefdataResCore.instance, GSOPerformGroupDialogueRefSet.assetPath, GSOPerformGroupDialogueRefSet.objName);
		//>>>>>>>>>> AUTO GENERATE END   <<<<<<<<<<
        
        //是否初始化完成
        private bool _m_bIsInitDone = false;

        public GRefdataCoreMgr()
            : base()
        {
            _m_bIsInitDone = false;
        }

        public bool isInitDone { get { return _m_bIsInitDone; } }

        //初始化加载队列
        protected override void _initLoadList(List<_IALInitRefObj> _list)
        {
            #region 代表确认过的，改动完确认过的模块挪进来

            _list.Add(npGeneralMap);//通用配置表

            _list.Add(sfxMap);//特效数据
            _list.Add(sfx3dList);//特效数据
            _list.Add(audioMap);//音效
            _list.Add(audioGroupRefCore);//音效组
            _list.Add(voiceKeyRefCore);//配音key表

            _list.Add(qualityRefCore);//品质信息部分
            _list.Add(qualityExtRefCore);//品质额外信息部分
            
            _list.Add(detectorCharacterMap);//屏蔽字
            _list.Add(detectorPlayerNameMap);//取名屏蔽字

            _list.Add(remoteEffectMap);//远程效果
            
            _list.Add(effectGoToRefCore);//Goto效果表

            _list.Add(localPushCore);//本地推送表
            _list.Add(mainCityPushNoticeRefCore);//主城推送弹窗表
            _list.Add(pushNoticeActivityMergeRefCore);//活动合并展示推送表
            
            _list.Add(ruleList);//规则
            _list.Add(ruleSubList);//规则子表
            
            _list.Add(unicodeLengthCheckRefCore);//语言长度表
            _list.Add(redTipRefCore);//红点表
            
            _list.Add(loginWayList);//登录方式数据
            _list.Add(loginAreaRefCore);//登入大区信息
            
            _list.Add(uniformRefCore);//uniform表
            
            _list.Add(tutorialRefCore);//引导数据表
            _list.Add(tutorialEdgeRefCore);//引导数据表
            _list.Add(simpleTutorialRefCore);//简易引导
            _list.Add(simpleTutorialEdgeRefCore);//简易引导边
            
            _list.Add(dialogueMap);//对话主表
            _list.Add(dialogueSentenceRefCore);//对话句子表
            _list.Add(dialogueResponseOptionRefCore);//对话选项表
            
            _list.Add(lazyCdMap);//LazyCD表
            _list.Add(rewardMap);//奖励

            _list.Add(chatRoomRefCore);//聊天室配置
            _list.Add(chatNPCRefCore);//聊天npc
            _list.Add(chatEmoteGroupRefCore);//聊天表情组
            _list.Add(chatEmoteItemRefCore);//聊天表情
            
            _list.Add(sysInfoRefMap);//系统信息
            _list.Add(simpleUnlockMap);//快速解锁信息
            
            _list.Add(accessRefCore);//获取途径表
            _list.Add(mainNodeToFunctionTypeRefCore);//界面跳转类型和系统功能的映射表
            _list.Add(funcUnlockRefCore);//功能解锁表
            
            _list.Add(languageArgsRefCore);//语言翻译替换表
            _list.Add(fixedCdMap);//固定时间恢复的CD表
            
            _list.Add(showCaseActorBehaviorRefCore);//showcase模型额外表现表
            _list.Add(timesPriceRefCore);//加个递增表
            _list.Add(particleMap);//粒子表
            _list.Add(uiResPathRefCore);//ui资源路径表
            
            _list.Add(sceneInfoRefCore);//主场景数据对象

            _list.Add(bagItemCore);//背包物品表
            _list.Add(itemDefCore);//item_def
            _list.Add(itemExchangeCore);//物品转换表
            _list.Add(bagItemUseCore);//背包物品使用表
            _list.Add(bagItemHeroCore);//骑士物品表
            _list.Add(bagItemConsortCore);//情人物品表
            _list.Add(bagItemDyeCore);//染色物品表
            _list.Add(itemConvertCore);//物品兑换表
            _list.Add(itemAlterCore);//物品替换表
            
            _list.Add(versionUpRewardMap);//版本升级奖励表

            _list.Add(achievePointRefCore);//成就点数
            _list.Add(achieveMap);//成就数据
            _list.Add(achieveStepList);//成就步骤数据
            _list.Add(achievePointStepList);//成就点数阶段数据
            _list.Add(achieveTypeMap);//成就类型数据

            _list.Add(mailRefCore);//邮件
            _list.Add(mailTypeRefCore);//邮件类型表
            _list.Add(mailSenderRefCore);//邮件发送者
            
            _list.Add(playerBuffMap);//玩家buff
            
            _list.Add(commonBoxMap);//通用宝箱表
            _list.Add(shareMap);//聊天分享表
            
            _list.Add(tipMap);//上浮提示表

            _list.Add(rankCommonRefCore);//排行榜通用表
            _list.Add(rankFixedRefCore);//排行榜常驻表
            
            _list.Add(questMap);//任务表
            _list.Add(questStepMap);//任务步骤表
            _list.Add(questTargetMap);//任务目标表
            _list.Add(questGroupMap);//任务章节表
            _list.Add(dailyQuestMap);//日常周常任务表
            _list.Add(dailyQuestRewardListCore);//日常周常奖励表
            
            _list.Add(shopMap); //商店表
            _list.Add(shopItemMap); //商品表
            _list.Add(shopItemGroupMap);//商店商品组表
            _list.Add(shopItemDiscountMap); //商品打折表
            
            _list.Add(queueDealerTipMap);//队列处理提示表
            
            _list.Add(currencyResCore);//玩家资源表
            
            _list.Add(iconBgkCore);//玩家头像框表
            _list.Add(playerIconCore);//玩家头像表
            _list.Add(playerBubbleCore);//玩家气泡框

            _list.Add(childAttrCore);
            _list.Add(childCareerCore);
            _list.Add(childInitResCore);//子嗣初始化资源
            _list.Add(childResCore);//子嗣资源
            _list.Add(childQualityCore);//子嗣天资
            _list.Add(childSeatCore);//子嗣座位

            _list.Add(consortRefCore);//情人表
            _list.Add(consortStoryRefCore);//家人故事表
            _list.Add(consortStoryBgRefCore);//妃子故事背景表
            _list.Add(consortFettersLvlRefCore);//家人羁绊等级
            _list.Add(consortFettersSkillRefCore);//家人羁绊技能
            _list.Add(consortFettersSkillLvlRefCore);//家人羁绊技能等级
            _list.Add(consortBusinessSkillRefCore);//家人经营技能表
            _list.Add(consortBusinessPotentialRefCore);//家人经营潜力表
            _list.Add(consortBlessSkillRefCore);//家人加护技能表
            _list.Add(consortBlessSkillLvlRefCore);//家人加护技能等级表
            _list.Add(consortSkinRefCore);//家人皮肤表
            _list.Add(consortSkinLvlRefCore);//家人皮肤等级表
            _list.Add(consortTravelRefCore);//家人旅游表
            _list.Add(consortHaloLvlRefCore);//家人光环等级表
            _list.Add(consortHaloSkillRefCore);//家人星辉技能表
            _list.Add(consortHaloSkillLvlRefCore);//家人星辉技能等级表
            _list.Add(consortVoiceGroupRefCore);//妃子配音组表
            _list.Add(consortCGRefCore);//妃子CG表

            _list.Add(proAddGroupRefCore);//加成概率表
            _list.Add(opCostGroupRefCore);//操作消耗表
            
            _list.Add(heroRefCore);//伙伴表
            _list.Add(heroLevelRefCore);//骑士等级表
            _list.Add(heroStarRefCore);//伙伴觉醒表
            _list.Add(heroStarSkillRefCore);//伙伴觉醒技能表
            _list.Add(heroStarSkillLevelRefCore);//伙伴觉醒技能等级表
            _list.Add(heroSkinRefCore);//伙伴皮肤表
            _list.Add(heroSkinLevelRefCore);//伙伴皮肤等级表
            _list.Add(heroStepRefCore);//伙伴阶段表
            _list.Add(heroTalentSkillRefCore);//伙伴资质技能表
            _list.Add(heroTalentSkillLevelRefCore);//伙伴资质技能等级表
            _list.Add(heroBusinessSkillRefCore);//伙伴经营技能表
            _list.Add(heroBusinessSkillUpgradeRefCore);//伙伴经营技能升级消耗组表
            _list.Add(heroHaloRefCore);//伙伴光环表
            _list.Add(heroHaloLevelRefCore);//伙伴光环等级表
            _list.Add(heroHaloSuitRefCore);//伙伴套系表
            _list.Add(heroHaloSuitSkillRefCore);//伙伴套系技能表
            _list.Add(heroHaloSuitSkillLevelRefCore);//伙伴套系技能等级表
            _list.Add(heroVoiceGroupRefCore);//伙伴配音组表

            _list.Add(basicAttrRefCore);//通用属性表
            _list.Add(entryPointRefCore);//入口点表
       
            _list.Add(commonEventAwardRefCore);//通用事件-奖励事件实例表
            _list.Add(commonEventAwardShowRefCore);//通用事件-选择事件选项表
            _list.Add(commonEventChoiceOptionRefCore);//通用事件-选择事件选项表
            _list.Add(commonEventChoiceRefCore);//通用选择事件实例表
            _list.Add(commonEventChoiceShowRefCore);//通用选择事件展示表
            _list.Add(commonEventDialogRefCore);//通用对话事件实例表
            _list.Add(commonEventDispatchCondRefCore);//通用派遣事件条件表
            _list.Add(commonEventDispatchRefCore);//通用派遣事件实例表
            _list.Add(commonEventDispatchResultRefCore);//通用派遣事件结果表
            _list.Add(commonEventDispatchShowRefCore);//通用派遣事件问题表
            _list.Add(commonEventRefCore);//通用事件主表
            _list.Add(commonEventRewardRefCore);//通用事件奖励
            _list.Add(commonEventPlotDialogRefCore);//通用事件-剧情对话事件表
            _list.Add(commonEventMiniGameRefCore);//通用事件-小游戏事件表
            _list.Add(commonEventMiniGameShowRefCore);//通用事件-小游戏事件展示表
            
            _list.Add(dinnerTypeRefCore);
            _list.Add(dinnerJoinCostRefCore);
            _list.Add(dinnerPermitRefCore);//宴会凭证表

            //玩家等级
            _list.Add(playerLvlCore);
            _list.Add(playerHeroUnlockShowCore); //玩家英雄解锁展示表
            
            _list.Add(npcRefCore);//NPC表
            _list.Add(npcActorRefCore);//NPC-Actor类型子表
            _list.Add(npcGoRefCore);//NPC-独立模型子表
            
            _list.Add(anecdoteEventChoiceOptionRefCore);//政务事件选项表
            _list.Add(anecdoteEventChoiceRefCore);//政务事件表
            _list.Add(anecdoteEventEarningsRefCore);//政务事件收益表
            _list.Add(anecdoteEventRefCore);//经营事件表
            _list.Add(anecdoteEventRewardRefCore);//经营事件奖励表
            _list.Add(anecdotePosRefCore);//经营事件位置表
            
            //游历
            _list.Add(travelPosCore);
            _list.Add(travelConsortRefCore);//游历妃子表
			_list.Add(travelEventTypeRefCore);//游历事件类型表
			_list.Add(travelEventRefCore);//游历事件表
			_list.Add(travelEventOnceRefCore);//游历单次事件表
			_list.Add(travelEventConsortLikeRefCore);//游历妃子好感度事件表
			_list.Add(travelEventConsortIntimacyRefCore);//游历妃子亲密度事件表
			_list.Add(travelEventConsortBarRefCore);//游历妃子酒馆事件表
			_list.Add(travelEventConsortBarCostRefCore);//游历妃子酒馆事件消耗表
			_list.Add(travelEventChangeRefCore);//游历交换事件表
			_list.Add(travelEventInvitationRefCore);//游历邀约事件表
			_list.Add(travelEventGiftedRefCore);//游历卷王事件表
            _list.Add(travelEventGambleRefCore);//游历博彩事件表
			_list.Add(travelEventAddPowerRefCore);//游历大臣实力事件表
			_list.Add(travelEventAkeyRefCore);//游历事件一键表
            
            //小游戏
            _list.Add(miniGameMainRefCore);
            _list.Add(puzzleGameRefCore);
            _list.Add(findThingsGameRefCore);
            _list.Add(takeThingsSequentiallyGameRefCore);
            _list.Add(qteGameRefCore);
            _list.Add(qteClickOpportunityGameRefCore);
            _list.Add(dragBoxGameRefCore);
            
            //漫画
            _list.Add(simpleCommicRefCore);
            
            //跑马灯
            _list.Add(marqueeRefCore);
            
            _list.Add(cuteActorRefCore);
            //猫咪气泡表
            _list.Add(catBubbleRefCore);
            
            //增量包表
            _list.Add(addPackRefCore);
            _list.Add(addPackPathsRefCore);

            //运营公告海报图表
            _list.Add(announcementBannerRefCore);
            //运营公告游戏内跳转表
            _list.Add(announcementJumpRefCore);
            //运营公告海报图表
            _list.Add(announcementTabRefCore);

            _list.Add(buildingRefCore);
            _list.Add(businessBuildingRefCore);
            _list.Add(businessBuildingHireCostRefCore);
            _list.Add(businessBuildingLevelRefCore);
            _list.Add(businessBuildingVideoGroupRefCore);
            _list.Add(farmingBuildingRefCore);
            _list.Add(farmingBuildingLevelRefCore);
            _list.Add(businessBuildingDevelopRefCore);
            _list.Add(businessBuildingProductRefCore);
            
            _list.Add(chapterRefCore);
            _list.Add(chapterCostRefCore);
            _list.Add(chapterBuildUnlockRefCore);
            _list.Add(chapterBossStyleRefCore);
            _list.Add(chapterNodeStyleRefCore);
            
            _list.Add(stageGoalBigStepRefCore);
            _list.Add(stageGoalRefCore);
            _list.Add(stageGoalTaskRefCore);
            
            _list.Add(eveningDungeonRankRewardRefCore);
            
            _list.Add(sevenDayGoalsTaskRefCore);
            _list.Add(sevenDayGoalsTaskRewardRefCore);
            _list.Add(sevenDayGoalsStepRewardRefCore);
            _list.Add(sevenDayGoalsGiftPackRefCore);
            
            #endregion

            #region 待确认，待删除的旧表
            
            _list.Add(bagItemPartyCore);//聚会保护站道具子表
            
            _list.Add(playerGenderCore);//性别表

            _list.Add(playerPropertyCore);//玩家属性表
            
            _list.Add(attachmentItemRefCore);//附加物表


            _list.Add(playerPrefabRefCore);//创角表

            #endregion

            #region 自动生成添加的表

            //>>>>>>>>>> AUTO GENERATE ADD LIST START <<<<<<<<<<
			_list.Add(guildLevelRefCore);//联盟等级表
			_list.Add(guildPositionRefCore);//联盟职位表
			_list.Add(guildFlagRefCore);//联盟旗帜表
			_list.Add(guildConstructRefCore);//联盟建设表
			_list.Add(guildLogRefCore);//联盟日志表
            _list.Add(guildJoinLimitRefCore);//入盟限制表
            _list.Add(guildConstructRewardRefCore);//联盟捐赠进度奖励表
            _list.Add(guildRandomEntrustRefCore);//联盟杂物委托表
            _list.Add(guildRandomEntrustQualityRefCore);//联盟杂物委托品质表
			_list.Add(equipRefCore);//藏品表
			_list.Add(commentRandomGroupRefCore);//弹幕组随机表
			_list.Add(commentPrefabRandomRefCore);//弹幕组随机表
			_list.Add(commentNameRandomRefCore);//弹幕名字随机表
			_list.Add(commentIconRandomRefCore);//弹幕头像随机表
			_list.Add(gachaPoolRefCore);//抽卡卡池表
			_list.Add(gachaItemRefCore);//抽卡道具表
			_list.Add(gachaItemShowInfoRefCore);//抽卡道具展示信息表
			_list.Add(gachaGuaranteeRefCore);//抽卡保底表
			_list.Add(recruitRefCore);//招募表
			_list.Add(recruitShopRefCore);//招募商店表
			_list.Add(arenaSelectAttackConsumeRefCore);//竞技场指定谈判道具表
			_list.Add(arenaStationLevelRefCore);//竞技场贸易站等级表
			_list.Add(arenaBuffRefCore);//竞技场临时增益表
			_list.Add(arenaRoundRewardRefCore);//竞技场轮次奖励表
			_list.Add(arenaFinalRewardRefCore);//竞技场最终奖励表
			_list.Add(commonTargetRewardRefCore);//通用目标奖励表
            _list.Add(commonRefreshRefCore);//通用刷新表
			_list.Add(activityMainRefCore);//通用活动表
			_list.Add(activityRankRewardRefCore);//通用活动排行奖励表
			_list.Add(activityStepRewardRefCore);//通用活动阶段奖励表
            _list.Add(activityRankRushRefCore);//活动限时冲榜表
			_list.Add(towerChapterRefCore);//爬塔表
			_list.Add(towerChapterStageRefCore);//爬塔关卡阶段表
			_list.Add(towerChapterStageShowRefCore);//爬塔调整展示阶梯表
			_list.Add(chapterEventRefCore);//关卡事件表
			_list.Add(chapterEventRewardRefCore);//关卡事件奖励表
			_list.Add(chapterEventChoiceRefCore);//关卡选择事件表
			_list.Add(chapterEventChoiceOptionRefCore);//关卡选择事件选项表
			_list.Add(chapterEventDispatchRefCore);//关卡派遣事件表
			_list.Add(chapterEventDispatchShowRefCore);//关卡派遣事件展示表
			_list.Add(chapterEventDispatchCondRefCore);//关卡派遣事件条件表
			_list.Add(chapterStageRefCore);//关卡节点表
			_list.Add(chapterStagePlotRefCore);//关卡节剧情表
			_list.Add(chapterStoryRefCore);//关卡故事表
			_list.Add(playerSkinRefCore);//玩家皮肤表
			_list.Add(playerSkinLevelRefCore);//玩家皮肤等级表
			_list.Add(playerTitleRefCore);//玩家称号表
			_list.Add(playerTitleLimitGroupRefCore);//玩家限时称号分组表
			_list.Add(playerTitlePrefixRefCore);//玩家组合称号前缀表
			_list.Add(playerTitleSuffixRefCore);//玩家组合称号后缀表
			_list.Add(playerTitleBgRefCore);//玩家组合称号底框表
			_list.Add(middayDungeonWaveRefCore);//午间副本Boss波次表
			_list.Add(stepRewardSetRefCore);//阶段奖励设置表
			_list.Add(stepRewardSetEventTaskRefCore);//阶段奖励设置事件任务表
			_list.Add(shopMainRefCore);//商店总表
			_list.Add(earningGoalRewardRefCore);//千万目标奖励
			_list.Add(earningGoalHonorRewardRefCore);//千万目标荣耀奖励表
			_list.Add(sevenDayLoginRefCore);//七日登录
			_list.Add(activityShopRefCore);//活动商店表
			_list.Add(activityShopItemRefCore);//活动商店道具表
			_list.Add(crystalGiftPackGroupRefCore);//钻石礼包组表
			_list.Add(crystalGiftPackRefCore);//钻石礼包表
			_list.Add(activityCurrencyRefCore);//活动兑换卷表
			_list.Add(activityPrefabSkinRefCore);//活动换皮表
			_list.Add(playerCreatPlayerPrefabRefCore);//创角预设表
			_list.Add(activityCenterRefCore);//活动中心表
			_list.Add(countdownEventRefCore);//倒计时事件表
			_list.Add(consortChatDialogueRefCore);//妃子预设对话表
			_list.Add(consortChatDialogueSentenceRefCore);//妃子预设句子表
			_list.Add(consortChatAIRefCore);//妃子AI对话表
			_list.Add(consortChatMomentsRefCore);//妃子朋友圈表
			_list.Add(consortChatImageGroupRefCore);//妃子朋友圈照片表
			_list.Add(innLevelRefCore);//旅店等级表
			_list.Add(innMedalLevelRefCore);//旅店奖牌等级表
			_list.Add(innStationRefCore);//旅店设施表
			_list.Add(innStationLevelRefCore);//旅店设施等级表
			_list.Add(innDishRefCore);//旅店菜品表
			_list.Add(innDishLevelRefCore);//旅店菜品等级表
			_list.Add(innGuestRefCore);//旅店客人表
			_list.Add(innSpecialGuestRefCore);//旅店特殊客人表
			_list.Add(innRecipeRefCore);//旅店表
			_list.Add(museumItemRefCore);//博物馆表
			_list.Add(museumItemLevelRefCore);//博物馆表
			_list.Add(museumItemUpgradeCostRefCore);//博物馆表
			_list.Add(hireRefCore);//招聘体验表
			_list.Add(systemQuestRefCore);//系统任务表
			_list.Add(systemQuestGroupRefCore);//系统任务组表
			_list.Add(towerResearchRefCore);//爬塔研究表
			_list.Add(giftPackRefCore);//礼包表
			_list.Add(giftPackGroupRefCore);//礼包组表
			_list.Add(payRefCore);//支付档位表
			_list.Add(treasureHuntOreRefCore);//太空寻宝 - 矿石表
			_list.Add(treasureHuntTreasureRefCore);//太空寻宝 - 奇物表
			_list.Add(treasureHuntLabRefCore);//太空寻宝 - 实验室表
			_list.Add(treasureHuntAreaRefCore);//太空寻宝 - 太空区域表
			_list.Add(treasureHuntCatalogTabRefCore);//太空寻宝 - 图鉴页签表
			_list.Add(treasureHuntStationLvlRefCore);//太空寻宝 - 太空舱等级表
			_list.Add(treasureHuntSkillRefCore);//太空寻宝 - 技能表
			_list.Add(treasureHuntSkillLevelRefCore);//太空寻宝 - 技能等级表
			_list.Add(treasureHuntCompositeCatalogRefCore);//太空寻宝 - 组合图鉴表
			_list.Add(graveMainRefCore);//杰出者大厅
			_list.Add(graveTypeRefCore);//杰出者大厅
			_list.Add(playerBuffEventRefCore);//玩家buff事件表
			_list.Add(treasureHuntAreaDistanceRefCore);//寻宝区域距离表
			_list.Add(guildDungeonRefCore);//联盟PVE副本
			_list.Add(guildDungeonLvlRefCore);//联盟PVE副本等级表
			_list.Add(guildDungeonMonsterRefCore);//联盟PVE副本怪物表
			_list.Add(guildDungeonMonsterShowRefCore);//联盟PVE副本怪物展示
			_list.Add(marsAllBuildingRefCore);//火星所有建筑表
			_list.Add(marsBuildingRefCore);//火星建筑
			_list.Add(marsBuildingLevelRefCore);//火星建筑等级
			_list.Add(marsBuildingHomeLevelRefCore);//火星主基地等级
			_list.Add(marsBuildingEquipmentBelongRefCore);//火星建筑部件归属
			_list.Add(marsBuildingSettleLevelRefCore);//火星建筑派遣等级
			_list.Add(marsEquipmentRefCore);//火星部件
			_list.Add(marsEquipmentLevelRefCore);//火星部件等级
			_list.Add(marsEquipmentEnergyLevelRefCore);//火星部件能源等级
			_list.Add(marsEquipmentLivingLevelRefCore);//火星部件生活等级
			_list.Add(marsEquipmentFoodLevelRefCore);//火星部件食物等级
			_list.Add(marsEquipmentHospitalLevelRefCore);//火星部件医院等级
			_list.Add(marsBagItemTimeReduceRefCore);//背包物品时间减少
			_list.Add(marsGoRouteRefCore);//火星航行表
            _list.Add(marsGoRouteLogRefCore);//火星航行日志表
			_list.Add(consortMomentsBgGroupRefCore);//妃子朋友圈背景组表
			_list.Add(consortMomentsConsortGroupRefCore);//妃子朋友圈妃子图片组表
			_list.Add(consortMomentsConsortRefCore);//妃子朋友圈妃子图片表
			_list.Add(consortMomentsBgRefCore);//妃子朋友圈背景图片表
			_list.Add(marsIntelligentControlRefCore);//火星智能控制表
			_list.Add(marsSatisfactionDegreeRefCore);//火星满意度表
			_list.Add(marsPeopleLetterRefCore);//火星人民信件表
			_list.Add(marsPeopleHelpRefCore);//火星居民求助表
			_list.Add(marsPeopleChoiceHelpRefCore);//火星居民选择求助表
			_list.Add(marsPeopleRewardHelpRefCore);//火星居民奖励求助表
			_list.Add(marsEventRefCore);//火星基地事件表
			_list.Add(marsImmigrationRefCore);//火星移民表
			_list.Add(guildCooperateAreaRefCore);//公会协作区域表
			_list.Add(guildCooperateAreaPosRefCore);//公会协作据点表
			_list.Add(treasureHuntTreasureOutputRefCore);//太空寻宝奇物产出表
			_list.Add(redMonitorRefCore);//红点监听表
			_list.Add(marsBuildingConditionRefCore);//火星建筑建造条件表
			_list.Add(childVoiceGroupRefCore);//子嗣配音组表
			_list.Add(innSpecialGuestChoiceRefCore);//特殊客人选择表
			_list.Add(innSpecialGuestChoiceOptionRefCore);//特殊客人选择选项表
			_list.Add(vipRefCore);//VIP表
			_list.Add(marsExploreLvlRefCore);//火星探索等级表
			_list.Add(marsExplorePosRefCore);//火星探索位置表
			_list.Add(marsExploreEventRefCore);//火星探索事件表
			_list.Add(marsExploreEventBattleRefCore);//火星探索战斗事件表
			_list.Add(marsExploreEventBossRefCore);//火星探索PVE事件表
			_list.Add(marsExploreTeamRefCore);//火星探索队伍表
			_list.Add(marsTechnologyRefCore);//火星科技表
			_list.Add(marsTechnologyLevelRefCore);//火星科技等级表
			_list.Add(giftPackExtraGainRefCore);//礼包额外获得表
			_list.Add(firstRechargeDayRefCore);//首充天数表
			_list.Add(rechargeRebateGroupRefCore);//充值返利组表
			_list.Add(rechargeRebateStepRefCore);//充值返利阶段表
			_list.Add(marsTechnologyTypeRefCore);//火星科技类型表
			_list.Add(playerPropertyShowRefCore);//玩家属性展示表
			_list.Add(marsPropertyShowRefCore);// 火星属性展示表
			_list.Add(marsExploreMineRefCore);//火星探索矿点表
			_list.Add(marsExploreCollectBonusRefCore);//火星探索收集奖励表
			_list.Add(marsBagItemTimeTypeRefCore);//火星道具时间类型表
			_list.Add(privilegeCardRefCore);//权益卡表
			_list.Add(playerPermissionsRefCore);//玩家权限表
            _list.Add(pushGiftGroupRefCore);//推送礼包组表
			_list.Add(pushGiftPackRefCore);//推送礼包表
			_list.Add(pushGiftItemTriggerRefCore);//缺少物品触发推送礼包表
			_list.Add(guildBoxRefCore);//联盟宝箱
			_list.Add(playerForeverAddRefCore);//玩家永久加成表
			_list.Add(middayDungeonBoxRefCore);//午间副本宝箱
			_list.Add(activityFundRefCore);//基金主表
			_list.Add(activityFundLevelRefCore);//基金等级表
			_list.Add(activityFundStepRefCore);//基金阶段表
			_list.Add(activityFundTaskRefCore);//基金任务表
			_list.Add(chatSystemLogRefCore);//聊天系统通知表
			_list.Add(rushExchangeRefCore);//限时兑换
			_list.Add(rushExchangeItemRefCore);//限时兑换物品价值
			_list.Add(rushExchangeGroupRefCore);//限时兑换组
			_list.Add(playerRoomSkinRefCore);//玩家卧室皮肤表
			_list.Add(activityTeamRefCore);//活动组队表
			_list.Add(performGroupRefCore);//通用表现组主表
			_list.Add(performGroupItemRefCore);//通用表现组子项表
			_list.Add(performGroupDialogueRefCore);//通用表现组对话类型子表
			//>>>>>>>>>> AUTO GENERATE ADD LIST END   <<<<<<<<<<

            #endregion
        }


        /** 加载失败处理 */
        protected override void _onRefInitFail(Type _class, _IALInitRefObj _obj)
        {
#if UNITY_EDITOR || UNITY_STANDALONE
        if(null == _class)
            NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.init_gameres_fail_str, "unkown"), TextTranslate.instance.getLanguage(TransKeyConst.confirm), _obj.finalDelegate);
        else
            NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.init_gameres_fail_str, _class.ToString()), TextTranslate.instance.getLanguage(TransKeyConst.confirm), _obj.finalDelegate);
#else
        if (null == _class)
            NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.init_gameres_fail_str, "unkown"), TextTranslate.instance.getLanguage(TransKeyConst.confirm), _obj.finalDelegate);
        else
            NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.init_gameres_fail_str, _class.ToString()), TextTranslate.instance.getLanguage(TransKeyConst.confirm), _obj.finalDelegate);
#endif

            //设置初始化完成
            _m_bIsInitDone = true;
        }


        protected override void _onInitAllRefCore()
        {
            //通用配置表
            npGeneral = npGeneralMap.getRef(NPSOGeneralRefSet.generalId);
            // npSpaceOther = npSpaceOtherMap.getRef(NPGSOSpaceOtherRefSet.spaceOtherId);
            // npMiniMapOther = npMiniMapOtherMap.getRef(NPGSOMiniMapOtherRefSet.minimapOtherId);
            
            //初始化所有替代信息
            ItemAlterRefObj alterRef = null;
            for (int i = 0; i < itemAlterCore.refList.Count; i++)
            {
                alterRef = itemAlterCore.refList[i];
                if (null == alterRef)
                    continue;

                //加入数据集
                _m_iamItemAlterMgr.addAlter(alterRef);
            }

            //初始化特效信息，将3d和数据关联
            _initSfxRef();

            //初始化任务相关信息
            _initQuestRefCore();

            //初始化大地图传送点配置
            // _initTeleportRef();
            
            //初始化小地图配置
            // _initMiniMapItemRef();
            
            //初始化成就相关数据
            _initAchieveRefCore();
            
            //初始化增量包相关数据
            _initAddPackRefCore();

            _initAnecdote();

            //初始化分享表相关信息
            _initShareRefCore();

            //初始化大地图Action附加表现配置
            // _initSpaceActionAdditionRef();

            //初始化大地图Action子表引用管理
            // _initSpaceActionSubRef();

            //初始化排行榜排名奖励数据
            //_initRankCommonRefCore();

            //次数价格表初始化
            _initTimePriceRef();

            //规则表初始化
            _initRuleRef();
            
            //情人相关初始化
            _initConsort();

            _initConsortChat();
            
            _initChat();

            _initChild();

            _initHero();

            _initBuilding();

            _initTravel();

            //活动相关
            _initActivityRefCore();
            
            //爬塔数据初始化
            _initTowerChapter();

            _initStageGoal();

            _initBagItem();

            _initSevenDayGoals();
            
            _initInn();
            
            _initMuseum();

            // 关卡数据初始化
            _initChapter();

            _initTreasureHunt();//太空寻宝相关数据初始化

            _initMarsBuilding();
            
            _initMarsRefCore();//初始化火星基地相关配表

            _initPushGiftPackData();//初始化推送礼包数据

            _initFund();//初始化基金数据
            
            //初始化击中音效
            // NPHitAudioMgr.instance.init();

            //非法字符表数据初始化
            CharacterDetermineMgr.instance.init();

            //设置初始化完成
            _m_bIsInitDone = true;

            //editor下校验一次资源
#if UNITY_EDITOR
            UIResPathAssistant.CheckRes();
#endif
        }

        /// <summary>
        /// 初始化设置语言相关
        /// </summary>
        /// <param name="_language"></param>
        public void InitRefLanguage(ENPLanguage _language, Action _doneAction)
        {
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.regAllDoneDelegate(_doneAction);
            stepCounter.chgTotalStepCount(2);
            playerNameRefCore.InitRefLanguage(_language, stepCounter.addDoneStepCount);
            childNameRefCore.InitRefLanguage(_language, stepCounter.addDoneStepCount);
        }
        
        /// <summary>
        /// 更新设置语言相关
        /// </summary>
        /// <param name="_language"></param>
        public void updateRefLanguage(ENPLanguage _language, Action _doneAction)
        {
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.regAllDoneDelegate(_doneAction);
            stepCounter.chgTotalStepCount(2);
            playerNameRefCore.updateRefLanguage(_language, stepCounter.addDoneStepCount);
            childNameRefCore.updateRefLanguage(_language, stepCounter.addDoneStepCount);
        }

        //获取品质信息
        public NPQualityRefObj getQualityRef(EQuality _quality)
        {
            return getQuality(ENPQualityClass.NONE, _quality);
        }

        //获取替代物品
        public List<ItemAlterRefObj> getItemAlterRefObj(NPEnum.ENPItemType _srcItemType, long _srcItemId)
        {
            return _m_iamItemAlterMgr.getAlterList(_srcItemType, _srcItemId);
        }
    }
}
