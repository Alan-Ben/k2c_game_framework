package GameLogicServer.GeneralListener.RequestDispather.p001_BasicOp;

import GameLogicServer.GeneralListener.RB_Writer.GOM2CD_RB_Writer_001_DataOp;
import GameLogicServer.GroupMgr.GroupInstanceInfo;
import GameLogicServer.GroupMgr.GroupInstanceMgr;
import NP2GLS_R.p001_BasicOp.NP2GLS_R_001_005_RegGroupInstance;
import NPCommon.Dispather.NPRequestDispatcher.NPRequestDealer;
import NPCommon.ErrMain.CommErr;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

public class RequestDealer_NP2GLS_R_001_005_RegGroupInstance extends NPRequestDealer<NP2GLS_R_001_005_RegGroupInstance>
{
    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2GLS_R_001_005_RegGroupInstance _msg)
    {
        GroupInstanceInfo instance = GroupInstanceMgr.getInstance().create(_msg.getGroupId(), _msg.getActivityId(), _msg.get_buffer_AddInfo());
        if(null == instance)
        {
            _committer.commitFailRes(CommErr.OBJ_ERR.getCode());
            return;
        }

        _committer.commitSucRes(GOM2CD_RB_Writer_001_DataOp.make_005_RegGroupInstance(instance.getInstanceId()));
    }
}
