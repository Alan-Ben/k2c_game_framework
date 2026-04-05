package NPUSServer.NPUserMsgDispather.p032_GuildOp;

import GC2GS.p032_GuildOp.GC2GS_032_015_ReqGuildBroadcastMessage;
import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_PlayerSendChatInfo;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.ErrMain.ChatErr;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.GuildErr;
import NPEnum.ENPGameEvent;
import NPEnum.ENPItemType;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.ChatSys.ChatUserInfo;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.PlayerFixedCdComp.PlayerFixedCD;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;

import java.util.ArrayList;
import java.util.List;

public class MsgDealer_GC2GS_032_015_ReqGuildBroadcastMessage extends NPUserMsgDealer<GC2GS_032_015_ReqGuildBroadcastMessage>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_032_015_ReqGuildBroadcastMessage _msg)
    {
        //检查发送用户是否聊天注册成功
        ChatUserInfo senderUser = _committer.getUserData().getUSServer().getChatUserMgr().lookupChatUser(_committer.getUserData().getCid());
        if(null == senderUser)
        {
            _committer.commitFailRes(ChatErr.CHAT_USER_JOIN_ROOM_FAIL.getCode());

            return;
        }

        //检查消息长度
        if (!RefGeneral.Ref().guild_broadcast_message_length_limit.inRange(_msg.getMessage().length()))
        {
            _committer.commitFailRes(GuildErr.STRING_LENGTH_OVER_LIMIT.getCode());
            return;
        }

        if (_msg.getCidList().isEmpty())
        {
            _committer.commitFailRes(CommErr.PARAM_ERROR.getCode());
            return;
        }

        //构造消耗列表
        List<NPCommonCostItem> costList = new ArrayList<>();

        //判断是否需要消耗cd
        long fixCdId = RefGeneral.Ref().guild_broadcast_message_daily_limit_fixcd_id;

        PlayerFixedCD playerFixedCD = _committer.getUserData().getFixedCdComponent().lookupByRefId(fixCdId);
        if (fixCdId != 0 && playerFixedCD != null && playerFixedCD.getCount() >= 1)
        {
            costList.add(new NPCommonCostItem(ENPItemType.FIXED_CD, fixCdId, 1));
        } else
        {
            costList.addAll(RefGeneral.Ref().guild_broadcast_message_cost);
        }

        //检查消耗
        if (!_committer.getUserData().hasCostItemList(costList))
        {
            _committer.commitFailRes(CommErr.ITEM_NOT_ENOUGH.getCode());
            return;
        }

        if (!_committer.getUserData().spendItem(costList, NPPlayerContext.createNew(ENPGameEvent.GUILD_BROADCAST_MESSAGE)))
        {
            _committer.commitFailRes(CommErr.CONSUME_FAIL.getCode());
            return;
        }

        GuildOp_PlayerSendChatInfo addInfo = new GuildOp_PlayerSendChatInfo();
        addInfo.setChatUid(senderUser.getChatUid());
        addInfo.setPlayerContent(_committer.getUserData().toChatPlayerProto());

        //转化为统一跨服消息进行处理
        getUSServer().dealGuildMsg(
                _committer,
                _committer.getUserData().getCid(),
                _committer.getUserData().getGuildComponent().getGuildId(),
                _msg,
                addInfo
        );
    }
}
