package NPUSServer.UserOfflineTmpDataMgr.PlayerHeroListInfo;

import Common.HeroObj.Hero_ArenaShowInfo;
import Common.HeroObj.Hero_ArenaShowList;
import NPCommon.CommonCache.Hero._ACachedPlayerHeroList;
import NPUSServer.UserOfflineTmpDataMgr._IUserOfflineTmpDataInfo;
import USDB.Bo.PlayerHeroListCacheBO;

import java.nio.ByteBuffer;

/**
 * 子嗣数据处理对象
 */
public class UserOfflineTmpDataInfo_HeroListCache extends _ACachedPlayerHeroList implements _IUserOfflineTmpDataInfo
{
    //数据库数据对象
    private PlayerHeroListCacheBO _m_bo;

    public UserOfflineTmpDataInfo_HeroListCache()
    {
        _m_bo = null;
    }

    protected PlayerHeroListCacheBO _getBO() {return _m_bo;}

    /**
     * 初始化缓存数据
     */
    public void initFromBo(PlayerHeroListCacheBO _bo)
    {
        _m_bo = _bo;

        Hero_ArenaShowList heroList = new Hero_ArenaShowList();
        heroList.readPackage(ByteBuffer.wrap(_bo.getHeroList()));

        //逐个数据放入
        for(Hero_ArenaShowInfo heroInfo : heroList.getHeroList())
        {
            addHero(heroInfo);
        }
    }

    /**
     * 返回数据Id，用于在管理器中校验数据匹配
     * @return
     */
    public long getDataId()
    {
        return _m_bo.getCid();
    }
}
