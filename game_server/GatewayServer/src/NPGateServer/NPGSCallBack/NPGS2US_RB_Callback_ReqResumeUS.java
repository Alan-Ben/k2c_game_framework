package NPGateServer.NPGSCallBack;

import ALBasicProtocolPack._IALProtocolStructure;
import ALServerLog.ALServerLog;
import NP2US_RB.p002_GSOp.NP2US_RB_002_004_RetResumeUSInfo;
import NPCommon.ErrMain.CommErr;
import NPGateServer.NPGCListener.NPGSGCListener;
import NPGateServer.NPGCListener.NPGSGCUSInfo;
import NPGateServer.NPGCListener.Writer.NPGS2GCWriter_001_BasicOp;
import NPGateServer.NPGateServer;
import NPServerProtocolWriter.NP2RCS.Request.NP2RCS_R_Writer_001_BasicOp;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;

/****************
 * 请求恢复US连接的处理
 * @author Administrator
 *
 */
public class NPGS2US_RB_Callback_ReqResumeUS implements _IWCGCallbackDealer
{
    private NPGSGCListener _m_glGCListener;

    //服务器Id
    private int _m_iUSId;
    //信息序列号，用于区分数据是否有效
    private long _m_lInfoSerialize;
    //客户端初始化操作序列号
    private long _m_lClientInitSerialize;

    public NPGS2US_RB_Callback_ReqResumeUS(NPGSGCUSInfo _usInfo, long _clientInitSerialize)
    {
        _m_glGCListener = _usInfo.getListener();

        _m_iUSId = _usInfo.getUSId();
        _m_lInfoSerialize = _usInfo.getInfoSerialize();
        _m_lClientInitSerialize = _clientInitSerialize;
    }

    @Override
    public _IALProtocolStructure createProtocolObj()
    {
        return new NP2US_RB_002_004_RetResumeUSInfo();
    }

    @Override
    public void dealFail(int _errCode)
    {
        //输出日志
        ALServerLog.Sys("Request Enter US uid: " + _m_glGCListener.getUid() + " UserServerId: " + _m_iUSId + " Error! - " + _errCode);

        //此时直接断开连接
        _m_glGCListener.unregUSInfo(_m_lInfoSerialize);
        _m_glGCListener.send(NPGS2GCWriter_001_BasicOp.make_009_ResumeUSConnectionRes(_m_lClientInitSerialize, _errCode));
    }

    @Override
    public void dealSuc(_IALProtocolStructure _protocol)
    {
        NP2US_RB_002_004_RetResumeUSInfo protocolObj = (NP2US_RB_002_004_RetResumeUSInfo) _protocol;
        if (null == protocolObj)
        {
            //此时直接断开连接
            _m_glGCListener.unregUSInfo(_m_lInfoSerialize);
            _m_glGCListener.send(NPGS2GCWriter_001_BasicOp.make_009_ResumeUSConnectionRes(_m_lClientInitSerialize, CommErr.OBJ_ERR.getCode()));
            return;
        }

        //调用初始化完成处理
        _m_glGCListener.getUSInfo().onGCUSResumeComfirmed(_m_lInfoSerialize, protocolObj.getCid());
        //更新登录记录到RecordServer
        NPGateServer.getInstance().sendRequestToBSServer(EServerType.SINGLE.ordinal(), ENPSingleServerType.RECORD.ordinal()
                , NP2RCS_R_Writer_001_BasicOp.make_001_001_ReqUpdateLoginRecord(protocolObj.getCid(), _m_glGCListener.getUid(),
                        _m_glGCListener.getUSInfo().getUSLogicId(), _m_glGCListener.getUSInfo().getUSId()));
    }

}
