using System;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public partial class NPGGUIWndDialogBox
    {
        private class NPDialogBoxState_Playing : _ANPDialogBoxState
        {
            private string _m_sContent;//内容文本
            private float _m_fIntervalMs;//字符显示间隔时间（毫秒）
            private float _m_fEnableTimeMs;//进入状态的计时
            private int _m_iCurLength;//当前已显示的长度

            public NPDialogBoxState_Playing([NotNull] NPGGUIWndDialogBox _wnd, float _intervalMs) : base(_wnd)
            {
                _m_fIntervalMs = _intervalMs > 0 ? _intervalMs : 10;
                _m_sContent = string.Empty;
                _m_fEnableTimeMs = 0;
                _m_iCurLength = 0;
            }

            public override ENPDialogBoxState state { get { return ENPDialogBoxState.PLAYING; } }

            public void setContent(string _content)
            {
                _m_sContent = _content;
            }

            public override bool canEnterState(_ATALStateBase<ENPDialogBoxState> _newState)
            {
                return _newState != null && _newState.state == ENPDialogBoxState.END;
            }

            public override void resetData()
            {
                _m_sContent = string.Empty;
                _m_fEnableTimeMs = 0;
                _m_iCurLength = 0;
            }

            protected override void _onEnter()
            {
                wnd._onEnterPlayingState();
            }

            protected override void _onExit()
            {
                wnd._onExitPlayingState();
            }

            protected override void _onTick(float _deltaTime)
            {
                if (_m_iCurLength >= _m_sContent.Length)
                {
                    wnd._onDialogueShowEnd();
                    return;
                }

                //累计进入状态的计时，并计算本次增长的文本长度
                float lastTimeMs = _m_fEnableTimeMs;
                _m_fEnableTimeMs += _deltaTime * 1000;
                int addLength = (int)(_m_fEnableTimeMs / _m_fIntervalMs) - (int)(lastTimeMs / _m_fIntervalMs);

                //没有增加，直接返回
                if (addLength <= 0)
                    return;

                //截取显示的文本
                _m_iCurLength += addLength;
                if (_m_iCurLength > _m_sContent.Length)
                    _m_iCurLength = _m_sContent.Length;
                string finalStr = _m_sContent.Substring(0, _m_iCurLength);

                //触发对应事件
                wnd._onContentTextChg(finalStr);
                if (_m_iCurLength >= _m_sContent.Length)
                    wnd._onDialogueShowEnd();
            }
        }
    }
}
