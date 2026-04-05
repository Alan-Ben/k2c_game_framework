package NPUSServer.NPUserMsgDispather.p003_FristTeamActivityOp;

import ALBasicProtocolPack._IALProtocolStructure;
import Common.ServerObj.ServerObj_GameLogicTestAdd;
import Common.ServerObj.ServerObj_GameLogicTestRet;
import GC2GS.p003_FristTeamActivityOp.GC2GS_003_002_ReqGameLogicRedirectTest;
import NPCommon.ErrMain.ActivityErr;
import NPCommon.ErrMain.CommErr;
import NPUSServer.CommonActivityMgr.Core.GameLogicDealer.ActivityGameLogicDealer;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ATNPUserMsgRedirectCommiter;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_003_FristTeamActivityOp;

public class MsgDealer_GC2GS_003_002_ReqGameLogicRedirectTest extends NPUserMsgDealer<GC2GS_003_002_ReqGameLogicRedirectTest>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_003_002_ReqGameLogicRedirectTest _msg)
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

        ServerObj_GameLogicTestAdd addInfo = new ServerObj_GameLogicTestAdd();
        addInfo.setParam1(activity.getStartTimeMs());

        dealer .dealMsgByRedirectCommiter(new _ATNPUserMsgRedirectCommiter(_commiter)
        {
            @Override
            protected _IALProtocolStructure _createNewTmpObj()
            {
                return new ServerObj_GameLogicTestRet();
            }

            @Override
            protected void _dealTmpCommitMsg(_ANPUSUserBasicMsgItem _commiter, _IALProtocolStructure _msg)
            {
                ServerObj_GameLogicTestRet ret = (ServerObj_GameLogicTestRet) _msg;

                _commiter.commitSucRes(US2GCWriter_003_FristTeamActivityOp.make_002_RetGameLogicRedirectTest(ret.getParam1()));
            }
        },
        userData.getCid(),
        0,
        _msg,
        addInfo,
        (_err, _msgItem) ->
        {
            _msgItem.commitFailRes(_err);
        });
    }
}
