using ALPackage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//弹幕容器item
public class GGUIMonoCommentContainerItem : _AALBasicUIWndMono
{
    [ALHeader("头像")]
    public RawImage iconImg;
    [ALHeader("名称")]
    public Text txtName;
    [ALHeader("内容")]
    public Text txtDesc;
}
