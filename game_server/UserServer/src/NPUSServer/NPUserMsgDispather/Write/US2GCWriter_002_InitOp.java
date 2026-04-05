package NPUSServer.NPUserMsgDispather.Write;

import Common.ActivityObj.Activity_HotRefInfo;
import Common.CommonFuncObj.GiftPack_Info;
import Common.Common_CrossServerGroupInfo;
import Common.Common_MarqueeShowPosReadInfo;
import Common.Common_RedDotInfo;
import Common.GuildCooperateObj.GuildCooperate_Info;
import Common.QuestObj.Quest_Count;
import Common.RankGiftPackObj.RankGiftPack_Info;
import CommonEnum.ESpecialItemType;
import GS2GC.p002_InitOp.*;
import NPEnum.ENPPlayerComboTitleType;
import NPUSServer.CommonActivityMgr.CommonActivityMgr;
import NPUSServer.DinnerMgr.DinnerInfo;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.Guild.Member.GuildMemberInfo;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ChapterComp.ChapterInfo;
import NPUSServer.NPUSUserMgr.UserComp.ChapterComp.Event._AChapterEvent;
import NPUSServer.NPUSUserMgr.UserComp.SpecialItemComponent.Dealer.SpecialItemDealer_FarmMutiple;
import NPUSServer.NPUSUserMgr.UserComp.SpecialItemComponent.Dealer.SpecialItemDealer_Gold;
import NPUSServer.NPUSUserMgr.UserComp.SpecialItemComponent.Dealer.SpecialItemDealer_MarsEnergy;
import NPUSServer.NPUSUserMgr.UserComp.SpecialItemComponent.Dealer.SpecialItemDealer_Station;
import NPUSServer.ShieldCidMgr.ShieldCidInfo;

import java.util.ArrayList;
import java.util.List;

public class US2GCWriter_002_InitOp
{
    public static GS2GC_002_001_RetPlayerInfo NPGS2GC_001_RetPlayerInfo(NPUSUserData _userData)
    {
        GS2GC_002_001_RetPlayerInfo proto = new GS2GC_002_001_RetPlayerInfo();
        proto.setCid(_userData.getCid());
        proto.setCname(_userData.getPlayerComponent().getBo().getCname());
        _userData.getPlayerComponent().getParamMgr().WriteProto(proto);

        return proto;
    }

    public static GS2GC_002_002_RetCrossServerGroupInfo make_002_RetCrossServerGroupInfo(Common_CrossServerGroupInfo _groupInfo)
    {
        return new GS2GC_002_002_RetCrossServerGroupInfo(_groupInfo);
    }

    public static GS2GC_002_003_RetCurrencyList NPGS2GC_003_RetCurrencyList(NPUSUserData _userData)
    {
        GS2GC_002_003_RetCurrencyList proto = new GS2GC_002_003_RetCurrencyList();
        _userData.getCurrencyComponent().makeProtocol(proto.getCurrencyList());

        return proto;
    }

    public static GS2GC_002_004_RetEquipInit make_004_RetEquipInit(NPUSUserData _userData)
    {
        GS2GC_002_004_RetEquipInit proto = new GS2GC_002_004_RetEquipInit();
        _userData.getEquipComponent().makeProtocol(proto.getEquipList());

        return proto;
    }

    public static GS2GC_002_005_RetPlayerSkinInit make_005_RetPlayerSkinInit(NPUSUserData _userData)
    {
        GS2GC_002_005_RetPlayerSkinInit proto = new GS2GC_002_005_RetPlayerSkinInit();
        _userData.getPlayerSkinComp().makeProto(proto.getSkinList());

        return proto;
    }
    
    public static GS2GC_002_006_RetTravelInit NPGS2GC_006_RetTravelInit(NPUSUserData _userData)
    {
    	GS2GC_002_006_RetTravelInit proto = new GS2GC_002_006_RetTravelInit();
    	_userData.getTravelComponent().makeCanDealEventsProto(proto.getCanDealEventList());
    	_userData.getTravelComponent().makeConsortsProto(proto.getTravelConsortList());

        return proto;
    }

    public static GS2GC_002_007_RetBagItemList NPGS2GC_007_RetBagItemList(NPUSUserData _userData)
    {
        GS2GC_002_007_RetBagItemList proto = new GS2GC_002_007_RetBagItemList();
        _userData.getBagItemComponent().makeGetBagItemList(proto.getBagItemList());

        return proto;
    }

    public static GS2GC_002_008_RetPlayerBuffList NPGS2GC_008_RetPlayerBuffList(NPUSUserData _userData)
    {
        GS2GC_002_008_RetPlayerBuffList proto = new GS2GC_002_008_RetPlayerBuffList();
        _userData.getBuffComponent().makeProtocol(proto.getPlayerBuffList());

        return proto;
    }
    
    public static GS2GC_002_009_RetAnecdoteInit make_009_RetAnecdoteInit(NPUSUserData _userData)
    {
    	GS2GC_002_009_RetAnecdoteInit proto = new GS2GC_002_009_RetAnecdoteInit();
    	proto.getEventList().addAll(_userData.getAnecdoteComponent().makeProtoList());
        return proto;
    }
    
    public static GS2GC_002_010_RetBuildingInit make_010_RetBuildingInit(NPUSUserData _userData)
    {
    	GS2GC_002_010_RetBuildingInit proto = new GS2GC_002_010_RetBuildingInit();
    	_userData.getBuildingComponent().makeProto(proto.getBuildingIdList(), proto.getFarmList(), proto.getBusinessList());

        //获取农场暴击信息
        SpecialItemDealer_FarmMutiple dealer = _userData.getSpecialItemComponent().getDealer(ESpecialItemType.FARM_MULTIPLE, SpecialItemDealer_FarmMutiple.class);
        if (dealer != null)
            proto.setFarmMultipleInfo(dealer.toProto());

        return proto;
    }

