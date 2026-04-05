package NPCrossGameServer.NPCrossGameCore.MsgMgr.Commiter;

import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import NPCrossGameServer.NPCrossGameCore._ACrossGameInstance;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

/*************
 * 地图中的交互对象的请求处理对象
 * @author mj
 *
 */
public class CrossGameInstanceCommiter extends _ACrossGameRequestCommiter
{
    //游戏实例对象
    private _ACrossGameInstance _m_ciCrossGameInstance;
    //玩家CID 0-非玩家操作
    private long _m_lCid;

    /**
     * 不带commiter
     * @param _instance
     * @param _cid
     */
    public CrossGameInstanceCommiter(_ACrossGameInstance _instance, long _cid)
    {
        _m_ciCrossGameInstance = _instance;
        _m_lCid = _cid;
    }

    /**
     * 需要commiter，需要回调处理
     * @param _instance
     * @param _cid
     * @param _commiter
     */
    public CrossGameInstanceCommiter(_ACrossGameInstance _instance, long _cid, _IWCGBasicRequestCommiter _commiter)
    {
        super(_commiter);

        _m_ciCrossGameInstance = _instance;
        _m_lCid = _cid;
    }

    public _ACrossGameInstance getGameInstance()
    {
        return _m_ciCrossGameInstance;
    }

    public long getCid()
    {
        return _m_lCid;
    }

    @Override
    public _IALProtocolReceiver getRequestDealer()
    {
        return _m_ciCrossGameInstance;
    }
}
