using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 剧情对话事件弹窗
    /// </summary>
    public class GGUIWndPlotDialogue : _ANPGGUIBasicWnd<GGUIMonoPlotDialogue>
    {
        private static GGUIWndPlotDialogue _g_instance;
        public static GGUIWndPlotDialogue instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndPlotDialogue();
                return _g_instance;
            }
        }

        //剧情对话附加窗口
        private GGUIWndSubPlotDialogue _m_wSubPlotDialogue;
        //对话结束回调
        private Action _m_aDoneAction;

        public GGUIWndPlotDialogue() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoPlotDialogue.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoPlotDialogue.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        public override bool needDiscardOnSwitch
        {
            get { return true; }
        }

        protected override void _onShowWnd()
        {
            _m_wSubPlotDialogue?.showWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wSubPlotDialogue?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wSubPlotDialogue?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wSubPlotDialogue?.discard();
            _m_wSubPlotDialogue = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoSubPlotDialogue != null)
                _m_wSubPlotDialogue = new GGUIWndSubPlotDialogue(wnd.monoSubPlotDialogue);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_refObj"></param>
        /// <param name="_doneAction"></param>
        public void setInfo(NPDialogueRefObj _refObj, Action _doneAction = null)
        {
            if (_refObj == null)
                return;

            _m_aDoneAction = _doneAction;
            _m_wSubPlotDialogue?.setInfo(_refObj, null, _onDialogueEnd);
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        public void playAudio(long _audioId)
        {
            _m_wSubPlotDialogue?.playAudio(_audioId);
        }

        private void _onDialogueEnd()
        {
            _m_aDoneAction?.Invoke();
            QueueMgr.instance.forceCloseNodeByType(typeof(GMainQueuePlotDialogueNode));
        }
    }
}