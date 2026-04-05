package NPGateServer.NPGSCallBack;

import ALBasicProtocolPack._IALProtocolStructure;
import ALServerLog.ALServerLog;
import NP2PS_RB.p001_BasicOp.NP2PS_RB_001_002_RetGSRegInfo;
import NPGateServer.NPGateServer;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;

/****************
 * 请求进入key的返回回调处理对象
 * @author Administrator
 *
 */
public class NPGS2PS_RB_Callback_RegGSServer implements _IWCGCallbackDealer
{
    @Override
    public _IALProtocolStructure createProtocolObj()
    {
        return new NP2PS_RB_001_002_RetGSRegInfo();
    }

    @Override
    public void dealFail(int _errCode)
    {
        //输出日志
        ALServerLog.Sys("WCG Gate Server Reg To Plat Error! - " + _errCode);
    }

    @Override
    public void dealSuc(_IALProtocolStructure _protocol)
    {
        NP2PS_RB_001_002_RetGSRegInfo protocolObj = (NP2PS_RB_001_002_RetGSRegInfo) _protocol;

        //初始化区域标记
        NPGateServer.getInstance().initAreaIdx(protocolObj.getAreaTagIdx());

    }

}
