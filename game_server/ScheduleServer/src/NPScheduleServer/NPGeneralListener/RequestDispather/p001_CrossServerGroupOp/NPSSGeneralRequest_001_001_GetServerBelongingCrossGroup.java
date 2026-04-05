package NPScheduleServer.NPGeneralListener.RequestDispather.p001_CrossServerGroupOp;

import NP2SS_R.p001_ScheduleOp.NP2SS_R_001_001_GetServerBelongingCrossGroup;
import NPCommon.Dispather.NPRequestDispatcher.NPRequestDealer;
import NPScheduleServer.CrossServerGroup.CrossServerGroupItem;
import NPScheduleServer.CrossServerGroup.CrossServerGroupMgr;
import NPScheduleServer.NPGeneralListener.RequestDispather.Writer.NP2SS_RB_Writer_001_CrossServerGroupOp;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

public class NPSSGeneralRequest_001_001_GetServerBelongingCrossGroup extends NPRequestDealer<NP2SS_R_001_001_GetServerBelongingCrossGroup>
{
    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _receiver, NP2SS_R_001_001_GetServerBelongingCrossGroup _msg)
    {
        CrossServerGroupItem groupItem = CrossServerGroupMgr.getInstance().getWorkingGroup().lookupUsBelongingGroupItem(_msg.getUsId());
        if (groupItem == null)
        {
            _receiver.commitSucRes(NP2SS_RB_Writer_001_CrossServerGroupOp.make_001_GetServerBelongingCrossGroup(false, null));
            return;
        }

        _receiver.commitSucRes(NP2SS_RB_Writer_001_CrossServerGroupOp.make_001_GetServerBelongingCrossGroup(true, groupItem.toProto()));
    }
}
