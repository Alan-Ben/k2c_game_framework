package NPUSServer.NPUserMsgDispather.p002_InitOp;

import GC2GS.p002_InitOp.GC2GS_002_083_ReqActivityFundInit;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_002_InitOp;

/**
 * 活动基金初始化请求处理器
 *
 * 功能：处理客户端请求活动基金初始化数据
 */
public class MsgDealer_GC2GS_002_083_ReqActivityFundInit extends NPUserMsgDealer<GC2GS_002_083_ReqActivityFundInit>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_002_083_ReqActivityFundInit _msg)
    {
        // 获取用户对象
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        // 构造并返回活动基金初始化数据
        _commiter.commitSucRes(US2GCWriter_002_InitOp.make_083_RetActivityFundInit(userData));
    }
}
