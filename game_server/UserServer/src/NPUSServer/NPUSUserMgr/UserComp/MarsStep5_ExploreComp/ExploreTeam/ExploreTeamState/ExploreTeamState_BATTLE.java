package NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.ExploreTeamState;

import Common.MarsEnum.EMarsExplorePVPLogType;
import Common.MarsEnum.EMarsExploreTeamState;
import Common.MarsObj.MarsTeamState_Battle;
import Common.MarsObj.Mars_BattleEventLog;
import Common.MarsObj.Mars_ExploreBattleNPCInfo;
import Common.MarsObj.Mars_ExploreBattlePlayerInfo;
import NPCommon.DB.BM.BM;
import NPCommon.NPLogDB.CommLogDB;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPPlayerParam;
import NPGameRes.GameObjs.Mars.UsMarsBattle.UsMarsBattleCal;
import NPGameRes.GameObjs.Mars.UsMarsBattle.UsMarsBattleCal.FightResult;
import NPGameRes.GameObjs.Mars.UsMarsBattle._IUsMarsBattleObj;
import NPGameRes.Refs.Mars.RefMarsExploreEventBattle;
import NPGameRes.Refs.Mars.RefMarsExploreLvl;
import NPUSServer.NPUSUserMgr.GameSystem.MarsMineSystem.MarsMineSystem;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreEvent.MarsExploreEvent_BATTLE;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreEvent._AMarsExploreEventInfo;
import NPUSServer.USLog;
import USDB.Bo.PlayerMarsExploreTeamBO;
import USLOGDB.Bo.LogMarsTeamBattleBO;

import java.nio.ByteBuffer;


/**
 * 火星队伍状态 - battle事件战斗中
 * @author mj
 *
 */
public class ExploreTeamState_BATTLE extends _AExploreTeamState implements _IUsMarsBattleObj
{
	private MarsTeamState_Battle _m_edExtraData;
	
	/**
	 * 来自行军状态的转变
	 * @param _preState
	 */
	public ExploreTeamState_BATTLE(ExploreTeamState_MARCH _preState) 
	{
		super(_preState.getTeam(), _preState.getStateEndMs(), _preState.getKeepTimeMS(), _preState.getTargetPos());
		
		_m_edExtraData = new MarsTeamState_Battle();
		_m_edExtraData.setEventInstanceId(_preState.getExtData().getInstanceId());
		_m_edExtraData.setMarchTimeMS(_preState.getKeepTimeMS());
		_m_edExtraData.setPos(_preState.getTargetPos());
	}

	@Override
	public EMarsExploreTeamState getStateType() 
	{
		return EMarsExploreTeamState.BATTLE;
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
	public MarsTeamState_Battle getExtData()
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
		//检查事件
		_AMarsExploreEventInfo event = getTeam().getUserData().getMarsExploreComponent().getEventMgr().lookup(_m_edExtraData.getEventInstanceId());
		if(null == event)
		{
			USLog.error(getTeam().getUSServer(), "player:{} team:{} event:{} mars explore event null."
					, getTeam().getCid(), getTeam().getTeamId(), _m_edExtraData.getEventInstanceId());
			return;
		}
		if(!(event instanceof MarsExploreEvent_BATTLE))
		{
			USLog.error(getTeam().getUSServer(), "player:{} team:{} id:{} event:{} type:{} mars explore event type error."
					, getTeam().getCid(), getTeam().getTeamId(), event.getId(), event.getEventId(), event.getEventType());
			return;
		}

		//获取对应的国力
		RefMarsExploreLvl exploreLvlRef = RefMarsExploreLvl.getMgr().get(event.getExploreLvl());
		if(null == exploreLvlRef)
		{
			USLog.error(getTeam().getUSServer(), "player:{} team:{} id:{} event:{} type:{} mars explore event type error."
					, getTeam().getCid(), getTeam().getTeamId(), event.getId(), event.getEventId(), event.getEventType());
			return;
		}

        RefMarsExploreEventBattle battleRef = RefMarsExploreEventBattle.getMgr().get(event.getEventId());
        if (battleRef == null)
        {
            USLog.error(getTeam().getUSServer(), "player:{} team:{} id:{} event:{} type:{} mars explore event type error."
                    , getTeam().getCid(), getTeam().getTeamId(), event.getId(), event.getEventId(), event.getEventType());
            return;
        }

		getTeam().getUserData().lockUser();

		try {
			//判断事件是否完成，已完成则不做战斗处理
			if (!event.isDone()) {

				//计算战斗
				//FightResult fightResult = UsMarsBattleCal.calFight(this, exploreLvlRef.getQualityBattle(event.getQuality()));
				FightResult fightResult = UsMarsBattleCal.curUseCalFight(this, exploreLvlRef.getQualityBattle(event.getQuality()));

				//如果胜利则设置战斗结束
                if (fightResult != null)
                {
                    if (fightResult.win)
                        event.setDone();

                    Mars_BattleEventLog battleEventLog = new Mars_BattleEventLog();
                    battleEventLog.setIsSucc(fightResult.win);
                    battleEventLog.setEventRefId(battleRef.Id());

                    //战斗双方数据
                    Mars_ExploreBattlePlayerInfo attackerInfo = battleEventLog.getAttacker();
                    attackerInfo.setCid(getTeam().getCid());
                    attackerInfo.setOriSoldierNum(fightResult.attackerOriSoldierNum);
                    attackerInfo.setHurtSoldierNum(fightResult.attackerHurtSoldierNum);
                    attackerInfo.setSingleSoldierPower(fightResult.attackerSingleSoldierPower);

                    Mars_ExploreBattleNPCInfo defenceInfo = battleEventLog.getNpc();
                    defenceInfo.setOriSoldierNum(fightResult.defencerOriSoldierNum);
                    defenceInfo.setHurtSoldierNum(fightResult.defencerHurtSoldierNum);
                    defenceInfo.setSingleSoldierPower(fightResult.defencerSingleSoldierPower);

                    //记录战斗日志
                    MarsMineSystem.SendLog(event.getUSServer(), getTeam().getCid(),
                            EMarsExplorePVPLogType.BATTLE_EVENT, CommonFunc.ByteBfferToBytes(battleEventLog.makePackage()));
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
		
		_AMarsExploreEventInfo event = getTeam().getUserData().getMarsExploreComponent().getEventMgr().lookup(_m_edExtraData.getEventInstanceId());
		if(null == event)
			return;
		
		BM bmObj = getTeam().getUSServer().getBM();

        //记录日志
        LogMarsTeamBattleBO logBo = new LogMarsTeamBattleBO();
        //玩家基础数据
        logBo.setCid(bmObj, getTeam().getCid());
        logBo.setPlayerLevel(bmObj, (int) getTeam().getUserData().getParam(ENPPlayerParam.LEVEL));
        //队伍基础数据
        logBo.setTeamId(bmObj, getTeam().getTeamId());
        //防守方玩家数据
        logBo.setEventInstanceId(bmObj, _m_edExtraData.getEventInstanceId());
        logBo.setBattleEventId(bmObj, event.getEventId());
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
