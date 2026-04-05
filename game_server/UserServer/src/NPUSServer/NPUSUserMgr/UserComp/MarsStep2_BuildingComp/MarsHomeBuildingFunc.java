package NPUSServer.NPUSUserMgr.UserComp.MarsStep2_BuildingComp;

import ALBasicCommon.ALBasicCommonFun;
import Common.MarsObj.Mars_HomeBuilding;
import NPCommon.DB._ASelectCallback;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CallBack._ICallBackBool;
import NPGameRes.Refs.Mars.RefMarsBuildingHomeLevel;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.SpecialItemComponent.Dealer.SpecialItemDealer_MarsEnergy;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_039_MarsBuildingOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerMarsBuildingHomeFuncBO;

/**
 * 主基地数据
 * @author mj
 *
 */
public class MarsHomeBuildingFunc extends _AMarsBuildingFunc
{
	//主基地等级配置
	private RefMarsBuildingHomeLevel _m_refHomeLevel;
	
	//功率开启标志
	private boolean _m_bIsNormalOn;
	private boolean _m_bIsOverdriveOn;
	private int _m_iLastCollectTimeS;
	//数据
	private PlayerMarsBuildingHomeFuncBO _m_boHome;

	public MarsHomeBuildingFunc(MarsBuildingInfo _info) 
	{
		super(_info);
	}
	
	public RefMarsBuildingHomeLevel getHomeLvlRef() {return _m_refHomeLevel;}
	
	public boolean isNormalOn() {return _m_bIsNormalOn;}
	public boolean isOverdriveOn() {return _m_bIsOverdriveOn;}
	public int lastCollectTimeS() {return _m_iLastCollectTimeS;}
	
	public PlayerMarsBuildingHomeFuncBO getHomeBo() {return _m_boHome;}
	
	public SpecialItemDealer_MarsEnergy getMarsEnergyDealer() {return getUserData().getMarsBuildingComponent().getMarsEnergyDealer();}
	
    /**
     * 加载主基地数据
     * @param _handler
     */
    protected void _initFromDB(_ICallBackBool _handler)
    {
		getUSServer().getBM().getBM(PlayerMarsBuildingHomeFuncBO.class).findOne("cid", getCid(), 
				new _ASelectCallback<PlayerMarsBuildingHomeFuncBO>() 
		{
			@Override
			public void dealSuc(PlayerMarsBuildingHomeFuncBO _bo) 
			{	
				_m_boHome = _bo;
				
				_m_bIsNormalOn = _bo.getIsNormalOn();
				_m_bIsOverdriveOn = _bo.getIsOverdriveOn();
				_m_iLastCollectTimeS = _bo.getLastCollectTimeS();
				
				_handler.onRunOver(true);
			}

			@Override
			public void dealFail() 
			{
				if(getHasErr())
				{
					_handler.onRunOver(false);
					return;
				}
				
				_handler.onRunOver(true);
			}
		});
	}
	
	/**
	 * 更新数据
	 */
	private void _save()
	{
		if(null == _m_boHome)
		{
			PlayerMarsBuildingHomeFuncBO bo = new PlayerMarsBuildingHomeFuncBO();
			bo.setCid(getBM(), getCid());
			bo.setIsNormalOn(getBM(), _m_bIsNormalOn);
			bo.setIsOverdriveOn(getBM(), _m_bIsOverdriveOn);
			bo.setLastCollectTimeS(getBM(), _m_iLastCollectTimeS);
			bo.insert(getBM());
			
			_m_boHome = bo;
		}
		else
		{
			_m_boHome.setIsNormalOn(getBM(), _m_bIsNormalOn);
			_m_boHome.setIsOverdriveOn(getBM(), _m_bIsOverdriveOn);
			_m_boHome.setLastCollectTimeS(getBM(), _m_iLastCollectTimeS);
			_m_boHome.saveAll(getBM());
		}
	}

	@Override
	protected void _onBuildingLoad() 
	{
		if(getBuildingLvl() > 0)
		{
			_m_refHomeLevel = RefMarsBuildingHomeLevel.getMgr().get(getBuildingLvl());
			if(null == _m_refHomeLevel)
			{
				USLog.error(getUSServer(), "player:{} building:{} lvl:{} mars building init home bo lvl ref fail, not find ref.", 
						getCid(), getBuildingId(), getBuildingLvl());
			}
			else if(_m_iLastCollectTimeS <= 0)
			{
				//数据错误做基本修正
				_m_iLastCollectTimeS = ALBasicCommonFun.getNowTime() - _m_refHomeLevel.build_output_bonus_s;
				_save();
			}

			//计算主基地属性加成数据
	    	getBuildingInfo().getBuildingValue().updateHomeOxygenYield(getOxygenYield(null));
			//更新能量消耗速度
	    	getBuildingInfo().getBuildingValue().updateHomeEnergyConsumePerMin(getConsumePerMin());
		}
	}

