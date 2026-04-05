package NPUSServer.NPUserMsgDispather.Write;

import ALBasicProtocolPack._IALProtocolStructure;
import Common.CommonFuncObj.CommonFunc_TargetReward;
import Common.Common_CrossServerGroupInfo;
import Common.Common_QuestionnaireRewardInfo;
import Common.Common_RedDotInfo;
import Common.CountdownEventObj.CountdownEvent_Info;
import Common.GachaObj.Gacha_PoolInfo;
import Common.GachaObj.Gacha_PublicRecordInfo;
import Common.GachaObj.Gacha_RecordInfo;
import Common.StageGoalObj.StageGoal_BigStepFirstReachInfo;
import Common.StageGoalObj.StageGoal_TopPlayerInfo;
import CommonEnum.ERedDotType;
import GS2GC.p007_CommOp.*;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.Util.CommonFunc;
import NPEnum.ENpRewardShowType;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineRewardInfo;
import NPUSServer.NPUSUserMgr.UserComp.StageGlobalComp.StageGoalTaskInfo;

import java.util.ArrayList;
import java.util.List;

public class US2GCWriter_007_CommOp
{
    public static GS2GC_007_001_RetDealRemoteEffect make_001_RetDealRemoteEffect(int _errCode, long _clientSerialize, long _refId)
    {
        GS2GC_007_001_RetDealRemoteEffect proto = new GS2GC_007_001_RetDealRemoteEffect();
        proto.setErrCode(_errCode);
        proto.setClientSerialize(_clientSerialize);
        proto.setRefId(_refId);
        return proto;
    }

    public static GS2GC_007_003_RetDrawQuestionnaireReward make_003_RetDrawQuestionnaireReward()
    {
        return new GS2GC_007_003_RetDrawQuestionnaireReward();
    }

    public static GS2GC_007_004_RetQuestionnaireInfo make_004_RetQuestionnaireInfo(Common_QuestionnaireRewardInfo _rewardInfo)
    {
        GS2GC_007_004_RetQuestionnaireInfo proto = new GS2GC_007_004_RetQuestionnaireInfo();
        if (_rewardInfo != null)
        {
            proto.setHasReward(true);
            proto.setRewardInfo(_rewardInfo);
        }
        return proto;
    }

    public static GS2GC_007_005_RetGenerateShareCode make_005_RetGenerateShareCode(String _shareCode)
    {
        GS2GC_007_005_RetGenerateShareCode proto = new GS2GC_007_005_RetGenerateShareCode();
        proto.setShareCode(_shareCode);
        return proto;
    }

    public static GS2GC_007_006_RetShareCodeData make_006_RetShareCodeData(byte[] _data)
    {
        GS2GC_007_006_RetShareCodeData proto = new GS2GC_007_006_RetShareCodeData();
        proto.setData(_data);
        return proto;
    }

    public static GS2GC_007_007_RetGetOnlineCidList make_007_RetGetOnlineCidList(int _num, NPUSUserData _userData)
    {
    	GS2GC_007_007_RetGetOnlineCidList proto = new GS2GC_007_007_RetGetOnlineCidList();

    	//服务端保护，不超过100条数据
    	int num = Math.min(_num, 100);

    	ArrayList<NPUSUserData> onlineUserDataList = _userData.getUSServer().getUsUserMgr().getAllCacheUserData();
    	if(num >= onlineUserDataList.size()) //所需数量 大于 当前在线玩家数量，则当前玩家数量全部放入结果列表
    	{
    		for(int i = 0; i < onlineUserDataList.size(); i++)
        	{
        		NPUSUserData userData = onlineUserDataList.get(i);
        		if(null == userData)
        			continue;

        		//过滤自身数据
        		if(userData.getCid() == _userData.getCid())
        			continue;

        		proto.addCidList(userData.getCid());
        	}
    	}
    	else //如果所需数量 小于 当前在线玩家数量，则进行随机挑选
    	{
    		while(onlineUserDataList.size() > 0)
    		{
    			int idx = CommonFunc.randomInt(0, onlineUserDataList.size() - 1);
    			NPUSUserData userData = onlineUserDataList.remove(idx);
    			if(null == userData)
    				continue;

        		//过滤自身数据
    			if(userData.getCid() == _userData.getCid())
    				continue;

    			proto.addCidList(userData.getCid());

    			if(proto.getCidList().size() >= num)
    				break;
    		}
    	}

        return proto;
    }



