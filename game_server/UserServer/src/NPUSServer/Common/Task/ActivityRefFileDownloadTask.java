package NPUSServer.Common.Task;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import NPCommon.ErrMain.HttpErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Http.HttpAsyncClient;
import NPCommon.Http._AResponseHandler;
import NPCommon.Log.CommLog;
import NPCommon.Util.CallBack._ICallBackResultT;
import org.apache.http.Header;

public class ActivityRefFileDownloadTask implements _IALSynTask
{

    private _ICallBackResultT<String> _m_callback;
    private String _m_url;
    private int _m_iMaxRetryNum;
    private int _m_iRetryNum;

    public ActivityRefFileDownloadTask(String _url, int _iMaxRetryNum, _ICallBackResultT<String> _callback)
    {
        _m_url = _url;
        _m_callback = _callback;
        _m_iMaxRetryNum = _iMaxRetryNum;
    }

    @Override
    public void run()
    {
        final ActivityRefFileDownloadTask theTask = this;
        HttpAsyncClient.startHttpGet(_m_url, new _AResponseHandler()
        {
            @Override
            public void onComplete(Header[] headers, int _code, String _response)
            {
                if (_code != 200)
                {
                    CommLog.error("download from url:{},got err code:{}", _m_url, _code);
                    _m_callback.onRunOver(HttpErr.HTTP_RESPONSE_ERROR, "");
                } else
                {
                    _m_callback.onRunOver(Result.SUCC, _response);
                }
            }

            @Override
            public void onFailed(Exception e)
            {
                CommLog.error("download from url:{},caught exception:", _m_url, e);
                if (_m_iRetryNum < _m_iMaxRetryNum)
                {
                    _m_iRetryNum++;

                    CommLog.error("download from url:{},time out!,retry {} ...", _m_url, _m_iRetryNum);
                    ALSynTaskManager.getInstance().regTask(theTask, 5000);
                } else
                {
                    CommLog.error("download from url:{},retry times used:{},return failed ", _m_url, _m_iRetryNum);
                    _m_callback.onRunOver(HttpErr.HTTP_RESPONSE_ERROR, "");
                }
            }
        });

    }
}
