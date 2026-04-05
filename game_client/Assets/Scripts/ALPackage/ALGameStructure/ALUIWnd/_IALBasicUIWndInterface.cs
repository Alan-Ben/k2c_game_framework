using System;

using UnityEngine;

/**********************
 * 基本的窗口加载与处理对象
 **/
namespace ALPackage
{
    public interface _IALBasicUIWndInterface
    {
        /**************
         * 获取用于操作的窗口对象
         **/
        GameObject getGameObj();

        /******************
         * 显示本窗口
         **/
        void showWnd();
        void showWndWithoutAni();
        void hideWnd();
        void hideWndWithoutAni();

        /***************
         * 重置窗口数据，代替原先的discard函数
         **/
        void resetWnd();
        /// <summary>
        /// 资源释放函数
        /// </summary>
        void discard();
    }
}
