package NPUSServer.NPUSUserMgr.UserComp.SystemQuestComp;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.QuestObj.SystemQuest_Info;
import EventSystem.NPHandlerEntry;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.QuestErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Log.CommLog;
import NPCommon.Util.Delegate.HandlerTwo;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPEnum.ENCounterDealType;
import NPEnum.ENpRewardShowType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.LogicEvent.MetaData.EventMeta;
import NPGameRes.LogicEvent.MetaData.EventMetaMgr_Refdata;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.Refs.Quest.SystemQuest.RefSystemQuest;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.EventParamVarTypeMap;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.EffectDealer.NPPlayerEffectDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.VariableDealer.NPPlayerVariableDeal;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_028_QuestOp;
import USDB.Bo.PlayerSystemQuestGroupBO;

public class SystemQuestGroupInfo implements _IHandlerHolder
{
    private SystemQuestComponent _m_comp;

    private long _m_dbId;
    private long _m_groupId;
    private int _m_step;
    private long _m_count;

    private RefSystemQuest _m_ref;

    //触发事件对象列表
    private NPHandlerEntry<NPUSUserData> _m_evtEntry;

    public SystemQuestGroupInfo(SystemQuestComponent _comp, long _groupId)
    {
        _m_comp = _comp;
        _m_groupId = _groupId;
        _m_step = 1;
        _m_ref = RefSystemQuest.getMgr().getQuestRef(_m_groupId, _m_step);
    }

    public SystemQuestGroupInfo(SystemQuestComponent _comp, PlayerSystemQuestGroupBO _bo)
    {
        _m_comp = _comp;
        _m_dbId = _bo.getId();
        _m_groupId = _bo.getGroupId();
        _m_step = _bo.getStep();
        _m_count = _bo.getCount();
        _m_ref = RefSystemQuest.getMgr().getQuestRef(_m_groupId, _m_step);
    }

    public long getGroupId()
    {
        return _m_groupId;
    }

    public int getStep()
    {
        return _m_step;
    }

    public NPUSUserData getUserData()
    {
        return _m_comp.getUserData();
    }

