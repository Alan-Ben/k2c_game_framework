package NPUSServer.NPUserMsgDispather.p002_InitOp;

import Common.Common_CrossServerGroupInfo;
import GC2GS.p002_InitOp.GC2GS_002_002_ReqCrossServerGroupInfo;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_002_InitOp;

public class MsgDealer_GC2GS_002_002_ReqCrossServerGroupInfo extends NPUserMsgDealer<GC2GS_002_002_ReqCrossServerGroupInfo>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_002_002_ReqCrossServerGroupInfo _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        Common_CrossServerGroupInfo groupInfo = userData.getUSServer().getLocalCrossServerGroupMgr().makeGroupInfo();

        _commiter.commitSucRes(US2GCWriter_002_InitOp.make_002_RetCrossServerGroupInfo(groupInfo));
    }
}
