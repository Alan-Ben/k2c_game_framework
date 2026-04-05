package NPUSServer.NPUSUserMgr.CommonEvent.Dealer;

import ALBasicProtocolPack._IALProtocolStructure;
import Common.EventEnum.ECommonEventType;
import Common.EventObj.CommonEvent_DealInfo_Dispatch;
import Common.EventObj.CommonEvent_DoneInfo;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.HeroErr;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.Log.CommLog;
import NPCommon.NPCommon_ItemInfo;
import NPGameRes.Refs.CommonEvent.SubClass.RefCommonEventDispatch;
import NPGameRes.Refs.CommonEvent.SubClass.RefCommonEventDispatchCond;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.CommonEvent._ACommonEventDealerWithExtra;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroInfo;

import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;

public class CommonEventDealer_Dispatch extends _ACommonEventDealerWithExtra<RefCommonEventDispatch, CommonEvent_DealInfo_Dispatch>
{
    @Override
    public ECommonEventType eventType()
    {
        return ECommonEventType.DISPATCH;
    }

    @Override
    public _IALProtocolStructure createShowInfo(NPUSUserData _userData)
    {
        return null;
    }

    @Override
    public List<NPCommonCostItem> _getCmdRewardList(RefCommonEventDispatch _detailRef)
    {
        if (_detailRef.event_reward_list.isEmpty())
            return null;

        //获取最后一个奖励
        return getEventRewardList(_detailRef.event_reward_list.get(_detailRef.event_reward_list.size() - 1));
    }

    @Override
    public CommonEvent_DealInfo_Dispatch createDealInfoInstance()
    {
        return new CommonEvent_DealInfo_Dispatch();
    }

    @Override
    public ResultOne<CommonEvent_DoneInfo> _dealEvent(NPUSUserData _userData, RefCommonEventDispatch _eventRef, CommonEvent_DealInfo_Dispatch _dealInfo, NPPlayerContext _context)
    {
        //对大臣列表进行去重
        HashSet<Long> heroSet = new HashSet<>(_dealInfo.getHeroList());
        //检查大臣数量是否超过配置
        if (heroSet.size() > _eventRef.hero_num)
            return ResultOne.failed(CommErr.PARAM_ERROR);

        //大臣列表
        List<HeroInfo> heroList = new ArrayList<>();
        //遍历大臣列表
        for (Long heroId : heroSet)
        {
            //检查大臣是否存在
            HeroInfo heroInfo = _userData.getHeroComponent().lookupHero(heroId);
            if (heroInfo == null)
                return ResultOne.failed(HeroErr.HERO_NOT_FOUND);

            heroList.add(heroInfo);
        }

        //把大臣列表中对象的数量填充到配置的数量
        while (heroList.size() < _eventRef.hero_num)
            heroList.add(null);

        //计算达成了多少条件
        int reachConditionNum = 0;
        //遍历配置的条件列表
        for (RefCommonEventDispatchCond refCond : _eventRef.condList)
        {
            //符合条件的大臣数量
            int condHeroNum = 0;
            for (HeroInfo heroInfo : heroList)
            {
                //检查大臣是否符合条件
                if (true)
                    condHeroNum++;
            }

            //如果符合条件的大臣数量大于等于配置的数量, 则达成了该条件
            if (condHeroNum >= refCond.num)
                reachConditionNum++;
        }

        //防止数组越界
        if (_eventRef.event_reward_list.size() <= reachConditionNum)
        {
            CommLog.error("CommonEventDealer_Dispatch _dealEvent index out of range, eventId:{} reachConditionNum:{} rewardIdListSize:{}",
                    _eventRef.Id(), reachConditionNum, _eventRef.event_reward_list.size());
            return ResultOne.failed(CommErr.PARAM_ERROR);
        }

        //获取奖励id(配置会包含达成0-n条条件的奖励, 如果达成0条, 则取索引0的奖励, 如果达成n条, 则取索引n的奖励)
        Long rewardId = _eventRef.event_reward_list.get(reachConditionNum);

        //领取奖励
        List<NPCommon_ItemInfo> itemList = drawEventReward(_userData, rewardId, _context);

        CommonEvent_DoneInfo doneInfo = new CommonEvent_DoneInfo();
        doneInfo.setExtraInfo(_dealInfo.makePackage().array());
        if (itemList != null)
            doneInfo.getRewardList().addAll(itemList);
        return ResultOne.succ(doneInfo);
    }
}
