package NPCommon.Util;

import ALBasicServer.ALSocket._IALServerStartMonitor;
import NPCommon.Log.CommLog;

public class CommonServerStartMonitor implements _IALServerStartMonitor
{
    @Override
    public void onServerOpenPortErr(int _port, Exception _exception)
    {
        CommLog.error("Open server port error!! Port： " + _port);
    }
}
