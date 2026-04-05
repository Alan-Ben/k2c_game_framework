package NPHttpServer.NPGeneralListener.RequestDispather.p001_BasicOp;

import NP2HS_R.p001_HSOp.NP2HS_R_001_004_ReqCheckIsInWhiteList;
import NPCommon.Dispather.NPRequestDispatcher.NPRequestDealer;
import NPHttpServer.NPGeneralListener.Writer.NP2HS_RB_Writer_001_PSOp;
import NPHttpServer.NPWhiteAccMgr.NPWhiteAccMgr;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

/**
 * @description: 请求检查玩家是否在白名单内
 * @author: ricci
 * @date: 2022-04-07 16:13:13
 */
public class NP2HS_R_001_004_ReqCheckIsInWhiteList_Handler extends NPRequestDealer<NP2HS_R_001_004_ReqCheckIsInWhiteList>
{
    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _receiver, NP2HS_R_001_004_ReqCheckIsInWhiteList _msg)
    {
        boolean isInWhiteList = NPWhiteAccMgr.getInstance().checkUidInWhiteList(_msg.getAccName());
        _receiver.commitSucRes(NP2HS_RB_Writer_001_PSOp.make_004_RetCheckIsInWhiteList(isInWhiteList));
    }
}
