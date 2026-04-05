package NPHttpServer.NPGeneralListener.RequestDispather.p001_BasicOp;

import Common.ServerObj.ServerObj_PHPParam;
import Common.ServerObj.ServerObj_PHPParamList;
import CommonEnum.EPlatParamType;
import NP2HS_R.p001_HSOp.NP2HS_R_001_006_ReqPlatParamList;
import NPCommon.Dispather.NPRequestDispatcher.NPRequestDealer;
import NPCommon.ErrMain.HttpErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Log.CommLog;
import NPCommon.Util.JsonUtil;
import NPHttpServer.Http.Core.NPHSHttpServiceCore;
import NPHttpServer.Http.Core.NPHSHttpUtil;
import NPHttpServer.Http.Core._INPHttpCallBack;
import NPHttpServer.Http.Entity.NPEntityPHPParamList;
import NPHttpServer.NPGeneralListener.Writer.NP2HS_RB_Writer_001_PSOp;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
import com.google.gson.JsonArray;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;
import org.apache.http.Header;
import org.apache.http.message.BasicHeader;
import org.apache.http.message.BasicNameValuePair;

import java.util.ArrayList;

/**
 * 请求获取平台参数
 * 
 */
public class NP2HS_R_001_006_ReqPlatParamList_Handler extends NPRequestDealer<NP2HS_R_001_006_ReqPlatParamList>
{
    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _receiver, NP2HS_R_001_006_ReqPlatParamList _msg)
    {
        //获取uri
        String url = NPHSHttpUtil.buildPlatFormCommonUrl("/api/game/get_plat");
        //生成header
        ArrayList<BasicHeader> headers = NPHSHttpUtil.buildPlatFormUrlHeader(null);
        //生成基础param
        ArrayList<BasicNameValuePair> parmList = NPHSHttpUtil.buildPlatFormUrlEntityParmList(null);
        //发起http请求
        NPHSHttpServiceCore.getInstance().httpPostUrlEntity(url, headers, parmList, new _INPHttpCallBack()
        {
            @Override
            public void onSuc(Header[] headers, int code, String _responseStr)
            {
                if (_responseStr.isEmpty() || code != 200)
                {
                    CommLog.error("get_plat fail! [response is null][CODE:{}]", code);
                    _receiver.commitFailRes(HttpErr.HTTP_RESPONSE_ERROR.getCode());
                    return;
                }
                //检查回包
                JsonObject obj = new JsonParser().parse(_responseStr).getAsJsonObject();
                if (!obj.has("code") || 1 != obj.get("code").getAsInt())
                {
                    CommLog.error("get_plat got err response:{}", _responseStr);
                    _receiver.commitFailRes(HttpErr.PLATFORM_RESPONSE_ERROR.getCode());
                    return;
                }
                //检查平台数据
                JsonObject data = obj.get("data").getAsJsonObject();
                if (null == data)
                {
                    CommLog.error("get_plat got err data:{}", obj.get("data"));
                    _receiver.commitFailRes(HttpErr.PLATFORM_RESPONSE_ERROR.getCode());
                    return;
                }
                
                StringBuilder errSb = new StringBuilder();
                ServerObj_PHPParamList listObj = parse(data, errSb);
                if(null == listObj)
                {
                    CommLog.error("get_plat parse err data:{} err:{}", obj.get("data"), errSb.toString());
                    _receiver.commitFailRes(HttpErr.PLATFORM_RESPONSE_DATA_ERROR.getCode());
                    return;
                }
                
                _receiver.commitSucRes(NP2HS_RB_Writer_001_PSOp.make_006_RetPlatParamList(NPEntityPHPParamList.buildDataSerial(), "", listObj));
            }

            @Override
            public void onFail(Result _result)
            {
            	CommLog.error("get_plat http errCode:{} errMsg:{} ", _result.getCode(), _result.getMsg());
                _receiver.commitFailRes(_result.getCode());
            }
        });

    }
    
    /**
     * 解析平台参数数据
     * @param _data
     * @param _errSb
     * @return
     */
    public ServerObj_PHPParamList parse(JsonObject _data, StringBuilder _errSb)
    {
    	//单台服务器详细数据
        JsonArray platParams = JsonUtil.getJsonArray(_data, "platParams");
        if (null == platParams)
        {
            _errSb.append("error platParams");
            return null;
        }

    	ServerObj_PHPParamList listObj = new ServerObj_PHPParamList();
    	
    	for (JsonElement param : platParams)
        {
            try
            {
                JsonObject paramObj = param.getAsJsonObject();
                
                //参数key
                String pKey = JsonUtil.getString(paramObj, "key", null);
                if (null == pKey || pKey.isEmpty())
                {
                    _errSb.append("error key");
                    return null;
                }
                //参数value
                String pValue = JsonUtil.getString(paramObj, "value", null);
                if (null == pValue)
                {
                    _errSb.append("error pValue");
                    return null;
                }
                
                ServerObj_PHPParam paramInfo = new ServerObj_PHPParam();
                paramInfo.setPKey(EPlatParamType.valueOf(pKey));
                paramInfo.setPValue(pValue);
                
                listObj.addPList(paramInfo);
            } 
            catch (Exception e)
            {
                _errSb.append(e.getMessage());
                CommLog.error("parse param error json src:{} !", param.toString(), e);
                return null;
            }
        }
    	
		return listObj;
    }
}
