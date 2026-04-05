package NPUSServer.NPUSUserMgr.UserComp.CacheComp.CacheData;

import Common.HeroObj.Hero_ArenaShowInfo;
import Common.HeroObj.Hero_ArenaShowList;
import NPCommon.DB._ASelectCallback;
import NPCommon.Util.CallBack._ICallBack;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.CacheComp._IBasicPlayerCacheData;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroInfo;
import NPUSServer.NPUserServer;
import NPUSServer.UserOfflineTmpDataMgr.PlayerHeroListInfo.UserOfflineTmpDataInfo_HeroListCache;
import USDB.Bo.PlayerHeroListCacheBO;

import java.util.ArrayList;

/**
 * 玩家基础数据cache
 */
public class PlayerHeroListCacheData extends UserOfflineTmpDataInfo_HeroListCache implements  _IBasicPlayerCacheData
{
    //存储服务器对象
    private NPUSUserData _m_userData;

    //设置是否需要保存
    private boolean _m_bNeedSaveAll;

    public PlayerHeroListCacheData(NPUSUserData _userData)
    {
        _m_userData = _userData;
        _m_bNeedSaveAll = false;
    }

    public NPUserServer getServer() {return _m_userData.getUSServer();}

    @Override
    public void init(_ICallBack _callback) {
        _m_userData.getUSServer().getBM().getBM(PlayerHeroListCacheBO.class).findOne("cid", _m_userData.getCid(), new _ASelectCallback<PlayerHeroListCacheBO>()
        {
            @Override
            public void dealFail()
            {
                //加载失败此时也需要直接设置完成，需要等待玩家数据都初始化完成之后才可以进行插入处理
                //调用回调
                _callback.onRunOver();
            }

            @Override
            public void dealSuc(PlayerHeroListCacheBO _bo)
            {
                //初始化数据
                initFromBo(_bo);

                //调用回调
                _callback.onRunOver();
            }
        });
    }

    /**
     * 确保缓存数据的处理，在玩家初始化完成后调用，确保数据已经加载完成
     */
    @Override
    public void ensureCache()
    {
        //直接调用bo的savemarked处理
        if(null == _getBO())
        {
            //创建最简缓存数据
            PlayerHeroListCacheBO newCacheBo = new PlayerHeroListCacheBO();
            newCacheBo.setCid(getServer().getBM(), _m_userData.getCid());

            Hero_ArenaShowList heroList = new Hero_ArenaShowList();
            //逐个玩家大臣数据进行处理
            ArrayList<HeroInfo> heroInfoList =  _m_userData.getHeroComponent().getAllHeroList();
            for(HeroInfo heroInfo : heroInfoList)
            {
                heroList.getHeroList().add(new Hero_ArenaShowInfo(heroInfo.getHeroId(), heroInfo.getSkinId(), heroInfo.getLevel(), heroInfo.getPower()));
            }
            newCacheBo.setHeroList(getServer().getBM(), heroList.makePackage().array());

            newCacheBo.insert(getServer().getBM());

            //初始化数据
            initFromBo(newCacheBo);
        }
    }

    @Override
    public void saveAll()
    {
        //直接调用bo的savemarked处理
        if(null == _getBO())
            return ;

        //不需要保存则不处理
        if(!_m_bNeedSaveAll)
            return ;

        //构造数据
        Hero_ArenaShowList saveData = makeArenaShowList();
        //设置数据
        _getBO().setHeroList(getServer().getBM(), saveData.makePackage().array());

        _getBO().saveAll(_m_userData.getUSServer().getBM());
    }

    //////////////////
    // 其他业务函数
    //////////////////

    public void setNeedSaveAll()
    {
        _m_bNeedSaveAll = true;
    }
    //////////////////
    // 其他业务函数 结束
    //////////////////

    /////////////////
    // 各属性变化的处理函数
    //////////////////
    public void onGainHero(HeroInfo _heroInfo)
    {
        if(null == _getBO() || null == _heroInfo)
            return ;

        addHero(new Hero_ArenaShowInfo(_heroInfo.getHeroId(), _heroInfo.getSkinId(), _heroInfo.getLevel(), _heroInfo.getPower()));
        //可能变动太频繁。不即时存储，等待定时存储
        setNeedSaveAll();
    }

    public void updateSkinId(HeroInfo _heroInfo)
    {
        if(null == _getBO() || null == _heroInfo)
            return ;

        //查询数据并更新
        Hero_ArenaShowInfo heroInfo = lookupHero(_heroInfo.getHeroId());
        if(null == heroInfo)
            addHero(new Hero_ArenaShowInfo(_heroInfo.getHeroId(), _heroInfo.getSkinId(), _heroInfo.getLevel(), _heroInfo.getPower()));
        else
            heroInfo.setSkinId(_heroInfo.getSkinId());
        //可能变动太频繁。不即时存储，等待定时存储
        setNeedSaveAll();
    }

    public void updatePower(HeroInfo _heroInfo)
    {
        if(null == _getBO() || null == _heroInfo)
            return ;

        //查询数据并更新
        Hero_ArenaShowInfo heroInfo = lookupHero(_heroInfo.getHeroId());
        if(null == heroInfo)
            addHero(new Hero_ArenaShowInfo(_heroInfo.getHeroId(), _heroInfo.getSkinId(), _heroInfo.getLevel(), _heroInfo.getPower()));
        else
            heroInfo.setPower(_heroInfo.getPower());
        //可能变动太频繁。不即时存储，等待定时存储
        setNeedSaveAll();
    }

    public void updateLevel(HeroInfo _heroInfo)
    {
        if(null == _getBO() || null == _heroInfo)
            return ;

        //查询数据并更新
        Hero_ArenaShowInfo heroInfo = lookupHero(_heroInfo.getHeroId());
        if(null == heroInfo)
            addHero(new Hero_ArenaShowInfo(_heroInfo.getHeroId(), _heroInfo.getSkinId(), _heroInfo.getLevel(), _heroInfo.getPower()));
        else
            heroInfo.setLevel(_heroInfo.getLevel());
        //可能变动太频繁。不即时存储，等待定时存储
        setNeedSaveAll();
    }
    ///////////////////////////
    // 各属性变化的处理函数结束
    ///////////////////////////
}
