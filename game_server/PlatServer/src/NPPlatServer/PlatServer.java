package NPPlatServer;

import ALServerLog.ALServerLog;
import NPCommon.DDAlert.DDAlert;
import NPCommon.GMCommand.GmCommandMgr;
import NPCommon.Util.ServerGMUtil;
import NPPlatServer.GMCommand.Cmds.CmdServer;
import NPPlatServer.NPPS_Listener.*;
import NPPlatServer.PlatListenerMgr.PlatListenerMgr;
import WCGBasicPlatServer.BasicServerListener._AWCGBasicServerListener;
import WCGBasicPlatServer._AWCGBasicPlatServer;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;

/**************
 * 登录服务器的服务器处理对象
 * @author Administrator
 *
 */
public class PlatServer extends _AWCGBasicPlatServer
{
    public static PlatServer getInstance()
    {
        return (PlatServer) PlatServer.getServerObj();
    }

    private DDAlert _m_dlDDAlert = new DDAlert();


    public DDAlert getDDAlert() {return _m_dlDDAlert;}

    @Override
    protected boolean _init()
    {
        //初始化GM命令
        GmCommandMgr.getInstance().init(CmdServer.class.getPackage().getName());

        //钉钉预警初始化
        getDDAlert().setServer(this);

        //服务器初始化相关处理
        ServerGMUtil.initServer(null,  getDDAlert(),"PlatServer");

        return true;
    }

    protected PlatServer()
    {

    }

    /*************************
     * 根据带入的处理对象和客户端类型以及自定义信息，生成对应的消息监听对象
     * @param _serverType
     * @param _serverTypeId
     * @return
     */
    @Override
    public _AWCGBasicServerListener createNewListener(int _serverType, int _serverTypeId)
    {
        EServerType serverType = EServerType.values()[_serverType];
        if (EServerType.LOGIN == serverType)
        {
            return new PS_LSListener(_serverTypeId);
        } else if (EServerType.GATE == serverType)
        {
            return new PS_GSListener(_serverTypeId);
        } else if (EServerType.USER == serverType)
        {
            return new PS_USListener(_serverTypeId);
        } else if (EServerType.CROSS_GAME == serverType)
        {
            return new PS_CGSListener(_serverTypeId);
        } else if (EServerType.CROSS_RANK == serverType)
        {
            return new PS_CRSListener(_serverTypeId);
        } else if (EServerType.GAME_LOGIC == serverType)
        {
            return new PS_GLSListener(_serverTypeId);
        } else if (EServerType.SINGLE == serverType)
        {
            ENPSingleServerType singleServerType = ENPSingleServerType.values()[_serverTypeId];
            if (singleServerType == ENPSingleServerType.LOGIN_CHECK)
                return new PS_LCSListener();
            else if (singleServerType == ENPSingleServerType.COMMON)
                return new PS_CSListener();
            else if (singleServerType == ENPSingleServerType.INTERFACE)
                return new PS_ISListener();
            else if (singleServerType == ENPSingleServerType.RECORD)
                return new PS_RCSListener();
            else if (singleServerType == ENPSingleServerType.HTTP)
                return new PS_HSListener();
            else if (singleServerType == ENPSingleServerType.SCHEDULE)
                return new PS_SSListener();
            else if (singleServerType == ENPSingleServerType.MARRY_MATCH)
            	return new PS_MarryMatchServerListener();
            else if (singleServerType == ENPSingleServerType.DINNER)
                return new PS_DinnerServerListener();
            else if (singleServerType == ENPSingleServerType.SHARE_CODE)
                return new PS_ShareCodeServerListener();
            else if (singleServerType == ENPSingleServerType.PAY)
                return new PS_PayServerListener();
            else if (singleServerType == ENPSingleServerType.CROSS_DATA)
                return new PS_CDSListener();
            else if(singleServerType == ENPSingleServerType.CROSS_TEAM)
                return new PS_CTSListener();
            else
                return null;
        } else
        {
            return null;
        }
    }

    /**********************
     * 在有服务器连接的时候调用的连接操作
     * @param _serverType
     * @param _serverTypeId
     * @param _listener
     */
    @Override
    public void onBasicServerReg(int _serverType, int _serverTypeId, _AWCGBasicServerListener _listener)
    {
        if (_serverType < 9999)
        {
            ALServerLog.Sys("ServerType: " + EServerType.values()[_serverType] + " ServerTypeId: " + _serverTypeId + " Reg Suc!");
        } else
        {
            ALServerLog.Sys("ServerType Bus ServerTypeId: " + _serverTypeId + " Reg Suc!");
        }

        PlatListenerMgr.getInstance().register(_serverType, _serverTypeId, _listener);
    }

    /**********************
     * 在有服务器注销的时候调用的连接操作
     * @param _serverType
     * @param _serverTypeId
     * @param _listener
     */
    @Override
    public void onBasicServerUnReg(int _serverType, int _serverTypeId, _AWCGBasicServerListener _listener)
    {
        if (_serverType < 9999)
        {
            ALServerLog.Sys("ServerType: " + EServerType.values()[_serverType] + " ServerTypeId: " + _serverTypeId + " UnReg!");
        } else
        {
            ALServerLog.Sys("ServerType Bus ServerTypeId: " + _serverTypeId + " UnReg!");
        }

        PlatListenerMgr.getInstance().unRegister(_serverType, _serverTypeId);
    }

    /******************
     * 在版本不匹配的时候构建监听对象的处理函数
     * @param _serverType
     * @param _serverTypeId
     * @return
     */
    @Override
    public _AWCGBasicServerListener createNewListenerWhenVersionNotFix(int _serverType, int _serverTypeId)
    {
        return null;
    }

    /**********************
     * 在已经有服务器的时候重复注册的事件响应函数
     * @param _serverType
     * @param _serverTypeId
     */
    @Override
    public void onBasicServerMultiReg(int _serverType, int _serverTypeId)
    {
        //暂时不做处理
    }

    /********************
     * 在没有办法给服务器分配总线服务器的时候触发的警告处理
     */
    @Override
    public void onBusHandleBSNoServer(int _serverType, int _serverTypeId)
    {
        ALServerLog.Fatal("No Bus Server for [" + _serverType + "] - [" + _serverTypeId + "]");
    }
}
