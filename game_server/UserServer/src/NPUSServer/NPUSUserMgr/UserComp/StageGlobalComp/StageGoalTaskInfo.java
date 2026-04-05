package NPUSServer.NPUSUserMgr.UserComp.StageGlobalComp;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.StageGoalObj.StageGoalTask_Info;
import EventSystem.NPHandlerEntry;
import MJLog.MJEventLog;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.StageGoalErr;
import NPCommon.Log.CommLog;
import NPCommon.Util.Delegate.HandlerTwo;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.LogicEvent.MetaData.EventMeta;
import NPGameRes.LogicEvent.MetaData.EventMetaMgr_Refdata;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.Refs.StageGoal.RefStageGoalTask;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.EventParamVarTypeMap;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.VariableDealer.NPPlayerVariableDeal;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_007_CommOp;
import USDB.Bo.PlayerStageGoalTaskBO;

import java.util.ArrayList;

/******************
 * 阶段任务数据，管理任务计数
 *
 * @author mj
 *
 */
public class StageGoalTaskInfo implements _IHandlerHolder
{
    //任务组件
    private StageGoalComponent _m_comp;

    //任务数据
    private long _m_dbId;
    private long _m_step;
    private long _m_taskId;
    private long _m_counter;

    //任务配置数据
    private RefStageGoalTask _m_ref;
    //触发事件对象列表
    private ArrayList<NPHandlerEntry<NPUSUserData>> _m_evtEntryList;

    //任务奖励领取状态
    private boolean _m_rewardDrawed;

    /**************
     * 存入BO的数据
     *
     * @param _comp
     * @param _bo
     * @param _ref
     */
    protected StageGoalTaskInfo(StageGoalComponent _comp, PlayerStageGoalTaskBO _bo, RefStageGoalTask _ref)
    {
        _m_comp = _comp;

        _m_dbId = _bo.getId();
        _m_step = _bo.getStep();
        _m_taskId = _bo.getTaskId();
        _m_counter = _bo.getCounter();
        _m_rewardDrawed = _bo.getRewardDrawed();
        _m_ref = _ref;
        _m_evtEntryList = new ArrayList<>();
    }

    /**************
     * 未存入BO的数据
     *
     * @param _comp
     * @param _ref
     */
    protected StageGoalTaskInfo(StageGoalComponent _comp, long _step, RefStageGoalTask _ref)
    {
        _m_comp = _comp;

        _m_step = _step;
        _m_taskId = _ref.id;
        _m_counter = 0;
        _m_rewardDrawed = false;
        _m_ref = _ref;
        _m_evtEntryList = new ArrayList<>();
    }

    public StageGoalComponent getComp()
    {
        return _m_comp;
    }

    public long getDBID()
    {
        return _m_dbId;
    }

    public long getStep()
    {
        return _m_step;
    }

    public long getTaskId()
    {
        return _m_taskId;
    }

    public long getCounter()
    {
        return _m_counter;
    }

    public RefStageGoalTask getRef()
    {
        return _m_ref;
    }

    public StageGoalTask_Info toProto()
    {
        StageGoalTask_Info proto = new StageGoalTask_Info();
        proto.setTaskId(_m_taskId);
        proto.setCounter(_m_counter);
        proto.setHadDraw(_m_rewardDrawed);
        return proto;
    }

    /*****************
     * 增加计数
     *
     * @param _counter
     * @param _context
     */
    protected void incrCounter(long _counter, NPPlayerContext _context)
    {
        _m_comp.getUserData().lockUser();

        try
        {
            _m_counter += _counter;
            //更新存储数据
            update();

            _m_comp.getUserData().sendMsgToGC(US2GCWriter_007_CommOp.make_070_OnStageGoalTaskChg(this));
        } finally
        {
            _m_comp.getUserData().unlockUser();
        }
    }

    /*************
     * 设置计数
     *
     * @param _counter
     * @param _context
     */
    public void setCounter(long _counter, NPPlayerContext _context)
    {
        _m_comp.getUserData().lockUser();

        try
        {
            if (_m_counter == _counter)
                return;

            _m_counter = _counter;
            //更新存储数据
            update();

            _m_comp.getUserData().sendMsgToGC(US2GCWriter_007_CommOp.make_070_OnStageGoalTaskChg(this));
        } finally
        {
            _m_comp.getUserData().unlockUser();
        }
    }

