package NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem;

import ALBasicProtocolPack._IALProtocolStructure;
import NPUSServer.NPUSUserMgr.NPUSUserData;

import java.nio.ByteBuffer;

/****************
 * 用户的请求式消息对象
 * @author mj
 *
 */
public class NPUSUserRequestMsgItem extends _ANPUSUserBasicMsgItem
{
    //用户客户端的消息请求序列号
    private long _m_lClientRequestSerialize;

    public NPUSUserRequestMsgItem(NPUSUserData _userData, long _requestSerialize, ByteBuffer _msgBuffer)
    {
        super(_userData, _msgBuffer);

        _m_lClientRequestSerialize = _requestSerialize;
    }

    @Override
    public void commitFailRes(int _errCode)
    {
        //返回回调处理失败
        if (null == getUserData())
            return;

        getUserData().sendBackRequestFailToGC(_m_lClientRequestSerialize, _errCode);
    }

    @Override
    public void commitSucRes(_IALProtocolStructure _retMsg)
    {
        //直接发送消息返回
        if (null == getUserData())
            return;

        getUserData().sendBackRequestResToGC(_m_lClientRequestSerialize, _retMsg);
    }

    /**************
     * 消息通过转发的方式，希望直接返回给客户端的时候通过本函数处理
     * @param _retMsg
     */
    @Override
    public void commitSucResByBuffer(ByteBuffer _retMsg)
    {
        //直接发送消息返回
        if (null == getUserData() || null == _retMsg)
            return;

        getUserData().sendBackRequestResToGC(_m_lClientRequestSerialize, _retMsg);
    }
}
