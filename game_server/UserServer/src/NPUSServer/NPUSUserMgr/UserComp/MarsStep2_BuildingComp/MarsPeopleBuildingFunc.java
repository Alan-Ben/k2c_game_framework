package NPUSServer.NPUSUserMgr.UserComp.MarsStep2_BuildingComp;

import Common.MarsObj.Mars_BuildingEnergyOutput;
import Common.MarsObj.Mars_PeopleBuilding;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.LazyTaskDealer.LazyTaskDealer;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.Mars.RefMarsBuildingSettleLevel;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_039_MarsBuildingOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerMarsPeopleFuncBO;

/**
 * 居民建筑数据
 * @author mj
 *
 */
public class MarsPeopleBuildingFunc extends _AMarsBuildingFunc
{
	//建筑结算等级配置
	private RefMarsBuildingSettleLevel _m_refSettleLevel;
	
	//派遣居民数
	private int _m_iDispatchedNum;
	
	//当前累积的能量数值
	private long _m_lEnergy;
	//能量数值最后一次计算时间（毫秒）
	private long _m_lEnergyLastCalMs;
	
	//数据
	private PlayerMarsPeopleFuncBO _m_boPeople;

	//产出值每分钟
	private long _m_lOutputValuePerMin;
	//最大资源储存量
	private long _m_lMaxStorage;
	//建筑产出计算器
    private LazyTaskDealer _m_cdCalOutputLazyDealer;
	
	public MarsPeopleBuildingFunc(MarsBuildingInfo _info) 
	{
		super(_info);
		
		_m_cdCalOutputLazyDealer = new LazyTaskDealer(() -> calOutput(null), 200);
	}

	public RefMarsBuildingSettleLevel getSettleLvlRef() {return _m_refSettleLevel;}
	public int getDispatchedLimit() {return null == _m_refSettleLevel ? 0 : _m_refSettleLevel.slot_num * RefGeneral.Ref().mars_building_slot_people_count;}
	
	public PlayerMarsPeopleFuncBO getPeopleBo() {return _m_boPeople;}

	public int getDispatchedNum() {return _m_iDispatchedNum;}
    
    public long getEnergy() {return _m_lEnergy;}
    public long getEnergyLastCalMs() {return _m_lEnergyLastCalMs;}

    //计算能源产出
    public void doLazyCalOutput() {_m_cdCalOutputLazyDealer.setNeedDeal();}
	
	protected void _loadPeopleBo(PlayerMarsPeopleFuncBO _bo) 
	{
		_m_boPeople = _bo;
		
		_m_iDispatchedNum = _m_boPeople.getDispatchedNum();
		
		_m_lEnergy = _m_boPeople.getEnergy();
		_m_lEnergyLastCalMs = _m_boPeople.getEnergyLastCalMs();
	}
	
	/**
	 * 更新数据
	 */
	private void _save()
	{
		if(null == _m_boPeople)
		{
			PlayerMarsPeopleFuncBO bo = new PlayerMarsPeopleFuncBO();
			bo.setCid(getBM(), getCid());
			bo.setBuildingId(getBM(), getBuildingId());
			bo.setDispatchedNum(getBM(), _m_iDispatchedNum);
			bo.setEnergy(getBM(), _m_lEnergy);
			bo.setEnergyLastCalMs(getBM(), _m_lEnergyLastCalMs);
			bo.insert(getBM());
			
			_m_boPeople = bo;
		}
		else
		{
			_m_boPeople.setDispatchedNum(getBM(), _m_iDispatchedNum);
			_m_boPeople.setEnergy(getBM(), _m_lEnergy);
			_m_boPeople.setEnergyLastCalMs(getBM(), _m_lEnergyLastCalMs);
			_m_boPeople.saveAll(getBM());
		}
	}

	@Override
	protected void _onBuildingLoad() 
	{
		//检查相关等级
		if(getBuildingLvl() > 0)
		{
			_m_refSettleLevel = getBuildingInfo().getRef().getSettleLevelMapMgr().getLevelData(getBuildingLvl());
			if(null == _m_refSettleLevel)
			{
				USLog.error(getUSServer(), "player:{} building:{} lvl:{} mars building people update lvl not find ref.", getCid(), getBuildingId(), getBuildingLvl());
			}
		}
	}

