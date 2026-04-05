using System.Collections.Generic;
using CommonEnum;
using NPEnum;

namespace GOE
{
    public partial class PlayerInfo
    {
        private class RedTipDealer
        {
            private readonly PlayerInfo _m_playerInfo;
            private List<WinMsgType> _m_lRefreshHeroCanGetMsgList;//刷新可领取顾问红点消息


            public RedTipDealer(PlayerInfo _component)
            {
                _m_playerInfo = _component;
            }
            

            /// <summary>
            /// 初始化红点
            /// </summary>
            public void init()
            {
                //获取所有可能影响伙伴解锁获取的消息类型
                _m_lRefreshHeroCanGetMsgList = new List<WinMsgType>();
                GRefdataCoreMgr.instance.playerHeroUnlockShowCore.dealAllRef(_ref =>
                {
                    if (_ref != null && _ref.refreshMsgTypeList != null && _ref.refreshMsgTypeList.Count > 0)
                    {
                        for (int i = 0; i < _ref.refreshMsgTypeList.Count; i++)
                        {
                            WinMsgType msgType = _ref.refreshMsgTypeList[i];
                            if (!_m_lRefreshHeroCanGetMsgList.Contains(msgType))
                                _m_lRefreshHeroCanGetMsgList.Add(msgType);
                        }
                    }
                });

                //刷新所有红点
                refreshDailyRewardState();
                refreshHeroUnlockState();
                refreshLevelUpState();
                refreshVipRewardRedTip();
                refreshVipRechargeRewardRedTip();

                //注册消息监听
                NPPlayer.instance.rescourceComp.onResourceCountChg += _onResourceCountChg;
                NPPlayer.instance.specialItemComp.goldData.onEarningsChg += _onEarningsChg;
                WinMsg.RegisterMsg(WinMsgType.ON_HERO_GAIN, _onHeroGain);
                if (_m_lRefreshHeroCanGetMsgList != null && _m_lRefreshHeroCanGetMsgList.Count > 0)
                {
                    for (int i = 0; i < _m_lRefreshHeroCanGetMsgList.Count; i++)
                    {
                        WinMsg.RegisterMsgAct(_m_lRefreshHeroCanGetMsgList[i], refreshHeroUnlockState);
                    }
                }
            }
            public void clear()
            {
                NPPlayer.instance.specialItemComp.goldData.onEarningsChg -= _onEarningsChg;
                NPPlayer.instance.rescourceComp.onResourceCountChg -= _onResourceCountChg;
                WinMsg.UnregisterMsg(WinMsgType.ON_HERO_GAIN, _onHeroGain);
                if (_m_lRefreshHeroCanGetMsgList != null && _m_lRefreshHeroCanGetMsgList.Count > 0)
                {
                    for (int i = 0; i < _m_lRefreshHeroCanGetMsgList.Count; i++)
                    {
                        WinMsg.UnregisterMsgAct(_m_lRefreshHeroCanGetMsgList[i], refreshHeroUnlockState);
                    }
                }
                _m_lRefreshHeroCanGetMsgList?.Clear();
                _m_lRefreshHeroCanGetMsgList = null;
            }
            
            /// <summary>
            /// 刷新所有红点
            /// </summary>
            public void refrshAll()
            {
                refreshDailyRewardState();
                refreshHeroUnlockState();
                refreshLevelUpState();
                refreshVipRewardRedTip();
                refreshVipRechargeRewardRedTip();
            }

