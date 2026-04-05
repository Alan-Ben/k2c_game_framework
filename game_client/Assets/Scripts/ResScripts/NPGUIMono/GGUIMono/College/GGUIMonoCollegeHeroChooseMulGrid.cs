using ALPackage;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 大学骑士多选容器
public class GGUIMonoCollegeHeroChooseMulGrid : _TALUGUIMonoGridWnd<GGUIMonoCollegeHeroChooseMulGridItem>
{
    [ALHeader("当前进修数量文本")]
    public Text learnNumTxt;

    [ALHeader("列表为空显示的GoList")]
    public List<GameObject> zeroShowGoList;
}
