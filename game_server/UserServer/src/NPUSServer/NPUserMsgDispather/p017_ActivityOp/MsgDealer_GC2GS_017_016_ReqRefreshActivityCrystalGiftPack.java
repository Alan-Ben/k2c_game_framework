package NPUSServer.NPUserMsgDispather.p017_ActivityOp;

import GC2GS.p017_ActivityOp.GC2GS_017_016_ReqRefreshActivityCrystalGiftPack;
import NPCommon.ErrMain.ActivityErr;
import NPCommon.ErrMain.Result.Result;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_017_ActivityOp;

public class MsgDealer_GC2GS_017_016_ReqRefreshActivityCrystalGiftPack extends NPUserMsgDealer<GC2GS_017_016_ReqRefreshActivityCrystalGiftPack>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_017_016_ReqRefreshActivityCrystalGiftPack _msg)
    {
        NPUSUserData userData = _committer.getUserData();

        //获取活动对象
        _AActivityBase activity = userData.getUSServer().getCommActivityMgr().lookupActivity(_msg.getInstanceId());
        if (activity == null)
        {
            _committer.commitFailRes(ActivityErr.ACTIVITY_NOT_FOUND.getCode());
            return;
        }

        Result result = activity.getCrystalGiftPackMgr().refreshGroup(_committer.getUserData(), _msg.getGroupId());
        if (!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());
            return;
        }

        //返回成功
        _committer.commitSucRes(US2GCWriter_017_ActivityOp.make_016_RetRefreshActivityCrystalGiftPack());
    }
}
