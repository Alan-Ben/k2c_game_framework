package NPUSServer.NPUserMsgDispather.p023_ArenaOp;

import Common.ArenaObj.Arena_BattleReport;
import GC2GS.p023_ArenaOp.GC2GS_023_010_ReqArenaBattleReport;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_023_ArenaOp;

import java.util.List;

public class MsgDealer_GC2GS_023_010_ReqArenaBattleReport extends NPUserMsgDealer<GC2GS_023_010_ReqArenaBattleReport>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_023_010_ReqArenaBattleReport _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        //查找战报
        List<Arena_BattleReport> reportList = userData.getArenaComponent().getFightReportMgr().getReportList();
        _commiter.commitSucRes(US2GCWriter_023_ArenaOp.make_010_RetArenaBattleReport(reportList));
    }
}
