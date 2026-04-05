package NPUSServer.NPUserMsgDispather.p017_ActivityOp;

import GC2GS.p017_ActivityOp.GC2GS_017_005_ReqActivityRankBaseInfoByKey;
import NPCommon.ErrMain.ActivityErr;
import NPUSServer.CommonActivityMgr.Core.Rank.ActivityRankInfo;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_017_ActivityOp;

public class MsgDealer_GC2GS_017_005_ReqActivityRankBaseInfoByKey extends NPUserMsgDealer<GC2GS_017_005_ReqActivityRankBaseInfoByKey>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_017_005_ReqActivityRankBaseInfoByKey _msg)
    {
        NPUSUserData userData = _committer.getUserData();

        //获取活动对象
        _AActivityBase activity = userData.getUSServer().getCommActivityMgr().lookupActivity(_msg.getInstanceId());
        if (activity == null)
        {
            _committer.commitFailRes(ActivityErr.ACTIVITY_NOT_FOUND.getCode());
            return;
        }

        //获取排行榜信息
        ActivityRankInfo rankInfo = activity.lookupRank(_msg.getRankId());
        if (rankInfo == null)
        {
            _committer.commitFailRes(ActivityErr.ACTIVITY_RANK_NOT_FOUND.getCode());
            return;
        }

        //获取排行榜基础数据
        rankInfo.makeRankBaseByKey(_msg.getKey(), _msg.getIsCross(), (_result, _rankBase) ->
        {
            if (!_result.isSucc())
            {
                _committer.commitFailRes(_result.getCode());
                return;
            }

            _committer.commitSucRes(US2GCWriter_017_ActivityOp.make_005_RetActivityRankBaseInfoByKey(_rankBase));
        });
    }
}
