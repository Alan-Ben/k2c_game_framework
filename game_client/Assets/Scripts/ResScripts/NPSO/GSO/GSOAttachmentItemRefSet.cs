
using System;
using ALPackage;

[Serializable]
public class AttachmentItemRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }

    public long id;
    public EAttachmentItemType attachment_item_type; // 附加物的类型，根据类型后面两个字段只有一个会生效
    public EAttachType attach_type;     // 附加类型
    public NPGGoIndex attach_go_index;  // 附加的 GO 索引
    public long attach_sfx_id;          // 附加的特效 id
}

public class GSOAttachmentItemRefSet : _TALSOBasicRefSet<AttachmentItemRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
    public static string objName { get { return "attachment_item"; } }
}
