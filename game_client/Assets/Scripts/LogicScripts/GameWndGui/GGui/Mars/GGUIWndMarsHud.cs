using System.Collections.Generic;
using ALPackage;
using GOE.FollowItem;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndMarsHud : _ATNPGGUIWndCommonFollowRootWnd<GGUIMonoMarsHud>
    {
        [NotNull] public static GGUIWndMarsHud instance { get { return _g_instance ??= new GGUIWndMarsHud(); } }
        private static GGUIWndMarsHud _g_instance;
        
        
        [NotNull] private readonly Dictionary<_AALGGUICommonFollowItemController, _AutoHideData> _m_autoHideControllers;
        private _AALGGUICommonFollowItemController _m_currentMutexController;
        private ALCommonEnableTaskController _m_tickTask;
        private float _m_autoHideTime;


        private GGUIWndMarsHud()
            : base()
        {
            _m_autoHideControllers = new Dictionary<_AALGGUICommonFollowItemController, _AutoHideData>();
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoMarsHud.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsHud.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            base._onShowWnd();

            WinMsg.RegisterMsgAct(WinMsgType.ON_NODE_CHG, _onNodeChg);
            _m_tickTask = ALCommonEnableTickActionMonoTask.addMonoTask(_tick);
        }
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_NODE_CHG, _onNodeChg);
            _m_tickTask.setDisable();

            base._onHideWnd();
        }
        protected override void _onDiscard()
        {
            _m_currentMutexController?.discard();
            _m_currentMutexController = null;
            
            base._onDiscard();
        }
        protected override void _onWndInitDone()
        {
            base._onWndInitDone();

            if (wnd == null)
                return;

            _m_autoHideTime = wnd.autoHideTime;
        }


        public void addMutexController(GGUICommonFollowTarget _followTarget, _AALGGUICommonFollowItemController _controller)
        {
            _m_currentMutexController?.discard();
            _m_currentMutexController = _controller;
            addController(_followTarget, _controller);
        }
        public void clearMutexController()
        {
            _m_currentMutexController?.discard();
            _m_currentMutexController = null;
        }
        public void addAutoHideController(GGUICommonFollowTarget _followTarget, _AALGGUICommonFollowItemController _controller)
        {
            if (_controller == null)
                return;

            if (_m_autoHideControllers.ContainsKey(_controller))
            {
                ALLog.Error("addAutoHideController failed, controller already exists.");
                return;
            }

            _AutoHideData autoHideData = new _AutoHideData(_controller, _m_autoHideTime);
            _m_autoHideControllers.Add(_controller, autoHideData);
            addController(_followTarget, _controller);
        }
        public void removeAutoHideController(_AALGGUICommonFollowItemController _controller)
        {
            if (_controller == null)
                return;

            if (_m_autoHideControllers.Remove(_controller, out _AutoHideData autoHideData))
            {
                _controller.discard();
                autoHideData.dispose();
            }
        }
        public void showAutoHideController(_AALGGUICommonFollowItemController _controller)
        {
            if (_controller == null)
                return;

            if (!_m_autoHideControllers.TryGetValue(_controller, out _AutoHideData autoHideData))
                return;

            autoHideData.show();
        }
        public void showAllAutoHideController()
        {
            foreach (_AutoHideData autoHideData in _m_autoHideControllers.Values)
            {
                autoHideData.show();
            }
        }


        private void _tick()
        {
            if (wnd == null)
                return;

            float autoHideTime = wnd.autoHideTime;
            if (autoHideTime <= 0f)
                return;

            float deltaTime = Time.deltaTime;
            foreach (_AutoHideData autoHideData in _m_autoHideControllers.Values)
            {
                autoHideData.tick(deltaTime);
            }
        }


        private void _onNodeChg()
        {
            clearMutexController();
        }


        private class _AutoHideData
        {
            [NotNull] private readonly _AALGGUICommonFollowItemController _m_controller;
            private readonly float _m_autoHideTime;
            private float _m_hideTimer;
            private bool _m_isShown;
            private bool _m_isEnable;


            public _AutoHideData([NotNull] _AALGGUICommonFollowItemController _controller, float _autoHideTime)
            {
                _m_controller = _controller;
                _m_autoHideTime = _autoHideTime;
                _m_isEnable = true;
                
                _m_hideTimer = 0f;
                if (_m_autoHideTime > 0f)
                    _hide();
                
                if (!_m_controller.ItemWndIsLoadDone)
                    _m_controller.regItemWndLoadDoneDelegate(_onControllerInit);
            }


            public void dispose()
            {
                _m_isEnable = false;
            }
            public void tick(float _deltaTime)
            {
                if (_m_hideTimer <= 0f)
                    return;

                _m_hideTimer -= _deltaTime;
                if (_m_hideTimer <= 0f)
                {
                    _hide();
                }
            }

            public void show()
            {
                _m_hideTimer = _m_autoHideTime;
                _show();
            }


            private void _show()
            {
                _m_isShown = true;
                ALUGUICommon.setGameObjEnable(_m_controller.itemMono, _m_isShown);
            }
            private void _hide()
            {
                _m_isShown = false;
                ALUGUICommon.setGameObjEnable(_m_controller.itemMono, _m_isShown);
            }
            private void _onControllerInit()
            {
                if (!_m_isEnable)
                    return;
                
                ALUGUICommon.setGameObjEnable(_m_controller.itemMono, _m_isShown);
            }
        }
    }
}