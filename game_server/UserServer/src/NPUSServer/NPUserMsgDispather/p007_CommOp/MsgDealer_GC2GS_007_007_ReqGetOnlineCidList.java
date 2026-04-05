package NPUSServer.NPUserMsgDispather.p007_CommOp;

import GC2GS.p007_CommOp.GC2GS_007_007_ReqGetOnlineCidList;
import NPCommon.ErrMain.CommErr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_007_CommOp;

/**
 * 获取当前玩家所在US所有在线玩家数据
 * @author mj
 *
 */
public class MsgDealer_GC2GS_007_007_ReqGetOnlineCidList extends NPUserMsgDealer<GC2GS_007_007_ReqGetOnlineCidList>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_007_007_ReqGetOnlineCidList _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();
        
        if(_msg.getNum() <= 0)
        {
        	_commiter.commitFailRes(CommErr.PARAM_ERROR.getCode());
        	return;
        }

        _commiter.commitSucRes(US2GCWriter_007_CommOp.make_007_RetGetOnlineCidList(_msg.getNum(), userData));
    }
}
