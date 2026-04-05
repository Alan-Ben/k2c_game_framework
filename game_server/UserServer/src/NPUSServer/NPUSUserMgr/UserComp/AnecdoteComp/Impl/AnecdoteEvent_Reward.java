package NPUSServer.NPUSUserMgr.UserComp.AnecdoteComp.Impl;

import ALBasicProtocolPack._IALProtocolStructure;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPGameRes.Refs.Anecdote.RefAnecdoteEvent;
import NPGameRes.Refs.Anecdote.RefAnecdoteEventReward;
import NPGameRes.Refs.Anecdote._ARefAnecdoteEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.AnecdoteComp.AnecdoteComponent;
import NPUSServer.NPUSUserMgr.UserComp.AnecdoteComp._AAnecdoteEvent;
import USDB.Bo.PlayerAnecdoteEventBO;

public class AnecdoteEvent_Reward extends _AAnecdoteEvent
{
    public AnecdoteEvent_Reward(AnecdoteComponent _comp, PlayerAnecdoteEventBO _bo, RefAnecdoteEvent _ref)
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
        _ARefAnecdoteEvent detailRef = getEventRef().detailRef;
        RefAnecdoteEventReward refReward = detailRef instanceof RefAnecdoteEventReward ? ((RefAnecdoteEventReward) detailRef) : null;
        if (refReward == null)
            return CommErr.REF_NOT_FOUND;

        getUserData().gainItem(refReward.reward_item, _context);

        return Result.SUCC;
    }

    @Override
    public boolean isDone()
    {
        return true;
    }
}
