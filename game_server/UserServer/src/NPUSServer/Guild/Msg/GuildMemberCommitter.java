package NPUSServer.Guild.Msg;

import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import ALBasicProtocolPack._IALProtocolStructure;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.Guild.Member.GuildMemberInfo;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

public class GuildMemberCommitter implements _IWCGBasicRequestCommiter
{
    //联盟对象
    private GuildInfo _m_guildInfo;
    //成员对象
    private GuildMemberInfo _m_memberInfo;
    //回调处理对象
    private _ANPUSUserBasicMsgItem _m_committer;

    public GuildMemberCommitter(GuildInfo _guildInfo, GuildMemberInfo _memberInfo, _ANPUSUserBasicMsgItem _committer)
    {
        _m_guildInfo = _guildInfo;
        _m_memberInfo = _memberInfo;
        _m_committer = _committer;
    }

    public GuildInfo getGuildInfo()
    {
        return _m_guildInfo;
    }

    public GuildMemberInfo getMemberInfo()
    {
        return _m_memberInfo;
    }

    public NPUSUserData getUserData()
    {
        return _m_committer.getUserData();
    }

    @Override
    public _IALProtocolReceiver getRequestDealer()
    {
        return null;
    }

    @Override
    public void commitSucRes(_IALProtocolStructure _retMsg)
    {
        _m_committer.commitSucRes(_retMsg);
    }

    @Override
    public void commitFailRes(int _errCode)
    {
        _m_committer.commitFailRes(_errCode);
    }
}
