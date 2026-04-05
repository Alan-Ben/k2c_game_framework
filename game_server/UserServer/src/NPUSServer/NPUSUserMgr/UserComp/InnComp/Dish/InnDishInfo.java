package NPUSServer.NPUSUserMgr.UserComp.InnComp.Dish;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.InnObj.Inn_DishInfo;
import CommonEnum.EBonusFilterType;
import MJLog.MJEventLog;
import NPCommon.ErrMain.InnErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPItemType;
import NPEnum.ENPPlayerRecordParam;
import NPGameRes.Refs.Inn.RefInnDish;
import NPGameRes.Refs.Inn.RefInnDishLevel;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_034_InnOp;
import USDB.Bo.PlayerInnDishBO;

/**
 * 旅店菜品信息
 */
public class InnDishInfo
{
    private final InnDishMgr _m_dishMgr;
    private final RefInnDish _m_refDish;

    // BO参数对应的成员变量
    private long _m_dbId;
    private long _m_dishId;
    private int _m_level;
    private long _m_finesse;
    private boolean _m_hadGainRecipe;
    private long _m_startLineUpId;

    public InnDishInfo(InnDishMgr _dishMgr, PlayerInnDishBO _bo, RefInnDish _refDish)
    {
        _m_dishMgr = _dishMgr;
        _m_refDish = _refDish;

        // 读取BO参数到成员变量
        _m_dbId = _bo.getId();
        _m_dishId = _bo.getDishId();
        _m_level = _bo.getLevel();
        _m_finesse = _bo.getFinesse();
        _m_hadGainRecipe = _bo.getHadGainRecipe();
        _m_startLineUpId = _bo.getStartLineUpId();

        //加载到全局属性容器对象
        if (hadUnlock())
            getUserData().getBonusMgr().addModifierToFilter(EBonusFilterType.BUILDING_ATTR,
                    _m_refDish.basic_attr.ordinal(), _m_refDish.bonus_prop_modifier_per_level, _m_level);
    }

    /**
     * 获取配表对象
     */
    public RefInnDish getRefDish()
    {
        return _m_refDish;
    }

    /**
     * 获取菜品管理器
     */
    public InnDishMgr getMgr()
    {
        return _m_dishMgr;
    }

    /**
     * 获取菜品ID
     */
    public long getDishId()
    {
        return _m_dishId;
    }

    /**
     * 获取等级
     */
    public int getLevel()
    {
        return _m_level;
    }

    /**
     * 获取熟练度
     */
    public long getFinesse()
    {
        return _m_finesse;
    }

    /**
     * 获取数据库ID
     */
    public long getDbId()
    {
        return _m_dbId;
    }

    /**
     * 获取当前菜品的排队ID
     * @return
     */
    public long getStartLineUpId()
    {
        return _m_startLineUpId;
    }

    /**
     * 是否获得菜谱
     */
    public boolean getHadGainRecipe()
    {
        return _m_hadGainRecipe || _m_level > 0; // 如果已解锁菜品，则视为已获得菜谱
    }

    /**
     * 是否已解锁菜品
     * @return
     */
    public boolean hadUnlock()
    {
        return _m_level > 0;
    }

