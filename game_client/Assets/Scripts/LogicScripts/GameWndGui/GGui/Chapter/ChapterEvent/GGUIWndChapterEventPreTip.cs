using System;
using System.Collections.Generic;
using ALPackage;
using GOE;
using NPCommon;
using NPEnum;
using UnityEngine;

namespace GOE
{
   
    /// <summary>
    /// 关卡事件提示窗口
    /// </summary>
    public class GGUIWndChapterEventPreTip : _ATALBasicUIWnd<GGUIMonoChapterEventPreTip>
    {
        private static GGUIWndChapterEventPreTip _g_instance = new GGUIWndChapterEventPreTip();

        public static GGUIWndChapterEventPreTip instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndChapterEventPreTip();
                return _g_instance;
            }
        }
        private Action _m_doneAction;//关闭回调


        public GGUIWndChapterEventPreTip() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoChapterEventPreTip.assetPath; }
        protected override string _monoObjName { get => GGUIMonoChapterEventPreTip.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
        /// <summary>
        /// 在切换主视图时是否会需要释放
        /// 一般不释放，如果子类有需要可以重载函数处理
        /// </summary>
        public override bool needDiscardOnSwitch { get { return true; } }

        protected override void _onShowWnd()
        {
            refreshWnd();
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        
        }

        protected override void _onDiscard()
        {
            if(null == wnd)
                return;
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
        
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }

        public void refreshWnd()
        {
            if (null == wnd)
            {
                _closePreTipWnd();
                return;
            }
            ALCommonActionMonoTask.addMonoTask(_closePreTipWnd, wnd.showTime);
        }
        
        public void setInfo(Action _onEventPreTipClose)
        {
            if (null == wnd)
                return;
            _m_doneAction = _onEventPreTipClose;
        }

        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        /// <param name="_"></param>
        private void _onBtnCloseClick(GameObject _)
        {
            if (null != _m_doneAction)
                _m_doneAction();
            _m_doneAction = null;
        }

        private void _closePreTipWnd()
        {
            _onBtnCloseClick(null);
        }
    }
}