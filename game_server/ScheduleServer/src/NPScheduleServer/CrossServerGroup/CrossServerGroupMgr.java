package NPScheduleServer.CrossServerGroup;

import ALBasicServer.ALBasicMutex.MutexObject;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.ScheduleErr;
import NPCommon.Log.CommLog;
import NPScheduleServer.CrossServerGroup.PrepareGroup.PrepareCrossServerGroupList;
import NPScheduleServer.CrossServerGroup.WorkGroup.WorkCrossServerGroupList;
import NPScheduleServer.NPScheduleServer;
import SSDB.Bo.CrossServerGroupItemBO;
import SSDB.Bo.CrossServerGroupMgrBO;

import java.util.List;

/**
 * 跨服服务器分组管理器
 * 管理器本身维护两个id：跨服分组过期的最大分组ID、跨服分组生效中的最大分组ID
 * 通过以上两个id，来区分分组所处状态。
 */
public class CrossServerGroupMgr
{
    private static CrossServerGroupMgr _g_instance = new CrossServerGroupMgr();

    public static CrossServerGroupMgr getInstance()
    {
        return _g_instance;
    }

    private CrossServerGroupMgrBO _m_mgrBo = new CrossServerGroupMgrBO();
    private MutexObject _m_mutex = new MutexObject();
    protected void _lock()
    {
        _m_mutex.lock();
    }

    protected void _unlock()
    {
        _m_mutex.unlock();
    }

    //预分组对象
    private PrepareCrossServerGroupList _m_prepareGroup = new PrepareCrossServerGroupList();
    //当前分组对象
    private WorkCrossServerGroupList _m_workingGroup = new WorkCrossServerGroupList();

    public PrepareCrossServerGroupList getPrepareGroup()
    {
        return _m_prepareGroup;
    }

    public WorkCrossServerGroupList getWorkingGroup()
    {
        return _m_workingGroup;
    }

    /**
     * 判断是否是过期的分组
     * @param _groupId
     * @return
     */
    public boolean isExpiredGroupId(long _groupId)
    {
        _lock();
        try{
            return _groupId <= _m_mgrBo.getMaxExpiredCrossServerGroupId();
        }finally{
            _unlock();
        }
    }

    /**
     * 判断是否是正在生效的分组
     * @param _groupId
     * @return
     */
    public boolean isWorkingGroupId(long _groupId)
    {
        _lock();
        try{
            return _groupId > _m_mgrBo.getMaxExpiredCrossServerGroupId() && _groupId <= _m_mgrBo.getMaxWorkCrossServerGroupId();
        }finally{
            _unlock();
        }
    }

    /**
     * 判断是否是已经使用的分组
     * @param _groupId
     * @return
     */
    public boolean isHadUseGroupId(long _groupId)
    {
        _lock();
        try{
            return _groupId <= _m_mgrBo.getMaxWorkCrossServerGroupId();
        }finally{
            _unlock();
        }
    }

    /**
     * 跨服分组过期的最大分组ID
     * @return
     */
    public long getMaxExpiredGroupId()
    {
        _lock();
        try{
            return _m_mgrBo.getMaxExpiredCrossServerGroupId();
        }finally{
            _unlock();
        }
    }

    /**
     * 跨服分组生效中的最大分组ID
     * @return
     */
    public long getMaxWorkCrossServerGroupId()
    {
        _lock();
        try{
            return _m_mgrBo.getMaxWorkCrossServerGroupId();
        }finally{
            _unlock();
        }
    }


    /**
     * 从数据库初始化
     * @return 是否成功
     */
    public boolean initFromDB()
    {
        if (!_initMgr())
            return false;

        if (!_initGroupItem())
            return false;

        getWorkingGroup().onInited();

        return true;
    }

    private boolean _initMgr()
    {
        List<CrossServerGroupMgrBO> boList = NPScheduleServer.getInstance().getBM().getBM(CrossServerGroupMgrBO.class).s_findAll();
        if (boList == null)
            return false;

        if (boList.isEmpty())
        {
            CrossServerGroupMgrBO bo = new CrossServerGroupMgrBO();
            bo.insert(NPScheduleServer.getInstance().getBM());

            _m_mgrBo = bo;
            return true;
        } else
        {
            _m_mgrBo = boList.get(0);
        }

        return true;
    }

    /**
     * 初始化分组信息
     * @return
     */
    private boolean _initGroupItem()
    {
        List<CrossServerGroupItemBO> boList = NPScheduleServer.getInstance().getBM().getBM(CrossServerGroupItemBO.class).s_findAll();
        if (null == boList)
        {
            CommLog.error("CrossServerGroupMgr _initGroupItem Fail, Bo Fail");
            return false;
        }

        for (CrossServerGroupItemBO bo : boList)
        {
            //如果是已经过期的分组，则不进内存，跳过
            if (isExpiredGroupId(bo.getGroupId()))
                continue;

            //通过判断分组，区分工作中分组和待生效分组
            if (isWorkingGroupId(bo.getGroupId()))
            {
                _m_workingGroup.initAddGroupItem(bo);
            } else
            {
                _m_prepareGroup.initAddGroupItem(bo);
            }
        }

        return true;
    }

    /**
     * 激活预分组
     * @return 是否成功
     */
    public Result activePrepareGroup()
    {
        _lock();
        try{
            //取出预备分组列表中的分组信息
            List<CrossServerGroupItem> groupItemList = _m_prepareGroup.getGroupItemList();
            //如果预备分组为空，则不允许切换
            if (groupItemList.isEmpty())
            {
                return ScheduleErr.CROSS_SERVER_GROUP_LIST_IS_EMPTY;
            }

            //预分组中最大的分组id
            long maxGroupItemId = 0;
            for (CrossServerGroupItem groupItem : groupItemList)
            {
                if (groupItem.getGroupId() > maxGroupItemId)
                {
                    maxGroupItemId = groupItem.getGroupId();
                }
            }

            BM bmObj = NPScheduleServer.getInstance().getBM();
            //设置新的标识位用于区分分组状态
            _m_mgrBo.setMaxExpiredCrossServerGroupId(bmObj, _m_mgrBo.getMaxWorkCrossServerGroupId());
            _m_mgrBo.setMaxWorkCrossServerGroupId(bmObj, maxGroupItemId);
            _m_mgrBo.saveAllMarked(bmObj);

            //清空工作分组和预备分组
            _m_workingGroup.cleanGroupData();
            _m_prepareGroup.cleanGroupData();

            //处理原预分组的数据
            _m_workingGroup.dealActiveGroup(groupItemList);

            return Result.SUCC;
        }finally{
            _unlock();
        }
    }
}
