package NPUSServer.NPGeneralListener.RequestDispather.p003_CommonOp;

import NPEnum.ENPGameEvent;
import NPUSServer.CollectLikesMgr.CollectLikeInfo;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPGeneralListener.RequestDispather._ABasicGeneralRequestDealer;
import NPUSServer.NPUserServer;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
import WCGCS2US_R.p003_CommOp.NP2US_R_003_019_ReqPlayerDetailLike;
import WCGCS2US_RB.p003_CommOp.NP2US_RB_003_019_RetPlayerDetailLike;

public class RequestDealer_NP2US_R_003_019_ReqPlayerDetailLike extends _ABasicGeneralRequestDealer<NP2US_R_003_019_ReqPlayerDetailLike>
{
    public RequestDealer_NP2US_R_003_019_ReqPlayerDetailLike(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2US_R_003_019_ReqPlayerDetailLike _msg)
    {
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.PLAYER_DETAIL_LIKE);
        context.setGuid(_msg.getGuid());

        CollectLikeInfo likeInfo = getUSServer().getCollectLikeMgr().ensure(_msg.getCid());
        likeInfo.incLikeCount(context);
    	
    	_committer.commitSucRes(new NP2US_RB_003_019_RetPlayerDetailLike(likeInfo.getLikeCount()));
    }
}
