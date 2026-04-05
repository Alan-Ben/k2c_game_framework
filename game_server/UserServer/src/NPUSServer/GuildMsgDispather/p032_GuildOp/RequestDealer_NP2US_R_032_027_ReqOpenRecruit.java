package NPUSServer.GuildMsgDispather.p032_GuildOp;

import Common.GuildEnum.EGuildPermissionType;
import GC2GS.p032_GuildOp.GC2GS_032_027_ReqOpenRecruit;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserServer;

/**
 * 发送公开招募信息
 */
public class RequestDealer_NP2US_R_032_027_ReqOpenRecruit extends _ATRequestDealer_GuildOp<GC2GS_032_027_ReqOpenRecruit>
{
    public RequestDealer_NP2US_R_032_027_ReqOpenRecruit(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_032_027_ReqOpenRecruit _msg)
    {
        GuildInfo guildInfo = _committer.getGuildInfo();

        //在处理中返回结果
        guildInfo.tryOpenRecruit(_committer);
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return EGuildPermissionType.OPEN_RECRUITMENT;
    }
}
