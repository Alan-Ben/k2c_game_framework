package NPUSServer.NPUSUserMgr.UserComp.BuildingComp;

import Common.BuildingEnum.EBuildingFuncEnum;
import CommonEnum.EBonusFilterType;
import CommonEnum.EBonusPropertyType;
import CommonEnum.ESpecAttrType;
import NPCommon.LazyTaskDealer.LazyTaskDealer;
import NPGameRes.GameObjs.PlayerBonusProperty.PlayerBonusPropertyContainer;
import NPGameRes.Refs.Building.RefBuilding;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.Function.BuildingBusinessFunc;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.Function.BuildingFarmFunc;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.Function._ABuildingFunc;
import NPUSServer.NPUSUserMgr.UserComp.Common.BuildingPropertyChgDealer;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroInfo;
import USDB.Bo.PlayerBuildingBO;

import java.util.ArrayList;
import java.util.List;

public class BuildingInfo implements _IBuildingConditionProxy
{
	//玩家数据
	private BuildingComponent _m_comp;

	//BO数据
	private long _m_lId;
	//配置数据
	private RefBuilding _m_refBuilding;
	
	//建筑功能对象（农田建筑/经营建筑）
	private _ABuildingFunc[] _m_arrBuildingFunc;

	//计算建筑产出速度的Dealer
	private LazyTaskDealer _m_ltCalOuptSpeed;
	//属性加成管理器
	private PlayerBonusPropertyContainer _m_bonusPropertyContainer;
	//驻扎大臣数据列表
	private List<HeroInfo> _m_heroList;

	protected BuildingInfo(BuildingComponent _comp, PlayerBuildingBO _bo, RefBuilding _ref)
	{
		_m_comp = _comp;

		_m_lId = _bo.getId();

		_m_refBuilding = _ref;

		_m_arrBuildingFunc = new _ABuildingFunc[EBuildingFuncEnum.values().length];

		_m_ltCalOuptSpeed = new LazyTaskDealer(new BuildingSpeedCalTask(this), 300);

		_m_bonusPropertyContainer = new PlayerBonusPropertyContainer();

		_m_heroList = new ArrayList<>();
	}

	public NPUSUserData getUserData() {return _m_comp.getUserData();}


	public BuildingComponent getComp()
	{
		return _m_comp;
	}

	public long getId() {return _m_lId;}
	
	public RefBuilding getRef() {return _m_refBuilding;}
	public long getBuildingId() {return _m_refBuilding.id;}

	@Override
	public ESpecAttrType getAttrType()
	{
		_ABuildingFunc buildingFunc = getBuildingFunc(EBuildingFuncEnum.BUSINESS);
		BuildingBusinessFunc businessFunc = buildingFunc instanceof BuildingBusinessFunc ? ((BuildingBusinessFunc) buildingFunc) : null;
		if (businessFunc == null)
			return null;

		return businessFunc.getRef().attr_type;
	}

	public PlayerBonusPropertyContainer getBonusPropertyContainer()
	{
		return _m_bonusPropertyContainer;
	}

	public void regBuildingFunc(_ABuildingFunc _funcObj)
	{
		_m_arrBuildingFunc[_funcObj.getFuncEnum().ordinal()] = _funcObj;
	}
	public _ABuildingFunc getBuildingFunc(EBuildingFuncEnum _type)
	{
		return _m_arrBuildingFunc[_type.ordinal()];
	}
	public _ABuildingFunc[] getBuildingFuncArr()
	{
		return _m_arrBuildingFunc;
	}
	
	public BuildingFarmFunc getFarm() {return (BuildingFarmFunc) getBuildingFunc(EBuildingFuncEnum.FARM);}
	public BuildingBusinessFunc getBusiness() {return (BuildingBusinessFunc) getBuildingFunc(EBuildingFuncEnum.BUSINESS);}

	/**
	 * 获得新建筑时调用初始化函数
	 * @param _context 上下文
	 */
	public void initNewBuilding(NPPlayerContext _context)
	{
		//检查建筑功能
		checkFunc(false);

		//注册属性容器的监听处理对象
		_m_bonusPropertyContainer.setOnPropertyChg(new BuildingPropertyChgDealer(this));
	}

	/**
	 * 计算赚速
	 */
	public void calOutputSpeed()
	{
		_m_ltCalOuptSpeed.setNeedDeal();
	}

	/**
	 * 放置大臣
	 * @param _heroInfo
	 */
	public void placeHero(HeroInfo _heroInfo)
	{
		getUserData().lockUser();
		try{
		    _m_heroList.add(_heroInfo);

		    //发起重新计算
		    calOutputSpeed();
		    
		}finally
		{
		    getUserData().unlockUser();
		}
	}

