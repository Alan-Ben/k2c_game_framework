using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;
using GOE;
using System;

// 简易漫画每个页面的脚本
public class GGUIMonoSimpleComicPage : _AALBasicUIWndMono
{
    [ALHeader("自动结束时间(秒)，配-1表示这一页不会自动切换到下一页")]
    public float next_time;
    
}
