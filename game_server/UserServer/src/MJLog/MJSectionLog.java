package MJLog;

import MJLog.Bo.MjSnapshotLogBO;
import NPCommon.DB.BM.BM;
import NPUSServer.NPUSUserMgr.NPUSUserData;

public class MJSectionLog
{
    /**
     * 截面日志
     * ["long", "cid", "角色id"],
     * @param _userdata
     */
    public static void sectionLog(NPUSUserData _userdata)
    {
        BM bmObj = _userdata.getUSServer().getBM();

        MjSnapshotLogBO logBo = new MjSnapshotLogBO();
        logBo.setCid(bmObj, _userdata.getCid());

        logBo.insert(bmObj);
    }
}
