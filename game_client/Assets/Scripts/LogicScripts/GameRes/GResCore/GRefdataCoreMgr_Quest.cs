using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    //任务相关
    public partial class GRefdataCoreMgr
    {
        //系统任务字典，<groupId,<stepId,QuestSystemRefObj>>
        [NotNull] private Dictionary<long, Dictionary<long, SystemQuestRefObj>> _m_dSystemQuestDic = new Dictionary<long, Dictionary<long, SystemQuestRefObj>>();

        private void _initQuestRefCore()
        {
            questTargetMap.dealAllRef(
                (_targetRef) => 
                {
                    if (null == _targetRef)
                        return;
                    
                    QuestStepRefObj stepRef = questStepMap.getRef(_targetRef.step_id);
                    if(null == stepRef)
                    {
                        ALLog.Error($"can not find quest step{_targetRef.step_id} for target{_targetRef.id}");
                        return;
                    }

                    //添加目标
                    stepRef.addTarget(_targetRef);
                });

            systemQuestRefCore.dealAllRef(_systemQuestRef =>
            {
                if (_systemQuestRef != null)
                {
                    if (_m_dSystemQuestDic.TryGetValue(_systemQuestRef.group_id, out Dictionary<long, SystemQuestRefObj> _subDic))
                    {
                        _subDic?.TryAdd(_systemQuestRef.step, _systemQuestRef);
                    }
                    else
                    {
                        Dictionary<long, SystemQuestRefObj> newSubDic = new Dictionary<long, SystemQuestRefObj>();
                        newSubDic[_systemQuestRef.step] = _systemQuestRef;
                        _m_dSystemQuestDic[_systemQuestRef.group_id] = newSubDic;
                    }
                }
            });
        }

        /// <summary>
        /// 获取任务系统配表数据
        /// </summary>
        /// <param name="_groupId"></param>
        /// <param name="_step"></param>
        /// <returns></returns>
        public SystemQuestRefObj getSystemQuestRefObj(long _groupId, long _step)
        {
            if (_m_dSystemQuestDic.TryGetValue(_groupId, out Dictionary<long, SystemQuestRefObj> _subDic))
            {
                if (_subDic != null && _subDic.TryGetValue(_step, out SystemQuestRefObj _ref))
                    return _ref;
            }

            return null;
        }
    }
}