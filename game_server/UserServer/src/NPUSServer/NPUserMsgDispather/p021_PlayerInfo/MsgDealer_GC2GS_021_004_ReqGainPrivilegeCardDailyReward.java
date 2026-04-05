package NPUSServer.NPUserMsgDispather.p021_PlayerInfo;

import GC2GS.p021_PlayerInfo.GC2GS_021_004_ReqGainPrivilegeCardDailyReward;
import NPCommon.ErrMain.PlayerErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.PrivilegeCardComp.PrivilegeCardInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_021_PlayerInfo;

/*************
 * 领取权益卡每日奖励
 * @author mj
 *
 */
public class MsgDealer_GC2GS_021_004_ReqGainPrivilegeCardDailyReward extends NPUserMsgDealer<GC2GS_021_004_ReqGainPrivilegeCardDailyReward>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_021_004_ReqGainPrivilegeCardDailyReward _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        PrivilegeCardInfo info = userData.getPrivilegeCardComponent().getPrivilegeCard(_msg.getCardType());
        if(null == info)
        {
        	_commiter.commitFailRes(PlayerErr.PRIVILEGE_CARD_NOT_FOUND.getCode());
        	return;
        }
        
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.PRIVILEGE_CARD_GAIN_DAILY_REWARD);
        //领取每日奖励
        Result result = info.gainDailyReward(context);
        if(!result.isSucc())
        {
        	_commiter.commitFailRes(result.getCode());
        	return;
        }
        
        //获得物品通用展示
        if(!context.getCollector().isEmpty())
        {
        	userData.sendMsgToGC(context.getCollector().toProto());
        }
        
        _commiter.commitSucRes(US2GCWriter_021_PlayerInfo.make_004_RetGainPrivilegeCardDailyReward(context));
    }
}
