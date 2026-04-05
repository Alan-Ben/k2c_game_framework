using System;
using System.Collections.Generic;

using UnityEngine;

/***********************
 * 超出一个轮询时间的其他定时任务管理对象
 **/
namespace ALPackage
{
    public class ALMonoTaskTimingNodeFarDelayTaskInfo
    {
        /** 对应回合标记 */
        private int _m_iRound;
        /** 任务对象 */
        private List<_IALBaseMonoTask> _m_stSynTaskList;

        public ALMonoTaskTimingNodeFarDelayTaskInfo(int _round)
        {
            _m_iRound = _round;
            _m_stSynTaskList = new List<_IALBaseMonoTask>();
        }
    
        public int getRound() {return _m_iRound;}
    
        /***************
         * 向本节点中插入任务
         * 
         * @author alzq.z
         * @time   Jul 16, 2015 11:41:12 PM
         */
        public void addSynTask(_IALBaseMonoTask _task)
        {
            if(null == _task)
                return ;
        
            _m_stSynTaskList.Add(_task);
        }
    
        /***************
         * 将所有延迟任务放入到接收队列中
         * 
         * @author alzq.z
         * @time   Jul 16, 2015 11:39:28 PM
         */
        public void popAllSynTask(List<_IALBaseMonoTask> _recList) 
        {
            if(null == _recList)
                return ;

            _recList.AddRange(_m_stSynTaskList);
            _m_stSynTaskList.Clear();
        }
    }
}
