package NPUSServer.NPUserMsgDispather.p031_RankOp;

import ALBasicProtocolPack._IALProtocolStructure;
import GC2GS.p031_RankOp.GC2GS_031_003_ReqRankFixedLikeScore;
import NPCommon.ErrMain.RankErr;
import NPCommon.Util.CommonFunc;
import NPEnum.ERankType;
import NPGameRes.Refs.Rank.RefRank;
import NPGameRes.Refs.Rank.RefRankFixed;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_031_RankOp;
import NPUSServer.RankPlayerDataMgr.RankFixedObjDataInfo;
import NPUSServer.RankPlayerDataMgr.RankFixedObjDataList;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCS2US_R.p003_CommOp.NP2US_R_003_005_ReqRankFixedLikeScore;
import WCGCS2US_RB.p003_CommOp.NP2US_RB_003_005_RetRankFixedLikeScore;
import WCGCommon.Enum.NPEnum;

public class MsgDealer_GC2GS_031_003_ReqRankFixedLikeScore extends NPUserMsgDealer<GC2GS_031_003_ReqRankFixedLikeScore>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_031_003_ReqRankFixedLikeScore _msg)
    {
        RefRankFixed refRankFixed = RefRankFixed.getMgr().get(_msg.getRankFixedId());
        if (refRankFixed == null)
        {
            _commiter.commitFailRes(RankErr.RANK_FIXED_NOT_FOUND.getCode());
            return;
        }

        RefRank refRank = RefRank.getMgr().get(refRankFixed.rank_id);
        if (refRank == null)
        {
            _commiter.commitFailRes(RankErr.RANK_FIXED_NOT_FOUND.getCode());
            return;
        }

        int targetUsId = -1;
        if (refRank.rank_type == ERankType.PLAYER)
        {
            targetUsId = CommonFunc.parseServerTypeIdFromCid(_msg.getKey());
        } else if (refRank.rank_type == ERankType.GUILD)
        {
            targetUsId = CommonFunc.parseServerTypeIdFromInstanced(_msg.getKey());
        }

        if (targetUsId == -1)
        {
            _commiter.commitFailRes(RankErr.RANK_FIXED_LIKE_LIST_EMPTY.getCode());
            return;
        }

        //区分本服和跨服
        if (targetUsId == getUSServer().getServerTypeId())
        {
            RankFixedObjDataList rankDataList = getUSServer().getRankFixedObjDataListMgr().lookupObj(_msg.getRankFixedId());
            if (rankDataList == null)
            {
                _commiter.commitSucRes(US2GCWriter_031_RankOp.make_003_RetRankFixedLikeScore(0));
                return;
            }

            RankFixedObjDataInfo dataInfo = rankDataList.lookupPlayer(_msg.getKey());
            if (dataInfo == null)
            {
                _commiter.commitSucRes(US2GCWriter_031_RankOp.make_003_RetRankFixedLikeScore(0));
                return;
            }

            _commiter.commitSucRes(US2GCWriter_031_RankOp.make_003_RetRankFixedLikeScore(dataInfo.getLikeScore(_msg.getIsCross())));
        } else
        {
            //跨服玩家分数查询
            getUSServer().sendRequestToBSServer(NPEnum.EServerType.USER.ordinal(), targetUsId,
                    new NP2US_R_003_005_ReqRankFixedLikeScore(_msg.getRankFixedId(), _msg.getKey(), _msg.getIsCross()), new _IWCGCallbackDealer()
                    {
                        @Override
                        public _IALProtocolStructure createProtocolObj()
                        {
                            return new NP2US_RB_003_005_RetRankFixedLikeScore();
                        }

                        @Override
                        public void dealSuc(_IALProtocolStructure _msg)
                        {
                            NP2US_RB_003_005_RetRankFixedLikeScore retMsg = (NP2US_RB_003_005_RetRankFixedLikeScore) _msg;
                            _commiter.commitSucRes(US2GCWriter_031_RankOp.make_003_RetRankFixedLikeScore(retMsg.getLikeScore()));
                        }

                        @Override
                        public void dealFail(int _errCode)
                        {
                            _commiter.commitFailRes(_errCode);
                        }
                    });
        }
    }
}
