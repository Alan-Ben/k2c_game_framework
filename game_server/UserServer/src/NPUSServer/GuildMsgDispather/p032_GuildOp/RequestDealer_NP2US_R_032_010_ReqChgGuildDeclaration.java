package NPUSServer.GuildMsgDispather.p032_GuildOp;

import Common.GuildEnum.EGuildPermissionType;
import GC2GS.p032_GuildOp.GC2GS_032_010_ReqChgGuildDeclaration;
import NPCommon.ErrMain.Result.Result;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_032_GuildOp;
import NPUSServer.NPUserServer;

/**
 * 修改联盟说明
 */
public class RequestDealer_NP2US_R_032_010_ReqChgGuildDeclaration extends _ATRequestDealer_GuildOp<GC2GS_032_010_ReqChgGuildDeclaration>
{
    public RequestDealer_NP2US_R_032_010_ReqChgGuildDeclaration(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_032_010_ReqChgGuildDeclaration _msg)
    {
        //修改公会宣言
        Result result = _committer.getGuildInfo().chgDeclaration(_msg.getDeclaration());
        if (!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());
            return;
        }

        _committer.commitSucRes(US2GCWriter_032_GuildOp.make_010_RetChgGuildDeclaration());
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return EGuildPermissionType.CHANGE_GUILD_INFO;
    }
}