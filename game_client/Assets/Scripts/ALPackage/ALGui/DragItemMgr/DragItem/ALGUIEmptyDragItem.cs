using System;
using System.Collections.Generic;

using UnityEngine;

#if AL_UNITY_GUI
namespace ALPackage
{
    /*******************
     * 空的拖拽对象信息
     **/
    public class ALGUIEmptyDragItemInfo : _AALGUIDragItemInfo
    {
        /********************
         * 获取本拖拽物的类型ID
         **/
        public override int getDragItemType()
        {
            return 0;
        }
        /********************
         * 获取本拖拽物的来源对象类型
         **/
        public override int getDragSrcType()
        {
            return 0;
        }

        /********************
         * 获取拖拽窗口与鼠标位置的偏差
         **/
        protected override internal Vector2 _getDragWndDeltaPos()
        {
            return new Vector2();
        }

        /********************
         * 创建一个被拖拽的窗口，如不使用窗口可自行使用绘制函数
         **/
        protected override internal ALGUIBaseWnd _createDragWnd()
        {
            return null;
        }

        /****************
         * 当拖拽信息被放下到任意容器时调用的事件函数
         **/
        protected override internal void _onDrop()
        {
        }

        /****************
         * 当拖拽信息无接收对象时的事件处理函数
         **/
        protected override internal void _onDropNoReceiver()
        {
        }
    }
}

#endif
