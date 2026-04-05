package NPUSServer.NPUSUserMgr.UserComp.BuildingComp.Function;

import Common.BuildingEnum.EBuildingFuncEnum;
import Common.BuildingObj.Building_Farm;
import GS2GC.p010_BuildingOp.GS2GC_010_055_OnFarmBuilt;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.BuildingErr;
import NPCommon.ErrMain.Result.Result;
import NPGameRes.Refs.Building.RefFarmingBuilding;
import NPGameRes.Refs.Building.RefFarmingBuildingLevel;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.BuildingInfo;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_010_BuildingOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerBuildingFarmBO;

public class BuildingFarmFunc extends _ABuildingFunc
{
	//上次点击时间点（毫秒）
	private long _m_lLastClickMS;
	//赚速加成
	private int _m_earningRate;
    //自动点击收集数量/秒
    private long _m_autoTapCollectNumPerSec;

	//配表数据
	private RefFarmingBuilding _m_refFarmBuilding;
	private RefFarmingBuildingLevel _m_refFarmBuildingLevel;

	//bo数据对象
	private PlayerBuildingFarmBO _m_boFarm;

	private long _m_clickTimeSec;
	private int _m_clickNum;

	protected BuildingFarmFunc(BuildingInfo _buildingInfo, RefFarmingBuilding _ref, RefFarmingBuildingLevel _lvlRef)
    {
        super(_buildingInfo);

        _m_refFarmBuilding = _ref;
        _m_refFarmBuildingLevel = _lvlRef;

		_m_earningRate = _m_refFarmBuildingLevel.earning_rate;
    }

	@Override
	public EBuildingFuncEnum getFuncEnum()
	{
		return EBuildingFuncEnum.FARM;
	}

	public NPUSUserData getUserData() {return getBuildingInfo().getUserData();}

	public long getLastClickMS() {return _m_lLastClickMS;}

	public RefFarmingBuilding getRef() {return _m_refFarmBuilding;}
	public long getBuildingId() {return _m_refFarmBuilding.building_id;}

	public RefFarmingBuildingLevel getLvlRef() {return _m_refFarmBuildingLevel;}
	public int getLevel() {return _m_boFarm == null ? 1 : _m_boFarm.getLvl();}

	public PlayerBuildingFarmBO getBo() {return _m_boFarm;}

	public int getEarningRate()
	{
		return _m_earningRate;
	}

	public int getLevelGap()
	{
		return getLevel() - _m_refFarmBuildingLevel.level;
	}

	public long getAutoTapCollectNumPerSec()
	{
		return _m_autoTapCollectNumPerSec;
	}

	/***
	 * 初始化BO数据
	 *
	 * @param _bo
	 * @param _lvlRef
	 */
	public void initBo(PlayerBuildingFarmBO _bo, RefFarmingBuildingLevel _lvlRef)
	{
		_m_boFarm = _bo;
		_m_lLastClickMS = _bo.getLastClickMS();

		int oriEarningRate = _m_earningRate;

		_m_refFarmBuildingLevel = _lvlRef;
		_m_earningRate = _m_refFarmBuildingLevel.earning_rate + _m_refFarmBuildingLevel.earning_rate_per_level * getLevelGap();
        recalAutoTapOutputSpeed(true);

		//变更加成
		getBuildingInfo().getComp().replaceEarningRate(oriEarningRate, _m_earningRate);
	}

	@Override
	public void _initCalFunc(boolean _isInit)
	{
		long oriAutoTapEarnings = _m_autoTapCollectNumPerSec;
		_m_autoTapCollectNumPerSec = calAutoTapCollectNumPerSec();

		getBuildingInfo().getUserData().getPlayerComponent().replaceAutoTapEarnings(oriAutoTapEarnings, _m_autoTapCollectNumPerSec, _isInit);
	}


