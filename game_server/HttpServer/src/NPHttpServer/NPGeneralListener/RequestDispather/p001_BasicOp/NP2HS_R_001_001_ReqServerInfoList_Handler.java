package NPHttpServer.NPGeneralListener.RequestDispather.p001_BasicOp;

import NP2HS_R.p001_HSOp.NP2HS_R_001_001_ReqServerInfoList;
import NPCommon.Dispather.NPRequestDispatcher.NPRequestDealer;
import NPCommon.ErrMain.HttpErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Log.CommLog;
import NPCommon.NP_SYS_ServerItem;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.JsonUtil;
import NPEnum.EServerOnlineState;
import NPEnum.EServerShowState;
import NPHttpServer.Http.Core.NPHSHttpServiceCore;
import NPHttpServer.Http.Core.NPHSHttpUtil;
import NPHttpServer.Http.Core._INPHttpCallBack;
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
 * @description: 请求注册新房间房间
 * @author: ricci
 * @date: 2022-04-07 16:13:13
 */
public class NP2HS_R_001_001_ReqServerInfoList_Handler extends NPRequestDealer<NP2HS_R_001_001_ReqServerInfoList>
{
    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _receiver, NP2HS_R_001_001_ReqServerInfoList _msg)
    {
        //获取uri
        String url = NPHSHttpUtil.buildPlatFormCommonUrl("/api/game/get_server_list");
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
                    CommLog.error("get_server_list fail! [response is null][CODE:{}]", code);
                    _receiver.commitFailRes(HttpErr.HTTP_RESPONSE_ERROR.getCode());
                    return;
                }
                //检查回包
                JsonObject obj = new JsonParser().parse(_responseStr).getAsJsonObject();
                if (!obj.has("code") || 1 != obj.get("code").getAsInt())
                {
                    CommLog.error("get_server_list got err response:{}", _responseStr);
                    _receiver.commitFailRes(HttpErr.PLATFORM_RESPONSE_ERROR.getCode());
                    return;
                }
                //检查平台数据
                JsonObject data = obj.get("data").getAsJsonObject();
                if (null == data)
                {
                    CommLog.error("get_server_list got err data:{}", obj.get("data"));
                    _receiver.commitFailRes(HttpErr.PLATFORM_RESPONSE_ERROR.getCode());
                    return;
                }
                //解析json获取服务器列表数据
                StringBuilder sb = new StringBuilder();
                ArrayList<NP_SYS_ServerItem> serverList = parse(data, sb);
                if (null == serverList)
                {
                    CommLog.error("get_server_list got parse error data:{},error:{}", obj.get("data"), sb);
                    _receiver.commitFailRes(HttpErr.PLATFORM_RESPONSE_DATA_ERROR.getCode());
                    return;
                }
                _receiver.commitSucRes(NP2HS_RB_Writer_001_PSOp.make_001_RetServerInfoList(_msg.getSerial(), serverList));
            }

            @Override
            public void onFail(Result _result)
            {
            	CommLog.error("get_server_list http errCode:{} errMsg:{} ", _result.getCode(), _result.getMsg());
                _receiver.commitFailRes(_result.getCode());
            }
        });

    }

    /**
     * 解析jason 创建服务器数据对象 ，解析失败返回 null ，并且将错误信息存储在_errSb中
     * @param _data  json
     * @param _errSb 错误信息
     * @return ArrayList<NP_SYS_ServerItem>
     */
    public ArrayList<NP_SYS_ServerItem> parse(JsonObject _data, StringBuilder _errSb)
    {
        //服务器数据
        ArrayList<NP_SYS_ServerItem> itemList = new ArrayList<>();
        //单台服务器详细数据
        JsonArray serverList = JsonUtil.getJsonArray(_data, "serverList");
        if (null == serverList)
        {
            _errSb.append("error serverList");
            return null;
        }

        for (JsonElement serverItem : serverList)
        {
            try
            {
                JsonObject serverObj = serverItem.getAsJsonObject();

                NP_SYS_ServerItem serverItemInfo = new NP_SYS_ServerItem();
                //服务器状态
                int iOnlineState = JsonUtil.getInt(serverObj, "onlineState", -1);
                if (iOnlineState == -1)
                {
                    _errSb.append("error onlineState:").append(serverObj.toString());
                    return null;
                }
                EServerOnlineState onlineState = EServerOnlineState.EServerOnlineState_FromInt(iOnlineState);
                if (null == onlineState)
                {
                    _errSb.append("error onlineState:").append(serverObj.toString());
                    return null;
                }
                //显示状态
                int iShowState = JsonUtil.getInt(serverObj, "showState", -1);
                if (iShowState == -1)
                {
                    _errSb.append("error showState:").append(serverObj.toString());
                    return null;
                }
                EServerShowState showState = EServerShowState.EServerShowState_FromInt(iShowState);
                if (null == showState)
                {
                    _errSb.append("error showState:").append(serverObj.toString());
                    return null;
                }

                int serverId = JsonUtil.getInt(serverObj, "serverId", 0);
                if (serverId == 0)
                {
                    _errSb.append("error serverId:").append(serverObj.toString());
                    return null;
                }
                serverItemInfo.setServerLogicId(serverId);

                int usTypeId = JsonUtil.getInt(serverObj, "usTypeId", 0);
                if (usTypeId == 0)
                {
                    _errSb.append("error usTypeId:").append(serverObj.toString());
                    return null;
                }
                serverItemInfo.setServerTypeId(usTypeId);

                String serverName = JsonUtil.getString(serverObj, "serverName", "");
                if (serverName.isEmpty())
                {
                    _errSb.append("error serverName:").append(serverObj.toString());
                    return null;
                }
                serverItemInfo.setServerName(serverName);

                serverItemInfo.setOnlineStateTypeId(onlineState.ordinal());
                serverItemInfo.setShowStateTypeId(showState.ordinal());
                String startDate = JsonUtil.getString(serverObj, "startDate", "").trim();
                if (!startDate.isEmpty())
                {
                    if (!CommonFunc.checkValidNewDate(startDate))
                    {
                        _errSb.append("error startDate:").append(startDate);
                        return null;
                    }
                }
                serverItemInfo.setStartDate(startDate);

                itemList.add(serverItemInfo);
            } catch (Exception e)
            {
                _errSb.append(e.getMessage());
                CommLog.error("parse server list error json src:{} !", serverItem.toString(), e);
                return null;
            }
        }
        return itemList;
    }
}
