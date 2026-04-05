using System;
using ALPackage;
using GOE;

/// <summary>
/// 主城推送弹窗表
/// </summary>
[Serializable]
public class MainCityPushNoticeRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }
    public long id;//唯一id

    public string notice_tag;//展示使用的notice的标记
    public int sorting_order;//排序顺序(越小越先展示)
    public _NPPlayerConditionSerializeInfo login_show_condition;//登录时的展示条件(有条件且通过情况才展示)
    public _NPPlayerConditionSerializeInfo other_show_condition;//其他情况的展示条件(有条件且通过情况才展示)
}

public class GSOMainCityPushNoticeRefSet : _TALSOBasicRefSet<MainCityPushNoticeRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/main_city_push_notice.unity3d"; } }
    public static string objName { get { return "main_city_push_notice"; } }
}