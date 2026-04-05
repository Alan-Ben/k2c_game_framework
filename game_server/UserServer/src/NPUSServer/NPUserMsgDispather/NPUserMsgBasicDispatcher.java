package NPUSServer.NPUserMsgDispather;

import NPCommon.Dispather._IAutoRegistMsgHandler;
import NPCommon.ErrMain.CommErr;
import NPCommon.Util.CommClass;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys.CustomCommiter._TWCGBasicRequestDispather_CustomCommiter;

import java.nio.ByteBuffer;
import java.util.List;

/*************
 * 为了优化US中的消息处理代码，重新编写的Dispather和Dealer对象
 * @author mj
 *
 */
public class NPUserMsgBasicDispatcher extends _TWCGBasicRequestDispather_CustomCommiter<_ANPUSUserBasicMsgItem>
{
    private final NPUserServer _m_usUSServer;

    public NPUserMsgBasicDispatcher(NPUserServer _usServer)
    {
        _m_usUSServer = _usServer;
    }

    public NPUserServer getUSServer() {return _m_usUSServer;}

    /**
     * 重写协议处理方法，添加协议屏蔽拦截逻辑
     *
     * 执行流程：
     * 1. 从ByteBuffer中读取主协议号和副协议号
     * 2. 检查该协议是否被屏蔽
     * 3. 如果被屏蔽，返回通用错误码并拦截
     * 4. 如果未屏蔽，调用父类方法继续处理
     *
     * @param _commiter 消息提交器
     * @param _msg 协议消息数据
     * @return 是否处理成功
     */
    @Override
    public boolean DealProtocol(_ANPUSUserBasicMsgItem _commiter, ByteBuffer _msg)
    {
        // 保存当前位置，以便后续读取
        int originalPosition = _msg.position();

        // 读取主协议号和副协议号（协议格式：主协议号1字节 + 副协议号1字节 + 数据）
        byte mainProtocol = _msg.get();
        byte subProtocol = _msg.get();

        // 恢复ByteBuffer位置，让父类正常处理
        _msg.position(originalPosition);

        // 检查协议是否被屏蔽
        if (getUSServer().getProtocolShieldMgr().isProtocolShielded(mainProtocol & 0xFF, subProtocol & 0xFF))
        {
            // 协议被屏蔽，记录日志并返回错误
            USLog.warn(getUSServer(), "Protocol shielded: main={}, sub={}, cid={}",
                    mainProtocol & 0xFF, subProtocol & 0xFF,
                    _commiter.getUserData() != null ? _commiter.getUserData().getCid() : 0);

            // 返回通用系统错误
            _commiter.commitFailRes(CommErr.PROTOCOL_BLOCKED.getCode());
            return true; // 已处理（已拦截）
        }

        // 协议未被屏蔽，调用父类方法继续处理
        return super.DealProtocol(_commiter, _msg);
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
                NPUserMsgDealer handler = (NPUserMsgDealer) clazz.newInstance();

                //设置服务器对象
                handler._initUSServer(getUSServer());

                regSubDealer(handler);
            } catch (Throwable e)
            {
                USLog.error(_m_usUSServer, "auto regist msg handler:{} err", clazz.getSimpleName(), e);
            }
        }
    }
}
