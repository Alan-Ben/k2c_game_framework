package NPUSServer.UsMars.MineCore;

import ALBasicServer.ALBasicMutex.MutexObject;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.Mars.RefMarsExploreMine;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.UsMars.MineCore.SynTask.SynUsMarsMineRmvEmptyOutOfDateMineTask;
import NPUSServer.UsMars.MineCore.SynTask.SynUsMarsMineSettleEndTask;

import java.util.ArrayList;
import java.util.Comparator;

/**
 * 不同等级矿的管理对象
 * 由于生成矿要分等级做处理，因此对于矿除了统一管理之外，还需要分等级进行处理
 */
public class UsMarsMineLvlMgr
{
    private UsMarsMineCore _m_mcMineCore;

    //对应等级的矿数据对象
    private RefMarsExploreMine _m_rLvlRef;
    /**
     * 本等级内的有效矿
     * 有效矿按照时间排序，超出有效时间的会放入监控队列进行监控，确保第一时间刷新
     */
    private ArrayList<UsMarsMineObj> _m_lMineList;
    /**
     * 矿监控队列，主要针对占领矿、无效矿做监控。需要保证第一时间清理无效矿
     */
    private ArrayList<UsMarsMineObj> _m_lMonitorMineList;

    private MutexObject _m_mutex;

    protected UsMarsMineLvlMgr(UsMarsMineCore _mineCore, RefMarsExploreMine _lvlRef)
    {
        _m_mcMineCore = _mineCore;
        _m_rLvlRef = _lvlRef;

        _m_lMineList = new ArrayList<UsMarsMineObj>();
        _m_lMonitorMineList = new ArrayList<UsMarsMineObj>();

        _m_mutex = new MutexObject();
    }

    public RefMarsExploreMine getMineRef() {return _m_rLvlRef;}

    protected void _lock() {_m_mutex.lock();}
    protected  void _unlock() {_m_mutex.unlock();}

    /**
     * 初始化矿数据对象，数据创建由总管理器负责，本对象负责做相关管理工作
     * @param _mineObj
     */
    protected void _initMineObj(UsMarsMineObj _mineObj)
    {
        if(null == _mineObj)
            return ;

        long curTimeMS = CommonFunc.getNowTimeMS();
        //根据在有效期，放入总队列
        if(curTimeMS <= _mineObj.getEndShowMs())
        {
            //放入总队列
            _m_lMineList.add(_mineObj);

            //判断是否有人占有，是则放入监控队列
            if(_mineObj.getTeamId() > 0)
            {
                _m_lMonitorMineList.add(_mineObj);
            }
        }
        else
        {
            //超出时间的都放入监控队列
            _m_lMonitorMineList.add(_mineObj);
        }
    }

    /**
     * 对矿总队列进行排序，确保按照结束显示时间进行升序排列，减少日常检测消耗
     */
    protected void _refreshMineOrder()
    {
        CommonFunc.sortAscList(_m_lMineList,
                new Comparator<UsMarsMineObj>()
                {
                    @Override
                    public int compare(UsMarsMineObj o1, UsMarsMineObj o2)
                    {
                        return Long.compare(o1.getEndShowMs(), o2.getEndShowMs());
                    }
                });
    }

    /**
     * 获取矿总数量
     * @return
     */
    public int getMineCount()
    {
        _lock();

        try
        {
            return _m_lMineList.size();
        }
        finally {
            _unlock();
        }
    }

    /**
     * 取出一个随机的有效矿
     * 如矿的总数量低于全局设置阈值，则返回空
     * @return
     */
    public UsMarsMineObj tryPopRandExistMine()
    {
        _lock();

        try {
            //判断总数量是否超过下限，如无则直接返回
            if(_m_lMineList.size() <= RefGeneral.Ref().mars_mine_num_min)
                return null;

            //随机一个矿
            int idx = CommonFunc.randomInt(_m_lMineList.size() - 1);
            return _m_lMineList.get(idx);
        }
        finally {
            _unlock();
        }
    }

