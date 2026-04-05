package NPScheduleServer.CrossServerGroup.PrepareGroup;

import Common.NpServerObj.NPServerObj_CrossServerGroupInfo;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.ScheduleErr;
import NPCommon.Util.CommonFunc;
import NPScheduleServer.CrossServerGroup.CrossServerGroupItem;
import NPScheduleServer.CrossServerGroup.CrossServerGroupMgr;
import NPScheduleServer.CrossServerGroup._ACrossServerGroupList;
import NPScheduleServer.NPScheduleServer;
import SSDB.Bo.CrossServerGroupItemBO;

/**
 * 预备的跨服分组列表对象
 * 包含添加分组、修改分组、删除分组操作
 * 基本规则：分组id不得重复，us只能存在于当前分组列表中的一个分组中
 */
public class PrepareCrossServerGroupList extends _ACrossServerGroupList
{
    /**
     * 加入分组信息
     * @param _groupInfo 分组信息结构体
     * @return 错误码
     */
    public Result addGroup(NPServerObj_CrossServerGroupInfo _groupInfo)
    {
        //检查是否是曾经被使用过的分组id
        if (CrossServerGroupMgr.getInstance().isHadUseGroupId(_groupInfo.getGroupId()))
        {
            return ScheduleErr.CROSS_SERVER_GROUP_ID_HAD_USE;
        }

        _lock();
        try{
            //检查是否已知存在于任何一个分组内，如果已存在则返回报错
            boolean hasInGroup = checkInGroup(_groupInfo.getUsIdList());
            if (hasInGroup)
            {
                return ScheduleErr.CROSS_SERVER_GROUP_US_IN_OTHER_GROUP;
            }

            //检查添加的分组id是否重复
            CrossServerGroupItem groupItem = lookupGroupItem(_groupInfo.getGroupId());
            if (groupItem != null)
            {
                return ScheduleErr.CROSS_SERVER_GROUP_ID_REPEAT;
            }

            BM bmObj = NPScheduleServer.getInstance().getBM();
            CrossServerGroupItemBO bo = new CrossServerGroupItemBO();
            bo.setGroupId(bmObj, _groupInfo.getGroupId());
            bo.setUsIdList(bmObj, CommonFunc.list2String(_groupInfo.getUsIdList()));
            bo.setCrsGroupInstanceId(bmObj, _groupInfo.getCrsInstanceId());
            bo.insert(bmObj);

            groupItem = new CrossServerGroupItem(bo, this);
            _m_groupItemList.add(groupItem);

            return Result.SUCC;
        }finally{
            _unlock();
        }
    }

    /**
     * 修改分组信息
     * @param _groupInfo 分组信息结构体
     * @return 错误码
     */
    public Result editGroup(NPServerObj_CrossServerGroupInfo _groupInfo)
    {
        _lock();
        try{
            CrossServerGroupItem groupItem = lookupGroupItem(_groupInfo.getGroupId());
            if (groupItem == null)
            {
                return ScheduleErr.CROSS_SERVER_GROUP_NOT_FOUND;
            }

            //检查us是否被重复分配
            for (Integer usId : _groupInfo.getUsIdList())
            {
                //如果已经在当前分组中，则说明没有变化，跳过
                if (groupItem.containsUs(usId))
                    continue;

                //如果存在于整个已分配列表中，则说明重复分配
                if (_m_hasOrderUsIdSet.contains(usId))
                    return ScheduleErr.CROSS_SERVER_GROUP_US_IN_OTHER_GROUP;
            }

            //移除原有的UsId
            groupItem.getUsIdList().forEach(_m_hasOrderUsIdSet::remove);
            //修改组存在的usId列表
            groupItem.edit(_groupInfo.getUsIdList());
            //加入新的usId列表
            _m_hasOrderUsIdSet.addAll(_groupInfo.getUsIdList());

            return Result.SUCC;
        }finally{
            _unlock();
        }
    }

    /**
     * 移除分组信息
     * @param _groupId 分组id
     * @return 错误码
     */
    public Result delGroup(long _groupId)
    {
        _lock();
        try{
            CrossServerGroupItem groupItem = lookupGroupItem(_groupId);
            if (groupItem == null)
            {
                return ScheduleErr.CROSS_SERVER_GROUP_NOT_FOUND;
            }

            _m_groupItemList.remove(groupItem);
            groupItem.dispose();

            return Result.SUCC;
        }finally{
            _unlock();
        }
    }
}

