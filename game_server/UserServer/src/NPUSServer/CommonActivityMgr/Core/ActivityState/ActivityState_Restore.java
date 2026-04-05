package NPUSServer.CommonActivityMgr.Core.ActivityState;

import ALBasicServer.ALProcess.ALProcess;
import ALBasicServer.ALProcess._AALProcess;
import Common.ActivityEnum.EActivityState;
import NPCommon.CommonProcess._IEZProcessMonitor;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.USLog;

/**
 * 活动状态机状态 - 恢复状态
 *
 * 主要功能：
 * 1. 从数据库加载活动状态并恢复到目标状态
 * 2. 根据目标状态执行不同的异步初始化流程
 * 3. 处理冲突管理和等待逻辑
 *
 * 执行流程：
 * 1. enter() 时尝试启动恢复流程（需要先通过冲突检查）
 * 2. 如果冲突检查失败，在 getCanTransToState() 中重复尝试
 * 3. 恢复流程完成后设置 _m_nextState
 * 4. tick 机制通过 getCanTransToState() 驱动状态转换
 *
 * 恢复逻辑：
 * - 目标是 PLAYING：初始化排行榜、奖励、事件、礼包、配置
 * - 目标是 SETTLING/REWARDING：只初始化热更配置
 * - 其他状态：直接转换，无需初始化
 *
 * 设计特点：
 * - 特殊的中间态，负责从持久化恢复活动状态
 * - 遵循 tick 驱动的被动转换模式
 * - 在 enter() 中尝试启动异步流程（需要冲突检查）
 * - 异步完成后设置 _m_nextState，由 tick 驱动转换
 *
 * 线程安全：由活动管理器的 tick 机制保证单线程访问
 */
public class ActivityState_Restore extends _AActivityState
{
    // 存储异步流程完成后的下一个状态（null 表示未完成或未启动）
    private _AActivityState _m_nextState = null;

    public ActivityState_Restore(_AActivityBase _activity)
    {
        super(_activity);
    }

    @Override
    public EActivityState getStateType()
    {
        return EActivityState.RESTORE;
    }

    @Override
    public boolean judgeCanToState(_AActivityState _state)
    {
        //该状态由于是准备状态 不限制转换到的状态
        return true;
    }

    @Override
    public void enter(boolean _isTransByRestore)
    {
        // 启动恢复流程
        _tryStartRestoreProcess();
    }

    @Override
    public void quit()
    {
    }

    @Override
    public _AActivityState getCanTransToState(long _nowMs)
    {
        // 返回异步流程设置的下一个状态
        // null 表示异步流程尚未完成或尚未启动（等待冲突检查），保持当前状态
        return _m_nextState;
    }

    /**
     * 尝试启动恢复流程
     *
     * 执行流程：
     * 1. 检查冲突管理，确认是否可以开始恢复
     * 2. 未通过检查则等待，通过检查则获取目标状态
     * 3. 根据目标状态选择不同的恢复流程
     * 4. 启动异步流程，完成后自动转换到目标状态
     */
    private void _tryStartRestoreProcess()
    {
        // 查询活动应该恢复到的状态
        final EActivityState targetState = EActivityState.EActivityState_FromInt(getActivity().getBo().getCurState());
        if (targetState == null)
        {
            USLog.error(getActivity().getUSServer(),
                "ActivityState_Restore._tryStartRestoreProcess - target state error: targetState is null, activityId={}, instanceId={}",
                getActivity().getActivityId(), getActivity().getInstanceId());
            return;
        }

        USLog.info(getActivity().getUSServer(),
            "ActivityState_Restore._tryStartRestoreProcess - start async restore: activityId={}, instanceId={}, targetState={}",
            getActivity().getActivityId(), getActivity().getInstanceId(), targetState);

        // 根据目标状态选择不同的恢复流程
        if (targetState == EActivityState.PLAYING)
        {
            _startRestoreToPlayingProcess(targetState);
        }
        else if (targetState == EActivityState.SETTLING || targetState == EActivityState.REWARDING || targetState == EActivityState.CLOSED)
        {
            _startRestoreToSettleOrRewardProcess(targetState);
        }
        else
        {
            // 其他状态直接转换，无需异步初始化
            USLog.info(getActivity().getUSServer(),
                "ActivityState_Restore._tryStartRestoreProcess - direct restore without async process: activityId={}, instanceId={}, targetState={}",
                getActivity().getActivityId(), getActivity().getInstanceId(), targetState);

            // 直接设置下一个状态，由 tick 驱动转换
            _m_nextState = getActivity().getMachine().getStateHandler(targetState);
        }
    }

