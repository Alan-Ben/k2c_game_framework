using ALPackage;
using System;

/// <summary>
/// 运营公告海报图表
/// </summary>
[Serializable]
public class AnnouncementBannerRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }

    public long id;//唯一id
    public NPGTextureIndex banner_image;//海报图片
}

public class GSOAnnouncementBannerRefSet : _TALSOBasicRefSet<AnnouncementBannerRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/announcement_refdata.unity3d"; } }
    public static string objName { get { return "announcement_banner"; } }
}
