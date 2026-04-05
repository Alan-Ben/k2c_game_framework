package NPLoginCheckServer.NPSDKLoginMgr;

import ALBasicServer.ALTask._IALAsynCallBackTask;
import LCSDB.Bo.AccountBO;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.LoginErr;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPLoginCheckServer.LoginCheckServerConf;
import NPLoginCheckServer.NPAccInfoMgr.NPAccInfoMgr;
import NPLoginCheckServer.NPLoginCheckServer;
import NPServerProtocolWriter.NP2LCS.RequestBack.NP2LCS_RB_Writer_001_BasicOp;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
import WCGCommon.Security.MD5;
import org.apache.http.message.BasicNameValuePair;

import java.util.ArrayList;
import java.util.Comparator;
import java.util.Locale;

public class NPSDKEnterUtil
{
    /**
     * 创建token校验的请求体，参考文档
     * http://public-api.dreamplusgames.com/web/#/31/727
     * @return ArrayList<BasicNameValuePair>
     */
    public static ArrayList<BasicNameValuePair> buildPostParam(String _clientIp, String _token, String _AccName)
    {
        //构造签名
        ArrayList<BasicNameValuePair> postParamList = new ArrayList<>();
        postParamList.add(new BasicNameValuePair("app_id", LoginCheckServerConf.getInstance().getStrAppId()));
        postParamList.add(new BasicNameValuePair("ip", _clientIp));
        postParamList.add(new BasicNameValuePair("timestamp", String.valueOf(CommonFunc.getNowTimeSec())));
        postParamList.add(new BasicNameValuePair("ext", ""));
        postParamList.add(new BasicNameValuePair("token", _token));
        postParamList.add(new BasicNameValuePair("user_id", _AccName));
        //按参数名升序排列
        postParamList.sort(Comparator.comparing(BasicNameValuePair::getName));

        StringBuilder totalStr = new StringBuilder();
        for (BasicNameValuePair param : postParamList)
        {
            totalStr.append(param.getValue());
        }
        /*
         * 构造签名
         * 签名说明：
         *
         * 所有请求参数排除sign，按参数名进行升序排序，对应的参数值（如果参数值为数组对象，请使用json格式）连接成字符串，paramsString
         * md5(‘MJ’ + md5( paramsString + 密钥 ));
         */
        String sign = MD5.md5("MJ" + MD5.md5(totalStr + LoginCheckServerConf.getInstance().getSDKKey()).toLowerCase(Locale.ROOT))
                .toLowerCase(Locale.ROOT);
        postParamList.add(new BasicNameValuePair("sign", sign));
        return postParamList;
    }


    /**
     * 处理AccName在LoginCheckerServer上的记录，生成内部Token，返回给客户端
     * 这个函数会需要确保commiter必须要被处理掉，否则可能导致请求无法返回
     */
    public static void dealAccName(String _accName, _IWCGBasicRequestCommiter _commiter)
    {
        //先到管理对象中查询是否有对应数据，有则直接处理，无则开启数据库处理
        NPAccInfoMgr.getInstance().getAccInfoByName(_accName, new NPAccInfoMgr._ILoadAccountCallBack()
        {
            @Override
            public void onLoad(boolean isSucc, boolean _isExists, AccountBO _bo)
            {
                BM bmObj = NPLoginCheckServer.getInstance().getBM();

                if (!isSucc)
                {
                    _commiter.commitFailRes(LoginErr.LOGIN_NO_USER_INFO.getCode());
                }

                if (_isExists)
                {
                    //重新设置登录串
                    _bo.clearMark();
                    _bo.setChkKey(bmObj, CommonFunc.genRandomStr(32));
                    _bo.setLastLoginTime(bmObj, CommonFunc.getNowTimeSec());
                    _bo.saveAllMarked(bmObj);
                    //直接处理结果，返回成功
                    _commiter.commitSucRes(NP2LCS_RB_Writer_001_BasicOp.make_001_RetAccInfoSuc(_bo.getAccName(), _bo.getChkKey()));

                } else //账号不存在,创建新账号.
                {
                    AccountBO bo = NPAccInfoMgr.getInstance().getCachedAccInfoByName(_accName);
                    if (null != bo) //名字已经被注册了,此时无法新增用户数据，当做系统失败处理
                    {
                        _commiter.commitFailRes(LoginErr.LOGIN_NO_USER_INFO.getCode());
                    } else
                    {
                        // 创建临时的验证串
                        String chkKey = CommonFunc.genRandomStr(32);
                        bo = new AccountBO();
                        bo.setAccName(bmObj, _accName);
                        bo.setChkKey(bmObj, chkKey);
                        bo.setLastLoginTime(bmObj, CommonFunc.getNowTimeSec());

                        bo.insert(bmObj, new _IALAsynCallBackTask<AccountBO>()
                        {
                            @Override
                            public void dealFail()//数据库插入失败
                            {
                                _commiter.commitFailRes(LoginErr.LOGIN_CHECK_DB_UNAVAILABLE.getCode());
                            }

                            @Override
                            public void dealSuc(AccountBO _bo)
                            {
                                //放入数据管理对象
                                if (!NPAccInfoMgr.getInstance().addAccInfo(_bo))
                                {
                                    CommLog.error("NPSDKEnterUtil dealAccName addAccInfo fail, dbId:{} accName:{}", _bo.getId(), _bo.getAccName());
                                    _commiter.commitFailRes(LoginErr.LOGIN_ADD_ACC_INFO_FAIL.getCode());
                                    return;
                                }

                                //提交结果
                                _commiter.commitSucRes(NP2LCS_RB_Writer_001_BasicOp.make_001_RetAccInfoSuc(_bo.getAccName(), _bo.getChkKey()));
                            }
                        });
                    }
                }
            }
        });
    }
}
