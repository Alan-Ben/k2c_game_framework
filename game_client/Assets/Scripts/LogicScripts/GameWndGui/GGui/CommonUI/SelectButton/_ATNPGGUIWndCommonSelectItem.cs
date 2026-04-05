using UnityEngine;
using System.Collections.Generic;
using ALPackage;
using System;

namespace GOE
{
    /// <summary>
    /// 本对象操作接收接口对象
    /// </summary>
    public interface _INPGGUIWndCommonSelectItemOpReceive
    {
        /// <summary>
        /// 选中对象的处理接口函数
        /// </summary>
        /// <param name="_idx"></param>
        void onSelectItem(int _idx);
        /// <summary>
        /// 点击对象操作
        /// </summary>
        /// <param name="_idx"></param>
        void onClickItem(int _idx);
    }


    public abstract class _ATNPGGUIWndCommonSelectItem<T> : _ANPGGUIBasicGridItemWnd<T> where T : NPGGUIMonoCommonSelectItem
    {
        /// <summary>
        /// 操作接收对象
        /// </summary>
        private _INPGGUIWndCommonSelectItemOpReceive _m_orOpReceiver;

        public _ATNPGGUIWndCommonSelectItem(_INPGGUIWndCommonSelectItemOpReceive _receiver, T _wnd)
            : base(_wnd)
        {
            _m_orOpReceiver = _receiver;
        }

        protected override void _onDiscard()
        {
            _m_orOpReceiver = null;

            _dealSelectBtnDiscard();
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClickSelectButton);
        }

        protected virtual void _onClickSelectButton(GameObject _go)
        {
            //处理点击操作
            if(null != _m_orOpReceiver)
                _m_orOpReceiver.onClickItem(itemIdx);

            //处理选中操作
            if(null != _m_orOpReceiver)
                _m_orOpReceiver.onSelectItem(itemIdx);
        }

        /// <summary>
        /// 设置选中状态
        /// </summary>
        /// <param name="_isSelect"></param>
        protected internal void _setSelected(bool _isSelect)
        {
            if(null == wnd)
                return;

            ALUGUICommon.setGameObjEnable(wnd.isOnShow, _isSelect);
            ALUGUICommon.setGameObjEnable(wnd.isOffShow, !_isSelect);
        }

        /// <summary>
        /// 处理本对象的释放操作
        /// </summary>
        protected abstract void _dealSelectBtnDiscard();
    }
}