	@Override
	protected void _onInited() 
	{
		//更新当前最新数据
		_m_lOutputValuePerMin = MarsBuildingValueCalculate.calEnergyOutputValuePerMin(this, null);
		_m_lMaxStorage = getBuildingInfo().getBuildingValue().getMaxStorage();	
	}

	@Override
	protected Result _checkUpdateSub(int _tarLvl) 
	{
		RefMarsBuildingSettleLevel settleLvlRef = getBuildingInfo().getRef().getSettleLevelMapMgr().getLevelData(_tarLvl);
		if(null == settleLvlRef)
			return CommErr.REF_NOT_FOUND;
		
		return Result.SUCC;
	}

	@Override
	protected void _doneSub(NPPlayerContext _context) 
	{
		_m_refSettleLevel = getBuildingInfo().getRef().getSettleLevelMapMgr().getLevelData(getBuildingLvl());
		if(null == _m_refSettleLevel)
		{
			USLog.error(getUSServer(), "player:{} building:{} lvl:{} mars building people update lvl not find ref.", getCid(), getBuildingId(), getBuildingLvl());
		}
		
		//计算一次产出
		calOutput(null);
	}
	
	/**
	 * 构造协议数据
	 * @return
	 */
	public Mars_PeopleBuilding toPeopleBuildingProto()
	{
		Mars_PeopleBuilding proto = new Mars_PeopleBuilding();
		proto.setBuildingId(getBuildingId());
		proto.setPeopleNum(getDispatchedNum());
		
		return proto;
	}
	
	/**
	 * 构造产出数据
	 * @return
	 */
	public Mars_BuildingEnergyOutput toOutputProto()
	{
		Mars_BuildingEnergyOutput proto = new Mars_BuildingEnergyOutput();
		proto.setBuildingId(getBuildingId());
		proto.setLastSettleMs(_m_lEnergyLastCalMs);
		proto.setOutput(_m_lEnergy);
		
		return proto;
	}

