package NPUSServer.CommonActivityMgr.Core.ActivityState;

import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;

/**
 * 冲突等待处理器
 *
 * 职责：
 * 1. 跟踪活动因冲突而等待的时间
 * 2. 当等待超过10分钟时发送钉钉报警
 * 3. 之后每隔10分钟重复报警一次
 *
 * 使用方式：
 * - 在状态类构造函数中创建handler实例
 * - 当checkAndSelect()返回false时调用handleWaiting()
 * - 当活动成功转换时调用reset()清理等待信息
 * - 在quit()中调用reset(0)清理状态
 */
public class WaitingConflictHandler
{
    // 报警间隔（10分钟）
    private static final long ALERT_INTERVAL_MS = 10 * 60 * 1000L;

    // 关联的活动实例，用于获取日志和报警所需信息
    private final _AActivityBase _m_activity;
    // 状态标签（如 PLAN、RESTORE），用于日志/报警标识
    private final String _m_stateTag;

    // 本轮等待的起始时间戳（毫秒），0表示当前没有等待
    private long _m_waitStartMs = 0L;
    // 最近一次报警的时间戳（毫秒），0表示尚未报警
    private long _m_lastAlertMs = 0L;

    /**
     * 构造函数
     *
     * @param _activity 活动实例
     * @param _stateTag 状态标签（如PLAN、RESTORE）
     */
    public WaitingConflictHandler(_AActivityBase _activity, String _stateTag)
    {
        _m_activity = _activity;
        _m_stateTag = _stateTag;
    }

    /**
     * 处理等待逻辑
     *
     * 执行流程：
     * 1. 第一次调用时记录等待开始时间
     * 2. 如果等待时间超过10分钟，发送报警
     * 3. 之后每隔10分钟重复报警一次
     *
     * @param _nowMs 当前时间（毫秒）
     */
    public void handleWaiting(long _nowMs)
    {
        if (_nowMs <= 0L)
        {
            return;
        }

        // 第一次检测到等待，记录开始时间
        if (_m_waitStartMs == 0L)
        {
            _m_waitStartMs = _nowMs;
            _m_lastAlertMs = 0L;

            USLog.info(_m_activity.getUSServer(),
                    "WaitingConflictHandler.handleWaiting - start waiting for conflict resolution: " +
                    "activityId={}, instanceId={}, state={}",
                    _m_activity.getActivityId(),
                    _m_activity.getInstanceId(),
                    _m_stateTag);
            return;
        }

        // 计算等待时长
        long waitDurationMs = _nowMs - _m_waitStartMs;
        if (waitDurationMs < ALERT_INTERVAL_MS)
        {
            return;
        }

        // 判断是否需要报警：首次报警 或 距上次报警超过10分钟
        if (_m_lastAlertMs == 0L || (_nowMs - _m_lastAlertMs) >= ALERT_INTERVAL_MS)
        {
            _m_lastAlertMs = _nowMs;
            _sendAlert(waitDurationMs);
        }
    }

    /**
     * 清理等待信息
     *
     * @param _nowMs 当前时间（毫秒），传0表示无需记录等待结束日志
     */
    public void reset(long _nowMs)
    {
        // 如果没有在等待，直接返回
        if (_m_waitStartMs == 0L)
        {
            return;
        }

        // 如果传入了有效时间，记录等待结束日志
        if (_nowMs > 0L && _nowMs >= _m_waitStartMs)
        {
            long totalWaitMs = _nowMs - _m_waitStartMs;
            USLog.info(_m_activity.getUSServer(),
                    "WaitingConflictHandler.reset - conflict resolved, waiting finished: " +
                    "activityId={}, instanceId={}, state={}, totalWaitMs={}",
                    _m_activity.getActivityId(),
                    _m_activity.getInstanceId(),
                    _m_stateTag,
                    totalWaitMs);
        }

        // 清理等待信息
        _m_waitStartMs = 0L;
        _m_lastAlertMs = 0L;
    }

    /**
     * 发送一次报警（warn级别 + 钉钉）
     *
     * @param _waitDurationMs 等待时长（毫秒）
     */
    private void _sendAlert(long _waitDurationMs)
    {
        NPUserServer server = _m_activity.getUSServer();
        long activityId = _m_activity.getActivityId();
        long instanceId = _m_activity.getInstanceId();
        long waitDurationSec = _waitDurationMs / 1000L;

        // 发送钉钉报警
        server.getDDAlert().warn("Activity " + activityId + " state conflict wait timeout",
                "WaitingConflictHandler._sendAlert - activity waiting too long for conflict resolution: activityId={}, instanceId={}, state={}, waitDurationSec={} seconds",
                activityId, instanceId, _m_stateTag, waitDurationSec);
    }
}
