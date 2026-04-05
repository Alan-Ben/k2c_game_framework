using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using ALPackage;

public class ALTestGridMainMono : _AALMonoMain
{
    public ALTestGridMono gridMono;


    /********************
     * 获取当前的资源对应整形
     **/
    public override int curVersionNum { get { return int.MaxValue; } }
    /********************
     * 获取当前的资源对应整形
     **/
    public override int curServerVersionNum { get { return int.MaxValue; } }
    /// <summary>
    /// 当Hotfix版本过低的时候的触发效果
    /// </summary>
    public override void onClickVersionLowerErr() { }
    /// <summary>
    /// 当服务器版本过低的时候的触发效果
    /// </summary>
    public override void onClickServerVersionLowerErr() { }
    /// <summary>
    /// 当Hotfix版本过低的时候的触发效果
    /// </summary>
    public override void onClickHotfixVersionLowerErr() { }
    /****************
     * 当发生了未知错误时
     **/
    public override void onUnknowErrorOccurred(Exception _e) { }
    /********************
     * 当执行窗口大小更改时
     **/
    public override void onClientScreenOnSize(int _newWidth, int _newHeight) { }

#if AL_UNITY_GUI
    /********************
     * 在GUI响应了鼠标操作时触发的事件
     **/
    protected override void _onALGUICatchMouse() { }
    /********************
     * 子类用于在GUI未处理鼠标操作时进行的鼠标操作处理函数
     **/
    protected override void _dealMouseActionWhenALGUIDidNotCatchMouse() { }
#endif

    /********************
     * 在脚本开始运行的时候调用的函数
     **/
    protected override void _onStart()
    {
        ALTestGrid.instance = new ALTestGrid(gridMono);
        ALTestGrid.instance.setItemCount(10000);

        ALTestGrid.instance.showWnd();
    }

    /********************
     * 子类用于处理GUI判断前的操作处理
     **/
    protected override void _onPreGuiUpdate() { }

    /********************
     * 子类用于处理正常帧处理操作
     **/
    protected override void _onUpdate() { }
}
