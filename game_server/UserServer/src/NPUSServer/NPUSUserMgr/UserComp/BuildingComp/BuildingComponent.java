package NPUSServer.NPUSUserMgr.UserComp.BuildingComp;

import ALBasicServer.ALProcess.ALProcess;
import Common.BuildingEnum.EBuildingFuncEnum;
import Common.BuildingObj.Building_Business;
import Common.BuildingObj.Building_Farm;
import NPCommon.CommonProcess._IEZProcessMonitor;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.NPLogDB.CommLogDB;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.Pair.WCGPair;
import NPCommon.Util.Random;
import NPEnum.*;
import NPGameRes.Refs.Building.RefBuilding;
import NPGameRes.Refs.Building.RefBusinessBuildingLevel;
import NPGameRes.Refs.Building.RefBusinessBuildingProduct;
import NPGameRes.Refs.Building.RefFarmingBuildingLevel;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ItemDealer._IUserItemBasicDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.Function.BuildingBusinessFunc;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.Function.BuildingFarmFunc;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.Function._ABuildingFunc;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_010_BuildingOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerBuildingBO;
import USDB.Bo.PlayerBuildingBusinessBO;
import USDB.Bo.PlayerBuildingBusinessProductBO;
import USDB.Bo.PlayerBuildingFarmBO;
import USLOGDB.Bo.LogBuildingBuildBO;
import USLOGDB.Bo.LogSectionBuildingV2BO;

import java.util.ArrayList;
import java.util.List;

public class BuildingComponent extends _ANPUserComponent implements _IUserItemBasicDealer
{
	//已建造的建筑ID列表
	private final ArrayList<BuildingInfo> _m_alBuildingInfoList;
	//农田总产速加成
	private int _m_totalEarningRate;
	//建筑总赚速
	private long _m_earnings;

