package NPUSServer.CrossGameSys.CallbackDealer;

import ALBasicProtocolPack._IALProtocolStructure;
import CrossGame_RB.p001_BasicOp.CrossGame_RB_001_001_RetGCForwardMsg;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;

/*****************
 * 玩家消息转发到Space的时候进行的回调处理
 * 此回调会直接将返回消息转发给玩家
 * @author mj
 *
 */
public class CrossGameCallbackDealer_GCForwardCallback implements _IWCGCallbackDealer
{
    //回调处理
    private _ICallBack_GCMsgForwardHandle _m_cdCallbackDealer;
    //消息处理对象
    private _ANPUSUserBasicMsgItem _m_msgItem;

    public CrossGameCallbackDealer_GCForwardCallback(_ICallBack_GCMsgForwardHandle _callbackDealer, _ANPUSUserBasicMsgItem _msgItem)
    {
        _m_cdCallbackDealer = _callbackDealer;

        _m_msgItem = _msgItem;
    }

    public CrossGameCallbackDealer_GCForwardCallback(_ANPUSUserBasicMsgItem _msgItem)
    {
        _m_msgItem = _msgItem;
    }

    @Override
    public _IALProtocolStructure createProtocolObj()
    {
        return new CrossGame_RB_001_001_RetGCForwardMsg();
    }

    @Override
    public void dealFail(int _errCode)
    {
        //处理错误回调
        if (null != _m_cdCallbackDealer)
        {
            _m_cdCallbackDealer.onRunOver(false);
        }

        //返回消息
        if (null != _m_msgItem)
        {
            _m_msgItem.commitFailRes(_errCode);
        }
    }

    @Override
    public void dealSuc(_IALProtocolStructure _msg)
    {
        CrossGame_RB_001_001_RetGCForwardMsg retMsg = (CrossGame_RB_001_001_RetGCForwardMsg) _msg;

        //处理成功回调
        if (null != _m_cdCallbackDealer)
        {
            _m_cdCallbackDealer.onRunOver(true);
        }

        //返回消息
        if (null != _m_msgItem)
        {
            _m_msgItem.commitSucResByBuffer(retMsg.get_buffer_Msg());
        }
    }
}
