package NPLoginServer.NPLSQueueMgr;

import ALBasicCommon.ALBasicCommonFun;
import NPLoginServer.NPGCListener.NPLSGCListener;

/*******************
 * 队列对象信息
 * @author Administrator
 *
 */
public class NPLSQueueInfo
{
    /**
     * 客户端对象
     */
    private NPLSGCListener _m_lListener;
    /**
     * 所处队列索引位置
     */
    private long _m_lQueueIdx;
    /**
     * 本队列对象是否还有效
     */
    private boolean _m_bEnable;
    /**
     * 加入队列的时间标记
     */
    private long _m_lEnterTimeTagMS;

    public NPLSQueueInfo(NPLSGCListener _listener, long _queueIdx)
    {
        _m_lListener = _listener;
        _m_lQueueIdx = _queueIdx;
        _m_bEnable = true;
        _m_lEnterTimeTagMS = ALBasicCommonFun.getNowTimeMS();
    }

    public NPLSGCListener getListener()
    {
        return _m_lListener;
    }

    public long getQueueIdx()
    {
        return _m_lQueueIdx;
    }

    public boolean enable()
    {
        return _m_bEnable;
    }

    public long getEnterTimeTagMS()
    {
        return _m_lEnterTimeTagMS;
    }

    /***************
     * 设置本节点无效
     */
    public void setDisable()
    {
        _m_bEnable = false;
    }
}
