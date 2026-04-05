package NPPlatServer.NPPS_Listener.LSCallBack;

import ALBasicProtocolPack._IALProtocolStructure;
import NP2GS_RB.p001_PSOp.NP2GS_RB_001_001_RetUserKey;
import NPCommon.ErrMain.CommErr;
import NPPlatServer.NPGeneralListener.Writer.NP2PS_RB_Writer_003_LSOp;
import NPPlatServer.NPPS_Listener.PS_GSListener;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

/****************
 * 请求进入key的返回回调处理对象
 * @author Administrator
 *
 */
public class NPPS2GSRBDealerRequestReqUserKey implements _IWCGCallbackDealer
{
    private PS_GSListener _m_glGSListener;
    private _IWCGBasicRequestCommiter _m_cCommiter;

    public NPPS2GSRBDealerRequestReqUserKey(PS_GSListener _gsListener, _IWCGBasicRequestCommiter _commiter)
    {
        _m_glGSListener = _gsListener;
        _m_cCommiter = _commiter;
    }

    @Override
    public _IALProtocolStructure createProtocolObj()
    {
        return new NP2GS_RB_001_001_RetUserKey();
    }

    @Override
    public void dealFail(int _errCode)
    {
        //减少人数
        _m_glGSListener.reduceHandleUser();
        //进行失败处理
        _m_cCommiter.commitFailRes(_errCode);
    }

    @Override
    public void dealSuc(_IALProtocolStructure _protocol)
    {
        NP2GS_RB_001_001_RetUserKey realProtocol = (NP2GS_RB_001_001_RetUserKey) _protocol;
        if (null == realProtocol || !realProtocol.getRes())
        {
            _m_cCommiter.commitFailRes(CommErr.SYS_ERR.getCode());
            return;
        }

        //返回成功操作
        _m_cCommiter.commitSucRes(NP2PS_RB_Writer_003_LSOp.make_002_RetGateKey(
                realProtocol.getUid(), _m_glGSListener.getConnectIp(), _m_glGSListener.getConnectPort(), realProtocol.getCheckCode()));
    }

}
