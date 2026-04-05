using Common.RushExchangeObj;

namespace GOE
{
    public class RushExchangeInfo
    {
        
        private long _m_groupId;// 礼包组ID
        private long _m_refId;// 当前礼包配置ID
        private long _m_activeTimeMs;// 当前兑换开始时间 如果没兑换影响刷新礼包时间
        private long _m_exchangeTimeMs;// 兑换时间 影响什么时候可以领奖
        private bool _m_isRewarded;// 是否已领奖
        private int _m_todayExchangeCount;// 当天兑换次数
        private long _m_nextResetCountTimeMs;// 下次刷新兑换次数时间
        
        private ERushExchangeState _m_state;
        private long _m_showTimeMs;
        
        public long groupId=>_m_groupId; // 礼包组ID
        public long refId=>_m_refId; // 当前礼包配置ID
        public long activeTimeMs=>_m_activeTimeMs; // 当前兑换开始时间 如果没兑换影响刷新礼包时间
        public long exchangeTimeMs=>_m_exchangeTimeMs; // 兑换时间 影响什么时候可以领奖
        public bool isRewarded=>_m_isRewarded; // 是否已领奖
        public int todayExchangeCount=>_m_todayExchangeCount; // 当天兑换次数
        public long nextResetCountTimeMs=>_m_nextResetCountTimeMs; // 下次刷新兑换次数时间
        
        public ERushExchangeState state=>_m_state;
        public long showTimeMs=>_m_showTimeMs;
        
        public void updateInfo(RushExchange_Info _info)
        {
            _m_groupId = _info.getGroupId();
            _m_refId = _info.getRefId();
            _m_activeTimeMs = _info.getActiveTimeMs();
            _m_exchangeTimeMs = _info.getExchangeTimeMs();
            _m_isRewarded = _info.getIsRewarded();
            _m_todayExchangeCount = _info.getTodayExchangeCount();
            _m_nextResetCountTimeMs = _info.getNextResetCountTimeMs();

            _refreshState();
            _refreshRedTip();
        }

        private void _refreshRedTip()
        {
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_RUSH_EXCHANGE_READY, _m_state == ERushExchangeState.Ready ? 1 : 0);
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_RUSH_EXCHANGE_REWARD, _m_state == ERushExchangeState.Reward ? 1 : 0);
        }

        private void _refreshState()
        {
            if (FpsAndPingMgr.instance.serverTimeTag > _m_nextResetCountTimeMs)
            {
                NPPlayer.instance.rushExchangeComp.reqRushExchangeRefresh();
            }
            NPSimpleUnlockRef unlockRef = GRefdataCoreMgr.instance.simpleUnlockMap.getRef(GRefdataCoreMgr.instance.npGeneral.rush_exchange_entry_simple_unlock_id);
            bool isUnlock = unlockRef == null || unlockRef.isConditionEnable(null);
            if (!isUnlock)
            {
                _m_state = ERushExchangeState.Lock;
                return;
            }

            if (_m_isRewarded && _m_todayExchangeCount >= GRefdataCoreMgr.instance.npGeneral.rush_exchange_day_can_exchange_times )
            {
                _m_state = ERushExchangeState.Wait;
                _m_showTimeMs = _m_nextResetCountTimeMs;
                return;
            }
            
            if ( _m_exchangeTimeMs > 0 && FpsAndPingMgr.instance.serverTimeTag >_m_exchangeTimeMs + GRefdataCoreMgr.instance.npGeneral.rush_exchange_done_need_wait_sec * 1000)
            {
                _m_state = ERushExchangeState.Reward;
                return;
            }
            if (_m_exchangeTimeMs > 0 && FpsAndPingMgr.instance.serverTimeTag < _m_exchangeTimeMs + GRefdataCoreMgr.instance.npGeneral.rush_exchange_done_need_wait_sec * 1000)
            {
                _m_state = ERushExchangeState.Exchange;
                _m_showTimeMs = _m_exchangeTimeMs + GRefdataCoreMgr.instance.npGeneral.rush_exchange_done_need_wait_sec * 1000;
                return;
            }
            
            _m_state = ERushExchangeState.Ready;
            long refreshTime = _m_activeTimeMs + GRefdataCoreMgr.instance.npGeneral.rush_exchange_refresh_sec * 1000;
            _m_showTimeMs = refreshTime;
        }

        public void checkRefresh()
        {
            if (_m_state == ERushExchangeState.Lock)
            {
                NPSimpleUnlockRef unlockRef = GRefdataCoreMgr.instance.simpleUnlockMap.getRef(GRefdataCoreMgr.instance.npGeneral.rush_exchange_entry_simple_unlock_id);
                bool isUnlock = unlockRef == null || unlockRef.isConditionEnable(null);
                if (isUnlock)
                {
                    _refreshState();
                    _refreshRedTip();
                }
            }
           
            if (_m_state == ERushExchangeState.Ready && FpsAndPingMgr.instance.serverTimeTag > _m_activeTimeMs + GRefdataCoreMgr.instance.npGeneral.rush_exchange_refresh_sec * 1000)
            {
                NPPlayer.instance.rushExchangeComp.reqRushExchangeRefresh();
            }
            if (_m_state == ERushExchangeState.Wait && FpsAndPingMgr.instance.serverTimeTag > _m_nextResetCountTimeMs)
            {
                NPPlayer.instance.rushExchangeComp.reqRushExchangeRefresh();
            }
        }
    }
}