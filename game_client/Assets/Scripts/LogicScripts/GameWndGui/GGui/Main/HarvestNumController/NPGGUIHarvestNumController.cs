using System.Collections.Generic;
using ALPackage;
using System;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 收获操作中用于方便各收获对象对于数值进行控制的阶段控制器
    /// 此控制器从属于某一个管理器，并由管理器调用对应的函数控制业务处理
    /// </summary>
    public class NPGGUIHarvestNumController
    {
        /// <summary>
        /// 最原始的值，假设有N个业务并行在跑，那么只有最开始跑的第一个业务的值会记录在这里
        /// </summary>
        private long _m_lSrcNum;

        /// <summary>
        /// 本控制对象中当前开启了几个汇总任务
        /// 只有当任务都完成的时候才算是本次业务完成
        /// 同时任务队列只有在完成之后才会一次清空，在执行过程中只会不断添加新任务
        /// </summary>
        [NotNull] private readonly List<NPGGUIHarvestNumTaskObj> _m_lTaskList;

        /// <summary>
        /// 当所有任务都完成的时候触发的事件函数
        /// </summary>
        private Action _m_dOnAllTaskDone;

        public NPGGUIHarvestNumController()
        {
            _m_lSrcNum = 0;
            _m_lTaskList = new List<NPGGUIHarvestNumTaskObj>();
            _m_dOnAllTaskDone = null;
        }

        /// <summary>
        /// 根据当前任务队列数量是否大于0判断当前的任务是否有效
        /// </summary>
        public bool taskEnable { get { return _m_lTaskList.Count > 0; } }

        //释放所有资源
        public void discard()
        {
            _m_lTaskList.Clear();
            _m_dOnAllTaskDone = null;
        }

        /// <summary>
        /// 使用当前基数与所有任务值计算当前值之和
        /// </summary>
        /// <returns></returns>
        public long calTotalCount()
        {
            long sum = _m_lSrcNum;

            NPGGUIHarvestNumTaskObj task = null;
            for (int i = 0; i < _m_lTaskList.Count; i++)
            {
                task = _m_lTaskList[i];
                if (null == task)
                    continue;

                sum += task.sumCount;
            }

            return sum;
        }

        /// <summary>
        /// 回调的注册和注销处理
        /// </summary>
        /// <param name="_delegate"></param>
        public void regAllTaskDoneDelegate(Action _delegate)
        {
            if (null == _m_dOnAllTaskDone)
                _m_dOnAllTaskDone = _delegate;
            else
                _m_dOnAllTaskDone += _delegate;
        }
        public void unregAllTaskDoneDelegate(Action _delegate)
        {
            if (null == _m_dOnAllTaskDone)
                return;

            _m_dOnAllTaskDone -= _delegate;
        }

        /// <summary>
        /// 开启一个数值变动业务
        /// </summary>
        /// <param name="_curNum"></param>
        /// <param name="_opSerialize">对应操作的序列号</param>
        public void startNumFunc(long _curNum, long _opSerialize)
        {
            //先判断是否有重复序列号
            NPGGUIHarvestNumTaskObj task = _getTaskObj(_opSerialize);
            if(null != task)
            {
                ALLog.Error($"multi harvest num task! serialize: {_opSerialize}");
                return;
            }

            //判断是否空队列，是的话则需要重置相关信息
            //将源数量设置为当前的数量，这样可以保证多个添加的累加值基本是一致的
            if(_m_lTaskList.Count <= 0)
            {
                _m_lSrcNum = _curNum;
            }

            //创建新任务对象
            task = new NPGGUIHarvestNumTaskObj(_opSerialize);

            //添加到队列中
            _m_lTaskList.Add(task);
        }

        /// <summary>
        /// 给某个任务添加数量
        /// </summary>
        /// <param name="_opSerialize"></param>
        /// <param name="_addCount"></param>
        public void addTaskNum(long _opSerialize, long _addCount)
        {
            NPGGUIHarvestNumTaskObj task = _getTaskObj(_opSerialize);
            if (null == task)
                return;

            task.addCount(_addCount);
        }

        /// <summary>
        /// 设置某一个任务完成，并且设置其总完成数量
        /// </summary>
        /// <param name="_serialize"></param>
        /// <param name="_totalCount"></param>
        public void setTaskDone(long _serialize)
        {
            NPGGUIHarvestNumTaskObj task = _getTaskObj(_serialize);
            if (null == task)
                return;

            task.setTaskDone();

            //此时需要检测是否所有任务都完成，是则调用控制对象完成的处理操作
            _checkAllTaskDone();
        }
        public void setTaskDone(long _serialize, long _totalCount)
        {
            NPGGUIHarvestNumTaskObj task = _getTaskObj(_serialize);
            if (null == task)
                return;

            task.setTaskDone(_totalCount);

            //此时需要检测是否所有任务都完成，是则调用控制对象完成的处理操作
            _checkAllTaskDone();
        }

        /// <summary>
        /// 查询对应序列号的任务对象
        /// </summary>
        /// <param name="_serialize"></param>
        /// <returns></returns>
        protected NPGGUIHarvestNumTaskObj _getTaskObj(long _serialize)
        {
            NPGGUIHarvestNumTaskObj task = null;
            for (int i = 0; i < _m_lTaskList.Count; i++)
            {
                task = _m_lTaskList[i];
                if (null == task)
                    continue;

                if (task.serialize == _serialize)
                    return task;
            }

            return null;
        }

        /// <summary>
        /// 检测是否所有任务都完成了
        /// </summary>
        protected void _checkAllTaskDone()
        {
            NPGGUIHarvestNumTaskObj task = null;
            for (int i = 0; i < _m_lTaskList.Count; i++)
            {
                task = _m_lTaskList[i];
                if (null == task)
                    continue;

                //只要有一个未完成则不处理后续
                if (!task.checkIsDone())
                    return;
            }

            //全部完成，此时需要清理队列
            _m_lTaskList.Clear();

            //并调用全部完成的处理
            if (null != _m_dOnAllTaskDone)
                _m_dOnAllTaskDone();
        }
    }
}