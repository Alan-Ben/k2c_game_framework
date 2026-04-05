package NPGateServer.NPGCCheckCodeMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import NPGateServer.NPGCCheckCodeMgr.SynTask.NPSynCheckCodeCheckTask;
import NPGateServer.NPGateServer;

import java.util.HashMap;
import java.util.LinkedList;

/*****************
 * 客户端连接验证串的管理对象
 * @author Administrator
 *
 */
public class NPGCCheckCodeMgr
{
    private static NPGCCheckCodeMgr _g_instance = new NPGCCheckCodeMgr();

    public static NPGCCheckCodeMgr getInstance()
    {
        return _g_instance;
    }

    private HashMap<String, NPGCCheckCodeInfo> _m_hmCheckCodeMap;
    //需要检测有效性的队列
    private LinkedList<NPGCCheckCodeInfo> _m_lCheckCodeList;
    private MutexAtom _m_mutex;

    protected NPGCCheckCodeMgr()
    {
        _m_hmCheckCodeMap = new HashMap<String, NPGCCheckCodeInfo>();
        _m_lCheckCodeList = new LinkedList<NPGCCheckCodeInfo>();
        _m_mutex = new MutexAtom();

        //注册处理
        ALSynTaskManager.getInstance().regTask(new NPSynCheckCodeCheckTask());
    }

    /****************
     * 增加一个用户的验证串
     * @param _uid
     * @return
     */
    public NPGCCheckCodeInfo addUserCheckCode(String _uid)
    {
        _lock();

        try
        {
            NPGCCheckCodeInfo newInfo = new NPGCCheckCodeInfo(_uid);
            _m_hmCheckCodeMap.put(_uid, newInfo);
            _m_lCheckCodeList.add(newInfo);

            return newInfo;
        } finally
        {
            _unlock();
        }
    }

    /****************
     * 验证对应串并返回结果
     * @param _uid
     * @return
     */
    public NPGCCheckCodeInfo checkUserCheckCode(String _uid, String _checkCode)
    {
        _lock();

        try
        {
            NPGCCheckCodeInfo newInfo = _m_hmCheckCodeMap.get(_uid);
            if (null == newInfo)
                return null;

            //判断数据是否匹配
            if (!newInfo.getCheckCode().equalsIgnoreCase(_checkCode))
                return null;

            //移除数据
            _m_hmCheckCodeMap.remove(_uid);
            newInfo.disable();

            return newInfo;
        } finally
        {
            _unlock();
        }
    }

    /****************
     * 验证对应串并返回结果
     * @param _uid
     * @return
     */
    public void checkCheckCodeTime()
    {
        do
        {
            _lock();

            try
            {
                if (_m_lCheckCodeList.isEmpty())
                    return;

                NPGCCheckCodeInfo firstInfo = _m_lCheckCodeList.getFirst();
                if (null == firstInfo || !firstInfo.isEnable())
                {
                    _m_lCheckCodeList.removeFirst();
                    continue;
                }

                //判断是否超时，是则进入超时处理，否则返回
                if (!firstInfo.isTimeout())
                    break;

                //移除第一个
                _m_lCheckCodeList.removeFirst();
                //发送消息减少用户承载
                NPGateServer.getInstance().reduceHandleUser(firstInfo.getUid());
            } finally
            {
                _unlock();
            }
        } while (true);
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
