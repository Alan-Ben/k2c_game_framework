package NPUSServer.NPUserMsgDispather.p021_PlayerInfo;

import GC2GS.p021_PlayerInfo.GC2GS_021_021_ReqDrawAnecdoteEarningsProcessReward;
import NPCommon.ErrMain.AnecdoteErr;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.AnecdoteComp.Impl.AnecdoteEvent_Earnings;
import NPUSServer.NPUSUserMgr.UserComp.AnecdoteComp._AAnecdoteEvent;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_021_PlayerInfo;

public class MsgDealer_GC2GS_021_021_ReqDrawAnecdoteEarningsProcessReward extends NPUserMsgDealer<GC2GS_021_021_ReqDrawAnecdoteEarningsProcessReward>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_021_021_ReqDrawAnecdoteEarningsProcessReward _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        //查询事件
        _AAnecdoteEvent event = userData.getAnecdoteComponent().lookupEvent(_msg.getInstanceId());
        AnecdoteEvent_Earnings earningsEvent = event instanceof AnecdoteEvent_Earnings ? ((AnecdoteEvent_Earnings) event) : null;
        if (earningsEvent == null)
        {
            _commiter.commitFailRes(AnecdoteErr.EVENT_NOT_EXIST.getCode());
            return;
        }

        //判断是否达成处理条件
        boolean canDeal = NPPlayerConditionDealerMgr.IsEnable(event.getEventRef().show_condition, userData, null);
        if (!canDeal)
        {
            _commiter.commitFailRes(CommErr.CONDITION_NOT_ENABLE.getCode());
            return;
        }

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.DEAL_ANECDOTE_CHOICE_EVENT);
        Result result = earningsEvent.drawProcessReward(context);
        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }

        //判断事件是否完结
        boolean isDone = earningsEvent.isDone();
        if (isDone)
        {
            userData.getAnecdoteComponent().disposeEvent(event, context);
        }

        _commiter.commitSucRes(US2GCWriter_021_PlayerInfo.make_021_RetDrawAnecdoteEarningsProcessReward(context));
    }
}
