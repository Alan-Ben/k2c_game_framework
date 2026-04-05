package NPUSServer.Guild.Msg;

import ALBasicProtocolPack._IALProtocolStructure;
import Common.GuildEnum.EGuildPermissionType;
import NPCommon.Dispather._IAutoRegistMsgHandler;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys.CustomCommiter._ATWCGBasicRequestAutoSubOrderDealer_CustomCommiter;

public abstract class GuildMemberMsgDealer<T extends _IALProtocolStructure>
        extends _ATWCGBasicRequestAutoSubOrderDealer_CustomCommiter<GuildMemberCommitter, T>
        implements _IAutoRegistMsgHandler
{
    /**
     * 获取需要的权限列表
     * @return 权限列表
     */
    public abstract EGuildPermissionType getNeedPermissionType();

    /**
     * 检查权限
     * @param _committer 提交者
     * @return
     */
    public boolean checkPermission(GuildMemberCommitter _committer)
    {
        if (getNeedPermissionType() == null)
            return true;

        return _committer.getMemberInfo().checkPermission(getNeedPermissionType());
    }
}
