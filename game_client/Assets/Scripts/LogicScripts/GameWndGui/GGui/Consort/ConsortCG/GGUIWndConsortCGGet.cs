using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 获取CG弹窗
    /// </summary>
    public class GGUIWndConsortCGGet : _ANPGGUIBasicWnd<GGUIMonoConsortCGGet>
    {
        private static GGUIWndConsortCGGet _g_instance;
        public static GGUIWndConsortCGGet instance { get { return _g_instance ??= new GGUIWndConsortCGGet(); } }
        
        private ConsortCGRefObj _m_rConsortCGRefObj;//妃子CG表数据
        
        private NPGGuiWndTexture _m_wConsortCGImg;//妃子CG图片
        
        public GGUIWndConsortCGGet() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoConsortCGGet.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoConsortCGGet.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        /// <summary>
        /// 在切换主视图时是否会需要释放
        /// 一般不释放，如果子类有需要可以重载函数处理
        /// </summary>
        public override bool needDiscardOnSwitch
        {
            get { return true; }
        }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.cgImage != null)
                _m_wConsortCGImg = new NPGGuiWndTexture(wnd.cgImage);
            
            ALUGUICommon.combineBtnClick(wnd.btnSure, _onSureBtnClick);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnSure, _onSureBtnClick);
            }

            _m_wConsortCGImg?.discard();
            _m_wConsortCGImg = null;
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wConsortCGImg?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wConsortCGImg?.discardTexture();
        }

        /// <summary>
        /// 设置CG数据
        /// </summary>
        /// <param name="_consortCgRef">CG配表数据</param>
        public void setData(ConsortCGRefObj _consortCgRef)
        {
            _m_rConsortCGRefObj = _consortCgRef;

            _refreshWnd();
        }

        /// <summary>
        /// 刷新显示
        /// </summary>
        private void _refreshWnd()
        {
            if(wnd == null || _m_rConsortCGRefObj == null || !isShow)
                return;

            // 设置CG名称
            ALUGUICommon.setLabelTxt(wnd.txtCGName, TextTranslate.instance.getLanguage(_m_rConsortCGRefObj.name));

            // 设置CG图片
            if (_m_wConsortCGImg != null && _m_rConsortCGRefObj.cg_icon != null && _m_rConsortCGRefObj.cg_icon.enable())
            {
                _m_wConsortCGImg.showWnd();
                _m_wConsortCGImg.setTexture(_m_rConsortCGRefObj.cg_icon);
            }
        }

        /// <summary>
        /// 点击确认按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onSureBtnClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_CONSORT_CG_GET);
        }
    }
}