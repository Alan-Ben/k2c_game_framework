package NPUSServer.NPUserMsgDispather.p004_PlayerOp;

import Common.GraveObj.GraveObj_Record;
import GC2GS.p004_PlayerOp.GC2GS_004_043_ReqGraveCelebrate;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.GraveErr;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Pair.WCGPairLong;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_GRAVE_CELEBRATE;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.PlayerBuffComp.PlayerBuffInfo;
import NPUSServer.NPUSUserMgr.UserComp.PlayerFixedCdComp.PlayerFixedCD;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;

public class MsgDealer_GC2GS_004_043_ReqGraveCelebrate extends NPUserMsgDealer<GC2GS_004_043_ReqGraveCelebrate>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_004_043_ReqGraveCelebrate _msg)
    {
    	//获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();
        
        //随机获取膜拜数据
        GraveObj_Record celeRecord = getUSServer().getGraveMgr().getGraveRecordMgr().lookupRnd();
        if(null == celeRecord)
        {
        	_commiter.commitFailRes(GraveErr.GRAVE_RECORD_NOT_FOUND.getCode());
        	return;
        }
        
        //检查每日膜拜次数
        long fixCdId = RefGeneral.Ref().grave_celebrate_fixed_cd_id;
        PlayerFixedCD fixedCD = userData.getFixedCdComponent().lookupByRefId(fixCdId);
        if(null == fixedCD || fixedCD.getCount() <= 0)
        {
        	_commiter.commitFailRes(GraveErr.GRAVE_CELE_TODAY_ERROR.getCode());
        	return;
        }
        
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.GRAVE_CELEBRATE);
        if (!userData.getFixedCdComponent().spendItem(fixCdId, 1, context))
        {
        	_commiter.commitFailRes(CommErr.CONSUME_FAIL.getCode());
        	return;
        }
        
        //获取领取次数
        userData.gainReward(RefGeneral.Ref().grave_celebrate_reward_id, context);
        
        //获取随机buff
        long buffId = 0;
        WCGPairLong buffObj = RefGeneral.Ref().grave_buff_list.rand();
        if(null != buffObj)
        {
        	//计算buff持续时间
        	long nowTimeMs = CommonFunc.getNowTimeMS();
        	long buffEndTimeMs = RefGeneral.Ref().graveBuffEndTimeObj.getNextFreshTimeTagMS(nowTimeMs);
        	int secs = (int) Math.max(0, ((buffEndTimeMs - nowTimeMs) / 1000)) ;
        	//增加buff
        	PlayerBuffInfo buffInfo = userData.getBuffComponent().ensureBuff(buffObj.first(), (int) buffObj.second(), secs, true, context);
        	if(null != buffInfo)
        	{
        		buffId = buffInfo.getBuffId();
        	}
        }
        
        _commiter.commitSucRes(US2GCWriter_004_PlayerOp.make_043_RetGraveCelebrate(celeRecord.getCid(), buffId, context));

        userData.onLogicEvent(new Event_P_GRAVE_CELEBRATE(context));
    }
}
