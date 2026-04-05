package NPUSServer.NPUserMsgDispather.Write;

import ALBasicProtocolPack._IALProtocolStructure;
import Common.AchieveObj.Achieve_AchievePointInfo;
import Common.AchieveObj.Achieve_Info;
import Common.AnecdoteObj.Anecdote_EventInfo;
import Common.CommonFuncObj.CommonFunc_Refresh;
import Common.FriendObj.Friend_ApplyInfo;
import Common.FriendObj.Friend_Info;
import Common.NpPlayerInfoObj.PlayerInfo_ChatEmoteGroup;
import Common.NpPlayerInfoObj.PlayerInfo_CuteActor;
import GS2GC.p021_PlayerInfo.*;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.PlayerInfo_IconShow;
import NPEnum.ENPFunctionType;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.AnecdoteComp._AAnecdoteEvent;
import NPUSServer.NPUSUserMgr.UserComp.BubbleComp.PlayerBubbleInfo;
import NPUSServer.NPUSUserMgr.UserComp.ForeverAddComp.ForeverAddInfo;
import NPUSServer.NPUSUserMgr.UserComp.HeroRecommend.HeroRecommendInfo;
import NPUSServer.NPUSUserMgr.UserComp.IconBgkComp.PlayerIconBgkInfo;
import NPUSServer.NPUSUserMgr.UserComp.IconComp.PlayerIconInfo;
import NPUSServer.NPUSUserMgr.UserComp.PlayerLazyCDComp.PlayerLazyCD;
import NPUSServer.NPUSUserMgr.UserComp.PrivilegeCardComp.PrivilegeCardInfo;
import NPUSServer.NPUSUserMgr.UserComp.RoomSkinComp.PlayerRoomSkinInfo;

import java.util.ArrayList;
import java.util.Collection;
import java.util.List;

public class US2GCWriter_021_PlayerInfo
{
    public static GS2GC_021_004_RetGainPrivilegeCardDailyReward make_004_RetGainPrivilegeCardDailyReward(NPPlayerContext context)
    {
    	GS2GC_021_004_RetGainPrivilegeCardDailyReward proto = new GS2GC_021_004_RetGainPrivilegeCardDailyReward();
        context.getCollector().fillProtoList(proto.getItemList());
        return proto;
    }
    
    /********
     * Icon部分协议
     * @param _info
     * @return
     */
    public static GS2GC_021_006_PlayerIconAdd make_006_PlayerIconAdd(PlayerIconInfo _info)
    {
        GS2GC_021_006_PlayerIconAdd proto = new GS2GC_021_006_PlayerIconAdd();
        proto.setId(_info.getRef().id);
        proto.setExpireTimeTagS(_info.getExpireTimeSec());
        proto.setViewed(_info.getViewed());
        return proto;
    }

    public static GS2GC_021_007_PlayerIconChg make_007_PlayerIconChg(PlayerIconInfo _info)
    {
        GS2GC_021_007_PlayerIconChg proto = new GS2GC_021_007_PlayerIconChg();
        proto.setId(_info.getRef().id);
        proto.setExpireTimeTagS(_info.getExpireTimeSec());
        return proto;
    }

    public static GS2GC_021_008_PlayerIconDel make_008_PlayerIconDel(long _iconId)
    {
        GS2GC_021_008_PlayerIconDel proto = new GS2GC_021_008_PlayerIconDel();
        proto.setId(_iconId);

        return proto;
    }


    /********
     * IconBgk部分协议
     * @param _info
     * @return
     */
    public static GS2GC_021_011_PlayerIconBgkAdd make_011_PlayerIconBgkAdd(PlayerIconBgkInfo _info)
    {
        GS2GC_021_011_PlayerIconBgkAdd proto = new GS2GC_021_011_PlayerIconBgkAdd();
        proto.setId(_info.getRef().id);
        proto.setExpireTimeTagS(_info.getExpireTimeSec());
        proto.setViewed(_info.getViewed());
        return proto;
    }

