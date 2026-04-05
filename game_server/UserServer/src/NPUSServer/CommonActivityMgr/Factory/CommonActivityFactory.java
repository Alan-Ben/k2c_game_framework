package NPUSServer.CommonActivityMgr.Factory;

import CommonEnum.ECommonActivityType;
import NPCommon.Log.CommLog;
import NPGameRes.Refs.Activity.RefActivity;
import NPUSServer.CommonActivityMgr.Activities.EarningsGoal.EarningsGoalActivity;
import NPUSServer.CommonActivityMgr.Activities.EmptyActivity.ActivityFundActivity;
import NPUSServer.CommonActivityMgr.Activities.EmptyActivity.RankGiftPackActivity;
import NPUSServer.CommonActivityMgr.Activities.EmptyActivity.RushRankActivity;
import NPUSServer.CommonActivityMgr.Activities.EmptyActivity.SevenDayGoalsActivity;
import NPUSServer.CommonActivityMgr.Activities.FirstTeamActivity.FirstTeamActivity;
import NPUSServer.CommonActivityMgr.Activities.RechargeRebate.RechargeRebateActivity;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUserServer;
import USDB.Bo.ActivityBaseBO;

import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

/******************************************************************
 * 活动对象工厂管理对象，根据配置获取后进行实例化
 *
 */
public class CommonActivityFactory
{
    private static CommonActivityFactory _g_instance = new CommonActivityFactory();
    public static CommonActivityFactory getInstance()
    {
        if(null == _g_instance)
            _g_instance = new CommonActivityFactory();
        return _g_instance;
    }


    /******************************
     *活动类型ID->创建对象
     */
    private Map<Integer, _IActivityCreator> _m_hmCreatorMap = new ConcurrentHashMap<>();
    private boolean _m_bIsInited = false;

    public synchronized boolean s_init()
    {
        if(_m_bIsInited)
            return true;

        _m_bIsInited = true;

        //注册活动
        regCreator(ECommonActivityType.RUSH_RANK, RushRankActivity::new);
        regCreator(ECommonActivityType.EARNINGS_GOAL, EarningsGoalActivity::new);
        regCreator(ECommonActivityType.SEVEN_DAY_GOALS, SevenDayGoalsActivity::new);
        regCreator(ECommonActivityType.RECHARGE_REBATE, RechargeRebateActivity::new);
        regCreator(ECommonActivityType.ACTIVITY_FUND, ActivityFundActivity::new);
        regCreator(ECommonActivityType.RANK_GIFT_PACK, RankGiftPackActivity::new);
        regCreator(ECommonActivityType.FIRST_TEAM, FirstTeamActivity::new);

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
            CommLog.error("duplicated regist activity of type:{} ",_activityType, new Exception());
            return false;
        }
        _m_hmCreatorMap.put(_activityType, _creator);
        return true;
    }

    /*************************
     * 根据className构造实例对象
     * @param _bo
     * @return
     */
    public _AActivityBase createInstance(NPUserServer _server, ActivityBaseBO _bo)
    {
        RefActivity ref = RefActivity.getMgr().get(_bo.getActivityId());
        if (null == ref)
        {
            CommLog.error("create activity failed ,not found ref for activityId:", _bo.getActivityId());
            return null;
        }
        _IActivityCreator creator = _m_hmCreatorMap.get(ref.type_id);
        if (null == creator)
        {
            CommLog.error("create activity id:{} typeId:{} fail, not register",_bo.getActivityId(), ref.type_id);
            return null;
        }
        return creator.create(_server, _bo);
    }


}
