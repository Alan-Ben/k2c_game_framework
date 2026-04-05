package NPUSServer.NPUserMsgDispather.p014_ChildOp;

import GC2GS.p014_ChildOp.GC2GS_014_019_ReqRandomGainChild;
import NPCommon.ErrMain.CommErr;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;

public class MsgDealer_GC2GS_014_019_ReqRandomGainChild extends NPUserMsgDealer<GC2GS_014_019_ReqRandomGainChild>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_014_019_ReqRandomGainChild _msg)
    {
        _commiter.commitFailRes(CommErr.PROTOCOL_BLOCKED.getCode());

//        NPUSUserData userData = _commiter.getUserData();
//
//        //检查训练席位
//        boolean hasUnusingSeat = userData.getChildComponent().getChildMgr().hasUnusingSeat();
//        if (!hasUnusingSeat)
//        {
//            _commiter.commitFailRes(ChildErr.NO_FREE_SEAT.getCode());
//            return;
//        }
//
//        ConsortInfo consortInfo = userData.getConsortComponent().randGetConsort();
//        if (consortInfo == null)
//        {
//            _commiter.commitFailRes(ConsortErr.CONSORT_NOT_EXISTS.getCode());
//            return;
//        }
//
//        ChildSeatInfo seatInfo = userData.getChildComponent().getChildMgr().lookupSeat(_msg.getSeatId());
//        if (seatInfo == null || seatInfo.isUsing())
//        {
//            _commiter.commitFailRes(ChildErr.NO_FREE_SEAT.getCode());
//            return;
//        }
//
//        boolean isGiftde = false;
//        //先检查是否必定卷王子嗣
//        int giftedCount = (int) userData.getParam(ENPPlayerParam.BIRTH_GIFTDE_COUM);
//        if (giftedCount > 0) //如果存在必定卷王子嗣，则无需权重随机
//        {
//            giftedCount--;
//            userData.setParam(ENPPlayerParam.BIRTH_GIFTDE_COUM, giftedCount);
//
//            isGiftde = true;
//        }
//
//        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.RAND_GAIN_CHILD);
//
//        //卷王子嗣检查
//        ChildInfo child = ChildSystem.createChild(consortInfo, isGiftde, seatInfo, context);
//        if (null == child)
//        {
//            //无添加子嗣，需要把扣除的效果数量补回去
//            if (isGiftde)
//            {
//                userData.incParam(ENPPlayerParam.BIRTH_GIFTDE_COUM, 1);
//            }
//
//            _commiter.commitFailRes(ChildErr.RAND_GAIN_CHILD_FAIL.getCode());
//            return;
//        }
//
//        _commiter.commitSucRes(US2GCWriter_014_ChildOp.make_019_RetRandomGainChild());
//
//        //获得子嗣事件
//        Event_P_GAIN_CHILD evt = new Event_P_GAIN_CHILD(context, 1);
//        userData.onLogicEvent(evt);
    }
}