package NPUSServer.NPUserMsgDispather.p023_ArenaOp;

import GC2GS.p023_ArenaOp.GC2GS_023_021_ReqTowerFight;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CallBack._ICallBackResultT;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_023_ArenaOp;
import NPUSServer.Tower.TowerAttackResult;

public class MsgDealer_GC2GS_023_021_ReqTowerFight extends NPUserMsgDealer<GC2GS_023_021_ReqTowerFight>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_023_021_ReqTowerFight _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        //检查是否解锁
        if(!NPPlayerConditionDealerMgr.IsEnable(RefGeneral.Ref().tower_unlock_simple_unlock_id, userData, null))
        {
        	_commiter.commitFailRes(CommErr.SYSTEM_UNLOCK.getCode());
        	return;
        }

		NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.TOWER_ATTACK);

		userData.getTowerComponent().attackFloor(_msg.getTargetPos().getChapterId(), _msg.getTargetPos().getLevel(), context,
				new _ICallBackResultT<TowerAttackResult>()
				{
					@Override
					public void onRunOver(Result _result, TowerAttackResult _resultObj)
					{
						if (!_result.isSucc())
						{
							_commiter.commitFailRes(_result.getCode());
							return;
						}

						_commiter.commitSucRes(US2GCWriter_023_ArenaOp.make_021_RetTowerFight(_resultObj, context));
					}
				});
    }
}
