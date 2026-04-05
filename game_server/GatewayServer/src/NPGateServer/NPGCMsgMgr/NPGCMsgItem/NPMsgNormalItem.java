package NPGateServer.NPGCMsgMgr.NPGCMsgItem;

import ALBasicProtocolPack._IALProtocolStructure;
import NPGateServer.NPGCListener.Writer.NPGS2GCWriter_001_BasicOp;

import java.nio.ByteBuffer;

/*******************
 * 常规的消息对象
 * @author mj
 *
 */
public class NPMsgNormalItem extends _ANPGSMsgItem
{
    //消息内容
    private byte[] _m_arrMsgBytes;

    public NPMsgNormalItem(byte[] _msgBytes)
    {
        _m_arrMsgBytes = _msgBytes;
    }

    //获取返回协议编号
    @Override
    public byte getMainOrder()
    {
        return _m_arrMsgBytes[0];
    }

    @Override
    public byte getSubOrder()
    {
        return _m_arrMsgBytes[1];
    }

    /****************
     * 获取消息的实际内容，在消息无法缓存验证的时候，会通过此接口直接发送给客户端
     * @return
     */
    @Override
    public ByteBuffer getMsgRealBytes()
    {
        return ByteBuffer.wrap(_m_arrMsgBytes);
    }

    /********
     * 获取本消息尺寸
     * @return
     */
    @Override
    public int getFullSize()
    {
        return _m_arrMsgBytes.length;
    }


    @Override
    public _IALProtocolStructure makeBackProtocol()
    {
        return NPGS2GCWriter_001_BasicOp.make_022_SendSerializeMsg(getMsgSendSerialize(), _m_arrMsgBytes);
    }

}
