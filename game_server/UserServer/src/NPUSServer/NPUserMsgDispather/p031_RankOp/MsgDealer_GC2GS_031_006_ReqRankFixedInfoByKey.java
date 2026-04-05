package NPUSServer.NPUserMsgDispather.p031_RankOp;

import GC2GS.p031_RankOp.GC2GS_031_006_ReqRankFixedInfoByKey;
import NPCommon.ErrMain.RankErr;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_031_RankOp;
import NPUSServer.RankFixedMgr.RankFixedInfo;

public class MsgDealer_GC2GS_031_006_ReqRankFixedInfoByKey extends NPUserMsgDealer<GC2GS_031_006_ReqRankFixedInfoByKey>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_031_006_ReqRankFixedInfoByKey _msg)
    {
        RankFixedInfo rankFixedInfo = getUSServer().getRankFixedMgr().lookupRank(_msg.getRankFixedId());
        if (rankFixedInfo == null)
        {
            _commiter.commitFailRes(RankErr.RANK_FIXED_NOT_FOUND.getCode());
            return;
        }

        rankFixedInfo.makeRankBaseByKey(_msg.getKey(), _msg.getIsCross(), (_result, _rankBase) ->
        {
            if (!_result.isSucc())
            {
                _commiter.commitFailRes(_result.getCode());
                return;
            }

            _commiter.commitSucRes(US2GCWriter_031_RankOp.make_006_RetRankFixedInfoByKey(_rankBase));
        });
    }
}
