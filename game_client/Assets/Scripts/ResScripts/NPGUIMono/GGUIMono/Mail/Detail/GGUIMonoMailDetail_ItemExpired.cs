using UnityEngine;
using ALPackage;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class GGUIMonoMailDetail_ItemExpired : GGUIMonoMailDetail_Base
{
    [ALHeader("物品图片")]
    public RawImage texItemIcon;
    [ALHeader("物品名")]
    public TextEx txtItemName;
    [ALHeader("物品描述")]
    public TextEx txtItemDesc;

}