package NPUSServer.NPUSUserMgr.UserComp.AchieveComp;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.AchieveObj.Achieve_Info;
import Common.PlayerEnum.EPlayerEventRecordType;
import EventSystem.NPHandlerEntry;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.AchieveErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Log.CommLog;
import NPCommon.Util.Delegate.HandlerTwo;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPCommon.Util.NumberCompact.NumberCompressList;
import NPEnum.ENCounterDealType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.LogicEvent.MetaData.EventMeta;
import NPGameRes.LogicEvent.MetaData.EventMetaMgr_Refdata;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.Refs.Achieve.RefAchieve;
import NPGameRes.Refs.Achieve.RefAchieveStep;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.EventParamVarTypeMap;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.SynTask.NPSynPlayerEvnetRecordTask;
import NPUSServer.NPUSUserMgr.VariableDealer.NPPlayerVariableDeal;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_021_PlayerInfo;
import USDB.Bo.PlayerAchieveBO;

import java.util.ArrayList;

public class AchieveInfo implements _IHandlerHolder
{
    //组件对象
    private AchieveComponent _m_comp;
    //成就数据
    private long _m_dbId;
    private long _m_counter;
    //配置数据
    private RefAchieve _m_ref;
    //触发事件对象列表
    private ArrayList<NPHandlerEntry<NPUSUserData>> _m_evtEntryList;
    //成就步骤领取数据
    private NumberCompressList _m_hadDrawStepList;

    public AchieveInfo(AchieveComponent _comp, RefAchieve _ref, PlayerAchieveBO _bo)
    {
        _m_comp = _comp;
        _m_ref = _ref;
        _m_evtEntryList = new ArrayList<>();
        _m_hadDrawStepList = new NumberCompressList();

        //注册对事件的监听
        regEvtEntry();

        if (_bo != null)
        {
            _m_dbId = _bo.getId();
            _m_counter = _bo.getCounter();
            _m_hadDrawStepList.parseFromString(_bo.getHadDrawStepList());
        }
    }

    public AchieveComponent getComp()
    {
        return _m_comp;
    }

    public RefAchieve getRef()
    {
        return _m_ref;
    }

    public long getAchieveId()
    {
        return _m_ref.achieve_id;
    }

