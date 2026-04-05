package NPCommonServer.PHPParmMgr;

import ALBasicCommon.ALSerializeMaker;
import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import AllRpcData.All_Service.PushPHPParamList;
import Common.ServerObj.ServerObj_PHPParamList;
import NPCommon.Log.CommLog;
import NPCommon.PHPParam.PHPParamMgr;
import NPCommonServer.NPCommonServer;
import RPC.RpcSender;
import RPC._ARpcCallBack;
import WCGCommon.Enum.NPEnum.EServerType;

import java.util.ArrayList;
import java.util.HashSet;

public class CSPHPParamMgr extends PHPParamMgr
{
    //////单例的//////
    private static final CSPHPParamMgr _s_instance = new CSPHPParamMgr();
    public static CSPHPParamMgr getInstance()
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
    
    //注册的需要监听的所有服务器连接数据 type * 100000 + typeId
    private HashSet<Long> _m_hsServerKeySet;
    //对连接信息的锁保护对象
    private MutexAtom _m_listenerMutex;
    
    public CSPHPParamMgr()
    {
    	_m_hsServerKeySet = new HashSet<>();
    	_m_listenerMutex = new MutexAtom();
    }
    
    private void _listenerLock() {_m_listenerMutex.lock();}
    private void _listenerUnlock() {_m_listenerMutex.unlock();}
    
    /**
     * 构造服务器key
     * @param _type
     * @param _typeId
     * @return
     */
    private long _buildServerKey(int _type, int _typeId)
    {
    	return _type * 100000 + _typeId;
    }
    
    /**
     * 开启加载
     */
    public void startLoad() 
    {
        //请求平台参数
    	sendToGetPHPParamList();
    }
	
    /**
     * 请求平台参数
     */
    public void sendToGetPHPParamList()
    {
    	ALSynTaskManager.getInstance().regTask(new CSSendToGetPHPParamListTask(_buildSendSerial()));
    }
    /**
     * 接收平台参数数据
     * @param _phpSerial
     * @param _dataSerial
     * @param _phpParamList
     */
    public void acceptPHPParamList(long _dataSerial, String _phpSerial, ServerObj_PHPParamList _phpParamList)
    {
    	CommLog.info("Send Get PHP-ParamList Suc, DataSerial:{}", _dataSerial);
    	
    	boolean res = CSPHPParamMgr.getInstance().loadAllData(_dataSerial, _phpSerial, _phpParamList);
    	//更新成功，则需要下发同步到已注册监听的服务器数据
    	if(res)
    	{
			ServerObj_PHPParamList phpParamList = new ServerObj_PHPParamList();
			CSPHPParamMgr.getInstance().makeData(phpParamList.getPList());

    		PushPHPParamList rpc = new PushPHPParamList();
    		rpc.req().setSerial(_dataSerial);
    		rpc.req().setPListObj(phpParamList);
    		
    		pushPHPParamListToAllServer(rpc);
    	}
    }

	/**
	 * 更新平台参数数据
	 * @param _phpSerial
	 * @param _dataSerial
	 * @param _phpParamList
	 */
	public void updatePHPParamList(long _dataSerial, String _phpSerial, ServerObj_PHPParamList _phpParamList)
	{
		CommLog.info("update PHP-ParamList, DataSerial:{}", _dataSerial);

		boolean res = CSPHPParamMgr.getInstance().updatePartialData(_dataSerial, _phpSerial, _phpParamList);
		//更新成功，则需要下发同步到已注册监听的服务器数据
		if(res)
		{
			ServerObj_PHPParamList phpParamList = new ServerObj_PHPParamList();
			CSPHPParamMgr.getInstance().makeData(phpParamList.getPList());

			PushPHPParamList rpc = new PushPHPParamList();
			rpc.req().setSerial(_dataSerial);
			rpc.req().setPListObj(phpParamList);

			pushPHPParamListToAllServer(rpc);
		}
	}

    /**
     * 注册监听平台数据变化的服务器key
     * @param _type
     * @param _typeId
     */
    public void regListenerServer(int _type, int _typeId)
    {
    	_listenerLock();
    	
    	try
    	{
    		long serverKey = _buildServerKey(_type, _typeId);
    		
    		//已经存在的连接，不再重复添加
    		if(_m_hsServerKeySet.contains(serverKey))
    			return;

    		_m_hsServerKeySet.add(serverKey);
    		
    		CommLog.info("CS PHP-ParamList Reg Server:{}-{}", _type, _typeId);
    	}
    	finally 
    	{
    		_listenerUnlock();
		}
    }
    
    /**
     * 推送平台参数给已注册监听的服务器
     * @param _rpc
     */
    public void pushPHPParamListToAllServer(PushPHPParamList _rpc)
    {
    	ArrayList<Long> keyList = new ArrayList<>();
    	
    	_listenerLock();
    	try
    	{
    		keyList.addAll(_m_hsServerKeySet);
    	}
    	finally 
    	{
    		_listenerUnlock();
		}
    
    	for(int i = 0; i < keyList.size(); i++)
    	{
    		long serverKey = keyList.get(i);
    		
    		int type = (int) (serverKey / 100000);
    		int typeId = (int) (serverKey % 100000);
    		
    		pushPHPParamListToServer(type, typeId, _rpc);
    	}
    }
    /**
     * 对单台服务器进行推送
     * @param _type
     * @param _typeId
     * @param _rpc
     */
    public void pushPHPParamListToServer(int _type, int _typeId, PushPHPParamList _rpc)
    {
    	//发起请求
    	RpcSender sender = new RpcSender(NPCommonServer.getInstance(), EServerType.EServerType_FromInt(_type), _typeId);
    	sender.requestToRepeat(_typeId, _rpc, new _ARpcCallBack<PushPHPParamList>() {

			@Override
			public void call_back(int _errCode, PushPHPParamList _rpc) 
			{
				if(!_rpc.retObj().getIsSucc())
				{
				}
			}
		}, 
    	10, //失败重复执行10次
    	()-> { //全部重试都失败后，输出日志并发送DD消息
    		
			CommLog.error("Push PHP-Param:{} To Server:{}-{} Fail.", _rpc.req().getSerial(), _type, _typeId);
			
			NPCommonServer.getInstance().getDDAlert().err("PushPHPParamFail-"+_type+"-"+_typeId, "推送平台参数到服务器"+_type+"-"+_typeId+"失败，请手动确认");
		});
    }
}
