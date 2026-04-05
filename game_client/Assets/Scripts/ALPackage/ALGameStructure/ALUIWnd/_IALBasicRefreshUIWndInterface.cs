using System;

using UnityEngine;

/**********************
 * 基本的窗口加载与处理对象
 **/
namespace ALPackage
{
    public interface _IALBasicRefreshUIWndInterface
    {
        /** 显示序列号接口对象 */
        int showOpSerialize { get; }

        /** 刷新函数 */
        void frameRefresh();
    }
}
