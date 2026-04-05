package NPCrossGameServer.NPCrossGameCore.RequestDispather.p001_BasicOp;

import CrossGame_R.p001_BasicOp.CrossGame_R_001_001_GCForwardMsg;
import NPCrossGameServer.NPCrossGameCore.GCMsgDispather.CrossGameGCMsgDispatcher;
import NPCrossGameServer.NPCrossGameCore.MsgMgr.Commiter.CrossGameInstanceCommiter;
import NPCrossGameServer.NPCrossGameCore.RequestDispather.CrossGameRequestDealer;
import NPCrossGameServer.NPCrossGameCore.RequestDispather.NPCrossGameGCCommiter;

public class NPCrossGameDealer_001_001_GCForwardMsg extends CrossGameRequestDealer<CrossGame_R_001_001_GCForwardMsg>
{
    @Override
    protected void _dealMessage(CrossGameInstanceCommiter _commiter, CrossGame_R_001_001_GCForwardMsg _msg)
    {
        //转发至下层分发协议处理
        CrossGameGCMsgDispatcher.getInstance().DealProtocol(new NPCrossGameGCCommiter(_commiter), _msg.get_buffer_Msg());
    }
}
