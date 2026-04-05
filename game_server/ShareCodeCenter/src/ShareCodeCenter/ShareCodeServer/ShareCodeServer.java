package ShareCodeCenter.ShareCodeServer;

import ALServerLog.ALServerLog;
import NPCommon.NPVersion;
import NPCommon.Util.CommonFunc;
import ShareCodeCenter.Conf.Server.ShareCodeServerConf;
import ShareCodeCenter.ShareCodeServer.GeneralListener.MsgDispatcher.SCSGeneralMsgDispatcher;
import ShareCodeCenter.ShareCodeServer.GeneralListener.RequestDispather.SCSGeneralRequestDispather;
import ShareCodeCenter.ShareCodeServer.GeneralListener.SCSBasicServerListener;
import WCGBasicServer.WCGBSRecieverListener._AWCGBSReceiverListener;
import WCGBasicServer.WCGPSClientListener.WCGPSRequestCommiter;
import WCGBasicServer._AWCGBasicServer;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;

import java.nio.ByteBuffer;

public class ShareCodeServer extends _AWCGBasicServer
{
    private ShareCodeServerConf _m_conf;

    public ShareCodeServer(ShareCodeServerConf _conf)
    {
        _m_conf = _conf;
    }

    @Override
    public boolean isBSDirectlySend(int i, int i1)
    {
        return false;
    }

    @Override
    public int getServerType()
    {
        return EServerType.SINGLE.ordinal();
    }

    @Override
    public int getServerTypeId()
    {
        return ENPSingleServerType.SHARE_CODE.ordinal();
    }

    @Override
    public void onPSRegSuc()
    {
        ALServerLog.Sys("Plat Server Reg Suc!");
    }

    @Override
    public void onPSDisconnect()
    {
        ALServerLog.Sys("Plat Server Disconnect!");
    }

    @Override
    public void dealPlatServerCustomMsg(ByteBuffer _msg)
    {
        SCSGeneralMsgDispatcher.getInstance().DealProtocol(null, _msg);
    }

    @Override
    public void dealPlatServerRequest(WCGPSRequestCommiter _commiter, ByteBuffer _msg)
    {
        SCSGeneralRequestDispather.getInstance().DealProtocol(_commiter, _msg);
    }

    @Override
    public void dealUndealBusServerCustomMsg(int _busTypeId, ByteBuffer _msg)
    {

    }

    @Override
    public void dealUndealBusServerRequest(int _busTypeId, _IWCGBasicRequestCommiter _commiter, ByteBuffer _msg)
    {

    }

    @Override
    public void dealBroadcastCustomMsg(int _serverType, int _serverId, ByteBuffer _msg)
    {

    }

    @Override
    public _AWCGBSReceiverListener createBasicServerReceiverListener(int _serverType, int _serverTypeId)
    {
        //转化枚举类型，根据不同服务器进行不同的处理
        EServerType serverType = EServerType.values()[_serverType];
        return new SCSBasicServerListener(this, serverType, _serverTypeId);
    }

    @Override
    public void dealCacheBusMsgAlert(int i)
    {

    }

    @Override
    public void tryGetHandleBusServerFail()
    {

    }

    @Override
    public String getMonitorServerStateStr(String s)
    {
        ShareCodeMonitorInfo monitorInfo = new ShareCodeMonitorInfo();
        monitorInfo.setTotalMemory(Runtime.getRuntime().totalMemory());
        monitorInfo.setMaxMemory(Runtime.getRuntime().maxMemory());
        monitorInfo.setFreeMemory(Runtime.getRuntime().freeMemory());
        monitorInfo.setVersion(NPVersion.getString());
        monitorInfo.setTimeZone(CommonFunc.getTimeZone().getDisplayName());
        monitorInfo.setServerType(EServerType.SINGLE);
        monitorInfo.setServerTypeId(ENPSingleServerType.SHARE_CODE.ordinal());
        monitorInfo.setIp(getConf().getInternalIp());
        monitorInfo.setInnerPort(getConf().getPort());
        return monitorInfo.makeJsonObj().toString();
    }

    @Override
    protected boolean _init()
    {
        return true;
    }

    @Override
    protected void _onBusServerSenderDisconnect(int i)
    {
    }

	@Override
	public void onBusServerHandleChg(int arg0) 
	{
	}
}
