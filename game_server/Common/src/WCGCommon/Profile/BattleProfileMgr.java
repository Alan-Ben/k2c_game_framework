package WCGCommon.Profile;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import com.google.gson.JsonObject;

import java.util.ArrayList;
import java.util.Comparator;

public class BattleProfileMgr
{
    public static BattleProfileMgr getInstance()
    {
        return _instance;
    }

    private static BattleProfileMgr _instance = new BattleProfileMgr();
    private static final int MAX_ID = 50000;
    private ProfileEventSummary[] _m_Profiles = new ProfileEventSummary[MAX_ID];
    private boolean _m_bOpen = false;


    private static class ProfileEventSummary
    {
        private int _m_id;
        private int _m_callCount = 0;
        private long _m_iMinCostTimeMs;
        private long _m_iMaxCostTimeMs;
        private long _m_iTotalCostTimeMs;
        private long _m_iStartTimeMs = 0;
        private long _m_tag = 0;

        public ProfileEventSummary(int _id)
        {
            _m_id = _id;
        }

        public void start()
        {
            _m_iStartTimeMs = CommonFunc.getNowTimeMicro();
        }

        public void end(long _tag)
        {
            long endTime = CommonFunc.getNowTimeMicro();
            _m_callCount++;
            long cost = endTime - _m_iStartTimeMs;
            if (_m_iMinCostTimeMs > cost) _m_iMinCostTimeMs = cost;
            if (_m_iMaxCostTimeMs < cost)
            {
                _m_iMaxCostTimeMs = cost;
                _m_tag = _tag;
            }
            _m_iTotalCostTimeMs += cost;
        }

        public JsonObject toJson()
        {
            JsonObject obj = new JsonObject();
            obj.addProperty("id", _m_id);
            obj.addProperty("count", _m_callCount);
            obj.addProperty("min_ms", _m_iMinCostTimeMs);
            obj.addProperty("max_ms", _m_iMaxCostTimeMs / 1000000.0d);
            obj.addProperty("tag", _m_tag);
            obj.addProperty("total_ms", _m_iTotalCostTimeMs / 1000000.0d);
            obj.addProperty("avg_cost_ms", (_m_iTotalCostTimeMs / _m_callCount) / 1000000.0d);
            return obj;


        }
    }

    public void beginProfile(int _id)
    {
        if (!_m_bOpen) return;
        if (_id < 0 || _id >= MAX_ID)
            return;
        ProfileEventSummary summary = _m_Profiles[_id];
        if (null == summary)
        {
            summary = new ProfileEventSummary(_id);
            _m_Profiles[_id] = summary;
        }
        summary.start();

    }

    public void endProfile(int _id, long _tag)
    {
        if (!_m_bOpen) return;

        if (_id < 0 || _id >= MAX_ID)
            return;
        ProfileEventSummary summary = _m_Profiles[_id];
        if (null == summary)
        {
            return;
        }
        summary.end(_tag);
    }

    public void dump()
    {
        ArrayList<ProfileEventSummary> ll = new ArrayList<>();
        for (int i = 0; i < _m_Profiles.length; i++)
        {
            ProfileEventSummary summary = _m_Profiles[i];
            if (summary == null) continue;
            ll.add(summary);

        }


        CommLog.info("==================================total cost time================");
        java.util.Collections.sort(ll, new Comparator<ProfileEventSummary>()
        {

            @Override
            public int compare(ProfileEventSummary arg0, ProfileEventSummary arg1)
            {
                return (int) (arg1._m_iTotalCostTimeMs / 1000000L - arg0._m_iTotalCostTimeMs / 1000000L);
            }

        });

        int icount = 0;
        for (ProfileEventSummary summary : ll)
        {
            icount++;
            CommLog.info(summary.toJson().toString());
            if (icount > 10) break;
        }
        CommLog.info("==================================max cost time================");
        java.util.Collections.sort(ll, new Comparator<ProfileEventSummary>()
        {

            @Override
            public int compare(ProfileEventSummary arg0, ProfileEventSummary arg1)
            {
                return (int) (arg1._m_iMaxCostTimeMs - arg0._m_iMaxCostTimeMs);
            }

        });

        icount = 0;
        for (ProfileEventSummary summary : ll)
        {
            icount++;
            CommLog.info(summary.toJson().toString());
            if (icount > 10) break;
        }
    }

    public void setOpen(boolean _isOpen)
    {
        _m_bOpen = _isOpen;

    }

    public void clear()
    {
        for (int i = 0; i < _m_Profiles.length; i++)
        {
            _m_Profiles[i] = null;
        }
    }

    public boolean isOpen()
    {
        return _m_bOpen;
    }
}
