package NPUSServer.UserOfflineTmpDataMgr.PlayerHeroListInfo;

import ALBasicServer.ALTask._IALAsynCallBackTask;
import NPCommon.DB._ASelectCallback;
import NPUSServer.UserOfflineTmpDataMgr._ATUserOfflineTmpDataMgr;
import NPUSServer.UserOfflineTmpDataMgr._AUsOfflineTmpDataMgr;
import USDB.Bo.PlayerHeroListCacheBO;

/**
 * 角色的子嗣离线临时数据管理对象
 *
 */
public class UserOfflineTmpDataMgr_HeroListCache extends _ATUserOfflineTmpDataMgr<UserOfflineTmpDataInfo_HeroListCache>
{
    protected UserOfflineTmpDataMgr_HeroListCache(_AUsOfflineTmpDataMgr _dataMgr, long _cid) {
        super(_dataMgr, _cid);
    }

    /**
     * 实际数据查询加载的处理，处理完毕后调用相关回调
     * @param _dataId
     * @param _callbackDealer
     */
    @Override
    protected void _loadDataOp(long _dataId, _IALAsynCallBackTask<UserOfflineTmpDataInfo_HeroListCache> _callbackDealer)
    {
        //调用数据库进行加载，这里不使用dataid，直接使用cid处理
        getUSServer().getBM().getBM(PlayerHeroListCacheBO.class).findOne("cid", getCid(), new _ASelectCallback<PlayerHeroListCacheBO>()
        {
            @Override
            public void dealFail()
            {
                if(null != _callbackDealer)
                    _callbackDealer.dealFail();
            }

            @Override
            public void dealSuc(PlayerHeroListCacheBO _bo)
            {
                //判断数据有效性
                if(null == _bo || _bo.getCid() != getCid())
                {
                    if (null != _callbackDealer)
                        _callbackDealer.dealFail();

                    return ;
                }

                //创建数据
                UserOfflineTmpDataInfo_HeroListCache tmpData = new UserOfflineTmpDataInfo_HeroListCache();
                //初始化数据
                tmpData.initFromBo(_bo);
                if (null != _callbackDealer)
                    _callbackDealer.dealSuc(tmpData);
            }
        });
    }
}
