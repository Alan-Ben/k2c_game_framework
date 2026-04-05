using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 游历地点解锁弹窗
    /// </summary>
    public class GGUIWndTravelPosUnlock : _ANPGGUIBasicWnd<GGUIMonoTravelPosUnlock>
    {
        public static GGUIWndTravelPosUnlock instance { get { return _g_instance ??= new GGUIWndTravelPosUnlock(); } }
        private static GGUIWndTravelPosUnlock _g_instance;

        /// <summary>
        /// 当前展示的地点配表数据
        /// </summary>
        private TravelPosRefObj _m_rTravelPosRefObj;
        // 前往按钮点击事件
        private Action _m_aOnGotoBtnClick;

        /// <summary>
        /// 地点icon纹理
        /// </summary>
        private NPGGuiWndTexture _m_wPosIcon;


        public GGUIWndTravelPosUnlock() : base(EALUIWndLayer.ADDITION)
        {
        }


        protected override string _monoAssetPath { get { return GGUIMonoTravelPosUnlock.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoTravelPosUnlock.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            // 构建地点icon纹理
            if (wnd.icon != null)
                _m_wPosIcon = new NPGGuiWndTexture(wnd.icon);

            // 绑定按钮
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.combineBtnClick(wnd.btnGoto, _onBtnGotoClick);
        }

        protected override void _onShowWnd()
        {
            _m_wPosIcon?.showWnd();

            refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wPosIcon?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wPosIcon?.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_wPosIcon?.discard();
            _m_wPosIcon = null;

            _m_rTravelPosRefObj = null;
            _m_aOnGotoBtnClick = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnGoto, _onBtnGotoClick);
        }


        /// <summary>
        /// 带参刷新 - 设置地点数据
        /// </summary>
        public void refreshWnd(TravelPosRefObj _posRefObj, Action _onGotoBtnClick)
        {
            _m_rTravelPosRefObj = _posRefObj;
            _m_aOnGotoBtnClick = _onGotoBtnClick;
            refreshWnd();
        }

        /// <summary>
        /// 刷新窗口
        /// </summary>
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_rTravelPosRefObj == null)
                return;

            // 地点icon
            if (_m_wPosIcon != null)
            {
                _m_wPosIcon.showWnd();
                _m_wPosIcon.setTexture(_m_rTravelPosRefObj.icon);
            }

            // 地点名称
            ALUGUICommon.setLabelTxt(wnd.txtPosName, TextTranslate.instance.getLanguage(_m_rTravelPosRefObj.name));

            // 地点描述
            ALUGUICommon.setLabelTxt(wnd.txtPosDesc, TextTranslate.instance.getLanguage(_m_rTravelPosRefObj.desc));
        }


        /// <summary>
        /// 关闭按钮点击
        /// </summary>
        private void _onBtnCloseClick(GameObject _)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TRAVEL_POS_UNLOCK);
        }

        /// <summary>
        /// 前往按钮点击
        /// </summary>
        private void _onBtnGotoClick(GameObject _)
        {
            if (_m_rTravelPosRefObj == null)
                return;

            _m_aOnGotoBtnClick?.Invoke();
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TRAVEL_POS_UNLOCK);
        }
    }
}
