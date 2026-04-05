package GameLogicServer.GroupMgr;

import ALBasicServer.ALBasicMutex.MutexObject;
import GLSDB.Bo.GroupInstanceBO;
import GameLogicServer.GameLogicServer;
import GameLogicServer.GroupMgr.ActivityFactory.ActivityFactory;
import GameLogicServer.GroupMgr.ActivityMgr._ATActivityInfo;
import NPCommon.DB.BM.BM;
import NPCommon.Log.CommLog;

import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

public class GroupInstanceMgr
{
    //单例模式
    private static GroupInstanceMgr _g_instance = new GroupInstanceMgr();
    public static GroupInstanceMgr getInstance()
    {
        return _g_instance;
    }

    //尚在运行的分组实例管理
    private HashMap<Long, GroupInstanceInfo> _m_hmGroupInstanceMap;
    //等待销毁的分组实例ID集合，定时检查销毁
    private ArrayList<GroupInstanceInfo> _m_alDiscardGroupInstanceList;
    //锁对象
    private MutexObject _m_mutex;

    public GroupInstanceMgr()
    {
        _m_hmGroupInstanceMap = new HashMap<>();

        _m_alDiscardGroupInstanceList = new ArrayList<>();

        _m_mutex = new MutexObject();
    }

    private void _lock() {_m_mutex.lock();}
    private void _unlock() {_m_mutex.unlock();}

    public BM getBM() {return GameLogicServer.getInstance().getBM();}

    public boolean sInit()
    {
        //排行榜数据
        List<GroupInstanceBO> boList = GameLogicServer.getInstance().getBM().getBM(GroupInstanceBO.class).s_findAll();
        if (null == boList)
        {
            CommLog.error("GroupInstanceBO init fail.");
            return false;
        }

        for(int i = 0; i < boList.size(); i++)
        {
            GroupInstanceBO bo = boList.get(i);
            if (null == bo)
                continue;

            _ATActivityInfo activity = ActivityFactory.getInstance().createInstance(bo.getGroupId(), bo.getActivityId());
            if(null == activity)
            {
                CommLog.error("GroupInstanceBO init fail, create group:{} activity:{} fail.", bo.getGroupId(), bo.getActivityId());
                return false;
            }

            GroupInstanceInfo info = new GroupInstanceInfo(bo, activity);
            if(info.isNeedDiscard())
            {
                _m_alDiscardGroupInstanceList.add(info);
            }
            else
            {
                _m_hmGroupInstanceMap.put(info.getInstanceId(), info);
            }
        }

        // 全部活跃实例创建完毕后，批量加载各活动业务BO数据并在内存中分配
        if (!ActivityFactory.getInstance().sInitAllBoData(getBM(), _m_hmGroupInstanceMap))
        {
            CommLog.error("GroupInstanceMgr.sInit - activity bo data init fail.");
            return false;
        }

        if(!_m_alDiscardGroupInstanceList.isEmpty())
        {
            ArrayList<GroupInstanceInfo> needDiscardList = new ArrayList<>(_m_alDiscardGroupInstanceList);
            for(int i = 0; i < needDiscardList.size(); i++)
            {
                GroupInstanceInfo info = needDiscardList.get(i);
                if(null == info)
                    continue;

                checkDiscard(info);
            }
        }

        return true;
    }

    /**
     * 创建分组实例
     * @param _groupId
     * @param _activityId
     * @param _addInfo
     * @return
     */
    public GroupInstanceInfo create(long _groupId, long _activityId, ByteBuffer _addInfo)
    {
        _lock();

        try
        {
            //检查是否已经存在分组实例
            if(_m_hmGroupInstanceMap.containsKey(_groupId))
            {
                CommLog.error("group:{} activity:{} create instance fail, group multi.", _groupId, _activityId);
                return null;
            }

            //创建活动对象
            _ATActivityInfo activity = ActivityFactory.getInstance().createInstance(_groupId, _activityId);
            if(null == activity)
            {
                CommLog.error("group:{} activity:{} create instance fail.", _groupId, _activityId);
                return null;
            }
            //活动对象初始化
            if(!activity.init(_addInfo))
            {
                CommLog.error("group:{} activity:{} create instance init fail.", _groupId, _activityId);
                return null;
            }

            //创建分组实例数据
            GroupInstanceBO bo = new GroupInstanceBO();
            bo.setGroupId(getBM(), _groupId);
            bo.setActivityId(getBM(), _activityId);
            bo.setNeedDiscard(getBM(), false);
            bo.insert(getBM());

            GroupInstanceInfo info = new GroupInstanceInfo(bo, activity);
            _m_hmGroupInstanceMap.put(info.getInstanceId(), info);

            CommLog.info("group:{} instanceId:{} activity:{} create instance success.", _groupId, info.getInstanceId(), _activityId);

            return info;
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 根据实例ID查询分组实例对象
     * @param _instanceId
     * @return
     */
    public GroupInstanceInfo lookup(long _instanceId)
    {
        _lock();

        try
        {
            return _m_hmGroupInstanceMap.get(_instanceId);
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 尝试获取分组实例对象，需要先在尚在运行的分组实例管理中查询，若不存在则在等待销毁的分组实例ID集合中查询
     * @param _instanceId
     * @return
     */
    public GroupInstanceInfo tryLookup(long _instanceId)
    {
        _lock();

        try
        {
            //先在尚在运行的分组实例管理中查询
            GroupInstanceInfo instance = _m_hmGroupInstanceMap.get(_instanceId);
            if(null != instance)
                return instance;

            //尚在运行的分组实例管理中不存在，则在等待销毁的分组实例ID集合中查询
            for(int i = 0; i < _m_alDiscardGroupInstanceList.size(); i++)
            {
                instance = _m_alDiscardGroupInstanceList.get(i);
                if(null == instance)
                    continue;

                if(instance.getInstanceId() == _instanceId)
                    return instance;
            }

            return null;
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 尝试销毁分组实例，更新标志位，等待后续检查销毁
     * @param _instanceId
     */
    public void tryDiscard(long _instanceId)
    {
        _lock();

        try
        {
            GroupInstanceInfo instance = _m_hmGroupInstanceMap.remove(_instanceId);
            if(null == instance)
                return;

            instance.tryDiscard();
            _m_alDiscardGroupInstanceList.add(instance);

            checkDiscard(instance);
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 检查分组实例是否满足销毁条件，满足则销毁
     * @param _instance
     */
    public void checkDiscard(GroupInstanceInfo _instance)
    {
        _lock();

        try
        {
            //参与US尚未完全退出，继续等待
            if(!_instance.isUsIdSetEmpty())
                return;

            //已经不再等待销毁队列，说明之前已经销毁，无需重复销毁
            if(!_m_alDiscardGroupInstanceList.remove(_instance))
                return;

            _instance._discard();
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 无需检查直接销毁分组实例，慎重使用！！！
     * @param _instanceId
     */
    public void notCheckDiscard(long _instanceId)
    {
        GroupInstanceInfo instance = _m_hmGroupInstanceMap.remove(_instanceId);
        if(null == instance)
            return;

        instance._discard();
    }
}
