package NPUSServer.NPUserMsgDispather.p040_MarsPeopleOp;

import GC2GS.p040_MarsPeopleOp.GC2GS_040_002_ReqPeopleConfirmImmigrant;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep3_PeopleComp.CmdConfirmResult;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_040_MarsPeopleOp;

public class MsgDealer_GC2GS_040_002_ReqPeopleConfirmImmigrant extends NPUserMsgDealer<GC2GS_040_002_ReqPeopleConfirmImmigrant>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_040_002_ReqPeopleConfirmImmigrant _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.MARS_PEOPLE_CONFIRM_IMMIGRANT);
        CmdConfirmResult result = userData.getMarsPeopleComponent().getImmigrantInfo().confirm(context);
        if(!result.result.isSucc())
        {
        	_commiter.commitFailRes(result.result.getCode());
        	return;
        }
        
        _commiter.commitSucRes(US2GCWriter_040_MarsPeopleOp.make_002_RetPeopleConfirmImmigrant(result.num, context));
    }
}
