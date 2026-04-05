package NPUSServer.NPUSUserMgr.UserComp.InnComp.Dish;

import Common.InnObj.Inn_DishInfo;
import NPCommon.DB._ASelectCallback;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.InnErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Pair.WCGPair;
import NPEnum.ENPItemType;
import NPGameRes.Refs.Inn.RefInnDish;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.ItemDealer._IUserItemBasicDealer;
import NPUSServer.NPUSUserMgr.UserComp.InnComp.InnComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_034_InnOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerInnDishBO;

import java.util.ArrayList;
import java.util.Comparator;
import java.util.List;

/**
 * 旅店菜品管理器
 */
public class InnDishMgr implements _IUserItemBasicDealer
{
    private final InnComponent _m_innComp;
    private final List<InnDishInfo> _m_hadUnlockDishList;
    // 用于随机菜品的索引列表 开始的客人id：可以随机到的菜品索引
    private final List<WCGPair<Long, Integer>> _m_randomIndexList;
    private final List<InnDishInfo> _m_dishList;

    public InnDishMgr(InnComponent innComp)
    {
        _m_innComp = innComp;
        _m_hadUnlockDishList = new ArrayList<>();
        _m_randomIndexList = new ArrayList<>();
        _m_dishList = new ArrayList<>();
    }

    /**
     * 获取旅店组件
     */
    public InnComponent getComp()
    {
        return _m_innComp;
    }

    protected void _lock()
    {
        _m_innComp.getUserData().lockUser();
    }

    protected void _unlock()
    {
        _m_innComp.getUserData().unlockUser();
    }

    /**
     * 初始化菜品数据
     */
    public void init(final _ICallBackBool _callBack)
    {
        _m_innComp.getUSServer().getBM().getBM(PlayerInnDishBO.class).findAll("cid", _m_innComp.getUserData().getCid(),
                new _ASelectCallback<List<PlayerInnDishBO>>()
                {
                    @Override
                    public void dealSuc(List<PlayerInnDishBO> boList)
                    {
                        for (PlayerInnDishBO bo : boList)
                        {
                            if (bo != null)
                            {
                                // 检查配表是否存在
                                RefInnDish refDish = RefInnDish.getMgr().get(bo.getDishId());
                                if (refDish == null)
                                {
                                    USLog.error(_m_innComp.getUSServer(), "Player {} inn dish {} config not found, skip loading",
                                            _m_innComp.getUserData().getCid(), bo.getDishId());
                                    continue;
                                }

                                InnDishInfo info = new InnDishInfo(InnDishMgr.this, bo, refDish);
                                _m_dishList.add(info);

                                if (info.hadUnlock())
                                    _m_hadUnlockDishList.add(info);
                            }
                        }

                        //构造随机菜品索引列表
                        rebuildRandomIndexList();

                        _callBack.onRunOver(true);
                    }

                    @Override
                    public void dealFail()
                    {
                        USLog.error(_m_innComp.getUSServer(), "Player {} inn dish data load failed", _m_innComp.getUserData().getCid());
                        _callBack.onRunOver(false);
                    }
                });
    }

    /**
     * 检查并创建初始默认菜品（已解锁+已获得菜谱）
     */
    public void checkDefaultDishes()
    {
        _lock();
        try
        {
            for (Long dishId : RefGeneral.Ref().inn_init_dish_list)
            {
                if (lookupDish(dishId) != null)
                    continue;

                RefInnDish refDish = RefInnDish.getMgr().get(dishId);
                if (refDish == null)
                {
                    USLog.error(_m_innComp.getUSServer(), "InnDishMgr.checkDefaultDishes - init dish config not found, dishId={}", dishId);
                    continue;
                }

                // 创建已解锁的菜品记录
                PlayerInnDishBO bo = new PlayerInnDishBO();
                bo.setCid(getComp().getUSServer().getBM(), getComp().getUserData().getCid());
                bo.setDishId(getComp().getUSServer().getBM(), dishId);
                bo.setLevel(getComp().getUSServer().getBM(), 1); // 1表示已解锁
                bo.setFinesse(getComp().getUSServer().getBM(), 0);
                bo.setHadGainRecipe(getComp().getUSServer().getBM(), true);
                bo.setStartLineUpId(getComp().getUSServer().getBM(), 1);
                bo.insert(getComp().getUSServer().getBM());

                InnDishInfo info = new InnDishInfo(this, bo, refDish);
                _m_dishList.add(info);
                _m_hadUnlockDishList.add(info);
            }

            // 有新增则重建随机索引
            rebuildRandomIndexList();
        } finally
        {
            _unlock();
        }
    }

