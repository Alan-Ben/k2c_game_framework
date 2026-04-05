package NPUSServer.GuildMsgDispather.p037_GuildDungeonOp;

import Common.GuildEnum.EGuildPermissionType;
import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_037_005_AttackDungeonInfo;
import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_037_005_AttackDungeonRet;
import GC2GS.p037_GuildDungeonOp.GC2GS_037_005_ReqAttackDungeon;
import NPCommon.ErrMain.GuildErr;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Guild.GuildDungeon.GuildDungeonAttackResult;
import NPUSServer.Guild.GuildDungeon.GuildDungeonInstanceInfo;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.Guild.Member.GuildMemberInfo;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserServer;

/**
 * 获取联盟副本信息
 */
public class RequestDealer_NP2US_R_037_005_ReqAttackDungeon extends _ATRequestDealer_GuildOp<GC2GS_037_005_ReqAttackDungeon>
{
    public RequestDealer_NP2US_R_037_005_ReqAttackDungeon(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_037_005_ReqAttackDungeon _msg)
    {
        GuildInfo guild = _committer.getGuildInfo();
        if(null == guild)
        {
            _committer.commitFailRes(GuildErr.GUILD_NOT_EXIST.getCode());
            return;
        }

        //检查副本
        GuildDungeonInstanceInfo instance = guild.getDungeonMgr().getInstanceMgr().lookup(_msg.getId());
        if(null == instance)
        {
            _committer.commitFailRes(GuildErr.GUILD_DUNGEON_NOT_START.getCode());
            return;
        }

        //设置权限
        GuildMemberInfo member = guild.getMemberMgr().lookup(_committer.getCid());
        if(null == member)
        {
            _committer.commitFailRes(GuildErr.NOT_MEMBER_OF_GUILD.getCode());
            return;
        }

        //结构附加信息
        GuildOp_037_005_AttackDungeonInfo addInfo = new GuildOp_037_005_AttackDungeonInfo();
        addInfo.readPackage(_committer.getAddInfo());

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.GUILD_DUNGEON_ATTACK_MONSTER);

        GuildDungeonAttackResult result = instance.cmdAttack(_committer.getCid(), _msg.getHeroId(), _msg.getMonsterId(), addInfo.getPower(), context);
        if(!result.result.isSucc())
        {
            _committer.commitFailRes(result.result.getCode());
            return;
        }

        //攻击排行
        guild.getDungeonMgr().getDamageRank().addRank(_committer.getCid(), result.damge);

        //攻击奖励：联盟币
        //min( int(伤害/GuildBloodTideDropDivisor）^GuildBloodTideDropPower+GuildBloodTideDropConstant),GuildBloodTideDropMax )
        float carVar1Power = 1.0f * RefGeneral.Ref().guild_dungeon_tidedrop_power / 10000;
        long calVar1 =  (long) (Math.pow((1.0f * result.damge / RefGeneral.Ref().guild_dungeon_tidedrop_divisor), carVar1Power) + RefGeneral.Ref().guild_dungeon_tidedrop_constant);
        long gainGuildCoin = Math.min(calVar1, RefGeneral.Ref().guild_dungeon_tidedrop_max);

        //完成奖励：联盟经验
        int gainGuildExp = instance.getLvlRef().finish_guild_exp;
        if(result.isDone && gainGuildExp > 0)
        {
            //获得联盟贡献
            guild.gainExp(_committer.getCid(), gainGuildExp, context);
        }

        //攻击奖励：联盟贡献
        int attackContriV = RefGeneral.Ref().guild_dungeon_attack_contri_v;
        if(attackContriV > 0 && addInfo.getFightedCount() <= RefGeneral.Ref().guild_dungeon_attack_contri_limit)
        {
            member.gainDevote(attackContriV, context);
        }

        GuildOp_037_005_AttackDungeonRet retInfo = new GuildOp_037_005_AttackDungeonRet();
        retInfo.setDungeonId(instance.getDungeonId());
        retInfo.setGainGuildExp(gainGuildExp);
        retInfo.setGainGuildCointCount(gainGuildCoin);
        retInfo.setGuildDevoteCount(attackContriV);
        retInfo.setIsKilled(result.isKilled);

        _committer.commitSucRes(retInfo);
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return null;
    }

}
