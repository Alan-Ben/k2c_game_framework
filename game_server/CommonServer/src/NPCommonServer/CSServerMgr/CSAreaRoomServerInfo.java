package NPCommonServer.CSServerMgr;

/******************
 * 公共服务器中间对应区域的房间信息对象
 * @author Administrator
 *
 */
public class CSAreaRoomServerInfo
{
    /**
     * 房间服务器的类型Id
     */
    private int _m_iRoomServerTypeId;

    /**
     * 当前服务器在申请的承载的人数
     */
    private long _m_lRoomTmpHandleUserCount;
    /**
     * 当前服务器已承载的人数
     */
    private long _m_lRoomServerhandleCount;

    public CSAreaRoomServerInfo(int _roomServerTypeId)
    {
        _m_iRoomServerTypeId = _roomServerTypeId;

        _m_lRoomTmpHandleUserCount = 0;
        _m_lRoomServerhandleCount = 0;
    }

    public int getRoomServerTypeId()
    {
        return _m_iRoomServerTypeId;
    }

    public long getRoomTotalHandleCount()
    {
        return _m_lRoomTmpHandleUserCount + _m_lRoomServerhandleCount;
    }

    public void chgTmpHandleCount(int _userCount)
    {
        _m_lRoomTmpHandleUserCount += _userCount;
    }

    /************
     * 根据房间承载人数，以及最新的服务器承载人数，刷新服务器承载信息
     * @param _roomHandleCount
     * @param _roomServerHandleCount
     */
    public void refreshHandleCount(long _roomHandleCount, long _roomServerHandleCount)
    {
        _m_lRoomTmpHandleUserCount -= _roomHandleCount;
        _m_lRoomServerhandleCount = _roomServerHandleCount;
    }
}
