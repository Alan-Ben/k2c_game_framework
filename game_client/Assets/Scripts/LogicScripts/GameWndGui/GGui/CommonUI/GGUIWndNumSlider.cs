using System;
using UnityEngine;
using ALPackage;

namespace GOE
{
    public class NPGGUIWndNumSlider : _ATNPBasicSimpleUISubWnd<NPGGUIMonoNumSlider>
    {
        public NPGGUIWndNumSlider(NPGGUIMonoNumSlider _wnd)
            : base(_wnd)
        {
            initWnd();
        }

        private int _m_iUnitValue = 1;//最小变化单位
        private long _m_lLastValue;//记录上一次的slider值
        private long _m_lMinLimit;//slider最小限制
        private long _m_lMaxLimit;//slider最大限制

        /// <summary> 数值变化回调 </summary>
        public Action<long> onNumValueChg;

        /// <summary> 数值变化结束回调（松开slider、点击增减按钮等） </summary>
        public Action<long> onNumValueChgDone;

        /// <summary> 当前slider数值 </summary>
        public long curValue
        {
            get
            {
                if (wnd.sldNumSelect == null)
                    return 1;
                else
                    return (long)wnd.sldNumSelect.value;
            }
        }


        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            onNumValueChg = null;
            onNumValueChgDone = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnNumSelectPlus, _onPlusBtnClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnNumSelectDec, _onDecBtnClick);

            if (wnd.sldNumSelect != null)
            {
                wnd.sldNumSelect.onValueChanged.RemoveListener(_onSliderChg);
                ALUGUICommon.uncombinePointerUp(wnd.sldNumSelect.gameObject, _onSldPointerUp);
            }
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnNumSelectPlus, _onPlusBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnNumSelectDec, _onDecBtnClick);

            if (wnd.sldNumSelect != null)
            {
                wnd.sldNumSelect.onValueChanged.AddListener(_onSliderChg);
                ALUGUICommon.combinePointerUp(wnd.sldNumSelect.gameObject, _onSldPointerUp);
            }

            _m_lLastValue = -1;
        }


        #region 点击事件

        /// <summary>
        /// 点击增加按钮
        /// </summary>
        /// <param name="go"></param>
        private void _onPlusBtnClick(GameObject go)
        {
            setCurValue(curValue + _m_iUnitValue);
        }

        /// <summary>
        /// 点击减少按钮
        /// </summary>
        /// <param name="go"></param>
        private void _onDecBtnClick(GameObject go)
        {
            setCurValue(curValue - _m_iUnitValue);
        }

        /// <summary>
        /// 松开slider
        /// </summary>
        /// <param name="_noUse"></param>
        private void _onSldPointerUp(Vector2 _noUse)
        {
            _onSliderChgDone();
        }

        #endregion


        #region 外部调用

        /// <summary>
        /// 设置滑动条数据
        /// </summary>
        /// <param name="_initValue">初始值</param>
        /// <param name="_minValue">最小值</param>
        /// <param name="_maxValue">最大值</param>
        /// <param name="_unitValue">最小单位</param>
        public void setSliderData(int _initValue, int _minValue, int _maxValue, int _unitValue = 1)
        {
            if (wnd == null || wnd.sldNumSelect == null)
                return;

            if (_unitValue <= 0)
                _unitValue = 1;

            _m_iUnitValue = _unitValue;
            wnd.sldNumSelect.maxValue = _maxValue;
            //最大值是跟当前选择值都是1的时候特殊处理，slider最小值设置为0，要进度条要展示百分百， 因为unity slider底层都是1进度条会百分之0
            if (_maxValue == _minValue && _maxValue == 1)
            {
                wnd.sldNumSelect.minValue = 0;
            }
            else
            {
                wnd.sldNumSelect.minValue = _minValue;
            }
            wnd.sldNumSelect.wholeNumbers = true;
            wnd.sldNumSelect.value = _initValue;

            //重新设置后，默认不开启限制
            _m_lMinLimit = (long)wnd.sldNumSelect.minValue;
            _m_lMaxLimit = (long)wnd.sldNumSelect.maxValue;

            //设置结束调用一次变更和变更结束事件
            _onSliderChg(_initValue);
            _onSliderChgDone();
        }

        /// <summary>
        /// 直接设置当前值
        /// </summary>
        /// <param name="_curValue"></param>
        public void setCurValue(long _curValue)
        {
            wnd.sldNumSelect.value = _curValue;
            _onSliderChg(_curValue);
            _onSliderChgDone();
        }

        #endregion


        #region slider事件

        /// <summary>
        /// slider数值变更
        /// </summary>
        /// <param name="_newValue"></param>
        private void _onSliderChg(float _newValue)
        {
            if (wnd == null || wnd.sldNumSelect == null)
                return;

            long newValue = (long)_newValue;
            if (newValue < 0)
            {
                newValue = 0;
            }

            // 如果最大数量足够匹配有一个单位，则限制选中的数目在一个单位内
            if (wnd.sldNumSelect.maxValue >= _m_iUnitValue)
            {
                long intNewValue = newValue;
                long unitValue = intNewValue % _m_iUnitValue;
                if (unitValue != 0)
                {
                    long calValue = (long)Mathf.Round((float)intNewValue / _m_iUnitValue) * _m_iUnitValue;

                    if (calValue <= 0)
                        calValue += _m_iUnitValue;

                    if (calValue > wnd.sldNumSelect.maxValue)
                        calValue -= _m_iUnitValue;
                    else if (calValue < wnd.sldNumSelect.minValue)
                        calValue += _m_iUnitValue;

                    newValue = calValue;
                }
            }

            newValue = newValue > _m_lMaxLimit ? _m_lMaxLimit : newValue;
            newValue = newValue < _m_lMinLimit ? _m_lMinLimit : newValue;

            //设置滑动条数值
            wnd.sldNumSelect.value = newValue;

            ALUGUICommon.setLabelTxt(wnd.txtSelectNum, wnd.sldNumSelect.value + "/" + wnd.sldNumSelect.maxValue);
            
            //如果数值未变，不进行操作
            if (_m_lLastValue == newValue)
                return;

            if (onNumValueChg != null)
                onNumValueChg((long)wnd.sldNumSelect.value); // 因为wholeNumbers为true，所以直接强转就好了。

            _m_lLastValue = newValue;
        }

        /// <summary>
        /// slider数值变更结束
        /// </summary>
        /// <param name="_value"></param>
        private void _onSliderChgDone(long _value)
        {
            onNumValueChgDone?.Invoke(_value);
        }

        /// <summary>
        /// slider数值变更结束（直接传入当前slider数值作参数）
        /// </summary>
        private void _onSliderChgDone()
        {
            _onSliderChgDone(curValue);
        }

        #endregion


    }
}
