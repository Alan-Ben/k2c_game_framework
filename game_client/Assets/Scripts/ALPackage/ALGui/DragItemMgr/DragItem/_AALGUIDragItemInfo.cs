using System;
using System.Collections.Generic;

using UnityEngine;

#if AL_UNITY_GUI
namespace ALPackage
{
    /******************
     * 正在拖拽的对象信息结构体
     **/
    public abstract class _AALGUIDragItemInfo
    {
        /********************
         * 获取本拖拽物的类型ID
         **/
        public abstract int getDragItemType();
        /********************
         * 获取本拖拽物的来源对象类型
         **/
        public abstract int getDragSrcType();
        /********************
         * 获取拖拽窗口与鼠标位置的偏差
         **/
        protected abstract internal Vector2 _getDragWndDeltaPos();
        /********************
         * 创建一个被拖拽的窗口，如不使用窗口可自行使用绘制函数
         **/
        protected abstract internal ALGUIBaseWnd _createDragWnd();
        /****************
         * 当拖拽信息被放下到任意容器时调用的事件函数
         **/
        protected abstract internal void _onDrop();
        /****************
         * 当拖拽信息无接收对象时的事件处理函数
         **/
        protected abstract internal void _onDropNoReceiver();
    }
}

#endif
