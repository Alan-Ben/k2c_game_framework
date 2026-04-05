using UnityEngine;
using System.Collections.Generic;
using ALPackage;

using UnityEngine.UI;

//教程窗口的阶段拖拽对象及信息
[System.Serializable]
public class NPGGUIMonoTutorialDragObj
{
    public RectTransform dragEndRect;//拖拽操作的终点Rect对象
    public GameObject dragGo;//拖拽对象
    public string dragEndFunc;//正确的拖拽结束后的操作
    public string dragBeginFunc;//开始拖拽时的操作
    public string dragFunc;    //拖拽时的操作
    public string dragFailureFunc; //拖拽失败的操作
}

/************************
 * 每个步骤优先处理的操作信息
 **/
[System.Serializable]
public class NPGGUIMonoTutorialStepDealCondFunc
{
    public string tutorialCondition;
    public string tutorialDealFunc;
}

/************************
 * 如在战斗中，此处会调整对应对象AI值的信息
 **/
[System.Serializable]
public class NPGGUIMonoTutorialChgAIValue
{
    public long actorId;
    public int aiId;
    public int aiValue;
}

/**************
 * 教程窗口的阶段对象
 **/
[System.Serializable]
public class NPGGUIMonoTutorialWndStepObj
{
    [ALHeader("第一个变量是name，在编辑引导步骤的时候，在列表会默认显示这个名字方便编辑")]
    public string name;
    [ALHeader("本步骤的父节点go,可以用来判断是否拖错了")]
    public GameObject selfGo;
    
    [ALHeader("聚焦区域的动态获取字符串，默认为空表示不更改")]
    public string moveMaskRectStr;
    [ALHeader("本阶段指定聚焦对象")]
    public RectTransform MoveMaskRect;
    [ALHeader("是否使用移动遮罩")]
    public bool isMoveMaskEnable = true;

    [ALHeader("本步骤需要高亮的物体")]
    public RectTransform highlightGoRectTransform;
    [ALHeader("高亮Go的定位字符串")]
    public string highlightGoLocationStr;
    
    [ALHeader("下一步的按钮对象")]
    public RectTransform nextStepBtn;
    [TextArea(2,6)]
    [ALHeader("本阶段执行的操作：使用tutorialFunc字符串方式解析，在开始本步骤之前执行的效果")]
    public string stepDealFunc;

    [ALHeader("点击时优先判断的处理效果队列，如处理了队列中的效果则不处理后续配置效果")]
    public List<NPGGUIMonoTutorialStepDealCondFunc> clickPreDealFunc;
    [TextArea(2,6)]
    [ALHeader("点击下一步按钮后执行的操作")]
    public string clickDealFunc;

    [ALHeader("触发本步骤完成的消息类型")]
    public ENPTutorialTriggerType triggerType;
    [ALHeader("触发本步骤完成的消息类型的监听参数，先预留，后面可能用得到")]
    public string triggerTypeArgs;
    
    [ALHeader("是否自动关闭本阶段condition,满足会自动调到下一步")]
    public string autoNextStepCondition;
    
    [ALHeader("本阶段的拖拽相关信息")]
    public NPGGUIMonoTutorialDragObj dragInfo;//拖拽相关信息
    [ALHeader("进入本阶段时需要有效的Go对象")]
    public List<GameObject> enableGo;
    [ALHeader("进入本阶段时需要无效的Go对象")]
    public List<GameObject> disableGo;

    [ALHeader("退出本阶段时需要有效的Go对象")]
    public List<GameObject> exitEnableGo;
    [ALHeader("退出本阶段时需要无效的Go对象")]
    public List<GameObject> exitDisableGo;

    [ALHeader("本阶段引导音源对象索引")]
    public NPGAudioIndex gudieVoiceIndex;
    
    [ALHeader("点击事件的延迟响应时间，延迟本时间后才能响应（秒）")]
    public float delayClickResponseTime;
    [ALHeader("遮罩初始化延迟时间（秒）")]
    public float delayInitMoveMaskTime;
    [ALHeader("延迟多久自动到下一步，当时间小于等于0时表示无效（秒）")]
    public float autoDelayToNextStep = -1;
    [ALHeader("引导步骤埋点ID")]
    public int traceId;

    //CustomDrawer用的，用于存储是否展示所有字段，还是只展示有数值的字段
    public bool _showAllFields = false;
}

/************
 * 匹配窗口脚本对象
 **/
public class NPGGUIMonoTutorialMainWnd : _AALBasicUIWndMono
{
    [ALHeader("引导窗口的步骤列表")]
    public List<NPGGUIMonoTutorialWndStepObj> stepList;

    [TextArea(2,6)]
    [ALHeader("正常结束tutorial执行脚本。使用PlayerEffect字符串方式解析")]
    public string turotialEndFunc;

    [ALHeader("跳过当前教程阶段")]
    public GameObject skipBtn;
    //是否大阶段完成后重置操作状态
    public bool resetOpState = true;
    [TextArea(2,6)]
    [ALHeader("跳过tutorial时执行的脚本。使用PlayerEffect字符串方式解析")]
    public string skipTurotialEndFunc;
}
