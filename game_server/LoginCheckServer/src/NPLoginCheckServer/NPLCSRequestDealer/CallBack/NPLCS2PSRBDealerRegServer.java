package NPLoginCheckServer.NPLCSRequestDealer.CallBack;

import ALBasicProtocolPack._IALProtocolStructure;
import ALServerLog.ALServerLog;
import NP2PS_RB.p001_BasicOp.NP2PS_RB_001_003_RetLCSRegInfo;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;

/************
 * lcs服务器注册服务器的处理消息
 *
 * @author alzq.z
 * @email zhuangfan@vip.163.com
 * @time 2019年3月28日 下午8:43:19
 */
public class NPLCS2PSRBDealerRegServer implements _IWCGCallbackDealer
{
    @Override
    public _IALProtocolStructure createProtocolObj()
    {
        return new NP2PS_RB_001_003_RetLCSRegInfo();
    }

    @Override
    public void dealFail(int _errCode)
    {
        ALServerLog.Error("Reg LCS Server Error! - " + _errCode);
    }

    @Override
    public void dealSuc(_IALProtocolStructure _msg)
    {
        NP2PS_RB_001_003_RetLCSRegInfo protocol = (NP2PS_RB_001_003_RetLCSRegInfo) _msg;
        if (null == protocol)
            return;
    }

}
