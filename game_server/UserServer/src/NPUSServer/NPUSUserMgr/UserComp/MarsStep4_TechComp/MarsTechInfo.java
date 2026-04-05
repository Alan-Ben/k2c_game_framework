package NPUSServer.NPUSUserMgr.UserComp.MarsStep4_TechComp;

import ALBasicProtocolPack._IALProtocolStructure;
import Common.GuildEnum.EGuildMarsHelpObjType;
import Common.GuildObj.Guild_MarsHelp_TechUp;
import Common.MarsObj.Mars_Technology;
import CommonEnum.ECurrency;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.NPCommon_ItemList;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPItemType;
import NPEnum.ENPPlayerPropertyType;
import NPGameRes.Refs.Mars.RefMarsBuildingCondition;
import NPGameRes.Refs.Mars.RefMarsTechnology;
import NPGameRes.Refs.Mars.RefMarsTechnologyLevel;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MarsComp._IGuildMarsHelp;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_039_MarsBuildingOp;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.PlayerMarsTechBO;

import java.nio.ByteBuffer;
import java.util.ArrayList;

public class MarsTechInfo implements _IGuildMarsHelp
{
	//玩家数据对象
	private NPUSUserData _m_udUserData;

	//基础配置
	private RefMarsTechnology _m_ref;
	
	//科技等级
	private int _m_iLvl;
	//是否升级标志
	private boolean _m_bIsUpgrading;
	//开启升级时间
	private long _m_lStartUpgradeLvlMs;
	//结束升级时间
	private long _m_lEndUpgradeLvlMs;
	
	//公会求助ID
	private long _m_lGuildHelpId;
	//公会助力时长（秒）
	private int _m_iGuildHelpSecs;
	
	//道具加速时长（秒）
	private int _m_iItemHelpSecs;
	
	//等级配置
	private RefMarsTechnologyLevel _m_refLvl;
	
	//数据bo
	private PlayerMarsTechBO _m_bo;
	
	//升级消耗道具列表
	private NPCommon_ItemList _m_ilUpgradeCostItemList;
	
	public MarsTechInfo(NPUSUserData _userData, RefMarsTechnology _ref)
	{
		_m_udUserData = _userData;
		
		_m_ref = _ref;
		
		_m_ilUpgradeCostItemList = new NPCommon_ItemList();
		
		_initLvl();
	}
	
	public NPUSUserData getUserData() {return _m_udUserData;}
    public NPUserServer getUSServer() {return getUserData().getUSServer();}
    public long getCid() {return getUserData().getCid();}
    public BM getBM() {return getUserData().getUSServer().getBM();}

    public RefMarsTechnology getRef() {return _m_ref;}
    public long getTechId() {return _m_ref.id;}
    
    public int getLvl() {return _m_iLvl;}
    public RefMarsTechnologyLevel getLvlRef() {return _m_refLvl;}
    public long getMarsPower() {return null == _m_refLvl ? 0 : _m_refLvl.mars_power;}

    public boolean isUnlock() {return _m_iLvl > 0;}
    
    public boolean isUpgrading() {return _m_bIsUpgrading;}
    public long getStartUpgradeLvlMs() {return _m_lStartUpgradeLvlMs;}
    public long getEndUpgradeLvlMs() {return _m_lEndUpgradeLvlMs;}

    public long getGuildHelpId() {return _m_lGuildHelpId;}
    public int getGuildHelpSecs() {return _m_iGuildHelpSecs;}
    
    public int getItemHelpSecs() {return _m_iItemHelpSecs;}

    /**
     * 最终截至时间，需要计算所有补助时间
     * @return
     */
    public long getFinalEndUpgradeLvlMs() 
    {
    	return _m_lEndUpgradeLvlMs - _m_iGuildHelpSecs * 1000 - _m_iItemHelpSecs * 1000;
    }
    
    /**
     * 默认0级开始
     */
    private void _initLvl() 
    {
    	_m_refLvl = _m_ref.getLevelMapMgr().getLevelData(_m_iLvl);
    	if(null == _m_refLvl)
    	{
    		USLog.error(getUSServer(), "player:{} lvl:{} mars tech lvl init fail, not find lvl ref.", getCid(), _m_iLvl);
    		return;
    	}
	}
    
