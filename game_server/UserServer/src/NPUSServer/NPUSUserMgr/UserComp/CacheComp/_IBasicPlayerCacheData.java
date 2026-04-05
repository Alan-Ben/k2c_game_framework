package NPUSServer.NPUSUserMgr.UserComp.CacheComp;

import NPCommon.Util.CallBack._ICallBack;

/***
 * 玩家的基础cache数据管理对象
 * 本对象根据不同的数据进行不同的初始化
 * 每个数据在cache中进行临时记录，在离线的时候进行统一的存储
 * 在线情况下由管理器统一每1分钟进行一次存储
 */
public interface _IBasicPlayerCacheData
{
    /**
     * 初始化缓存数据的处理，完成初始化后调用callback
     * @param _callback
     */
    void init(_ICallBack _callback);

    /**
     * 确保缓存数据的处理，在玩家初始化完成后调用，确保数据已经加载完成
     */
    void ensureCache();

    /**
     * 保存修改数据的处理，一般在玩家在线一段事件或者数据离线或者卸载的时候调用
     */
    void saveAll();
}
