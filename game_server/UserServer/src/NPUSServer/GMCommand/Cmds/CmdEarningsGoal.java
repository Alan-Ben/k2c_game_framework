package NPUSServer.GMCommand.Cmds;

import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPUSServer.CommonActivityMgr.Activities.EarningsGoal.EEarningsGoalType;
import NPUSServer.CommonActivityMgr.Activities.EarningsGoal.EarningsGoalActivity;
import NPUSServer.CommonActivityMgr.Activities.EarningsGoal._AEarningsGoalInfo;
import NPUSServer.CommonActivityMgr.Activities.EarningsGoal._AEarningsGoalRecord;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.GMCommand.UsCmdBase;
import NPUSServer.USLog;
import USDB.Bo.EarningsGoalRecordBO;

import java.lang.reflect.Field;

/**
 * 赚速目标活动GM命令
 *
 * 主要功能：
 * 1. 修改首达玩家CID（first_reach_cid字段）
 * 2. 查询首达玩家信息
 *
 * 实现方式：
 * - 通过反射访问_AEarningsGoalRecord的私有_m_bo字段
 * - 直接调用BO对象的saveXxx方法更新数据库
 *
 * 线程安全：通过BO对象的数据库操作保证
 */
@ACommander(comment = "赚速目标活动GM命令", name = "earningsGoal")
public class CmdEarningsGoal extends UsCmdBase
{
    /**
     * 修改首达玩家CID
     *
     * 执行流程：
     * 1. 参数验证（类型范围、CID合法性）
     * 2. 获取活动对象和记录对象
     * 3. 通过反射获取BO对象
     * 4. 调用BO的save方法更新数据库
     * 5. 记录系统日志
     *
     * @param _instanceId 活动实例ID
     * @param _type 目标类型（0=REWARD, 1=HONOR_REWARD）
     * @param _refId 配置ID
     * @param _newCid 新的首达玩家CID（0表示清空）
     * @return 执行结果字符串
     */
    @ACommand(comment = "修改首达玩家[活动实例ID,目标类型,配置ID,新的CID]")
    public String setFirstReachCid(long _instanceId, int _type, long _refId, long _newCid)
    {
        try
        {
            // 1. 参数验证
            if (_type < 0 || _type >= EEarningsGoalType.values().length)
            {
                return "fail: invalid type=" + _type + ", valid range: [0-" + (EEarningsGoalType.values().length - 1) + "]";
            }

            if (_newCid < 0)
            {
                return "fail: invalid cid=" + _newCid;
            }

            // 2. 获取活动对象
            _AActivityBase activityBase = getUserServer().getCommActivityMgr().lookupActivity(_instanceId);
            if (activityBase == null)
            {
                return "fail: activity not found, instanceId=" + _instanceId;
            }

            if (!(activityBase instanceof EarningsGoalActivity))
            {
                return "fail: activity is not EarningsGoalActivity, instanceId=" + _instanceId;
            }

            EarningsGoalActivity activity = (EarningsGoalActivity) activityBase;

            // 3. 获取目标记录对象
            EEarningsGoalType goalType = EEarningsGoalType.values()[_type];
            _AEarningsGoalInfo goalInfo = activity.getRewardInfo(goalType);
            if (goalInfo == null)
            {
                return "fail: goalInfo not found, type=" + goalType;
            }

            _AEarningsGoalRecord record = goalInfo.lookupGoal(_refId);
            if (record == null)
            {
                return "fail: record not found, refId=" + _refId;
            }

            // 记录旧值（用于日志）
            long oldCid = record.getFirstReachCid();

            // 4. 反射访问BO对象
            Field boField = _AEarningsGoalRecord.class.getDeclaredField("_m_bo");
            boField.setAccessible(true);
            EarningsGoalRecordBO bo = (EarningsGoalRecordBO) boField.get(record);

            if (bo == null)
            {
                return "fail: bo is null";
            }

            // 5. 更新数据库（直接调用BO的save方法）
            long newTimestamp = CommonFunc.getNowTimeMS();

            // saveXxx方法会自动完成：值校验 + 数据库更新 + 内存同步
            bo.saveFirstReachCid(getUserServer().getBM(), _newCid);
            bo.saveTimestamp(getUserServer().getBM(), newTimestamp);

            // 6. 记录日志
            USLog.sys(getUserServer(),
                    "GM setFirstReachCid success: instanceId={}, type={}, refId={}, oldCid={}, newCid={}, timestamp={}",
                    _instanceId, goalType, _refId, oldCid, _newCid, newTimestamp);

            return String.format("success: updated first_reach_cid from %d to %d, timestamp=%d",
                    oldCid, _newCid, newTimestamp);

        } catch (NoSuchFieldException e)
        {
            CommLog.error("", e);
            return "fail: field '_m_bo' not found, error=" + e.getMessage();
        } catch (IllegalAccessException e)
        {
            CommLog.error("", e);
            return "fail: cannot access field '_m_bo', error=" + e.getMessage();
        } catch (Exception e)
        {
            CommLog.error("", e);
            return "fail: unexpected error=" + e.getMessage();
        }
    }

    /**
     * 查询首达玩家信息
     *
     * 执行流程：
     * 1. 参数验证
     * 2. 获取活动对象和记录对象
     * 3. 读取first_reach_cid和timestamp字段
     * 4. 格式化输出结果
     *
     * @param _instanceId 活动实例ID
     * @param _type 目标类型（0=REWARD, 1=HONOR_REWARD）
     * @param _refId 配置ID
     * @return 查询结果字符串（包含CID和时间戳）
     */
    @ACommand(comment = "查询首达玩家信息[活动实例ID,目标类型,配置ID]")
    public String queryFirstReachCid(long _instanceId, int _type, long _refId)
    {
        try
        {
            // 1. 参数验证
            if (_type < 0 || _type >= EEarningsGoalType.values().length)
            {
                return "fail: invalid type=" + _type + ", valid range: [0-" + (EEarningsGoalType.values().length - 1) + "]";
            }

            // 2. 获取活动和记录
            _AActivityBase activityBase = getUserServer().getCommActivityMgr().lookupActivity(_instanceId);
            if (activityBase == null)
            {
                return "fail: activity not found, instanceId=" + _instanceId;
            }

            if (!(activityBase instanceof EarningsGoalActivity))
            {
                return "fail: activity is not EarningsGoalActivity, instanceId=" + _instanceId;
            }

            EarningsGoalActivity activity = (EarningsGoalActivity) activityBase;
            EEarningsGoalType goalType = EEarningsGoalType.values()[_type];
            _AEarningsGoalInfo goalInfo = activity.getRewardInfo(goalType);

            if (goalInfo == null)
            {
                return "fail: goalInfo not found, type=" + goalType;
            }

            _AEarningsGoalRecord record = goalInfo.lookupGoal(_refId);
            if (record == null)
            {
                return "fail: record not found, refId=" + _refId;
            }

            // 3. 直接通过公开方法获取信息（无需反射）
            long firstReachCid = record.getFirstReachCid();
            long timestamp = record.getTimestamp();

            // 4. 格式化输出
            String timeStr = CommonFunc.getTimeStringMs(timestamp);
            return String.format("first_reach_cid=%d, timestamp=%d (%s)",
                    firstReachCid, timestamp, timeStr);

        } catch (Exception e)
        {
            CommLog.error("", e);
            return "fail: error=" + e.getMessage();
        }
    }
}