    protected void _loadBo(PlayerMarsTechBO _bo) 
    {
    	_m_bo = _bo;

    	_m_iLvl = _bo.getLvl();
		_m_bIsUpgrading = _bo.getIsUpgrading();
		_m_lStartUpgradeLvlMs = _bo.getStartUpgradeLvlMs();
		_m_lEndUpgradeLvlMs = _bo.getEndUpgradeLvlMs();
		if(null != _m_bo.getUpgradeCost())
		{
			ByteBuffer buff = ByteBuffer.wrap(_m_bo.getUpgradeCost());
			_m_ilUpgradeCostItemList.readPackage(buff);
		}
		
		_m_lGuildHelpId = _bo.getGuildHelpId();
		_m_iGuildHelpSecs = _bo.getGuildHelpSecs();
		
		//记录原数据
    	RefMarsTechnologyLevel preLvlRef = _m_refLvl;
		
		//更新当前等级数据
    	_m_refLvl = _m_ref.getLevelMapMgr().getLevelData(_m_iLvl);
    	if(null == _m_refLvl)
    	{
    		USLog.error(getUSServer(), "player:{} lvl:{} mars tech lvl load fail, not find lvl ref.", getCid(), _m_iLvl);
    		return;
        }
    	
    	//更新玩家属性
		getUserData().getMarsComponent().getPlayerPropertyContainer().replaceModifier(null == preLvlRef ? null : preLvlRef.player_property, _m_refLvl.player_property);
		//火星属性系统属性
		getUserData().getMarsComponent().getMarsPropertyContainer().replaceModifier(null == preLvlRef ? null : preLvlRef.mars_property, _m_refLvl.mars_property);
	}
    
    protected void _onInited() 
    {
		//注册公会求助目标数据
		getUserData().getMarsComponent().getGuildMarsHelpMgr().regMarsHelpObj(this);
	}
    
    /**
     * 保存数据
     */
    private void _save()
    {
    	if(null == _m_bo)
    	{
    		PlayerMarsTechBO bo = new PlayerMarsTechBO();
    		bo.setCid(getBM(), getCid());
    		bo.setTechId(getBM(), getTechId());
    		bo.setLvl(getBM(), _m_iLvl);
    		bo.setIsUpgrading(getBM(), _m_bIsUpgrading);
    		bo.setStartUpgradeLvlMs(getBM(), _m_lStartUpgradeLvlMs);
    		bo.setEndUpgradeLvlMs(getBM(), _m_lEndUpgradeLvlMs);
			bo.setUpgradeCost(getBM(), CommonFunc.ByteBfferToBytes(_m_ilUpgradeCostItemList.makePackage()));
			bo.setGuildHelpId(getBM(), _m_lGuildHelpId);
			bo.setGuildHelpSecs(getBM(), _m_iGuildHelpSecs);
			bo.setItemHelpSecs(getBM(), _m_iItemHelpSecs);
    		bo.insert(getBM());
    		
    		_m_bo = bo;
    	}
    	else
    	{
    		_m_bo.setLvl(getBM(), _m_iLvl);
    		_m_bo.setIsUpgrading(getBM(), _m_bIsUpgrading);
    		_m_bo.setStartUpgradeLvlMs(getBM(), _m_lStartUpgradeLvlMs);
    		_m_bo.setEndUpgradeLvlMs(getBM(), _m_lEndUpgradeLvlMs);
    		_m_bo.setUpgradeCost(getBM(), CommonFunc.ByteBfferToBytes(_m_ilUpgradeCostItemList.makePackage()));
    		_m_bo.setGuildHelpId(getBM(), _m_lGuildHelpId);
    		_m_bo.setGuildHelpSecs(getBM(), _m_iGuildHelpSecs);
    		_m_bo.setItemHelpSecs(getBM(), _m_iItemHelpSecs);
    		_m_bo.saveAllMarked(getBM());
    	}
    }
    
    /**
     * 构造协议数据
     * @return
     */
    public Mars_Technology toProto()
    {
    	Mars_Technology proto = new Mars_Technology();
    	proto.setTechnologyId(getTechId());
    	proto.setLvl(getLvl());
    	proto.setIsUpgrading(isUpgrading());
    	proto.setStartUpgradeLvlMs(getStartUpgradeLvlMs());
    	proto.setEndUpgradeLvlMs(getEndUpgradeLvlMs());
    	proto.setGuildHelpId(getGuildHelpId());
    	proto.setGuildHelpSecs(getGuildHelpSecs());
    	proto.setItemHelpSecs(getItemHelpSecs());
    	
    	return proto;
    }
	
