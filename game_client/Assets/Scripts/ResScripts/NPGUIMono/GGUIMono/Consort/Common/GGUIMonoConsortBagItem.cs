using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

//情人物品使用item
public class GGUIMonoConsortBagItem:_ANPGGUIMonoSingleChoiceItem
{
    [ALHeader("显示的物品")] 
    public NPGGUIMonoCommonItem item;
    [ALHeader("使用后获得的物品icon")] 
    public RawImage texGainIcon;
    [ALHeader("使用后获得的物品数量")] 
    public TextEx txtGainCount;
    [ALHeader("数量为0置灰的列表")] 
    public List<MaskableGraphic> goEmptyGray;
    [ALHeader("数量为0显示的列表")] 
    public List<GameObject> goEmptyShow;

    [ALHeader("红点")]
    public NPGGUIMonoCommonRedTip monoRedTip;
}