package NPUSServer.GuildMsgDispather.p032_GuildOp;

import Common.GuildEnum.EGuildPermissionType;
import Common.GuildObj.Guild_MemberContributeInfo;
import GC2GS.p032_GuildOp.GC2GS_032_019_ReqMemberContribute;
import NPCommon.ErrMain.GuildErr;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_032_GuildOp;
import NPUSServer.NPUserServer;

/**
 * 申请联盟成员信息
 */
public class RequestDealer_NP2US_R_032_019_ReqMemberContribute extends _ATRequestDealer_GuildOp<GC2GS_032_019_ReqMemberContribute>
{
    public RequestDealer_NP2US_R_032_019_ReqMemberContribute(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_032_019_ReqMemberContribute _msg)
    {
        Guild_MemberContributeInfo contributeInfo = _committer.getGuildInfo().getMemberMgr().getMemberContributeInfo(_msg.getMemberId());
        if (contributeInfo == null)
        {
            _committer.commitFailRes(GuildErr.MEMBER_NOT_FOUND.getCode());
            return;
        }

        _committer.commitSucRes(US2GCWriter_032_GuildOp.make_019_RetMemberContribute(contributeInfo));
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return null;
    }
}
