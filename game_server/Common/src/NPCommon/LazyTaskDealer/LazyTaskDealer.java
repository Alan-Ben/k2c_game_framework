package NPCommon.LazyTaskDealer;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;

/******************
 * 本对象管理一个任务处理对象
 * 管理的任务在一定时间内只会触发一次
 * 可以多次激活，会保证触发一次，但是不保证及时触发
 *
 * 本处理类是为了避免某些任务被多次触发执行而设计的
 */
public class LazyTaskDealer implements  _IALSynTask
{
    //任务对象
    private _IALSynTask _m_task;
    //执行的最小间隔时间，一般1秒即可
    private long _m_lDealMinDurationMS;

    //是否需要执行，每次执行之前会重置此变量
    private boolean _m_bNeedDeal;

    public LazyTaskDealer(_IALSynTask _task, long _lDealMinDurationMS)
    {
        _m_task = _task;
        _m_lDealMinDurationMS = _lDealMinDurationMS;
        _m_bNeedDeal = false;
    }

    /*********
     * 本任务的执行主体
     */
    public void run()
    {
        //用本类自己做锁进行互斥
        synchronized (this) {
            //设置执行标记
            _m_bNeedDeal = false;
        }

        //执行任务
        if(null != _m_task)
            _m_task.run();
    }

    /**************
     * 设置本对象需要处理对应业务
     */
    public void setNeedDeal()
    {
        //用本类自己做锁进行互斥
        synchronized (this)
        {
            //如果任务未计划执行，则排下计划
            if(!_m_bNeedDeal)
            {
                //注册任务
                ALSynTaskManager.getInstance().regTask(this, _m_lDealMinDurationMS);
            }

            //设置执行标记
            _m_bNeedDeal = true;
        }
    }
}