    public static GS2GC_007_015_RetDrawStageGoalFirstReachReward make_015_RetDrawStageGoalFirstReachReward()
    {
        return new GS2GC_007_015_RetDrawStageGoalFirstReachReward();
    }

    public static GS2GC_007_016_RetStageGoalFirstReachDetailInfo make_016_RetStageGoalFirstReachDetailInfo(List<StageGoal_BigStepFirstReachInfo> _dataList)
    {
        GS2GC_007_016_RetStageGoalFirstReachDetailInfo proto = new GS2GC_007_016_RetStageGoalFirstReachDetailInfo();
        if (_dataList != null)
            proto.getInfoList().addAll(_dataList);
        return proto;
    }

    public static GS2GC_007_017_RetStageGoalFirstReachBaseInfo make_017_RetStageGoalFirstReachBaseInfo(
            StageGoal_TopPlayerInfo _top1Info, List<StageGoal_BigStepFirstReachInfo> _dataList)
    {
        GS2GC_007_017_RetStageGoalFirstReachBaseInfo proto = new GS2GC_007_017_RetStageGoalFirstReachBaseInfo();
        proto.setTopInfo(_top1Info);
        proto.getInfoList().addAll(_dataList);
        return proto;
    }

    public static GS2GC_007_018_RetTakeStageGoalSubTaskReward make_018_RetTakeStageGoalSubTaskReward(NPPlayerContext _context)
    {
        GS2GC_007_018_RetTakeStageGoalSubTaskReward proto = new GS2GC_007_018_RetTakeStageGoalSubTaskReward();
        _context.getCollector().fillProtoList(proto.getItemList());
        return proto;
    }

    public static GS2GC_007_019_RetTakeStageGoalBigStepReward make_019_RetTakeStageGoalBigStepReward(NPPlayerContext _context)
    {
        GS2GC_007_019_RetTakeStageGoalBigStepReward proto = new GS2GC_007_019_RetTakeStageGoalBigStepReward();
        _context.getCollector().fillProtoList(proto.getItemList());
        return proto;
    }

    public static GS2GC_007_020_RetTakeOfflineReward make_020_RetTakeOfflineReward(_IALProtocolStructure _protoObj)
    {
        GS2GC_007_020_RetTakeOfflineReward proto = new GS2GC_007_020_RetTakeOfflineReward();
        if(null != _protoObj)
        {
        	proto.setExtData(_protoObj.makePackage());
        }

        return proto;
    }

    public static GS2GC_007_020_RetTakeOfflineReward make_020_RetTakeOfflineReward()
    {
        GS2GC_007_020_RetTakeOfflineReward proto = new GS2GC_007_020_RetTakeOfflineReward();

        return proto;
    }

    public static GS2GC_007_021_RetTakeOfflineRewardList make_021_RetTakeOfflineRewardList(ArrayList<Long> _sucIdList)
    {
        GS2GC_007_021_RetTakeOfflineRewardList proto = new GS2GC_007_021_RetTakeOfflineRewardList();
        proto.getIdList().addAll(_sucIdList);

        return proto;
    }

    public static GS2GC_007_022_RetDrawTargetReward make_022_RetDrawTargetReward()
    {
        return new GS2GC_007_022_RetDrawTargetReward();
    }

    public static GS2GC_007_023_RetRemoveCountdownEvent make_023_RetRemoveCountdownEvent()
    {
        return new GS2GC_007_023_RetRemoveCountdownEvent();
    }

    public static GS2GC_007_024_RetResetCountdownEvent make_024_RetResetCountdownEvent()
    {
        return new GS2GC_007_024_RetResetCountdownEvent();
    }

    public static GS2GC_007_025_RetTakeStageGoalTaskReward make_025_RetTakeStageGoalTaskReward(NPPlayerContext _context)
    {
    	GS2GC_007_025_RetTakeStageGoalTaskReward proto = new GS2GC_007_025_RetTakeStageGoalTaskReward();
        _context.getCollector().fillProtoList(proto.getItemList());
        return proto;
    }

