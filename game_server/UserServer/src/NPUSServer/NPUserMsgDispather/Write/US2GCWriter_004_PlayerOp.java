package NPUSServer.NPUserMsgDispather.Write;

import Common.CommonFuncObj.Order_Info;
import Common.Common_TodayLikeCidInfo;
import Common.LevyObj.Levy_FoodOfflineInfo;
import Common.NpPlayerInfoObj.PlayerInfo_CommonShow;
import Common.NpPlayerInfoObj.PlayerInfo_Record;
import Common.PlayerObj.Player_EventRecordInfo;
import Common.RankGiftPackObj.RankGiftPack_Info;
import Common.WeekCardObj.WeekCard_Info;
import Common.WeekCardObj.WeekCard_SettleInfo;
import GS2GC.p004_PlayerOp.*;
import NGSGC.p004_PlayerOp.*;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.PlayerInfo_IconShow;
import NPCommon.Util.Pair.WCGPairLong;
import NPEnum.ENPBoxChatStatus;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.PlayerBuffComp.PlayerBuffInfo;
import USDB.Bo.PlayerForbidChatBO;

import java.util.ArrayList;


public class US2GCWriter_004_PlayerOp
{
    public static GS2GC_004_001_RetReqLevelUp make_001_RetReqLevelUp(int _newLevel)
    {
        GS2GC_004_001_RetReqLevelUp proto = new GS2GC_004_001_RetReqLevelUp();
        proto.setNewLevel(_newLevel);
        return proto;
    }

    public static GS2GC_004_004_SetNameRes make_004_SetNameRes()
    {
        GS2GC_004_004_SetNameRes proto = new GS2GC_004_004_SetNameRes();
        return proto;
    }

    public static GS2GC_004_005_SetIconRes make_005_SetIconRes()
    {
        GS2GC_004_005_SetIconRes proto = new GS2GC_004_005_SetIconRes();
        return proto;
    }

    public static GS2GC_004_006_RetSetPrefab make_006_RetSetPrefab()
    {
        GS2GC_004_006_RetSetPrefab proto = new GS2GC_004_006_RetSetPrefab();
        return proto;
    }

    public static GS2GC_004_007_SetIconBgkRes make_007_SetIconBgkRes()
    {
        GS2GC_004_007_SetIconBgkRes proto = new GS2GC_004_007_SetIconBgkRes();
        return proto;
    }

    public static GS2GC_004_008_RetSetDefault make_008_RetSetDefault()
    {
    	GS2GC_004_008_RetSetDefault proto = new GS2GC_004_008_RetSetDefault();
        return proto;
    }

    public static GS2GC_004_009_SetBubbleRes make_009_SetBubbleRes()
    {
        GS2GC_004_009_SetBubbleRes proto = new GS2GC_004_009_SetBubbleRes();
        return proto;
    }

    public static GS2GC_004_010_RetSomeOnePlayerInfo make_010_RetSomeOnePlayerInfo(PlayerInfo_CommonShow playerInfo)
    {
        GS2GC_004_010_RetSomeOnePlayerInfo proto = new GS2GC_004_010_RetSomeOnePlayerInfo();
        proto.setSomeOneShowInfo(playerInfo);
        return proto;
    }

    public static GS2GC_004_012_RetTitleRecordList make_012_RetTitleRecordList(ArrayList<Long> _titleIdList)
    {
    	GS2GC_004_012_RetTitleRecordList proto = new GS2GC_004_012_RetTitleRecordList();
    	proto.getTitleIdList().addAll(_titleIdList);
    	
        return proto;
    }

    public static GS2GC_004_013_RetReportPlayer make_013_RetReportPlayer()
    {
        GS2GC_004_013_RetReportPlayer proto = new GS2GC_004_013_RetReportPlayer();

        return proto;
    }

    public static GS2GC_004_011_RetSomeOnePlayerBriefInfo make_011_RetSomeOnePlayerBriefInfo(PlayerInfo_IconShow _playerBrief)
    {
        GS2GC_004_011_RetSomeOnePlayerBriefInfo proto = new GS2GC_004_011_RetSomeOnePlayerBriefInfo();
        proto.setPlayerBrief(_playerBrief);
        return proto;
    }