	@Override
	protected void _onInited() 
	{
	}
	
	@Override
	protected Result _checkUpdateSub(int _tarLvl) 
	{
		RefMarsBuildingHomeLevel homeLvlRef = RefMarsBuildingHomeLevel.getMgr().get(_tarLvl);
		if(null == homeLvlRef)
			return CommErr.REF_NOT_FOUND;
		
		return Result.SUCC;
	}

	@Override
	protected void _doneSub(NPPlayerContext _context) 
	{
		//更新等级配置
		_m_refHomeLevel = RefMarsBuildingHomeLevel.getMgr().get(getBuildingLvl());
		if(null == _m_refHomeLevel)
		{
			USLog.error(getUSServer(), "player:{} building:{} lvl:{} mars building home update lvl not find ref.", getCid(), getBuildingId(), getBuildingLvl());
			return;
		}
		
		//创建完成，默认开启普通功率
		if(1 == getBuildingLvl())
		{
			_m_bIsNormalOn = false;
			_m_bIsOverdriveOn = false;
			
			//初始化玩家收集资源时间
			//GOB-8323 主基地每次升级，默认提供火星币产出
			//https://www.teambition.com/task/695a7da72626e1eef2ff84c7
			_m_iLastCollectTimeS = ALBasicCommonFun.getNowTime() - _m_refHomeLevel.build_output_bonus_s;
			//强制推送一次收集时间变革
			getUserData().sendMsgToGC(US2GCWriter_039_MarsBuildingOp.make_009_RetHomeCollectTimeChg(_m_iLastCollectTimeS));
			//更新数据
			_save();
		}
		else
		{
			//强制收集一次资源
			NPPlayerContext context = NPPlayerContext.createNew(_context);
			collectOutput(context);

			//GOB-8323 主基地每次升级，默认提供火星币产出
			//https://www.teambition.com/task/695a7da72626e1eef2ff84c7
			_m_iLastCollectTimeS = _m_iLastCollectTimeS - _m_refHomeLevel.build_output_bonus_s;
			//强制推送一次收集时间变革
			getUserData().sendMsgToGC(US2GCWriter_039_MarsBuildingOp.make_009_RetHomeCollectTimeChg(_m_iLastCollectTimeS));
			//更新数据
			_save();
		}
		
    	//同步更新探索等级
		getBuildingInfo().getUserData().getMarsExploreComponent().getExploreInfo().setLvl(getBuildingLvl(), _context);

		//计算主基地属性加成数据
    	getBuildingInfo().getBuildingValue().updateHomeOxygenYield(getOxygenYield(null));
		//更新能量消耗速度
    	getBuildingInfo().getBuildingValue().updateHomeEnergyConsumePerMin(getConsumePerMin());
	}

