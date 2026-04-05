package NPScheduleServer.CrossServerGroup.WorkGroup;

import NPCommon.Log.CommLog;
import NPScheduleServer.CrossServerGroup.CrossServerGroupItem;
import NPScheduleServer.CrossServerGroup._ACrossServerGroupList;

import java.util.List;

/**
 * 当前正在工作的跨服分组列表对象
 */
public class WorkCrossServerGroupList extends _ACrossServerGroupList
{
    /**
     * 处理激活待分组
     * @param _groupItemList
     */
    public void dealActiveGroup(List<CrossServerGroupItem> _groupItemList)
    {
        _lock();
        try{
            _m_groupItemList.addAll(_groupItemList);

            //初始化分组的额外信息
            for (CrossServerGroupItem groupItem : _m_groupItemList)
            {
                groupItem.initGroupExtInfoAndPush();
            }
        }finally{
            _unlock();
        }
    }

    public void onInited()
    {
        _lock();
        try{
            //初始化分组的额外信息
            for (CrossServerGroupItem groupItem : _m_groupItemList)
            {
                if (groupItem.hasPush())
                    continue;

                groupItem.initGroupExtInfoAndPush();

                CommLog.info("CrossServerGroupMgr start deal not push group groupId:{}", groupItem.getGroupId());
            }
        }finally{
            _unlock();
        }
    }
}
