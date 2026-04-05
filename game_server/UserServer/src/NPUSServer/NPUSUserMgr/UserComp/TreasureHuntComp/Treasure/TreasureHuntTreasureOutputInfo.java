package NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp.Treasure;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.TreasureHuntObj.TreasureHunt_TreasureOutputInfo;
import CommonEnum.EBonusFilterType;
import CommonEnum.EBonusPropertyType;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.TreasureHuntErr;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.TreasureHunt.RefTreasureHuntTreasureOutput;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_036_TreasureHuntOp;
import USDB.Bo.PlayerTreasureHuntTreasureOutputBO;

public class TreasureHuntTreasureOutputInfo
{
    private long _m_dbId;
    private long _m_treasureId;
    private long _m_nextCanDrawTimeMs;
    private int _m_nextCanDrawNum;

    private RefTreasureHuntTreasureOutput _m_ref;
    private TreasureHuntTreasureMgr _m_mgr;

    public TreasureHuntTreasureOutputInfo(RefTreasureHuntTreasureOutput _ref, PlayerTreasureHuntTreasureOutputBO _bo, TreasureHuntTreasureMgr _mgr)
    {
        _m_ref = _ref;
        _m_mgr = _mgr;

        _m_dbId = _bo.getId();
        _m_treasureId = _bo.getTreasureId();
        _m_nextCanDrawTimeMs = _bo.getNextCanDrawTimeMs();
        _m_nextCanDrawNum = _bo.getNextCanDrawNum();
    }

    public NPUSUserData getUserData()
    {
        return _m_mgr.getUserData();
    }

    /**
     * 获取产出速度
     * @return
     */
    public static int getOutputSpeed(NPUSUserData _userdata, RefTreasureHuntTreasureOutput _ref)
    {
        return (int) (_ref.basic_value + _userdata.getBonusMgr()
                        .getFilterPropertyBonus(EBonusPropertyType.TREASURE_HUNT_TREASURE_OUTPUT_ADD, EBonusFilterType.TREASURE_HUNT_TREASURE_ID, _ref.treasure_id));
    }

    /**
     * 检查产出速度变化
     */
    public void checkOutputChange()
    {
        int newOutputSpeed = getOutputSpeed(getUserData(), _m_ref);
        if (newOutputSpeed == _m_nextCanDrawNum)
            return;

        onGemOutputSpeedChg(newOutputSpeed);
    }

    /**
     * 产出速度变更处理
     * @param _gemOutputSpeed
     */
    public void onGemOutputSpeedChg(int _gemOutputSpeed)
    {
        getUserData().lockUser();
        try
        {
            if (_gemOutputSpeed <= 0 || _gemOutputSpeed == _m_nextCanDrawNum)
                return;

            //如果下次可领取钻石时间为0，则设置为当天0点
            if (_m_nextCanDrawTimeMs == 0)
                _m_nextCanDrawTimeMs = CommonFunc.getTodayZeroClockMS(0);

            _m_nextCanDrawNum = _gemOutputSpeed;

            //保存数据
            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("next_can_draw_time_ms", _m_nextCanDrawTimeMs);
            updateValue.addValueObj("next_can_draw_num", _m_nextCanDrawNum);
            getUserData().getUSServer().getBM().getBM(PlayerTreasureHuntTreasureOutputBO.class).update("id", _m_dbId, updateValue);

            getUserData().sendMsgToGC(US2GCWriter_036_TreasureHuntOp.make_061_OnTreasureHuntTreasureOutputChg(makeProto()));
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 领取钻石
     * @param _context
     */
    public Result draw(NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            if (_m_nextCanDrawTimeMs == 0 || CommonFunc.getNowTimeMS() < _m_nextCanDrawTimeMs)
                return TreasureHuntErr.TREASURE_HUNT_CANT_DRAW_GEM;

            _m_nextCanDrawTimeMs = _m_ref.refresh_time.getNextFreshTimeTagMS(CommonFunc.getNowTimeMS());

            //保存数据
            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("next_can_draw_time_ms", _m_nextCanDrawTimeMs);
            getUserData().getUSServer().getBM().getBM(PlayerTreasureHuntTreasureOutputBO.class).update("id", _m_dbId, updateValue);

            getUserData().gainItem(_m_ref.output_item, _m_nextCanDrawNum, _context);

            getUserData().sendMsgToGC(US2GCWriter_036_TreasureHuntOp.make_061_OnTreasureHuntTreasureOutputChg(makeProto()));

            return Result.SUCC;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 清除钻石领取时间
     */
    public void clearNextCanDrawTime()
    {
        getUserData().lockUser();
        try
        {
            if (_m_nextCanDrawTimeMs == 0)
                return;

            _m_nextCanDrawTimeMs = CommonFunc.getTodayZeroClockMS(0);

            //保存数据
            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("next_can_draw_time_ms", _m_nextCanDrawTimeMs);
            getUserData().getUSServer().getBM().getBM(PlayerTreasureHuntTreasureOutputBO.class).update("id", _m_dbId, updateValue);

            getUserData().sendMsgToGC(US2GCWriter_036_TreasureHuntOp.make_061_OnTreasureHuntTreasureOutputChg(makeProto()));
        } finally
        {
            getUserData().unlockUser();
        }
    }
    
    /**
     * 创建协议
     * @return
     */
    public TreasureHunt_TreasureOutputInfo makeProto()
    {
        TreasureHunt_TreasureOutputInfo proto = new TreasureHunt_TreasureOutputInfo();
        proto.setTreasureId(_m_treasureId);
        proto.setNextCanDrawGemNum(_m_nextCanDrawNum);
        proto.setNextCanDrawGemTimeMs(_m_nextCanDrawTimeMs);
        return proto;
    }
    
}
