package NPHttpServer.NPGeneralListener.RequestDispather;


import NPCommon.Dispather.NPRequestDispatcher;

/********************
 * 平台服务器发送来请求的处理对象
 *
 * @author Administrator
 *
 */
public class NPHSGeneralRequestDispather extends NPRequestDispatcher
{
    private static final NPHSGeneralRequestDispather _g_instance = new NPHSGeneralRequestDispather();

    protected NPHSGeneralRequestDispather()
    {
        NPHSGeneral_001_RequestDispatcher_BasicOp.regist(this);
        NPHSGeneral_255_RequestDispatcher.init(this);
    }

    public static NPHSGeneralRequestDispather getInstance()
    {
        return _g_instance;
    }
}
