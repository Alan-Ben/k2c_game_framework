package NPUSServer.NPUserMsgDispather.p004_PlayerOp;

import GC2GS.p004_PlayerOp.GC2GS_004_014_ReqGainVisitOtherPlayerReward;
import NPCommon.ErrMain.PlayerErr;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPGameEvent;
import NPEnum.ENPPlayerParam;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;
import USLOGDB.OptBo.Opt004014PlayerGainVisitRewardBO;

public class MsgDealer_GC2GS_004_014_ReqGainVisitOtherPlayerReward extends NPUserMsgDealer<GC2GS_004_014_ReqGainVisitOtherPlayerReward>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_004_014_ReqGainVisitOtherPlayerReward _msg)
    {
    	//获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();
        
        //检查日期
        int curDay = CommonFunc.getNowTagYYYYMMDD();
        if(curDay <= userData.getParam(ENPPlayerParam.LAST_GAIN_VISIT_REWARD_DAY))
        {
        	_commiter.commitFailRes(PlayerErr.ALREADY_GAINED_VISIT_REWARD.getCode());
        	return;
        }
        
        //更新最后一次领取拜访奖励天数
        userData.setParam(ENPPlayerParam.LAST_GAIN_VISIT_REWARD_DAY, curDay);
        
        //领取奖励
       NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.GAIN_VISIT_OTHER_PLAYER_REWARD);
       userData.gainItemList(RefGeneral.Ref().visit_other_player_gain_item_list, context);
    
       //回包处理
       _commiter.commitSucRes(US2GCWriter_004_PlayerOp.make_014_RetGainVisitOtherPlayerReward(context));

       //日志
        Opt004014PlayerGainVisitRewardBO optBo = new Opt004014PlayerGainVisitRewardBO();
        optBo.setCurDay(getUSServer().getBM(), curDay);
        userData.logEvent(optBo, context);
    }
}
