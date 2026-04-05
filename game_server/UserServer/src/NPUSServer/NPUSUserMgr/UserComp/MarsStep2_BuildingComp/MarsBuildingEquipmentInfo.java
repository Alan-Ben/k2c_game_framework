package NPUSServer.NPUSUserMgr.UserComp.MarsStep2_BuildingComp;

import Common.MarsObj.Mars_BuildingEquipment;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPPlayerRecordParam;
import NPGameRes.Refs.Mars.*;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_039_MarsBuildingOp;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.PlayerMarsBuildingEquipmentBO;

public class MarsBuildingEquipmentInfo 
{
	//玩家数据对象
	private MarsBuildingInfo _m_biBuildingInfo;
	
	//bo数据
	private PlayerMarsBuildingEquipmentBO _m_bo;
	
	//部件配置
	private RefMarsEquipment _m_ref;
	//等级配置
	private RefMarsEquipmentLevel _m_refLvl;
	
	//额外等级配置列表
	private RefMarsEquipmentEnergyLevel _m_refEnergyLvl;
	private RefMarsEquipmentFoodLevel _m_refFoolLvl;
	private RefMarsEquipmentHospitalLevel _m_refHospitalLvl;
	private RefMarsEquipmentLivingLevel _m_refLivingLvl;
	
	public MarsBuildingEquipmentInfo(MarsBuildingInfo _info, PlayerMarsBuildingEquipmentBO _bo)
	{
		_m_biBuildingInfo = _info;
		
		_m_bo = _bo;
		
		_initRef();
	}
	public MarsBuildingEquipmentInfo(MarsBuildingInfo _info, PlayerMarsBuildingEquipmentBO _bo, RefMarsEquipment _ref, RefMarsEquipmentLevel _lvlRef)
	{
		_m_biBuildingInfo = _info;
		
		_m_bo = _bo;
		
		_m_ref = _ref;
		_m_refLvl = _lvlRef;

    	_setExtraLvlRef();
	}

	public NPUSUserData getUserData() {return _m_biBuildingInfo.getUserData();}
    public NPUserServer getUSServer() {return _m_biBuildingInfo.getUserData().getUSServer();}
    public long getCid() {return _m_biBuildingInfo.getUserData().getCid();}
    
    public PlayerMarsBuildingEquipmentBO getBo() {return _m_bo;}
    public long getEquipmentId() {return _m_bo.getEquipmentId();}
    public int getLvl() {return _m_bo.getLvl();}
    
    public MarsBuildingInfo getBuilding() {return _m_biBuildingInfo;}
    public long getBuildingId() {return _m_biBuildingInfo.getBuildingId();}
    
    public RefMarsEquipment getRef() {return _m_ref;}
    public RefMarsEquipmentLevel getLvlRef() {return _m_refLvl;}

    public RefMarsEquipmentFoodLevel getFoolLvl() {return _m_refFoolLvl;}
    public RefMarsEquipmentEnergyLevel getEnergyLvl() {return _m_refEnergyLvl;}
    public RefMarsEquipmentHospitalLevel getHospitalLvl() {return _m_refHospitalLvl;}
    public RefMarsEquipmentLivingLevel getLivingLvl() {return _m_refLivingLvl;}
    
    private void _initRef()
    {
    	_m_ref = RefMarsEquipment.getMgr().get(getEquipmentId());
    	if(null == _m_ref)
    	{
    		USLog.error(getUSServer(), "player:{} building:{} equip:{} mars equip ref not find.", getCid(), getBuildingId(), getEquipmentId());
    		return;
    	}
    	
    	_m_refLvl = _m_ref.getLevelMapMgr().getLevelData(getLvl());
    	if(null == _m_refLvl)
    	{
    		USLog.error(getUSServer(), "player:{} building:{} equip:{} lvl:{} mars equip lvl ref not find.", getCid(), getBuildingId(), getEquipmentId(), getLvl());
    		return;
    	}
    	
    	//同步部件数据
    	_setExtraLvlRef();
    }
    
