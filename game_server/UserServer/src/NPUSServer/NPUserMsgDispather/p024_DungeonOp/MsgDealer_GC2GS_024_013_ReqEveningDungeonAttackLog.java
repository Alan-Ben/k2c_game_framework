package NPUSServer.NPUserMsgDispather.p024_DungeonOp;

import Common.DungeonObj.EveningDungeon_AttackLog;
import GC2GS.p024_DungeonOp.GC2GS_024_013_ReqEveningDungeonAttackLog;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_024_DungeonOp;

import java.util.List;

public class MsgDealer_GC2GS_024_013_ReqEveningDungeonAttackLog extends NPUserMsgDealer<GC2GS_024_013_ReqEveningDungeonAttackLog>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_024_013_ReqEveningDungeonAttackLog _msg)
    {
        List<EveningDungeon_AttackLog> logList =
                getUSServer().getEveningDungeonMgr().getInfo().getAttackLogMgr().makeLogList(_msg.getSerial(), _msg.getNeedNum());

        _commiter.commitSucRes(US2GCWriter_024_DungeonOp.make_013_RetEveningDungeonAttackLog(logList));
    }
}
