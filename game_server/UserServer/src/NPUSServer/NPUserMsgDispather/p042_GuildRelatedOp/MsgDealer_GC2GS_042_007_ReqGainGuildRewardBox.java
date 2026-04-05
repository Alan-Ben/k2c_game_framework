package NPUSServer.NPUserMsgDispather.p042_GuildRelatedOp;

import GC2GS.p042_GuildRelatedOp.GC2GS_042_007_ReqGainGuildRewardBox;
import NPCommon.ErrMain.GuildErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.GuildBoxComp._APlayerGuildBoxInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_042_GuildRelatedOp;

public class MsgDealer_GC2GS_042_007_ReqGainGuildRewardBox extends NPUserMsgDealer<GC2GS_042_007_ReqGainGuildRewardBox>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_042_007_ReqGainGuildRewardBox _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        //获取玩家对应宝箱数据
        _APlayerGuildBoxInfo playerBox = userData.getGuildBoxComponent().getGuildBox(_msg.getBoxType());
        if(null == playerBox)
        {
        	_commiter.commitFailRes(GuildErr.GUILD_BOX_TYPE_ERR.getCode());
        	return;
        }
        
        //刷新玩家可领取宝箱数据
        Result refreshResult = playerBox.refreshBox();
        if(!refreshResult.isSucc())
        {
        	_commiter.commitFailRes(refreshResult.getCode());
        	return;
        }
        
        //领取奖励
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.GUILD_BOX_GAIN_REWARD);
        Result result = playerBox.gainBox(_msg.getId(), context);
        if(!result.isSucc())
        {
        	_commiter.commitFailRes(result.getCode());
        	return;
        }
        
        _commiter.commitSucRes(US2GCWriter_042_GuildRelatedOp.make_007_RetGainGuildRewardBox());
        
        //日志数据
    }
}
