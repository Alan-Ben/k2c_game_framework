using System;
using System.Collections.Generic;
using UnityEngine;

namespace ALPackage
{
    /// <summary>
    /// 跟随UI对象的窗口模板基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class _ATALGGUIWndCommonFollowItem<T> : _ATALBasicUISubWnd<T>, _IALGGUIWndCommonFollowItem where T : ALGGUIMonoCommonFollowItem
    {
        public _ATALGGUIWndCommonFollowItem(T _wnd) : base(_wnd)
        {
        }

        /// <summary>
        /// 资源数据对象
        /// </summary>
        public _AALBasicLoadResIndexInfo followItemIndex { get { return wnd._resIndex; } }

        /// <summary>
        /// 设置对应父节点的UI位置
        /// 如果子类有特殊需求，可以进行重载处理
        /// </summary>
        /// <param name="_uiPos"></param>
        public virtual void setUIPos(Vector3 _uiPos)
        {
            if (null == wnd)
                return;

            //设置坐标
            ALUGUICommon.setUIPos(getGameObj(), _uiPos);
        }
    }
}