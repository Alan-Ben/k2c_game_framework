using UnityEngine;
using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    public abstract class _ATNPGGUIWndCommonSelectItemGrid<T, TGM, TW> : _ANPGGUIBasicGridSubWnd<T, TGM, TW>, _INPGGUIWndCommonSelectItemOpReceive
     where T : NPGGUIMonoCommonSelectItem where TGM : _TNPGGUIMonoCommonSelectItemGrid<T> where TW : _ATNPGGUIWndCommonSelectItem<T>
    {
        /// <summary>
        /// 当前选中的索引
        /// </summary>
        private int _m_iSelectIdx;

        public _ATNPGGUIWndCommonSelectItemGrid(TGM _containerMono)
            : base(_containerMono)
        {
            _m_iSelectIdx = -1;
        }

        // 刷新对象
        protected override void _refreshItemwnd(TW _itemWnd, int _itemIdx)
        {
            bool isSelected = (_m_iSelectIdx == _itemIdx);
            //根据选中状态设置选中标记
            if(null != _itemWnd)
                _itemWnd._setSelected(isSelected);

            // 刷新物品UI
            _refreshGridItemwnd(_itemWnd, _m_iSelectIdx == _itemIdx, _itemIdx);
        }

        /******************
         * 重置窗口数据的事件函数
         **/
        protected override void _onReset()
        {
            _m_iSelectIdx = -1;

            _dealGridReset();
        }
        /******************
         * 释放资源时触发的事件
         **/
        protected override void _onDiscard()
        {
            _m_iSelectIdx = -1;

            _dealGridDiscard();
        }
        /*************
         * 窗口初始化完成调用的函数
         * */
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;
        }

        /// <summary>
        /// 选中对象的处理接口函数
        /// </summary>
        /// <param name="_idx"></param>
        public virtual void onSelectItem(int _idx)
        {
            //设置选中标记
            _m_iSelectIdx = _idx;
            //刷新所有对象
            forceRefreshAllItem();
        }

        /// <summary>
        /// 清空选中对象
        /// </summary>
        public void clearSelected()
        {
            _m_iSelectIdx = -1;
            //刷新所有对象
            forceRefreshAllItem();
        }

        /// <summary>
        /// 处理本对象的释放操作
        /// </summary>
        protected abstract void _dealGridReset();
        protected abstract void _dealGridDiscard();
        /// <summary>
        /// 本对象的刷新对象处理函数
        /// </summary>
        /// <param name="_itemWnd"></param>
        /// <param name="_itemIdx"></param>
        protected abstract void _refreshGridItemwnd(TW _itemWnd, bool _isSelected, int _itemIdx);
        /// <summary>
        /// 点击对象操作
        /// </summary>
        /// <param name="_idx"></param>
        public abstract void onClickItem(int _idx);
    }
}
