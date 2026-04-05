using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;


namespace GOE
{
    /// <summary>
    /// 通用的container物品进度条数字选择子窗口
    /// </summary>
    public class NPGGUIWndCommonContainerItemNumSlider : _ATALBasicUISubWnd<NPGGUIMonoCommonContainerItemNumSlider>
    {
        private NPGGUIWndCommonItem _m_wndCommonItem;//物品信息
        private NPGGUIWndNumSlider _m_wndNumSlider;//进度条

        private NPCommonCostItem _m_commonCostItem;//物品信息

        private Action<long> _m_onItemChg;//数量变化回调
        private Action<long> _m_onItemChgDone;//数量变化结束回调
        

        /// <summary> 当前slider值 </summary>
        public long curValue
        {
            get
            {
                if (_m_wndNumSlider == null)
                    return 1;
                else
                    return _m_wndNumSlider.curValue;
            }
        }

        /// <summary> 数量变化回调 </summary>
        public Action<long> OnItemChg { get { return _m_onItemChg; } set { _m_onItemChg = value; } }
        /// <summary> 数量变化结束回调 </summary>
        public Action<long> OnItemChgDone { get { return _m_onItemChgDone; } set { _m_onItemChgDone = value; } }
        /// <summary> 物品信息 </summary>
        public NPCommonCostItem CommonCostItem { get { return _m_commonCostItem; } }


        public NPGGUIWndCommonContainerItemNumSlider(NPGGUIMonoCommonContainerItemNumSlider _wnd) : base(_wnd)
        {
            initWnd();
        }


        protected override void _onReset()
        {
            if(null != _m_wndCommonItem)
                _m_wndCommonItem.resetWnd();

            if(null != _m_wndNumSlider)
                _m_wndNumSlider.resetWnd();
        }

        protected override void _onDiscard()
        {
            if(null != _m_wndCommonItem)
                _m_wndCommonItem.discard();
            _m_wndCommonItem = null;

            if(null != _m_wndNumSlider)
                _m_wndNumSlider.discard();
            _m_wndNumSlider = null;
        }

        protected override void _onHideWnd()
        {

        }

        protected override void _onShowWnd()
        {
            if(null != _m_wndCommonItem)
                _m_wndCommonItem.showWnd();

            if(null != _m_wndNumSlider)
                _m_wndNumSlider.showWnd();
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;

            if(null != wnd.item)
                _m_wndCommonItem = new NPGGUIWndCommonItem(wnd.item);

            if (null != wnd.monoNumSlider)
            {
                _m_wndNumSlider = new NPGGUIWndNumSlider(wnd.monoNumSlider);
                _m_wndNumSlider.onNumValueChg += _onNumSliderValueChg;
                _m_wndNumSlider.onNumValueChgDone += _onNumSliderValueChgDone;
            }
        }


        #region 刷新

        /// <summary>
        /// 刷新
        /// </summary>
        private void _refresh()
        {
            if (null == _m_commonCostItem)
                return;

            //物品信息
            if (null != _m_wndCommonItem)
            {
                _m_wndCommonItem.setItem(_m_commonCostItem.toCommonItemData());
            }

            //进度条信息
            if (null != _m_wndNumSlider)
            {
                _m_wndNumSlider.setSliderData(0, 0, (int)_m_commonCostItem.count);
            }
        }

        #endregion


        #region 数值变化事件

        /// <summary>
        /// 数量变化
        /// </summary>
        /// <param name="_selectNum"></param>
        private void _onNumSliderValueChg(long _selectNum)
        {
            if (null != _m_onItemChg)
                _m_onItemChg(_selectNum);
        }

        /// <summary>
        /// 数量变化结束
        /// </summary>
        /// <param name="_selectNum"></param>
        private void _onNumSliderValueChgDone(long _selectNum)
        {
            if (null != _m_onItemChgDone)
                _m_onItemChgDone(_selectNum);
        }

        #endregion


        #region 外部调用

        /// <summary>
        /// 设置物品内容
        /// </summary>
        /// <param name="_commonCostItem"></param>
        public void setInfo(NPCommonCostItem _commonCostItem)
        {
            if (null == _commonCostItem)
                return;

            _m_commonCostItem = _commonCostItem;

            _refresh();
        }

        /// <summary>
        /// 直接设置进度条
        /// </summary>
        /// <param name="_curValue"></param>
        public void setSliderData(long _curValue)
        {
            //进度条信息
            if (null != _m_wndNumSlider)
            {
                _m_wndNumSlider.setCurValue(_curValue);
            }
        }

        /// <summary>
        /// 设置slider限制
        /// </summary>
        /// <param name="_minValue"></param>
        /// <param name="_maxValue"></param>
        public void setSliderLimit(int _initValue, int _minValue, int _maxValue)
        {
            //进度条信息
            if (null != _m_wndNumSlider)
            {
                _m_wndNumSlider.setSliderData(_initValue, _minValue, _maxValue);
            }
        }

        #endregion
    }
}
