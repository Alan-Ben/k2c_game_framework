package NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem;

import ALBasicProtocolPack._IALProtocolStructure;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_007_CommOp;

import java.nio.ByteBuffer;

/****************
 * 日常的消息处理
 * @author mj
 *
 */
public class NPUSUserNormalMsgItem extends _ANPUSUserBasicMsgItem
{
    public NPUSUserNormalMsgItem(NPUSUserData _userData, ByteBuffer _msgBuffer)
    {
        super(_userData, _msgBuffer);
    }

    @Override
    public void commitFailRes(int _errCode)
    {
        //返回回调处理失败
        if (null == getUserData())
            return;

        getUserData().sendMsgToGC(US2GCWriter_007_CommOp.make_051_OnCommError(_errCode));
    }

    @Override
    public void commitSucRes(_IALProtocolStructure _retMsg)
    {
        //直接发送消息返回
        if (null == getUserData() || null == _retMsg)
            return;

        getUserData().sendMsgToGC(_retMsg);
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

        getUserData().sendMsgToGC(_retMsg);
    }
}