    /**
     * 根据菜品索引获取菜品信息
     * @param _dishIndex
     * @return
     */
    public InnDishInfo getUnlockedDishByIndex(int _dishIndex)
    {
        _lock();
        try
        {
            if (_dishIndex < 0 || _dishIndex >= _m_hadUnlockDishList.size())
                return null;
            return _m_hadUnlockDishList.get(_dishIndex);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 构造随机菜品索引列表
     */
    public void rebuildRandomIndexList()
    {
        _lock();
        try
        {
            _m_randomIndexList.clear();

            //先按开始排队ID排序，如果相同则按菜品ID排序
            _m_hadUnlockDishList.sort(Comparator.comparingLong(InnDishInfo::getStartLineUpId).thenComparingLong(InnDishInfo::getDishId));

            for (int i = 0; i < _m_hadUnlockDishList.size(); i++)
            {
                InnDishInfo dishInfo = _m_hadUnlockDishList.get(i);

                if (!_m_randomIndexList.isEmpty())
                {
                    WCGPair<Long, Integer> pair = _m_randomIndexList.get(_m_randomIndexList.size() - 1);

                    // 如果当前菜品的开始排队ID与上一个菜品相同，则不添加
                    if (dishInfo.getStartLineUpId() == pair.first)
                    {
                        pair.second = i;
                        continue;
                    }
                }

                _m_randomIndexList.add(new WCGPair<>(dishInfo.getStartLineUpId(), i));
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 获取随机范围索引
     * @param _lineUpId 排队id
     * @return 范围索引，下次需要变更的排队id
     */
    public WCGPair<WCGPair<Long, Integer>, Long> getRandomPool(long _lineUpId)
    {
        _lock();
        try
        {
            WCGPair<Long, Integer> startPair = null;
            long stopLineUpId = -1;

            for (WCGPair<Long, Integer> pair : _m_randomIndexList)
            {
                if (_lineUpId >= pair.first)
                {
                    startPair = pair;
                } else
                {
                    stopLineUpId = pair.first;
                    break;
                }
            }

            return new WCGPair<>(startPair, stopLineUpId);
        } finally
        {
            _unlock();
        }
    }


    /**
     * 根据菜品ID获取菜品信息
     */
    public InnDishInfo lookupDish(long dishId)
    {
        _lock();
        try
        {
            for (InnDishInfo info : _m_dishList)
            {
                if (info.getDishId() == dishId)
                {
                    return info;
                }
            }
            return null;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 解锁菜品
     * @param dishId  菜品ID
     * @param context 上下文
     * @return Result
     */
    public Result unlockDish(long dishId, NPPlayerContext context)
    {
        _lock();
        try
        {
            // 检查配表
            RefInnDish refDish = RefInnDish.getMgr().get(dishId);
            if (refDish == null)
                return CommErr.REF_NOT_FOUND;

            // 检查是否已获得菜谱
            InnDishInfo existingDish = lookupDish(dishId);
            boolean hasRecipe = existingDish != null && existingDish.getHadGainRecipe();
            if (!hasRecipe)
                return InnErr.INN_DISH_RECIPE_NOT_GAINED;

            // 检查是否已解锁
            if (existingDish.hadUnlock())
                return InnErr.INN_DISH_ALREADY_UNLOCKED;

            // 判断关联设施是否都存在
            if (!getComp().getStationMgr().checkStationsExist(refDish.need_station_id_list))
                return InnErr.INN_REQUIRE_STATION_NOT_EXIST;

            // 检查是否满足解锁条件
            if (!NPPlayerConditionDealerMgr.IsEnable(refDish.unlock_condition, getComp().getUserData(), null))
                return InnErr.INN_DISH_UNLOCK_CONDITION_NOT_MET;

            // 已有菜谱记录，直接解锁
            existingDish.unlock(context);

            _m_hadUnlockDishList.add(existingDish);

            //构造随机菜品索引列表
            rebuildRandomIndexList();

            // 推送到客户端
            getComp().getUserData().sendMsgToGC(US2GCWriter_034_InnOp.make_052_OnInnDishChg(existingDish.makeProto()));

            return Result.SUCC;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 作弊解锁菜品
     * @param _dishId
     * @param _context
     * @return
     */
    public Result cheatUnlockDish(long _dishId, NPPlayerContext _context)
    {
        _lock();
        try
        {
            // 检查配表
            RefInnDish refDish = RefInnDish.getMgr().get(_dishId);
            if (refDish == null)
                return CommErr.REF_NOT_FOUND;

            // 检查是否已获得菜谱
            InnDishInfo existingDish = lookupDish(_dishId);
            boolean hasRecipe = existingDish != null && existingDish.getHadGainRecipe();
            if (!hasRecipe)
                return InnErr.INN_DISH_RECIPE_NOT_GAINED;

            // 已有菜谱记录，直接解锁
            existingDish.unlock(_context);

            _m_hadUnlockDishList.add(existingDish);

            //构造随机菜品索引列表
            rebuildRandomIndexList();

            // 推送到客户端
            getComp().getUserData().sendMsgToGC(US2GCWriter_034_InnOp.make_052_OnInnDishChg(existingDish.makeProto()));

            return Result.SUCC;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 随机菜品（仅包括已解锁的菜品）
     */
    public InnDishInfo randomHadUnlockDish()
    {
        _lock();
        try
        {
            return CommonFunc.randSelect(_m_hadUnlockDishList);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 检查是否解锁菜品
     * @return
     */
    public boolean hadUnlockDish(long _dishId)
    {
        _lock();
        try
        {
            for (InnDishInfo dishInfo : _m_hadUnlockDishList)
            {
                if (dishInfo.getDishId() == _dishId)
                {
                    return true;
                }
            }
            return false;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 获取已解锁的菜品数量
     * @return
     */
    public long getHadUnlockDishNum()
    {
        return _m_hadUnlockDishList.size();
    }

    /**
     * 将菜品信息填充到列表中
     * @param _dishList
     */
    public void fillDishList(List<Inn_DishInfo> _dishList)
    {
        _lock();
        try
        {
            for (InnDishInfo dishInfo : _m_dishList)
            {
                _dishList.add(dishInfo.makeProto());
            }
        } finally
        {
            _unlock();
        }
    }

    @Override
    public ENPItemType getItemType()
    {
        return ENPItemType.INN_RECIPE;
    }

    @Override
    public long getItemCount(long _itemId)
    {
        _lock();
        try
        {
            // 检查是否拥有对应菜品的菜谱，如果有则返回1，否则返回0
            InnDishInfo dishInfo = lookupDish(_itemId);
            if (dishInfo != null && dishInfo.getHadGainRecipe())
                return 1;
            return 0;
        } finally
        {
            _unlock();
        }
    }

    @Override
    public boolean hasItem(long _itemId, long _count)
    {
        _lock();
        try
        {
            // 检查是否拥有对应菜品的菜谱（包括未解锁的菜品）
            InnDishInfo dishInfo = lookupDish(_itemId);
            return dishInfo != null && dishInfo.getHadGainRecipe();
        } finally
        {
            _unlock();
        }
    }

    @Override
    public void initGainItem(long _itemId, long _count, NPPlayerContext _context)
    {
        _lock();
        try
        {
            // 初始化时不处理菜谱获得，由gainItem处理
            gainItem(_itemId, _count, false, _context);
        } finally
        {
            _unlock();
        }
    }

    @Override
    public void gainItem(long _itemId, long _count, boolean _isNotMerge, NPPlayerContext _context)
    {
        _lock();
        try
        {
            // 获得菜谱（菜谱ID等于菜品ID）
            InnDishInfo dishInfo = lookupDish(_itemId);
            if (dishInfo != null)
            {
                // 如果菜品已存在，直接获得菜谱
                dishInfo.gainRecipe(_context);
            }
            else
            {
                // 菜品未解锁，创建一个仅拥有菜谱的记录（未解锁状态）
                RefInnDish refDish = RefInnDish.getMgr().get(_itemId);
                if (refDish != null)
                {
                    // 创建菜品记录，但不解锁
                    PlayerInnDishBO newDishBO = new PlayerInnDishBO();
                    newDishBO.setCid(getComp().getUSServer().getBM(), getComp().getUserData().getCid());
                    newDishBO.setDishId(getComp().getUSServer().getBM(), _itemId);
                    newDishBO.setLevel(getComp().getUSServer().getBM(), 0); // 0表示未解锁
                    newDishBO.setFinesse(getComp().getUSServer().getBM(), 0);
                    newDishBO.setHadGainRecipe(getComp().getUSServer().getBM(), true); // 拥有菜谱
                    newDishBO.insert(getComp().getUSServer().getBM());

                    // 创建菜品信息对象并加入列表
                    InnDishInfo newDishInfo = new InnDishInfo(this, newDishBO, refDish);
                    _m_dishList.add(newDishInfo);

                    // 推送到客户端
                    getComp().getUserData().sendMsgToGC(US2GCWriter_034_InnOp.make_055_OnInnDishAdd(newDishInfo.makeProto()));

                    _context.collectItem(ENPItemType.INN_RECIPE, _itemId, 1, false); // 收集菜谱
                }
            }
        } finally
        {
            _unlock();
        }
    }

    @Override
    public boolean spendItem(long _itemId, long _count, NPPlayerContext _context)
    {
        return false; // 菜谱不消耗
    }
}
