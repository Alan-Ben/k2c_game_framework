package NPUSServer.GuildMsgDispather.p032_GuildOp;

import Common.GuildEnum.EGuildPermissionType;
import Common.GuildEnum.EGuildPositionType;
import GC2GS.p032_GuildOp.GC2GS_032_008_ReqChgGuildFlag;
import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_PlayerCname;
import NPCommon.ErrMain.Result.Result;
import NPUSServer.Guild.Member.GuildMemberInfo;
import NPUSServer.Guild.Msg.GuildLogFunc;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_032_GuildOp;
import NPUSServer.NPUserServer;

/**
 * 修改联盟标记
 */
public class RequestDealer_NP2US_R_032_008_ReqChgGuildFlag extends _ATRequestDealer_GuildOp<GC2GS_032_008_ReqChgGuildFlag>
{
    public RequestDealer_NP2US_R_032_008_ReqChgGuildFlag(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_032_008_ReqChgGuildFlag _msg)
    {
        //修改公会旗帜
        Result result = _committer.getGuildInfo().chgFlag(_msg.getFlagId());
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

        GuildLogFunc.sendGuildFlagChangeLog(addInfo.getCname(),
                null == memberInfo ? EGuildPositionType.NONE : memberInfo.getPosition(), _committer.getGuildInfo());

        _committer.commitSucRes(US2GCWriter_032_GuildOp.make_008_RetChgGuildFlag());
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return EGuildPermissionType.CHANGE_GUILD_INFO;
    }
}