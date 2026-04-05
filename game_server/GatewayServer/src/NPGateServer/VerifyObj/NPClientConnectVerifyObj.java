package NPGateServer.VerifyObj;

import ALBasicServer.ALSocket.ALBasicServerSocket;
import ALBasicServer.ALVerifyObj.ALVerifyDealerObj;
import ALBasicServer.ALVerifyObj._IALVerifyFun;
import NPGateServer.NPClientReloginMgr;
import NPGateServer.NPClientReloginMgr.ReloginSession;
import NPGateServer.NPGCCheckCodeMgr.NPGCCheckCodeInfo;
import NPGateServer.NPGCCheckCodeMgr.NPGCCheckCodeMgr;
import NPGateServer.NPGCListener.NPGSGCListener;
import NPGateServer.NPGCMsgMgr.NPGCMsgDealer;
import NPGateServer.NPGCMsgMgr.NPGCMsgMgr;
import NPGateServer.NPGateServer;
import WCGCommon.Enum.NPEnum.EClientGSLoginType;

/**************
 * 客户端连接的验证处理对象
 * @author Administrator
 *
 */
public class NPClientConnectVerifyObj implements _IALVerifyFun
{
    /*************
     * 验证处理对象
     */
    @Override
    public void verifyIdentity(ALVerifyDealerObj _dealer, ALBasicServerSocket _Socket, int _clientType, String _userName, String _userPass, String _customMsg)
    {
        try
        {
            EClientGSLoginType eLoginType = EClientGSLoginType.NORMAL;
            if (_clientType > 0 && _clientType < EClientGSLoginType.values().length)
            {
                eLoginType = EClientGSLoginType.values()[_clientType];
            }

            String uid = _userName.trim();
            if (null == uid || uid.length() <= 0)
            {
                _dealer.comfirmResult(null);
                return;
            }

            if (eLoginType == EClientGSLoginType.NORMAL)
            {
                //验证串是否一致
                NPGCCheckCodeInfo checkInfo = NPGCCheckCodeMgr.getInstance().checkUserCheckCode(uid, _userPass);
                if (null == checkInfo)
                {
                    _dealer.comfirmResult(null);
                    return;
                }

                //次数需要将之前可能存在的用户连接消息处理对象删除
                NPGCMsgMgr.getInstance().removeMsgDealer(uid);

                //创建对应的处理对象
                NPGSGCListener listener = new NPGSGCListener(checkInfo, _customMsg);
                _dealer.comfirmResult(listener);
            } else if (eLoginType == EClientGSLoginType.WITH_RELOGIN_KEY)
            {
                ReloginSession reloginSession = NPClientReloginMgr.getInstance().checkReloginSession(uid, _userPass);
                if (null == reloginSession)
                {
                    _dealer.comfirmResult(null);
                    return;
                }

                //尝试提出原先的用户
//            	WCGGSGCListener preListener = WCGGSGCMgr.getInstance().unregGCListener(uid);
//            	if(null != preListener)
//            	{
//            	    preListener.logout();
//            	}

                NPGCMsgDealer msgDealer = NPGCMsgMgr.getInstance().tryGetMsgDealer(reloginSession._mUid);
                //无效则进行验证失败处理
                if (!msgDealer.isEnable())
                {
                    NPGCMsgMgr.getInstance().removeMsgDealer(uid);
                    _dealer.comfirmResult(null);
                    return;
                }

                //由于用token验证不会通过plat所以这里需要补充处理增加承载对象
                NPGateServer.getInstance().addHandleUser(reloginSession._mUid);

                //创建对应的处理对象
                NPGSGCListener listener = new NPGSGCListener(reloginSession._mUid, _customMsg, reloginSession._mKey);
                _dealer.comfirmResult(listener);
            } else
            {
                _dealer.comfirmResult(null);
                return;
            }
        } catch (Exception _ex)
        {
            _ex.printStackTrace();
            _dealer.comfirmResult(null);
        }
    }
}
