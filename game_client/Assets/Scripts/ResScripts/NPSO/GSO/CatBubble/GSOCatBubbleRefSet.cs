using ALPackage;
using System;
using System.Collections.Generic;
using GOE;

/// <summary>
/// 猫咪气泡表
/// </summary>
[Serializable]
public class CatBubbleRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }
    public long id;//唯一识别标记
    public _NPPlayerConditionSerializeInfo show_condition;//显示条件
    public _NPPlayerConditionSerializeInfo invalid_condition;//失效条件
    public List<string> bubble_list;//气泡文本列表
}

public class GSOCatBubbleRefSet : _TALSOBasicRefSet<CatBubbleRefObj> 
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/cat_bubble_refdata.unity3d"; } }
    public static string objName { get { return "cat_bubble"; } }
}
