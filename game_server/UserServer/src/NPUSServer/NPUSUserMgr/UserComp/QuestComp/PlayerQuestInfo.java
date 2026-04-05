package NPUSServer.NPUSUserMgr.UserComp.QuestComp;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.QuestEnum.EQuestStatus;
import Common.QuestObj.Quest_Step;
import Common.QuestObj.Quest_info;
import EventSystem.NPHandlerEntry;
import NPCommon.DB.BM.BM;
import NPCommon.Log.CommLog;
import NPCommon.NPLogDB.CommLogDB;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate.HandlerTwo;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPEnum.ENCounterDealType;
import NPEnum.ENpLogType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.LogicEvent.MetaData.EventMeta;
import NPGameRes.LogicEvent.MetaData.EventMetaMgr_Refdata;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.Refs.Quest.RefQuest;
import NPGameRes.Refs.Quest.RefQuestStep;
import NPGameRes.Refs.Quest.RefQuestStepExtraTrigger;
import NPGameRes.Refs.Quest.RefQuestTarget;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.EventParamVarTypeMap;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.EffectDealer.NPPlayerEffectDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_028_QuestOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerQuestBO;
import USDB.Bo.PlayerQuestTargetBO;
import USLOGDB.Bo.LogQuestBO;

import java.util.ArrayList;
import java.util.List;

public class PlayerQuestInfo implements _IHandlerHolder
{
    //任务组件
    private PlayerQuestComponent _m_comp;
    //任务BO数据
    private long _m_lDbid;
    private long _m_lQuestStep;
    private int _m_iStepExpireTimeTag;
    //任务基础数据
    private RefQuest _m_refQuest;
    private RefQuestStep _m_refQuestStep;
    //任务目标数据列表
    private ArrayList<PlayerQuestTargetInfo> _m_alQuestTargetList;
    //任务状态
    private EQuestStatus _m_eStatus;
    //触发事件对象列表
    private ArrayList<NPHandlerEntry<NPUSUserData>> _m_evtEntryList;

    /**
     * 所有的任务数据都预先设置等待，再设置步骤数据，统一构造函数入口，避免错过初始化任务步骤的处理
     * @param _comp
     * @param _bo
     * @param _questRef
     */
    protected PlayerQuestInfo(PlayerQuestComponent _comp, PlayerQuestBO _bo, RefQuest _questRef)
    {
        _m_comp = _comp;

        _m_lDbid = _bo.getId();
        _m_lQuestStep = _bo.getQuestStep();
        _m_iStepExpireTimeTag = _bo.getStepExpireTimeTag();

        _m_refQuest = _questRef;

        _m_alQuestTargetList = new ArrayList<>();

        //尚未有步骤，认为是未达到开启条件的任务，处在等待中
        _m_eStatus = EQuestStatus.WAITING;

        _m_evtEntryList = new ArrayList<>();
    }

    public PlayerQuestComponent getComp()
    {
        return _m_comp;
    }

    public RefQuest getRef()
    {
        return _m_refQuest;
    }

    public RefQuestStep getStepRef()
    {
        return _m_refQuestStep;
    }

    public long getQuestId()
    {
        return _m_refQuest.quest_id;
    }

    public long getDbid()
    {
        return _m_lDbid;
    }

    public long getQuestStep()
    {
        return _m_lQuestStep;
    }

    public int getStepExpireTimeTag()
    {
        return _m_iStepExpireTimeTag;
    }

    public EQuestStatus getStatus()
    {
        return _m_eStatus;
    }

    public List<PlayerQuestTargetInfo> getAllTarget()
    {
        return _m_alQuestTargetList;
    }

    /**
     * 初始化任务步骤
     * @return
     */
    protected void initQuestStep(RefQuestStep _questStepRef)
    {
        _m_refQuestStep = _questStepRef;

        //初始化目标数据
        if (!setAllTarget())
        {
            _m_eStatus = EQuestStatus.NONE;
        } else
        {
            //已经有步骤的任务，认为是进行中的任务
            _m_eStatus = EQuestStatus.PROGRESSING;
        }
    }

