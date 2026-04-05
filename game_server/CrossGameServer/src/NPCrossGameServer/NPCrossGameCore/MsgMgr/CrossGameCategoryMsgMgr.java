package NPCrossGameServer.NPCrossGameCore.MsgMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import NPCommon.ErrMain.CommErr;
import NPCrossGameServer.NPCrossGameCore.MsgMgr.MsgItem.CrossGameCategoryMsgItem;
import NPCrossGameServer.NPCrossGameCore.MsgMgr.MsgItem.CrossGameCategoryRequestItem;
import NPCrossGameServer.NPCrossGameCore.MsgMgr.MsgItem._ACrossGameCategoryMsgItem;
import NPCrossGameServer.NPCrossGameCore._ACrossGameInstanceCategory;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

import java.nio.ByteBuffer;
import java.util.LinkedList;

/*********************
 * 空间的消息管理对象，发送到单个空间的消息要求在一个任务链中处理
 * 空间没有过大的运算消耗
 * @author Alzq
 *
 */

@SuppressWarnings("rawtypes")
public class CrossGameCategoryMsgMgr
{
    //归属空间对象
    private _ACrossGameInstanceCategory _m_ccCrossGameCategory;
    //消息队列
    private LinkedList<_ACrossGameCategoryMsgItem> _m_lCrossGameMsgList;
    //锁对象
    private MutexAtom _m_mutex;

    public CrossGameCategoryMsgMgr(_ACrossGameInstanceCategory _crossGameCategory)
    {
        _m_ccCrossGameCategory = _crossGameCategory;

        _m_lCrossGameMsgList = new LinkedList<_ACrossGameCategoryMsgItem>();

        _m_mutex = new MutexAtom();
    }

    protected void _lock()
    {
        _m_mutex.lock();
    }

    protected void _unlock()
    {
        _m_mutex.unlock();
    }

    /********************
     * 协议处理对象
     * @param _protocol
     */
    public void addMsg(long _instanceId, long _cid, ByteBuffer _msgBuffer)
    {
        addMessage(new CrossGameCategoryMsgItem(_instanceId, _cid, _msgBuffer));
    }

    public void addRequest(long _instanceId, long _cid, ByteBuffer _msgBuffer, _IWCGBasicRequestCommiter _msgCommiter)
    {
        addMessage(new CrossGameCategoryRequestItem(_instanceId, _cid, _msgBuffer, _msgCommiter));
    }

    /*****************
     * 清空处理对象
     */
    public void dispose()
    {
        _lock();

        try
        {
            while (!_m_lCrossGameMsgList.isEmpty())
            {
                _ACrossGameCategoryMsgItem item = _m_lCrossGameMsgList.pop();
                if (null == item)
                    continue;

                //一律按照无空间处理
                item.dealFail(CommErr.SYS_BUSY.getCode());
            }
        } finally
        {
            _unlock();
        }
    }

    /********************
     * 添加一个需要处理的消息
     *
     * @author alzq.z
     * @time Mar 4, 2013 10:19:30 PM
     */
    public void addMessage(_ACrossGameCategoryMsgItem _msgItem)
    {
        //消息无效或角色无加载完成则直接返回
        if (null == _msgItem)
            return;

        _lock();

        try
        {
            _m_lCrossGameMsgList.addLast(_msgItem);
        } finally
        {
            _unlock();
        }
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
        _ACrossGameCategoryMsgItem msgItem = null;

        //取出第一个消息
        _lock();

        try
        {
            if (!_m_lCrossGameMsgList.isEmpty())
                msgItem = _m_lCrossGameMsgList.getFirst();
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
                msgItem.dealMsg(_m_ccCrossGameCategory);
            } catch (Exception ex)
            {
                ex.printStackTrace();
            } catch (Throwable _th)
            {
                _th.printStackTrace();
            }
        }

        _lock();

        try
        {
            if (!_m_lCrossGameMsgList.isEmpty())
            {
                _m_lCrossGameMsgList.pop();
            }

            //判断消息队列是否为空
            return !_m_lCrossGameMsgList.isEmpty();
        } finally
        {
            _unlock();
        }
    }
}
