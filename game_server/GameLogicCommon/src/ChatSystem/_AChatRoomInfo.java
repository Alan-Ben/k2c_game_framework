package ChatSystem;

import ALBasicCommon.ALSerializeMaker;
import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.ServerObj.ServerObj_ChatUser;
import GS2GC.p022_ChatOp.GS2GC_022_051_OnChatRoomDisconnect;
import NP2IS_RB.p001_ISOp.*;
import NPCommon.ErrMain.ChatErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Log.CommLog;
import NPCommon.Util.CallBack._ICallBackResult;
import NPCommon.Util.CallBack._ICallBackResultT;
import NPCommon.Util.CommonFunc;
import NPCommon.Util._ABasicServerObj;
import NPEnum.ENPChatMsgType;
import NPEnum.ENPChatRoomType;
import NPServerProtocolWriter.NP2IS.Request.Np2IS_R_Writer_001_ISOp;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCS2US_R.p003_CommOp.NP2US_R_003_001_ReqSendProtocol;
import WCGCommon.Enum.NPEnum;

import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.HashSet;
import java.util.Map;

public abstract class _AChatRoomInfo
{
    //服务器对象
    private _ABasicServerObj _m_server;

    //聊天房间类型
    private ENPChatRoomType _m_eRoomType;
    //聊天房间类型ID
    private long _m_lRoomTypeId;

    //数据序列号，用于异步回调检查，避免重复发起
    private long _m_lSerial;
    //聊天服务器返回的实际房间ID
    private long _m_lRoomSdkId;

    //加入聊天房间的成员ID集合，用于对房间进行群发处理
    private ArrayList<ServerObj_ChatUser> _m_alChatUserList;
    //所有US服务器ID集合
    private HashSet<Integer> _m_hsUsIdSet;

    //锁对象
    private MutexAtom _m_mutex;

    /**
     * 房间销毁时调用
     */
    abstract protected void _onDiscard();

    public _AChatRoomInfo(_ABasicServerObj _server, ENPChatRoomType _roomType, long _roomTypeId)
    {
        _m_server = _server;
        _m_eRoomType = _roomType;
        _m_lRoomTypeId = _roomTypeId;

        _m_lSerial = ALSerializeMaker.makeNewSerialize();
        _m_lRoomSdkId = 0;

        _m_alChatUserList = new ArrayList<>();
        _m_hsUsIdSet = new HashSet<>();

        _m_mutex = new MutexAtom();
    }

    private void _lock() {_m_mutex.lock();}
    private void _unlock() {_m_mutex.unlock();}

    public _ABasicServerObj getServer() {return _m_server;}
    public ENPChatRoomType getRoomType() {return _m_eRoomType;}
    public long getRoomTypeId() {return _m_lRoomTypeId;}
    public long getRoomSdkId() {return _m_lRoomSdkId;}

