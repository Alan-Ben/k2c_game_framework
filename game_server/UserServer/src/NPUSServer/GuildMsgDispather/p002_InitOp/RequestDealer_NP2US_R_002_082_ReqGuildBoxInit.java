package NPUSServer.GuildMsgDispather.p002_InitOp;

import Common.GuildEnum.EGuildPermissionType;
import GC2GS.p002_InitOp.GC2GS_002_082_ReqGuildBoxInit;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_002_InitOp;
import NPUSServer.NPUserServer;

/**
 * 充值通知请求处理器
 * 处理PayCenter发送的充值成功通知
 */
public class RequestDealer_NP2US_R_002_082_ReqGuildBoxInit extends _ATRequestDealer_GuildOp<GC2GS_002_082_ReqGuildBoxInit>
{
    public RequestDealer_NP2US_R_002_082_ReqGuildBoxInit(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_002_082_ReqGuildBoxInit _msg)
    {
        //返回协议
        _committer.commitSucRes(US2GCWriter_002_InitOp.make_082_RetGuildBoxInit_onlyGuild(_committer.getCid(), _committer.getGuildInfo()));
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return null;
    }
}