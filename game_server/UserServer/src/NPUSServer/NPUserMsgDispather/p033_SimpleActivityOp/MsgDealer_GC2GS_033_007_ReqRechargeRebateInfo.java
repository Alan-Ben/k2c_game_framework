package NPUSServer.NPUserMsgDispather.p033_SimpleActivityOp;

import GC2GS.p033_SimpleActivityOp.GC2GS_033_007_ReqRechargeRebateInfo;
import NPCommon.ErrMain.ActivityErr;
import NPUSServer.CommonActivityMgr.Activities.RechargeRebate.RechargeRebateActivity;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_033_SimpleActivityOp;

/**
 * 请求充值返利信息处理器
 *
 * 处理客户端查询充值返利活动数据的请求
 */
public class MsgDealer_GC2GS_033_007_ReqRechargeRebateInfo extends NPUserMsgDealer<GC2GS_033_007_ReqRechargeRebateInfo>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_033_007_ReqRechargeRebateInfo _msg)
    {
        // 查找活动实例
        RechargeRebateActivity activity = getUSServer().getCommActivityMgr().lookupActivity(
                _msg.getActivityInstanceId(), RechargeRebateActivity.class);

        if (activity == null)
        {
            _committer.commitFailRes(ActivityErr.ACTIVITY_NOT_FOUND.getCode());
            return;
        }

        // 检查活动状态
        if (!activity.isRunning())
        {
            _committer.commitFailRes(ActivityErr.ACTIVITY_NOT_FOUND.getCode());
            return;
        }

        // 返回充值返利信息
        _committer.commitSucRes(US2GCWriter_033_SimpleActivityOp.make_007_RetRechargeRebateInfo(
                activity, _committer.getUserData().getCid()));
    }
}
