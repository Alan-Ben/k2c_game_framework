package NPUSServer.GuildMsgDispather.p032_GuildOp;

import Common.GuildEnum.EGuildPermissionType;
import Common.GuildEnum.EGuildPositionType;
import GC2GS.p032_GuildOp.GC2GS_032_009_ReqChgGuildName;
import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_PlayerCname;
import NPCommon.ErrMain.Result.Result;
import NPUSServer.Guild.Member.GuildMemberInfo;
import NPUSServer.Guild.Msg.GuildLogFunc;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_032_GuildOp;
import NPUSServer.NPUserServer;

/**
 * 修改联盟名称
 */
public class RequestDealer_NP2US_R_032_009_ReqChgGuildName extends _ATRequestDealer_GuildOp<GC2GS_032_009_ReqChgGuildName>
{
    public RequestDealer_NP2US_R_032_009_ReqChgGuildName(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_032_009_ReqChgGuildName _msg)
    {
        //修改公会名称
        Result result = _committer.getGuildInfo().chgName(_msg.getName(), _msg.getSimpleName());
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

        GuildLogFunc.sendGuildRenameLog(addInfo.getCname(),
                null == memberInfo ? EGuildPositionType.NONE : memberInfo.getPosition(), _committer.getGuildInfo());

        _committer.commitSucRes(US2GCWriter_032_GuildOp.make_009_RetChgGuildName());
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return EGuildPermissionType.CHANGE_GUILD_INFO;
    }
}