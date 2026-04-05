using JetBrains.Annotations;

namespace GOE
{
    public partial class PlayerInnComponent
    {
        public class RedTipDealer
        {
            [NotNull] private readonly PlayerInnComponent _m_comp;
            
            private _ARedTipNode _m_createGuestLazyCDHighPercentNode; // 创建客人的体力值高于最大值的 70%
            private _ARedTipNode _m_upgradableOrBuildableStationNode; // 可升级或可建造的设施
            private _ARedTipNode _m_upgradableDishNode; // 可升级的菜品
            private _ARedTipNode _m_unlockableDishNode; // 可解锁的菜品
            private _ARedTipNode _m_medalLevelUpgradeNode; // 勋章可升级
            private _ARedTipNode _m_normalGuestHandbookReward; // 普通客人图鉴奖励可领取
            private _ARedTipNode _m_specialGuestHandbookReward; // 特殊客人图鉴奖励可领取
            private _ARedTipNode _m_cashRegisterReward; // 收银台奖励可领取
            
            
            public RedTipDealer([NotNull] PlayerInnComponent _comp)
            {
                _m_comp = _comp;
            }
            
            
            public void init()
            {
                _m_createGuestLazyCDHighPercentNode = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_INN_CREATE_GUEST_LAZY_CD_HIGH_PERCENT);
                _m_upgradableOrBuildableStationNode = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_INN_UPGRADABLE_OR_BUILDABLE_STATION);
                _m_upgradableDishNode = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_INN_UPGRADABLE_DISH);
                _m_unlockableDishNode = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_INN_UNLOCKABLE_DISH);
                _m_medalLevelUpgradeNode = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_INN_MEDAL_LEVEL_UPGRADE);
                _m_normalGuestHandbookReward = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_INN_NORMAL_GUEST_HANDBOOK_REWARD);
                _m_specialGuestHandbookReward = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_INN_SPECIAL_GUEST_HANDBOOK_REWARD);
                _m_cashRegisterReward = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_INN_CASH_REGISTER_REWARD);
                
                // Initial refresh for all red tips
                refreshGuestLazyCDRedTip();
                refreshStationRedTip();
                refreshDishRedTip();
                refreshMedalLevelRedTip();
                refreshNormalGuestHandbookRewardRedTip();
                refreshSpecialGuestHandbookRewardRedTip();
                refreshCashRegisterRewardRedTip();
                
                // Register WinMsg events for lazy updates
                WinMsg.RegisterMsgAct(WinMsgType.ENTER_CITY, refreshGuestLazyCDRedTip);
                WinMsg.RegisterMsgAct(WinMsgType.ENTER_CITY, refreshStationRedTip);
                WinMsg.RegisterMsgAct(WinMsgType.ENTER_CITY, refreshDishRedTip);
                WinMsg.RegisterMsgAct(WinMsgType.ENTER_CITY, refreshCashRegisterRewardRedTip);
                WinMsg.RegisterMsgAct(WinMsgType.ON_INN_VIEW_GUEST_SETTLED, refreshStationRedTip);
            }
            public void clear()
            {
                // Unregister WinMsg events
                WinMsg.UnregisterMsgAct(WinMsgType.ON_INN_VIEW_GUEST_SETTLED, refreshStationRedTip);
                WinMsg.UnregisterMsgAct(WinMsgType.ENTER_CITY, refreshCashRegisterRewardRedTip);
                WinMsg.UnregisterMsgAct(WinMsgType.ENTER_CITY, refreshDishRedTip);
                WinMsg.UnregisterMsgAct(WinMsgType.ENTER_CITY, refreshStationRedTip);
                WinMsg.UnregisterMsgAct(WinMsgType.ENTER_CITY, refreshGuestLazyCDRedTip);
                
                // Clear all node references
                _m_createGuestLazyCDHighPercentNode = null;
                _m_upgradableOrBuildableStationNode = null;
                _m_upgradableDishNode = null;
                _m_unlockableDishNode = null;
                _m_medalLevelUpgradeNode = null;
                _m_normalGuestHandbookReward = null;
                _m_specialGuestHandbookReward = null;
            }


            public void refreshGuestLazyCDRedTip()
            {
                if (_m_createGuestLazyCDHighPercentNode == null)
                    return;
                
                PlayerLazyCDInfo lazyCdInfo = NPPlayer.instance.lazyCdComp.getLazyCDInfo(GRefdataCoreMgr.instance.npGeneral.inn_receive_guest_lazy_cd_id);
                long threshold = GRefdataCoreMgr.instance.npGeneral.inn_receive_guest_cd_red_tip_threshold;
                if (lazyCdInfo == null)
                    _m_createGuestLazyCDHighPercentNode.setCount(0);
                else
                    _m_createGuestLazyCDHighPercentNode.setCount(lazyCdInfo.getCount() > threshold ? 1 : 0);
            }
            public void refreshStationRedTip()
            {
                if (_m_upgradableOrBuildableStationNode == null)
                    return;
                
                bool hasUpgradableOrBuildableStation = false;
                foreach (InnStationInfo station in _m_comp._m_stationList)
                {
                    if (station.isUpgradable() || station.isUnlockable())
                    {
                        hasUpgradableOrBuildableStation = true;
                        break;
                    }
                }
                
                _m_upgradableOrBuildableStationNode.setCount(hasUpgradableOrBuildableStation ? 1 : 0);
            }
            public void refreshDishRedTip()
            {
                if (_m_upgradableDishNode != null)
                {
                    bool hasUpgradableDish = false;
                    foreach (InnDishInfo dish in _m_comp._m_dishList)
                    {
                        if (dish.canUpgrade)
                        {
                            hasUpgradableDish = true;
                            break;
                        }
                    }
                    _m_upgradableDishNode.setCount(hasUpgradableDish ? 1 : 0);
                }
                
                if (_m_unlockableDishNode != null)
                {
                    bool hasUnlockableDish = false;
                    foreach (InnDishInfo dish in _m_comp._m_dishList)
                    {
                        if (dish.checkCanUnlock())
                        {
                            hasUnlockableDish = true;
                            break;
                        }
                    }
                    _m_unlockableDishNode.setCount(hasUnlockableDish ? 1 : 0);
                }
            }
            public void refreshMedalLevelRedTip()
            {
                if (_m_medalLevelUpgradeNode == null)
                    return;
                
                int upgradeCount = _m_comp.getMedalUpgradeCount();
                _m_medalLevelUpgradeNode.setCount(upgradeCount > 0 ? 1 : 0);
            }
            public void refreshNormalGuestHandbookRewardRedTip()
            {
                if (_m_normalGuestHandbookReward == null) 
                    return;
                
                bool hasNormalReward = false;
                foreach (InnNormalGuestHandbookInfo handbookInfo in _m_comp._m_normalGuestHandbookInfoList)
                {
                    if (handbookInfo.isUnlock && !handbookInfo.hadDrawReward)
                    {
                        hasNormalReward = true;
                        break;
                    }
                }
                _m_normalGuestHandbookReward.setCount(hasNormalReward ? 1 : 0);
            }
            public void refreshSpecialGuestHandbookRewardRedTip()
            {
                if (_m_specialGuestHandbookReward == null) 
                    return;
                
                bool hasSpecialReward = false;
                foreach (InnSpecialGuestHandbookInfo handbookInfo in _m_comp._m_specialGuestHandbookInfoList)
                {
                    if (handbookInfo.isUnlock && !handbookInfo.hadDrawReward)
                    {
                        hasSpecialReward = true;
                        break;
                    }
                }
                _m_specialGuestHandbookReward.setCount(hasSpecialReward ? 1 : 0);
            }
            public void refreshCashRegisterRewardRedTip()
            {
                if (_m_cashRegisterReward == null) 
                    return;
                
                bool hasCashRegisterReward = _m_comp.receiveInfo.getRewardCountNow() > 0;
                bool hasOpenedInnToday = AccountSettingMgr.instance.warningTipSaver != null && !AccountSettingMgr.instance.warningTipSaver.needShowWarningTip(ENPWarningType.INN_CASH_REGISTER_REWARD_TODAY);
                _m_cashRegisterReward.setCount((hasCashRegisterReward && !hasOpenedInnToday) ? 1 : 0);
            }
        }
    }
}