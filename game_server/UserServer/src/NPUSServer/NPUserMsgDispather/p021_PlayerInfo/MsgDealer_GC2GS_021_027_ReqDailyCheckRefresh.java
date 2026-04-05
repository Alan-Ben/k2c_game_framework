package NPUSServer.NPUserMsgDispather.p021_PlayerInfo;

import GC2GS.p021_PlayerInfo.GC2GS_021_027_ReqDailyCheckRefresh;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_021_PlayerInfo;

/*************
 * 设置称号信息已查看
 * @author mj
 *
 */
public class MsgDealer_GC2GS_021_027_ReqDailyCheckRefresh extends NPUserMsgDealer<GC2GS_021_027_ReqDailyCheckRefresh>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_021_027_ReqDailyCheckRefresh _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        userData.getDailyCheckComponent().tryRefresh();

        //回包协议
        _commiter.commitSucRes(US2GCWriter_021_PlayerInfo.make_027_RetDailyCheckRefresh());
    }
}
