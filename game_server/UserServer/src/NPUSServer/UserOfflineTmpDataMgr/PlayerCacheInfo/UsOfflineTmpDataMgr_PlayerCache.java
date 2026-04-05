package NPUSServer.UserOfflineTmpDataMgr.PlayerCacheInfo;

import NPUSServer.UserOfflineTmpDataMgr.Const_UsOfflineTmpData;
import NPUSServer.UserOfflineTmpDataMgr.UsOfflineTmpDataCore;
import NPUSServer.UserOfflineTmpDataMgr._AUsOfflineTmpDataMgr;

/**
 * 子嗣离线临时数据管理对象
 */
public class UsOfflineTmpDataMgr_PlayerCache extends _AUsOfflineTmpDataMgr<UserOfflineTmpDataInfo_PlayerCache, UserOfflineTmpDataMgr_PlayerCache> {
    public UsOfflineTmpDataMgr_PlayerCache(UsOfflineTmpDataCore _dataCore) {
        super(_dataCore, Const_UsOfflineTmpData.C_TmpDataType_PlayerCache);
    }

    @Override
    protected UserOfflineTmpDataMgr_PlayerCache _createCidTmpDataMgr(long _cid) {
        return new UserOfflineTmpDataMgr_PlayerCache(this, _cid);
    }
}
