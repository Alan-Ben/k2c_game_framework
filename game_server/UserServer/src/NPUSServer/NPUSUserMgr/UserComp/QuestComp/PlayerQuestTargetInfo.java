package NPUSServer.NPUSUserMgr.UserComp.QuestComp;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.QuestObj.Quest_Target;
import EventSystem.NPHandlerEntry;
import NPCommon.DB.BM.BM;
import NPCommon.Log.CommLog;
import NPCommon.NPLogDB.CommLogDB;
import NPCommon.Util.Delegate.HandlerTwo;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPEnum.ENCounterDealType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.LogicEvent.MetaData.EventMeta;
import NPGameRes.LogicEvent.MetaData.EventMetaMgr_Refdata;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.Refs.Quest.RefQuestTarget;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.EventParamVarTypeMap;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.EffectDealer.NPPlayerEffectDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.VariableDealer.NPPlayerVariableDeal;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_028_QuestOp;
import NPUSServer.NPUserServer;
import USDB.Bo.PlayerQuestTargetBO;
import USLOGDB.Bo.LogQuestTargetBO;

import java.util.ArrayList;

public class PlayerQuestTargetInfo implements _IHandlerHolder
{
    private NPUserServer _m_server;

    //任务对象
    private PlayerQuestInfo _m_quest;
    //数据BO
    private long _m_lDbid;
    private long _m_lQuestTargetCount;
    //配置数据
    private RefQuestTarget _m_ref;
    //触发事件对象列表
    private ArrayList<NPHandlerEntry<NPUSUserData>> _m_evtEntryList;

    //尚未设置任务计数
    public PlayerQuestTargetInfo(NPUserServer _server, PlayerQuestInfo _quest, RefQuestTarget _ref)
    {
        _m_server = _server;

        _m_quest = _quest;

        _m_lDbid = 0;
        _m_lQuestTargetCount = 0;

        _m_ref = _ref;

        _m_evtEntryList = new ArrayList<>();
    }

    public NPUserServer getUSServer(){return _m_server;}

    public PlayerQuestInfo getQuest()
    {
        return _m_quest;
    }

    public RefQuestTarget getRef()
    {
        return _m_ref;
    }

    public long getTargetId()
    {
        return _m_ref.id;
    }

    public long getTargetSelfCount()
    {
        return _m_lQuestTargetCount;
    }

    /**
     * 设置数据BO
     * @param _bo
     */
    protected void setBo(PlayerQuestTargetBO _bo)
    {
        _m_lDbid = _bo.getId();
        _m_lQuestTargetCount = _bo.getQuestTargetCount();
    }

    /**
     * 注册监听
     */
    protected void regEvtEntry()
    {
        for (String event : _m_ref.trigger_event)
        {
            EventMeta eventMeta = EventMetaMgr_Refdata.getInstance().lookupMetaByEventName(event.toUpperCase());
            if (eventMeta == null)
            {
                CommLog.error("PlayerQuestTargetInfo regEvtEntry failed, event not found, questTargetId:{} event:{}", getRef().Id(), event, new Exception());
                continue;
            }

            //注册事件监听
            NPHandlerEntry<NPUSUserData> evtEntry = _m_quest.getComp().getUserData().getEventHandlerMgr().
                    regHandler(eventMeta.getEventId(), this, new HandlerTwo<_ALogicEventBase, NPUSUserData>()
                    {
                        @Override
                        public void handle(_ALogicEventBase _evt, NPUSUserData _userData)
                        {
                            //时间超时，不再处理
                            if (_m_quest.isStepExpired())
                                return;

                            NPPlayerContext context = (NPPlayerContext) _evt.getContext();
                            onLogicEvt(_evt, context);
                        }
                    });

            _m_evtEntryList.add(evtEntry);
        }
    }

    /**
     * 注销监听
     */
    protected void unregEvtEntry()
    {
        for (int i = 0; i < _m_evtEntryList.size(); i++)
        {
            NPHandlerEntry<NPUSUserData> entry = _m_evtEntryList.get(i);
            if (null == entry)
                continue;

            _m_quest.getComp().getUserData().getEventHandlerMgr().unregHandler(entry);
        }
    }

