package NPUSServer.NPUserMsgDispather.p023_ArenaOp;

import GC2GS.p023_ArenaOp.GC2GS_023_024_ReqTowerResearchActive;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_023_ArenaOp;

public class MsgDealer_GC2GS_023_024_ReqTowerResearchActive extends NPUserMsgDealer<GC2GS_023_024_ReqTowerResearchActive>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_023_024_ReqTowerResearchActive _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

		NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.TOWER_RESEARCH_ACTIVE);

		Result result = userData.getTowerComponent().activeResearch(_msg.getTargetPos().getChapterId(), _msg.getTargetPos().getLevel(), context);
		if (!result.isSucc())
		{
			_commiter.commitFailRes(result.getCode());
			return;
		}

		_commiter.commitSucRes(US2GCWriter_023_ArenaOp.make_024_RetTowerResearchActive());
	}
}
