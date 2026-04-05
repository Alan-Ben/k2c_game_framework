package NPCrossGameServer.NPCrossGameCore.RequestDispather;

import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import ALBasicProtocolPack._IALProtocolStructure;
import NPCrossGameServer.NPCrossGameCore.MsgMgr.Commiter.CrossGameInstanceCommiter;
import NPCrossGameServer.NPCrossGameCore.RequestDispather.Writer.NPCrossGame_RB_Writer_001_BasicOp;
import NPCrossGameServer.NPCrossGameCore._ACrossGameInstance;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

/*************
 * 地图中的交互对象的请求处理对象
 * @author mj
 *
 */
public class NPCrossGameGCCommiter implements _IWCGBasicRequestCommiter
{
    private CrossGameInstanceCommiter _m_ccCrossGameCommiter;

    public NPCrossGameGCCommiter(CrossGameInstanceCommiter _commiter)
    {
        _m_ccCrossGameCommiter = _commiter;
    }

    public long getCid()
    {
        return _m_ccCrossGameCommiter.getCid();
    }

    public _ACrossGameInstance getGameInstance()
    {
        return _m_ccCrossGameCommiter.getGameInstance();
    }

    @Override
    public void commitFailRes(int _errCode)
    {
        if (null == _m_ccCrossGameCommiter)
            return;

        _m_ccCrossGameCommiter.commitFailRes(_errCode);
    }

    @Override
    public void commitSucRes(_IALProtocolStructure _retMsg)
    {
        if (null == _m_ccCrossGameCommiter)
            return;

        _m_ccCrossGameCommiter.commitSucRes(NPCrossGame_RB_Writer_001_BasicOp.make_001_RetGCForwardMsg(_retMsg));
    }

    @Override
    public _IALProtocolReceiver getRequestDealer()
    {
        return _m_ccCrossGameCommiter.getRequestDealer();
    }
}
