package NPGateServer.NPGCListener;

import ALBasicCommon.ALSerializeMaker;
import ALBasicProtocolPack._IALProtocolStructure;
import ALServerLog.ALServerLog;
import NP2US_RB.p002_GSOp.NP2US_RB_002_006_RetQueueInfo;
import NP2US_RB.p002_GSOp.NP2US_RB_002_007_RetQuitQueueRes;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Log.CommLog;
import NPGateServer.GateServerConf;
import NPGateServer.NPGCListener.Writer.NPGS2GCWriter_001_BasicOp;
import NPGateServer.NPGSCallBack.NPGS2US_RB_Callback_RegUserGate;
import NPGateServer.NPGSCallBack.NPGS2US_RB_Callback_RegUserGateByResumeGC;
import NPGateServer.NPGSCallBack.NPGS2US_RB_Callback_ReqEnterUS;
import NPGateServer.NPGSCallBack.NPGS2US_RB_Callback_ReqResumeUS;
import NPGateServer.NPGateServer;
import NPServerProtocolWriter.NP2US.Msg.NP2US_Writer_001_BasicOp;
import NPServerProtocolWriter.NP2US.Request.NP2US_R_Writer_002_GSOp;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum.EServerType;

import java.nio.ByteBuffer;

/*******************
 * 玩家对应US的信息存储和管理对象
 * @author mj
 *
 */
public class NPGSGCUSInfo
{
    private NPGSGCListener _m_lListener;

    //信息序列号，使用全局获取，保证唯一性
    private long _m_lInfoSerialize;
    //对应队列的索引
    private long _m_lQueueIndex;

    //用户服务器logicId
    private int _m_iUSLogicId;
    //US服务器Id
    private int _m_iUSId;
    //对应用户Cid
    private long _m_lCid;
    /**
     * 用户对应数据的序列号
     */
    private long _m_lUserSerialize;
    /**
     * 客户端登录的序列号
     */
    private long _m_lClientInitSerialize;

    //是否已经注册完毕，需要发送消息到US设置连接信息
    private boolean _m_bIsStartInit;
    /**
     * 用户是否已经连接到对应的用户服务器
     */
    private boolean _m_bIsInitUSDone;

    public NPGSGCUSInfo(NPGSGCListener _listener, int _usLogicId, int _usId, long _clientInitSerialize)
    {
        _m_lListener = _listener;

        _m_lInfoSerialize = ALSerializeMaker.makeNewSerialize();
        _m_lQueueIndex = -1;

        _m_iUSId = _usId;
        _m_iUSLogicId = _usLogicId;
        _m_lCid = 0;
        _m_lUserSerialize = 0;
        _m_lClientInitSerialize = _clientInitSerialize;

        _m_bIsStartInit = false;
        _m_bIsInitUSDone = false;
    }

    public NPGSGCListener getListener()
    {
        return _m_lListener;
    }

    public int getUSId()
    {
        return _m_iUSId;
    }

    public int getUSLogicId()
    {
        return _m_iUSLogicId;
    }

    public long getInfoSerialize()
    {
        return _m_lInfoSerialize;
    }

    public long getQueueIndex()
    {
        return _m_lQueueIndex;
    }

    public long getUserSerialize()
    {
        return _m_lUserSerialize;
    }

    public long getClientInitSerialize()
    {
        return _m_lClientInitSerialize;
    }

    public boolean isInitUSDone()
    {
        return _m_bIsInitUSDone;
    }

    /*************
     * 设置本对象的队列信息
     * @param _infoSerialize
     * @param _queueIndex
     */
    public void setQueueIndex(long _infoSerialize, long _queueIndex)
    {
        if (_m_lInfoSerialize != _infoSerialize)
            return;

        _m_lQueueIndex = _queueIndex;
    }

    /********************
     * 初始化GS信息
     */
    public void requestEnterUS(long _clientInitSerialize)
    {
        if (_m_bIsStartInit)
            return;

        //设置状态，开始处理
        _m_bIsStartInit = true;

        // 发送消息到用户信息服务器对象注册连接服务器
        NPGateServer.getInstance().sendRequestToBSServer(EServerType.USER.ordinal(), _m_iUSId,
                NP2US_R_Writer_002_GSOp.make_005_ReqEnterUS(_m_lListener.getUid(), _m_lListener.getSessionId(), _m_lInfoSerialize, _m_lListener.getClientCustomData())
                , new NPGS2US_RB_Callback_ReqEnterUS(this, _clientInitSerialize));
    }

    /********************
     * 初始化GS信息，恢复原有连接的方式进行处理
     */
    public void resumeUSConnection(long _clientInitSerialize)
    {
        if (_m_bIsStartInit)
            return;

        //设置状态，开始处理
        _m_bIsStartInit = true;

        // 发送消息到用户信息服务器对象注册连接服务器
        NPGateServer.getInstance().sendRequestToBSServer(EServerType.USER.ordinal(), _m_iUSId,
                NP2US_R_Writer_002_GSOp.make_004_ReqResumeUSInfo(_m_lListener.getUid(), _m_lListener.getSessionId(), _m_lInfoSerialize)
                , new NPGS2US_RB_Callback_ReqResumeUS(this, _clientInitSerialize));
    }


