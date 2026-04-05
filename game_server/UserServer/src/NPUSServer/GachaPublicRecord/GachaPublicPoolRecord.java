package NPUSServer.GachaPublicRecord;

import ALBasicServer.ALBasicMutex.MutexAtom;
import Common.GachaObj.Gacha_PublicRecordInfo;
import NPGameRes.Refs.AvatarGacha.RefGachaPool;
import USDB.Bo.GachaPublicRecordBO;

import java.util.ArrayList;
import java.util.List;

public class GachaPublicPoolRecord
{
    private GachaPublicRecordMgr _m_mgr;
    private RefGachaPool _m_ref;
    private List<GachaPublicRecord> _m_recordList;
    private MutexAtom _m_mutex;

    public GachaPublicPoolRecord(GachaPublicRecordMgr _mgr, RefGachaPool _ref) {
        _m_mgr = _mgr;
        _m_ref = _ref;
        _m_recordList = new ArrayList<>();
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

    /**
     * 初始化记录
     */
    public void initRecord(GachaPublicRecordBO _bo)
    {
        _m_recordList.add(new GachaPublicRecord(_bo));
    }

    /**
     * 新增记录
     * @param _itemId
     * @param _playerName
     */
    public void addRecord(long _itemId, String _playerName)
    {
        //如果记录限制为0，则不记录
        if (_m_ref.roll_record_limit == 0)
            return;

        GachaPublicRecordBO bo = new GachaPublicRecordBO();
        bo.setPoolId(_m_mgr.getBmObj(), _m_ref.Id());
        bo.setItemId(_m_mgr.getBmObj(), _itemId);
        bo.setPlayerName(_m_mgr.getBmObj(), _playerName);
        bo.insert(_m_mgr.getBmObj());

        _lock();
        try{
            _m_recordList.add(new GachaPublicRecord(bo));

            //如果记录数量超过限制，则删除最早的记录
            while (_m_recordList.size() > _m_ref.roll_record_limit)
            {
                GachaPublicRecord remove = _m_recordList.remove(0);
                if (remove != null)
                    remove.discard(_m_mgr.getBmObj());
            }
        }finally
        {
            _unlock();
        }
    }

    /**
     * 构造协议信息
     * @param _startDbId
     * @return
     */
    public List<Gacha_PublicRecordInfo> makeProtoList(long _startDbId)
    {
        _lock();
        try{
            List<Gacha_PublicRecordInfo> list = new ArrayList<>();
            for (int i = _m_recordList.size() - 1; i >= 0; i--)
            {
                GachaPublicRecord record = _m_recordList.get(i);
                if (record.getDbId() <= _startDbId)
                    break;

                list.add(record.toProto());
            }
            return list;
        }finally
        {
            _unlock();
        }
    }
}