    /**
     * 初始化任务目标数据
     */
    protected void initQuestTargetFromDB(PlayerQuestTargetBO _questStepTargetBo)
    {
        for (int i = 0; i < _m_alQuestTargetList.size(); i++)
        {
            PlayerQuestTargetInfo target = _m_alQuestTargetList.get(i);
            if (null == target)
                continue;

            //设置目标bo
            if (target.getTargetId() == _questStepTargetBo.getQuestTarget())
            {
                target.setBo(_questStepTargetBo);

                break;
            }
        }
    }

    /**
     * 注册监听
     */
    protected void regExtraEventEntry()
    {
        RefQuestStepExtraTrigger refStepExtraTrigger = RefQuestStepExtraTrigger.getMgr().get(_m_lQuestStep);
        if (refStepExtraTrigger != null)
        {
            for (String event : refStepExtraTrigger.trigger_event_list)
            {
                EventMeta eventMeta = EventMetaMgr_Refdata.getInstance().lookupMetaByEventName(event.toUpperCase());
                if (eventMeta == null)
                {
                    CommLog.error("PlayerQuestInfo regExtraEventEntry failed, event not found, questId:{} stepId:{} event:{}", getRef().Id(), _m_lQuestStep, event, new Exception());
                    continue;
                }

                //注册事件监听
                NPHandlerEntry<NPUSUserData> evtEntry = getComp().getUserData().getEventHandlerMgr().regHandler(eventMeta.getEventId(),
                        this, new HandlerTwo<_ALogicEventBase, NPUSUserData>()
                {
                    @Override
                    public void handle(_ALogicEventBase _evt, NPUSUserData _userData)
                    {
                        //时间超时，不再处理
                        if (isStepExpired())
                            return;

                        NPPlayerContext context = (NPPlayerContext) _evt.getContext();
                        onLogicEvt(_evt, context);
                    }
                });

                _m_evtEntryList.add(evtEntry);
            }
        }
    }

    /**
     * 注销监听
     */
    protected void unRegExtraEventEntry()
    {
        for (int i = 0; i < _m_evtEntryList.size(); i++)
        {
            NPHandlerEntry<NPUSUserData> entry = _m_evtEntryList.get(i);
            if (null == entry)
                continue;

            getComp().getUserData().getEventHandlerMgr().unregHandler(entry);
        }
        _m_evtEntryList.clear();
    }

    /**
     * 监听事件的处理
     * @param _evt
     * @param _context
     */
    protected void onLogicEvt(_ALogicEventBase _evt, NPPlayerContext _context)
    {
        RefQuestStepExtraTrigger refStepExtraTrigger = RefQuestStepExtraTrigger.getMgr().get(_m_lQuestStep);
        if (refStepExtraTrigger == null)
            return;

    	NPVarInfo varInfo = EventParamVarTypeMap.getInstance().makeVarInfo(_evt);
    	
        //检查触发条件
        if (!NPPlayerConditionDealerMgr.IsEnable(refStepExtraTrigger.trigger_condition, getComp().getUserData(), varInfo))
            return;

        //触发额外效果
        NPPlayerEffectDealer.dealEffect(refStepExtraTrigger.ext_trigger_effect, getComp().getUserData(), varInfo, _context);
    }

    /**
     * 检查任务是否进行中
     * @param _questStep
     * @param _questStepTarget
     * @return
     */
    protected boolean isDoing(long _questStep, long _questStepTarget)
    {
        //检查任务状态
        if (!isProgressing())
            return false;

        //如果 _questStep = 0，表示只检查任务，不需要检查任务步骤
        if (_questStep == 0)
            return true;

        //并非当前任务步骤
        if (_questStep != _m_lQuestStep)
            return false;

        //如果 _questStepTarget = 0，表示只检查任务步骤，不需要检查任务目标
        if (_questStepTarget == 0)
            return true;

        //检查当前步骤
        PlayerQuestTargetInfo questTarget = lookupTarget(_questStepTarget);
        if (null == questTarget)
            return false;

        //检查当前步骤是否完成
        return !questTarget.isDone();
    }

