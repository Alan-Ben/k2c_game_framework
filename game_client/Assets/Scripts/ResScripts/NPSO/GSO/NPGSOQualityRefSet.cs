using ALPackage;
using NPEnum;
using UnityEngine;


[System.Serializable]
public class NPQualityRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }

    public long id;//唯一id
    public ENPQualityClass quality_class;
    public EQuality quality;//品质
    public NPGSpriteIndex icon;//普通卡牌的品质图标,品质框（展示环境1，默认为道具item展示时品质框）
    public NPGSpriteIndex sp_icon;//特殊的品质图标,特殊品质框（展示环境2，各个类型独特的展示环境）
    public string name;//品质的名称key
    public Color color;//颜色(读取excel上的颜色16进制字符串(color_hex_string)转化而来)
}


public class NPGSOQualityRefSet : _TALSOBasicRefSet<NPQualityRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
    public static string objName { get { return "quality"; } }
}
