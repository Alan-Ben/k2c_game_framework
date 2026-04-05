using ALPackage;
using NPEnum;

namespace GOE
{
    public interface _IRecruitShopIconItemWnd : _IBasicDiffItemContainerItemWnd
    {
        RecruitShopInfo recruitShopInf { get; }

        _ARecruitItemInfo recruitItemInfo { get; }

        void setData(RecruitShopInfo _recruitShopInfo, _ARecruitItemInfo _recruitItemInfo);
        void setItemSelect(bool _select);
    }
    
    public abstract class _AGGUIWndRecruitShopIconItem<T_Mono, T_RecruitItemInfo> : _ATNPGGUIWndSingleChoiceItem<T_Mono, _AGGUIWndRecruitShopIconItem<T_Mono, T_RecruitItemInfo>>, _IRecruitShopIconItemWnd 
        where T_Mono : GGUIMonoRecruitShopIconItem
        where T_RecruitItemInfo : _ARecruitItemInfo
    {
        protected RecruitShopInfo _m_iRecruitShopInfo;
        protected T_RecruitItemInfo _m_iRecruitItemInfo;
        
        public _AGGUIWndRecruitShopIconItem(T_Mono _mono) : base(_mono)
        {
            initWnd();
        }
        
        public RecruitShopInfo recruitShopInf => _m_iRecruitShopInfo;
        public _ARecruitItemInfo recruitItemInfo => _m_iRecruitItemInfo;

        protected override void _onWndInitDoneEx()
        {
            _onWndInitDoneSub();
        }
        
        protected override void _onDiscardEx()
        {
            _onDiscardSub();
        }
        
        protected override void _onShowWndEx()
        {
            _onShowWndSub();
        }

        protected override void _onHideWndEx()
        {
            _onHideWndSub();
        }

        protected override void _onResetEx()
        {
            _onResetSub();
        }
        
        public _AALBasicUIWndMono getWndMono() { return wnd; }

        public void setData(RecruitShopInfo _recruitShopInfo, _ARecruitItemInfo _recruitItemInfo)
        {
            _m_iRecruitShopInfo = _recruitShopInfo;
            if (!(_recruitItemInfo is T_RecruitItemInfo))
            {
                Debug.LogError($"_AGGUIWndRecruitShopIconItem<{typeof(T_Mono)}, {typeof(T_RecruitItemInfo)}> setData error, _recruitItemInfo:{_recruitItemInfo} is not {typeof(T_RecruitItemInfo)}");
            }
            
            _m_iRecruitItemInfo = _recruitItemInfo as T_RecruitItemInfo;
            
            _onSetData();

            _refreshWnd();
        }

        protected void _refreshWnd()
        {
            if (wnd == null)
                return;

            ERecruitState recruitState = _m_iRecruitShopInfo?.getRecruitState(_m_iRecruitItemInfo, false) ?? ERecruitState.NONE;
            wnd.setRecruitState(recruitState);
            
            _onRefreshWnd();
        }

        public void setItemSelect(bool _select)
        {
            setSelectShow(_select);
        }
        
        protected GGUIMonoCommonQualityImgConfig _getQualityImgConfig(EQuality _quality)
        {
            if (wnd == null || wnd.qualityImgConfigList == null)
                return null;

            foreach (GGUIMonoCommonQualityImgConfig config in wnd.qualityImgConfigList)
            {
                if (config != null && config.quality == _quality)
                    return config;
            }

            return null;
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