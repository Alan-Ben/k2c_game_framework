package NPUSServer.GuildMsgDispather.p042_GuildRelatedOp;

import Common.GuildEnum.EGuildPermissionType;
import GC2GS.p042_GuildRelatedOp.GC2GS_042_008_ReqSetGuildBoxShareAnonymous;
import NPCommon.ErrMain.GuildErr;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.Guild.Member.GuildMemberInfo;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_042_GuildRelatedOp;
import NPUSServer.NPUserServer;

/**
 * 获取联盟副本信息
 */
public class RequestDealer_NP2US_R_042_008_ReqSetGuildBoxShareAnonymous extends _ATRequestDealer_GuildOp<GC2GS_042_008_ReqSetGuildBoxShareAnonymous>
{
    public RequestDealer_NP2US_R_042_008_ReqSetGuildBoxShareAnonymous(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_042_008_ReqSetGuildBoxShareAnonymous _msg)
    {
        GuildInfo guildInfo = _committer.getGuildInfo();
        if(null == guildInfo)
        {
            _committer.commitFailRes(GuildErr.GUILD_NOT_EXIST.getCode());
            return;
        }

        GuildMemberInfo member = guildInfo.getMemberMgr().lookup(_committer.getCid());
        if(null == member)
        {
            _committer.commitFailRes(GuildErr.GUILD_NOT_EXIST.getCode());
            return;
        }

        //设置分享宝箱匿名
        member.saveIsGuildBoxShareAnonymous(_msg.getIsAnonymous());

        _committer.commitSucRes(US2GCWriter_042_GuildRelatedOp.make_008_RetSetGuildBoxShareAnonymous());
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return null;
    }

}
