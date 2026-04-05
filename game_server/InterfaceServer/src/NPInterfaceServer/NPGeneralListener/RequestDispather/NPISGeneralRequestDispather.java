package NPInterfaceServer.NPGeneralListener.RequestDispather;


import NPCommon.Dispather.NPRequestDispatcher;

/********************
 * 平台服务器发送来请求的处理对象
 *
 * @author Administrator
 *
 */
public class NPISGeneralRequestDispather extends NPRequestDispatcher
{
    private static final NPISGeneralRequestDispather _g_instance = new NPISGeneralRequestDispather();

    public static NPISGeneralRequestDispather getInstance()
    {
        return _g_instance;
    }

    protected NPISGeneralRequestDispather()
    {
        NPISGeneral_001_RequestDispatcher_ChatOp.init(this);
        NPISGeneral_255_RequestDispatcher.init(this);
    }
}
