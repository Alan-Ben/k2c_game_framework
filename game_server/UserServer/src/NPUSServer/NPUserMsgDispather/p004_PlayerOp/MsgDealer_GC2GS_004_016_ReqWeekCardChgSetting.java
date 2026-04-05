package NPUSServer.NPUserMsgDispather.p004_PlayerOp;

import GC2GS.p004_PlayerOp.GC2GS_004_016_ReqWeekCardChgSetting;
import NPCommon.ErrMain.Result.Result;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;

public class MsgDealer_GC2GS_004_016_ReqWeekCardChgSetting extends NPUserMsgDealer<GC2GS_004_016_ReqWeekCardChgSetting>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_004_016_ReqWeekCardChgSetting _msg)
    {
    	//获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        Result result = userData.getWeekCardComponent().chgSetting(_msg.getSettingInfo());
        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }

       //回包处理
       _commiter.commitSucRes(US2GCWriter_004_PlayerOp.make_016_RetWeekCardChgSetting());
    }
}
