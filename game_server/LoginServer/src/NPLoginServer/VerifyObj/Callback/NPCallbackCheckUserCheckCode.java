package NPLoginServer.VerifyObj.Callback;

import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALVerifyObj.ALVerifyDealerObj;
import NP2CS_RB.np_p002_serverInfoOp.NP2CS_RB_002_006_RetUSFreezeInfo;
import NP2LCS_RB.p001_BasicOp.NP2LCS_RB_001_001_RetAccInfo;
import NPLoginServer.NPGCListener.NPLSGCListener;
import NPLoginServer.NPLoginServer;
import NPServerProtocolWriter.NP2CS.Request.NP2CS_R_Writer_002_ServerInfoOp;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;
import com.google.gson.JsonObject;

/*****************
 * 检测用户登录验证串的验证处理回调对象
 * @author Administrator
 *
 */
public class NPCallbackCheckUserCheckCode implements _IWCGCallbackDealer
{
    /**
     * 验证处理的结果提交对象
     */
    private ALVerifyDealerObj _m_dealer;

    public NPCallbackCheckUserCheckCode(ALVerifyDealerObj _dealer)
    {
        _m_dealer = _dealer;
    }

    @Override
    public _IALProtocolStructure createProtocolObj()
    {
        return new NP2LCS_RB_001_001_RetAccInfo();
    }

    @Override
    public void dealFail(int _errCode)
    {
        _m_dealer.comfirmResult(null);
    }

    @Override
    public void dealSuc(_IALProtocolStructure _msg)
    {
        NP2LCS_RB_001_001_RetAccInfo msg = (NP2LCS_RB_001_001_RetAccInfo) _msg;
        if (null == msg || !msg.getRes())
        {
            _m_dealer.comfirmResult(null);
            return;
        }
        //查询封禁情况

        NPLoginServer.getInstance().sendRequestToBSServer(EServerType.SINGLE.ordinal(),
                ENPSingleServerType.COMMON.ordinal(),
                NP2CS_R_Writer_002_ServerInfoOp.make_006_ReqUSFreezeInfo(((NP2LCS_RB_001_001_RetAccInfo) _msg).getUid())
                , new _IWCGCallbackDealer()
                {
                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2CS_RB_002_006_RetUSFreezeInfo();
                    }

                    @Override
                    public void dealSuc(_IALProtocolStructure _proto)
                    {
                        NP2CS_RB_002_006_RetUSFreezeInfo proto = (NP2CS_RB_002_006_RetUSFreezeInfo) _proto;
                        if (proto.getIsFreeze())
                        {
                            JsonObject js = new JsonObject();
                            js.addProperty("errCode", 0);
                            js.addProperty("freezeTime", proto.getFreezeTimeMs());
                            js.addProperty("isFreeze", proto.getIsFreeze());
                            //TODO：这里处理是有问题的，当listener没有建立时，customMsg无法发回，只是提示信息，这一版不处理。
                            _m_dealer.comfirmResult(null, js.toString());
                            return;
                        }
                        _m_dealer.comfirmResult(new NPLSGCListener(msg.getUid(), proto.getInWhitelist()), msg.getChkKey());
                    }

                    @Override
                    public void dealFail(int i)
                    {
                        JsonObject js = new JsonObject();
                        js.addProperty("errCode", i);
                        js.addProperty("freezeTime", 0);
                        js.addProperty("isFreeze", false);
                        //TODO：这里处理是有问题的，当listener没有建立时，customMsg无法发回，只是提示信息，这一版不处理。
                        _m_dealer.comfirmResult(null, js.toString());
                    }
                });
    }

}
