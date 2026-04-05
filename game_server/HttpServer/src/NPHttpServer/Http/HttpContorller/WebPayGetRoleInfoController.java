package NPHttpServer.Http.HttpContorller;

import ALBasicProtocolPack._IALProtocolStructure;
import NP2US_RB.p005_WebPayOp.NP2US_RB_005_003_RetGetWebPayRoleInfo;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result.ResultMgr;
import NPCommon.Http.HttpService.annotation.RequestMapping;
import NPCommon.Http.HttpService.server.HttpMethod;
import NPCommon.Http.HttpService.server.HttpRequest;
import NPCommon.Http.HttpService.server.HttpResponse;
import NPCommon.Log.CommLog;
import NPCommon.Util.HttpUtils;
import NPHttpServer.HttpServerConf;
import NPHttpServer.NPHttpServer;
import NPServerProtocolWriter.NP2US.Request.NP2US_R_Writer_005_WebPayOp;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum.EServerType;
import com.google.gson.JsonObject;
import org.apache.http.NameValuePair;
import org.apache.http.client.utils.URLEncodedUtils;

import java.util.HashMap;
import java.util.List;

import static org.apache.http.Consts.UTF_8;

/**
 * 角色信息查询接口
 *
 * 功能：获取单个角色的基础信息
 *
 * 请求参数：
 * - game_id: 游戏ID（必填）
 * - region_id: 平台区域ID（必填）
 * - cid: 角色ID（必填）
 * - server_id: 服务器ID（必填，对应运营后台ServerID）
 * - lang: 语言标识（必填，如 CN、EN）
 * - timestamp: 时间戳（必填，10位）
 * - sign: 签名（必填）
 *
 * 返回格式：
 * {
 *   "code": 1,
 *   "result": {
 *     "cid": "角色ID",
 *     "uid": "用户ID",
 *     "nickname": "角色昵称",
 *     "lv": 等级,
 *     "vip_lv": VIP等级
 *   },
 *   "msg": "success"
 * }
 *
 * @author: system
 * @date: 2025-01-07
 */
public class WebPayGetRoleInfoController
{
    @RequestMapping(uri = "/webPay/getRoleInfo", method = HttpMethod.POST)
    public void getRoleInfo(HttpRequest _request, final HttpResponse _response)
    {
        try
        {
            // 解析POST表单数据
            List<NameValuePair> parseList = URLEncodedUtils.parse(_request.getPostBody(), UTF_8);
            HashMap<String, String> keyValMap = new HashMap<>();
            for (NameValuePair nameValuePair : parseList)
            {
                keyValMap.put(nameValuePair.getName(), nameValuePair.getValue());
            }

            // 签名验证
            if (!HttpUtils.checkWebPaySign(parseList, HttpServerConf.getInstance().getPlatFromClientKey()))
            {
                _response.response(401, "Sign check fail!");
                CommLog.error("WebPayGetRoleInfoController signCheck fail, data:{}", _request.getPostBody());
                return;
            }

            // 参数解析和验证
            String gameIdStr = keyValMap.get("game_id");
            String regionIdStr = keyValMap.get("region_id");
            String cidStr = keyValMap.get("cid");
            String serverIdStr = keyValMap.get("server_id");
            String lang = keyValMap.get("lang");
            String timestampStr = keyValMap.get("timestamp");

            // 必填参数校验
            if (gameIdStr == null || regionIdStr == null || cidStr == null ||
                serverIdStr == null || lang == null || timestampStr == null)
            {
                _response.response(400, "Missing required parameters!");
                CommLog.error("WebPayGetRoleInfoController missing parameters, data:{}", _request.getPostBody());
                return;
            }

            // 参数类型转换
            int gameId = Integer.parseInt(gameIdStr);
            int regionId = Integer.parseInt(regionIdStr);
            long cid = Long.parseLong(cidStr);
            int serverId = Integer.parseInt(serverIdStr);
            long timestamp = Long.parseLong(timestampStr);

            // 调用UserServer获取角色信息
            NPHttpServer.getInstance().sendRequestToBSServer(
                EServerType.USER.ordinal(),
                serverId,
                NP2US_R_Writer_005_WebPayOp.make_003_ReqGetWebPayRoleInfo(cid),
                new _IWCGCallbackDealer()
                {
                    @Override
                    public void dealSuc(_IALProtocolStructure _retProto)
                    {
                        NP2US_RB_005_003_RetGetWebPayRoleInfo ret = (NP2US_RB_005_003_RetGetWebPayRoleInfo) _retProto;

                        // 构造返回数据
                        JsonObject result = new JsonObject();
                        result.addProperty("code", 1);
                        result.addProperty("msg", "success");

                        // 将角色信息转换为 JSON 格式
                        JsonObject data = new JsonObject();
                        Common.ServerObj.ServerObj_WebPayRoleInfo roleInfo = ret.getRoleInfo();
                        data.addProperty("cid", roleInfo.getCid());
                        data.addProperty("uid", roleInfo.getUid());
                        data.addProperty("nickname", roleInfo.getName());
                        data.addProperty("lv", roleInfo.getLvl());
                        data.addProperty("vip_lv", roleInfo.getVipLvl());

                        result.add("result", data);
                        _response.response(200, result.toString());
                        CommLog.info("WebPayGetRoleInfoController getRoleInfo success, cid:{}", cid);
                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                        // 构造错误返回数据
                        Result result = ResultMgr.getInstance().lookupResult(_errCode);
                        JsonObject response = new JsonObject();
                        response.addProperty("code", _errCode);
                        response.addProperty("msg", result == null ? "Unknown error" : result.getMsg());
                        _response.response(200, response.toString());

                        CommLog.error("WebPayGetRoleInfoController getRoleInfo fail, cid:{}, errCode:{}", cid, _errCode);
                    }

                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2US_RB_005_003_RetGetWebPayRoleInfo();
                    }
                });

        }
        catch (NumberFormatException e)
        {
            _response.response(400, "Invalid parameter format!");
            CommLog.error("WebPayGetRoleInfoController parameter format error, exception:{}", e);
        }
        catch (Exception e)
        {
            _response.response(500, "Internal server error!");
            CommLog.error("WebPayGetRoleInfoController exception:{}", e);
        }
    }

}
