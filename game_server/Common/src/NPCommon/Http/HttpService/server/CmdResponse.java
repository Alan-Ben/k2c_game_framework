package NPCommon.Http.HttpService.server;

import NPCommon.Util.CallBack._ICallBackT;

public class CmdResponse implements _IResponse
{
    private _ICallBackT<String> _m_callBack;

    public CmdResponse(_ICallBackT<String> _callBack)
    {
        _m_callBack = _callBack;
    }

    @Override
    public void response(String result)
    {
        _m_callBack.onRunOver(result);
    }

    @Override
    public void response(int code, String result)
    {
        _m_callBack.onRunOver("code: " + code + " result:" + result);
    }

    @Override
    public void error(int code, String format, Object... param)
    {
        String msg = String.format(format, param);
        _m_callBack.onRunOver("code: " + code + " msg:" + msg);
    }
}
