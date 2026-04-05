package NPUSServer.NPUSUserMgr.UserComp.ChapterComp.Event;

import ALBasicProtocolPack._IALProtocolStructure;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPGameRes.Refs.Chapter.Event.RefChapterEventReward;
import NPGameRes.Refs.Chapter.Event._ARefChapterEvent;
import NPGameRes.Refs.Chapter.RefChapterEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.ChapterComp.ChapterComponent;
import USDB.Bo.PlayerChapterEventBO;

public class ChapterEvent_Reward extends _AChapterEvent
{
    public ChapterEvent_Reward(ChapterComponent _comp, PlayerChapterEventBO _bo, RefChapterEvent _ref)
    {
        super(_comp, _bo, _ref);
    }

    @Override
    public _IALProtocolStructure makeExtraData()
    {
        return null;
    }

    public Result deal(NPPlayerContext _context)
    {
        //查询配置
        _ARefChapterEvent detailRef = getEventRef().detailRef;
        RefChapterEventReward refReward = detailRef instanceof RefChapterEventReward ? ((RefChapterEventReward) detailRef) : null;
        if (refReward == null)
            return CommErr.REF_NOT_FOUND;

        getUserData().gainItemList(refReward.reward_item, _context);

        return Result.SUCC;
    }

    @Override
    public boolean isDone()
    {
        return true;
    }
}
