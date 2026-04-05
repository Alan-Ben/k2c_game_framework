package NPUSServer.GMCommand.Cmds;

import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPUSServer.GMCommand.UsCmdBase;

/**
 * 排行榜相关GM命令
 *
 * 主要功能：
 * 1. 删除排行榜子对象（玩家在排行榜中的数据）
 * 2. 删除排行榜对象（整个排行榜数据）
 */
@ACommander(comment = "排行榜命令", name = "rank")
public class CmdRank extends UsCmdBase
{
    /**
     * 删除排行榜子对象
     *
     * 功能说明：从指定排行榜实例中删除某个对象下的子对象数据
     *
     * @param _rankId 排行榜ID
     * @param _rankInstanceId 排行榜实例ID
     * @param _objId 对象ID
     * @param _subObjId 子对象ID
     * @return 执行结果
     */
    @ACommand(comment = "删除排行榜子对象[排行榜ID,排行榜实例ID,对象ID,子对象ID]")
    public String removeSubObj(long _rankId, long _rankInstanceId, long _objId, long _subObjId)
    {
        // 调用排行榜实例功能类的删除子对象方法
        getUserServer().getRankingInstanceFunc().removeSubObj(_rankId, _rankInstanceId, _objId, _subObjId);

        return "ok";
    }

    /**
     * 删除排行榜对象
     *
     * 功能说明：从指定排行榜实例中删除某个对象的所有数据
     *
     * @param _rankId 排行榜ID
     * @param _rankInstanceId 排行榜实例ID
     * @param _objId 对象ID
     * @return 执行结果
     */
    @ACommand(comment = "删除排行榜对象[排行榜ID,排行榜实例ID,对象ID]")
    public String removeObj(long _rankId, long _rankInstanceId, long _objId)
    {
        // 调用排行榜实例功能类的删除对象方法
        getUserServer().getRankingInstanceFunc().removeObj(_rankId, _rankInstanceId, _objId);

        return "ok";
    }
}
