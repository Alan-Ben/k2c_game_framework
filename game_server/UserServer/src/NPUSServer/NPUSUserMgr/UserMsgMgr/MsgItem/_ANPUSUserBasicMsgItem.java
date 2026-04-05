package NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem;

import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

import java.nio.ByteBuffer;

/****************
 * 日常的消息处理
 * @author mj
 *
 */
public abstract class _ANPUSUserBasicMsgItem implements _INPUSUserMsgItem, _IWCGBasicRequestCommiter
{
    //用户数据对象
    private NPUSUserData _m_udUserData;
    //消息内容
    private ByteBuffer _m_bMsgBuffer;

    public _ANPUSUserBasicMsgItem(NPUSUserData _userData, ByteBuffer _msgBuffer)
    {
        _m_udUserData = _userData;
        _m_bMsgBuffer = _msgBuffer;
    }

    public NPUSUserData getUserData()
    {
        return _m_udUserData;
    }

    /*************
     * 处理本消息对象的消息
     */
    public void dealMsg()
    {
        if (null == _m_udUserData)
            return;

        _m_udUserData.dealMsg(this, _m_bMsgBuffer);
    }

    @Override
    public _IALProtocolReceiver getRequestDealer()
    {
        return _m_udUserData;
    }

    /**************
     * 消息通过转发的方式，希望直接返回给客户端的时候通过本函数处理
     * @param _retMsg
     */
    public abstract void commitSucResByBuffer(ByteBuffer _retMsg);
}
