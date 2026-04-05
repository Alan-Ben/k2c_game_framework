package NPUSServer.NPUserMsgDispather;

import NPUSServer.NPUserServer;

/**************
 * 客户端协议处理对象
 *
 * @author Administrator
 *
 */
public class NPUserMsgDispatcher extends NPUserMsgBasicDispatcher
{
    public NPUserMsgDispatcher(NPUserServer _usServer)
    {
        super(_usServer);

        //自动注册本类所属的包下的所有NPUserMsgDealer,必须为NPUserMsgDealer类必须为public类型，否则反射取无法创建实例
        autoRegistHandler(this.getClass().getPackage().getName());
    }
}
