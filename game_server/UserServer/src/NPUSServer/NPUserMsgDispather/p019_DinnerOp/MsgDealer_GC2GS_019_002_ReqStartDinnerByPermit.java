package  NPUSServer.NPUserMsgDispather.p019_DinnerOp;
import GC2GS.p019_DinnerOp.GC2GS_019_002_ReqStartDinnerByPermit;
import MJLog.MJEventLog;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.DinnerErr;
import NPEnum.ENPGameEvent;
import NPEnum.ENPPlayerRecordParam;
import NPGameRes.Refs.Dinner.RefDinnerType;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.DinnerMgr.DinnerInfo;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.DinnerComp.DinnerPermitInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_019_DinnerOp;
public class  MsgDealer_GC2GS_019_002_ReqStartDinnerByPermit extends NPUserMsgDealer<GC2GS_019_002_ReqStartDinnerByPermit>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_019_002_ReqStartDinnerByPermit _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        //检查是否开启宴会
        if(null != getUSServer().getDinnerPool().lookupByOwnerCid(userData.getCid()))
        {
        	_commiter.commitFailRes(DinnerErr.DINNER_OWNER_REWARD.getCode());
        	return;
        }
        //检查是否开启宴会
        if(null != getUSServer().getDinnerPool().lookupByOwnerCid(userData.getCid()))
        {
        	_commiter.commitFailRes(DinnerErr.DINNER_STARTED.getCode());
            return;
        }
        
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.DINNER_START);
        
        //获取许可信息，许可信息获取后立即销毁
        DinnerPermitInfo permit = userData.getDinnerComponent().getAndDelPermit(_msg.getPermitInstanceId(), context);
        if(null == permit)
        {
        	_commiter.commitFailRes(DinnerErr.DINNER_PERMIT_NOT_FOUND.getCode());
        	return;
        }
        if(null == permit.getRef())
        {
        	_commiter.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
        	return;
        }
        
        //检查对应的宴会配置数据
        RefDinnerType dinnerRef = permit.getRef().dinnerRef;
        if(null == dinnerRef)
        {
        	_commiter.commitFailRes(CommErr.REF_ERROR.getCode());
        	return;
        }
        
        //检查消耗物品
        if(!userData.hasCostItemList(dinnerRef.open_cost))
        {
            _commiter.commitFailRes(CommErr.ITEM_NOT_ENOUGH.getCode());
            return;
        }
        
        //消耗物品
        if(!userData.spendCostItemList(dinnerRef.open_cost, context))
        {
            _commiter.commitFailRes(CommErr.CONSUME_FAIL.getCode());
            return;
        }
        
        //创建宴会
        DinnerInfo info = getUSServer().getDinnerPool().start(userData, dinnerRef, permit.getType(), permit.getTypeId(), context);
        //回包数据
        _commiter.commitSucRes(US2GCWriter_019_DinnerOp.make_002_RetStartDinnerByPermit(info));
        
		//增加开宴计数
		userData.getRecordComponent().addRecord(ENPPlayerRecordParam.START_DINNER_COUNT, 1, context);

        MJEventLog.logParty(userData, dinnerRef.Id(), 2);
    }
}