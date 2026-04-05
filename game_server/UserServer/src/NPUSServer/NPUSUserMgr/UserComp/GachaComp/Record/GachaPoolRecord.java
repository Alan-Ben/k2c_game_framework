package NPUSServer.NPUSUserMgr.UserComp.GachaComp.Record;

import Common.GachaObj.Gacha_RecordInfo;
import NPUSServer.NPUSUserMgr.UserComp.GachaComp.GachaComponent;
import USDB.Bo.PlayerGachaRecordBO;

import java.util.ArrayList;
import java.util.List;

public class GachaPoolRecord
{
    private GachaComponent _m_comp;
    private long _m_poolId;
    private List<GachaPoolRecordInfo> _m_recordList;

    public GachaPoolRecord(GachaComponent _comp, long _poolId)
    {
        _m_comp = _comp;
        _m_poolId = _poolId;
        _m_recordList = new ArrayList<>();
    }

    public long getPoolId()
    {
        return _m_poolId;
    }

    /**
     * 添加记录
     * @param _recordBo 记录bo
     */
    private void _addRecord(PlayerGachaRecordBO _recordBo)
    {
        _m_recordList.add(new GachaPoolRecordInfo(_recordBo));

        while (_m_recordList.size() > 500)
        {
            GachaPoolRecordInfo remove = _m_recordList.remove(0);
            if (remove != null)
                remove.discard(_m_comp.getUserData().getUSServer().getBM());
        }
    }

    /**
     * 从数据库初始化记录
     * @param _bo
     */
    public void initFromDb(PlayerGachaRecordBO _bo)
    {
        _addRecord(_bo);
    }

    /**
     * 记录抽卡
     * @param _itemId  抽中的卡牌id
     * @param _timeSec 抽卡时间
     */
    public void record(long _itemId, int _timeSec)
    {
        PlayerGachaRecordBO bo = new PlayerGachaRecordBO();
        bo.setCid(_m_comp.getUserData().getUSServer().getBM(), _m_comp.getUserData().getCid());
        bo.setPoolId(_m_comp.getUserData().getUSServer().getBM(), _m_poolId);
        bo.setItemId(_m_comp.getUserData().getUSServer().getBM(), _itemId);
        bo.setRollTimeSec(_m_comp.getUserData().getUSServer().getBM(), _timeSec);
        bo.insert(_m_comp.getUserData().getUSServer().getBM());

        _addRecord(bo);
    }

    /**
     * 获取记录列表
     * @return 记录列表
     */
    public List<Gacha_RecordInfo> makeProtoList()
    {
        List<Gacha_RecordInfo> list = new ArrayList<>();
        for (GachaPoolRecordInfo info : _m_recordList)
        {
            list.add(info.toProto());
        }
        return list;
    }
}
