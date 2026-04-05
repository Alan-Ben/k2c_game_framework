using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

//情报中NPCitem
public class GGUIMonoTravelNpcItem:_AALBasicUIWndMono
{
    [ALHeader("名字")]
    public TextEx name; 
    [ALHeader("头像")]
    public RawImage icon; 
    [ALHeader("npc离开显示列表")]
    public List<GameObject> leaveShowList; 
    [ALHeader("未解锁置灰列表")]
    public List<MaskableGraphic> lockGrayList;
}