    public static GS2GC_021_012_PlayerIconBgkChg make_012_PlayerIconBgkChg(PlayerIconBgkInfo _info)
    {
        GS2GC_021_012_PlayerIconBgkChg proto = new GS2GC_021_012_PlayerIconBgkChg();
        proto.setId(_info.getRef().id);
        proto.setExpireTimeTagS(_info.getExpireTimeSec());
        return proto;
    }

    public static GS2GC_021_014_PlayerIconBgkDel make_014_PlayerIconBgkDel(long _iconBgkId)
    {
        GS2GC_021_014_PlayerIconBgkDel proto = new GS2GC_021_014_PlayerIconBgkDel();
        proto.setId(_iconBgkId);

        return proto;
    }


    /********
     * Bubble部分协议
     * @param _info
     * @return
     */
    public static GS2GC_021_016_PlayerBubbleAdd make_016_PlayerBubbleAdd(PlayerBubbleInfo _info)
    {
        GS2GC_021_016_PlayerBubbleAdd proto = new GS2GC_021_016_PlayerBubbleAdd();
        proto.setId(_info.getRefId());
        proto.setExpireTimeTagS(_info.getExpireTimeSec());
        proto.setViewed(_info.getViewed());

        return proto;
    }

    public static GS2GC_021_017_PlayerBubbleChg make_017_PlayerBubbleChg(PlayerBubbleInfo _info)
    {
        GS2GC_021_017_PlayerBubbleChg proto = new GS2GC_021_017_PlayerBubbleChg();
        proto.setId(_info.getRefId());
        proto.setExpireTimeTagS(_info.getExpireTimeSec());

        return proto;
    }

    public static GS2GC_021_018_PlayerBubbleDel make_018_PlayerBubbleDel(long _bubbleId)
    {
        GS2GC_021_018_PlayerBubbleDel proto = new GS2GC_021_018_PlayerBubbleDel();
        proto.setId(_bubbleId);

        return proto;
    }

    public static GS2GC_021_020_RetDealAnecdoteRewardEvent make_020_RetDealAnecdoteRewardEvent(NPPlayerContext _context)
    {
        GS2GC_021_020_RetDealAnecdoteRewardEvent proto = new GS2GC_021_020_RetDealAnecdoteRewardEvent();
        _context.getCollector().fillProtoList(proto.getItemList());
        return proto;
    }

    public static GS2GC_021_021_RetDrawAnecdoteEarningsProcessReward make_021_RetDrawAnecdoteEarningsProcessReward(NPPlayerContext _context)
    {
        GS2GC_021_021_RetDrawAnecdoteEarningsProcessReward proto = new GS2GC_021_021_RetDrawAnecdoteEarningsProcessReward();
        _context.getCollector().fillProtoList(proto.getItemList());
        return proto;
    }

    public static GS2GC_021_022_RetDrawAnecdoteEarningsFinalReward make_022_RetDrawAnecdoteEarningsFinalReward(NPPlayerContext _context)
    {
        GS2GC_021_022_RetDrawAnecdoteEarningsFinalReward proto = new GS2GC_021_022_RetDrawAnecdoteEarningsFinalReward();
        _context.getCollector().fillProtoList(proto.getItemList());
        return proto;
    }

    public static GS2GC_021_024_RetDoneFuncUnlock make_024_RetDoneFuncUnlock(NPPlayerContext context)
    {
        GS2GC_021_024_RetDoneFuncUnlock proto = new GS2GC_021_024_RetDoneFuncUnlock();
        context.getCollector().fillProtoList(proto.getItemList());
        return proto;
    }

    public static GS2GC_021_025_RetDoneAchieveStep make_025_RetDoneAchieveStep()
    {
        return new GS2GC_021_025_RetDoneAchieveStep();
    }

    public static GS2GC_021_026_RetGainAchievePointReward make_026_RetGainAchievePointReward()
    {
        return new GS2GC_021_026_RetGainAchievePointReward();
    }

    public static GS2GC_021_027_RetDailyCheckRefresh make_027_RetDailyCheckRefresh()
    {
        return new GS2GC_021_027_RetDailyCheckRefresh();
    }