    public static GS2GC_002_011_RetGoldInit make_011_RetGoldInit(NPUSUserData _userData)
    {
        GS2GC_002_011_RetGoldInit proto = new GS2GC_002_011_RetGoldInit();
        //金币结算信息
        SpecialItemDealer_Gold goldDealer = _userData.getSpecialItemComponent().getDealer(ESpecialItemType.GOLD, SpecialItemDealer_Gold.class);
        if (goldDealer != null)
        {
            proto.setGoldInfo(goldDealer.toProto());
            proto.setOfflineProduceCount(goldDealer.getOfflineProduceCount());
            proto.setOfflineProduceTime(goldDealer.getOfflineProduceTime());
        }

        return proto;
    }

    public static GS2GC_002_012_RetHeroInit NPGS2GC_012_RetHeroInit(NPUSUserData _userData)
    {
        GS2GC_002_012_RetHeroInit proto = new GS2GC_002_012_RetHeroInit();
        _userData.getHeroComponent().makeInitProto(proto);
        _userData.getHeroComponent().getSuitMgr().makeInitProto(proto);
        return proto;
    }
    
    public static GS2GC_002_013_RetChildList make_013_RetChildList(NPUSUserData _userData)
    {
    	GS2GC_002_013_RetChildList proto = new GS2GC_002_013_RetChildList();
    	_userData.getChildComponent().getChildMgr().makeChildProto(proto.getChildList());
    	_userData.getChildComponent().getChildMgr().makeSeatProto(proto.getSeatList());
    	_userData.getChildComponent().getAdultMgr().makeUnmarryAdultList(proto.getUnmarriedAdultList());
    	_userData.getChildComponent().getAdultMgr().makeMarriedAdultList(proto.getMarriedAdultIdList());
    	_userData.getChildComponent().getToMeMarryApplyMgr().makeProto(proto.getApplyToMeBaseList());
    	proto.setBonus(_userData.getChildComponent().getBonusSum());
    	proto.setAdultBonus(_userData.getChildComponent().getAdultMgr().getBonusSum());
    	
        return proto;
    }

    public static GS2GC_002_014_RetMailStatInfo GS2GC_014_RetMailStatInfo(NPUSUserData _userData)
    {
        GS2GC_002_014_RetMailStatInfo proto = new GS2GC_002_014_RetMailStatInfo();

        //初始化进行邮件上限数量检查
        _userData.getMailComponent().removeExpiredMails(true, _userData.getPlayerInitContext());
        
        proto.setMailStatInfo(_userData.getMailComponent().makeMailStatInfo());

        return proto;
    }

	public static GS2GC_002_015_RetConsortList make_015_RetConsortList(NPUSUserData _userData)
    {
		GS2GC_002_015_RetConsortList proto = new GS2GC_002_015_RetConsortList();
		_userData.getConsortComponent().makeProto(proto.getConsortList());
		_userData.getConsortComponent().getCGMgr().makeProto(proto.getUnlockedCGList());
		_userData.getConsortComponent().makeRandCallConsortProto(proto.getRandCallConsortIdList());

        return proto;
    }

    public static GS2GC_002_017_RetCollegeSeatList make_017_RetCollegeSeatList(NPUSUserData _userData)
    {
        GS2GC_002_017_RetCollegeSeatList proto = new GS2GC_002_017_RetCollegeSeatList();
//        _userData.getCollegeComponent().fillProto(proto.getSeatList());
        return proto;
    }

    public static GS2GC_002_018_RetActivityList make_018_RetActivityList(NPUSUserData _userData)
    {
        CommonActivityMgr activityMgr = _userData.getUSServer().getCommActivityMgr();

        GS2GC_002_018_RetActivityList proto = new GS2GC_002_018_RetActivityList();
        proto.getActivityList().addAll(activityMgr.makeProto());
        proto.getStepRewardList().addAll(activityMgr.makeStepRewardProto(_userData.getCid()));
        activityMgr.fillPlayerData(_userData.getCid(), proto.getPlayerDataList());
        return proto;
    }

    public static GS2GC_002_019_RetChapterInit make_019_RetChapterInit(NPUSUserData _userData)
    {
        ChapterInfo chapterInfo = _userData.getChapterComponent().getChapterInfo();

        GS2GC_002_019_RetChapterInit proto = new GS2GC_002_019_RetChapterInit();
        proto.setPosInfo(chapterInfo.getPosInfo());
        proto.setInspireInfo(chapterInfo.getInspireInfo());
        _AChapterEvent eventInfo = chapterInfo.getEventInfo();
        if (eventInfo != null)
            proto.setEventInfo(eventInfo.makeInfo());
        proto.getHadDrawPlotRewardList().addAll(_userData.getChapterComponent().getHadDrawPlotRewardList());
        return proto;
    }

    @SuppressWarnings("unchecked")
	public static GS2GC_002_020_RetPlayerTitle NPGS2GC_020_RetPlayerTitle(NPUSUserData _userData)
    {
        GS2GC_002_020_RetPlayerTitle proto = new GS2GC_002_020_RetPlayerTitle();
        //普通称号
        _userData.getTitleComponent().makeProtocol(proto.getInfoList());
        //组合称号
        _userData.getPlayerComboTitleComp().getUnitMgr(ENPPlayerComboTitleType.PRE).makeProto(proto.getComboPreList());
        _userData.getPlayerComboTitleComp().getUnitMgr(ENPPlayerComboTitleType.SFX).makeProto(proto.getComboSfxList());
        _userData.getPlayerComboTitleComp().getUnitMgr(ENPPlayerComboTitleType.BG).makeProto(proto.getComboBgList());
        
        //当前穿戴数据
        proto.setCurInfo(_userData.getPlayerComponent().getCurTitleMgr().getCurTitle());
        proto.setIsShow(_userData.getPlayerComponent().getCurTitleMgr().getShow());
        
        return proto;
    }

