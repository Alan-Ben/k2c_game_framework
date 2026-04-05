using UnityEngine;
using System.Collections;
using ALPackage;
using UnityEngine.UI;
using System.Collections.Generic;



//邮件内的奖励item，主要加了已领取的展示goList
public class GGUIMonoMailRewardItem : _AALBasicUIWndMono
{
    [ALHeader("展示的item")]
    public NPGGUIMonoCommonItem item;
    [ALHeader("已领取的时候显示的go列表")]
    public List<GameObject> goShowOnRewardGet;
}

