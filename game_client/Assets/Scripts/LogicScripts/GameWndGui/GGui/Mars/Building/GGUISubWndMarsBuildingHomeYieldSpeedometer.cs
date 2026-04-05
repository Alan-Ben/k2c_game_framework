using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUISubWndMarsBuildingHomeYieldSpeedometer : _ATALBasicUISubWnd<GGUIMonoMarsBuildingHomeYieldSpeedometer>
    {
        private float _m_currentValue;
        private float _m_targetValue;
        private float _m_currentVelocity;
        private ALCommonEnableTaskController _m_tickTask;


        public GGUISubWndMarsBuildingHomeYieldSpeedometer(GGUIMonoMarsBuildingHomeYieldSpeedometer _wnd) 
            : base(_wnd)
        {
            initWnd();
        }


        protected override void _onShowWnd()
        {
            _m_tickTask = ALCommonEnableTickActionMonoTask.addMonoTask(_tick);
        }
        protected override void _onHideWnd()
        {
            _m_tickTask.setDisable();
        }
        protected override void _onReset()
        {
        }
        protected override void _onDiscard()
        {
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            _m_currentValue = wnd.minValue;
            _m_targetValue = wnd.minValue;
            _m_currentVelocity = 0f;
            _updateDisplay(_m_currentValue);
            _refreshLabels();
        }


        public void setValue(float _value, bool _immediate = false)
        {
            if (wnd == null)
                return;

            _m_targetValue = _value;

            if (_immediate)
            {
                _m_currentValue = _m_targetValue;
                _m_currentVelocity = 0f;
                _updateDisplay(_m_currentValue);
            }
        }


        private void _tick()
        {
            if (wnd == null)
                return;

            if (Mathf.Abs(_m_currentValue - _m_targetValue) < 0.001f && Mathf.Abs(_m_currentVelocity) < 0.001f)
            {
                _m_currentValue = _m_targetValue;
                _m_currentVelocity = 0f;
                _updateDisplay(_m_currentValue);
                return;
            }

            float deltaTime = Time.deltaTime;
            float smoothTime = Mathf.Max(0.01f, wnd.smoothTime);

            if (wnd.maxOvershoot > 0f)
            {
                _m_currentValue = NPGameUtility.SpringDamp(_m_currentValue, _m_targetValue, ref _m_currentVelocity, smoothTime, deltaTime, wnd.maxOvershoot);
            }
            else
            {
                _m_currentValue = NPGameUtility.SpringDamp(_m_currentValue, _m_targetValue, ref _m_currentVelocity, smoothTime, deltaTime);
            }

            _updateDisplay(_m_currentValue);
        }

        private void _updateDisplay(float _value)
        {
            if (wnd == null)
                return;

            float range = wnd.maxValue - wnd.minValue;
            float t = range > 0f ? (_value - wnd.minValue) / range : 0f;

            if (wnd.transNeedle != null)
            {
                float angle = Mathf.LerpUnclamped(wnd.startAngle, wnd.endAngle, t);
                wnd.transNeedle.localEulerAngles = new Vector3(0f, 0f, angle);
            }
            
            if (wnd.sliderSpeedometer != null)
            {
                float totalAngle = Mathf.Abs(wnd.endAngle - wnd.startAngle);
                float sliderRange = totalAngle / 360f;
                float sliderOffset = 0.5f - (sliderRange / 2f);
                wnd.sliderSpeedometer.minValue = 0;
                wnd.sliderSpeedometer.maxValue = 1;
                wnd.sliderSpeedometer.value = sliderOffset + t * sliderRange;
            }
        }

        private void _refreshLabels()
        {
            if (wnd == null || wnd.listTextSpeedometerLabels == null || wnd.listTextSpeedometerLabels.Count == 0)
                return;

            int labelCount = wnd.listTextSpeedometerLabels.Count;
            float valueRange = wnd.maxValue - wnd.minValue;

            if (labelCount <= 1)
            {
                Text txt = wnd.listTextSpeedometerLabels[0];
                if (txt != null)
                {
                    string labelText = wnd.maxValue.ToString("F0");
                    ALUGUICommon.setLabelTxt(txt, labelText);
                }
                
                return;
            }
            
            for (int i = 0; i < labelCount; i++)
            {
                if (wnd.listTextSpeedometerLabels[i] == null)
                    continue;

                float t = (float)i / (labelCount - 1);
                float labelValue = wnd.minValue + valueRange * t;

                string labelText = labelValue.ToString("F0");
                ALUGUICommon.setLabelTxt(wnd.listTextSpeedometerLabels[i], labelText);
            }
        }
    }
}
