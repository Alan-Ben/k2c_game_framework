using ALPackage;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// item
/// </summary>
public class GGUIMonoMomentsImageItem : _AALBasicUIWndMono
{
    [ALHeader("点击按钮")]
    public GameObject btnClick;

    public Transform imageGroupParent;	    
    
    [ALHeader("朋友圈图片预览ui")]
    public long momentImageGroupPathId = 6210; 

}

