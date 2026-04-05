package NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp.Guarantee;

import NPCommon.DB._ASelectCallback;
import NPCommon.Util.CallBack._ICallBackBool;
import NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp.TreasureHuntComponent;
import NPUSServer.USLog;
import USDB.Bo.PlayerTreasureHuntGuaranteeBO;

import java.util.List;
import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

/**
 * 太空寻宝保底管理器
 * 负责管理不同区域的奇物保底机制
 */
public class TreasureHuntGuaranteeMgr
{
    private final TreasureHuntComponent _m_comp;
    private final Map<Long, TreasureHuntGuaranteeAreaInfo> _m_areaGuaranteeMap;

    public TreasureHuntGuaranteeMgr(TreasureHuntComponent _component)
    {
        _m_comp = _component;
        _m_areaGuaranteeMap = new ConcurrentHashMap<>();
    }

    public TreasureHuntComponent getComp()
    {
        return _m_comp;
    }

    /**
     * 从数据库加载保底数据
     * @param _handler 加载完成回调
     */
    public void _initGuaranteeFromDB(_ICallBackBool _handler)
    {
        _m_comp.getUSServer().getBM().getBM(PlayerTreasureHuntGuaranteeBO.class)
                .findAll("cid", _m_comp.getUserData().getCid(), new _ASelectCallback<List<PlayerTreasureHuntGuaranteeBO>>()
                {
                    @Override
                    public void dealSuc(List<PlayerTreasureHuntGuaranteeBO> _boList)
                    {
                        // 加载所有区域的保底数据
                        for (PlayerTreasureHuntGuaranteeBO bo : _boList)
                        {
                            // 使用BO对象构造数据对象
                            TreasureHuntGuaranteeAreaInfo areaInfo = new TreasureHuntGuaranteeAreaInfo(TreasureHuntGuaranteeMgr.this, bo);
                            _m_areaGuaranteeMap.put(bo.getAreaId(), areaInfo);

                            USLog.debug("TreasureHuntGuaranteeMgr loaded guarantee data, cid:{}, areaId:{}, count:{}",
                                    _m_comp.getUserData().getCid(), bo.getAreaId(), bo.getTreasureGuaranteeCount());
                        }

                        _handler.onRunOver(true);
                    }

                    @Override
                    public void dealFail()
                    {
                        if (getHasErr())
                        {
                            USLog.error("TreasureHuntGuaranteeMgr load guarantee data failed, cid:{}",
                                    _m_comp.getUserData().getCid());
                            _handler.onRunOver(false);
                            return;
                        }

                        // 没有数据是正常情况，直接成功
                        _handler.onRunOver(true);
                    }
                });
    }


    /**
     * 获取指定区域的保底信息
     * @param _areaId 区域ID
     * @return 保底信息，如果不存在则创建新的
     */
    public TreasureHuntGuaranteeAreaInfo getAreaGuaranteeInfo(long _areaId)
    {
        return _m_areaGuaranteeMap.computeIfAbsent(_areaId, k -> new TreasureHuntGuaranteeAreaInfo(TreasureHuntGuaranteeMgr.this, _areaId));
    }
}