    /******************************
     * 在用户服务器相关操作初始化完成时的相关处理
     *
     * @param _newSerialize
     */
    public void onGCUSInited(long _infoSerialize, long _cid)
    {
        //判断信息序列号是否匹配
        if (_infoSerialize != _m_lInfoSerialize)
            return;

        CommLog.sys("US Inited Uid:{} ,cid:{}", _m_lListener.getUid(), _cid);

        _m_lCid = _cid;

        //发送请求注册GS连接对象
        String remoteIP = _m_lListener.getSocket().getIP();
        // 发送消息到用户信息服务器对象注册连接服务器
        NPGateServer.getInstance().sendRequestToBSServer(EServerType.USER.ordinal(), _m_iUSId,
                NP2US_R_Writer_002_GSOp.make_001_RegUserGate(_m_lCid, _m_lListener.getSessionId()
                        , _m_lListener.getMsgDealer().getSerialize(), remoteIP, _m_lListener.getClientCustomData(), GateServerConf.getInstance().getAreaTag())
                , new NPGS2US_RB_Callback_RegUserGate(this));
    }

    /******************************
     * 在尝试向us恢复连接成功的时候的后续处理
     *
     * @param _newSerialize
     */
    public void onGCUSResumeComfirmed(long _infoSerialize, long _cid)
    {
        //判断信息序列号是否匹配
        if (_infoSerialize != _m_lInfoSerialize)
            return;

        CommLog.sys("US resume Uid:{} ,cid:{}", _m_lListener.getUid(), _cid);

        _m_lCid = _cid;

        //发送请求注册GS连接对象
        String remoteIP = _m_lListener.getSocket().getIP();
        // 发送消息到用户信息服务器对象注册连接服务器
        NPGateServer.getInstance().sendRequestToBSServer(EServerType.USER.ordinal(), _m_iUSId,
                NP2US_R_Writer_002_GSOp.make_001_RegUserGate(_m_lCid, _m_lListener.getSessionId()
                        , _m_lListener.getMsgDealer().getSerialize(), remoteIP, _m_lListener.getClientCustomData(), GateServerConf.getInstance().getAreaTag())
                , new NPGS2US_RB_Callback_RegUserGateByResumeGC(this));
    }

