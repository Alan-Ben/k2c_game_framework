package NPUSServer.NPUserMsgDispather.p002_InitOp;

import Common.GuildCooperateObj.GuildCooperate_Info;
import GC2GS.p002_InitOp.GC2GS_002_075_ReqGuildCooperateInit;
import NPCommon.Util.CallBack._ICallBackIntT;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter._ATGuildUserMsgRedirectCommiter;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_002_InitOp;

public class MsgDealer_GC2GS_002_075_ReqGuildCooperateInit extends NPUserMsgDealer<GC2GS_002_075_ReqGuildCooperateInit>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_002_075_ReqGuildCooperateInit _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        //无公会直接返回
        if(userData.getGuildComponent().getGuildId() <= 0)
        {
            _commiter.commitSucRes(US2GCWriter_002_InitOp.make_075_RetGuildCooperateInit(userData, new GuildCooperate_Info()));
            return ;
        }

        //转化为统一跨服消息进行处理
        getUSServer().dealGuildMsgByRedirectCommiter(
                new _ATGuildUserMsgRedirectCommiter<GuildCooperate_Info>(_commiter) {
                    @Override
                    protected GuildCooperate_Info _createNewTmpObj() {
                        return new GuildCooperate_Info();
                    }

                    @Override
                    protected void _dealTmpCommitMsg(_ANPUSUserBasicMsgItem _commiter, GuildCooperate_Info _retMsg) {

                        _commiter.commitSucRes(US2GCWriter_002_InitOp.make_075_RetGuildCooperateInit(userData, _retMsg));
                    }
                },
                _commiter.getUserData().getCid(),
                _commiter.getUserData().getGuildComponent().getGuildId(),
                _msg,
                null,
                new _ICallBackIntT<_ANPUSUserBasicMsgItem>() {
                    @Override
                    public void onRunOver(int _retValue, _ANPUSUserBasicMsgItem _commiter)
                    {
                        _commiter.commitSucRes(US2GCWriter_002_InitOp.make_075_RetGuildCooperateInit(userData, new GuildCooperate_Info()));
                    }
                }
        );
    }
}
