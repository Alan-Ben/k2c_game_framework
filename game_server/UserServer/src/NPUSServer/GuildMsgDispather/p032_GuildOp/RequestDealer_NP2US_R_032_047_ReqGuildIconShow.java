package NPUSServer.GuildMsgDispather.p032_GuildOp;

import GC2GS.p032_GuildOp.GC2GS_032_047_ReqGuildIconShow;
import NPCommon.ErrMain.GuildErr;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.NPGeneralListener.RequestDispather._ABasicGeneralRequestDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_032_GuildOp;
import NPUSServer.NPUserServer;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

/**
 * 获取联盟图标
 */
public class RequestDealer_NP2US_R_032_047_ReqGuildIconShow extends _ABasicGeneralRequestDealer<GC2GS_032_047_ReqGuildIconShow>
{
    public RequestDealer_NP2US_R_032_047_ReqGuildIconShow(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _committer, GC2GS_032_047_ReqGuildIconShow _msg)
    {
        GuildInfo guildInfo = getUSServer().getGuildMgr().lookupGuild(_msg.getGuildId());
        if (null == guildInfo)
        {
            _committer.commitFailRes(GuildErr.GUILD_NOT_EXIST.getCode());
            return;
        }

        _committer.commitSucRes(US2GCWriter_032_GuildOp.make_047_RetGuildIconShow(guildInfo.makeIconShowInfo()));
    }
}