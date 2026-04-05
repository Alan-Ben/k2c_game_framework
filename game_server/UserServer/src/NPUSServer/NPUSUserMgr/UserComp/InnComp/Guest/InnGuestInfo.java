package NPUSServer.NPUSUserMgr.UserComp.InnComp.Guest;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.InnObj.Inn_GuestInfo;
import NPCommon.ErrMain.InnErr;
import NPCommon.ErrMain.Result.Result;
import NPGameRes.Refs.Inn.RefInnGuest;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_034_InnOp;
import USDB.Bo.PlayerInnGuestBO;

public class InnGuestInfo
{
    private InnGuestMgr _m_mgr;
    private RefInnGuest _m_ref;

    private long _m_dbId;
    private boolean _m_hadDrawHandbookReward;
    private long _m_startLineUpId;

    public InnGuestInfo(InnGuestMgr _mgr, RefInnGuest _ref, PlayerInnGuestBO _bo)
    {
        _m_mgr = _mgr;
        _m_ref = _ref;

        _m_dbId = _bo.getId();
        _m_hadDrawHandbookReward = _bo.getHadDrawHandbookReward();
        _m_startLineUpId = _bo.getStartLineUpId();
    }

    /**
     * 获取客人ID
     * @return
     */
    public long getGuestId()
    {
        return _m_ref.Id();
    }

    public RefInnGuest getRef()
    {
        return _m_ref;
    }

    public long getStartLineUpId()
    {
        return _m_startLineUpId;
    }

    /**
     * 领取图鉴奖励
     * @return
     */
    public Result drawHandbookReward(NPPlayerContext _context)
    {
        _m_mgr._lock();
        try
        {
            // 检查是否已领取图鉴奖励
            if (_m_hadDrawHandbookReward)
                return InnErr.INN_HANDBOOK_REWARD_HAD_DRAW;

            _m_hadDrawHandbookReward = true;

            // 领取图鉴奖励
            _m_mgr.getComp().getUserData().gainItemList(_m_ref.handbook_reward, _context);

            // 推送到客户端
            _m_mgr.getComp().getUserData().sendMsgToGC(US2GCWriter_034_InnOp.make_060_OnInnGuestChg(makeProto()));

            // 更新数据库
            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("had_draw_handbook_reward", 1);
            _m_mgr.getComp().getUSServer().getBM().getBM(PlayerInnGuestBO.class).update("id", _m_dbId, updateValue);

            return Result.SUCC;
        } finally
        {
            _m_mgr._unlock();
        }
    }

    /**
     * 将客人信息转换为协议对象
     * @return
     */
    public Inn_GuestInfo makeProto()
    {
        Inn_GuestInfo guestInfo = new Inn_GuestInfo();
        guestInfo.setGuestId(_m_ref.Id());
        guestInfo.setHadDrawHandbookReward(_m_hadDrawHandbookReward);
        guestInfo.setStartLineUpId(_m_startLineUpId);
        return guestInfo;
    }
}
