package NPUSServer.NPUserMsgDispather.p040_MarsPeopleOp;

import GC2GS.p040_MarsPeopleOp.GC2GS_040_001_ReqPeopleImmigrant;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_040_MarsPeopleOp;

public class MsgDealer_GC2GS_040_001_ReqPeopleImmigrant extends NPUserMsgDealer<GC2GS_040_001_ReqPeopleImmigrant>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_040_001_ReqPeopleImmigrant _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.MARS_PEOPLE_IMMIGRANT);
        Result result = userData.getMarsPeopleComponent().getImmigrantInfo().start(context);
        if(!result.isSucc())
        {
        	_commiter.commitFailRes(result.getCode());
        	return;
        }
        
        _commiter.commitSucRes(US2GCWriter_040_MarsPeopleOp.make_001_RetPeopleImmigrant());
    }
}
