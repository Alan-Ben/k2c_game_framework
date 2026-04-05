using System.Collections;
using ALPackage;
using System;
using System.Collections.Generic;

#if NP_GAME
using GOE;
#endif


[System.Serializable]
public class GMailSenderRefObj : _IALBasicRefObj
{
    public long _refId {
        get {
            return id;
        }
    }
    public long id;  //发送者id
    public string name;// 发送者名称
    public NPGTextureIndex icon;// 发送者图标
}

public class GSOMailSenderRefSet : _TALSOBasicRefSet<GMailSenderRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/mail_refdata.unity3d"; } }
    public static string objName { get { return "mail_sender"; } }
}
