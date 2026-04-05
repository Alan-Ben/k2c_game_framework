package NPGateServer.NPGCListener;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALSocket._AALBasicServerSocketListener;
import ALBasicServer.ALTask._IALSynTask;
import ALServerLog.ALServerLog;
import Common.NpServerObj.NpServerObj_SYS_ServerIndexInfo;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPGateServer.NPClientReloginMgr;
import NPGateServer.NPGCCheckCodeMgr.NPGCCheckCodeInfo;
import NPGateServer.NPGCListener.Writer.NPGS2GCWriter_001_BasicOp;
import NPGateServer.NPGCMsgMgr.NPGCMsgDealer;
import NPGateServer.NPGCMsgMgr.NPGCMsgMgr;
import NPGateServer.NPGSGCMgr.NPGSGCMgr;
import NPGateServer.NPGateServer;
import NPGateServer.USServerInfoListMgr.USServerIndexInfoListMgr;
import WCGCommon.Enum.NPEnum.EWCGCountryType;

import java.nio.ByteBuffer;

/**************
 * 客户端连接的监听对象
 *
 * @author Administrator
 *
 */
public class NPGSGCListener extends _AALBasicServerSocketListener
{
    /**
     * Gate监听序列号
     */
    private static long _g_ClientSessionId = 1;

    private synchronized long _GetNewSerialize()
    {
        return _g_ClientSessionId++;
    }

    private long _m_lClientSessionId;
    /**
     * 用户Id
     */
    private String _m_sUid;
    /**
     * 用户所属国籍
     */
    private EWCGCountryType _m_eCountryType;
    /**
     * 断线重连key
     */
    private String _mReloginKey;
    /**
     * 客户端上传自定义数据
     */
    private String _m_customData;

    /**
     * US服务器连接信息对象
     */
    private NPGSGCUSInfo _m_uiUsInfo;

    /**
     * 本连接对象对应的消息处理对象
     */
    private NPGCMsgDealer _m_msgDealer;

    private boolean _m_bIsEnable;

    public NPGSGCListener(NPGCCheckCodeInfo _checkInfo, String _customData)
    {
        _m_lClientSessionId = _GetNewSerialize();
        _m_sUid = _checkInfo.getUid();
        _m_eCountryType = EWCGCountryType.CHINA;
        _m_customData = _customData;

        //设置为null，在成功登录后会被重置
        _mReloginKey = null;

        _m_uiUsInfo = null;

        _m_msgDealer = NPGCMsgMgr.getInstance().tryGetMsgDealer(_m_sUid);
        _m_msgDealer.setNotCheckReconnect();

        _m_bIsEnable = true;
    }

    public NPGSGCListener(String _uid, String _customData)
    {
        _m_lClientSessionId = _GetNewSerialize();
        _m_sUid = _uid;
        _m_eCountryType = EWCGCountryType.CHINA;
        _m_customData = _customData;

        //设置为null，在成功登录后会被重置
        _mReloginKey = null;

        _m_uiUsInfo = null;

        _m_msgDealer = NPGCMsgMgr.getInstance().tryGetMsgDealer(_m_sUid);
        _m_msgDealer.setNotCheckReconnect();

        _m_bIsEnable = true;
    }

    public NPGSGCListener(String _uid, String _customData, String _reloginKey)
    {
        _m_lClientSessionId = _GetNewSerialize();
        _m_sUid = _uid;
        _m_eCountryType = EWCGCountryType.CHINA;
        _m_customData = _customData;

        //设置值，延续原先key，不重置
        _mReloginKey = _reloginKey;

        _m_uiUsInfo = null;

        _m_msgDealer = NPGCMsgMgr.getInstance().tryGetMsgDealer(_m_sUid);
        _m_msgDealer.setNotCheckReconnect();

        _m_bIsEnable = true;
    }

    public long getSessionId()
    {
        return _m_lClientSessionId;
    }

    public String getUid()
    {
        return _m_sUid;
    }

    public EWCGCountryType getCountryType()
    {
        return _m_eCountryType;
    }

    public String getClientCustomData()
    {
        return _m_customData;
    }

    public NPGCMsgDealer getMsgDealer()
    {
        return _m_msgDealer;
    }

    public NPGSGCUSInfo getUSInfo()
    {
        return _m_uiUsInfo;
    }