    /**
     * 设置额外等级配置数据
     * 注意：额外等级配置不存在是正常的，根据策划配置需求
     */
    private void _setExtraLvlRef()
    {
    	_m_refEnergyLvl = _m_ref.getEnergyLevelMapMgr().getLevelData(getLvl());
    	_m_refFoolLvl = _m_ref.getFoodLevelMapMgr().getLevelData(getLvl());
    	_m_refHospitalLvl = _m_ref.getHospitalLevelMapMgr().getLevelData(getLvl());
    	_m_refLivingLvl = _m_ref.getLivingLevelMapMgr().getLevelData(getLvl());
    }
    
    /**
     * 构造数据
     * @return
     */
	public Mars_BuildingEquipment toProto()
	{
		Mars_BuildingEquipment proto = new Mars_BuildingEquipment();
		proto.setBuildingId(getBuildingId());
		proto.setEquipmentId(getEquipmentId());
		proto.setLvl(getLvl());
		
		return proto;
	}
	
	/**
	 * 设置等级
	 * @param _lvl
	 * @param _context
	 * @return
	 */
	public boolean setLvl(int _lvl, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			if(null == _m_ref)
				return false;
			
			//检查目标配置
			RefMarsEquipmentLevel tarLvlRef = _m_ref.getLevelMapMgr().getLevelData(_lvl);
			if(null == tarLvlRef)
				return false;
			
			//更新数据
			_m_bo.setLvl(getUSServer().getBM(), _lvl);
			_m_bo.saveAll(getUSServer().getBM());
			
			_m_refLvl = tarLvlRef;
			
			//更新附加等级配置
			_setExtraLvlRef();
			
			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_039_MarsBuildingOp.make_053_OnEquipmentChg(this));
			
			//重新计算属性
			_m_biBuildingInfo.doLazyCalBuildingProperty();
			
			return true;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 检查部件是否满级
	 * @return
	 */
	public boolean isLvlFull()
	{
		getUserData().lockUser();
		
		try
		{
			if(null == _m_ref)
				return false;
			
			int lvlLimit = _m_biBuildingInfo.getBuildingLvl() * _m_ref.level_limit_ratio;
			
			return getLvl() >= lvlLimit;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 升级处理
	 * @param _context
	 * @return
	 */
	public Result upgrade(NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			//检查对应配置
			if(null == _m_ref || null == _m_refLvl)
				return CommErr.OBJ_ERR;
			
			int tarLvl = _m_refLvl.level + 1;
			//检查目标等级上限
			int lvlLimit = _m_biBuildingInfo.getBuildingLvl() * _m_ref.level_limit_ratio;
			if(tarLvl > lvlLimit)
				return MarsErr.MARS_BUILDING_EQUIPMENT_LVL_LIMIT;
			
			//检查目标等级配置
			RefMarsEquipmentLevel tarLvlRef = _m_ref.getLevelMapMgr().getLevelData(tarLvl);
			if(null == tarLvlRef)
				return CommErr.REF_NOT_FOUND;
			
			//检查并消耗
			if(!getUserData().hasItem(_m_refLvl.upgrade_cost))
				return CommErr.ITEM_NOT_ENOUGH;
			
			if(!getUserData().spendItem(_m_refLvl.upgrade_cost, _context))
				return CommErr.CONSUME_FAIL;
			
			//更新数据
			setLvl(tarLvl, _context);
				
			//增加建筑部件解锁计数
			getUserData().getRecordComponent().addRecord(ENPPlayerRecordParam.MARS_BUILDING_EQUIP_UP_NUM, 1, _context);
			
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
			
			sb.append("\nid:").append(getEquipmentId());
			sb.append("\nlvl:").append(getLvl());
			
			return sb.toString();
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
}
