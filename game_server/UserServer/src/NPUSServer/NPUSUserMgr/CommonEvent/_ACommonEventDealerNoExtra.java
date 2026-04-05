package NPUSServer.NPUSUserMgr.CommonEvent;

import Common.EventObj.CommonEvent_DoneInfo;
import NPCommon.ErrMain.Result.ResultOne;
import NPGameRes.Refs.CommonEvent._ARefCommonEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;

/**
 * 通用事件处理器基类（无需事件处理过程基类，即直接结算领奖的事件）
 */
public abstract class _ACommonEventDealerNoExtra<T extends _ARefCommonEvent> extends _ACommonEventDealer<T>
{
    @Override
    public ResultOne<CommonEvent_DoneInfo> _dealInfo(NPUSUserData _userData, T _eventRef, byte[] _dealInfo, NPPlayerContext _context)
    {
        return _dealEvent(_userData, _eventRef, _context);
    }

    public abstract ResultOne<CommonEvent_DoneInfo> _dealEvent(NPUSUserData _userData, T _eventRef, NPPlayerContext _context);
}
