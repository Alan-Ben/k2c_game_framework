using UnityEngine;
using System.Collections;
using ALPackage;
using System;
using System.Collections.Generic;

#if NP_GAME
using GOE;
#endif


[System.Serializable]
public class GMailRefObj : _IALBasicRefObj
{
    public long _refId {
        get {
            return id;
        }
    }

    public long id;  //邮件id
    public int sender_id;//发送者配表Id
    public int type_id;//邮件类型配表Id
    public string title;// 标题
    public string sub_title;// 副标题
    public string content;// 内容
    public NPGTextureIndex sender_icon;// 发送者图标
}

public class GSOMailRefSet : _TALSOBasicRefSet<GMailRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/mail_refdata.unity3d"; } }
    public static string objName { get { return "mail"; } }
}
