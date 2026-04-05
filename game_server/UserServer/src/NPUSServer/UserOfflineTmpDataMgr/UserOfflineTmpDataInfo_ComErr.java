package NPUSServer.UserOfflineTmpDataMgr;

import ALBasicProtocolPack._IALProtocolStructure;
import NPCommon.ErrMain.Result.Result;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

/**
 * 通用的错误数据错误处理，避免重复查询。如果查询错误会直接存储本类
 */
public class UserOfflineTmpDataInfo_ComErr implements  _IUserOfflineTmpDataInfo{
    private long _m_lDataId;
    private Result _m_iErr;

    public UserOfflineTmpDataInfo_ComErr(long _dataId, Result _err)
    {
        _m_lDataId = _dataId;
        _m_iErr = _err;
    }

    public Result getErrCode() {return _m_iErr;}

    /**
     * 返回数据Id，用于在管理器中校验数据匹配
     * @return
     */
    public long getDataId()
    {
        return _m_lDataId;
    }
    /**
     * 确认数据并返回，带入请求的协议方便判断可能不同的数据访问类型
     * @param _commiter
     * @param _requestMsg
     */
    public void comfirmData(_IWCGBasicRequestCommiter _commiter, _IALProtocolStructure _requestMsg)
    {
        if(null == _commiter)
            return ;

        _commiter.commitFailRes(_m_iErr.getCode());
    }
}
