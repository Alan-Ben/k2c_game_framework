package NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineDealer;

import ALLRPC.US.Mars.MarsRallyJoinFailBack_Req;
import Common.OfflineRewardEnum.EOfflineRewardEnum;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.MarsExploreTeam;
import NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineRewardInfo;
import NPUSServer.USLog;

/**
 * 火星集结加入失败后遣返状态的离线处理器
 */
public class OfflineDealer_MarsRallyJoinFailBack extends _AOfflineDataDealer
{
    @Override
    public EOfflineRewardEnum getEnum()
    {
        return EOfflineRewardEnum.MARS_RALLY_JOIN_FAIL_BACK;
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
     * 预处理离线数据，保证离线期间也会执行集结加入失败遣返
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
            MarsRallyJoinFailBack_Req req = new MarsRallyJoinFailBack_Req();
            req.readPackage(_info.getOfflineData());

            // 创建统一业务上下文，便于状态切换过程追踪
            NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.MARS_EXPLORE_TEAM_STATUS_CHG);

            // 定位队伍并执行失败遣返
            MarsExploreTeam team = _info.getUserData().getMarsExploreComponent().getTeamMgr().lookup(req.getTeamId());
            if (team == null) {
                USLog.error(_info.getUserData().getUSServer(),
                        "OfflineDealer_MarsRallyJoinFailBack._preDeal - team not found: cid={}, teamId={}",
                        _info.getUserData().getCid(), req.getTeamId());
                return;
            }

            Result result = team.joinRallyFailBack(context);
            if (!result.isSucc()) {
                USLog.error(_info.getUserData().getUSServer(),
                        "OfflineDealer_MarsRallyJoinFailBack._preDeal - joinRallyFailBack fail: cid={}, teamId={}, code={}",
                        _info.getUserData().getCid(), req.getTeamId(), result.getCode());
            }
        }
        catch (Exception ex) {
            USLog.error(_info.getUserData().getUSServer(),
                    "OfflineDealer_MarsRallyJoinFailBack._preDeal - exception: cid={}",
                    _info.getUserData().getCid(), ex);
        }
    }
}

