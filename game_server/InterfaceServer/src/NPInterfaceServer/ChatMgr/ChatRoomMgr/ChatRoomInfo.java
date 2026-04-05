package NPInterfaceServer.ChatMgr.ChatRoomMgr;

import ALBasicCommon.ALSerializeMaker;
import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import ChatSDK.CallbackInterface._ICommonDealFunc;
import ChatSDK.CallbackInterface._ICreateRoomDealFunc;
import ChatSDK.CallbackInterface._IInitRoomDealFunc;
import ChatSDK.Common.Log.CommLog;
import ISDB.Bo.ChatRoomBO;
import NPCommon.ErrMain.CommErr;
import NPInterfaceServer.ChatMgr.ChatMgr;
import NPInterfaceServer.NPGeneralListener.Writer.NP2IS_RB_Writer_001_ISOp;
import NPInterfaceServer.NPInterfaceServer;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

public class ChatRoomInfo 
{
	//固定数据
	private final long _m_lId;
	private final int _m_iServerType;
	private final int _m_iServerTypeId;
	private final int _m_iRoomType;
	private final long _m_lRoomTypeId;
	
	//动态数据-聊天服务器房间序列号
	private long _m_lRoomSerial;
	
	//通过SDK获取的聊天服务器房间实例ID
	private long _m_lSdkRoomId;
	//初始化过程标志位
	private boolean _m_bIniting;
	//初始化完成标志位
	private boolean _m_bInited;
	//已经移除标志位
	private boolean _m_bDeled;
	
	//数据序列号，当SDK状态发生改变时再进行变更，用于区分SDK的状态
	private long _m_lSerial;
	
	//上次发起的提交对象
	private _IWCGBasicRequestCommiter _m_cLastRegCommiter;
	
	//锁对象
	private MutexAtom _m_mutex;
	
	public ChatRoomInfo(ChatRoomBO _bo)
	{
		_m_lId = _bo.getId();
		_m_iServerType = _bo.getServerType();
		_m_iServerTypeId = _bo.getServerTypeId();
		_m_iRoomType = _bo.getRoomType();
		_m_lRoomTypeId = _bo.getRoomTypeId();
		
		_m_lSdkRoomId = _bo.getSdkRoomId();
		
		_m_lSerial = ALSerializeMaker.makeNewSerialize();
		
		_m_mutex = new MutexAtom();
	}
	
	private void _lock() {_m_mutex.lock();}
	private void _unlock() {_m_mutex.unlock();}
	
	public long getId() {return _m_lId;}
	public int getServerType() {return _m_iServerType;}
	public int getServerTypeId() {return _m_iServerTypeId;}
	public int getRoomType() {return _m_iRoomType;}
	public long getRoomTypeId() {return _m_lRoomTypeId;}
	public long getSdkRoomId() {return _m_lSdkRoomId;}
	
	public boolean isIniting() {return _m_bIniting;}
	public boolean isInited() {return _m_bInited;}
	public boolean isDeled() {return _m_bDeled;}
	
	public long getRoomSerial() {return _m_lRoomSerial;}

	/************
	 * 重置房间初始化完成标志并重新发起注册
	 */
	protected void unsetAndRegToSDK() 
	{
		_lock();
		
		try
		{
			_m_lSerial = ALSerializeMaker.makeNewSerialize();
			
			_m_bInited = false;
		}
		finally
		{
			_unlock();
		}
		
		regToSDK();
	}
	
	/**************
	 * 重置房间状态并重新发起注册
	 */
	protected void resetAndRegToSDK() 
	{
		_lock();
		
		try
		{
			_m_lSerial = ALSerializeMaker.makeNewSerialize();

			_m_bIniting = false;
			_m_bInited = false;
		}
		finally
		{
			_unlock();
		}
		
		regToSDK();
	}
	
	/******************
	 * 销毁数据
	 */
	protected void discard() 
	{
		_lock();
		
		try
		{
			_m_bDeled = true;
			
			NPInterfaceServer.getInstance().getBM().getBM(ChatRoomBO.class).delAll("id", _m_lId);
			
			_commitRegFail(CommErr.DATA_STATE_ERR.getCode());
			
			unRegToSDK();
		}
		finally 
		{
			_unlock();
		}
	}
	
	/*****************
	 * SDK发起注销房间
	 */
	protected void unRegToSDK()
	{
		if(_m_lSdkRoomId > 0)
		{
			//异步发起注销请求
			ALSynTaskManager.getInstance().regTask(()->
			{
				ChatMgr.getInstance().getChatSdk().destroyRoom(_m_lSdkRoomId, new _ICommonDealFunc() {
					
					@Override
					public void dealSuc() 
					{
						CommLog.info("sdkRoomId:{} roomType:{} roomTypeId:{} serverType:{} serverTypeId:{} unreg sdk suc.",
								_m_lSdkRoomId, _m_iRoomType, _m_lRoomTypeId, _m_iServerType, _m_iServerTypeId);
					}
					
					@Override
					public void dealFail(int _err) 
					{
						CommLog.error("sdkRoomId:{} roomType:{} roomTypeId:{} serverType:{} serverTypeId:{} unreg sdk err:{}.",
								_m_lSdkRoomId, _m_iRoomType, _m_lRoomTypeId, _m_iServerType, _m_iServerTypeId, _err);
					}
				});
			});
		}
	}
	
