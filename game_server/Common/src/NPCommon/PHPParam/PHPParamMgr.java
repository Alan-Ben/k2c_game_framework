package NPCommon.PHPParam;

import ALBasicServer.ALBasicMutex.MutexAtom;
import Common.ServerObj.ServerObj_PHPParam;
import Common.ServerObj.ServerObj_PHPParamList;
import CommonEnum.EPlatParamType;
import NPCommon.Log.CommLog;
import NPCommon.Util.Delegate.GlobalADelegateTwo;
import com.google.gson.JsonObject;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;

/**
 * 平台参数数据管理
 * 
 * @author mj
 *
 */
abstract public class PHPParamMgr 
{
	//数据序列号，用于检查是否是最新数据
	private long _m_lDataSerial;
	//平台序列号，用于与平台进行校对
	private String _m_sPHPSerial;
	//平台参数
	private HashMap<EPlatParamType, String> _m_hmParamMap;
	//锁对象，保护平台参数数据
	final private MutexAtom _m_mutex;
	//参数变更触发
	private GlobalADelegateTwo<EPlatParamType, String> _m_paramChgDelegate;
	
	public PHPParamMgr()
	{
		_m_hmParamMap = new HashMap<>();
		_m_mutex = new MutexAtom();
		_m_paramChgDelegate = new GlobalADelegateTwo<>(this);
	}
	
	final private void _dataLock() {_m_mutex.lock();}
	final private void _dataUnlock() {_m_mutex.unlock();}
	
	final public long getDataSerial() {return _m_lDataSerial;}
	final public String getPHPSerial() {return _m_sPHPSerial;}

	public GlobalADelegateTwo<EPlatParamType, String> getParamChgDelegate()
	{
		return _m_paramChgDelegate;
	}

	/**
	 * 是否加载完成
	 * @return
	 */
	final public boolean isLoaded() {return _m_lDataSerial > 0;}

	/**
	 * 重新载入平台参数，需要记录序列号
	 * @param _dataSerial
	 * @param _phpSerial
	 * @param _paramList
	 * @return
	 */
	final public boolean loadAllData(long _dataSerial, String _phpSerial, ServerObj_PHPParamList _paramList)
	{
		_dataLock();
		
		try
		{
			//检查当前数据是否最新数据，当前已是最新数据则不再处理
			if(_dataSerial < _m_lDataSerial)
				return false;
			
			_m_hmParamMap.clear();
			
			_m_lDataSerial = _dataSerial;
			
			if(null == _phpSerial)
			{
				_m_sPHPSerial = "no php-serial";
			}
			else
			{
				_m_sPHPSerial = _phpSerial;
			}
			
			for(int i = 0; i < _paramList.getPList().size(); i++)
			{
				ServerObj_PHPParam param = _paramList.getPList().get(i);
				if(null == param)
					continue;
				
				_m_hmParamMap.put(param.getPKey(), param.getPValue());
				_m_paramChgDelegate.onAsyncEvent(param.getPKey(), param.getPValue());
			}
			
			//日志输出
			CommLog.info("{}", toString());
			
			return true;
		}
		finally
		{
			_dataUnlock();
		}
	}
	/**
	 * 无需平台序列号的加载数据
	 * @param _dataSerial
	 * @param _paramList
	 * @return
	 */
	final public boolean loadAllData(long _dataSerial, ServerObj_PHPParamList _paramList)
	{
		return loadAllData(_dataSerial, null, _paramList);
	}

	/**
	 * 更新部分平台参数
	 *
	 * 执行流程：
	 * 1. 检查数据序列号是否为最新
	 * 2. 只更新提供的参数，不清空现有参数
	 * 3. 触发参数变更事件
	 * 4. 输出日志
	 *
	 * @param _dataSerial 数据序列号
	 * @param _phpSerial 平台序列号
	 * @param _paramList 需要更新的参数列表
	 * @return true-更新成功，false-序列号过期
	 *
	 * 线程安全：通过_dataLock()/_dataUnlock()保护数据一致性
	 */
	final public boolean updatePartialData(long _dataSerial, String _phpSerial, ServerObj_PHPParamList _paramList)
	{
		_dataLock();

		try
		{
			// 检查当前数据是否最新数据，当前已是最新数据则不再处理
			if(_dataSerial < _m_lDataSerial)
				return false;

			// 不清空现有参数，只更新提供的参数
			_m_lDataSerial = _dataSerial;

			if(null == _phpSerial)
			{
				_m_sPHPSerial = "no php-serial";
			}
			else
			{
				_m_sPHPSerial = _phpSerial;
			}

			// 遍历更新参数列表
			for(int i = 0; i < _paramList.getPList().size(); i++)
			{
				ServerObj_PHPParam param = _paramList.getPList().get(i);
				if(null == param)
					continue;

				// 更新或添加参数
				_m_hmParamMap.put(param.getPKey(), param.getPValue());
				_m_paramChgDelegate.onAsyncEvent(param.getPKey(), param.getPValue());
			}

			// 日志输出
			CommLog.info("{}", toString());

			return true;
		}
		finally
		{
			_dataUnlock();
		}
	}
	
	/**
	 * 构造平台参数数据
	 * @param _list
	 */
	final public void makeData(ArrayList<ServerObj_PHPParam> _list)
	{
		_dataLock();
		
		try
		{
			for (Map.Entry<EPlatParamType, String> entry : _m_hmParamMap.entrySet())
			{
				_list.add(new ServerObj_PHPParam(entry.getKey(), entry.getValue()));
			}
		}
		finally
		{
			_dataUnlock();
		}
	}

	/**
	 * 获取参数
	 * @param _type
	 * @return
	 */
	public String getParam(EPlatParamType _type)
	{
		_dataLock();

		try
		{
			return _m_hmParamMap.get(_type);
		}
		finally
		{
			_dataUnlock();
		}
	}
	
	/**
	 * 构造json对象
	 * @return
	 */
	public JsonObject toJsonObj()
	{
		_dataLock();
		
		try
		{
			JsonObject obj = new JsonObject();
			
			for (Map.Entry<EPlatParamType, String> entry : _m_hmParamMap.entrySet())
			{
				obj.addProperty(entry.getKey().toString(), entry.getValue());
			}
			
			return obj;
		}
		finally
		{
			_dataUnlock();
		}
	}
	
	/**
	 * 输出所有平台列表
	 */
	public String toString()
	{
		_dataLock();
		
		try
		{
			StringBuilder sb = new StringBuilder();
			sb.append("\n====================== 平台参数列表 ======================");
			sb.append("\n数据序列号：").append(_m_lDataSerial).append("\n").append("平台序列号：").append(_m_sPHPSerial);
			sb.append("\n------------------------------------------------------------------------------------------------------------------------------");
			for (Map.Entry<EPlatParamType, String> entry : _m_hmParamMap.entrySet())
			{
			    sb.append("\n").append(entry.getKey()).append("：").append(entry.getValue());
			}
			sb.append("\n=====================================================");
			
			return sb.toString();
		}
		finally
		{
			_dataUnlock();
		}
	}
}
