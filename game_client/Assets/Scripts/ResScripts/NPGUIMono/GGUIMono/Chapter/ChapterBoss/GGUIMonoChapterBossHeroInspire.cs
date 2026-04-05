using System;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// boos战大臣鼓舞item
/// </summary>
public class GGUIMonoChapterBossHeroInspire : ALGGUIMonoCommonFollowItem
{
    [ALHeader("出现动画")]
    public Animation showAni;
    
    [ALHeader("骑士信息")] 
    public GGUIMonoHeroCommonCardItem monoHeroInfo;
    
    [ALHeader("大臣废话文本")] 
    public Text txtHeroTalk;
    [ALHeader("boss废话文本")] 
    public Text txtBossTalk;
    
    public event Action onEventReduceHp;
    
    /// <summary>
    /// 执行扣血表现
    /// </summary>
    public void ReduceHP()
    {
        if (onEventReduceHp != null) 
            onEventReduceHp.Invoke();
    }
}