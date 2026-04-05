using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//妃子卡牌item
public class GGUIMonoConsortIconItem:_ANPGGUIMonoSingleChoiceItem
{
    [ALHeader("妃子名字")] 
    public TextEx txtName;
    
    [ALHeader("妃子图片")] 
    public RawImage texConsortIcon;
    [ALHeader("妃子图片背景")]
    public Image imgConsortIconBg;
    
    [ALHeader("用于处理缩放的go")] 
    public Transform scaleGo;
}