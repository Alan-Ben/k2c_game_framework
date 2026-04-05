using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using CommonEnum;

namespace GOE
{
    //活动相关
    public partial class GRefdataCoreMgr
    {
        //冲榜配置字典<活动id，配置列表>
        [NotNull] private  Dictionary<long, List<ActivityRankRushRefObj>> _m_activityRankRushRefDic = new Dictionary<long, List<ActivityRankRushRefObj>>();
        [NotNull] private  Dictionary<long, List<GActivityStepRewardRefObj>> _m_stepRewardRefDic = new Dictionary<long, List<GActivityStepRewardRefObj>>();
        //阶段奖励事件任务配置字典<step_reward_set_id，配置列表>
        [NotNull] private Dictionary<long, List<ActivityStepRewardSetEventTaskRefObj>> _m_stepRewardSetEventTaskRefDic = new Dictionary<long, List<ActivityStepRewardSetEventTaskRefObj>>();

        //活动换皮配置字典<活动id，<预制体资源key，换皮资源id>>
        [NotNull] private  Dictionary<long, Dictionary<string, long>> _m_dActivityPrefabSkinDic = new Dictionary<long, Dictionary<string, long>>();

        /// <summary>
        /// 获取活动相关
        /// </summary>
        private void _initActivityRefCore()
        {
            //初始化冲榜活动相关
            _m_activityRankRushRefDic.Clear();

            activityMainRefCore.dealAllRef(_activityRef =>
            {
                if (_activityRef == null) return;
                foreach (var rankId in _activityRef.rank_id_list)
                {
                    activityRankRushRefCore.dealAllRef(_ref =>
                    {
                        if (_ref != null && _ref.rank_id == rankId)
                        {
                            if (_m_activityRankRushRefDic.TryGetValue(_activityRef.activity_id, out List<ActivityRankRushRefObj> _activityRankRushRefList))
                                _activityRankRushRefList.Add(_ref);
                            else
                                _m_activityRankRushRefDic[_activityRef.activity_id] = new List<ActivityRankRushRefObj>() {_ref};
                        }
                    });
                }
            });

            activityPrefabSkinRefCore.dealAllRef(_ref =>
            {
                if (_ref == null || string.IsNullOrEmpty(_ref.key)) 
                    return;

                if (_m_dActivityPrefabSkinDic.TryGetValue(_ref.activity_id, out Dictionary<string, long> _activityPrefabSkinDic))
                    _activityPrefabSkinDic[_ref.key.ToLowerInvariant()] = _ref.ui_res_id;
                else
                    _m_dActivityPrefabSkinDic[_ref.activity_id] = new Dictionary<string, long>() { { _ref.key.ToLowerInvariant(), _ref.ui_res_id } };
            });
        }

        /// <summary>
        /// 根据活动id获取冲榜配置列表
        /// </summary>
        /// <param name="_activityId"></param>
        /// <param name="_refList"></param>
        public void getRankRushRefListByActivityId(long _activityId, List<ActivityRankRushRefObj> _refList)
        {
            if (_refList == null)
                return;

            _refList.Clear();
            if (_m_activityRankRushRefDic.TryGetValue(_activityId, out List<ActivityRankRushRefObj> _activityRankRushRefList))
            {
                if(_activityRankRushRefList != null)
                    _refList.AddRange(_activityRankRushRefList);
            }
        }

