package NPUSServer.NPUSUserMgr.UserComp.DailyQuestComp;

import Common.QuestEnum.EDailyQuestType;
import Common.QuestObj.DailyQuest_Info;
import EventSystem.NPHandlerEntry;
import NPCommon.ErrMain.QuestErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Log.CommLog;
import NPCommon.Util.Delegate.HandlerTwo;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.LogicEvent.MetaData.EventMeta;
import NPGameRes.LogicEvent.MetaData.EventMetaMgr_Refdata;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.Refs.Quest.DailyQuest.RefDailyQuest;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.EventParamVarTypeMap;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.VariableDealer.NPPlayerVariableDeal;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_028_QuestOp;
import USDB.Bo.PlayerDailyQuestBO;

import java.util.ArrayList;

/**
 * @description: 日常任务数据信息
 * @author: ricci
 * @date: 2022-10-12 14:31:11
 */
public class DailyQuestInfo implements _IHandlerHolder
{
    /**
     * 任务数据
     */
    private final PlayerDailyQuestBO _m_bo;
    /**
     * 任务组件
     */
    private final DailyQuestComponent _m_comp;

    /**
     * 任务配置
     */
    private RefDailyQuest _m_refQuest;

    /**
     * 触发事件对象列表
     */
    private ArrayList<NPHandlerEntry<NPUSUserData>> _m_evtEntryList;

    public DailyQuestInfo(DailyQuestComponent _comp, RefDailyQuest _refDailyQuest, PlayerDailyQuestBO _bo)
    {
        _m_comp = _comp;
        _m_refQuest = _refDailyQuest;
        _m_bo = _bo;
        _m_evtEntryList = new ArrayList<>();

        //注册事件监听
        _regEvtEntry();
    }

    public RefDailyQuest getRef()
    {
        return _m_refQuest;
    }

    public long getRefId()
    {
        return _m_refQuest.id;
    }

    public boolean getIsRandom()
    {
        return _m_bo.getIsRandom();
    }

    public PlayerDailyQuestBO getBo()
    {
        return _m_bo;
    }

    public long getCount()
    {
        //高级公式
        long count = NPPlayerVariableDeal.getInstance().CalculateVariableResult(getComp().getUserData(), _m_refQuest.process_cur_count, null);

        return getBo().getCount() + count;
    }

    public boolean getIsFinish()
    {
        return getBo().getHasTaken();
    }

    public DailyQuestComponent getComp()
    {
        return _m_comp;
    }

    public long getSerial()
    {
        return getBo().getFreshSerial();
    }

    public EDailyQuestType getType()
    {
        return EDailyQuestType.EDailyQuestType_FromInt(getBo().getDailyQuestType());
    }

    /**
     * 设置计数
     * @param _count 指定计数
     */
    public void setCount(long _count)
    {
        getBo().saveCount(_m_comp.getUSServer().getBM(), _count);
        getComp().getUserData().sendMsgToGC(US2GCWriter_028_QuestOp.
                make_060_OnPlayerDailyQuestChg(getType(), makeProto()));
    }

    /**
     * 销毁任务数据
     */
    public void discard()
    {
        _m_bo.del(_m_comp.getUSServer().getBM());

        //移除所有监听
        _unRegEvtEntry();
    }

    /**
     * 记录领取任务奖励
     * @param _isSys   是否是系统操作 是的话不需要推送
     * @param _context
     * @return boolean
     */
    public Result recordTakeReward(boolean _isSys, NPPlayerContext _context)
    {
        //检查是否已经领取
        if (getBo().getHasTaken())
            return QuestErr.DAILY_QUEST_HAS_FINISH;

        //检查是否达到领取条件
        if (getRef().process_count > getCount())
            return QuestErr.DAILY_QUEST_NOT_REACH_SCORE_REQUIRE;

        //条件检查
        if (!NPPlayerConditionDealerMgr.IsEnable(getRef().simple_unlock_id, getComp().getUserData(), null))
            return QuestErr.DAILY_QUEST_NOT_UNLOCK;

        getBo().saveHasTaken(_m_comp.getUSServer().getBM(), true);

        //如果不是玩家操作，不需要推送
        if (!_isSys)
            getComp().getUserData().sendMsgToGC(US2GCWriter_028_QuestOp.make_060_OnPlayerDailyQuestChg(getType(), makeProto()));

        return Result.SUCC;
    }

    /**
     * 构造任务协议数据
     * @return DailyQuest_Info
     */
    public DailyQuest_Info makeProto()
    {
        DailyQuest_Info proto = new DailyQuest_Info();
        proto.setTargetId(getRefId());
        proto.setCurCount(_m_bo.getCount());
        proto.setIsFinish(getIsFinish());
        return proto;
    }

    /**
     * 注册监听
     */
    protected void _regEvtEntry()
    {
        for (String event : _m_refQuest.trigger_event)
        {
            EventMeta eventMeta = EventMetaMgr_Refdata.getInstance().lookupMetaByEventName(event.toUpperCase());
            if (eventMeta == null)
            {
                CommLog.error("DailyQuestInfo regEvtEntry failed, event not found, questId:{} event:{}", getRef().Id(), event, new Exception());
                continue;
            }

            //注册事件监听
            NPHandlerEntry<NPUSUserData> evtEntry = getComp().getUserData().getEventHandlerMgr()
                    .regHandler(eventMeta.getEventId(), this, new HandlerTwo<_ALogicEventBase, NPUSUserData>()
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

    /**
     * 注销监听
     */
    protected void _unRegEvtEntry()
    {
        getComp().getUserData().getEventHandlerMgr().unregHandler(this);
    }

    /**
     * 监听事件的处理
     * @param _evt     时间数据
     * @param _context 上下文数据
     */
    protected void onLogicEvt(_ALogicEventBase _evt, NPPlayerContext _context)
    {
    	NPVarInfo varInfo = EventParamVarTypeMap.getInstance().makeVarInfo(_evt);
    	
        //检查触发条件
        if (!NPPlayerConditionDealerMgr.IsEnable(_m_refQuest.trigger_condition, getComp().getUserData(), varInfo))
            return;

        //增加任务计数
        long chgCount = _evt.getValue(_m_refQuest.trigger_count_rate);
        if (chgCount != 0)
        {
            long newCount = _m_bo.getCount() + chgCount;
            setCount(newCount);
        }
    }
}
