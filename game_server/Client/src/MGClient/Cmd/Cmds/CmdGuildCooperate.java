package MGClient.Cmd.Cmds;

import Common.GuildCooperateObj.GuildCooperate_RewardPointPos;
import GC2GS.p002_InitOp.GC2GS_002_075_ReqGuildCooperateInit;
import GC2GS.p032_GuildOp.*;
import MGClient.Cmd.Annotation.Command;
import MGClient.Cmd.Annotation.Commander;
import MGClient.Cmd.CmdBase;

@Commander(comment = "联盟协作", name = "guildCooperate")
public class CmdGuildCooperate extends CmdBase
{
    @Command(comment = "初始化联盟协作数据")
    public void info()
    {
        GC2GS_002_075_ReqGuildCooperateInit msg = new GC2GS_002_075_ReqGuildCooperateInit();
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "获取联盟协作攻击日志列表 [lastDbId] [num]")
    public void attackLogList(long lastDbId, int num)
    {
        GC2GS_032_041_ReqGuildCooperateAttackLogList msg = new GC2GS_032_041_ReqGuildCooperateAttackLogList();
        msg.setLastDbId(lastDbId);
        msg.setNum(num);
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "设置推荐奖励点 [areaId] [index]")
    public void setRecommendRewardPoint(long areaId, int index)
    {
        GC2GS_032_042_ReqSetRecommendRewardPoint msg = new GC2GS_032_042_ReqSetRecommendRewardPoint();
        GuildCooperate_RewardPointPos pos = new GuildCooperate_RewardPointPos();
        pos.setAreaId(areaId);
        pos.setIndex(index);
        msg.setPos(pos);
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "领取奖励点奖励 [areaId] [index]")
    public void drawRewardPointReward(long areaId, int index)
    {
        GC2GS_032_043_ReqDrawRewardPointReward msg = new GC2GS_032_043_ReqDrawRewardPointReward();
        GuildCooperate_RewardPointPos pos = new GuildCooperate_RewardPointPos();
        pos.setAreaId(areaId);
        pos.setIndex(index);
        msg.setPos(pos);
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "攻击属性点 [areaId] [index] [propertyPointIndex] [heroId]")
    public void attackPropertyPoint(long areaId, int index, int propertyPointIndex, long heroId)
    {
        GC2GS_032_044_ReqAttackPropertyPoint msg = new GC2GS_032_044_ReqAttackPropertyPoint();
        GuildCooperate_RewardPointPos pos = new GuildCooperate_RewardPointPos();
        pos.setAreaId(areaId);
        pos.setIndex(index);
        msg.setPos(pos);
        msg.setPropertyPointIndex(propertyPointIndex);
        msg.setHeroId(heroId);
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "增加英雄恢复次数 [heroId]")
    public void addHeroRecoveryCount(long heroId)
    {
        GC2GS_032_045_ReqAddHeroRecoveryCount msg = new GC2GS_032_045_ReqAddHeroRecoveryCount();
        msg.setHeroId(heroId);
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "获取联盟协作伤害排行榜")
    public void damageRank()
    {
        GC2GS_032_046_ReqGuildCooperateDamageRank msg = new GC2GS_032_046_ReqGuildCooperateDamageRank();
        getOwner().sendGameMsg(msg);
    }
}
