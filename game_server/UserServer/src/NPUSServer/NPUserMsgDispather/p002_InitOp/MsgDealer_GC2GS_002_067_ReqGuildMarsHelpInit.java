package NPUSServer.NPUserMsgDispather.p002_InitOp;

import GC2GS.p002_InitOp.GC2GS_002_067_ReqGuildMarsHelpInit;
import NPCommon.Util.CallBack._ICallBackIntT;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_002_InitOp;

public class MsgDealer_GC2GS_002_067_ReqGuildMarsHelpInit extends NPUserMsgDealer<GC2GS_002_067_ReqGuildMarsHelpInit>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_002_067_ReqGuildMarsHelpInit _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        //无公会直接返回
        if(userData.getGuildComponent().getGuildId() <= 0)
        {
            _commiter.commitSucRes(US2GCWriter_002_InitOp.make_067_RetGuildMarsHelpInit(userData.getCid(), null));
            return ;
        }

        //转化为统一跨服消息进行处理
        getUSServer().dealGuildMsg(
                _commiter,
                _commiter.getUserData().getCid(),
                _commiter.getUserData().getGuildComponent().getGuildId(),
                _msg,
                null,
                new _ICallBackIntT<_ANPUSUserBasicMsgItem>() {
                    @Override
                    public void onRunOver(int _retValue, _ANPUSUserBasicMsgItem _commiter)
                    {
                        _commiter.commitSucRes(US2GCWriter_002_InitOp.make_067_RetGuildMarsHelpInit(userData.getCid(), null));
                    }
                }
                );
    }
}
