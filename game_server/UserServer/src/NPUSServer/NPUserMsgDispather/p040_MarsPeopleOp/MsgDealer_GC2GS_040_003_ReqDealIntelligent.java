package NPUSServer.NPUserMsgDispather.p040_MarsPeopleOp;

import GC2GS.p040_MarsPeopleOp.GC2GS_040_003_ReqDealIntelligent;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_MARS_DEAL_INTELLIGENT;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep3_PeopleComp.MarsPeopleIntelligentInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_040_MarsPeopleOp;
import USLOGDB.OptBo.Opt040003MarsPeopleDealInteBO;

public class MsgDealer_GC2GS_040_003_ReqDealIntelligent extends NPUserMsgDealer<GC2GS_040_003_ReqDealIntelligent>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_040_003_ReqDealIntelligent _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        MarsPeopleIntelligentInfo info = userData.getMarsPeopleComponent().getIntelligentMgr().lookup(_msg.getId());
        if(null == info)
        {
        	_commiter.commitFailRes(MarsErr.MARS_PEOPLE_INTELLIGENT_NOT_FOUND.getCode());
        	return;
        }

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.MARS_PEOPLE_USE_INTELLIGENT);
        Result result = info.useIntelligent(context);
        if(!result.isSucc())
        {
        	_commiter.commitFailRes(result.getCode());
        	return;
        }
        
        //通用展示
        if(!context.getCollector().isEmpty())
        {
        	_commiter.commitSucRes(context.getCollector().toProto());
        }
        
        _commiter.commitSucRes(US2GCWriter_040_MarsPeopleOp.make_003_RetDealIntelligent());
        
        //触发事件
        Event_P_MARS_DEAL_INTELLIGENT evt = new Event_P_MARS_DEAL_INTELLIGENT(context, _msg.getId());
        userData.onLogicEvent(evt);

        //日志数据
        Opt040003MarsPeopleDealInteBO optBo = new Opt040003MarsPeopleDealInteBO();
        optBo.setInteId(getUSServer().getBM(), _msg.getId());
        _commiter.getUserData().logEvent(optBo, context);
    }
}
