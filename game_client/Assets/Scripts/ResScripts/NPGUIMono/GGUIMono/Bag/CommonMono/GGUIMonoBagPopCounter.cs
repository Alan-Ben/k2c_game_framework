using ALPackage;
using GOE;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GGUIMonoBagPopCounter : _AALBasicUIWndMono
{

    [ALHeader("增加按钮")]
    public GameObject increaseBtn;

    [ALHeader("减少按钮")]
    public GameObject decreaseBtn;

    [ALHeader("单次增加多个按钮")]
    public GameObject addLotBtn;

    [ALHeader("单次减少多个按钮")]
    public GameObject reduceLotBtn;

    [ALHeader("最大数量按钮")]
    public GameObject maxBtn;

    [ALHeader("最小数量按钮")]
    public GameObject minBtn;
    
    [ALHeader("数量输入框")]
    public InputField numInputField;

    [ALHeader("slider内数量信息")]
    public Text countInfo;
    [ALHeader("slider外数量信息")]
    public Text selectNumInfo;

    [ALHeader("当前使用数量 xN")]
    public Text curCountTxt;

    [ALHeader("达到最大数量时需要置灰的列表")]
    public List<MaskableGraphic> maxGrayImgList;

    [ALHeader("达到最小数量时需要置灰的列表")]
    public List<MaskableGraphic> minGrayImgList;

    [ALHeader("选择数量进度条")]
    public NPGGUIMonoCommonSlider selectCountSlider;
}
