package NPUSServer.NPUserMsgDispather.p037_GuildDungeonOp;

import CommonEnum.ECurrency;
import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_037_005_AttackDungeonInfo;
import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_037_005_AttackDungeonRet;
import GC2GS.p037_GuildDungeonOp.GC2GS_037_005_ReqAttackDungeon;
import NPCommon.ErrMain.HeroErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CallBack._ICallBackIntT;
import NPEnum.ENPGameEvent;
import NPEnum.ENPItemType;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter._ATGuildUserMsgRedirectCommiter;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.GuildDungeonComp.GuildDungeonHeroFightInfo;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_037_GuildDungeonOp;
import USLOGDB.OptBo.Opt037005GuildDungeonAttackBO;

public class MsgDealer_GC2GS_037_005_ReqAttackDungeon extends NPUserMsgDealer<GC2GS_037_005_ReqAttackDungeon>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_037_005_ReqAttackDungeon _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        //检查大臣
        HeroInfo hero = userData.getHeroComponent().lookupHero(_msg.getHeroId());
        if(null == hero)
        {
        	_commiter.commitFailRes(HeroErr.HERO_NOT_FOUND.getCode());
        	return;
        }
        
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.GUILD_DUNGEON_ATTACK_MONSTER);

        //出战大臣
        Result fightResult = userData.getGuildDungeonComponent().heroAttack(hero, context);
        if(!fightResult.isSucc())
        {
        	_commiter.commitFailRes(fightResult.getCode());
        	return;
        }

        //发送消息请求对战
        GuildOp_037_005_AttackDungeonInfo addInfo = new GuildOp_037_005_AttackDungeonInfo();
        addInfo.setHeroId(_msg.getHeroId());
        addInfo.setPower(hero.getPower());

        GuildDungeonHeroFightInfo fightInfo = userData.getGuildDungeonComponent().lookupHeroFight(_msg.getHeroId());
        addInfo.setFightedCount(null == fightInfo ? 0 : fightInfo.getFightedCount());

        //发送请求进行战斗处理
        //转化为统一跨服消息进行处理
        getUSServer().dealGuildMsgByRedirectCommiter(
                new _ATGuildUserMsgRedirectCommiter<GuildOp_037_005_AttackDungeonRet>(_commiter) {
                    @Override
                    protected GuildOp_037_005_AttackDungeonRet _createNewTmpObj() {
                        return new GuildOp_037_005_AttackDungeonRet();
                    }

                    @Override
                    protected void _dealTmpCommitMsg(_ANPUSUserBasicMsgItem _commiter, GuildOp_037_005_AttackDungeonRet _retMsg) {
                        //给予玩家公会币
                        if (_retMsg.getGainGuildCointCount() > 0) {
                            userData.gainItem(ENPItemType.CURRENCY, ECurrency.GUILD_COIN.ordinal(), _retMsg.getGainGuildCointCount(), context);
                        }

                        //增加公会经验
                        context.getCollector().addItem(RefGeneral.Ref().guild_exp_common_item, _retMsg.getGainGuildExp());
                        //增加公会贡献返回
                        context.getCollector().addItem(RefGeneral.Ref().personal_contribution_common_item, _retMsg.getGuildDevoteCount());

                        //返回结果
                        _commiter.commitSucRes(US2GCWriter_037_GuildDungeonOp.make_005_RetAttackDungeon(_retMsg.getIsKilled(), context));

                        //日志数据
                        Opt037005GuildDungeonAttackBO optBo = new Opt037005GuildDungeonAttackBO();
                        optBo.setGuildId(getUSServer().getBM(), userData.getGuildComponent().getGuildId());
                        optBo.setDungeonId(getUSServer().getBM(), _retMsg.getDungeonId());
                        optBo.setMonsterId(getUSServer().getBM(), _msg.getMonsterId());
                        optBo.setHeroId(getUSServer().getBM(), _msg.getHeroId());
                        optBo.setHeroPower(getUSServer().getBM(), hero.getPower());
                        _commiter.getUserData().logEvent(optBo, context);
                    }
                },
                _commiter.getUserData().getCid(),
                _commiter.getUserData().getGuildComponent().getGuildId(),
                _msg,
                addInfo,
                new _ICallBackIntT<_ANPUSUserBasicMsgItem>() {
                    @Override
                    public void onRunOver(int _errCode, _ANPUSUserBasicMsgItem _commiter) {
                        //攻击失败，返回次数
                        NPPlayerContext returnContext = NPPlayerContext.createNew(ENPGameEvent.GUILD_DUNGEON_ATTACK_FAIL);
                        returnContext.setGuid(context.getGuid());
                        userData.getGuildDungeonComponent().heroAttackReturn(hero, returnContext);

                        _commiter.commitFailRes(_errCode);
                    }
                }
        );
    }
}