    /**
     * 尝试将矿放入监控队列
     */
    protected void _tryMonitorMine(UsMarsMineObj _mineObj)
    {
        if(null == _mineObj)
            return ;

        _lock();

        try {
            //已在队列则不处理
            if(_m_lMonitorMineList.contains(_mineObj))
                return ;

            //加入监控队列
            _m_lMonitorMineList.add(_mineObj);
        }
        finally {
            _unlock();
        }
    }

    /**
     * 等级管理器中的tick处理
     * @param _nowTimeMS
     */
    protected void _tick(long _nowTimeMS)
    {
        _lock();

        try
        {
            //检查未超时的队列，将超时的数据进行管理
            do
            {
                //队列空直接不处理
                if(_m_lMineList.isEmpty())
                    break;

                //取出第一个判断
                UsMarsMineObj mineObj = _m_lMineList.get(0);
                if(null == mineObj)
                {
                    //移除无效数据
                    _m_lMineList.remove(0);
                    continue;
                }

                //判断是否超时，如未超时则跳过后续处理数据
                if(_nowTimeMS <= mineObj.getEndShowMs())
                    break;

                //将数据移到监控队列，多做一次判断，避免重复添加
                _m_lMineList.remove(0);
                if(!_m_lMonitorMineList.contains(mineObj))
                    _m_lMonitorMineList.add(mineObj);
            }while(!_m_lMineList.isEmpty());

            //检查监控队列，如果监控队列的矿无人、采集完毕等，开启单独任务做后续处理
            //这里单开任务的原因是，处理这个事务必须从Core层开始调用，因此不在本任务内处理
            for(int i = 0; i < _m_lMonitorMineList.size(); )
            {
                UsMarsMineObj mineObj = _m_lMonitorMineList.get(i);
                if(null == mineObj) {
                    _m_lMonitorMineList.remove(i);
                    continue;
                }

                //判断是否有人占领，如无人占领，且未采集完毕，且未超过有效时间。直接从队列移除
                //因此如果未超时，未采集完毕的队列在总队列中存在
                if(mineObj.getTeamId() == 0)
                {
                    //此时无占领队伍
                    if(_nowTimeMS <= mineObj.getEndShowMs())
                    {
                        //还在有效期内
                        if(mineObj.isDisable())
                        {
                            //此时矿无效，直接从队列删除
                            _m_lMineList.remove(mineObj);
                            _m_lMonitorMineList.remove(i);
                            //开启任务从总管理器移除
                            ALSynTaskManager.getInstance().regTask(new SynUsMarsMineSettleEndTask(_m_mcMineCore.getUSServer(), mineObj.getDataId()));
                        }
                        else
                        {
                            //还未开采完毕
                            //直接从监控队列移除，此时总队列有此数据
                            _m_lMonitorMineList.remove(i);
                        }
                    }
                    else {
                        //已经超出有效期，此时由于无队伍占领，直接从监控队列中清除
                        //由于已经超过有效期，不需要从总队列中移除
                        _m_lMonitorMineList.remove(i);
                        //开启任务从总管理器移除
                        ALSynTaskManager.getInstance().regTask(new SynUsMarsMineRmvEmptyOutOfDateMineTask(_m_mcMineCore.getUSServer(), mineObj.getDataId()));
                    }
                }
                else {
                    //此时有队伍占领
                    if(mineObj.isDisable())
                    {
                        //如果已经开采完毕，此时开启任务结算，本队列内先直接移除
                        if(_nowTimeMS <= mineObj.getEndShowMs())
                            _m_lMineList.remove(mineObj);
                        _m_lMonitorMineList.remove(i);
                        //开启任务从总管理器移除
                        ALSynTaskManager.getInstance().regTask(new SynUsMarsMineSettleEndTask(_m_mcMineCore.getUSServer(), mineObj.getDataId()));
                    }
                    else
                    {
                        //此时还未开采完毕，不做处理
                        i++;
                    }
                }
            }
        }
        finally {
            _unlock();
        }
    }
}
