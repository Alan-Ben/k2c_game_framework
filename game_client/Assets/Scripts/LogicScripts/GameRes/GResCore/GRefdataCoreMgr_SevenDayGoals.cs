using System.Collections.Generic;
using JetBrains.Annotations;

namespace GOE
{
    public partial class GRefdataCoreMgr
    {
        // 开服七日目标的每一天的任务列表 [day][taskList]
        [NotNull] private readonly List<List<SevenDayGoalsTaskRewardRefObj>> sevenDayGoalsDayTasks = new List<List<SevenDayGoalsTaskRewardRefObj>>();
        
        private void _initSevenDayGoals()
        {
            sevenDayGoalsDayTasks.Clear();
            foreach (SevenDayGoalsTaskRewardRefObj refObj in sevenDayGoalsTaskRewardRefCore.refList)
            {
                if (refObj == null)
                    continue;

                // 赋值上任务配置
                refObj.task_ref = sevenDayGoalsTaskRefCore.getRef(refObj.task_id);
                
                // 把任务按照天数整理
                if (sevenDayGoalsDayTasks.Count < refObj.day)
                    sevenDayGoalsDayTasks.SetCount(refObj.day);
                List<SevenDayGoalsTaskRewardRefObj> taskList = sevenDayGoalsDayTasks[refObj.day - 1];
                if (taskList == null)
                {
                    taskList = new List<SevenDayGoalsTaskRewardRefObj>();
                    sevenDayGoalsDayTasks[refObj.day - 1] = taskList;
                }
                taskList.Add(refObj);
            }

            // 按照需求分数进行排序，防止乱配
            sevenDayGoalsStepRewardRefCore.refList.Sort((_a, _b) => _a.need_score.CompareTo(_b.need_score));
        }
        
        
        /// <summary>
        /// 获取指定天的开服七日任务列表
        /// </summary>
        [ItemNotNull]
        public List<SevenDayGoalsTaskRewardRefObj> getSevenDayGoalsDayTasks(int _day)
        {
            if (_day < 1 || _day > sevenDayGoalsDayTasks.Count)
                return null;
            
            return sevenDayGoalsDayTasks[_day - 1];
        }
        public int getSevenDayGoalsMaxDay()
        {
            return sevenDayGoalsDayTasks.Count;
        }
    }
}