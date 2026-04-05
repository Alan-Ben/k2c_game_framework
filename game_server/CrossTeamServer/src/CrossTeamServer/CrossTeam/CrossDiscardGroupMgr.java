package CrossTeamServer.CrossTeam;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import CTSDB.Bo.CrossGroupDiscardBO;
import CrossTeamServer.CrossTeamServer;
import NPCommon.Util.CommonFunc;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

public class CrossDiscardGroupMgr
{
    //单例模式
    private static CrossDiscardGroupMgr _g_instance = new CrossDiscardGroupMgr();
    public static CrossDiscardGroupMgr getInstance()
    {
        return _g_instance;
    }

    private HashMap<Long, CrossGroupDiscardBO> _m_hmDiscardGroupMap;
    private ArrayList<CrossGroupDiscardBO> _m_alDiscardGroupList;

    private MutexAtom _m_mutex;

    public CrossDiscardGroupMgr()
    {
        _m_hmDiscardGroupMap = new HashMap<>();
        _m_alDiscardGroupList = new ArrayList<>();

        _m_mutex = new MutexAtom();
    }

    private void _lock() { _m_mutex.lock(); }
    private void _unlock() { _m_mutex.unlock(); }

    /**
     * 初始化数据库数据
     * @return
     */
    public boolean initFromDB()
    {
        List<CrossGroupDiscardBO> boList = CrossTeamServer.getInstance().getBM().getBM(CrossGroupDiscardBO.class).s_findAll();
        if (null == boList)
        {
            return false;
        }

        for (int i = 0; i < boList.size(); i++)
        {
            CrossGroupDiscardBO bo = boList.get(i);
            if (null == bo)
                continue;

            _m_hmDiscardGroupMap.put(bo.getGroupId(), bo);
            _m_alDiscardGroupList.add(bo);
        }

        _sort();

        //注册检查过期分组的任务
        ALSynTaskManager.getInstance().regTask(new CrossGroupCheckExpiredTask());

        return true;
    }

    /**
     * 根据销毁时间排序，时间越久远的排在越后面
     */
    private void _sort()
    {
        _m_alDiscardGroupList.sort((o1, o2) ->
        {
            return Long.compare(o2.getDiscardMs(), o1.getDiscardMs());
        });
    }

    /**
     * 获取指定分组队伍最大索引
     */
    public int getTeamLastIdx(long _groupId)
    {
        _lock();

        try
        {
            if (!_m_hmDiscardGroupMap.containsKey(_groupId))
                return 0;

            return _m_hmDiscardGroupMap.get(_groupId).getTeamMaxIdx();
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 添加销毁的分组
     * @param _group
     */
    public void addDiscardGroup(CrossGroup _group)
    {
        _lock();

        try
        {
            CrossGroupDiscardBO bo = _m_hmDiscardGroupMap.get(_group.getGroupId());
            if(null == bo)
            {
                bo = new CrossGroupDiscardBO();
                bo.setGroupId(CrossTeamServer.getInstance().getBM(), _group.getGroupId());
                bo.setTeamMaxIdx(CrossTeamServer.getInstance().getBM(), _group.getTeamMaxIdx());
                bo.setDiscardMs(CrossTeamServer.getInstance().getBM(), CommonFunc.getNowTimeMS());
                bo.insert(CrossTeamServer.getInstance().getBM());

                _m_hmDiscardGroupMap.put(bo.getGroupId(), bo);
                _m_alDiscardGroupList.add(bo);
            }
            else
            {
                bo.saveDiscardMs(CrossTeamServer.getInstance().getBM(), CommonFunc.getNowTimeMS());
            }

            _sort();
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 检查过期的分组，过期的分组会被删除
     * 数据只保留1天，过期后会被删除
     */
    public void checkExpired()
    {
        _lock();

        try
        {
            //删除的时间节点，小于该时间的分组会被删除
            long expiredTimeMS = CommonFunc.getNowTimeMS() - 86400000;

            while(_m_alDiscardGroupList.size() > 0)
            {
                int lastIdx = _m_alDiscardGroupList.size() - 1;
                CrossGroupDiscardBO bo = _m_alDiscardGroupList.remove(lastIdx);
                if (null == bo)
                    continue;

                if(bo.getDiscardMs() > expiredTimeMS)
                {
                    _m_alDiscardGroupList.add(bo);
                    break;
                }

                _m_hmDiscardGroupMap.remove(bo.getGroupId());
                bo.del(CrossTeamServer.getInstance().getBM());
            }
        }
        finally
        {
            _unlock();
        }
    }
}