    public static GS2GC_002_021_RetPlayerIcon NPGS2GC_021_RetPlayerIcon(NPUSUserData _userData)
    {
        GS2GC_002_021_RetPlayerIcon proto = new GS2GC_002_021_RetPlayerIcon();
        _userData.getIconComponent().makeProtocol(proto.getIconInfoList());

        return proto;
    }

    public static GS2GC_002_022_RetPlayerIconBgk NPGS2GC_022_RetPlayerIconBgk(NPUSUserData _userData)
    {
        GS2GC_002_022_RetPlayerIconBgk proto = new GS2GC_002_022_RetPlayerIconBgk();
        _userData.getIconBgkComponent().makeProtocol(proto.getIconBgkInfoList());

        return proto;
    }

    public static GS2GC_002_023_RetPlayerBubble NPGS2GC_023_RetPlayerBubble(NPUSUserData _userData)
    {
        GS2GC_002_023_RetPlayerBubble proto = new GS2GC_002_023_RetPlayerBubble();
        _userData.getBubbleComponent().makeProtocol(proto.getBubbleInfoList());

        return proto;
    }

    /**
     * 构造房间皮肤初始化响应协议
     * @param _userData 玩家数据
     * @return 响应协议对象
     */
    public static GS2GC_002_087_RetPlayerRoomSkin make_085_RetPlayerRoomSkin(NPUSUserData _userData)
    {
        GS2GC_002_087_RetPlayerRoomSkin proto = new GS2GC_002_087_RetPlayerRoomSkin();
        _userData.getRoomSkinComponent().makeProtocol(proto.getRoomSkinInfoList());
        return proto;
    }

    public static GS2GC_002_024_RetHeroRecommendInit make_024_RetHeroRecommendInit(NPUSUserData _userData)
    {
    	GS2GC_002_024_RetHeroRecommendInit proto = new GS2GC_002_024_RetHeroRecommendInit();
        _userData.getHeroRecommendComponent().makeProto(proto.getInfoList());

        return proto;
    }

    public static GS2GC_002_025_RetArenaInit make_025_RetArenaInit(NPUSUserData _userData)
    {
        //初始化检查重置竞技场数据
        _userData.getArenaComponent().checkResetDailyData(true);

        GS2GC_002_025_RetArenaInit proto = new GS2GC_002_025_RetArenaInit();
        proto.setInfo(_userData.getArenaComponent().makeProto());
        return proto;
    }

    public static GS2GC_002_026_RetDailyCheckInfo make_026_RetDailyCheckInfo(NPUSUserData _userData)
    {
        GS2GC_002_026_RetDailyCheckInfo proto = new GS2GC_002_026_RetDailyCheckInfo();
        proto.setInfo(_userData.getDailyCheckComponent().makeCheckInfo());
        proto.setRewardInfo(_userData.getDailyCheckComponent().makeCheckRewardInfo());
        return proto;
    }

    public static GS2GC_002_027_RetPrivilegeCardInit make_027_RetPrivilegeCardInit(NPUSUserData _userData)
    {
    	GS2GC_002_027_RetPrivilegeCardInit proto = new GS2GC_002_027_RetPrivilegeCardInit();
        _userData.getPrivilegeCardComponent().makeProto(proto.getCardList());
        return proto;
    }

    public static GS2GC_002_028_RetChatEmoteGroupInit make_028_RetChatEmoteGroupInit(NPUSUserData _userData)
    {
        GS2GC_002_028_RetChatEmoteGroupInit proto = new GS2GC_002_028_RetChatEmoteGroupInit();
        _userData.getEmoteGroupComponent().makeProtocol(proto.getEmoteGroupList());
        return proto;
    }

    public static GS2GC_002_029_RetPlayerFuncUnlockInit make_029_RetPlayerFuncUnlockInit(NPUSUserData _userData)
    {
        GS2GC_002_029_RetPlayerFuncUnlockInit proto = new GS2GC_002_029_RetPlayerFuncUnlockInit();
        proto.getHadUnlockTypeList().addAll(_userData.getFuncUnlockComponent().getUnlockTypeList());
        proto.getClientNotifiedTypeList().addAll(_userData.getFuncUnlockComponent().getClientNotifiedTypeList());
        return proto;
    }

    public static GS2GC_002_030_RetCuteActorInit make_030_RetCuteActorInit(NPUSUserData _userData)
    {
        GS2GC_002_030_RetCuteActorInit proto = new GS2GC_002_030_RetCuteActorInit();
        _userData.getCuteActorComponent().makeProtocol(proto.getCuteActorList());
        return proto;
    }

    public static GS2GC_002_031_RetPlayerStationInit make_031_RetPlayerStationInit(NPUSUserData _userData)
    {
        GS2GC_002_031_RetPlayerStationInit proto = new GS2GC_002_031_RetPlayerStationInit();
        SpecialItemDealer_Station stationDealer = _userData.getSpecialItemComponent().getDealer(ESpecialItemType.ARENA_STATION,SpecialItemDealer_Station.class);
        if (stationDealer != null)
            proto.setStationInfo(stationDealer.toProto());
        return proto;
    }

