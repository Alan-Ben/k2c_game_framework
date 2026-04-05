package NPUSServer.NPGeneralListener.RequestDispather.p009_CrossRankOp;

import GS2GC.p017_ActivityOp.GS2GC_017_053_OnActivityRankScoreChg;
import NP2US_R.p009_CrossRankOp.ToUS_R_009_001_PushCrossRankScoreChg;
import NP2US_RB.p009_CrossRankOp.ToUS_RB_009_001_PushCrossRankScoreChg;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPGeneralListener.RequestDispather._ABasicGeneralRequestDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

/**
 * 跨服排行榜分数变更推送处理器
 *
 * 主要功能：
 * 1. 接收来自CrossRankServer的排名变动推送
 * 2. 查找对应的在线玩家
 * 3. 转发GS2GC_017_053协议给客户端
 *
 * 执行流程：
 * 1. 根据crossInstanceId查找活动实例
 * 2. 根据cid查找玩家数据
 * 3. 检查玩家是否在线
 * 4. 构造并发送GS2GC协议给客户端
 * 5. 返回空响应给CRS
 */
public class RequestDealer_ToUS_R_009_001_PushCrossRankScoreChg
    extends _ABasicGeneralRequestDealer<ToUS_R_009_001_PushCrossRankScoreChg>
{
    public RequestDealer_ToUS_R_009_001_PushCrossRankScoreChg(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _committer, ToUS_R_009_001_PushCrossRankScoreChg _msg)
    {
        // 根据跨服实例ID查找活动
        _AActivityBase activityBase = getUSServer().getCommActivityMgr()
            .lookupActivityByCrossInstanceId(_msg.getCrossInstanceId());

        if (activityBase == null) {
            USLog.error(getUSServer(),
                "RequestDealer_NP2US_R_009_001_PushCrossRankScoreChg._dealMessage - activity not found: crossInstanceId={}, cid={}, rankId={}",
                _msg.getCrossInstanceId(), _msg.getCid(), _msg.getRankId());
            _committer.commitSucRes(new ToUS_RB_009_001_PushCrossRankScoreChg());
            return;
        }

        // 获取玩家数据
        NPUSUserData userData = getUSServer().getUsUserMgr().lookupCacheUserData(_msg.getCid());

        // 检查玩家是否在线
        if (userData == null) {
            // 玩家不在线，静默忽略（这是正常情况）
            _committer.commitSucRes(new ToUS_RB_009_001_PushCrossRankScoreChg());
            return;
        }

        // 构造并发送协议给客户端
        userData.sendMsgToGC(new GS2GC_017_053_OnActivityRankScoreChg(
            activityBase.getInstanceId(),  // 活动实例ID（本服实例ID）
            _msg.getRankId(),               // 排行榜ID
            _msg.getOriRank(),              // 原排名
            _msg.getCurRank(),              // 当前排名
            _msg.getScore()                 // 当前分数
        ));

        // 返回成功响应给CRS
        _committer.commitSucRes(new ToUS_RB_009_001_PushCrossRankScoreChg());
    }
}