    public static GS2GC_007_026_RetGachaRoll make_026_RetGachaRoll(boolean _isTen, List<Long> _itemIdList, NPPlayerContext _context)
    {
        GS2GC_007_026_RetGachaRoll proto = new GS2GC_007_026_RetGachaRoll();
        proto.setIsTen(_isTen);
        proto.getPoolItemIdList().addAll(_itemIdList);
        _context.getCollector().fillProtoList(proto.getItemList());
        return proto;
    }

    public static GS2GC_007_027_RetGachaRollRecord make_027_RetGachaRollRecord(List<Gacha_RecordInfo> _recordList)
    {
        GS2GC_007_027_RetGachaRollRecord proto = new GS2GC_007_027_RetGachaRollRecord();
        if (_recordList != null)
            proto.getRecordList().addAll(_recordList);
        return proto;
    }

    public static GS2GC_007_028_RetRecruit make_028_RetRecruit()
    {
        return new GS2GC_007_028_RetRecruit();
    }

    public static GS2GC_007_029_RetGachaPublicRollRecord make_029_RetGachaPublicRollRecord(List<Gacha_PublicRecordInfo> _recordList)
    {
        GS2GC_007_029_RetGachaPublicRollRecord proto = new GS2GC_007_029_RetGachaPublicRollRecord();
        if (_recordList != null)
            proto.getRecordList().addAll(_recordList);
        return proto;
    }

    public static GS2GC_007_030_RetDrawGachaCumulativeReward make_030_RetDrawGachaCumulativeReward()
    {
        return new GS2GC_007_030_RetDrawGachaCumulativeReward();
    }

    /**
     * 构造清除红点响应协议
     *
     * @return 清除响应协议对象
     */
    public static GS2GC_007_031_RetClearRedDot make_031_RetClearRedDot() {
        return new GS2GC_007_031_RetClearRedDot();
    }

    /**
     * 构造检查红点状态响应协议
     *
     * @return 检查红点响应协议对象
     */
    public static GS2GC_007_032_RetCheckRedDot make_032_RetCheckRedDot() 
    {
        return new GS2GC_007_032_RetCheckRedDot();
    }

    public static GS2GC_007_033_RetStoreReviewsRecordRoast make_033_RetStoreReviewsRecordRoast() 
    {
        return new GS2GC_007_033_RetStoreReviewsRecordRoast();
    }

    public static GS2GC_007_034_RetStoreReviewsGetReward make_034_RetStoreReviewsGetReward() 
    {
        return new GS2GC_007_034_RetStoreReviewsGetReward();
    }

    public static GS2GC_007_050_OnGainItemList make_050_OnGainItemList(ArrayList<NPCommonCostItem> _itemList)
    {
        return make_050_OnGainItemList(ENpRewardShowType.DEFAULT, _itemList);
    }

    public static GS2GC_007_050_OnGainItemList make_050_OnGainItemList(ENpRewardShowType _showType, ArrayList<NPCommonCostItem> _itemList)
    {
        if (null == _itemList || _itemList.size() <= 0)
            return null;

        GS2GC_007_050_OnGainItemList proto = new GS2GC_007_050_OnGainItemList();
        proto.setRewardShowType(_showType);

        if (null != _itemList)
        {
            NPCommonCostItem tmpItem = null;
            for (int i = 0; i < _itemList.size(); i++)
            {
                tmpItem = _itemList.get(i);
                if (null == tmpItem)
                    continue;

                proto.addItemList(tmpItem.toProto());
            }
        }

        return proto;
    }

    public static GS2GC_007_051_OnCommError make_051_OnCommError(int _errCode)
    {
        GS2GC_007_051_OnCommError proto = new GS2GC_007_051_OnCommError();
        proto.setErrCode(_errCode);

        return proto;
    }

    public static GS2GC_007_052_OnAnnouncementVersionChg make_052_OnAnnouncementVersionChg(String _version)
    {
        GS2GC_007_052_OnAnnouncementVersionChg proto = new GS2GC_007_052_OnAnnouncementVersionChg();
        proto.setVersion(_version);
        return proto;
    }

    public static GS2GC_007_054_OnOfflineRewardDel make_054_OnOfflineRewardDel(long _id)
    {
        GS2GC_007_054_OnOfflineRewardDel proto = new GS2GC_007_054_OnOfflineRewardDel();
        proto.setId(_id);

        return proto;
    }

