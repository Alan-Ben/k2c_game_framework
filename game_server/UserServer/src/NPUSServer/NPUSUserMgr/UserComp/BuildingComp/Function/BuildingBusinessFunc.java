package NPUSServer.NPUSUserMgr.UserComp.BuildingComp.Function;

import Common.BuildingEnum.EBuildingFuncEnum;
import Common.BuildingObj.Building_Business;
import CommonEnum.EBonusFilterType;
import CommonEnum.EBonusPropertyType;
import GS2GC.p010_BuildingOp.GS2GC_010_054_OnBusinessBuilt;
import GS2GC.p010_BuildingOp.GS2GC_010_057_OnBusinessUnlockProduct;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.BuildingErr;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPGameRes.Refs.Building.RefBusinessBuilding;
import NPGameRes.Refs.Building.RefBusinessBuildingDevelop;
import NPGameRes.Refs.Building.RefBusinessBuildingLevel;
import NPGameRes.Refs.Building.RefBusinessBuildingProduct;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.BuildingInfo;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_010_BuildingOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerBuildingBusinessBO;
import USDB.Bo.PlayerBuildingBusinessProductBO;

import java.util.ArrayList;
import java.util.List;

public class BuildingBusinessFunc extends _ABuildingFunc
{
	//雇员数量
	private int _m_iEmployeeCount;

	//配表数据
	private RefBusinessBuilding _m_refBusinessBuilding;//当前经营建筑配表
	private RefBusinessBuildingLevel _m_refBusinessBuildingLevel;//当前经营建筑等级配表

	private List<BuildingBusinessProduct> _m_productList;
	private List<BuildingBusinessDevelop> _m_developList;

	//速度
	private long _m_speed;
	
	//bo数据
	private PlayerBuildingBusinessBO _m_boBusiness;

	protected BuildingBusinessFunc(BuildingInfo _buildingInfo, RefBusinessBuilding _ref, RefBusinessBuildingLevel _lvlRef)
	{
		super(_buildingInfo);

		_m_iEmployeeCount = 1;//默认1个雇佣员工

		_m_refBusinessBuilding = _ref;
		_m_refBusinessBuildingLevel = _lvlRef;

		_m_productList = new ArrayList<>();
		_m_developList = new ArrayList<>();

		//检查解锁经营发展
		checkUnlockDevelop();
	}

	public long getSpeed()
	{
		return _m_speed;
	}

	public void setSpeed(long _speed)
	{
		_m_speed = _speed;
	}

	public NPUSUserData getUserData()
	{
		return getBuildingInfo().getUserData();
	}

	public int getEmployeeCount()
	{
		return _m_iEmployeeCount;
	}

	public RefBusinessBuilding getRef()
	{
		return _m_refBusinessBuilding;
	}

	public long getBuildingId()
	{
		return _m_refBusinessBuilding.building_id;
	}

	public RefBusinessBuildingLevel getLvlRef()
	{
		return _m_refBusinessBuildingLevel;
	}

	public int getLvl()
	{
		return _m_refBusinessBuildingLevel.level;
	}


	public PlayerBuildingBusinessBO getBo()
	{
		return _m_boBusiness;
	}

	@Override
	public EBuildingFuncEnum getFuncEnum() 
	{
		return EBuildingFuncEnum.BUSINESS;
	}

