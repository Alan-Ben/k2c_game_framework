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
public class _ATNPGGUIMonoCommonSort<T> : _ATNPGGUIMonoCommonMutexFiiterWnd<T>
{
    [ALHeader("当前选择的排序文本")]
    public Text sortTypeTxt;
    
    [ALHeader("展开收起toggle")]
    public NPGGUIMonoCommonToggleEx toggleSelect;

    [ALHeader("收起按钮")]
    public GameObject closeBtn;
}
