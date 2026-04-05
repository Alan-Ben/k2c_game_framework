package NPUSServer.NPUSUserMgr.UserComp.InnComp.SpecialGuest;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.InnObj.Inn_SpecialGuestInfo;
import NPCommon.ErrMain.InnErr;
import NPCommon.ErrMain.Result.Result;
import NPGameRes.Refs.Inn.RefInnSpecialGuest;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_034_InnOp;
import USDB.Bo.PlayerInnSpecialGuestBO;

public class InnSpecialGuestInfo
{
    private InnSpecialGuestMgr _m_mgr;
    private RefInnSpecialGuest _m_ref;

    private long _m_dbId;
    private boolean _m_hadDrawHandbookReward;

    public InnSpecialGuestInfo(InnSpecialGuestMgr _mgr, RefInnSpecialGuest _ref, PlayerInnSpecialGuestBO _bo)
    {
        _m_mgr = _mgr;
        _m_ref = _ref;

        _m_dbId = _bo.getId();
        _m_hadDrawHandbookReward = _bo.getHadDrawHandbookReward();
    }

    /**
     * 获取客人ID
     * @return
     */
    public long getGuestId()
    {
        return _m_ref.Id();
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
            if (_m_hadDrawHandbookReward)
                return InnErr.INN_HANDBOOK_REWARD_HAD_DRAW;

            _m_hadDrawHandbookReward = true;

            // 领取图鉴奖励
            _m_mgr.getComp().getUserData().gainItemList(_m_ref.handbook_reward, _context);

            // 更新数据库
            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("had_draw_handbook_reward", 1);
            _m_mgr.getComp().getUSServer().getBM().getBM(PlayerInnSpecialGuestBO.class).update("id", _m_dbId, updateValue);

            // 推送到客户端
            _m_mgr.getComp().getUserData().sendMsgToGC(US2GCWriter_034_InnOp.make_061_OnInnSpecialGuestChg(makeProto()));

            return Result.SUCC;
        } finally
        {
            _m_mgr._unlock();
        }
    }

    /**
     * 构造信息
     * @return
     */
    public Inn_SpecialGuestInfo makeProto()
    {
        Inn_SpecialGuestInfo proto = new Inn_SpecialGuestInfo();
        proto.setSpecialGuestId(_m_ref.Id());
        proto.setHadBeenServe(true);
        proto.setHadDrawHandbookReward(_m_hadDrawHandbookReward);
        return proto;
    }
}
