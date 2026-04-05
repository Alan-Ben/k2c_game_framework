using ALPackage;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    // 物品计数器（支持自定义最小值，默认最小值为 1）
    public class GGUIWndBagPopCounterEx : _ANPGGUIBasicSubWnd<GGUIMonoBagPopCounter>
    {
        private Action<long> _m_dCountChangedEvent = null;
        private long _m_iSellCount = 0;
        private long _m_iTotalCount = 0;
        private long _m_perCount = 10;

        private string _m_txtKey = null;

        //最大使用限制 999
        private long _m_maxCount = 999;

        //可选最小值，默认为 1
        private long _m_iMinCount = 1;

        public GGUIWndBagPopCounterEx(GGUIMonoBagPopCounter _wnd) : base(_wnd)
        {
            initWnd();
        }

        //获取当前个数
        public long currentCount { get { return _m_iSellCount; } }
        public long totalCount { get { return _m_iTotalCount; } }

        protected override void _onDiscard()
        {
            _m_dCountChangedEvent = null;
            _m_iSellCount = 0;
            _m_iTotalCount = 0;
            _m_iMinCount = 1;

            ALUGUICommon.uncombineBtnClick(wnd.decreaseBtn, _onDecreaseBtnClick);
            ALUGUICommon.uncombineBtnClick(wnd.increaseBtn, _onIncreaseBtnClick);
            ALUGUICommon.uncombineBtnClick(wnd.addLotBtn, _onAddLotBtnClick);
            ALUGUICommon.uncombineBtnClick(wnd.reduceLotBtn, _onReduceLotBtnClick);

            ALUGUICommon.uncombineBtnClick(wnd.minBtn, _onMinBtnClick);
            ALUGUICommon.uncombineBtnClick(wnd.maxBtn, _onMaxBtnClick);

            if (wnd != null)
            {
                if (wnd.numInputField != null)
                {
                    wnd.numInputField.onEndEdit.RemoveListener(_onInputFieldValueChanged);
                }
            }
        }

        protected override void _onHideWnd()
        {
            if (null == wnd.selectCountSlider)
                return;

            if (null != wnd.selectCountSlider)
            {
                wnd.selectCountSlider.onValueChanged.RemoveListener(_onSliderValueChg);
            }
        }

        protected override void _onReset()
        {
        }

        protected override void _onShowWnd()
        {
            if (null != wnd.selectCountSlider)
            {
                wnd.selectCountSlider.onValueChanged.AddListener(_onSliderValueChg);
            }
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (null != GRefdataCoreMgr.instance.npGeneral && GRefdataCoreMgr.instance.npGeneral.common_use_add_num > 0)
                _m_perCount = GRefdataCoreMgr.instance.npGeneral.common_use_add_num;

            if (null != GRefdataCoreMgr.instance.npGeneral && GRefdataCoreMgr.instance.npGeneral.batch_use_item_max_count > 0)
                _m_maxCount = GRefdataCoreMgr.instance.npGeneral.batch_use_item_max_count;

            ALUGUICommon.combineBtnClick(wnd.decreaseBtn, _onDecreaseBtnClick);
            ALUGUICommon.combineBtnClick(wnd.increaseBtn, _onIncreaseBtnClick);
            ALUGUICommon.combineBtnClick(wnd.addLotBtn, _onAddLotBtnClick);
            ALUGUICommon.combineBtnClick(wnd.reduceLotBtn, _onReduceLotBtnClick);

            ALUGUICommon.combineBtnClick(wnd.minBtn, _onMinBtnClick);
            ALUGUICommon.combineBtnClick(wnd.maxBtn, _onMaxBtnClick);

            if (wnd.numInputField != null)
                wnd.numInputField.onEndEdit.AddListener(_onInputFieldValueChanged);
        }

        // 初始化（指定最小值）
        public void init(long _totalCount, long _initUseCount, long _minCount = 1, string _txtKey = null, long _maxCount = -1)
        {
            _m_iMinCount = _minCount > 0 ? _minCount : 1;
            _m_iTotalCount = _totalCount;
            if (_maxCount != -1)
                _m_maxCount = _maxCount;
            if (_m_iTotalCount > _m_maxCount)
                _m_iTotalCount = _m_maxCount;

            _m_iSellCount = _m_iTotalCount > _m_iMinCount
                ? Math.Clamp(_initUseCount, _m_iMinCount, _m_iTotalCount)
                : _m_iMinCount;

            _m_txtKey = _txtKey;

            if (wnd != null && wnd.selectCountSlider != null)
            {
                if (_m_iTotalCount <= _m_iMinCount)//进度条最大值 < 进度条最小值时, 禁用进度条
                    wnd.selectCountSlider.interactable = false;    
                else
                    wnd.selectCountSlider.interactable = true;
            }
            
            _onSellCountChanged(_m_iSellCount);
        }

        // 注册计数器更改事件
        public void regCounterChangedEvent(Action<long> _event)
        {
            if (_event == null)
                return;

            if (_m_dCountChangedEvent == null)
                _m_dCountChangedEvent = _event;
            else
                _m_dCountChangedEvent += _event;
        }

        /// <summary>
        /// 设置使用数量
        /// </summary>
        public void setUseCount(long _count)
        {
            _m_iSellCount = _m_iTotalCount > _m_iMinCount
                ? Math.Clamp(_count, _m_iMinCount, _m_iTotalCount)
                : _m_iMinCount;
            _onSellCountChanged(_m_iSellCount);
        }

        // 响应减少按钮点击事件
        private void _onDecreaseBtnClick(GameObject _btn)
        {
            if (_m_iSellCount <= _m_iMinCount)
                return;

            _m_iSellCount--;
            _onSellCountChanged(_m_iSellCount);

            ALUGUICommon.setGameObjEnable(wnd.minBtn, true);
        }

        // 响应增加按钮点击事件
        private void _onIncreaseBtnClick(GameObject _btn)
        {
            if (_m_iSellCount >= _m_iTotalCount)
                return;

            _m_iSellCount++;
            _onSellCountChanged(_m_iSellCount);

            ALUGUICommon.setGameObjEnable(wnd.maxBtn, true);
        }

        // 响应单次增加多个按钮点击事件
        private void _onAddLotBtnClick(GameObject _btn)
        {
            if (_m_iSellCount >= _m_iTotalCount)
                return;
            if (_m_iSellCount + _m_perCount >= _m_iTotalCount)
                _m_iSellCount = _m_iTotalCount;
            else
                _m_iSellCount = _m_iSellCount + _m_perCount;

            _onSellCountChanged(_m_iSellCount);

            ALUGUICommon.setGameObjEnable(wnd.maxBtn, true);
        }

        // 响应单次减少多个按钮点击事件
        private void _onReduceLotBtnClick(GameObject _btn)
        {
            if (_m_iSellCount <= _m_iMinCount)
                return;
            if (_m_iSellCount - _m_perCount <= _m_iMinCount)
                _m_iSellCount = _m_iMinCount;
            else
                _m_iSellCount = _m_iSellCount - _m_perCount;
            _onSellCountChanged(_m_iSellCount);

            ALUGUICommon.setGameObjEnable(wnd.minBtn, true);
        }

        // 响应最大数量按钮点击事件
        private void _onMaxBtnClick(GameObject _btn)
        {
            if (_m_iSellCount >= _m_iTotalCount)
                return;

            _m_iSellCount = _m_iTotalCount;
            if (_m_iSellCount < _m_iMinCount)
                _m_iSellCount = _m_iMinCount;
            _onSellCountChanged(_m_iSellCount);
        }

        // 响应最小数量按钮点击事件
        private void _onMinBtnClick(GameObject _btn)
        {
            if (_m_iSellCount == _m_iMinCount)
                return;

            _m_iSellCount = _m_iMinCount;
            _onSellCountChanged(_m_iSellCount);
        }

        // 响应计数器更改事件
        private void _onSellCountChanged(long _newCount)
        {
            _setShowInfo(_newCount);
            _m_dCountChangedEvent?.Invoke(_newCount);
        }

        // 设置显示数据
        private void _setShowInfo(long _count)
        {
            if (wnd == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.curCountTxt, "x" + _count.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));

            string txtStr = null != _m_txtKey
                ? TextTranslate.instance.getLanguage(_m_txtKey, _count.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT), _m_iTotalCount.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT))
                : TextTranslate.instance.getLanguage(TransKeyConst.bag_convert_item_num, _count.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT), _m_iTotalCount.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));

            ALUGUICommon.setLabelTxt(wnd.selectNumInfo, txtStr);
            ALUGUICommon.setLabelTxt(wnd.countInfo, TextTranslate.instance.getLanguage(TransKeyConst.bag_itemUseCount_num_num, _count.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT), _m_iTotalCount.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));

            GGameCommonInfo.grayImage(wnd.maxGrayImgList, _count >= _m_iTotalCount);
            GGameCommonInfo.grayImage(wnd.minGrayImgList, _count <= _m_iMinCount);

            if (null != wnd.selectCountSlider)
            {
                long _sliderRange = _m_iTotalCount - _m_iMinCount;
                wnd.selectCountSlider.value = _sliderRange > 0
                    ? (float)((_count - _m_iMinCount) * 1.0 / _sliderRange)
                    : 1f;
            }

            ALUGUICommon.setInputTxt(wnd.numInputField, _count.ToString());
        }

        private void _onSliderValueChg(float _value)
        {
            if (wnd == null || wnd.selectCountSlider == null)
                return;

            //将滑块 [0,1] 映射到有效区间 [minCount, totalCount]
            long _sliderRange = _m_iTotalCount - _m_iMinCount;
            if(_sliderRange <= 0)//若可拖动范围小于等于0, 不进行处理
                return;
            
            long curCount = _m_iMinCount + (long)Math.Round(_sliderRange * _value);
            if (curCount < _m_iMinCount) curCount = _m_iMinCount;
            if (curCount > _m_iTotalCount) curCount = _m_iTotalCount;

            _m_iSellCount = curCount;
            _onSellCountChanged(_m_iSellCount);
        }

        /// <summary>
        /// 当输入框内容变化
        /// </summary>
        private void _onInputFieldValueChanged(string _text)
        {
            if (long.TryParse(_text, out long inputCount))
            {
                // 输入数量钳制在最小值到最大数量之间
                _m_iSellCount = _m_iTotalCount > _m_iMinCount
                    ? Math.Clamp(inputCount, _m_iMinCount, _m_iTotalCount)
                    : _m_iMinCount;
                _onSellCountChanged(_m_iSellCount);
            }
            else //若输入非法，则输入框恢复之前的值
            {
                if (wnd != null)
                    ALUGUICommon.setInputTxt(wnd.numInputField, _m_iSellCount.ToString());
            }
        }
    }
}
