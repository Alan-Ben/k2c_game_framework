package NPHttpServer.NPGeneralListener.RequestDispather.p001_BasicOp;

import Common.ServerObj.ServerObj_WhiteAccList;
import NP2HS_R.p001_HSOp.NP2HS_R_001_007_ReqWhiteAccList;
import NPCommon.Dispather.NPRequestDispatcher.NPRequestDealer;
import NPHttpServer.NPGeneralListener.Writer.NP2HS_RB_Writer_001_PSOp;
import NPHttpServer.NPWhiteAccMgr.NPWhiteAccMgr;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

import java.util.List;

/**
 * 请求获取白名单列表处理器
 *
 * 执行流程：
 * 1. 从NPWhiteAccMgr获取完整白名单列表
 * 2. 构造ServerObj_WhiteAccList对象
 * 3. 返回给CommonServer
 */
public class NP2HS_R_001_007_ReqWhiteAccList_Handler extends NPRequestDealer<NP2HS_R_001_007_ReqWhiteAccList>
{
    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _receiver, NP2HS_R_001_007_ReqWhiteAccList _msg)
    {
        // 获取白名单列表
        List<String> accList = NPWhiteAccMgr.getInstance().getAllAccList();

        // 构造响应对象
        ServerObj_WhiteAccList whiteAccList = new ServerObj_WhiteAccList();
        if (accList != null && !accList.isEmpty())
        {
            whiteAccList.getAccList().addAll(accList);
        }

        // 返回成功响应
        _receiver.commitSucRes(NP2HS_RB_Writer_001_PSOp.make_007_RetWhiteAccList(whiteAccList));
    }
}
