package NPUSServer.NPUserMsgDispather.p021_PlayerInfo;

import GC2GS.p021_PlayerInfo.GC2GS_021_049_ReqViewPlayerChatEmoteGroup;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;

/*************
 * 设置称号信息已查看
 * @author mj
 *
 */
public class MsgDealer_GC2GS_021_049_ReqViewPlayerChatEmoteGroup extends NPUserMsgDealer<GC2GS_021_049_ReqViewPlayerChatEmoteGroup>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_021_049_ReqViewPlayerChatEmoteGroup _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        //设置对应气泡框已查看
        userData.getEmoteGroupComponent().setItemViewed(_msg.getRefId());
    }
}
