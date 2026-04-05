package MGClient.Cmd.Cmds;

import GC2GS.p037_GuildDungeonOp.GC2GS_037_002_ReqSetAutoStartDungeon;
import GC2GS.p037_GuildDungeonOp.GC2GS_037_005_ReqAttackDungeon;
import GC2GS.p037_GuildDungeonOp.GC2GS_037_006_ReqRecoverHeroFight;
import MGClient.Cmd.Annotation.Command;
import MGClient.Cmd.Annotation.Commander;
import MGClient.Cmd.CmdBase;

import java.util.ArrayList;

@Commander(comment = "联盟副本", name = "guildDungeon")
public class CmdGuildDungeon extends CmdBase
{
    @Command(comment = "设置联盟副本自动启动")
    public void SetAutoStartDungeon(ArrayList<Long> _autoStartDungeonIdList, int _hour, int _min)
    {
        GC2GS_037_002_ReqSetAutoStartDungeon msg = new GC2GS_037_002_ReqSetAutoStartDungeon(_autoStartDungeonIdList, _hour, _min);
        getOwner().sendGameMsg(msg);
    }	
    
    @Command(comment = "攻击副本怪物")
    public void AttackDungeon(long _id, long _monsterId, long _heroId)
    {
        GC2GS_037_005_ReqAttackDungeon msg = new GC2GS_037_005_ReqAttackDungeon(_id, _monsterId, _heroId);
        getOwner().sendGameMsg(msg);
    }	
    
    @Command(comment = "攻击副本怪物-恢复指定大臣出战次数")
    public void RecoverHeroFight(long _heroId)
    {
        GC2GS_037_006_ReqRecoverHeroFight msg = new GC2GS_037_006_ReqRecoverHeroFight(_heroId);
        getOwner().sendGameMsg(msg);
    }	
}
