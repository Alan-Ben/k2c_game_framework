package NPCrossGameServer.NPCrossGameCore.MsgMgr.MsgItem;

import NPCrossGameServer.NPCrossGameCore._ACrossGameInstanceCategory;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

import java.nio.ByteBuffer;

/*********************
 * 空间的消息对象，会根据实际的消息情况进行不同的处理
 *
 * 空间消息会分为Item处理的以及Space处理的
 * Item需要有对应的ItemSerialize信息
 * @author mj
 *
 */
public abstract class _ACrossGameCategoryMsgItem
{
    protected long _m_lInstaceId;
    //对应玩家CID 0-非玩家操作
    protected long _m_lCid;
    //消息对象
    protected ByteBuffer _m_bMsgBuffer;
    //消息的实际提交对象
    protected _IWCGBasicRequestCommiter _m_cMsgCommiter;

    public _ACrossGameCategoryMsgItem(long _instanceId, long _cid, ByteBuffer _msgBuffer, _IWCGBasicRequestCommiter _commiter)
    {
        _m_lInstaceId = _instanceId;
        _m_lCid = _cid;
        _m_bMsgBuffer = _msgBuffer;
        _m_cMsgCommiter = _commiter;
    }

    /*************
     * 直接失败的处理
     */
    public void dealFail(int _errCode)
    {
        if (null == _m_cMsgCommiter)
            return;

        _m_cMsgCommiter.commitFailRes(_errCode);
    }

    /**************
     * 根据不同子类进行不同的消息处理
     */
    @SuppressWarnings("rawtypes")
    public abstract void dealMsg(_ACrossGameInstanceCategory _crossGameInstance);
}
