using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

public enum EDinnerInvitePageState
{
    InviteSever,
    InviteFriend,
    InviteGuild,
}
/// <summary>
/// 宴会邀请弹窗
/// </summary>
public class GGUIMonoDinnerInviteMain : _AALBasicUIWndMono
{
    [ALHeader("页签父物体")]
    public Transform pageParent;
    [ALHeader("邀请在线玩家列表请求数量")]
    public int serverInviteCount = 5;
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    [ALHeader("关闭按钮2")]
    public GameObject btnClose2;
    [ALHeader("邀请服务器推荐玩家")]
    public NPGGUIMonoCommonTab tabInviteServer;
    [ALHeader("邀请好友")]
    public NPGGUIMonoCommonTab tabInviteFriend;
    [ALHeader("邀请联盟玩家")]
    public NPGGUIMonoCommonTab tabInviteGuild;
    [ALHeader("不同状态配置")]
    public List<NPCommonEnumStatInfo<EDinnerInvitePageState>> statInfos;

    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(2905); } }
    public static string objName { get { return UIResPathAssistant.getObjName(2905);} }
}