    public static GS2GC_002_032_RetPlayerLazyCDList NPGS2GC_032_RetPlayerLazyCDList(NPUSUserData _userData)
    {
        GS2GC_002_032_RetPlayerLazyCDList proto = new GS2GC_002_032_RetPlayerLazyCDList();
        _userData.getLazyCDComponent().fillProto(proto.getCdList());

        return proto;
    }

    public static GS2GC_002_033_RetMarqueeInit make_033_RetMarqueeInit(NPUSUserData _userData, ArrayList<Common_MarqueeShowPosReadInfo> _marqueeReadList, long _onlineTimeMs)
    {
        GS2GC_002_033_RetMarqueeInit proto = new GS2GC_002_033_RetMarqueeInit();
        proto.getMarqueeList().addAll(_userData.getUSServer().getMarqueeMgr().makeProto(_userData.getSdkInfo().language, _marqueeReadList, _onlineTimeMs));
        return proto;
    }

    public static GS2GC_002_034_RetTargetRewardInit make_034_RetTargetRewardInit(NPUSUserData _userData)
    {
        GS2GC_002_034_RetTargetRewardInit proto = new GS2GC_002_034_RetTargetRewardInit();
        proto.getInfoList().addAll(_userData.getTargetRewardComponent().makeProtoList());
        return proto;
    }

    public static GS2GC_002_035_RetCommonRefreshInit make_035_RetCommonRefreshInit(NPUSUserData _userData)
    {
        GS2GC_002_035_RetCommonRefreshInit proto = new GS2GC_002_035_RetCommonRefreshInit();
        proto.getInfoList().addAll(_userData.getRefreshComponent().makeProtoList());
        return proto;
    }

    public static GS2GC_002_036_RetQuestInit make_036_RetQuestInit(NPUSUserData _userData)
    {
        GS2GC_002_036_RetQuestInit proto = new GS2GC_002_036_RetQuestInit();
        _userData.getQuestComponent().makeProto(proto.getQuestList());

        return proto;
    }

    public static GS2GC_002_037_RetWeekCardInit make_037_RetWeekCardInit(NPUSUserData _userData)
    {
        GS2GC_002_037_RetWeekCardInit proto = new GS2GC_002_037_RetWeekCardInit();
        proto.setInfo(_userData.getWeekCardComponent().makeInfoProto());
        proto.getSettingList().addAll(_userData.getWeekCardComponent().makeAllSettingProto());
        return proto;
    }

    public static GS2GC_002_038_RetRecordInit make_038_RetRecordInit(NPUSUserData _userData)
    {
        GS2GC_002_038_RetRecordInit proto = new GS2GC_002_038_RetRecordInit();
        _userData.getRecordComponent().makeProto(proto.getRecordList());

        return proto;
    }

    public static GS2GC_002_039_RetQuestCountInit make_039_RetQuestCountInit()
    {
        return new GS2GC_002_039_RetQuestCountInit();
    }

    public static GS2GC_002_041_RetRecuritInfo make_041_RetRecuritInfo(NPUSUserData _userData)
    {
        GS2GC_002_041_RetRecuritInfo proto = new GS2GC_002_041_RetRecuritInfo();
        proto.getHadRecuritIdList().addAll(_userData.getRecruitComponent().getRecruitList());
        return proto;
    }

    public static GS2GC_002_042_RetQuestionnaireInit make_042_RetQuestionnaireInit(NPUSUserData _userData)
    {
        GS2GC_002_042_RetQuestionnaireInit proto = new GS2GC_002_042_RetQuestionnaireInit();
        _userData.getUSServer().getQuestionnaireMgr().fillProto(_userData.getCid(), proto.getInfoList(), proto.getRewardList());
        return proto;
    }

    public static GS2GC_002_043_RetEventRecordInit make_043_RetEventRecordInit(NPUSUserData _userData)
    {
        GS2GC_002_043_RetEventRecordInit proto = new GS2GC_002_043_RetEventRecordInit();
        _userData.getEventRecordComp().makeProto(proto.getRecordList());

        return proto;
    }

    public static GS2GC_002_044_RetTowerInit make_044_RetTowerInit(NPUSUserData _userData)
    {
    	GS2GC_002_044_RetTowerInit proto = new GS2GC_002_044_RetTowerInit();
    	proto.setPosInfo(_userData.getTowerComponent().toProto());
    	proto.setHadActiveResearchPos(_userData.getTowerComponent().makeResearchPosInfo());
    	proto.setHighestPosHadReach(_userData.getTowerComponent().makeHighestPosInfo());
    	proto.setBuildingProfitAddPer(_userData.getTowerComponent().getBuildingProfitAddPer());
        _userData.getTowerComponent().fillHadDrawResearchRewardList(proto.getHadDrawResearchRewardList());
        return proto;
    }

    public static GS2GC_002_045_RetMiddayDungeonInit make_045_RetMiddayDungeonInit(NPUSUserData _userData)
    {
        GS2GC_002_045_RetMiddayDungeonInit proto = new GS2GC_002_045_RetMiddayDungeonInit();
    	proto.setInfo(_userData.getMiddayDungeonComponent().getDungeonInfo().makeInfo());
    	proto.setTimeInfo(_userData.getUSServer().getMiddayDungeonMgr().makeTimeInfo());
        return proto;
    }
    
    public static GS2GC_002_046_RetDailyQuest make_046_RetDailyQuest(NPUSUserData _userData)
    {
        GS2GC_002_046_RetDailyQuest proto = new GS2GC_002_046_RetDailyQuest();
        _userData.getDailyQuestComponent().makeProto(proto.getDailyquestInfoList());

        return proto;
    }

