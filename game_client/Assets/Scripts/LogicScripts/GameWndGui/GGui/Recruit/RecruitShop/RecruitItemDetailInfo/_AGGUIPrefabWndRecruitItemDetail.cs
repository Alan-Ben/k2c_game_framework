using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public interface _IGGUIPrefabWndRecruitItemDetail
    {
        public void setData(RecruitShopInfo _recruitShopInfo, _ARecruitItemInfo _recruitItemInfo);

        void load();
        void discard();
        void showWnd();
        void hideWnd();
        void resetWnd();

        bool isLoaded { get; }

        void regLoadDoneDelegate(Action _delegate);
    }
    
    public abstract class _AGGUIPrefabWndRecruitItemDetail<T_Mono, T_RecruitItemInfo> : _ANPGGUIBasicLoadPrefabSubWnd<T_Mono>, _IGGUIPrefabWndRecruitItemDetail
        where T_Mono : GGUIPrefabMonoRecruitItemDetail
        where T_RecruitItemInfo : _ARecruitItemInfo
    {
        private NPCommonAssetPathInfo _m_iAssetPathInfo;//资源加载路径

        protected RecruitShopInfo _m_iRecruitShopInfo;
        protected T_RecruitItemInfo _m_iRecruitItemInfo;
        
        public _AGGUIPrefabWndRecruitItemDetail(NPCommonAssetPathInfo _assetPath, Transform _parent) : base(_parent)
        {
            _m_iAssetPathInfo = _assetPath;
        }

        protected override string _monoAssetPath { get { return _m_iAssetPathInfo?.asset_path; } }
        protected override string _monoObjName { get { return _m_iAssetPathInfo?.obj_name; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            _onWndInitDoneSub();
        }
        
        protected override void _onDiscard()
        {
            _onDiscardSub();
        }
        
        protected override void _onShowWnd()
        {
            _onShowWndSub();
        }

        protected override void _onHideWnd()
        {
            _onHideWndSub();
        }

        protected override void _onReset()
        {
            _onResetSub();
        }

        public void setData(RecruitShopInfo _recruitShopInfo, _ARecruitItemInfo _recruitItemInfo)
        {
            _m_iRecruitShopInfo = _recruitShopInfo;
            if (!(_recruitItemInfo is T_RecruitItemInfo))
            {
                Debug.LogError($"_AGGUIPrefabWndRecruitItemDetail<{typeof(T_Mono)}, {typeof(T_RecruitItemInfo)}> setData error, _recruitItemInfo:{_recruitItemInfo} is not {typeof(T_RecruitItemInfo)}");
            }
            _m_iRecruitItemInfo = _recruitItemInfo as T_RecruitItemInfo;

            _onSetData();
            
            _refreshWnd();
        }

        protected void _refreshWnd()
        {
            if(wnd == null)
                return;

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