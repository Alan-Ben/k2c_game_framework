package NPUSServer.CommonActivityMgr.Core.ActivityState;

import ALBasicServer.ALProcess.ALProcess;
import ALBasicServer.ALProcess._AALProcess;
import Common.ActivityEnum.EActivityState;
import NPCommon.CommonProcess._IEZProcessMonitor;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.USLog;

/**
 * 活动状态机状态 - 初始化中
 *
 * 主要功能：
 * 1. 异步初始化排行榜实例和事件监听
 * 2. 异步初始化阶段奖励实例和事件监听
 * 3. 异步初始化活动事件监听
 * 4. 异步注册关联礼包
 * 5. 异步初始化活动热更配置引用
 *
 * 执行流程：
 * 1. enter() 时启动异步初始化流程
 * 2. 所有初始化任务并行执行
 * 3. 初始化成功 → 设置下一状态为 PLAYING
 * 4. 初始化失败 → 设置下一状态为 CLOSED
 * 5. tick 机制通过 getCanTransToState() 获取下一状态并自动转换
 *
 * 设计特点：
 * - 纯粹的中间态，只负责异步初始化
 * - 遵循 tick 驱动的被动转换模式
 * - 在 enter() 中启动异步流程
 * - 异步完成后设置 _m_nextState，由 tick 驱动转换
 *
 * 线程安全：由活动管理器的 tick 机制保证单线程访问
 */
public class ActivityState_Initializing extends _AActivityState
{
    // 标记处理流程是否已完成
    private boolean _m_processDone = false;

    public ActivityState_Initializing(_AActivityBase _activity)
    {
        super(_activity);
    }

    @Override
    public EActivityState getStateType()
    {
        return EActivityState.INITIALIZING;
    }

    @Override
    public boolean judgeCanToState(_AActivityState _state)
    {
        // INITIALIZING 可以转到 PLAYING
        EActivityState targetType = _state.getStateType();
        return targetType == EActivityState.PLAYING;
    }

    @Override
    public void enter(boolean _isTransByRestore)
    {
        // 启动异步初始化流程
        _startAsyncInitProcess();
    }

    @Override
    public void quit()
    {
        // 无需清理，异步流程会自动完成
    }

    @Override
    public _AActivityState getCanTransToState(long _nowMs)
    {
        // 如果清理流程还未完成，则不可转换
        if (!_m_processDone)
            return null;

        // 返回异步流程设置的下一个状态
        // null 表示异步流程尚未完成，保持当前状态
        return new ActivityState_Playing(getActivity());
    }

    /**
     * 启动异步初始化流程
     *
     * 执行流程：
     * 1. 创建异步流程对象
     * 2. 添加各种初始化任务（排行榜、奖励、事件、礼包、配置）
     * 3. 启动流程并监听结果
     * 4. 成功 → 设置 _m_nextState 为 PLAYING
     * 5. 失败 → 报错, 等待处理
     * 6. tick 机制通过 getCanTransToState() 驱动状态转换
     */
    private void _startAsyncInitProcess()
    {
        USLog.info(getActivity().getUSServer(),
            "ActivityState_Initializing._startAsyncInitProcess - start async init: activityId={}, instanceId={}",
            getActivity().getActivityId(), getActivity().getInstanceId());

        ALProcess process = ALProcess.CreateProcess("activity_initializing_process");

        // 初始化排行榜流程
        getActivity().initRankInstance(process);
        getActivity().initRankEvent(process);

        // 初始化阶段奖励流程
        getActivity().initStepRewardInstance(process);
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
                // 记录具体错误信息
                getActivity().onActivityStateTransErr(_processTag, _exInfo, _ex);
            }

            @Override
            public void onProcessFailStop(_AALProcess _process)
            {
                // 记录失败的流程标签
                getActivity().onActivityStateTransFail(_process.getFullProcessTag());
            }

            @Override
            public void onRootProecssStop()
            {
                // 初始化失败
                USLog.error(getActivity().getUSServer(),
                    "ActivityState_Initializing._startAsyncInitProcess - async init failed, activityId={}, instanceId={}",
                    getActivity().getActivityId(), getActivity().getInstanceId());
            }

            @Override
            public void onRootProecssSuc()
            {
                // 初始化成功，设置下一状态为 PLAYING
                USLog.info(getActivity().getUSServer(),
                    "ActivityState_Initializing._startAsyncInitProcess - async init success: next state will be PLAYING, activityId={}, instanceId={}",
                    getActivity().getActivityId(), getActivity().getInstanceId());

                _m_processDone = true;
            }
        });
    }
}
