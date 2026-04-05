using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    public partial class GRefdataCoreMgr
    {
        private void _initStageGoal()
        {
            stageGoalBigStepRefCore.refList.Sort((_a, _b) => _a.begins_from_small_step.CompareTo(_b.begins_from_small_step));
            stageGoalRefCore.refList.Sort((_a, _b) => _a.step.CompareTo(_b.step));

            // 客户端自动赋值上这个大阶段的结束小阶段
            List<StageGoalBigStepRefObj> bigStepList = stageGoalBigStepRefCore.refList;
            for (int i = 0; i < bigStepList.Count; i++)
            {
                StageGoalBigStepRefObj bigStep = bigStepList[i];
                // 如果是最后一个阶段了，就赋值为自己
                if (i == bigStepList.Count - 1)
                    bigStep.end_small_step = (int)stageGoalRefCore.refList[stageGoalRefCore.refList.Count - 1].step;
                else
                    bigStep.end_small_step = bigStepList[i + 1].begins_from_small_step - 1;
            }
        }
        
        
        /// <summary>
        /// 传入小阶段id，获取大阶段配置
        /// </summary>
        public StageGoalBigStepRefObj getStageGoalBigStepRefObj(int _smallStep)
        {
            int index = stageGoalBigStepRefCore.refList.binarySearchFloor(_smallStep, _ref => _ref.begins_from_small_step);
            if (index < 0)
            {
                ALLog.Error("[阶段目标] 没有找到大阶段配置，传入的小阶段为：" + _smallStep);
                return null;
            }
         
            return stageGoalBigStepRefCore.refList[index];
        }
    }
}