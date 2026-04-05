using ALPackage;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 下拉框Item
    /// </summary>
    public abstract class _ATNPGGUIWndDropDownBoxItem<T> : _ATALBasicUISubWnd<T>  where T : NPGGUIMonoDropDownBoxItem
    {
        public _ATNPGGUIWndDropDownBoxItem(T _wnd)
            : base(_wnd)
        {
            initWnd();
        }
        
        private bool _m_bIsSelected;//是否选中
        private Action<_INPGGUICommonDropDownBoxInstance> _m_aOnClickItem;//点击item的回调
        private _INPGGUICommonDropDownBoxInstance _m_instanceInfo;
        private NPGGuiWndTexture _m_wIconWnd;//图片

        public Action<_INPGGUICommonDropDownBoxInstance> onClickItem { get { return _m_aOnClickItem; } set { _m_aOnClickItem = value; } }
        
        public _INPGGUICommonDropDownBoxInstance InstanceInfo { get { return _m_instanceInfo; } }

        protected override void _onShowWnd()
        {
            if (_m_wIconWnd != null)
                _m_wIconWnd.showWnd();
        }

        protected override void _onHideWnd()
        {
            if (_m_wIconWnd != null)
                _m_wIconWnd.hideWnd();
        }

        protected override void _onReset()
        {
            if (_m_wIconWnd != null)
                _m_wIconWnd.discardTexture();
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            if (_m_wIconWnd != null)
                _m_wIconWnd.discard();
            _m_wIconWnd = null;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnSelect, _onClickSelectDate);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgIcon != null)
                _m_wIconWnd = new NPGGuiWndTexture(wnd.imgIcon);
            
            ALUGUICommon.combineBtnClick(wnd.btnSelect, _onClickSelectDate);
        }

        /// <summary>
        /// 设置item数据
        /// </summary>
        public void setItemInfo(_INPGGUICommonDropDownBoxInstance _instanceInfo)
        {
            _m_instanceInfo = _instanceInfo;
            
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if (wnd == null)
                return;

            if (null != _m_instanceInfo)
            {
                ALUGUICommon.setLabelTxt(wnd.txtContent, TextTranslate.instance.getLanguage(_m_instanceInfo.content));
                if (null != _m_wIconWnd && null != _m_instanceInfo.textureIndex)
                {
                    _m_wIconWnd.setTexture(_m_instanceInfo.textureIndex);
                }
            }

            _refreshIsSelected();
        }


        /// <summary>
        /// 设置item是否选中
        /// </summary>
        /// <param name="_isSelected"></param>
        public void setIsItemSelected(bool _isSelected)
        {
            _m_bIsSelected = _isSelected;
            _refreshIsSelected();
        }

        //刷新是否选中
        private void _refreshIsSelected()
        {
            if (wnd == null)
                return;

            ALUGUICommon.setGameObjEnable(wnd.selectedShowList, _m_bIsSelected);
        }

        private void _onClickSelectDate(GameObject _go)
        {
            if (_m_aOnClickItem != null)
                _m_aOnClickItem.Invoke(_m_instanceInfo);
        }
    }
}
