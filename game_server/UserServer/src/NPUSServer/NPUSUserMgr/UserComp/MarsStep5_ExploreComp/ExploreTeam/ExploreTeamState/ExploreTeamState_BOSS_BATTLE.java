package NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.ExploreTeamState;

import Common.MarsEnum.EMarsExploreTeamState;
import Common.MarsObj.MarsTeamState_BossBattle;
import Common.MarsObj.Mars_BossBattleEventLog;
import Common.MarsObj.Mars_ExploreBattleNPCInfo;
import Common.MarsObj.Mars_ExploreBattlePlayerInfo;
import NPCommon.DB.BM.BM;
import NPCommon.NPLogDB.CommLogDB;
import NPEnum.ENPGameEvent;
import NPEnum.ENPPlayerParam;
import NPEnum.ENPPlayerRecordParam;
import NPGameRes.GameObjs.Mars.UsMarsBattle.UsMarsBattleCal;
import NPGameRes.GameObjs.Mars.UsMarsBattle.UsMarsBattleCal.FightResult;
import NPGameRes.GameObjs.Mars.UsMarsBattle._IUsMarsBattleObj;
import NPGameRes.Refs.Mars.RefMarsExploreEventBoss;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.GameSystem.MarsMineSystem.MarsMineSystem;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreEvent.MarsExploreEvent_BOSS;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreEvent._AMarsExploreEventInfo;
import NPUSServer.USLog;
import USDB.Bo.PlayerMarsExploreTeamBO;
import USLOGDB.Bo.LogMarsTeamBossBattleBO;

import java.nio.ByteBuffer;


/**
 * 火星队伍状态 - boss事件战斗
 * @author mj
 *
 */
public class ExploreTeamState_BOSS_BATTLE extends _AExploreTeamState implements _IUsMarsBattleObj
{
	private MarsTeamState_BossBattle _m_edExtraData;

	/**
	 * 来自行军状态的转变
	 * @param _preState
	 */
	public ExploreTeamState_BOSS_BATTLE(ExploreTeamState_MARCH _preState) 
	{
		super(_preState.getTeam(), _preState.getStateEndMs(), _preState.getKeepTimeMS(), _preState.getTargetPos());
		
		_m_edExtraData = new MarsTeamState_BossBattle();
		_m_edExtraData.setEventInstanceId(_preState.getExtData().getInstanceId());
		_m_edExtraData.setMarchTimeMS(_preState.getKeepTimeMS());
		_m_edExtraData.setPos(_preState.getTargetPos());
	}

	@Override
	public EMarsExploreTeamState getStateType() 
	{
		return EMarsExploreTeamState.BOSS_BATTLE;
	}
	
	@Override
	protected	void _loadExtData(PlayerMarsExploreTeamBO _bo)
	{
		if(null != _bo.getExtData())
		{
			ByteBuffer buff = ByteBuffer.wrap(_bo.getExtData());
			_m_edExtraData.readPackage(buff);
		}
	}
	
	@Override
	public MarsTeamState_BossBattle getExtData()
	{
		return _m_edExtraData;
	}
	
	@Override
	public boolean canEnter(_AExploreTeamState _state) 
	{
		//只能从行军状态开启
		return EMarsExploreTeamState.MARCH == _state.getStateType();
	}

