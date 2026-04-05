package NPUSServer.NPUSUserMgr.UserComp.MarsStep2_BuildingComp;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.MarsObj.Mars_Building;
import Common.NpChatObj.ChatObj_SystemLog;
import Common.NpChatObj.NPCommon_ChatPlayerContent;
import CommonEnum.ECurrency;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.CommonObj.NPItemCollector;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.LazyTaskDealer.LazyTaskDealer;
import NPCommon.Util.CommonFunc;
import NPEnum.*;
import NPGameRes.Refs.Mars.RefMarsBuilding;
import NPGameRes.Refs.Mars.RefMarsBuildingCondition;
import NPGameRes.Refs.Mars.RefMarsBuildingLevel;
import NPGameRes.Refs.Mars.RefMarsEquipment;
import NPGameRes.Refs.RefGeneral;
import NPGameRes.Refs.Share.RefBoxComm;
import NPUSServer.CommBoxMgr.CommBoxInfo;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_039_MarsBuildingOp;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.PlayerMarsBuildingBO;

import java.util.ArrayList;
import java.util.List;

/**
 * 火星建筑基类
 *  - 主基地
 *  - 居民建筑
 *  
 * @author mj
 *
 */
public class MarsBuildingInfo
{
	//玩家数据对象
	private NPUSUserData _m_udUserData;

	//基础配置
	private RefMarsBuilding _m_ref;
	
	//数据bo
	private PlayerMarsBuildingBO _m_bo;

	//等级数据
	private int _m_iBuildingLvl;
	//food开关
	private boolean _m_bIsFoodPowerOn;
	
	//等级配置
	private RefMarsBuildingLevel _m_refLvl;
	
	//建筑部件数据管理
	private MarsBuildingEquipmentMgr _m_mgrBuildingEquipmentMgr;
    
    //当前挂载的功能组件
    private ArrayList<_AMarsBuildingFunc> _m_alBuildingFuncList;
    
    //建筑属性数据
    private MarsBuildingValue _m_bpBuildingValue;

	//建筑属性计算器
    private LazyTaskDealer _m_cdCalBuildingPropertyLazyDealer;
    
	public MarsBuildingInfo(NPUSUserData _userData, RefMarsBuilding _ref)
	{
		_m_udUserData = _userData;
		
		_m_ref = _ref;
		
		_m_mgrBuildingEquipmentMgr = new MarsBuildingEquipmentMgr(this);
		
		_m_alBuildingFuncList = new ArrayList<>();
		
		_m_bpBuildingValue = new MarsBuildingValue(this);
		
		_m_cdCalBuildingPropertyLazyDealer = new LazyTaskDealer(() -> calBuildingAllProperty(false), 200);
	}
	
	public NPUSUserData getUserData() {return _m_udUserData;}
    public NPUserServer getUSServer() {return getUserData().getUSServer();}
    public long getCid() {return getUserData().getCid();}
    public BM getBM() {return getUserData().getUSServer().getBM();}
    
    public PlayerMarsBuildingBO getBo() {return _m_bo;}

    public int getBuildingLvl() {return _m_iBuildingLvl;}
    public boolean isFoodPowerOn() {return _m_bIsFoodPowerOn;}
    
    public RefMarsBuilding getRef() {return _m_ref;}
    public long getBuildingId() {return _m_ref.id;}
    
    public RefMarsBuildingLevel getLvlRef() {return _m_refLvl;}
    public long getMarsPower() {return null == _m_refLvl ? 0 : _m_refLvl.mars_power_value;}

    public MarsBuildingEquipmentMgr getBuildingEquipmentMgr() {return _m_mgrBuildingEquipmentMgr;}
    
    //注册建筑功能对象
    public void regFunc(_AMarsBuildingFunc _func) {_m_alBuildingFuncList.add(_func);}
    //获取所有建筑功能数据列表
    public ArrayList<_AMarsBuildingFunc> getFuncList() {return _m_alBuildingFuncList;}
    //获取居民建筑功能对象
    public MarsPeopleBuildingFunc getPeopleFunc()
    {
    	for(int i = 0; i < _m_alBuildingFuncList.size(); i++)
    	{
    		_AMarsBuildingFunc func = _m_alBuildingFuncList.get(i);
    		if(func instanceof MarsPeopleBuildingFunc)
    		{
    			return (MarsPeopleBuildingFunc) func;
    		}
    	}
    	
    	return null;
    }
    
    //建筑属性
    public MarsBuildingValue getBuildingValue() {return _m_bpBuildingValue;}
    
    //计算建筑属性
    public void doLazyCalBuildingProperty() {_m_cdCalBuildingPropertyLazyDealer.setNeedDeal();}
    
	private void _lock() {getUserData().lockUser();}
	private void _unlock() {getUserData().unlockUser();}
	