    public static GS2GC_021_028_RetDailyCheck make_028_RetDailyCheck()
    {
        return new GS2GC_021_028_RetDailyCheck();
    }

    public static GS2GC_021_029_RetDailyCheckDrawReward make_029_RetDailyCheckDrawReward(NPPlayerContext _context)
    {
    	GS2GC_021_029_RetDailyCheckDrawReward proto = new GS2GC_021_029_RetDailyCheckDrawReward();
    	
    	for(int i = 0; i < _context.getCollector().getAllItemList().size(); i++)
    	{
    		NPCommonCostItem item = _context.getCollector().getAllItemList().get(i);
    		if(null == item)
    			continue;
    		
    		proto.addItemList(item.toProto());
    	}
    	
        return proto;
    }

    public static GS2GC_021_030_RetSendFriendApply make_030_RetSendFriendApply()
    {
        GS2GC_021_030_RetSendFriendApply proto = new GS2GC_021_030_RetSendFriendApply();

        return proto;
    }

    public static GS2GC_021_031_RetDealFriendApply make_031_RetDealFriendApply()
    {
        GS2GC_021_031_RetDealFriendApply proto = new GS2GC_021_031_RetDealFriendApply();

        return proto;
    }

    public static GS2GC_021_032_RetRemoveFriend make_032_RetRemoveFriend()
    {
        GS2GC_021_032_RetRemoveFriend proto = new GS2GC_021_032_RetRemoveFriend();

        return proto;
    }
    
    public static GS2GC_021_033_RetFriendApplyExpired make_033_RetFriendApplyExpired()
    {
    	GS2GC_021_033_RetFriendApplyExpired proto = new GS2GC_021_033_RetFriendApplyExpired();

        return proto;
    }

    public static GS2GC_021_034_RetFriendRecommend make_034_RetFriendRecommend(Collection<PlayerInfo_IconShow> _playerList)
    {
        GS2GC_021_034_RetFriendRecommend proto = new GS2GC_021_034_RetFriendRecommend();
        proto.getPlayerList().addAll(_playerList);
        return proto;
    }

    public static GS2GC_021_035_RetCreateFriendGroup make_035_RetCreateFriendGroup()
    {
        return new GS2GC_021_035_RetCreateFriendGroup();
    }

    public static GS2GC_021_036_RetChgBelongFriendGroup make_036_RetChgBelongFriendGroup()
    {
        return new GS2GC_021_036_RetChgBelongFriendGroup();
    }

    public static GS2GC_021_037_RetDeleteFriendGroup make_037_RetDeleteFriendGroup()
    {
        return new GS2GC_021_037_RetDeleteFriendGroup();
    }

    public static GS2GC_021_038_RetChgFriendGroupOrderList make_038_RetChgFriendGroupOrderList()
    {
        return new GS2GC_021_038_RetChgFriendGroupOrderList();
    }

    public static GS2GC_021_040_RetDealAnecdoteChoiceEvent make_040_RetDealAnecdoteChoiceEvent(NPPlayerContext _context)
    {
        GS2GC_021_040_RetDealAnecdoteChoiceEvent proto = new GS2GC_021_040_RetDealAnecdoteChoiceEvent();
    	_context.getCollector().fillProtoList(proto.getItemList());
        return proto;
    }
    
    public static GS2GC_021_041_RetDealCommonRefresh make_041_RetDealCommonRefresh()
    {
        return new GS2GC_021_041_RetDealCommonRefresh();
    }
    
    public static GS2GC_021_042_RetDealHeroRecommend make_042_RetDealHeroRecommend(long _heroId)
    {
    	GS2GC_021_042_RetDealHeroRecommend proto = new GS2GC_021_042_RetDealHeroRecommend();
    	proto.setHeroId(_heroId);
    	return proto;
    }

    public static GS2GC_021_044_RetDrawFirstRechargeReward make_044_RetDrawFirstRechargeReward(NPPlayerContext context)
    {
    	GS2GC_021_044_RetDrawFirstRechargeReward proto = new GS2GC_021_044_RetDrawFirstRechargeReward();
    	context.getCollector().fillProtoList(proto.getItemList());
    	return proto;
    }

