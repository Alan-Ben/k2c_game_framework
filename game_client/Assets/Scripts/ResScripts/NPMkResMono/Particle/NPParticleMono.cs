using System.Collections.Generic;
using GOE;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 粒子类型对应的缓存池及预制体
/// </summary>
public class NPParticleMono : MonoBehaviour
{
    [ALHeader("图标")]
    public Image imgIcon;
    [ALHeader("道具")]
    public GGUIMonoCommonSimpleItem monoSimpleItem;
    [ALHeader("飞行过程中需要改变透明度的对象")] 
    public List<Graphic> alphaList;
    
    public void setAlpha(float _alpha)
    {
        if(null == alphaList)
            return;
        
        foreach (Graphic graphic in alphaList)
        {
            if(null == graphic)
                continue;

            graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, _alpha);
        }
    }
}