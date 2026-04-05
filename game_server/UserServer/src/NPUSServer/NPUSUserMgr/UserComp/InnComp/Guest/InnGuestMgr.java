package NPUSServer.NPUSUserMgr.UserComp.InnComp.Guest;

import Common.InnObj.Inn_GuestInfo;
import NPCommon.DB._ASelectCallback;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.InnErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.Pair.WCGPair;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;
import NPGameRes.Refs.Inn.RefInnGuest;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.UserComp.InnComp.InnComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_034_InnOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerInnGuestBO;

import java.util.ArrayList;
import java.util.Comparator;
import java.util.List;

public class InnGuestMgr
{
    private final InnComponent _m_innComp;
    // 客人数据
    private List<InnGuestInfo> _m_guestList;
    // 用于随机的索引列表 开始的客人id：可以随机到的索引
    private final List<WCGPair<Long, Integer>> _m_randomIndexList;

    public InnGuestMgr(InnComponent _innComp)
    {
        _m_innComp = _innComp;
        _m_guestList = new ArrayList<>();
        _m_randomIndexList = new ArrayList<>();
    }

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
     * 初始化客人数据
     */
    public void init(final _ICallBackBool _callBack)
    {
        _m_innComp.getUSServer().getBM().getBM(PlayerInnGuestBO.class).findAll("cid", _m_innComp.getUserData().getCid(),
                new _ASelectCallback<List<PlayerInnGuestBO>>()
                {
                    @Override
                    public void dealSuc(List<PlayerInnGuestBO> boList)
                    {
                        for (PlayerInnGuestBO bo : boList)
                        {
                            if (bo != null)
                            {
                                // 检查配表是否存在
                                RefInnGuest refGuest = RefInnGuest.getMgr().get(bo.getGuestId());
                                if (refGuest == null)
                                {
                                    USLog.error(_m_innComp.getUSServer(), "Player {} inn guest {} config not found, skip loading",
                                            _m_innComp.getUserData().getCid(), bo.getGuestId());
                                    continue;
                                }

                                InnGuestInfo info = new InnGuestInfo(InnGuestMgr.this, refGuest, bo);
                                _m_guestList.add(info);
                            }
                        }

                        // 重建随机索引列表
                        rebuildRandomIndexList();

                        _callBack.onRunOver(true);
                    }

                    @Override
                    public void dealFail()
                    {
                        USLog.error(_m_innComp.getUSServer(), "Player {} inn guest data load failed", _m_innComp.getUserData().getCid());
                        _callBack.onRunOver(false);
                    }
                });
    }