            public void refreshDailyRewardState()
            {
                PlayerLvlRefObj curLevelRef = _m_playerInfo?.curLevelRef;
                bool checkNeedShowRed =
                    curLevelRef is { daily_reward_item: { Count: > 0 } } &&
                    !_m_playerInfo.isDailyRewardDrawn();
                
                RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_PLAYER_INFO_DAILY_REWARD, checkNeedShowRed ? 1 : 0);
            }
            public void refreshHeroUnlockState()
            {
                bool checkNeedShowRed = false;
                List<PlayerHeroUnlockShowRefObj> refList = GRefdataCoreMgr.instance.playerHeroUnlockShowCore.refList;
                foreach (PlayerHeroUnlockShowRefObj refObj in refList)
                {
                    if (refObj == null)
                        continue;

                    bool isGetHero = NPPlayer.instance.heroComponent.isHeroUnlock(refObj.hero_id);
                    //如果是已经获得的顾问，则记录点击跳转过的ID
                    if (isGetHero)
                        AccountSettingMgr.instance.accountSetting?.addClickPlayerGainHeroJumpId(refObj.id);

                    //没有获得顾问，且解锁条件满足，且没有点击过跳转按钮或者获取条件满足
                    if (!isGetHero && (refObj.unlock_condition == null || refObj.unlock_condition.IsEnable(null)) && 
                        ((AccountSettingMgr.instance.accountSetting != null && !AccountSettingMgr.instance.accountSetting.isClickPlayerGainHeroJump(refObj.id)) ||
                         refObj.get_condition == null || refObj.get_condition.IsEnable(null)))
                    {
                        if(!checkNeedShowRed)
                            checkNeedShowRed = true;
                    }
                }

                RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_PLAYER_INFO_HERO_UNLOCK, checkNeedShowRed ? 1 : 0);
            }
            public void refreshLevelUpState()
            {
                bool checkNeedShowRed = _m_playerInfo.canUpgrade();
                RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_PLAYER_INFO_LEVEL_UP, checkNeedShowRed ? 1 : 0);
            }

            /// <summary>
            /// 刷新vip等级奖励红点
            /// </summary>
            public void refreshVipRewardRedTip()
            {
                //刷新红点
                long vipRewardCount = 0;
                long vipExp = GCommon.getItemCount(ENPItemType.CURRENCY, (long)ECurrency.VIP_EXP);
                for (int i = 1; i <= _m_playerInfo.getValue(ENPPlayerParam.VIP_LVL); i++)
                {
                    VipRefObj vipRef = GRefdataCoreMgr.instance.vipRefCore.getRef(i);
                    if (!_m_playerInfo.isGetVipReward(i) && vipRef != null && vipRef.vip_exp <= vipExp)
                        vipRewardCount++;
                }
                RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_VIP_REWARD, vipRewardCount);
            }

            /// <summary>
            /// 刷新vip等级充值奖励红点
            /// </summary>
            public void refreshVipRechargeRewardRedTip()
            {
                //刷新红点
                long vipRechargeRewardCount = 0;
                long vipExp = GCommon.getItemCount(ENPItemType.CURRENCY, (long) ECurrency.VIP_EXP);
                for (int i = 1; i <= _m_playerInfo.getValue(ENPPlayerParam.VIP_LVL); i++)
                {
                    VipRefObj vipRef = GRefdataCoreMgr.instance.vipRefCore.getRef(i);
                    if (!_m_playerInfo.isGetVipRechargeReward(i) && vipRef != null && vipRef.recharge_reward_list != null && vipRef.recharge_reward_list.Count > 0 && vipRef.vip_exp <= vipExp)
                        vipRechargeRewardCount++;
                }
                RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_VIP_RECHARGE_REWARD, vipRechargeRewardCount);
            }


            private void _onResourceCountChg(ECurrency _type, long _srcValue, long _destValue)
            {
                if (_type == ECurrency.P_EXP)
                    refreshLevelUpState();
                else if (_type == ECurrency.VIP_EXP)
                {
                    refreshVipRewardRedTip();
                    refreshVipRechargeRewardRedTip();
                }
            }
            private void _onEarningsChg()
            {
                refreshLevelUpState();   
            }
            private void _onHeroGain(object[] _params)
            {
                refreshHeroUnlockState();
            }
        }
    }
}