	/**
	 * 计算产出速度
	 * @return
	 */
	public long calOutputSpeed(StringBuilder _sb)
	{
		if (_sb != null)
			_sb.append("产出速度万分比加成：\n");
		//店铺升级加成（万分比）
		int upgradeAddPer = getLvlRef().earning_rate;
		if (_sb != null)
			_sb.append(" 店铺升级加成：").append(upgradeAddPer).append("\n");
		//农田加成（万分比）
		int farmAddPer = getBuildingInfo().getComp().getTotalEarningRate();
		if (_sb != null)
			_sb.append(" 农田加成：").append(farmAddPer).append("\n");
		//伙伴驻扎加成（万分比）
		int heroPlaceAddPer = (int) getBuildingInfo().getBonusPropertyContainer().getValue(EBonusPropertyType.BUILDING_PROFIT_ADD_PER);
		if (_sb != null)
			_sb.append(" 伙伴驻扎加成：").append(heroPlaceAddPer).append("\n");
		//外部属性加成（万分比）
		int bonusAddPer = (int) getPlayerBonusAddValue(EBonusPropertyType.BUILDING_PROFIT_ADD_PER);
		if (_sb != null)
			_sb.append(" bonus属性加成：").append(bonusAddPer).append("\n");
		//联盟属性加成（万分比）
		int guildAddPer = getUserData().getGuildComponent().getBuildingAddPer(_m_refBusinessBuilding.attr_type);
		if (_sb != null)
			_sb.append(" 联盟属性加成：").append(guildAddPer).append("\n");
		//爬塔关卡加成（万分比）
		int towerAddPer = getUserData().getTowerComponent().getBuildingProfitAddPer();
		if (_sb != null)
			_sb.append(" 爬塔关卡属性加成：").append(towerAddPer).append("\n");
		//总加成数值=店铺升级加成+农田加成+伙伴驻扎加成+外部属性加成
		int profitAddPer = upgradeAddPer + farmAddPer + heroPlaceAddPer + bonusAddPer + guildAddPer + towerAddPer;
		if (_sb != null)
			_sb.append("总万分比加成数值：").append(profitAddPer).append("\n");

		//单个建筑收益=[(伙伴总实力/1000)+员工基础收益]*(1+收益百分比加成)，结果向上取整

		//**员工基础收益=建筑员工数量*每个员工收益
		long singleWorkerProfit = (long) getRef().employee_earnings + getPlayerBonusAddValue(EBonusPropertyType.BUILDING_EMPLOYEE_PROFIT_ADD);
		if (_sb != null)
			_sb.append("单个员工收益：").append(singleWorkerProfit).append("\n");
		long baseWorkerProfit = singleWorkerProfit * _m_iEmployeeCount;
		if (_sb != null)
			_sb.append("员工基础收益：").append(baseWorkerProfit).append("\n");
		//**伙伴总实力=所有伙伴实力之和
		long totalPower = getUserdata().getHeroComponent().getTotalPower();
		if (_sb != null)
			_sb.append("伙伴总实力：").append(totalPower).append("\n");
		//总收益数值=员工建筑总收益+伙伴经营总收益
		long outputBS = (long) Math.ceil((((double) totalPower /1000) + baseWorkerProfit) * (10000 + profitAddPer) / 10000) + getPlayerBonusAddValue(EBonusPropertyType.BONUS);
		if (_sb != null)
			_sb.append("总收益数值：").append(outputBS).append("\n");

		return outputBS;
	}

	/**
	 * 获取外部属性加成
	 * @param _propertyType
	 * @return
	 */
	public long getPlayerBonusAddValue(EBonusPropertyType _propertyType)
	{
		//获取玩家身上加成
		long playerPowerAddition = getUserdata().getBonusMgr().getTotalPropertyBonus(_propertyType);

		//建筑有对应的加成
		playerPowerAddition += getUserdata().getBonusMgr().getFilterPropertyBonus(_propertyType, EBonusFilterType.BUILDING_ID, getBuildingInfo().getBuildingId());

		//经营建筑有对应的相性加成
		playerPowerAddition += getUserdata().getBonusMgr().getFilterPropertyBonus(_propertyType, EBonusFilterType.BUILDING_ATTR,
				getBuildingInfo().getBusiness().getRef().attr_type.ordinal());

		return playerPowerAddition;
	}

	@Override
	public void onBuilt()
	{
		GS2GC_010_054_OnBusinessBuilt proto = new GS2GC_010_054_OnBusinessBuilt();
		proto.setBusiness(toProto());
		getBuildingInfo().getUserData().sendMsgToGC(proto);
	}

	@Override
	public void _initCalFunc(boolean _isInit)
	{
		long oriSpeed = _m_speed;
		_m_speed = calOutputSpeed(null);

		//替换玩家产出速度
		getBuildingInfo().getComp().replaceEarnings(oriSpeed, _m_speed, _isInit);
	}

