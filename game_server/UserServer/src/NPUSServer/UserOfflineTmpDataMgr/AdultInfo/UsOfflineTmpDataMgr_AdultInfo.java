package NPUSServer.UserOfflineTmpDataMgr.AdultInfo;

import NPUSServer.UserOfflineTmpDataMgr.Const_UsOfflineTmpData;
import NPUSServer.UserOfflineTmpDataMgr.UsOfflineTmpDataCore;
import NPUSServer.UserOfflineTmpDataMgr._AUsOfflineTmpDataMgr;

/**
 * 子嗣离线临时数据管理对象
 */
public class UsOfflineTmpDataMgr_AdultInfo extends _AUsOfflineTmpDataMgr<UserOfflineTmpDataInfo_AdultInfo, UserOfflineTmpDataMgr_AdultInfo> {
    public UsOfflineTmpDataMgr_AdultInfo(UsOfflineTmpDataCore _dataCore) {
        super(_dataCore, Const_UsOfflineTmpData.C_TmpDataType_AdultInfo);
    }

    @Override
    protected UserOfflineTmpDataMgr_AdultInfo _createCidTmpDataMgr(long _cid) {
        return new UserOfflineTmpDataMgr_AdultInfo(this, _cid);
    }
}
