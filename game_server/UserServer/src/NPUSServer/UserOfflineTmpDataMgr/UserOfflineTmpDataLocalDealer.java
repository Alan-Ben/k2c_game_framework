package NPUSServer.UserOfflineTmpDataMgr;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CallBack._ICallBackResultT;

/**
 * 当时无数据时，临时存储请求的相关请求信息对象
 * 方便查询到之后对结果进行返回
 */
public class UserOfflineTmpDataLocalDealer implements  _IUserOfflineTmpDataSearchDoneDealer {
    private _ICallBackResultT _m_callback;

    public UserOfflineTmpDataLocalDealer(_ICallBackResultT _callback)
    {
        _m_callback = _callback;
    }

    /**
     * 查询到数据后的处理函数
     */
    public void onSearchDone_InLock(_IUserOfflineTmpDataInfo _dataInfo)
    {
        if(null == _m_callback)
            return ;

        //开启任务处理
        ALSynTaskManager.getInstance().regTask(new _IALSynTask() {
            @Override
            public void run() {
                if(null == _dataInfo) {
                    if(null != _m_callback)
                        _m_callback.onRunOver(CommErr.OBJ_ERR, null);
                }
                else if(_dataInfo instanceof UserOfflineTmpDataInfo_ComErr)
                {
                    if(null != _m_callback)
                        _m_callback.onRunOver(((UserOfflineTmpDataInfo_ComErr)_dataInfo).getErrCode(), null);
                }
                else
                {
                    if(null != _m_callback)
                        _m_callback.onRunOver(Result.SUCC, _dataInfo);
                }
            }
        });
    }
}
