package NPUSServer.NPUserMsgDispather.p021_PlayerInfo;

import GC2GS.p021_PlayerInfo.GC2GS_021_029_ReqDailyCheckDrawReward;
import NPCommon.ErrMain.PlayerErr;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.DailyCheck.DailyCheckLoopRewardShow;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_021_PlayerInfo;

import java.util.ArrayList;

/*************
 * 设置称号信息已查看
 * @author mj
 *
 */
public class MsgDealer_GC2GS_021_029_ReqDailyCheckDrawReward extends NPUserMsgDealer<GC2GS_021_029_ReqDailyCheckDrawReward>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_021_029_ReqDailyCheckDrawReward _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.DAILY_CHECK_DRAW_REWARD);
        
        ArrayList<DailyCheckLoopRewardShow> resultList = userData.getDailyCheckComponent().drawLoopReward(context);
        if(null == resultList || resultList.isEmpty())
        {
        	_commiter.commitFailRes(PlayerErr.NOT_REWARD_GAN_GAIN.getCode());
          return;
        }

        //回包协议
        _commiter.commitSucRes(US2GCWriter_021_PlayerInfo.make_029_RetDailyCheckDrawReward(context));
    }
}