    public static GS2GC_004_014_RetGainVisitOtherPlayerReward make_014_RetGainVisitOtherPlayerReward(NPPlayerContext _context)
    {
    	GS2GC_004_014_RetGainVisitOtherPlayerReward proto = new GS2GC_004_014_RetGainVisitOtherPlayerReward();
        
    	for(int i = 0; i < _context.getCollector().getAllItemList().size(); i++)
    	{
    		NPCommonCostItem item = _context.getCollector().getAllItemList().get(i);
    		if(null == item)
    			continue;
    		
    		proto.addItemList(item.toProto());
        }

        return proto;
    }

    public static GS2GC_004_015_RetWeekCardSettleInfo make_015_RetWeekCardSettleInfo(WeekCard_SettleInfo _settleInfo, Levy_FoodOfflineInfo _foodOfflineInfo)
    {
        GS2GC_004_015_RetWeekCardSettleInfo proto = new GS2GC_004_015_RetWeekCardSettleInfo();
        if (_settleInfo != null)
            proto.setSettleInfo(_settleInfo);
        if (_foodOfflineInfo != null)
            proto.setFoodOfflineInfo(_foodOfflineInfo);
        return proto;
    }

    public static GS2GC_004_016_RetWeekCardChgSetting make_016_RetWeekCardChgSetting()
    {
        return new GS2GC_004_016_RetWeekCardChgSetting();
    }

    public static GS2GC_004_017_RetWeekCardActiveFreeTrial make_017_RetWeekCardActiveFreeTrial()
    {
        return new GS2GC_004_017_RetWeekCardActiveFreeTrial();
    }

    public static GS2GC_004_018_RetWeekCardChgNPC make_018_RetWeekCardChgNPC()
    {
        return new GS2GC_004_018_RetWeekCardChgNPC();
    }

    public static GS2GC_004_019_RetDrawDailyReward make_019_RetDrawDailyReward()
    {
        return new GS2GC_004_019_RetDrawDailyReward();
    }

    public static GS2GC_004_022_RetSetCuteActor make_022_RetSetCuteActor()
    {
        GS2GC_004_022_RetSetCuteActor proto = new GS2GC_004_022_RetSetCuteActor();
        return proto;
    }

    public static GS2GC_004_023_RetGainChatBox make_023_RetGainChatBox(long _instanceId, ENPBoxChatStatus _boxStatus)
    {
        GS2GC_004_023_RetGainChatBox proto = new GS2GC_004_023_RetGainChatBox();
        proto.setInstanceId(_instanceId);
        proto.setBoxStatus(_boxStatus);
        return proto;
    }

    public static GS2GC_004_023_RetGainChatBox make_023_RetGainChatBox(long _instanceId
            , ENPBoxChatStatus _boxStatus
            , ArrayList<Long> _gainedCidList)
    {
        GS2GC_004_023_RetGainChatBox proto = new GS2GC_004_023_RetGainChatBox();
        proto.setInstanceId(_instanceId);
        proto.setBoxStatus(_boxStatus);
        proto.getGainedCidList().addAll(_gainedCidList);
        return proto;
    }

    public static GS2GC_004_024_RetRefreshChatBoxStatus make_024_RetRefreshChatBoxStatus(long _instanceId, ENPBoxChatStatus _boxStatus)
    {
        GS2GC_004_024_RetRefreshChatBoxStatus proto = new GS2GC_004_024_RetRefreshChatBoxStatus();
        proto.setInstanceId(_instanceId);
        proto.setBoxStatus(_boxStatus);
        return proto;
    }

    public static GS2GC_004_024_RetRefreshChatBoxStatus make_024_RetRefreshChatBoxStatus(long _instanceId
            , ENPBoxChatStatus _boxStatus
            , ArrayList<Long> _gainedCidList)
    {
        GS2GC_004_024_RetRefreshChatBoxStatus proto = new GS2GC_004_024_RetRefreshChatBoxStatus();
        proto.setInstanceId(_instanceId);
        proto.setBoxStatus(_boxStatus);
        proto.getGainedCidList().addAll(_gainedCidList);
        return proto;
    }

