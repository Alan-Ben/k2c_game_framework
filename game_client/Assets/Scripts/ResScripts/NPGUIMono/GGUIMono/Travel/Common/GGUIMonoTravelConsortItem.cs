using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

//情报中，妃子item
public class GGUIMonoTravelConsortItem:_AALBasicUIWndMono
{
    [ALHeader("点击按钮")]
    public GameObject btnClick;
    [ALHeader("头像")]
    public RawImage icon; 
    [ALHeader("好感度数值")]
    public TextEx txtLike; 
    [ALHeader("好感度进度条")]
    public Slider sliderLike; 
    [ALHeader("亲密度数值")]
    public TextEx txtIntimacy; 
    [ALHeader("概率提升显示的列表")]
    public List<GameObject> upShowList; 
    [ALHeader("以获得妃子显示的列表，否则隐藏")]
    public List<GameObject> gotShowList; 
    [ALHeader("未获得显示的列表，否则隐藏")]
    public List<GameObject> ungetShowList; 
    [ALHeader("未解锁置灰列表")]
    public List<MaskableGraphic> lockGrayList;
}