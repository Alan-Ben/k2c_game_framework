package NPUSServer.NPUserMsgDispather.p023_ArenaOp;

import GC2GS.p023_ArenaOp.GC2GS_023_014_ReqSysSelectAttackSelectHero;
import NPCommon.ErrMain.ArenaErr;
import NPCommon.ErrMain.HeroErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Promise.SerialPromise;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ArenaComp.EArenaAttackType;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_023_ArenaOp;

public class MsgDealer_GC2GS_023_014_ReqSysSelectAttackSelectHero extends NPUserMsgDealer<GC2GS_023_014_ReqSysSelectAttackSelectHero>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_023_014_ReqSysSelectAttackSelectHero _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        //检查是否解锁竞技场
        if (!userData.getArenaComponent().hadUnlockArena())
        {
            _commiter.commitFailRes(ArenaErr.HERO_NUM_NOT_ENOUGH_TO_ENTER_ARENA.getCode());
            return;
        }

        //查找英雄
        HeroInfo heroInfo = userData.getHeroComponent().lookupHero(_msg.getHeroId());
        if (heroInfo == null)
        {
            _commiter.commitFailRes(HeroErr.HERO_NOT_FOUND.getCode());
            return;
        }

        SerialPromise promise = new SerialPromise(userData.getUserLock());

        //先选择对手
        promise.then(p->{
            userData.getArenaComponent().selectUsOpponent(EArenaAttackType.CELEBRITY_RANK, _result ->
            {
                if (!_result.isSucc())
                {
                    _commiter.commitFailRes(_result.getCode());
                    promise.breakOut();
                    return;
                }

                promise.commit();
            });
        });

        //再选择英雄
        promise.then(p->{
            NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.ARENA_SELECT_ATTACK);
            Result result = userData.getArenaComponent().selectBattleHero(heroInfo, "", false, _msg.getItemId(), _msg.getBuffId(), context);
            if (!result.isSucc())
            {
                _commiter.commitFailRes(result.getCode());
                return;
            }

            _commiter.commitSucRes(US2GCWriter_023_ArenaOp.make_003_RetSelectAttackSelectHero());
        });
    }
}