    /**
     * 监听事件的处理
     * @param _evt
     * @param _context
     */
    protected void onLogicEvt(_ALogicEventBase _evt, NPPlayerContext _context)
    {
    	NPVarInfo varInfo = EventParamVarTypeMap.getInstance().makeVarInfo(_evt);
    	
        //检查触发条件
        if (!NPPlayerConditionDealerMgr.IsEnable(_m_ref.trigger_condition, getQuest().getComp().getUserData(), varInfo))
            return;

        //增加任务计数
        long chgCount = _evt.getValue(_m_ref.trigger_count_rate);
        if (chgCount != 0)
        {
            addCount(chgCount, _context);
        }

        //触发额外效果
        NPPlayerEffectDealer.dealEffect(_m_ref.ext_trigger_effect, getQuest().getComp().getUserData(), varInfo, _context);
    }

    /**
     * 构造数据
     * @return
     */
    public Quest_Target toProto()
    {
        Quest_Target proto = new Quest_Target();
        proto.setTargetId(_m_ref.id);
        proto.setCurCount(_m_lQuestTargetCount);

        return proto;
    }

    /**
     * 获取当前计数:高级公式 + 任务计数
     * @return
     */
    public long getCurCount()
    {
        //高级公式
        long count = NPPlayerVariableDeal.getInstance().CalculateVariableResult(getQuest().getComp().getUserData(), _m_ref.process_cur_count, null);

        //高级公式计数 + 任务目标计数
        return (count + _m_lQuestTargetCount);
    }

    /**
     * 是否完成
     * @return
     */
    public boolean isDone()
    {
        return getCurCount() >= _m_ref.process_count;
    }

    /**
     * 更新任务步骤目标计数
     * @param _chgCount
     * @param _dealType
     */
    protected void chgCount(long _chgCount, ENCounterDealType _dealType, NPPlayerContext _context)
    {
        //原有目标计数
        long oriCount = _m_lQuestTargetCount;

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
                curCount = oriCount < _chgCount ? _chgCount : oriCount;
                break;
            case ADD:
            default:
                curCount = oriCount + _chgCount;
                break;
        }

        _m_lQuestTargetCount = curCount;

        BM bmObj = getUSServer().getBM();

        //更新数据表计数
        if (_m_lDbid == 0) //创建数据
        {
            PlayerQuestTargetBO bo = new PlayerQuestTargetBO();
            bo.setCid(bmObj, getQuest().getComp().getUserData().getCid());
            bo.setQuestId(bmObj, _m_quest.getQuestId());
            bo.setQuestStep(bmObj, _m_quest.getQuestStep());
            bo.setQuestTarget(bmObj, _m_ref.id);
            bo.setQuestTargetCount(bmObj, _m_lQuestTargetCount);
            bo.insert(bmObj);

            _m_lDbid = bo.getId();
        } else //更新数据
        {
            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("quest_target_count", _m_lQuestTargetCount);
            bmObj.getBM(PlayerQuestTargetBO.class).update("id", _m_lDbid, updateValue);
        }

        //推送协议
        getQuest().getComp().getUserData().sendMsgToGC(US2GCWriter_028_QuestOp.make_051_OnPlayerQuestStepCountChg(
                _m_quest.getQuestId(), _m_quest.getQuestStep(), _m_ref.id, _m_lQuestTargetCount));
        
        //日志数据
        LogQuestTargetBO logBo = new LogQuestTargetBO();
        logBo.setCid(bmObj, getQuest().getComp().getUserData().getCid());
        logBo.setQuestId(bmObj, getQuest().getQuestId());
        logBo.setQuestDbid(bmObj, getQuest().getDbid());
        logBo.setStep(bmObj, getQuest().getQuestStep());
        logBo.setTarget(bmObj, getTargetId());
        logBo.setOriCount(bmObj, oriCount);
        logBo.setCurCount(bmObj, getTargetSelfCount());
        
        CommLogDB.log(bmObj, logBo, _context);
    }

    /**************** 快捷计数变更 ******************/
    protected void addCount(long _chgCount, NPPlayerContext _context)
    {
        chgCount(_chgCount, ENCounterDealType.ADD, _context);
    }

    protected void reduceCount(long _chgCount, NPPlayerContext _context)
    {
        chgCount(_chgCount, ENCounterDealType.REDUCE, _context);
    }

    protected void setCount(long _chgCount, NPPlayerContext _context)
    {
        chgCount(_chgCount, ENCounterDealType.SET, _context);
    }

    protected void setGtCount(long _chgCount, NPPlayerContext _context)
    {
        chgCount(_chgCount, ENCounterDealType.SET_GT, _context);
    }

    public void deal()
    {
        getUSServer().getBM().getBM(PlayerQuestTargetBO.class).delAll("id", _m_lDbid);
    }
}
