using ALPackage;

namespace GOE
{
    public class LazyNextFrameTaskDealer : _IALBaseMonoTask
    {
        //任务对象
        private _IALBaseMonoTask _m_task;
        //是否需要执行，每次执行之前会重置此变量
        private bool _m_bNeedDeal;

        public LazyNextFrameTaskDealer(_IALBaseMonoTask _task)
        {
            _m_task = _task;
            _m_bNeedDeal = false;
        }
        
        /*********
         * 本任务的执行主体
         */
        public void deal()
        {
            //设置执行标记
            _m_bNeedDeal = false;

            //执行任务
            if(null != _m_task)
                _m_task.deal();
        }

        /**************
         * 设置本对象需要处理对应业务
         */
        public void setNeedDeal()
        {
            if(_m_bNeedDeal)
                return;

            _m_bNeedDeal = true;
            
            //如果任务未计划执行，则排下计划
           ALMonoTaskMgr.instance.addNextFrameTask(this);
        }
    }
}