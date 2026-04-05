package GameLogicServer.GeneralListener.RequestDispather.p001_BasicOp;

import GameLogicServer.GeneralListener.GeneralBasicServerListener;
import GameLogicServer.GeneralListener.RB_Writer.GOM2CD_RB_Writer_001_DataOp;
import GameLogicServer.GroupMgr.GroupInstanceInfo;
import GameLogicServer.GroupMgr.GroupInstanceMgr;
import NP2GLS_R.p001_BasicOp.NP2GLS_R_001_007_QuitInstance;
import NPCommon.Dispather.NPRequestDispatcher.NPRequestDealer;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

public class RequestDealer_NP2GLS_R_001_007_QuitInstance extends NPRequestDealer<NP2GLS_R_001_007_QuitInstance>
{
    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2GLS_R_001_007_QuitInstance _msg)
    {
        GeneralBasicServerListener listener = (GeneralBasicServerListener) _committer.getRequestDealer();

        GroupInstanceInfo instance = GroupInstanceMgr.getInstance().tryLookup(_msg.getInstanceId());
        if(null != instance)
        {
            instance.removeUsId(_msg.getUsId());
        }

        _committer.commitSucRes(GOM2CD_RB_Writer_001_DataOp.make_007_QuitInstance());
    }
}
