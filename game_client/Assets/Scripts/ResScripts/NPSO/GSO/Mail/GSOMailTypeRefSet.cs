using System.Collections;
using ALPackage;
using System;
using System.Collections.Generic;

#if NP_GAME
using GOE;
#endif


[System.Serializable]
public class GMailTypeRefObj : _IALBasicRefObj
{
    public long _refId {
        get {
            return id;
        }
    }
    public long id;  //邮件类型id
    public string name;// 标题 
    public long grid_item_ui_path_id;// 本类型在邮件列表中的预制体uipathId
    public long content_ui_path_id;// 详情内容预制体ui_res_path id
    public EMailDetailPrefabType prefab_type;
}

public class GSOMailTypeRefSet : _TALSOBasicRefSet<GMailTypeRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/mail_refdata.unity3d"; } }
    public static string objName { get { return "mail_type"; } }
}
