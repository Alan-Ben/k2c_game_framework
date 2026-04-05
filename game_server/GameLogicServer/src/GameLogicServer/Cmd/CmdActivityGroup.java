package GameLogicServer.Cmd;

import GameLogicServer.GroupMgr.GroupInstanceInfo;
import GameLogicServer.GroupMgr.GroupInstanceMgr;
import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.GMCommand.CmdClassBase;

/**
 * 活动分组实例相关GM命令
 *
 * 主要功能：
 * 1. 对指定分组实例手动增减US服务器
 * 2. 手动触发分组实例销毁流程
 */
@ACommander(comment = "活动分组实例相关命令", name = "activityGroup")
public class CmdActivityGroup extends CmdClassBase
{
    /**
     * 对指定分组实例添加US服务器（实例ID，US服务器ID）
     */
    @ACommand(comment = "对指定分组实例添加US（实例ID，US服务器ID）")
    public String addUs(long _instanceId, int _usId)
    {
        GroupInstanceInfo instance = GroupInstanceMgr.getInstance().tryLookup(_instanceId);
        if (null == instance)
            return "instance not found, instanceId=" + _instanceId;

        instance.addUsId(_usId);
        return "done, instanceId=" + _instanceId + " usId=" + _usId;
    }

    /**
     * 对指定分组实例移除US服务器（实例ID，US服务器ID）
     */
    @ACommand(comment = "对指定分组实例移除US（实例ID，US服务器ID）")
    public String removeUs(long _instanceId, int _usId)
    {
        GroupInstanceInfo instance = GroupInstanceMgr.getInstance().tryLookup(_instanceId);
        if (null == instance)
            return "instance not found, instanceId=" + _instanceId;

        instance.removeUsId(_usId);
        return "done, instanceId=" + _instanceId + " usId=" + _usId;
    }

    /**
     * 尝试销毁指定分组实例（实例ID）
     */
    @ACommand(comment = "尝试销毁指定分组实例（实例ID）")
    public String tryDiscard(long _instanceId)
    {
        GroupInstanceMgr.getInstance().tryDiscard(_instanceId);
        return "tryDiscard sent, instanceId=" + _instanceId;
    }
}