    public static GS2GC_021_046_RetClientNotifyFuncUnlock make_046_RetClientNotifyFuncUnlock()
    {
    	return new GS2GC_021_046_RetClientNotifyFuncUnlock();
    }
    
    public static GS2GC_021_053_OnFuncUnlockDone make_053_OnFuncUnlockDone(ENPFunctionType _type)
    {
        return new GS2GC_021_053_OnFuncUnlockDone(_type);
    }

    public static GS2GC_021_054_OnPrivilegeCardChg make_054_OnPrivilegeCardChg(PrivilegeCardInfo _info)
    {
    	GS2GC_021_054_OnPrivilegeCardChg proto = new GS2GC_021_054_OnPrivilegeCardChg();
    	proto.setCard(_info.toProto());

        return proto;
    }
    
    public static GS2GC_021_055_OnFriendGroupOrderListChg make_055_OnFriendGroupOrderListChg(List<Long> _groupIdList)
    {
        GS2GC_021_055_OnFriendGroupOrderListChg proto = new GS2GC_021_055_OnFriendGroupOrderListChg();
        proto.getFriendGroupOrderList().addAll(_groupIdList);
        return proto;
    }

    public static GS2GC_021_056_OnFriendGroupCreate make_056_OnFriendGroupCreate(long targetGroupId, String _name)
    {
        GS2GC_021_056_OnFriendGroupCreate proto = new GS2GC_021_056_OnFriendGroupCreate();
        proto.setGroupDbId(targetGroupId);
        proto.setName(_name);
        return proto;
    }

    public static GS2GC_021_057_OnFriendGroupDelete make_057_OnFriendGroupDelete(long targetGroupId)
    {
        GS2GC_021_057_OnFriendGroupDelete proto = new GS2GC_021_057_OnFriendGroupDelete();
        proto.setGroupDbId(targetGroupId);
        return proto;
    }

    public static GS2GC_021_058_OnFriendBelongGroupChg make_058_OnFriendBelongGroupChg(long targetGroupId, List<Long> cidList)
    {
        GS2GC_021_058_OnFriendBelongGroupChg proto = new GS2GC_021_058_OnFriendBelongGroupChg();
        proto.setTargetGroupDbId(targetGroupId);
        proto.getCidList().addAll(cidList);
        return proto;
    }

    public static GS2GC_021_059_OnFriendGroupNameChg make_059_OnFriendGroupNameChg(long targetGroupId, String _name)
    {
        GS2GC_021_059_OnFriendGroupNameChg proto = new GS2GC_021_059_OnFriendGroupNameChg();
        proto.setGroupDbId(targetGroupId);
        proto.setName(_name);
        return proto;
    }

    ////////////////////////////////////// 推送协议部分 //////////////////////////////////////

    public static GS2GC_021_060_OnFriendApplyChg make_060_OnFriendApplyChg(Friend_ApplyInfo _apply)
    {
        GS2GC_021_060_OnFriendApplyChg proto = new GS2GC_021_060_OnFriendApplyChg();
        proto.setApply(_apply);

        return proto;
    }

    public static GS2GC_021_061_OnFriendApplyRemove make_061_OnFriendApplyRemove(long _applyCid)
    {
        GS2GC_021_061_OnFriendApplyRemove proto = new GS2GC_021_061_OnFriendApplyRemove();
        proto.setApplyCid(_applyCid);

        return proto;
    }

    public static GS2GC_021_062_OnPlayerPermissionsChg make_062_OnPlayerPermissionsChg(ArrayList<Long> _idList)
    {
    	GS2GC_021_062_OnPlayerPermissionsChg proto = new GS2GC_021_062_OnPlayerPermissionsChg();
        proto.getEffectIdList().addAll(_idList);
        return proto;
    }

    public static GS2GC_021_063_OnFriendChg make_063_OnFriendChg(Friend_Info _friend, boolean _isAgree)
    {
        GS2GC_021_063_OnFriendChg proto = new GS2GC_021_063_OnFriendChg();
        proto.setFriend(_friend);
        proto.setIsAgree(_isAgree);

        return proto;
    }

