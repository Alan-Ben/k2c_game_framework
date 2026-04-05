package NPHttpServer.Http.Core;

import NPCommon.Http.HttpAsyncClient;
import NPCommon.Http._AResponseHandler;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.JsonUtil;
import NPHttpServer.HttpServerConf;
import WCGCommon.Security.MD5;
import com.google.gson.JsonObject;
import org.apache.http.Header;
import org.apache.http.message.BasicHeader;
import org.apache.http.message.BasicNameValuePair;

import java.util.ArrayList;
import java.util.Comparator;

/**
 * @description: 工具类
 * @author: ricci
 * @date: 2023-03-23 15:12:53
 */
public class NPHSHttpUtil
{
    /**
     * 创建一个jsonObj，里面有发给平台的基础参数
     * @return JsonObject
     */
    public static JsonObject buildPlatFormCommonJsonObj()
    {
        JsonObject jsObj = new JsonObject();
        jsObj.addProperty("platform_id", HttpServerConf.getInstance().getPlatformId());
        jsObj.addProperty("area_id", HttpServerConf.getInstance().getPlatAreaId());
        return jsObj;
    }

    /**
     * 创建一个header列表，请求后台通用的header参数加上自定义参数
     * @return String
     */
    public static ArrayList<BasicHeader> buildPlatFormJsonHeader(ArrayList<BasicHeader> _headerList)
    {
        ArrayList<BasicHeader> headers = new ArrayList<>();
        headers.add(new BasicHeader("content-type", "application/json"));
        if (_headerList != null)
        {
            headers.addAll(_headerList);
        }
        return headers;
    }

    /**
     * 创建一个header列表，请求后台通用的header参数加上自定义参数
     * @return String
     */
    public static ArrayList<BasicHeader> buildPlatFormUrlHeader(ArrayList<BasicHeader> _headerList)
    {
        ArrayList<BasicHeader> headers = new ArrayList<>();
        headers.add(new BasicHeader("content-type", "application/x-www-form-urlencoded; charset=UTF-8"));
        if (_headerList != null)
        {
            headers.addAll(_headerList);
        }
        return headers;
    }

    /**
     * 创建一个url，是配置的后台地址+请求url
     * @return String
     */
    public static String buildPlatFormCommonUrl(String _url)
    {
        return HttpServerConf.getInstance().getPlatUrl() + _url;
    }

    /**
     * 创建一个url格式请求的参数列表，里面有发给平台的基础参数
     * @return ArrayList<BasicNameValuePair>
     */
    public static ArrayList<BasicNameValuePair> buildPlatFormUrlEntityParmList(ArrayList<BasicNameValuePair> _paramList)
    {
        ArrayList<BasicNameValuePair> paramList = new ArrayList<>();
        paramList.add(new BasicNameValuePair("platform_id", String.valueOf(HttpServerConf.getInstance().getPlatformId())));
        paramList.add(new BasicNameValuePair("area_id", String.valueOf(HttpServerConf.getInstance().getPlatAreaId())));
        paramList.add(new BasicNameValuePair("timestamp", String.valueOf(CommonFunc.getNowTimeSec())));
        if (_paramList != null)
        {
            paramList.addAll(_paramList);
        }
        //通用sign签名生成规则
        //按key升序
        paramList.sort(Comparator.comparing(BasicNameValuePair::getName));
        //生成签名
        StringBuilder sb = new StringBuilder();
        for (BasicNameValuePair nameValuePair : paramList)
        {
            sb.append(nameValuePair.getName()).append("=").append(nameValuePair.getValue());
        }
        sb.append(HttpServerConf.getInstance().getPlatKey());
        String sign = MD5.md5(sb.toString()).toLowerCase();
        paramList.add(new BasicNameValuePair("sign", sign));

        return paramList;
    }
    
    /**
     * 自带私钥进行拼凑
     * @param _paramList
     * @param _skey
     */
    public static void buildPlatFormUrlEntityParmList(ArrayList<BasicNameValuePair> _paramList, String _skey)
    {
        _paramList.add(new BasicNameValuePair("platform_id", String.valueOf(HttpServerConf.getInstance().getPlatformId())));
        _paramList.add(new BasicNameValuePair("area_id", String.valueOf(HttpServerConf.getInstance().getPlatAreaId())));
        _paramList.add(new BasicNameValuePair("timestamp", String.valueOf(CommonFunc.getNowTimeSec())));
        //通用sign签名生成规则
        //按key升序
        _paramList.sort(Comparator.comparing(BasicNameValuePair::getName));
        //生成签名
        StringBuilder sb = new StringBuilder();
        for (BasicNameValuePair nameValuePair : _paramList)
        {
            sb.append(nameValuePair.getName()).append("=").append(nameValuePair.getValue());
        }
        sb.append(_skey);
        String srcStr = sb.toString();
        //生成key
        String sign = MD5.md5Encryption(srcStr).toLowerCase();
        //组成提交数据
        _paramList.add(new BasicNameValuePair("sign", sign));
    }
    
    /**
     * 回调平台接口
     * 接口文档：https://apidoc.mjggpt.com/web/#/9/200
     * @param _event
     * @param _serial
     * @param _result
     * @param _msg
     * @param _data
     */
    public static void callbackToPHP(String _event, String _serial, int _result, String _msg, String _data)
    {
    	//构造参数
    	ArrayList<BasicNameValuePair> paramList = new ArrayList<>();
    	paramList.add(new BasicNameValuePair("event", _event));
    	paramList.add(new BasicNameValuePair("serial", _serial));
    	paramList.add(new BasicNameValuePair("result", String.valueOf(_result)));
    	paramList.add(new BasicNameValuePair("msg", _msg));
    	if(null != _data)
    	{
    		paramList.add(new BasicNameValuePair("data", _data));
    	}
    	
    	buildPlatFormUrlEntityParmList(paramList, HttpServerConf.getInstance().getPlatKey());

    	//构造URL
    	String callbackUrl = HttpServerConf.getInstance().getPlatUrl() + "/api/game/notice_to_finish";

    	//起始时间，用于计算耗时
        long startTimeMs = CommonFunc.getNowTimeMS();

        //发送请求到后台
        HttpAsyncClient.startHttpPostFromData(callbackUrl, paramList, new _AResponseHandler()
        {
            @Override
            public void onComplete(Header[] _headers, int _code, String _response)
            {
                //输出日志用于排查源站速度
                CommLog.info("callbackToPHP receive post response cost {}secs url:{} httpCode:{}", (CommonFunc.getNowTimeMS() - startTimeMs)/1000, callbackUrl, _code);

                //这里的_code是http报错，这里不代表收到了正常反馈
                if (_code != 200)
                {
                    CommLog.error("callbackToPHP http errCode:{} url:{} serial:{}", _code, callbackUrl, _serial);
                    return;
                }

                //后续的是实际平台报错
                JsonObject jsonObject = CommonFunc.string2JsonObject(_response);
                
                int responseCode = JsonUtil.getInt(jsonObject, "code");
                String responseMsg = JsonUtil.getString(jsonObject, "msg");
                //后台实际逻辑返回的1表示成功，其他情况都是失败。
                if (responseCode != 1)
                {
                	CommLog.error("callbackToPHP http php errCode:{} msg:{} url:{} serial:{}", responseCode, responseMsg, callbackUrl, _serial);
                    return;
                }

                CommLog.info("callbackToPHP http suc url:{} serial:{}", callbackUrl, _serial);
            }

            @Override
            public void onFailed(Exception e)
            {
                CommLog.error("http fail ,url:{}", callbackUrl);
                
                e.printStackTrace();
            }
        });
    
    }
}
