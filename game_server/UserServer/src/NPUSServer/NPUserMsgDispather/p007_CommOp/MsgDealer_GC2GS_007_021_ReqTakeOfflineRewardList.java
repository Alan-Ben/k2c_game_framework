package NPUSServer.NPUserMsgDispather.p007_CommOp;

import GC2GS.p007_CommOp.GC2GS_007_021_ReqTakeOfflineRewardList;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineRewardTakeResult;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_007_CommOp;
import USLOGDB.OptBo.Opt007021OfflineRewardTakeAllBO;

import java.util.ArrayList;

public class MsgDealer_GC2GS_007_021_ReqTakeOfflineRewardList extends NPUserMsgDealer<GC2GS_007_021_ReqTakeOfflineRewardList>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_007_021_ReqTakeOfflineRewardList _msg)
    {
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.OFFLINE_REWARD_LIST_TAKE);

        ArrayList<Long> sucIdList = new ArrayList<>();
        for (int i = 0; i < _msg.getIdList().size(); i++)
        {
        	OfflineRewardTakeResult result = _commiter.getUserData().getOfflineRewardComponent().takeReward(_msg.getIdList().get(i), context);
            if (!result.getResult().isSucc())
                continue;

            sucIdList.add(_msg.getIdList().get(i));
        }

        _commiter.commitSucRes(US2GCWriter_007_CommOp.make_021_RetTakeOfflineRewardList(sucIdList));

        //操作日志
        Opt007021OfflineRewardTakeAllBO optBo = new Opt007021OfflineRewardTakeAllBO();
        optBo.setInstanceIdList(getUSServer().getBM(), CommonFunc.list2String(_msg.getIdList(), ','));
        _commiter.getUserData().logEvent(optBo, context);
    }
}
