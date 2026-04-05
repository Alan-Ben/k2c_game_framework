package NPLoginServer.NPGCListener;

import ALBasicServer.ALSocket._AALBasicServerSocketListener;
import NPLoginServer.NPLSQueueMgr.NPLSQueueInfo;
import WCGCommon.Enum.NPEnum.EWCGCountryType;

import java.nio.ByteBuffer;

/**************
 * 客户端连接的监听对象
 * @author Administrator
 *
 */
public class NPLSGCListener extends _AALBasicServerSocketListener
{
    /**
     * 用户Id
     */
    private String _m_sUid;
    /**
     * 用户所属国籍
     */
    private EWCGCountryType _m_eCountryType;

    /**
     * 进入的排队节点对象
     */
    private NPLSQueueInfo _m_qiQueueInfo;
    private String _m_clientJson;
    private String _m_clinetToken;
    private String _m_pid;

    private boolean _m_inWhitelist;

    public NPLSGCListener(String _uid, boolean _inWhitelist)
    {
        _m_sUid = _uid;
        _m_eCountryType = EWCGCountryType.CHINA;
        _m_qiQueueInfo = null;
        _m_inWhitelist = _inWhitelist;
    }

    public String getUid()
    {
        return _m_sUid;
    }

    public EWCGCountryType getCountryType()
    {
        return _m_eCountryType;
    }

    public boolean inWhitelist()
    {
        return _m_inWhitelist;
    }

    @Override
    public void disconnect()
    {
    }

    @Override
    public void login()
    {
        //注册本对象到管理器中

    }

    @Override
    public void receiveMsg(ByteBuffer _msg)
    {
        NPLSGCDispather.getInstance().DealProtocol(this, _msg);
    }

    /**************
     * 接收的消息长度超出时调用的函数
     */
    @Override
    public void onBuffLengthOverSize(ByteBuffer _srcBuf, ByteBuffer _curReadingBuf)
    {
    }

    /******************
     * 设置排队对象
     */
    public void setQueue(NPLSQueueInfo _queueInfo)
    {
        if (null != _m_qiQueueInfo)
        {
            _m_qiQueueInfo.setDisable();
        }

        //设置变量对象
        _m_qiQueueInfo = _queueInfo;
    }

    /******************
     * 尝试退出队列
     */
    public void quitQueue()
    {
        if (null == _m_qiQueueInfo)
            return;

        //设置队列对象无效
        _m_qiQueueInfo.setDisable();
        _m_qiQueueInfo = null;
    }

    public void setClinetJson(String jsonExtends)
    {
        _m_clientJson = jsonExtends;
    }

    public String getClientJson()
    {
        return _m_clientJson;
    }

    public void setClientToken(String token)
    {
        _m_clinetToken = token;
    }

    public String getClientToken()
    {
        return _m_clinetToken;
    }

    public void setPid(String pid)
    {
        _m_pid = pid;
    }

    public String getPid()
    {
        return _m_pid;
    }

    public void setUid(String _uid)
    {
        _m_sUid = _uid;
    }
}
