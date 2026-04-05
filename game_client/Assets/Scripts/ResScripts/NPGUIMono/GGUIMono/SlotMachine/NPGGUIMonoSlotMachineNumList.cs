using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;
using GOE;


/// <summary>
/// 老虎机带个数字的mono
/// </summary>
///
public class NPGGUIMonoSlotMachineNumList : _AALBasicUIWndMono
{
    
    [ALInfo("从个位开始填入")]
    [ALHeader("数字集合")]
    public List<NPGGUIMonoSlotMachinePerNum> numList;
}
