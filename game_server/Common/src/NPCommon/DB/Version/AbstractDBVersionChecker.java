package NPCommon.DB.Version;

import NPCommon.DB.WCGDBObj;

import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

/**
 * abstract class, the basic function of DB version manager and automatic
 * updates
 * @author Aaron
 */
public abstract class AbstractDBVersionChecker
{

    // for record the update logic of someone's version
    protected Map<String, IUpdateDBVersion> m_vesionUpdateDict;


    // save the error info
    private String m_errorInfo;

    /**
     * constructor
     */
    protected AbstractDBVersionChecker()
    {
        m_vesionUpdateDict = new ConcurrentHashMap<>();
        m_errorInfo = "";
    }

    /**
     * get current db version, abstract method for the implementation class to
     * override
     * @return
     */
    public abstract String getCurVersion();

    /**
     * set current db version, abstract method for the implementation class to
     * override
     * @param version
     * @return
     */
    protected abstract boolean _setCurVersion(String version);

    /**
     * get the newest db version, abstract method for the implementation class
     * to override
     * @return
     */
    public abstract String getNewestVersion();

    /**
     * call this when error happen, abstract method for the implementation class
     * to override
     * @param errorInfo
     */
    protected abstract void _onError(String errorInfo);

    /**
     * call this when you need record some message, abstract method for the
     * implementation class to override
     * @param msg
     */
    protected abstract void _onMsg(String msg);

    /**
     * initialize db version info, abstract method for the implementation class
     * to override
     * @return
     */
    protected abstract boolean initCurrentVersion();

    /**
     * get the db version for no version info
     * @return
     */
    protected String _getNoVersionString()
    {
        return "0.0.0";
    }

    /**
     * get the last error info
     * @return
     */
    public String getLastError()
    {
        return m_errorInfo;
    }

    /**
     * set the last error info
     * @param errorInfo
     */
    protected void _setLastError(String errorInfo)
    {
        m_errorInfo = errorInfo;
    }

    /**
     * clear the last error info
     */
    protected void _clearLastError()
    {
        m_errorInfo = "";
    }

    /**
     * register one version update logic
     * @param versionUpdataObj
     * @return is successful to register
     */
    public boolean regVersionUpdate(IUpdateDBVersion versionUpdataObj)
    {
        String version = versionUpdataObj.getRequestVersion();

        if (m_vesionUpdateDict.containsKey(version) == false)
        {
            m_vesionUpdateDict.put(version, versionUpdataObj);
            this._clearLastError();
            // this._onMsg("RegVersionUpdate: registered version=[" + version +
            // "]!");
            return true;
        } else
        {
            this._onError("RegVersionUpdate: version=[" + version + "] has registered!!!");
            return false;
        }
    }

    /**
     * update db table structure, form current version to newest version
     * @return is successful to update
     */
    protected boolean _updateContent()
    {
        String curVersion = this.getCurVersion();

        while (m_vesionUpdateDict.containsKey(curVersion))
        {
            IUpdateDBVersion versionUpdataObj = m_vesionUpdateDict.get(curVersion);

            if (versionUpdataObj.run(getDBObj()) == false)
            {
                this._onError("AutoUpdate: version=[" + curVersion + "] update failed!!!");
                return false;
            }

            String targetVersion = versionUpdataObj.getTargetVersion();
            if (this._setCurVersion(targetVersion) == false)
            {
                this._onError("AutoUpdate: set db version=[" + targetVersion + "] failed!!!");
                return false;
            }

            this._onMsg("AutoUpdate: update from [" + curVersion + "] to [" + targetVersion + "]");
            curVersion = targetVersion;
        }

        String newestVersion = this.getNewestVersion();
        if (curVersion.equals(newestVersion))
        {
            this._clearLastError();
            this._onMsg("AutoUpdate: [" + this.getDBObj().getDBName() + "] successfully update to the version=[" + newestVersion + "]");
            return true;
        } else
        {
            this._onError("AutoUpdate: [" + this.getDBObj().getDBName() + "] failed by version current=[" + curVersion + "] is not newest=[" + newestVersion + "]!!!");
            return false;
        }
    }

    /**
     * call this, when the current version is the newest
     * @return true
     */
    protected boolean _notNeedUpdate()
    {
        this._onMsg("AutoUpdate: [" + this.getDBObj().getDBName() + "] current version=[" + this.getNewestVersion() + "] is the newest.");
        return true;
    }

    /**
     * check the current version info is existent
     * @return if db has version info return true, else return false
     */
    protected boolean _checkCurrentVersion()
    {
        return this.getCurVersion().equals(this._getNoVersionString());
    }

    /**
     * create or update db structure form current version to newest version
     * @return is successful done
     */
    protected Boolean run()
    {
        boolean isOk = this.initCurrentVersion();
        if (isOk)
        {
            if (this.getCurVersion().equalsIgnoreCase(this.getNewestVersion()))
            {
                isOk = this._notNeedUpdate();
            } else
            {
                isOk = this._updateContent();
            }
        }

        return isOk;
    }

    public abstract WCGDBObj getDBObj();

}