    public static GS2GC_004_025_RetHireEmployee make_025_RetHireEmployee(int _num)
    {
        GS2GC_004_025_RetHireEmployee proto = new GS2GC_004_025_RetHireEmployee();
        proto.setNum(_num);
        return proto;
    }

    public static GS2GC_004_031_RetSetShieldPlayer make_031_RetSetShieldPlayer()
    {
    	GS2GC_004_031_RetSetShieldPlayer proto = new GS2GC_004_031_RetSetShieldPlayer();

        return proto;
    }
    
    public static GS2GC_004_032_RetUnsetShieldPlayer make_032_RetUnsetShieldPlayer()
    {
    	GS2GC_004_032_RetUnsetShieldPlayer proto = new GS2GC_004_032_RetUnsetShieldPlayer();

        return proto;
    }

    public static GS2GC_004_033_RetTodayLikeCidInfo make_033_RetTodayLikeCidInfo(Common_TodayLikeCidInfo _info)
    {
        GS2GC_004_033_RetTodayLikeCidInfo proto = new GS2GC_004_033_RetTodayLikeCidInfo();
        proto.setTodayLikeCidInfo(_info);
        return proto;
    }

    public static GS2GC_004_034_RetPlayerDetailLike make_034_RetPlayerDetailLike(long _likeCount)
    {
        GS2GC_004_034_RetPlayerDetailLike proto = new GS2GC_004_034_RetPlayerDetailLike();
        proto.setLikeCount(_likeCount);
        return proto;
    }

    public static GS2GC_004_035_RetSelfLikeCount make_035_RetSelfLikeCount(WCGPairLong _likeCount)
    {
        GS2GC_004_035_RetSelfLikeCount proto = new GS2GC_004_035_RetSelfLikeCount();
        proto.setLastLikeCount(_likeCount.first());
        proto.setLikeCount(_likeCount.second());
        return proto;
    }

    public static GS2GC_004_036_RetDrawLoginCountReward make_036_RetDrawLoginCountReward()
    {
        return new GS2GC_004_036_RetDrawLoginCountReward();
    }

    public static GS2GC_004_037_RetDrawVipLevelReward make_037_RetDrawVipLevelReward()
    {
        return new GS2GC_004_037_RetDrawVipLevelReward();
    }

    public static GS2GC_004_041_RetGraveNewInfoList make_041_RetGraveNewInfoList(NPUSUserData _userData, ArrayList<Long> _titleIdList)
    {
    	GS2GC_004_041_RetGraveNewInfoList proto = new GS2GC_004_041_RetGraveNewInfoList();
    	_userData.getUSServer().getGraveMgr().getGraveNewMgr().makeGraveTitleNewInfoList(_titleIdList, proto.getNewInfoList());
    	
        return proto;
    }

    public static GS2GC_004_042_RetGraveRecordList make_042_RetGraveRecordList(ArrayList<Long> _titleIdList, int _curPage, int _pageCount, NPUSUserData _userData)
    {
    	GS2GC_004_042_RetGraveRecordList proto = new GS2GC_004_042_RetGraveRecordList();
    	int count = _userData.getUSServer().getGraveMgr().getGraveRecordMgr().makeRecordList(_titleIdList, _curPage, _pageCount, proto.getRecordList());
    	proto.setCount(count);
    	
        return proto;
    }

    public static GS2GC_004_043_RetGraveCelebrate make_043_RetGraveCelebrate(long _cid, long _buffId, NPPlayerContext _context)
    {
    	GS2GC_004_043_RetGraveCelebrate proto = new GS2GC_004_043_RetGraveCelebrate();
    	proto.setCid(_cid);
    	proto.setBuffId(_buffId);
    	_context.getCollector().fillProtoList(proto.getGainItemList());
    	
        return proto;
    }

