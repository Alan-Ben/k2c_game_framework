package NPUSServer.NPUSUserMgr.UserComp.TargetRewardComp;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.ActivityEnum.EActivityState;
import Common.CommonFuncObj.CommonFunc_TargetReward;
import EventSystem.NPHandlerEntry;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.PlayerErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Log.CommLog;
import NPCommon.Util.Delegate.HandlerTwo;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPEnum.ENCounterDealType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.LogicEvent.MetaData.EventMeta;
import NPGameRes.LogicEvent.MetaData.EventMetaMgr_Refdata;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.Refs.Common.RefCommonTargetReward;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.EventParamVarTypeMap;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.VariableDealer.NPPlayerVariableDeal;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_007_CommOp;
import USDB.Bo.PlayerTargetRewardBO;

import java.util.ArrayList;
import java.util.List;

public class TargetRewardInfo implements _IHandlerHolder
{
    private TargetRewardComponent _m_comp;
    private RefCommonTargetReward _m_ref;

    //成就数据
    private long _m_dbId;
    private long _m_counter;
    private boolean _m_hadDraw;
    private long _m_activityInstanceId;
    //触发事件对象列表
    private ArrayList<NPHandlerEntry<NPUSUserData>> _m_evtEntryList;

    public TargetRewardInfo(TargetRewardComponent _comp, PlayerTargetRewardBO _bo, RefCommonTargetReward _ref)
    {
        this(_comp, _ref);
        _m_dbId = _bo.getId();
        _m_counter = _bo.getExtraCount();
        _m_hadDraw = _bo.getHadDraw();
        _m_activityInstanceId = _bo.getActivityInstanceId();
    }

    public TargetRewardInfo(TargetRewardComponent _comp, RefCommonTargetReward _ref)
    {
        _m_comp = _comp;
        _m_ref = _ref;

        _m_evtEntryList = new ArrayList<>();
    }

    public long getRefId()
    {
        return _m_ref.Id();
    }

