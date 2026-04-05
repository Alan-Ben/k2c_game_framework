package NPUSServer.ShieldCidMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.Common_LongList;
import NPCommon.Util.CommonFunc;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;
import NPUSServer.NPUserServer;
import USDB.Bo.UsShieldBO;

import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.HashSet;

public class ShieldCidInfo 
{
	//对应的US服务器实例对象
	private NPUserServer _m_usUserServer;
	
	//数据实例ID
	private long _m_lId;
	//玩家CID
	private long _m_lCid;
	//屏蔽玩家CID列表
	private HashSet<Long> _m_hsShieldCidSet;
	//锁对象
	private MutexAtom _m_mutex;
	
	public ShieldCidInfo(NPUserServer _userServer, long _cid)
	{
		_m_usUserServer = _userServer;
		_m_lCid = _cid;
		_m_hsShieldCidSet = new HashSet<>();
		
		_m_mutex = new MutexAtom();
	}
	
	public ShieldCidInfo(NPUserServer _userServer, UsShieldBO _bo)
	{
		_m_usUserServer = _userServer;
		_m_lId = _bo.getId();
		_m_lCid = _bo.getCid();
		
		_m_hsShieldCidSet = new HashSet<>();
		if(null != _bo.getShieldCidList())
		{
			ByteBuffer buff = ByteBuffer.wrap(_bo.getShieldCidList());
			Common_LongList obj = new Common_LongList();
			obj.readPackage(buff);
			
			_m_hsShieldCidSet.addAll(obj.getValueList());
		}
		
		_m_mutex = new MutexAtom();
	}

	public NPUserServer getUserServer() {return _m_usUserServer;}
	
	public long getId() {return _m_lId;}
	public long getCid() {return _m_lCid;}
	
	private void _lock() {_m_mutex.lock();}
	private void _unlock() {_m_mutex.unlock();}
	
	/**
	 * 构造屏蔽玩家CID数据列表
	 * @param _shieldCidList
	 */
	public void makeProto(ArrayList<Long> _shieldCidList)
	{
		_lock();
		
		try
		{
			_shieldCidList.addAll(_m_hsShieldCidSet);
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 增加屏蔽玩家CID
	 * @param _shieldCid
	 */
	public void addShieldCid(long _shieldCid) 
	{
		_lock();
		
		try
		{
			if(_m_hsShieldCidSet.contains(_shieldCid))
				return;
			
			_m_hsShieldCidSet.add(_shieldCid);
			
			_saveData();
		}
		finally
		{
			_unlock();
		}

		//推送协议
		NPUSUserData belongingUser = getUserServer().getUsUserMgr().lookupCacheUserData(getCid());
		if (belongingUser != null)
		{
			belongingUser.sendMsgToGC(US2GCWriter_004_PlayerOp.make_071_OnShieldCidAdd(_shieldCid));
		}
	}
	
	/**
	 * 移除屏蔽玩家CID
	 * @param _shieldCid
	 */
	public void removeShieldCid(long _shieldCid) 
	{
		_lock();
		
		try
		{
			if(!_m_hsShieldCidSet.contains(_shieldCid))
				return;
			
			_m_hsShieldCidSet.remove(_shieldCid);
			
			_saveData();
		}
		finally
		{
			_unlock();
		}

		//推送协议
		NPUSUserData belongingUser = getUserServer().getUsUserMgr().lookupCacheUserData(getCid());
		if (belongingUser != null)
		{
			belongingUser.sendMsgToGC(US2GCWriter_004_PlayerOp.make_072_OnShieldCidRemove(_shieldCid));
		}
	}
	
	/**
	 * 保存数据
	 */
	private void _saveData()
	{
		Common_LongList obj = new Common_LongList();
		obj.getValueList().addAll(_m_hsShieldCidSet);
		
		if(!_m_hsShieldCidSet.isEmpty()) //有数据的处理
		{
			if(0 == _m_lId)
			{
				UsShieldBO bo = new UsShieldBO();
				bo.setCid(getUserServer().getBM(), _m_lCid);
				bo.setShieldCidList(getUserServer().getBM(), CommonFunc.ByteBfferToBytes(obj.makePackage()));
				bo.insert(getUserServer().getBM());
				
				_m_lId = bo.getId();
			}
			else
			{
				ALMySqlUpdateValue update = new ALMySqlUpdateValue();
				update.addValueObj("shield_cid_list", obj.makePackage());
				
				getUserServer().getBM().getBM(UsShieldBO.class).update("id", _m_lId, update);
			}
		}
		else //无数据，需要移除数据
		{
			if(_m_lId > 0)
			{
				getUserServer().getBM().getBM(UsShieldBO.class).delAll("id", _m_lId);
			}
		}
	}
	
	/**
	 * 获取屏蔽玩家数量
	 * @return
	 */
	public int getShieldCount()
	{
		_lock();
		
		try
		{
			return _m_hsShieldCidSet.size();
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 检查是否屏蔽对应玩家
	 * @param _cid
	 * @return
	 */
	public boolean hasShieldCid(long _cid)
	{
		_lock();
		
		try
		{
			return _m_hsShieldCidSet.contains(_cid);
		}
		finally
		{
			_unlock();
		}
	}
}
