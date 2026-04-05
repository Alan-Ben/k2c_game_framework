package NPUSServer.RankGift;

import Common.RankGiftPackObj.RankGiftPack_Info;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.CommonObj.NPDateTimeObj;
import NPCommon.DB.BM.BM;
import NPCommon.Util.CommonFunc;
import NPUSServer.NPUserServer;
import USDB.Bo.UsRankGiftPackBO;

import java.util.List;

/**
 * 冲榜礼包信息数据对象
 *
 * 主要功能：
 * 1. 缓存配表所有字段到数据库（避免热更配表导致数据异常）
 * 2. 管理礼包激活状态
 * 3. 提供礼包数据访问接口
 *
 * 设计特点：
 * - 数据库ID懒加载：首次保存时才创建数据库记录
 * - activate_time_ms > 0 表示已激活
 * - 道具列表序列化为字符串存储
 *
 * 线程安全：由外层管理器保证线程安全
 */
public class RankGiftPackInfo
{
    // 管理器引用
    private RankGiftPackMgr _m_mgr;

    private UsRankGiftPackBO _m_bo;

    // 礼包名称
    private String _m_name;

    // 消耗道具
    private NPCommonCostItem _m_costItem;

    // 原价消耗道具
    private NPCommonCostItem _m_oriCostItem;

    // 奖励道具列表
    private List<NPCommonCostItem> _m_rewardItemList;

    /**
     * 从BO构造（用于数据库加载）
     *
     * @param _mgr 管理器引用
     * @param _bo BO对象
     */
    public RankGiftPackInfo(RankGiftPackMgr _mgr, UsRankGiftPackBO _bo)
    {
        _m_mgr = _mgr;
        _m_bo = _bo;

        _m_name = _bo.getName();

        _m_costItem = new NPCommonCostItem();
        _m_costItem.parseFromString(_bo.getCost());

        _m_oriCostItem = new NPCommonCostItem();
        _m_oriCostItem.parseFromString(_bo.getOriCost());

        _m_rewardItemList = CommonFunc.listStringableFromString(_bo.getRewardItemList(), NPCommonCostItem.class);
    }

    /**
     * 获取BM对象
     */
    public BM getBM()
    {
        return _m_mgr.getServer().getBM();
    }

    /**
     * 获取服务器对象
     */
    public NPUserServer getServer()
    {
        return _m_mgr.getServer();
    }

    /**
     * 删除数据库记录
     */
    public void deleteFromDB()
    {
        _m_bo.del(getBM());
    }

    public long getEndTimeMs()
    {
        return _m_bo.getEndTimeMs();
    }

    public long getDbId()
    {
        return _m_bo.getId();
    }

    public int getBuyLimit()
    {
        return _m_bo.getBuyLimit();
    }

    public String getName()
    {
        return _m_name;
    }

    public NPCommonCostItem getCostItem()
    {
        return _m_costItem;
    }

    public NPCommonCostItem getOriCostItem()
    {
        return _m_oriCostItem;
    }

    public List<NPCommonCostItem> getRewardItemList()
    {
        return _m_rewardItemList;
    }

    public long getUiResPathId()
    {
        return _m_bo.getUiResPathId();
    }

    public int getSale()
    {
        return _m_bo.getSale();
    }

    /**
     * 修改礼包信息
     * @param _uiResPathId 资源路径ID
     * @param _sale 折扣
     * @param _buyLimit 购买限制
     * @param _name 礼包名称
     * @param _cost 消耗道具
     * @param _oriCost 原价消耗道具
     * @param _rewardList 奖励列表
     * @param _endDate 结束日期
     * @param _endTime 结束时间
     */
    public void modifyInfo(long _uiResPathId, int _sale, int _buyLimit, String _name, String _cost, String _oriCost, String _rewardList, String _endDate, String _endTime)
    {
        _m_bo.setUiResPathId(getBM(), _uiResPathId);
        _m_bo.setSale(getBM(), _sale);
        _m_bo.setBuyLimit(getBM(), _buyLimit);
        _m_bo.setName(getBM(), _name);
        _m_bo.setCost(getBM(), _cost);
        _m_bo.setOriCost(getBM(), _oriCost);
        _m_bo.setRewardItemList(getBM(), _rewardList);

        NPDateTimeObj endDateTime = new NPDateTimeObj();
        endDateTime.parseFromString(_endDate + " " + _endTime);
        _m_bo.setEndTimeMs(getBM(), endDateTime.getTimeMs());

        _m_bo.saveAllMarked(getBM());

        _m_name = _name;

        NPCommonCostItem cost = new NPCommonCostItem();
        cost.parseFromString(_cost);
        _m_costItem = cost;

        NPCommonCostItem oriCost = new NPCommonCostItem();
        oriCost.parseFromString(_oriCost);
        _m_oriCostItem = oriCost;

        _m_rewardItemList = CommonFunc.listStringableFromString(_rewardList, NPCommonCostItem.class);
    }

    /**
     * 修改结束时间
     * @param _endDate
     * @param _endTime
     */
    public void chgEndTime(String _endDate, String _endTime)
    {
        NPDateTimeObj endDateTime = new NPDateTimeObj();
        endDateTime.parseFromString(_endDate + " " + _endTime);
        _m_bo.saveEndTimeMs(getBM(),endDateTime.getTimeMs());
    }

    public RankGiftPack_Info toProto()
    {
        _m_mgr._lock();
        try{
            RankGiftPack_Info proto = new RankGiftPack_Info();
            proto.setDbId(_m_bo.getId());
            proto.setBuyLimitTimes(_m_bo.getBuyLimit());
            proto.setEndTimeMs(_m_bo.getEndTimeMs());
            proto.setName(_m_name);

            // 消耗道具（单个）
            if (_m_costItem != null)
            {
                proto.setConsume(_m_costItem.toProto());
            }

            // 原价消耗道具（单个）
            if (_m_oriCostItem != null)
            {
                proto.setOriConsume(_m_oriCostItem.toProto());
            }

            // 奖励列表
            proto.getRewardList().addAll(CommonFunc.costItemListToProto(getRewardItemList()));
            proto.setUiResPathId(getUiResPathId());
            proto.setSale(getSale());
            return proto;
        }finally
        {
            _m_mgr._unlock();
        }
    }
}
