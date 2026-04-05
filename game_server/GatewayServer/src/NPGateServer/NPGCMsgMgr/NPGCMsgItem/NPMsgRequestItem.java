package NPGateServer.NPGCMsgMgr.NPGCMsgItem;

import ALBasicProtocolPack._IALProtocolStructure;
import NPGateServer.NPGCListener.Writer.NPGS2GCWriter_001_BasicOp;

import java.nio.ByteBuffer;

/*******************
 * 常规的消息对象
 * @author mj
 *
 */
public class NPMsgRequestItem extends _ANPGSMsgItem
{
    //客户端请求相关信息
    private long _m_lClientRequestSerialize;

    //客户端请求处理结果信息
    private boolean _m_bRes;
    private int _m_iErrCode;

    //消息内容
    private byte[] _m_arrMsgBytes;

    public NPMsgRequestItem(long _clientRequestSerialize, boolean _res, int _errCode, byte[] _msgBytes)
    {
        _m_lClientRequestSerialize = _clientRequestSerialize;

        _m_bRes = _res;
        _m_iErrCode = _errCode;

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
        return _m_arrMsgBytes == null ? 0 : _m_arrMsgBytes.length;
    }


    @Override
    public _IALProtocolStructure makeBackProtocol()
    {
        return NPGS2GCWriter_001_BasicOp.make_023_RetClientRequest(getMsgSendSerialize(), _m_lClientRequestSerialize, _m_bRes, _m_iErrCode, _m_arrMsgBytes);
    }

}
