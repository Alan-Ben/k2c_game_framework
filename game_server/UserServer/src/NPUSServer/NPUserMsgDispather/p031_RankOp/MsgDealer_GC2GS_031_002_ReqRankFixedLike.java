package NPUSServer.NPUserMsgDispather.p031_RankOp;

import Common.RankObj.RankFixed_LikeResult;
import GC2GS.p031_RankOp.GC2GS_031_002_ReqRankFixedLike;
import NPCommon.ErrMain.RankErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CallBack._ICallBackResultT;
import NPEnum.ENPGameEvent;
import NPEnum.ENPItemType;
import NPEnum.ENPPlayerRecordParam;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_RANK_FIXED_LIKE;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_031_RankOp;
import NPUSServer.RankFixedMgr.RankFixedInfo;

public class MsgDealer_GC2GS_031_002_ReqRankFixedLike extends NPUserMsgDealer<GC2GS_031_002_ReqRankFixedLike>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_031_002_ReqRankFixedLike _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (userData == null)
            return;

        RankFixedInfo rankFixedInfo = getUSServer().getRankFixedMgr().lookupRank(_msg.getRankFixedId());
        if (rankFixedInfo == null)
        {
            _commiter.commitFailRes(RankErr.RANK_FIXED_NOT_FOUND.getCode());
            return;
        }

        long fixedCdId = _msg.getIsCross() ? rankFixedInfo.getRef().cross_like_fixed_cd_id : rankFixedInfo.getRef().like_fixed_cd_id;

        //判断该排行榜是否可以点赞
        if (fixedCdId == 0)
        {
            _commiter.commitFailRes(RankErr.RANK_FIXED_CANT_LIKE.getCode());
            return;
        }

        boolean hasCd = userData.hasItem(ENPItemType.FIXED_CD, fixedCdId, 1);
        if (!hasCd)
        {
            _commiter.commitFailRes(RankErr.RANK_FIXED_LIKE_CD_NOT_ENOUGH.getCode());
            return;
        }

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.MINI_GAME_DONE);
        //调用点赞接口
        RankFixedLikeFunc.randomLike(userData, rankFixedInfo, _msg.getIsCross(), context, new _ICallBackResultT<RankFixed_LikeResult>()
        {
            @Override
            public void onRunOver(Result _result, RankFixed_LikeResult _resultObj)
            {
                if (!_result.isSucc())
                {
                    _commiter.commitFailRes(_result.getCode());
                    return;
                }

                _commiter.commitSucRes(US2GCWriter_031_RankOp.make_002_RetRankFixedLike(_resultObj));

                userData.onLogicEvent(new Event_P_RANK_FIXED_LIKE(context, 1));

                //记录次数
                userData.getRecordComponent().addRecord(ENPPlayerRecordParam.RANK_FIXED_LIKE, 1, context);
            }
        });
    }
}
