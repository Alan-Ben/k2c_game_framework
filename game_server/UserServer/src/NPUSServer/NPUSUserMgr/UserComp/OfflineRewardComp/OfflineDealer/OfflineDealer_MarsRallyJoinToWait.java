package NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineDealer;

import ALLRPC.US.Mars.MarsRallyJoinToWait_Req;
import Common.OfflineRewardEnum.EOfflineRewardEnum;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.MarsExploreTeam;
import NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineRewardInfo;
import NPUSServer.USLog;

/**
 * 火星集结加入到达后切换等待状态的离线处理器
 */
public class OfflineDealer_MarsRallyJoinToWait extends _AOfflineDataDealer
{
    @Override
    public EOfflineRewardEnum getEnum()
    {
        return EOfflineRewardEnum.MARS_RALLY_JOIN_TO_WAIT;
    }

    @Override
    public boolean isValid()
    {
        return true;
    }

    @Override
    public boolean syncToClient()
    {
        return false;
    }

    /**
     * 预处理离线数据，确保玩家离线期间的队伍状态也能正确回放
     */
    @Override
    protected void _preDeal(OfflineRewardInfo _info, NPPlayerContext _context)
    {
        // 离线数据为空时无需处理
        if (_info.getOfflineData() == null) {
            return;
        }

        try {
            // 反序列化RPC请求体，复用线上同一份入参结构
            MarsRallyJoinToWait_Req req = new MarsRallyJoinToWait_Req();
            req.readPackage(_info.getOfflineData());

            // 创建统一业务上下文，便于状态切换过程追踪
            NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.MARS_EXPLORE_TEAM_STATUS_CHG);

            // 定位队伍并执行状态切换
            MarsExploreTeam team = _info.getUserData().getMarsExploreComponent().getTeamMgr().lookup(req.getTeamId());
            if (team == null) {
                USLog.error(_info.getUserData().getUSServer(),
                        "OfflineDealer_MarsRallyJoinToWait._preDeal - team not found: cid={}, teamId={}",
                        _info.getUserData().getCid(), req.getTeamId());
                return;
            }

            Result result = team.joinRallyToWait(context);
            if (!result.isSucc()) {
                USLog.error(_info.getUserData().getUSServer(),
                        "OfflineDealer_MarsRallyJoinToWait._preDeal - joinRallyToWait fail: cid={}, teamId={}, code={}",
                        _info.getUserData().getCid(), req.getTeamId(), result.getCode());
            }
        }
        catch (Exception ex) {
            USLog.error(_info.getUserData().getUSServer(),
                    "OfflineDealer_MarsRallyJoinToWait._preDeal - exception: cid={}",
                    _info.getUserData().getCid(), ex);
        }
    }
}

