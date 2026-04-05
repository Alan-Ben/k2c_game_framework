package NPUSServer.NPUserMsgDispather.p021_PlayerInfo;

import GC2GS.p021_PlayerInfo.GC2GS_021_020_ReqDealAnecdoteRewardEvent;
import NPCommon.ErrMain.AnecdoteErr;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.AnecdoteComp.Impl.AnecdoteEvent_Reward;
import NPUSServer.NPUSUserMgr.UserComp.AnecdoteComp._AAnecdoteEvent;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_021_PlayerInfo;

public class MsgDealer_GC2GS_021_020_ReqDealAnecdoteRewardEvent extends NPUserMsgDealer<GC2GS_021_020_ReqDealAnecdoteRewardEvent>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_021_020_ReqDealAnecdoteRewardEvent _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        //查询事件
        _AAnecdoteEvent event = userData.getAnecdoteComponent().lookupEvent(_msg.getInstanceId());
        AnecdoteEvent_Reward rewardEvent = event instanceof AnecdoteEvent_Reward ? ((AnecdoteEvent_Reward) event) : null;
        if (rewardEvent == null)
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
        Result result = rewardEvent.deal(context);
        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }

        //判断事件是否完结
        boolean isDone = rewardEvent.isDone();
        if (isDone)
        {
            userData.getAnecdoteComponent().disposeEvent(event, context);
        }

        _commiter.commitSucRes(US2GCWriter_021_PlayerInfo.make_020_RetDealAnecdoteRewardEvent(context));
    }
}