        /// <summary>
        /// 根据活动id和rankId获取冲榜配置
        /// </summary>
        /// <param name="_activityId"></param>
        /// <param name="_rankId"></param>
        /// <returns></returns>
        public ActivityRankRushRefObj getRankRushRefByActivityIdRankId(long _activityId, long _rankId)
        {
            if (_m_activityRankRushRefDic.TryGetValue(_activityId, out List<ActivityRankRushRefObj> _activityRankRushRefList))
            {
                if (_activityRankRushRefList != null)
                {
                    for (int i = 0; i < _activityRankRushRefList.Count; i++)
                    {
                        if (_activityRankRushRefList[i] != null && _activityRankRushRefList[i].rank_id == _rankId)
                            return _activityRankRushRefList[i];
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 获取冲榜活动id列表
        /// </summary>
        /// <returns></returns>
        public List<long> getRankRushActivityIdList()
        {
            return _m_activityRankRushRefDic.Keys.ToList();
        }

        /// <summary>
        /// 根据活动id获取排行奖励配置列表
        /// </summary>
        /// <param name="_refList"></param>
        public void getRankRewardRefList(long _rankId, List<GActivityRankRewardRefObj> _refList)
        {
            if (_refList == null)
                return;

            _refList.Clear();
            activityRankRewardRefCore.dealAllRef(_ref =>
            {
                if(_ref != null && _ref.rank_id == _rankId)
                    _refList.Add(_ref);
            });
        }
        
        /// <summary>
        /// 根据StepRewardSet id获取配置列表
        /// </summary>
        /// <param name="_refList"></param>
        public List<GActivityStepRewardRefObj> getStepRewardRefList(long _stepRewardSetId)
        {
            if (_m_stepRewardRefDic.TryGetValue(_stepRewardSetId, out List<GActivityStepRewardRefObj> _activityStepRewardRefList))
                return _activityStepRewardRefList;
            else
            {
                List<GActivityStepRewardRefObj> refList = new List<GActivityStepRewardRefObj>();
                activityStepRewardRefCore.dealAllRef(_ref =>
                {
                    if(_ref != null && _ref.step_reward_set_id == _stepRewardSetId)
                        refList.Add(_ref);
                });
                _m_stepRewardRefDic[_stepRewardSetId] = refList;
                return _m_stepRewardRefDic[_stepRewardSetId];
            }
        }
        
        public void getStepRewardRefList(long _stepRewardSetId, List<GActivityStepRewardRefObj> _refList)
        {
            if (_refList == null)
                return;

            _refList.Clear();
            List<GActivityStepRewardRefObj> refList = getStepRewardRefList(_stepRewardSetId);
            if(refList != null)
                _refList.AddRange(refList);
        }

        /// <summary>
        /// 根据StepRewardSet id获取事件任务配置列表
        /// </summary>
        /// <param name="_stepRewardSetId">阶段奖励设置id</param>
        /// <returns>事件任务配置列表</returns>
        public List<ActivityStepRewardSetEventTaskRefObj> getStepRewardSetEventTaskRefList(long _stepRewardSetId)
        {
            if (_m_stepRewardSetEventTaskRefDic.TryGetValue(_stepRewardSetId, out List<ActivityStepRewardSetEventTaskRefObj> _eventTaskRefList))
                return _eventTaskRefList;
            else
            {
                List<ActivityStepRewardSetEventTaskRefObj> refList = new List<ActivityStepRewardSetEventTaskRefObj>();
                stepRewardSetEventTaskRefCore.dealAllRef(_ref =>
                {
                    if (_ref != null && _ref.step_reward_set_id == _stepRewardSetId)
                        refList.Add(_ref);
                });
                _m_stepRewardSetEventTaskRefDic[_stepRewardSetId] = refList;
                return _m_stepRewardSetEventTaskRefDic[_stepRewardSetId];
            }
        }

        /// <summary>
        /// 根据StepRewardSet id获取事件任务配置列表
        /// </summary>
        /// <param name="_stepRewardSetId">阶段奖励设置id</param>
        /// <param name="_refList">输出的配置列表</param>
        public void getStepRewardSetEventTaskRefList(long _stepRewardSetId, List<ActivityStepRewardSetEventTaskRefObj> _refList)
        {
            if (_refList == null)
                return;

            _refList.Clear();
            List<ActivityStepRewardSetEventTaskRefObj> refList = getStepRewardSetEventTaskRefList(_stepRewardSetId);
            if (refList != null)
                _refList.AddRange(refList);
        }

        /// <summary>
        /// 获取换皮资源预制体id
        /// </summary>
        /// <param name="_activityId"></param>
        /// <param name="_key"></param>
        /// <returns></returns>
        public long getActivityPrefabSkinUIResId(long _activityId, string _key)
        {
            if (string.IsNullOrEmpty(_key) || _activityId <= 0)
                return 0;

            string targetKey = _key.ToLowerInvariant();
            if (_m_dActivityPrefabSkinDic.TryGetValue(_activityId, out Dictionary<string, long> _activityPrefabSkinDic))
            {
                if (_activityPrefabSkinDic.TryGetValue(targetKey, out long uiResId))
                    return uiResId;
                else
                    return 0;
            }
            else
                return 0;
        }

        /// <summary>
        /// 根据活动类型获取活动主配置列表
        /// </summary>
        /// <param name="_typeId">活动类型id</param>
        /// <param name="_refList">输出的配置列表</param>
        public void getActivityMainRefByType(ECommonActivityType _typeId, List<GActivityMainRefObj> _refList)
        {
            if (_refList == null)
                return;

            _refList.Clear();
            activityMainRefCore.dealAllRef(_ref =>
            {
                if (_ref != null && _ref.type_id == _typeId)
                    _refList.Add(_ref);
            });
        }
    }
}