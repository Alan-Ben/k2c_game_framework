package NPUSServer.NPUSUserMgr.UserComp.MarsStep2_BuildingComp;

import Common.MarsObj.Mars_BuildingEquipment;
import NPGameRes.Refs.Mars.RefMarsEquipment;
import NPGameRes.Refs.Mars.RefMarsEquipmentLevel;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_039_MarsBuildingOp;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.PlayerMarsBuildingEquipmentBO;

import java.util.ArrayList;

/**
 * 建筑部件数据管理
 * @author mj
 *
 */
public class MarsBuildingEquipmentMgr 
{
	//玩家数据对象
	private MarsBuildingInfo _m_biBuildingInfo;
	//部件数据列表
	private ArrayList<MarsBuildingEquipmentInfo> _m_alBuildingEquipmentList;
	
	public MarsBuildingEquipmentMgr(MarsBuildingInfo _info)
	{
		_m_biBuildingInfo = _info;
		
		_m_alBuildingEquipmentList = new ArrayList<>();
	}
	
	protected void _lock() {_m_biBuildingInfo.getUserData().lockUser();}
	protected void _unlock() {_m_biBuildingInfo.getUserData().unlockUser();}
	
	public NPUSUserData getUserData() {return _m_biBuildingInfo.getUserData();}
    public NPUserServer getUSServer() {return _m_biBuildingInfo.getUserData().getUSServer();}
    public long getCid() {return _m_biBuildingInfo.getUserData().getCid();}
    
    public MarsBuildingInfo getBuilding() {return _m_biBuildingInfo;}
    public long getBuildingId() {return _m_biBuildingInfo.getBuildingId();}
    
	protected void _initBo(PlayerMarsBuildingEquipmentBO _bo)
	{
		MarsBuildingEquipmentInfo info = new MarsBuildingEquipmentInfo(_m_biBuildingInfo, _bo);
		_m_alBuildingEquipmentList.add(info);
	}
	
	protected void _calProperty() 
	{
		//计算基础属性
		for(int i = 0; i < _m_alBuildingEquipmentList.size(); i++)
		{
			MarsBuildingEquipmentInfo info = _m_alBuildingEquipmentList.get(i);
			if(null == info)
				continue;
			
			_m_biBuildingInfo.getBuildingValue()._calAddEquipment(info);
		}
	}
	
	/**
	 * 获取部件等级总和
	 * @return
	 */
	public int getLvlSum()
	{
		_lock();
		
		try
		{
			int sum = 0;
			for(int i = 0; i < _m_alBuildingEquipmentList.size(); i++)
			{
				MarsBuildingEquipmentInfo info = _m_alBuildingEquipmentList.get(i);
				if(null == info)
					continue;
				
				sum += info.getLvl();
			}
			
			return sum;
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 构造数据
	 * @param _list
	 */
	public void makeProto(ArrayList<Mars_BuildingEquipment> _list)
	{
		_lock();
		
		try
		{
			for(int i = 0; i < _m_alBuildingEquipmentList.size(); i++)
			{
				MarsBuildingEquipmentInfo info = _m_alBuildingEquipmentList.get(i);
				if(null == info)
					continue;
				
				_list.add(info.toProto());
			}
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 查找建筑部件数据
	 * @param _equipmentId
	 * @return
	 */
	public MarsBuildingEquipmentInfo lookup(long _equipmentId)
	{
		_lock();
		
		try
		{
			for(int i = 0; i < _m_alBuildingEquipmentList.size(); i++)
			{
				MarsBuildingEquipmentInfo info = _m_alBuildingEquipmentList.get(i);
				if(null == info)
					continue;
				
				if(info.getEquipmentId() == _equipmentId)
					return info;
			}
			
			return null;
		}
		finally
		{
			_unlock();
		}
	}

	/**
	 * 解锁部件
	 * @param _bInit
	 * @param _equipmentId
	 * @param _context
	 */
	public void unlock(boolean _bInit, long _equipmentId, NPPlayerContext _context)
	{
		RefMarsEquipment ref = RefMarsEquipment.getMgr().get(_equipmentId);
		if(null == ref)
		{
			return;
		}
		
		unlock(_bInit, ref, _context);
	}
	public void unlock(boolean _bInit, RefMarsEquipment _equipmentRef, NPPlayerContext _context)
	{
		_lock();
		
		try
		{
			//已经解锁
			if(null != lookup(_equipmentRef.id))
				return;
			
			//从1级解锁
			RefMarsEquipmentLevel lvlRef = _equipmentRef.getLevelMapMgr().getLevelData(1);
			if(null == lvlRef)
			{
				USLog.error(getUSServer(), "player:{} building:{} equipment:{} lvl:{} mars building unlock equipment fail, not find ref.", 
						getCid(), getBuildingId(), _equipmentRef.id, 1);
				return;
			}
			
			//构造数据
			PlayerMarsBuildingEquipmentBO bo = new PlayerMarsBuildingEquipmentBO();
			bo.setCid(getUSServer().getBM(), getCid());
			bo.setBuildingId(getUSServer().getBM(), getBuildingId());
			bo.setEquipmentId(getUSServer().getBM(), _equipmentRef.id);
			bo.setLvl(getUSServer().getBM(), lvlRef.level);
			bo.insert(getUSServer().getBM());
			
			MarsBuildingEquipmentInfo info = new MarsBuildingEquipmentInfo(_m_biBuildingInfo, bo, _equipmentRef, lvlRef);
			_m_alBuildingEquipmentList.add(info);

			//重新计算属性
			if(!_bInit)
			{
				//推送数据
				getUserData().sendMsgToGC(US2GCWriter_039_MarsBuildingOp.make_053_OnEquipmentChg(info));
				//计算所有建筑属性
				_m_biBuildingInfo.doLazyCalBuildingProperty();
			}
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 检查主要部件列表（升级需要先把这些部件升满）
	 * 注：只对已解锁的部件进行检查
	 * @return
	 */
	public boolean checkMainEquipLvlFull()
	{
		_lock();
		
		try
		{
			if(null == _m_biBuildingInfo.getRef().equipmentBelongRef)
				return true;
			
			ArrayList<Long> equipmentIdList = _m_biBuildingInfo.getRef().equipmentBelongRef.main_equipment_id_list;
			for(int i = 0; i < equipmentIdList.size(); i++)
			{
				//检查该部件是否已解锁
				RefMarsEquipment equipmentRef = RefMarsEquipment.getMgr().get(equipmentIdList.get(i));
				if(null == equipmentRef)
				{
					USLog.error(getUSServer(), "player:{} building:{} equip:{} mars checkMainEquipLvlFull fail, not find ref.", 
							getCid(), getBuildingId(), equipmentIdList.get(i));
					return false;
				}
				
				if(equipmentRef.unlock_level > getBuilding().getBuildingLvl())
					continue;
				
				//已解锁的部件进行检查
				MarsBuildingEquipmentInfo info = lookup(equipmentRef.id);
				if(null == info)
					return false;
				
				if(!info.isLvlFull())
					return false;
			}
			
			return true;
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
			sb.append("\nequipment size:").append(_m_alBuildingEquipmentList.size());
			
			for(int i = 0; i < _m_alBuildingEquipmentList.size(); i++)
			{
				MarsBuildingEquipmentInfo info = _m_alBuildingEquipmentList.get(i);
				if(null == info)
					continue;
				
				sb.append("\n--- equipment ").append(info.getEquipmentId()).append(" ---");
				sb.append(info.toString());
			}
			
			return sb.toString();
		}
		finally
		{
			_unlock();
		}
	}
}
