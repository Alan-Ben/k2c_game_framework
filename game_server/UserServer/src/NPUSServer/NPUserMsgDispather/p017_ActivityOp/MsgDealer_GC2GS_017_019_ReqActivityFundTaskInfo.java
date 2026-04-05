package NPUSServer.NPUserMsgDispather.p017_ActivityOp;

import Common.ActivityFundObj.ActivityFund_TaskInfo;
import GC2GS.p017_ActivityOp.GC2GS_017_019_ReqActivityFundTaskInfo;
import NPCommon.ErrMain.ActivityFundErr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ActivityFund.ActivityFundInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_017_ActivityOp;

import java.util.List;

/**
 * 活动基金任务信息请求处理器
 *
 * 功能：处理客户端请求活动基金的任务信息
 */
public class MsgDealer_GC2GS_017_019_ReqActivityFundTaskInfo extends NPUserMsgDealer<GC2GS_017_019_ReqActivityFundTaskInfo>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_017_019_ReqActivityFundTaskInfo _msg)
    {
        // 获取用户对象
        NPUSUserData userData = _committer.getUserData();

        // 获取活动基金信息
        ActivityFundInfo fundInfo = userData.getActivityFundComponent().lookupActivityFundInfoByFundId(_msg.getFundId());
        if (fundInfo == null)
        {
            _committer.commitFailRes(ActivityFundErr.FUND_NOT_FOUND.getCode());
            return;
        }

        // 检查并重置过期的任务数据
        fundInfo.checkAndResetExpiredTasks();

        // 获取任务信息列表
        List<ActivityFund_TaskInfo> taskList = fundInfo.makeTaskProtoList();

        // 返回任务信息
        _committer.commitSucRes(US2GCWriter_017_ActivityOp.make_019_RetActivityFundTaskInfo(taskList, fundInfo.getNextTaskRefreshTimeMs()));
    }
}
