package GameLogicServer.GroupMgr.ActivityFactory;


import CommonEnum.ECommonActivityType;
import GameLogicServer.GroupMgr.ActivityMgr.FirstTeamActivity.FirstTeamActivity;
import GameLogicServer.GroupMgr.ActivityMgr.FirstTeamActivity.FirstTeamActivityBoInitializer;
import GameLogicServer.GroupMgr.ActivityMgr._ATActivityInfo;
import GameLogicServer.GroupMgr.GroupInstanceInfo;
import NPCommon.DB.BM.BM;
import NPCommon.Log.CommLog;
import NPGameRes.Refs.Activity.RefActivity;

import java.util.HashMap;
import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

/******************************************************************
 * 活动对象工厂管理对象，根据配置获取后进行实例化
 *
 */
public class ActivityFactory
{
    //单例对象
    private static ActivityFactory _g_instance = new ActivityFactory();
    public static ActivityFactory getInstance()
    {
        return _g_instance;
    }

    // 活动类型ID -> 创建对象
    private Map<Integer, _IActivityCreator> _m_hmCreatorMap = new ConcurrentHashMap<>();
    // 活动类型ID -> BO批量初始化对象
    private Map<Integer, _IActivityBoInitializer> _m_hmBoInitializerMap = new ConcurrentHashMap<>();
    // 是否已经初始化
    private boolean _m_bIsInited = false;

    public synchronized boolean sInit()
    {
        if(_m_bIsInited)
            return true;

        _m_bIsInited = true;

        // 注册活动创建器与BO初始化器
        regCreator(ECommonActivityType.FIRST_TEAM, FirstTeamActivity::new);
        regBoInitializer(ECommonActivityType.FIRST_TEAM, new FirstTeamActivityBoInitializer());

        return true;
    }
    
    /**
     * 活动工厂数量
     * @return
     */
    public int getCreatorSize() {return _m_hmCreatorMap.size();}

    public boolean regCreator(ECommonActivityType _activityType, _IActivityCreator _creator)
    {
        return regCreator(_activityType.ordinal(), _creator);
    }

    public boolean regCreator(int _activityType, _IActivityCreator _creator)
    {
        if(_m_hmCreatorMap.containsKey(_activityType))
        {
            CommLog.error("duplicated reg activity of type:{} ", _activityType, new Exception());
            return false;
        }

        _m_hmCreatorMap.put(_activityType, _creator);
        return true;
    }

    /**
     * 注册活动BO批量初始化器
     * @param _activityType 活动类型
     * @param _initializer  初始化器实现
     */
    public boolean regBoInitializer(ECommonActivityType _activityType, _IActivityBoInitializer _initializer)
    {
        return regBoInitializer(_activityType.ordinal(), _initializer);
    }

    public boolean regBoInitializer(int _activityType, _IActivityBoInitializer _initializer)
    {
        if (_m_hmBoInitializerMap.containsKey(_activityType))
        {
            CommLog.error("duplicated reg bo initializer of type:{}", _activityType, new Exception());
            return false;
        }

        _m_hmBoInitializerMap.put(_activityType, _initializer);
        return true;
    }

    /**
     * 驱动所有已注册的BO初始化器，完成全量数据加载与内存分配
     * @param _bm              数据库访问对象
     * @param _activeInstances 当前活跃分组实例（key=instanceId）
     * @return true=全部初始化成功
     */
    public boolean sInitAllBoData(BM _bm, HashMap<Long, GroupInstanceInfo> _activeInstances)
    {
        for (Map.Entry<Integer, _IActivityBoInitializer> entry : _m_hmBoInitializerMap.entrySet())
        {
            if (!entry.getValue().sInitAll(_bm, _activeInstances))
            {
                CommLog.error("ActivityFactory.sInitAllBoData - activityType:{} bo init fail.", entry.getKey());
                return false;
            }
        }
        return true;
    }

    /**
     * 创建活动对象实例
     * @param _groupId
     * @param _activityId
     * @return
     */
    public _ATActivityInfo createInstance(long _groupId, long _activityId)
    {
        RefActivity ref = RefActivity.getMgr().get(_activityId);
        if (null == ref)
        {
            CommLog.error("create activity failed, not found ref for activityId:{}", _activityId);
            return null;
        }

        _IActivityCreator creator = _m_hmCreatorMap.get(ref.type_id);
        if (null == creator)
        {
            CommLog.error("create activity id:{} typeId:{} fail, not register", _activityId, ref.type_id);
            return null;
        }

        return creator.create(_groupId, _activityId);
    }
}