	/**
	 * 移除大臣
	 * @param _heroInfo
	 */
	public void removeHero(HeroInfo _heroInfo)
	{
		getUserData().lockUser();
		try{
		    _m_heroList.remove(_heroInfo);
		    
		    //发起重新计算
		    calOutputSpeed();
		    
		}finally
		{
		    getUserData().unlockUser();
		}
	}

	/**
	 * 初始化计算建筑产出
	 */
	public void _initCalBuilding()
	{
		for (_ABuildingFunc _func : _m_arrBuildingFunc)
		{
			if (_func == null)
				continue;

			_func._initCalFunc(true);
		}

		//注册属性容器的监听处理对象
		_m_bonusPropertyContainer.setOnPropertyChg(new BuildingPropertyChgDealer(this));
	}

	/**
	 * 是否满员
	 * @return
	 */
	public boolean isFullHero()
	{
		getUserData().lockUser();
		try{
			return _m_heroList.size() >= getCanPlaceHeroNum();
		}finally
		{
			getUserData().unlockUser();
		}
	}

	/**
	 * 获取可放置大臣数量
	 * @return
	 */
	private int getCanPlaceHeroNum()
	{
		_ABuildingFunc buildingFunc = getBuildingFunc(EBuildingFuncEnum.BUSINESS);
		BuildingBusinessFunc businessFunc = buildingFunc instanceof BuildingBusinessFunc ? ((BuildingBusinessFunc) buildingFunc) : null;
		if (businessFunc == null)
			return 0;
		return businessFunc.getCanPlaceHeroNum();
	}

	/**
	 * 获取建筑相性
	 * @return
	 */
	public ESpecAttrType getAttr()
	{
		_ABuildingFunc buildingFunc = getBuildingFunc(EBuildingFuncEnum.BUSINESS);
		BuildingBusinessFunc businessFunc = buildingFunc instanceof BuildingBusinessFunc ? ((BuildingBusinessFunc) buildingFunc) : null;
		if (businessFunc == null)
			return ESpecAttrType.NONE;

		return businessFunc.getRef().attr_type;
	}

	/*************************
	 * 检查创建建筑对应功能
	 */
	protected void checkFunc(boolean _isInit)
	{
		for(int i = 0; i < _m_arrBuildingFunc.length; i++)
		{
			_ABuildingFunc funcObj = _m_arrBuildingFunc[i];
			if(null != funcObj)
				continue;

			EBuildingFuncEnum funcEnum = EBuildingFuncEnum.EBuildingFuncEnum_FromInt(i);
			if(EBuildingFuncEnum.FARM == funcEnum)
				funcObj = BuildingFarmFunc.checkBuild(this);
			else if(EBuildingFuncEnum.BUSINESS == funcEnum)
				funcObj = BuildingBusinessFunc.checkBuild(this);

			if (null != funcObj)
			{
				regBuildingFunc(funcObj);

				if (!_isInit)
				{
					funcObj.onBuilt();
					funcObj._initCalFunc(false);
				}
			}
		}
	}

	/**
	 * 获取建筑功能等级
	 * @param _buildingFuncEnum
	 * @return
	 */
	public int getBuildingFuncLevel(EBuildingFuncEnum _buildingFuncEnum)
	{
		_ABuildingFunc buildingFunc = getBuildingFunc(_buildingFuncEnum);

		if (_buildingFuncEnum == EBuildingFuncEnum.FARM)
		{
			BuildingFarmFunc buildingFarmFunc = buildingFunc instanceof BuildingFarmFunc ? ((BuildingFarmFunc) buildingFunc) : null;
			if (buildingFarmFunc == null)
				return 0;

			return buildingFarmFunc.getLevel();
		}else if (_buildingFuncEnum == EBuildingFuncEnum.BUSINESS)
		{
			BuildingBusinessFunc buildingBusinessFunc = buildingFunc instanceof BuildingBusinessFunc ? ((BuildingBusinessFunc) buildingFunc) : null;
			if (buildingBusinessFunc == null)
				return 0;

			return buildingBusinessFunc.getLvl();
		}

		return 0;
	}

	/**
	 * 获取建筑全局数据
	 * @param _type
	 * @return
	 */
	public long getBonusPropertyValue(EBonusPropertyType _type)
	{
		return getUserData().getBonusMgr().getTotalPropertyBonus(_type)
				+ getUserData().getBonusMgr().getFilterPropertyBonus(_type, EBonusFilterType.BUILDING_ATTR, getAttr().ordinal())
                + getUserData().getBonusMgr().getFilterPropertyBonus(_type, EBonusFilterType.BUILDING_ID, getBuildingId());
	}
}
