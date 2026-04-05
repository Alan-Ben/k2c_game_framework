using System.Collections.Generic;
using ALPackage;

/// <summary>
/// 妃子旅游
/// </summary>
[System.Serializable]
public class ConsortTravelRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }

    public long id;//
    public string name;//名称
    public string desc;//描述
    public int add_bless;//增加的加护力
    public int bless_point_multiple;//加护点倍数
    public int giftde_probability;//出现卷王的概率（万分比）
    public long time_price_id;//旅行消耗的time_price
    public NPGTextureIndex banner;//背景图
    public string btn_desc;//出游按钮描述

    public string discount_desc;//折扣描述
    public List<long> discount_desc_show_discount_left_list;//折扣描述中显示的剩余折扣万分次数列表

    public NPGGoIndex bg_go;//背景go
}

public class GSOConsortTravelRefSet : _TALSOBasicRefSet<ConsortTravelRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/consort.unity3d"; } }
    public static string objName { get { return "consort_travel"; } }
}