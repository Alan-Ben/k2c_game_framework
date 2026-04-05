using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 以long为值的通用进度条
    /// </summary>
    public class GGUIWndLongProgress : _ANPGGUIBasicSubWnd<NPGGUIMonoProgress>
    {
        private long _m_lSldLeftValue;//进度条左边值
        private long _m_lSldRightValue;//进度条右边
        private string _m_sRightValueStr;//进度条右边值文本
        private long _m_lSldNow;//进度条当前值
        private bool _m_bCanSetNowValue;//是否可以设置当前值(若初始化给的数据有问题(如进度条左右值一样时, 会导致除数为0出问题, 所以需要手动将左右值变成正常的, 但是进度条当前值不可通过代码设置))
        private Func<string, string, string> _m_fSetSldValueTxt;//进度值描述
        private EValueFormatType _m_eValueFormatType;//值格式化类型
        
        private long _m_lChgTaskSerialize;//变化任务序列号

        private bool sldIsSmallToLarge { get { return _m_lSldLeftValue < _m_lSldRightValue; } }
        public long nowValue { get { return _m_lSldNow; } }

        public long leftValue => _m_lSldLeftValue;
        public long rightValue => _m_lSldRightValue;

        //构造函数
        public GGUIWndLongProgress(NPGGUIMonoProgress _wnd) : base(_wnd)
        {
            initWnd();
        }
        
        /// <summary>
        /// 进度条值初始化，若想实现有一个进度条要求是值从100->0, 进度条由不满变满的情况，将min设置为100，max设置为0就行
        /// </summary>
        /// <param name="_sldLeftValue">进度条左边值</param>
        /// <param name="_sldRightValue">进度条右边值</param>
        /// <param name="_valueDesc">当前值描述</param>
        /// <param name="_isShowLarge">是否显示大数</param>
        /// <param name="_whenLeftRightSameSldIsZero">当进度条左右值相同时, 进度条值是否为0, 否则为1</param>
        public void initSld(long _sldLeftValue, long _sldRightValue, Func<string, string, string> _setSldValueTxt = null, EValueFormatType _valueFormatType = EValueFormatType.NORMAL_NOT_LARGE_STR, bool _whenLeftRightSameSldIsZero = false)
        {
            _m_lSldLeftValue = _m_lSldNow = _sldLeftValue;
            _m_lSldRightValue = _sldRightValue;

            _m_bCanSetNowValue = true;
            _m_fSetSldValueTxt = _setSldValueTxt;
            _m_eValueFormatType = _valueFormatType;

            if (_m_lSldLeftValue == _m_lSldRightValue)
            {
                _m_bCanSetNowValue = false;//不可设值
                
                _m_lSldLeftValue = 0;
                _m_lSldRightValue = 1;
                if (_whenLeftRightSameSldIsZero)
                    _m_lSldNow = 0;
                else
                    _m_lSldNow = 1;
            }

            _m_sRightValueStr = GCommon.getValueFormatStr(_m_eValueFormatType, _m_lSldRightValue);
            
            if (wnd != null && wnd.progressSlider != null)
            {
                if (sldIsSmallToLarge)//若从小到大
                {
                    wnd.progressSlider.minValue = _m_lSldLeftValue;
                    wnd.progressSlider.maxValue = _m_lSldRightValue;
                }
                else
                {
                    wnd.progressSlider.minValue = _m_lSldRightValue;
                    wnd.progressSlider.maxValue = _m_lSldLeftValue;
                }
            }
        }

        /// <summary>
        /// 设置进度条当前值，有变化过程
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="_chgTime"></param>
        /// <param name="???"></param>
        public void setNowValueChg(long _value, float _chgTime, Action _chgDone)
        {
            if (!_m_bCanSetNowValue || wnd ==null || wnd.progressSlider == null)
            {
                _chgDone?.Invoke();
                return;
            }
            
            if (_chgTime <= 0)
            {
                setNowValue(_value);
                _chgDone?.Invoke();
                return;
            }

            if (sldIsSmallToLarge)//若是从小到大
            {
                _value = Math.Clamp(_value, _m_lSldLeftValue, _m_lSldRightValue);
            }
            else
            {
                _value = Math.Clamp(_value, _m_lSldRightValue, _m_lSldLeftValue);
            }
            
            long oldValue = Mathf.RoundToInt(wnd.progressSlider.value);
            if (!sldIsSmallToLarge)
                oldValue = _m_lSldLeftValue - _m_lSldNow + _m_lSldRightValue;
            
            long serialize = _m_lChgTaskSerialize = ALSerializeOpMgr.next();
            NPMonoTaskLerpStartEndValueByTime.startLerpTask(() => { return wnd == null || !isShow; }, serialize, oldValue, _value, _chgTime,
                (_nowValue) =>
                {
                    if (wnd == null || _m_lChgTaskSerialize != serialize || wnd.progressSlider == null)
                        return;

                    _m_lSldNow = _nowValue;
                    string nowValueStr = GCommon.getValueFormatStr(_m_eValueFormatType, _nowValue);

                    if (sldIsSmallToLarge)
                        wnd.progressSlider.value = _nowValue;
                    else
                        wnd.progressSlider.value = _m_lSldLeftValue - _nowValue + _m_lSldRightValue;
                    
                    if(_m_fSetSldValueTxt == null)
                        ALUGUICommon.setLabelTxt(wnd.progressTxt, nowValueStr);
                    else
                        ALUGUICommon.setLabelTxt(wnd.progressTxt, _m_fSetSldValueTxt(nowValueStr, _m_sRightValueStr));

                    _refreshColor(_nowValue);
                    _refreshFullShow(_nowValue);
                }, 
                ()=> _m_lChgTaskSerialize, 
                () =>
                {
                    if (wnd == null || _m_lChgTaskSerialize != serialize || wnd.progressSlider == null)
                        return;

                    _m_lSldNow = _value;
                    string nowValueStr = GCommon.getValueFormatStr(_m_eValueFormatType, _m_lSldNow);

                    if (sldIsSmallToLarge)
                        wnd.progressSlider.value = _value;
                    else
                        wnd.progressSlider.value = _m_lSldLeftValue - _value + _m_lSldRightValue;
                    
                    if(_m_fSetSldValueTxt == null)
                        ALUGUICommon.setLabelTxt(wnd.progressTxt, nowValueStr);
                    else
                        ALUGUICommon.setLabelTxt(wnd.progressTxt, _m_fSetSldValueTxt(nowValueStr, _m_sRightValueStr));
                    
                    _refreshColor(_value);
                    _refreshFullShow(_value);
                    
                    _chgDone?.Invoke();
                });
        }

        /// <summary>
        /// 设置进度条当前值(没有变化过程)
        /// </summary>
        /// <param name="_nowValue">进度条当前值</param>
        public void setNowValue(long _nowValue)
        {
            if (!_m_bCanSetNowValue || wnd ==null || wnd.progressSlider == null)
                return;

            _m_lChgTaskSerialize = ALSerializeOpMgr.next();

            _m_lSldNow = _nowValue;
            if (sldIsSmallToLarge)//若是从小到大
            {
                _m_lSldNow = Math.Clamp(_m_lSldNow, _m_lSldLeftValue, _m_lSldRightValue);

                if(wnd.progressSlider != null)
                    wnd.progressSlider.value = _nowValue;
            }
            else
            {
                _m_lSldNow = Math.Clamp(_m_lSldNow, _m_lSldRightValue, _m_lSldLeftValue);

                if(wnd.progressSlider != null)
                    wnd.progressSlider.value = _m_lSldLeftValue - _m_lSldNow + _m_lSldRightValue;
            }

            string _nowValueStr = GCommon.getValueFormatStr(_m_eValueFormatType, _nowValue);
            if(_m_fSetSldValueTxt == null)
                ALUGUICommon.setLabelTxt(wnd.progressTxt, _nowValueStr);
            else
                ALUGUICommon.setLabelTxt(wnd.progressTxt, _m_fSetSldValueTxt(_nowValueStr, _m_sRightValueStr));
            
            _refreshColor(_nowValue);
            _refreshFullShow(_nowValue);
        }

        private void _refreshFullShow(long _nowValue)
        {
            if (wnd == null)
                return;

            bool isComplete = false;
            if (sldIsSmallToLarge)
                isComplete = _nowValue >= _m_lSldRightValue;
            else
                isComplete = _nowValue <= _m_lSldRightValue;

            ALUGUICommon.setGameObjEnable(wnd.goFullHideList, !isComplete);
            ALUGUICommon.setGameObjEnable(wnd.goFullShowList, isComplete);
        }

        private void _refreshColor(long _nowValue)
        {
            if (wnd == null)
                return;
            if (!wnd.useTextColor)
                return;

            bool isComplete = false;
            if (sldIsSmallToLarge)
                isComplete = _nowValue >= _m_lSldRightValue;
            else
                isComplete = _nowValue <= _m_lSldRightValue;
            
            Color color = isComplete ? wnd.colorComplete : wnd.colorUnComplete;
            ALUGUICommon.setUIObjColor(wnd.progressTxt, color);
        }
        

        #region override
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.progressSlider == null)
            {
                Debug.LogError("[GGUIWndLongProgress] 窗口配置的wnd.progressSlider == null");
                return;
            }
            
            wnd.progressSlider.interactable = false;//进度条不可交互
            wnd.progressSlider.wholeNumbers = true;
        }
        protected override void _onShowWnd()
        {
        }
        protected override void _onHideWnd()
        {
            _m_lChgTaskSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onReset()
        {
            _m_lChgTaskSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onDiscard()
        {
            _m_lChgTaskSerialize = ALSerializeOpMgr.next();
        }
        #endregion
    }
}
