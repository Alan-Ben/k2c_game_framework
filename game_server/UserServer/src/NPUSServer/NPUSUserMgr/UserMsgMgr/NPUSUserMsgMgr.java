package NPUSServer.NPUSUserMgr.UserMsgMgr;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.SynTask.NPSynUserDealMsgTask;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem.NPUSUserNormalMsgItem;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem.NPUSUserRequestMsgItem;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._INPUSUserMsgItem;

import java.nio.ByteBuffer;
import java.util.LinkedList;
import java.util.concurrent.locks.ReentrantLock;

public class NPUSUserMsgMgr
{
    /**
     * 用户数据对象
     */
    private NPUSUserData _m_udUserData;
    /**
     * 缓存请求的队列
     */
    private LinkedList<_INPUSUserMsgItem> _m_lMsgList;
    private ReentrantLock _m_mutex;

    public NPUSUserMsgMgr(NPUSUserData _userData)
    {
        _m_udUserData = _userData;

        _m_lMsgList = new LinkedList<_INPUSUserMsgItem>();
        _m_mutex = new ReentrantLock();
    }

    public NPUSUserData getUserData()
    {
        return _m_udUserData;
    }

    /********************
     * 添加一个需要处理的消息
     *
     * @author alzq.z
     * @time Mar 4, 2013 10:19:30 PM
     */
    public void addMessage(long _clientRequestSerialize, ByteBuffer _msg)
    {
        //消息无效或角色无加载完成则直接返回
        if (null == _msg)
            return;

        boolean needStartTask = false;

        _lock();

        try
        {
            if (_m_lMsgList.isEmpty())
                needStartTask = true;

            //根据是否有客户端序列号创建不同的处理类，并放入队列
            if (_clientRequestSerialize == 0)
            {
                //加入直接处理的消息
                _m_lMsgList.addLast(new NPUSUserNormalMsgItem(_m_udUserData, _msg));
            } else
            {
                //加入回调处理的消息
                _m_lMsgList.addLast(new NPUSUserRequestMsgItem(_m_udUserData, _clientRequestSerialize, _msg));
            }
        } finally
        {
            _unlock();
        }

        if (needStartTask)
            ALSynTaskManager.getInstance().regTask(new NPSynUserDealMsgTask(this));
    }

    /******************
     * 自行从消息队列中取出一个消息进行处理
     * 返回是否需要继续处理消息
     *
     * @author alzq.z
     * @time Mar 4, 2013 10:24:27 PM
     */
    public boolean dealMessage()
    {
        _INPUSUserMsgItem msgItem = null;

        //取出第一个消息
        _lock();

        try
        {
            if (!_m_lMsgList.isEmpty())
                msgItem = _m_lMsgList.getFirst();
        } finally
        {
            _unlock();
        }

        if (null != msgItem)
        {
            //处理消息
            try
            {
                //调用处理函数
                msgItem.dealMsg();
            } catch (Exception ex)
            {
                ex.printStackTrace();
            }
        }

        _lock();

        try
        {
            _m_lMsgList.pop();

            //判断消息队列是否为空
            return !_m_lMsgList.isEmpty();
        } finally
        {
            _unlock();
        }
    }

    protected void _lock()
    {
        _m_mutex.lock();
    }

    protected void _unlock()
    {
        _m_mutex.unlock();
    }
}