    public static GS2GC_021_064_OnFriendRemove make_064_OnFriendRemove(long _fCid)
    {
        GS2GC_021_064_OnFriendRemove proto = new GS2GC_021_064_OnFriendRemove();
        proto.setFCid(_fCid);

        return proto;
    }

    public static GS2GC_021_065_OnClientNotifyFuncUnlock make_065_OnClientNotifyFuncUnlock(ENPFunctionType _type)
    {
        return new GS2GC_021_065_OnClientNotifyFuncUnlock(_type);
    }

    public static GS2GC_021_066_OnAchieveChg make_066_OnAchieveChg(Achieve_Info _info)
    {
        GS2GC_021_066_OnAchieveChg proto = new GS2GC_021_066_OnAchieveChg();
        proto.setAchieve(_info);
        return proto;
    }

    public static GS2GC_021_067_OnAchievePointChg make_067_OnAchievePointChg(Achieve_AchievePointInfo _info)
    {
        GS2GC_021_067_OnAchievePointChg proto = new GS2GC_021_067_OnAchievePointChg();
        proto.setAchievePointInfo(_info);
        return proto;
    }
    
    //////////////////////////////////////////// 政务相关协议 ////////////////////////////////////////////

    public static GS2GC_021_070_OnAnecdoteChg make_070_OnAnecdoteChg(_AAnecdoteEvent _info)
    {
        GS2GC_021_070_OnAnecdoteChg proto = new GS2GC_021_070_OnAnecdoteChg();
        proto.setInstanceId(_info.getDbId());
        _IALProtocolStructure extraData = _info.makeExtraData();
        if (extraData != null)
            proto.setExtraData(extraData.makePackage());
        return proto;
    }
    
    public static GS2GC_021_071_OnAnecdoteEventAdd make_071_OnAnecdoteEventAdd(List<Anecdote_EventInfo> _eventList)
    {
        GS2GC_021_071_OnAnecdoteEventAdd proto = new GS2GC_021_071_OnAnecdoteEventAdd();
        proto.getEventList().addAll(_eventList);
        return proto;
    }

    public static GS2GC_021_071_OnAnecdoteEventAdd make_071_OnAnecdoteEventAdd(Anecdote_EventInfo _event)
    {
        GS2GC_021_071_OnAnecdoteEventAdd proto = new GS2GC_021_071_OnAnecdoteEventAdd();
        proto.getEventList().add(_event);
        return proto;
    }

    public static GS2GC_021_072_OnAnecdoteEventRemove make_072_OnAnecdoteEventRemove(long _dbId)
    {
        return new GS2GC_021_072_OnAnecdoteEventRemove(_dbId);
    }

    public static GS2GC_021_073_OnCommonRefreshChg make_073_OnCommonRefreshChg(CommonFunc_Refresh _refreshInfo)
    {
        GS2GC_021_073_OnCommonRefreshChg proto = new GS2GC_021_073_OnCommonRefreshChg();
    	proto.setRefreshData(_refreshInfo);
        return proto;
    }
    
    public static GS2GC_021_075_OnForeverAddChg make_075_OnForeverAddChg(ForeverAddInfo _info)
    {
    	GS2GC_021_075_OnForeverAddChg proto = new GS2GC_021_075_OnForeverAddChg();
        proto.setAdd(_info.toProto());

        return proto;
    }

    public static GS2GC_021_083_OnChatEmoteGroupDel make_083_OnChatEmoteGroupDel(long _refId)
    {
        GS2GC_021_083_OnChatEmoteGroupDel proto = new GS2GC_021_083_OnChatEmoteGroupDel();
        proto.setRefId(_refId);
        return proto;
    }

    public static GS2GC_021_084_OnChatEmoteGroupChg make_084_OnChatEmoteGroupChg(PlayerInfo_ChatEmoteGroup _info)
    {
        GS2GC_021_084_OnChatEmoteGroupChg proto = new GS2GC_021_084_OnChatEmoteGroupChg();
        proto.setInfo(_info);
        return proto;
    }

