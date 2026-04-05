package NPUSServer.GuildMsgDispather.p032_GuildOp;

import Common.GuildEnum.EGuildPermissionType;
import Common.GuildEnum.EGuildPositionType;
import GC2GS.p032_GuildOp.GC2GS_032_014_ReqGuildKickOutMember;
import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_PlayerCname;
import NPCommon.ErrMain.Result.Result;
import NPUSServer.Guild.Member.GuildMemberInfo;
import NPUSServer.Guild.Msg.GuildLogFunc;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_032_GuildOp;
import NPUSServer.NPUserServer;

/**
 * 踢出玩家
 */
public class RequestDealer_NP2US_R_032_014_ReqGuildKickOutMember extends _ATRequestDealer_GuildOp<GC2GS_032_014_ReqGuildKickOutMember>
{
    public RequestDealer_NP2US_R_032_014_ReqGuildKickOutMember(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_032_014_ReqGuildKickOutMember _msg)
    {
        //踢人处理
        Result result = _committer.getGuildInfo().getGuildMgr().removeMember(_committer.getGuildInfo(), _msg.getMemberId(), true);
        if (!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());
            return;
        }

        //结构附加信息
        GuildOp_PlayerCname addInfo = new GuildOp_PlayerCname();
        addInfo.readPackage(_committer.getAddInfo());

        //获取成员信息
        GuildMemberInfo memberInfo = _committer.getGuildInfo().getMemberMgr().lookup(_committer.getCid());

        GuildLogFunc.sendKickOutMemberLog(addInfo.getCname()
                , null == memberInfo ? EGuildPositionType.NONE : memberInfo.getPosition()
                , _msg.getMemberId(), _committer.getGuildInfo());

        _committer.commitSucRes(US2GCWriter_032_GuildOp.make_014_RetGuildKickOutMember());
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return EGuildPermissionType.KICK_OUT;
    }
}