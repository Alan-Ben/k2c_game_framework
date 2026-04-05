package NPUSServer.NPUserMsgDispather.p021_PlayerInfo;

import GC2GS.p021_PlayerInfo.GC2GS_021_045_ReqViewPlayerLazyCD;
import NPCommon.ErrMain.CommErr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.PlayerLazyCDComp.PlayerLazyCD;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_021_PlayerInfo;

/**
 * 查询玩家懒惰CD信息
 */
public class MsgDealer_GC2GS_021_045_ReqViewPlayerLazyCD extends NPUserMsgDealer<GC2GS_021_045_ReqViewPlayerLazyCD>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_021_045_ReqViewPlayerLazyCD _msg)
    {
        // 获取用户数据
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        // 查找指定CD对象
        PlayerLazyCD cd = userData.getLazyCDComponent().lookupCD(_msg.getCdId());
        if (null == cd)
        {
            _commiter.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
            return;
        }

        // 返回CD信息
        _commiter.commitSucRes(US2GCWriter_021_PlayerInfo.make_045_RetViewPlayerLazyCD(cd));
    }
}
