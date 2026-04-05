package NPUSServer.QueueMgr;

/*********************
 * 当前玩家所在队列信息，每个队列信息有一个ticket，防止数据错乱
 * @author mj
 *
 */
public class QueueInfo
{
    //所在队列序号，每次服务器重启，序号从1开始
    private long _m_lQueueIndex;

    //用户Id
    private String _m_sUid;
    //对应的gs识别Id
    private long _m_lGSSessionId;

    //对应GS服务器Id
    private int _m_iGSId;
    //对应GS服务器用户的信息序列号
    private long _m_lGSInfoSerialize;

    //当前是否有效，当gs链接断开的时候会设置本对象无效
    private boolean _m_bIsEnable;
    //客户端自定义数据
    private String _m_customData;

    protected QueueInfo(long _index, String _uid, long _gsSessionId, int _gsId, long _gsInfoSerialize, String _customData)
    {
        _m_lQueueIndex = _index;

        _m_sUid = _uid;
        _m_lGSSessionId = _gsSessionId;

        _m_iGSId = _gsId;
        _m_lGSInfoSerialize = _gsInfoSerialize;

        _m_bIsEnable = false;
        _m_customData = _customData;
    }

    public long getQueueIndex()
    {
        return _m_lQueueIndex;
    }

    public String getUid()
    {
        return _m_sUid;
    }

    public long getGSSessionId()
    {
        return _m_lGSSessionId;
    }

    public int getGSId()
    {
        return _m_iGSId;
    }

    public long getGSInfoSerialize()
    {
        return _m_lGSInfoSerialize;
    }

    public boolean isEnable()
    {
        return _m_bIsEnable;
    }

    public String getCustomData()
    {
        return _m_customData;
    }

    /********
     * 设置为无效
     */
    public void setDisable()
    {
        _m_bIsEnable = false;
    }

    /**
     * 更新数据 并 重设数据为有效数据
     * @param _gsSessionId     对应的gs识别Id
     * @param _gsId            对应GS服务器Id
     * @param _gsInfoSerialize 对应GS服务器用户的信息序列号
     * @param _customData      客户端自定义数据
     */
    public void updateAndEnable(long _gsSessionId, int _gsId, long _gsInfoSerialize, String _customData)
    {
        _m_lGSSessionId = _gsSessionId;
        _m_iGSId = _gsId;
        _m_lGSInfoSerialize = _gsInfoSerialize;
        _m_customData = _customData;

        //重设为有效数据
        _m_bIsEnable = true;
    }
}