    /**
     * 检查任务是否完成
     * @param _stepId
     * @return
     */
    protected boolean isStepDone(long _stepId)
    {
        //检查任务状态
        if (!isProgressing())
            return false;

        //如果为当前任务步骤
        if (_stepId == _m_lQuestStep)
        {
            return false;
        }
        RefQuestStep refStep = RefQuestStep.getMgr().get(_stepId);
        if (refStep == null)
        {
            return false;
        }
        int targetStep = getRef().listStep.indexOf(refStep);
        int nowStep = getRef().listStep.indexOf(getStepRef());
        return targetStep >= 0 && nowStep >= 0 && targetStep < nowStep;
    }

    /**
     * 设置所有目标数据
     * @return
     */
    protected boolean setAllTarget()
    {
        //清空之前的任务步骤
        clearAllTarget();

        //获取步骤下所有目标
        List<RefQuestTarget> targetList = _m_refQuestStep.listTarget;
        if (null == targetList)
        {
            USLog.error(_m_comp.getUSServer(), "can not init quest target, cid:{} quest:{} step:{}"
                    , getComp().getUserData().getCid(), _m_refQuest.quest_id, _m_lQuestStep);
            return false;
        }

        //初始化目标数据
        for (RefQuestTarget targetRef : targetList)
        {
            if (null == targetRef)
                continue;

            PlayerQuestTargetInfo info = new PlayerQuestTargetInfo(_m_comp.getUSServer(), this, targetRef);
            _m_alQuestTargetList.add(info);
            //注册监听handler
            info.regEvtEntry();
        }

        regExtraEventEntry();

        return true;
    }

    /**
     * 清除当前所有目标
     */
    protected void clearAllTarget()
    {
        //移除监听
        for (int i = 0; i < _m_alQuestTargetList.size(); i++)
        {
            PlayerQuestTargetInfo target = _m_alQuestTargetList.get(i);
            if (null == target)
                continue;

            target.unregEvtEntry();
        }

        //清空步骤目标列表
        _m_alQuestTargetList.clear();
        //移除额外触发事件监听
        unRegExtraEventEntry();
    }

    /**
     * 无效任务，需要手动处理
     */
    public boolean isInvalid()
    {
        return EQuestStatus.NONE == _m_eStatus;
    }

    /**
     * 等待中的任务
     */
    public boolean isWaiting()
    {
        return EQuestStatus.WAITING == _m_eStatus;
    }

    /**
     * 进行中的任务
     */
    public boolean isProgressing()
    {
        return EQuestStatus.PROGRESSING == _m_eStatus;
    }

    /**
     * 检查步骤是否超时
     * @return
     */
    protected boolean isStepExpired()
    {
        return _m_iStepExpireTimeTag > 0 && CommonFunc.getNowTimeSec() > _m_iStepExpireTimeTag;
    }

    /**
     * 构造协议对象
     * @return
     */
    public Quest_info toProto()
    {
        Quest_info proto = new Quest_info();
        proto.setQuestId(_m_refQuest.quest_id);
        proto.setQuestStatus(_m_eStatus);
        //任务步骤
        Quest_Step step = new Quest_Step();
        step.setQuestStep(_m_lQuestStep);
        step.setExpireTimeTagS(_m_iStepExpireTimeTag);
        //目标列表
        for (int i = 0; i < _m_alQuestTargetList.size(); i++)
        {
            PlayerQuestTargetInfo target = _m_alQuestTargetList.get(i);
            if (null == target)
                continue;

            step.addTargetList(target.toProto());
        }
        proto.setQuestStep(step);

        return proto;
    }

    /**
     * 查找指定目标对象
     * @param _targetId
     * @return
     */
    protected PlayerQuestTargetInfo lookupTarget(long _targetId)
    {
        //目标列表
        for (int i = 0; i < _m_alQuestTargetList.size(); i++)
        {
            PlayerQuestTargetInfo target = _m_alQuestTargetList.get(i);
            if (null == target)
                continue;

            if (target.getTargetId() == _targetId)
                return target;
        }

        return null;
    }

