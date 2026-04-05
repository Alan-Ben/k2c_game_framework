package NPUSServer.NPUSUserMgr.UserComp.CacheComp;

import ALBasicCommon.ALSerializeMaker;
import ALBasicServer.ALProcess.ALStepCounter;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.CacheComp.CacheData.PlayerCacheData;
import NPUSServer.NPUSUserMgr.UserComp.CacheComp.CacheData.PlayerHeroListCacheData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;

import java.util.ArrayList;
import java.util.List;

/*************************
 * 玩家背包系统的组件
 * @author mj
 *
 */
public class PlayerCacheComponent extends _ANPUserComponent implements _IHandlerHolder
{
    private long _m_lSerialize;
    /**
     * 存储缓存数据类型队列的数据
     */
    private List<_IBasicPlayerCacheData> _m_lCacheDataList;

    //玩家缓存数据对象，方便外围调用修改
    private PlayerCacheData _m_pcPlayerCacheData;
    private PlayerHeroListCacheData _m_hlcHeroListCacheData;

    public PlayerCacheComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.PLAYER_CACHE);

        _m_lSerialize = ALSerializeMaker.makeNewSerialize();

        _m_lCacheDataList = new ArrayList<_IBasicPlayerCacheData>();

        //添加子数据对象
        _m_pcPlayerCacheData = new PlayerCacheData(_userData);
        _m_lCacheDataList.add(_m_pcPlayerCacheData);

        _m_hlcHeroListCacheData = new PlayerHeroListCacheData(_userData);
        _m_lCacheDataList.add(_m_hlcHeroListCacheData);
    }

    public long getSerialie() {return _m_lSerialize;}
    public PlayerCacheData getPlayerCacheData() {return _m_pcPlayerCacheData;}
    public PlayerHeroListCacheData getPlayerHeroListCachedata() {return _m_hlcHeroListCacheData;}

    @Override
    protected void _init()
    {
        ALStepCounter stepCounter = new ALStepCounter();
        stepCounter.chgTotalStepCount(_m_lCacheDataList.size());
        //都完成则设置初始化完成
        stepCounter.setAllDoneDelegate(this::setInited);

        //逐个数据初始化
        for(_IBasicPlayerCacheData cacheData : _m_lCacheDataList) {
            cacheData.init(stepCounter::addDoneStepCount);
        }
    }

    //获取需要依赖的加载组件项，无依赖则返回null
    @Override
    public ENPPlayerCompType[] getDependCompList()
    {
        return null;
    }

    @Override
    public void onInited()
    {
        //这里不处理，因为其他组件不一定计算完全
    }

    /***********
     * 玩家数据释放时候的处理，一般需要做关联处理。如注销监听等的处理
     */
    @Override
    public void dispose()
    {
        //保存所有数据
        saveAll();

        //重置序列号
        _m_lSerialize = ALSerializeMaker.makeNewSerialize();
    }

    //////////////////////////////
    // 其他业务部分开始
    //////////////////////////////

    /**
     * 在其他组件都完成时，调用的缓存数据初始化处理
     */
    public void onAllComponentInitedEnsureCache()
    {
        //逐个缓存数据对象调用处理
        for (_IBasicPlayerCacheData cacheData : _m_lCacheDataList) {
            if (null == cacheData)
                continue;

            cacheData.ensureCache();
        }

        //开启定时任务进行数据存储
        ALSynTaskManager.getInstance().regTask(new SyncPlayerCacheTimerTask(this), 5000);
    }

    /**
     * 保存所有cache数据的处理函数
     */
    public void saveAll()
    {
        //逐个缓存数据对象调用处理
        for (_IBasicPlayerCacheData cacheData : _m_lCacheDataList) {
            if (null == cacheData)
                continue;

            cacheData.saveAll();
        }
    }

    //////////////////////////////
    // 其他业务部分结束
    //////////////////////////////

}
