package NPCommonServer.NPCSGeneralListener.RequestDispather.CallBack;

import ALBasicProtocolPack._IALProtocolStructure;
import ALServerLog.ALServerLog;
import NPCommonServer.NPCommonServer;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCS2US_RB.p003_MailOp.WCGCS2US_RB_003_003_RetSendPersonalMailByMID;
import WCGCommon.Enum.NPEnum.EServerType;

/******************************
 * Common服向User服同步邮件的回调，失败了重试3次。
 *
 * @author jeb
 *
 */
public class NP2US_RB_Callback_RetSendPersonalMailByMID implements _IWCGCallbackDealer
{
    private int _tryTimes = 0;

    private int _m_serverId;
    private _IALProtocolStructure _m_proto;

    public NP2US_RB_Callback_RetSendPersonalMailByMID(int _serverId, _IALProtocolStructure _proto)
    {
        _m_serverId = _serverId;
        _m_proto = _proto;
    }

    @Override
    public _IALProtocolStructure createProtocolObj()
    {
        return new WCGCS2US_RB_003_003_RetSendPersonalMailByMID();
    }

    @Override
    public void dealFail(int _errCode)
    {
        if (this._tryTimes > 2)
        {
            return;
        }
        NPCommonServer.getInstance().sendRequestToBSServer(EServerType.USER.ordinal(), _m_serverId, _m_proto, this);
        this._tryTimes++;

        ALServerLog.Sys("sync to userServer: " + _m_serverId + " fail, msg:" + _m_proto.getClass().getName() + " error: " + _errCode);
    }

    @Override
    public void dealSuc(_IALProtocolStructure _msg)
    {
        ALServerLog.Sys("sync to userServer: " + _m_serverId + " success, msg:" + _m_proto.getClass().getName());
    }
}
