using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// item
/// </summary>
public class GGUIMonoDinnerMainJoinerItem : _AALBasicUIWndMono
{ 
    [ALHeader("点击按钮")]
    public GameObject btnClick;
    [ALHeader("名字")]
    public Text textName;
    [ALHeader("宴会人气")]
    public Text txtPopularity;
    [ALHeader("宴会人气2")]
    public Text txtPopularity2;
    [ALHeader("玩家信息")]
    public NPGGUIMonoPlayerIcon playerIcon;
    [ALHeader("大臣信息")]
    public GGUIMonoHeroIconItem heroIcon;
    [ALHeader("玩家详情加载的父节点")]
    public RectTransform playerDetailParent;
    [ALHeader("好友需要显示的go")]
    public List<GameObject> friendShowGos;
    [ALHeader("待加入需要显示的go")]
    public List<GameObject> waitJoinShowGos;
    [ALHeader("有人加入需要显示的go")]
    public List<GameObject> waitJoinHideGos;
    [ALHeader("玩家自己需要显示的go")]
    public List<GameObject> selfJoinShowGos;
    
    [ALHeader("气泡文本")]
    public Text txtTalk;
    [ALHeader("气泡动画")]
    public Animation talkAnim;
    [ALHeader("气泡动画显示动画名")]
    public string talkShowAnimName;
    
    [ALHeader("Item显示动画")]
    public Animation showAnim;
    [ALHeader("其他玩家显示动画名")]
    public string showAnimName;
    [ALHeader("玩家本人显示动画名")]
    public string playerSelfShowAnimName;
}

