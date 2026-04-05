package NPUSServer.NPUserMsgDispather.p032_GuildOp;

import CommonEnum.ECurrency;
import GC2GS.p032_GuildOp.GC2GS_032_044_ReqAttackPropertyPoint;
import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_043_AttackpropertyPointInfo;
import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_043_AttackpropertyPointRetInfo;
import NPCommon.ErrMain.HeroErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CallBack._ICallBackIntT;
import NPEnum.ENPGameEvent;
import NPEnum.ENPItemType;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter._ATGuildUserMsgRedirectCommiter;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_032_GuildOp;

/**
 * 攻击属性据点处理器
 */
public class MsgDealer_GC2GS_032_044_ReqAttackPropertyPoint extends NPUserMsgDealer<GC2GS_032_044_ReqAttackPropertyPoint>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_032_044_ReqAttackPropertyPoint _msg)
    {
        NPUSUserData userData = _committer.getUserData();

        HeroInfo heroInfo = userData.getHeroComponent().lookupHero(_msg.getHeroId());
        if (heroInfo == null)
        {
            _committer.commitFailRes(HeroErr.HERO_NOT_FOUND.getCode());
            return;
        }

        String playerName = userData.getPlayerComponent().getName();

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.GUILD_COOPERATE_ATTACK);

        //记录大臣使用情况，避免重复发送消息导致重复记录
        Result result = userData.getGuildCooperateComponent().recordHeroUse(_msg.getHeroId(), context);
        if (!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());
            return;
        }

        boolean canGetDevote = false;
        // 公会协助派遣可获得工会贡献
        if (userData.spendItem(ENPItemType.FIXED_CD, RefGeneral.Ref().guild_cooperate_dispatch_can_gain_guild_devote_time_fix_cd_id, 1, context))
        {
            canGetDevote = true;
        }

        //构造发送的附加信息
        GuildOp_043_AttackpropertyPointInfo addInfo = new GuildOp_043_AttackpropertyPointInfo();
        addInfo.setPlayerName(userData.getPlayerComponent().getName());
        addInfo.setHeroId(_msg.getHeroId());
        addInfo.setPower(heroInfo.getPower());
        addInfo.setCanGetDevote(canGetDevote);

        //转化为统一跨服消息进行处理
        getUSServer().dealGuildMsgByRedirectCommiter(
                new _ATGuildUserMsgRedirectCommiter<GuildOp_043_AttackpropertyPointRetInfo>(_committer) {
                    @Override
                    protected GuildOp_043_AttackpropertyPointRetInfo _createNewTmpObj() {
                        return new GuildOp_043_AttackpropertyPointRetInfo();
                    }

                    @Override
                    protected void _dealTmpCommitMsg(_ANPUSUserBasicMsgItem _commiter, GuildOp_043_AttackpropertyPointRetInfo _msg) {

                        // 根据伤害获取公会币
                        if (userData.spendItem(ENPItemType.FIXED_CD, RefGeneral.Ref().guild_cooperate_dispatch_can_gain_guild_coin_time_fix_cd_id, 1, context)) {
                            userData.gainItem(ENPItemType.CURRENCY, ECurrency.GUILD_COIN.ordinal(), _msg.getGainGuildCointCount(), context);
                        }

                        // 构造回包，合并公会币奖励和据点贡献点奖励
                        GS2GC.p032_GuildOp.GS2GC_032_044_RetAttackPropertyPoint retProto = US2GCWriter_032_GuildOp.make_044_RetAttackPropertyPoint(_msg.getDamageHp(), context.getCollector());
                        retProto.getItemList().addAll(_msg.getItemList());
                        _committer.commitSucRes(retProto);
                    }
                },
                _committer.getUserData().getCid(),
                _committer.getUserData().getGuildComponent().getGuildId(),
                _msg,
                addInfo,
                new _ICallBackIntT<_ANPUSUserBasicMsgItem>() {
                    @Override
                    public void onRunOver(int _errCode, _ANPUSUserBasicMsgItem _commiter) {
                        // 回滚大臣使用记录
                        userData.getGuildCooperateComponent().rollbackHeroUse(_msg.getHeroId(), context);
                        //如果扣除了贡献次数需要加回来
                        if(addInfo.getCanGetDevote())
                        {
                            userData.gainItem(ENPItemType.FIXED_CD, RefGeneral.Ref().guild_cooperate_dispatch_can_gain_guild_devote_time_fix_cd_id, 1, context);
                        }

                        //返回失败
                        _commiter.commitFailRes(_errCode);
                    }
                }
        );
    }
}