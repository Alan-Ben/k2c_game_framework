package WCGCommon.XYLog.Entity;

import WCGCommon.XYLog.XYLogManager;
import com.google.gson.Gson;

public abstract class BaseXYLog
{
    public String key = "pgto07vvjd0j2t0";

    public abstract String getApi();


    public String toString()
    {
        return new Gson().toJson(this);
    }

    public void logXy()
    {
        XYLogManager.getInstance().addLog(this);
    }
}