    /**
     * 获得菜谱
     */
    public void gainRecipe(NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            // 检查是否已经获得菜谱
            if (_m_hadGainRecipe)
                return;

            _m_hadGainRecipe = true;

            // 推送到客户端
            _m_dishMgr.getComp().getUserData().sendMsgToGC(US2GCWriter_034_InnOp.make_052_OnInnDishChg(makeProto()));

            // 更新数据库
            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("had_gain_recipe", _m_hadGainRecipe ? 1 : 0);
            getUserData().getUSServer().getBM().getBM(PlayerInnDishBO.class).update("id", _m_dbId, updateValue);

            _context.collectItem(ENPItemType.INN_RECIPE, _m_dishId, 1, false); // 收集菜谱
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 解锁菜品
     */
    public void unlock(NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            if (hadUnlock())
                return; // 已经解锁

            _m_level = 1; // 设置为1级表示已解锁
            _m_startLineUpId = getMgr().getComp().getTotalNeedReceiveGuestNum() + 1;// 设置为当前总客人数量+1，表示排在下个客人可用

            // 加载到全局属性容器对象
            getUserData().getBonusMgr().addModifierToFilter(EBonusFilterType.BUILDING_ATTR,
                    _m_refDish.basic_attr.ordinal(), _m_refDish.bonus_prop_modifier_per_level, _m_level);

            // 更新数据库
            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("level", _m_level);
            updateValue.addValueObj("start_line_up_id", _m_startLineUpId);
            getUserData().getUSServer().getBM().getBM(PlayerInnDishBO.class).update("id", _m_dbId, updateValue);

            // 记录日志
            MJEventLog.logInnUnlock(getUserData(), _m_dishId);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 获取用户数据
     * @return
     */
    public NPUSUserData getUserData()
    {
        return _m_dishMgr.getComp().getUserData();
    }

    /**
     * 增加熟练度
     * @param _value 增加的熟练度数值
     * @return 是否升级了
     */
    public void addFinesse(long _value, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            if (_value <= 0)
                return;

            _m_finesse += _value;

            // 推送到客户端
            _m_dishMgr.getComp().getUserData().sendMsgToGC(US2GCWriter_034_InnOp.make_052_OnInnDishChg(makeProto()));

            // 更新数据库
            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("finesse", _m_finesse);
            getUserData().getUSServer().getBM().getBM(PlayerInnDishBO.class).update("id", _m_dbId, updateValue);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 升级菜品
     */
    public Result upgrade(NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            // 菜品未解锁
            if (!hadUnlock())
                return InnErr.INN_DISH_NOT_FOUND;

            // 查询升级配置
            RefInnDishLevel refUpgradeCost = RefInnDishLevel.getMgr().getByGroupAndLevel(_m_refDish.upgrade_group_id, _m_level);
            if (refUpgradeCost == null)
                return InnErr.INN_DISH_LEVEL_REACH_MAX;

            // 检查熟练度是否足够
            if (_m_finesse < refUpgradeCost.up_need_finesse)
                return InnErr.INN_DISH_FINESSE_NOT_ENOUGH;

            // 记录升级前等级
            int beforeLevel = _m_level;

            // 升级菜品
            _m_level++;
            _m_finesse -= refUpgradeCost.up_need_finesse;

            //加载到全局属性容器对象
            getUserData().getBonusMgr().addModifierToFilter(EBonusFilterType.BUILDING_ATTR,
                    _m_refDish.basic_attr.ordinal(), _m_refDish.bonus_prop_modifier_per_level);

            // 记录升级次数
            getUserData().getRecordComponent().addRecord(ENPPlayerRecordParam.INN_DISH_UPGRADE_TIMES, 1, _context);

            // 推送到客户端
            _m_dishMgr.getComp().getUserData().sendMsgToGC(US2GCWriter_034_InnOp.make_052_OnInnDishChg(makeProto()));

            // 更新数据库
            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("level", _m_level);
            updateValue.addValueObj("finesse", _m_finesse);
            getUserData().getUSServer().getBM().getBM(PlayerInnDishBO.class).update("id", _m_dbId, updateValue);

            // 记录日志
            MJEventLog.logInnUpdate(getUserData(), _m_dishId,
                    beforeLevel, _m_level);

            return Result.SUCC;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 构造信息
     * @return
     */
    public Inn_DishInfo makeProto()
    {
        Inn_DishInfo dish = new Inn_DishInfo();
        dish.setDishId(getDishId());
        dish.setLevel(getLevel());
        dish.setFinesse(getFinesse());
        dish.setHadUnlock(hadUnlock());
        dish.setHadGainRecipe(getHadGainRecipe());
        dish.setStartLineUpId(getStartLineUpId());
        return dish;
    }
}
