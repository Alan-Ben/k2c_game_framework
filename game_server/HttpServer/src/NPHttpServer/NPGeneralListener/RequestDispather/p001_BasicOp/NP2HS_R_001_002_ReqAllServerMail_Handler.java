package NPHttpServer.NPGeneralListener.RequestDispather.p001_BasicOp;

import Common.NpServerObj.NpServerObj_PlatFormMail;
import NP2HS_R.p001_HSOp.NP2HS_R_001_002_ReqAllServerMail;
import NPCommon.Dispather.NPRequestDispatcher.NPRequestDealer;
import NPHttpServer.NPGeneralListener.Writer.NP2HS_RB_Writer_001_PSOp;
import NPHttpServer.NPHSAllServerMail.NPHSAllServerMailMgr;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

import java.util.ArrayList;

/**
 * @description: 请求注册新房间房间
 * @author: ricci
 * @date: 2022-04-07 16:13:13
 */
public class NP2HS_R_001_002_ReqAllServerMail_Handler extends NPRequestDealer<NP2HS_R_001_002_ReqAllServerMail>
{
    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _receiver, NP2HS_R_001_002_ReqAllServerMail _msg)
    {
        //查找符合要求的邮件列表
        ArrayList<NpServerObj_PlatFormMail> mailArrayList = new ArrayList<>();
        long maxMailDBId = NPHSAllServerMailMgr.getInstance().makeMailList(_msg.getMaxMailDbId(), _msg.getUsTypeId(), mailArrayList);

        _receiver.commitSucRes(NP2HS_RB_Writer_001_PSOp.make_002_RetAllServerMail(maxMailDBId, mailArrayList));
    }
}