	@Override
	protected void _onEnter() 
	{
		//标记对应的事件已经完成
		_AMarsExploreEventInfo eventObj = getTeam().getUserData().getMarsExploreComponent().getEventMgr().lookup(_m_edExtraData.getEventInstanceId());
		if(null == eventObj)
		{
			USLog.error(getTeam().getUSServer(), "player:{} team:{} boss event:{} mars explore event null."
					, getTeam().getCid(), getTeam().getTeamId(), _m_edExtraData.getEventInstanceId());
			return;
		}
		if(!(eventObj instanceof MarsExploreEvent_BOSS))
		{
			USLog.error(getTeam().getUSServer(), "player:{} team:{} id:{} boss event:{} type:{} mars explore event type error."
					, getTeam().getCid(), getTeam().getTeamId(), eventObj.getId(), eventObj.getEventId(), eventObj.getEventType());
			return;
		}

		//boss事件数据
		RefMarsExploreEventBoss bossRef = RefMarsExploreEventBoss.getMgr().get(eventObj.getEventId());
		if(null == bossRef)
		{
			USLog.error(getTeam().getUSServer(), "player:{} team:{} id:{} boss event:{} type:{} mars explore event type error."
					, getTeam().getCid(), getTeam().getTeamId(), eventObj.getId(), eventObj.getEventId(), eventObj.getEventType());
			return;
		}

		getTeam().getUserData().lockUser();

		try {
			//判断事件是否完成，已完成则不做战斗处理
			if (!eventObj.isDone()) {
				//计算战斗
				//FightResult fightResult = UsMarsBattleCal.calFight(this, bossRef);
				FightResult fightResult = UsMarsBattleCal.curUseCalFight(this, bossRef);

				//如果胜利则设置战斗结束
                if (fightResult != null)
                {
                    if (fightResult.win)
                    {
                        eventObj.setDone();
                        //记录次数
                        getTeam().getUserData().getRecordComponent().addRecord(ENPPlayerRecordParam.MARS_EXPLORER_ATTACK_BOSS,
                                1, NPPlayerContext.createNew(ENPGameEvent.MARS_EXPLORE_TEAM_STATUS_CHG));
                    }

					//记录战斗日志
					Mars_BossBattleEventLog battleEventLog = new Mars_BossBattleEventLog();
					battleEventLog.setIsSucc(fightResult.win);
					battleEventLog.setEventRefId(bossRef.Id());

					//进攻方玩家信息
					Mars_ExploreBattlePlayerInfo attackerInfo = battleEventLog.getAttacker();
					attackerInfo.setCid(getTeam().getCid());
					attackerInfo.setOriSoldierNum(fightResult.attackerOriSoldierNum);
					attackerInfo.setHurtSoldierNum(fightResult.attackerHurtSoldierNum);
					attackerInfo.setSingleSoldierPower(fightResult.attackerSingleSoldierPower);

					//NPC信息
					Mars_ExploreBattleNPCInfo npcInfo = battleEventLog.getNpc();
					npcInfo.setOriSoldierNum(fightResult.defencerOriSoldierNum);
					npcInfo.setHurtSoldierNum(fightResult.defencerHurtSoldierNum);
					npcInfo.setSingleSoldierPower(fightResult.defencerSingleSoldierPower);

                    //发送战斗事件日志
                    MarsMineSystem.SendLog(
                            getTeam().getUSServer(),
                            getTeam().getCid(),
                            Common.MarsEnum.EMarsExplorePVPLogType.BOSS_BATTLE_EVENT,
                            NPCommon.Util.CommonFunc.ByteBfferToBytes(battleEventLog.makePackage()));

                }

			}

			//不做其他处理，状态机会切换到返回状态
		}
		finally {
			getTeam().getUserData().unlockUser();
		}

	}

	@Override
	public boolean canQuit(_AExploreTeamState _targetState)
	{
		return true;
	}

	@Override
	protected void _onQuit() 
	{
	}

	@Override
	public _AExploreTeamState getNextState() 
	{
		return new ExploreTeamState_BACK(getTeam(), getStateStartMs(), getKeepTimeMS(), getTargetPos());
	}
    /**
     * 服务器启动后的一次性状态校验
     * 默认不处理，具体状态按需重载
     */
    @Override
    public void onSInitedCheck() { }

	/************************* 战斗处理对象的防守方重载函数 ******************/
	/**
	 * 获取当前可参战的战斗人员数量
	 * @return
	 */
	public long getTeamSoldierNum()
	{
		return getTeam().getTroopNum() - getTeam().getLossValue();
	}

	/**
	 * 获取参战的单兵实力
	 * @return
	 */
	public long getTeamSoldierPower()
	{
		return getTeam().getSoldierPower();
	}

	/**
	 * 增加伤兵数量，注意这里是增量不是全量
	 * @param _hurtNum
	 */
	public void addHurtSoldierNum(long _hurtNum)
	{
		//直接设置战损
		getTeam().setLossValue(getTeam().getLossValue() + _hurtNum);
	}

	/**
	 * 记录战斗日志的接口
	 * @param _isAttacker
	 * @param _isAttWin
	 * @param _attackPower
	 * @param _attckHurtNum
	 * @param _attackTroopNum
	 * @param _defencePower
	 * @param _defenceHurtNum
	 * @param _defenceTroopNum
	 */
	public void logBattle(boolean _isAttacker, boolean _isAttWin, _IUsMarsBattleObj _enemy, long _attackPower, long _attckHurtNum
			, long _attackTroopNum, long _defencePower, long _defenceHurtNum, long _defenceTroopNum)
	{
		//无需记录防守方（NPC）
		if(!_isAttacker)
			return;
		
		BM bmObj = getTeam().getUSServer().getBM();

        //记录日志
        LogMarsTeamBossBattleBO logBo = new LogMarsTeamBossBattleBO();
        //玩家基础数据
        logBo.setCid(bmObj, getTeam().getCid());
        logBo.setPlayerLevel(bmObj, (int) getTeam().getUserData().getParam(ENPPlayerParam.LEVEL));
        //队伍基础数据
        logBo.setTeamId(bmObj, getTeam().getTeamId());
        //防守方玩家数据
        logBo.setEventInstanceId(bmObj, _m_edExtraData.getEventInstanceId());
        RefMarsExploreEventBoss enemyInfo = (RefMarsExploreEventBoss)_enemy;
        logBo.setBossEventId(bmObj, enemyInfo.Id());
        //进攻结果
        logBo.setIsWin(bmObj, _isAttWin);
        //进攻方数据
        logBo.setTroopNum(bmObj, _attackTroopNum);
        logBo.setTeamPower(bmObj, _attackPower);
        //防守方数据
        logBo.setDefencePower(bmObj, _defencePower);

        CommLogDB.log(bmObj, logBo);
	}
	/************************* 战斗处理对象的防守方重载函数 end ******************/
}
