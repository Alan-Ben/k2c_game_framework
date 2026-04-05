package NPUSServer.GuildMsgDispather.p002_InitOp;

import Common.GuildEnum.EGuildPermissionType;
import GC2GS.p002_InitOp.GC2GS_002_063_ReqGuildInit;
import GS2GC.p002_InitOp.GS2GC_002_063_RetGuildInit;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserServer;

/**
 * 充值通知请求处理器
 * 处理PayCenter发送的充值成功通知
 */
public class RequestDealer_NP2US_R_002_063_ReqGuildInit extends _ATRequestDealer_GuildOp<GC2GS_002_063_ReqGuildInit>
{
    public RequestDealer_NP2US_R_002_063_ReqGuildInit(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_002_063_ReqGuildInit _msg)
    {
        //结构附加信息
        GS2GC_002_063_RetGuildInit guildInfo = new GS2GC_002_063_RetGuildInit();
        guildInfo.readPackage(_committer.getAddInfo());

        //构造详情信息
        guildInfo.setGuildInfo(_committer.getGuildInfo().makeDetailInfo(_committer.getCid()));

        //返回协议
        _committer.commitSucRes(guildInfo);
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return null;
    }
}