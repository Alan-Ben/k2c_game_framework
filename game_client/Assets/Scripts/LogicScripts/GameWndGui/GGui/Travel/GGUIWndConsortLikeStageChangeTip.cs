using System;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 妃子好感阶段变化提示
    /// </summary>
    public class GGUIWndConsortLikeStageChangeTip : _ANPGGUIBasicWnd<GGUIMonoConsortLikeStageChangeTip>
    {
        private ConsortRefShowInfo _m_ConsortRefShow;
        private TravelConsortRefObj _m_TravelConsortRefObj;//游历妃子配表数据
        private int _m_like;
        private Action _m_aDealCloseWnd;
        
        private NPGGuiWndTexture _m_consortImg;
        private NPGGUIWndCommonShowCase _m_wConsortShowCase;
        private NPGGUIWndProgress _m_wLikeProgress;//好感度进度条
        
        public GGUIWndConsortLikeStageChangeTip() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoConsortLikeStageChangeTip.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoConsortLikeStageChangeTip.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.imgConsort != null)
                _m_consortImg = new NPGGuiWndTexture(wnd.imgConsort);

            if (wnd.monoConsortShowCase != null)
                _m_wConsortShowCase = new NPGGUIWndCommonShowCase(wnd.monoConsortShowCase);

            if (wnd.likeProgress != null)
                _m_wLikeProgress = new NPGGUIWndProgress(wnd.likeProgress);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseBtnClick);
        }
        
        protected override void _onDiscard()
        {
            if (wnd == null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onCloseBtnClick);
            }
            
            _m_aDealCloseWnd = null;
            
            _m_consortImg?.discard();
            _m_consortImg = null;
            
            _m_wConsortShowCase?.discard();
            _m_wConsortShowCase = null;
            
            _m_wLikeProgress?.discard();
            _m_wLikeProgress = null;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_consortImg?.hideWnd();
            
            _m_wConsortShowCase?.hideWnd();
            
            _m_wLikeProgress?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_consortImg?.discardTexture();
            
            _m_wConsortShowCase?.resetWnd();
            
            _m_wLikeProgress?.resetWnd();
        }

        /// <summary>
        /// 妃子id
        /// </summary>
        /// <param name="_consortId"></param>
        public void setData(long _consortId, int _like, Action _dealCloseWnd)
        {
            _m_ConsortRefShow = new ConsortRefShowInfo(_consortId);
            _m_TravelConsortRefObj = GRefdataCoreMgr.instance.travelConsortRefCore.getRef(_consortId);
            _m_like = _like;

            _m_aDealCloseWnd = _dealCloseWnd;

            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(wnd == null || _m_ConsortRefShow == null)
                return;

            if (string.IsNullOrEmpty(wnd.txtDescKey))
            {
                ALUGUICommon.setLabelTxt(wnd.txtDesc, _m_ConsortRefShow.consortTransName);
            }
            else
            {
                ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(wnd.txtDescKey, _m_ConsortRefShow.consortTransName));
            }

            if (_m_consortImg != null)
            {
                _m_consortImg.showWnd();
                _m_consortImg.setTexture(_m_ConsortRefShow.consortSkinShowInfo?.consortCardImage);
            }

            if (_m_wConsortShowCase != null)
            {
                int bgShowIndex = wnd.consortActorShowCaseIndex < 0 ? 0 : wnd.consortActorShowCaseIndex;
                _AShowCaseUnitInfoObj[] unitInfoObjs = new _AShowCaseUnitInfoObj[bgShowIndex + 1];
                ShowCaseCommonResUnitInfoObj resUnitInfoObj = new ShowCaseCommonResUnitInfoObj(_m_ConsortRefShow.consortSkinShowInfo?.tdShow);
                
                unitInfoObjs[bgShowIndex] = resUnitInfoObj;
                
                _m_wConsortShowCase.showWnd(unitInfoObjs);
            }

            if (_m_wLikeProgress != null)
            {
                _m_wLikeProgress.showWnd();
                _m_wLikeProgress.setProgress(_m_like, _m_TravelConsortRefObj?.marry_need_like ?? _m_like, EValueFormatType.NORMAL);
            }
        }

        private void _onCloseBtnClick(GameObject _go)
        {
            _m_aDealCloseWnd?.Invoke();
        }
    }
}