    public static GS2GC_007_055_OnOfflineRewardAdd make_055_OnOfflineRewardAdd(OfflineRewardInfo _info)
    {
        GS2GC_007_055_OnOfflineRewardAdd proto = new GS2GC_007_055_OnOfflineRewardAdd();
        proto.setReward(_info.toProto());

        return proto;
    }

    public static GS2GC_007_057_OnCrossServerGroupInfoChg make_057_OnCrossServerGroupInfoChg(Common_CrossServerGroupInfo _groupInfo)
    {
        return new GS2GC_007_057_OnCrossServerGroupInfoChg(_groupInfo);
    }

    /**
     * 构造新增红点推送协议
     *
     * @param _redDotInfo 新增的红点类型
     * @return 新增红点推送协议对象
     */
    public static GS2GC_007_059_PushRedDotChg make_059_PushRedDotChg(Common_RedDotInfo _redDotInfo) {
        GS2GC_007_059_PushRedDotChg proto = new GS2GC_007_059_PushRedDotChg();
        proto.setRedDotInfo(_redDotInfo);
        return proto;
    }

    /**
     * 构造移除红点推送协议
     *
     * @param _redDotType 移除的红点类型
     * @return 移除红点推送协议对象
     */
    public static GS2GC_007_060_PushRedDotRemove make_060_PushRedDotRemove(ERedDotType _redDotType) {
        GS2GC_007_060_PushRedDotRemove proto = new GS2GC_007_060_PushRedDotRemove();
        proto.setRedDotType(_redDotType);
        return proto;
    }

    public static GS2GC_007_062_OnTargetRewardUpdate make_062_OnTargetRewardUpdate(CommonFunc_TargetReward _info)
    {
        return new GS2GC_007_062_OnTargetRewardUpdate(_info);
    }

    public static GS2GC_007_069_OnStageGoalBigStepRewardDraw make_069_OnStageGoalBigStepRewardDraw(long _bigStep)
    {
        GS2GC_007_069_OnStageGoalBigStepRewardDraw proto = new GS2GC_007_069_OnStageGoalBigStepRewardDraw();
    	proto.setBigStep(_bigStep);
        return proto;
    }

    public static GS2GC_007_070_OnStageGoalTaskChg make_070_OnStageGoalTaskChg(StageGoalTaskInfo _task)
    {
    	GS2GC_007_070_OnStageGoalTaskChg proto = new GS2GC_007_070_OnStageGoalTaskChg();
    	proto.setStep(_task.getStep());
    	proto.setTask(_task.toProto());

        return proto;
    }

    public static GS2GC_007_071_OnStageGoalChg make_071_OnStageGoalChg(NPUSUserData _userData)
    {
    	GS2GC_007_071_OnStageGoalChg proto = new GS2GC_007_071_OnStageGoalChg();
    	_userData.getStageGoalComponent().makeProto(proto.getStageGoal());

        return proto;
    }

    public static GS2GC_007_073_OnRecuritRecordAdd make_073_OnRecuritRecordAdd(long _recuritId)
    {
        return new GS2GC_007_073_OnRecuritRecordAdd(_recuritId);
    }

    public static GS2GC_007_074_OnGachaPoolChg make_074_OnGachaPoolChg(Gacha_PoolInfo _poolInfo)
    {
        return new GS2GC_007_074_OnGachaPoolChg(_poolInfo);
    }

    public static GS2GC_007_076_OnCountdownEventChg make_076_OnCountdownEventChg(CountdownEvent_Info _eventInfo)
    {
        if (_eventInfo == null)
            return new GS2GC_007_076_OnCountdownEventChg();
        return new GS2GC_007_076_OnCountdownEventChg(_eventInfo);
    }

    public static GS2GC_007_077_OnStageGoalBigStepFirstReachAdd make_077_OnStageGoalBigStepFirstReachAdd(long _bigStepId)
    {
        return new GS2GC_007_077_OnStageGoalBigStepFirstReachAdd(_bigStepId);
    }

    public static GS2GC_007_078_OnStageGoalBigStepFirstReachRewardDraw make_078_OnStageGoalBigStepFirstReachRewardDraw(long _bigStepId)
    {
        return new GS2GC_007_078_OnStageGoalBigStepFirstReachRewardDraw(_bigStepId);
    }
}
