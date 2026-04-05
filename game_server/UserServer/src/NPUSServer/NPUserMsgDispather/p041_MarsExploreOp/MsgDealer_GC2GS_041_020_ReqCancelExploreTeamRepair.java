package NPUSServer.NPUserMsgDispather.p041_MarsExploreOp;

import GC2GS.p041_MarsExploreOp.GC2GS_041_020_ReqCancelExploreTeamRepair;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.MarsExploreTeam;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_041_MarsExploreOp;
import USLOGDB.OptBo.Opt041020MarsExploreTeamCancelRepairBO;

/**
 * 取消队伍维修协议处理器
 *
 * 功能：允许玩家主动取消正在进行的队伍维修
 *
 * 业务规则：
 * - 返还已消耗的资源
 * - 不返还已消耗的加速道具
 * - 损耗士兵数量保持不变
 */
public class MsgDealer_GC2GS_041_020_ReqCancelExploreTeamRepair
	extends NPUserMsgDealer<GC2GS_041_020_ReqCancelExploreTeamRepair>
{
	@Override
	protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter,
							   GC2GS_041_020_ReqCancelExploreTeamRepair _msg)
	{
		NPUSUserData userData = _commiter.getUserData();
		if (null == userData)
			return;

		// 检查队伍
		MarsExploreTeam team = userData.getMarsExploreComponent()
			.getTeamMgr().lookup(_msg.getTeamId());
		if(null == team)
		{
			_commiter.commitFailRes(MarsErr.MARS_EXPLORE_TEAM_NOT_FOUND.getCode());
			return;
		}

		// 取消维修
		NPPlayerContext context = NPPlayerContext.createNew(
			ENPGameEvent.MARS_EXPLORE_TEAM_CANCEL_REPAIR);
		Result result = team.cancelRepair(context);
		if(!result.isSucc())
		{
			_commiter.commitFailRes(result.getCode());
			return;
		}

		_commiter.commitSucRes(
			US2GCWriter_041_MarsExploreOp.make_020_RetCancelExploreTeamRepair());

        //日志数据
        Opt041020MarsExploreTeamCancelRepairBO optBo = new Opt041020MarsExploreTeamCancelRepairBO();
        optBo.setTeamId(getUSServer().getBM(), _msg.getTeamId());
        _commiter.getUserData().logEvent(optBo, context);
	}
}
