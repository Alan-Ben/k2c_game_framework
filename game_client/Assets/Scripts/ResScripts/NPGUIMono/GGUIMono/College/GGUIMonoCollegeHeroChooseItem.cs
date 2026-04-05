using ALPackage;
using GOE;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//大学骑士item
public class GGUIMonoCollegeHeroChooseItem : _AALBasicUIWndMono
{
    [ALHeader("骑士等级文本")]
    public Text heroLvTxt;

    [ALHeader("骑士生命值文本")]
    public Text heroHPTxt;

    [ALHeader("骑士攻击文本")]
    public Text heroATKTxt;

    [ALHeader("骑士天赋经验文本")]
    public Text talentExpTxt;
}
