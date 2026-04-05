using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;
using NPEnum;

namespace GOE
{
    // 通用进度条
    public class NPGGUIWndProgress : _ANPGGUIBasicSubWnd<NPGGUIMonoProgress>
    {
        private long _m_lChgTaskSerialize;//变化任务序列号
        //构造函数
        public NPGGUIWndProgress(NPGGUIMonoProgress _wnd)
           : base(_wnd)
        {
            initWnd();
        }
        
        public void setProgress(float _curPro)
        {
            if(wnd == null)
                return;

            _m_lChgTaskSerialize = ALSerializeOpMgr.next();
            
            if (_curPro > 1)
                _curPro = 1;
            if (_curPro < 0)
                _curPro = 0;

            //设置进度条
            if (null != wnd.progressSlider)
                wnd.progressSlider.value = _curPro;
        }

        //设置数据
        public void setProgress(long _currentPro, long _allPro, EValueFormatType _valueFormatType, string _keyStr = null)
        {
            if(wnd == null)
                return;
            
            _m_lChgTaskSerialize = ALSerializeOpMgr.next();
            
            if (_currentPro < 0)
                _currentPro = 0;

            if (_allPro <= 0)
                _allPro = _currentPro;

            string curProStr = _currentPro.ToString();
            string allProStr = _allPro.ToString();
            
            curProStr = GCommon.getValueFormatStr(_valueFormatType, _currentPro);
            allProStr = GCommon.getValueFormatStr(_valueFormatType, _allPro);

            if (null != _keyStr)
                ALUGUICommon.setLabelTxt(wnd.progressTxt, TextTranslate.instance.getLanguage(_keyStr, curProStr, allProStr));
            else
                ALUGUICommon.setLabelTxt(wnd.progressTxt, TextTranslate.instance.getLanguage(TransKeyConst.common_currentTotalNum_num_num, curProStr, allProStr));

            //设置进度条
            if (null != wnd.progressSlider)
                wnd.progressSlider.value = (float)(_currentPro * 1.0 / _allPro);
            
            bool isComplete = _currentPro >= _allPro;
            _refreshColor(isComplete);
            _refreshFullShow(isComplete);
        }
        
        /// <summary>
        /// 带有变化过程的进度条设值
        /// </summary>
        /// <param name="_currentPro"></param>
        /// <param name="_allPro"></param>
        /// <param name="_chgTime"></param>
        /// <param name="_setSldValueTxt">参数1:进度条当前值, 参数2:进度条最大值, 返回:要设置的str</param>
        /// <param name="_chgDone"></param>
        public void setProgressChg(long _currentPro, long _allPro, float _chgTime, EValueFormatType _valueFormatType, Func<string, string, string> _setSldValueTxt = null, Action _chgDone = null)
        {
            if(wnd == null)
                return;

            string _currentProString = GCommon.getValueFormatStr(_valueFormatType, _currentPro);
            string _allProString = GCommon.getValueFormatStr(_valueFormatType, _allPro);
            
            if (_chgTime <= 0 || wnd.progressSlider == null)
            {
                setProgress(_currentPro, _allPro, _valueFormatType);
                if(_setSldValueTxt != null)
                    setProgressTxt(_setSldValueTxt(_currentProString, _allProString), _currentPro >= _allPro);
                _chgDone?.Invoke();
                return;
            }
            
            if (_currentPro < 0)
                _currentPro = 0;

            if (_allPro <= 0)
                _allPro = _currentPro;

            float oldSldValue = wnd.progressSlider.value;
            float newSldValue = (float)(_currentPro * 1.0 / _allPro);

            long serialize = _m_lChgTaskSerialize = ALSerializeOpMgr.next();
            NPMonoTaskLerpFloatStartEndValueByTime.startLerpTask(() => { return wnd == null || !isShow; }, serialize, oldSldValue, newSldValue, _chgTime,
                (_sldValue) =>
                {
                    if (wnd == null || _m_lChgTaskSerialize != serialize)
                        return;

                    long nowValue = (long) (_sldValue * _allPro);
                    string nowValueStr = GCommon.getValueFormatStr(_valueFormatType, nowValue);
                    
                    ALUGUICommon.setSliderScale(wnd.progressSlider, _sldValue);
                    ALUGUICommon.setLabelTxt(wnd.progressTxt, _setSldValueTxt == null ? 
                        TextTranslate.instance.getLanguage(TransKeyConst.common_currentTotalNum_num_num, nowValueStr, _allProString) : _setSldValueTxt(nowValueStr, _allProString));

                    bool isComplete = nowValue >= _allPro;
                    _refreshColor(isComplete);
                    _refreshFullShow(isComplete);
                }, 
                ()=> _m_lChgTaskSerialize, 
                () =>
                {
                    if (wnd == null || _m_lChgTaskSerialize != serialize)
                        return;
                    
                    ALUGUICommon.setSliderScale(wnd.progressSlider, newSldValue);
                    ALUGUICommon.setLabelTxt(wnd.progressTxt, _setSldValueTxt == null ? 
                        TextTranslate.instance.getLanguage(TransKeyConst.common_currentTotalNum_num_num, _currentProString, _allProString) : _setSldValueTxt(_currentProString, _allProString));

                    bool isComplete = _currentPro >= _allPro;
                    _refreshColor(isComplete);
                    _refreshFullShow(isComplete);
                    
                    _chgDone?.Invoke();
                });
        }

