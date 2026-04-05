package NPUSServer.GachaPublicRecord;

import ALBasicServer.ALBasicMutex.MutexAtom;
import NPCommon.DB.BM.BM;
import NPGameRes.Refs.AvatarGacha.RefGachaPool;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.GachaPublicRecordBO;

import java.util.Comparator;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class GachaPublicRecordMgr
{
    private NPUserServer _m_server;
    private Map<Long, GachaPublicPoolRecord> _m_gachaPoolRecordMap;
    private MutexAtom _m_mutex;

    public GachaPublicRecordMgr(NPUserServer _server)
    {
        _m_server = _server;
        _m_gachaPoolRecordMap = new HashMap<>();
        _m_mutex = new MutexAtom();
    }

    private void _lock()
    {
        _m_mutex.lock();
    }

    private void _unlock()
    {
        _m_mutex.unlock();
    }

    public NPUserServer getServer()
    {
        return _m_server;
    }

    public BM getBmObj()
    {
        return _m_server.getBM();
    }

    /**
     * 初始化
     */
    public boolean init()
    {
        List<GachaPublicRecordBO> recordBoList = getBmObj().getBM(GachaPublicRecordBO.class).s_findAll();
        //先做一次排序，省去后续排序操作
        recordBoList.sort(Comparator.comparingLong(GachaPublicRecordBO::getId));
        //初始化记录
        for (GachaPublicRecordBO recordBo : recordBoList)
        {
            GachaPublicPoolRecord poolRecord = ensurePoolRecord(recordBo.getPoolId());
            if (poolRecord != null)
                poolRecord.initRecord(recordBo);
        }

        return true;
    }

    /**
     * 获取记录对象
     * @param _poolId
     * @return
     */
    public GachaPublicPoolRecord lookupPoolRecord(long _poolId)
    {
        _lock();
        try
        {
            return _m_gachaPoolRecordMap.get(_poolId);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 获取记录对象
     * @param _poolId
     * @return
     */
    public GachaPublicPoolRecord ensurePoolRecord(long _poolId)
    {
        _lock();
        try
        {
            //查找缓存
            if (_m_gachaPoolRecordMap.containsKey(_poolId))
                return _m_gachaPoolRecordMap.get(_poolId);

            //查询配置
            RefGachaPool refPool = RefGachaPool.getMgr().get(_poolId);
            if (refPool == null)
            {
                USLog.error(_m_server, "GachaPublicRecordMgr ensureRecord refPool not found _poolId:{}", _poolId);
                return null;
            }

            return _m_gachaPoolRecordMap.put(_poolId, new GachaPublicPoolRecord(this, refPool));
        } finally
        {
            _unlock();
        }
    }

    /**
     * 记录抽卡
     */
    public void addRecord(long _poolId, long _itemId, String _playerName)
    {
        GachaPublicPoolRecord poolRecord = ensurePoolRecord(_poolId);
        if (poolRecord != null)
            poolRecord.addRecord(_itemId, _playerName);
    }
}
