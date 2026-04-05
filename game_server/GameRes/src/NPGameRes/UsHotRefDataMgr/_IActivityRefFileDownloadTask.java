package NPGameRes.UsHotRefDataMgr;

import NPCommon.Util.CallBack._ICallBackResultT;

public interface _IActivityRefFileDownloadTask
{
    void downloadFile(String _url, _ICallBackResultT<String> _callback);
}