    @Override
    public void disconnect()
    {
        _m_bIsEnable = false;
        CommLog.sys("Uid:{} Disconnect!", _m_sUid);
        //调用key处理
        NPClientReloginMgr.getInstance().onUserDisconnect(_m_sUid, _mReloginKey);

        if(null != _m_uiUsInfo) {
            // 注销对象 - 不在此注销，将在返回踢出操作的地方进行注销处理，这里可能导致断线重连的对象无法继续发送对应消息
            //WCGGSGCMgr.getInstance().unregGCListener(this);
        }
        else
        {
            //没有连接到US数据则直接断开
            NPGSGCMgr.getInstance().unregGCListener(this);
        }
        //发送消息减少用户承载
        NPGateServer.getInstance().reduceHandleUser(_m_sUid);

        //注销对应US连接
        unregUSInfo();
    }

    @Override
    public void login()
    {
        CommLog.sys("player Uid:{} Login!", _m_sUid);
        // 注册本对象到管理器中
        NPGSGCListener preListener = NPGSGCMgr.getInstance().regGCListener(this);
        if (null != preListener)
        {
            //调用被踢处理
            preListener.onDeviceKickout();
        }

        // 设置断线重连Key
        if (null == _mReloginKey)
            this._mReloginKey = CommonFunc.genRandomStr(32);
        // 断线重连
        NPClientReloginMgr.getInstance().regReloginSession(getUid(), _m_lClientSessionId, _mReloginKey);
        // 发送消息通知客户端相关操作可以继续
        send(NPGS2GCWriter_001_BasicOp.make_003_RetReloginKey(_mReloginKey));
    }

    /************
     * 客户端消息处理，如未处理成功则通过USInfo转发
     */
    @Override
    public void receiveMsg(ByteBuffer _msg)
    {
        if (!_m_bIsEnable)
            return;

        if (!NPGCMsgDispather.getInstance().DealProtocol(this, _msg))
        {
            //处理失败，此时将数据转发到对应的用户数据服务器
            _msg.position(0);

            //到此则需要报错，正确的处理是在1-22的地方的处理
            System.err.println(getUid() + "接收到错误消息，正常消息需要在1-22发送 Msg: " + _msg.get(0) + " - " + _msg.get(1));

            //转发数据
            resendToUS(_msg);
        }
    }

    /******************
     * 初始化US信息
     * @param _serverLogicId
     */
    public void requestEnterUS(int _serverLogicId, long _clientInitSerialize)
    {
        //如果原先存在数据则不允许进行新的处理，需要重新建立连接
        if (null != _m_uiUsInfo)
        {
            ALServerLog.Error("Can not init USInfo when usInfo is not null!");
            return;
        }

        NpServerObj_SYS_ServerIndexInfo indexInfo = USServerIndexInfoListMgr.getInstance().lookupByLogicId(_serverLogicId);
        int serverTypeId = _serverLogicId;
        //有效数据的时候直接使用有效数据，否则使用外部Id直接作为服务器内部数据Id处理。方便不同登录方式
        if (indexInfo != null)
        {
            serverTypeId = indexInfo.getServerTypeId();
        }

        //构建新对象
        _m_uiUsInfo = new NPGSGCUSInfo(this, _serverLogicId, serverTypeId, _clientInitSerialize);
        //开始处理初始化
        _m_uiUsInfo.requestEnterUS(_clientInitSerialize);
    }

    /******************
     * 尝试还原Us连接对象
     * @param _serverLogicId
     */
    public void resumeUSConnection(int _serverLogicId, long _clientInitSerialize)
    {
        //如果原先存在数据则不允许进行新的处理，需要重新建立连接
        if (null != _m_uiUsInfo)
        {
            ALServerLog.Error("Can not init USInfo when usInfo is not null!");
            return;
        }

        NpServerObj_SYS_ServerIndexInfo indexInfo = USServerIndexInfoListMgr.getInstance().lookupByLogicId(_serverLogicId);
        int serverTypeId = _serverLogicId;
        //有效数据的时候直接使用有效数据，否则使用外部Id直接作为服务器内部数据Id处理。方便不同登录方式
        if (indexInfo != null)
        {
            serverTypeId = indexInfo.getServerTypeId();
        }

        //构建新对象
        _m_uiUsInfo = new NPGSGCUSInfo(this, _serverLogicId, serverTypeId, _clientInitSerialize);
        //开始处理初始化
        _m_uiUsInfo.resumeUSConnection(_clientInitSerialize);
    }

    /******************
     * 退出对应的US服务器
     * @param _usId
     * @return
     */
    public Result quitUS(int _usId)
    {
        if (null == _m_uiUsInfo)
            return CommErr.SYS_ERR;

        if (_m_uiUsInfo.getUSId() != _usId)
            return CommErr.DATA_STATE_ERR;

        //注销信息
        unregUSInfo();

        return Result.SUCC;
    }