    public static GS2GC_002_047_RetGachaInit make_047_RetGachaInit(NPUSUserData _userData)
    {
        GS2GC_002_047_RetGachaInit proto = new GS2GC_002_047_RetGachaInit();
        _userData.getGachaComponent().fillProto(proto.getPoolList());

        return proto;
    }

    public static GS2GC_002_048_RetFixedCdInfo make_048_ReqFixedCdInfo(NPUSUserData _userData)
    {
        GS2GC_002_048_RetFixedCdInfo proto = new GS2GC_002_048_RetFixedCdInfo();
        _userData.getFixedCdComponent().makeProto(proto.getCdList());

        return proto;
    }

    public static GS2GC_002_049_RetFriendInit make_049_RetFriendInit(NPUSUserData _userData)
    {
        GS2GC_002_049_RetFriendInit proto = new GS2GC_002_049_RetFriendInit();
        _userData.getFriendComponent().getFriendApplyMgr().makeProto(proto.getApplyList());
        _userData.getFriendComponent().getFriendMgr().makeProto(proto.getFriendList());
        _userData.getFriendComponent().getFriendGroupMgr().makeProto(proto.getGroupList(), proto.getGroupOrderList());
        return proto;
    }

    public static GS2GC_002_050_RetAchieveInit make_050_RetAchieveInit(NPUSUserData _userData)
    {
        GS2GC_002_050_RetAchieveInit proto = new GS2GC_002_050_RetAchieveInit();
        _userData.getAchieveComponent().makeProto(proto.getAchieveList(), proto.getAchievePointList());
        return proto;
    }

    public static GS2GC_002_051_RetShopInit make_051_RetShopInit(NPUSUserData _userData)
    {
        GS2GC_002_051_RetShopInit proto = new GS2GC_002_051_RetShopInit();
        _userData.getShopComponent().makeProto(proto.getShopList());

        return proto;
    }

    public static GS2GC_002_052_RetEveningDungeonInit make_052_RetEveningDungeonInit(NPUSUserData _userData)
    {
        GS2GC_002_052_RetEveningDungeonInit proto = new GS2GC_002_052_RetEveningDungeonInit();
        proto.setInfo(_userData.getEveningDungeonComponent().getDungeonInfo().makeInfo());
        proto.setTimeInfo(_userData.getUSServer().getEveningDungeonMgr().getInfo().makeTimeInfo());
        return proto;
    }

    public static GS2GC_002_053_RetLoginCountInit make_053_RetLoginCountInit(NPUSUserData _userData)
    {
        GS2GC_002_053_RetLoginCountInit proto = new GS2GC_002_053_RetLoginCountInit();
        proto.getHadDrawRewardDays().addAll(_userData.getSevenLoginComponent().getHadDrawDayList());
        return proto;
    }

    public static GS2GC_002_054_RetOfflineRewardInit make_054_RetOfflineRewardInit(NPUSUserData _userData)
    {
        GS2GC_002_054_RetOfflineRewardInit proto = new GS2GC_002_054_RetOfflineRewardInit();
        _userData.getOfflineRewardComponent().makeProtoList(proto.getRewardList());

        return proto;
    }

    public static GS2GC_002_055_RetConsortChatInit make_055_RetConsortChatInit(NPUSUserData _userData)
    {
        GS2GC_002_055_RetConsortChatInit proto = new GS2GC_002_055_RetConsortChatInit();
        proto.getChatList().addAll(_userData.getConsortChatComponent().makeProto());
        return proto;
    }

    public static GS2GC_002_056_RetCountdownEventInit make_056_RetCountdownEventInit(NPUSUserData _userData)
    {
        GS2GC_002_056_RetCountdownEventInit proto = new GS2GC_002_056_RetCountdownEventInit();
        _userData.getCountdownEventComponent().fillProto(proto);
        return proto;
    }

    public static GS2GC_002_057_RetInnInit make_057_RetInnInit(NPUSUserData _userData)
    {
        GS2GC_002_057_RetInnInit proto = new GS2GC_002_057_RetInnInit();
        proto.setInnInfo(_userData.getInnComponent().makeProto());
        return proto;
    }

    public static GS2GC_002_058_RetMuseumInit make_058_RetMuseumInit(NPUSUserData _userData)
    {
        GS2GC_002_058_RetMuseumInit proto = new GS2GC_002_058_RetMuseumInit();
        _userData.getMuseumComponent().getItemMgr().fillItemList(proto.getItemList());
        return proto;
    }

    public static GS2GC_002_059_RetPlayerPermissionsInit make_059_RetPlayerPermissionsInit(NPUSUserData _userData)
    {
    	GS2GC_002_059_RetPlayerPermissionsInit proto = new GS2GC_002_059_RetPlayerPermissionsInit();
        _userData.getPlayerPermissionsComponent().makeProto(proto.getEffectIdList());
        return proto;
    }

    public static GS2GC_002_060_RetDinnerInit make_060_RetDinnerInit(NPUSUserData _userData)
    {
    	GS2GC_002_060_RetDinnerInit proto = new GS2GC_002_060_RetDinnerInit();
    	
    	DinnerInfo dinner = _userData.getUSServer().getDinnerPool().lookupByOwnerCid(_userData.getCid());
    	if(null != dinner)
    	{
    		proto.setInstanceId(dinner.getInstanceId());
    	}
    	
    	proto.setHasOwnerReward(_userData.getDinnerComponent().hasOwnerReward());
    	_userData.getDinnerComponent().makePermitProto(proto.getPermitList());
    	
        return proto;
    }
    
