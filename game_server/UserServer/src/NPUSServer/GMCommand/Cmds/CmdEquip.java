package NPUSServer.GMCommand.Cmds;

import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.Util.Pair.WCGPairInt;
import NPUSServer.GMCommand.UsCmdBase;


/**
 * 藏品GM命令
 */
@ACommander(comment = "藏品", name = "equip")
public class CmdEquip extends UsCmdBase
{
    @ACommand(comment = "获得藏品[藏品id]")
    public String gain(long _equipId)
    {
        return getOwner().getEquipComponent().gainEquip(_equipId, false, getContext()).toString();
    }

    @ACommand(comment = "一键下架所有藏品")
    public String unWearAll()
    {
        WCGPairInt data = getOwner().getEquipComponent().gmUnWearAll(getContext());
        return "unwear count:" + data.first() + ", fail unwear count:" + data.second();
    }

    @ACommand(comment = "删除藏品[藏品数据库id]")
    public String delete(long _equipDbId)
    {
        return getOwner().getEquipComponent().gmRemoveEquip(_equipDbId, getContext()).toString();
    }

    @ACommand(comment = "重置藏品技能为1%[藏品数据库id]")
    public String resetSkills(long _equipDbId)
    {
        return getOwner().getEquipComponent().gmResetSkills(_equipDbId, getContext()).toString();
    }

    @ACommand(comment = "设置藏品等级[藏品数据库id][等级]")
    public String setLevel(long _equipDbId, int _level)
    {
        return getOwner().getEquipComponent().gmSetLevel(_equipDbId, _level, getContext()).toString();
    }
}
