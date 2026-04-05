using System;
using System.Collections.Generic;

using UnityEngine;

/**********************
 * 基本的窗口加载与处理对象
 **/
namespace ALPackage
{
    public abstract class _AALBasicLoadUIWndBasicClass : _AALBasicLoadObj, _IALBasicUIWndInterface
    {
        /// <summary>
        /// 在切换主视图时是否会需要释放
        /// 一般不释放，如果子类有需要可以重载函数处理
        /// </summary>
        public virtual bool needDiscardOnSwitch
        {
            get { return false; }
        }

        /// <summary>
        /// 以下是interface的接口声明
        /// </summary>
        /// <returns></returns>
        /**************
         * 获取用于操作的窗口对象
         **/
        public abstract GameObject getGameObj();
        public abstract RectTransform rectTransform { get; }

        /******************
         * 显示本窗口
         **/
        public abstract void showWnd();
        public abstract void showWnd(Action _delayDoneAction);
        public abstract void showWndWithoutAni();
        public abstract void showWndWithoutAni(Action _delayDoneAction);
        public abstract void hideWnd();
        public abstract void hideWnd(Action _delayDoneAction);
        public abstract void hideWndWithoutAni();
        public abstract void hideWndWithoutAni(Action _doneAction);

        /***************
         * 重置窗口数据，代替原先的discard函数
         **/
        public abstract void resetWnd();
    }
}
