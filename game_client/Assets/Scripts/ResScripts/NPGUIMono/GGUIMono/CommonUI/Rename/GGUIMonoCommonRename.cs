using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;
using GOE;
using System;

/// <summary>
/// 改名子弹窗
/// /// </summary>
public  class GGUIMonoCommonRename :  _AALBasicUIWndMono
{
    [ALHeader("输入框")]
    public InputField nameInputField;

    [ALHeader("当前个数显示文本")]
    public Text inputCountTxt;

    [ALHeader("消耗item")]
    public NPGGUIMonoCommonItem costItemMono;

    [ALHeader("确定按钮")]
    public GameObject confirmBtn;

    [ALHeader("不可使用时需要置灰的列表")]
    public List<MaskableGraphic> grayImgList;
    [ALHeader("有输入时需要显示的GO列表")]
    public List<GameObject> goInputShowList;
    [ALHeader("有输入时需要隐藏的GO列表")]
    public List<GameObject> goInputHideList;

    [ALHeader("有消耗显示的GoList")]
    public List<GameObject> costShowGoList;

    [ALHeader("没有消耗显示的GoList")]
    public List<GameObject> noCostShowGoList;

    [ALHeader("关闭按钮")]
    public GameObject closeBtn;

    [ALHeader("随机按钮")]
    public GameObject randomBtn;
    [ALHeader("自定义名字长度不在范围内提示Key，未配置会提示默认tip")]
    public string nameNotInRangeTipKey;
}
