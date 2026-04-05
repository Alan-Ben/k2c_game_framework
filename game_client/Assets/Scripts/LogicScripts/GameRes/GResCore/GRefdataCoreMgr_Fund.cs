using System.Collections.Generic;

namespace GOE
{
    public partial class GRefdataCoreMgr
    {
        private void _initFund()
        {
            //第一步：初始化基金主表的等级列表
            activityFundRefCore.dealAllRef(_fundRef =>
            {
                if (_fundRef == null)
                    return;

                _fundRef.level_ref_list ??= new List<ActivityFundLevelRefObj>();
                _fundRef.level_ref_list.Clear();

                //获取该基金的所有等级
                activityFundLevelRefCore.dealAllRef(_levelRef =>
                {
                    if (_levelRef == null)
                        return;

                    if (_levelRef.activity_fund_id == _fundRef.activity_fund_id)
                        _fundRef.level_ref_list.Add(_levelRef);
                });

                //按等级排序
                _fundRef.level_ref_list.Sort((_a, _b) => _a.level.CompareTo(_b.level));

                //初始化每个 level 的 step 列表
                //假设：step 和 level 都是从 1 开始且连续递增的
                //1. 先获取该基金的所有 step 并排序
                List<ActivityFundStepRefObj> stepList = new List<ActivityFundStepRefObj>();
                activityFundStepRefCore.dealAllRef(_stepRef =>
                {
                    if (_stepRef != null && _stepRef.activity_fund_id == _fundRef.activity_fund_id)
                        stepList.Add(_stepRef);
                });
                stepList.Sort((_a, _b) => _a.step.CompareTo(_b.step));

                //2. 设置每个 step 的 prev_step_need_count
                for (int j = 0; j < stepList.Count; j++)
                {
                    stepList[j].prev_step_need_count = j > 0 ? stepList[j - 1].need_count : 0;
                }

                //3. 线性分配 step 到各个 level（优化版：假设连续性）
                int stepIndex = 0;
                for (int i = 0; i < _fundRef.level_ref_list.Count; i++)
                {
                    ActivityFundLevelRefObj levelRef = _fundRef.level_ref_list[i];
                    levelRef.step_ref_list ??= new List<ActivityFundStepRefObj>();
                    levelRef.step_ref_list.Clear();

                    //获取前一个等级的 last_step
                    long prevLastStep = i > 0 ? _fundRef.level_ref_list[i - 1].last_step : 0;

                    //如果前一个 level 的 last_step 为负数（无限大），当前 level 不会有任何 step
                    if (prevLastStep < 0)
                        continue;

                    //当前 level 的 last_step（负数表示无限大）
                    long currentLastStep = levelRef.last_step;
                    bool isInfinite = currentLastStep < 0;

                    //分配 step：step > prevLastStep && step <= currentLastStep (或无限大)
                    while (stepIndex < stepList.Count)
                    {
                        long stepValue = stepList[stepIndex].step;

                        //如果是无限大，包含所有剩余 step
                        if (isInfinite)
                        {
                            levelRef.step_ref_list.Add(stepList[stepIndex]);
                            stepIndex++;
                        }
                        //否则检查 step 是否在当前 level 范围内
                        else if (stepValue <= currentLastStep)
                        {
                            levelRef.step_ref_list.Add(stepList[stepIndex]);
                            stepIndex++;
                        }
                        else
                        {
                            //当前 step 超出范围，处理下一个 level
                            break;
                        }
                    }
                }
            });
        }
    }
}