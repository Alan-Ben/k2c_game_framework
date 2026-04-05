using System;
using System.Collections.Generic;
using Common.ActivityObj;
using CommonEnum;
using JetBrains.Annotations;

namespace GOE
{
    public class CommonActivityFactory
    {
        private static CommonActivityFactory _g_instance = new CommonActivityFactory();

        [NotNull]public static CommonActivityFactory instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new CommonActivityFactory();
                return _g_instance;
            }
        }
        
        //每种活动的数据创建方法字典，key为活动类型id，value为创建方法
        [NotNull]private Dictionary<ECommonActivityType, Func<Activity_Info, _ABaseActivityInfo>> _m_activityCreatorDic = new Dictionary<ECommonActivityType, Func<Activity_Info, _ABaseActivityInfo>>(){};


        public CommonActivityFactory()
        {
            regCreator(ECommonActivityType.NONE, (activityServerInfo) =>
            {
                DefaultActivityInfo activityInfo = new DefaultActivityInfo();
                activityInfo.init(activityServerInfo);
                return activityInfo;
            });
            regCreator(ECommonActivityType.RUSH_RANK, (activityServerInfo) =>
            {
                DefaultActivityInfo activityInfo = new DefaultActivityInfo();
                activityInfo.init(activityServerInfo);
                return activityInfo;
            });
            regCreator(ECommonActivityType.EARNINGS_GOAL, (activityServerInfo) =>
            {
                DefaultActivityInfo activityInfo = new DefaultActivityInfo();
                activityInfo.init(activityServerInfo);
                return activityInfo;
            });
            regCreator(ECommonActivityType.SEVEN_DAY_GOALS, (activityServerInfo) =>
            {
                DefaultActivityInfo activityInfo = new DefaultActivityInfo();
                activityInfo.init(activityServerInfo);
                return activityInfo;
            });
            regCreator(ECommonActivityType.RECHARGE_REBATE, (activityServerInfo) =>
            {
                DefaultActivityInfo activityInfo = new DefaultActivityInfo();
                activityInfo.init(activityServerInfo);
                return activityInfo;
            });
            regCreator(ECommonActivityType.ACTIVITY_FUND, (activityServerInfo) =>
            {
                DefaultActivityInfo activityInfo = new DefaultActivityInfo();
                activityInfo.init(activityServerInfo);
                return activityInfo;
            });
            regCreator(ECommonActivityType.RANK_GIFT_PACK, (activityServerInfo) =>
            {
                DefaultActivityInfo activityInfo = new DefaultActivityInfo();
                activityInfo.init(activityServerInfo);
                return activityInfo;
            });
            regCreator(ECommonActivityType.FIRST_TEAM, (activityServerInfo) =>
            {
                FirstTeamActivityInfo activityInfo = new FirstTeamActivityInfo();
                activityInfo.init(activityServerInfo);
                return activityInfo;
            });
        }

        public void regCreator(ECommonActivityType _activityType, Func<Activity_Info, _ABaseActivityInfo> _creator)
        {
            if(_m_activityCreatorDic.ContainsKey(_activityType))
            {
                Debug.LogError($"CommonActivityFactory regCreator failed, activityType {_activityType} already exist");
            }
            _m_activityCreatorDic.Add(_activityType, _creator);
        }

        public _ABaseActivityInfo createInstance(ECommonActivityType _activityType, Activity_Info _activityInfo)
        {
            _m_activityCreatorDic.TryGetValue(_activityType, out Func<Activity_Info, _ABaseActivityInfo> creator);
            if (creator == null)
            { 
                Debug.LogError($"CommonActivityFactory createInstance failed, activityType {_activityType} creator not exist");
                return null; 
            }
            else
            {
                return creator(_activityInfo);
            }
        }
    }
}