using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;
using GOE;

/// <summary>
/// 通用排序mono 
/// </summary>
///
public class NPGGUIMonoCommonFitterTab : _AALBasicUIWndMono
{
    [ALHeader("当前选择的排序文本")]
    public List<TextEx> sortTypeTxt;
    
    [ALHeader("当前选择的排序图片")]
    public RawImage sortTypeIcon;

    [ALHeader("选中tog")]
    public NPGGUIMonoCommonToggleEx togSelect;

}
