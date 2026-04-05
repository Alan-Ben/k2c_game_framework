using UnityEngine;
using System.Collections;
using ALPackage;
using System.Collections.Generic;
using UnityEngine.UI;
using System;


namespace GOE
{
    /// <summary>
    /// 自定义窗口
    /// </summary>
	public class NPGGUICustomMonoCustomWnd : _AALBasicUIWndMono
	{
	    //回退按钮
	    public GameObject backBtn;
	    //在退出本显示节点的时候是否需要释放，如需释放会引起一定的卡顿，非常用界面建议使用
	    public bool needDeleteWhenQuitNode;
	    public bool CanNotRollBackQuit;//自定义界面节点是否支持界面回退
	    public bool NeedAutoRemove;//自定义界面节点是否自动删除
	    public bool IsDoLastNodeEnter;//自定义界面节点退出是否执行上一个节点的进入
	}
}