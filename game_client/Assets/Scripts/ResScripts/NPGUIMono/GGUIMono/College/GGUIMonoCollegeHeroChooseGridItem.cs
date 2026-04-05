using ALPackage;
using GOE;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//大学骑士单选item
public class GGUIMonoCollegeHeroChooseGridItem : _TALUGUIMonoGridItem
{
    [ALHeader("骑士展示")]
    public GGUIMonoCollegeHeroChooseItem heorItemMono;

    [ALHeader("进修按钮")]
    public GameObject learnBtn;

    [ALHeader("正在进修中显示的GoList")]
    public List<GameObject> isLearningShowGoList;

    [ALHeader("正在进修中隐藏的GoList")]
    public List<GameObject> isLearningHideGoList;
}
