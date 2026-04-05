package NPInterfaceServer.ChatMgr.ChatRoomMgr;

import ALBasicServer.ALBasicMutex.MutexObject;
import ALServerLog.ALServerLog;
import AllRpcData.All_Service.Chat.ChatRoomErr;
import ISDB.Bo.ChatRoomBO;
import NPCommon.DB.BM.BM;
import NPInterfaceServer.ChatMgr.ChatMgr;
import NPInterfaceServer.NPInterfaceServer;
import RPC._ARpcCallBack;
import WCGCommon.Enum.NPEnum;

import java.util.ArrayList;
import java.util.List;


public class ChatRoomMgr
{
    private static final ChatRoomMgr _g_instance = new ChatRoomMgr();
    public static ChatRoomMgr getInstance()
    {
        return _g_instance;
    }
    
    //所有房间数据列表
    private final ArrayList<ChatRoomInfo> _m_alRoomList;
    //锁对象
    private final MutexObject _m_mutex;
    
    private ChatRoomMgr()
    {
    	_m_alRoomList = new ArrayList<>();
    	
    	_m_mutex = new MutexObject();
    }
    
    private void _lock() {_m_mutex.lock();}
    private void _unlock() {_m_mutex.unlock();}
    
    public boolean initFromDB()
    {
    	List<ChatRoomBO> boList = NPInterfaceServer.getInstance().getBM().getBM(ChatRoomBO.class).s_findAll();
    	if(null == boList)
    	{
    		return false;
    	}
    	
    	for(int i = 0; i < boList.size(); i++)
    	{
    		ChatRoomBO bo = boList.get(i);
    		if(null == bo)
    			continue;
    		
    		ChatRoomInfo info = new ChatRoomInfo(bo);
    		_m_alRoomList.add(info);
    	}
    	
    	return true;
    }
    
    /***********************
     * 注册当前所有房间到聊天服务器
     */
    public void regAllRoomToSDK()
    {
    	_lock();
    	
    	try
    	{
    		for(int i = 0; i < _m_alRoomList.size(); i++)
    		{
    			ChatRoomInfo info = _m_alRoomList.get(i);
    			if(null == info)
    				continue;
    			
    			info.regToSDK();
    		}
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /******************
     * 查找相关索引的
     * 
     * @param _serverType
     * @param _serverTypeId
     * @param _roomType
     * @param _roomTypeId
     * @return
     */
    public ChatRoomInfo lookup(int _serverType, int _serverTypeId, int _roomType, long _roomTypeId)
    {
    	_lock();
    	
    	try
    	{
    		for(int i = 0; i < _m_alRoomList.size(); i++)
    		{
    			ChatRoomInfo info = _m_alRoomList.get(i);
    			if(null == info)
    				continue;
    			
    			if(info.getServerType() == _serverType && info.getServerTypeId() == _serverTypeId
    					&& info.getRoomType() == _roomType && info.getRoomTypeId() == _roomTypeId)
    				return info;
    		}
    		
    		return null;
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /******************
     * 根据 sdkRoomId 查找对应房间对象
     * 
     * @param _sdkRoomId
     * @return
     */
    public ChatRoomInfo lookupBySdkRoomId(long _sdkRoomId)
    {
    	if(_sdkRoomId <= 0)
    		return null;
    	
    	_lock();
    	
    	try
    	{
    		for(int i = 0; i < _m_alRoomList.size(); i++)
    		{
    			ChatRoomInfo info = _m_alRoomList.get(i);
    			if(null == info)
    				continue;
    			
    			if(info.getSdkRoomId() == _sdkRoomId)
    				return info;
    		}
    		
    		return null;
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /*********
     * 注册房间
     * 
     * @param _serverType
     * @param _serverTypeId
     * @param _roomType
     * @param _roomTypeId
     * @return
     */
    public ChatRoomInfo regRoom(int _serverType, int _serverTypeId, int _roomType, long _roomTypeId)
    {
    	_lock();
    	
    	try
    	{
    		ChatRoomInfo info = lookup(_serverType, _serverTypeId, _roomType, _roomTypeId);
    		if(null == info)
    		{
    			BM bmObj = NPInterfaceServer.getInstance().getBM();
    			
    			ChatRoomBO bo = new ChatRoomBO();
    			bo.setServerType(bmObj, _serverType);
    			bo.setServerTypeId(bmObj, _serverTypeId);
    			bo.setRoomType(bmObj, _roomType);
    			bo.setRoomTypeId(bmObj, _roomTypeId);
    			bo.insert(bmObj);
    			
    			info = new ChatRoomInfo(bo);
    			_m_alRoomList.add(info);
    			
    			//开启注册聊天服务器
    			info.regToSDK();
    		}
    		
    		return info;
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /******************
     * 注销房间
     * 
     * @param _serverType
     * @param _serverTypeId
     * @param _roomType
     * @param _roomTypeId
     */
    public void unRegRoom(int _serverType, int _serverTypeId, int _roomType, long _roomTypeId)
    {
    	_lock();
    	
    	try
    	{
    		ChatRoomInfo info = lookup(_serverType, _serverTypeId, _roomType, _roomTypeId);
    		if(null == info)
    			return;
    		
    		//移除房间列表数据
    		_m_alRoomList.remove(info);
    		
    		//销毁房间数据
    		info.discard();
    	}
    	finally
    	{
    		_unlock();
    	}
    }

    /*******************
     * 全部数据重置
     * 
     * @param _sdkSerial
     */
    public void resetAll(long _sdkSerial)
    {
    	_lock();
    	
    	try
    	{
    		//需要检查
    		if(_sdkSerial != ChatMgr.getInstance().getChatSdkSerial())
    			return;
    		
    		for(int i = 0; i < _m_alRoomList.size(); i++)
    		{
    			ChatRoomInfo room = _m_alRoomList.get(i);
    			if(null == room)
    				continue;
    			
    			room.resetAndRegToSDK();
    		}
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /*******************
     * 聊天服务器通知房间错误
     * 
     * @param _sdkRoomId
     * @param _roomSerial
     */
    public void onRoomErr(long _sdkRoomId, long _roomSerial)
    {
    	_lock();
    	
    	try
    	{
    		ChatRoomInfo room = lookupBySdkRoomId(_sdkRoomId);
    		if(null == room || room.getRoomSerial() != _roomSerial)
    			return;

            //通报聊天房间所在服务器，后续的通知由聊天房间下发
            ChatRoomErr rpc = new ChatRoomErr();
            rpc.req().setRoomId(_sdkRoomId);

            if(NPEnum.EServerType.USER.ordinal() == room.getServerType())
            {
                NPInterfaceServer.getInstance().rpc2us().requestTo(room.getServerTypeId(), rpc,
                        new _ARpcCallBack<ChatRoomErr>()
                {
                    @Override
                    public void call_back(int _errCode, ChatRoomErr _rpc)
                    {
                        if(_errCode > 0)
                        {
                            ALServerLog.Error("ChatRoomErr Rpc CallBack Err, errCode: " + _errCode);
                        }
                    }
                });
            }
            else if(NPEnum.EServerType.SINGLE.ordinal() == room.getServerType())
            {

            }
    	}
    	finally
    	{
    		_unlock();
    	}
    }
}