	public BuildingComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.BUILDING);

		_m_alBuildingInfoList = new ArrayList<>();
		_m_totalEarningRate = 0;
    }

	@Override
	protected void _init()
	{
        ALProcess process = ALProcess.CreateProcess("building_component_init");
        //1.初始化已建造建筑ID数据
        process.addResDelegateProcess(_action -> _initBuilding(_action::dealAction),
                "init_building", null, false);
        //2.初始化农田建筑数据
        process.addResDelegateProcess(_action -> _initFarm(_action::dealAction),
                "init_building_farm", null, false);
        //3.初始化经营建筑数据
        process.addResDelegateProcess(_action -> _initBusiness(_action::dealAction),
                "init_building_business", null, false);
        //3.初始化经营建筑数据
        process.addResDelegateProcess(_action -> _initBusinessProduct(_action::dealAction),
                "init_building_business_product", null, false);

        //执行初始化逻辑
        process.dealProcess(new _IEZProcessMonitor()
        {
            @Override
            public void onRootProecssStop()
            {
            	USLog.error(getUSServer(), "player:{} init building fail.", getUserData().getCid());

                getUserData().setDataLoadFail();
            }

            @Override
            public void onRootProecssSuc()
            {
                setInited();
            }
        });
	}

	/******************
	 * 初始化建筑数据
	 *
	 * @param _callBack
	 */
	private void _initBuilding(final _ICallBackBool _callBack)
	{
		getUSServer().getBM().getBM(PlayerBuildingBO.class).findAll("cid", getUserData().getCid(),
				new _ASelectCallback<List<PlayerBuildingBO>>()
        {
            @Override
            public void dealSuc(List<PlayerBuildingBO> _boList)
            {
            	for(int i = 0; i < _boList.size(); i++)
            	{
            		PlayerBuildingBO bo = _boList.get(i);
            		if(null == bo)
            			continue;

            		RefBuilding ref = RefBuilding.getMgr().get(bo.getBuildingId());
            		if(null == ref)
            		{
            			continue;
            		}

            		BuildingInfo info = new BuildingInfo(BuildingComponent.this, bo, ref);
            		_m_alBuildingInfoList.add(info);

            		//检查构造建筑功能对象
            		info.checkFunc(true);
            	}

                _callBack.onRunOver(true);
            }

            @Override
            public void dealFail()
            {
            	USLog.error(getUSServer(), "player:{} init building bo fail.", getUserData().getCid());

                _callBack.onRunOver(false);
            }
        });
	}

	/****************
	 * 初始化农田建筑数据
	 *
	 * @param _callBack
	 */
	private void _initFarm(final _ICallBackBool _callBack)
	{
		getUSServer().getBM().getBM(PlayerBuildingFarmBO.class).findAll("cid", getUserData().getCid(),
				new _ASelectCallback<List<PlayerBuildingFarmBO>>()
        {
            @Override
            public void dealSuc(List<PlayerBuildingFarmBO> _boList)
            {
            	for(int i = 0; i < _boList.size(); i++)
            	{
            		PlayerBuildingFarmBO bo = _boList.get(i);
            		if(null == bo)
            			continue;

            		//检查建筑数据合法性
            		BuildingInfo info = lookupBuilding(bo.getBuildingId());
            		if(null == info)
            		{
            			USLog.error(getUSServer(), "player:{} init building:{} farm fail, not find building.",
								getUserData().getCid(), bo.getBuildingId());
            			continue;
            		}
            		if(null == info.getFarm())
            		{
            			USLog.error(getUSServer(), "player:{} init building:{} farm fail, building not have farm func.",
								getUserData().getCid(), bo.getBuildingId());
            			continue;
            		}

            		//检查 农田建筑 相关配表
            		RefFarmingBuildingLevel lvlRef = info.getFarm().getRef().getLevelMapMgr().getLevelData(bo.getLvl());
            		if(null == lvlRef)
            		{
            			USLog.error(getUSServer(), "player:{} init building:{} farm lvl:{} fail, not find ref.",
								getUserData().getCid(), bo.getBuildingId(), bo.getLvl());
            			continue;
            		}

            		//挂载数据
            		info.getFarm().initBo(bo, lvlRef);
            	}

                _callBack.onRunOver(true);
            }

            @Override
            public void dealFail()
            {
    			USLog.error(getUSServer(), "player:{} init building farm bo fail", getUserData().getCid());

                _callBack.onRunOver(false);
            }
        });
	}

	/****************
	 * 初始化经营建筑数据
	 *
	 * @param _callBack
	 */
	private void _initBusiness(final _ICallBackBool _callBack)
	{
		getUSServer().getBM().getBM(PlayerBuildingBusinessBO.class).findAll("cid", getUserData().getCid(),
				new _ASelectCallback<List<PlayerBuildingBusinessBO>>()
        {
            @Override
            public void dealSuc(List<PlayerBuildingBusinessBO> _boList)
            {
            	for(int i = 0; i < _boList.size(); i++)
            	{
            		PlayerBuildingBusinessBO bo = _boList.get(i);
            		if(null == bo)
            			continue;

            		//检查建筑数据合法性
            		BuildingInfo info = lookupBuilding(bo.getBuildingId());
            		if(null == info)
            		{
            			USLog.error(getUSServer(), "player:{} init building:{} business fail, not find building.",
								getUserData().getCid(), bo.getBuildingId());
            			continue;
            		}
            		if(null == info.getBusiness())
            		{
            			USLog.error(getUSServer(), "player:{} init building:{} business fail, building not have business func.",
								getUserData().getCid(), bo.getBuildingId());
            			continue;
            		}

            		//检查 经营建筑 相关配表
            		RefBusinessBuildingLevel lvlRef = info.getBusiness().getRef().getLevelMapMgr().getLevelData(bo.getLvl());
            		if(null == lvlRef)
            		{
            			USLog.error(getUSServer(), "player:{} init building:{} business lvl:{} fail, not find ref.",
								getUserData().getCid(), bo.getBuildingId(), bo.getLvl());
            			continue;
            		}

            		//挂载数据
            		info.getBusiness().initBo(bo, lvlRef);
            	}

                _callBack.onRunOver(true);
            }

            @Override
            public void dealFail()
            {
    			USLog.error(getUSServer(), "player:{} init building business bo fail", getUserData().getCid());

                _callBack.onRunOver(false);
            }
        });
	}

	/****************
	 * 初始化经营建筑数据
	 *
	 * @param _callBack
	 */
	private void _initBusinessProduct(final _ICallBackBool _callBack)
	{
		getUSServer().getBM().getBM(PlayerBuildingBusinessProductBO.class).findAll("cid", getUserData().getCid(),
				new _ASelectCallback<List<PlayerBuildingBusinessProductBO>>()
        {
            @Override
            public void dealSuc(List<PlayerBuildingBusinessProductBO> _boList)
            {
            	for(int i = 0; i < _boList.size(); i++)
            	{
					PlayerBuildingBusinessProductBO bo = _boList.get(i);
            		if(null == bo)
            			continue;

					RefBusinessBuildingProduct refProduct = RefBusinessBuildingProduct.getMgr().get(bo.getProductId());
					if(null == refProduct)
					{
						USLog.error(getUSServer(), "player:{} init building:{} product:{} fail, not find product ref.",
								getUserData().getCid(), bo.getProductId());
						continue;
					}

            		//检查建筑数据合法性
            		BuildingInfo info = lookupBuilding(refProduct.building_id);
            		if(null == info)
            		{
            			USLog.error(getUSServer(), "player:{} init building:{} business product fail, not find building.",
								getUserData().getCid(), refProduct.building_id);
            			continue;
            		}
            		if(null == info.getBusiness())
            		{
            			USLog.error(getUSServer(), "player:{} init building:{} business product fail, building not have business func.",
								getUserData().getCid(), refProduct.building_id);
            			continue;
            		}

            		//挂载数据
            		info.getBusiness().initProductBo(bo, refProduct);
            	}

                _callBack.onRunOver(true);
            }

            @Override
            public void dealFail()
            {
    			USLog.error(getUSServer(), "player:{} init building business product bo fail", getUserData().getCid());

                _callBack.onRunOver(false);
            }
        });
	}

	@Override
	public ENPPlayerCompType[] getDependCompList()
	{
		return null;
	}

	@Override
	public void onInited()
	{

	}

	@Override
	public void dispose()
	{
	}

	/**
	 * 获取赚速加成
	 * @return
	 */
	public int getTotalEarningRate()
	{
		getUserData().lockUser();
		try{
		    return _m_totalEarningRate;
		}finally
		{
			getUserData().unlockUser();
		}
	}

	/**
	 * 替换赚速加成
	 * @param _oriEarningRate
	 * @param _newEarningRate
	 */
	public void replaceEarningRate(int _oriEarningRate, int _newEarningRate)
	{
		getUserData().lockUser();
		try{
		    _m_totalEarningRate -= _oriEarningRate;
    		_m_totalEarningRate += _newEarningRate;

			//重新计算所有建筑的赚速
			recalAllBuilding();
		}finally
		{
		    getUserData().unlockUser();
		}
	}

	/***********
	 * 查找指定的建筑数据
	 *
	 * @param _buildingId
	 * @return
	 */
	public BuildingInfo lookupBuilding(long _buildingId)
	{
		getUserData().lockUser();

		try
		{
			for(int i = 0; i < _m_alBuildingInfoList.size(); i++)
			{
				BuildingInfo info = _m_alBuildingInfoList.get(i);
				if(null == info)
					continue;

				if(info.getBuildingId() == _buildingId)
					return info;
			}

			return null;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

	/******************
	 * 检查是否拥有建筑
	 *
	 * @param _buildingId
	 * @return
	 */
	public boolean hasBuilding(long _buildingId)
	{
		return null != lookupBuilding(_buildingId);
	}

	/*****************
	 * 构造初始化数据
	 *
	 * @param _buildingIdList
	 * @param _farmList
	 * @param _businessList
	 */
	public void makeProto(ArrayList<Long> _buildingIdList, ArrayList<Building_Farm> _farmList, ArrayList<Building_Business> _businessList)
	{
		getUserData().lockUser();

		try
		{
			//已建造建筑列表
			for(int i = 0; i < _m_alBuildingInfoList.size(); i++)
			{
				BuildingInfo info = _m_alBuildingInfoList.get(i);
				if(null == info)
					continue;

				_buildingIdList.add(info.getBuildingId());

				if(null != info.getFarm())
				{
					_farmList.add(info.getFarm().toProto());
				}

				if(null != info.getBusiness())
				{
					_businessList.add(info.getBusiness().toProto());
				}
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

	/*******************
	 * 创建建筑
	 *
	 * @param _buildingId
	 * @param _context
	 * @return
	 */
	public BuildingInfo create(long _buildingId, NPPlayerContext _context)
	{
		getUserData().lockUser();

		try
		{
			if(hasBuilding(_buildingId))
			{
				return null;
			}

			RefBuilding ref = RefBuilding.getMgr().get(_buildingId);
			if(null == ref)
			{
				return null;
			}

			//构造建筑数据
			BM bmObj = getUSServer().getBM();

			PlayerBuildingBO bo = new PlayerBuildingBO();
			bo.setCid(bmObj, getUserData().getCid());
			bo.setBuildingId(bmObj, _buildingId);
			bo.insert(bmObj);

			BuildingInfo info = new BuildingInfo(this, bo, ref);
			_m_alBuildingInfoList.add(info);

			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_010_BuildingOp.make_050_OnBuidingBuilt(info));

			//初始化建筑
			info.initNewBuilding(_context);

			_context.collectItem(ENPItemType.BUILDING, _buildingId, 1, true);

			LogBuildingBuildBO logBo = new LogBuildingBuildBO();
			logBo.setCid(getUSServer().getBM(), getUserData().getCid());
			logBo.setBuildingId(getUSServer().getBM(), _buildingId);
			CommLogDB.log(getUSServer().getBM(), logBo, _context);

			return info;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

	/**
	 * 初始化计算所有建筑数值
	 */
	public void _initCalAllBuilding()
	{
		getUserData().lockUser();
		try
		{
			for (BuildingInfo buildingInfo : _m_alBuildingInfoList)
			{
				buildingInfo._initCalBuilding();
			}
		} finally
		{
			getUserData().unlockUser();
		}
	}

	/**
	 * 重新计算所有建筑产出
	 */
	public void recalAllBuilding()
	{
		getUserData().lockUser();
		try
		{
			for (BuildingInfo buildingInfo : _m_alBuildingInfoList)
			{
				//调用计算处理
				buildingInfo.calOutputSpeed();
			}
		} finally
		{
			getUserData().unlockUser();
		}
	}

	/**
	 * 重新计算所有建筑产出
	 */
	@Override
	public String toString()
	{
		getUserData().lockUser();
		try
		{
			StringBuilder sb = new StringBuilder();
			for (BuildingInfo buildingInfo : _m_alBuildingInfoList)
			{
				BuildingBusinessFunc businessFunc = (BuildingBusinessFunc) buildingInfo.getBuildingFunc(EBuildingFuncEnum.BUSINESS);
				BuildingFarmFunc farmFunc = (BuildingFarmFunc) buildingInfo.getBuildingFunc(EBuildingFuncEnum.FARM);

				sb.append("buildingId: ").append(buildingInfo.getBuildingId()).append(", ")
						.append("earnings: ").append(businessFunc == null ? 0 :businessFunc.getSpeed()).append(", ")
						.append("autoTapEarnings: ").append(farmFunc == null ? 0 :farmFunc.getAutoTapCollectNumPerSec()).append("\n");
			}
			sb.append("earningAddRate: ").append(getTotalEarningRate()).append("\n");
			sb.append("earnings: ").append(getUserData().getPlayerComponent().getEarnings()).append("\n");
			sb.append("autoTapEarnings: ").append(getUserData().getPlayerComponent().getAutoTapEarnings()).append("\n");
			return sb.toString();
		} finally
		{
			getUserData().unlockUser();
		}
	}

	/**
	 * 随机增加员工
	 * @param _workerNum
	 * @param _context
	 * @return
	 */
	public List<WCGPair<Long, Integer>> randomAddWorkers(int _workerNum, int _useCount, NPPlayerContext _context)
	{
		getUserData().lockUser();
		try
		{
			List<WCGPair<Long, Integer>> resultList = new ArrayList<>();
			//从businessFunc中随机一个建筑
			List<BuildingBusinessFunc> allBusinessFuncList = new ArrayList<>();
			for (BuildingInfo buildingInfo : _m_alBuildingInfoList)
			{
				_ABuildingFunc buildingFunc = buildingInfo.getBuildingFunc(EBuildingFuncEnum.BUSINESS);
				BuildingBusinessFunc businessFunc = buildingFunc instanceof BuildingBusinessFunc ? ((BuildingBusinessFunc) buildingFunc) : null;
				if (businessFunc == null)
					continue;

				allBusinessFuncList.add(businessFunc);
			}

			if (allBusinessFuncList.isEmpty())
				return resultList;

			int[] randomUseCount = new int[allBusinessFuncList.size()];
			//把useCount随机分配到建筑上
			for (int i = 0; i < _useCount; i++)
			{
				//随机一个建筑
				int index = Random.nextInt(allBusinessFuncList.size());
				randomUseCount[index]++;
			}

			//增加员工
			for (int i = 0; i < randomUseCount.length; i++)
			{
				BuildingBusinessFunc businessFunc = allBusinessFuncList.get(i);
				int useCount = randomUseCount[i];
				if (useCount <= 0)
					continue;

				//增加员工
				int addNum = _workerNum * useCount;
				businessFunc.addEmployeeCount(addNum, _context);

				resultList.add(new WCGPair<>(businessFunc.getBuildingId(), addNum));
			}

			return resultList;
		} finally
		{
			getUserData().unlockUser();
		}
	}

	/**
	 * 获取指定建筑的员工数量
	 * @param _buildingId
	 * @return
	 */
	public long getWorkerNum(long _buildingId)
	{
        //如果_buildingId为0，则获取所有建筑的员工数量
		if (_buildingId == 0)
		{
			return getAllWorkerNum();
		}else
		{
			BuildingInfo buildingInfo = lookupBuilding(_buildingId);
			if (buildingInfo == null)
				return 0;

			BuildingBusinessFunc businessFunc = (BuildingBusinessFunc) buildingInfo.getBuildingFunc(EBuildingFuncEnum.BUSINESS);
			if (businessFunc == null)
				return 0;

			return businessFunc.getEmployeeCount();
		}
	}

	/**
	 * 获取所有员工数量
	 * @return
	 */
	private long getAllWorkerNum()
	{
		getUserData().lockUser();
		try
		{
			long workerNum = 0;
			for (BuildingInfo buildingInfo : _m_alBuildingInfoList)
			{
				BuildingBusinessFunc businessFunc = (BuildingBusinessFunc) buildingInfo.getBuildingFunc(EBuildingFuncEnum.BUSINESS);
				if (businessFunc == null)
					continue;

				workerNum += businessFunc.getEmployeeCount();
			}
			return workerNum;
		} finally
		{
			getUserData().unlockUser();
		}
	}

    /**
     * 获取建筑等级
     * @param _buildingFuncEnum
     * @param _buildingId
     * @return
     */
	public long getBuildingLevel(EBuildingFuncEnum _buildingFuncEnum, long _buildingId)
	{
        //如果_buildingId为0，则获取所有建筑的等级
		if (_buildingId != 0)
		{
			BuildingInfo buildingInfo = lookupBuilding(_buildingId);
			if (buildingInfo == null)
				return 0;

			return buildingInfo.getBuildingFuncLevel(_buildingFuncEnum);
        }else
        {
            long level = 0;
            getUserData().lockUser();
            try{
                for (BuildingInfo buildingInfo : _m_alBuildingInfoList)
                {
                    long buildingLevel = buildingInfo.getBuildingFuncLevel(_buildingFuncEnum);
                    level += buildingLevel;
                }
            }finally
            {
				getUserData().unlockUser();
            }
            return level;
        }
	}

	public int getBuildingNum()
	{
		getUserData().lockUser();
		try
		{
			return _m_alBuildingInfoList.size();
		} finally
		{
			getUserData().unlockUser();
		}
	}

	@Override
	public ENPItemType getItemType()
	{
		return ENPItemType.BUILDING;
	}

	@Override
	public long getItemCount(long _itemId)
	{
		return hasBuilding(_itemId) ? 1 : 0;
	}

	@Override
	public boolean hasItem(long _itemId, long _count)
	{
		return hasBuilding(_itemId);
	}

	@Override
	public void initGainItem(long _itemId, long _count, NPPlayerContext _context)
	{
		create(_itemId, _context);
	}

	@Override
	public void gainItem(long _itemId, long _count, boolean _isNotMerge, NPPlayerContext _context)
	{
		create(_itemId, _context);
	}

	@Override
	public boolean spendItem(long _itemId, long _count, NPPlayerContext _context)
	{
		return false;
	}

	/**
	 * 获取农田等级
	 *
	 * 特别说明：在当前代码体系下，可以存在多个farm，但是在需求里，只有一个farm，所以直接获取第一个farm即可
	 *
	 * @return
	 */
	public BuildingFarmFunc getFarm()
	{
		getUserData().lockUser();

		try
		{
			for(int i = 0; i < _m_alBuildingInfoList.size(); i++)
			{
				BuildingInfo info = _m_alBuildingInfoList.get(i);
				if(null == info)
					continue;

				BuildingFarmFunc farmFunc = info.getFarm();
				if(null != farmFunc)
				{
					return farmFunc;
				}
			}

			return null;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	/**
	 * 获取农田等级
	 * @return
	 */
	public int getFarmLvl()
	{
		BuildingFarmFunc obj = getFarm();
		if(null == obj)
			return 0;

		return obj.getLevel();
	}

	/**
	 * 替换产出速度
	 */
	public void replaceEarnings(long _preSpeed, long _newSpeed, boolean _isInit)
	{
		getUserData().lockUser();
		try
		{
			_m_earnings -= _preSpeed;
			_m_earnings += _newSpeed;

			//初始化的时候不进行处理，在最后统一调用结算
			if (!_isInit)
			{
				checkExceedMaxEarningsRecord();

				getUserData().getPlayerComponent().setNeedCalEarnings();
			}
		} finally
		{
			getUserData().unlockUser();
		}
	}

	/**
	 * 检查是否超过历史最大赚速
	 */
	public void checkExceedMaxEarningsRecord()
	{
		getUserData().lockUser();
		try{
		    //记录建筑最大赚速
    		if (_m_earnings > getUserData().getPlayerComponent().getParamV(ENPPlayerParam.BUILDINGS_EARNINGS_MAX_RECORD))
    			getUserData().getPlayerComponent().setParam(ENPPlayerParam.BUILDINGS_EARNINGS_MAX_RECORD, _m_earnings);
		}finally
		{
		    getUserData().unlockUser();
		}
	}

	/**
	 * 获取赚速
	 * @return
	 */
	public long getTotalEarning()
	{
		getUserData().lockUser();
		try{
			return _m_earnings;
		}finally
		{
			getUserData().unlockUser();
		}
	}

	/**
	 * 获取所有建筑的产出速度之和
	 * @return
	 */
	public long getBuildingSpeedSum()
	{
		getUserData().lockUser();

		try
		{
			long sum = 0;

			for(int i = 0; i < _m_alBuildingInfoList.size(); i++)
			{
				BuildingInfo info = _m_alBuildingInfoList.get(i);
				if(null == info)
					continue;

				BuildingBusinessFunc businessFunc = info.getBusiness();
				if(null == businessFunc)
					continue;

				sum += businessFunc.getSpeed();
			}

			return sum;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

	/**
	 * 获取建筑总等级
	 * @return
	 */
	public int getBuildingLvlSum()
	{
		getUserData().lockUser();

		try
		{
			int sum = 0;

			for(int i = 0; i < _m_alBuildingInfoList.size(); i++)
			{
				BuildingInfo info = _m_alBuildingInfoList.get(i);
				if(null == info)
					continue;

				BuildingBusinessFunc businessFunc = info.getBusiness();
				if(null == businessFunc)
					continue;

				sum += businessFunc.getLvl();
			}

			return sum;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

	/**
	 * 建筑截面数据
	 *
	 * @param _sectionLogType
	 */
	public void logSection(ELogSectionType _sectionLogType)
    {
		getUserData().lockUser();

		try
		{
			BM bmObj = getUSServer().getBM();

			for(int i = 0; i < _m_alBuildingInfoList.size(); i++)
			{
				BuildingInfo info = _m_alBuildingInfoList.get(i);
				if(null == info)
					continue;

				BuildingBusinessFunc businessFunc = info.getBusiness();
				if(null == businessFunc)
					continue;

				LogSectionBuildingV2BO logBo = new LogSectionBuildingV2BO();
		        //截面日志类型
		        logBo.setSectionType(getUSServer().getBM(), _sectionLogType.ordinal());
		        //玩家
		        logBo.setCid(bmObj, getUserData().getCid());
		        //建筑
		        logBo.setBuildingId(bmObj, businessFunc.getBuildingId());
		        logBo.setBuildingLvl(bmObj, businessFunc.getLvl());
		        logBo.setBuildingHireNum(bmObj, businessFunc.getEmployeeCount());

		        CommLogDB.log(bmObj, logBo);
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
    }
}