    protected void _regEvtEntry()
    {
       getUserData().lockUser();
       try
       {
            if (_m_ref == null || _m_evtEntry != null)
                return;

            if (_m_ref.trigger_event.isEmpty())
                return;

            EventMeta eventMeta = EventMetaMgr_Refdata.getInstance().lookupMetaByEventName(_m_ref.trigger_event.toUpperCase());
            if (eventMeta == null)
            {
                CommLog.error("SystemQuestGroupInfo regEvtEntry failed, event not found, questId:{} event:{}", _m_ref.Id(), _m_ref.trigger_event, new Exception());
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
       } finally
       {
           getUserData().unlockUser();
       }
    }

    protected void _unRegEvtEntry()
    {
        if (_m_evtEntry == null)
            return;

        if (_m_ref.trigger_event.isEmpty())
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
        getUserData().lockUser();
        try
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

            //触发额外效果
            NPPlayerEffectDealer.dealEffect(_m_ref.ext_trigger_effect, _m_comp.getUserData(), varInfo, _context);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    public void setCount(long _chgCount, NPPlayerContext _context)
    {
        chgCount(_chgCount, ENCounterDealType.SET);
    }

    public void addCount(long _chgCount, NPPlayerContext _context)
    {
        chgCount(_chgCount, ENCounterDealType.ADD);
    }

    /**
     * 更新步骤目标计数
     * @param _chgCount 目标计数变化值
     * @param _dealType 变化类型
     */
    protected void chgCount(long _chgCount, ENCounterDealType _dealType)
    {
        getUserData().lockUser();
        try
        {
            //无数据变化不处理
            if (_chgCount == 0)
                return;

            //原有目标计数
            long oriCount = _m_count;

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

            _m_count = curCount;

            //推送协议
            _m_comp.getUserData().sendMsgToGC(US2GCWriter_028_QuestOp.make_070_OnPlayerSystemQuestChg(toProto()));

            BM bmObj = _m_comp.getUSServer().getBM();

            //更新数据表计数
            if (_m_dbId == 0) //创建数据
            {
                PlayerSystemQuestGroupBO bo = new PlayerSystemQuestGroupBO();
                bo.setCid(bmObj, _m_comp.getUserData().getCid());
                bo.setGroupId(bmObj, _m_groupId);
                bo.setStep(bmObj, _m_step);
                bo.insert(bmObj);

                _m_dbId = bo.getId();
            } else //更新数据
            {
                ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
                updateValue.addValueObj("count", _m_count);
                bmObj.getBM(PlayerSystemQuestGroupBO.class).update("id", _m_dbId, updateValue);
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 获取总计数
     * @return
     */
    public long getTotalCount()
    {
        getUserData().lockUser();
        try
        {
            return _m_count + NPPlayerVariableDeal.getInstance().CalculateVariableResult(_m_comp.getUserData(), _m_ref.process_cur_count, null);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 领取步骤奖励
     * @param _context
     * @return
     */
    public Result drawStepReward(int _step, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            //参数错误
            if (_m_ref == null || _m_ref.step != _step)
                return CommErr.PARAM_ERROR;

            //计数不足
            if (getTotalCount() < _m_ref.process_count)
                return QuestErr.QUEST_FINISH_FAIL;

            //奖励已领取
            _m_comp.getUserData().gainItemList(_m_ref.done_gain_item_list, _context);

            //更新步骤
            _m_step++;
            _m_count = 0;
            ENpRewardShowType tipReward = _m_ref.tip_reward;
            _unRegEvtEntry();

            _m_ref = RefSystemQuest.getMgr().getQuestRef(_m_groupId, _m_step);
            _regEvtEntry();

            //推送协议
            _m_comp.getUserData().sendMsgToGC(US2GCWriter_028_QuestOp.make_070_OnPlayerSystemQuestChg(toProto()));

            _m_comp.getUserData().sendMsgToGC(_context.getCollector().toProto(tipReward));

            BM bmObj = _m_comp.getUSServer().getBM();
            //更新数据表计数
            if (_m_dbId == 0) //创建数据
            {
                PlayerSystemQuestGroupBO bo = new PlayerSystemQuestGroupBO();
                bo.setCid(bmObj, _m_comp.getUserData().getCid());
                bo.setGroupId(bmObj, _m_groupId);
                bo.setStep(bmObj, _m_step);
                bo.setCount(bmObj, _m_count);
                bo.insert(bmObj);

                _m_dbId = bo.getId();
            } else //更新数据
            {
                ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
                updateValue.addValueObj("count", _m_count);
                updateValue.addValueObj("step", _m_step);
                bmObj.getBM(PlayerSystemQuestGroupBO.class).update("id", _m_dbId, updateValue);
            }

            return Result.SUCC;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 修改步骤
     */
    public void chgStep(int _step, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            //更新步骤
            _m_step = _step;
            _m_count = 0;
            _unRegEvtEntry();

            _m_ref = RefSystemQuest.getMgr().getQuestRef(_m_groupId, _m_step);
            _regEvtEntry();

            //推送协议
            _m_comp.getUserData().sendMsgToGC(US2GCWriter_028_QuestOp.make_070_OnPlayerSystemQuestChg(toProto()));

            BM bmObj = _m_comp.getUSServer().getBM();
            //更新数据表计数
            if (_m_dbId == 0) //创建数据
            {
                PlayerSystemQuestGroupBO bo = new PlayerSystemQuestGroupBO();
                bo.setCid(bmObj, _m_comp.getUserData().getCid());
                bo.setGroupId(bmObj, _m_groupId);
                bo.setStep(bmObj, _m_step);
                bo.setCount(bmObj, _m_count);
                bo.insert(bmObj);

                _m_dbId = bo.getId();
            } else //更新数据
            {
                ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
                updateValue.addValueObj("count", _m_count);
                updateValue.addValueObj("step", _m_step);
                bmObj.getBM(PlayerSystemQuestGroupBO.class).update("id", _m_dbId, updateValue);
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 客户端请求增加计数
     * @param _chgCount
     * @param _context
     * @return
     */
    public Result clientAddCount(int _step,long _chgCount, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            //参数错误
            if (_m_ref == null || _m_ref.step != _step)
                return CommErr.PARAM_ERROR;

            if (!_m_ref.is_client_target)
                return CommErr.OP_DISABLE;

            addCount(_chgCount, _context);
            return Result.SUCC;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 构造信息
     * @return
     */
    public SystemQuest_Info toProto()
    {
        getUserData().lockUser();
        try
        {
            SystemQuest_Info info = new SystemQuest_Info();
            info.setGroupId(_m_groupId);
            info.setStep(_m_step);
            info.setCount(_m_count);
            return info;
        } finally
        {
            getUserData().unlockUser();
        }
    }
}
