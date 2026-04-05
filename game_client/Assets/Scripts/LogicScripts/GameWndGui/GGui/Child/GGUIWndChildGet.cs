using System;
using ALPackage;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUIWndChildGet : _ANPGGUIBasicWnd<GGUIMonoChildGet>
    {
        [NotNull] public static GGUIWndChildGet instance { get { return _g_instance ??= new GGUIWndChildGet(); } }
        private static GGUIWndChildGet _g_instance;


        private ChildInfo _m_childInfo;
        private bool _m_bIsGotoChildInfoWnd;//是否跳转到子嗣信息界面
        private Action _m_aOnCamebackFromChildInfo;//从子嗣信息界面回来时的回调

        private GGUISubWndChildInfo _m_childInfoWnd;
        
        public GGUIWndChildGet()
            : base(EALUIWndLayer.ADDITION)
        {
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoChildGet.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoChildGet.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        public bool isGotoChildInfoWnd { get { return _m_bIsGotoChildInfoWnd; } }

        protected override void _onShowWnd()
        {
            _m_childInfoWnd?.showWnd();

            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_childInfoWnd?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_childInfoWnd?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_childInfoWnd?.discard();
            _m_childInfoWnd = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnJumpTo, _onBtnJumpToClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnSkipAnim, _onBtnSkipAnimClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.monoChildInfo != null)
                _m_childInfoWnd = new GGUISubWndChildInfo(wnd.monoChildInfo);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.combineBtnClick(wnd.btnJumpTo, _onBtnJumpToClick);
            ALUGUICommon.combineBtnClick(wnd.btnSkipAnim, _onBtnSkipAnimClick);
        }


        public void refreshWnd(ChildInfo _childInfo, Action _onCamebackFromChildInfo = null)
        {
            _m_childInfo = _childInfo;
            _m_bIsGotoChildInfoWnd = false;
            _m_aOnCamebackFromChildInfo = _onCamebackFromChildInfo;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;
            
            _m_childInfoWnd?.refreshWnd(_m_childInfo);
        }
        
        
        private void _onBtnCloseClick(GameObject _)
        {
            QueueMgr.instance.DoUIRollBackByEsc();
        }
        private void _onBtnSkipAnimClick(GameObject _)
        {
            if (wnd == null || wnd.skipAnimation == null || string.IsNullOrEmpty(wnd.skipAnimationName))
                return;

            wnd.skipAnimation.Stop();
            wnd.skipAnimation.Sample(wnd.skipAnimationName, 1);
        }
        private void _onBtnJumpToClick(GameObject _)
        {
            Action onCamebackFromChildInfo = _m_aOnCamebackFromChildInfo;
            _m_aOnCamebackFromChildInfo = null;
            _m_bIsGotoChildInfoWnd = true;
            
            NPUINoticeMgr.instance.removeDealerByType(typeof(NoticeDealer_ChildGet));
            _onBtnCloseClick(null);

            if (QueueMgr.instance._lastNode is GNodeChild childNode)
            {
                childNode.viewMgr.selectSeat(_m_childInfo?.seatInfo);
                childNode.regCloseNodeCallback(onCamebackFromChildInfo);
                return;
            }
            
            
            QueueMgr.instance.AddNode(new GNodeChild(_m_childInfo?.seatInfo, onCamebackFromChildInfo));            
        }
    }
}