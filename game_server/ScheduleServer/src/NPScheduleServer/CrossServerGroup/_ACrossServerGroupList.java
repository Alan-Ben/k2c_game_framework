package NPScheduleServer.CrossServerGroup;

import ALBasicServer.ALBasicMutex.MutexAtom;
import NPCommon.Log.CommLog;
import SSDB.Bo.CrossServerGroupItemBO;

import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Set;

/**
 * 跨服分组组对象列表基类
 * 用于管理跨服对象
 */
public abstract class _ACrossServerGroupList
{
    //已经处理的服务器集合 用于判断是否已经被安排进分组中
    protected Set<Integer> _m_hasOrderUsIdSet;
    //分组对象列表
    protected List<CrossServerGroupItem> _m_groupItemList;
    //锁
    protected MutexAtom _m_mutex;

    public _ACrossServerGroupList()
    {
        _m_hasOrderUsIdSet = new HashSet<>();
        _m_groupItemList = new ArrayList<>();
        _m_mutex = new MutexAtom();
    }

    protected void _lock()
    {
        _m_mutex.lock();
    }

    protected void _unlock()
    {
        _m_mutex.unlock();
    }

    /**
     * 初始化分组对象
     * @param _bo 分组对象bo
     */
    public void initAddGroupItem(CrossServerGroupItemBO _bo)
    {
        CrossServerGroupItem groupItem = new CrossServerGroupItem(_bo, this);
        _m_groupItemList.add(groupItem);
        _m_hasOrderUsIdSet.addAll(groupItem.getUsIdList());
    }

    /**
     * 检查是否已经在分组内
     * @param _usId usId
     * @return 是否已经在分组内
     */
    public boolean checkInGroup(int _usId)
    {
        _lock();
        try
        {
            return _m_hasOrderUsIdSet.contains(_usId);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 检查是否已经在分组内
     * @param _usIdList usId列表
     * @return 是否已经在分组内
     */
    public boolean checkInGroup(List<Integer> _usIdList)
    {
        _lock();
        try
        {
            for (Integer usId : _usIdList)
            {
                if (_m_hasOrderUsIdSet.contains(usId))
                    return true;
            }
            return false;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 查找us对应的分组对象
     * @param _usId usId
     * @return 分组对象
     */
    public CrossServerGroupItem lookupUsBelongingGroupItem(int _usId)
    {
        _lock();
        try
        {
            for (CrossServerGroupItem groupItem : _m_groupItemList)
            {
                if (groupItem.containsUs(_usId))
                    return groupItem;
            }
            return null;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 查找对应的分组对象
     * @param _groupId 分组id
     * @return 分组对象
     */
    public CrossServerGroupItem lookupGroupItem(long _groupId)
    {
        _lock();
        try
        {
            for (CrossServerGroupItem groupItem : _m_groupItemList)
            {
                if (groupItem.getGroupId() == _groupId)
                    return groupItem;
            }
            return null;
        } finally
        {
            _unlock();
        }
    }

    public List<CrossServerGroupItem> getGroupItemList()
    {
        _lock();
        try
        {
            return new ArrayList<>(_m_groupItemList);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 获取分组包含的UsId列表，如果找不到对应的分组，则返回空
     * @param _groupIdList 分组id列表
     * @return UsId列表
     */
    public List<Integer> getUsListByGroupList(List<Long> _groupIdList)
    {
        _lock();
        try
        {
            List<Integer> usIdList = new ArrayList<>();
            for (Long groupId : _groupIdList)
            {
                //如果找不到对应的分组
                CrossServerGroupItem groupItem = lookupGroupItem(groupId);
                if (groupItem == null)
                {
                	CommLog.error("Get Cross Server Group:{} fail.", groupId);
                	return null;
                }

                usIdList.addAll(groupItem.getUsIdList());
            }
            return usIdList;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 清除分组数据
     */
    public void cleanGroupData()
    {
        _lock();
        try
        {
            _m_groupItemList.clear();
            _m_hasOrderUsIdSet.clear();
        } finally
        {
            _unlock();
        }
    }
}
