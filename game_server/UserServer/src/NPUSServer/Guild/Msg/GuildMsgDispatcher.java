package NPUSServer.Guild.Msg;

import ALBasicProtocolPack._IALProtocolStructure;
import NPCommon.Dispather._IAutoRegistMsgHandler;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.GuildErr;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommClass;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys.CustomCommiter._TWCGBasicRequestDispather_CustomCommiter;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys.CustomCommiter._TWCGBasicRequestMainOrderDealer_CustomCommiter;

import java.util.List;

public class GuildMsgDispatcher extends _TWCGBasicRequestDispather_CustomCommiter<GuildMemberCommitter>
{
    private static final GuildMsgDispatcher _g_instance = new GuildMsgDispatcher();

    public static GuildMsgDispatcher getInstance()
    {
        return _g_instance;
    }

    private GuildMsgDispatcher()
    {
        autoRegistHandler(this.getClass().getPackage().getName());
    }

    /**
     * 处理协议
     * @param _committer 提交者
     * @param _proto     协议
     */
    public void dealMsg(GuildMemberCommitter _committer, _IALProtocolStructure _proto)
    {
        _TWCGBasicRequestMainOrderDealer_CustomCommiter<GuildMemberCommitter> mainDealer = getDealer(_proto.getMainOrder());
        if (mainDealer == null)
        {
            _committer.commitFailRes(CommErr.SYS_ERR.getCode());
            return;
        }

        //检查权限
        GuildMemberMsgDealer<?> msgDealer = (GuildMemberMsgDealer<?>) mainDealer.getSubDealer(_proto.getSubOrder());
        if (!msgDealer.checkPermission(_committer))
        {
            _committer.commitFailRes(GuildErr.DONT_HAVE_PERMISSION.getCode());
            return;
        }

        DealProtocol(_committer, _proto);
    }

    /*********
     * 自动注册协议处理dealer
     * 处理类类必须为public类型，否则反射取无法创建实例
     */
    @SuppressWarnings("rawtypes")
    public void autoRegistHandler(String _packageName)
    {
        List<Class<?>> clazzs = CommClass.getAllClassByInterface(_IAutoRegistMsgHandler.class, _packageName);
        for (Class<?> clazz : clazzs)
        {
            try
            {
                regSubDealer((GuildMemberMsgDealer) clazz.newInstance());
            } catch (Throwable e)
            {
                CommLog.error("GuildMsgDispatcher auto regist msg handler:{} err", clazz.getSimpleName(), e);
            }
        }
    }
}