    public static GS2GC_004_044_RetGraveConNewInfoList make_044_RetGraveConNewInfoList(NPUSUserData _userData)
    {
    	GS2GC_004_044_RetGraveConNewInfoList proto = new GS2GC_004_044_RetGraveConNewInfoList();
    	_userData.getUSServer().getGraveMgr().getGraveNewMgr().makeGraveNewRewardList(_userData.getCid(), proto.getNewInfoList());
    	
        return proto;
    }
    
    public static GS2GC_004_045_RetGraveConNewInfo make_045_RetGraveConNewInfo(long _cid, NPPlayerContext _context)
    {
    	GS2GC_004_045_RetGraveConNewInfo proto = new GS2GC_004_045_RetGraveConNewInfo();
    	proto.setCid(_cid);
    	_context.getCollector().fillProtoList(proto.getGainItemList());
    	
        return proto;
    }

    public static GS2GC_004_051_PushCurrencyInfo make_051_PushCurrencyInfo(int _type, long _count)
    {
        GS2GC_004_051_PushCurrencyInfo proto = new GS2GC_004_051_PushCurrencyInfo();
        proto.getCurrencyInfo().setType(_type);
        proto.getCurrencyInfo().setCount(_count);
        return proto;
    }

    public static GS2GC_004_052_OnPlayerBuffRemove make_052_OnPlayerBuffRemove(long _buffId)
    {
    	GS2GC_004_052_OnPlayerBuffRemove proto = new GS2GC_004_052_OnPlayerBuffRemove();
        proto.setBuffId(_buffId);
        return proto;
    }

    public static GS2GC_004_053_OnPlayerBuffChg make_053_OnPlayerBuffChg(PlayerBuffInfo _info)
    {
    	GS2GC_004_053_OnPlayerBuffChg proto = new GS2GC_004_053_OnPlayerBuffChg();
        proto.setBuff(_info.toProto());
        return proto;
    }

    public static GS2GC_004_055_OnPlayerNameUpdated make_055_OnPlayerNameUpdated(String _name)
    {
        GS2GC_004_055_OnPlayerNameUpdated proto = new GS2GC_004_055_OnPlayerNameUpdated();
        proto.setNewName(_name);
        return proto;
    }

    public static GS2GC_004_059_PushActivityCurrencyInfo make_059_PushActivityCurrencyInfo(long _activityId, long _count)
    {
        GS2GC_004_059_PushActivityCurrencyInfo proto = new GS2GC_004_059_PushActivityCurrencyInfo();
        proto.getCurrencyInfo().setRefId(_activityId);
        proto.getCurrencyInfo().setCount(_count);
        return proto;
    }

    public static GS2GC_004_060_OnPlayerRecordChg make_060_OnPlayerRecordChg(PlayerInfo_Record _recordProto)
    {
        GS2GC_004_060_OnPlayerRecordChg proto = new GS2GC_004_060_OnPlayerRecordChg();
        proto.setRecord(_recordProto);
        return proto;
    }

    public static GS2GC_004_061_OnPlayerBuffTrigger make_061_OnPlayerBuffTrigger(long _buffId)
    {
    	GS2GC_004_061_OnPlayerBuffTrigger proto = new GS2GC_004_061_OnPlayerBuffTrigger();
        proto.setBuffId(_buffId);
        return proto;
    }

    public static GS2GC_004_063_OnWeekCardChg make_063_OnWeekCardChg(WeekCard_Info _cardInfo)
    {
    	GS2GC_004_063_OnWeekCardChg proto = new GS2GC_004_063_OnWeekCardChg();
        proto.setInfo(_cardInfo);
        return proto;
    }

    public static GS2GC_004_065_OnEventRecordChg make_065_OnEventRecordChg(Player_EventRecordInfo _record)
    {
        GS2GC_004_065_OnEventRecordChg proto = new GS2GC_004_065_OnEventRecordChg();
        proto.setRecord(_record);
        return proto;
    }

