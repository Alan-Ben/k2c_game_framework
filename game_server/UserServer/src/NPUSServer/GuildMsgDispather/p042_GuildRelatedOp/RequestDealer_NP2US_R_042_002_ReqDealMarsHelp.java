package NPUSServer.GuildMsgDispather.p042_GuildRelatedOp;

import Common.GuildEnum.EGuildPermissionType;
import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_RetIntInfo;
import GC2GS.p042_GuildRelatedOp.GC2GS_042_002_ReqDealMarsHelp;
import NPCommon.ErrMain.GuildErr;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserServer;

/**
 * 获取联盟副本信息
 */
public class RequestDealer_NP2US_R_042_002_ReqDealMarsHelp extends _ATRequestDealer_GuildOp<GC2GS_042_002_ReqDealMarsHelp>
{
    public RequestDealer_NP2US_R_042_002_ReqDealMarsHelp(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_042_002_ReqDealMarsHelp _msg)
    {
        GuildInfo guildInfo = _committer.getGuildInfo();
        if(null == guildInfo)
        {
            _committer.commitFailRes(GuildErr.GUILD_NOT_EXIST.getCode());
            return;
        }

        //处理帮助请求
        int dealCount = guildInfo.getMarsHelpMgr().cmdDealHelp(_committer.getCid(), false);

        GuildOp_RetIntInfo retInfo = new GuildOp_RetIntInfo();
        retInfo.setNum(dealCount);

        _committer.commitSucRes(retInfo);
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return null;
    }

}
