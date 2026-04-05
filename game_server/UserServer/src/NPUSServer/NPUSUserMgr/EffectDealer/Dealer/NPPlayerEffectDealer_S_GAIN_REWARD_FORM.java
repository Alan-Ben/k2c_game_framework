package NPUSServer.NPUSUserMgr.EffectDealer.Dealer;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPPlayerEffectType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerEffect.EffectObj.NPPlayerEffect_S_GAIN_REWARD_FORM;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;
import NPGameRes.GameObjs.Reward.RewardMgr;
import NPGameRes.GameObjs.Reward.RewardObj;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.EffectDealer._ANPPlayerEffectDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.VariableDealer.NPPlayerVariableDeal;
import NPUSServer.USLog;

import java.util.List;

public class NPPlayerEffectDealer_S_GAIN_REWARD_FORM extends _ANPPlayerEffectDealer
{
    @Override
    public ENPPlayerEffectType effectType()
    {
        return ENPPlayerEffectType.S_GAIN_REWARD_FORM;
    }

    @Override
    public void dealEffect(_ANPPlayerEffectInfo _effectInfo, NPUSUserData _userData, NPVarInfo _varVariableInfo, NPPlayerContext _context)
    {
    	NPPlayerEffect_S_GAIN_REWARD_FORM effect = (NPPlayerEffect_S_GAIN_REWARD_FORM) _effectInfo;

        //计算奖励id
        long rewardId = NPPlayerVariableDeal.getInstance().CalculateVariableResult(_userData, effect.getRewardObj(), _varVariableInfo);
        RewardObj rewardObj = RewardMgr.getInstance().lookupReward(rewardId);
        if(null == rewardObj)
        {
        	USLog.error(_userData.getUSServer(), "player:{} reward:{} S_GAIN_REWARD_FORM fail, not find reward obj.", _userData.getCid(), rewardId);
        	return;
        }
        
        //计算奖励倍数
        long multiple = NPPlayerVariableDeal.getInstance().CalculateVariableResult(_userData, effect.getMultipleVarObj(), _varVariableInfo);
        //计算物品列表
        List<NPCommonCostItem> itemList = CommonFunc.itemMultiple(rewardObj.getItemList(), multiple);
        
        //领取奖励
        _userData.gainItemList(itemList, _context);
    }
}
