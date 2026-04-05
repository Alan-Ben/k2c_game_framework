package NPScheduleServer.ActivityScheduleMgr;

import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALProcess.ALProcess;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.ScheduleObj.Schedule_ActivityInfo;
import Common.ScheduleObj.Schedule_UsPushData;
import NP2CS_R.np_p006_RankOp.NP2CS_R_006_001_ReqCrossInstance;
import NP2CS_R.np_p006_RankOp.NP2CS_R_006_002_ReqDiscardCrossInstance;
import NP2CS_R.np_p007_GameLogicOp.NP2CS_R_007_001_ReqGameLogicInstance;
import NP2CS_R.np_p007_GameLogicOp.NP2CS_R_007_002_ReqDiscardGameLogicInstance;
import NP2CS_RB.np_p006_RankOp.NP2CS_RB_006_001_RetCrossInstance;
import NP2CS_RB.np_p006_RankOp.NP2CS_RB_006_002_RetDiscardCrossInstance;
import NP2CS_RB.np_p007_GameLogicOp.NP2CS_RB_007_001_RetGameLogicInstance;
import NP2CS_RB.np_p007_GameLogicOp.NP2CS_RB_007_002_RetDiscardGameLogicInstance;
import NPCommon.CommonProcess._IEZProcessMonitor;
import NPCommon.DB.BM.BM;
import NPCommon.Log.CommLog;
import NPCommon.Util.CallBack._ICallBackBool;
import NPGameRes.Refs.Activity.RefActivity;
import NPScheduleServer.ActivityScheduleMgr.Task.ActivitySchedulePushTask;
import NPScheduleServer.ActivityScheduleMgr.Task.NoticeCanSendRewardTask;
import NPScheduleServer.NPScheduleServer;
import SSDB.Bo.ActivityScheduleUsGroupBO;
import SSDB.Bo.ActivityScheduleUsInfoBO;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;

import java.util.ArrayList;
import java.util.List;

/**
 * 活动排期用户服务器分组管理类
 * <p>
 * 主要功能：
 * 1. 管理活动排期的用户服务器分组信息
 * 2. 处理跨服实例数据获取
 * 3. 执行分组推送任务
 * 4. 构造推送协议对象
 * <p>
 * 设计特点：
 * - 使用_m_dbId缓存数据库主键，减少内存占用
 * - 延迟创建数据库记录，避免不必要的数据库操作
 * - 高效的增量数据库更新机制
 * <p>
 * 线程安全：通过BM事务保证数据一致性
 */
public class ActivityScheduleUsGroup
{
    private ActivityScheduleData _m_scheduleData;

    // 数据库主键ID
    private long _m_dbId;
    // 缓存的业务数据字段
    private long _m_crossInstanceId;
    private long _m_gameLogicInstanceId;
    private String _m_resFileName;
    private String _m_resFileMd5;
    private String _m_resFileDir;

    private List<ActivityScheduleUsInfo> _m_usList;

    // 缓存的有序UsId数组,用于快速比较
    private List<Integer> _m_sortedUsIdList;

    // 流程处理标志（使用volatile保证跨线程可见性）
    private volatile boolean _m_isProcessPush;
    private volatile boolean _m_isProcessDiscard;

    // 是否已完成废弃处理（持久化字段，使用volatile保证可见性）
    private volatile boolean _m_hadDiscarded;

    /**
     * 从已存在的BO对象构造分组（用于数据加载）
     * @param _scheduleData 排期数据对象
     * @param _bo           数据库BO对象
     */
    public ActivityScheduleUsGroup(ActivityScheduleData _scheduleData, ActivityScheduleUsGroupBO _bo)
    {
        _m_scheduleData = _scheduleData;
        _m_usList = new ArrayList<>();

        _m_dbId = _bo.getId();
        _m_crossInstanceId = _bo.getCrossInstanceId();
        _m_gameLogicInstanceId = _bo.getGameLogicInstanceId();
        _m_resFileName = _bo.getResFileName();
        _m_resFileMd5 = _bo.getResFileMd5();
        _m_resFileDir = _bo.getResFileDir();
        _m_hadDiscarded = _bo.getHadDiscarded();
    }

    public ActivityScheduleData getScheduleData()
    {
        return _m_scheduleData;
    }

    public void initUsInfo(ActivityScheduleUsInfoBO _usInfoBO)
    {
        ActivityScheduleUsInfo usInfo = new ActivityScheduleUsInfo(this, _usInfoBO);
        addUsInfo(usInfo);
    }

