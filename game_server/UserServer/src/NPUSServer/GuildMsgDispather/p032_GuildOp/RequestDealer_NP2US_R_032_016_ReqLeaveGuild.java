package NPUSServer.GuildMsgDispather.p032_GuildOp;

import Common.GuildEnum.EGuildPermissionType;
import GC2GS.p032_GuildOp.GC2GS_032_016_ReqLeaveGuild;
import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_PlayerCname;
import NPCommon.ErrMain.Result.Result;
import NPUSServer.Guild.Msg.GuildLogFunc;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_032_GuildOp;
import NPUSServer.NPUserServer;

/**
 * 离开联盟
 */
public class RequestDealer_NP2US_R_032_016_ReqLeaveGuild extends _ATRequestDealer_GuildOp<GC2GS_032_016_ReqLeaveGuild>
{
    public RequestDealer_NP2US_R_032_016_ReqLeaveGuild(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_032_016_ReqLeaveGuild _msg)
    {
        //退盟处理
        Result result = _committer.getGuildInfo().getGuildMgr().removeMember(_committer.getGuildInfo(), _committer.getCid(), false);
        if (!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());
            return;
        }

        //结构附加信息
        GuildOp_PlayerCname addInfo = new GuildOp_PlayerCname();
        addInfo.readPackage(_committer.getAddInfo());

        GuildLogFunc.sendMemberLeaveLog(addInfo.getCname(), _committer.getGuildInfo());

        _committer.commitSucRes(US2GCWriter_032_GuildOp.make_016_RetLeaveGuild());
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return EGuildPermissionType.LEAVE_GUILD;
    }
}