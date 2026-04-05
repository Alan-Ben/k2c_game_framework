package NPUSServer.NPUSUserMgr.UserComp.InnComp.SpecialGuest;

import Common.InnObj.Inn_SpecialGuestInfo;
import NPCommon.DB._ASelectCallback;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.InnErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CallBack._ICallBackBool;
import NPEnum.ENPItemType;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;
import NPGameRes.Refs.Inn.RefInnSpecialGuest;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.UserComp.InnComp.InnComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_034_InnOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerInnSpecialGuestBO;

import java.util.ArrayList;
import java.util.List;

public class InnSpecialGuestMgr
{
    private final InnComponent _m_innComp;
    private List<InnSpecialGuestInfo> _m_specialGuestList;

    public InnSpecialGuestMgr(InnComponent _innComp)
    {
        _m_innComp = _innComp;
        _m_specialGuestList = new ArrayList<>();
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
        _m_innComp.getUSServer().getBM().getBM(PlayerInnSpecialGuestBO.class).findAll("cid", _m_innComp.getUserData().getCid(),
                new _ASelectCallback<List<PlayerInnSpecialGuestBO>>()
                {
                    @Override
                    public void dealSuc(List<PlayerInnSpecialGuestBO> boList)
                    {
                        for (PlayerInnSpecialGuestBO bo : boList)
                        {
                            if (bo != null)
                            {
                                // 检查配表是否存在
                                RefInnSpecialGuest refGuest = RefInnSpecialGuest.getMgr().get(bo.getGuestId());
                                if (refGuest == null)
                                {
                                    USLog.error(_m_innComp.getUSServer(), "Player {} inn special guest {} config not found, skip loading",
                                            _m_innComp.getUserData().getCid(), bo.getGuestId());
                                    continue;
                                }

                                InnSpecialGuestInfo info = new InnSpecialGuestInfo(InnSpecialGuestMgr.this, refGuest, bo);
                                _m_specialGuestList.add(info);
                            }
                        }
                        _callBack.onRunOver(true);
                    }

                    @Override
                    public void dealFail()
                    {
                        USLog.error(_m_innComp.getUSServer(), "Player {} inn special guest data load failed", _m_innComp.getUserData().getCid());
                        _callBack.onRunOver(false);
                    }
                });
    }

    /**
     * 获取指定特殊客人数据
     * @param _guestId
     * @return
     */
    public InnSpecialGuestInfo lookupSpecialGuestInfo(long _guestId)
    {
        _lock();
        try
        {
            for (InnSpecialGuestInfo guestInfo : _m_specialGuestList)
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
     * 接待特殊客人
     * @param _guestId
     * @return Result
     */
    public Result serveSpecialGuest(long _guestId, NPPlayerContext _context)
    {
        _lock();
        try
        {
            RefInnSpecialGuest refSpecialGuest = RefInnSpecialGuest.getMgr().get(_guestId);
            if (refSpecialGuest == null)
                return CommErr.REF_NOT_FOUND;

            // 检查是否满足接待条件
            if (!_checkCanUnlockSingleGuest(refSpecialGuest))
                return CommErr.CONDITION_NOT_ENABLE;

            // 检查是否已经接待过
            InnSpecialGuestInfo specialGuestInfo = lookupSpecialGuestInfo(_guestId);
            if (specialGuestInfo != null)
                return InnErr.INN_SPECIAL_GUEST_HAD_SERVE;

            // 创建新的特殊客人数据
            PlayerInnSpecialGuestBO bo = new PlayerInnSpecialGuestBO();
            bo.setCid(getComp().getUSServer().getBM(), getComp().getUserData().getCid());
            bo.setGuestId(getComp().getUSServer().getBM(), _guestId);
            bo.insert(getComp().getUSServer().getBM());

            // 创建特殊客人信息
            specialGuestInfo = new InnSpecialGuestInfo(this, refSpecialGuest, bo);
            _m_specialGuestList.add(specialGuestInfo);

            // 推送到客户端
            getComp().getUserData().sendMsgToGC(US2GCWriter_034_InnOp.make_061_OnInnSpecialGuestChg(specialGuestInfo.makeProto()));

            // 获取解锁奖励
            getComp().getUserData().gainItem(ENPItemType.MUSEUM_ITEM, refSpecialGuest.first_gain_gift_id, 1, _context);

            return Result.SUCC;
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
    private boolean _checkCanUnlockSingleGuest(RefInnSpecialGuest _refInnGuest)
    {
        for (NPPlayerConditionGroupObj conditionGroupObj : _refInnGuest.unlock_condition_list)
        {
            if (!NPPlayerConditionDealerMgr.IsEnable(conditionGroupObj, getComp().getUserData(), null))
                return false;
        }
        return true;
    }

    /**
     * 作弊接待所有特殊客人并发放首次接待珍宝奖励
     * 跳过解锁条件检查，只处理未接待的客人
     */
    public void cheatServeAllSpecialGuests(NPPlayerContext _context)
    {
        _lock();
        try
        {
            for (RefInnSpecialGuest refSpecialGuest : RefInnSpecialGuest.getMgr().getList())
            {
                // 跳过已接待的客人
                if (lookupSpecialGuestInfo(refSpecialGuest.id) != null)
                    continue;

                // 创建新的特殊客人数据
                PlayerInnSpecialGuestBO bo = new PlayerInnSpecialGuestBO();
                bo.setCid(getComp().getUSServer().getBM(), getComp().getUserData().getCid());
                bo.setGuestId(getComp().getUSServer().getBM(), refSpecialGuest.id);
                bo.insert(getComp().getUSServer().getBM());

                // 创建特殊客人信息
                InnSpecialGuestInfo specialGuestInfo = new InnSpecialGuestInfo(this, refSpecialGuest, bo);
                _m_specialGuestList.add(specialGuestInfo);

                // 推送到客户端
                getComp().getUserData().sendMsgToGC(US2GCWriter_034_InnOp.make_061_OnInnSpecialGuestChg(specialGuestInfo.makeProto()));

                // 发放首次接待珍宝奖励
                getComp().getUserData().gainItem(ENPItemType.MUSEUM_ITEM, refSpecialGuest.first_gain_gift_id, 1, _context);
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 将信息填充到列表中
     * @param _guestList
     */
    public void fillSpecialGuestList(List<Inn_SpecialGuestInfo> _guestList)
    {
        _lock();
        try
        {
            for (InnSpecialGuestInfo guestInfo : _m_specialGuestList)
            {
                _guestList.add(guestInfo.makeProto());
            }
        } finally
        {
            _unlock();
        }
    }

}
