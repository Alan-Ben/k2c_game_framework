package NPUSServer.NPUserMsgDispather.p023_ArenaOp;

import GC2GS.p023_ArenaOp.GC2GS_023_002_ReqRandomAttackSelectHero;
import NPCommon.ErrMain.HeroErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_023_ArenaOp;

public class MsgDealer_GC2GS_023_002_ReqRandomAttackSelectHero extends NPUserMsgDealer<GC2GS_023_002_ReqRandomAttackSelectHero>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_023_002_ReqRandomAttackSelectHero _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        //查找英雄
        HeroInfo heroInfo = userData.getHeroComponent().lookupHero(_msg.getHeroId());
        if (heroInfo == null)
        {
            _commiter.commitFailRes(HeroErr.HERO_NOT_FOUND.getCode());
            return;
        }

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.ARENA_SELECT_ATTACK);
        Result result = userData.getArenaComponent().selectBattleHero(heroInfo,_msg.getOpponentBotName(), true, 0, _msg.getBuffId(), context);
        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }

        _commiter.commitSucRes(US2GCWriter_023_ArenaOp.make_002_RetRandomAttackSelectHero());
    }
}
