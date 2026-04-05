package ChatSystem;

import ALBasicServer.ALBasicMutex.MutexObject;
import NPCommon.Util._ABasicServerObj;
import NPEnum.ENPChatRoomType;

import java.util.ArrayList;

public class ChatRoomMgr
{
    //服务器对象
    private _ABasicServerObj _m_server;

    //房间列表
    private ArrayList<_AChatRoomInfo> _m_alRoomList;

    //锁对象
    private MutexObject _m_mutex;

    public ChatRoomMgr(_ABasicServerObj _server)
    {
        _m_server = _server;

        _m_alRoomList = new ArrayList<>();

        _m_mutex = new MutexObject();
    }

    private void _lock() {_m_mutex.lock();}
    private void _unlock() {_m_mutex.unlock();}

    /**
     * 查找对应的聊天房间对象
     * @param _roomType
     * @param _roomTypeId
     * @return
     */
    public _AChatRoomInfo lookupRoom(ENPChatRoomType _roomType, long _roomTypeId)
    {
        _lock();

        try
        {
            for(int i = 0; i < _m_alRoomList.size(); i++)
            {
                _AChatRoomInfo room = _m_alRoomList.get(i);
                if(null == room)
                    continue;

                if(room.getRoomType() == _roomType && room.getRoomTypeId() == _roomTypeId)
                    return room;
            }

            return null;
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 查找指定的聊天房间（通过聊天房间ID）
     * @param _roomId
     * @return
     */
    public _AChatRoomInfo lookupRoomById(long _roomId)
    {
        if(_roomId <= 0)
            return null;

        _lock();

        try
        {
            for (int i = 0; i < _m_alRoomList.size(); i++)
            {
                _AChatRoomInfo room = _m_alRoomList.get(i);
                if(null == room)
                    continue;

                if(room.getRoomSdkId() == _roomId)
                    return room;
            }

            return null;
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 注册聊天房间，并开启注册流程
     * @param _room
     */
    public void regRoom(_AChatRoomInfo _room)
    {
        _lock();

        try
        {
            //不允许重复注册
            if(null != lookupRoom(_room.getRoomType(), _room.getRoomTypeId()))
                return;

            _m_alRoomList.add(_room);

            _room.StartReg();
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 注销聊天房间，并开启注销流程
     * @param _room
     */
    public void unRegRoom(_AChatRoomInfo _room)
    {
        _lock();

        try
        {
            _m_alRoomList.remove(_room);
            _room._discard();

            _room.startUnReg();
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 聊天房间错误时处理
     * @param _roomId
     * @return
     */
    public void onRomErr(long _roomId)
    {
        if(_roomId <= 0)
            return;

    	_lock();

    	try
    	{
    		_AChatRoomInfo room = lookupRoomById(_roomId);
    		if(null == room)
    			return;

            room._reset();
    	}
    	finally
    	{
    		_unlock();
    	}
    }
}
