package NPLoginCheckServer.NPSDKLoginMgr.SynTask;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALProcess.ALProcess;
import ALBasicServer.ALTask._IALSynTask;
import NPCommon.CommonProcess._IEZProcessMonitorNoTimeOut;
import NPCommon.ErrMain.LoginErr;
import NPCommon.Http.HttpAsyncClient;
import NPCommon.Http._AResponseHandler;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.JsonUtil;
import NPLoginCheckServer.LoginCheckServerConf;
import NPLoginCheckServer.NPLoginCheckServer;
import NPLoginCheckServer.NPSDKLoginMgr.NPSDKEnterUtil;
import NPLoginCheckServer.NPSDKLoginMgr.NPSDKLoginMgr;
import NPLoginCheckServer.NPSDKLoginMgr._ILoginOverDealer;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
import com.google.gson.JsonObject;
import org.apache.http.Header;

import java.util.List;

/*************
 * 帐号进入游戏服务器的处理函数
 * 账号通过各种对应手段获取对应的UID，并返回
 * 最终根据实际的选择进入对应的US服务器
 * @author Administrator
 *
 */
public class NPSynSDKEnterAndCheckTask implements _IALSynTask
{
    private static final MutexAtom _m_mutex = new MutexAtom();
    //帐号信息
    private String _m_sAccName;
    private String _m_sToken;
    private final String _m_sClientIp;

    //登录链接对象
    private _IWCGBasicRequestCommiter _m_cCommiter;

    //是否已经检测完成
    private boolean _m_bHasDone;

    private _ILoginOverDealer _m_overDealer;

    public NPSynSDKEnterAndCheckTask(String _accName, String _accPass, String _clientIp, _IWCGBasicRequestCommiter _commiter, _ILoginOverDealer _overDealer)
    {
        _m_sAccName = _accName;
        _m_sToken = _accPass;
        _m_sClientIp = _clientIp;
        _m_cCommiter = _commiter;
        _m_bHasDone = false;
        _m_overDealer = _overDealer;
    }

    @Override
    public void run()
    {
        //可供选择的SDK链接列表
        List<String> canChooseUrlList = NPSDKLoginMgr.getInstance().getCanChooseUrlList();
        //SDK登录验证地址
        String sdkLoginUrl = LoginCheckServerConf.getInstance().getSDKLoginUrl();

        //创建对应长度的process列表
        ALProcess[] list = new ALProcess[canChooseUrlList.size()];

        //遍历检查
        for (int i = 0; i < canChooseUrlList.size(); i++)
        {
            String sdkUrl = canChooseUrlList.get(i);

            //将本task作为锁对象
            NPSynSDKEnterAndCheckTask curTask = this;

            list[i] = ALProcess.CreateProcess("url_check_process");
            list[i].addResDelegateProcess(action ->
            {
                long startTimeMs = CommonFunc.getNowTimeMS();

                //发送请求到后台
                HttpAsyncClient.startHttpPostFromData(sdkUrl + sdkLoginUrl, NPSDKEnterUtil.buildPostParam(_m_sClientIp, _m_sToken, _m_sAccName), new _AResponseHandler()
                {
                    @Override
                    public void onComplete(Header[] _headers, int _code, String _response)
                    {
                        _m_mutex.lock();
                        try{
                            //输出日志用于排查源站速度
                            CommLog.info("NPSynSDKEnterAndCheckTask receive post response cost {}ms url:{} httpCode:{}", CommonFunc.getNowTimeMS() - startTimeMs, sdkUrl, _code);

                            //这里的_code是http报错，这里不代表收到了正常反馈
                            if (_code != 200)
                            {
                                //此处不做处理直接返回
                                action.dealAction(true);
                                return;
                            }

                            //如果已经处理了结果则不处理
                            if (_m_bHasDone)
                            {
                                action.dealAction(true);
                                return;
                            }

                            //设置结果
                            _m_bHasDone = true;

                            //后续的是实际平台报错
                            JsonObject jsonObject = CommonFunc.string2JsonObject(_response);
                            int responseCode = JsonUtil.getInt(jsonObject, "code");
                            //后台实际逻辑返回的200表示成功，其他情况都是失败。
                            if (responseCode != 200)
                            {
                                CommLog.error("NPSynSDKEnterAndCheckTask loginFail response:{} accName:{} token:{}", _response, _m_sAccName, _m_sToken);
                                _m_cCommiter.commitFailRes(LoginErr.LOGIN_SDK_RESPONSE_ERR.getCode());
                                _m_overDealer.onLoginFail(_code);
                                action.dealAction(true);
                                return;
                            }

                            //此处进入正式的登录流程
                            NPSDKEnterUtil.dealAccName(_m_sAccName, _m_cCommiter);

                            //设置成功的最快链接
                            NPSDKLoginMgr.getInstance().setFastURLSuc(sdkUrl);

                            _m_overDealer.onLoginSuc();
                            action.dealAction(true);
                        }finally
                        {
                            _m_mutex.unlock();
                        }
                    }

                    @Override
                    public void onFailed(Exception e)
                    {
                        e.printStackTrace();
                        //失败情况这里不做额外处理
                        action.dealAction(true);
                    }
                });
            }, "url_check_process_" + i);
        }

        ALProcess mainProcess = ALProcess.CreateProcess("sdk_enter_check_process");
        mainProcess.addMultiProcess(list);
        mainProcess.dealProcess(new _IEZProcessMonitorNoTimeOut()
        {
            @Override
            public void onRootProecssStop()
            {
                System.out.println("sdk_enter_check_process on Root Process Stop");
            }

            @Override
            public void onRootProecssSuc()
            {
                //如果还没成功则直接返回失败
                if (!_m_bHasDone)
                {
                    _m_cCommiter.commitFailRes(LoginErr.LOGIN_SDK_ENTER_SERVICE_UNAVAILABLE.getCode());

                    //如果没有任何一个连接响应，则钉钉预警
                    NPLoginCheckServer.getInstance().getDDAlert().err("NPSynSDKEnterAndCheckTask fail", "sdk url check task fail, all url no response!");
                }
            }
        });
    }
}
