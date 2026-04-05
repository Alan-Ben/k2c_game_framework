package NPUSServer.NPUserMsgDispather.p024_DungeonOp;

import Common.DungeonObj.EveningDungeon_DefeatInfo;
import GC2GS.p024_DungeonOp.GC2GS_024_015_ReqEveningDungeonDefeatLog;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_024_DungeonOp;

import java.util.List;

public class MsgDealer_GC2GS_024_015_ReqEveningDungeonDefeatLog extends NPUserMsgDealer<GC2GS_024_015_ReqEveningDungeonDefeatLog>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_024_015_ReqEveningDungeonDefeatLog _msg)
    {
        List<EveningDungeon_DefeatInfo> logList =
                getUSServer().getEveningDungeonMgr().getInfo().getAttackLogMgr().makeDefeatLogList();

        _commiter.commitSucRes(US2GCWriter_024_DungeonOp.make_015_RetEveningDungeonDefeatLog(logList));
    }
}
