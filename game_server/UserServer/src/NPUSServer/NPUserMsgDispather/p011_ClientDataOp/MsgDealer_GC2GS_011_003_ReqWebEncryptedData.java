package NPUSServer.NPUserMsgDispather.p011_ClientDataOp;

import GC2GS.p011_ClientDataOp.GC2GS_011_003_ReqWebEncryptedData;
import GS2GC.p011_ClientDataOp.GS2GC_011_003_RetWebEncryptedData;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPPlayerParam;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import WCGCommon.Security.MD5;
import com.google.gson.JsonObject;
import org.apache.http.message.BasicNameValuePair;

import java.util.ArrayList;
import java.util.Base64;
import java.util.Comparator;

/**
 * 【C/W/S】页面启动透传参数
 * https://alidocs.dingtalk.com/i/nodes/EpGBa2Lm8azLMxb3tdwzMzvOWgN7R35y?corpId=&doc_type=wiki_doc&rnd=0.7021966955901446
 */
public class MsgDealer_GC2GS_011_003_ReqWebEncryptedData extends NPUserMsgDealer<GC2GS_011_003_ReqWebEncryptedData>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_011_003_ReqWebEncryptedData _msg)
    {
        NPUSUserData _userData = _committer.getUserData();

        String project = _msg.getProject();
        String regionId = String.valueOf(_userData.getUSServer().getRegionId());
        int serverId = _userData.getUSServer().getServerTypeId();
        String uid = _userData.getUid();
        long cid = _userData.getCid();
        String nickname = _userData.getPlayerComponent().getName();
        String icon = String.valueOf(_userData.getParam(ENPPlayerParam.ICON));
        float userPay = _msg.getUserPay();
        int vipLvl = (int) _userData.getParam(ENPPlayerParam.VIP_LVL);
        int level = (int) _userData.getParam(ENPPlayerParam.LEVEL);
        String langCode = _msg.getLangCode();
        int ts = CommonFunc.getNowTimeSec();
        String packageName = _msg.getPackageName();
        String products = _msg.getProducts();

        //构造签名
        String sign = buildSign(project, regionId, serverId, uid, cid, nickname, icon, userPay, vipLvl, level, langCode, ts);

        //构造json对象
        JsonObject jsonObject = buildJsonObject(project, regionId, serverId, uid, cid, nickname, icon, userPay, vipLvl, level, langCode, ts, sign, packageName, products);

        //将jsonObject字符串base64加密
        String base64String = Base64.getEncoder().encodeToString(jsonObject.toString().getBytes());

        _committer.commitSucRes(new GS2GC_011_003_RetWebEncryptedData(base64String));
    }

    /**
     * 构造签名
     * @return
     */
    private static String buildSign(String project, String regionId, int serverId, String uid, long cid, String nickname, String icon,
                                    float userPay, int vipLvl, int level, String langCode, int ts)
    {
        ArrayList<BasicNameValuePair> paramList = new ArrayList<>();
        paramList.add(new BasicNameValuePair("project", project));
        paramList.add(new BasicNameValuePair("region_id", regionId));
        paramList.add(new BasicNameValuePair("server_id", String.valueOf(serverId)));
        paramList.add(new BasicNameValuePair("uid", uid));
        paramList.add(new BasicNameValuePair("cid", String.valueOf(cid)));
        paramList.add(new BasicNameValuePair("nickname", nickname));
        paramList.add(new BasicNameValuePair("icon", icon));
        paramList.add(new BasicNameValuePair("user_pay", String.valueOf(userPay)));
        paramList.add(new BasicNameValuePair("vip_lv", String.valueOf(vipLvl)));
        paramList.add(new BasicNameValuePair("level", String.valueOf(level)));
        paramList.add(new BasicNameValuePair("lang_code", langCode));
        paramList.add(new BasicNameValuePair("ts", String.valueOf(ts)));
        paramList.sort(Comparator.comparing(BasicNameValuePair::getName));

        StringBuilder sb = new StringBuilder();
        for (BasicNameValuePair pair : paramList)
        {
            sb.append(pair);
        }
        String key = "activitymengjia";
        sb.append(key);
        return MD5.md5(sb.toString()).toLowerCase();
    }

    /**
     * 构造json对象
     * @return
     */
    private static JsonObject buildJsonObject(String project, String regionId, int serverId, String uid, long cid, String nickname,
                                        String icon, float userPay, int vipLvl, int level, String langCode, int ts, String sign, String packageName, String products)
    {
        JsonObject jsonObject = new JsonObject();
        jsonObject.addProperty("project", project);
        jsonObject.addProperty("region_id", regionId);
        jsonObject.addProperty("server_id", serverId);
        jsonObject.addProperty("uid", uid);
        jsonObject.addProperty("cid", cid);
        jsonObject.addProperty("nickname", nickname);
        jsonObject.addProperty("icon", icon);
        jsonObject.addProperty("user_pay", userPay);
        jsonObject.addProperty("vip_lv", vipLvl);
        jsonObject.addProperty("level", level);
        jsonObject.addProperty("lang_code", langCode);
        jsonObject.addProperty("ts", ts);
        jsonObject.addProperty("sign", sign);
        jsonObject.addProperty("package_name", packageName);
        jsonObject.addProperty("products", products);
        return jsonObject;
    }
}