    public void addUsInfo(ActivityScheduleUsInfo _usInfo)
    {
        _m_usList.add(_usInfo);
    }

    /**
     * 检查分组是否可废弃
     *
     * 状态说明：
     * - _m_hadDiscarded 在第一次调用 startProcessDiscard() 时设置
     * - 本服分组（crossInstanceId == 0）：立即标记为 true
     * - 跨服分组：发送废弃请求成功后标记为 true
     * - 该字段持久化到数据库，重启后状态保持
     *
     * @return true-可废弃，false-不可废弃
     */
    public boolean canDiscard()
    {
        return _m_hadDiscarded;
    }

    /**
     * 获取数据库管理器
     * @return BM对象
     */
    public BM getBM()
    {
        return NPScheduleServer.getInstance().getBM();
    }

    /**
     * 获取分组数据库ID
     * @return 数据库主键ID
     */
    public long getGroupDbId()
    {
        return _m_dbId;
    }

    public long getScheduleDbId()
    {
        return _m_scheduleData.getBO().getId();
    }

    public ActivityScheduleUsInfo lookupUsInfo(int _usId)
    {
        for (ActivityScheduleUsInfo usInfo : _m_usList)
        {
            if (usInfo.getUsId() == _usId)
                return usInfo;
        }
        return null;
    }

    /**
     * 检查是否包含指定UserServer
     *
     * @param usId 目标UserServer ID
     * @return true-包含，false-不包含
     */
    public boolean containsUsId(int usId)
    {
        for (ActivityScheduleUsInfo usInfo : _m_usList)
        {
            if (usInfo.getUsId() == usId)
            {
                return true;
            }
        }
        return false;
    }

    /**
     * 获取所有未完成的US信息列表
     *
     * @return US信息列表
     */
    public List<ActivityScheduleUsInfo> getAllUsInfoList()
    {
        return new ArrayList<>(_m_usList);
    }

    /**
     * 检查活动排期组是否已完成
     * @return
     */
    public boolean isAllDone()
    {
        for (ActivityScheduleUsInfo usInfo : _m_usList)
        {
            if (!usInfo.hadDone())
                return false;
        }
        return true;
    }

    /**
     * 检查是否所有US都已进入结算状态
     * @return
     */
    public boolean isAllEnterSettling()
    {
        for (ActivityScheduleUsInfo usInfo : _m_usList)
        {
            if (!usInfo.hadEnterSettle())
                return false;
        }
        return true;
    }

