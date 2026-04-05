package GameLogicServer.GeneralListener.RequestDispather.p001_BasicOp;

import GameLogicServer.GeneralListener.RequestDispather.ActivityMsgDispatcher.ActivityMsgCommiter;
import GameLogicServer.GroupMgr.GroupInstanceInfo;
import GameLogicServer.GroupMgr.GroupInstanceMgr;
import NP2GLS_R.p001_BasicOp.NP2GLS_R_001_020_DealMsg;
import NPCommon.Dispather.NPRequestDispatcher.NPRequestDealer;
import NPCommon.ErrMain.CommErr;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

public class RequestDealer_NP2GLS_R_001_020_DealMsg extends NPRequestDealer<NP2GLS_R_001_020_DealMsg>
{
    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2GLS_R_001_020_DealMsg _msg)
    {
        GroupInstanceInfo instance = GroupInstanceMgr.getInstance().lookup(_msg.getInstanceId());
        if (null == instance)
        {
            _committer.commitFailRes(CommErr.OBJ_ERR.getCode());
            return;
        }

        //调用实例对象处理消息
        instance.dealMsg(_msg.getCid(), _msg.getPlayerGroupId(),
                new ActivityMsgCommiter(_committer, _msg.getCid(), instance.getActivity(), _msg.get_buffer_AddInfo()),
                _msg.getMsg(), _msg.getAddInfo());
    }
}
