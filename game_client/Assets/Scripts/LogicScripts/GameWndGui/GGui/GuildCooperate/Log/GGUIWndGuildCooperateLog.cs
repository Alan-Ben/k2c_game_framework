using System.Collections.Generic;
using ALPackage;
using CommonEnum;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 公会协作日志弹窗
    /// </summary>
    public class GGUIWndGuildCooperateLog : _ANPGGUIBasicWnd<GGUIMonoGuildCooperateLog>
    {
        private static GGUIWndGuildCooperateLog _g_instance;
        public static GGUIWndGuildCooperateLog instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndGuildCooperateLog();
                return _g_instance;
            }
        }

        // 日志列表
        private GGUIWndGuildCooperateLogGrid _m_wLogGrid;

        public GGUIWndGuildCooperateLog() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoGuildCooperateLog.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoGuildCooperateLog.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        protected override bool isShowAniPlayOnlyOne { get { return true; } }

        protected override void _onShowWnd()
        {
            _m_wLogGrid?.showWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wLogGrid?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wLogGrid?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wLogGrid?.discard();
            _m_wLogGrid = null;

            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if(wnd.monoLogGrid != null)
                _m_wLogGrid = new GGUIWndGuildCooperateLogGrid(wnd.monoLogGrid);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_GUIlD_COOPERATE_LOG);
        }
    }
}