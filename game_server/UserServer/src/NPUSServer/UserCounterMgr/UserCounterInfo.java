package NPUSServer.UserCounterMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import NPEnum.EPlayerCounterEnum;
import NPUSServer.NPUserServer;
import USDB.Bo.PlayerCounterBO;

public class UserCounterInfo 
{
	private NPUserServer _m_server;

	//玩家CID
	private long _m_lCid;
	//玩家计数数组
	private long[] _m_arrCounterArr;
	//锁对象
	private MutexAtom _m_mutex;
	
	public UserCounterInfo(NPUserServer _server, PlayerCounterBO _bo)
	{
		_m_server = _server;

		_m_lCid = _bo.getCid();
		
		_m_arrCounterArr = new long[EPlayerCounterEnum.EPlayerCounterEnum_Length];
		
		_m_mutex = new MutexAtom();
		
		for(int i = 0; i < _m_arrCounterArr.length; i++)
		{
			_m_arrCounterArr[i] = _bo.getCounter(i);
		}
	}
	
	private void _lock() {_m_mutex.lock();}
	private void _unlock() {_m_mutex.unlock();}

	public NPUserServer getUSServer() {return _m_server;}
	public long getCid() {return _m_lCid;}
	/**
	 * 获取玩家对应类型计数
	 * @param _type
	 * @return
	 */
	public long getCounter(EPlayerCounterEnum _type)
	{
		return _m_arrCounterArr[_type.ordinal()];
	}
	
	/**
	 * 更新玩家数
	 * @param _type
	 * @param _counter
	 */
	public void setCounter(EPlayerCounterEnum _type, long _counter)
	{
		_lock();
		
		try
		{
			//数量一致，不做处理
			if(getCounter(_type) == _counter)
				return;
			
			_m_arrCounterArr[_type.ordinal()] = _counter;
		
			ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
			updateValue.addValueObj(__counterStr(_type), _counter);
			getUSServer().getBM().getBM(PlayerCounterBO.class).update("cid", _m_lCid, updateValue);
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 玩家计数+1
	 * @param _type
	 */
	public void incrtCounter(EPlayerCounterEnum _type)
	{
		_lock();
		
		try
		{
			long newConter = _m_arrCounterArr[_type.ordinal()] + 1;
			
			setCounter(_type, newConter);
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 构造counter字段名称
	 * @param _type
	 * @return
	 */
	private String __counterStr(EPlayerCounterEnum _type)
	{
		StringBuilder sb = new StringBuilder();
		sb.append("counter_").append(_type.ordinal());
		
		return sb.toString();
	}
}
