package NPUSServer.NPUserMsgDispather.p040_MarsPeopleOp;

import GC2GS.p040_MarsPeopleOp.GC2GS_040_004_ReqDealRewardHelp;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPEnum.ENpRewardShowType;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep3_PeopleComp.MarsPeopleHelpInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_040_MarsPeopleOp;
import USLOGDB.OptBo.Opt040004MarsPeopleDealDrawHelpBO;

public class MsgDealer_GC2GS_040_004_ReqDealRewardHelp extends NPUserMsgDealer<GC2GS_040_004_ReqDealRewardHelp>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_040_004_ReqDealRewardHelp _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        MarsPeopleHelpInfo info = userData.getMarsPeopleComponent().getHelpMgr().lookup(_msg.getId());
        if(null == info)
        {
        	_commiter.commitFailRes(MarsErr.MARS_PEOPLE_HELP_NOT_FOUND.getCode());
        	return;
        }
        
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.MARS_PEOPLE_DEAL_HELP_REWARD);
        Result result = info.dealReward(context);
        if(!result.isSucc())
        {
        	_commiter.commitFailRes(result.getCode());
        	return;
        }
        
        //弹出tips
        if(!context.getCollector().isEmpty())
        {
        	userData.sendMsgToGC(context.getCollector().toProto(ENpRewardShowType.TIP));
        }
        
        _commiter.commitSucRes(US2GCWriter_040_MarsPeopleOp.make_004_RetDealRewardHelp(context));

        //日志数据
        Opt040004MarsPeopleDealDrawHelpBO optBo = new Opt040004MarsPeopleDealDrawHelpBO();
        optBo.setHelpId(getUSServer().getBM(), _msg.getId());
        _commiter.getUserData().logEvent(optBo, context);
    }
}
