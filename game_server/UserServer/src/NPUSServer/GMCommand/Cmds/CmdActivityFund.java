package NPUSServer.GMCommand.Cmds;

import Common.ActivityFundObj.ActivityFund_Info;
import NPCommon.ErrMain.ActivityFundErr;
import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPUSServer.GMCommand.UsCmdBase;
import NPUSServer.NPUSUserMgr.UserComp.ActivityFund.ActivityFundInfo;

import java.util.List;

/**
 * 活动基金GM命令类
 * <p>
 * 主要功能：
 * 1. 修改任务分数
 * 2. 修改公式分数
 * 3. 修改已领取进度
 * 4. 设置任务计数
 * 5. 查询基金状态
 * <p>
 * 用于测试活动基金相关功能
 */
@ACommander(comment = "活动基金测试命令", name = "activityFund")
public class CmdActivityFund extends UsCmdBase
{
    /**
     * 查看所有活动基金信息
     */
    @ACommand(comment = "查看所有活动基金信息")
    public String listAllFunds()
    {
        StringBuilder sb = new StringBuilder();
        sb.append("=== 活动基金列表 ===\n");

        List<ActivityFund_Info> protoList = getOwner().getActivityFundComponent().makeProtoList();
        if (protoList.isEmpty())
        {
            sb.append("没有活动基金数据\n");
            return sb.toString();
        }

        for (ActivityFund_Info proto : protoList)
        {
            sb.append("基金ID: ").append(proto.getFundId()).append("\n");
            sb.append("  活动实例ID: ").append(proto.getActivityInstanceId()).append("\n");
            sb.append("  任务分数: ").append(proto.getTaskScore()).append("\n");
            sb.append("  公式分数: ").append(proto.getFormulaScore()).append("\n");
            sb.append("  总分: ").append(proto.getTaskScore() + proto.getFormulaScore()).append("\n");
            sb.append("  免费档已领取: ").append(proto.getHadDrawFreeStep()).append("\n");
            sb.append("  付费档已领取: ").append(proto.getHadDrawPayStep()).append("\n");
            sb.append("---\n");
        }

        return sb.toString();
    }

    /**
     * 增加任务分数
     * @param _fundId 基金ID
     * @param _score 增加的分数
     */
    @ACommand(comment = "增加任务分数[基金ID][分数]")
    public String addTaskScore(long _fundId, long _score)
    {
        ActivityFundInfo fundInfo = getOwner().getActivityFundComponent()
            .lookupActivityFundInfoByFundId(_fundId);

        if (fundInfo == null)
            return ActivityFundErr.FUND_NOT_FOUND.toString();

        fundInfo.addTaskScore(_score, getContext());

        return String.format("成功增加任务分数 %d，当前阶段: %d",
            _score, fundInfo.getCurrentStep());
    }

    /**
     * 设置免费档已领取阶段
     * @param _fundId 基金ID
     * @param _step 阶段号
     */
    @ACommand(comment = "设置免费档已领取阶段[基金ID][阶段号]")
    public String setFreeDrawStep(long _fundId, int _step)
    {
        ActivityFundInfo fundInfo = getOwner().getActivityFundComponent()
            .lookupActivityFundInfoByFundId(_fundId);

        if (fundInfo == null)
            return ActivityFundErr.FUND_NOT_FOUND.toString();

        fundInfo.setHadDrawFreeStep(_step);

        return String.format("成功设置免费档已领取阶段为 %d", _step);
    }

    /**
     * 设置付费档已领取阶段
     * @param _fundId 基金ID
     * @param _step 阶段号
     */
    @ACommand(comment = "设置付费档已领取阶段[基金ID][阶段号]")
    public String setPayDrawStep(long _fundId, int _step)
    {
        ActivityFundInfo fundInfo = getOwner().getActivityFundComponent()
            .lookupActivityFundInfoByFundId(_fundId);

        if (fundInfo == null)
            return ActivityFundErr.FUND_NOT_FOUND.toString();

        fundInfo.setHadDrawPayStep(_step);

        return String.format("成功设置付费档已领取阶段为 %d", _step);
    }


    /**
     * 设置任务计数
     * @param _fundId 基金ID
     * @param _taskId 任务ID
     * @param _count 计数值
     */
    @ACommand(comment = "设置任务计数[基金ID][任务ID][计数值]")
    public String setTaskCount(long _fundId, long _taskId, long _count)
    {
        ActivityFundInfo fundInfo = getOwner().getActivityFundComponent()
            .lookupActivityFundInfoByFundId(_fundId);

        if (fundInfo == null)
            return ActivityFundErr.FUND_NOT_FOUND.toString();

        fundInfo.setTaskCount(_taskId, _count, getContext());

        return String.format("成功设置任务 %d 计数为 %d", _taskId, _count);
    }

    /**
     * 增加任务计数
     * @param _fundId 基金ID
     * @param _taskId 任务ID
     * @param _count 增加的计数
     */
    @ACommand(comment = "增加任务计数[基金ID][任务ID][计数值]")
    public String addTaskCount(long _fundId, long _taskId, long _count)
    {
        ActivityFundInfo fundInfo = getOwner().getActivityFundComponent()
            .lookupActivityFundInfoByFundId(_fundId);

        if (fundInfo == null)
            return ActivityFundErr.FUND_NOT_FOUND.toString();

        fundInfo.addTaskCount(_taskId, _count, getContext());

        return String.format("成功增加任务 %d 计数 %d", _taskId, _count);
    }

    /**
     * 设置任务刷新时间
     * @param _fundId 基金ID
     * @param _delaySeconds 延迟秒数（从现在开始计算，0表示取消刷新）
     */
    @ACommand(comment = "设置任务刷新时间[基金ID][延迟秒数]")
    public String setTaskRefreshTime(long _fundId, long _delaySeconds)
    {
        ActivityFundInfo fundInfo = getOwner().getActivityFundComponent()
            .lookupActivityFundInfoByFundId(_fundId);

        if (fundInfo == null)
            return ActivityFundErr.FUND_NOT_FOUND.toString();

        long refreshTimeMs = 0;
        if (_delaySeconds > 0)
        {
            refreshTimeMs = System.currentTimeMillis() + _delaySeconds * 1000L;
        }

        fundInfo.setGmTaskRefreshTimeMs(refreshTimeMs);

        if (_delaySeconds > 0)
        {
            return String.format("成功设置任务刷新时间为 %d 秒后", _delaySeconds);
        }
        else
        {
            return "已取消任务刷新";
        }
    }
}