	/**
	 * 加速升级
	 * @param _reduceSecs
	 * @param _context
	 * @return 实际减少的秒数
	 */
	public int reduceSecs(int _reduceSecs, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			//当前无升级
			if(_m_lEndUpgradeLvlMs == 0)
				return 0;
			
			//计算并更新数据
			long remainMs = getFinalEndUpgradeLvlMs() - CommonFunc.getNowTimeMS();
			//无需加速
			if(remainMs <= 0)
				return 0;
			
			//计算加速时长
			long realReduceMs = Math.min((_reduceSecs * 1000), remainMs);
			int realReduceSecs = (int) Math.ceil(realReduceMs / 1000f);
			realReduceSecs = Math.max(realReduceSecs, 0);
			//更新加速时长（秒）
			if(realReduceSecs > 0)
			{
				_m_iItemHelpSecs += realReduceSecs;
				_save();
			}
			
			return realReduceSecs;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
    
    /**
     * 升级科技等级
     * @param _context
     * @return
     */
    public Result upgradeLvl(NPPlayerContext _context)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		if(null == _m_refLvl)
    			return CommErr.OBJ_ERR;
    		
    		//正在升级
    		if(_m_bIsUpgrading)
    			return MarsErr.MARS_TECH_UPGRADING;
    		
    		//检查下一等级数据
    		int nextLvl = _m_iLvl + 1;
    		RefMarsTechnologyLevel nextLvlRef = _m_ref.getLevelMapMgr().getLevelData(nextLvl);
    		if(null == nextLvlRef)
    			return CommErr.REF_NOT_FOUND;
    		
    		//检查前置科技是否解锁
    		if(!getUserData().getMarsTechComponent().checkUnlockParent(_m_ref.parent_list))
    			return MarsErr.MARS_TECH_PARENT_NOT_UNLOCK;

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
			
			//检查消耗
			long costProperty = getUserData().getPlayerComponent().getPropertyMgr().getValue(ENPPlayerPropertyType.MARS_TECH_COST_PER);
			ArrayList<NPCommonCostItem> realCostItemList = new ArrayList<>();
			for(int i = 0; i < _m_refLvl.upgrade_consume_list.size(); i++)
			{
				NPCommonCostItem costItem = _m_refLvl.upgrade_consume_list.get(i);
				if(null == costItem)
					continue;
				
				long realNum = costItem.getCount() * (10000 - costProperty) / 10000;
				realNum = Math.max(realNum, 1);
				
				NPCommonCostItem realCostItem = costItem.duplicate();
				realCostItem.setCount(realNum);	
				realCostItemList.add(realCostItem);
			}
			
			if(!getUserData().hasCostItemList(realCostItemList))
				return CommErr.ITEM_NOT_ENOUGH;
			
			if(!getUserData().spendCostItemList(realCostItemList, _context))
				return CommErr.CONSUME_FAIL;
    		
    		//更新数据
			_m_bIsUpgrading = true;
			_m_lStartUpgradeLvlMs = CommonFunc.getNowTimeMS();
			long property = getUserData().getPlayerComponent().getPropertyMgr().getValue(ENPPlayerPropertyType.MARS_TECH_SPEED_PER);
			//GOB-8288 修正所有涉及加速万分比的计算 https://www.teambition.com/task/69593ecb0bc2f864a6a42c82
			//time = time / ((10000 + per)/10000)
			long upgradeMs = _m_refLvl.upgrade_time_sec * 1000 * 10000 / (10000 + property);
			upgradeMs = Math.max(upgradeMs, 0);//保底为0
			_m_lEndUpgradeLvlMs = _m_lStartUpgradeLvlMs + upgradeMs;
			//升级消耗的材料需要记录，用于取消时返还
			_m_ilUpgradeCostItemList.getItemList().clear();
			for(int i = 0; i < realCostItemList.size(); i++)
			{
				NPCommonCostItem item = realCostItemList.get(i);
				if(null == item)
					continue;
				
				_m_ilUpgradeCostItemList.getItemList().add(item.toProto());
			}
    		_save();
    		
    		//推送数据
    		getUserData().sendMsgToGC(US2GCWriter_039_MarsBuildingOp.make_060_OnMarsTechnologyChg(this));
    		
    		return Result.SUCC;
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 确认升级
     * @param _context
     * @return
     */
    public Result doneUpgradeLvl(NPPlayerContext _context)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		if(null == _m_refLvl)
    			return CommErr.OBJ_ERR;
    		
    		//不在升级状态
    		if(!_m_bIsUpgrading)
    			return MarsErr.MARS_TECH_NOT_UPGRADING;
    		
    		//截至时间
    		if(CommonFunc.getNowTimeMS() <= getFinalEndUpgradeLvlMs())
    			return MarsErr.MARS_TECH_UPGRADING_SECS_NOT_FULL;
    		
    		//检查下一等级数据
    		int nextLvl = _m_iLvl + 1;
    		RefMarsTechnologyLevel nextLvlRef = _m_ref.getLevelMapMgr().getLevelData(nextLvl);
    		if(null == nextLvlRef)
    			return CommErr.REF_NOT_FOUND;

			//重置公会求助
			getUserData().getMarsComponent().getGuildMarsHelpMgr().unsetHelp(this);
    		
    		//原配置数据
    		RefMarsTechnologyLevel preLvlRef = _m_refLvl;
    		
    		//更新数据
    		_m_iLvl = nextLvl;
			_m_bIsUpgrading = false;
			_m_lStartUpgradeLvlMs = 0;
			_m_lEndUpgradeLvlMs = 0;
			_m_lGuildHelpId = 0;
			_m_iGuildHelpSecs = 0;
			_m_iItemHelpSecs = 0;
    		_save();
    		
    		_m_refLvl = nextLvlRef;
    		
			//更新玩家属性
			getUserData().getMarsComponent().getPlayerPropertyContainer().replaceModifier(null == preLvlRef ? null : preLvlRef.player_property, _m_refLvl.player_property);
			//火星属性系统属性
			getUserData().getMarsComponent().getMarsPropertyContainer().replaceModifier(null == preLvlRef ? null : preLvlRef.mars_property, _m_refLvl.mars_property);

    		//推送数据
    		getUserData().sendMsgToGC(US2GCWriter_039_MarsBuildingOp.make_060_OnMarsTechnologyChg(this));

			//计算火星属性
			getUserData().getMarsComponent().doLazyCalMarsProperty();
			//计算火星实力
			getUserData().getMarsComponent().doLazyCalMarsPower();
    		
    		return Result.SUCC;
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 取消升级
     * @param _context
     * @return
     */
    public Result cancelUpgradeLvl(NPPlayerContext _context)
    {
    	getUserData().lockUser();
		
		try
		{
			//检查升级状态
			if(!_m_bIsUpgrading)
				return MarsErr.MARS_TECH_NOT_UPGRADING;
			
			//重置公会求助
			getUserData().getMarsComponent().getGuildMarsHelpMgr().unsetHelp(this);
			
			//获取对应的升级消耗，用于返还
			NPCommon_ItemList costItemList = new NPCommon_ItemList();
			costItemList.getItemList().addAll(_m_ilUpgradeCostItemList.getItemList());
			
			//更新数据
			_m_bIsUpgrading = false;
			_m_ilUpgradeCostItemList.getItemList().clear();
			_m_lGuildHelpId = 0;
			_m_iGuildHelpSecs = 0;
			_m_iItemHelpSecs = 0;
			//保存数据
			_save();
			
			//推送协议
			getUserData().sendMsgToGC(US2GCWriter_039_MarsBuildingOp.make_060_OnMarsTechnologyChg(this));
			
			//返还消耗
			getUserData().gainItemListP(costItemList, _context);
			
			return Result.SUCC;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
    
    /**
     * 立即完成
     * @param _context
     * @return
     */
    public Result setDone(NPPlayerContext _context)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		if(null == _m_refLvl)
    			return CommErr.OBJ_ERR;

			//检查配置
			int diamondRatio = RefGeneral.Ref().mars_building_sec_to_diamond_ratio;
			if(diamondRatio <= 0)
				return CommErr.REF_ERROR;

    		//检查下一等级数据
    		int nextLvl = _m_iLvl + 1;
    		RefMarsTechnologyLevel nextLvlRef = _m_ref.getLevelMapMgr().getLevelData(nextLvl);
    		if(null == nextLvlRef)
    			return CommErr.REF_NOT_FOUND;
    		
    		//检查前置科技是否解锁
    		if(!getUserData().getMarsTechComponent().checkUnlockParent(_m_ref.parent_list))
    			return MarsErr.MARS_TECH_PARENT_NOT_UNLOCK;
    		
			if(_m_bIsUpgrading) //完成升级
			{
				//计算消耗钻石数
				long needMs = getFinalEndUpgradeLvlMs() - CommonFunc.getNowTimeMS();
				int num = (int) Math.ceil(1.0f * needMs / (diamondRatio * 1000f));
				//检查并扣除钻石
				if(!getUserData().hasItem(ENPItemType.CURRENCY, ECurrency.GEM.ordinal(), num))
					return CommErr.ITEM_NOT_ENOUGH;
				
				if(!getUserData().spendItem(ENPItemType.CURRENCY, ECurrency.GEM.ordinal(), num, _context))
					return CommErr.CONSUME_FAIL;
			}
			else //直接设置下一个等级
			{
				//计算消耗钻石数
				long property = getUserData().getPlayerComponent().getPropertyMgr().getValue(ENPPlayerPropertyType.MARS_TECH_SPEED_PER);
				//GOB-8288 修正所有涉及加速万分比的计算 https://www.teambition.com/task/69593ecb0bc2f864a6a42c82
				//time = time / ((10000 + per)/10000)
				long needMs = _m_refLvl.upgrade_time_sec * 1000 * 10000 / (10000 + property);
				needMs = Math.max(needMs, 0);//保底为0
				int num = (int) Math.ceil(1.0f * needMs / (diamondRatio * 1000f));
				//检查并扣除钻石
				if(!getUserData().hasItem(ENPItemType.CURRENCY, ECurrency.GEM.ordinal(), num))
					return CommErr.ITEM_NOT_ENOUGH;
				
				if(!getUserData().spendItem(ENPItemType.CURRENCY, ECurrency.GEM.ordinal(), num, _context))
					return CommErr.CONSUME_FAIL;
				
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
	    		
	    		//重置消耗
				_m_ilUpgradeCostItemList.getItemList().clear();
			}

			//重置公会求助
			getUserData().getMarsComponent().getGuildMarsHelpMgr().unsetHelp(this);
			
    		//原配置数据
    		RefMarsTechnologyLevel preLvlRef = _m_refLvl;
    		
    		//更新数据
    		_m_iLvl = nextLvl;
			_m_bIsUpgrading = false;
			_m_lStartUpgradeLvlMs = 0;
			_m_lEndUpgradeLvlMs = 0;
			_m_lGuildHelpId = 0;
			_m_iGuildHelpSecs = 0;
			_m_iItemHelpSecs = 0;
    		_save();

    		_m_refLvl = nextLvlRef;
    		
    		//更新玩家属性
			getUserData().getMarsComponent().getPlayerPropertyContainer().replaceModifier(null == preLvlRef ? null : preLvlRef.player_property, _m_refLvl.player_property);
			//火星属性系统属性
			getUserData().getMarsComponent().getMarsPropertyContainer().replaceModifier(null == preLvlRef ? null : preLvlRef.mars_property, _m_refLvl.mars_property);

    		//推送数据
    		getUserData().sendMsgToGC(US2GCWriter_039_MarsBuildingOp.make_060_OnMarsTechnologyChg(this));

			//计算火星属性
			getUserData().getMarsComponent().doLazyCalMarsProperty();
			//计算火星实力
			getUserData().getMarsComponent().doLazyCalMarsPower();
			
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
    		sb.append("\ntech:").append(getTechId());
    		sb.append("\ntechLvl:").append(getLvl());
    		sb.append("\ntechPower:").append(getMarsPower());
    		
    		return sb.toString();
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
	}

	//--------------------------- 求助对象接口方法 ---------------------------//
	@Override
	public EGuildMarsHelpObjType getObjType() 
	{
		return EGuildMarsHelpObjType.TECH_UP;
	}
	
	@Override
	public long getObjId() 
	{
		return getTechId();
	}

	@Override
	public _IALProtocolStructure makeExt() 
	{
		Guild_MarsHelp_TechUp proto = new Guild_MarsHelp_TechUp();
		proto.setTechId(getTechId());
		proto.setLvl(getLvl() + 1);
		
		return proto;
	}

	@Override
	public long getHelpId() 
	{
		return getGuildHelpId();
	}

	@Override
	public int getHelpSecs() 
	{
		return getGuildHelpSecs();
	}

	@Override
	public boolean canSendHelp(EGuildMarsHelpObjType _objType)
	{
		return isUpgrading() && getGuildHelpId() <= 0;
	}

	@Override
	public void setSendHelp(long _helpId) 
	{
		getUserData().lockUser();
		
		try
		{
			_m_lGuildHelpId = _helpId;
			_save();
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

	@Override
	public void updateHelpSecs(long _helpId, int _helpSecs) 
	{
		getUserData().lockUser();
		
		try
		{
			//请求数据
			if(_m_lGuildHelpId != _helpId)
				return;
			
			//检查互助时长（只大不小）
			if(_helpSecs <= _m_iGuildHelpSecs)
				return;
			
			//帮助时间增大，不会变小
			_m_iGuildHelpSecs = _helpSecs;
			_save();
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
}
