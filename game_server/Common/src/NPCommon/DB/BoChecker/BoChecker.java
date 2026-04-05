package NPCommon.DB.BoChecker;

import ALBasicCommon.ALSerializeMaker;
import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import NPCommon.DB.BM.BM;
import NPCommon.DB.BaseBO;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;

import java.util.ArrayList;
import java.util.LinkedList;
import java.util.List;
import java.util.concurrent.atomic.AtomicLong;

public class BoChecker
{
    private static  BoChecker _g_instance = new BoChecker();
    public static BoChecker getInstance()
    {
        if(null == _g_instance)
            _g_instance = new BoChecker();
        return _g_instance;
    }

    private long _m_lDealSrialize; //序列号
    private AtomicLong _m_BoCheckSerial = new AtomicLong(1); //检测事务序列号自增量
    private LinkedList<CheckNode> _m_lCheckList = new LinkedList<>(); //检测事务队列
    private MutexAtom _m_locker = new MutexAtom(); //列表_m_lCheckList的锁，
    private ArrayList<_ICheckerFilter> _m_filter; //过滤器，可以过滤掉某些Bo不做处理
    private boolean _m_isEnabled = false;//GM命令可以设置开关
    private int _m_iMaxQueueSize = 3000;


    protected BoChecker()
    {
        _m_lDealSrialize = ALSerializeMaker.makeNewSerialize();
        _m_filter = new ArrayList<_ICheckerFilter>();

        _m_locker.reducePriority(8888);
    }

    public int getMaxQueueSize()
    {
        return _m_iMaxQueueSize;
    } //返回队列最大长度

    //设置队列最大长度
    public void setMaxQueueSize(int _size)
    {
        if (_size <= 0)
            return;
        _m_iMaxQueueSize = _size;
    }

    /*****
     * 设置开关
     * @param _bOpen
     */
    public void setEnabled(boolean _bOpen)
    {
        _m_isEnabled = _bOpen;
    }

    /*********
     * 增加一个Bo的检测对象
     * @param _bo
     * @return
     */
    public long addCheck(BM _bm, BaseBO _bo)
    {
        if (!isEnabled())
            return 0;
        if (_m_filter == null)
            return 0;
        if(null == _bm)
            return 0;

        //逐个判断，如果都不符合则不处理
        if(!_m_filter.isEmpty())
        {
            boolean res = false;
            for(_ICheckerFilter filter : _m_filter)
            {
                if(filter.filterBo(_bo))
                {
                    res = true;
                    break;
                }
            }

            //判断是否有结果
            if(!res)
                return 0;
        }

        _m_locker.lock();
        try
        {
            if (_m_lCheckList.size() > _m_iMaxQueueSize)//超出最大队列，不做检测
            {
                return 0;
            }
        } finally
        {
            _m_locker.unlock();
        }

        //添加新的任务节点。
        long serial = _m_BoCheckSerial.incrementAndGet();
        CheckNode node = new CheckNode();
        node.BMObj = _bm;
        node.bo = _bo;
        node.serial = serial;
        node.addTime = CommonFunc.getNowTimeMS();
        _m_locker.lock();
        try
        {
            _m_lCheckList.addLast(node);
        } finally
        {
            _m_locker.unlock();
        }
        return serial;
    }

    /*********
     * 每秒检测，处理2秒前的所有bo
     */
    public boolean tickCheck(long _dealSerialize)
    {
        //判断序列号，不对则返回false
        if (_dealSerialize != _m_lDealSrialize)
            return false;

        List<CheckNode> errNodeList = null;//检测不通过的节点列表，如果有错误，直接加入列表，尽快解锁，避免卡顿，错误数据后续再处理。

        long nowMs = CommonFunc.getNowTimeMS();
        _m_locker.lock();
        try
        {
            CheckNode node = _m_lCheckList.peekFirst();
            while (node != null)
            {
                if (nowMs - node.addTime < 1100)//Tick间隔是1秒，判断时间为1.1秒，实际bo的检测延时为1.1-2.1秒之间,考虑到跨天结算可能有大量数据库操作，时间间隔不应太长
                {
                    break;
                }

                _m_lCheckList.pop();

                if (node.serial == node.bo.getCheckSerial() && node.bo.hasMarkedField())
                {
                    if (null == errNodeList)
                    {
                        errNodeList = new ArrayList<>();
                    }
                    errNodeList.add(node);
                }
                node = _m_lCheckList.peekFirst();
            }
        } finally
        {
            _m_locker.unlock();
        }

        if (errNodeList != null)
        {
            for (CheckNode node : errNodeList)
            {
                CommLog.error("WARNING!!!!BaseBo {} has Dirty Field not saved,[{}]", node.bo.getTableName(), node.bo.getMarkedUpdateKeyValue());
                try
                {
                    if (node.bo.getId() > 0)
                    {
                        node.bo.saveAllMarked(node.BMObj);
                    } else
                    {
                        node.bo.insert(node.BMObj);
                    }

                } catch (Exception e)
                {
                    continue;
                }

            }
            errNodeList.clear();
        }

        return true;
    }

    //开始监控
    public void startCheck()
    {
        startCheck(null);
    }

    //开始监控
    public synchronized void startCheck(_ICheckerFilter _filter)
    {
        _m_filter.add(_filter);
        //添加条件
        setEnabled(true);

        //获取新序列号
        _m_lDealSrialize = ALSerializeMaker.makeNewSerialize();
        ALSynTaskManager.getInstance().regTask(new CheckTask(_m_lDealSrialize), 1000);
    }

    public boolean isEnabled()
    {
        return _m_isEnabled;
    }

    /***
     * 返回队列实际长度
     * @return
     */
    public int getQueueSize()
    {
        _m_locker.lock();
        try
        {
            return _m_lCheckList.size();
        } finally
        {
            _m_locker.unlock();
        }
    }


    @Override
    public String toString()
    {
        return String.format("IsEnable:%s QueueSize:%d/%d ", isEnabled(), getQueueSize(), getMaxQueueSize());
    }


}
