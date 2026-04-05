package PayCenter.PayServer.GeneralListener.RequestDispather;

import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import ALBasicProtocolPack._IALProtocolStructure;
import PayCenter.PayServer.PayServer;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

public class PayServerRequestCommitter implements _IWCGBasicRequestCommiter
{
    private PayServer _m_payServer;
    private _IWCGBasicRequestCommiter _m_commiter;

    public PayServerRequestCommitter(PayServer _payServer, _IWCGBasicRequestCommiter _commiter)
    {
        _m_payServer = _payServer;
        _m_commiter = _commiter;
    }

    public PayServer getPayServer()
    {
        return _m_payServer;
    }

    @Override
    public _IALProtocolReceiver getRequestDealer()
    {
        return _m_commiter.getRequestDealer();
    }

    @Override
    public void commitSucRes(_IALProtocolStructure _proto)
    {
        _m_commiter.commitSucRes(_proto);
    }

    @Override
    public void commitFailRes(int _errCode)
    {
        _m_commiter.commitFailRes(_errCode);
    }
}
