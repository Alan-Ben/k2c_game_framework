using ALPackage;
using ChatPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndChatInfoListPage : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoChatInfoListPage>
    {
        // 这个窗口涉及的一些子窗口
        private NPGGUISubWndMsgItemList _m_msgListWnd;
        private NPGGUISubWndChatInputter _m_inputterWnd;
        private _AChatInfo _m_chatInfo;
        private NPGGuiWndTexture _m_iconWnd;
        private enum AdditionContentState { Hide, FadeIn, Show, FadeOut }
        [NotNull] private readonly _TALSimpleStateMachine<AdditionContentState> _m_additionContentStateMachine;
        private float _m_additionContentSize;
        private float _m_additionContentFadeTime;

        public GGUIWndChatInfoListPage(Transform _parent) : base(_parent)
        {
            _m_additionContentStateMachine = new _TALSimpleStateMachine<AdditionContentState>();
        }

        protected override string _monoAssetPath { get => GGUIMonoChatInfoListPage.assetPath; }
        protected override string _monoObjName { get => GGUIMonoChatInfoListPage.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

        protected override void _onShowWnd()
        {
            _refreshWnd();
            _m_iconWnd?.showWnd();
            _m_msgListWnd?.showWnd();
            _m_inputterWnd?.showWnd();
        }

        protected override void _onHideWnd()
        {
            _m_iconWnd?.hideWnd();
            _m_msgListWnd?.hideWnd();
            _m_inputterWnd?.hideWnd();

            _m_additionContentStateMachine.changeState(new HideAdditionContentState(this));
        }

        protected override void _onReset()
        {
            _m_iconWnd?.discardTexture();
            _m_msgListWnd?.resetWnd();
            _m_inputterWnd?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_msgListWnd?.discard();
            _m_msgListWnd = null;
            
            _m_inputterWnd?.discard();
            _m_inputterWnd = null;
            
            _m_iconWnd?.discard();
            _m_iconWnd = null;
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;
            if (wnd.monoMsgList != null)
                _m_msgListWnd = new NPGGUISubWndMsgItemList(wnd.monoMsgList, NPGGUIChatMsgItemFactory.instance.msgItemCacheMgr);
            if (wnd.monoInputter != null)
            {
                _m_inputterWnd = new NPGGUISubWndChatInputter(wnd.monoInputter);
                _m_inputterWnd.onAddPageIsShow += _onAddPageIsShow;
            }
            if (wnd.rawImgChatIcon != null)
                _m_iconWnd = new NPGGuiWndTexture(wnd.rawImgChatIcon);

            _m_additionContentSize = wnd.additionContentSize;
            if (_m_additionContentSize <= 0)
                _m_additionContentSize = 520;
            _m_additionContentFadeTime = wnd.additionContentAnimTime;
            if (_m_additionContentFadeTime <= 0)
                _m_additionContentFadeTime = 0.25f;
        }

        public void setInfo(_AChatInfo _chatInfo)
        {
            if (null == _chatInfo || !(_chatInfo is _INPChatInfo))
                return;
            
            _m_chatInfo = _chatInfo;
            
            _refreshWnd();
        }

        private void _onAddPageIsShow(bool _isShow)
        {
            _m_additionContentStateMachine.changeState(_isShow ? new FadeInAdditionContentState(this) : new FadeOutAdditionContentState(this));
        }

        private void _refreshWnd()
        {
            if (null == wnd)
                return;
            if (null == _m_chatInfo)
                return;
            // 设置消息列表
            _m_msgListWnd?.setShowData(_m_chatInfo);

            // 设置输入模块
            _m_inputterWnd?.setShowData(_m_chatInfo);

            _INPChatInfo info = _m_chatInfo as _INPChatInfo;

            if (null == info)
                return;
            // 设置icon和名字
            _m_iconWnd?.setTexture(info.getChatIcon());
            ALUGUICommon.setLabelTxt(wnd.txtChatName, TextTranslate.instance.getLanguage(info.getChatName()));
            _m_additionContentStateMachine.changeState(new HideAdditionContentState(this));
            _m_msgListWnd?.moveToButtom();
        }
        /// <summary>
        /// 把窗口上移以便展示附加内容
        /// </summary>
        private void moveUpTheWnd(float _sizeDelta)
        {
            if (wnd == null)
                return;
            
            Vector2 viewportSizeDelta = ((RectTransform)wnd.monoMsgList.scrollRect.transform).sizeDelta;
            viewportSizeDelta = new Vector2(viewportSizeDelta.x, viewportSizeDelta.y - _sizeDelta);
            ((RectTransform)wnd.monoMsgList.scrollRect.transform).sizeDelta = viewportSizeDelta;

            Vector2 contentPosition = wnd.monoMsgList.scrollRect.content.anchoredPosition;
            contentPosition = new Vector2(contentPosition.x, contentPosition.y + _sizeDelta);
            wnd.monoMsgList.scrollRect.content.anchoredPosition = contentPosition;
        }
        /// <summary>
        /// 直接设置界面抬高多少
        /// </summary>
        private void setTheWndUp(float _size)
        {
            if (wnd == null)
                return;
            
            Vector2 viewportSizeDelta = ((RectTransform)wnd.monoMsgList.scrollRect.transform).sizeDelta;
            viewportSizeDelta = new Vector2(viewportSizeDelta.x, -_size);
            ((RectTransform)wnd.monoMsgList.scrollRect.transform).sizeDelta = viewportSizeDelta;

            // Vector2 contentPosition = wnd.monoMsgList.scrollRect.content.anchoredPosition;
            // contentPosition = new Vector2(contentPosition.x, _size);
            // wnd.monoMsgList.scrollRect.content.anchoredPosition = contentPosition;
        }
        /// <summary>
        /// 获得这个界面抬高了多少
        /// </summary>
        private float getTheWndUp()
        {
            if (wnd == null)
                return 0;

            return -((RectTransform)wnd.monoMsgList.scrollRect.transform).sizeDelta.y;
        }


        private class HideAdditionContentState : _ASimpleState<AdditionContentState>
        {
            [NotNull] private readonly GGUIWndChatInfoListPage _m_wnd;
            public HideAdditionContentState([NotNull] GGUIWndChatInfoListPage _wnd) { _m_wnd = _wnd; }
            public override AdditionContentState state { get { return AdditionContentState.Hide; } }
            

            protected override void _onEnter()
            {
                _m_wnd.setTheWndUp(0);
            }
            protected override void _onExit()
            {
            }
            public override bool canEnterState(AdditionContentState _newState)
            {
                return _newState is AdditionContentState.FadeIn or AdditionContentState.Show;
            }
        }
        private class FadeInAdditionContentState : _ASimpleState<AdditionContentState>
        {
            [NotNull] private readonly GGUIWndChatInfoListPage _m_wnd;
            public FadeInAdditionContentState([NotNull] GGUIWndChatInfoListPage _wnd) { _m_wnd = _wnd; }
            public override AdditionContentState state { get { return AdditionContentState.FadeIn; } }


            private float _m_timer;
            private float _m_targetValue;
            private float _m_totalTime;
            private ALCommonEnableTaskController _m_tickTask;


            protected override void _onEnter()
            {
                _m_totalTime = _m_wnd._m_additionContentFadeTime;
                _m_targetValue = _m_wnd._m_additionContentSize;

                if (_m_targetValue > 0)
                {
                    float upValue = _m_wnd.getTheWndUp();
                    _m_timer = Mathf.Acos(1f - 2f * upValue / _m_targetValue) / Mathf.PI * _m_totalTime;
                }

                _m_tickTask = ALCommonEnableTickActionMonoTask.addMonoTask(_update);
                if (_m_targetValue <= 0 || _m_totalTime <= 0)
                    _m_wnd._m_additionContentStateMachine.changeState(new ShowAdditionContentState(_m_wnd));
            }
            protected override void _onExit()
            {
                _m_tickTask.setDisable();
            }
            public override bool canEnterState(AdditionContentState _newState)
            {
                return true;
            }


            private void _update()
            {
                float deltaTime = Time.deltaTime;
                
                _m_timer += deltaTime;
                if (_m_timer >= _m_totalTime)
                {
                    _m_wnd._m_additionContentStateMachine.changeState(new ShowAdditionContentState(_m_wnd));
                    return;
                }

                float currentValue = _m_wnd.getTheWndUp();
                float fadeValue = -(Mathf.Cos(Mathf.PI * _m_timer / _m_totalTime) - 1) / 2 * _m_targetValue;
                _m_wnd.moveUpTheWnd(fadeValue - currentValue);
            }
        }
        private class ShowAdditionContentState : _ASimpleState<AdditionContentState>
        {
            [NotNull] private readonly GGUIWndChatInfoListPage _m_wnd;
            public ShowAdditionContentState([NotNull] GGUIWndChatInfoListPage _wnd) { _m_wnd = _wnd; }
            public override AdditionContentState state { get { return AdditionContentState.Show; } }
            

            protected override void _onEnter()
            {
                _m_wnd.setTheWndUp(_m_wnd._m_additionContentSize);
            }
            protected override void _onExit()
            {
            }
            public override bool canEnterState(AdditionContentState _newState)
            {
                return true;
            }
        }
        private class FadeOutAdditionContentState : _ASimpleState<AdditionContentState>
        {
            [NotNull] private readonly GGUIWndChatInfoListPage _m_wnd;
            public FadeOutAdditionContentState([NotNull] GGUIWndChatInfoListPage _wnd) { _m_wnd = _wnd; }
            public override AdditionContentState state { get { return AdditionContentState.FadeOut; } }
            

            private float _m_timer;
            private float _m_targetValue;
            private float _m_totalTime;
            private ALCommonEnableTaskController _m_tickTask;


            protected override void _onEnter()
            {
                _m_totalTime = _m_wnd._m_additionContentFadeTime;
                _m_targetValue = _m_wnd._m_additionContentSize;

                if (_m_targetValue > 0)
                {
                    float upValue = _m_wnd.getTheWndUp();
                    _m_timer = Mathf.Acos(1f - 2f * (_m_targetValue - upValue) / _m_targetValue) / Mathf.PI * _m_totalTime;
                }

                _m_tickTask = ALCommonEnableTickActionMonoTask.addMonoTask(_update);
                if (_m_targetValue <= 0 || _m_totalTime <= 0)
                    _m_wnd._m_additionContentStateMachine.changeState(new HideAdditionContentState(_m_wnd));
            }
            protected override void _onExit()
            {
                _m_tickTask.setDisable();
            }
            public override bool canEnterState(AdditionContentState _newState)
            {
                return true;
            }


            private void _update()
            {
                float deltaTime = Time.deltaTime;
                
                _m_timer += deltaTime;
                if (_m_timer >= _m_totalTime)
                {
                    _m_wnd._m_additionContentStateMachine.changeState(new HideAdditionContentState(_m_wnd));
                    return;
                }

                float currentValue = _m_wnd.getTheWndUp();
                float fadeValue = (1 - (-(Mathf.Cos(Mathf.PI * _m_timer / _m_totalTime) - 1) / 2)) * _m_targetValue;
                _m_wnd.moveUpTheWnd(fadeValue - currentValue);
            }
        }
    }
}