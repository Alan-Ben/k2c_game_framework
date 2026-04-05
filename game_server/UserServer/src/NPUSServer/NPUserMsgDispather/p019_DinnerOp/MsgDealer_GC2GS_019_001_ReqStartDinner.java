package  NPUSServer.NPUserMsgDispather.p019_DinnerOp;
import GC2GS.p019_DinnerOp.GC2GS_019_001_ReqStartDinner;
import MJLog.MJEventLog;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.DinnerErr;
import NPEnum.ENPGameEvent;
import NPEnum.ENPPlayerRecordParam;
import NPGameRes.Refs.Dinner.RefDinnerType;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.DinnerMgr.DinnerInfo;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_019_DinnerOp;
public class  MsgDealer_GC2GS_019_001_ReqStartDinner extends NPUserMsgDealer<GC2GS_019_001_ReqStartDinner>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_019_001_ReqStartDinner _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        //检查是否有未领取的开宴奖励
        if(userData.getDinnerComponent().hasOwnerReward())
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
        
        //检查宴会配置
        RefDinnerType ref = RefDinnerType.getMgr().get(_msg.getDinnerId());
        if(null == ref)
        {
            _commiter.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
            return;
        }
        //当前为普通开宴方式，不包括许可证开启宴会
        if(ref.is_permit_open)
        {
        	_commiter.commitFailRes(DinnerErr.DINNER_NOT_PERMIT.getCode());
            return;
        }
        
        //检查消耗物品
        if(!userData.hasCostItemList(ref.open_cost))
        {
            _commiter.commitFailRes(CommErr.ITEM_NOT_ENOUGH.getCode());
            return;
        }
        
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.DINNER_START);
        
        //消耗物品
        if(!userData.spendCostItemList(ref.open_cost, context))
        {
            _commiter.commitFailRes(CommErr.CONSUME_FAIL.getCode());
            return;
        }
        
        //创建宴会
        DinnerInfo info = getUSServer().getDinnerPool().start(userData, ref, context);
        //回包数据
        _commiter.commitSucRes(US2GCWriter_019_DinnerOp.make_001_RetStartDinner(info));
        
		//增加开宴计数
		userData.getRecordComponent().addRecord(ENPPlayerRecordParam.START_DINNER_COUNT, 1, context);

        MJEventLog.logParty(userData, _msg.getDinnerId(), 2);
    }
}