    /**
     * 通过索引获取客人数据
     * @param _guestIndex
     * @return
     */
    public InnGuestInfo getGuestByIndex(int _guestIndex)
    {
        _lock();
        try
        {
            if (_guestIndex < 0 || _guestIndex >= _m_guestList.size())
                return null;
            return _m_guestList.get(_guestIndex);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 获取指定客人数据
     * @param _guestId
     * @return
     */
    public InnGuestInfo lookupGuestInfo(long _guestId)
    {
        _lock();
        try
        {
            for (InnGuestInfo guestInfo : _m_guestList)
            {
                if (guestInfo.getGuestId() == _guestId)
                {
                    return guestInfo;
                }
            }
            return null;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 检查是否有客人可以接待
     * @return
     */
    public boolean hasGuest()
    {
        _lock();
        try
        {
            return !_m_guestList.isEmpty();
        } finally
        {
            _unlock();
        }
    }

    /**
     * 检查是否可以解锁客人
     */
    public void checkDefaultUnlockGuest()
    {
        _lock();
        try
        {
            List<RefInnGuest> refList = RefInnGuest.getMgr().getList();
            for (RefInnGuest refInnGuest : refList)
            {
                if (!refInnGuest.unlock_condition_list.isEmpty())
                    continue;

                if (lookupGuestInfo(refInnGuest.Id()) != null)
                    continue;

                PlayerInnGuestBO bo = new PlayerInnGuestBO();
                bo.setCid(getComp().getUSServer().getBM(), getComp().getUserData().getCid());
                bo.setGuestId(getComp().getUSServer().getBM(), refInnGuest.Id());
                bo.setStartLineUpId(getComp().getUSServer().getBM(), _getStartLineUpId());
                bo.insert(getComp().getUSServer().getBM());

                // 可以解锁，创建客人信息
                _m_guestList.add(new InnGuestInfo(this, refInnGuest, bo));

                rebuildRandomIndexList();
            }
        } finally
        {
            _unlock();
        }
    }


    /**
     * 解锁指定客人
     * @param _guestId
     * @param _context
     * @return
     */
    public Result unlockGuest(long _guestId, NPPlayerContext _context)
    {
        _lock();
        try
        {
            InnGuestInfo guestInfo = lookupGuestInfo(_guestId);
            if (guestInfo != null)
                return InnErr.INN_GUEST_HAD_UNLOCK;

            RefInnGuest refInnGuest = RefInnGuest.getMgr().get(_guestId);
            if (refInnGuest == null)
                return CommErr.REF_NOT_FOUND;

            // 检查是否可以解锁
            if (!_checkCanUnlockSingleGuest(refInnGuest))
                return InnErr.INN_GUEST_UNLOCK_CONDITION_NOT_MEET;

            PlayerInnGuestBO bo = new PlayerInnGuestBO();
            bo.setCid(getComp().getUSServer().getBM(), getComp().getUserData().getCid());
            bo.setGuestId(getComp().getUSServer().getBM(), refInnGuest.Id());
            bo.setStartLineUpId(getComp().getUSServer().getBM(), _getStartLineUpId());
            bo.insert(getComp().getUSServer().getBM());

            // 可以解锁，创建客人信息
            guestInfo = new InnGuestInfo(this, refInnGuest, bo);
            _m_guestList.add(guestInfo);

            getComp().getUserData().sendMsgToGC(US2GCWriter_034_InnOp.make_060_OnInnGuestChg(guestInfo.makeProto()));

            // 重建随机索引列表
            rebuildRandomIndexList();

            return Result.SUCC;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 获取下一个可用的排队ID
     * @return
     */
    private long _getStartLineUpId()
    {
        _lock();
        try
        {
            long totalGuestNum = getComp().getTotalNeedReceiveGuestNum();
            for (InnGuestInfo guestInfo : _m_guestList)
            {
                if (guestInfo.getStartLineUpId() > totalGuestNum)
                    totalGuestNum = guestInfo.getStartLineUpId();
            }
            return totalGuestNum + 1; // 新客人排在最后
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
            _m_guestList.sort(Comparator.comparingLong(InnGuestInfo::getStartLineUpId).thenComparingLong(InnGuestInfo::getGuestId));

            for (int i = 0; i < _m_guestList.size(); i++)
            {
                InnGuestInfo guestInfo = _m_guestList.get(i);

                if (!_m_randomIndexList.isEmpty())
                {
                    WCGPair<Long, Integer> pair = _m_randomIndexList.get(_m_randomIndexList.size() - 1);

                    // 如果当前菜品的开始排队ID与上一个菜品相同，则不添加
                    if (guestInfo.getStartLineUpId() == pair.first)
                    {
                        pair.second = i;
                        continue;
                    }
                }

                _m_randomIndexList.add(new WCGPair<>(guestInfo.getStartLineUpId(), i));
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
     * 检查是否可以解锁单个客人
     * @param _refInnGuest
     * @return
     */
    private boolean _checkCanUnlockSingleGuest(RefInnGuest _refInnGuest)
    {
        for (NPPlayerConditionGroupObj conditionGroupObj : _refInnGuest.unlock_condition_list)
        {
            if (!NPPlayerConditionDealerMgr.IsEnable(conditionGroupObj, getComp().getUserData(), null))
                return false;
        }
        return true;
    }

    /**
     * 将信息填充到列表中
     * @param _guestList
     */
    public void fillGuestList(List<Inn_GuestInfo> _guestList)
    {
        _lock();
        try
        {
            for (InnGuestInfo guestInfo : _m_guestList)
            {
                _guestList.add(guestInfo.makeProto());
            }
        } finally
        {
            _unlock();
        }
    }
}
