package GameLogicServer.GeneralListener.RequestDispather.ActivityMsgDispatcher;

import GameLogicServer.GroupMgr.ActivityMgr._ATActivityInfo;
import NPCommon.Dispather._IAutoRegistMsgHandler;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommClass;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys.CustomCommiter._TWCGBasicRequestDispather_CustomCommiter;

import java.util.List;

public class ActivityRequestDispather extends _TWCGBasicRequestDispather_CustomCommiter<ActivityMsgCommiter>
{
    private _ATActivityInfo _m_activity;

    public  ActivityRequestDispather(_ATActivityInfo _activity)
    {
        _m_activity = _activity;

        autoRegistHandler(this.getClass().getPackage().getName());
    }

    public _ATActivityInfo getActivity() {return _m_activity;}

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
                regSubDealer((ActivityMsgDealer) clazz.newInstance());
            }
            catch (Throwable e)
            {
                CommLog.error("ActivityRequestDispather auto regist msg handler:{} err", clazz.getSimpleName(), e);
            }
        }
    }
}
