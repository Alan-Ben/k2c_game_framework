using ALPackage;
using GOE;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//大学骑士多选item
public class GGUIMonoCollegeHeroChooseMulGridItem : _TALUGUIMonoGridItem
{
    [ALHeader("骑士展示")]
    public GGUIMonoCollegeHeroChooseItem heorItemMono;

    [ALHeader("选择按钮")]
    public GameObject chooseBtn;

    [ALHeader("选中时显示的GoList")]
    public List<GameObject> chooseShowGoList;

    [ALHeader("上次选择额外显示Go")]
    public GameObject extraShowGo;

    [ALHeader("正在进修中显示的GoList")]
    public List<GameObject> isLearningShowGoList;

    [ALHeader("正在进修中隐藏的GoList")]
    public List<GameObject> isLearningHideGoList;
}
