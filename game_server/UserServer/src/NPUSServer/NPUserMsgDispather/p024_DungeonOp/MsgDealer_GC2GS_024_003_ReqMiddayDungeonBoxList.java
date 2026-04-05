package NPUSServer.NPUserMsgDispather.p024_DungeonOp;

import Common.DungeonObj.MiddayDungeon_BoxInfo;
import GC2GS.p024_DungeonOp.GC2GS_024_003_ReqMiddayDungeonBoxList;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_024_DungeonOp;

import java.util.List;

public class MsgDealer_GC2GS_024_003_ReqMiddayDungeonBoxList extends NPUserMsgDealer<GC2GS_024_003_ReqMiddayDungeonBoxList>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_024_003_ReqMiddayDungeonBoxList _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        List<MiddayDungeon_BoxInfo> boxList = getUSServer().getMiddayDungeonMgr().getBoxMgr().makeAllBoxList(_msg.getBoxType());

        _commiter.commitSucRes(US2GCWriter_024_DungeonOp.make_003_RetMiddayDungeonBoxList(boxList));
    }
}