    /**
     * 启动恢复到 PLAYING 状态的异步流程
     *
     * @param _targetState 目标状态
     */
    private void _startRestoreToPlayingProcess(final EActivityState _targetState)
    {
        ALProcess process = ALProcess.CreateProcess("activity_restore_to_playing_process");

        // 初始化排行榜事件
        getActivity().initRankEvent(process);
        // 初始化阶段奖励事件
        getActivity().initStepRewardEvent(process);
        // 初始化活动事件
        getActivity().initActivityEvent(process);
        // 注册关联礼包
        getActivity().initActivityGiftPack(process);
        // 初始化活动热更配置引用
        getActivity().initActiveHotRef(process);

        // 开启流程并监听结果
        process.dealProcess(new _IEZProcessMonitor()
        {
            @Override
            public long monitorTimeMS(String _processTag)
            {
                return 1000; // 每秒检查一次超时
            }

            @Override
            public void onErr(long _processTimeMS, String _processTag, String _exInfo, Exception _ex)
            {
                getActivity().onActivityStateTransErr(_processTag, _exInfo, _ex);
            }

            @Override
            public void onProcessFailStop(_AALProcess _process)
            {
                getActivity().onActivityStateTransFail(_process.getFullProcessTag());
            }

            @Override
            public void onRootProecssStop()
            {
                // 恢复失败
                USLog.error(getActivity().getUSServer(),
                    "ActivityState_Restore._startRestoreToPlayingProcess - async restore failed, activityId={}, instanceId={}",
                    getActivity().getActivityId(), getActivity().getInstanceId());

            }

            @Override
            public void onRootProecssSuc()
            {
                // 恢复成功，设置下一状态为目标状态
                USLog.info(getActivity().getUSServer(),
                    "ActivityState_Restore._startRestoreToPlayingProcess - async restore success: activityId={}, instanceId={}, targetState={}",
                    getActivity().getActivityId(), getActivity().getInstanceId(), _targetState);

                // 设置下一个状态，由 tick 驱动转换
                _m_nextState = new ActivityState_Playing(getActivity());
            }
        });
    }

    /**
     * 启动恢复到 SETTLING/REWARDING 状态的异步流程
     *
     * @param _targetState 目标状态
     */
    private void _startRestoreToSettleOrRewardProcess(final EActivityState _targetState)
    {
        ALProcess process = ALProcess.CreateProcess("activity_restore_to_settle_or_reward_process");

        // 初始化活动热更配置引用
        getActivity().initActiveHotRef(process);

        // 开启流程并监听结果
        process.dealProcess(new _IEZProcessMonitor()
        {
            @Override
            public long monitorTimeMS(String _processTag)
            {
                return 1000; // 每秒检查一次超时
            }

            @Override
            public void onErr(long _processTimeMS, String _processTag, String _exInfo, Exception _ex)
            {
                getActivity().onActivityStateTransErr(_processTag, _exInfo, _ex);
            }

            @Override
            public void onProcessFailStop(_AALProcess _process)
            {
                getActivity().onActivityStateTransFail(_process.getFullProcessTag());
            }

            @Override
            public void onRootProecssStop()
            {
                // 恢复失败
                USLog.error(getActivity().getUSServer(),
                    "ActivityState_Restore._startRestoreToSettleOrRewardProcess - async restore failed, activityId={}, instanceId={}",
                    getActivity().getActivityId(), getActivity().getInstanceId());
            }

            @Override
            public void onRootProecssSuc()
            {
                // 恢复成功，设置下一状态为目标状态
                USLog.info(getActivity().getUSServer(),
                    "ActivityState_Restore._startRestoreToSettleOrRewardProcess - async restore success: activityId={}, instanceId={}, targetState={}",
                    getActivity().getActivityId(), getActivity().getInstanceId(), _targetState);

                // 设置下一个状态，由 tick 驱动转换
                _m_nextState = getActivity().getMachine().getStateHandler(_targetState);
            }
        });
    }

    @Override
    public boolean needSaveToDb()
    {
        return false;
    }
}
