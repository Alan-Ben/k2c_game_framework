package GameLogicServer.GroupMgr.ActivityFactory;

import GameLogicServer.GroupMgr.ActivityMgr._ATActivityInfo;

/******
 * 活动对象的创建接口
 */
@FunctionalInterface
public interface _IActivityCreator
{
    _ATActivityInfo create(long _groupId, long _activityId);
}