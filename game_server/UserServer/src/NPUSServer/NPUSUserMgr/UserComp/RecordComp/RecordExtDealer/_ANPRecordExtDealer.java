package NPUSServer.NPUSUserMgr.UserComp.RecordComp.RecordExtDealer;

import NPEnum.ENPPlayerRecordParam;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;

public abstract class _ANPRecordExtDealer
{
    /**
     * 记录类型
     */
    public abstract ENPPlayerRecordParam getRecordParam();

    /**
     * 计数变化的触发
     * @param _userData
     * @param _oriCount
     * @param _curCount
     * @param _context
     */
    public abstract void onCountChg(NPUSUserData _userData, long _oriCount, long _curCount, NPPlayerContext _context);
}
