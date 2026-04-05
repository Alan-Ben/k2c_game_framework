package NPUSServer.NPUserMsgDispather.p032_GuildOp;

import GC2GS.p032_GuildOp.GC2GS_032_041_ReqGuildCooperateAttackLogList;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;

/**
 * 联盟协作攻击日志查询处理器
 */
public class MsgDealer_GC2GS_032_041_ReqGuildCooperateAttackLogList extends NPUserMsgDealer<GC2GS_032_041_ReqGuildCooperateAttackLogList>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_032_041_ReqGuildCooperateAttackLogList _msg)
    {
        //转化为统一跨服消息进行处理
        getUSServer().dealGuildMsg(
                _committer,
                _committer.getUserData().getCid(),
                _committer.getUserData().getGuildComponent().getGuildId(),
                _msg,
                null
        );
    }
}