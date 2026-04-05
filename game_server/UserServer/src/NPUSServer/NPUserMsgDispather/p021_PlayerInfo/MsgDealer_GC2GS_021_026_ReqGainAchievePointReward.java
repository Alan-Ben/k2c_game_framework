package NPUSServer.NPUserMsgDispather.p021_PlayerInfo;

import GC2GS.p021_PlayerInfo.GC2GS_021_026_ReqGainAchievePointReward;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_021_PlayerInfo;
import USLOGDB.OptBo.Opt021026AchieveGainPointRewardBO;

/*************
 * 设置称号信息已查看
 * @author mj
 *
 */
public class MsgDealer_GC2GS_021_026_ReqGainAchievePointReward extends NPUserMsgDealer<GC2GS_021_026_ReqGainAchievePointReward>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_021_026_ReqGainAchievePointReward _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.GAIN_ACHIEVE_POINT_REWARD);

        //领取成就点奖励
        Result result = userData.getAchieveComponent().drawAchievePointStepReward(_msg.getAchievePointRewardId(), context);
        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }

        _commiter.getUserData().sendMsgToGC(context.getCollector().toProto());

        //回包协议
        _commiter.commitSucRes(US2GCWriter_021_PlayerInfo.make_026_RetGainAchievePointReward());
        
        //操作日志
        Opt021026AchieveGainPointRewardBO optBo = new Opt021026AchieveGainPointRewardBO();
        optBo.setStepId(userData.getUSServer().getBM(), _msg.getAchievePointRewardId());
        userData.logEvent(optBo, context);
    }
}
