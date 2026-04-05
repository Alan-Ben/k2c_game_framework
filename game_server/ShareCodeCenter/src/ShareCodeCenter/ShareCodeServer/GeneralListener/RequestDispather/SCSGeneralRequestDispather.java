package ShareCodeCenter.ShareCodeServer.GeneralListener.RequestDispather;


import NPCommon.Dispather.NPRequestDispatcher;

/********************
 * 平台服务器发送来请求的处理对象
 *
 * @author Administrator
 *
 */
public class SCSGeneralRequestDispather extends NPRequestDispatcher
{
    private static final SCSGeneralRequestDispather _g_instance = new SCSGeneralRequestDispather();

    public static SCSGeneralRequestDispather getInstance()
    {
        return _g_instance;
    }

    protected SCSGeneralRequestDispather()
    {
        SCSGeneral_001_RequestDispatcher_ShareCodeOp.init(this);
        SCSGeneral_255_RequestDispatcher.init(this);
    }
}
