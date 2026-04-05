package NPHttpServer.Http.Entity;

/**
 * 通知角色游戏事件
 */
public class NPEntityNotifyRoleGameEvent
{
    private long _m_cid;
    //问卷编码
    private String _m_activityCode;
    //事件类型
    private String _m_type;

    public long getCid()
    {
        return _m_cid;
    }

    public void setCid(long _cid)
    {
        _m_cid = _cid;
    }

    public String getActivityCode()
    {
        return _m_activityCode;
    }

    public void setActivityCode(String _activityCode)
    {
        _m_activityCode = _activityCode;
    }

    public String getType()
    {
        return _m_type;
    }

    public void setType(String _type)
    {
        _m_type = _type;
    }
}
