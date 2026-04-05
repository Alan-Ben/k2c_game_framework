package NPUSServer.UserOfflineTmpDataMgr.PlayerHeroListInfo;

import NPUSServer.UserOfflineTmpDataMgr.Const_UsOfflineTmpData;
import NPUSServer.UserOfflineTmpDataMgr.UsOfflineTmpDataCore;
import NPUSServer.UserOfflineTmpDataMgr._AUsOfflineTmpDataMgr;

/**
 * 子嗣离线临时数据管理对象
 */
public class UsOfflineTmpDataMgr_HeroListCache extends _AUsOfflineTmpDataMgr<UserOfflineTmpDataInfo_HeroListCache, UserOfflineTmpDataMgr_HeroListCache> {
    public UsOfflineTmpDataMgr_HeroListCache(UsOfflineTmpDataCore _dataCore) {
        super(_dataCore, Const_UsOfflineTmpData.C_TmpDataType_PlayerHeroListCache);
    }

    @Override
    protected UserOfflineTmpDataMgr_HeroListCache _createCidTmpDataMgr(long _cid) {
        return new UserOfflineTmpDataMgr_HeroListCache(this, _cid);
    }
}
