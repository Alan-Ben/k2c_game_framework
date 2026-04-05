using System.Collections.Generic;
using GOE;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 周卡选形象的item
/// </summary>
public class GGUIMonoWeekCardAssignTDShowItem : _ANPGGUIMonoSingleChoiceItem
{
    [ALHeader("图片")]
    public RawImage texIcon;
    [ALHeader("当前选择好的形象的显示")]
    public List<GameObject> curSelectedShow;
}
