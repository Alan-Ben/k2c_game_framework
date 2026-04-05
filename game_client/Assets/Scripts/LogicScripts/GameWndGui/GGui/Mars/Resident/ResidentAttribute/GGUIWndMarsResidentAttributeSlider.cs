using ALPackage;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星基地 - 居民属性进度条子窗口
    /// </summary>
    public class GGUIWndMarsResidentAttributeSlider : _ANPGGUIBasicSubWnd<GGUIMonoMarsResidentAttributeSlider>
    {
        private NPGGUIWndProgress _m_wAttributeProgress;

        public GGUIWndMarsResidentAttributeSlider(GGUIMonoMarsResidentAttributeSlider _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoAttributeSlider != null)
                _m_wAttributeProgress = new NPGGUIWndProgress(wnd.monoAttributeSlider);
        }

        protected override void _onDiscard()
        {
            _m_wAttributeProgress?.discard();
            _m_wAttributeProgress = null;
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wAttributeProgress?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wAttributeProgress?.resetWnd();
        }

        /// <summary>
        /// 设置进度条的值
        /// </summary>
        /// <param name="_curValue">当前值</param>
        /// <param name="_maxValue">最大值</param>
        /// <param name="_formatType">数值格式化类型</param>
        public void setProgress(long _curValue, long _maxValue, EValueFormatType _formatType = EValueFormatType.NORMAL_NOT_LARGE_STR)
        {
            if (_m_wAttributeProgress != null)
            {
                _m_wAttributeProgress.showWnd();
                _m_wAttributeProgress.setProgress(_curValue, _maxValue, _formatType);
                _m_wAttributeProgress.setProgressTxt(GCommon.getValueFormatStr(_formatType, _curValue),_curValue == _maxValue);
                // _m_wAttributeProgress.setProgressTxt((_curValue / _maxValue * 100).ToString("0.##"), _curValue == _maxValue);
            }

            _refreshNormalizeValueShow(_curValue, _maxValue);
        }

        /// <summary>
        /// 设置进度条的标准化值(0-1)
        /// </summary>
        /// <param name="_normalizeValue">标准化值</param>
        public void setProgress(float _normalizeValue)
        {
            if (_m_wAttributeProgress != null)
            {
                _m_wAttributeProgress.showWnd();
                _m_wAttributeProgress.setProgress(_normalizeValue);
                _m_wAttributeProgress.setProgressTxt(_normalizeValue.ToString("0.##"), _normalizeValue >= 1);
            }

            _refreshNormalizeValueShow(_normalizeValue);
        }

        /// <summary>
        /// 根据当前值和最大值刷新标准化值显示
        /// </summary>
        /// <param name="_curValue">当前值</param>
        /// <param name="_maxValue">最大值</param>
        private void _refreshNormalizeValueShow(long _curValue, long _maxValue)
        {
            if (wnd == null || wnd.normalizeValueShowList == null)
                return;

            float normalizeValue = _maxValue > 0 ? (float)_curValue / _maxValue : 0f;
            _refreshNormalizeValueShow(normalizeValue);
        }

        /// <summary>
        /// 根据标准化值刷新显示，只显示小于等于_normalizeValue的最后一档（单次遍历）
        /// </summary>
        /// <param name="_normalizeValue">标准化值</param>
        private void _refreshNormalizeValueShow(float _normalizeValue)
        {
            if (wnd == null || wnd.normalizeValueShowList == null || wnd.normalizeValueShowList.Count == 0)
                return;

            MarsResidentAttributeSliderNormalizeValueShow needShownValueShow = null;
            foreach (var item in wnd.normalizeValueShowList)
            {
                if(item == null)
                    continue;

                if (needShownValueShow == null || needShownValueShow.sliderNormalizeValue <= _normalizeValue)
                {
                    needShownValueShow = item;
                }
                
                ALUGUICommon.setGameObjEnable(item.showGoList, false);
            }
            
            if(needShownValueShow != null)
                ALUGUICommon.setGameObjEnable(needShownValueShow.showGoList, true);
        }
    }
}