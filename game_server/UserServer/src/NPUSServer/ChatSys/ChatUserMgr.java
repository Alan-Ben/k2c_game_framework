package NPUSServer.ChatSys;

import ALBasicServer.ALBasicMutex.MutexObject;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;

import java.util.HashMap;

public class ChatUserMgr 
{
	private final NPUserServer _m_soServerObj;
	
	private final HashMap<Long, ChatUserInfo> _m_hmChatUserMap;
	private final MutexObject _m_mutex;
	
	public ChatUserMgr(NPUserServer _serverObj)
	{
		_m_soServerObj = _serverObj;
		
		_m_hmChatUserMap = new HashMap<>();
		
		_m_mutex = new MutexObject();
	}
	
	private void _lock() {_m_mutex.lock();}
	private void _unlock() {_m_mutex.unlock();}
	
	public NPUserServer getServerObj() {return _m_soServerObj;}
	
	/***********
	 * 注册聊天用户
	 * 
	 * @param _userData
	 * @return
	 */
	public ChatUserInfo regChatUser(NPUSUserData _userData, String _chatUid)
	{
		_lock();
		
		try
		{
			ChatUserInfo user = new ChatUserInfo(_userData.getCid(), _userData.getSerialize(), _chatUid);
			_m_hmChatUserMap.put(_userData.getCid(), user);
			
			return user;
		}
		finally
		{
			_unlock();
		}
	}
	
	/******
	 * 注销用户
	 * 
	 * @param _cid
	 * @param _chatUserSerial
	 * @return
	 */
	public boolean unRegChatUser(NPUSUserData _userData)
	{
		_lock();
		
		try
		{
			ChatUserInfo user = _m_hmChatUserMap.remove(_userData.getCid());
			if(null != user && user.getSerial() != _userData.getSerialize())
			{
				_m_hmChatUserMap.put(user.getCid(), user);
				return false;
			}
			
			return true;
		}
		finally
		{
			_unlock();
		}
	}

    /**
     * 查找聊天用户
     * @param _cid
     * @return
     */
	public ChatUserInfo lookupChatUser(long _cid)
	{
		_lock();
		
		try
		{
			return _m_hmChatUserMap.get(_cid);
		}
		finally
		{
			_unlock();
		}
	}
}
