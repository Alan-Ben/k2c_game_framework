package NPUSServer.GuildMsgDispather.p002_InitOp;

import Common.GuildEnum.EGuildPermissionType;
import GC2GS.p002_InitOp.GC2GS_002_071_ReqGuildDungeonInit;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_002_InitOp;
import NPUSServer.NPUserServer;

/**
 * 充值通知请求处理器
 * 处理PayCenter发送的充值成功通知
 */
public class RequestDealer_NP2US_R_002_071_ReqGuildDungeonInit extends _ATRequestDealer_GuildOp<GC2GS_002_071_ReqGuildDungeonInit>
{
    public RequestDealer_NP2US_R_002_071_ReqGuildDungeonInit(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_002_071_ReqGuildDungeonInit _msg)
    {
        //返回协议
        _committer.commitSucRes(US2GCWriter_002_InitOp.make_071_RetGuildDungeonInit_onlyGuild(_committer.getCid(), _committer.getGuildInfo()));
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return null;
    }
}