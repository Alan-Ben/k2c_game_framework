package NPPlatServer.NPPS_Listener;

import ALServerLog.ALServerLog;
import NPPlatServer.NPGeneralListener._ANPPSBasicServerListener;
import NPPlatServer.NPPlatAreaMgr.NPPlatAreaInfo;
import NPPlatServer.NPPlatAreaMgr.NPPlatAreaMgr;
import WCGCommon.Enum.NPEnum.EServerType;

import java.nio.ByteBuffer;

public class PS_LSListener extends _ANPPSBasicServerListener
{
    /**
     * 对应区域的序列号
     */
    private int _m_iAreaTagIdx;
    /**
     * 服务器对应的序列标记
     */
    private String _m_sAreaTag;

    public PS_LSListener(int _serverTypeId)
    {
        super(EServerType.LOGIN, _serverTypeId);
    }

    public String getAreaTag()
    {
        return _m_sAreaTag;
    }

    public int getAreaTagIdx()
    {
        return _m_iAreaTagIdx;
    }

    @Override
    public void login()
    {
        ALServerLog.Sys("Login Server: " + getServerTypeId() + " Login!");
    }

    /*****************
     * 在服务器断开连接的时候处理的函数
     */
    @Override
    protected void _onDisconnect()
    {
        ALServerLog.Sys("Login Server: " + getServerTypeId() + " Disconnect!");

        //注销服务器的区域类型
        NPPlatAreaInfo areaInfo = NPPlatAreaMgr.getInstance().getAreaInfo(getAreaTagIdx());
        if (null != areaInfo)
            areaInfo.unregLS(this);
    }

    /**************
     * 接收的消息长度超出时调用的函数
     */
    @Override
    public void onBuffLengthOverSize(ByteBuffer _srcBuf, ByteBuffer _curReadingBuf)
    {
    }

    /**************
     * 初始化登录服务器相关信息
     * @param _areaTag
     * @param _areaTagIdx
     */
    public void initInfo(String _areaTag, int _areaTagIdx)
    {
        _m_sAreaTag = _areaTag;
        _m_iAreaTagIdx = _areaTagIdx;
    }
}