    public static GS2GC_002_061_RetShieldInit make_061_RetShieldInit(NPUSUserData _userData)
    {
    	GS2GC_002_061_RetShieldInit proto = new GS2GC_002_061_RetShieldInit();
    	
    	ShieldCidInfo shield = _userData.getUSServer().getShieldCidMgr().lookup(_userData.getCid());
    	if(null != shield)
    	{
    		shield.makeProto(proto.getShieldCidList());
    	}
    	
        return proto;
    }
    
    public static GS2GC_002_062_RetStageGoalInit make_062_RetStageGoalInit(NPUSUserData _userData)
    {
    	GS2GC_002_062_RetStageGoalInit proto = new GS2GC_002_062_RetStageGoalInit();
    	_userData.getStageGoalComponent().makeProto(proto.getCurStageGoal());
        proto.getHadDrawBigStepList().addAll(_userData.getStageGoalComponent().getHadDrawBigStepList());
        proto.getHadDrawBigStepFirstReachList().addAll(_userData.getStageGoalComponent().getHadDrawBigStepFirstReachList());
        proto.getCanDrawBigStepFirstReachList().addAll(_userData.getUSServer().getStageGoalFirstReachMgr().getCanDrawBigStepFirstReachList());
        return proto;
    }

    public static GS2GC_002_063_RetGuildInit make_063_RetGuildInit_OnlyUS(NPUSUserData _userData)
    {
        GS2GC_002_063_RetGuildInit proto = new GS2GC_002_063_RetGuildInit();
        proto.getSelfRequestJoinGuildList().addAll(
                _userData.getUSServer().getGuildMgr().getJoinRequestMgr().makePlayerRequestGuildList(_userData.getCid()));

        proto.setJoinCdInfo(_userData.getGuildComponent().makeJoinCdInfo());
        proto.setDailyData(_userData.getGuildComponent().makeDailyInfo());
        return proto;
    }

    public static GS2GC_002_064_RetActivityCurrencyList make_064_RetActivityCurrencyList(NPUSUserData _userData)
    {
        GS2GC_002_064_RetActivityCurrencyList proto = new GS2GC_002_064_RetActivityCurrencyList();
        _userData.getActivityCurrencyComponent().makeProtocol(proto.getCurrencyList());

        return proto;
    }

    public static GS2GC_002_065_RetSystemQuestInit make_065_RetSystemQuestInit(NPUSUserData _userData)
    {
        GS2GC_002_065_RetSystemQuestInit proto = new GS2GC_002_065_RetSystemQuestInit();
        _userData.getSystemQuestComponent().makeProtocol(proto.getInfoList());
        return proto;
    }

    public static GS2GC_002_066_RetTreasureHuntInit make_066_RetTreasureHuntInit(NPUSUserData _userData)
    {
        GS2GC_002_066_RetTreasureHuntInit proto = new GS2GC_002_066_RetTreasureHuntInit();
        proto.setInfo(_userData.getTreasureHuntComponent().makeProto());
        return proto;
    }

    public static GS2GC_002_067_RetGuildMarsHelpInit make_067_RetGuildMarsHelpInit(long _cid, GuildInfo _guildInfo)
    {
    	GS2GC_002_067_RetGuildMarsHelpInit proto = new GS2GC_002_067_RetGuildMarsHelpInit();

    	if(null != _guildInfo)
    	{
            _guildInfo.getMarsHelpMgr().makePlayerCanDealMarsHelpIdProto(_cid, proto.getCanDealIdList());
    	}
    	
        return proto;
    }

    public static GS2GC_002_068_RetGraveInit make_068_RetGraveInit(NPUSUserData _userData)
    {
    	GS2GC_002_068_RetGraveInit proto = new GS2GC_002_068_RetGraveInit();
    	proto.setHasGraveNewReward(_userData.getUSServer().getGraveMgr().getGraveNewMgr().hasGraveNewReward(_userData.getCid()));
    	proto.setHasGraveRecord(_userData.getUSServer().getGraveMgr().getGraveRecordMgr().hasRecords());
    	
        return proto;
    }

    public static GS2GC_002_069_RetForeverAddInit make_069_RetForeverAddInit(NPUSUserData _userData)
    {
    	GS2GC_002_069_RetForeverAddInit proto = new GS2GC_002_069_RetForeverAddInit();
    	_userData.getForeverAddComponent().makeProto(proto.getAddList());
    	
        return proto;
    }

    /**
     * 礼包初始化协议构造方法
     * @param giftPackInfoList 礼包信息列表
     * @return 礼包初始化返回协议
     */
    public static GS2GC_002_070_RetGiftPackInit make_070_RetGiftPackInit(ArrayList<GiftPack_Info> giftPackInfoList)
    {
        GS2GC_002_070_RetGiftPackInit proto = new GS2GC_002_070_RetGiftPackInit();
        if (giftPackInfoList != null)
        {
            proto.getInfoList().addAll(giftPackInfoList);
        }
        return proto;
    }

    /**
     * 礼包初始化协议构造方法（从用户数据生成）
     * @param _userData 用户数据
     * @return 礼包初始化返回协议
     */
    public static GS2GC_002_070_RetGiftPackInit make_070_RetGiftPackInit(NPUSUserData _userData)
    {
        GS2GC_002_070_RetGiftPackInit proto = new GS2GC_002_070_RetGiftPackInit();
        _userData.getOrderComponent().getMgr().makeProtoList(proto.getInfoList());
        return proto;
    }

