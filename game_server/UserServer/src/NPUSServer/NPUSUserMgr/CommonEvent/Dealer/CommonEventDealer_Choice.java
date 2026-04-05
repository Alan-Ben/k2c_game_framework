package NPUSServer.NPUSUserMgr.CommonEvent.Dealer;

import ALBasicProtocolPack._IALProtocolStructure;
import Common.EventEnum.ECommonEventType;
import Common.EventObj.CommonEvent_DealInfo_Choice;
import Common.EventObj.CommonEvent_DoneInfo;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.Log.CommLog;
import NPCommon.NPCommon_ItemInfo;
import NPGameRes.Refs.CommonEvent.SubClass.RefCommonEventChoice;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.CommonEvent._ACommonEventDealerWithExtra;
import NPUSServer.NPUSUserMgr.NPUSUserData;

import java.util.List;

public class CommonEventDealer_Choice extends _ACommonEventDealerWithExtra<RefCommonEventChoice,CommonEvent_DealInfo_Choice>
{
    @Override
    public ECommonEventType eventType()
    {
        return ECommonEventType.CHOICE;
    }

    @Override
    public _IALProtocolStructure createShowInfo(NPUSUserData _userData)
    {
        return null;
    }

    @Override
    public List<NPCommonCostItem> _getCmdRewardList(RefCommonEventChoice _detailRef)
    {
        if (_detailRef.event_reward_id_list.isEmpty())
            return null;

        return getEventRewardList(_detailRef.event_reward_id_list.get(0));
    }

    @Override
    public CommonEvent_DealInfo_Choice createDealInfoInstance()
    {
        return new CommonEvent_DealInfo_Choice();
    }

    @Override
    public ResultOne<CommonEvent_DoneInfo> _dealEvent(NPUSUserData _userData, RefCommonEventChoice _eventRef, CommonEvent_DealInfo_Choice _dealInfo, NPPlayerContext _context)
    {
        //计算奖励索引
        int index = _eventRef.option_id_list.indexOf(_dealInfo.getOptionId());
        if (index < 0)
            return ResultOne.failed(CommErr.PARAM_ERROR);

        //判断索引是否越界
        if (index >= _eventRef.event_reward_id_list.size())
        {
            CommLog.error("CommonEventDealer_Choice _dealEvent index out of range, eventId:{} index:{} rewardIdListSize:{}",
                    _eventRef.Id(), index, _eventRef.event_reward_id_list.size());
            return ResultOne.failed(CommErr.PARAM_ERROR);
        }

        Long rewardId = _eventRef.event_reward_id_list.get(index);

        //领取奖励
        List<NPCommon_ItemInfo> itemList = drawEventReward(_userData, rewardId, _context);

        CommonEvent_DoneInfo doneInfo = new CommonEvent_DoneInfo();
        doneInfo.setExtraInfo(_dealInfo.makePackage().array());
        if (itemList != null)
            doneInfo.getRewardList().addAll(itemList);
        return ResultOne.succ(doneInfo);
    }

}
