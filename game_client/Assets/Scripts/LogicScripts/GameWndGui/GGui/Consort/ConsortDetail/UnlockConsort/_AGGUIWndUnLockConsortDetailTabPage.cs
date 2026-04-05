using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public abstract class _AGGUIWndUnLockConsortDetailTabPage<T> : _ANPGGUIBasicLoadPrefabSubWnd<T>, _IUnLockConsortDetailTabPage 
        where T : _AGGUIMonoUnLockConsortDetailTabPage
    {
        protected NPCommonAssetPathInfo _m_iCommonAssetPathInfo;//通用资源路径信息
        
        protected GGottenConsortInfo _m_iConsortShowInfo;//当前显示的妃子信息
        protected Action<EUnLockConsortDetailWndTabType> _m_aOnCloseTabPage;//关闭页面回调
        
        private GGUISubWndUnlockConsortDetailInfo _m_wConsortDetailInfo;//妃子详细信息窗口
        
        public _AGGUIWndUnLockConsortDetailTabPage(NPCommonAssetPathInfo _commonAssetPathInfo, Transform _parent) : base(_parent)
        {
            _m_iCommonAssetPathInfo = _commonAssetPathInfo;
        }
        
        protected override string _monoAssetPath { get { return _m_iCommonAssetPathInfo?.asset_path; } }
        protected override string _monoObjName { get { return _m_iCommonAssetPathInfo?.obj_name; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        /// <summary>
        /// 获取显示窗口对象
        /// </summary>
        public _AALBasicLoadUIWndBasicClass tabWndObj { get { return this; } }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoConsortDetailInfo != null)
                _m_wConsortDetailInfo = new GGUISubWndUnlockConsortDetailInfo(wnd.monoConsortDetailInfo);
            
            ALUGUICommon.combineBtnClick(wnd.closeBtn, _onCloseBtnClick);
            
            _onWndInitDoneSub();
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.closeBtn, _onCloseBtnClick);
            }
            
            _m_wConsortDetailInfo?.discard();
            _m_wConsortDetailInfo = null;
            
            _m_aOnCloseTabPage = null;

            _onDiscardSub();
        }
        
        protected override void _onShowWnd()
        {
            if (wnd != null)
            {
                _sampleAnimation(wnd.onlyShowActorAnim, wnd.onlyShowActorFuncOffAnimName, 1f);
            }
            
            _onShowWndSub();

            _refreshWnd();
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

        /// <summary>
        /// 本窗口对应的页签类型
        /// </summary>
        public abstract EUnLockConsortDetailWndTabType tabPageType { get; }
        
        protected abstract void _onWndInitDoneSub();
        protected abstract void _onDiscardSub();
        protected abstract void _onShowWndSub();
        protected abstract void _onHideWndSub();
        protected abstract void _onResetSub();

        public virtual void setData(GGottenConsortInfo _consortShowInfo, Action<EUnLockConsortDetailWndTabType> _onCloseTabPage)
        {
            _m_iConsortShowInfo = _consortShowInfo;
            _m_aOnCloseTabPage = _onCloseTabPage;

            _setDataSub();
            
            _refreshWnd();
        }

        protected virtual void _refreshWnd()
        {
            if (_m_wConsortDetailInfo != null)
            {
                _m_wConsortDetailInfo.showWnd();
                _m_wConsortDetailInfo.setData(_m_iConsortShowInfo);
            }
            
            _refreshWndSub();
        }

        // public _AALBasicLoadUIWndBasicClass getPageWnd()
        // {
        //     return this;
        // }

        protected abstract void _setDataSub();
        
        protected abstract void _refreshWndSub();

        /// <summary>
        /// 关闭按钮被点击
        /// </summary>
        private void _onCloseBtnClick(GameObject _go)
        {
            _m_aOnCloseTabPage?.Invoke(tabPageType);
        }
        
        #region 仅展示形象功能

        /// <summary>
        /// 仅展示形象
        /// </summary>
        /// <param name="_show"></param>
        public virtual void onlyShowActor(bool _show)
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