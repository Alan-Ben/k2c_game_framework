package NPUSServer.NPUserMsgDispather.p031_RankOp;

import Common.RankObj.RankFixed_LikeResult;
import GC2GS.p031_RankOp.GC2GS_031_004_ReqRankFixedAKeyLike;
import NPCommon.ErrMain.RankErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Promise.Promise;
import NPCommon.Util.CallBack._ICallBackResultT;
import NPEnum.ENPGameEvent;
import NPEnum.ENPPlayerRecordParam;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_RANK_FIXED_LIKE;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_031_RankOp;
import NPUSServer.RankFixedMgr.RankFixedInfo;
import NPUSServer.USLog;

import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.CopyOnWriteArrayList;

public class MsgDealer_GC2GS_031_004_ReqRankFixedAKeyLike extends NPUserMsgDealer<GC2GS_031_004_ReqRankFixedAKeyLike>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_031_004_ReqRankFixedAKeyLike _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (userData == null)
            return;

        //检查是否解锁
        if(!NPPlayerConditionDealerMgr.IsEnable(RefGeneral.Ref().rank_fixed_akey_simple_unlock_id, userData, null))
        {
            _commiter.commitFailRes(RankErr.RANK_FIXED_A_KEY_LIKE_NOT_UNLOCK.getCode());
            return;
        }

        List<RankFixedInfo> canLikeRankList = new ArrayList<>();
        //去掉点赞次数不够的排行榜
        for (int i = _msg.getRankFixedIdList().size() - 1; i >= 0; i--)
        {
            RankFixedInfo rankFixedInfo = getUSServer().getRankFixedMgr().lookupRank(_msg.getRankFixedIdList().get(i));
            //判断排行榜是否存在
            if (rankFixedInfo == null)
                continue;

            long fixedCdId = _msg.getIsCross() ? rankFixedInfo.getRef().cross_like_fixed_cd_id : rankFixedInfo.getRef().like_fixed_cd_id;

            //判断该排行榜是否可以点赞
            if (fixedCdId == 0)
                continue;

            //检查玩家是否还有点赞次数
            int canLikeTimes = (int) userData.getFixedCdComponent().getItemCount(fixedCdId);
            if (canLikeTimes <= 0)
                continue;

            for (int j = 0; j < canLikeTimes; j++)
            {
                canLikeRankList.add(rankFixedInfo);
            }
        }

        //判断是否有可以点赞的排行榜
        if (canLikeRankList.isEmpty())
        {
            _commiter.commitFailRes(RankErr.RANK_FIXED_LIKE_CD_NOT_ENOUGH.getCode());
            return;
        }

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.RANK_FIXED_A_KEY_LIKE);

        //点赞结果列表, 因为有并发, 所以使用线程安全的List
        List<RankFixed_LikeResult> likeResultList = new CopyOnWriteArrayList<>();

        Promise promise = new Promise();
        for (int i = 0; i < canLikeRankList.size(); i++)
        {
            RankFixedInfo rankFixedInfo = canLikeRankList.get(i);

            final int finalIndex = i;
            promise.then(finalIndex, p ->
            {
                //调用点赞接口
                RankFixedLikeFunc.randomLike(userData, rankFixedInfo, _msg.getIsCross(), context, new _ICallBackResultT<RankFixed_LikeResult>()
                {
                    @Override
                    public void onRunOver(Result _result, RankFixed_LikeResult _resultObj)
                    {
                        if (!_result.isSucc())
                        {
                            USLog.error(getUSServer(), "MsgDealer_GC2GS_031_004_ReqRankFixedAKeyLike _dealMessage randomLike fail, cid:{} rankFixedId:{} code:{}",
                                    userData.getCid(), rankFixedInfo.getRefId(), _result.getCode());
                            promise.commit(finalIndex);
                            return;
                        }

                        likeResultList.add(_resultObj);
                        promise.commit(finalIndex);
                    }
                });
            });
        }

        promise.over(p ->
        {
            //判断是否有可以点赞的排行榜
            if (likeResultList.isEmpty())
            {
                _commiter.commitFailRes(RankErr.RANK_FIXED_NO_TARGET_CAN_LIKE.getCode());
                return;
            }

            _commiter.commitSucRes(US2GCWriter_031_RankOp.make_004_RetRankFixedAKeyLike(likeResultList));

            for (int i = 0; i < likeResultList.size(); i++)
            {
                userData.onLogicEvent(new Event_P_RANK_FIXED_LIKE(context, 1));
            }

            //记录次数
            userData.getRecordComponent().addRecord(ENPPlayerRecordParam.RANK_FIXED_LIKE, likeResultList.size(), context);
        });
    }
}
