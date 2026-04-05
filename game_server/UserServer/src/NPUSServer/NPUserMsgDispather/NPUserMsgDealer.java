package NPUSServer.NPUserMsgDispather;

import ALBasicProtocolPack._IALProtocolStructure;
import NPCommon.Dispather._IAutoRegistMsgHandler;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserServer;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys.CustomCommiter._ATWCGBasicRequestAutoSubOrderDealer_CustomCommiter;

/***************
 * 通用的Sub Order dealer
 * @author mj
 *
 * @param <T>
 */
public abstract class NPUserMsgDealer<T extends _IALProtocolStructure>
        extends _ATWCGBasicRequestAutoSubOrderDealer_CustomCommiter<_ANPUSUserBasicMsgItem, T>
        implements _IAutoRegistMsgHandler
{
    private NPUserServer _m_usUSServer = null;

    /**
     * 初始化设置服务器对象
     * @param _usServer
     */
    public void _initUSServer(NPUserServer _usServer) {_m_usUSServer = _usServer;}


    public NPUserServer getUSServer() {return _m_usUSServer;}
}