    /********************
     * 注销US信息
     * @param _usId
     */
    public void unregUSInfo()
    {
        if (null == _m_uiUsInfo)
            return;

        unregUSInfo(_m_uiUsInfo.getInfoSerialize());
    }

    public void unregUSInfo(long _infoSerialize)
    {
        if (null == _m_uiUsInfo || _m_uiUsInfo.getInfoSerialize() != _infoSerialize)
            return;

        //处理注销
        _m_uiUsInfo.unregUSInfo();
        _m_uiUsInfo = null;
    }

    /******************************
     * 在用户服务器相关操作初始化完成时的相关处理
     *
     * @param _newSerialize
     */
    public void onUSRegGSDone(long _infoSerialize, long _userSerialize)
    {
        //调用信息对象处理
        if (null != _m_uiUsInfo)
        {
            //正常的登录成功，需要在reg之前发送20，避免在regdone的时候由于客户端会同时发送大量初始化协议，导致大量初始化协议重复发送报错
            //发送重连消息验证
            send(NPGS2GCWriter_001_BasicOp.make_020_RetReconnectInfo(getMsgDealer().getReceivedMsgCount()));

            //设置连接注册完成
            _m_uiUsInfo.onUSRegGSDone(_infoSerialize, _userSerialize);
        }
    }

    /******************************
     * 在用户服务器相关操作初始化完成时的相关处理
     *
     * @param _newSerialize
     */
    public void onUSResumeGSDone(long _infoSerialize, long _userSerialize)
    {
        //调用信息对象处理
        if (null != _m_uiUsInfo)
        {
            _m_uiUsInfo.onUSResumeGCDone(_infoSerialize, _userSerialize);

            //发送重连消息验证
            send(NPGS2GCWriter_001_BasicOp.make_020_RetReconnectInfo(getMsgDealer().getReceivedMsgCount()));
        }
    }

    /****************
     * 将消息转发到US服务器
     */
    public void resendToUS(ByteBuffer _msg)
    {
        if (null == _m_uiUsInfo)
        {
            ALServerLog.Error("Send Msg to Empty USInfo!");
            return;
        }

        _m_uiUsInfo.resendToUS(_msg);
    }

    public void resendToUS(long _clientRequestSerialize, byte[] _msg)
    {
        if (null == _m_uiUsInfo)
        {
            ALServerLog.Error("Send Msg to Empty USInfo!");
            return;
        }

        _m_uiUsInfo.resendToUS(_clientRequestSerialize, _msg);
    }

    /******************************
     * 处理踢出Gate服的操作
     *
     * @param _kickType
     */
    public void onGCUSKickout(int _kickType)
    {
        _m_bIsEnable = false;
        CommLog.info("WCGUS2GS_001_002_UserGateKicked " + _m_sUid + " kicked type: " + _kickType);

        // 删除重连key
        NPClientReloginMgr.getInstance().removeReloginSession(_m_sUid, _m_lClientSessionId);

        // 注销对象
        _discardGC();

        //断开连接
        this.logout();
    }

    /******************************
     * 被设备顶下的特殊处理。延迟10秒进行logout
     *
     * @param _kickType
     */
    public void onDeviceKickout()
    {
        //发送被踢消息到客户端
        send(NPGS2GCWriter_001_BasicOp.make_030());

        _m_bIsEnable = false;
        CommLog.info("WCGUS2GS_001_002_UserGateKicked " + _m_sUid + " kicked type: DVICE");

        // 删除重连 session，防止用户已在其他 GS 登录时本 GS 的 session 成为孤儿
        NPClientReloginMgr.getInstance().removeReloginSession(_m_sUid, _m_lClientSessionId);

        // 注销对象
        _discardGC();

        NPGSGCListener thisListener = this;
        //断开连接
        ALSynTaskManager.getInstance().regTask(new _IALSynTask()
        {

            @Override
            public void run()
            {
                thisListener.logout();
            }
        }, 10000);
    }

    /**
     * 释放GC相关数据
     */
    protected void _discardGC()
    {
        // 注销对象
        NPGSGCMgr.getInstance().unregGCListener(this);
        //清除数据发送部分处理对象
        NPGCMsgMgr.getInstance().removeMsgDealer(_m_msgDealer, _m_lClientSessionId);

        //注销对应US连接
        unregUSInfo();
    }

    /**************
     * 接收的消息长度超出时调用的函数
     */
    @Override
    public void onBuffLengthOverSize(ByteBuffer _srcBuf, ByteBuffer _curReadingBuf)
    {
    }
}