    public static GS2GC_002_071_RetGuildDungeonInit make_071_RetGuildDungeonInit_noGuild(NPUSUserData _userData)
    {
        GS2GC_002_071_RetGuildDungeonInit proto = new GS2GC_002_071_RetGuildDungeonInit();
        //玩家数据
        _userData.getGuildDungeonComponent().makeFightHeroList(proto.getFightHeroList());

        return proto;
    }
    public static GS2GC_002_071_RetGuildDungeonInit make_071_RetGuildDungeonInit_onlyGuild(long _cid, GuildInfo _guildInfo)
    {
    	GS2GC_002_071_RetGuildDungeonInit proto = new GS2GC_002_071_RetGuildDungeonInit();
    	//公会副本相关
    	if(null != _guildInfo)
    	{
    		//设置数据列表
            _guildInfo.getDungeonMgr().getSetMgr().makeUnlcokProto(proto.getSetList());
    		//实例数据列表
            _guildInfo.getDungeonMgr().getInstanceMgr().makeProto(proto.getInstanceList());
    		//已领取奖励的怪物数据列表
            _guildInfo.getDungeonMgr().getInstanceMgr().makeGainedRewardList(_cid, proto.getGainedRewardMonsterIdList());
    	}
    	//这里不处理玩家数据，是在公会服务器处理的消息
    	//_userData.getGuildDungeonComponent().makeFightHeroList(proto.getFightHeroList());
    	
        return proto;
    }
    
    public static GS2GC_002_072_RetMarsGoRouteInit make_072_RetMarsGoRouteInit(NPUSUserData _userData)
    {
    	GS2GC_002_072_RetMarsGoRouteInit proto = new GS2GC_002_072_RetMarsGoRouteInit();
    	proto.setIsAllDone(_userData.getMarsGoRouteComponent().isAllDone());
    	proto.setStage(_userData.getMarsGoRouteComponent().getStage());
    	proto.setStartMs(_userData.getMarsGoRouteComponent().getArrivedMs());
    	proto.setStageStartMs(_userData.getMarsGoRouteComponent().getStageStartMs());
        
        return proto;
    }
    
    public static GS2GC_002_073_RetMarsBuildingInit make_073_RetMarsBuildingInit(NPUSUserData _userData)
    {
    	GS2GC_002_073_RetMarsBuildingInit proto = new GS2GC_002_073_RetMarsBuildingInit();
        _userData.getMarsBuildingComponent().makeBuildingProto(proto.getBuildingList());
    	proto.setHomeBuilding(_userData.getMarsBuildingComponent().getHomeFunc().toHomeBuildingProto());
        _userData.getMarsBuildingComponent().getPeopleBuildingFuncMgr().makePeopleBuildingProto(proto.getPeopleBuildingList());
        _userData.getMarsBuildingComponent().makeBuildingEquipmentProto(proto.getEquipmentList());
        _userData.getMarsBuildingComponent().getPeopleBuildingFuncMgr().makeBuildingEnergyOutputProto(proto.getEnergyOutputList());
        _userData.getMarsBuildingComponent().makeFoodProto(proto.getFoodEquipmentList());
        _userData.getMarsBuildingComponent().getBuildingUpQueueMgr().makeProto(proto.getBuildingUpQueueList());
        proto.setExtMoodIndex(_userData.getMarsBuildingComponent().getCompInfo().getExtMoodIndex());
        
        return proto;
    }
    
    public static GS2GC_002_074_RetMarsPeopleInit make_074_RetMarsPeopleInit(NPUSUserData _userData)
    {
    	GS2GC_002_074_RetMarsPeopleInit proto = new GS2GC_002_074_RetMarsPeopleInit();
    	proto.setPeopleNum(_userData.getMarsPeopleComponent().getNumInfo().toProto());
    	_userData.getMarsPeopleComponent().getIntelligentMgr().makeProto(proto.getIntelligentList());
    	proto.setSatisfaction(_userData.getMarsPeopleComponent().getSatisfaction());
    	_userData.getMarsPeopleComponent().getLetterMgr().makeProto(proto.getLetterList());
    	_userData.getMarsPeopleComponent().getHelpMgr().makeProto(proto.getHelpList());
    	proto.setImmigrant(_userData.getMarsPeopleComponent().getImmigrantInfo().toImmigrantProto());
    	proto.setImmigrantCount(_userData.getMarsPeopleComponent().getImmigrantInfo().toImmigrantCountProto());
        
        return proto;
    }

    public static GS2GC_002_075_RetGuildCooperateInit make_075_RetGuildCooperateInit(NPUSUserData _userData, GuildCooperate_Info _guildCooperateInfo)
    {
        GS2GC_002_075_RetGuildCooperateInit proto = new GS2GC_002_075_RetGuildCooperateInit();
        _userData.getGuildCooperateComponent().fillProto(proto);

        proto.setInfo(_guildCooperateInfo);
        return proto;
    }

    public static GS2GC_002_076_RetMarsEnergyInit make_076_RetMarsEnergyInit(NPUSUserData _userData)
    {
    	GS2GC_002_076_RetMarsEnergyInit proto = new GS2GC_002_076_RetMarsEnergyInit();
        SpecialItemDealer_MarsEnergy dealer = _userData.getSpecialItemComponent().getDealer(ESpecialItemType.MARS_ENERGY, SpecialItemDealer_MarsEnergy.class);
        if (dealer != null)
            proto.setInfo(dealer.toProto());
    	
        return proto;
    }

