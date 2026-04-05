package NPUSServer.GuildMsgDispather.p032_GuildOp;

import Common.GuildCooperateObj.GuildCooperate_AttackLog;
import Common.GuildEnum.EGuildPermissionType;
import GC2GS.p032_GuildOp.GC2GS_032_041_ReqGuildCooperateAttackLogList;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_032_GuildOp;
import NPUSServer.NPUserServer;

import java.util.List;

/**
 * 获取联盟据点攻击日志
 */
public class RequestDealer_NP2US_R_032_041_ReqGuildCooperateAttackLogList extends _ATRequestDealer_GuildOp<GC2GS_032_041_ReqGuildCooperateAttackLogList>
{
    public RequestDealer_NP2US_R_032_041_ReqGuildCooperateAttackLogList(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_032_041_ReqGuildCooperateAttackLogList _msg)
    {
        GuildInfo guildInfo = _committer.getGuildInfo();

        List<GuildCooperate_AttackLog> logList = guildInfo.getCooperateInfo().makeLogList(_msg.getLastDbId(), _msg.getNum());

        _committer.commitSucRes(US2GCWriter_032_GuildOp.make_041_RetGuildCooperateAttackLogList(logList));
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return null;
    }

}