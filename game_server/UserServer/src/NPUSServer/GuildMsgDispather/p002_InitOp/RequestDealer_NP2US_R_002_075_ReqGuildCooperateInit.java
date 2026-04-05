package NPUSServer.GuildMsgDispather.p002_InitOp;

import Common.GuildEnum.EGuildPermissionType;
import GC2GS.p002_InitOp.GC2GS_002_075_ReqGuildCooperateInit;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserServer;

/**
 * 充值通知请求处理器
 * 处理PayCenter发送的充值成功通知
 */
public class RequestDealer_NP2US_R_002_075_ReqGuildCooperateInit extends _ATRequestDealer_GuildOp<GC2GS_002_075_ReqGuildCooperateInit>
{
    public RequestDealer_NP2US_R_002_075_ReqGuildCooperateInit(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_002_075_ReqGuildCooperateInit _msg)
    {
        //返回协议
        _committer.commitSucRes(_committer.getGuildInfo().getCooperateInfo().makeProto());
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return null;
    }
}