package NPUSServer.GuildMsgDispather.p032_GuildOp;

import Common.GuildEnum.EGuildPermissionType;
import GC2GS.p032_GuildOp.GC2GS_032_044_ReqAttackPropertyPoint;
import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_043_AttackpropertyPointInfo;
import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_043_AttackpropertyPointRetInfo;
import NPCommon.ErrMain.GuildCooperateErr;
import NPCommon.ErrMain.GuildErr;
import NPCommon.ErrMain.Result.ResultOne;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.Guild.RefGuildCooperateArea;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Guild.Cooperate.GuildCooperatePointInfo;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.Guild.Member.GuildMemberInfo;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserServer;

/**
 * 攻击属性据点处理器
 */
public class RequestDealer_NP2US_R_032_044_ReqAttackPropertyPoint extends _ATRequestDealer_GuildOp<GC2GS_032_044_ReqAttackPropertyPoint>
{
    public RequestDealer_NP2US_R_032_044_ReqAttackPropertyPoint(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_032_044_ReqAttackPropertyPoint _msg)
    {
        GuildInfo guildInfo = _committer.getGuildInfo();

        //结构附加信息
        GuildOp_043_AttackpropertyPointInfo addInfo = new GuildOp_043_AttackpropertyPointInfo();
        addInfo.readPackage(_committer.getAddInfo());

        // 获取据点信息用于奖励计算
        GuildCooperatePointInfo pointInfo = guildInfo.getCooperateInfo().lookupPoint(_msg.getPos());
        if (pointInfo == null)
        {
            _committer.commitFailRes(GuildCooperateErr.REWARD_POINT_NOT_FOUND.getCode());
            return;
        }

        //获取公会成员
        GuildMemberInfo memberInfo = guildInfo.getMemberMgr().lookup(_committer.getCid());
        if(null == memberInfo)
        {
            _committer.commitFailRes(GuildErr.MEMBER_NOT_FOUND.getCode());
            return ;
        }

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.GUILD_COOPERATE_ATTACK);

        // 攻击属性据点
        ResultOne<Long> attackResult = guildInfo.getCooperateInfo().attack(_msg.getPos(), _msg.getPropertyPointIndex(), _committer.getCid(), addInfo);
        if (!attackResult.isSucc())
        {
            _committer.commitFailRes(attackResult.getCode());
            return;
        }

        long damageHp = attackResult.getData();
        long propertyPointMaxHp = pointInfo.getPropertyPointMaxHp();

        // 获取区域配置和据点血量
        RefGuildCooperateArea areaRef = pointInfo.getAreaRef();

        // 计算奖励倍数
        int rewardTimes = (int) Math.ceil((double) damageHp / propertyPointMaxHp * 10000) / RefGeneral.Ref().guild_cooperate_dispatch_property_point_hp_per;

        // 判断是否达到阈值并计算奖励
        long guildCoinReward;
        int guildDevoteReward;

        if (rewardTimes < 1)
        {
            // 未达到n%，使用最低奖励
            guildCoinReward = RefGeneral.Ref().guild_cooperate_dispatch_guild_coin_reward_min;
            guildDevoteReward = RefGeneral.Ref().guild_cooperate_dispatch_guild_devote_reward_min;
        }
        else
        {
            // 达到n%，按百分比计算
            guildCoinReward = rewardTimes * areaRef.property_point_hp_per_guild_coin_reward;
            guildDevoteReward = rewardTimes * areaRef.property_point_hp_per_guild_devote_reward;
        }

        // 公会协助派遣可获得工会贡献
        if (addInfo.getCanGetDevote())
        {
            memberInfo.gainDevote(guildDevoteReward, context);
        }

        // 收集贡献点
        context.getCollector().addItem(RefGeneral.Ref().guild_cooperate_construction_common_item, damageHp);

        //构造公会币奖励，将贡献点物品列表一并返回
        GuildOp_043_AttackpropertyPointRetInfo retInfo = new GuildOp_043_AttackpropertyPointRetInfo();
        retInfo.setGainGuildCointCount(guildCoinReward);
        retInfo.setDamageHp(damageHp);
        context.getCollector().fillProtoList(retInfo.getItemList());

        _committer.commitSucRes(retInfo);
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return null;
    }

}