package NPUSServer.NPUSUserMgr.UserComp.TravelComp.Dealer;

import Common.TravelEnum.ETravelEventType;
import Common.TravelEnum.ETravelGambleResult;
import Common.TravelObj.Travel_EventResult;
import Common.TravelObj.Travel_GambleResult;
import CommonEnum.ECurrency;
import GC2GS.p008_TravelOp.GC2GS_008_011_ReqDealGamblingTravel;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.TravelErr;
import NPCommon.Util.Random;
import NPEnum.ENPItemType;
import NPEnum.ENPPlayerPropertyType;
import NPGameRes.Refs.Travel.RefTravelEvent;
import NPGameRes.Refs.Travel.RefTravelEventGamble;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.TravelComp.TravelCanDealEventInfo;
import NPUSServer.USLog;

/**
 * TravelEventDealer_Gambling - 博彩游历事件处理器
 *
 * 主要功能：
 * 1. 支持玩家放弃事件（isAbandon=true），不扣钻石不给奖励
 * 2. 校验押注额度，扣除钻石
 * 3. 加权随机（成功/失败/特别大奖三段权重）
 * 4. 按结果发放对应倍率钻石，写入 ext 数据
 * 5. 一键游历时自动以最小额押注直接结算；钻石不足则返回放弃结果
 */
public class TravelEventDealer_Gambling extends _ATravelEventDealer<GC2GS_008_011_ReqDealGamblingTravel>
{
    @Override
    public ETravelEventType getType()
    {
        return ETravelEventType.GAMBLING;
    }

    /**
     * 覆盖基类 deal，放弃时跳过所有奖励发放
     */
    @Override
    public void deal(TravelEventDealerResult _result, TravelCanDealEventInfo _info, GC2GS_008_011_ReqDealGamblingTravel _msgParam, NPPlayerContext _context)
    {
        // 玩家选择放弃：不扣钻石、不发奖励，直接构造放弃结果
        if (_msgParam.getIsAbandon())
        {
            if (null == _info.getRef())
            {
                _result.setErrCode(CommErr.REF_NOT_FOUND.getCode());
                return;
            }
            _result.getEventResult().setExt(_makeAbandonGambleResult().makePackage());
            return;
        }
        super.deal(_result, _info, _msgParam, _context);
    }

    // 博彩事件押注处理
    @Override
    protected void _dealExt(TravelEventDealerResult _result, TravelCanDealEventInfo _info, GC2GS_008_011_ReqDealGamblingTravel _msgParam, NPPlayerContext _context)
    {
        RefTravelEventGamble ref = _info.getRef().gamblingEventRef;
        if (null == ref)
        {
            _result.setErrCode(CommErr.REF_NOT_FOUND.getCode());
            return;
        }

        // 校验押注额度范围
        int betAmount = _msgParam.getBetAmount();
        if (betAmount <= 0 || betAmount < ref.min_ante || betAmount > ref.max_ante)
        {
            _result.setErrCode(TravelErr.TRAVEL_GAMBLE_BET_INVALID.getCode());
            return;
        }

        // 校验并扣除押注钻石
        if (!_info.getUserData().hasItem(ENPItemType.CURRENCY, ECurrency.GEM.ordinal(), betAmount))
        {
            _result.setErrCode(CommErr.ITEM_NOT_ENOUGH.getCode());
            return;
        }
        if (!_info.getUserData().spendItem(ENPItemType.CURRENCY, ECurrency.GEM.ordinal(), betAmount, _context))
        {
            _result.setErrCode(CommErr.CONSUME_FAIL.getCode());
            return;
        }

        // 结算并写入 ext
        Travel_GambleResult gambleResult = _doSettle(ref, betAmount, _info.getUserData(), _context);
        _result.getEventResult().setExt(gambleResult.makePackage());
    }

    @Override
    public boolean canAkeySpecDeal()
    {
        return true;
    }