    /**
     * 获取当前房间里所有玩家所在的US服务器ID列表
     * @return
     */
    public ArrayList<Integer> getUsIdList()
    {
        _lock();

        try
        {
            return new ArrayList<>(_m_hsUsIdSet);
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 注册聊天房间
     * @param _serial
     */
    private void _RegChatRoom(final long _serial)
    {
        _lock();

        try
        {
            if(_serial != _m_lSerial)
                return;

            _m_server.sendRequestToBSServer(NPEnum.EServerType.SINGLE.ordinal(), NPEnum.ENPSingleServerType.INTERFACE.ordinal(),
                    Np2IS_R_Writer_001_ISOp.make_001_ReqRegRoom(_m_eRoomType.ordinal(), _m_lRoomTypeId),
                    new _IWCGCallbackDealer()
                    {
                        @Override
                        public void dealSuc(_IALProtocolStructure _ret)
                        {
                            NP2IS_RB_001_001_RetRegRoom proto = (NP2IS_RB_001_001_RetRegRoom) _ret;

                            _RoomRegSuc(_serial, proto.getSdkRoomId());
                        }

                        @Override
                        public void dealFail(int _err)
                        {
                            _RoomRegFail(_serial);
                        }

                        @Override
                        public _IALProtocolStructure createProtocolObj()
                        {
                            return new NP2IS_RB_001_001_RetRegRoom();
                        }
                    });
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 房间注册成功
     * @param _serial
     * @param _roomId
     */
    private void _RoomRegSuc(final long _serial, final long _roomId)
    {
        _lock();

        try
        {
            //序列号不一致，不予处理
            if(_serial != _m_lSerial)
                return;

            _m_lRoomSdkId = _roomId;
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 房间注册失败
     * @param _serial
     */
    private void _RoomRegFail(final long _serial)
    {
        _lock();

        try
        {
            //序列号不一致，不予处理
            if(_serial != _m_lSerial)
                return;

            //1秒后重试
            ALSynTaskManager.getInstance().regTask(() ->
            {
                _RegChatRoom(_serial);
            }, 1000);
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 注销聊天房间
     * @param _serial
     */
    private void _UnRegChatRoom(final long _serial)
    {
        _lock();

        try
        {
            //序列号不一致，不予处理
            if(_serial != _m_lSerial)
                return;

            _m_server.sendRequestToBSServer(NPEnum.EServerType.SINGLE.ordinal(), NPEnum.ENPSingleServerType.INTERFACE.ordinal(),
                    Np2IS_R_Writer_001_ISOp.make_002_ReqUnRegRoom(_m_eRoomType.ordinal(), _m_lRoomTypeId),
                    new _IWCGCallbackDealer()
                    {
                        @Override
                        public void dealSuc(_IALProtocolStructure _ret)
                        {
                        }

                        @Override
                        public void dealFail(int _err)
                        {
                            _RoomUnRegFail(_serial);
                        }

                        @Override
                        public _IALProtocolStructure createProtocolObj()
                        {
                            return new NP2IS_RB_001_002_RetUnRegRoom();
                        }
                    });
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 房间注销失败
     * @param _serial
     */
    private void _RoomUnRegFail(final long _serial)
    {
        _lock();

        try
        {
            //序列号不一致，不予处理
            if(_serial != _m_lSerial)
                return;

            //1秒后重试
            ALSynTaskManager.getInstance().regTask(() ->
            {
                _UnRegChatRoom(_serial);
            }, 1000);
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 开启注册房间
     */
    public void StartReg()
    {
        long serial = 0;

        _lock();

        try
        {
            _m_lSerial = ALSerializeMaker.makeNewSerialize();
            serial = _m_lSerial;

            _RegChatRoom(serial);
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 开启注销房间
     */
    public void startUnReg()
    {
        long serial = 0;

        _lock();

        try
        {
            _m_lSerial = ALSerializeMaker.makeNewSerialize();
            serial = _m_lSerial;

            _UnRegChatRoom(serial);
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
    public ServerObj_ChatUser lookupChatUser(long _cid)
    {
        _lock();

        try
        {
            for(int i = 0; i < _m_alChatUserList.size(); i++)
            {
                ServerObj_ChatUser chatUser = _m_alChatUserList.get(i);
                if(chatUser.getCid() == _cid)
                {
                    return chatUser;
                }
            }

            return null;
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 增加聊天用户
     * @param _chatUser
     */
    public void addChatUser(ServerObj_ChatUser _chatUser)
    {
        _lock();

        try
        {
            if(null != lookupChatUser(_chatUser.getCid()))
                return;

            _m_alChatUserList.add(_chatUser);

            //增加US服务器ID记录
            int usId = CommonFunc.parseServerTypeIdFromCid(_chatUser.getCid());
            if(usId > 0)
            {
                _m_hsUsIdSet.add(usId);
            }
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 移除聊天用户
     * @param _cid
     * @return
     */
    private ServerObj_ChatUser _removeChatUser(long _cid)
    {
        _lock();

        try
        {
            for(int i = 0; i < _m_alChatUserList.size(); i++)
            {
                ServerObj_ChatUser chatUser = _m_alChatUserList.get(i);
                if(chatUser.getCid() == _cid)
                {
                    _m_alChatUserList.remove(i);
                    return chatUser;
                }
            }

            return null;
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 加入聊天房间
     * @param _chatUser
     * @param _callback
     */
    public void JoinRoom(ServerObj_ChatUser _chatUser, _ICallBackResultT<Long> _callback)
    {
        _lock();

        try
        {
            //尚未注册完成聊天房间
            if(_m_lRoomSdkId <= 0)
            {
                _callback.onRunOver(ChatErr.CHAT_ROOM_NOT_INITED, 0L);
                return;
            }

            //先设置加入聊天房间
            addChatUser(_chatUser);

            //向IS请求创建聊天用户
            _m_server.sendRequestToBSServer(NPEnum.EServerType.SINGLE.ordinal(), NPEnum.ENPSingleServerType.INTERFACE.ordinal(),
                    Np2IS_R_Writer_001_ISOp.make_004_ReqJoinRoom(_chatUser.getChatUid(), _m_lRoomSdkId),
                    new _IWCGCallbackDealer()
                    {
                        @Override
                        public _IALProtocolStructure createProtocolObj()
                        {
                            return new NP2IS_RB_001_004_RetJoinRoom();
                        }

                        @Override
                        public void dealSuc(_IALProtocolStructure _retProto)
                        {
                            _callback.onRunOver(Result.SUCC, _m_lRoomSdkId);
                        }

                        @Override
                        public void dealFail(int _err)
                        {
                            _callback.onRunOver(ChatErr.CHAT_USER_JOIN_ROOM_FAIL, 0L);
                        }
                    });
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 退出聊天房间
     * @param _cid
     */
    public void QuitRoom(long _cid)
    {
        _lock();

        try
        {
            //尚未注册完成聊天房间
            if(_m_lRoomSdkId <= 0)
                return;

            //检查用户是否在房间内
            ServerObj_ChatUser chatUser = _removeChatUser(_cid);
            if(null == chatUser)
                return;

            //向IS请求创建聊天用户
            _m_server.sendRequestToBSServer(NPEnum.EServerType.SINGLE.ordinal(), NPEnum.ENPSingleServerType.INTERFACE.ordinal(),
                    Np2IS_R_Writer_001_ISOp.make_005_ReqQuitRoom(chatUser.getChatUid(), _m_lRoomSdkId),
                    new _IWCGCallbackDealer()
                    {
                        @Override
                        public _IALProtocolStructure createProtocolObj()
                        {
                            return new NP2IS_RB_001_005_RetQuitRoom();
                        }

                        @Override
                        public void dealSuc(_IALProtocolStructure _retProto)
                        {
                        }

                        @Override
                        public void dealFail(int _err)
                        {
                            //输出日志
                            CommLog.error("ChatRoomInfo.QuitRoom fail, cid:{} err:{}", _cid, _err);
                        }
                    });
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 发送消息到聊天房间
     * @param _cid
     * @param _msgType
     * @param _userContent
     * @param _msgContent
     * @param _callback
     */
    public void SendRoomMsg(long _cid, ENPChatMsgType _msgType, ByteBuffer _userContent, ByteBuffer _msgContent, _ICallBackResult _callback)
    {
        _lock();

        try
        {
            //尚未注册完成聊天房间
            if(_m_lRoomSdkId <= 0)
            {
                if(null != _callback)
                    _callback.onRunOver(Result.failed(ChatErr.CHAT_ROOM_NOT_INITED.getCode()));
                return;
            }

            //检查用户是否在房间内，cid-0：系统用户
            String chatUid = "0";
            if(_cid > 0)
            {
                ServerObj_ChatUser chatUser = lookupChatUser(_cid);
                if(null == chatUser)
                {
                    if(null != _callback)
                        _callback.onRunOver(Result.failed(ChatErr.CHAT_USER_JOIN_ROOM_FAIL.getCode()));
                    return;
                }

                chatUid =  chatUser.getChatUid();
            }

            //发送消息
            _m_server.sendRequestToBSServer(NPEnum.EServerType.SINGLE.ordinal(), NPEnum.ENPSingleServerType.INTERFACE.ordinal(),
                    Np2IS_R_Writer_001_ISOp.make_006_ReqSendRoomMsg(chatUid, _m_lRoomSdkId, _msgType.ordinal(), _userContent, _msgContent),
                    new _IWCGCallbackDealer()
                    {
                        @Override
                        public void dealSuc(_IALProtocolStructure _ret)
                        {
                            if(null != _callback)
                                _callback.onRunOver(Result.SUCC);
                        }

                        @Override
                        public void dealFail(int _err)
                        {
                            if(null != _callback)
                                _callback.onRunOver(Result.failed(_err));
                        }

                        @Override
                        public _IALProtocolStructure createProtocolObj()
                        {
                            return new NP2IS_RB_001_006_RetSendRoomMsg();
                        }
                    });
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 对房间里里所有玩家进行广播协议
     * @param _proto
     */
    public void Broadcast(_IALProtocolStructure _proto)
    {
        _lock();

        try
        {
            //根据US整理玩家列表，进行分批广播
            HashMap<Integer, ArrayList<Long>> usCidMap = new HashMap<>();
            for(int i = 0; i < _m_alChatUserList.size(); i++)
            {
                ServerObj_ChatUser chatUser = _m_alChatUserList.get(i);
                if(null == chatUser)
                    continue;

                int usId = CommonFunc.parseServerTypeIdFromCid(chatUser.getCid());
                if(usId <= 0)
                {
                    CommLog.error("ChatRoomInfo.broadcast fail, invalid usId parsed from cid:{}", chatUser.getCid());
                    continue;
                }

                usCidMap.computeIfAbsent(usId, k -> new ArrayList<>()).add(chatUser.getCid());
            }

            for(Map.Entry<Integer, ArrayList<Long>> entry : usCidMap.entrySet())
            {
                int usId = entry.getKey();
                ArrayList<Long> cidList = entry.getValue();

                NP2US_R_003_001_ReqSendProtocol proto = new NP2US_R_003_001_ReqSendProtocol();
                proto.getCidList().addAll(cidList);
                proto.setProtocol(_proto.makeFullPackage());

                _m_server.sendRequestToBSServer(NPEnum.EServerType.USER.ordinal(), usId, proto);
            }
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 通知聊天玩家重新加入房间并重置加房间玩家数据
     */
    protected void _reset()
    {
        _lock();

        try
        {
            _m_lSerial = ALSerializeMaker.makeNewSerialize();

            //通知房间里所有用户房间错误，用于客户端重新发起加入房间
            GS2GC_022_051_OnChatRoomDisconnect proto = new GS2GC_022_051_OnChatRoomDisconnect();
            proto.setRoomId(_m_lRoomSdkId);

            Broadcast(proto);

            //清空加入玩家数据
            _m_alChatUserList.clear();
            _m_hsUsIdSet.clear();
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 销毁聊天房间对象，清理资源
     */
    protected void _discard()
    {
        _lock();

        try
        {
            _m_lSerial = ALSerializeMaker.makeNewSerialize();

            //外部销毁处理
            ALSynTaskManager.getInstance().regTask(()->
            {
                _onDiscard();
            });
        }
        finally
        {
            _unlock();
        }
    }
}
