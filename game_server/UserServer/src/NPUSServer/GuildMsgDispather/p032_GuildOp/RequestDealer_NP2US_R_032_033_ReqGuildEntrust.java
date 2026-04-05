package NPUSServer.GuildMsgDispather.p032_GuildOp;

import Common.GuildEnum.EGuildPermissionType;
import GC2GS.p032_GuildOp.GC2GS_032_033_ReqGuildEntrust;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserServer;

/**
 * 处理联盟委托
 */
public class RequestDealer_NP2US_R_032_033_ReqGuildEntrust extends _ATRequestDealer_GuildOp<GC2GS_032_033_ReqGuildEntrust>
{
    public RequestDealer_NP2US_R_032_033_ReqGuildEntrust(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_032_033_ReqGuildEntrust _msg)
    {
        GuildInfo guildInfo = _committer.getGuildInfo();

        guildInfo.dealEntrust(_committer);
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return null;
    }
}
