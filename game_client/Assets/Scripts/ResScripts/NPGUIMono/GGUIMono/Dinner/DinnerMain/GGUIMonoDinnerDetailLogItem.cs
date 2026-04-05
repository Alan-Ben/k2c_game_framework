using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// item
/// </summary>
public class GGUIMonoDinnerDetailLogItem : _TALUGUIMonoGridItem
{
    [ALHeader("内容")]
    public Text txtContent;
    [ALHeader("时间")]
    public Text txtTime;
    
    [ALHeader("玩家头像")]
    public NPGGUIMonoPlayerIcon playerIcon;
    [ALHeader("英雄头像")]
    public RawImage headIcon;

    public List<GameObject> heroJoinerShowGos;
}
