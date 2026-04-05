package NPUSServer.NPUSUserMgr.UserComp.MarsStep2_BuildingComp;

import ALBasicServer.ALProcess.ALProcess;
import Common.MarsEnum.EMarsBuildingType;
import Common.MarsObj.Mars_Building;
import Common.MarsObj.Mars_BuildingEquipment;
import Common.MarsObj.Mars_BuildingGainEnergyResult;
import Common.MarsObj.Mars_Mars_BuildingEquipment_Food;
import CommonEnum.ESpecialItemType;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.CommonProcess._IEZProcessMonitorNoTimeOut;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.ErrMain.Result.Result;
import NPCommon.LazyTaskDealer.LazyTaskDealer;
import NPCommon.Util.CallBack._ICallBackBool;
import NPEnum.ENPGameEvent;
import NPEnum.ENPPlayerRecordParam;
import NPGameRes.Refs.Mars.RefMarsBuilding;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_MARS_GAIN_BUILDING_ENERGY;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.SpecialItemComponent.Dealer.SpecialItemDealer_MarsEnergy;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.USLog;
import USDB.Bo.PlayerMarsBuildingBO;
import USDB.Bo.PlayerMarsBuildingEquipmentBO;

import java.util.ArrayList;
import java.util.List;

/**
 * 火星 - 火星建筑
 * @author mj
 *
 */
public class MarsBuildingComponent extends _ANPUserComponent
{
	//建筑组件数据
	private MarsBuildingCompInfo _m_biBuildingCompInfo;

	//所有建筑数据列表
	private ArrayList<MarsBuildingInfo> _m_alAllBuildingList;
	
	//主基地功能数据
	private MarsHomeBuildingFunc _m_hiHomeBuildingFunc;
	//居民建筑功能数据管理
	private MarsPeopleBuildingFuncMgr _m_mgrPeopleBuildingFuncMgr;
	
	//火星全部建筑属性计算
	private MarsAllBuildingValue _m_bvAllBuildingValue;
    //火星全部建筑属性延迟计算
    private LazyTaskDealer _m_ldCalAllBuildingValueDealer;
	
	//能量数据延迟处理
	private SpecialItemDealer_MarsEnergy _m_sdMarsEnergyDealer;

    //健康指数
    private long _m_lHealthIndex;
    //健康指数延迟计算
    private LazyTaskDealer _m_ldCalHealthIndexDealer;
    
    //幸福指数
    private long _m_lHappyIndex;
    //幸福指数延迟计算
    private LazyTaskDealer _m_ldCalHappyIndexDealer;

    //治愈率
    private long _m_lCureRate;
    //治愈率延迟计算
    private LazyTaskDealer _m_ldCureRateDealer;

    //最高派遣数量计算
    private LazyTaskDealer _m_ldCalDispatchedNumSumDealer;
    
    //建筑队列管理
    private MarsBuildingUpQueueMgr _m_mgrBuildingUpQueueMgr;
    
