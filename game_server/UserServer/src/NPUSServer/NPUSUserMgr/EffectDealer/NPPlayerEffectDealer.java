package NPUSServer.NPUSUserMgr.EffectDealer;

import NPEnum.ENPPlayerEffectType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;
import NPGameRes.Refs.Parse.NPPlayerEffectListParse;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.EffectDealer.Dealer.*;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.USLog;

import java.util.ArrayList;

public class NPPlayerEffectDealer
{
    public static NPPlayerEffectDealer _g_instance = new NPPlayerEffectDealer();

    public static NPPlayerEffectDealer getInstance()
    {
        return _g_instance;
    }

    public static void dealEffect(_ANPPlayerEffectInfo _effectInfo, NPUSUserData _userData, NPVarInfo _varVariableInfo, NPPlayerContext _context)
    {
        if (null == _effectInfo)
            return;

        _ANPPlayerEffectDealer dealer = NPPlayerEffectDealer.getInstance().getDealer(_effectInfo.effectType());
        if (null == dealer)
            return;

        dealer.dealEffect(_effectInfo, _userData, _varVariableInfo, _context);
    }

    //效果处理对象数组
    public _ANPPlayerEffectDealer[] _m_arrEffectDealerArr;

    public NPPlayerEffectDealer()
    {
        _m_arrEffectDealerArr = new _ANPPlayerEffectDealer[ENPPlayerEffectType.values().length];
        //注册效果处理对象
        _regDealer(new NPPlayerEffectDealer_S_R_EFFECT());
        _regDealer(new NPPlayerEffectDealer_S_GAIN_ITEM());
        _regDealer(new NPPlayerEffectDealer_S_SPEND_ITEM());
        _regDealer(new NPPlayerEffectDealer_S_GAIN_ITEM_FORM());
        _regDealer(new NPPlayerEffectDealer_S_SET_BUFF());
        _regDealer(new NPPlayerEffectDealer_S_CHG_BUFF());
        _regDealer(new NPPlayerEffectDealer_S_SET_BUFF());
        _regDealer(new NPPlayerEffectDealer_S_DEL_BUFF());
        _regDealer(new NPPlayerEffectDealer_S_GAIN_REWARD());
        _regDealer(new NPPlayerEffectDealer_S_START_QUEST());
        _regDealer(new NPPlayerEffectDealer_S_RECORD_CHG());
        _regDealer(new NPPlayerEffectDealer_S_EVENT_RECORD_CHG());
        _regDealer(new NPPlayerEffectDealer_S_QUEST_COUNTER_CHG());
        _regDealer(new NPPlayerEffectDealer_S_DEAL_ID());
        _regDealer(new NPPlayerEffectDealer_S_GAIN_X_CONSORT_CHARM_FROM());
        _regDealer(new NPPlayerEffectDealer_S_GAIN_X_CONSORT_INTIMACY_FROM());
        _regDealer(new NPPlayerEffectDealer_S_GAIN_X_CONSORT_LIKE_FROM());
        _regDealer(new NPPlayerEffectDealer_S_GAIN_LAZY_CD());
        _regDealer(new NPPlayerEffectDealer_S_ADD_P_V());
        _regDealer(new NPPlayerEffectDealer_S_ADD_P_V_FORM());
        _regDealer(new NPPlayerEffectDealer_S_GAIN_AND_SET_CUTE_ACTOR());
        _regDealer(new PlayerEffectDealer_S_RND_GAIN_X_BUSINESS_WORKERS());
        _regDealer(new PlayerEffectDealer_S_UNLOCK_SEVEN_DAYS_LOGIN());
        _regDealer(new PlayerEffectDealer_S_ANECDOTE_REFRESH());
        _regDealer(new NPPlayerEffectDealer_S_GAIN_X_CONSORT_CHARM_POINT_FROM());
        _regDealer(new NPPlayerEffectDealer_S_GAIN_X_CHILD_EXP_POINT_FROM());
        _regDealer(new NPPlayerEffectDealer_S_MARS_CHG_MOOD());
        _regDealer(new NPPlayerEffectDealer_S_MARS_CURE());
        _regDealer(new NPPlayerEffectDealer_S_MARS_RND_B_RND_SICK());
        _regDealer(new NPPlayerEffectDealer_S_MARS_RND_B_RND_LOST());
        _regDealer(new NPPlayerEffectDealer_S_MARS_GAIN_ENERGY());
        _regDealer(new NPPlayerEffectDealer_S_GAIN_REWARD_FORM());
        _regDealer(new NPPlayerEffectDealer_S_INN_ADD_GUEST());
        _regDealer(new NPPlayerEffectDealer_S_CHG_BIRTH_GIFTDE_COUM());
    }

    private void _regDealer(_ANPPlayerEffectDealer _dealer)
    {
        _m_arrEffectDealerArr[_dealer.effectType().ordinal()] = _dealer;
    }

    public _ANPPlayerEffectDealer getDealer(ENPPlayerEffectType _type)
    {
        return _m_arrEffectDealerArr[_type.ordinal()];
    }

    //////////////////////////////////////////////////////////////

    public static void dealEffect(NPPlayerEffectListParse _effectInfo, NPUSUserData _userData, NPVarInfo _varVariableInfo, NPPlayerContext _context)
    {
        if (null == _effectInfo)
            return;

        dealEffect(_effectInfo.getPlayerEffectList(), _userData, _varVariableInfo, _context);
    }

    public static void dealEffect(ArrayList<_ANPPlayerEffectInfo> _effectInfoList, NPUSUserData _userData, NPVarInfo _varVariableInfo, NPPlayerContext _context)
    {
        if (null == _effectInfoList)
            return;

        for (int i = 0; i < _effectInfoList.size(); i++)
        {
            _ANPPlayerEffectInfo info = _effectInfoList.get(i);
            if (null == info)
                continue;
            try
            {
                dealEffect(info, _userData, _varVariableInfo, _context);
            } catch (Exception e)
            {
                USLog.error(_userData.getUSServer(), "player:{} deal effect :{} failed", _userData.getCid(), info.effectType(), e);
            }

        }
    }
}
