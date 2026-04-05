using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 收获操作的数字控制类中的单个任务对象
    /// 每个任务对象是互相独立的，根据序列号进行创建和关闭
    /// </summary>
    public class NPGGUIHarvestNumTaskObj
    {
        /// <summary>
        /// 本任务的操作序列号
        /// </summary>
        private long _m_lSerialize;
        /// <summary>
        /// 本任务当前的总技术
        /// </summary>
        private long _m_lTaskSumCount;

        /// <summary>
        /// 标记任务是否完成的标记，初始未完成
        /// </summary>
        private bool _m_bIsDone;

        /// <summary>
        /// 任务开始的时间标记，用于避免一些任务驻留
        /// 当任务时长超过30秒则视为任务已经完成
        /// </summary>
        private float _m_fTaskStartTime;

        public NPGGUIHarvestNumTaskObj(long _serialize)
        {
            _m_lSerialize = _serialize;
            _m_lTaskSumCount = 0;

            _m_bIsDone = false;

            _m_fTaskStartTime = Time.time;
        }

        public long serialize { get { return _m_lSerialize; } }
        public bool isDone { get { return _m_bIsDone; } }
        public long sumCount { get { return _m_lTaskSumCount; } }

        /// <summary>
        /// 添加累加的数量
        /// </summary>
        /// <param name="_count"></param>
        public void addCount(long _count)
        {
            _m_lTaskSumCount += _count;
        }

        /// <summary>
        /// 设置本任务完成，并且设置最终数量
        /// </summary>
        /// <param name="_totalCount"></param>
        public void setTaskDone()
        {
            _m_bIsDone = true;
        }
        public void setTaskDone(long _totalCount)
        {
            _m_bIsDone = true;
            _m_lTaskSumCount = _totalCount;
        }

        /// <summary>
        /// 检测本任务是否完成
        /// 这个函数有做超时的额外处理，避免可能的内存泄漏
        /// </summary>
        /// <returns></returns>
        public bool checkIsDone()
        {
            if (_m_bIsDone)
                return true;

            if (Time.time - _m_fTaskStartTime >= 30)
                return true;

            return false;
        }
    }
}