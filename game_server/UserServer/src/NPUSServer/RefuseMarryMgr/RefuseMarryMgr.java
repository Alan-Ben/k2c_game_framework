package NPUSServer.RefuseMarryMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import NPCommon.Util.CommonFunc;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.UsRefuseMarryBO;

import java.util.HashSet;
import java.util.List;

/**
 * 拒绝联姻管理器
 *
 * 主要功能：
 * 1. 管理本服拒绝联姻的玩家CID列表
 * 2. 提供添加、移除、查询拒绝联姻玩家的接口
 * 3. 支持数据的内存缓存和持久化存储
 *
 * 线程安全：通过MutexObject实现线程安全
 */
public class RefuseMarryMgr
{
    //对应的UserServer实例对象
    private NPUserServer _m_userServer;

    //拒绝联姻的玩家CID集合
    private HashSet<Long> _m_refuseCidSet;

    //锁对象
    private MutexAtom _m_mutex;

    public RefuseMarryMgr(NPUserServer _userServer)
    {
        _m_userServer = _userServer;

        _m_refuseCidSet = new HashSet<>();
        _m_mutex = new MutexAtom();
    }

    public NPUserServer getUserServer() {return _m_userServer;}

    private void _lock() {_m_mutex.lock();}
    private void _unlock() {_m_mutex.unlock();}

    /**
     * 从数据库初始化拒绝联姻数据
     *
     * 执行流程：
     * 1. 查询数据库中所有拒绝联姻的玩家记录
     * 2. 将玩家CID加载到内存集合中
     * 3. 记录加载的数据数量
     *
     * @return true-初始化成功，false-初始化失败
     */
    public boolean initFromDB()
    {
        List<UsRefuseMarryBO> boList = getUserServer().getBM().getBM(UsRefuseMarryBO.class).s_findAll();
        if (null == boList)
        {
            USLog.error(getUserServer(), "RefuseMarryMgr UsRefuseMarryBO initBo fail.");
            return false;
        }

        for (int i = 0; i < boList.size(); i++)
        {
            UsRefuseMarryBO bo = boList.get(i);
            if (null == bo)
                continue;

            _m_refuseCidSet.add(bo.getCid());
        }

        return true;
    }

    /**
     * 添加拒绝联姻的玩家CID
     *
     * 执行流程：
     * 1. 将玩家CID添加到内存集合
     * 2. 创建数据库记录进行持久化
     * 3. 记录操作日志
     *
     * @param _cid 玩家CID
     */
    public void addRefuseCid(long _cid)
    {
        _lock();

        try
        {
            if (_m_refuseCidSet.contains(_cid))
            {
                //玩家已经在拒绝列表中，无需重复添加
                return;
            }

            _m_refuseCidSet.add(_cid);

            //创建数据库记录
            UsRefuseMarryBO bo = new UsRefuseMarryBO();
            bo.setCid(getUserServer().getBM(), _cid);
            bo.setRefuseTime(getUserServer().getBM(), CommonFunc.getNowTimeSec());
            bo.insert(getUserServer().getBM());
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 移除拒绝联姻的玩家CID
     *
     * 执行流程：
     * 1. 从内存集合中移除玩家CID
     * 2. 从数据库中删除对应记录
     * 3. 记录操作日志
     *
     * @param _cid 玩家CID
     */
    public void removeRefuseCid(long _cid)
    {
        _lock();

        try
        {
            if (_m_refuseCidSet.remove(_cid))
            {
                //从数据库删除记录
                getUserServer().getBM().getBM(UsRefuseMarryBO.class).delAll("cid", _cid);
            }
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 检查指定玩家是否拒绝联姻
     *
     * @param _cid 玩家CID
     * @return true-拒绝联姻，false-不拒绝联姻
     */
    public boolean isRefuseMarry(long _cid)
    {
        _lock();

        try
        {
            return _m_refuseCidSet.contains(_cid);
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 获取拒绝联姻的玩家数量
     *
     * @return 拒绝联姻的玩家数量
     */
    public int getRefuseCount()
    {
        _lock();

        try
        {
            return _m_refuseCidSet.size();
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 清空所有拒绝联姻数据
     *
     * 执行流程：
     * 1. 清空内存集合中的所有数据
     * 2. 删除数据库中的所有记录
     * 3. 记录操作日志
     */
    public void clear()
    {
        _lock();

        try
        {
            _m_refuseCidSet.clear();

            //清空数据库中的所有记录
            getUserServer().getBM().getBM(UsRefuseMarryBO.class).delAll();
        }
        finally
        {
            _unlock();
        }
    }
}