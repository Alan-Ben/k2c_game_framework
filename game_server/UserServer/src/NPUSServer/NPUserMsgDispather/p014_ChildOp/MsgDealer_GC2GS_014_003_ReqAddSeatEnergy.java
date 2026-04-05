package  NPUSServer.NPUserMsgDispather.p014_ChildOp;
import GC2GS.p014_ChildOp.GC2GS_014_003_ReqAddSeatEnergy;
import NPCommon.ErrMain.ChildErr;
import NPCommon.ErrMain.CommErr;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.Child.ChildSeatInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_014_ChildOp;
import USLOGDB.OptBo.Opt014003ChildAddSeatEnergyBO;
public class  MsgDealer_GC2GS_014_003_ReqAddSeatEnergy extends NPUserMsgDealer<GC2GS_014_003_ReqAddSeatEnergy>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_014_003_ReqAddSeatEnergy _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        //检查参数
        if(_msg.getAddCount() <= 0)
        {
        	_commiter.commitFailRes(CommErr.PARAM_ERROR.getCode());
        	return;
        }
        
        ChildSeatInfo seat = userData.getChildComponent().getChildMgr().lookupSeat(_msg.getSeatId());
        //训练位尚未解锁
        if(!seat.checkUnlock())
        {
        	_commiter.commitFailRes(ChildErr.CHILD_SEAT_NOT_UNLOCK.getCode());
        	return;
        }
        
        //检查消耗物品
        if(!userData.hasItem(RefGeneral.Ref().child_seat_recover_item, _msg.getAddCount()))
        {
        	_commiter.commitFailRes(CommErr.ITEM_NOT_ENOUGH.getCode());
        	return;
        }
        
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.CHILD_SEAT_ADD_ENERGY);
        //消耗物品
        if(!userData.spendItem(RefGeneral.Ref().child_seat_recover_item, _msg.getAddCount(), context))
        {
        	_commiter.commitFailRes(CommErr.CONSUME_FAIL.getCode());
        	return;
        }
        //增加训练位的额外脑力值
        seat.addEnergyExt(_msg.getAddCount(), context);

        _commiter.commitSucRes(US2GCWriter_014_ChildOp.make_003_RetAddSeatEnergy());
        
        //日志
        Opt014003ChildAddSeatEnergyBO optBo = new Opt014003ChildAddSeatEnergyBO();
        optBo.setSeatId(getUSServer().getBM(), _msg.getSeatId());
        optBo.setAddCount(getUSServer().getBM(), _msg.getAddCount());
        userData.logEvent(optBo, context);
    }
}