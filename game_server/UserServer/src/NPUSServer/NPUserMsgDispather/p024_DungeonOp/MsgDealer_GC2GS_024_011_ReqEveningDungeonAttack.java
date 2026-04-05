package NPUSServer.NPUserMsgDispather.p024_DungeonOp;

import GC2GS.p024_DungeonOp.GC2GS_024_011_ReqEveningDungeonAttack;
import NPCommon.ErrMain.Result.ResultOne;
import NPEnum.ENPGameEvent;
import NPEnum.ENPPlayerRecordParam;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Dungeon.Evening.EveningDungeonAttackResult;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_024_DungeonOp;

public class MsgDealer_GC2GS_024_011_ReqEveningDungeonAttack extends NPUserMsgDealer<GC2GS_024_011_ReqEveningDungeonAttack>
        {
            @Override
            protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_024_011_ReqEveningDungeonAttack _msg)
            {
                //获取用户对象，基类有做空判断，这边不做处理
                NPUSUserData userData = _commiter.getUserData();

                NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.EVENING_DUNGEON_ATTACK);

                ResultOne<EveningDungeonAttackResult> attackResult =
                        getUSServer().getEveningDungeonMgr().getInfo().attack(userData, _msg.getHeroId(), context);
                if (!attackResult.isSucc())
                {
                    _commiter.commitFailRes(attackResult.getCode());
                    return;
                }

                // 如果击杀了boss，增加晚间副本boss击杀计数器
                if (attackResult.getData().isKill)
                {
                    userData.getRecordComponent().addRecord(ENPPlayerRecordParam.EVENING_DUNGEON_BOSS_KILL_COUNT, 1, context);
                }

                _commiter.commitSucRes(US2GCWriter_024_DungeonOp.make_011_RetEveningDungeonAttack(attackResult.getData()));
    }
}
