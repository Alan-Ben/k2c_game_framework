package NPHttpServer;

import ALBasicServer.ALSocket.ALBasicServerSocket;
import ALBasicServer.ALVerifyObj.ALVerifyDealerObj;
import ALBasicServer.ALVerifyObj._IALVerifyFun;
import NPHttpServer.HsClientListener.HsClientListener;

public class NPClientConnectVerifyObj implements _IALVerifyFun
{
    @Override
    public void verifyIdentity(ALVerifyDealerObj _verifyDealer, ALBasicServerSocket _socket, int _clientType, String _userName, String _userPassword, String _customMsg)
    {
        if (_socket.getIP().compareToIgnoreCase("127.0.0.1") != 0)
        {
            _verifyDealer.comfirmResult(null);
            return;
        }
        _verifyDealer.comfirmResult(new HsClientListener(), "success");
    }
}
