using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class GGUIMonoConsortMomentDetail : _AALBasicUIWndMono
{
    [ALHeader("关闭发送评论按钮")]
    public GameObject btnClose;
    public Transform contentRoot; 
    [ALHeader("点击评论显示列表")]
    public List<GameObject> openCommentShowList;
    [ALHeader("发送评论按钮")]
    public GameObject btnSendComment;
    [ALHeader("评论输入框")]
    public InputFieldEmoji inputComment;
    [ALHeader("关闭发送评论按钮")]
    public GameObject btnCloseComment;
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(6213); } }
    public static string objName { get { return UIResPathAssistant.getObjName(6213);} }
}