    /******************
     * 申请当前US排队最前队列索引
     * @param _commiter
     */
    public void requestUSCurIndex(long _clientSerialize)
    {
        //如果已经初始化完成则直接返回
        if (_m_bIsInitUSDone)
        {
            _m_lListener.send(NPGS2GCWriter_001_BasicOp.make_007_RetQueueInfo(_clientSerialize, -1));
            return;
        }

        // 发送消息到用户信息服务器对象注册连接服务器
        NPGateServer.getInstance().sendRequestToBSServer(EServerType.USER.ordinal(), _m_iUSId,
                NP2US_R_Writer_002_GSOp.make_006_ReqQueueInfo()
                , new _IWCGCallbackDealer()
                {

                    @Override
                    public void dealSuc(_IALProtocolStructure _msg)
                    {
                        NP2US_RB_002_006_RetQueueInfo retMsg = (NP2US_RB_002_006_RetQueueInfo) _msg;
                        if (null == retMsg)
                        {
                            _m_lListener.send(NPGS2GCWriter_001_BasicOp.make_007_RetQueueInfo(_clientSerialize, -1));
                            return;
                        }

                        //返回结果
                        _m_lListener.send(NPGS2GCWriter_001_BasicOp.make_007_RetQueueInfo(_clientSerialize, retMsg.getCurQueueIndex()));
                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                        _m_lListener.send(NPGS2GCWriter_001_BasicOp.make_007_RetQueueInfo(_clientSerialize, -1));
                    }

                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2US_RB_002_006_RetQueueInfo();
                    }
                });
    }

    /******************
     * 尝试退出当前队列
     * @param _commiter
     */
    public void reqQuitQueue(long _clientSerialize)
    {
        //如果已经初始化完成则直接报错
        if (_m_bIsInitUSDone)
        {
            _m_lListener.send(NPGS2GCWriter_001_BasicOp.make_008_QuitQueue(_clientSerialize, CommErr.DATA_STATE_ERR.getCode()));
            return;
        }

        // 发送消息到用户信息服务器对象注册连接服务器
        NPGateServer.getInstance().sendRequestToBSServer(EServerType.USER.ordinal(), _m_iUSId,
                NP2US_R_Writer_002_GSOp.make_007_ReqQuitQueue(_m_lInfoSerialize, _m_lQueueIndex)
                , new _IWCGCallbackDealer()
                {

                    @Override
                    public void dealSuc(_IALProtocolStructure _msg)
                    {
                        NP2US_RB_002_007_RetQuitQueueRes retMsg = (NP2US_RB_002_007_RetQuitQueueRes) _msg;
                        if (null == retMsg)
                        {
                            _m_lListener.send(NPGS2GCWriter_001_BasicOp.make_008_QuitQueue(_clientSerialize, CommErr.SYS_ERR.getCode()));
                            return;
                        }

                        //返回结果
                        _m_lListener.send(NPGS2GCWriter_001_BasicOp.make_008_QuitQueue(_clientSerialize, retMsg.getErrCode()));
                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                        _m_lListener.send(NPGS2GCWriter_001_BasicOp.make_008_QuitQueue(_clientSerialize, _errCode));
                    }

                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2US_RB_002_007_RetQuitQueueRes();
                    }
                });
    }


    /******************************
     * 在用户服务器相关操作初始化完成时的相关处理
     *
     * @param _newSerialize
     */
    public void onUSRegGSDone(long _infoSerialize, long _userSerialize)
    {
        //判断信息序列号是否匹配
        if (_infoSerialize != _m_lInfoSerialize)
            return;

        CommLog.sys("User GS reg suc!! Uid:{}, Cid:{}, serialize:{}", _m_lListener.getUid(), _m_lCid, _userSerialize);

        _m_lUserSerialize = _userSerialize;
        _m_lQueueIndex = 0;

        _m_bIsInitUSDone = true;

        //发送消息通知客户端进入成功
        _m_lListener.send(NPGS2GCWriter_001_BasicOp.make_004_OnUSEnterDone(_m_iUSId, _m_lCid, _m_lClientInitSerialize));
    }

    /******************************
     * 在用户服务器恢复连接流程完毕的相关操作
     *
     * @param _newSerialize
     */
    public void onUSResumeGCDone(long _infoSerialize, long _userSerialize)
    {
        //判断信息序列号是否匹配
        if (_infoSerialize != _m_lInfoSerialize)
            return;

        CommLog.sys("User GS reg suc!! Uid:{}, Cid:{}, serialize:{}", _m_lListener.getUid(), _m_lCid, _userSerialize);

        _m_lUserSerialize = _userSerialize;
        _m_lQueueIndex = 0;

        _m_bIsInitUSDone = true;

        //发送消息通知客户端进入成功
        _m_lListener.send(NPGS2GCWriter_001_BasicOp.make_009_ResumeUSConnectionRes(_m_lClientInitSerialize, Result.SUCC.getCode()));
    }

    /*******************
     * 转发消息到对应US的处理
     * @param _msg
     */
    public void resendToUS(ByteBuffer _msg)
    {
        // 判断用户服务器是否已注册连接
        if (!_m_bIsInitUSDone)
        {
            ALServerLog.Error("User Server Is not regged for cid: " + _m_lCid + " userServerId: " + _m_iUSId);
            return;
        }

        //转发数据
        NPGateServer.getInstance().sendMessageToBSServer(EServerType.USER.ordinal(), _m_iUSId,
                NP2US_Writer_001_BasicOp.make_001_ToUSUserMsg(_m_lCid, getUserSerialize(), _msg));
    }

    public void resendToUS(long _clientRequestSerialize, byte[] _msg)
    {
        // 判断用户服务器是否已注册连接
        if (!_m_bIsInitUSDone)
        {
            ALServerLog.Error("User Server Is not regged for cid: " + _m_lCid + " userServerId: " + _m_iUSId);
            return;
        }

        //转发数据
        NPGateServer.getInstance().sendMessageToBSServer(EServerType.USER.ordinal(), _m_iUSId,
                NP2US_Writer_001_BasicOp.make_001_ToUSUserMsg(_m_lCid, getUserSerialize(), _clientRequestSerialize, _msg));
    }

    /******************
     * 注销本US信息
     */
    public void unregUSInfo()
    {
        //如果队列索引非0表示有在队列，需要取消队列
        if (_m_lQueueIndex > 0)
        {
            NPGateServer.getInstance().sendRequestToBSServer(EServerType.USER.ordinal(), _m_iUSId,
                    NP2US_R_Writer_002_GSOp.make_007_ReqQuitQueue(_m_lInfoSerialize, _m_lQueueIndex));
        }

        // 发送消息注销连接服务器对象
        NPGateServer.getInstance().sendRequestToBSServer(EServerType.USER.ordinal(), _m_iUSId,
                NP2US_R_Writer_002_GSOp.make_002_UnregUserGate(_m_lListener.getSessionId(), _m_lCid, getUserSerialize()));

        //修改序列号
        _m_lInfoSerialize = ALSerializeMaker.makeNewSerialize();
        //重置状态
        _m_bIsStartInit = false;
        _m_bIsInitUSDone = false;
    }
}
