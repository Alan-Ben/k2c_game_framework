package NPGateServer.NPGSCallBack;

import ALBasicProtocolPack._IALProtocolStructure;
import ALServerLog.ALServerLog;
import NP2US_RB.p002_GSOp.NP2US_RB_002_005_RetEnterUSInfo;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPGateServer.NPGCListener.NPGSGCListener;
import NPGateServer.NPGCListener.NPGSGCUSInfo;
import NPGateServer.NPGCListener.Writer.NPGS2GCWriter_001_BasicOp;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;

/****************
 * 请求进入key的返回回调处理对象
 * @author Administrator
 *
 */
public class NPGS2US_RB_Callback_ReqEnterUS implements _IWCGCallbackDealer
{
    private NPGSGCListener _m_glGCListener;

    //服务器Id
    private int _m_iUSId;
    //信息序列号，用于区分数据是否有效
    private long _m_lInfoSerialize;
    //客户端初始化操作序列号
    private long _m_lClientInitSerialize;

    public NPGS2US_RB_Callback_ReqEnterUS(NPGSGCUSInfo _usInfo, long _clientInitSerialize)
    {
        _m_glGCListener = _usInfo.getListener();

        _m_iUSId = _usInfo.getUSId();
        _m_lInfoSerialize = _usInfo.getInfoSerialize();
        _m_lClientInitSerialize = _clientInitSerialize;
    }

    @Override
    public _IALProtocolStructure createProtocolObj()
    {
        return new NP2US_RB_002_005_RetEnterUSInfo();
    }

    @Override
    public void dealFail(int _errCode)
    {
        //输出日志
        ALServerLog.Sys("Request Enter US uid: " + _m_glGCListener.getUid() + " UserServerId: " + _m_iUSId + " Error! - " + _errCode);

        //此时直接断开连接
        _m_glGCListener.unregUSInfo(_m_lInfoSerialize);
        _m_glGCListener.send(NPGS2GCWriter_001_BasicOp.make_005_EnterUSRes(_errCode, _m_lClientInitSerialize));
    }

    @Override
    public void dealSuc(_IALProtocolStructure _protocol)
    {
        NP2US_RB_002_005_RetEnterUSInfo protocolObj = (NP2US_RB_002_005_RetEnterUSInfo) _protocol;
        if (null == protocolObj)
        {
            //输出日志
            ALServerLog.Sys("Request Enter US uid: " + _m_glGCListener.getUid() + " UserServerId: " + _m_iUSId + " Fail!");

            //此时直接断开连接
            _m_glGCListener.unregUSInfo(_m_lInfoSerialize);
            _m_glGCListener.send(NPGS2GCWriter_001_BasicOp.make_005_EnterUSRes(CommErr.SYS_ERR.getCode(), _m_lClientInitSerialize));
            return;
        }

        //设置队列信息
        if (null != _m_glGCListener.getUSInfo())
            _m_glGCListener.getUSInfo().setQueueIndex(_m_lInfoSerialize, protocolObj.getQueueIndex());

        //发送消息通知客户端进入成功
        _m_glGCListener.send(NPGS2GCWriter_001_BasicOp.make_005_EnterUSRes(Result.SUCC.getCode(), _m_lClientInitSerialize, protocolObj.getQueueIndex()));
    }

}
