package NPUSServer.NPUSUserMgr.CommonEvent.Dealer;

import ALBasicProtocolPack._IALProtocolStructure;
import Common.EventEnum.ECommonEventType;
import Common.EventObj.CommonEvent_DoneInfo;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.NPCommon_ItemInfo;
import NPGameRes.Refs.CommonEvent.SubClass.RefCommonEventMiniGame;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.CommonEvent._ACommonEventDealerNoExtra;
import NPUSServer.NPUSUserMgr.NPUSUserData;

import java.util.List;

public class CommonEventDealer_MiniGame extends _ACommonEventDealerNoExtra<RefCommonEventMiniGame>
{
    @Override
    public ECommonEventType eventType()
    {
        return ECommonEventType.MINI_GAME;
    }

    @Override
    public _IALProtocolStructure createShowInfo(NPUSUserData _userData)
    {
        return null;
    }

    @Override
    public List<NPCommonCostItem> _getCmdRewardList(RefCommonEventMiniGame _detailRef)
    {
        return getEventRewardList(_detailRef.event_reward_id);
    }

    @Override
    public ResultOne<CommonEvent_DoneInfo> _dealEvent(NPUSUserData _userData, RefCommonEventMiniGame _eventRef, NPPlayerContext _context)
    {
        //领取奖励
        List<NPCommon_ItemInfo> itemList = drawEventReward(_userData, _eventRef.event_reward_id, _context);

        CommonEvent_DoneInfo doneInfo = new CommonEvent_DoneInfo();
        if (itemList != null)
            doneInfo.getRewardList().addAll(itemList);
        return ResultOne.succ(doneInfo);
    }
}
