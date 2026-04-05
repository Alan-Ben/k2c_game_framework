package NPUSServer.NPUserMsgDispather.p023_ArenaOp;

import Common.ArenaObj.Arena_FightBackInfo;
import GC2GS.p023_ArenaOp.GC2GS_023_011_ReqArenaFightBackData;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_023_ArenaOp;

import java.util.List;

public class MsgDealer_GC2GS_023_011_ReqArenaFightBackData extends NPUserMsgDealer<GC2GS_023_011_ReqArenaFightBackData>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_023_011_ReqArenaFightBackData _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        //查找战报
        List<Arena_FightBackInfo> reportList = userData.getArenaComponent().getFightReportMgr().getFightBackList();
        _commiter.commitSucRes(US2GCWriter_023_ArenaOp.make_011_RetArenaFightBackData(reportList));
    }
}
