package NPCommon.DB.Version;

import NPCommon.DB.WCGDBObj;

/**
 * interfac
 * @author Aaron
 */
public interface IUpdateDBVersion
{

    /**
     * get the version for updating requested
     * @return
     */
    public String getRequestVersion();

    /**
     * get the version after updated
     * @return
     */
    public String getTargetVersion();

    /**
     * do logic for updating db structure
     * @return true if updating successfully, else false
     */
    public boolean run(WCGDBObj _dbObj);

}
