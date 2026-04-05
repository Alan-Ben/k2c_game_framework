package NPUSServer.NPUserMsgDispather.p041_MarsExploreOp;

import GC2GS.p041_MarsExploreOp.GC2GS_041_021_ReqStartAndFinishRepair;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.MarsExploreTeam;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_041_MarsExploreOp;
import USLOGDB.OptBo.Opt041021MarsExploreTeamAkeyRepairBO;

/**
 * 火星探险-开始并立即完成队伍维修处理器
 *
 * 功能说明：
 * 处理客户端请求，一次性完成维修，同时扣除维修成本和钻石加速成本
 *
 * 处理流程：
 * 1. 验证用户数据
 * 2. 查找对应的队伍
 * 3. 调用业务方法 cmdStartAndFinishRepair
 * 4. 返回处理结果
 */
public class MsgDealer_GC2GS_041_021_ReqStartAndFinishRepair
	extends NPUserMsgDealer<GC2GS_041_021_ReqStartAndFinishRepair>
{
	@Override
	protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter,
                                GC2GS_041_021_ReqStartAndFinishRepair _msg)
	{
		// 获取玩家数据
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

		// 创建操作上下文
		NPPlayerContext context = NPPlayerContext.createNew(
			ENPGameEvent.MARS_EXPLORE_TEAM_START_AND_FINISH_REPAIR);

		// 执行开始并立即完成维修
		Result result = team.startAndFinishRepair(_msg.getRepairNum(), context);
		if(!result.isSucc())
		{
			_commiter.commitFailRes(result.getCode());
			return;
		}

		// 返回成功响应
		_commiter.commitSucRes(
			US2GCWriter_041_MarsExploreOp.make_021_RetStartAndFinishRepair());

        //日志数据
        Opt041021MarsExploreTeamAkeyRepairBO optBo = new Opt041021MarsExploreTeamAkeyRepairBO();
        optBo.setTeamId(getUSServer().getBM(), _msg.getTeamId());
        optBo.setRepairNum(getUSServer().getBM(), _msg.getRepairNum());
        _commiter.getUserData().logEvent(optBo, context);
	}
}
