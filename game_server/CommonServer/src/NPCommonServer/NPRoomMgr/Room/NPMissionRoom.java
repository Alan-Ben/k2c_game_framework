package NPCommonServer.NPRoomMgr.Room;

import NPCommonServer.NPRoomMgr._ANPRoom;

/***************
 * 关卡对应的房间
 *
 * @author alzq.z
 * @email zhuangfan@vip.163.com
 * @time 2019年9月5日 上午12:01:27
 */
public class NPMissionRoom extends _ANPRoom
{
    /**
     * 对应处理的Room服务器类型Id
     */
    private int _m_iRoomServerTypeId;

    public NPMissionRoom(long _serial)
    {
        super(_serial);

        _m_iRoomServerTypeId = -1;
    }

    public NPMissionRoom(long _serial, int _roomServerTypeId)
    {
        super(_serial);

        _m_iRoomServerTypeId = _roomServerTypeId;
    }

    public int getRoomServerTypeId()
    {
        return _m_iRoomServerTypeId;
    }

    @Override
    public void onQuit()
    {
    }

    @Override
    public void onRegisted()
    {
    }

    @Override
    public void onUnRegisted()
    {
    }
}