    /**
     * 获取跨服实例数据（本服分组无需处理）
     *
     * 守卫检查：
     * - 已废弃的分组不应再获取跨服实例
     *
     * @param _handler 回调处理器
     */
    private void _getCross(_ICallBackBool _handler)
    {
        // 已废弃的分组，直接返回成功
        if (_m_hadDiscarded)
        {
            _handler.onRunOver(true);
            return;
        }

        // 本服分组，不需要获取跨服数据
        if (_m_usList.size() == 1)
        {
            _handler.onRunOver(true);
            return;
        }

        // 已获取跨服数据
        if (_m_crossInstanceId > 0)
        {
            _handler.onRunOver(true);
            return;
        }

        //发起CS请求，获取跨服数据
        NP2CS_R_006_001_ReqCrossInstance proto = new NP2CS_R_006_001_ReqCrossInstance();

        NPScheduleServer.getInstance().sendRequestToBSServer(EServerType.SINGLE.ordinal(), ENPSingleServerType.COMMON.ordinal(), proto,
                new _IWCGCallbackDealer()
                {
                    @Override
                    public void dealSuc(_IALProtocolStructure _proto)
                    {
                        NP2CS_RB_006_001_RetCrossInstance ret = (NP2CS_RB_006_001_RetCrossInstance) _proto;

                        _m_crossInstanceId = ret.getCrossInstanceId();

                        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
                        updateValue.addValueObj("cross_instance_id", _m_crossInstanceId);
                        getBM().getBM(ActivityScheduleUsGroupBO.class).update("id", _m_dbId, updateValue);

                        _handler.onRunOver(true);
                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                        CommLog.error("scheduleId:{} usGroup:{} get cross fail, errCode:{}", getScheduleDbId(), getGroupDbId(), _errCode);
                        _handler.onRunOver(false);
                    }

                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2CS_RB_006_001_RetCrossInstance();
                    }
                });
    }

    /**
     * 获取游戏逻辑主体实例ID
     * 是否有游戏逻辑主体通过配置（对应字段：RefActivity.is_game_logic）
     * 实例ID需要带上GameLogic服务器ID，以便GameLogic服务器正确识别
     * @param _handler
     */
    private void _getGameLogic(_ICallBackBool _handler)
    {
        //检查对应的活动配置数据
        RefActivity ref = RefActivity.getMgr().get(_m_scheduleData.getBO().getActivityId());
        if(null == ref)
        {
            CommLog.error("scheduleId:{} usGroup:{} get RefActivity fail, activityId:{}",
                    getScheduleDbId(), getGroupDbId(), _m_scheduleData.getBO().getActivityId());
            _handler.onRunOver(false);
            return;
        }

        //已获取游戏主体
        if (_m_gameLogicInstanceId > 0)
        {
            _handler.onRunOver(true);
            return;
        }

        //无需游戏逻辑主体
        if(!ref.is_game_logic)
        {
            _handler.onRunOver(true);
            return;
        }

        //发起CS请求，获取跨服数据
        NP2CS_R_007_001_ReqGameLogicInstance proto = new NP2CS_R_007_001_ReqGameLogicInstance();
        proto.setGroupId(getGroupDbId());
        proto.setActivityId(getScheduleData().getBO().getActivityId());

        NPScheduleServer.getInstance().sendRequestToBSServer(EServerType.SINGLE.ordinal(), ENPSingleServerType.COMMON.ordinal(), proto,
                new _IWCGCallbackDealer()
                {
                    @Override
                    public void dealSuc(_IALProtocolStructure _proto)
                    {
                        NP2CS_RB_007_001_RetGameLogicInstance ret = (NP2CS_RB_007_001_RetGameLogicInstance) _proto;

                        _m_gameLogicInstanceId = ret.getInstanceId();

                        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
                        updateValue.addValueObj("game_logic_instance_id", _m_gameLogicInstanceId);
                        getBM().getBM(ActivityScheduleUsGroupBO.class).update("id", _m_dbId, updateValue);

                        _handler.onRunOver(true);
                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                        CommLog.error("scheduleId:{} usGroup:{} get game logic fail, errCode:{}", getScheduleDbId(), getGroupDbId(), _errCode);
                        _handler.onRunOver(false);
                    }

                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2CS_RB_007_001_RetGameLogicInstance();
                    }
                });
    }

    /**
     * 开始推送
     */
    public void startPush()
    {
        if (_m_isProcessPush)
            return;

        _m_isProcessPush = true;

        // 构造US列表
        ALProcess process = ALProcess.CreateProcess("schedule-usGroup-push");

        //1.获取跨服实例数据
        process.addResDelegateProcess(_doneAction -> _getCross(_doneAction::dealAction), "schedule-usGroup-push-getCross"
                , () -> CommLog.error("scheduleId:{} usGroup:{} get cross fail.", getScheduleDbId(), getGroupDbId()), false);

        //2.获取游戏逻辑主体实例数据
        process.addResDelegateProcess(_doneAction -> _getGameLogic(_doneAction::dealAction), "schedule-usGroup-push-getGameLogic"
                , () -> CommLog.error("scheduleId:{} usGroup:{} get game logic fail.", getScheduleDbId(), getGroupDbId()), false);

        //3.推送分组信息
        //创建对应长度的process列表
        ALProcess[] list = new ALProcess[_m_usList.size()];
        for (int i = 0; i < _m_usList.size(); i++)
        {
            ActivityScheduleUsInfo usInfo = _m_usList.get(i);
            list[i] = ALProcess.CreateProcess("schedule-usGroup-push_sub");
            list[i].addResDelegateProcess(action ->
                            new ActivitySchedulePushTask(toUsSchedulePushProto(), usInfo, action).run(),
                    "schedule-usGroup-push_process_" + i);
        }
        process.addMultiProcess("schedule-usGroup-push_main", list);

        //开启执行
        process.dealProcess(new _IEZProcessMonitor()
        {
            //异常终止的事件函数
            @Override
            public void onRootProecssStop()
            {
                NPScheduleServer.getInstance().getDDAlert()
                        .err("ActivityScheduleUsGroup dealPush", "ActivityScheduleUsGroup dealPush process stop scheduleId:{}", getScheduleDbId());

                _m_isProcessPush = false;
            }

            @Override
            public void onRootProecssSuc()
            {
                CommLog.info("scheduleId:{} usGroup:{} push suc.", getScheduleDbId(), getGroupDbId());

                _m_isProcessPush = false;
            }

            @Override
            public void onErr(long _processTimeMS, String _processTag, String _exInfo, Exception _ex)
            {
                CommLog.error("scheduleId:{} usGroup:{} push err:{} ", getScheduleDbId(), getGroupDbId(), _ex.getMessage());
            }
        });
    }

    /**
     * 通知分组可发放奖励
     */
    public void noticeCanSendReward()
    {
        //通知可以发放奖励
        for (ActivityScheduleUsInfo usInfo : _m_usList)
        {
            ALSynTaskManager.getInstance().regTask(new NoticeCanSendRewardTask(getScheduleDbId(), usInfo.getUsId()));
        }
    }

    /**
     * 创建活动信息对象 - 从排期数据构造活动信息
     * @return Schedule_ActivityInfo 活动信息对象
     */
    private Schedule_ActivityInfo createActivityInfo()
    {
        Schedule_ActivityInfo activityInfo = new Schedule_ActivityInfo();
        activityInfo.setActivityId(_m_scheduleData.getBO().getActivityId());
        activityInfo.setStartTimeMs(_m_scheduleData.getBO().getStartTimeMs());
        activityInfo.setEndTimeMs(_m_scheduleData.getBO().getEndTimeMs());
        activityInfo.setCloseTimeMs(_m_scheduleData.getBO().getCloseTimeMs());
        return activityInfo;
    }

    /**
     * 构造US推送协议对象 - 将活动排期数据转换为下发协议
     * <p>
     * 执行流程：
     * 1. 创建Schedule_UsPush协议对象
     * 2. 设置排期基础信息（排期ID、分组ID、跨服实例ID）
     * 3. 设置活动信息对象
     * 4. 收集所有US ID列表
     * 5. 设置资源文件列表
     * @return Schedule_UsPush 完整的推送协议对象
     */
    public Schedule_UsPushData toUsSchedulePushProto()
    {
        Schedule_UsPushData pushProto = new Schedule_UsPushData();

        // 设置排期基础信息
        pushProto.setScheduleId(_m_scheduleData.getScheduleId());
        pushProto.setUsGroupId(getGroupDbId());
        pushProto.setCrossInstanceId(_m_crossInstanceId);
        pushProto.setGameLogicInstanceId(_m_gameLogicInstanceId);
        // 构造活动信息对象
        pushProto.setActivity(createActivityInfo());
        // 收集US列表
        for (ActivityScheduleUsInfo usInfo : _m_usList)
            pushProto.addUsIdList(usInfo.getUsId());
        pushProto.setResFile(_m_resFileName);
        pushProto.setResFileMd5(_m_resFileMd5);
        pushProto.setResFileDir(_m_resFileDir);
        pushProto.setSubmitCount(_m_scheduleData.getSubmitCount());
        return pushProto;
    }

    /**
     * 输出分组状态信息 - 用于调试和日志输出
     *
     * 输出内容：
     * 1. 分组基本信息（分组ID、排期ID、跨服实例ID）
     * 2. 资源文件信息
     * 3. 所有US的状态详情（按生命周期顺序：push -> playing -> settle -> done）
     *
     * @return 包含分组基本信息和所有US状态的字符串
     */
    @Override
    public String toString()
    {
        StringBuilder sb = new StringBuilder();
        sb.append("ActivityScheduleUsGroup{");
        sb.append("groupDbId=").append(_m_dbId);
        sb.append(", scheduleDbId=").append(getScheduleDbId());
        sb.append(", crossInstanceId=").append(_m_crossInstanceId);
        sb.append(", gameLogicInstanceId=").append(_m_gameLogicInstanceId);
        sb.append(", resFileName='").append(_m_resFileName).append('\'');
        sb.append(", usCount=").append(_m_usList.size());
        sb.append(", usList=[");

        for (int i = 0; i < _m_usList.size(); i++)
        {
            ActivityScheduleUsInfo usInfo = _m_usList.get(i);
            if (i > 0)
            {
                sb.append(", ");
            }
            sb.append("{usId=").append(usInfo.getUsId());
            sb.append(", push=").append(usInfo.hadPush());
            sb.append(", playing=").append(usInfo.hadEnterPlaying());
            sb.append(", settle=").append(usInfo.hadEnterSettle());
            sb.append(", done=").append(usInfo.hadDone());
            sb.append("}");
        }

        sb.append("]}");
        return sb.toString();
    }

    /**
     * 获取缓存的有序UsId数组
     *
     * 性能优化：
     * - 懒加载机制，仅在首次调用时构建
     * - 缓存后续直接返回，避免重复构建
     * - 使用原始int[]数组，减少内存占用
     *
     * @return 有序的UserServer ID数组
     */
    public List<Integer> getSortedUsIdList()
    {
        if (_m_sortedUsIdList == null)
        {
            _m_sortedUsIdList = new ArrayList<>();
            for (ActivityScheduleUsInfo activityScheduleUsInfo : _m_usList)
            {
                _m_sortedUsIdList.add(activityScheduleUsInfo.getUsId());
            }
            _m_sortedUsIdList.sort(Integer::compare);
        }
        return _m_sortedUsIdList;
    }

    /**
     * 更新资源信息（热更新）
     *
     * 执行流程：
     * 1. 更新内存缓存的资源字段
     * 2. 使用ALMySqlUpdateValue进行增量数据库更新
     *
     * @param resFile 资源文件名
     * @param resMd5  资源文件MD5
     * @param resDir  资源文件目录
     *
     * 线程安全：通过BM事务保证数据一致性
     */
    public void updateResInfo(String resFile, String resMd5, String resDir)
    {
        String origResFile = _m_resFileName;

        // 更新内存数据
        _m_resFileName = resFile;
        _m_resFileMd5 = resMd5;
        _m_resFileDir = resDir;

        // 使用ALMySqlUpdateValue增量更新数据库
        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("res_file_name", _m_resFileName);
        updateValue.addValueObj("res_file_md5", _m_resFileMd5);
        updateValue.addValueObj("res_file_dir", _m_resFileDir);

        getBM().getBM(ActivityScheduleUsGroupBO.class).update("id", _m_dbId, updateValue);

        CommLog.info("scheduleId:{} usGroup:{} update res info suc. origResFile:{} newResFile:{}",
                getScheduleDbId(), getGroupDbId(), origResFile, _m_resFileName);
    }

    /**
     * 开始处理废弃流程
     *
     * 执行流程：
     * 1. 幂等性检查：已废弃则直接返回
     * 2. 检查是否已在处理中，避免重复执行
     * 3. 本服分组：持久化 -> 更新内存 -> 更新计数器
     * 4. 跨服分组：发送销毁请求到CommonServer
     * 5. 成功后：持久化 -> 更新内存 -> 更新计数器
     * 6. 失败时重置处理标记，允许后续重试
     *
     * 重要：
     * - crossInstanceId 保留不清零，作为业务数据保存
     * - 持久化顺序：先DB后内存后计数器，确保崩溃恢复一致性
     */
    public void startProcessDiscard()
    {
        // 幂等性检查：已废弃则直接返回
        if (_m_hadDiscarded)
            return;

        if (_m_isProcessDiscard)
            return;

        _m_isProcessDiscard = true;

        //发起销毁流程
        ALProcess process = ALProcess.CreateProcess("schedule-discard");

        //1.销毁跨服实例数据
        process.addResDelegateProcess(_doneAction -> _discardCross(_doneAction::dealAction), "schedule-discard-_discardCross"
                , () -> CommLog.error("scheduleId:{} usGroup:{} discard cross fail.", getScheduleDbId(), getGroupDbId()), false);

        //2.销毁游戏逻辑主体实例数据
        process.addResDelegateProcess(_doneAction -> _discardGameLogic(_doneAction::dealAction), "schedule-discard-_discardGameLogic"
                , () -> CommLog.error("scheduleId:{} usGroup:{} discard game logic fail.", getScheduleDbId(), getGroupDbId()), false);

        //开启执行
        process.dealProcess(new _IEZProcessMonitor()
        {
            //异常终止的事件函数
            @Override
            public void onRootProecssStop()
            {
                CommLog.error("ActivityScheduleUsGroup.startProcessDiscard - discard failed: scheduleId={}, groupId={}, crossInstanceId={}, gameLogicInstanceId={}",
                        getScheduleDbId(), getGroupDbId(), _m_crossInstanceId, _m_gameLogicInstanceId);

                // 失败后重置标记，允许后续重试
                _m_isProcessDiscard = false;
            }

            @Override
            public void onRootProecssSuc()
            {
                // 1. 更新内存状态
                _m_hadDiscarded = true;
                _m_isProcessDiscard = false;

                // 2. 先持久化到数据库
                ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
                updateValue.addValueObj("had_discarded", 1);
                getBM().getBM(ActivityScheduleUsGroupBO.class).update("id", _m_dbId, updateValue);

                // 3. 通知父对象增加废弃计数器
                _m_scheduleData.incrementDiscardedCount();

                CommLog.info("ActivityScheduleUsGroup.startProcessDiscard - discarded suc: scheduleId={}, groupId={}",
                        getScheduleDbId(), getGroupDbId());
            }

            @Override
            public void onErr(long _processTimeMS, String _processTag, String _exInfo, Exception _ex)
            {
                CommLog.error("scheduleId:{} usGroup:{} discard err:{} ", getScheduleDbId(), getGroupDbId(), _ex.getMessage());
            }
        });
    }

    /**
     * 销毁跨服实例
     * @param _handler
     */
    private void _discardCross(_ICallBackBool _handler)
    {
        if (_m_crossInstanceId == 0)
        {
            _handler.onRunOver(true);
            return;
        }

        // 跨服分组：发起CS请求，销毁跨服实例
        NP2CS_R_006_002_ReqDiscardCrossInstance proto = new NP2CS_R_006_002_ReqDiscardCrossInstance();
        proto.setCrossInstanceId(_m_crossInstanceId);

        NPScheduleServer.getInstance().sendRequestToBSServer(EServerType.SINGLE.ordinal(), ENPSingleServerType.COMMON.ordinal(), proto,
                new _IWCGCallbackDealer()
                {
                    @Override
                    public void dealSuc(_IALProtocolStructure _proto)
                    {
                        // 1. 更新内存状态
                        long preCrossInstanceId = _m_crossInstanceId;
                        _m_crossInstanceId = 0;

                        // 2. 先持久化到数据库
                        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
                        updateValue.addValueObj("cross_instance_id", 0);
                        getBM().getBM(ActivityScheduleUsGroupBO.class).update("id", _m_dbId, updateValue);

                        CommLog.info("ActivityScheduleUsGroup.startProcessDiscard - discard cross success: scheduleId={}, groupId={}, crossInstanceId={}",
                                getScheduleDbId(), getGroupDbId(), preCrossInstanceId);

                        _handler.onRunOver(true);
                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                        CommLog.error("ActivityScheduleUsGroup.startProcessDiscard - discard cross failed: scheduleId={}, groupId={}, crossInstanceId={}, errorCode={}",
                                getScheduleDbId(), getGroupDbId(), _m_crossInstanceId, _errCode);

                        _handler.onRunOver(false);
                    }

                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2CS_RB_006_002_RetDiscardCrossInstance();
                    }
                });
    }

    /**
     * 销毁游戏逻辑实例
     * @param _handler
     */
    private void _discardGameLogic(_ICallBackBool _handler)
    {
        if (_m_gameLogicInstanceId == 0)
        {
            _handler.onRunOver(true);
            return;
        }

        // 跨服分组：发起CS请求，销毁跨服实例
        NP2CS_R_007_002_ReqDiscardGameLogicInstance proto = new NP2CS_R_007_002_ReqDiscardGameLogicInstance();
        proto.setInstanceId(_m_gameLogicInstanceId);

        NPScheduleServer.getInstance().sendRequestToBSServer(EServerType.SINGLE.ordinal(), ENPSingleServerType.COMMON.ordinal(), proto,
                new _IWCGCallbackDealer()
                {
                    @Override
                    public void dealSuc(_IALProtocolStructure _proto)
                    {
                        // 1. 更新内存状态
                        long preInstanceId = _m_gameLogicInstanceId;
                        _m_gameLogicInstanceId = 0;

                        // 2. 先持久化到数据库
                        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
                        updateValue.addValueObj("game_logic_instance_id", 0);
                        getBM().getBM(ActivityScheduleUsGroupBO.class).update("id", _m_dbId, updateValue);

                        CommLog.info("ActivityScheduleUsGroup.startProcessDiscard - discard game-logic success: scheduleId={}, groupId={}, gameLogicInstanceId={}",
                                getScheduleDbId(), getGroupDbId(), preInstanceId);

                        _handler.onRunOver(true);
                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                        CommLog.error("ActivityScheduleUsGroup.startProcessDiscard - discard game-logic failed: scheduleId={}, groupId={}, gameLogicInstanceId={}, errorCode={}",
                                getScheduleDbId(), getGroupDbId(), _m_gameLogicInstanceId, _errCode);

                        _handler.onRunOver(false);
                    }

                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2CS_RB_007_002_RetDiscardGameLogicInstance();
                    }
                });
    }
}
