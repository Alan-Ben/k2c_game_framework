package NPUSServer.NPUSUserMgr.UserComp.CacheComp;

import ALBasicCommon.ALBasicCommonFun;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;

/**
 * 玩家cache数据定时存储任务
 */
public class SyncPlayerCacheTimerTask implements _IALSynTask
{
    private PlayerCacheComponent _m_cacheComp;
    private long _m_lTimerSerialize;
    //下一次保存时间，未到达保存时间不处理
    private long _m_lNextSaveTime;

    public SyncPlayerCacheTimerTask(PlayerCacheComponent _cacheComp)
    {
        _m_cacheComp = _cacheComp;
        _m_lTimerSerialize = _m_cacheComp.getSerialie();
        _m_lNextSaveTime = ALBasicCommonFun.getNowTimeMS() + 60000;
    }

    @Override
    public void run() {
        //序列号不一致则不处理
        if(_m_lTimerSerialize != _m_cacheComp.getSerialie())
            return ;

        long nowTimeMS = ALBasicCommonFun.getNowTimeMS();
        if(nowTimeMS > _m_lNextSaveTime) {
            //执行保存操作
            _m_cacheComp.saveAll();

            //更新下一次保存时间
            _m_lNextSaveTime = nowTimeMS + 60000;
        }

        //定时5秒处理下一次保存，避免太久执行任务的内存驻留
        ALSynTaskManager.getInstance().regTask(this, 5000);
    }
}
