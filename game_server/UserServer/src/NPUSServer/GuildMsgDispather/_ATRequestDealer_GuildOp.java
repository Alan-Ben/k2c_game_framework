package NPUSServer.GuildMsgDispather;

import ALBasicProtocolPack._IALProtocolStructure;
import Common.GuildEnum.EGuildPermissionType;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.GuildErr;
import NPUSServer.Guild.Member.GuildMemberInfo;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.NPGeneralListener.RequestDispather._ABasicGeneralRequestDealer;
import NPUSServer.NPUserServer;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

/**
 * 充值通知请求处理器
 * 处理PayCenter发送的充值成功通知
 */
public abstract class _ATRequestDealer_GuildOp<T extends _IALProtocolStructure> extends _ABasicGeneralRequestDealer<T>
{
    public _ATRequestDealer_GuildOp(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _committer, T _msg)
    {
        //获取联盟对象
        if(!(_committer instanceof GuildMsgCommiter))
        {
            _committer.commitFailRes(CommErr.SYS_ERR.getCode());
            return;
        }

        GuildMsgCommiter guildMsgCommiter = (GuildMsgCommiter)_committer;
        if (!checkPermission(guildMsgCommiter))
        {
            _committer.commitFailRes(GuildErr.DONT_HAVE_PERMISSION.getCode());
            return;
        }

        //实际处理协议
        _dealGuildMessage(guildMsgCommiter, _msg);
    }

    /**
     * 获取需要的权限列表
     * @return 权限列表
     */
    public abstract EGuildPermissionType getNeedPermissionType();

    /***
     * 实际处理联盟成员消息的函数
     * @param _committer
     * @param _msg
     */
    protected abstract void _dealGuildMessage(GuildMsgCommiter _committer, T _msg);

    /**
     * 检查权限
     * @param _committer 提交者
     * @return
     */
    public boolean checkPermission(GuildMsgCommiter _committer)
    {
        if (getNeedPermissionType() == null)
            return true;

        GuildMemberInfo memberInfo = _committer.getGuildInfo().getMemberMgr().lookup(_committer.getCid());
        if(null == memberInfo)
            return false;

        return memberInfo.checkPermission(getNeedPermissionType());
    }
}