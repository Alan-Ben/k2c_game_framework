package NPMonitorServer;

import ALBasicCommon.ALBasicCommonFun;
import ALBasicProtocolPack.ALProtocolCommon;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPMonitorClientType;
import WCGBasicMonitorServer.VerifyObj._IWCGBasicExternalListenerVerifyInterface;

/**************
 * 对外部链接进行验证合法性的处理类
 * @author mj
 *
 */
public class NPMonitorVerifyObj implements _IWCGBasicExternalListenerVerifyInterface
{
    /******************
     * 验证用户信息，并返回是否通过验证
     * @param _clientType
     * @param _userName
     * @param _userPass
     * @param _customMsg
     * @return
     */
    public boolean checkUserInfo(int _clientType, String _userName, String _userPass, String _customMsg)
    {
        ENPMonitorClientType clientType = ENPMonitorClientType.ENPMonitorClientType_FromInt(_clientType);
        if (clientType != ENPMonitorClientType.PHP)
        {
            CommLog.error("can not verify php client type! [type:{}]", _clientType);
            //验证失效
            return false;
        }

        // 检查传入参数，先获取并验证time()，然后以time()为key检查userName，userPassword
        byte[] _verfyTime = CommonFunc.decry(MonitorServerConf.getInstance().getPhpClientKey().getBytes(), ALBasicCommonFun.getHexBytes(_customMsg));
        // 获取userName，userPassword
        byte[] _verfyUserNameByte = CommonFunc.decry(_verfyTime, ALBasicCommonFun.getHexBytes(_userName));
        String _verfyUserName = ALProtocolCommon.GetStringFromBuf(_verfyUserNameByte);
        byte[] _verfyUserPasswordByte = CommonFunc.decry(_verfyTime, ALBasicCommonFun.getHexBytes(_userPass));
        String _verfyUserPassword = ALProtocolCommon.GetStringFromBuf(_verfyUserPasswordByte);

        if (!_verfyUserName.equals(MonitorServerConf.getInstance().getPhpClientUser())
                || !_verfyUserPassword.equals(MonitorServerConf.getInstance().getPhpClientPassword()))
        {
            CommLog.error("can not verfy php client user/password! [user:{}, password:{}]", _userName, _userPass);
            //验证失效
            return false;
        }
        //所有都返回验证成功
        return true;
    }
}