        /// <summary>
        /// 带有变化过程的进度条设值
        /// </summary>
        /// <param name="_currentPro"></param>
        /// <param name="_allPro"></param>
        /// <param name="_chgTime"></param>
        /// <param name="_setSldValueTxt">参数1:进度条当前值, 参数2:进度条最大值, 返回:要设置的str</param>
        /// <param name="_chgDone"></param>
        public void setProgressChg(long _currentPro, long _allPro, float _chgTime, Func<long, long, string> _setSldValueTxt, Action _chgDone = null)
        {
            if(wnd == null)
                return;

            if (_setSldValueTxt == null)
            {
                Debug.LogError_EditorOnly("调用这个方法时必须传入_setSldValueTxt参数, 否则进度条数值不会更新, 若想要进度条值同步更新, 可以调用另一个setProgressChg方法");
            }
            
            if (_chgTime <= 0 || wnd.progressSlider == null)
            {
                if(_setSldValueTxt != null)
                    setProgressTxt(_setSldValueTxt(_currentPro, _allPro), _currentPro >= _allPro);
                _chgDone?.Invoke();
                return;
            }
            
            if (_currentPro < 0)
                _currentPro = 0;

            if (_allPro <= 0)
                _allPro = _currentPro;

            float oldSldValue = wnd.progressSlider.value;
            float newSldValue = (float)(_currentPro * 1.0 / _allPro);

            long serialize = _m_lChgTaskSerialize = ALSerializeOpMgr.next();
            NPMonoTaskLerpFloatStartEndValueByTime.startLerpTask(() => { return wnd == null || !isShow; }, serialize, oldSldValue, newSldValue, _chgTime,
                (_sldValue) =>
                {
                    if (wnd == null || _m_lChgTaskSerialize != serialize)
                        return;

                    long nowValue = (long) (_sldValue * _allPro);
                    
                    ALUGUICommon.setSliderScale(wnd.progressSlider, _sldValue);
                    if(_setSldValueTxt != null)
                        ALUGUICommon.setLabelTxt(wnd.progressTxt, _setSldValueTxt(nowValue, _allPro));
                    
                    bool isComplete = nowValue >= _allPro;
                    _refreshColor(isComplete);
                    _refreshFullShow(isComplete);
                }, 
                ()=> _m_lChgTaskSerialize, 
                () =>
                {
                    if (wnd == null || _m_lChgTaskSerialize != serialize)
                        return;
                    
                    ALUGUICommon.setSliderScale(wnd.progressSlider, newSldValue);
                    if(_setSldValueTxt != null)
                        ALUGUICommon.setLabelTxt(wnd.progressTxt, _setSldValueTxt(_currentPro, _allPro));

                    bool isComplete = _currentPro >= _allPro;
                    _refreshColor(isComplete);
                    _refreshFullShow(isComplete);
                    
                    _chgDone?.Invoke();
                });
        }
        
        //设置进度文本
        public void setProgressTxt(string _txt, bool _isComplete)
        {
            if(wnd == null)
                return;
            ALUGUICommon.setLabelTxt(wnd.progressTxt, _txt);
            _refreshColor(_isComplete);
            _refreshFullShow(_isComplete);
        }


        private void _refreshFullShow(bool _isComplete)
        {
            if (wnd == null)
                return;

            ALUGUICommon.setGameObjEnable(wnd.goFullHideList, !_isComplete);
            ALUGUICommon.setGameObjEnable(wnd.goFullShowList, _isComplete);
        }

        private void _refreshColor(bool _isComplete)
        {
            if (wnd == null)
                return;
            if (!wnd.useTextColor)
                return;
            
            Color color = _isComplete ? wnd.colorComplete : wnd.colorUnComplete;
            ALUGUICommon.setUIObjColor(wnd.progressTxt, color);
        }

        #region override
        protected override void _onWndInitDone()
        {
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
