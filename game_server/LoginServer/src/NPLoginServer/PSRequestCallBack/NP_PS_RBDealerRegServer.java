package NPLoginServer.PSRequestCallBack;

import ALBasicProtocolPack._IALProtocolStructure;
import ALServerLog.ALServerLog;
import NP2PS_RB.p001_BasicOp.NP2PS_RB_001_004_RetLSRegInfo;
import NPLoginServer.NPLoginServer;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;

/****************
 * 请求进入key的返回回调处理对象
 *
 * @author Administrator
 *
 */
public class NP_PS_RBDealerRegServer implements _IWCGCallbackDealer
{
    @Override
    public _IALProtocolStructure createProtocolObj()
    {
        return new NP2PS_RB_001_004_RetLSRegInfo();
    }

    @Override
    public void dealFail(int _errCode)
    {
        // 输出日志
        ALServerLog.Sys("WCG Login Server Reg To Plat Error! - " + _errCode);
    }

    @Override
    public void dealSuc(_IALProtocolStructure _protocol)
    {
        NP2PS_RB_001_004_RetLSRegInfo protocolObj = (NP2PS_RB_001_004_RetLSRegInfo) _protocol;

        // 初始化区域标记
        NPLoginServer.getInstance().initAreaIdx(protocolObj.getAreaTagIdx());
    }

}
