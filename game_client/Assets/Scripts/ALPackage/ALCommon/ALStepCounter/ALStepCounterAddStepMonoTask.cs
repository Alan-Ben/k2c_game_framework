using System;
using System.Collections.Generic;

using UnityEngine;

/****************
 * 单纯增加操作步骤的处理任务，为了避免在多线程外调用主线程操作而使用的任务
 **/
namespace ALPackage
{
    public class ALStepCounterAddStepMonoTask : _IALBaseMonoTask
    {
        /***************
         * 静态函数增加一个步骤统计对象的步骤
         **/
        public static void addStep(ALStepCounter _stepCounter)
        {
            if(null == _stepCounter)
                return ;

            ALStepCounterAddStepMonoTask task = new ALStepCounterAddStepMonoTask(_stepCounter);
            ALMonoTaskMgr.instance.addMonoTask(task);
        }

        private ALStepCounter _m_scStepCounter;

        public ALStepCounterAddStepMonoTask(ALStepCounter _stepCounter)
        {
            _m_scStepCounter = _stepCounter;
        }

        public void deal()
        {
            _m_scStepCounter.addDoneStepCount();
        }
    }
}
