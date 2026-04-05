package NPUSServer.NPUserMsgDispather.p041_MarsExploreOp;

import Common.MarsEnum.EMarsExploreTeamState;
import Common.MarsObj.MarsTeamState_March;
import Common.ServerObj.ServerObj_MarsTeam_OccupyMinePlayer;
import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_041_018_RetOccupyMineInfo;
import GC2GS.p041_MarsExploreOp.GC2GS_041_018_ReqGuildMateForwardCollectMine;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.MarsErr;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.Mars.RefMarsExploreCollectBonus;
import NPGameRes.Refs.Mars.RefMarsExploreMine;
import NPGameRes.Refs.Mars.RefMarsExplorePos;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter._ATGuildUserMsgRedirectCommiter;
import NPUSServer.NPUSUserMgr.GameSystem.MarsMineSystem.MarsMineSystem;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MarsComp.MarsCalculate;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.ExploreTeamState.ExploreTeamState_MARCH;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.MarsExploreTeam;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_041_MarsExploreOp;
import USLOGDB.OptBo.Opt041010MarsExploreForwardMineBO;

public class MsgDealer_GC2GS_041_018_ReqGuildMateForwardCollectMine extends NPUserMsgDealer<GC2GS_041_018_ReqGuildMateForwardCollectMine>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_041_018_ReqGuildMateForwardCollectMine _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

		//检查队伍
		MarsExploreTeam team = userData.getMarsExploreComponent().getTeamMgr().lookup(_msg.getTeamId());
		if(null == team)
		{
			_commiter.commitFailRes(MarsErr.MARS_EXPLORE_TEAM_NOT_FOUND.getCode());
			return;
		}

		//此时转发消息开启
		//转化为统一跨服消息进行处理
		getUSServer().dealGuildMsgByRedirectCommiter(
				new _ATGuildUserMsgRedirectCommiter<GuildOp_041_018_RetOccupyMineInfo>(_commiter) {
					@Override
					protected GuildOp_041_018_RetOccupyMineInfo _createNewTmpObj() {
						return new GuildOp_041_018_RetOccupyMineInfo();
					}

					@Override
					protected void _dealTmpCommitMsg(_ANPUSUserBasicMsgItem _commiter, GuildOp_041_018_RetOccupyMineInfo _retMsg)
					{
						long mineInstanceId = _retMsg.getMineInfo().getId();

						RefMarsExplorePos posRef = RefMarsExplorePos.getMgr().get(_retMsg.getPosId());
						if(null == posRef)
						{
							_commiter.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
							return;
						}

						//计算采集速度
						RefMarsExploreMine mineRef = RefMarsExploreMine.getMgr().get(_retMsg.getMineInfo().getRefId());
						if(null == mineRef)
						{
							_commiter.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
							return;
						}

						long collectPer = RefMarsExploreCollectBonus.getMgr().getAddPer(team.getTeamPower());
						long collectSpeed = mineRef.mars_mine_collects_speed * (10000 + collectPer) / 10000;
						if(collectSpeed <= 0)
						{
							_commiter.commitFailRes(MarsErr.MARS_MINE_COLLECT_SPEED_ERROR.getCode());
							return;
						}

						if(_retMsg.getMineInfo().getCid() == userData.getCid())
						{
							_commiter.commitFailRes(MarsErr.MARS_MINE_ALREADY_OCCUPY.getCode());
							return ;
						}

						//判断是否有其他玩家占有，如果是非pvp且是其他玩家占有，则报错
						if(!_msg.getIsPvpOp() && _retMsg.getMineInfo().getTeamId() > 0)
						{
							_commiter.commitFailRes(MarsErr.MARS_MINE_OTHER_PLAYER_OCCUPY.getCode());
							return ;
						}

						long startTimeMS = CommonFunc.getNowTimeMS();
						long keepTimeMS = MarsCalculate.calMarchTimeMS(userData, posRef);
						//增加采矿队列数据
						ServerObj_MarsTeam_OccupyMinePlayer player = team.toServerOccupyPlayer();
						//计算开始采集的时间
						long startCollectMs = startTimeMS + keepTimeMS;

						//发起添加进军的协议
						MarsMineSystem.GoToOccupyMarsMine(getUSServer(), mineInstanceId,
								_msg.getIsOtherTeamForward(), player, startCollectMs, collectSpeed, (_occupyErr) ->
								{
									if(_occupyErr > 0)
									{
										_commiter.commitFailRes(_occupyErr);
										return;
									}

									NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.MARS_EXPLORE_FORWARD_COLLECT_MINE);

									//用于队伍的额外数据
									MarsTeamState_March extData = new MarsTeamState_March();
									extData.setInstanceId(mineInstanceId);
									extData.setTargetState(EMarsExploreTeamState.COLLECT.ordinal());
									//目标状态
									ExploreTeamState_MARCH targetState = new ExploreTeamState_MARCH(team, startTimeMS, keepTimeMS, posRef.id, extData);
									//尝试切换状态
									if(!team.getStateMachine().transState(targetState))
									{
										_commiter.commitFailRes(MarsErr.MARS_EXPLORE_TEAM_STATE_ERROR.getCode());
										return;
									}

									//先返回操作成功，再推送队伍变更状态
									_commiter.commitSucRes(
											US2GCWriter_041_MarsExploreOp.make_018_RetGuildMateForwardCollectMine());

									//日志数据
									Opt041010MarsExploreForwardMineBO optBo = new Opt041010MarsExploreForwardMineBO();
									optBo.setTeamId(userData.getUSServer().getBM(), _msg.getTeamId());
									optBo.setPos(userData.getUSServer().getBM(), posRef.id);
									optBo.setMineInstanceId(userData.getUSServer().getBM(), mineInstanceId);
									optBo.setMineRefId(userData.getUSServer().getBM(), _retMsg.getMineInfo().getRefId());
									optBo.setStartCollectMs(userData.getUSServer().getBM(), startCollectMs);
									optBo.setCollectSpeed(userData.getUSServer().getBM(), collectSpeed);
									_commiter.getUserData().logEvent(optBo, context);
								});
					}
				},
				_commiter.getUserData().getCid(),
				_commiter.getUserData().getGuildComponent().getGuildId(),
				_msg,
				null
		);
    }
}
