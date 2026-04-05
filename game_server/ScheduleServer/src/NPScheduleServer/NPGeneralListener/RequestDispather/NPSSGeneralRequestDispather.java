package NPScheduleServer.NPGeneralListener.RequestDispather;


import NPCommon.Dispather.NPRequestDispatcher;

/********************
 * 平台服务器发送来请求的处理对象
 *
 * @author Administrator
 *
 */
public class NPSSGeneralRequestDispather extends NPRequestDispatcher
{
    private static final NPSSGeneralRequestDispather _g_instance = new NPSSGeneralRequestDispather();

    public static NPSSGeneralRequestDispather getInstance()
    {
        return _g_instance;
    }

    protected NPSSGeneralRequestDispather()
    {
        NPSSGeneral_001_RequestDispatcher_CrossServerGroupOp.init(this);
        NPSSGeneral_255_RequestDispatcher.init(this);
    }
}
