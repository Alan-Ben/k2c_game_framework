package NPCommon.DB.Version.TbIDMgr;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;

/********
 * 定时保存Id数据的定时任务
 */
public class SyncTaskSaveDBIDInfo implements _IALSynTask
{
    private TBIdMgr _m_tbIdMgr;
    private int _m_lDurationMS;

    public SyncTaskSaveDBIDInfo(TBIdMgr _idMgr, int _durationMS)
    {
        _m_tbIdMgr = _idMgr;
        _m_lDurationMS = _durationMS;
    }

    /****
     * 实际执行函数
     */
    public void run()
    {
        if(null == _m_tbIdMgr)
            return ;

        //调用纯粹指令进行保存
        _m_tbIdMgr.resyncTbIdBlob();

        //延迟开启任务
        ALSynTaskManager.getInstance().regTask(this, _m_lDurationMS);
    }

}
