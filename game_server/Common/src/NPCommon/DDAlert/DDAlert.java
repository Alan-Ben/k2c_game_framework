package NPCommon.DDAlert;

import ALServerLog.ALServerLog.LogLevel;
import AllRpcData.HS_Service.Common.HSDDAlert;
import NPCommon.Log.CommLog;
import NPCommon.Log.FormattingTuple;
import NPCommon.Log.MessageFormatter;
import NPCommon.NPVersion;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPDDAlertType;
import RPC.RpcSender;
import WCGBasicPlatServer._AWCGBasicPlatServer;
import WCGBasicServer._AWCGBasicServer;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;

//钉钉发送消息
public class DDAlert
{
    private RpcSender _m_Rpc2HttpServer;

    private String _m_sServer = "unknown";
    private String _m_sTimeZone = "";

    public String getServer()
    {
        return _m_sServer;
    }

    public String getTimeZone()
    {
        return _m_sTimeZone;
    }

    public void setRpcSender(RpcSender _rpcSender)
    {
        _m_Rpc2HttpServer = _rpcSender;
    }

    public void setServer(_AWCGBasicServer _server)
    {
        _m_Rpc2HttpServer = new RpcSender(_server, EServerType.SINGLE, ENPSingleServerType.HTTP.ordinal());

        //服务器类型
        StringBuilder sb = new StringBuilder();
        EServerType serverType = EServerType.EServerType_FromInt(_server.getServerType());

        if (null == serverType)
        {
            if (9999 == _server.getServerType())
            {
                sb.append("[bus]-").append(_server.getServerTypeId());
            } else
            {
                sb.append("[").append(_server.getServerType()).append("]-").append(_server.getServerTypeId());
            }
        } else if (EServerType.SINGLE == serverType)
        {
            ENPSingleServerType singleServerType = ENPSingleServerType.ENPSingleServerType_FromInt(_server.getServerTypeId());
            if (null == singleServerType || ENPSingleServerType.NONE == singleServerType)
                return;

            sb.append("[").append(singleServerType).append("]");
        } else
        {
            sb.append("[").append(serverType).append("]-").append(_server.getServerTypeId());
        }

        if (null != _server.getConf())
        {
            sb.append(", ").append(_server.getConf().getInternalIp()).append(":").append(_server.getConf().getInternalPort());
        }

        sb.append(", ").append("pid:").append(CommonFunc.getPid());

        _m_sServer = sb.toString();

        //时区
        if (null != CommonFunc.getTimeZone())
        {
            _m_sTimeZone = String.valueOf(CommonFunc.getTimeZone().toZoneId());
        }
    }

    /**
     * 针对平台服务器
     * @param _server
     */
    public void setServer(_AWCGBasicPlatServer _server)
    {
        if (null == _server)
            return;

        //服务器类型
        StringBuilder sb = new StringBuilder();
        sb.append("[plat]");
        _m_sServer = sb.toString();

        //时区
        if (null != CommonFunc.getTimeZone())
        {
            _m_sTimeZone = String.valueOf(CommonFunc.getTimeZone().toZoneId());
        }
    }

    public void fatal(String tag, String str)
    {
        alert(LogLevel.FATAL, tag, str);
        CommLog.fatal(str);
    }

    public void fatal(String tag, String str, Object... args)
    {
        FormattingTuple ft = MessageFormatter.arrayFormat(str, args);
        alert(LogLevel.FATAL, tag, ft.getMessage());
        CommLog.fatal(ft.getMessage(), ft.getThrowable());
    }

    public void err(String tag, String str)
    {
        alert(LogLevel.ERROR, tag, str);
        CommLog.error(str);
    }

    public void err(String tag, String str, Object... args)
    {
        FormattingTuple ft = MessageFormatter.arrayFormat(str, args);
        alert(LogLevel.ERROR, tag, ft.getMessage());
        CommLog.error(ft.getMessage(), ft.getThrowable());
    }

    public void warn(String tag, String str)
    {
        alert(LogLevel.WARNING, tag, str);
        CommLog.warn(str);
    }

    public void warn(String tag, String str, Object... args)
    {
        FormattingTuple ft = MessageFormatter.arrayFormat(str, args);
        alert(LogLevel.WARNING, tag, ft.getMessage());
        CommLog.warn(ft.getMessage(), ft.getThrowable());
    }

    public void alert(LogLevel lvl, String tag, String str)
    {
        DDAlertDelayExecutor.getInstance().delayExecute(tag, () -> sendToHS(ENPDDAlertType.COMMON, lvl, str));
    }

    public void sendToHS(ENPDDAlertType _robotType, LogLevel _log, String _content)
    {
        RpcSender sender = _m_Rpc2HttpServer;
        if (sender == null)
        {
            CommLog.error("DDAlert rpc send fail to hs rpc sender is null");
            return;
        }

        //服务器信息
        StringBuilder sbContent = new StringBuilder();
        sbContent.append("=========== 警报信息 ===========");
        sbContent.append("\n级别：").append(_log);
        sbContent.append("\n").append(_content);
        sbContent.append("\n服务器时间：").append(CommonFunc.getTimeString(CommonFunc.getNowTimeSec())).append(" ").append(_m_sTimeZone);
        sbContent.append("\n=========== 服务器信息 ===========");
        sbContent.append("\n服务器类型：").append(_m_sServer);
        sbContent.append("\n版本：").append(NPVersion.majorVersion())
        													.append(".").append(NPVersion.minorVersion())
        													.append(".").append(NPVersion.buildVersion())
        													.append(".").append(NPVersion.fixVersion());

        HSDDAlert rpc = new HSDDAlert();
        rpc.req().setAlertType(_robotType);
        rpc.req().setLogLvl(_log.ordinal());
        rpc.req().setTitle("");//标题目前不需要
        rpc.req().setTimeInfo(sbContent.toString());

        //无需回调
        sender.request(rpc, null);
    }
}
