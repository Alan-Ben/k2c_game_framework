package NPScheduleServer.ActivityScheduleMgr;

import SSDB.Bo.ActivityScheduleUsInfoBO;

public class ActivityScheduleUsInfo
{
    private ActivityScheduleUsGroup _m_usGroup;
    private ActivityScheduleUsInfoBO _m_bo;

    public ActivityScheduleUsInfo(ActivityScheduleUsGroup _usGroup, ActivityScheduleUsInfoBO _bo)
    {
        _m_usGroup = _usGroup;
        _m_bo = _bo;
    }

    public ActivityScheduleUsGroup getUsGroup()
    {
        return _m_usGroup;
    }

    public int getUsId()
    {
        return _m_bo.getUsId();
    }

    public boolean hadPush()
    {
        return _m_bo.getHadPush();
    }

    public void markHadPush()
    {
        _m_bo.saveHadPush(_m_usGroup.getBM(), true);
    }

    public void markHadEnterSettle()
    {
        _m_bo.saveHadEnterSettle(_m_usGroup.getBM(), true);
    }

    public void markHadDone()
    {
        _m_bo.saveHadDone(_m_usGroup.getBM(), true);
    }

    public void markPlaying()
    {
        _m_bo.saveHadEnterPlaying(_m_usGroup.getBM(), true);
    }

    public boolean hadDone()
    {
        return  _m_bo.getHadDone();
    }

    public boolean hadEnterSettle()
    {
        return  _m_bo.getHadEnterSettle();
    }

    public boolean hadEnterPlaying()
    {
        return _m_bo.getHadEnterPlaying();
    }


}
