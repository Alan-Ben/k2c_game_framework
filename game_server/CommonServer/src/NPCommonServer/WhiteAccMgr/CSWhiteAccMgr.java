package NPCommonServer.WhiteAccMgr;

import ALBasicCommon.ALSerializeMaker;
import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.ServerObj.ServerObj_WhiteAccList;
import NPCommon.Log.CommLog;

import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;

/**
 * CommonServer白名单管理器
 *
 * 主要功能：
 * 1. 维护白名单账号集合（仅内存缓存）
 * 2. 启动时从HttpServer拉取完整白名单
 * 3. 接收HttpServer推送的白名单更新
 * 4. 提供线程安全的白名单查询接口
 *
 * 线程安全：使用MutexAtom锁保护白名单数据
 */
public class CSWhiteAccMgr
{
    // 单例
    private static final CSWhiteAccMgr _s_instance = new CSWhiteAccMgr();

    public static CSWhiteAccMgr getInstance()
    {
        return _s_instance;
    }

    // 白名单账号数据容器
    private HashSet<String> _m_hsWhiteAccountSet;

    // 锁对象
    private MutexAtom _m_mutex;

    // 操作序列号，避免任务重启引发重新加载引发计数错误
    private long _m_lOpSerialize;

    public CSWhiteAccMgr()
    {
        _m_hsWhiteAccountSet = new HashSet<>();
        _m_mutex = new MutexAtom();

        _m_lOpSerialize = ALSerializeMaker.makeNewSerialize();
    }

    private void _lock()
    {
        _m_mutex.lock();
    }

    private void _unlock()
    {
        _m_mutex.unlock();
    }

    public long getOpSerialize()
    {
        return _m_lOpSerialize;
    }

    /**
     * 开启加载白名单
     *
     * 执行流程：
     * 1. 创建拉取任务
     * 2. 立即注册到任务管理器执行
     */
    public void startLoad()
    {
        _m_lOpSerialize = ALSerializeMaker.makeNewSerialize();

        CommLog.info("CSWhiteAccMgr start load white acc list from HttpServer");

        ALSynTaskManager.getInstance().regTask(new CSSendToGetWhiteAccListTask(_m_lOpSerialize));
    }

    /**
     * 加载白名单数据
     * @param _whiteAccList  白名单数据对象
     *                       <p>
     *                       执行流程：
     *                       1. 清空现有数据
     *                       2. 加载新的白名单列表
     *                       3. 输出加载结果日志
     * @param _opSerialize
     */
    public void loadWhiteAccList(ServerObj_WhiteAccList _whiteAccList, long _opSerialize)
    {
        // 检查操作序列号，避免任务重启引发重新加载引发计数错误
        if (_opSerialize != _m_lOpSerialize)
            return;

        _lock();
        try
        {
            _m_hsWhiteAccountSet.clear();

            if (_whiteAccList != null && _whiteAccList.getAccList() != null)
            {
                for (String acc : _whiteAccList.getAccList())
                {
                    if (acc != null && !acc.isEmpty())
                    {
                        _m_hsWhiteAccountSet.add(acc);
                    }
                }
            }

            CommLog.info("CSWhiteAccMgr load white acc list success, count:{} serial:{}", _m_hsWhiteAccountSet.size(), _opSerialize);
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 添加账号到白名单（接收HttpServer推送）
     *
     * @param _acc 账号
     *
     * 线程安全：通过锁保护
     */
    public void addAcc(String _acc)
    {
        _lock();
        try
        {
            if (_acc == null || _acc.isEmpty())
            {
                CommLog.error("CSWhiteAccMgr addAcc fail, acc is null or empty");
                return;
            }

            if (_m_hsWhiteAccountSet.contains(_acc))
            {
                CommLog.info("CSWhiteAccMgr addAcc, acc already exists: {}", _acc);
                return;
            }

            _m_hsWhiteAccountSet.add(_acc);
            CommLog.info("CSWhiteAccMgr addAcc success, acc: {}, total count: {}", _acc, _m_hsWhiteAccountSet.size());
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 从白名单移除账号（接收HttpServer推送）
     *
     * @param _acc 账号
     *
     * 线程安全：通过锁保护
     */
    public void removeAcc(String _acc)
    {
        _lock();
        try
        {
            if (_acc == null || _acc.isEmpty())
            {
                CommLog.error("CSWhiteAccMgr removeAcc fail, acc is null or empty");
                return;
            }

            boolean removed = _m_hsWhiteAccountSet.remove(_acc);
            if (removed)
            {
                CommLog.info("CSWhiteAccMgr removeAcc success, acc: {}, total count: {}", _acc, _m_hsWhiteAccountSet.size());
            }
            else
            {
                CommLog.info("CSWhiteAccMgr removeAcc, acc not found: {}", _acc);
            }
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 检测指定账户是否在白名单中
     *
     * @param _acc 账号
     * @return true=在白名单中，false=不在白名单中
     *
     * 线程安全：通过锁保护
     */
    public boolean checkAccInWhiteList(String _acc)
    {
        _lock();
        try
        {
            return _m_hsWhiteAccountSet.contains(_acc);
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 获取所有白名单账号列表
     *
     * @return 白名单账号列表
     *
     * 线程安全：通过锁保护
     */
    public List<String> getAllAccList()
    {
        _lock();
        try
        {
            return new ArrayList<>(_m_hsWhiteAccountSet);
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 打印所有白名单列表
     */
    @Override
    public String toString()
    {
        _lock();
        try
        {
            return String.join(",", _m_hsWhiteAccountSet);
        }
        finally
        {
            _unlock();
        }
    }
}
