package HttpServer.RPCDispatcher.Common;

import AllRpcData.HS_Service.Common.HSDDAlert;
import Common.NpServerObj.NpServerObj_DDRobot;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Log.CommLog;
import NPEnum.ENPDDAlertType;
import NPHttpServer.DDAlert.DingDingAlertKeyMgr;
import NPHttpServer.Http.Core.NPHSHttpServiceCore;
import NPHttpServer.Http.Core.NPHSHttpUtil;
import NPHttpServer.Http.Core._INPHttpCallBack;
import NPHttpServer.HttpServerConf;
import RPC.RpcDispatcher;
import RPC.RpcRequestHandler;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
import org.apache.http.Header;
import org.apache.http.message.BasicHeader;
import org.apache.http.message.BasicNameValuePair;

import java.io.UnsupportedEncodingException;
import java.net.URLEncoder;
import java.util.ArrayList;
import java.util.concurrent.atomic.AtomicLong;

public class HSDDAlert_Handler
{
    private static HSDDAlert_Handler _g_instance = new HSDDAlert_Handler();

    public static HSDDAlert_Handler getInstance()
    {
        return _g_instance;
    }

    //当前队列发送钉钉的总数量,发送前新增,发送后移除
    private AtomicLong _m_lProcessingDDCount = new AtomicLong(0);

    public void sendAlertToDD(ENPDDAlertType _robotType, int _logLvl, String _title, String _content)
    {
        NpServerObj_DDRobot robot = DingDingAlertKeyMgr.getInstance().lookupRobot(_robotType);
        if (null == robot)
        {
            robot = DingDingAlertKeyMgr.getInstance().lookupRobot(ENPDDAlertType.COMMON);
        }
        if (null != robot)
        {
            sendAlertToDD(robot, _logLvl, _title, _content);
        }
    }

    /********************************
     * 发送钉钉消息
     * 
     * 运营平台新版接口：https://apidoc.mjggpt.com/web/#/39/1693
     * 域名：http://public-tools.mjyx.com
     * 
     * @param _robot
     * @param _logLvl
     * @param _title
     * @param _content
     */
    public void sendAlertToDD(NpServerObj_DDRobot _robot, int _logLvl, String _title, String _content)
    {
        if (!HttpServerConf.getInstance().getOpenDDAlert())
            return;

        //超过500条钉钉预警不再发送到钉钉,错误输出到hs日志
        if (_m_lProcessingDDCount.get() >= 500)
        {
            CommLog.error("DDAlert reach limit, logLvl:{} content:{}", _logLvl, _content);
            return;
        }

        //构造钉钉预警信息
        String strContent = createDDAlertInfo(_title, _content);
        if(null == strContent)
        {
        	CommLog.error("DDAlert create content err");
        	return;
        }
        
        //保存发送次数
        _m_lProcessingDDCount.incrementAndGet();

        //获取uri
        //String url = "http://10.0.0.227:9505/api/dingding/robot/common_send";
        String url = "https://public-tools.mjyx.com/api/dingding/robot/common_send";
        //生成header
        ArrayList<BasicHeader> headers = NPHSHttpUtil.buildPlatFormUrlHeader(null);

        //钉钉预警相关参数
        ArrayList<BasicNameValuePair> paramList = new ArrayList<>();
        //填充钉钉预警相关信息
        paramList.add(new BasicNameValuePair("access_token", _robot.getToken()));
        paramList.add(new BasicNameValuePair("secret", _robot.getSecret()));
        paramList.add(new BasicNameValuePair("msgtype", "text"));
        paramList.add(new BasicNameValuePair("title", ""));
        paramList.add(new BasicNameValuePair("content", strContent));
        paramList.add(new BasicNameValuePair("encode", "urldecode"));
        //构造完整列表
        NPHSHttpUtil.buildPlatFormUrlEntityParmList(paramList, "mengjia");

        //发起http请求
        NPHSHttpServiceCore.getInstance().httpPostUrlEntity(url, headers, paramList, new _INPHttpCallBack()
        {
            @Override
            public void onSuc(Header[] headers, int code, String _responseStr)
            {
                if (code != 200)
                {
                    CommLog.error("HsRpcHandler_DDAlert sendAlertToDD post suc return fail code:{} alertContent:{}", code, strContent);
                }

                _m_lProcessingDDCount.decrementAndGet();
            }

            @Override
            public void onFail(Result _result)
            {
                CommLog.error("HsRpcHandler_DDAlert sendAlertToDD fail error:{} alertContent:{}", _result.getCode(), strContent);
                _m_lProcessingDDCount.decrementAndGet();
            }
        });
    }

    /**
     * 完善预警信息
     * @param _title
     * @param _content
     * @return
     */
    private static String createDDAlertInfo(String _title, String _content)
    {
        //构造钉钉预警消息
        StringBuilder alertContentBuilder = new StringBuilder();
        //已有内容信息
        alertContentBuilder.append(_content);
        //补充平台信息
        alertContentBuilder.append("\n平台：")
        	.append(HttpServerConf.getInstance().getPlatAreaId())
        	.append("-")
        	.append(HttpServerConf.getInstance().getPlatformId());
        //完整内容
        String content = alertContentBuilder.toString();
        
        //URLEncode转码，用于中文解析
        try 
        {
			return URLEncoder.encode(content, "utf-8");
		} 
        catch (UnsupportedEncodingException e) 
        {
			e.printStackTrace();
			return null;
		}
    }

    ////////////////////////////////////////////// 注册监听RPC //////////////////////////////////////////////

    public static void register(RpcDispatcher _dispatcher)
    {
        _dispatcher.regHandler(new RpcRequestHandler<HSDDAlert>()
        {
            @Override
            public void deal(_IWCGBasicRequestCommiter _committer, HSDDAlert _rpc)
            {
                //钉钉接口：https://apidoc.mjggpt.com/web/#/39/1693
                HSDDAlert_Handler.getInstance().sendAlertToDD(_rpc.req().getAlertType(), _rpc.req().getLogLvl(), _rpc.req().getTitle(), _rpc.req().getTimeInfo());
                //RPC必须执行回调，否则会造成内存泄露
                _rpc.commit();
            }
        });
    }
}
