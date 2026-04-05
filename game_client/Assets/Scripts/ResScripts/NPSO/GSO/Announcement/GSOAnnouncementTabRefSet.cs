using ALPackage;
using System;

/// <summary>
/// 运营公告页签图表
/// </summary>
[Serializable]
public class AnnouncementTabRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }

    public long id;//唯一id
    public NPGTextureIndex tab_image;//页签图片
}

public class GSOAnnouncementTabRefSet : _TALSOBasicRefSet<AnnouncementTabRefObj> 
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/announcement_refdata.unity3d"; } }
    public static string objName { get { return "announcement_tab"; } }
}
