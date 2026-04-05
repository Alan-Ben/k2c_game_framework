package NPUSServer.GuildMsgDispather.p042_GuildRelatedOp;

import Common.GuildEnum.EGuildPermissionType;
import GC2GS.p042_GuildRelatedOp.GC2GS_042_004_ReqMarsHelpDealedList;
import NPCommon.ErrMain.GuildErr;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.Guild.MarsHelp.GuildMarsHelpInfo;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_042_GuildRelatedOp;
import NPUSServer.NPUserServer;

/**
 * 获取联盟副本信息
 */
public class RequestDealer_NP2US_R_042_004_ReqMarsHelpDealedList extends _ATRequestDealer_GuildOp<GC2GS_042_004_ReqMarsHelpDealedList>
{
    public RequestDealer_NP2US_R_042_004_ReqMarsHelpDealedList(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_042_004_ReqMarsHelpDealedList _msg)
    {
        GuildInfo guildInfo = _committer.getGuildInfo();
        if(null == guildInfo)
        {
            _committer.commitFailRes(GuildErr.GUILD_NOT_EXIST.getCode());
            return;
        }

        //构造求助数据
        GuildMarsHelpInfo helpInfo = guildInfo.getMarsHelpMgr().lookup(_msg.getId());
        if(null == helpInfo)
        {
            _committer.commitFailRes(GuildErr.GUILD_MARS_HELP_SEND_FAIL.getCode());
            return;
        }

        _committer.commitSucRes(US2GCWriter_042_GuildRelatedOp.make_004_RetMarsHelpDealedList(helpInfo));
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return null;
    }

}
