package NPUSServer.NPUserMsgDispather.p004_PlayerOp;

import GC2GS.p004_PlayerOp.GC2GS_004_045_ReqGraveConNewInfo;
import NPCommon.ErrMain.GraveErr;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPGameEvent;
import NPEnum.ENPItemType;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;

import java.util.ArrayList;

public class MsgDealer_GC2GS_004_045_ReqGraveConNewInfo extends NPUserMsgDealer<GC2GS_004_045_ReqGraveConNewInfo>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_004_045_ReqGraveConNewInfo _msg)
    {
    	//获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();
        
        //获取可以祝贺的新晋杰出者名单列表
        ArrayList<Long> congNewCidList = getUSServer().getGraveMgr().getGraveNewMgr().congGraveNewRewardList(userData.getCid());
        if(congNewCidList.isEmpty())
        {
        	_commiter.commitFailRes(GraveErr.GRAVE_CONG_NOT_NEW_INFO.getCode());
        	return;
        }
        
        //获取对应次数的奖励
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.GRAVE_CONGRATULATE);
        int congSize = congNewCidList.size();
        for(int i = 0; i < congSize; i++)
        {
        	//GOB-8351【优化-0】杰出者大厅-膜拜新晋杰出者的钻石奖励有上限
        	//https://www.teambition.com/task/695b67165937e3f8487189e2
            if(userData.spendItem(ENPItemType.FIXED_CD, RefGeneral.Ref().grave_congratulate_reward_gain_fixed_cd, 1, context))
            {
            	userData.gainReward(RefGeneral.Ref().grave_congratulate_reward_id, context);
            }
        }
        
        //获取随机祝贺的玩家
        int idx = CommonFunc.randomInt(congSize - 1);
        long congCid = congNewCidList.get(idx);
        
        _commiter.commitSucRes(US2GCWriter_004_PlayerOp.make_045_RetGraveConNewInfo(congCid, context));
    }
}
