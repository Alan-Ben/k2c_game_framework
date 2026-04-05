package NPCommonServer.NPRoomMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import NPCommon.Log.CommLog;
import NPCommonServer.NPCommParams;
import WCGCommon.Enum.ECommParam;

import java.util.HashMap;
import java.util.Map;

/*********************
 * 所有房间的注册管理对象
 *
 * @author alzq.z
 * @email zhuangfan@vip.163.com
 * @time 2019年9月4日 下午11:50:41
 */
public class NPRoomMgr
{
    private static NPRoomMgr _instance = new NPRoomMgr();

    public static NPRoomMgr getInstance()
    {
        return _instance;
    }

    public static synchronized long getNewRoomSerialize()
    {
        return NPCommParams.getInstance().incParam(ECommParam.ROOM_SERIALIZE_ID);
    }


    private Map<Long, _ANPRoom> _m_roomMap = new HashMap<Long, _ANPRoom>();
    private MutexAtom _m_mutex = new MutexAtom();
    ;

    protected void _lock()
    {
        _m_mutex.lock();
    }

    protected void _unlock()
    {
        _m_mutex.unlock();
    }

    /******************************
     * 查看指定战斗房间
     *
     * @param _serial
     * @return
     */
    public _ANPRoom lookupCommRoom(long _serial)
    {
        _lock();
        try
        {
            return _m_roomMap.get(_serial);
        } finally
        {
            _unlock();
        }
    }

    /******************************
     * 注册战斗房间
     *
     * @param _room
     */
    public void registRoom(_ANPRoom _room)
    {
        if (null == _room)
            return;

        _lock();
        try
        {
            _ANPRoom old = _m_roomMap.put(_room.getSerial(), _room);
            if (old != null)
            {
                CommLog.error("duplicated regist _room id:{}", _room.getSerial());
                old.onUnRegisted();
            }

            //调用创建事件
            _room.onRegisted();
        } finally
        {
            _unlock();
        }
    }

    /******************************
     * 卸载战斗房间
     *
     * @param _room
     * @return
     */
    public _ANPRoom unRegistRoom(_ANPRoom _room)
    {
        return unRegistRoom(_room.getSerial());
    }

    /******************************
     * 卸载战斗房间by房间号
     *
     * @param _roomSerial
     * @return
     */
    public _ANPRoom unRegistRoom(long _roomSerial)
    {
        _lock();
        try
        {
            _ANPRoom old = _m_roomMap.remove(_roomSerial);

            if (null != old)
                old.onUnRegisted();

            return old;
        } finally
        {
            _unlock();
        }
    }

    public int getRoomCount()
    {
        _lock();
        try
        {
            return _m_roomMap.size();
        } finally
        {
            _unlock();
        }
    }
}