	/***
	 * 初始化BO数据
	 * 
	 * @param _bo
	 * @param _lvlRef
	 */
    public void initBo(PlayerBuildingBusinessBO _bo, RefBusinessBuildingLevel _lvlRef)
	{
		_m_boBusiness = _bo;
		_m_iEmployeeCount = _m_boBusiness.getEmployeeCount();
		
		_m_refBusinessBuildingLevel = _lvlRef;

		//检查解锁经营发展
		checkUnlockDevelop();

		_m_speed = calOutputSpeed(null);
		//替换玩家产出速度
		getBuildingInfo().getComp().replaceEarnings(0, _m_speed, true);
	}

	/**
	 * 初始化产品BO数据
	 * @param _bo
	 * @param _refProduct
	 */
	public void initProductBo(PlayerBuildingBusinessProductBO _bo, RefBusinessBuildingProduct _refProduct)
	{
		_m_productList.add(new BuildingBusinessProduct(_refProduct, _bo));
		getUserData().getBonusMgr().addBonus(_refProduct.add_bonus);
	}
	
	/*********
	 * 构造数据协议
	 * @return
	 */
	public Building_Business toProto()
	{
		getUserData().lockUser();
		
		try
		{
			Building_Business proto = new Building_Business();
			proto.setBuildingId(getBuildingId());
			proto.setLvl(getLvl());
			proto.setEmployeeCount(_m_iEmployeeCount);
			for (BuildingBusinessProduct product : _m_productList)
			{
				proto.getProductList().add(product.getRefId());
			}
			return proto;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**************
	 * 计算获取最大雇佣人数 = 建筑最大允许雇佣人数 + 等级 * 每级所提供的额外员工容量
	 * 
	 * @return
	 */
	public int getMaxEmployeeCount()
	{
		return _m_refBusinessBuilding.employee_base_max_count
				+ (getLvl() - 1) * _m_refBusinessBuilding.addition_employee_count_per_level;
	}
	
	/**************
	 * 检查是否到达最大雇佣人数
	 *  
	 * @return
	 */
	public boolean checkHireEmployeeCount()
	{
		return getMaxEmployeeCount() >= _m_iEmployeeCount;
	}
	
	/*************
	 * 设置建筑等级
	 * 
	 * @param _lvlRef
	 * @param _context
	 */
	public void setLvl(RefBusinessBuildingLevel _lvlRef, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			if(getLvl() == _lvlRef.level)
				return;
			
			//更新等级数据
			_m_refBusinessBuildingLevel = _lvlRef;
			//保存数据
			_save();

			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_010_BuildingOp.make_052_OnBusinessChg(this));

			//检查解锁经营发展
			checkUnlockDevelop();

			//发起计算
			getBuildingInfo().calOutputSpeed();
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

	/**
	 * 增加雇佣人数
	 * @param _count
	 * @param _context
	 */
	public void addEmployeeCount(int _count, NPPlayerContext _context)
	{
		getUserData().lockUser();
		try
		{
			setEmployeeCount(_m_iEmployeeCount + _count, _context);
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/***************
	 * 设置雇佣人数
	 * 
	 * @param _count
	 * @param _context
	 */
	public void setEmployeeCount(int _count, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			if(_m_iEmployeeCount == _count)
				return;
	
			//设置雇佣数量
			_m_iEmployeeCount = _count;
			//保存数据
			_save();

			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_010_BuildingOp.make_052_OnBusinessChg(this));
			
			//发起计算
			getBuildingInfo().calOutputSpeed();
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

	/********************
	 * 保存数据
	 */
	private void _save()
	{
		BM bmObj = getUserData().getUSServer().getBM();
		
		if(null == _m_boBusiness)
		{
			PlayerBuildingBusinessBO bo = new PlayerBuildingBusinessBO();
			bo.setCid(bmObj, getUserData().getCid());
			bo.setBuildingId(bmObj, getBuildingId());
			bo.setLvl(bmObj, getLvl());
			bo.setEmployeeCount(bmObj, _m_iEmployeeCount);
			bo.insert(bmObj);
			
			_m_boBusiness = bo;
		}
		else
		{
			_m_boBusiness.setLvl(bmObj, getLvl());
			_m_boBusiness.setEmployeeCount(bmObj, _m_iEmployeeCount);
			_m_boBusiness.saveAll(bmObj);
		}
	}

	/************************
	 * 检查创建农场建筑
	 *
	 * @param _buildingInfo
	 * @return
	 */
	public static BuildingBusinessFunc checkBuild(BuildingInfo _buildingInfo)
	{
		RefBusinessBuilding ref = RefBusinessBuilding.getMgr().get(_buildingInfo.getBuildingId());
		if (null == ref)
			return null;

		RefBusinessBuildingLevel lvlRef = ref.getLevelMapMgr().getLevelData(1);
		if (null == lvlRef)
		{
			USLog.error(_buildingInfo.getUserData().getUSServer(), "player:{} building:{} building lvl:{} fail, not find lvl ref.",
					_buildingInfo.getUserData().getCid(), _buildingInfo.getBuildingId(), 1);
			return null;
		}

		//构造经营建筑内存数据
        return new BuildingBusinessFunc(_buildingInfo, ref, lvlRef);
	}

	/**
	 * 获取可以放置的大臣数量
	 * @return
	 */
	public int getCanPlaceHeroNum()
	{
		int num = 0;
		for (Integer needEmployeeNum : _m_refBusinessBuilding.hero_slot_employee_num_list)
		{
			if (needEmployeeNum <= _m_iEmployeeCount)
			{
				num++;
			}else
			{
				break;
			}
		}
		return num;
	}

	/**
	 * 检查解锁经营发展
	 */
	public void checkUnlockDevelop()
	{
		//遍历检查
		getUserData().lockUser();
		try
		{
		    for (RefBusinessBuildingDevelop refDevelop : _m_refBusinessBuilding.getDevelopList())
    		{
    			if (hasUnlockDevelop(refDevelop))
    				continue;

				if (refDevelop.level_required > getLvl())
					continue;

    			_m_developList.add(new BuildingBusinessDevelop(refDevelop));

    			//添加到全局加成管理器
    			getUserData().getBonusMgr().addBonus(refDevelop.add_bonus);
    		}
		} finally
		{
		    getUserData().unlockUser();
		}
	}

	/**
	 * 是否已经解锁经营发展
	 * @param _refDevelop
	 * @return
	 */
	public boolean hasUnlockDevelop(RefBusinessBuildingDevelop _refDevelop)
	{
		getUserData().lockUser();
		try
		{
		    for (BuildingBusinessDevelop develop : _m_developList)
    		{
    			if (develop.getRefId() == _refDevelop.Id())
    			{
    				return true;
    			}
    		}
    		return false;
		} finally
		{
		    getUserData().unlockUser();
		}
	}

	/**
	 * 解锁经营产品
	 */
	public Result unlockProduct(long _productId)
	{
		getUserData().lockUser();
		try
		{
			RefBusinessBuildingProduct refProduct = RefBusinessBuildingProduct.getMgr().get(_productId);
			if (refProduct == null)
				return CommErr.REF_NOT_FOUND;

			//检查经营发展是否解锁
			if (hasUnlockProduct(_productId))
				return BuildingErr.BUSINESS_PRODUCT_ALREADY_UNLOCK;

			//检查是否达到要求
			if (refProduct.employee_required > getEmployeeCount())
				return BuildingErr.BUSINESS_PRODUCT_UNLOCK_REQUIRE_NOT_REACH;

			PlayerBuildingBusinessProductBO bo = new PlayerBuildingBusinessProductBO();
			bo.setCid(getUserdata().getUSServer().getBM(), getUserData().getCid());
			bo.setProductId(getUserdata().getUSServer().getBM(), _productId);
			bo.insert(getUserdata().getUSServer().getBM());

			_m_productList.add(new BuildingBusinessProduct(refProduct, bo));

			//添加到全局加成管理器
			getUserData().getBonusMgr().addBonus(refProduct.add_bonus);

			getUserData().sendMsgToGC(new GS2GC_010_057_OnBusinessUnlockProduct(_productId));
		} finally
		{
		    getUserData().unlockUser();
		}

		return Result.SUCC;
	}

	/**
	 * 是否已经解锁经营商品
	 * @param _productId
	 * @return
	 */
	public boolean hasUnlockProduct(long _productId)
	{
		getUserData().lockUser();
		try
		{
		    for (BuildingBusinessProduct product : _m_productList)
    		{
    			if (product.getRefId() == _productId)
    			{
    				return true;
    			}
    		}
    		return false;
		} finally
		{
		    getUserData().unlockUser();
		}
	}
}
