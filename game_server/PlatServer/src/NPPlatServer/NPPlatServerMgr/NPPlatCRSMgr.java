package NPPlatServer.NPPlatServerMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALServerLog.ALServerLog;
import NPPlatServer.NPPS_Listener.PS_CRSListener;
import NPPlatServer.NPPS_Listener.PS_CSListener;
import NPServerProtocolWriter.NP2CS.Msg.NP2CS_Writer_001_BasicOp;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;

/****************
 * 平台中管理的RoomServer信息对象
 *
 * @author Administrator
 *
 */
public class NPPlatCRSMgr
{
    private static NPPlatCRSMgr _g_instance = new NPPlatCRSMgr();

    public static NPPlatCRSMgr getInstance()
    {
        if (null == _g_instance)
            _g_instance = null;

        return _g_instance;
    }

    /**
     * 本区域的房间服务器列表
     */
    private Map<Integer, PS_CRSListener> _m_lAreaCRSMap;

    /**
     * 管理类操作锁，由于管理类中的列表操作不频繁，因此与统计共用一个锁
     */
    private MutexAtom _m_mutex;

    protected NPPlatCRSMgr()
    {
        _m_lAreaCRSMap = new HashMap<>();

        _m_mutex = new MutexAtom();
    }

    /*****************
     * 注册房间服务器对象
     */
    public void regCRS(PS_CRSListener _crsListener)
    {
        if (null == _crsListener)
            return;

        _lock();

        try
        {
            // 将对象加入队列
            _m_lAreaCRSMap.put(_crsListener.getServerTypeId(), _crsListener);

            ALServerLog.Sys("Reg Room Server: " + _crsListener.getServerTypeId());

            // 发送消息通知common服务器，添加房间服务器
            PS_CSListener.sendCustMsg(NP2CS_Writer_001_BasicOp.make_003_AddCrossRankServer(_crsListener.getServerTypeId()));
        } finally
        {
            _unlock();
        }
    }

    /**
     * 构造数据列表
     * @param _typeIdList
     */
    public void makeProtoList(ArrayList<Integer> _typeIdList)
    {
        _lock();
        try
        {
            for (PS_CRSListener listener : _m_lAreaCRSMap.values())
            {
                if (null == listener)
                    continue;

                _typeIdList.add(listener.getServerTypeId());
            }
        } finally
        {
            _unlock();
        }
    }

    protected void _lock()
    {
        _m_mutex.lock();
    }

    protected void _unlock()
    {
        _m_mutex.unlock();
    }
}
