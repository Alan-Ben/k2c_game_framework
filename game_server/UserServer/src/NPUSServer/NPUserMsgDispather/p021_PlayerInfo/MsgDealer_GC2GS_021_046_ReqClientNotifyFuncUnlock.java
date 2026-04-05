package NPUSServer.NPUserMsgDispather.p021_PlayerInfo;

import GC2GS.p021_PlayerInfo.GC2GS_021_046_ReqClientNotifyFuncUnlock;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_021_PlayerInfo;

/*************
 * 设置称号信息已查看
 * @author mj
 *
 */
public class MsgDealer_GC2GS_021_046_ReqClientNotifyFuncUnlock extends NPUserMsgDealer<GC2GS_021_046_ReqClientNotifyFuncUnlock>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_021_046_ReqClientNotifyFuncUnlock _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        if(null == _msg.getFuncType())
        {
            _commiter.commitFailRes(CommErr.PARAM_ERROR.getCode());
            return;
        }

        //领取奖励
        Result result = userData.getFuncUnlockComponent().markClientNotified(_msg.getFuncType());
        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }

        //回包协议
        _commiter.commitSucRes(US2GCWriter_021_PlayerInfo.make_046_RetClientNotifyFuncUnlock());
    }
}