	/**
	 * 配表初始化完成时触发
	 */
	protected void _onInitedFromRef()
	{
		_m_refLvl = _m_ref.getLevelMapMgr().getLevelData(_m_iBuildingLvl);
		if(null == _m_refLvl)
			return;

		//玩家属性部分
		getUserData().getMarsComponent().getPlayerPropertyContainer().addModifier(_m_refLvl.player_property);
		//火星属性系统属性
		getUserData().getMarsComponent().getMarsPropertyContainer().addModifier(_m_refLvl.mars_property);
	}
    
	/**
	 * 加载数据
	 * @param _bo
	 */
    protected void _loadBo(PlayerMarsBuildingBO _bo) 
	{
    	RefMarsBuildingLevel preLvlRef = _m_refLvl;
    	
		_m_bo = _bo;
		_m_iBuildingLvl = _m_bo.getLvl();
		
		//已经创建的建筑需要获取对应的等级配表
		_m_refLvl = _m_ref.getLevelMapMgr().getLevelData(_m_iBuildingLvl);
		if(null == _m_refLvl)
		{
			USLog.error(getUSServer(), "player:{} building:{} lvl:{} mars building load from bo fail.", getCid(), getBuildingId(), _m_iBuildingLvl);
			return;
		}
		
		//等级改变处理
		if(preLvlRef != _m_refLvl)
		{
			//移除原有配表带来的属性
			if(null != preLvlRef)
			{
				//玩家属性部分
				getUserData().getMarsComponent().getPlayerPropertyContainer().removeModifier(preLvlRef.player_property);
				//火星属性系统属性
				getUserData().getMarsComponent().getMarsPropertyContainer().removeModifier(preLvlRef.mars_property);
			}

			//玩家属性部分
			getUserData().getMarsComponent().getPlayerPropertyContainer().addModifier(_m_refLvl.player_property);
			//火星属性系统属性
			getUserData().getMarsComponent().getMarsPropertyContainer().addModifier(_m_refLvl.mars_property);
		}
		
		//对建筑功能进行检查加载
		for(int i = 0; i < _m_alBuildingFuncList.size(); i++)
		{
			_AMarsBuildingFunc func = _m_alBuildingFuncList.get(i);
			if(null == func)
				continue;
			
			func._onBuildingLoad();
		}
	}
    
    /**
     * 数据加载完成后的后续数据
     */
    protected void _onInited() 
    {
    	for(int i = 0; i < _m_alBuildingFuncList.size(); i++)
		{
			_AMarsBuildingFunc func = _m_alBuildingFuncList.get(i);
			if(null == func)
				continue;
			
			func._onInited();
		}
    }
    
    /**
     * 更新数据
     */
	private void _save()
	{
		if(null == _m_bo)
		{
			PlayerMarsBuildingBO bo = new PlayerMarsBuildingBO();
			bo.setCid(getBM(), getCid());
			bo.setBuildingId(getBM(), getBuildingId());
			bo.setLvl(getBM(), _m_iBuildingLvl);
			bo.setIsFoodPowerOn(getBM(), _m_bIsFoodPowerOn);
			bo.insert(getBM());
			
			_m_bo = bo;
		}
		else
		{
			_m_bo.setLvl(getBM(), _m_iBuildingLvl);
			_m_bo.setIsFoodPowerOn(getBM(), _m_bIsFoodPowerOn);
			_m_bo.saveAll(getBM());
		}
	}

    /**
     * 解锁完成
     * @return
     */
    public boolean isUnlock() {return null != _m_refLvl;}
    /**
     * 创建完成：等级至少1级已经完成
     * @return
     */
    public boolean isBuilt() {return _m_iBuildingLvl > 0;}
    