    public MarsBuildingComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.MARS_BUILDING);
        
        _m_biBuildingCompInfo = new MarsBuildingCompInfo(getUserData());
                
        _m_alAllBuildingList = new ArrayList<>();
        
        _m_mgrPeopleBuildingFuncMgr = new MarsPeopleBuildingFuncMgr(getUserData());
		
        _m_bvAllBuildingValue = new MarsAllBuildingValue(this);
        _m_ldCalAllBuildingValueDealer = new LazyTaskDealer(() -> calAllBuildingValue(), 200);

		_m_sdMarsEnergyDealer = getUserData().getSpecialItemComponent().getDealer(ESpecialItemType.MARS_ENERGY, SpecialItemDealer_MarsEnergy.class);
		
		_m_ldCalHealthIndexDealer = new LazyTaskDealer(() -> MarsAllBuildingIndexCalculate.calHealthIndex(getUserData(), null), 200);
		_m_ldCalHappyIndexDealer = new LazyTaskDealer(() -> MarsAllBuildingIndexCalculate.calHappyIndex(getUserData(), null), 200);
		
		_m_ldCureRateDealer = new LazyTaskDealer(() -> MarsAllBuildingIndexCalculate.calCureRate(getUserData(), null), 200);
		
		_m_ldCalDispatchedNumSumDealer = new LazyTaskDealer(() -> calDispatchedNumSum(), 500);
		
		_m_mgrBuildingUpQueueMgr = new MarsBuildingUpQueueMgr(getUserData());
		
        _initAllRef();
    }
    
    public MarsBuildingCompInfo getCompInfo() {return _m_biBuildingCompInfo;}
    
    public MarsHomeBuildingFunc getHomeFunc() {return _m_hiHomeBuildingFunc;}
    public MarsPeopleBuildingFuncMgr getPeopleBuildingFuncMgr() {return _m_mgrPeopleBuildingFuncMgr;}

    public MarsAllBuildingValue getAllBuildingValue() {return _m_bvAllBuildingValue;}
	public void doLazyCalAllBuildingValue() {_m_ldCalAllBuildingValueDealer.setNeedDeal();}

	public SpecialItemDealer_MarsEnergy getMarsEnergyDealer() {return _m_sdMarsEnergyDealer;}
    
	public long getHealthIndex() {return _m_lHealthIndex;}
	public void setHealthIndex(long _value) {_m_lHealthIndex = _value;}
	public void doLazyCalHealthIndex() {_m_ldCalHealthIndexDealer.setNeedDeal();}
	
	public long getHappyIndex() {return _m_lHappyIndex;}
	public void setHappyIndex(long _value) {_m_lHappyIndex = _value;}
	public void doLazyCalHappyIndex() {_m_ldCalHappyIndexDealer.setNeedDeal();}

	public long getCureRate() {return _m_lCureRate;}
	public void setCureRate(long _value) {_m_lCureRate = _value;}
	public void doLazyCalCureRate() {_m_ldCureRateDealer.setNeedDeal();}
	
	public void doLazyCalDispatchedNumSumDealer() {_m_ldCalDispatchedNumSumDealer.setNeedDeal();}
	
    public MarsBuildingUpQueueMgr getBuildingUpQueueMgr() {return _m_mgrBuildingUpQueueMgr;}
	
    //预先加载所有配置建筑数据
    private void _initAllRef()
    {
    	List<RefMarsBuilding> refList = RefMarsBuilding.getMgr().getList();
    	for(int i = 0; i < refList.size(); i++)
    	{
    		RefMarsBuilding ref = refList.get(i);
    		if(null == ref)
    			continue;
    		
    		MarsBuildingInfo building = new MarsBuildingInfo(getUserData(), ref);
    		_m_alAllBuildingList.add(building);
    		//更新配表处理
    		building._onInitedFromRef();
    		
    		//注册建筑功能
    		//主基地
    		if(RefGeneral.Ref().mars_building_home_id == ref.id)
    		{
    			_m_hiHomeBuildingFunc = new MarsHomeBuildingFunc(building);
    			building.regFunc(_m_hiHomeBuildingFunc);
    		}
    		
    		//居民建筑
    		if(ref.settle_group_id > 0) 
    		{
    			MarsPeopleBuildingFunc peopleBuildingFunc = new MarsPeopleBuildingFunc(building);
    			_m_mgrPeopleBuildingFuncMgr._initInfo(peopleBuildingFunc);

    			building.regFunc(peopleBuildingFunc);
    		}
    	}
    }
    
    @Override
    protected void _init()
    {
    	//检查主基地数据
    	if(null == _m_hiHomeBuildingFunc)
    	{
    		USLog.error(getUSServer(), "player:{} home:{} mars building init home fail, not find ref.", getCid(), RefGeneral.Ref().mars_building_home_id);
    		getUserData().setDataLoadFail();
    		return;
    	}
    	
        ALProcess process = ALProcess.CreateProcess("mars_building_comp_init");
        //步骤1：建筑组件数据加载
        process.addResDelegateProcess(action -> _m_biBuildingCompInfo._initFromDB(action::dealAction), "mars_building_comp_info_init",
                () -> USLog.error(getUSServer(), "load mars building info fail[mars_building_comp_init], cid:{}.", getCid()), false);
        //步骤2：建筑通用数据加载
        process.addResDelegateProcess(action -> _initBuildingFromDB(action::dealAction), "mars_building_info_init",
                () -> USLog.error(getUSServer(), "load mars building info fail[mars_building_comp_init], cid:{}.", getCid()), false);
        //步骤3：主基地数据加载
    	process.addResDelegateProcess(action -> _m_hiHomeBuildingFunc._initFromDB(action::dealAction), "mars_building_comp_home_init",
                () -> USLog.error(getUSServer(), "load mars building home func fail[mars_building_comp_init], cid:{}.", getCid()), false);
        //步骤4：居民建筑数据加载
        process.addResDelegateProcess(action -> _m_mgrPeopleBuildingFuncMgr._initFromDB(action::dealAction), "mars_building_comp_people_init",
                () -> USLog.error(getUSServer(), "load mars building people func fail[mars_building_comp_init], cid:{}.", getCid()), false);
        //步骤5：建筑部件据加载
        process.addResDelegateProcess(action -> _initEquipmentFromDB(action::dealAction), "mars_building_comp_equipment_init",
                () -> USLog.error(getUSServer(), "load mars building equipment fail[mars_building_comp_init], cid:{}.", getCid()), false);
        //步骤6：建造队列数据加载
        process.addResDelegateProcess(action -> _m_mgrBuildingUpQueueMgr._initFromDB(action::dealAction), "mars_building_comp_up_queue_init",
                () -> USLog.error(getUSServer(), "load mars building up queue fail[mars_building_comp_init], cid:{}.", getCid()), false);

        process.dealProcess(new _IEZProcessMonitorNoTimeOut()
        {
            @Override
            public void onRootProecssStop()
            {
                USLog.error(getUSServer(), "load quest comp fail[onRootProecssStop], cid:{}", getUserData().getCid());
                getUserData().setDataLoadFail();
            }

            @Override
            public void onRootProecssSuc()
            {
                setInited();
            }
        });
    }
    
    /**
     * 加载建筑基础数据
     * @param _handler
     */
    private void _initBuildingFromDB(_ICallBackBool _handler)
    {
        getUSServer().getBM().getBM(PlayerMarsBuildingBO.class).findAll("cid", getCid(), 
        		new _ASelectCallback<List<PlayerMarsBuildingBO>>()
        {
            @Override
            public void dealFail()
            {
            	_handler.onRunOver(false);
            }
            
            @Override
            public void dealSuc(List<PlayerMarsBuildingBO> _list)
            {
            	_initBuildingBoList(_list);
            	
            	_handler.onRunOver(true);
            }
        });
    }
	private void _initBuildingBoList(List<PlayerMarsBuildingBO> _list)
	{
		for(int i = 0; i < _list.size(); i++)
		{
			PlayerMarsBuildingBO bo = _list.get(i);
			if(null == bo)
				continue;
			
			MarsBuildingInfo building = lookupBuilding(bo.getBuildingId());
			if(null == building)
			{
				USLog.error(getUSServer(), "player:{} building:{} mars people building init bo fail, not find building.", 
						getCid(), bo.getBuildingId());
				continue;
			}
			
			building._loadBo(bo);
		}
	}
    
	/**
	 * 加载部件数据
	 * @param _handler
	 */
    private void _initEquipmentFromDB(_ICallBackBool _handler)
    {
        getUSServer().getBM().getBM(PlayerMarsBuildingEquipmentBO.class).findAll("cid", getCid(), 
        		new _ASelectCallback<List<PlayerMarsBuildingEquipmentBO>>()
        {
            @Override
            public void dealFail()
            {
            	_handler.onRunOver(false);
            }
            
            @Override
            public void dealSuc(List<PlayerMarsBuildingEquipmentBO> _list)
            {
            	_initEquipmentBoList(_list);
            	
            	_handler.onRunOver(true);
            }
        });
    }
	private void _initEquipmentBoList(List<PlayerMarsBuildingEquipmentBO> _list)
	{
		for(int i = 0; i < _list.size(); i++)
		{
			PlayerMarsBuildingEquipmentBO bo = _list.get(i);
			if(null == bo)
				continue;
			
			MarsBuildingInfo building = lookupBuilding(bo.getBuildingId());
			if(null == building)
			{
				USLog.error(getUSServer(), "player:{} building:{} equipment:{} mars people building init equipment bo fail, not find building.", 
						getCid(), bo.getBuildingId(), bo.getEquipmentId());
				continue;
			}
			
			building.getBuildingEquipmentMgr()._initBo(bo);
		}
	}

    //获取需要依赖的加载组件项，无依赖则返回null
    @Override
    public ENPPlayerCompType[] getDependCompList()
    {
        return null;
    }

    @Override
    public void onInited()
    {
    	//计算主基地属性加成数据
    	_m_hiHomeBuildingFunc.getBuildingInfo().getBuildingValue().setHomeOxygenYield(_m_hiHomeBuildingFunc.getOxygenYield(null));
		//更新能量消耗速度
    	_m_hiHomeBuildingFunc.getBuildingInfo().getBuildingValue().setHomeEnergyConsumePerMin(_m_hiHomeBuildingFunc.getConsumePerMin());
    	//计算所有属性加成数据
		for(int i = 0; i < _m_alAllBuildingList.size(); i++)
		{
			MarsBuildingInfo info = _m_alAllBuildingList.get(i);
			if(null == info)
				continue;
			
			//检查自动结算的建筑部件
			info.checkUnlockEquipment(true, getUserData().getPlayerInitContext());
			//计算
			info.calBuildingAllProperty(true);
		}
		//计算全部建筑属性汇总
		_initCalAllBuildingValue();
		//计算健康/幸福指数/治愈率
		MarsAllBuildingIndexCalculate.calHealthIndex(getUserData(), null);
		MarsAllBuildingIndexCalculate.calHappyIndex(getUserData(), null);
		MarsAllBuildingIndexCalculate.calCureRate(getUserData(), null);
    }

    /***********
     * 玩家数据释放时候的处理，一般需要做关联处理。如注销监听等的处理
     */
    @Override
    public void dispose()
    {
    }
    
    /**
     * 所有组件加载后执行
     */
    public void _onAllCompInitedDeal()
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		//建筑数据加载后执行
    		for(int i = 0; i < _m_alAllBuildingList.size(); i++)
    		{
    			MarsBuildingInfo info = _m_alAllBuildingList.get(i);
    			if(null == info)
    				continue;
    			
    			info._onInited();
    		}
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 初始化全部建筑属性
     */
    protected void _initCalAllBuildingValue()
    {
		long oxygenValue = 0;
		long energyConsumePerMin = 0;
		long satietyValue = 0;
		long cureRate = 0;
	    long comfortValue = 0;
	    long moodValue = 0;
	    long sleepValue = 0;
	    long peopleNumLimit = 0;
	    long outputValuePerMin = 0;
	    
	    for(int i = 0; i < _m_alAllBuildingList.size(); i++)
	    {
	    	MarsBuildingInfo info = _m_alAllBuildingList.get(i);
	    	if(null == info)
	    		continue;
	    	
	    	oxygenValue += info.getBuildingValue().getHomeOxygenYield();
	    	energyConsumePerMin += info.getBuildingValue().getEnergyConsumePerMin() 
	    			+ info.getBuildingValue().getHomeEnergyConsumePerMin();
	    	satietyValue += info.getBuildingValue().getFinalSatietyValue();
	    	cureRate += info.getBuildingValue().getCureRate();
	    	comfortValue += info.getBuildingValue().getFinalComfortValue();
	    	moodValue += info.getBuildingValue().getFinalMoodValue();
	    	sleepValue += info.getBuildingValue().getFinalSleepValue();
	    	peopleNumLimit += info.getBuildingValue().getPeopleNumLimit();
	    	outputValuePerMin += info.getBuildingValue().getOutputValuePerMin();
	    	//派遣人数的产出需要计算人数
	    	MarsPeopleBuildingFunc peopleFunc = info.getPeopleFunc();
	    	if(null != peopleFunc)
	    	{
	    		outputValuePerMin += info.getBuildingValue().getPeopleOutputValuePerMin() * peopleFunc.getDispatchedNum();
	    	}
	    	
	    	//累计治愈居民数区间
    	    _m_bvAllBuildingValue.getCureNum().add(info.getBuildingValue().getCureNum());
	    }
	    
	    _m_bvAllBuildingValue.setOxygenValue(oxygenValue);
	    _m_bvAllBuildingValue.setEnergyConsumePerMin(energyConsumePerMin);
	    _m_bvAllBuildingValue.setSatietyValue(satietyValue);
	    _m_bvAllBuildingValue.setCureRate(cureRate);
	    _m_bvAllBuildingValue.setComfortValue(comfortValue);
	    _m_bvAllBuildingValue.setMoodValue(moodValue);
	    _m_bvAllBuildingValue.setSleepValue(sleepValue);
	    _m_bvAllBuildingValue.setPeopleNumLimit(peopleNumLimit);
	    _m_bvAllBuildingValue.setOutputValuePerMin(outputValuePerMin);
    }
    
    /**
     * 计算全部建筑的属性汇总
     * @param _bInit 是否初始化操作
     */
    public void calAllBuildingValue()
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		long oxygenValue = 0;
    		long energyConsumePerMin = 0;
    		long satietyValue = 0;
    		long cureRate = 0;
    	    long comfortValue = 0;
    	    long moodValue = 0;
    	    long sleepValue = 0;
    	    long peopleNumLimit = 0;
    	    long outputValuePerMin = 0;
    	    
    	    for(int i = 0; i < _m_alAllBuildingList.size(); i++)
    	    {
    	    	MarsBuildingInfo info = _m_alAllBuildingList.get(i);
    	    	if(null == info)
    	    		continue;
    	    	
    	    	oxygenValue += info.getBuildingValue().getHomeOxygenYield();
    	    	energyConsumePerMin += info.getBuildingValue().getEnergyConsumePerMin() 
    	    			+ info.getBuildingValue().getHomeEnergyConsumePerMin();
    	    	satietyValue += info.getBuildingValue().getFinalSatietyValue();
    	    	cureRate += info.getBuildingValue().getCureRate();
    	    	comfortValue += info.getBuildingValue().getFinalComfortValue();
    	    	moodValue += info.getBuildingValue().getFinalMoodValue();
    	    	sleepValue += info.getBuildingValue().getFinalSleepValue();
    	    	peopleNumLimit += info.getBuildingValue().getPeopleNumLimit();
    	    	outputValuePerMin += info.getBuildingValue().getOutputValuePerMin();
    	    	//派遣人数的产出需要计算人数
    	    	MarsPeopleBuildingFunc peopleFunc = info.getPeopleFunc();
    	    	if(null != peopleFunc)
    	    	{
    	    		outputValuePerMin += info.getBuildingValue().getPeopleOutputValuePerMin() * peopleFunc.getDispatchedNum();
    	    	}

    	    	//累计治愈居民数区间
        	    _m_bvAllBuildingValue.getCureNum().add(info.getBuildingValue().getCureNum());
    	    }
    	    
    	    _m_bvAllBuildingValue.updateOxygenValue(oxygenValue);
    	    _m_bvAllBuildingValue.updateEnergyConsumePerMin(energyConsumePerMin);
    	    _m_bvAllBuildingValue.updateSatietyValue(satietyValue);
    	    _m_bvAllBuildingValue.updateCureRate(cureRate);
    	    _m_bvAllBuildingValue.updateComfortValue(comfortValue);
    	    _m_bvAllBuildingValue.updateMoodValue(moodValue);
    	    _m_bvAllBuildingValue.updateSleepValue(sleepValue);
    	    _m_bvAllBuildingValue.updatePeopleNumLimit(peopleNumLimit);
    	    _m_bvAllBuildingValue.setOutputValuePerMin(outputValuePerMin);
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 计算所有玩家的产出
     */
    public void calAllBuildingOutput()
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		for(int i = 0; i < _m_alAllBuildingList.size(); i++)
    		{
    			MarsBuildingInfo info = _m_alAllBuildingList.get(i);
    			if(null == info)
    				continue;
    			
    			info.calBuildingOutput();
    		}
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 构造所有建筑协议
     * @param _list
     */
    public void makeBuildingProto(ArrayList<Mars_Building> _list)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		for(int i = 0; i < _m_alAllBuildingList.size(); i++)
    		{
    			MarsBuildingInfo info = _m_alAllBuildingList.get(i);
    			if(null == info)
    				continue;
    			
    			_list.add(info.toProto());
    		}
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
	/**
	 * 构造建筑部件数据
	 * @param _list
	 */
	public void makeBuildingEquipmentProto(ArrayList<Mars_BuildingEquipment> _list)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alAllBuildingList.size(); i++)
    		{
    			MarsBuildingInfo info = _m_alAllBuildingList.get(i);
    			if(null == info)
    				continue;
				
				info.getBuildingEquipmentMgr().makeProto(_list);
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * FOOD开启power
	 * @param _list
	 */
	public void makeFoodProto(ArrayList<Mars_Mars_BuildingEquipment_Food> _list)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alAllBuildingList.size(); i++)
    		{
    			MarsBuildingInfo info = _m_alAllBuildingList.get(i);
    			if(null == info)
    				continue;
				
    			if(info.isFoodPowerOn())
				{
    				Mars_Mars_BuildingEquipment_Food obj = new Mars_Mars_BuildingEquipment_Food();
    				obj.setBuildingId(info.getBuildingId());
    				obj.setIsPowerOn(info.isFoodPowerOn());
				}
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 所有建筑等级总和
	 * @return
	 */
	public int getAllBuildingLvlSum()
	{
		getUserData().lockUser();
		
		try
		{
			int sum = 0;
			for(int i = 0; i < _m_alAllBuildingList.size(); i++)
    		{
    			MarsBuildingInfo info = _m_alAllBuildingList.get(i);
    			if(null == info)
    				continue;
    			
    			if(!info.isUnlock())
    				continue;
				
    			sum += info.getBuildingLvl();
			}
			
			return sum;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 获取所有建筑的所有部件的等级总和
	 * @return
	 */
	public int getAllBuildingEquipLvlSum()
	{
		getUserData().lockUser();
		
		try
		{
			int sum = 0;
			for(int i = 0; i < _m_alAllBuildingList.size(); i++)
    		{
    			MarsBuildingInfo info = _m_alAllBuildingList.get(i);
    			if(null == info)
    				continue;
    			
    			if(!info.isUnlock())
    				continue;
				
    			sum += info.getBuildingEquipmentMgr().getLvlSum();
			}
			
			return sum;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 获取所有建筑的火星实力总和
	 * @return
	 */
	public long getMarsPowerSum(StringBuilder _sb)
	{
		getUserData().lockUser();
		
		try
		{
			if(null != _sb)
			{
				_sb.append("\n---------------- cal building mars power ----------------");
			}
			
			long value = 0;
			for(int i = 0; i < _m_alAllBuildingList.size(); i++)
			{
				MarsBuildingInfo info = _m_alAllBuildingList.get(i);
				if(null == info)
					continue;
				
				long power = info.getMarsPower();
				if(null != _sb)
				{
					_sb.append("\nbuilding:").append(info.getBuildingId()).append(", lvl:").append(info.getBuildingLvl()).append(", power:").append(power);
				}
				
				value += power;
			}
			
			return value;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
    /**
     * 查找指定建筑数据
     * @param _buildingId
     * @return
     */
    public MarsBuildingInfo lookupBuilding(long _buildingId)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		for(int i = 0; i < _m_alAllBuildingList.size(); i++)
    		{
    			MarsBuildingInfo info = _m_alAllBuildingList.get(i);
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
    
    /**
     * 查找指定类型的建筑数据
     * @param _buildingType
     * @return
     */
    public MarsBuildingInfo lookupBuildingByType(EMarsBuildingType _buildingType)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		for(int i = 0; i < _m_alAllBuildingList.size(); i++)
    		{
    			MarsBuildingInfo info = _m_alAllBuildingList.get(i);
    			if(null == info)
    				continue;
    		
    			if(info.getRef().building_type == _buildingType)
    				return info;
    		}
    		
    		return null;
    	}
    	finally 
    	{
    		getUserData().unlockUser();
		}
    }
    
    /**
     * 获取所有建筑派遣居民数量总和
     * @return
     */
    public long getDispatchedNumSum()
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		long sum = 0;
    		for(int i = 0; i < _m_alAllBuildingList.size(); i++)
    		{
    			MarsBuildingInfo info = _m_alAllBuildingList.get(i);
    			if(null == info)
    				continue;
    			
    			MarsPeopleBuildingFunc func = info.getPeopleFunc();
                if(null != func)
                {
                	sum += func.getDispatchedNum();
                }
    		}
    		
    		return sum;
    	}
    	finally 
    	{
    		getUserData().unlockUser();
		}
    }
    
    /**
     * 发起计算历史最高记录
     */
    public void calDispatchedNumSum()
    {
    	long sum = getDispatchedNumSum();
    	
    	getUserData().getRecordComponent().setGtRecord(ENPPlayerRecordParam.MARS_BUILDING_DISPATCH_NUM, sum, NPPlayerContext.createNew(ENPGameEvent.MARS_BUILDING_PEOPLE_DISPATCH_SUM_CAL));
    }
    
    /**
     * 获取所有建筑的能量
     * @param _list
     * @param _context
     * @return
     */
    public Result gainAllBuildingEnergy(ArrayList<Mars_BuildingGainEnergyResult> _list, NPPlayerContext _context)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		long energySum = _m_mgrPeopleBuildingFuncMgr.settleAll(_list, _context);
    		if(energySum > 0)
    		{
    			NPCommonCostItem item = new NPCommonCostItem(RefGeneral.Ref().mars_building_energy_output_item, energySum);
    			getUserData().gainItem(item, _context);
    		}

			//触发事件
			Event_P_MARS_GAIN_BUILDING_ENERGY evt = new Event_P_MARS_GAIN_BUILDING_ENERGY(_context, 1);
			getUserData().onLogicEvent(evt);
    		
    		return Result.SUCC;
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
	@Override
	public String toString()
	{
		getUserData().lockUser();
		
		try
		{
			StringBuilder sb = new StringBuilder();
			
			MarsAllBuildingIndexCalculate.calHappyIndex(getUserData(), sb);
			MarsAllBuildingIndexCalculate.calHealthIndex(getUserData(), sb);
			MarsAllBuildingIndexCalculate.calCureRate(getUserData(), sb);
			
			//基础数据
			sb.append("\n--------------------- result sum ---------------------");
			sb.append(getAllBuildingValue().toString())
				.append("\nHappyIndex:").append(_m_lHealthIndex)
				.append("\nHappyIndex:").append(_m_lHappyIndex)
				.append("\nCureRate:").append(_m_lCureRate);
			
			//建筑数据
			sb.append("\nbuilding size:").append(_m_alAllBuildingList.size());
			for(int i = 0; i < _m_alAllBuildingList.size(); i++)
    		{
    			MarsBuildingInfo info = _m_alAllBuildingList.get(i);
    			if(null == info)
    				continue;
    			
    			sb.append("\n============ building ").append(info.getBuildingId()).append(" ============");
    			sb.append(info.toString());
    		}
			
			sb.append("\n");
			
			return sb.toString();
		}
    	finally
    	{
    		getUserData().unlockUser();
    	}
	}
}
