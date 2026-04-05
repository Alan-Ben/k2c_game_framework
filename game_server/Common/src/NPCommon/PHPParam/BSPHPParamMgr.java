package NPCommon.PHPParam;

import ALBasicCommon.ALSerializeMaker;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import NPCommon.Util._ABasicServerObj;

/**
 * 接收方服务器的平台参数管理对象，有需要同步平台参数的服务器需要初始化该管理对象
 * 平台参数数据源服务器是CommonServer
 * @author mj
 *
 */
public class BSPHPParamMgr extends PHPParamMgr
{
	//////单例的//////
	private static final BSPHPParamMgr _s_instance = new BSPHPParamMgr();
	public static BSPHPParamMgr getInstance()
	{
		return _s_instance;
	}

    //请求序列号，用于避免同时存在多个请求任务
    private static long _m_lSendSerial;
    private synchronized static long _buildSendSerial()
    {
    	_m_lSendSerial = ALSerializeMaker.makeNewSerialize();
    	return _m_lSendSerial;
    }
    public static long getSendSerial()
    {
    	return _m_lSendSerial;
    }
	
	public BSPHPParamMgr()
	{
		
	}
	
	/**
	 * 开启加载
	 * @param _server
	 */
	public void startLoad(_ABasicServerObj _server)
	{
		sendToGetPHPParamList(_server, true);
	}
	public void startLoad(_ABasicServerObj _server, boolean _needRetry)
	{
		sendToGetPHPParamList(_server, _needRetry);
	}
	
	/**
	 * 向CS发起请求平台参数
	 * @param _server
	 */
	public void sendToGetPHPParamList(_ABasicServerObj _server, boolean _needRetry)
	{
		ALSynTaskManager.getInstance().regTask(new BSSendToGetPHPParamListTask(_buildSendSerial(), _server, _needRetry));
	}
}
