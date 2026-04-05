using System;
using System.Collections.Generic;

namespace ALPackage
{
    /*********************
     * 延迟的任务信息
     **/
    public struct ALDelayMonoTaskInfo
    {
        //任务对象
        public _IALBaseMonoTask task;
        //延迟时间
        public float delayTime;
        //是否Late处理对象
        public bool isLateTask;

        public ALDelayMonoTaskInfo(_IALBaseMonoTask _task, float _delayTime, bool _isLateTask)
        {
            task = _task;
            delayTime = _delayTime;
            isLateTask = _isLateTask;
        }
    }
}