	/**
	 * 获取当前的消耗值
	 * @return
	 */
	public int getConsumePerMin()
	{
		_lock();
		
		try
		{
			if(null == _m_refHomeLevel)
				return 0;
			
			return _m_refHomeLevel.getConsumePerMin(_m_bIsNormalOn, _m_bIsOverdriveOn);
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 获取当前氧气值
	 * @return
	 */
	public int getOxygenYield(StringBuilder _sb)
	{
		_lock();
		
		try
		{
			if(null != _sb)
			{
				_sb.append("\n------- cal oxygenYield ---------------");
			}
			
			if(null == _m_refHomeLevel)
			{
				if(null != _sb)
				{
					_sb.append("\noxygenYield:").append("0");
				}
				return 0;
			}
			
			int oxygenYield = _m_refHomeLevel.getOxygenYield(_m_bIsNormalOn, _m_bIsOverdriveOn);
			if(null != _sb)
			{
				_sb.append("\noxygenYieldConfig:").append(oxygenYield);
			}
			
			int value = oxygenYield * _m_refHomeLevel.oxygen_yield_adjust_coefficient;
			if(null != _sb)
			{
				_sb.append("\noxygenYield:").append(value);
			}
			
			return value;
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 设置开关功率关闭
	 * @param _context
	 */
	public void setPowerOff(NPPlayerContext _context)
	{
		_lock();
		
		try
		{
			_m_bIsNormalOn = false;
			_m_bIsOverdriveOn = false;
			_save();

			//计算主基地属性加成数据
	    	getBuildingInfo().getBuildingValue().updateHomeOxygenYield(getOxygenYield(null));
			//更新能量消耗速度
	    	getBuildingInfo().getBuildingValue().updateHomeEnergyConsumePerMin(getConsumePerMin());
		}
		finally
		{
			_unlock();
		}
	}

	/**
	 * 收集奖励
	 * @param _context
	 */
	public void collectOutput(NPPlayerContext _context)
	{
		_lock();

		try
		{
			if(null == _m_refHomeLevel)
				return ;

			if(null == _context)
			{
				USLog.error(getUSServer(), "player:{} building:{} mars home collect output fail, context is null.", getCid(), getBuildingId());
				//返回时间更新协议
				getUserData().sendMsgToGC(US2GCWriter_039_MarsBuildingOp.make_009_RetHomeCollectTimeChg(_m_iLastCollectTimeS));
				return ;
			}

			//计算可收集时间
			int nowTimeS = ALBasicCommonFun.getNowTime();

			//计算举例上次收集的时间
			int deltaTimeS = nowTimeS - _m_iLastCollectTimeS;
			//小于一分钟不做处理
			if(deltaTimeS < 60)
				return ;

			//计算是否超过时间上限
			if(deltaTimeS > RefGeneral.Ref().mars_home_output_remain_max_time_s)
			{
				deltaTimeS = RefGeneral.Ref().mars_home_output_remain_max_time_s;

				//超出上限，则之间将当前时间设置为最后收集时间
				_m_iLastCollectTimeS = nowTimeS;
			}
			else
			{
				//计算60秒的整数时间
				deltaTimeS = (deltaTimeS / 60) * 60;
				//将最后收集时间累积到增加的时间上
				_m_iLastCollectTimeS += deltaTimeS;
			}

			//保存数据
			_save();

			//根据收集时间计算奖励
			int outputCount = (deltaTimeS / 60);
			getUserData().gainItemList(_m_refHomeLevel.output_per_min, outputCount, false, _context);

			//返回时间变更协议
			getUserData().sendMsgToGC(US2GCWriter_039_MarsBuildingOp.make_009_RetHomeCollectTimeChg(_m_iLastCollectTimeS));
			//返回物品获得协议
			getUserData().sendMsgToGC(_context.getCollector().toProto());
		}
		finally {
			_unlock();
		}
	}
	
	/**
	 * 构造协议数据
	 * @return
	 */
	public Mars_HomeBuilding toHomeBuildingProto()
	{
		Mars_HomeBuilding proto = new Mars_HomeBuilding();
		proto.setBuildingId(getBuildingId());
		proto.setIsNormalOn(_m_bIsNormalOn);
		proto.setIsOverdriveOn(_m_bIsOverdriveOn);
		proto.setLastCollectTimeS(_m_iLastCollectTimeS);
		
		return proto;
	}

	/**
	 * 设置开关
	 * @param _isNormalOn
	 * @param _isOverdriveOn
	 * @param _context
	 * @return
	 */
	public Result setPowerOn(boolean _isNormalOn, boolean _isOverdriveOn, NPPlayerContext _context)
	{
		_lock();
		
		try
		{
			if(!getBuildingInfo().isUnlock())
				return MarsErr.MARS_BUILDING_NOT_BUILD;
			
			if(null == _m_refHomeLevel)
				return CommErr.REF_NOT_FOUND;
			
			//更新数据
			_m_bIsNormalOn = _isNormalOn;
			_m_bIsOverdriveOn = _isOverdriveOn;
			_save();
			
			//推送协议
			getUserData().sendMsgToGC(US2GCWriter_039_MarsBuildingOp.make_051_OnHomeChg(this));

			//计算主基地属性加成数据
	    	getBuildingInfo().getBuildingValue().updateHomeOxygenYield(getOxygenYield(null));
			//更新能量消耗速度
	    	getBuildingInfo().getBuildingValue().updateHomeEnergyConsumePerMin(getConsumePerMin());
	    	
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
			sb.append("\ntype:home")
				.append("\nisNormalOn:").append(_m_bIsNormalOn)
				.append("\nisOverdriveOn:").append(_m_bIsOverdriveOn);
			
			sb.append("\noxygenYield:");
			getOxygenYield(sb);
			
			return sb.toString();
		}
		finally
		{
			_unlock();
		}
	}
}
