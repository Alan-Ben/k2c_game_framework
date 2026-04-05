package NPUSServer.NPUserMsgDispather.p017_ActivityOp;

import GC2GS.p017_ActivityOp.GC2GS_017_006_ReqActivityHotRefInfo;
import NPCommon.ErrMain.ActivityErr;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_017_ActivityOp;
import NPUSServer.UsActivityScheduleMgr.UsActivityScheduleInfo;

public class MsgDealer_GC2GS_017_006_ReqActivityHotRefInfo extends NPUserMsgDealer<GC2GS_017_006_ReqActivityHotRefInfo>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_017_006_ReqActivityHotRefInfo _msg)
    {
        NPUSUserData userData = _committer.getUserData();

        //获取活动对象
        _AActivityBase activity = userData.getUSServer().getCommActivityMgr().lookupActivity(_msg.getInstanceId());
        if (activity == null)
        {
            _committer.commitFailRes(ActivityErr.ACTIVITY_NOT_FOUND.getCode());
            return;
        }

        UsActivityScheduleInfo scheduleInfo = activity.getSchedule();
        if (scheduleInfo == null)
        {
            _committer.commitFailRes(ActivityErr.ACTIVITY_SCHEDULE_NOT_FOUND.getCode());
            return;
        }

        _committer.commitSucRes(US2GCWriter_017_ActivityOp.make_006_RetActivityHotRefInfo(scheduleInfo.makeHotRefInfo()));
    }
}