    /**
     * 设置第首步骤，设置首步骤
     * @param _isServerStart
     * @param _context
     * @return
     */
    protected boolean setFirstStep(boolean _isServerStart, NPPlayerContext _context)
    {
        //获取任务首步骤
        RefQuestStep firstStepRef = RefQuestStep.getMgr().get(_m_refQuest.first_step_id);
        if (null == firstStepRef)
        {
            //设置无效状态
            _m_eStatus = EQuestStatus.NONE;

            USLog.error(_m_comp.getUSServer(), "start quest fail, not find first step, cid:{} quest:{}-{}", _m_comp.getUserData().getCid(), _m_refQuest.quest_id, _m_refQuest.first_step_id);
            return false;
        }

        //更新数据
        _m_refQuestStep = firstStepRef;

        _m_lQuestStep = firstStepRef.step_id;
        _m_iStepExpireTimeTag = firstStepRef.getStepExpireTimeS();

        BM bmObj = _m_comp.getUSServer().getBM();

        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("quest_step", _m_lQuestStep);
        updateValue.addValueObj("step_expire_time_tag", _m_iStepExpireTimeTag);
        bmObj.getBM(PlayerQuestBO.class).update("id", _m_lDbid, updateValue);

        //同步目标数据
        if (!setAllTarget())
        {
            //同步目标失败，设置无效状态
            _m_eStatus = EQuestStatus.NONE;
            return false;
        }

        //获取给定的物品
        NPPlayerContext questStepContext = NPPlayerContext.createNew(_context);
        getComp().getUserData().gainItemList(_m_refQuestStep.receive_give_item_list, questStepContext);
        //推送奖励列表
        if (!questStepContext.getCollector().isEmpty())
        {
            _m_comp.getUserData().sendMsgToGC(questStepContext.getCollector().toProto(_m_refQuestStep.tip_reward));
        }

        //设置任务进行中状态
        _m_eStatus = EQuestStatus.PROGRESSING;

        return true;
    }

    /**
     * 完成步骤
     * @return
     */
    protected boolean finishStep()
    {
        //检查所有目标是否完成
        for (int i = 0; i < _m_alQuestTargetList.size(); i++)
        {
            PlayerQuestTargetInfo info = _m_alQuestTargetList.get(i);
            if (null == info)
                continue;

            if (!info.isDone())
                return false;
        }

        return true;
    }

    /**
     * 是否拥有下一步骤
     * @return
     */
    protected boolean hasNextStep()
    {
        return null != _m_refQuestStep && _m_refQuestStep.next_step_id > 0;
    }

    /**
     * 设置下一个步骤
     * @param _context
     * @return
     */
    protected boolean setNextStep(NPPlayerContext _context)
    {
        //清空之前步骤的任务列表
        clearAllTarget();

        //设置下一个步骤
        RefQuestStep stepRef = RefQuestStep.getMgr().get(_m_refQuestStep.next_step_id);
        if (null == stepRef)
        {
            USLog.error(_m_comp.getUSServer(), "can not set next step, ref is null, cid:{} quest:{}, nextStep:{}"
                    , _m_comp.getUserData().getCid(), _m_refQuest.quest_id, _m_refQuestStep.next_step_id);
            //设置任务无效
            _m_eStatus = EQuestStatus.NONE;
            return false;
        }

        //设置当前任务
        _m_refQuestStep = stepRef;

        _m_lQuestStep = stepRef.step_id;
        _m_iStepExpireTimeTag = stepRef.getStepExpireTimeS();

        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("quest_step", _m_lQuestStep);
        updateValue.addValueObj("step_expire_time_tag", _m_iStepExpireTimeTag);
        _m_comp.getUSServer().getBM().getBM(PlayerQuestBO.class).update("id", _m_lDbid, updateValue);

        //同步当前步骤的目标监听对象
        if (!setAllTarget())
        {
            //设置任务无效
            _m_eStatus = EQuestStatus.NONE;
            return false;
        }

        //获取给定的物品
        NPPlayerContext questStepContext = NPPlayerContext.createNew(_context);
        getComp().getUserData().gainItemList(_m_refQuestStep.receive_give_item_list, questStepContext);
        //推送奖励列表
        if (!questStepContext.getCollector().isEmpty())
        {
            _m_comp.getUserData().sendMsgToGC(questStepContext.getCollector().toProto(_m_refQuestStep.tip_reward));
        }

        //推送协议
        getComp().getUserData().sendMsgToGC(US2GCWriter_028_QuestOp.make_050_OnPlayerQuestChg(toProto()));
        
        return true;
    }

