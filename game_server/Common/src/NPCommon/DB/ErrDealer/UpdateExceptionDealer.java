package NPCommon.DB.ErrDealer;

import ALMySqlCommon.ALMySqlCommonObj.ALMySqlErrDealer._IALMySqlDBExeErrorDealer;
import NPCommon.Log.CommLog;

import java.util.ArrayList;

public class UpdateExceptionDealer implements _IALMySqlDBExeErrorDealer
{
    private boolean _m_bHasException;

    @Override
    public void onException(Exception e, String _query, ArrayList<byte[]> _byteList)
    {
        _m_bHasException = true;
        CommLog.error("[UPDATE_ERROR] {}", _query);
        CommLog.error("", e);
    }

    public boolean isHasException()
    {
        return _m_bHasException;
    }

    public void markHasException()
    {
        _m_bHasException = true;
    }
}
