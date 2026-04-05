package NPUSServer.NPUserMsgDispather.p006_BagItemOp;

import GC2GS.p006_BagItemOp.GC2GS_006_004_ReqClickBagTime;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;

public class MsgDealer_GC2GS_006_004_ReqClickBagTime extends NPUserMsgDealer<GC2GS_006_004_ReqClickBagTime>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_006_004_ReqClickBagTime _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        userData.getBagItemComponent().refreshItemClickTime(_msg.getItemId());

        _commiter.commitSucRes(null);
    }
}
