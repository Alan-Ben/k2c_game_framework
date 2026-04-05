package NPHttpServer.NPGeneralListener.RequestDispather.p001_BasicOp;

import NP2HS_R.p001_HSOp.NP2HS_R_001_005_ReqPlatformInfo;
import NPCommon.Dispather.NPRequestDispatcher.NPRequestDealer;
import NPHttpServer.HttpServerConf;
import NPHttpServer.NPGeneralListener.Writer.NP2HS_RB_Writer_001_PSOp;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

/**
 * @description: 请求平台ID信息
 * @author: ricci
 * @date: 2022-04-07 16:13:13
 */
public class NP2HS_R_001_005_ReqPlatformInfo_Handler extends NPRequestDealer<NP2HS_R_001_005_ReqPlatformInfo>
{
    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _receiver, NP2HS_R_001_005_ReqPlatformInfo _msg)
    {
        _receiver.commitSucRes(NP2HS_RB_Writer_001_PSOp.make_005_RetPlatformInfo(HttpServerConf.getInstance().getPlatformId(), HttpServerConf.getInstance().getPlatAreaId()));
    }
}
