package NPUSServer.NPUserMsgDispather.p002_InitOp;

import GC2GS.p002_InitOp.GC2GS_002_070_ReqGiftPackInit;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_002_InitOp;

/********
 * 礼包初始化请求处理
 */
public class MsgDealer_GC2GS_002_070_ReqGiftPackInit extends NPUserMsgDealer<GC2GS_002_070_ReqGiftPackInit>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_002_070_ReqGiftPackInit _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        _commiter.commitSucRes(US2GCWriter_002_InitOp.make_070_RetGiftPackInit(userData));
    }
}