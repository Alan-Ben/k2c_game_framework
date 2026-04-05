
using System;
using ALPackage;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GOE
{
    /// <summary>
    /// 直接控制显隐的toggle
    /// </summary>
    public class NPGGUIWndCommonPKSlider : _ATNPBasicSimpleUISubWnd<NPGGUIMonoCommonPKSlider>
    {
        private Tween _m_tween;
        private float _m_leftTargetValue;
        private float _m_rightTargetValue;
        private long _m_serialNum;
        
        public NPGGUIWndCommonPKSlider(NPGGUIMonoCommonPKSlider _wnd)
            : base(_wnd)
        {
            initWnd();
        }

        public float leftValue { get { return _m_leftTargetValue; } }
        public float rightValue { get { return _m_rightTargetValue; } }

        protected override void _onShowWnd()
        {
            _m_serialNum++;
        }

        protected override void _onHideWnd()
        {
            _m_tween.Kill();
            _m_tween = null;
            
            _m_serialNum++;
            
            _reset();

        }


        protected override void _onReset()
        {
            if(wnd == null)
                return;
            _m_tween.Kill();
            _m_tween = null;
            
            _m_serialNum++;
            _reset();

        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;
            _m_tween.Kill();
            _m_tween = null;
            
            _m_serialNum++;
            _reset();

        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;

            _m_serialNum++;
            _reset();

        }
        
        /// <summary>
        /// 初始化信息
        /// </summary>
        /// <param name="_leftStartValue">左边起始值</param>
        /// <param name="_leftTargetValue">左边目标值</param>
        /// <param name="_rightStartValue">右边起始值</param>
        /// <param name="_rightTargetValue">右边目标值</param>
        /// <param name="_time">变化时间</param>
        public void setInfo(float _leftTargetValue, float _rightTargetValue)
        {
            if (wnd == null)
                return;

            _m_leftTargetValue = _leftTargetValue;
            _m_rightTargetValue = _rightTargetValue;
            
            _refreshValue(_m_leftTargetValue, _m_rightTargetValue);
        }
        
        public void setLeftValue(float _leftTargetValue)
        {
            if (wnd == null)
                return;

            _m_leftTargetValue = _leftTargetValue;
            
            _refreshValue(_m_leftTargetValue, _m_rightTargetValue);
        }
        
        public void setRightValue(float _rightTargetValue)
        {
            if (wnd == null)
                return;

            _m_rightTargetValue = _rightTargetValue;
            
            _refreshValue(_m_leftTargetValue, _m_rightTargetValue);
        }
        
        private void _refreshValue(float _leftTargetValue, float _rightTargetValue)
        {
            if (wnd == null || null == wnd.sld)
                return;
            
            if (null != wnd.leftNumAni)
            {
                wnd.leftNumAni.ForcePlay(wnd.leftNumAniName);
            }
            
            if (null != wnd.rightNumAni)
            {
                wnd.rightNumAni.ForcePlay(wnd.rightNumAniName);
            }
            
            float scale = 0.5f;
            if(_leftTargetValue > 0 || _rightTargetValue > 0)
            {
                scale = Mathf.Clamp01(_leftTargetValue / (_leftTargetValue + _rightTargetValue));
            }
            //变动到指定值
            if (wnd.sldChangeTimeS > 0)
            {
                if (_m_tween != null) 
                    _m_tween.Kill();
                _m_tween = wnd.sld.DOValue(scale, wnd.sldChangeTimeS).SetEase(Ease.Linear);
            }
            else
            {
                if (_m_tween != null) 
                    _m_tween.Kill();
                //直接刷新
                ALUGUICommon.setSliderScale(wnd.sld, scale);
            }
            
            long serialNum = _m_serialNum; 
            ALCommonTaskController.CommonActionAddMonoTask(() =>
            {
                if(null == wnd)
                    return;
                
                if(serialNum != _m_serialNum)
                    return;
                
                ALUGUICommon.setLabelTxt(wnd.txtLeftNum, _leftTargetValue.ToLargeString());
                ALUGUICommon.setLabelTxt(wnd.txtRightNum, _rightTargetValue.ToLargeString());
            }, wnd.textChangeTimeS);
        }

        private void _reset()
        {
            _m_leftTargetValue = 0;
            _m_rightTargetValue = 0;

            if (null != wnd)
            {
                ALUGUICommon.setLabelTxt(wnd.txtLeftNum, 0);
                ALUGUICommon.setLabelTxt(wnd.txtRightNum, 0);
                ALUGUICommon.setSliderScale(wnd.sld, 0.5);   
            }
        }
    }
}