package NPUSServer.NPUSUserMgr.UserComp.AnecdoteComp.Impl;

import ALBasicProtocolPack._IALProtocolStructure;
import Common.AnecdoteObj.Anecdote_EventExtraData_Earnings;
import NPCommon.ErrMain.AnecdoteErr;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPGameRes.Refs.Anecdote.RefAnecdoteEvent;
import NPGameRes.Refs.Anecdote.RefAnecdoteEventEarnings;
import NPGameRes.Refs.Anecdote._ARefAnecdoteEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.AnecdoteComp.AnecdoteComponent;
import NPUSServer.NPUSUserMgr.UserComp.AnecdoteComp._AAnecdoteEvent;
import USDB.Bo.PlayerAnecdoteEventBO;

import java.nio.ByteBuffer;

public class AnecdoteEvent_Earnings extends _AAnecdoteEvent
{
    private Anecdote_EventExtraData_Earnings _m_extraData;

    public AnecdoteEvent_Earnings(AnecdoteComponent _comp, PlayerAnecdoteEventBO _bo, RefAnecdoteEvent _ref)
    {
        super(_comp, _bo, _ref);

        if (_bo.getExtraData() != null)
        {
            _m_extraData = new Anecdote_EventExtraData_Earnings();
            _m_extraData.readPackage(ByteBuffer.wrap(_bo.getExtraData()));
        }
    }

    @Override
    public _IALProtocolStructure makeExtraData()
    {
        if (_m_extraData == null)
            _m_extraData = new Anecdote_EventExtraData_Earnings();

        return _m_extraData;
    }

    /**
     * 领取过程奖励
     * @param _context
     * @return
     */
    public Result drawProcessReward(NPPlayerContext _context)
    {
        //检查是否已经领取过
        if (_m_extraData.getHadDrawFirstReward())
            return AnecdoteErr.EARNINGS_HAD_DRAW_FIRST_REWARD;

        //查询配置
        _ARefAnecdoteEvent detailRef = getEventRef().detailRef;
        RefAnecdoteEventEarnings refEarnings = detailRef instanceof RefAnecdoteEventEarnings ? ((RefAnecdoteEventEarnings) detailRef) : null;
        if (refEarnings == null)
            return CommErr.REF_NOT_FOUND;

        //领取奖励
        _m_extraData.setHadDrawFirstReward(true);
        onDataChg(false);

        getUserData().gainItem(refEarnings.reward_item, _context);

        return Result.SUCC;
    }

    /**
     * 领取过程奖励
     * @param _context
     * @return
     */
    public Result drawFinalReward(NPPlayerContext _context)
    {
        //检查是否已经领取过
        if (!_m_extraData.getHadDrawFirstReward())
            return AnecdoteErr.EARNINGS_DIDNT_DRAW_FIRST_REWARD;

        //检查是否已经领取过
        if (_m_extraData.getHadDrawFinalReward())
            return AnecdoteErr.EARNINGS_HAD_DRAW_FINAL_REWARD;

        //查询配置
        _ARefAnecdoteEvent detailRef = getEventRef().detailRef;
        RefAnecdoteEventEarnings refEarnings = detailRef instanceof RefAnecdoteEventEarnings ? ((RefAnecdoteEventEarnings) detailRef) : null;
        if (refEarnings == null)
            return CommErr.REF_NOT_FOUND;

        if (getUserData().getPlayerComponent().getEarnings() < refEarnings.earnings)
            return AnecdoteErr.EVENT_EARNINGS_NOT_REACH;

        //领取奖励
        _m_extraData.setHadDrawFinalReward(true);
        onDataChg(false);

        getUserData().gainItem(refEarnings.result_reward_item, _context);

        return Result.SUCC;
    }

    @Override
    public boolean isDone()
    {
        return _m_extraData != null && _m_extraData.getHadDrawFinalReward();
    }
}
