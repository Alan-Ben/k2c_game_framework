package NPCommonServer.CSServerMgr;

/******************
 * 公共服务器中间对应区域的用户信息对象
 *
 * @author Administrator
 *
 */
public class CSAreaUserServerInfo
{
    /**
     * 用户服务器的类型Id
     */
    private int _m_iUserServerTypeId;

    private long _m_lUsServerId;

    public CSAreaUserServerInfo(int _userServerTypeId, long _usServerId)
    {
        _m_iUserServerTypeId = _userServerTypeId;
        _m_lUsServerId = _usServerId;
    }

    public int getUserServerTypeId()
    {
        return _m_iUserServerTypeId;
    }

    public long getUsServerId()
    {
        return _m_lUsServerId;
    }
}
