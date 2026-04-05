package NPHttpServer.Http.Entity;

import NPEnum.ENPChatRoomType;

import java.util.HashSet;
import java.util.Set;

/**
 * @description: 玩家聊天禁言
 * @author: mark
 * @date: 2023-04-08 16:22:54
 */
public class NPEntityForbidPlayerChat
{
    //cid列表
    private Set<Long> _m_hsCidSet;

    //禁言时长 	0-永久
    private int _m_iHours;

    //聊天房间类型
    private ENPChatRoomType _m_eChatRoomType;

    public NPEntityForbidPlayerChat()
    {
        _m_hsCidSet = new HashSet<>();
    }

    public void addForbidCid(long _cid)
    {
        _m_hsCidSet.add(_cid);
    }

    public Set<Long> getForbidCidList()
    {
        return _m_hsCidSet;
    }

    public int getHours()
    {
        return _m_iHours;
    }

    public void setHours(int _hours)
    {
        _m_iHours = _hours;
    }

    public ENPChatRoomType getChatRoomType()
    {
        return _m_eChatRoomType;
    }

    public void setChatRoomType(ENPChatRoomType _chatRoomType)
    {
        _m_eChatRoomType = _chatRoomType;
    }
}
