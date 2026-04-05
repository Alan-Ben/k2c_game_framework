using ALPackage;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 骑士单选容器
public class GGUIMonoCollegeHeroChooseGrid : _TALUGUIMonoGridWnd<GGUIMonoCollegeHeroChooseGridItem>
{
    [ALHeader("列表为空显示的GoList")]
    public List<GameObject> zeroShowGoList;
}