	/**
	 * 计算产出数据
	 * @param _context
	 */
	public void calOutput(StringBuilder _sb)
	{
		_lock();
		
		try
		{
			//尚未创建数据
			if(!getBuildingInfo().isBuilt())
				return;
			
			long nowTimeMs = CommonFunc.getNowTimeMS();
			//如果尚未满额，需要补足的时间
			long complementMs = 0;
			
			do
			{
				//尚未开启结算，不处理并更新最后一次时间
				if(0 == _m_lEnergyLastCalMs)
					break;
				
				//没有产出速度
				if(_m_lOutputValuePerMin <= 0)
					break;
				
				//检查容量是否超过限制，超过限制则更新最后一次计算时间
				long canStorage = _m_lMaxStorage - _m_lEnergy;
				if(canStorage <= 0)
					break;

				//计算可用时长
				long outputMs = nowTimeMs - _m_lEnergyLastCalMs;
				long outputMins = outputMs / 60000;
				//尚未满足单次生产周期时间
				if(outputMins <= 0)
				{
					//这部分差额时间需要返回给玩家
					complementMs = outputMs;
					break;
				}
				
				//计算当前产出
				long planOutputCount = _m_lEnergy + outputMins * _m_lOutputValuePerMin;
				//计算实际产出
				if(planOutputCount >= _m_lMaxStorage) //超过了最大储存值
				{
					_m_lEnergy = _m_lMaxStorage;
					complementMs = 0;
				}
				else
				{
					_m_lEnergy = planOutputCount;
					//计算差额
					complementMs = outputMs - outputMins * 60000;
				}
			}
			while(false);
			
			_m_lEnergyLastCalMs = nowTimeMs - complementMs;
			_save();
			
			//更新当前最新数据
			_m_lOutputValuePerMin = MarsBuildingValueCalculate.calEnergyOutputValuePerMin(this, _sb);
			_m_lMaxStorage = getBuildingInfo().getBuildingValue().getMaxStorage();
			
			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_039_MarsBuildingOp.make_054_OnEnergyOutputChg(this));
		}
		finally
		{
			_unlock();
		}
	}

	
	/**
	 * 结算能量产出
	 * @param _context
	 * @return
	 */
	public long settle(NPPlayerContext _context)
	{
		_lock();
		
		try
		{
			//计算产出
			calOutput(null);

			//当前无产能
			if(_m_lEnergy <= 0)
				return 0;
			
			long settleEnergy = _m_lEnergy;
			//清空产出能量值
			_m_lEnergy = 0;
			_save();
			
			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_039_MarsBuildingOp.make_054_OnEnergyOutputChg(this));
			
			return settleEnergy;
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 减少工作居民数量
	 * @param _num
	 * @param _isSick
	 * @param _context
	 */
	public void reducePeople(int _num, boolean _isSick, NPPlayerContext _context)
	{
		_lock();
		
		try
		{
			if(_m_iDispatchedNum <= 0)
				return;
			
			int preNum = _m_iDispatchedNum;
			//检查新居民数
			int newNum = Math.max(_m_iDispatchedNum - _num,  0);
			
			//更新人数
			_m_iDispatchedNum = newNum;
			_save();
			
			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_039_MarsBuildingOp.make_052_OnPeopleChg(this));

			//实际变更人数
			int realReduceNum = preNum - _m_iDispatchedNum;
			//进入生病状态
			if(_isSick)
			{
				getUserData().getMarsPeopleComponent().getNumInfo().incrSickNum(realReduceNum, _context);
			}
			
			//发起重新计算能量产出
			doLazyCalOutput();
			//触发重新计算健康/幸福指数
			getUserData().getMarsBuildingComponent().doLazyCalHealthIndex();
			getUserData().getMarsBuildingComponent().doLazyCalHappyIndex();
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 减少工作居民数（随机）
	 * @param _isSick
	 * @param _context
	 */
	public void reduceRndPeople(boolean _isSick, NPPlayerContext _context)
	{
		_lock();
		
		try
		{
			if(_m_iDispatchedNum <= 0)
				return;
			
			int rndNum = CommonFunc.randomInt(_m_iDispatchedNum);
			reducePeople(rndNum, _isSick, _context);
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 展示建筑产出
	 * @param _context
	 * @return
	 */
	public String showEnergyOutputValuePerMin(NPPlayerContext _context)
	{
		_lock();
		
		try
		{
			StringBuilder sb = new StringBuilder();
			
			calOutput(sb);
			
			sb.append("\nbuilding:").append(getBuildingId());
			sb.append("\nbuildingLvl:").append(getBuildingLvl());
			sb.append("\noutputValuePerMin:").append(getBuildingInfo().getBuildingValue().getOutputValuePerMin());
			sb.append("\nmaxStorage:").append(getBuildingInfo().getBuildingValue().getMaxStorage());
			
			return sb.toString();
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 派遣居民
	 * @param _peopleNum
	 * @param _context
	 * @return
	 */
	public Result dispatchPeople(int _peopleNum, NPPlayerContext _context)
	{
		_lock();
		
		try
		{
			if(!getBuildingInfo().isBuilt())
				return MarsErr.MARS_BUILDING_NOT_BUILD;
			
			//检查派遣人数上限
			int newDispatchNum = getDispatchedNum() + _peopleNum;
			if(newDispatchNum > getDispatchedLimit())
				return MarsErr.MARS_BUILDING_DISPATCH_LIMIT;
			
			//检查空闲人数
			if(_peopleNum > getUserData().getMarsPeopleComponent().getNumInfo().getIdleNum())
				return MarsErr.MARS_PEOPLE_IDLE_NOT_ENOUGH;
			
			//更新派遣人数
			_m_iDispatchedNum = newDispatchNum;
			_save();
			
			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_039_MarsBuildingOp.make_052_OnPeopleChg(this));

			//更新人数变化
			getUserData().getMarsPeopleComponent().getNumInfo().reduceIdleNum(_peopleNum, _context);

			//发起重新计算能量产出
			doLazyCalOutput();
			
			//发起计算派遣总数
			getUserData().getMarsBuildingComponent().doLazyCalDispatchedNumSumDealer();
			
			return Result.SUCC;
		}
		finally
		{
			_unlock();
		}
	}
	
	@Override
	public String toString()
	{
		_lock();
		
		try
		{
			StringBuilder sb = new StringBuilder();
			sb.append("\ntype:people")
				.append("\ndispatchedNum:").append(_m_iDispatchedNum)
				.append("\nenergy:").append(_m_lEnergy)
				.append("\nenergyLastCalMs:").append(_m_lEnergyLastCalMs);
			
			return sb.toString();
		}
		finally
		{
			_unlock();
		}
	}
}
