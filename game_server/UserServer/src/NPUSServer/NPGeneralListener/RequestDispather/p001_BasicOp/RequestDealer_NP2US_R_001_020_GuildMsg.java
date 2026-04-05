package NPUSServer.NPGeneralListener.RequestDispather.p001_BasicOp;

import NP2US_R.p001_BasicOp.NP2US_R_001_020_GuildMsg;
import NPCommon.ErrMain.GuildErr;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.NPGeneralListener.RequestDispather._ABasicGeneralRequestDealer;
import NPUSServer.NPUserServer;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

public class RequestDealer_NP2US_R_001_020_GuildMsg extends _ABasicGeneralRequestDealer<NP2US_R_001_020_GuildMsg>
{
    public RequestDealer_NP2US_R_001_020_GuildMsg(NPUserServer _server) {
        super(_server);
    }

    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2US_R_001_020_GuildMsg _msg)
    {
        //查询对应公会对象
        GuildInfo guildInfo = getUSServer().getGuildMgr().lookupGuild(_msg.getGuildId());
        //不存在则报错
        if(null == guildInfo)
        {
            _committer.commitFailRes(GuildErr.GUILD_NOT_EXIST.getCode());
            return ;
        }

        //调用公会对象的消息处理操作
        getUSServer().getGuildRequestDispather().DealProtocol(
                new GuildMsgCommiter(_msg.getCid(), guildInfo, _committer, _msg.get_buffer_AddInfo(), false)
                , _msg.get_buffer_Msg());
    }
}
