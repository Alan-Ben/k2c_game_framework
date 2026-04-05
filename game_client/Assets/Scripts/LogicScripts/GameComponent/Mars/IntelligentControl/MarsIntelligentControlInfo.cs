using Common.MarsObj;

namespace GOE
{
    public class MarsIntelligentControlInfo : _IMarsIntelligentControlInfo
    {
        private long _m_lIntelligentControlId;//智能控制id
        private MarsIntelligentControlRefObj _m_refObj;//配表数据
        private NPPlayerBuffRefObj _m_buffRefObj;//对应buff配表数据
        private NPPlayerBuffInfo _m_buffInfo;//对应的buff服务端数据
        private long _m_lCoolingEndTimeMs;//冷却结束时间(毫秒)

        public MarsIntelligentControlInfo(long _intelligentControlId, long _coolingEndTimeMs)
        {
            updateData(_intelligentControlId, _coolingEndTimeMs);
        }

        public MarsIntelligentControlInfo(Mars_Intelligent _serverData)
        {
            updateData(_serverData);
        }
        
        public long id { get { return _m_lIntelligentControlId; } }

        public MarsIntelligentControlRefObj refObj
        {
            get
            {
                if (_m_refObj == null || _m_refObj.id != _m_lIntelligentControlId)
                    _m_refObj = GRefdataCoreMgr.instance.marsIntelligentControlRefCore.getRef(_m_lIntelligentControlId);
                if(_m_refObj == null)
                    Debug.LogError_EditorOnly($"[MarsIntelligentControlInfo refObj]: 找不到id为{_m_lIntelligentControlId}的MarsIntelligentControlRefObj配表数据");
                
                return _m_refObj;
            }
        }

        public long buffId { get { return refObj?.buff_id ?? 0; } }
        
        public NPPlayerBuffRefObj buffRefObj
        {
            get
            {
                if(_m_buffRefObj == null || _m_buffRefObj.id != buffId)
                    _m_buffRefObj = GRefdataCoreMgr.instance.playerBuffMap.getRef(buffId);

                return _m_buffRefObj;
            }
        }

        public NPPlayerBuffInfo buffInfo
        {
            get
            {
                // 当原来有buffInfo, 但是buffId变了, 需要重新获取
                if(_m_buffInfo != null && _m_buffInfo.buffId != buffId)
                    updateBuffInfo();

                return _m_buffInfo;
            }
        }

        public long coolingEndTimeMs { get { return _m_lCoolingEndTimeMs; } }

        public long coolingLeftTimeMs
        {
            get
            {
                long leftTimeMs = _m_lCoolingEndTimeMs - FpsAndPingMgr.instance.serverTimeTag;
                return leftTimeMs < 0 ? 0 : leftTimeMs;
            }
        }

        public void updateData(Mars_Intelligent _serverData)
        {
            if(_serverData == null)
                return;

            updateData(_serverData.getId(), _serverData.getEndMs());
        }

        public void updateData(long _intelligentControlId, long _coolingEndTimeMs)
        {
            _m_lIntelligentControlId = _intelligentControlId;
            _m_lCoolingEndTimeMs = _coolingEndTimeMs;
            
            updateBuffInfo();
        }
        
        /// <summary>
        /// 更新buff信息
        /// </summary>
        public void updateBuffInfo()
        {
            _m_buffInfo = NPPlayer.instance.playerBuffComp.lookup(buffId);
        }

        public EMarsIntelligentControlState getState(bool _showTip = false)
        {
            MarsIntelligentControlRefObj intelligentControlRefObj = refObj;
            if (intelligentControlRefObj == null)
                return EMarsIntelligentControlState.NONE;

            // 有解锁条件, 并且解锁条件不满足
            if (intelligentControlRefObj.unlock_cond != null && !intelligentControlRefObj.unlock_cond.isNoConditionOrEnable(null))
            {
                if(_showTip)
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(intelligentControlRefObj.unlock_cond_unable_tip);
                
                return EMarsIntelligentControlState.LOCK;
            }

            // 若决策冷却结束时间还没到, 需要判断是否有buff在生效中, 有则为生效中, 没有则为冷却中
            if (_m_lCoolingEndTimeMs > FpsAndPingMgr.instance.serverTimeTag)
            {
                NPPlayerBuffInfo tmpBuffInfo = buffInfo;
                if (tmpBuffInfo != null && !tmpBuffInfo.hasExpired())
                {
                    if(_showTip)
                        NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.mars_intelligentControlEffective_str, TimeUtil.millisecondsToTime_hms(tmpBuffInfo.curLeftTimeMS)));
                    
                    return EMarsIntelligentControlState.EFFECTIVE;
                }
                else
                {
                    if(_showTip)
                        NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.mars_intelligentControlCoolingDown_str, TimeUtil.millisecondsToTime_hms(coolingLeftTimeMs)));
                    
                    return EMarsIntelligentControlState.COOLING_DOWN;
                }
            }
                
            // 决策冷却时间已经到了, 不用管是否有buff在生效中, 直接判断是否能使用
            if (intelligentControlRefObj.cannotUseStateCheck(_showTip))
                return EMarsIntelligentControlState.CANNOT_USE;

            return EMarsIntelligentControlState.CAN_USE;
        }
    }
}