using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

public enum EDinnerPlayerStat
{
    OWNER,//举办者
    CANNOTJOIN,//不可以参加
    CANJOIN,//可以参加
}


/// <summary>
/// 宴会详情主界面
/// </summary>
public class GGUIMonoDinnerMain : _AALBasicUIWndMono
{
    public Transform videoRoot;
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    [ALHeader("邀请")]
    public GameObject btnInvite;
    [ALHeader("宴会日志按钮")]
    public GameObject btnLog;
    [ALHeader("开宴玩家信息")]
    public NPGGUIMonoPlayerIcon playerInfo;
    [ALHeader("宴会名称")]
    public Text txtDinnerName;
    [ALHeader("宴会人气")]
    public Text txtDinnerScore;
    [ALHeader("宴会席位")]
    public Text txtDinnerSeat;
    [ALHeader("宴会倒计时")]
    public List<Text> txtDinnerTimeDown;
    [ALHeader("宴会日志mini文本")]
    public Text txtLogMini;
    
    // 参加宴会信息
    [ALHeader("加入宴会按钮")]
    public GameObject btnJoin;
    [ALHeader("快速加入宴会toggle")]
    public NPGGUIMonoCommonToggleEx toggleQuickJoin;
    [ALHeader("上一个宴会按钮")]
    public GameObject btnLast;
    [ALHeader("下一个宴会按钮")]
    public GameObject btnNext;
    
    [ALHeader("不同状态显示配置")]
    public List<NPCommonEnumStatInfo<EDinnerPlayerStat>> statInfos;
    [ALHeader("宴会详情刷新频率（秒）")]
    public float refreshContentTimes = 2f;
    [ALHeader("妃子头像")]
    public GGUIMonoConsortIconItem consortIconItem;
    [ALHeader("妃子宴会需要显示的go")]
    public List<GameObject> consortDinnerShowGos;
    [ALHeader("不同宴会类型界面表现区分用的动画")]
    public Animation showTypeAnim;
    [ALHeader("默认显示类型动画名")]
    public string defaultAnimName;
    [ALHeader("妃子宴会类型动画名")]
    public string consortAnimName;
    [ALHeader("子嗣头像（子嗣庆功宴）")]
    public GGUIMonoChildInfo childIconItem;
    [ALHeader("子嗣庆功宴需要显示的go")]
    public List<GameObject> childCeleDinnerShowGos;
    [ALHeader("子嗣庆功宴类型动画名")]
    public string childCeleAnimName;
    
    [ALHeader("加入者列表容器")]
    public GGUIMonoDinnerMainJoinerItemContainer joinerItemContainer;
    [ALHeader("欢迎文本")]
    public Text txtWelcome;
    [ALHeader("欢迎动画")]
    public Animation welcomeAnim;
    [ALHeader("欢迎动画显示动画名")]
    public string welcomeShowAnimName;
    public WCGFloatRange randomTalkShowTimeRange = new WCGFloatRange(0.5f, 2.5f);

    [ALHeader("宴会类型文本颜色控制")]
    public List<Graphic> txtColorGraphics;
    [ALHeader("宴会类型背景颜色控制")]
    public List<Graphic> bgColorGraphics;
    [ALHeader("当前勾选的快速加入消耗")]
    public NPGGUIMonoCommonItem curQuickJoinCostItem;
    [ALHeader("满员显示隐藏go列表")]
    public List<GameObject> goShowInSeatFullList;
    public List<GameObject> goHideInSeatFullList;

    public GGUIMonoDinnerVideo videoMono1;
    public GGUIMonoDinnerVideo videoMono2;
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(2903); } }
    public static string objName { get { return UIResPathAssistant.getObjName(2903);} }
}