    // 一键游历：自动以最小额押注直接结算；钻石不足则放弃事件
    @Override
    public Travel_EventResult dealAkeySpec(NPUSUserData _userData, RefTravelEvent _ref, NPPlayerContext _context)
    {
        RefTravelEventGamble ref = _ref.gamblingEventRef;
        if (null == ref)
        {
            USLog.error(_userData.getUSServer(), "player:{} event:{} deal travel gambling event error, not find sub ref.", _userData.getCid(), _ref.event_id);
            return null;
        }

        int betAmount = ref.min_ante;
        if (betAmount <= 0)
        {
            USLog.error(_userData.getUSServer(), "player:{} event:{} deal travel gambling event error, min_ante<=0.", _userData.getCid(), _ref.event_id);
            return null;
        }

        // 钻石不足：直接放弃事件，返回放弃结果（不进待处理列表）
        if (!_userData.hasItem(ENPItemType.CURRENCY, ECurrency.GEM.ordinal(), betAmount))
            return _makeAkeyAbandonResult(_ref.event_id);

        if (!_userData.spendItem(ENPItemType.CURRENCY, ECurrency.GEM.ordinal(), betAmount, _context))
            return _makeAkeyAbandonResult(_ref.event_id);

        // 构造返回结果并结算
        Travel_EventResult result = new Travel_EventResult();
        result.setEventId(_ref.event_id);

        Travel_GambleResult gambleResult = _doSettle(ref, betAmount, _userData, _context);
        result.setExt(gambleResult.makePackage());

        // 发放事件奖励物品和经验
        _userData.gainItemList(_ref.event_item_list, _context);
        long travelExpAdd = _userData.getPlayerComponent().getPropertyMgr().getValue(ENPPlayerPropertyType.TRAVEL_EVENT_GAIN_PLAYER_EXP_ADD);
        long gainPlayerExp = _ref.gain_player_exp + travelExpAdd;
        _userData.gainItem(ENPItemType.CURRENCY, ECurrency.P_EXP.ordinal(), gainPlayerExp, _context);
        _context.getCollector().fillProtoList(result.getItemList());

        return result;
    }

    /**
     * 核心结算：加权随机 → 计算钻石变化 → 发放钻石 → 返回结果
     */
    private Travel_GambleResult _doSettle(RefTravelEventGamble _ref, int _betAmount, NPUSUserData _userData, NPPlayerContext _context)
    {
        ETravelGambleResult resultType = _spinWheel(_ref);

        // 计算钻石收益
        long gainAmount;
        if (resultType == ETravelGambleResult.JACKPOT)
            gainAmount = (long) _betAmount * _ref.jackpot_rate / 10000;
        else if (resultType == ETravelGambleResult.WIN)
            gainAmount = (long) _betAmount * _ref.win_rate / 10000;
        else
            gainAmount = (long) _betAmount * (10000 - _ref.lose_rate) / 10000;

        // 发放钻石
        _userData.gainItem(ENPItemType.CURRENCY, ECurrency.GEM.ordinal(), gainAmount, _context);

        Travel_GambleResult gambleResult = new Travel_GambleResult();
        gambleResult.setResultType(resultType);
        gambleResult.setDiamondChange(gainAmount - _betAmount);
        gambleResult.setBetAmount(_betAmount);
        return gambleResult;
    }

    /**
     * 加权随机，直接返回结果类型
     */
    private ETravelGambleResult _spinWheel(RefTravelEventGamble _ref)
    {
        int totalWeight = _ref.win_weight + _ref.lose_weight + _ref.jackpot_weight;
        if (totalWeight <= 0)
            return ETravelGambleResult.WIN;

        int r = Random.nextInt(totalWeight);
        if (r < _ref.win_weight)
            return ETravelGambleResult.WIN;
        r -= _ref.win_weight;
        if (r < _ref.lose_weight)
            return ETravelGambleResult.LOSE;
        return ETravelGambleResult.JACKPOT;
    }

    /** 构造放弃结果（用于客户端主动放弃，无钻石变化） */
    private Travel_GambleResult _makeAbandonGambleResult()
    {
        Travel_GambleResult gambleResult = new Travel_GambleResult();
        gambleResult.setResultType(ETravelGambleResult.ABANDON);
        gambleResult.setDiamondChange(0);
        return gambleResult;
    }

    /** 构造一键游历放弃结果（无事件奖励、无钻石变化） */
    private Travel_EventResult _makeAkeyAbandonResult(long _eventId)
    {
        Travel_EventResult result = new Travel_EventResult();
        result.setEventId(_eventId);
        result.setExt(_makeAbandonGambleResult().makePackage());
        return result;
    }
}
