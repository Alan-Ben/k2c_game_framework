package NPLoginCheckServer.NPSDKLoginMgr.SynTask;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import NPCommon.ErrMain.LoginErr;
import NPCommon.Http.HttpAsyncClient;
import NPCommon.Http._AResponseHandler;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.JsonUtil;
import NPLoginCheckServer.LoginCheckServerConf;
import NPLoginCheckServer.NPSDKLoginMgr.NPSDKEnterUtil;
import NPLoginCheckServer.NPSDKLoginMgr._ILoginOverDealer;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
import com.google.gson.JsonObject;
import org.apache.http.Header;

/*************
 * 帐号进入游戏服务器的处理函数
 * 账号通过各种对应手段获取对应的UID，并返回
 * 最终根据实际的选择进入对应的US服务器
 * @author Administrator
 *
 */
public class NPSynSDKEnterTask implements _IALSynTask
{
    //帐号信息
    private String _m_sAccName;
    private String _m_sToken;
    private final String _m_sClientIp;

    //登录链接对象
    private _IWCGBasicRequestCommiter _m_cCommiter;

    //SDK链接
    private String _m_sSdkUrl;

    private _ILoginOverDealer _m_overDealer;

    public NPSynSDKEnterTask(String _accName, String _accPass, String _clientIp, _IWCGBasicRequestCommiter _commiter, String _sdkUrl, _ILoginOverDealer _overDealer)
    {
        _m_sAccName = _accName;
        _m_sToken = _accPass;
        _m_sClientIp = _clientIp;
        _m_cCommiter = _commiter;
        _m_sSdkUrl = _sdkUrl;
        _m_overDealer = _overDealer;
    }

    @Override
    public void run()
    {
        long startTimeMs = CommonFunc.getNowTimeMS();
        //发送http请求，通过后台校验登录数据
        String sdkUrl = _m_sSdkUrl;
        String sdkLoginUrl = LoginCheckServerConf.getInstance().getSDKLoginUrl();
        //发送请求到后台
        HttpAsyncClient.startHttpPostFromData(sdkUrl + sdkLoginUrl, NPSDKEnterUtil.buildPostParam(_m_sClientIp, _m_sToken, _m_sAccName), new _AResponseHandler()
        {
            @Override
            public void onComplete(Header[] _headers, int _code, String _response)
            {
                //输出日志用于排查源站速度
                CommLog.info("NPSynSDKEnterTask receive post response cost {}ms url:{} httpCode:{}", CommonFunc.getNowTimeMS() - startTimeMs, sdkUrl, _code);

                //这里的_code是http报错，这里不代表收到了正常反馈
                if (_code != 200)
                {
                    CommLog.error("http errcode 200 ,url:{}", sdkUrl);
                    //访问失败的，开启多线路并发处理，不做错误返回处理
                    ALSynTaskManager.getInstance().regTask(new NPSynSDKEnterAndCheckTask(_m_sAccName, _m_sToken, _m_sClientIp, _m_cCommiter, _m_overDealer));
                    return;
                }

                //后续的是实际平台报错
                JsonObject jsonObject = CommonFunc.string2JsonObject(_response);
                int responseCode = JsonUtil.getInt(jsonObject, "code");
                //后台实际逻辑返回的200表示成功，其他情况都是失败。
                if (responseCode != 200)
                {
                    CommLog.error("NPSynSDKEnterAndCheckTask loginFail response:{} accName:{} token:{}", _response, _m_sAccName, _m_sToken);
                    _m_cCommiter.commitFailRes(LoginErr.LOGIN_SDK_RESPONSE_ERR.getCode());
                    _m_overDealer.onLoginFail(_code);
                    return;
                }

                //此处进入正式的登录流程
                NPSDKEnterUtil.dealAccName(_m_sAccName, _m_cCommiter);

                _m_overDealer.onLoginSuc();
            }

            @Override
            public void onFailed(Exception e)
            {
                CommLog.error("http fail ,url:{}", sdkUrl);
                e.printStackTrace();

                //访问失败的，开启多线路并发处理，不做错误返回处理
                ALSynTaskManager.getInstance().regTask(new NPSynSDKEnterAndCheckTask(_m_sAccName, _m_sToken, _m_sClientIp, _m_cCommiter, _m_overDealer));
            }
        });
    }
}
