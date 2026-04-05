using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public interface _IRecruitShopCardItemWnd : _IBasicDiffItemContainerItemWnd
    {
        void setData(RecruitShopInfo _recruitShopInfo, _ARecruitItemInfo _recruitItemInfo);
    }
    
    public abstract class _AGGUIWndRecruitShopCardItem<T_MONO, T_RecruitItemInfo> : _ATALBasicUISubWnd<T_MONO>, _IRecruitShopCardItemWnd 
        where T_MONO : GGUIMonoRecruitShopCardItem
        where T_RecruitItemInfo : _ARecruitItemInfo
    {
        protected RecruitShopInfo _m_iRecruitShopInfo;
        protected T_RecruitItemInfo _m_rRecruitItemInfo;
        
        protected GGUISubWndRecruitExchangeBtn _m_subWndRecruitExchangeBtn;//招募兑换按钮
        
        protected _AGGUIWndRecruitShopCardItem(T_MONO _wnd) : base(_wnd)
        {
        }
        
        public _AALBasicUIWndMono getWndMono() { return wnd; }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoExchangeBtn != null)
                _m_subWndRecruitExchangeBtn = new GGUISubWndRecruitExchangeBtn(wnd.monoExchangeBtn);
            
            _onWndInitDoneSub();
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
            }
            
            _m_subWndRecruitExchangeBtn?.discard();
            _m_subWndRecruitExchangeBtn = null;

            _onDiscardSub();
        }
        
        protected override void _onShowWnd()
        {
            _onShowWndSub();
        }

        protected override void _onHideWnd()
        {
            _m_subWndRecruitExchangeBtn?.hideWnd();

            _onHideWndSub();
        }

        protected override void _onReset()
        {
            _m_subWndRecruitExchangeBtn?.resetWnd();

            _onResetSub();
        }

        public void setData(RecruitShopInfo _recruitShopInfo, _ARecruitItemInfo _recruitItemInfo)
        {
            _m_iRecruitShopInfo = _recruitShopInfo;
            if (!(_recruitItemInfo is T_RecruitItemInfo))
            {
                Debug.LogError($"_AGGUIWndRecruitShopCardItem<{typeof(T_MONO)}, {typeof(T_RecruitItemInfo)}> setData error, _recruitItemInfo:{_recruitItemInfo} is not {typeof(T_RecruitItemInfo)}");
            }
            
            _m_rRecruitItemInfo = _recruitItemInfo as T_RecruitItemInfo;
            
            _onSetData();

            _refreshWnd();
        }

        protected void _refreshWnd()
        {
            if(wnd == null)
                return;

            if (_m_subWndRecruitExchangeBtn != null)
            {
                _m_subWndRecruitExchangeBtn.showWnd();
                _m_subWndRecruitExchangeBtn.setData(_m_iRecruitShopInfo, _m_rRecruitItemInfo);
            }

            _onRefreshWnd();
        }
        
        protected abstract void _onWndInitDoneSub();
        protected abstract void _onDiscardSub();
        protected abstract void _onShowWndSub();
        protected abstract void _onHideWndSub();
        protected abstract void _onResetSub();
        protected abstract void _onSetData();
        protected abstract void _onRefreshWnd();

    }
}