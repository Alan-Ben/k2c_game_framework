package NPCommon.CommonCache.Hero;

import ALBasicServer.ALBasicMutex.MutexAtom;
import Common.HeroObj.Hero_ArenaShowInfo;
import Common.HeroObj.Hero_ArenaShowList;
import NPCommon.CommonCache.ComCachedDataBase;
import NPCommon.Log.CommLog;

import java.util.ArrayList;

public class _ACachedPlayerHeroList extends ComCachedDataBase
{
    private long _m_lCid;
    private ArrayList<Hero_ArenaShowInfo> _m_heroList;
    private MutexAtom _m_mutex;

    public _ACachedPlayerHeroList()
    {
        _m_heroList = new ArrayList<Hero_ArenaShowInfo>();
        _m_mutex = new MutexAtom();
    }

    protected void _lock()
    {
        _m_mutex.lock();
    }

    protected void _unlock()
    {
        _m_mutex.unlock();
    }

    public Hero_ArenaShowInfo lookupHero(long _heroId)
    {
        _lock();
        try
        {
            Hero_ArenaShowInfo heroInfo = null;
            for(int i = 0; i < _m_heroList.size(); i++)
            {
                heroInfo = _m_heroList.get(i);
                if(null == heroInfo)
                    continue;

                if(heroInfo.getHeroId() == _heroId)
                    return heroInfo;
            }

            return null;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 增加大臣
     * @param _cacheInfo
     */
    public void addHero(Hero_ArenaShowInfo _cacheInfo)
    {
        _lock();
        try
        {
            Hero_ArenaShowInfo preData = lookupHero(_cacheInfo.getHeroId());
            if(null != preData)
            {
                CommLog.error("hero already exist, heroId: " + _cacheInfo.getHeroId());

                //从队列移除旧数据
                _m_heroList.remove(preData);
            }

            _m_heroList.add(_cacheInfo);
        } finally
        {
            _unlock();
        }
    }

    public Hero_ArenaShowList makeArenaShowList()
    {
        Hero_ArenaShowList list = new Hero_ArenaShowList();
        _lock();
        try
        {
            for (Hero_ArenaShowInfo info : _m_heroList)
            {
                list.addHeroList(info);
            }
        } finally
        {
            _unlock();
        }
        return list;
    }
}