    /**
     * 更改任务步骤目标计数
     * @param _qusetStep
     * @param _targetId
     * @param _dealType
     * @param _context
     * @return
     */
    protected boolean chgTargetCount(long _qusetStep, long _targetId, long _chgCount, ENCounterDealType _dealType, NPPlayerContext _context)
    {
        //不在进行中的任务不处理
        if (!isProgressing())
            return false;

        //检查任务步骤
        if (_qusetStep != getQuestStep())
            return false;

        //检查对应目标
        PlayerQuestTargetInfo target = lookupTarget(_targetId);
        if (null == target)
            return false;

        //修改任务计数
        target.chgCount(_chgCount, _dealType, _context);

        return true;
    }

    /****
     * GM：设置指定步骤
     * @param _stepRef
     * @param _context
     * @return
     */
    protected boolean gmSetStep(RefQuestStep _stepRef, NPPlayerContext _context)
    {
        if (getStepRef().step_id == _stepRef.step_id)
            return true;

        //清空之前步骤的任务列表
        clearAllTarget();

        //设置当前任务
        _m_refQuestStep = _stepRef;

        _m_lQuestStep = _stepRef.step_id;
        _m_iStepExpireTimeTag = _stepRef.getStepExpireTimeS();

        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("quest_step", _m_lQuestStep);
        updateValue.addValueObj("step_expire_time_tag", _m_iStepExpireTimeTag);
        _m_comp.getUSServer().getBM().getBM(PlayerQuestBO.class).update("id", _m_lDbid, updateValue);

        //同步当前步骤的目标监听对象
        if (!setAllTarget())
        {
            //设置任务无效
            _m_eStatus = EQuestStatus.NONE;
            return false;
        }

        //获取给定的物品
        NPPlayerContext questStepContext = NPPlayerContext.createNew(_context);
        getComp().getUserData().gainItemList(_m_refQuestStep.receive_give_item_list, questStepContext);
        //推送奖励列表
        if (!questStepContext.getCollector().isEmpty())
        {
            _m_comp.getUserData().sendMsgToGC(questStepContext.getCollector().toProto(_m_refQuestStep.tip_reward));
        }

        //推送协议
        getComp().getUserData().sendMsgToGC(US2GCWriter_028_QuestOp.make_050_OnPlayerQuestChg(toProto()));

        return true;
    }

    /**
     * 日志数据
     * @param _logType
     * @param _context
     */
    protected void _log(ENpLogType _logType, NPPlayerContext _context)
    {
    	BM bmObj = getComp().getUserData().getUSServer().getBM();
    	
    	LogQuestBO logBo = new LogQuestBO();
    	logBo.setCid(bmObj, getComp().getUserData().getCid());
    	logBo.setLogType(bmObj, _logType.ordinal());
    	logBo.setQuestId(bmObj, getQuestId());
    	logBo.setQuestDbid(bmObj, getDbid());
    	logBo.setType(bmObj, getRef().quest_type.ordinal());
    	logBo.setStep(bmObj, getQuestStep());
    	logBo.setExpiredTs(bmObj, getStepExpireTimeTag());
    	logBo.setStatus(bmObj, getStatus().ordinal());
    	
    	CommLogDB.log(bmObj, logBo, _context);
    }
}
