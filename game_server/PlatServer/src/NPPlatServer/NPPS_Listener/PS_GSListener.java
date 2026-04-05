package NPPlatServer.NPPS_Listener;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALServerLog.ALServerLog;
import NPCommon.Log.CommLog;
import NPPlatServer.NPGeneralListener._ANPPSBasicServerListener;
import NPPlatServer.NPPlatAreaMgr.NPPlatAreaInfo;
import NPPlatServer.NPPlatAreaMgr.NPPlatAreaMgr;
import WCGCommon.Enum.NPEnum.EServerType;

import java.nio.ByteBuffer;

public class PS_GSListener extends _ANPPSBasicServerListener
{
    /**
     * 总可处理用户数量
     */
    private int _m_iTotalHandleUserCount;
    /**
     * 已处理用户数量
     */
    private int _m_iHandleUserCount;
    /**
     * 单个用户的权重
     */
    private int _m_iSingleUserWeight;
    /**
     * 已处理用户权重
     */
    private long _m_lHandleUserWeight;

    /**
     * 服务器连接信息
     */
    private String _m_sConnectIp;
    private int _m_iConnectPort;

    /**
     * 区域标记索引
     */
    private int _m_iAreaTagIdx;

    /**
     * 处理信息锁
     */
    private MutexAtom _m_handleMutex;

    public PS_GSListener(int _serverTypeId)
    {
        super(EServerType.GATE, _serverTypeId);

        _m_iTotalHandleUserCount = 0;
        _m_iHandleUserCount = 0;
        _m_iSingleUserWeight = 0;
        _m_lHandleUserWeight = 0;

        _m_sConnectIp = "";
        _m_iConnectPort = 0;

        _m_iAreaTagIdx = 0;

        _m_handleMutex = new MutexAtom();
    }

    public int getTotalHandleUserCount()
    {
        return _m_iTotalHandleUserCount;
    }

    public int getHandleUserCount()
    {
        return _m_iHandleUserCount;
    }

    //是否还可以处理用户
    public boolean canHandle()
    {
        return _m_iHandleUserCount < _m_iTotalHandleUserCount;
    }

    //当前用户已处理权重总值
    public long getHandleUserWeight()
    {
        return _m_lHandleUserWeight;
    }

    public String getConnectIp()
    {
        return _m_sConnectIp;
    }

    public int getConnectPort()
    {
        return _m_iConnectPort;
    }

    public int getAreaTagIdx()
    {
        return _m_iAreaTagIdx;
    }

    @Override
    public void login()
    {
        ALServerLog.Sys("WCG Gate Server: " + getServerTypeId() + " Login!");
    }

    /*****************
     * 在服务器断开连接的时候处理的函数
     */
    @Override
    protected void _onDisconnect()
    {
        ALServerLog.Sys("Gate Server: " + getServerTypeId() + " Disconnect!");

        //注销服务器的区域类型
        NPPlatAreaInfo areaInfo = NPPlatAreaMgr.getInstance().getAreaInfo(getAreaTagIdx());
        if (null != areaInfo)
            areaInfo.unregGS(this);
    }

    /**************
     * 接收的消息长度超出时调用的函数
     */
    @Override
    public void onBuffLengthOverSize(ByteBuffer _srcBuf, ByteBuffer _curReadingBuf)
    {
    }

    /*****************
     * 初始化服务器相关信息
     */
    public void initServerInfo(int _areaTagIdx, int _totalHandleUserCount, int _singleUserWeight, String _connectIp, int _connectPort)
    {
        _lockHandle();

        try
        {
            _m_iAreaTagIdx = _areaTagIdx;
            _m_iTotalHandleUserCount = _totalHandleUserCount;
            _m_iSingleUserWeight = _singleUserWeight;
            _m_sConnectIp = _connectIp;
            _m_iConnectPort = _connectPort;
        } finally
        {
            _unlockHandle();
        }
    }

    /****************
     * 增加处理用户，返回是否操作成功
     */
    public boolean addHandleUser()
    {
        _lockHandle();

        try
        {
            if (_m_iHandleUserCount >= _m_iTotalHandleUserCount)
            {
                CommLog.fatal("Gs Listener over load for: " + _m_iHandleUserCount + " / " + _m_iTotalHandleUserCount);
                return false;
            }

            //增加用户数量
            _m_iHandleUserCount++;
            _m_lHandleUserWeight += _m_iSingleUserWeight;

            return true;
        } finally
        {
            _unlockHandle();
        }
    }

    public boolean directAddHandleUser()
    {
        _lockHandle();

        try
        {
            //增加用户数量
            _m_iHandleUserCount++;
            _m_lHandleUserWeight += _m_iSingleUserWeight;

            return true;
        } finally
        {
            _unlockHandle();
        }
    }

    /****************
     * 减少处理用户
     */
    public void reduceHandleUser()
    {
        _lockHandle();

        try
        {
            //减少用户数量
            _m_iHandleUserCount--;
            _m_lHandleUserWeight -= _m_iSingleUserWeight;
        } finally
        {
            _unlockHandle();
        }
    }

    /****************
     * 清空所有处理的用户,返回减少的所有用户数量
     */
    public int clearAllUser()
    {
        _lockHandle();

        try
        {
            int handleUserCount = _m_iHandleUserCount;

            _m_iHandleUserCount = 0;
            _m_lHandleUserWeight = 0;

            return handleUserCount;
        } finally
        {
            _unlockHandle();
        }
    }

    protected void _lockHandle()
    {
        _m_handleMutex.lock();
    }

    protected void _unlockHandle()
    {
        _m_handleMutex.unlock();
    }
}
