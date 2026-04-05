package NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem;

import ALBasicProtocolPack._IALProtocolStructure;
import NPCommon.ErrMain.CommErr;
import NPCommon.Log.CommLog;

import java.nio.ByteBuffer;

/***
 * 用户转发消息时，如果针对返回的消息需要做二次处理的，通过本对象进行处理
 */
public abstract class _ATNPUserMsgRedirectCommiter<T extends  _IALProtocolStructure> extends _ANPUSUserBasicMsgItem
{
    //基础转发的消息对象
    private _ANPUSUserBasicMsgItem _m_miBasicMsgItem;

    public _ATNPUserMsgRedirectCommiter(_ANPUSUserBasicMsgItem _basicMsgItem)
    {
        super(_basicMsgItem.getUserData(), null);

        _m_miBasicMsgItem = _basicMsgItem;
    }

    public _ANPUSUserBasicMsgItem getBasicMsgItem() {return _m_miBasicMsgItem;}

    /**************
     * 消息通过转发的方式，希望直接返回给客户端的时候通过本函数处理
     * @param _retMsg
     */
    @Override
    public void commitSucResByBuffer(ByteBuffer _retMsg)
    {
        T readObj = _createNewTmpObj();
        //读取数据，由于转发都是full所以需要提前读取2个字节
        _retMsg.get();
        _retMsg.get();
        readObj.readPackage(_retMsg);

        //进行消息处理
        _dealTmpCommitMsg(_m_miBasicMsgItem, readObj);
    }

    @Override
    public void commitSucRes(_IALProtocolStructure _msg)
    {
        if(null == _msg)
        {
            CommLog.error("_ATNPUserMsgRedirectCommiter commitSucRes error, msg type is not correct, msg class:" + _msg.getClass().getName());
            commitFailRes(CommErr.OBJ_ERR.getCode());
            return ;
        }

        _dealTmpCommitMsg(_m_miBasicMsgItem, (T)_msg);
    }

    @Override
    public void commitFailRes(int _errCode)
    {
        if(null != _m_miBasicMsgItem)
            _m_miBasicMsgItem.commitFailRes(_errCode);
    }

    /**
     * 实际的最终提交处理
     * @param _retMsg
     */
    public void finalCommitSuc(_IALProtocolStructure _retMsg)
    {
        if(null != _m_miBasicMsgItem)
            _m_miBasicMsgItem.commitSucRes(_retMsg);
    }

    /**
     * 创建临时的消息结构体对象用于解析Bytebuffer，并用于实际处理
     * @return
     */
    protected abstract T _createNewTmpObj();

    /**
     * 处理提交到本处理对象的中间消息对象
     * @param _msg
     */
    protected abstract void _dealTmpCommitMsg(_ANPUSUserBasicMsgItem _commiter, T _msg);
}
