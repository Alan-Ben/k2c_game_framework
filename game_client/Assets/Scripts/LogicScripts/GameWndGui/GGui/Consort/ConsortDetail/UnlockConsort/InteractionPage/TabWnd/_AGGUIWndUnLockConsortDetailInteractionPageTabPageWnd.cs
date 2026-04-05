using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public abstract class _AGGUIWndUnLockConsortDetailInteractionPageTabPageWnd<T> : _ANPGGUIBasicLoadPrefabSubWnd<T> 
        where T : _AGGUIMonoUnLockConsortDetailInteractionPageTabPageMono
    {
        protected _IGGUIWndUnlockConsortDetailInteractionPageParam _m_PageParam;//页面参数
        protected NPCommonAssetPathInfo _m_iCommonAssetPathInfo;//通用资源路径信息

        protected GGottenConsortInfo _m_iConsortInfo;//妃子信息
        
        private GGUISubWndUnlockConsortDetailInfo _m_wConsortDetailInfo;//妃子详细信息窗口
        
        public _AGGUIWndUnLockConsortDetailInteractionPageTabPageWnd(_IGGUIWndUnlockConsortDetailInteractionPageParam _pageParam, NPCommonAssetPathInfo _commonAssetPathInfo, Transform _parent) : base(_parent)
        {
            _m_PageParam = _pageParam;
            _m_iCommonAssetPathInfo = _commonAssetPathInfo;

            if (_m_PageParam == null)
            {
                Debug.LogError("_AGGUIWndUnLockConsortDetailInteractionPageTabPageWnd 初始化传入的参数为空");
            }
        }

        protected override string _monoAssetPath { get { return _m_iCommonAssetPathInfo?.asset_path; } }
        protected override string _monoObjName { get { return _m_iCommonAssetPathInfo?.obj_name; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        public abstract EUnlockConsortDetailWndInteractionPageTabType tabPageType { get; }
        
        public event Action<EUnlockConsortDetailWndInteractionPageTabType> onCloseTabPageBtnClick;//关闭页面回调

        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoConsortDetailInfo != null)
                _m_wConsortDetailInfo = new GGUISubWndUnlockConsortDetailInfo(wnd.monoConsortDetailInfo);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseBtnClick);
            
            _onWndInitDoneSub();
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onCloseBtnClick);
            }

            _m_wConsortDetailInfo?.discard();
            _m_wConsortDetailInfo = null;
            
            onCloseTabPageBtnClick = null;
            _onDiscardSub();
        }

        
        protected override void _onShowWnd()
        {
            if (wnd != null)
            {
                _sampleAnimation(wnd.onlyShowActorAnim, wnd.onlyShowActorFuncOffAnimName, 1f);
            }
            
            _onShowWndSub();
        }

        protected override void _onHideWnd()
        {
            _m_wConsortDetailInfo?.hideWnd();

            _onHideWndSub();
        }

        protected override void _onReset()
        {
            _m_wConsortDetailInfo?.resetWnd();

            _onResetSub();
        }

        public void setData(GGottenConsortInfo _consortInfo)
        {
            _m_iConsortInfo = _consortInfo;

            _setDataSub();

            _refreshWnd();
        }
        
        protected void _refreshWnd()
        {
            if (_m_wConsortDetailInfo != null)
            {
                _m_wConsortDetailInfo.showWnd();
                _m_wConsortDetailInfo.setData(_m_iConsortInfo);
            }
            
            _refreshWndSub();
        }

        protected abstract void _onWndInitDoneSub();
        protected abstract void _onDiscardSub();
        protected abstract void _onShowWndSub();
        protected abstract void _onHideWndSub();
        protected abstract void _onResetSub();

        protected abstract void _setDataSub();
        protected abstract void _refreshWndSub();
        
        /// <summary>
        /// 
        /// </summary>
        protected void _onCloseBtnClick(GameObject _go)
        {
            onCloseTabPageBtnClick?.Invoke(tabPageType);
        }
        
        #region 仅展示形象功能

        /// <summary>
        /// 仅展示形象
        /// </summary>
        /// <param name="_show"></param>
        public void onlyShowActor(bool _show)
        {
            if(wnd == null)
                return;
            
            if (_show)
            {
                _playAnimation(wnd.onlyShowActorAnim, wnd.onlyShowActorFuncOnAnimName);
            }
            else
            {
                _playAnimation(wnd.onlyShowActorAnim, wnd.onlyShowActorFuncOffAnimName);
            }
        }

        #endregion

        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="_anim"></param>
        /// <param name="_animationName"></param>
        private void _playAnimation(Animation _anim, string _animationName, Action _playDone = null)
        {
            if (_anim == null || string.IsNullOrEmpty(_animationName))
            {
                _playDone?.Invoke();
                return;
            }

            _anim.ForcePlay(_animationName, 0f, _playDone);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_anim"></param>
        /// <param name="_animationName"></param>
        private void _sampleAnimation(Animation _anim, string _animationName, float _normalizeTime)
        {
            if(_anim == null || string.IsNullOrEmpty(_animationName))
                return;
            
            _anim.Sample(_animationName, _normalizeTime);
        }
    }
}