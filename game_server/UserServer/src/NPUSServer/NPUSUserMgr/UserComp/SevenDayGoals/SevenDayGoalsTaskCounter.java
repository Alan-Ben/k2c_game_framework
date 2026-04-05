package NPUSServer.NPUSUserMgr.UserComp.SevenDayGoals;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.SimpleActivityObj.SevenDayGoals_TaskInfo;
import EventSystem.NPHandlerEntry;
import GS2GC.p033_SimpleActivityOp.GS2GC_033_103_OnSevenDayGoalTaskChg;
import NPCommon.DB.BM.BM;
import NPCommon.Log.CommLog;
import NPCommon.Util.Delegate.HandlerTwo;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPEnum.ENCounterDealType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.LogicEvent.MetaData.EventMeta;
import NPGameRes.LogicEvent.MetaData.EventMetaMgr_Refdata;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.Refs.SevenDayGoals.RefSevenDayGoalsTask;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.EventParamVarTypeMap;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import USDB.Bo.PlayerSevenDayGoalsTaskCounterBO;

public class SevenDayGoalsTaskCounter implements _IHandlerHolder
{
    private SevenDayGoalsComponent _m_comp;
    private RefSevenDayGoalsTask _m_ref;
    private NPHandlerEntry<NPUSUserData> _m_evtEntry;

    private long _m_dbId;
    private long _m_counter;

    public SevenDayGoalsTaskCounter(SevenDayGoalsComponent _comp, RefSevenDayGoalsTask _ref, PlayerSevenDayGoalsTaskCounterBO _bo)
    {
        this(_comp, _ref);
        _m_dbId = _bo.getId();
    }

    public SevenDayGoalsTaskCounter(SevenDayGoalsComponent _comp, RefSevenDayGoalsTask _ref)
    {
        _m_comp = _comp;
        _m_ref = _ref;
    }

    /**
     * 获取计数
     * @return
     */
    public long getExtraCount()
    {
        return _m_counter;
    }

    /**
     * 注册事件监听
     */
    public void regEvtEntry()
    {
        if (_m_evtEntry != null)
            return;

        EventMeta eventMeta = EventMetaMgr_Refdata.getInstance().lookupMetaByEventName(_m_ref.trigger_event.toUpperCase());
        if (eventMeta == null)
        {
            CommLog.error("SevenDayGoalsTaskCounter regEvtEntry failed, event not found, taskId:{} event:{}", _m_ref.Id(), _m_ref.trigger_event, new Exception());
            return;
        }

        //注册事件监听
        _m_evtEntry = _m_comp.getUserData().getEventHandlerMgr().regHandler(eventMeta.getEventId()
                , this, new HandlerTwo<_ALogicEventBase, NPUSUserData>()
                {
                    @Override
                    public void handle(_ALogicEventBase _evt, NPUSUserData _userData)
                    {
                        NPPlayerContext context = (NPPlayerContext) _evt.getContext();
                        onLogicEvt(_evt, context);
                    }
                });
    }

    /**
     * 注销监听
     */
    protected void unRegEvtEntry()
    {
        if (_m_evtEntry == null)
            return;

        _m_comp.getUserData().getEventHandlerMgr().unregHandler(_m_evtEntry);
        _m_evtEntry = null;
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
        _m_comp.getUserData().sendMsgToGC(new GS2GC_033_103_OnSevenDayGoalTaskChg(makeProto()));

        BM bmObj = _m_comp.getUSServer().getBM();

        //更新数据表计数
        if (_m_dbId == 0) //创建数据
        {
            PlayerSevenDayGoalsTaskCounterBO bo = new PlayerSevenDayGoalsTaskCounterBO();
            bo.setCid(bmObj, _m_comp.getUserData().getCid());
            bo.setTaskId(bmObj, _m_ref.Id());
            bo.setExtraCount(bmObj, _m_counter);
            bo.insert(bmObj);

            _m_dbId = bo.getId();
        } else //更新数据
        {
            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("extra_count", _m_counter);
            bmObj.getBM(PlayerSevenDayGoalsTaskCounterBO.class).update("id", _m_dbId, updateValue);
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

    /**
     * 构造协议
     * @return
     */
    public SevenDayGoals_TaskInfo makeProto()
    {
        SevenDayGoals_TaskInfo proto = new SevenDayGoals_TaskInfo();
        proto.setTaskId(_m_ref.Id());
        proto.setExtraCount(_m_counter);
        return proto;
    }
}
