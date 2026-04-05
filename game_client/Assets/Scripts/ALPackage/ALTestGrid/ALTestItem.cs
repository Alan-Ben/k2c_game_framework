using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using ALPackage;

public class ALTestItem : _ATALUGUIBasicGridItemWnd<ALTestItemMono>
{
    public ALTestItem(ALTestItemMono _wnd)
            : base(_wnd)
    {
     initWnd();
    }

    /******************
     * 显示窗口的事件函数
     **/
    protected override void _onShowWnd()
    {

    }
    /******************
     * 隐藏窗口的事件函数
     **/
    protected override void _onHideWnd()
    {

    }
    /******************
     * 重置窗口数据的事件函数
     **/
    protected override void _onReset()
    {

    }

    //重置Grid单个对象
    protected override void _resetGridItem()
    {
    }
    /******************
     * 释放资源时触发的事件
     **/
    protected override void _onDiscard()
    {

    }
    /*************
     * 窗口初始化完成调用的函数
     * */
    protected override void _onWndInitDone()
    {

    }
}