    /**
     * 注册监听
     */
    protected void regEvtEntry()
    {
        if (_m_ref.trigger_event.isEmpty())
            return;

        EventMeta eventMeta = EventMetaMgr_Refdata.getInstance().lookupMetaByEventName(_m_ref.trigger_event.toUpperCase());
        if (eventMeta == null)
        {
            CommLog.error("TargetRewardInfo regEvtEntry failed, event not found, targetRewardId:{} event:{}", _m_ref.Id(), _m_ref.trigger_event, new Exception());
            return;
        }

        //注册事件监听
        NPHandlerEntry<NPUSUserData> evtEntry = _m_comp.getUserData().getEventHandlerMgr().regHandler(eventMeta.getEventId()
                , this, new HandlerTwo<_ALogicEventBase, NPUSUserData>()
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

    /**
     * 注销监听
     */
    protected void unRegEvtEntry()
    {
        for (int i = _m_evtEntryList.size() - 1; i >= 0; i--)
        {
            NPHandlerEntry<NPUSUserData> entry = _m_evtEntryList.get(i);
            if (null == entry)
                continue;

            _m_comp.getUserData().getEventHandlerMgr().unregHandler(entry);
        }
    }

    /**
     * 监听事件的处理
     * @param _evt     事件对象
     * @param _context 玩家上下文
     */
    protected void onLogicEvt(_ALogicEventBase _evt, NPPlayerContext _context)
    {
    	NPVarInfo varInfo = EventParamVarTypeMap.getInstance().makeVarInfo(_evt);
    	
        //检查触发条件
        if (!NPPlayerConditionDealerMgr.IsEnable(_m_ref.trigger_condition, _m_comp.getUserData(), varInfo))
            return;

        //增加任务计数
        long chgCount = _evt.getValue(_m_ref.trigger_count_rate);
        if (_m_ref.is_set)
        {
            setCount(chgCount, _context);
        } else
        {
            addCount(chgCount, _context);
        }
    }

    /**
     * 更新步骤目标计数
     * @param _chgCount 目标计数变化值
     * @param _dealType 变化类型
     */
    protected void chgCount(long _chgCount, ENCounterDealType _dealType)
    {
        //无数据变化不处理
        if (_chgCount == 0)
            return;

        //原有目标计数
        long oriCount = _m_counter;

        //计算当前计数
        long curCount = 0;
        switch (_dealType)
        {
            case REDUCE:
                curCount = oriCount - _chgCount;
                break;
            case SET:
                curCount = _chgCount;
                break;
            case SET_GT:
                curCount = Math.max(oriCount, _chgCount);
                break;
            case ADD:
            default:
                curCount = oriCount + _chgCount;
                break;
        }

        _m_counter = curCount;

        //推送协议
        _m_comp.getUserData().sendMsgToGC(US2GCWriter_007_CommOp.make_062_OnTargetRewardUpdate(toProto()));

        BM bmObj = _m_comp.getUSServer().getBM();

        //更新数据表计数
        if (_m_dbId == 0) //创建数据
        {
            PlayerTargetRewardBO bo = new PlayerTargetRewardBO();
            bo.setCid(bmObj, _m_comp.getUserData().getCid());
            bo.setRefId(bmObj, _m_ref.Id());
            bo.setExtraCount(bmObj, _m_counter);
            bo.insert(bmObj);

            _m_dbId = bo.getId();
        } else //更新数据
        {
            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("extra_count", _m_counter);
            bmObj.getBM(PlayerTargetRewardBO.class).update("id", _m_dbId, updateValue);
        }
    }

    public void addCount(long _chgCount, NPPlayerContext _context)
    {
        chgCount(_chgCount, ENCounterDealType.ADD);
    }

    public void reduceCount(long _chgCount, NPPlayerContext _context)
    {
        chgCount(_chgCount, ENCounterDealType.REDUCE);
    }

    public void setCount(long _chgCount, NPPlayerContext _context)
    {
        chgCount(_chgCount, ENCounterDealType.SET);
    }

    public void setGtCount(long _chgCount, NPPlayerContext _context)
    {
        chgCount(_chgCount, ENCounterDealType.SET_GT);
    }

    protected void reset()
    {
        _m_counter = 0;
        _m_hadDraw = false;

        //推送协议
        _m_comp.getUserData().sendMsgToGC(US2GCWriter_007_CommOp.make_062_OnTargetRewardUpdate(toProto()));

        if (_m_dbId == 0)
            return;

        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("counter", 0);
        updateValue.addValueObj("had_draw", _m_hadDraw ? 1 : 0);
        _m_comp.getUSServer().getBM().getBM(PlayerTargetRewardBO.class).update("id", _m_dbId, updateValue);
    }

    /**
     * 领取奖励
     * @param _context
     * @return
     */
    public Result drawReward(NPPlayerContext _context)
    {
        if (_m_hadDraw)
            return PlayerErr.TARGET_REWARD_HAD_DRAW;

        //检查是否满足领取条件
        if (getCurCounter() < _m_ref.process_count)
            return PlayerErr.TARGET_REWARD_NOT_REACH;

        //保存已经领取
        saveHadDraw();

        _m_comp.getUserData().gainItemList(_m_ref.gain_item_list, _context);

        return Result.SUCC;
    }

    /**
     * 保存已经领取
     */
    private void saveHadDraw()
    {
        _m_hadDraw = true;

        //推送协议
        _m_comp.getUserData().sendMsgToGC(US2GCWriter_007_CommOp.make_062_OnTargetRewardUpdate(toProto()));

        BM bmObj = _m_comp.getUSServer().getBM();

        //更新数据表计数
        if (_m_dbId == 0) //创建数据
        {
            PlayerTargetRewardBO bo = new PlayerTargetRewardBO();
            bo.setCid(bmObj, _m_comp.getUserData().getCid());
            bo.setRefId(bmObj, _m_ref.Id());
            bo.setHadDraw(bmObj, _m_hadDraw);
            bo.insert(bmObj);

            _m_dbId = bo.getId();
        } else //更新数据
        {
            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("had_draw", _m_hadDraw ? 1 : 0);
            bmObj.getBM(PlayerTargetRewardBO.class).update("id", _m_dbId, updateValue);
        }
    }

    /**
     * 获取当前计数:高级公式 + 任务计数
     * @return
     */
    public long getCurCounter()
    {
        //高级公式
        long count = NPPlayerVariableDeal.getInstance().CalculateVariableResult(_m_comp.getUserData(), _m_ref.process_cur_count, null);

        //高级公式计数 + 成就步骤计数
        return (count + _m_counter);
    }

    /**
     * 生成协议数据
     * @return
     */
    public CommonFunc_TargetReward toProto()
    {
        return new CommonFunc_TargetReward(_m_ref.Id(), _m_counter, _m_hadDraw);
    }

    /**
     * 检查活动状态
     */
    public void initCheck()
    {
        if (_m_ref.activity_id == 0)
            return;

        //区分是否有活动实例
        if (_m_activityInstanceId == 0)
        {
            //查找一个活动用于初始化
            List<_AActivityBase> activityList = _m_comp.getUserData().getUSServer().getCommActivityMgr().lookupActivityByActivityId(_m_ref.activity_id);
            for (_AActivityBase activity : activityList)
            {
                if (!activity.isRunning())
                    continue;

                saveActivityInstanceId(activity.getInstanceId());
                break;
            }
        } else
        {
            _AActivityBase activity = _m_comp.getUserData().getUSServer().getCommActivityMgr().lookupActivity(_m_activityInstanceId);
            if (activity == null || !activity.isRunning())
            {
                saveActivityInstanceId(0);
                unRegEvtEntry();
            }
        }

        //如果有活动实例ID，注册事件
        if (_m_activityInstanceId != 0)
            regEvtEntry();
    }

    /**
     * 保存活动实例ID
     * @param _instanceId
     */
    private void saveActivityInstanceId(long _instanceId)
    {
        _m_activityInstanceId = _instanceId;
        _m_counter = 0;
        _m_hadDraw = false;

        BM bmObj = _m_comp.getUSServer().getBM();
        //更新数据表计数
        if (_m_dbId == 0) //创建数据
        {
            PlayerTargetRewardBO bo = new PlayerTargetRewardBO();
            bo.setCid(bmObj, _m_comp.getUserData().getCid());
            bo.setRefId(bmObj, _m_ref.Id());
            bo.setActivityInstanceId(bmObj, _m_activityInstanceId);
            bo.insert(bmObj);

            _m_dbId = bo.getId();
        } else //更新数据
        {
            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("extra_count", _m_counter);
            updateValue.addValueObj("had_draw", _m_hadDraw ? 1 : 0);
            updateValue.addValueObj("activity_instance_id", _m_activityInstanceId);
            _m_comp.getUSServer().getBM().getBM(PlayerTargetRewardBO.class).update("id", _m_dbId, updateValue);
        }

        //推送协议
        _m_comp.getUserData().sendMsgToGC(US2GCWriter_007_CommOp.make_062_OnTargetRewardUpdate(toProto()));
    }

    public void checkActivityState(Long _activityInstanceId, Long _activityId, EActivityState _state)
    {
        if (_m_ref.activity_id != _activityId)
            return;

        if (_m_activityInstanceId == _activityInstanceId)
        {
            if (_AActivityBase.CLOSE_STATE.contains(_state))
            {
                saveActivityInstanceId(0);
                unRegEvtEntry();
            }
        } else if (_m_activityInstanceId == 0)
        {
            if (_AActivityBase.RUNNING_STATE.contains(_state))
            {
                saveActivityInstanceId(_activityInstanceId);
                regEvtEntry();
            }
        }
    }
}