	/*********************
	 * SDK发起注册房间
	 */
	public void regToSDK()
	{
		long serial = 0;
		_lock();
		try
		{
			if(_m_bInited || _m_bIniting || _m_bDeled)
				return;
			
			_m_bIniting = true;
			
			serial = _m_lSerial;
		}
		finally 
		{
			_unlock();
		}
		
		final long curSerial = serial;
		if(_m_lSdkRoomId > 0) //调用加载房间接口
		{
			ChatMgr.getInstance().getChatSdk().loadRoom(_m_lSdkRoomId, new _IInitRoomDealFunc() {
				
				@Override
				public void dealSuc(long _roomSerial) 
				{
					_setRoomSuc(curSerial, _roomSerial);
				}
				
				@Override
				public void dealFail(int _err) 
				{
					_setRoomFail(curSerial, _err);
				}
			});
		}
		else //调用创建房间接口
		{
			ChatMgr.getInstance().getChatSdk().createRoom(new _ICreateRoomDealFunc() {
				
				@Override
				public void dealSuc(long _roomId, long _roomSerial) 
				{
					_setSdkRoomId(_roomId);
					
					_setRoomSuc(curSerial, _roomSerial);
				}
				
				@Override
				public void dealFail(int _err) 
				{
					_setRoomFail(curSerial, _err);
				}
			});
		}
	}
	
	/*******************
	 * 更新SDK传递的聊天房间实例ID
	 * 
	 * @param _sdkRoomId
	 */
	private void _setSdkRoomId(long _sdkRoomId) 
	{
		if(_m_lSdkRoomId == _sdkRoomId)
			return;
		
		_m_lSdkRoomId = _sdkRoomId;
		
		ALMySqlUpdateValue update = new ALMySqlUpdateValue();
		update.addValueObj("sdk_room_id", _m_lSdkRoomId);
		
		NPInterfaceServer.getInstance().getBM().getBM(ChatRoomBO.class).update("id", _m_lId, update);
	}
	
	/*******************
	 * SDK接口返回成功
	 * 
	 * @param _serial
	 * @param _roomSerial
	 */
	private void _setRoomSuc(long _serial, long _roomSerial)
	{
		_lock();
		
		try
		{
			if(_serial != _m_lSerial)
				return;
			
			if(_m_bDeled)
			{
				unRegToSDK();
				return;
			}
			
			_m_bIniting = false;
			_m_bInited = true;
			
			_m_lRoomSerial = _roomSerial;
			
			_commitRegSuc();
			
			CommLog.info("sdkRoomId:{} roomType:{} roomTypeId:{} serverType:{} serverTypeId:{} reg suc."
					, _m_lSdkRoomId, _m_iRoomType, _m_lRoomTypeId, _m_iServerType, _m_iServerTypeId);
		}
		finally
		{
			_unlock();
		}
	}
	
	/***************
	 * SDK接口返回失败
	 * 
	 * @param _err
	 */
	private void _setRoomFail(long _serial, int _err)
	{
		_lock();
		
		try
		{
			if(_serial != _m_lSerial)
				return;
			
			if(_m_bDeled)
				return;
			
			_m_bIniting = false;
			
			_commitRegFail(_err);
			
			//1秒后再次重试
			ALSynTaskManager.getInstance().regTask(()-> 
			{
				regToSDK();
			}, 1000);
		}
		finally
		{
			_unlock();
		}
	}
	
	/*************
	 * 设置最后一次注册提交对象，当注册成功后，提交数据
	 * 
	 * @param _commiter
	 */
	public void setLastRegCommiter(_IWCGBasicRequestCommiter _commiter)
	{
		_lock();
		
		try
		{
			_IWCGBasicRequestCommiter preCommit = _m_cLastRegCommiter;
			if(null != preCommit)
			{
				preCommit.commitFailRes(_m_iRoomType);
			}
			
			_m_cLastRegCommiter = _commiter;
			if(_m_bInited)
			{
				_commitRegSuc();
			}
		}
		finally
		{
			_unlock();
		}
	}
	
	/*****************
	 * 通知提交对象注册成功
	 */
	private void _commitRegSuc()
	{
		_lock();
		
		try
		{
			if(null == _m_cLastRegCommiter)
				return;
			
			_m_cLastRegCommiter.commitSucRes(NP2IS_RB_Writer_001_ISOp.make_001_RetRegRoom(_m_lSdkRoomId));
			
			_m_cLastRegCommiter = null;
		}
		finally
		{
			_unlock();
		}
	}
	
	/*******************
	 * 通知提交对象注册失败
	 */
	private void _commitRegFail(int _err)
	{
		_lock();
		
		try
		{
			if(null == _m_cLastRegCommiter)
				return;
			
			_m_cLastRegCommiter.commitFailRes(_err);
			
			_m_cLastRegCommiter = null;
		}
		finally
		{
			_unlock();
		}
	}
}
