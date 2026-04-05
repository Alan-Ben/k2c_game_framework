package GameLogicServer.GeneralListener.RequestDispather.p001_BasicOp;
import GameLogicServer.GeneralListener.RB_Writer.GOM2CD_RB_Writer_001_DataOp;
import GameLogicServer.GroupMgr.GroupInstanceInfo;
import GameLogicServer.GroupMgr.GroupInstanceMgr;
import NP2GLS_R.p001_BasicOp.NP2GLS_R_001_008_RegUs;
import NPCommon.Dispather.NPRequestDispatcher.NPRequestDealer;
import NPCommon.ErrMain.CommErr;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
/** 处理 US 向 GLS 注册自身的请求，对应 _initGameLogicDealer 流程 */
public class RequestDealer_NP2GLS_R_001_008_RegUs extends NPRequestDealer<NP2GLS_R_001_008_RegUs>
{
    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2GLS_R_001_008_RegUs _msg)
    {
        GroupInstanceInfo instance = GroupInstanceMgr.getInstance().tryLookup(_msg.getInstanceId());
        if (null == instance)
        {
            _committer.commitFailRes(CommErr.OBJ_ERR.getCode());
            return;
        }
        // 将 US 服务器 ID 添加到实例管理集合中
        instance.addUsId(_msg.getUsId());
        _committer.commitSucRes(GOM2CD_RB_Writer_001_DataOp.make_008_RegUs());
    }
}
