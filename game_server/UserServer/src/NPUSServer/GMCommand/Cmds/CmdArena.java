package NPUSServer.GMCommand.Cmds;

import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPUSServer.GMCommand.UsCmdBase;

@ACommander(comment = "竞技场相关", name = "arena")
public class CmdArena extends UsCmdBase
{
    @ACommand(comment = "重置随机攻击次数")
    public String resetRandomCount()
    {
        getOwner().getArenaComponent().resetHadRandomAttackNum();
        return "done";
    }

    @ACommand(comment = "重置指定攻击次数")
    public String resetSelectCount()
    {
        getOwner().getArenaComponent().resetHadSelectAttackNum();
        return "done";
    }

    @ACommand(comment = "重置已购买随机攻击次数")
    public String resetHadBuyRandomAttackNum()
    {
        getOwner().getArenaComponent().resetHadBuyRandomAttackNum();
        return "done";
    }

    @ACommand(comment = "重置已选择攻击的英雄id")
    public String resetHadSelectAttackHeroList()
    {
        getOwner().getArenaComponent().resetHadSelectAttackHeroList();
        return "done";
    }

    @ACommand(comment = "重置已随机攻击的英雄id")
    public String resetHadRandomAttackHeroList()
    {
        getOwner().getArenaComponent().resetHadRandomAttackHeroList();
        return "done";
    }

    @ACommand(comment = "删除当前玩家战斗")
    public String deleteBattle()
    {
        boolean success = getOwner().getArenaComponent().deleteBattleInfo();
        return success ? "done" : "battle not found";
    }


}