    public static GS2GC_004_071_OnShieldCidAdd make_071_OnShieldCidAdd(long _cid)
    {
        return new GS2GC_004_071_OnShieldCidAdd(_cid);
    }

    public static GS2GC_004_072_OnShieldCidRemove make_072_OnShieldCidRemove(long _cid)
    {
        return new GS2GC_004_072_OnShieldCidRemove(_cid);
    }

    public static GS2GC_004_073_OnTodayLikeCidInfoChg make_073_OnTodayLikeCidInfoChg(Common_TodayLikeCidInfo _info)
    {
        return new GS2GC_004_073_OnTodayLikeCidInfoChg(_info);
    }

    public static GS2GC_004_062_OnGraveNewReward make_062_OnGraveNewReward()
    {
    	GS2GC_004_062_OnGraveNewReward proto = new GS2GC_004_062_OnGraveNewReward();
        return proto;
    }

    // ============= 订单相关协议 =============

    /**
     * 创建支付订单返回消息
     * @param orderId 订单号
     * @return 创建支付订单返回协议
     */
    public static GS2GC_004_020_RetCreatePayOrder make_020_RetCreatePayOrder(String orderId)
    {
        return new GS2GC_004_020_RetCreatePayOrder(orderId);
    }

    /**
     * 客户端支付完成返回消息
     * @return 客户端支付完成返回协议
     */
    public static GS2GC_004_021_RetClientPayDone make_021_RetClientPayDone()
    {
        return new GS2GC_004_021_RetClientPayDone();
    }

    /**
     * 客户端支付取消返回消息
     * @return 客户端支付取消返回协议
     */
    public static GS2GC_004_026_RetClientPayCancel make_026_RetClientPayCancel()
    {
        return new GS2GC_004_026_RetClientPayCancel();
    }


    /**
     * 订单代金券支付返回消息
     * @return 订单代金券支付返回协议
     */
    public static GS2GC_004_027_RetOrderVoucherPay make_027_RetOrderVoucherPay()
    {
        return new GS2GC_004_027_RetOrderVoucherPay();
    }

    /**
     * 订单添加推送消息
     * @param orderInfo 订单信息
     * @return 订单添加推送协议
     */
    public static GS2GC_004_066_OnOrderAdd make_066_OnOrderAdd(Order_Info orderInfo)
    {
        return new GS2GC_004_066_OnOrderAdd(orderInfo);
    }

    /**
     * 订单状态变更推送消息
     * @param orderInfo 订单信息
     * @return 订单状态变更推送协议
     */
    public static GS2GC_004_067_OnOrderChg make_067_OnOrderChg(Order_Info orderInfo)
    {
        return new GS2GC_004_067_OnOrderChg(orderInfo);
    }

    /**
     * 触发推送礼包组返回协议（046协议，空响应）
     *
     * @return 触发推送礼包组返回协议
     */
    public static GS2GC_004_046_RetTriggerPushGiftGroup make_046_RetTriggerPushGiftGroup()
    {
        return new GS2GC_004_046_RetTriggerPushGiftGroup();
    }

    /**
     * 设置玩家房间皮肤返回协议（047协议，空响应）
     *
     * @return 设置房间皮肤返回协议
     */
    public static GS2GC_004_047_RetSetRoomSkin make_047_RetSetRoomSkin()
    {
        return new GS2GC_004_047_RetSetRoomSkin();
    }

    /**
     * 标记推送礼包为已读返回协议（048协议，空响应）
     *
     * @return 标记已读返回协议
     */
    public static GS2GC_004_048_RetMarkPushGiftAsRead make_048_RetMarkPushGiftAsRead()
    {
        return new GS2GC_004_048_RetMarkPushGiftAsRead();
    }

    /**
     * 构造购买冲榜礼包响应
     *
     * @return 购买冲榜礼包响应协议
     */
    public static GS2GC_004_039_RetBuyRankGiftPack make_039_RetBuyRankGiftPack()
    {
        return new GS2GC_004_039_RetBuyRankGiftPack();
    }

