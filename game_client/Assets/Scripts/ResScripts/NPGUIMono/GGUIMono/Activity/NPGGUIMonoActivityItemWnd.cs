using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;
using UnityEngine.UI;

//活动阶段数据
[System.Serializable]
public class NPActivityStepShow {
    public int step;//哪个阶段
    public List<GameObject> showList;//需要显示的对象
}


//单个活动的窗口或者对象脚本
//本脚本可作为顶层窗口，作为点击活动对象之后展示的顶层窗口
public class NPGGUIMonoActivityItemWnd : _AALBasicUIWndMono
{
    //活动Id
    public long activityId;
    //活动时间
    public Text txtActivityTime;

    //活动当前阶段的条件
    public Text txtCurStepConditon;

    public Text txtLeftCanCompleteTimes;//展示活动完成次数

    //领取按钮
    public GameObject getRewardBtn;
    //跳转按钮
    public GameObject goToBtn;

    //允许获取奖励的展示信息
    public List<GameObject> canGetReward;
    //不可完成的展示信息
    public List<GameObject> noComplete;
    //已领奖后的展示信息
    public List<GameObject> gotRewardDone;

    //无法参与活动的展示信息
    public List<GameObject> canNotJoin;
    //各阶段需要显示的go列表
    public List<NPActivityStepShow> stepShowGoList;
    public List<GameObject> noneShowGoList;//当无阶段展示的时候显示的列表
    
    //奖励集合窗口Mono
    public NPGGUIMonoRewardGrid taskRewardGrid;

    public Slider parocessSld;//活动进度条
    public Text processText;//进度文本
    public string processKey;//进度翻译文本
}
