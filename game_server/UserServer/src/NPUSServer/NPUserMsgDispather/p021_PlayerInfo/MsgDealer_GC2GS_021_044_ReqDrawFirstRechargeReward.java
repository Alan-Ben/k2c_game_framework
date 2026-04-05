package NPUSServer.NPUserMsgDispather.p021_PlayerInfo;

import GC2GS.p021_PlayerInfo.GC2GS_021_044_ReqDrawFirstRechargeReward;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.PlayerErr;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.RefFirstRechargeDay;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.PlayerBuffComp.PlayerBuffInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_021_PlayerInfo;
import NPUSServer.USLog;

/*************
 * 设置称号信息已查看
 * @author mj
 *
 */
public class MsgDealer_GC2GS_021_044_ReqDrawFirstRechargeReward extends NPUserMsgDealer<GC2GS_021_044_ReqDrawFirstRechargeReward>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_021_044_ReqDrawFirstRechargeReward _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        RefFirstRechargeDay ref = RefFirstRechargeDay.getMgr().get(_msg.getDay());
        if (null == ref)
        {
            _commiter.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
            return;
        }

        PlayerBuffInfo info = userData.getBuffComponent().lookupBuff(ref.buff_id);
        if (null == info)
        {
            _commiter.commitFailRes(PlayerErr.FIRST_RECHARGE_GIFT_PACK_NOT_BUY.getCode());
            return;
        }

        // 判断是否已经领取
        if (info.getLayer() < 1)
        {
            _commiter.commitFailRes(PlayerErr.FIRST_RECHARGE_REWARD_HAD_DRAW.getCode());
            return;
        }

        // 判断是否满足条件
        if (!NPPlayerConditionDealerMgr.IsEnable(ref.condition, userData, null))
        {
            _commiter.commitFailRes(PlayerErr.FIRST_RECHARGE_REWARD_DAY_NUM_NOT_MEET.getCode());
            return;
        }

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.DRAW_FIRST_RECHARGE_REWARD);

        // 扣除层数
        if(!userData.getBuffComponent().checkLogicEvt(ref.buff_id, null, context))
        {
            USLog.error(getUSServer(),
                    "MsgDealer_GC2GS_021_044_ReqDrawFirstRechargeReward checkLogicEvt failed, uid={}, buffId={}", userData.getUid(), ref.buff_id);
            _commiter.commitFailRes(PlayerErr.FIRST_RECHARGE_REWARD_DRAW_FAIL.getCode());
            return;
        }

        userData.gainItemList(ref.item_list, context);

        if (ref.hasSpecialItem())
            userData.gainItem(ref.special_item, context);

        _commiter.commitSucRes(US2GCWriter_021_PlayerInfo.make_044_RetDrawFirstRechargeReward(context));
    }
}
