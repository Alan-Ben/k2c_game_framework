package NPUSServer.NPUSUserMgr.UserComp.MuseumComp.Gift;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.MuseumObj.Museum_ItemInfo;
import MJLog.MJEventLog;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.MuseumErr;
import NPCommon.ErrMain.Result.Result;
import NPGameRes.Refs.Museum.RefMuseumItem;
import NPGameRes.Refs.Museum.RefMuseumItemLevel;
import NPGameRes.Refs.Museum.RefMuseumItemUpgradeCost;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_035_MuseumOp;
import USDB.Bo.PlayerMuseumItemBO;

/**
 * 旅店珍宝信息
 */
public class MuseumItemInfo
{
    private final MuseumItemMgr _m_itemMgr;
    private final RefMuseumItem _m_refItem;
    private RefMuseumItemLevel _m_refItemLevel;

    // BO参数对应的成员变量
    private long _m_dbId;
    private long _m_itemId;
    private int _m_level;
    private boolean _m_isActive;
    private long _m_gainTimeMs;

    public MuseumItemInfo(MuseumItemMgr _itemMgr, PlayerMuseumItemBO _bo, RefMuseumItem _refItem)
    {
        _m_itemMgr = _itemMgr;
        _m_refItem = _refItem;
        _m_refItemLevel = _m_refItem.lookupLevelRef(_bo.getLevel());

        // 读取BO参数到成员变量
        _m_dbId = _bo.getId();
        _m_itemId = _bo.getItemId();
        _m_level = _bo.getLevel();
        _m_isActive = _bo.getIsActive();
        _m_gainTimeMs = _bo.getGainTimeMs();

        //加载到全局属性容器对象
        if (_m_isActive)
            _m_itemMgr.getComp().getUserData().getBonusMgr().addBonus(_m_refItemLevel != null ? _m_refItemLevel.bonus : null);
    }

    /**
     * 获取配表对象
     */
    public RefMuseumItem getRef()
    {
        return _m_refItem;
    }

    /**
     * 获取珍宝管理器
     */
    public MuseumItemMgr getMgr()
    {
        return _m_itemMgr;
    }

    /**
     * 获取珍宝ID
     */
    public long getItemId()
    {
        return _m_itemId;
    }

    /**
     * 获取等级
     */
    public int getLevel()
    {
        return _m_level;
    }

    /**
     * 是否激活
     */
    public boolean isActive()
    {
        return _m_isActive;
    }

    /**
     * 获取数据库ID
     */
    public long getDbId()
    {
        return _m_dbId;
    }

    /**
     * 升级珍宝
     * @param _context
     * @return
     */
    public Result upgrade(NPPlayerContext _context)
    {
        _m_itemMgr._lock();
        try
        {
            if (!_m_isActive)
                return MuseumErr.MUSEUM_ITEM_NOT_ACTIVE;

            // 查询珍宝下一等级配置
            RefMuseumItemLevel nextLevelRef = _m_refItem.lookupLevelRef(_m_level + 1);
            if (nextLevelRef == null)
                return MuseumErr.MUSEUM_ITEM_LEVEL_REACH_MAX;

            // 查询升级配置
            RefMuseumItemUpgradeCost refUpgradeCost = RefMuseumItemUpgradeCost.getMgr().getUpgradeCostByGroupAndLevel(_m_refItem.upgrade_cost_group_id, _m_level);
            if (refUpgradeCost == null)
                return MuseumErr.MUSEUM_ITEM_LEVEL_REACH_MAX;

            // 检查道具是否足够
            if (!getMgr().getComp().getUserData().hasCostItemList(refUpgradeCost.upgrade_cost_item))
                return CommErr.ITEM_NOT_ENOUGH;

            // 记录升级前等级
            int beforeLevel = _m_level;

            // 扣除消耗物品
            if (!getMgr().getComp().getUserData().spendCostItemList(refUpgradeCost.upgrade_cost_item, _context))
                return CommErr.CONSUME_FAIL;

            // 升级珍宝
            _m_level++;

            //加载到全局属性容器对象
            _m_itemMgr.getComp().getUserData().getBonusMgr().replaceBonus(_m_refItemLevel == null ? null : _m_refItemLevel.bonus, nextLevelRef.bonus);

            _m_refItemLevel = nextLevelRef;

            // 推送到客户端
            _m_itemMgr.getComp().getUserData().sendMsgToGC(US2GCWriter_035_MuseumOp.make_051_OnMuseumItemChg(makeProto()));

            // 更新数据库
            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("level", _m_level);
            getMgr().getComp().getUserData().getUSServer().getBM().getBM(PlayerMuseumItemBO.class).update("id", _m_dbId, updateValue);

            // 记录日志
            MJEventLog.logInnUpdate(getMgr().getComp().getUserData(), _m_itemId,
                    beforeLevel, _m_level);

            return Result.SUCC;
        } finally
        {
            _m_itemMgr._unlock();
        }
    }

    /**
     * 激活珍宝
     * @param _context
     * @return
     */
    public Result activate(NPPlayerContext _context)
    {
        _m_itemMgr._lock();
        try
        {
            // 检查是否已经激活
            if (_m_isActive)
                return MuseumErr.MUSEUM_ITEM_ALREADY_ACTIVE;

            // 激活珍宝
            _m_isActive = true;

            // 加载到全局属性容器对象
            _m_itemMgr.getComp().getUserData().getBonusMgr().addBonus(_m_refItemLevel != null ? _m_refItemLevel.bonus : null);

            // 推送到客户端
            _m_itemMgr.getComp().getUserData().sendMsgToGC(US2GCWriter_035_MuseumOp.make_051_OnMuseumItemChg(makeProto()));

            // 更新数据库
            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("is_active", 1);
            getMgr().getComp().getUserData().getUSServer().getBM().getBM(PlayerMuseumItemBO.class).update("id", _m_dbId, updateValue);

            // 记录日志
            MJEventLog.logInnUnlock(getMgr().getComp().getUserData(), _m_itemId);

            return Result.SUCC;
        } finally
        {
            _m_itemMgr._unlock();
        }
    }

    /**
     * 构造信息对象
     * @return
     */
    public Museum_ItemInfo makeProto()
    {
        Museum_ItemInfo info = new Museum_ItemInfo();
        info.setItemId(getItemId());
        info.setLevel(getLevel());
        info.setIsActive(isActive());
        info.setGainTimeMs(_m_gainTimeMs);
        return info;
    }
}
