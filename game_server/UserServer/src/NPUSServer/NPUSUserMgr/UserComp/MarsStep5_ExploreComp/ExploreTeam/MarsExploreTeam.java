package NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam;

import Common.Common_LongList;
import Common.MarsEnum.EMarsExploreTeamState;
import Common.MarsObj.*;
import Common.ServerObj.ServerObj_MarsTeam_OccupyMinePlayer;
import CommonEnum.ECurrency;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.LazyTaskDealer.LazyTaskDealer;
import NPCommon.NPCommon_ItemInfo;
import NPCommon.Util.CallBack._ICallBackInt;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate.HandlerNone;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPEnum.ENPGameEvent;
import NPEnum.ENPItemType;
import NPEnum.ENPPlayerPropertyType;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.GameSystem.MarsMineSystem.MarsMineSystem;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroInfo;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.CollectResult.CollectResultMgr;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.ExploreTeamState.*;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_041_MarsExploreOp;
import NPUSServer.NPUserServer;
import USDB.Bo.PlayerMarsExploreTeamBO;

import java.nio.ByteBuffer;
import java.util.ArrayList;

/**
 * 火星队伍
 * @author mj
 *
 */
public class MarsExploreTeam implements _IHandlerHolder
{
    //加入集结行军默认时长（毫秒）
    private static final long JOIN_RALLY_MARCH_KEEP_MS = 3000L;
    // 默认集结前往的位置
    private static final long RALLY_TARGET_POS_DEFAULT = 101;

	//玩家数据对象
	private NPUSUserData _m_udUserData;
	
	//队伍 team_id
	private long _m_lTeamId;
	//队伍名称
	private String _m_sName;
	//入驻大臣列表
	private Common_LongList _m_llHeroListObj;
	//当前战损数量
	private long _m_lLossValue;
	
	//队伍状态
	private ExploreTeamStateMachine _m_smStateMachine;
	
	//数据Bo
	private PlayerMarsExploreTeamBO _m_bo;

	//采集结果数据管理
	private CollectResultMgr _m_crCollectResultMgr;
	
	//带兵量
	private long _m_lTroopNum;
    //健康指数延迟计算
    private LazyTaskDealer _m_ldCalTroopNumDealer;
    
    //单兵实力
    private long _m_lSoldierPower;
    //单兵实力延迟计算
    private LazyTaskDealer _m_ldCalTeamSoldierPowerDealer;
	
	public MarsExploreTeam(NPUSUserData _userData, long _teamId)
	{
		_m_udUserData = _userData;
		
		_m_lTeamId = _teamId;
		
		_m_sName = "";
		_m_llHeroListObj = new Common_LongList();
		
		_m_smStateMachine = new ExploreTeamStateMachine(this);
		
		_m_crCollectResultMgr = new CollectResultMgr(this);
		
		_m_ldCalTroopNumDealer = new LazyTaskDealer(() -> MarsExploreTeamCalculate.calTroopNum(this, null), 200);
		_m_ldCalTeamSoldierPowerDealer = new LazyTaskDealer(() -> MarsExploreTeamCalculate.calSoldierTeamPower(this, null), 200);
	}
	
	public NPUSUserData getUserData() {return _m_udUserData;}
    public NPUserServer getUSServer() {return getUserData().getUSServer();}
    public long getCid() {return getUserData().getCid();}
    public BM getBM() {return getUserData().getUSServer().getBM();}
    
    public long getTeamId() {return _m_lTeamId;}
    public String getName() {return _m_sName;}
    public ArrayList<Long> getHeroIdList() {return new ArrayList<>(_m_llHeroListObj.getValueList());}
    public long getLossValue() {return _m_lLossValue;}
    
    public ExploreTeamStateMachine getStateMachine() {return _m_smStateMachine;}
    
    public PlayerMarsExploreTeamBO getBo() {return _m_bo;}
    
    public CollectResultMgr getCollectResultMgr() {return _m_crCollectResultMgr;}
    
