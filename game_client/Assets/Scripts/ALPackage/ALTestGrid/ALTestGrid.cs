using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using ALPackage;

public class ALTestGrid : _AALUGUIBasicGridSubWnd<ALTestItemMono, ALTestGridMono, ALTestItem>
{
    private static ALTestGrid _g_instance = null;
    public static ALTestGrid instance
    {
        get { return _g_instance; }
        set { _g_instance = value; }
    }

    public ALTestGrid(ALTestGridMono _grid)
            : base(_grid)
    {
     initWnd();
    }

    /*************
     * 根据带入的已经实例化的图标对象，创建一个对应子窗口的管理对象
     **/
    protected override ALTestItem _createItemWnd(ALTestItemMono _itemMono)
    {
        return new ALTestItem(_itemMono);
    }
    /********************
     * 根据带入的窗口对象以及索引刷新相关的显示信息
     **/
    protected override void _refreshItemwnd(ALTestItem _item, int _itemIdx)
    {
        ALUGUICommon.setLabelTxt(_item.wnd.testTxt, _itemIdx.ToString());
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
