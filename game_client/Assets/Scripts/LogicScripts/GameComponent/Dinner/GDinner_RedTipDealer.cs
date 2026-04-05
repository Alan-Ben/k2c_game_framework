using System.Collections.Generic;
using ALPackage;
using ChatPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public partial class GPlayerDinnerComponent
    {
        private class RedTipDealer
        {
            private GPlayerDinnerComponent _m_component;

            public RedTipDealer(GPlayerDinnerComponent _component)
            {
                _m_component = _component;
            }

            /// <summary>
            /// 初始化聊天红点
            /// </summary>
            public void init()
            {
                _refreshRed();
                _m_component.onRewardChg += _refreshRed;
                _m_component.onPermitChg += _refreshRed;
            }

            public void clear()
            {
                _m_component.onRewardChg -= _refreshRed;
                _m_component.onPermitChg -= _refreshRed;
            }

            /// <summary>
            /// 玩家信息详细红点
            /// </summary>
            private void _refreshRed()
            {
                int enterCount = 0;
                // 如果有宴会奖励则显示红点
                if(_m_component != null && !_m_component.hasDinnerOpen && _m_component._m_hasOwenrReward) 
                    enterCount = 1;
                // 如果登录后未进入宴会，并且凭证未使用，同时还没有开宴会则显示红点
                if (_m_component != null && !_m_component._m_hasEnterDinner && !_m_component.hasDinnerOpen && _m_component.hasPermit())
                    enterCount = 1;
                
                RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_DINNER_ENTER, enterCount);
                
                _refreshCreateRed();
            }

            private void _refreshCreateRed()
            {
                int createCount = 0;
                // 如果有宴会凭证，并且没有开宴会则显示红点
                if(_m_component != null && _m_component.hasPermit() && !_m_component.hasDinnerOpen)
                    createCount = 1;
                
                RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_DINNER_CREATE, createCount);
            }
        }
        public static string getConsortSubSaveKey(long _saveMainId, long _consortId, long _subId)
        {
            return $"{_saveMainId}_{_consortId}_{_subId}";
        }
    }
}