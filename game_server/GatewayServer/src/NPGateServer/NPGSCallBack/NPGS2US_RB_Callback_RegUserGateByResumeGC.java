package NPGateServer.NPGSCallBack;

import ALBasicProtocolPack._IALProtocolStructure;
import ALServerLog.ALServerLog;
import NP2US_RB.p002_GSOp.NP2US_RB_002_001_RegUserGate;
import NPCommon.ErrMain.CommErr;
import NPGateServer.NPGCListener.NPGSGCListener;
import NPGateServer.NPGCListener.NPGSGCUSInfo;
import NPGateServer.NPGCListener.Writer.NPGS2GCWriter_001_BasicOp;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;

/****************
 * 请求进入key的返回回调处理对象
 * @author Administrator
 *
 */
public class NPGS2US_RB_Callback_RegUserGateByResumeGC implements _IWCGCallbackDealer
{
    private NPGSGCListener _m_glGCListener;

    //服务器Id
    private int _m_iUSId;
    //信息序列号，用于区分数据是否有效
    private long _m_lInfoSerialize;
    //客户端初始化操作序列号
    private long _m_lClientInitSerialize;

    public NPGS2US_RB_Callback_RegUserGateByResumeGC(NPGSGCUSInfo _usInfo)
    {
        _m_glGCListener = _usInfo.getListener();

        _m_iUSId = _usInfo.getUSId();
        _m_lInfoSerialize = _usInfo.getInfoSerialize();
        _m_lClientInitSerialize = _usInfo.getClientInitSerialize();
    }

    @Override
    public _IALProtocolStructure createProtocolObj()
    {
        return new NP2US_RB_002_001_RegUserGate();
    }

    @Override
    public void dealFail(int _errCode)
    {
        //输出日志
        ALServerLog.Sys("Reg User Gate Server for uid: " + _m_glGCListener.getUid() + " UserServerId: " + _m_iUSId + " Error! - " + _errCode);

        //返回失败
        _m_glGCListener.send(NPGS2GCWriter_001_BasicOp.make_009_ResumeUSConnectionRes(_m_lClientInitSerialize, _errCode));
    }

    @Override
    public void dealSuc(_IALProtocolStructure _protocol)
    {
        NP2US_RB_002_001_RegUserGate protocolObj = (NP2US_RB_002_001_RegUserGate) _protocol;
        if (null == protocolObj || !protocolObj.getRes())
        {
            ALServerLog.Sys("Reg User Gate Server for uid: " + _m_glGCListener.getUid() + " UserServerId: " + _m_iUSId + " Error!");

            //返回失败
            _m_glGCListener.send(NPGS2GCWriter_001_BasicOp.make_004_OnUSEnterDone(CommErr.SYS_ERR.getCode(), _m_iUSId, _m_lClientInitSerialize));
            return;
        }

        //设置用户对应服务器已连接
        _m_glGCListener.onUSResumeGSDone(_m_lInfoSerialize, protocolObj.getNewSerialize());
    }

}