    /**
     * 计算建筑全部属性
     * @param _bInit 是否初始化
     */
    public void calBuildingAllProperty(boolean _bInit)
    {
    	_lock();
    	
    	try
    	{
    		//重置全部属性
    		_m_bpBuildingValue._resetAll();
    		//计算全部部件属性
    		_m_mgrBuildingEquipmentMgr._calProperty();
    		//计算加成部分
    		_m_bpBuildingValue._calAddPer();
    		
    		//如果不是初始化操作的处理
    		if(!_bInit)
    		{
    			//计算建筑产出
    			calBuildingOutput();
    			
    			//计算全部建筑属性的操作
    			getUserData().getMarsBuildingComponent().doLazyCalAllBuildingValue();
    		}
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /**
     * 计算产出
     */
    public void calBuildingOutput()
    {
    	_lock();
    	
    	try
    	{
    		for(int i = 0; i < _m_alBuildingFuncList.size(); i++)
    		{
    			_AMarsBuildingFunc func = _m_alBuildingFuncList.get(i);
    			if(null == func)
    				continue;
    			
    			if(func instanceof MarsPeopleBuildingFunc)
    			{
    				MarsPeopleBuildingFunc peopleFunc = (MarsPeopleBuildingFunc) func;
    				peopleFunc.doLazyCalOutput();
    			}
    		}
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
	/**
	 * 构造协议数据
	 * @return
	 */
	public Mars_Building toProto()
	{
		Mars_Building proto = new Mars_Building();
		proto.setBuildingId(getBuildingId());
		proto.setLvl(_m_iBuildingLvl);
		
		return proto;
	}
	
	/**
	 * 检查部件解锁
	 * @param _bInit
	 * @param _context
	 */
	public void checkUnlockEquipment(boolean _bInit, NPPlayerContext _context)
	{
		_lock();
		
		try
		{
			if(!isBuilt())
				return;
			
			ArrayList<RefMarsEquipment> equipmentRefList = _m_ref.equipmentRefList;
			for(int i = 0; i < equipmentRefList.size(); i++)
			{
				RefMarsEquipment equipmentRef = equipmentRefList.get(i);
				if(null == equipmentRef)
					continue;
				
				if(getBuildingLvl() < equipmentRef.unlock_level)
					continue;
				
				//执行解锁操作
				_m_mgrBuildingEquipmentMgr.unlock(_bInit, equipmentRef, _context);
			}
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 建造完成（非升级）
	 * @param _context
	 */
	private void _onBuildDone(NPPlayerContext _context)
	{
	    //【GOB-1】引导-居民中心刚解锁时，立即推送一个事件
	    //https://www.teambition.com/task/69708ef85cffe652c0b88f0a
		if(getBuildingId() == RefGeneral.Ref().mars_building_people_id)
		{
			getUserData().getMarsPeopleComponent().getHelpMgr().buildHelp(_context);
            
            //发送宝箱聊天
            RefBoxComm ref = RefBoxComm.getMgr().get(RefGeneral.Ref().mars_go_route_done_box_id);
            if (null == ref)
            {
                USLog.error(getUSServer(), "MarsGoRouteComponent setAllDone, boxRef is null, cid:{} boxId:{}",
                        getUserData().getCid(), RefGeneral.Ref().mars_go_route_done_box_id);
                return;
            }

            //聊天用户数据
            NPCommon_ChatPlayerContent userProto = getUserData().toChatPlayerProto();

            //创建宝箱
            CommBoxInfo boxInfo = getUserData().getUSServer().getCommBoxMgr().buildBox(ref, getUserData().getCid(),
                    NPPlayerContext.createNew(ENPGameEvent.CLIENT_NOTIFY_FUNC_UNLOCK));
            getUserData().getPlayerChatRoomDealer().sendRoomMsg(ENPChatRoomType.US_SERVER.ordinal()
                    , 0
                    , ENPChatMsgType.COMM_BOX.ordinal()
                    , userProto.makePackage()
                    , boxInfo.toChatProto().makePackage()
                    , null);

            // 构造聊天系统消息
            ChatObj_SystemLog proto = new ChatObj_SystemLog();
            proto.setLogType(RefGeneral.Ref().mars_go_route_done_chat_system_log);
            // 发送到联盟聊天频道
            ALSynTaskManager.getInstance().regTask(() ->
                    getUserData().getPlayerChatRoomDealer().sendRoomMsg(ENPChatRoomType.GUILD.ordinal()
                            , 0
                            , ENPChatMsgType.SYSTEM_LOG.ordinal()
                            , userProto.makePackage()
                            , proto.makePackage()
                            , null)
            );
		}
	}
	
	/**
	 * 创建建筑
	 * @param _context
	 * @return
	 */
	public Result build(NPPlayerContext _context)
	{
		_lock();
		
		try
		{
			//已解锁
			if(isBuilt())
				return MarsErr.MARS_BUILDING_BUILT;
			
    		//检查建造队列
			if(getUserData().getMarsBuildingComponent().getBuildingUpQueueMgr().isBuildingUp(getBuildingId()))
				return MarsErr.MARS_BUILDING_BUILT;

			if(!getUserData().getMarsBuildingComponent().getBuildingUpQueueMgr().checkBuildingLimit())
				return MarsErr.MARS_BUILDING_UPGRADING_LIMIT;
			
			//检查下一个等级
			int nextLvl = 1;
			RefMarsBuildingLevel nextLvlRef = _m_ref.getLevelMapMgr().getLevelData(nextLvl);
			if(null == nextLvlRef)
				return CommErr.REF_NOT_FOUND;

			//检查条件
			ArrayList<RefMarsBuildingCondition> upgradeCondList = _m_ref.conditionRefList;
			for(int i = 0; i < upgradeCondList.size(); i++)
			{
				RefMarsBuildingCondition condObj = upgradeCondList.get(i);
				if(null == condObj)
					continue;
				
				if(!NPPlayerConditionDealerMgr.IsEnable(condObj.condition, getUserData(), null))
					return CommErr.CONDITION_NOT_ENABLE;
			}
			
			//子类检查
			for(int i = 0; i < _m_alBuildingFuncList.size(); i++)
			{
				_AMarsBuildingFunc func = _m_alBuildingFuncList.get(i);
				if(null == func)
					continue;
				
				Result subCheck = func._checkUpdateSub(nextLvl);
				if(!subCheck.isSucc())
					return subCheck;
			}
			
			//计算实际消耗
			long costProperty = getUserData().getPlayerComponent().getPropertyMgr().getValue(ENPPlayerPropertyType.MARS_BUILDING_UP_COST_PER);
			NPItemCollector realCostItemObj = new NPItemCollector(_context.getContextId());
			for(int i = 0; i < _m_ref.build_cost_list.size(); i++)
			{
				NPCommonCostItem costItem = _m_ref.build_cost_list.get(i);
				if(null == costItem)
					continue;
				
				long realNum = costItem.getCount() * (10000 - costProperty) / 10000;
				realNum = Math.max(realNum, 1);
				
				NPCommonCostItem realCostItem = costItem.duplicate();
				realCostItem.setCount(realNum);
				realCostItemObj.addItem(realCostItem);
			}
			
			List<NPCommonCostItem> realCostItemList = realCostItemObj.getAllItemList();
			
			//检查消耗
			if(!getUserData().hasCostItemList(realCostItemList))
				return CommErr.ITEM_NOT_ENOUGH;
			
			if(!getUserData().spendCostItemList(realCostItemList, _context))
				return CommErr.CONSUME_FAIL;
			
			//计算建造时长
			long property = getUserData().getPlayerComponent().getPropertyMgr().getValue(ENPPlayerPropertyType.MARS_BUILDING_UP_TIME_PER);
			//GOB-8288 修正所有涉及加速万分比的计算 https://www.teambition.com/task/69593ecb0bc2f864a6a42c82
			//time = time / ((10000 + per)/10000)
			long upgradeMs = _m_ref.build_time_cost_sec * 1000 * 10000 / (10000 + property);
			upgradeMs = Math.max(upgradeMs, 0);//保底为0
			
			//创建建造队列数据
			if(!getUserData().getMarsBuildingComponent().getBuildingUpQueueMgr().addUpQueue(getBuildingId(), nextLvl, upgradeMs, realCostItemList, _context))
				return MarsErr.MARS_BUILDING_BUILD_FAIL;
			
			return Result.SUCC;
		}
		finally 
		{
			_unlock();
		}
	}
	
	/**
	 * 对建筑进行升级
	 * @param _context
	 * @return
	 */
	public Result upgradeLvl(NPPlayerContext _context)
	{
		_lock();
		
		try
		{
			//未解锁
			if(!isBuilt())
				return MarsErr.MARS_BUILDING_NOT_BUILD;
			
			//检查当前等级配置
			if(null == _m_refLvl)
				return CommErr.REF_NOT_FOUND;

    		//检查建造队列
			if(getUserData().getMarsBuildingComponent().getBuildingUpQueueMgr().isBuildingUp(getBuildingId()))
				return MarsErr.MARS_BUILDING_UPGRADING;

			if(!getUserData().getMarsBuildingComponent().getBuildingUpQueueMgr().checkBuildingLimit())
				return MarsErr.MARS_BUILDING_UPGRADING_LIMIT;

			//检查下一个等级
			int nextLvl = _m_refLvl.level + 1;
			RefMarsBuildingLevel nextLvlRef = _m_ref.getLevelMapMgr().getLevelData(nextLvl);
			if(null == nextLvlRef)
				return CommErr.REF_NOT_FOUND;
			
			//检查条件
			ArrayList<RefMarsBuildingCondition> upgradeCondList = _m_refLvl.conditionRefList;
			for(int i = 0; i < upgradeCondList.size(); i++)
			{
				RefMarsBuildingCondition condObj = upgradeCondList.get(i);
				if(null == condObj)
					continue;
				
				if(!NPPlayerConditionDealerMgr.IsEnable(condObj.condition, getUserData(), null))
					return CommErr.CONDITION_NOT_ENABLE;
			}
			
			//检查对应的部件是否满级
			if(!_m_mgrBuildingEquipmentMgr.checkMainEquipLvlFull())
				return MarsErr.MARS_BUILDING_EQUIPMENT_LVL_NOT_FULL;
			
			//子类检查
			for(int i = 0; i < _m_alBuildingFuncList.size(); i++)
			{
				_AMarsBuildingFunc func = _m_alBuildingFuncList.get(i);
				if(null == func)
					continue;
				
				Result subCheck = func._checkUpdateSub(nextLvl);
				if(!subCheck.isSucc())
					return subCheck;
			}
			
			//计算实际消耗
			long costProperty = getUserData().getPlayerComponent().getPropertyMgr().getValue(ENPPlayerPropertyType.MARS_BUILDING_UP_COST_PER);
			NPItemCollector realCostItemObj = new NPItemCollector(_context.getContextId());
			for(int i = 0; i < _m_refLvl.upgrade_cost_list.size(); i++)
			{
				NPCommonCostItem costItem = _m_refLvl.upgrade_cost_list.get(i);
				if(null == costItem)
					continue;
				
				long realNum = costItem.getCount() * (10000 - costProperty) / 10000;
				realNum = Math.max(realNum, 1);

				NPCommonCostItem realCostItem = costItem.duplicate();
				realCostItem.setCount(realNum);
				realCostItemObj.addItem(realCostItem);
			}
			
			List<NPCommonCostItem> realCostItemList = realCostItemObj.getAllItemList();
			
			//检查消耗
			if(!getUserData().hasCostItemList(realCostItemList))
				return CommErr.ITEM_NOT_ENOUGH;
			
			if(!getUserData().spendCostItemList(realCostItemList, _context))
				return CommErr.CONSUME_FAIL;
			
			//计算建造时长
			long property = getUserData().getPlayerComponent().getPropertyMgr().getValue(ENPPlayerPropertyType.MARS_BUILDING_UP_TIME_PER);
			//GOB-8288 修正所有涉及加速万分比的计算 https://www.teambition.com/task/69593ecb0bc2f864a6a42c82
			//time = time / ((10000 + per)/10000)
			long upgradeMs = _m_refLvl.upgrade_time_cost_sec * 1000 * 10000 / (10000 + property);
			upgradeMs = Math.max(upgradeMs, 0);//保底为0

			//创建建造队列数据
			if(!getUserData().getMarsBuildingComponent().getBuildingUpQueueMgr().addUpQueue(getBuildingId(), nextLvl, upgradeMs, realCostItemList, _context))
				return MarsErr.MARS_BUILDING_BUILD_FAIL;
			
			return Result.SUCC;
		}
		finally 
		{
			_unlock();
		}
	}
	
	/**
	 * 完成升级
	 * @param _context
	 * @return
	 */
	public Result doneUpgrade(NPPlayerContext _context)
	{
		_lock();
		
		try
		{
			//检查建造队列
			MarsBuildingUpQueueInfo upQueue = getUserData().getMarsBuildingComponent().getBuildingUpQueueMgr().lookupByBuildingId(getBuildingId());
			if(null == upQueue)
				return MarsErr.MARS_BUILDING_NOT_UPGRADING;
			
			//未到完成时间
			long nowTimeMs = CommonFunc.getNowTimeMS();
			//容错客户端，截至时间提前1秒
			long checkEndMs = upQueue.getFinalEndUpgradeLvlMs() - 1000;
			if(checkEndMs > nowTimeMs)
				return MarsErr.MARS_BUILDING_UPGRADING_NOT_END;
			
			//检查下一个等级
			int nextLvl = _m_iBuildingLvl + 1;
			RefMarsBuildingLevel nextLvlRef = _m_ref.getLevelMapMgr().getLevelData(nextLvl);
			if(null == nextLvlRef)
				return CommErr.REF_NOT_FOUND;
			
			//检查建造队列的目标等级（该错误不该发生）
			if(upQueue.getTargetLvl() != nextLvl)
				return CommErr.UNKNOW_ERR;
			
			//子类检查
			for(int i = 0; i < _m_alBuildingFuncList.size(); i++)
			{
				_AMarsBuildingFunc func = _m_alBuildingFuncList.get(i);
				if(null == func)
					continue;
				
				Result subCheck = func._checkUpdateSub(nextLvl);
				if(!subCheck.isSucc())
					return subCheck;
			}
			
			//是否建造完成（原等级=0）
			boolean isBuildDone = (_m_iBuildingLvl == 0);
			
			//移除建造队列
			getUserData().getMarsBuildingComponent().getBuildingUpQueueMgr().delUpQueue(upQueue, _context);

			//记录原数据
			RefMarsBuildingLevel preLvlRef = _m_refLvl;
			
			//更新数据
			_m_iBuildingLvl = nextLvl;
			_save();
			
			_m_refLvl = nextLvlRef;
			
			//推送协议
			getUserData().sendMsgToGC(US2GCWriter_039_MarsBuildingOp.make_050_OnBuildingChg(this));

			//更新玩家属性
			getUserData().getMarsComponent().getPlayerPropertyContainer().replaceModifier(null == preLvlRef ? null : preLvlRef.player_property, _m_refLvl.player_property);
			//火星属性系统属性
			getUserData().getMarsComponent().getMarsPropertyContainer().replaceModifier(null == preLvlRef ? null : preLvlRef.mars_property, _m_refLvl.mars_property);
			
			//子类建筑数据处理
			for(int i = 0; i < _m_alBuildingFuncList.size(); i++)
			{
				_AMarsBuildingFunc func = _m_alBuildingFuncList.get(i);
				if(null == func)
					continue;
				
				func._doneSub(_context);
			}
			
			//检查对应的部件解锁
			checkUnlockEquipment(false, _context);

			//建造完成处理
			if(isBuildDone)
			{
				_onBuildDone(_context);
			}
			
			//计算火星属性
			getUserData().getMarsComponent().doLazyCalMarsProperty();
			//计算火星实力
			getUserData().getMarsComponent().doLazyCalMarsPower();
			
			return Result.SUCC;
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 立即完成创建/升级
	 * 
	 * 优化记录列表：
	 * 20251018【优化-1】火星基地-建筑升级逻辑优化 https://www.teambition.com/task/68f0b641e67aaa40f1d3e0ea
	 * 
	 * @param _context
	 * @return
	 */
	public Result setBuildingDone(NPPlayerContext _context)
	{
		_lock();
		
		try
		{
			//检查配置
			int diamondRatio = RefGeneral.Ref().mars_building_sec_to_diamond_ratio;
			if(diamondRatio <= 0)
				return CommErr.REF_ERROR;

			//检查下一个等级
			int nextLvl = _m_iBuildingLvl + 1;
			RefMarsBuildingLevel nextLvlRef = _m_ref.getLevelMapMgr().getLevelData(nextLvl);
			if(null == nextLvlRef)
				return CommErr.REF_NOT_FOUND;

			//记录原数据
			RefMarsBuildingLevel preLvlRef = _m_refLvl;
			
			//【优化-1】火星基地-建筑升级逻辑优化 https://www.teambition.com/task/68f0b641e67aaa40f1d3e0ea
			//建造中状态-》完成当前建造
			//未建造状态-》完成下一个等级
			MarsBuildingUpQueueInfo upQueue = getUserData().getMarsBuildingComponent().getBuildingUpQueueMgr().lookupByBuildingId(getBuildingId());
			if(null != upQueue) //未完成建造-》完成当前建造
			{
				//检查目标等级（不应该出现问题）
				if(upQueue.getTargetLvl() != nextLvl)
					return CommErr.UNKNOW_ERR;
				
				//计算时长
				long needMs = upQueue.getFinalEndUpgradeLvlMs() - CommonFunc.getNowTimeMS();
				int num = (int) Math.ceil(1.0f * needMs / (diamondRatio * 1000f));
				
				//检查并扣除钻石
				if(!getUserData().hasItem(ENPItemType.CURRENCY, ECurrency.GEM.ordinal(), num))
					return CommErr.ITEM_NOT_ENOUGH;
				
				if(!getUserData().spendItem(ENPItemType.CURRENCY, ECurrency.GEM.ordinal(), num, _context))
					return CommErr.CONSUME_FAIL;
				
				//移除建筑队列数据
				getUserData().getMarsBuildingComponent().getBuildingUpQueueMgr().delUpQueue(upQueue, _context);
			}
			else if(isBuilt()) //已建造，立即升级
			{
				//检查当前等级配置
				if(null == _m_refLvl)
					return CommErr.REF_NOT_FOUND;
				
				//检查条件
				ArrayList<RefMarsBuildingCondition> upgradeCondList = _m_refLvl.conditionRefList;
				for(int i = 0; i < upgradeCondList.size(); i++)
				{
					RefMarsBuildingCondition condObj = upgradeCondList.get(i);
					if(null == condObj)
						continue;
					
					if(!NPPlayerConditionDealerMgr.IsEnable(condObj.condition, getUserData(), null))
						return CommErr.CONDITION_NOT_ENABLE;
				}
				
				//检查对应的部件是否满级
				if(!_m_mgrBuildingEquipmentMgr.checkMainEquipLvlFull())
					return MarsErr.MARS_BUILDING_EQUIPMENT_LVL_NOT_FULL;
				
				//子类检查
				for(int i = 0; i < _m_alBuildingFuncList.size(); i++)
				{
					_AMarsBuildingFunc func = _m_alBuildingFuncList.get(i);
					if(null == func)
						continue;
					
					Result subCheck = func._checkUpdateSub(nextLvl);
					if(!subCheck.isSucc())
						return subCheck;
				}
				
				//计算实际消耗
				long costProperty = getUserData().getPlayerComponent().getPropertyMgr().getValue(ENPPlayerPropertyType.MARS_BUILDING_UP_COST_PER);
				NPItemCollector realCostItemObj = new NPItemCollector(_context.getContextId());
				for(int i = 0; i < _m_refLvl.upgrade_cost_list.size(); i++)
				{
					NPCommonCostItem costItem = _m_refLvl.upgrade_cost_list.get(i);
					if(null == costItem)
						continue;
					
					long realNum = costItem.getCount() * (10000 - costProperty) / 10000;
					realNum = Math.max(realNum, 1);

					NPCommonCostItem realCostItem = costItem.duplicate();
					realCostItem.setCount(realNum);
					realCostItemObj.addItem(realCostItem);
				}
				
				//计算截至时间所需的钻石
				long property = getUserData().getPlayerComponent().getPropertyMgr().getValue(ENPPlayerPropertyType.MARS_BUILDING_UP_TIME_PER);
				//GOB-8288 修正所有涉及加速万分比的计算 https://www.teambition.com/task/69593ecb0bc2f864a6a42c82
				//time = time / ((10000 + per)/10000)
				long needMs = _m_ref.build_time_cost_sec * 1000 * 10000 / (10000 + property);
				needMs = Math.max(needMs, 0);//保底为0
				int num = (int) Math.ceil(1.0f * needMs / (diamondRatio * 1000f));
				realCostItemObj.addItem(new NPCommonCostItem(ENPItemType.CURRENCY, ECurrency.GEM.ordinal(), num));

				List<NPCommonCostItem> realCostItemList = realCostItemObj.getAllItemList();
				
				//检查消耗
				if(!getUserData().hasCostItemList(realCostItemList))
					return CommErr.ITEM_NOT_ENOUGH;
				
				if(!getUserData().spendCostItemList(realCostItemList, _context))
					return CommErr.CONSUME_FAIL;
			}
			else //未建造，立即建造
			{
				//检查条件
				ArrayList<RefMarsBuildingCondition> upgradeCondList = _m_ref.conditionRefList;
				for(int i = 0; i < upgradeCondList.size(); i++)
				{
					RefMarsBuildingCondition condObj = upgradeCondList.get(i);
					if(null == condObj)
						continue;
					
					if(!NPPlayerConditionDealerMgr.IsEnable(condObj.condition, getUserData(), null))
						return CommErr.CONDITION_NOT_ENABLE;
				}
				
				//子类检查
				for(int i = 0; i < _m_alBuildingFuncList.size(); i++)
				{
					_AMarsBuildingFunc func = _m_alBuildingFuncList.get(i);
					if(null == func)
						continue;
					
					Result subCheck = func._checkUpdateSub(nextLvl);
					if(!subCheck.isSucc())
						return subCheck;
				}

				//计算实际消耗
				long costProperty = getUserData().getPlayerComponent().getPropertyMgr().getValue(ENPPlayerPropertyType.MARS_BUILDING_UP_COST_PER);
				NPItemCollector realCostItemObj = new NPItemCollector(_context.getContextId());
				for(int i = 0; i < _m_ref.build_cost_list.size(); i++)
				{
					NPCommonCostItem costItem = _m_ref.build_cost_list.get(i);
					if(null == costItem)
						continue;
					
					long realNum = (long) Math.ceil(1.0f * costItem.getCount() * (1 + 1.0f * costProperty / 10000f));
					if(realNum > 0)
					{
						NPCommonCostItem realCostItem = costItem.duplicate();
						realCostItem.setCount(realNum);
						
						realCostItemObj.addItem(realCostItem);
					}
				}
				
				//计算建造时长所需的钻石
				long property = getUserData().getPlayerComponent().getPropertyMgr().getValue(ENPPlayerPropertyType.MARS_BUILDING_UP_TIME_PER);
				long needMs = _m_ref.build_time_cost_sec * 1000 * 10000 / (10000 + property);
				needMs = Math.max(needMs, 0);//保底为0
				int num = (int) Math.ceil(1.0f * needMs / (diamondRatio * 1000f));
				realCostItemObj.addItem(new NPCommonCostItem(ENPItemType.CURRENCY, ECurrency.GEM.ordinal(), num));

				List<NPCommonCostItem> realCostItemList = realCostItemObj.getAllItemList();
				
				//检查消耗
				if(!getUserData().hasCostItemList(realCostItemList))
					return CommErr.ITEM_NOT_ENOUGH;
				
				if(!getUserData().spendCostItemList(realCostItemList, _context))
					return CommErr.CONSUME_FAIL;
			}

			//是否建造完成
			boolean isBuildDone = (_m_iBuildingLvl == 0);
			
			//更新数据
			_m_iBuildingLvl = nextLvl;
			_save();
			
			_m_refLvl = nextLvlRef;
			
			//推送协议
			getUserData().sendMsgToGC(US2GCWriter_039_MarsBuildingOp.make_050_OnBuildingChg(this));

			//建造完成处理
			if(isBuildDone)
			{
				_onBuildDone(_context);
			}

			//更新玩家属性
			getUserData().getMarsComponent().getPlayerPropertyContainer().replaceModifier(null == preLvlRef ? null : preLvlRef.player_property, _m_refLvl.player_property);
			//火星属性系统属性
			getUserData().getMarsComponent().getMarsPropertyContainer().replaceModifier(null == preLvlRef ? null : preLvlRef.mars_property, _m_refLvl.mars_property);
			
			//子类建筑数据处理
			for(int i = 0; i < _m_alBuildingFuncList.size(); i++)
			{
				_AMarsBuildingFunc func = _m_alBuildingFuncList.get(i);
				if(null == func)
					continue;
				
				func._doneSub(_context);
			}
			
			//检查对应的部件解锁
			checkUnlockEquipment(false, _context);

			//计算火星属性
			getUserData().getMarsComponent().doLazyCalMarsProperty();
			//计算火星实力
			getUserData().getMarsComponent().doLazyCalMarsPower();
			
			return Result.SUCC;
		}
		finally
		{
			_unlock();
		}
	}

	/**
	 * GM命令：直接建造建筑
	 * 不检查条件,不消耗资源,直接建造到1级
	 *
	 * @param _context 操作上下文
	 * @return 建造结果
	 */
	public Result cmdGmBuild(NPPlayerContext _context)
	{
		_lock();

		try
		{
			//已经建造,直接返回
			if(isBuilt())
				return Result.SUCC;

			//获取1级配置
			RefMarsBuildingLevel lvl1Ref = _m_ref.getLevelMapMgr().getLevelData(1);
			if(null == lvl1Ref)
				return CommErr.REF_NOT_FOUND;

			//移除建造队列(如果有)
			MarsBuildingUpQueueInfo upQueue = getUserData().getMarsBuildingComponent().getBuildingUpQueueMgr().lookupByBuildingId(getBuildingId());
			if(null != upQueue)
			{
				getUserData().getMarsBuildingComponent().getBuildingUpQueueMgr().delUpQueue(upQueue, _context);
			}

			//记录原数据
			RefMarsBuildingLevel preLvlRef = _m_refLvl;

			//更新数据
			_m_iBuildingLvl = 1;
			_save();

			_m_refLvl = lvl1Ref;

			//推送协议
			getUserData().sendMsgToGC(US2GCWriter_039_MarsBuildingOp.make_050_OnBuildingChg(this));

			//建造完成处理
			_onBuildDone(_context);

			//更新玩家属性
			getUserData().getMarsComponent().getPlayerPropertyContainer().replaceModifier(null == preLvlRef ? null : preLvlRef.player_property, _m_refLvl.player_property);
			//火星属性系统属性
			getUserData().getMarsComponent().getMarsPropertyContainer().replaceModifier(null == preLvlRef ? null : preLvlRef.mars_property, _m_refLvl.mars_property);

			//子类建筑数据处理
			for(int i = 0; i < _m_alBuildingFuncList.size(); i++)
			{
				_AMarsBuildingFunc func = _m_alBuildingFuncList.get(i);
				if(null == func)
					continue;

				func._doneSub(_context);
			}

			//检查对应的部件解锁
			checkUnlockEquipment(false, _context);

			//计算建筑属性
			calBuildingAllProperty(false);

			//计算火星属性
			getUserData().getMarsComponent().doLazyCalMarsProperty();
			//计算火星实力
			getUserData().getMarsComponent().doLazyCalMarsPower();

			return Result.SUCC;
		}
		finally
		{
			_unlock();
		}
	}

	/**
	 * 设置food部件开关
	 * @param _powerOn
	 * @param _context
	 */
	public void setFoodPower(boolean _powerOn, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			//更新数据
			_m_bIsFoodPowerOn = _powerOn;
			_save();
		
			//重新计算
			doLazyCalBuildingProperty();
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	@Override
	public String toString()
	{
		_lock();
		
		try
		{
			StringBuilder sb = new StringBuilder();
			
			//基础数据
			sb.append("\nid:").append(getBuildingId());
			sb.append("\ntype:").append(getRef().building_type);
			sb.append("\nlvl:").append(getBuildingLvl());
			sb.append("\nisFoodPowerOn:").append(_m_bIsFoodPowerOn);
			
			//尚未解锁
			if(!isUnlock())
			{
				sb.append("\nnot unlock");
			}
			
			//建造数据
			MarsBuildingUpQueueInfo upQueue = getUserData().getMarsBuildingComponent().getBuildingUpQueueMgr().lookup(getBuildingId());
			if(null == upQueue)
			{
				sb.append("\nnot upgrading.");
			}
			else
			{
				sb.append("\nupgrading, start:").append(CommonFunc.getTimeStringMs(upQueue.getStartUpgradeLvlMs()))
					.append(", end:").append(CommonFunc.getTimeStringMs(upQueue.getFinalEndUpgradeLvlMs()))
					.append(", targetLvl:").append(upQueue.getTargetLvl());
			}

			//计算建筑属性
			calBuildingAllProperty(false);
			
			//尚未解锁，无需打印建筑功能数据
			if(!isUnlock())
			{
				return sb.toString();
			}
			
			//功能数据
			for(int i = 0; i < _m_alBuildingFuncList.size(); i++)
			{
				_AMarsBuildingFunc func = _m_alBuildingFuncList.get(i);
				if(null == func)
					continue;
				
				sb.append("\n--- func ---");
				sb.append(func.toString());
			}
			
			//部件数据
			sb.append(getBuildingEquipmentMgr().toString());
			
			//属性数据
			sb.append("\n--- value ---");
			sb.append(getBuildingValue().toString());
			
			return sb.toString();
		}
		finally
		{
			_unlock();
		}
	}
}