    public int getMaxDrawStepId()
    {
        return _m_hadDrawStepList.getMaxNum();
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
     * 注册监听
     */
    protected void regEvtEntry()
    {
        if (_m_ref.trigger_event.isEmpty())
            return;

        EventMeta eventMeta = EventMetaMgr_Refdata.getInstance().lookupMetaByEventName(_m_ref.trigger_event.toUpperCase());
        if (eventMeta == null)
        {
            CommLog.error("Achieve logic_event_id is null, please check, achieve_id:{} event:{}", _m_ref.Id(), _m_ref.trigger_event, new Exception());
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
        for (NPHandlerEntry<NPUSUserData> entry : _m_evtEntryList)
        {
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
     * 构造数据协议对象
     * @return 数据协议对象
     */
    public Achieve_Info toProto()
    {
        Achieve_Info proto = new Achieve_Info();
        proto.setAchieveId(getAchieveId());
        proto.setCounter(_m_counter);
        proto.getHadDrawStepList().addAll(_m_hadDrawStepList.getNumberList());
        return proto;
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
        getComp().getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_066_OnAchieveChg(toProto()));

        BM bmObj = getComp().getUSServer().getBM();

        //更新数据表计数
        if (_m_dbId == 0) //创建数据
        {
            PlayerAchieveBO bo = new PlayerAchieveBO();
            bo.setCid(bmObj, _m_comp.getUserData().getCid());
            bo.setAchieveId(bmObj, getAchieveId());
            bo.setCounter(bmObj, _m_counter);
            bo.insert(bmObj);

            _m_dbId = bo.getId();
        } else //更新数据
        {
            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("counter", _m_counter);
            bmObj.getBM(PlayerAchieveBO.class).update("id", _m_dbId, updateValue);
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
        _m_hadDrawStepList.clear();

        //推送协议
        getComp().getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_066_OnAchieveChg(toProto()));

        if (_m_dbId == 0)
            return;

        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("counter", 0);
        updateValue.addValueObj("had_draw_step_list", _m_hadDrawStepList.toString());
        getComp().getUSServer().getBM().getBM(PlayerAchieveBO.class).update("id", _m_dbId, updateValue);
    }

    /**
     * 领取阶段奖励
     * @param _ref     配置数据
     * @param _context 玩家上下文
     * @return 成功返回suc, 失败返回对应错误码
     */
    public Result drawStepReward(RefAchieveStep _ref, NPPlayerContext _context)
    {
        //检查是否已经领取过
        if (hasDrawStep(_ref.step))
            return AchieveErr.ACHIEVE_STEP_REWARD_HAD_DRAW;

        //检查是否满足领取条件
        if (getCurCounter() < _ref.process_count)
            return AchieveErr.ACHIEVE_STEP_NOT_SATISFIED;

        //保存已经领取过的步骤
        saveHadDrawStep(_ref.step);

        getComp().getUserData().gainItemList(_ref.done_item_list, _context);

        //记录领取成就阶段奖励次数
        NPSynPlayerEvnetRecordTask.syncRecord(getComp().getUserData(), ENCounterDealType.ADD,
                EPlayerEventRecordType.DRAW_ACHIEVE_STEP_REWARD.ordinal(), getAchieveId(), 1);

        return Result.SUCC;
    }

    /**
     * 领取国力目标成就阶段奖励
     * @param _ref
     * @param _context
     * @return
     */
    public Result drawGDPGoalStepReward(RefAchieveStep _ref, NPPlayerContext _context)
    {
        //检查是否已经领取过
        if (hasDrawStep(_ref.step))
            return AchieveErr.ACHIEVE_STEP_REWARD_HAD_DRAW;

        //保存已经领取过的步骤
        saveHadDrawStep(_ref.step);

        //针对情人需要特殊处理，其他物品照旧
        for (int i = 0; i < _ref.done_item_list.size(); i++)
        {
            NPCommonCostItem item = _ref.done_item_list.get(i);
            if (null == item)
                continue;

            getComp().getUserData().gainItem(item, _context);
        }

        //记录领取成就阶段奖励次数
        NPSynPlayerEvnetRecordTask.syncRecord(getComp().getUserData(), ENCounterDealType.ADD,
                EPlayerEventRecordType.DRAW_ACHIEVE_STEP_REWARD.ordinal(), getAchieveId(), 1);

        return Result.SUCC;
    }

    /**
     * 保存已经领取过的步骤
     * @param _step 阶段
     */
    public void saveHadDrawStep(int _step)
    {
        _m_hadDrawStepList.addNumber(_step);

        //推送协议
        getComp().getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_066_OnAchieveChg(toProto()));

        BM bmObj = getComp().getUSServer().getBM();

        //更新数据表计数
        if (_m_dbId == 0) //创建数据
        {
            PlayerAchieveBO bo = new PlayerAchieveBO();
            bo.setCid(bmObj, _m_comp.getUserData().getCid());
            bo.setAchieveId(bmObj, getAchieveId());
            bo.setCounter(bmObj, _m_counter);
            bo.setHadDrawStepList(bmObj, _m_hadDrawStepList.toString());
            bo.insert(bmObj);

            _m_dbId = bo.getId();
        } else //更新数据
        {
            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("had_draw_step_list", _m_hadDrawStepList.toString());
            bmObj.getBM(PlayerAchieveBO.class).update("id", _m_dbId, updateValue);
        }
    }

    /**
     * 返回是否已经领取
     * @param _step 阶段
     * @return true:已领取过 false:未领取过
     */
    public boolean hasDrawStep(int _step)
    {
        return _m_hadDrawStepList.containsNumber(_step);
    }
}
