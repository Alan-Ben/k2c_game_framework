package NPCommon.NPLogDB;

import NPCommon.Context._IContext;
import NPCommon.DB.BM.BM;
import NPCommon.Util.CommonFunc;

public class CommLogDB
{
    public static void log(BM _bm, BaseLogBo _bo)
    {
        log(_bm, _bo, null);
    }

    /******
     * 写入日志
     * @param _bo
     * @param _context
     */
    public static void log(BM _bm, BaseLogBo _bo, _IContext _context)
    {
        if (null != _context)
        {
            _bo.setEventId(_bm, _context.getContextId());
            _bo.setGuid(_bm, _context.getGuid());
        }
        _bo.setDateTime(_bm, CommonFunc.getNowTagYYYYMMDD());
        _bo.setTimestamp(_bm, CommonFunc.getNowTimeSec());
        _bo.insert(_bm);
    }
}
