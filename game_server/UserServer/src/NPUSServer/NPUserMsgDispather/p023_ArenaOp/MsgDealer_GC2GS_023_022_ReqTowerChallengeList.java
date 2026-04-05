package NPUSServer.NPUserMsgDispather.p023_ArenaOp;

import Common.TowerObj.Tower_OpponentInfo;
import GC2GS.p023_ArenaOp.GC2GS_023_022_ReqTowerChallengeList;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CallBack._ICallBackResultT;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_023_ArenaOp;

import java.util.List;

public class MsgDealer_GC2GS_023_022_ReqTowerChallengeList extends NPUserMsgDealer<GC2GS_023_022_ReqTowerChallengeList>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_023_022_ReqTowerChallengeList _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        userData.getTowerComponent().makeChallengeList(new _ICallBackResultT<List<Tower_OpponentInfo>>()
        {
            @Override
            public void onRunOver(Result _result, List<Tower_OpponentInfo> _opponentList)
            {
                if (!_result.isSucc())
                {
                    _commiter.commitFailRes(_result.getCode());
                    return;
                }

                _commiter.commitSucRes(US2GCWriter_023_ArenaOp.make_022_RetTowerChallengeList(_opponentList));
            }
        });
    }
}
