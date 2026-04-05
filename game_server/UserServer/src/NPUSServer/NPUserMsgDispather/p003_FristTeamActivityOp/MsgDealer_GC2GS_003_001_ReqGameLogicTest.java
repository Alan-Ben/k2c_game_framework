package NPUSServer.NPUserMsgDispather.p003_FristTeamActivityOp;

import GC2GS.p003_FristTeamActivityOp.GC2GS_003_001_ReqGameLogicTest;
import NPCommon.ErrMain.ActivityErr;
import NPCommon.ErrMain.CommErr;
import NPUSServer.CommonActivityMgr.Core.GameLogicDealer.ActivityGameLogicDealer;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;

public class MsgDealer_GC2GS_003_001_ReqGameLogicTest extends NPUserMsgDealer<GC2GS_003_001_ReqGameLogicTest>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_003_001_ReqGameLogicTest _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        _AActivityBase activity = userData.getUSServer().getCommActivityMgr().lookupActivity(_msg.getInstanceId());
        if(null == activity)
        {
            _commiter.commitFailRes(ActivityErr.ACTIVITY_NOT_FOUND.getCode());
            return;
        }

        ActivityGameLogicDealer dealer = activity.getGameLogicDealer();
        if(null == dealer)
        {
            _commiter.commitFailRes(CommErr.OBJ_ERR.getCode());
            return;
        }

        dealer.dealMsg(_commiter, userData.getCid(), 0, _msg, null , (_err, _msgItem) ->
        {
            _msgItem.commitFailRes(_err);
        });
    }
}
