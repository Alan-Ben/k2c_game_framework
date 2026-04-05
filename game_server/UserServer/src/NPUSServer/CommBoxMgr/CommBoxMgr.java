package NPUSServer.CommBoxMgr;

import ALBasicServer.ALBasicMutex.MutexObject;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import NPCommon.DB.BM.BM;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.Share.RefBoxComm;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.GeneralV.UsID;
import NPUSServer.NPUserServer;
import USDB.Bo.UsBoxBO;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

public class CommBoxMgr
{
    private NPUserServer _m_usUSServer;
    //宝箱数据Map，用于查询 key-宝箱实例ID
    private HashMap<Long, CommBoxInfo> _m_hmCommBoxMap;
    //宝箱数据List，用于遍历
    private ArrayList<CommBoxInfo> _m_alCommBoxList;
    //锁对象
    private MutexObject _m_mutex;

    public CommBoxMgr(NPUserServer _usServer)
    {
        _m_usUSServer = _usServer;

        _m_hmCommBoxMap = new HashMap<>();
        _m_alCommBoxList = new ArrayList<>();
        _m_mutex = new MutexObject();
    }

    private void _lock()
    {
        _m_mutex.lock();
    }

    private void _unlock()
    {
        _m_mutex.unlock();
    }

    public NPUserServer getUSServer() {return _m_usUSServer;}

    /*******
     * 初始化数据
     * @return
     */
    public boolean initFromDB()
    {
        List<UsBoxBO> boList = getUSServer().getBM().getBM(UsBoxBO.class).s_findAll();
        if (null == boList)
        {
            CommLog.error("CommBoxMgr initFromDB fail, boList is null");
            return false;
        }

        for (int i = 0; i < boList.size(); i++)
        {
            UsBoxBO bo = boList.get(i);
            if (null == bo)
                continue;

            RefBoxComm ref = RefBoxComm.getMgr().get(bo.getRefId());
            if (null == ref)
            {
                CommLog.error("CommBoxMgr initFromDB fail, box ref not found, refId:" + bo.getRefId());
                continue;
            }

            CommBoxInfo boxInfo = new CommBoxInfo(getUSServer(), bo, ref);
            _m_hmCommBoxMap.put(bo.getInstanceId(), boxInfo);
            _m_alCommBoxList.add(boxInfo);
        }

        //开启任务，移除过期宝箱
        ALSynTaskManager.getInstance().regTask(new SynTask_RemoveExpiredCommBox(this));

        return true;
    }

    /****
     * 查找指定实例ID宝箱
     * @param _instanceId
     * @return
     */
    public CommBoxInfo lookupBox(long _instanceId)
    {
        _lock();

        try
        {
            return _m_hmCommBoxMap.get(_instanceId);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 创建宝箱
     * @param _ref
     * @param _senderCid
     * @param _context
     * @return
     */
    public CommBoxInfo buildBox(RefBoxComm _ref, long _senderCid, NPPlayerContext _context)
    {
        _lock();

        try
        {
            BM bmObj = getUSServer().getBM();

            UsBoxBO bo = new UsBoxBO();
            bo.setInstanceId(bmObj, UsID.makeCommBoxInstanceId(getUSServer()));
            bo.setRefId(bmObj, _ref.id);
            bo.setCreatedAt(bmObj, CommonFunc.getNowTimeMS());
            bo.setSenderCid(bmObj, _senderCid);
            bo.insert(bmObj);

            CommBoxInfo info = new CommBoxInfo(getUSServer(), bo, _ref);

            _m_hmCommBoxMap.put(info.getInstanceId(), info);
            _m_alCommBoxList.add(info);

            return info;
        } finally
        {
            _unlock();
        }
    }

    /****
     * 移除过期宝箱
     */
    public void removeExpired()
    {
        _lock();

        try
        {
            for (int i = _m_alCommBoxList.size() - 1; i >= 0; i--)
            {
                CommBoxInfo info = _m_alCommBoxList.get(i);
                if (null == info)
                    continue;

                if (info.isExpired())
                {
                    _m_alCommBoxList.remove(i);
                    _m_hmCommBoxMap.remove(info.getInstanceId());

                    info.del();
                }
            }
        } finally
        {
            _unlock();
        }
    }

    /********
     * GM移除宝箱已领取玩家
     * @param _instanceId
     * @param _cid
     */
    public void cmdClearGainedCid(long _instanceId, long _cid)
    {
        _lock();

        try
        {
            for (int i = 0; i < _m_alCommBoxList.size(); i++)
            {
                CommBoxInfo box = _m_alCommBoxList.get(i);
                if (null == box)
                    continue;

                if (_instanceId == 0 || _instanceId == box.getInstanceId())
                {
                    box.clearGainedCid(_cid);

                    //如果指定实例ID，执行一次即可
                    if (_instanceId > 0)
                        break;
                }
            }
        } finally
        {
            _unlock();
        }
    }
}
