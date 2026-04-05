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
public class CrossGameCategoryRequestItem extends _ACrossGameCategoryMsgItem
{
    public CrossGameCategoryRequestItem(long _instanceId, long _cid, ByteBuffer _msgBuffer, _IWCGBasicRequestCommiter _commiter)
    {
        super(_instanceId, _cid, _msgBuffer, _commiter);
    }

    /**************
     * 根据不同子类进行不同的消息处理
     */
    @SuppressWarnings("rawtypes")
    @Override
    public void dealMsg(_ACrossGameInstanceCategory _crossGameCategory)
    {
        if (null == _crossGameCategory)
            return;

        //处理协议
        _crossGameCategory.dealRequest(_m_cMsgCommiter, _m_lInstaceId, _m_lCid, _m_bMsgBuffer);
    }
}
