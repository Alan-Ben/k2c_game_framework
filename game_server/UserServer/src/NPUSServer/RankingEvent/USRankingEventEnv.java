package NPUSServer.RankingEvent;

import EventSystem.NPHandlerEntry;
import NPCommon.Util.Delegate.HandlerTwo;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPCommon.Util.Pair.WCGPair;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.Refs._ARefRankingEvent;
import NPUSServer.Common.Event.EventParamVarTypeMap;
import NPUSServer.NPEvent.EventMgr.EventObj._INPGlobalUserEventObj;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.VariableDealer.NPPlayerVariableDeal;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import RankingEvent.RankingEventMgr;
import RankingEvent._IEventListenerTriggerDealer;
import RankingEvent._IRankingEventEnv;
import WCGBasicServer._AWCGBasicServer;

public class USRankingEventEnv implements _IRankingEventEnv, _IHandlerHolder
{
    private NPUserServer _m_usServer;

    public USRankingEventEnv (NPUserServer _usServer)
    {
        _m_usServer = _usServer;
    }

    public NPUserServer getUServer() { return _m_usServer; }

    @Override
    public _AWCGBasicServer getBasicServerObj()
    {
        return _m_usServer;
    }

    @Override
    public boolean onEventMgrInit(RankingEventMgr _mgr)
    {
        return true;
    }

    @Override
    public WCGPair<Boolean, Long> calcRankCountChg(_ARefRankingEvent _rankEventRef, long _cid, _ALogicEventBase _evt)
    {
        boolean needOnline = _rankEventRef.getProcessCurCount().hasVariable() || _rankEventRef.getTriggerCondition().hasCondition();
        if (needOnline)
        {
            NPUSUserData userData = _m_usServer.getUsUserMgr().lookupCacheUserData(_cid);
            if (userData == null)
            {
                USLog.warn(_m_usServer, "USRankingEventEnv canTrigger userData not found, cid:{} eventId:{}", _cid, _evt.getEventId());
                return new WCGPair<>(false, 0L);
            }else
            {
            	NPVarInfo varInfo = EventParamVarTypeMap.getInstance().makeVarInfo(_evt);
            	
                boolean canTrigger = NPPlayerConditionDealerMgr.IsEnable(_rankEventRef.getTriggerCondition(), userData, varInfo);
                if (!canTrigger)
                {
                    return new WCGPair<>(false, 0L);
                }else
                {
                    // 计算触发的值
                    long value = _evt.getValue(_rankEventRef.getTriggerCountRate())
                            + NPPlayerVariableDeal.getInstance().CalculateVariableResult(userData, _rankEventRef.getProcessCurCount(), varInfo);
                    return new WCGPair<>(true, value);
                }
            }
        }else
        {
            return new WCGPair<>(true, _evt.getValue(_rankEventRef.getTriggerCountRate()));
        }
    }

    @Override
    public long calcRankCountSourceId(_ARefRankingEvent _rankEventRef, _ALogicEventBase _evt)
    {
        //获取分数来源
        return _evt.getValue(_rankEventRef.getScoreSource());
    }

    @Override
    public Object regEventTrigger(int _eventId, _IEventListenerTriggerDealer _triggerDealer)
    {
        final USRankingEventEnv usEnv = this;

        return _m_usServer.getGlobalEventHandlerMgr().regHandler(_eventId, this,
                new HandlerTwo<_ALogicEventBase, _INPGlobalUserEventObj>()
                {
                    @Override
                    public void handle(_ALogicEventBase _event, _INPGlobalUserEventObj _userEventObj)
                    {
                        _triggerDealer.onEventTrigger(_userEventObj.getCid(), usEnv, _event);
                    }
                });
    }

    @Override
    @SuppressWarnings("unchecked")
    public void unregEventTrigger(int _eventId, Object _regObj)
    {
        _m_usServer.getGlobalEventHandlerMgr().unregHandler((NPHandlerEntry<_INPGlobalUserEventObj>) _regObj);
    }

    @Override
    public void onLocalEventTrigger(long _dealSerialize, long _cid, long _scoreSourceId, long _chgCount)
    {
        USRankingEventRecordCallbackMgr.getInstance().onRankingEvent(_dealSerialize, _cid, _scoreSourceId, _chgCount);
    }
}
