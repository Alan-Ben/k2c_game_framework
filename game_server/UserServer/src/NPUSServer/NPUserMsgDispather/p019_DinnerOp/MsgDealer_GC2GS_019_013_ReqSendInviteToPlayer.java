package  NPUSServer.NPUserMsgDispather.p019_DinnerOp;

import GC2GS.p019_DinnerOp.GC2GS_019_013_ReqSendInviteToPlayer;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.DinnerErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CallBack._ICallBackResultT;
import NPEnum.ENPChatMsgType;
import NPUSServer.DinnerMgr.DinnerInfo;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_019_DinnerOp;
public class  MsgDealer_GC2GS_019_013_ReqSendInviteToPlayer extends NPUserMsgDealer<GC2GS_019_013_ReqSendInviteToPlayer>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_019_013_ReqSendInviteToPlayer _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        //检查参数
        if(_msg.getTargetCid() == userData.getCid())
        {
        	_commiter.commitFailRes(CommErr.PARAM_ERROR.getCode());
        	return;
        }
        
        //获取玩家当前举办的宴会数据
        DinnerInfo dinner = getUSServer().getDinnerPool().lookupByOwnerCid(userData.getCid());
        if(null == dinner)
        {
        	_commiter.commitFailRes(DinnerErr.DINNER_NOT_FOUND.getCode());
        	return;
        }
        
        //发送私聊
        _commiter.getUserData().getPlayerChatRoomDealer().sendPrivateMsg(_msg.getTargetCid(), ENPChatMsgType.DINNER_INVITE.ordinal(),
                userData.toChatPlayerProto().makePackage(), dinner.toChatProto().makePackage(), new _ICallBackResultT<Long>()
                {
                    @Override
                    public void onRunOver(Result _result, Long _msgId)
                    {
                        if (!_result.isSucc())
                        {
                            _commiter.commitFailRes(_result.getCode());
                            return;
                        }

                        _commiter.commitSucRes(US2GCWriter_019_DinnerOp.make_013_RetSendInviteToPlayer(_msgId));
                    }
                });
    }
}