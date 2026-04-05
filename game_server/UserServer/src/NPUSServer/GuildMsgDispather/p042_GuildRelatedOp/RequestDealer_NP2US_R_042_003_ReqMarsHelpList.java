package NPUSServer.GuildMsgDispather.p042_GuildRelatedOp;

import Common.GuildEnum.EGuildPermissionType;
import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_RetLongListInfo;
import GC2GS.p042_GuildRelatedOp.GC2GS_042_003_ReqMarsHelpList;
import NPCommon.ErrMain.GuildErr;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_042_GuildRelatedOp;
import NPUSServer.NPUserServer;

/**
 * 获取联盟副本信息
 */
public class RequestDealer_NP2US_R_042_003_ReqMarsHelpList extends _ATRequestDealer_GuildOp<GC2GS_042_003_ReqMarsHelpList>
{
    public RequestDealer_NP2US_R_042_003_ReqMarsHelpList(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_042_003_ReqMarsHelpList _msg)
    {
        GuildInfo guildInfo = _committer.getGuildInfo();
        if(null == guildInfo)
        {
            _committer.commitFailRes(GuildErr.GUILD_NOT_EXIST.getCode());
            return;
        }

        //结构附加信息
        GuildOp_RetLongListInfo addInfo = new GuildOp_RetLongListInfo();
        addInfo.readPackage(_committer.getAddInfo());

        //构造数据返回
        _committer.commitSucRes(US2GCWriter_042_GuildRelatedOp.make_003_RetMarsHelpList(guildInfo, _committer.getCid(), addInfo.getList()));
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return null;
    }

}
