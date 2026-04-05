using ALPackage;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    // 物品计数器
    public class GGUIWndBagPopCounter : _ANPGGUIBasicSubWnd<GGUIMonoBagPopCounter>
    {
        private Action<long> _m_dCountChangedEvent = null;
        private long _m_iSellCount = 0;
        private long _m_iTotalCount = 0;
        private long _m_perCount = 10;

        private string _m_txtKey = null;

        //最大使用限制 999 
        private long _m_maxCount = 999;

        public GGUIWndBagPopCounter(GGUIMonoBagPopCounter _wnd) : base(_wnd)
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

            //监听滑块滑动事件
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
            //监听滑块滑动事件
            if(null != wnd.selectCountSlider)
            {
                wnd.selectCountSlider.onValueChanged.AddListener( _onSliderValueChg);
            }
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
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
            
            if(wnd.numInputField != null)
                wnd.numInputField.onEndEdit.AddListener(_onInputFieldValueChanged);
        }

        // 初始化
        public void init(BagItem _item,long _useMaxCount = -1)
        {
            _m_iSellCount = 1;
            if (_useMaxCount != -1)
                _m_maxCount = _useMaxCount;

            _getCostItemCount(_item);

            _setShowInfo(_m_iSellCount);
        }

        //获取最大使用消耗的个数
        private void _getCostItemCount(BagItem _item)
        {
            long totalCount = _item.count;

            if (_item.itemUseRefObj == null)
            {
                _m_iTotalCount = totalCount;
                return;
            }
            
            List<NPCommonCostItem> costItemList = _item.itemUseRefObj.cost_item_list;
            NPCommonCostItem temp = null;
            for (int i = 0; i < costItemList.Count; i++)
            {
                temp = costItemList[i];
                if (null == temp)
                    continue;

                long tempCount = GCommon.getCostItemCount(temp);
                if (tempCount < totalCount)
                    totalCount = tempCount;
            }

            _m_iTotalCount = totalCount;
            if (_m_iTotalCount > _m_maxCount)
                _m_iTotalCount = _m_maxCount;

        }

        /// <summary>
        ///带个数的初始化 
        /// </summary>
        /// <param name="_totalCount"></param>
        /// <param name="txtKey">中间显示文本使用的key</param>
        public void init(long _totalCount,string _txtKey = null, long _maxCount = -1)
        {
            _m_iSellCount = 1;
            _m_iTotalCount = _totalCount;
            if (_maxCount != -1)
                _m_maxCount = _maxCount;
            if (_m_iTotalCount > _m_maxCount)
                _m_iTotalCount = _m_maxCount;

            _m_txtKey = _txtKey;
            _setShowInfo(_m_iSellCount);
        }
        
        public void init(long _totalCount, long _initUseCount, string _txtKey = null, long _maxCount = -1)
        {
            _m_iTotalCount = _totalCount;
            if (_maxCount != -1)
                _m_maxCount = _maxCount;
            if (_m_iTotalCount > _m_maxCount)
                _m_iTotalCount = _m_maxCount;

            if(_m_iTotalCount > 1)//钳制在1到最大数量之间
                _m_iSellCount = Math.Clamp(_initUseCount, 1, _m_iTotalCount);
            else
                _m_iSellCount = 1;
            
            _m_txtKey = _txtKey;
            
            _onSellCountChanged(_m_iSellCount);
        }

        // 响应减少按钮点击事件
        private void _onDecreaseBtnClick(GameObject _btn)
        {
            if (_m_iSellCount <= 1)
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
            if (_m_iSellCount <= 1)
                return;
            if (_m_iSellCount - _m_perCount <= 1)
                _m_iSellCount = 1;
            else
                _m_iSellCount = _m_iSellCount - _m_perCount;
            _onSellCountChanged(_m_iSellCount);

            ALUGUICommon.setGameObjEnable(wnd.minBtn, true);
        }

        // 响应最大数量按钮点击事件
        private void _onMaxBtnClick(GameObject _btn)
        {
            if (_m_iSellCount == _m_iTotalCount)
                return;

            _m_iSellCount = _m_iTotalCount;
            if (_m_iSellCount == 0)
                _m_iSellCount = 1;
            _onSellCountChanged(_m_iSellCount);
        }

        // 响应最小数量按钮点击事件
        private void _onMinBtnClick(GameObject _btn)
        {
            if (_m_iSellCount == 1)
                return;

            _m_iSellCount = 1;
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

        // 响应计数器更改事件
        private void _onSellCountChanged(long _newCount)
        {
            _setShowInfo(_newCount);
            if (_m_dCountChangedEvent != null)
                _m_dCountChangedEvent(_newCount);
        }

        /// <summary>
        /// 设置使用数量
        /// </summary>
        /// <param name="_count"></param>
        public void setUseCount(long _count)
        {
            if(_m_iTotalCount > 1)//钳制在1到最大数量之间
                _m_iSellCount = Math.Clamp(_count, 1, _m_iTotalCount);
            else
                _m_iSellCount = 1;
            _onSellCountChanged(_m_iSellCount);
        }
        
        //设置显示数据
        private void _setShowInfo(long _count)
        {
            if(wnd == null)
                return;
            
            ALUGUICommon.setLabelTxt(wnd.curCountTxt, "x" + _count.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));

            string txtStr = null;
            if(null != _m_txtKey)
                txtStr = TextTranslate.instance.getLanguage(_m_txtKey, _count.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT), _m_iTotalCount.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
            else
                txtStr = TextTranslate.instance.getLanguage(TransKeyConst.bag_convert_item_num, _count.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT), _m_iTotalCount.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));

            ALUGUICommon.setLabelTxt(wnd.selectNumInfo, txtStr);
            ALUGUICommon.setLabelTxt(wnd.countInfo, TextTranslate.instance.getLanguage(TransKeyConst.bag_itemUseCount_num_num, _count.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT), _m_iTotalCount.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));

            GGameCommonInfo.grayImage(wnd.maxGrayImgList, _count >= _m_iTotalCount);
            GGameCommonInfo.grayImage(wnd.minGrayImgList, _count <= 1);

            if (null != wnd.selectCountSlider)
                wnd.selectCountSlider.value = (float)(_count * 1.0 / _m_iTotalCount);
            
            ALUGUICommon.setInputTxt(wnd.numInputField, _count.ToString());
        }

        private void _onSliderValueChg(float _value)
        {
            if (wnd == null || wnd.selectCountSlider == null)
                return;

            //取整
            int curCount = (int)Math.Round(_m_iTotalCount * _value);
            if (curCount < 1)
                curCount = 1;

            _m_iSellCount = curCount;
            _onSellCountChanged(_m_iSellCount);
        }
        
        /// <summary>
        /// 当输入框内容变化
        /// </summary>
        /// <param name="_text"></param>
        private void _onInputFieldValueChanged(string _text)
        {
            string curText = _text;
            if(long.TryParse(curText, out long inputCount))
            {
                // 输入数量钳制在 1 到 最大数量 之间
                if(_m_iTotalCount > 1)
                    _m_iSellCount = Math.Clamp(inputCount, 1, _m_iTotalCount);
                else
                    _m_iSellCount = 1;
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
