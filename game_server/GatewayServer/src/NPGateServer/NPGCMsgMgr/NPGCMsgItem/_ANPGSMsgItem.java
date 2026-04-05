package NPGateServer.NPGCMsgMgr.NPGCMsgItem;

import ALBasicProtocolPack._IALProtocolStructure;

import java.nio.ByteBuffer;

/******************
 * 发送回客户端的基本消息接口声明对象
 * @author mj
 *
 */
public abstract class _ANPGSMsgItem
{
    private int _m_iMsgSendSerialize;

    public _ANPGSMsgItem()
    {
        _m_iMsgSendSerialize = 0;
    }

    /***************
     * 设置消息发送序列号，需要用此数据进行返回消息的拼凑
     * @param _serialize
     */
    public int getMsgSendSerialize()
    {
        return _m_iMsgSendSerialize;
    }

    public void setMsgSendSerialize(int _serialize)
    {
        _m_iMsgSendSerialize = _serialize;
    }

    //获取返回协议编号
    public abstract byte getMainOrder();

    public abstract byte getSubOrder();

    /****************
     * 获取消息的实际内容，在消息无法缓存验证的时候，会通过此接口直接发送给客户端
     * @return
     */
    public abstract ByteBuffer getMsgRealBytes();

    /********
     * 获取本消息尺寸
     * @return
     */
    public abstract int getFullSize();

    /*****************
     * 获取返回的消息包
     * @return
     */
    public abstract _IALProtocolStructure makeBackProtocol();
}
