package NPUSServer.NPUSUserMgr.CommonEvent;

import Common.EventObj.CommonEvent_DoneInfo;
import NPCommon.ErrMain.Result.Result;

public class CommonEventDealResult
{
    private Result _m_result;
    private CommonEvent_DoneInfo _m_doneInfo;

    public CommonEventDealResult(Result _result, CommonEvent_DoneInfo _doneInfo)
    {
        _m_result = _result;
        _m_doneInfo = _doneInfo;
    }
}