    /**
     * 构造冲榜礼包信息变更推送
     *
     * @param _giftPackInfo 礼包信息
     * @return 冲榜礼包信息变更推送协议
     */
    public static GS2GC_004_064_OnRankGiftPackChg make_064_OnRankGiftPackChg(RankGiftPack_Info _giftPackInfo)
    {
        GS2GC_004_064_OnRankGiftPackChg proto = new GS2GC_004_064_OnRankGiftPackChg();
        if (_giftPackInfo != null)
            proto.setGiftPackInfo(_giftPackInfo);
        return proto;
    }

    /**
     * 构造急速兑换响应（101协议，空响应）
     *
     * @return 急速兑换响应协议
     */
    public static GS2GC.p004_PlayerOp.GS2GC_004_101_RetRushExchange make_101_RetRushExchange()
    {
        return new GS2GC.p004_PlayerOp.GS2GC_004_101_RetRushExchange();
    }

    /**
     * 构造急速兑换刷新响应（102协议，空响应）
     *
     * @return 急速兑换刷新响应协议
     */
    public static GS2GC.p004_PlayerOp.GS2GC_004_102_RetRushExchangeRefresh make_102_RetRushExchangeRefresh()
    {
        return new GS2GC.p004_PlayerOp.GS2GC_004_102_RetRushExchangeRefresh();
    }

    /**
     * 构造急速兑换领取奖励响应（103协议，空响应）
     *
     * @return 急速兑换领取奖励响应协议
     */
    public static GS2GC.p004_PlayerOp.GS2GC_004_103_RetRushExchangeReward make_103_RetRushExchangeReward()
    {
        return new GS2GC.p004_PlayerOp.GS2GC_004_103_RetRushExchangeReward();
    }

    /**
     * 构造急速兑换信息变更推送
     *
     * @param _info 急速兑换信息
     * @return 急速兑换信息变更推送协议
     */
    public static GS2GC_004_075_OnRushExchangeChg make_075_OnRushExchangeChg(Common.RushExchangeObj.RushExchange_Info _info)
    {
        GS2GC_004_075_OnRushExchangeChg proto = new GS2GC_004_075_OnRushExchangeChg();
        if (_info != null)
            proto.setInfo(_info);
        return proto;
    }

    /**
     * 构造情人收集数据变更推送
     */
    public static GS2GC_004_076_OnLoverCollectChg make_076_OnLoverCollectChg(long _targetLoverId, boolean _isClaimed)
    {
        GS2GC_004_076_OnLoverCollectChg proto = new GS2GC_004_076_OnLoverCollectChg();
        proto.setTargetLoverId(_targetLoverId);
        proto.setIsClaimed(_isClaimed);
        return proto;
    }

    /**
     * 构造情人收集-选择目标情人回包
     */
    public static GS2GC_004_104_RetSetLoverTarget make_104_RetSetLoverTarget()
    {
        return new GS2GC_004_104_RetSetLoverTarget();
    }

    /**
     * 构造情人收集-领取情人回包
     */
    public static GS2GC_004_105_RetClaimLover make_105_RetClaimLover()
    {
        return new GS2GC_004_105_RetClaimLover();
    }

    /**
     * 构造禁言数据变更推送
     * @param _bo
     * @return
     */
    public static GS2GC_004_068_OnForbidChatChg make_068_OnForbidChatChg(PlayerForbidChatBO _bo)
    {
        GS2GC_004_068_OnForbidChatChg proto = new GS2GC_004_068_OnForbidChatChg();
        proto.getForbidChat().setRoomType(_bo.getRoomType());
        proto.getForbidChat().setEndMs(_bo.getEndMs());

        return proto;
    }

    /**
     * 构造解除禁言推送
     * @param _roomType
     * @return
     */
    public static GS2GC_004_069_OnRemoveForbidChat make_069_OnRemoveForbidChat(int _roomType)
    {
        GS2GC_004_069_OnRemoveForbidChat proto = new GS2GC_004_069_OnRemoveForbidChat();
        proto.setRoomType(_roomType);

        return proto;
    }
}
