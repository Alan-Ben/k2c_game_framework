package GameLogicServer.GroupMgr.ActivityFactory;

import GameLogicServer.GroupMgr.GroupInstanceInfo;
import NPCommon.DB.BM.BM;

import java.util.HashMap;

/**
 * 活动BO数据批量初始化接口
 *
 * 主要功能：
 * 1. 每种活动类型注册一个实现
 * 2. 服务器启动时一次性加载全量BO数据并在内存中按groupId分配给对应实例
 */
@FunctionalInterface
public interface _IActivityBoInitializer
{
    /**
     * 批量加载本活动类型全部BO数据，并注入对应活动实例
     * @param _bm              数据库访问对象
     * @param _activeInstances 当前活跃分组实例（key=instanceId）
     * @return true=加载成功
     */
    boolean sInitAll(BM _bm, HashMap<Long, GroupInstanceInfo> _activeInstances);
}

