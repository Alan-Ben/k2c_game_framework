using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;
using GOE;
[Serializable]
public class NPGGUICommonPageJumper
{
    public GameObject btn;
    public short pageIndex;
}
/// <summary>
/// 规则弹窗
/// </summary>
///
public class NPGGUIMonoRuleMain : _AALBasicUIWndMono
{
    [ALHeader("关闭按钮")]
    public GameObject closeBtn;
    [ALHeader("关闭按钮2")]
    public GameObject closeBtn2;
    [ALHeader("左切换按钮")]
    public GameObject btnLeft;
    [ALHeader("左切换按钮2")]
    public GameObject btnLeft2;
    [ALHeader("右切换按钮")]
    public GameObject btnRight;
    [ALHeader("右切换按钮2")]
    public GameObject btnRight2;
    [ALHeader("滚动scroll")]
    public ScrollRect scrollRect;
    [ALHeader("拖拽按钮")]
    public GameObject btnDrag;
    [ALHeader("预览图片列表")]
    public GameObject[] goPreviewList;
    [ALHeader("小圆点列表")]
    public GameObject[] goCircleList;
    [ALHeader("页数文本")]
    public Text txtPageNum;
    [ALHeader("页数展示需要额外减去的目录页数")]
    public long needSubtractContentsNum = 1;
    [ALHeader("图片滑动速率")]
    public float fSpeed = 5f;
    [ALInfo("页数是从0开始数的，0是第一页")]
    [ALHeader("页面跳转按钮")]
    public List<NPGGUICommonPageJumper> listPageJumper;
    
    [ALHeader("第0页需要隐藏的go列表")]
    public GameObject[] goZeroPageHide;

    [ALHeader("最后一页需要显示的go列表")]
    public GameObject[] goLastPageShow;
    [ALHeader("最后一页需要隐藏的go列表")]
    public GameObject[] goLastPageHide;
    [ALHeader("至少移动X距离就可以切换到下一页")] 
    public float moveDistanceToNext = 250f;

}
