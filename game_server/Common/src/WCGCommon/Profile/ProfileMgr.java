package WCGCommon.Profile;

import ALBasicServer.ALBasicMutex.MutexObject;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import WCGCommon.Enum.NPEnum.EProfileEvent;
import com.google.gson.JsonArray;
import com.google.gson.JsonObject;

import java.util.LinkedList;
import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.atomic.AtomicLong;

public class ProfileMgr implements _IALSynTask
{
    public static ProfileMgr getInstance()
    {
        return _instance;
    }

    private static ProfileMgr _instance = new ProfileMgr();

    private AtomicLong _m_lSerial = new AtomicLong(0L);
    private ConcurrentHashMap<Long, ProfileEvent> _m_eventMap = new ConcurrentHashMap<>();
    private ConcurrentHashMap<EProfileEvent, ProfileEventSummary> _m_summarys = new ConcurrentHashMap<>();

    private static class ProfileEvent
    {
        private EProfileEvent _m_event;
        private long _m_startTimeMs;
        private long _m_endtimeMs = 0;

        public ProfileEvent(EProfileEvent _event)
        {
            _m_event = _event;
            _m_startTimeMs = CommonFunc.getNowTimeMS();
        }

        public void end()
        {
            _m_endtimeMs = CommonFunc.getNowTimeMS();
        }

        public EProfileEvent getEvent()
        {
            return _m_event;
        }

        public long getStartTimeMs()
        {
            return _m_endtimeMs;
        }

        public long getEndTimeMs()
        {
            return _m_endtimeMs;
        }

        public long getCostTimeMs()
        {
            return _m_endtimeMs - _m_startTimeMs;
        }
    }

    private static class SecondCounter
    {
        private static class SecondNode
        {
            int second;
            int count;
        }

        private MutexObject _m_locker = new MutexObject();
        private LinkedList<SecondNode> _m_nodeList = new LinkedList<>();

        public void addCounter(int _second)
        {
            _m_locker.lock();
            try
            {
                if (_m_nodeList.isEmpty())
                {
                    SecondNode node = new SecondNode();
                    node.second = _second;
                    node.count = 1;
                    _m_nodeList.addLast(node);
                } else
                {
                    SecondNode oldNode = _m_nodeList.getLast();
                    if (oldNode.second == _second)
                    {
                        oldNode.count++;
                    } else
                    {
                        SecondNode newNode = new SecondNode();
                        newNode.second = _second;
                        newNode.count = 1;
                        _m_nodeList.addLast(newNode);
                    }

                }
            } finally
            {
                _m_locker.unlock();
            }

        }

        public JsonArray toJsonArray()
        {
            _m_locker.lock();
            try
            {

                JsonArray arr = new JsonArray();
                for (SecondNode node : _m_nodeList)
                {
                    JsonObject obj = new JsonObject();
                    obj.addProperty("sec", node.second);
                    obj.addProperty("count", node.count);
                    arr.add(obj);
                }
                return arr;
            } finally
            {
                _m_locker.unlock();
            }
        }

    }

    private static class ProfileEventSummary
    {
        private EProfileEvent _m_event;
        private MutexObject _m_locker = new MutexObject();

        private int _m_callCount = 0;
        private long _m_iMinCostTimeMs = 999999999999L;
        private long _m_iMaxCostTimeMs = 0L;
        private long _m_iTotalCostTimeMs = 0L;

        private long _m_iStartTimeMs = 0;
        private long _m_iEndTimeMs = 0;
        private SecondCounter _m_SecondCounter = new SecondCounter();

        public ProfileEventSummary(EProfileEvent _event)
        {
            _m_event = _event;
        }

        public void applyEvent(ProfileEvent _eventObj)
        {
            long endSec = 0;
            _m_locker.lock();
            try
            {

                _m_callCount++;
                if (_m_iStartTimeMs == 0)
                {
                    _m_iStartTimeMs = _eventObj.getStartTimeMs();
                }
                _m_iEndTimeMs = _eventObj.getEndTimeMs();


                if (_eventObj.getCostTimeMs() < _m_iMinCostTimeMs)
                {
                    _m_iMinCostTimeMs = _eventObj.getCostTimeMs();
                }
                if (_eventObj.getCostTimeMs() > _m_iMaxCostTimeMs)
                {
                    _m_iMaxCostTimeMs = _eventObj.getCostTimeMs();
                }
                _m_iTotalCostTimeMs += _eventObj.getCostTimeMs();
                endSec = (_eventObj.getEndTimeMs() - _m_iStartTimeMs) / 1000;

            } finally
            {
                _m_locker.unlock();
            }
            _m_SecondCounter.addCounter((int) endSec);

        }

        public JsonObject toJson()
        {
            JsonObject obj = new JsonObject();
            obj.addProperty("name", _m_event.toString());
            obj.addProperty("count", _m_callCount);
            obj.addProperty("min_ms", _m_iMinCostTimeMs);
            obj.addProperty("max_ms", _m_iMaxCostTimeMs);
            obj.addProperty("total_ms", _m_iTotalCostTimeMs);
            obj.addProperty("avg_cost_ms", _m_iTotalCostTimeMs / _m_callCount);
            obj.addProperty("elapsed_ms", _m_iEndTimeMs - _m_iStartTimeMs);
            obj.addProperty("avg_freq_per_sec", _m_callCount * 1.f / ((_m_iEndTimeMs - _m_iStartTimeMs) / 1000));
            obj.add("freq_list", _m_SecondCounter.toJsonArray());
            return obj;


        }
    }

    public long beginProfile(EProfileEvent _event)
    {
        ProfileEvent eventObj = new ProfileEvent(_event);
        long serial = _m_lSerial.incrementAndGet();
        _m_eventMap.put(serial, eventObj);
        return serial;

    }

    public void endProfile(long _serial)
    {
        ProfileEvent eventObj = _m_eventMap.get(_serial);
        if (eventObj == null)
        {
            CommLog.error("Profile invalid serial:" + _serial);
            return;
        }
        eventObj.end();
        _m_eventMap.remove(_serial);

        ensureSummay(eventObj.getEvent()).applyEvent(eventObj);
    }

    public void breakProfile(long _serial)
    {
        _m_eventMap.remove(_serial);
    }

    public synchronized ProfileEventSummary ensureSummay(EProfileEvent _event)
    {
        ProfileEventSummary summary = _m_summarys.get(_event);
        if (null == summary)
        {
            summary = new ProfileEventSummary(_event);
            _m_summarys.put(_event, summary);
        }
        return summary;
    }

    public void Dump()
    {
        try
        {
            if (_m_summarys.isEmpty()) return;

            StringBuilder sb = new StringBuilder();
            for (ProfileEventSummary summary : _m_summarys.values())
            {
                sb.append(summary.toJson().toString());
                sb.append("\n");
            }
            _m_summarys.clear();
            CommLog.info(sb.toString());
        } catch (Exception e)
        {
            CommLog.error("", e);
        }

    }

    private ProfileMgr()
    {
        ALSynTaskManager.getInstance().regTask(this, 1 * 60 * 1000);
    }

    @Override
    public void run()
    {
        Dump();
        ALSynTaskManager.getInstance().regTask(this, 1 * 60 * 1000);

    }
}
