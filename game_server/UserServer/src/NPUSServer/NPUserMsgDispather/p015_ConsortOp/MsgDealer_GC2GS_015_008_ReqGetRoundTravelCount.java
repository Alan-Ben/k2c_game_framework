package  NPUSServer.NPUserMsgDispather.p015_ConsortOp;
import GC2GS.p015_ConsortOp.GC2GS_015_008_ReqGetRoundTravelCount;
import NPCommon.ErrMain.CommErr;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortTravel.ConsortTravelInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_015_ConsortOp;
public class  MsgDealer_GC2GS_015_008_ReqGetRoundTravelCount extends NPUserMsgDealer<GC2GS_015_008_ReqGetRoundTravelCount>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_015_008_ReqGetRoundTravelCount _msg)
    {
    	NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        ConsortTravelInfo travel = userData.getConsortComponent().getTravelMgr().lookup(_msg.getConsortTravelId());
        if(null == travel)
        {
        	_commiter.commitFailRes(CommErr.DATA_STATE_ERR.getCode());
        	return;
        }
        
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.CONSORT_TRAVEL_REFRESH);
        userData.getConsortComponent().getTravelMgr().refreshGemCallCount(context);
        
        _commiter.commitSucRes(US2GCWriter_015_ConsortOp.make_008_RetGetRoundTravelCount(travel.getTravelCount()));
    }
}