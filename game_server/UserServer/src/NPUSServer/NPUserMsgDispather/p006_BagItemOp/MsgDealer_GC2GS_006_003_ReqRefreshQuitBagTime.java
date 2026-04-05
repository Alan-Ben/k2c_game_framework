package NPUSServer.NPUserMsgDispather.p006_BagItemOp;

import ALBasicCommon.ALBasicCommonFun;
import GC2GS.p006_BagItemOp.GC2GS_006_003_ReqRefreshQuitBagTime;
import NPEnum.ENPPlayerParam;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;

public class MsgDealer_GC2GS_006_003_ReqRefreshQuitBagTime extends NPUserMsgDealer<GC2GS_006_003_ReqRefreshQuitBagTime>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_006_003_ReqRefreshQuitBagTime _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        userData.getPlayerComponent().setParam(ENPPlayerParam.LAST_LEFT_BAG_TIME, ALBasicCommonFun.getNowTime());

        _commiter.commitSucRes(null);
    }
}
