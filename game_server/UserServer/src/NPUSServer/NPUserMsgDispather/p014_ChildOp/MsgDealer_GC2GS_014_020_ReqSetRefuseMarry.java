package NPUSServer.NPUserMsgDispather.p014_ChildOp;

import GC2GS.p014_ChildOp.GC2GS_014_020_ReqSetRefuseMarry;
import NPEnum.ENPPlayerParam;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_014_ChildOp;

public class MsgDealer_GC2GS_014_020_ReqSetRefuseMarry extends NPUserMsgDealer<GC2GS_014_020_ReqSetRefuseMarry>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_014_020_ReqSetRefuseMarry _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        //根据请求设置或取消拒绝联姻状态
        userData.setParam(ENPPlayerParam.IS_REFUSE_MARRY_REQUEST, _msg.getIsRefuse() ? 1 : 0);

        //返回设置成功响应
        _commiter.commitSucRes(US2GCWriter_014_ChildOp.make_020_RetSetRefuseMarry());
    }
}