    protected void update()
    {
        if (0 == _m_dbId)
        {
            BM bmObj = _m_comp.getUSServer().getBM();

            PlayerStageGoalTaskBO bo = new PlayerStageGoalTaskBO();
            bo.setCid(bmObj, _m_comp.getUserData().getCid());
            bo.setStep(bmObj, _m_step);
            bo.setTaskId(bmObj, _m_taskId);
            bo.setCounter(bmObj, _m_counter);
            bo.setRewardDrawed(bmObj, _m_rewardDrawed);
            bo.insert(bmObj);

            _m_dbId = bo.getId();
        } else
        {
            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("counter", _m_counter);
            updateValue.addValueObj("reward_drawed", _m_rewardDrawed ? 1 : 0);

            _m_comp.getUSServer().getBM().getBM(PlayerStageGoalTaskBO.class).update("id", _m_dbId, updateValue);
        }
    }

    /******************
     * 注册事件监听
     */
    protected void regEvtEntry()
    {
        for (String event : _m_ref.trigger_event)
        {
            EventMeta eventMeta = EventMetaMgr_Refdata.getInstance().lookupMetaByEventName(event.toUpperCase());
            if (eventMeta == null)
            {
                CommLog.error("StageGoalTaskInfo regEvtEntry failed, event not found, taskId:{} event:{}", getRef().Id(), event, new Exception());
                continue;
            }

            //注册事件监听
            NPHandlerEntry<NPUSUserData> evtEntry = _m_comp.getUserData().getEventHandlerMgr().regHandler(eventMeta.getEventId(), this,
                    new HandlerTwo<_ALogicEventBase, NPUSUserData>()
                    {
                        @Override
                        public void handle(_ALogicEventBase _evt, NPUSUserData _userData)
                        {
                            NPPlayerContext context = (NPPlayerContext) _evt.getContext();
                            onLogicEvt(_evt, context);
                        }
                    });

            _m_evtEntryList.add(evtEntry);
        }
    }

    /*********************
     * 注销事件监听
     */
    protected void unregEvtEntry()
    {
        for (int i = 0; i < _m_evtEntryList.size(); i++)
        {
            NPHandlerEntry<NPUSUserData> entry = _m_evtEntryList.get(i);
            if (null == entry)
                continue;

            _m_comp.getUserData().getEventHandlerMgr().unregHandler(entry);
        }
    }

    /*****************
     * 监听事件的处理
     *
     * @param _evt
     * @param _context
     */
    protected void onLogicEvt(_ALogicEventBase _evt, NPPlayerContext _context)
    {
    	NPVarInfo varInfo = EventParamVarTypeMap.getInstance().makeVarInfo(_evt);
    	
        //检查触发条件
        if (!NPPlayerConditionDealerMgr.IsEnable(_m_ref.trigger_condition, _m_comp.getUserData(), varInfo))
            return;

        //计算任务计数
        long chgCounter = _evt.getValue(_m_ref.trigger_count_rate);
        if (chgCounter != 0)
        {
            if (_m_ref.is_set)
            {
                setCounter(chgCounter, _context);
            } else
            {
                incrCounter(chgCounter, _context);
            }
        }
    }

    /**
     * 获取当前计数:高级公式 + 任务计数
     * @return
     */
    public long getTotalCounter()
    {
        //高级公式
        long count = NPPlayerVariableDeal.getInstance().CalculateVariableResult(_m_comp.getUserData(), _m_ref.process_cur_count, null);

        //高级公式计数 + 任务目标计数
        return (count + _m_counter);
    }

    /**
     * 是否完成
     * @return
     */
    public boolean canDone()
    {
        return getTotalCounter() >= _m_ref.process_count;
    }

    /**
     * 是否已领取任务奖励
     * @return
     */
    public boolean isRewardDrawed()
    {
        return _m_rewardDrawed;
    }

    /**
     * 领取任务奖励
     * @param _context
     * @return
     */
    public Result drawReward(NPPlayerContext _context)
    {
        // 检查任务是否完成
        if (!canDone())
            return StageGoalErr.STAGE_GOAL_TASK_NOT_DONE;

        // 检查奖励是否已领取
        if (_m_rewardDrawed)
            return StageGoalErr.STAGE_GOAL_TASK_ALREADY_DONE;

        _m_comp.getUserData().gainItemList(_m_ref.reward_item_list, _context);
        _m_rewardDrawed = true;
        update();

        _m_comp.getUserData().sendMsgToGC(US2GCWriter_007_CommOp.make_070_OnStageGoalTaskChg(this));

        MJEventLog.logTask(getComp().getUserData(), 3, Long.toString(getTaskId()));

        return Result.SUCC;
    }
}
