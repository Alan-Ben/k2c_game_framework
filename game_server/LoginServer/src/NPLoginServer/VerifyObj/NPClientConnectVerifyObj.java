package NPLoginServer.VerifyObj;

import ALBasicServer.ALSocket.ALBasicServerSocket;
import ALBasicServer.ALVerifyObj.ALVerifyDealerObj;
import ALBasicServer.ALVerifyObj._IALVerifyFun;
import ALServerLog.ALServerLog;
import NPCommon.Log.CommLog;
import NPCommon.Util.JsonUtil;
import NPEnum.ENPLSGameClientType;
import NPLoginServer.LoginServerConf;
import NPLoginServer.NPLoginServer;
import NPLoginServer.VerifyObj.Callback.NPCallbackCheckUserCheckCode;
import NPServerProtocolWriter.NP2LCS.Request.NP2LCS_R_Writer_001_BasicOp;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EProfileEvent;
import WCGCommon.Enum.NPEnum.EServerType;
import WCGCommon.Profile.ProfileMgr;
import com.google.gson.JsonParser;

import java.util.regex.Pattern;

/**************
 * 客户端连接的验证处理对象
 *
 * @author Administrator
 *
 */
public class NPClientConnectVerifyObj implements _IALVerifyFun
{
    /*************
     * 验证处理对象
     */
    @Override
    public void verifyIdentity(final ALVerifyDealerObj _dealer, ALBasicServerSocket _Socket, int _clientType, final String _userName, String _userPass, String _customMsg)
    {
        long serialPro = ProfileMgr.getInstance().beginProfile(EProfileEvent.eProfile_verifyIdentity);
        final String pid = _userName.trim();
        final String token = _userPass.trim();

        if (!NPLoginServer.getInstance().isServerReady())
        {
            CommLog.error("server not ready while player login pid:" + pid);
            _dealer.comfirmResult(null);
        }

        if (pid.length() >= 32)
        {
            CommLog.error("login name {} too long!" + pid);
            _dealer.comfirmResult(null);
        }
        if (token.length() > 256)
        {
            CommLog.error("_userPass {} too long!", _userPass);
            _dealer.comfirmResult(null);
        }

        if (_clientType < 0 || _clientType >= ENPLSGameClientType.values().length)
        {
            CommLog.error("invalid client type: {} !", _clientType);
            _dealer.comfirmResult(null);
        }

        //登陆类型
        ENPLSGameClientType clientType = ENPLSGameClientType.values()[_clientType];

        //禁止登陆
        if (NPLoginServer.getInstance().isForbidenLogin())
        {
            if (ENPLSGameClientType.USER == clientType
                    || ENPLSGameClientType.USER_CHECK_CODE == clientType
                    || ENPLSGameClientType.VISITORS == clientType
            )
            {
                _dealer.comfirmResult(null);
                return;
            }
        }

        //逻辑处理
        switch (clientType)
        {
            case USER:
            {
                userLogin(_dealer, pid, token);
                break;
            }
            case USER_CHECK_CODE:
            {
                checkCodeLogin(_dealer, pid, token);
                break;
            }
            case CHEAT:
            {
                //不允许作弊登陆则直接处理
                if (!LoginServerConf.getInstance().getCheatEnterEnable())
                {
                    _dealer.comfirmResult(null);
                    return;
                }

                //判断是否纯数字，避免非白名单对象使用ID侵入他人帐号
                cheatLogin(_dealer, pid, token);
                break;
            }
            case SDK:
            {
                //约定，以这种方式，customMsg里存放客户端IP
                JsonParser parser = new JsonParser();
                //解析json获取client_ip
                String client_ip = JsonUtil.getString(parser.parse(_customMsg).getAsJsonObject(), "clientIp", "");
                sdkLogin(_dealer, pid, token, client_ip);
                break;
            }
            default:
            {
                ALServerLog.Error("Unknow Client Type: " + _clientType);
                _dealer.comfirmResult(null);
            }
        }

        ProfileMgr.getInstance().endProfile(serialPro);
    }

    /**
     * 通过SDK登录游戏
     * @param _dealer   请求验证
     * @param _accName  后台的用户名
     * @param _token    校验token
     * @param _clientIp
     */
    private void sdkLogin(final ALVerifyDealerObj _dealer, final String _accName, final String _token, String _clientIp)
    {
        //向 LoginChecker 服务器校验用户名和token
        NPLoginServer.getInstance().sendRequestToBSServer(EServerType.SINGLE.ordinal(), ENPSingleServerType.LOGIN_CHECK.ordinal()
                , NP2LCS_R_Writer_001_BasicOp.make_004_ReqSDKCheck(_accName, _token, _clientIp)
                , new NPCallbackCheckUserCheckCode(_dealer));
    }

    //判断是否整数
    public static boolean isNumeric(String str)
    {
        Pattern pattern = Pattern.compile("[0-9]*");
        return pattern.matcher(str).matches();
    }

    /*************
     * 正常的用户名登录
     *
     * @param _dealer
     * @param _userName
     * @param pid
     * @param token
     * @param jsonExtends
     */
    public void userLogin(final ALVerifyDealerObj _dealer, final String pid, final String token)
    {
        if (token.isEmpty())
        {
            CommLog.error("Normal Login,_userPass(token) should not be empty");
            _dealer.comfirmResult(null);
            return;
        } else
        {
            //向Account服务器注册或获取玩家uid
            NPLoginServer.getInstance().sendRequestToBSServer(EServerType.SINGLE.ordinal(), ENPSingleServerType.LOGIN_CHECK.ordinal()
                    , NP2LCS_R_Writer_001_BasicOp.make_001_ReqAccInfo(pid, token)
                    , new NPCallbackCheckUserCheckCode(_dealer));
        }
    }

    /*************
     * 用户使用验证串登录
     *
     * @param _dealer
     * @param pid
     * @param token
     * @param jsonExtends
     */
    public void checkCodeLogin(final ALVerifyDealerObj _dealer, final String pid, final String token)
    {
        //使用验证串登录的使用其他验证方式
        NPLoginServer.getInstance().sendRequestToBSServer(EServerType.SINGLE.ordinal()
                , ENPSingleServerType.LOGIN_CHECK.ordinal()
                , NP2LCS_R_Writer_001_BasicOp.make_002_ReqAccInfoByChkKey(String.valueOf(pid), token)
                , new NPCallbackCheckUserCheckCode(_dealer));
    }

    /*************
     * 用户作弊登陆
     *
     * @param _dealer
     * @param pid
     */
    public void cheatLogin(final ALVerifyDealerObj _dealer, final String pid, final String _token)
    {
        //允许作弊本用户类型才可登录
        NPLoginServer.getInstance().sendRequestToBSServer(EServerType.SINGLE.ordinal()
                , ENPSingleServerType.LOGIN_CHECK.ordinal()
                , NP2LCS_R_Writer_001_BasicOp.make_003_ReqCheatUidInfo(pid, _token)
                , new NPCallbackCheckUserCheckCode(_dealer));
    }
}
