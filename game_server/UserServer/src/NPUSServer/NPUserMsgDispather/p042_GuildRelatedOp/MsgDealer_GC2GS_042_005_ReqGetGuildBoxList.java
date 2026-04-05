package NPUSServer.NPUserMsgDispather.p042_GuildRelatedOp;

import GC2GS.p042_GuildRelatedOp.GC2GS_042_005_ReqGetGuildBoxList;
import NPCommon.ErrMain.GuildErr;
import NPCommon.ErrMain.Result.Result;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.GuildBoxComp._APlayerGuildBoxInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_042_GuildRelatedOp;

public class MsgDealer_GC2GS_042_005_ReqGetGuildBoxList extends NPUserMsgDealer<GC2GS_042_005_ReqGetGuildBoxList>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_042_005_ReqGetGuildBoxList _msg)
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
        
        _commiter.commitSucRes(US2GCWriter_042_GuildRelatedOp.make_005_RetGetGuildBoxList());

        //日志数据
    }
}
