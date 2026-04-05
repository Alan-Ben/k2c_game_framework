package NPUSServer.NPUserMsgDispather.p002_InitOp;

import GC2GS.p002_InitOp.GC2GS_002_071_ReqGuildDungeonInit;
import GS2GC.p002_InitOp.GS2GC_002_071_RetGuildDungeonInit;
import NPCommon.Util.CallBack._ICallBackIntT;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter._ATGuildUserMsgRedirectCommiter;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_002_InitOp;

public class MsgDealer_GC2GS_002_071_ReqGuildDungeonInit extends NPUserMsgDealer<GC2GS_002_071_ReqGuildDungeonInit>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_002_071_ReqGuildDungeonInit _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        //无公会直接返回
        if(userData.getGuildComponent().getGuildId() <= 0)
        {
            _commiter.commitSucRes(US2GCWriter_002_InitOp.make_071_RetGuildDungeonInit_noGuild(userData));
            return ;
        }

        //转化为统一跨服消息进行处理
        getUSServer().dealGuildMsgByRedirectCommiter(
                new _ATGuildUserMsgRedirectCommiter<GS2GC_002_071_RetGuildDungeonInit>(_commiter) {
                    @Override
                    protected GS2GC_002_071_RetGuildDungeonInit _createNewTmpObj() {
                        return new GS2GC_002_071_RetGuildDungeonInit();
                    }

                    @Override
                    protected void _dealTmpCommitMsg(_ANPUSUserBasicMsgItem _commiter, GS2GC_002_071_RetGuildDungeonInit _retMsg) {
                        //玩家数据
                        userData.getGuildDungeonComponent().makeFightHeroList(_retMsg.getFightHeroList());

                        //提交协议
                        _commiter.commitSucRes(_retMsg);
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
                        _commiter.commitSucRes(US2GCWriter_002_InitOp.make_071_RetGuildDungeonInit_noGuild(userData));
                    }
                }
        );
    }
}