    public static GS2GC_021_087_OnChatEmoteGroupAdd make_087_OnChatEmoteGroupAdd(PlayerInfo_ChatEmoteGroup _info)
    {
        GS2GC_021_087_OnChatEmoteGroupAdd proto = new GS2GC_021_087_OnChatEmoteGroupAdd();
        proto.setInfo(_info);
        return proto;
    }

    public static GS2GC_021_088_OnCuteActorDel make_088_OnCuteActorDel(long _refId)
    {
        GS2GC_021_088_OnCuteActorDel proto = new GS2GC_021_088_OnCuteActorDel();
        proto.setRefId(_refId);
        return proto;
    }

    public static GS2GC_021_089_OnCuteActorChg make_089_OnCuteActorChg(PlayerInfo_CuteActor _info)
    {
        GS2GC_021_089_OnCuteActorChg proto = new GS2GC_021_089_OnCuteActorChg();
        proto.setInfo(_info);
        return proto;
    }

    public static GS2GC_021_090_OnCuteActorAdd make_090_OnCuteActorAdd(PlayerInfo_CuteActor _info)
    {
        GS2GC_021_090_OnCuteActorAdd proto = new GS2GC_021_090_OnCuteActorAdd();
        proto.setInfo(_info);
        return proto;
    }

    //////////////////////////////////////////// 大臣推荐相关协议 ////////////////////////////////////////////
    
    public static GS2GC_021_085_OnHeroRecommendAdd make_085_OnHeroRecommendAdd(HeroRecommendInfo _info)
    {
    	GS2GC_021_085_OnHeroRecommendAdd proto = new GS2GC_021_085_OnHeroRecommendAdd();
    	proto.setInfo(_info.toProto());
        
        return proto;
    }
    
    public static GS2GC_021_086_OnHeroRecommendDel make_086_OnHeroRecommendDel(long _instanceId)
    {
    	GS2GC_021_086_OnHeroRecommendDel proto = new GS2GC_021_086_OnHeroRecommendDel();
    	proto.setInstanceId(_instanceId);

        return proto;
    }

    /**
     * 查看玩家懒惰CD信息
     */
    public static GS2GC_021_045_RetViewPlayerLazyCD make_045_RetViewPlayerLazyCD(PlayerLazyCD _cd)
    {
        GS2GC_021_045_RetViewPlayerLazyCD proto = new GS2GC_021_045_RetViewPlayerLazyCD();
        proto.setCdInfo(_cd.makeProto());
        return proto;
    }

    //////////////////////////////
    // RoomSkin部分协议
    //////////////////////////////

    /**
     * 新增房间皮肤推送
     * @param _info 房间皮肤信息
     * @return 推送协议对象
     */
    public static GS2GC_021_092_OnRoomSkinAdd make_092_OnRoomSkinAdd(PlayerRoomSkinInfo _info)
    {
        GS2GC_021_092_OnRoomSkinAdd proto = new GS2GC_021_092_OnRoomSkinAdd();
        proto.setInfo(_info.makeProto());
        return proto;
    }

    /**
     * 房间皮肤变更推送
     * @param _info 房间皮肤信息
     * @return 推送协议对象
     */
    public static GS2GC_021_093_OnRoomSkinChg make_093_OnRoomSkinChg(PlayerRoomSkinInfo _info)
    {
        GS2GC_021_093_OnRoomSkinChg proto = new GS2GC_021_093_OnRoomSkinChg();
        proto.setInfo(_info.makeProto());
        return proto;
    }

    /**
     * 房间皮肤删除推送
     * @param _roomSkinId 房间皮肤ID
     * @return 推送协议对象
     */
    public static GS2GC_021_094_OnRoomSkinDel make_094_OnRoomSkinDel(long _roomSkinId)
    {
        GS2GC_021_094_OnRoomSkinDel proto = new GS2GC_021_094_OnRoomSkinDel();
        proto.setRoomSkinId(_roomSkinId);
        return proto;
    }
}
