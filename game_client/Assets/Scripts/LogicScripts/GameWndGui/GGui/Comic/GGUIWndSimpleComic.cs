using UnityEngine;
using System.Collections;
using ALPackage;
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 简易漫画窗口
    /// </summary>
    public class GGUIWndSimpleComic : _ATALBasicUIWnd<GGUIMonoSimpleComic>
    {
        private static GGUIWndSimpleComic _g_instance = new GGUIWndSimpleComic();
        [NotNull]public static GGUIWndSimpleComic instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndSimpleComic();
                return _g_instance;
            }
        }
        
        //当前的字页签
        private GGUIWndSimpleComicSubWnd _m_curPageWnd;
        
        public GGUIWndSimpleComic() : base(EALUIWndLayer.NORMAL)
        {
           
        }

        protected override string _monoAssetPath { get { return GGUIMonoSimpleComic.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoSimpleComic.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            if (_m_curPageWnd != null) 
                _m_curPageWnd.showWnd();
        }

        protected override void _onHideWnd()
        {
            if (_m_curPageWnd != null) 
                _m_curPageWnd.hideWnd();
        }

        protected override void _onReset()
        {
            if (_m_curPageWnd != null) 
                _m_curPageWnd.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (_m_curPageWnd != null) 
                _m_curPageWnd.discard();
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            _m_curPageWnd = new GGUIWndSimpleComicSubWnd(wnd.pageParent);
            _m_curPageWnd.load();

            _m_curPageWnd.regSkipAndDoneAction(_onClickSkipBtn, _onClickDoneBtn);

        }

        //初始化信息
        public void initShowInfo(long _comicId)
        {
            if(null == _m_curPageWnd)
                return;
            
            _m_curPageWnd.initShowInfo(_comicId);
        }

        public void initShowInfo(SimpleComicRefObj _comicRefObj)
        {
            if(null == _m_curPageWnd)
                return;
            
            _m_curPageWnd.initShowInfo(_comicRefObj);
        }
        
        //点击跳过按钮
        private void _onClickSkipBtn()
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_SIMPLE_COMIC);
        }
        
        //点击完成按钮
        private void _onClickDoneBtn()
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_SIMPLE_COMIC);
        }
    }
}
