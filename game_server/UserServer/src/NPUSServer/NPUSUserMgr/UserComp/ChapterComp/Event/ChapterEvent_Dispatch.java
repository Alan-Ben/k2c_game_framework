package NPUSServer.NPUSUserMgr.UserComp.ChapterComp.Event;

import ALBasicProtocolPack._IALProtocolStructure;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.HeroErr;
import NPCommon.ErrMain.Result.ResultOne;
import NPGameRes.Refs.Chapter.Event.RefChapterEventDispatch;
import NPGameRes.Refs.Chapter.Event.RefChapterEventDispatchCond;
import NPGameRes.Refs.Chapter.Event.RefChapterEventDispatchReward;
import NPGameRes.Refs.Chapter.Event._ARefChapterEvent;
import NPGameRes.Refs.Chapter.RefChapterEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.ChapterComp.ChapterComponent;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.ConditionDealer.HeroConditionDealerMgr;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroInfo;
import NPUSServer.USLog;
import USDB.Bo.PlayerChapterEventBO;

import java.util.ArrayList;
import java.util.List;

public class ChapterEvent_Dispatch extends _AChapterEvent
{
    public ChapterEvent_Dispatch(ChapterComponent _comp, PlayerChapterEventBO _bo, RefChapterEvent _ref)
    {
        super(_comp, _bo, _ref);
    }

    @Override
    public _IALProtocolStructure makeExtraData()
    {
        return null;
    }

    /**
     * 选择大臣
     * @param _heroList
     * @param _context
     * @return
     */
    public ResultOne<Integer> chooseHero(List<Long> _heroList, NPPlayerContext _context)
    {
        //查询配置
        _ARefChapterEvent detailRef = getEventRef().detailRef;
        RefChapterEventDispatch refDispatch = detailRef instanceof RefChapterEventDispatch ? ((RefChapterEventDispatch) detailRef) : null;
        if (refDispatch == null)
            return ResultOne.failed(CommErr.REF_NOT_FOUND);

        //检查大臣数量是否正确
        if (_heroList.size() > refDispatch.hero_num)
            return ResultOne.failed(CommErr.PARAM_ERROR);

        //检查奖励是否存在
        if (refDispatch.event_reward_list.isEmpty())
        {
            USLog.error(getUserData().getUSServer(), "ChapterEvent_Dispatch.chooseHero event_reward_list is empty, eventId:{}" , getEventRef().Id());
            return ResultOne.failed(CommErr.REF_NOT_FOUND);
        }

        //查询对应的条件配置
        List<RefChapterEventDispatchCond> conditionList = new ArrayList<>();
        for (Long conditionId : refDispatch.condition_id_list)
        {
            RefChapterEventDispatchCond refCondition = RefChapterEventDispatchCond.getMgr().get(conditionId);
            if (refCondition == null)
            {
                USLog.error(getUserData().getUSServer(), "ChapterEvent_Dispatch.chooseHero refCondition is null, conditionId:{}" , conditionId);
                return ResultOne.failed(CommErr.REF_NOT_FOUND);
            }

            conditionList.add(refCondition);
        }

        //计算符合条件的大臣数量
        int meetCount = 0;
        for (long heroId : _heroList)
        {
            HeroInfo heroInfo = getUserData().getHeroComponent().lookupHero(heroId);
            if (heroInfo == null)
                return ResultOne.failed(HeroErr.HERO_NOT_FOUND);

            //检查大臣是否满足条件
            boolean isMeet = true;
            for (RefChapterEventDispatchCond refCondition : conditionList)
            {
                if (!HeroConditionDealerMgr.IsEnable(refCondition.condition, heroInfo, null))
                {
                    isMeet = false;
                    break;
                }
            }

            if (isMeet)
                meetCount++;
        }

        //获取对应的奖励
        Long rewardId = refDispatch.event_reward_list.get(Math.min(meetCount, refDispatch.event_reward_list.size() - 1));
        RefChapterEventDispatchReward refReward = RefChapterEventDispatchReward.getMgr().get(rewardId);
        if (refReward == null)
        {
            USLog.error(getUserData().getUSServer(), "ChapterEvent_Dispatch.chooseHero refReward is null, rewardId:{}" , rewardId);
            return ResultOne.failed(CommErr.REF_NOT_FOUND);
        }

        getUserData().gainItemList(refReward.reward_item_list, _context);

        return ResultOne.succ(meetCount);
    }

    @Override
    public boolean isDone()
    {
        return true;
    }
}
