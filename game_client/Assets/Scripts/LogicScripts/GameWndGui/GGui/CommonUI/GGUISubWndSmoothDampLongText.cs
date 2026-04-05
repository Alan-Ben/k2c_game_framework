using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndSmoothDampLongText : _ANPGGUIBasicSubWnd<GGUIMonoSmoothDampLongText>
    {
        [NotNull] private readonly _SmoothDampTask _m_smoothDampTask;
        
        private EValueFormatType _m_eValueFormatType;
        private long _m_curValue;
        private float _m_curValueF;
        private long _m_targetValue;
        private float _m_targetValueF;
        private object[] _m_keyParams;
        private object[] _m_keyParamsCombined;
        
        
        public GGUISubWndSmoothDampLongText(GGUIMonoSmoothDampLongText _wnd) 
            : base(_wnd)
        {
            _m_smoothDampTask = new _SmoothDampTask(this);
            
            initWnd();
        }
        

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
        }
        protected override void _onWndInitDone()
        {
        }


        public void refreshWnd(long _value, EValueFormatType _valueFormatType = EValueFormatType.NORMAL, bool _isSmooth = true, params object[] _keyParams)
        {
            _m_eValueFormatType = _valueFormatType;
            _m_keyParams = _keyParams;
            _m_keyParamsCombined = new object[(_keyParams?.Length ?? 0) + 1];
            for (int i = 0; i < _keyParams?.Length; i++)
                _m_keyParamsCombined[i + 1] = _keyParams[i];
            
            if (!_isSmooth)
            {
                _m_curValue = _value;
                _m_curValueF = _value;
                _m_targetValue = _value;
                _m_targetValueF = _value;
                _m_smoothDampTask.stop();
                refreshWnd();
            }
            
            _m_targetValue = _value;
            _m_targetValueF = _value;
            _m_smoothDampTask.start();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_keyParamsCombined == null)
                return;

            string valueStr = wnd.showLargeNumber ? _m_curValue.ToLargeString(_m_eValueFormatType.toLargeStringType()) : _m_curValue.ToString();
            if (string.IsNullOrEmpty(wnd.textKey))
                ALUGUICommon.setLabelTxt(wnd.txtValue, valueStr);
            else
            {
                _m_keyParamsCombined[0] = valueStr;
                ALUGUICommon.setLabelTxt(wnd.txtValue, TextTranslate.instance.getLanguage(wnd.textKey, _m_keyParamsCombined));
            }
        }


        private class _SmoothDampTask : _IALBaseMonoTask
        {
            [NotNull] private readonly GGUISubWndSmoothDampLongText _m_instance;
            private bool _m_isStart;
            private float _m_velocity;
            
            
            public _SmoothDampTask([NotNull] GGUISubWndSmoothDampLongText _instance)
            {
                _m_instance = _instance;
            }
            
            
            private long curValue { get { return _m_instance._m_curValue; } set { _m_instance._m_curValue = value; } }
            private float curValueF { get { return _m_instance._m_curValueF; } set { _m_instance._m_curValueF = value; } }
            private long targetValue { get { return _m_instance._m_targetValue; } set { _m_instance._m_targetValue = value; } }
            private float targetValueF { get { return _m_instance._m_targetValueF; } set { _m_instance._m_targetValueF = value; } }


            public void start()
            {
                if (_m_isStart)
                    return;
                
                _m_isStart = true;
                ALMonoTaskMgr.instance.addMonoTask(this);
            }
            public void stop()
            {
                if (!_m_isStart)
                    return;
                
                _m_isStart = false;
            }
            
            
            public void deal()
            {
                if (!_m_isStart)
                    return;
                
                if (curValue == targetValue || _m_instance.wnd == null)
                {
                    _m_instance.refreshWnd();
                    stop();
                    return;
                }
                
                curValueF = Mathf.SmoothDamp(curValueF, targetValueF, ref _m_velocity, _m_instance.wnd.smoothTime);
                curValue = (long) Math.Ceiling(curValueF);
                _m_instance.refreshWnd();
                
                ALMonoTaskMgr.instance.addNextFrameTask(this);
            }
        }
    }
}