	/**
     * 重新计算自动点击收集数量/秒
     */
    public void recalAutoTapOutputSpeed(boolean _isInit)
    {
        getUserData().lockUser();
        try{
            long preSpeed = _m_autoTapCollectNumPerSec;

            _m_autoTapCollectNumPerSec = calAutoTapCollectNumPerSec();

            getBuildingInfo().getUserData().getPlayerComponent().replaceAutoTapEarnings(preSpeed, _m_autoTapCollectNumPerSec, _isInit);
        }finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 计算自动点击收集数量/秒
     * @return
     */
    public long calAutoTapCollectNumPerSec()
    {
        return (getLvlRef().tap_to_collect_num + (long) getLvlRef().tap_to_collect_num_per_level * getLevelGap())
                * (getLvlRef().auto_tap_num_per_sec + (long) getLvlRef().auto_tap_num_per_sec_per_level * getLevelGap());
    }

    public Building_Farm toProto()
	{
		getUserData().lockUser();

		try
		{
			Building_Farm proto = new Building_Farm();
			proto.setBuildingId(getBuildingId());
			proto.setLvl(getLevel());
			proto.setLastClickMS(_m_lLastClickMS);

			return proto;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

	/*************
	 * 设置建筑等级
	 *  @param _lvlRef
	 * @param _newLevel
	 * @param _context
	 */
	public void setLvl(RefFarmingBuildingLevel _lvlRef, int _newLevel, NPPlayerContext _context)
	{
		getUserData().lockUser();

		try
		{
			if(getLevel() == _newLevel)
				return;

			int oriEarningRate = getEarningRate();

			_m_refFarmBuildingLevel = _lvlRef;
			//保存数据
			_saveLevel(_newLevel);

			_m_earningRate = _m_refFarmBuildingLevel.earning_rate + _m_refFarmBuildingLevel.earning_rate_per_level * getLevelGap();
            recalAutoTapOutputSpeed(false);

			//变更加成
			getBuildingInfo().getComp().replaceEarningRate(oriEarningRate, _m_earningRate);

			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_010_BuildingOp.make_051_OnFarmChg(this));
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

	/**************
	 * 设置最后一次点击时间
	 * @param _clickMs
	 */
	public Result recordClick(long _clickMs)
	{
		getUserData().lockUser();
		try
		{
			long curTimeSec = _clickMs / 1000;
			if (curTimeSec != _m_clickTimeSec)
			{
				_m_clickTimeSec = curTimeSec;
				_m_clickNum = 0;
			}

			int canClickNum = (int) Math.ceil(1000d / RefGeneral.Ref().farming_building_click_spaceMS);
			if (_m_clickNum >= canClickNum)
				return BuildingErr.FARM_CLICK_GAP_LESS;

			_m_clickNum++;

			return Result.SUCC;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

	/*************
	 * 保存数据
	 */
	private void _saveLevel(int _newLevel)
	{
		BM bmObj = getUserData().getUSServer().getBM();

		if(null == _m_boFarm)
		{
			PlayerBuildingFarmBO bo = new PlayerBuildingFarmBO();
			bo.setCid(bmObj, getUserData().getCid());
			bo.setBuildingId(bmObj, getBuildingId());
			bo.setLvl(bmObj, _newLevel);
			bo.insert(bmObj);

			_m_boFarm = bo;
		}
		else
		{
			_m_boFarm.saveLvl(bmObj, _newLevel);
		}
	}

	@Override
	public void onBuilt()
	{
		GS2GC_010_055_OnFarmBuilt proto = new GS2GC_010_055_OnFarmBuilt();
		proto.setFarm(toProto());
		getBuildingInfo().getUserData().sendMsgToGC(proto);
	}

	/************************
	 * 检查创建农场建筑
	 *
	 * @param _buildingInfo
	 * @return
	 */
	public static BuildingFarmFunc checkBuild(BuildingInfo _buildingInfo)
	{
		RefFarmingBuilding ref = RefFarmingBuilding.getMgr().get(_buildingInfo.getBuildingId());
		if (null == ref)
			return null;

		RefFarmingBuildingLevel lvlRef = ref.getLevelMapMgr().getLevelData(1);
		if (null == lvlRef)
		{
			USLog.error(_buildingInfo.getUserData().getUSServer(), "player:{} building:{} farm lvl:{} fail, not find lvl ref.",
					_buildingInfo.getUserData().getCid(), _buildingInfo.getBuildingId(), 1);
			return null;
		}

		//创建数据
        return new BuildingFarmFunc(_buildingInfo, ref, lvlRef);
	}
}