    public static GS2GC_002_077_RetMarsExploreInit make_077_RetMarsExploreInit(NPUSUserData _userData)
    {
    	GS2GC_002_077_RetMarsExploreInit proto = new GS2GC_002_077_RetMarsExploreInit();
    	proto.setExplore(_userData.getMarsExploreComponent().getExploreInfo().toProto());
    	_userData.getMarsExploreComponent().getEventMgr().makeProto(proto.getEventList());
    	_userData.getMarsExploreComponent().getTeamMgr().makeProto(proto.getTeamList());
    	_userData.getMarsMineComponent().makeProto(proto.getMineIdx());
    	
        return proto;
    }

    public static GS2GC_002_078_RetMarsTechInit make_078_RetMarsTechInit(NPUSUserData _userData)
    {
    	GS2GC_002_078_RetMarsTechInit proto = new GS2GC_002_078_RetMarsTechInit();
        _userData.getMarsTechComponent().makeProto(proto.getTechnologyList());
    	
        return proto;
    }

    public static GS2GC_002_079_RetActivityHotRefList make_079_RetActivityHotRefList(List<Activity_HotRefInfo> _hotRefList)
    {
        GS2GC_002_079_RetActivityHotRefList proto = new GS2GC_002_079_RetActivityHotRefList();
        proto.getHotRefList().addAll(_hotRefList);
        return proto;
    }

    /**
     * 构造初始化响应协议
     *
     * @param _redDotList 当前激活的红点列表
     * @return 初始化响应协议对象
     */
    public static GS2GC_002_080_RetRedDotInit make_080_RetRedDotInit(List<Common_RedDotInfo> _redDotList) {
        GS2GC_002_080_RetRedDotInit proto = new GS2GC_002_080_RetRedDotInit();
        if (_redDotList != null && !_redDotList.isEmpty()) {
            proto.getRedDotList().addAll(_redDotList);
        }
        return proto;
    }

    public static GS2GC_002_255_OnQuestCountInit make_255_OnQuestCountInit(List<Quest_Count> _countList)
    {
        GS2GC_002_255_OnQuestCountInit proto = new GS2GC_002_255_OnQuestCountInit();
        proto.getQuestCountList().addAll(_countList);
        return proto;
    }

    /**
     * 推送礼包列表初始化协议构造方法（081协议）
     *
     * @param _userData 玩家数据
     * @return 推送礼包列表返回协议
     */
    public static GS2GC_002_081_RetPushGiftPackList make_081_RetPushGiftPackList(NPUSUserData _userData)
    {
        GS2GC_002_081_RetPushGiftPackList proto = new GS2GC_002_081_RetPushGiftPackList();
        proto.getGroupList().addAll(_userData.getPushGiftPackComponent().makeGroupProtoList());
        return proto;
    }

    public static GS2GC_002_082_RetGuildBoxInit make_082_RetGuildBoxInit_onlyGuild(long _cid, GuildInfo _guildInfo)
    {
        GS2GC_002_082_RetGuildBoxInit proto = new GS2GC_002_082_RetGuildBoxInit();

        if(null != _guildInfo)
        {
            proto.setActivePoint(_guildInfo.getActivePoint());
            proto.setTargetLvl(_guildInfo.getActivePointTargetLvl());

            GuildMemberInfo member = _guildInfo.getMemberMgr().lookup(_cid);
            if(null != member)
            {
                proto.setIsGuildBoxShareAnonymous(member.isGuildBoxShareAnonymous());
            }
        }

        return proto;
    }

    /**
     * 活动基金初始化协议构造方法
     *
     * @param _userData 玩家数据
     * @return 活动基金初始化返回协议
     */
    public static GS2GC_002_083_RetActivityFundInit make_083_RetActivityFundInit(NPUSUserData _userData)
    {
        GS2GC_002_083_RetActivityFundInit proto = new GS2GC_002_083_RetActivityFundInit();
        proto.getFundList().addAll(_userData.getActivityFundComponent().makeProtoList());
        return proto;
    }

    /**
     * 构造冲榜礼包初始化响应
     *
     * @param _giftPackInfo 礼包信息
     * @return 冲榜礼包初始化响应协议
     */
    public static GS2GC_002_084_RetRankGiftPackInit make_084_RetRankGiftPackInit(RankGiftPack_Info _giftPackInfo)
    {
        GS2GC_002_084_RetRankGiftPackInit proto = new GS2GC_002_084_RetRankGiftPackInit();
        if (_giftPackInfo != null)
            proto.setGiftPackInfo(_giftPackInfo);
        return proto;
    }

    /**
     * 构造急速兑换初始化响应
     *
     * @param _info 急速兑换信息
     * @return 急速兑换初始化响应协议
     */
    public static GS2GC_002_085_RetRushExchangeInit make_085_RetRushExchangeInit(Common.RushExchangeObj.RushExchange_Info _info)
    {
        GS2GC_002_085_RetRushExchangeInit proto = new GS2GC_002_085_RetRushExchangeInit();
        if (_info != null)
            proto.setInfo(_info);
        return proto;
    }

    /**
     * 构造情人收集初始化回包
     */
    public static GS2GC_002_088_RetLoverCollectInit make_088_RetLoverCollectInit(long _targetLoverId, boolean _isClaimed)
    {
        GS2GC_002_088_RetLoverCollectInit proto = new GS2GC_002_088_RetLoverCollectInit();
        proto.setTargetLoverId(_targetLoverId);
        proto.setIsClaimed(_isClaimed);
        return proto;
    }

    public static GS2GC_002_089_RetForbidChatInit make_089_RetForbidChatInit(NPUSUserData _userData)
    {
        GS2GC_002_089_RetForbidChatInit proto = new GS2GC_002_089_RetForbidChatInit();
        _userData.getForbidChatComponent().makeProto(proto.getForbidChatList());

        return proto;
    }
}