    //带兵量相关
    public long getTroopNum() {return _m_lTroopNum;}
    public void setTroopNum(long _value) 
    {
    	getUserData().lockUser();
    	
    	try
    	{
			if(_m_lTroopNum == _value)
				return ;

    		_m_lTroopNum = _value;
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    public void doLazyCalTroopNum() {_m_ldCalTroopNumDealer.setNeedDeal();}
    
    //队伍实力相关
    public long getSoldierPower() {return _m_lSoldierPower;}
    public long getTeamPower() {return _m_lSoldierPower * _m_lTroopNum;}
    public void setSoldierPower(long _soldierPower)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		_m_lSoldierPower = _soldierPower;
    		
    		//发起计算全部队伍最高实力
    		getUserData().getMarsExploreComponent().getTeamMgr().calMarsTeamPowerSum();
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    public void doLazyCalSoldierTeamPower() {_m_ldCalTeamSoldierPowerDealer.setNeedDeal();}
    
    protected void _loadBo(PlayerMarsExploreTeamBO _bo) 
    {
    	_m_bo = _bo;
    	
    	//基础数据
    	_m_lLossValue = _bo.getLossValue();
    	
    	//加载入驻大臣数据
    	if(null != _bo.getHeroIdList())
    	{
    		ByteBuffer buff = ByteBuffer.wrap(_bo.getHeroIdList());
    		_m_llHeroListObj.readPackage(buff);
    		
    		getUserData().getMarsExploreComponent().getTeamMgr()._initHeroList(_m_llHeroListObj.getValueList());
    	}
    	
    	//加载状态机
    	_m_smStateMachine._loadFromBo(_m_bo);
	}
    
    /**
     * 所有组件加载完成后调用
     */
    protected void _onInited()
    {
    	//设置当前大臣监听
    	_setHeroPowerChgDealer();
    }

    /**
     * 服务器启动后的队伍状态校验
     */
    public void onSInitedCheck()
    {
        getStateMachine().onSInitedCheck();
    }

    /**
     * 设置当前大臣实力变化监听
     */
    private void _setHeroPowerChgDealer()
    {
    	for(int i = 0; i < _m_llHeroListObj.getValueList().size(); i++)
    	{
    		long heroId = _m_llHeroListObj.getValueList().get(i);
    		HeroInfo hero = getUserData().getHeroComponent().lookupHero(heroId);
    		if(null == hero)
    			continue;
    		
    		hero.getPowerChgDelegate().addHandler(this, 
    				new HandlerNone() 
    		{	
				@Override
				public void handle() 
				{
					//计算带兵量
					doLazyCalTroopNum();
					//计算单兵实力
					doLazyCalSoldierTeamPower();
				}
			});
    	}
    }
    /**
     * 清空当前大臣实力变化监听
     */
    private void _clearHeroPowerChgDealer()
    {
    	for(int i = 0; i < _m_llHeroListObj.getValueList().size(); i++)
    	{
    		long heroId = _m_llHeroListObj.getValueList().get(i);
    		HeroInfo hero = getUserData().getHeroComponent().lookupHero(heroId);
    		if(null == hero)
    			continue;
    		
    		hero.getPowerChgDelegate().clear(this);
    	}
    }
    
    /**
     * 保存数据，外部方法确认是否创建数据
     */
	public void _saveAll()
	{
		getUserData().lockUser();

		try
		{
			if(null == _m_bo)
			{
				_m_bo = __saveNew_InLock();
			}
			else
			{
				_m_bo.setName(getBM(), _m_sName);
				_m_bo.setCurState(getBM(), _m_smStateMachine.getCurState().getStateType().ordinal());
				_m_bo.setHeroIdList(getBM(), CommonFunc.ByteBfferToBytes(_m_llHeroListObj.makePackage()));
				_m_bo.setCurStateStartMs(getBM(), _m_smStateMachine.getCurState().getStateStartMs());
				_m_bo.setCurStateKeepTimeMS(getBM(), _m_smStateMachine.getCurState().getKeepTimeMS());
				_m_bo.setTargetPos(getBM(), _m_smStateMachine.getCurState().getTargetPos());
				if(null != _m_smStateMachine.getCurState().getExtData())
				{
					_m_bo.setExtData(getBM(), CommonFunc.ByteBfferToBytes(_m_smStateMachine.getCurState().getExtData().makePackage()));
				}
				_m_bo.setLossValue(getBM(), _m_lLossValue);
				_m_bo.saveAll(getBM());
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	//保存非扩展信息的部分
	public void _saveUnExNoHeroData()
	{
		getUserData().lockUser();

		try
		{
			if(null == _m_bo)
			{
				_m_bo = __saveNew_InLock();
			}
			else
			{
				_m_bo.setName(getBM(), _m_sName);
				_m_bo.setCurState(getBM(), _m_smStateMachine.getCurState().getStateType().ordinal());
				_m_bo.setCurStateStartMs(getBM(), _m_smStateMachine.getCurState().getStateStartMs());
				_m_bo.setCurStateKeepTimeMS(getBM(), _m_smStateMachine.getCurState().getKeepTimeMS());
				_m_bo.setTargetPos(getBM(), _m_smStateMachine.getCurState().getTargetPos());
				_m_bo.setLossValue(getBM(), _m_lLossValue);
				_m_bo.saveAll(getBM());
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	public void _saveExData()
	{
		getUserData().lockUser();

		try
		{
			if(null == _m_bo)
			{
				_m_bo = __saveNew_InLock();
			}
			else
			{
				if(null != _m_smStateMachine.getCurState().getExtData())
				{
					_m_bo.setExtData(getBM(), CommonFunc.ByteBfferToBytes(_m_smStateMachine.getCurState().getExtData().makePackage()));
				}
				_m_bo.saveAll(getBM());
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

	/**
	 * 仅允许内部函数调用，这里不加锁，由调用函数加锁
	 */
	private PlayerMarsExploreTeamBO __saveNew_InLock()
	{
		PlayerMarsExploreTeamBO bo = new PlayerMarsExploreTeamBO();
		bo.setCid(getBM(), getCid());
		bo.setTeamId(getBM(), _m_lTeamId);
		bo.setCurState(getBM(), _m_smStateMachine.getCurState().getStateType().ordinal());
		bo.setName(getBM(), _m_sName);
		bo.setHeroIdList(getBM(), CommonFunc.ByteBfferToBytes(_m_llHeroListObj.makePackage()));
		bo.setCurStateStartMs(getBM(), _m_smStateMachine.getCurState().getStateStartMs());
		bo.setCurStateKeepTimeMS(getBM(), _m_smStateMachine.getCurState().getKeepTimeMS());
		bo.setTargetPos(getBM(), _m_smStateMachine.getCurState().getTargetPos());
		if(null != _m_smStateMachine.getCurState().getExtData())
		{
			bo.setExtData(getBM(), CommonFunc.ByteBfferToBytes(_m_smStateMachine.getCurState().getExtData().makePackage()));
		}
		bo.setLossValue(getBM(), _m_lLossValue);
		bo.insert(getBM());

		return bo;
	}
	
    /**
     * 检查是否解锁
     * @return
     */
    public boolean checkUnlock()
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		//刷新队伍状态
    		_m_smStateMachine.refreshState();
    		
    		//尚未解锁
    		if(EMarsExploreTeamState.NONE == _m_smStateMachine.getCurState().getStateType())
    			return false;
    		
    		//错误状态无法解锁
    		if(EMarsExploreTeamState.ERROR == _m_smStateMachine.getCurState().getStateType())
    			return false;
    		
    		//其他情况表示已经解锁
    		return true;
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }

	/**
	 * 检查是否解锁
	 * @return
	 */
	public boolean isUnlock()
	{
		//尚未解锁
		if(EMarsExploreTeamState.NONE == _m_smStateMachine.getCurState().getStateType())
			return false;

		//错误状态无法解锁
		if(EMarsExploreTeamState.ERROR == _m_smStateMachine.getCurState().getStateType())
			return false;

		//其他情况表示已经解锁
		return true;
	}
    
    /**
     * 检查是否拥有指定大臣
     * @param _heroId
     * @return
     */
    public boolean hasHero(long _heroId)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		return _m_llHeroListObj.getValueList().contains(_heroId);
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 获取队伍大臣的实力
     * @return
     */
    public long getHeroPowerSum()
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		long powerSum = 0;
    		for(int i = 0; i < _m_llHeroListObj.getValueList().size(); i++)
    		{
    			HeroInfo hero = getUserData().getHeroComponent().lookupHero(_m_llHeroListObj.getValueList().get(i));
    			if(null == hero)
    				continue;
    			
    			powerSum += Math.ceil(Math.sqrt(hero.getPower()));
    		}
    		
    		return powerSum;
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 构造状态数据
     * @return
     */
    public Mars_TeamState toStateProto()
    {
    	return getStateMachine().getCurState().toProto();
    }
    
    /**
     * 构造占据玩家数据
     * @return
     */
    public ServerObj_MarsTeam_OccupyMinePlayer toServerOccupyPlayer()
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		ServerObj_MarsTeam_OccupyMinePlayer proto = new ServerObj_MarsTeam_OccupyMinePlayer();
        	proto.setCid(getCid());
        	proto.setTeamId(_m_lTeamId);
        	proto.setTeamSoldierPower(_m_lSoldierPower);
        	proto.setTeamTroopNum(_m_lTroopNum); 
        	proto.setTeamLossValue(_m_lLossValue);
        	proto.setGuildId(getUserData().getGuildComponent().getGuildId());
            proto.setCname(getUserData().getPlayerComponent().getName());
            proto.setGuildBName(getUserData().getGuildComponent().getGuildSimpleName());
        	return proto;
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 构造数据协议
     * @return
     */
    public Mars_Team toProto()
    {
    	getUserData().lockUser();
    	
    	try
    	{
        	Mars_Team proto = new Mars_Team();
        	proto.setTeamId(_m_lTeamId);
        	proto.setName(_m_sName);
        	proto.getHeroIdList().addAll(_m_llHeroListObj.getValueList());
        	proto.setState(toStateProto());
        	proto.setLossValue(_m_lLossValue);
        	
        	return proto;
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }

    public MarsBattleV2_MemberInfo makeBattleV2_MemberInfo()
    {
        getUserData().lockUser();

        try
        {
            MarsBattleV2_MemberInfo teamSnapshot = new MarsBattleV2_MemberInfo();

            //TODO : 构造队伍截面数据，改造火星属性系统的时候再处理

            return teamSnapshot;
        }
        finally
        {
            getUserData().unlockUser();
        }
    }
    
    /**
     * 设置队伍损耗数量
     * @param _value
     */
    public void setLossValue(long _value)
    {
    	getUserData().lockUser();
    	
    	try
    	{
			//数据一致不同步，不修改
			if(_m_lLossValue == _value)
				return ;

    		//更新数据
    		_m_lLossValue = _value;
			_saveUnExNoHeroData();
    		
    		//推送数据
    		getUserData().sendMsgToGC(US2GCWriter_041_MarsExploreOp.make_062_OnExploreTeamLossValueChg(this));
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 减少队伍损耗
     * @param _value
     */
    public void reduceLossValue(long _value)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		//更新数据
    		_m_lLossValue = Math.max(_m_lLossValue - _value, 0);
			_saveUnExNoHeroData();
    		
    		//推送数据
    		getUserData().sendMsgToGC(US2GCWriter_041_MarsExploreOp.make_062_OnExploreTeamLossValueChg(this));
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 队伍返回触发
     */
    public void onBackDone()
    {
		NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.MARS_EXPLORE_TEAM_BACK_DONE);
    	//清空全部采集奖励
    	getCollectResultMgr().dispatchAll(context);
	}
    
    /**
     * 设置队伍名称
     * @param _name
     * @param _context
     * @return
     */
    public Result setName(String _name, NPPlayerContext _context)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		if(!checkUnlock())
    			return MarsErr.MARS_EXPLORE_TEAM_NOT_UNLOCK;
    		
    		_m_sName = _name;
			_saveUnExNoHeroData();
    		
    		//推送数据
    		getUserData().sendMsgToGC(US2GCWriter_041_MarsExploreOp.make_056_OnExploreTeamNameChg(this));
    		
    		return Result.SUCC;
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 增加队伍大臣
     * @param _heroList
     * @param _context
     * @return
     */
    public Result setHeroList(ArrayList<HeroInfo> _heroList, NPPlayerContext _context)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		//检查队伍解锁
    		if(!checkUnlock())
    			return MarsErr.MARS_EXPLORE_TEAM_NOT_UNLOCK;
    		
    		//检查队伍状态
    		if(!getStateMachine().isIdle())
    			return MarsErr.MARS_EXPLORE_TEAM_STATE_ERROR;
    		
    		//检查队伍损耗
    		if(getLossValue() > 0)
    			return MarsErr.MARS_TEAM_LOSS_NOT_EMPTY;
    		
    		//移除原有大臣的监听
    		_clearHeroPowerChgDealer();
    		
    		//新大臣数据列表
    		ArrayList<Long> newHeroIdList = new ArrayList<>();
    		//对新大臣列表进行检查
    		for(int i = 0; i < _heroList.size(); i++)
    		{
    			HeroInfo hero = _heroList.get(i);
    			if(null == hero)
    				continue;

        		//避免重复使用同一大臣
    			if(newHeroIdList.contains(hero.getHeroId()))
    				return MarsErr.MARS_EXPLORE_TEAM_HERO_REPEAT;
    			
    			//对不再本队伍的大臣进行检查
    			if(!_m_llHeroListObj.getValueList().contains(hero.getHeroId())
    					&& getUserData().getMarsExploreComponent().getTeamMgr().hasHero(hero.getHeroId()))
    				return MarsErr.MARS_EXPLORE_TEAM_HERO_USED;

    			newHeroIdList.add(hero.getHeroId());
    		}
    		
    		//更新组件原有数据
    		getUserData().getMarsExploreComponent().getTeamMgr()._replaceHeroList(_m_llHeroListObj.getValueList(), newHeroIdList);
    		
    		//增加大臣数据
    		_m_llHeroListObj.getValueList().clear();
    		_m_llHeroListObj.getValueList().addAll(newHeroIdList);
    		
    		_saveAll();
    		
    		//设置当前大臣实力变化监听
    		_setHeroPowerChgDealer();
    		
    		//推送数据
    		getUserData().sendMsgToGC(US2GCWriter_041_MarsExploreOp.make_057_OnExploreTeamHeroChg(this));

			//计算带兵量
			doLazyCalTroopNum();
    		//发起队伍实力计算
    		doLazyCalSoldierTeamPower();
    		
    		return Result.SUCC;
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 刷新队伍状态
     */
    public _AExploreTeamState cmdRefreshState()
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		getStateMachine().refreshState();
    		
    		return getStateMachine().getCurState();
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 开启队伍修复
     * @param _repairNum
     * @param _context
     * @return
     */
    public Result startRepair(long _repairNum, NPPlayerContext _context)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		//获取实际修复伤兵数量，如果为0，则使用当前全部的伤兵数量
    		long realRepairNum = 0;
    		if(_repairNum <= 0)
                realRepairNum = _m_lLossValue;
            else
                realRepairNum = _repairNum;
    		
    		//没有精兵，不需要进行修复
    		if(realRepairNum <= 0)
    			return MarsErr.MARS_TEAM_LOSS_EMPTY;
    		else if(realRepairNum > _m_lLossValue)
    			return MarsErr.MARS_TEAM_LOSS_NUM_ERROR;
    		
    		//计算消耗
			ArrayList<NPCommonCostItem> realCostItemList = _calRepairCostItemList(realRepairNum);
			
			//检查消耗
			if(!getUserData().hasCostItemList(realCostItemList))
				return CommErr.ITEM_NOT_ENOUGH;
			
			if(!getUserData().spendCostItemList(realCostItemList, _context))
				return CommErr.CONSUME_FAIL;
    		
    		//计算时间
    		long repairMS = realRepairNum * RefGeneral.Ref().mars_explore_team_repair_unit_ms;
    		long property = getUserData().getPlayerComponent().getPropertyMgr().getValue(ENPPlayerPropertyType.MARS_REPAIR_SPEED_PER);
    		//GOB-8288 修正所有涉及加速万分比的计算 https://www.teambition.com/task/69593ecb0bc2f864a6a42c82
    		//time = time / ((10000 + per)/10000)
			repairMS = repairMS * 10000 / (10000 + property);
			repairMS = Math.max(repairMS, 0);
    		
    		//状态额外数据
    		MarsTeamState_Repair extData = new MarsTeamState_Repair();
    		extData.setRepairNum(realRepairNum);
    		//状态数据
    		ExploreTeamState_REPAIR targetState = new ExploreTeamState_REPAIR(this, CommonFunc.getNowTimeMS(), repairMS, extData);
    		//尝试切换状态
    		if(!getStateMachine().transState(targetState))
    		{
    			return MarsErr.MARS_EXPLORE_TEAM_STATE_ERROR;
    		}
    		
    		return Result.SUCC;
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 离开火星矿
     * @param _callback
     */
    public void leaveMarsMine(_ICallBackInt _callback)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		//检查当前状态
    		_AExploreTeamState state = getStateMachine().getCurState();
    		if(!(state instanceof ExploreTeamState_COLLECT))
    		{
    			_callback.onRunOver(MarsErr.MARS_EXPLORE_TEAM_STATE_ERROR.getCode());
    			return;
    		}
 
    		//结束当前采集状态
    		ExploreTeamState_COLLECT collectState = (ExploreTeamState_COLLECT) state;
    		
    		//发起离开火星矿
    		MarsMineSystem.LeaveMarsMine(getUSServer(), collectState, (err) -> 
    		{
    			if(err > 0)
    			{
    	    		_callback.onRunOver(err);  	
    				return;
    			}

        		//返回成功
        		_callback.onRunOver(0);
    		});    		
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 强制设置队伍空闲
     * @param _context
     */
    public void setIdleStatus(NPPlayerContext _context)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		//强制设置空闲状态
    		getStateMachine().setIdle();
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 设置队伍损耗数量
     * @param _lossValue
     * @param _context
     */
    public void cmdSetLossValue(long _lossValue, NPPlayerContext _context)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		_m_lLossValue = _lossValue;
			_saveUnExNoHeroData();
    		
    		//推送数据
    		getUserData().sendMsgToGC(US2GCWriter_041_MarsExploreOp.make_062_OnExploreTeamLossValueChg(this));
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 设置修理完成
     * @param _context
     * @return
     */
    public Result setRepairDone(NPPlayerContext _context)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		//检查配置
			int diamondRatio = RefGeneral.Ref().mars_building_sec_to_diamond_ratio;
			if(diamondRatio <= 0)
				return CommErr.REF_ERROR;
			
    		//检查当前状态
    		getStateMachine().refreshState();
    		
    		_AExploreTeamState state = getStateMachine().getCurState();
    		if(!(state instanceof ExploreTeamState_REPAIR))
    		{
    			return MarsErr.MARS_EXPLORE_TEAM_STATE_ERROR;
    		}
    		
    		ExploreTeamState_REPAIR repairState = (ExploreTeamState_REPAIR) state;
    		
    		//检查消耗物品
    		long needMs = repairState.getRepairEndTime() - CommonFunc.getNowTimeMS();
    		int num = (int) Math.ceil(1.0f * needMs / (diamondRatio * 1000f));
			
			//检查并扣除钻石
			if(!getUserData().hasItem(ENPItemType.CURRENCY, ECurrency.GEM.ordinal(), num))
				return CommErr.ITEM_NOT_ENOUGH;
			
			if(!getUserData().spendItem(ENPItemType.CURRENCY, ECurrency.GEM.ordinal(), num, _context))
				return CommErr.CONSUME_FAIL;
			
			//更新维修状态数据
			repairState.setDone();
    		
    		//切换状态
    		ExploreTeamState_IDLE targetState = new ExploreTeamState_IDLE(this);
    		//尝试切换状态
    		if(!getStateMachine().transState(targetState))
    		{
    			return MarsErr.MARS_EXPLORE_TEAM_TRANS_FAIL;
    		}
    		
    		return Result.SUCC;
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }

    /**
     * 开始加入等待集结行军状态
     * 此行为为创建者执行的行为
     *
     * @param _context 操作上下文
     * @return 切换结果
     */
    public ResultOne<Long> startRally(NPPlayerContext _context) {
        getUserData().lockUser();

        try {
            ExploreTeamState_WAIT_RALLY targetState =
                    new ExploreTeamState_WAIT_RALLY(this, CommonFunc.getNowTimeMS(), JOIN_RALLY_MARCH_KEEP_MS,
                            RALLY_TARGET_POS_DEFAULT);

            if (!getStateMachine().transState(targetState)) {
                return new ResultOne<Long>(MarsErr.MARS_EXPLORE_TEAM_STATE_ERROR, 0L);
            }

            return new ResultOne<Long>(Result.SUCC, targetState.getStateSerialize());
        } finally {
            getUserData().unlockUser();
        }
    }

    /**
     * 绑定WAIT_RALLY状态的集结ID
     *
     * 处理规则：
     * 1. 仅允许更新当前序列号对应的WAIT_RALLY状态
     * 2. 更新后立即持久化extData，保证重启校验可用
     *
     * @param _stateSerialize 期望状态序列号
     * @param _rallyId 集结ID
     * @return 处理结果
     */
    public Result bindWaitRallyId(long _stateSerialize, long _rallyId)
    {
        getUserData().lockUser();

        try {
            if (_rallyId <= 0) {
                return CommErr.PARAM_ERROR;
            }

            _AExploreTeamState state = getStateMachine().getCurState();
            if (state == null
                    || state.getStateSerialize() != _stateSerialize
                    || state.getStateType() != EMarsExploreTeamState.WAIT_RALLY) {
                return MarsErr.MARS_EXPLORE_TEAM_STATE_ERROR;
            }

            ExploreTeamState_WAIT_RALLY waitRallyState = (ExploreTeamState_WAIT_RALLY) state;
            Result bindResult = waitRallyState.bindRallyId(_rallyId);
            if (!bindResult.isSucc()) {
                return bindResult;
            }

            //保存额外数据
            _saveExData();

            return Result.SUCC;
        } finally {
            getUserData().unlockUser();
        }
    }

    /**
     * 开始加入集结行军状态
     *
     * 执行顺序：
     * 1. 检查当前状态是否可进入加入行军态
     * 2. 切换到 WAIT_RALLY
     *
     * @param _rallyId 集结ID
     * @param _context 操作上下文
     * @return 切换结果
     */
    public ResultOne<Long> startJoinRally(long _rallyId, NPPlayerContext _context) {
        getUserData().lockUser();

        try {
            if (_rallyId <= 0)
                return new ResultOne<Long>(CommErr.PARAM_ERROR, 0L);

            ExploreTeamState_MARCH targetState =
                    new ExploreTeamState_MARCH(this, CommonFunc.getNowTimeMS(), JOIN_RALLY_MARCH_KEEP_MS,
                            RALLY_TARGET_POS_DEFAULT, new MarsTeamState_March(_rallyId, EMarsExploreTeamState.WAIT_RALLY.ordinal()));

            if (!getStateMachine().transState(targetState)) {
                return new ResultOne<Long>(MarsErr.MARS_EXPLORE_TEAM_STATE_ERROR, 0L);
            }

            return new ResultOne<Long>(Result.SUCC, targetState.getStateSerialize());
        } finally {
            getUserData().unlockUser();
        }
    }

    /**
     * 加入集结行军到达后切换到等待集结出发状态
     *
     * 处理规则：
     * 1. 当前已是WAIT_RALLY时按幂等成功处理
     * 2. 当前必须为MARCH状态，其他状态返回状态错误
     *
     * @param _context 操作上下文
     * @return 处理结果
     */
    public Result joinRallyToWait(NPPlayerContext _context)
    {
        getUserData().lockUser();

        try {
            _AExploreTeamState state = getStateMachine().getCurState();
            if (state == null) {
                return MarsErr.MARS_EXPLORE_TEAM_STATE_ERROR;
            }

            if (state.getStateType() == EMarsExploreTeamState.WAIT_RALLY) {
                return Result.SUCC;
            }

            if (state.getStateType() != EMarsExploreTeamState.MARCH) {
                return MarsErr.MARS_EXPLORE_TEAM_STATE_ERROR;
            }

            ExploreTeamState_MARCH marchState = (ExploreTeamState_MARCH) state;
            if(marchState.getExtData().getTargetState() != EMarsExploreTeamState.WAIT_RALLY.ordinal())
            {
                return MarsErr.MARS_EXPLORE_TEAM_STATE_ERROR;
            }

            ExploreTeamState_WAIT_RALLY targetState =
                    new ExploreTeamState_WAIT_RALLY(this, CommonFunc.getNowTimeMS(), JOIN_RALLY_MARCH_KEEP_MS,
                            state.getTargetPos(), marchState.getExtData().getInstanceId());
            if (!getStateMachine().transState(targetState)) {
                return MarsErr.MARS_EXPLORE_TEAM_STATE_ERROR;
            }

            return Result.SUCC;
        } finally {
            getUserData().unlockUser();
        }
    }

    /**
     * 加入集结失败后遣返队伍状态
     *
     * 处理规则：
     * 1. 当前状态必须为MARCH或WAIT_RALLY
     * 2. 通过状态机统一进入BACK状态
     *
     * @param _context 操作上下文
     * @return 处理结果
     */
    public Result joinRallyFailBack(NPPlayerContext _context)
    {
        getUserData().lockUser();

        try {
            _AExploreTeamState state = getStateMachine().getCurState();
            if (state == null) {
                return MarsErr.MARS_EXPLORE_TEAM_STATE_ERROR;
            }

            if (state.getStateType() != EMarsExploreTeamState.MARCH
                    && state.getStateType() != EMarsExploreTeamState.WAIT_RALLY) {
                return MarsErr.MARS_EXPLORE_TEAM_STATE_ERROR;
            }

            return getStateMachine().sendbackTeam(CommonFunc.getNowTimeMS(), _context);
        } finally {
            getUserData().unlockUser();
        }
    }

    /**
     * 任意失败需要回滚：将队伍状态重置为空闲
     * 时根据带入的序列号一致性判断是否允许重置
     *
     * @param _context 操作上下文
     */
    public void resetIdleState(long _stateSerialize, NPPlayerContext _context)
    {
        getUserData().lockUser();

        try {
            _AExploreTeamState state = getStateMachine().getCurState();
            if(state.getStateSerialize() != _stateSerialize)
            {
                return ;
            }

            //重置状态
            setIdleStatus(_context);
        } finally {
            getUserData().unlockUser();
        }
    }

    /**
     * 取消队伍维修
     *
     * 业务规则：
     * 1. 返还已消耗的资源（按开始维修时的计算公式）
     * 2. 不返还已消耗的加速道具
     * 3. 损耗士兵数量保持不变（不执行恢复）
     *
     * 执行流程：
     * 1. 检查当前状态是否为维修中
     * 2. 根据 repairNum 重新计算消耗的资源
     * 3. 返还资源
     * 4. 强制转换到 IDLE 状态
     *
     * @param _context 操作上下文
     * @return 执行结果
     *
     * 线程安全：通过 lockUser() 保护
     */
    public Result cancelRepair(NPPlayerContext _context)
    {
    	getUserData().lockUser();

    	try
    	{
    		// 刷新状态
    		getStateMachine().refreshState();

    		// 检查当前状态
    		_AExploreTeamState state = getStateMachine().getCurState();
    		if(!(state instanceof ExploreTeamState_REPAIR))
    		{
    			return MarsErr.MARS_EXPLORE_TEAM_STATE_ERROR;
    		}

            ExploreTeamState_REPAIR repairState = (ExploreTeamState_REPAIR) state;

            //更新维修状态数据
            repairState.setCancel();

            ArrayList<NPCommon_ItemInfo> costItemList = repairState.getExtData().getCostItemList();

            // 先切换状态到 IDLE
    		ExploreTeamState_IDLE targetState = new ExploreTeamState_IDLE(this);
    		if(!getStateMachine().transState(targetState))
    		{
    			return MarsErr.MARS_EXPLORE_TEAM_TRANS_FAIL;
    		}

    		// 再返还资源
    		if(!costItemList.isEmpty())
    			getUserData().gainItemListP(costItemList, _context);

    		return Result.SUCC;
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }

    /**
     * 开始并立即完成队伍维修
     *
     * 功能说明：
     * 一次性完成维修，同时扣除维修成本和钻石加速成本，不进入维修状态
     *
     * 业务规则：
     * 1. 扣除维修成本（材料资源）
     * 2. 扣除钻石加速成本（基于维修时长）
     * 3. 直接恢复士兵损失值
     * 4. 保持在当前状态（不进入 REPAIR 状态）
     *
     * 执行流程：
     * 1. 检查当前状态（必须是可以开始维修的状态）
     * 2. 验证并获取实际修复士兵数量
     * 3. 计算维修成本（材料）
     * 4. 计算维修时间
     * 5. 计算钻石加速成本
     * 6. 检查并扣除维修成本
     * 7. 检查并扣除钻石
     * 8. 直接恢复士兵损失值
     *
     * @param _repairNum 修复士兵数，0表示全部
     * @param _context 操作上下文
     * @return 执行结果
     *
     * 线程安全：通过 lockUser() 保护
     */
    public Result startAndFinishRepair(long _repairNum, NPPlayerContext _context)
    {
    	getUserData().lockUser();

    	try
    	{
    		// 检查当前状态（必须是能开始维修的状态）
    		getStateMachine().refreshState();

    		_AExploreTeamState state = getStateMachine().getCurState();
    		if(!(state instanceof ExploreTeamState_IDLE))
    		{
    			return MarsErr.MARS_EXPLORE_TEAM_STATE_ERROR;
    		}

    		// 获取实际修复伤兵数量，如果为0，则使用当前全部的伤兵数量
    		long realRepairNum = 0;
    		if(_repairNum <= 0)
    			realRepairNum = _m_lLossValue;
    		else
    			realRepairNum = _repairNum;

    		// 没有损失士兵，不需要进行修复
    		if(realRepairNum <= 0)
    			return MarsErr.MARS_TEAM_LOSS_EMPTY;
    		else if(realRepairNum > _m_lLossValue)
    			return MarsErr.MARS_TEAM_LOSS_NUM_ERROR;

    		// 1. 计算维修成本（材料资源）
    		ArrayList<NPCommonCostItem> realCostItemList = _calRepairCostItemList(realRepairNum);

    		// 2. 计算维修时间
    		long repairMS = realRepairNum * RefGeneral.Ref().mars_explore_team_repair_unit_ms;
    		long timeProperty = getUserData().getPlayerComponent().getPropertyMgr().getValue(ENPPlayerPropertyType.MARS_REPAIR_SPEED_PER);
    		//GOB-8288 修正所有涉及加速万分比的计算 https://www.teambition.com/task/69593ecb0bc2f864a6a42c82
    		//time = time / ((10000 + per)/10000)
    		repairMS =  repairMS * 10000 / (10000 + timeProperty);
    		repairMS = Math.max(repairMS, 0);

    		// 3. 计算钻石加速成本
    		int diamondRatio = RefGeneral.Ref().mars_building_sec_to_diamond_ratio;
    		if(diamondRatio <= 0)
    			return CommErr.REF_ERROR;

    		int diamondCost = (int) Math.ceil(1.0f * repairMS / (diamondRatio * 1000f));
            realCostItemList.add(new NPCommonCostItem(ENPItemType.CURRENCY, ECurrency.GEM.ordinal(), diamondCost));

    		// 4. 检查维修成本
    		if(!getUserData().hasCostItemList(realCostItemList))
    			return CommErr.ITEM_NOT_ENOUGH;

    		// 6. 扣除维修成本
    		if(!getUserData().spendCostItemList(realCostItemList, _context))
    			return CommErr.CONSUME_FAIL;

    		// 8. 直接恢复士兵损失值（避免重复加锁，直接操作）
            reduceLossValue(realRepairNum);

    		return Result.SUCC;
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }

	/**
	 * 计算维修成本
	 * @param _repairNum
	 * @return
	 */
	protected ArrayList<NPCommonCostItem> _calRepairCostItemList(long _repairNum)
	{
		// 1. 计算维修成本（材料资源）
		long costProperty = getUserData().getPlayerComponent().getPropertyMgr()
				.getValue(ENPPlayerPropertyType.MARS_REPAIR_COST_PER);
		ArrayList<NPCommonCostItem> realCostItemList = new ArrayList<>();
		ArrayList<NPCommonCostItem> costItemList = RefGeneral.Ref().mars_explore_team_repair_unit_cost_list;
		//仅取出玩家单兵实力的绝对值基数，用于计算维修成本倍率
		//https://www.teambition.com/task/695bddb5dc51a877c25e5b42
		long pureSoldierPower = getUserData().getPlayerComponent().getPropertyMgr().getValue(ENPPlayerPropertyType.MARS_TEAM_SOLDIER_POWER);

		for(int i = 0; i < costItemList.size(); i++)
		{
			NPCommonCostItem costItem = costItemList.get(i);
			if(null == costItem)
				continue;

			long realNum = costItem.getCount() * _repairNum * (10000 - costProperty) / 10000;
			//计算单兵实力
			if(RefGeneral.Ref().repair_power_basic > 0 && pureSoldierPower > 0)
			{
				//根据队伍维修实力计算
				realNum = realNum * pureSoldierPower / RefGeneral.Ref().repair_power_basic;
			}

			realNum = Math.max(realNum, 1);

			NPCommonCostItem realCostItem = costItem.duplicate();
			realCostItem.setCount(realNum);
			realCostItemList.add(realCostItem);
		}

		return realCostItemList;
	}

	@Override
	public String toString()
	{
		getUserData().lockUser();
		
		try
		{
			StringBuilder sb = new StringBuilder();
			sb.append("\nteamId:").append(getTeamId());
			sb.append("\nUnlock:").append(isUnlock());
			sb.append("\nstatus:").append(getStateMachine().getCurState().getStateType());
			
			sb.append("\nheroSize:").append(_m_llHeroListObj.getValueList().size());
			sb.append("\nheroIdList:").append(CommonFunc.list2String(_m_llHeroListObj.getValueList()));
			
			return sb.toString();
		}
    	finally
    	{
    		getUserData().unlockUser();
    	}
	}
}


