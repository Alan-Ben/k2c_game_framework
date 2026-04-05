using UnityEngine;
using System.Collections.Generic;
using ALPackage;
using UnityEngine.UI;
using GOE;
using TMPro;


/// <summary>
/// 玩家头像框
/// </summary>
public class NPGGUIMonoPlayerIcon : _AALBasicUIWndMono {

    [ALHeader("非玩家本人的头像是否需要翻转")]
    public bool isOverturnOtherPlayerIcon = false;
    
    [ALHeader("头像")]
    public RawImage imgIcon;

    [ALHeader("头像挂载父节点")]
    public Transform iconParPos;

    [ALHeader("头像框")]
    public RawImage imgIconBgk;

    [ALHeader("头像框挂载父节点")]
    public Transform iconBgkParPos;
    
    [ALHeader("半身像")]
    public RawImage imgCard;

    [ALHeader("称号")]
    public GGUIMonoSubPlayerTitle monoSubPlayerTitle;
    [ALHeader("没有或隐藏称号时需要隐藏的GO列表")]
    public List<GameObject> goNoOrPrivateTitleHideList;

    [ALHeader("等级")]
    public GGUIMonoPlayerLv playerLvMono;

    [ALHeader("名字，前面带服务器名")]
    public Text txtNameWithServer;
    [ALHeader("名字")]
    public Text txtName;
    [ALHeader("名字TMP")]
    public TextMeshProUGUI txtNameTmp;
    
    [ALHeader("名字需要使用的key")]
    public string txtNameKey;

    [ALHeader("id")]
    public Text txtId;

    [ALHeader("服务器信息")]
    public Text txtServer;
    [ALHeader("服务器信息不带翻译key")]
    public Text txtServerEx;

    [ALHeader("vip 等级")]
    public Text txtVipLv;

    [ALHeader("vip等级为0时 需要隐藏的GoList")]
    public List<GameObject> zeroVipHideGoList;
    [ALHeader("vip等级为0时 需要置灰的列表")]
    public List<MaskableGraphic> zeroVipGrayList;

    [ALHeader("联盟名称")]
    public Text txtUnion;

    [ALHeader("赚速（国力值）")]
    public Text txtPower;

    [ALHeader("纯数字赚速（国力值）")]
    public Text numPower;
    [ALHeader("赚速（国力）详情按钮")]
    public GameObject btnEarningsDetail;
    [ALHeader("赚速（国力）详情tooltip偏移量")]
    public Vector2 earningDetailToolTipInterval;

    [ALHeader("纯数字总亲密度")]
    public Text numIntimacy;

    [ALHeader("总亲密度")]
    public Text txtIntimacy;

    [ALHeader("纯数字伙伴总实力（总战力）")]
    public Text numFight;

    [ALHeader("伙伴总实力（总战力）")]
    public Text txtFight;

    [ALHeader("动画")]
    public Animation anim;

    [ALHeader("点击按钮")]
    public GameObject btnClick;
    
    [ALHeader("点击后定位用transform，主要用于个人信息卡片的定位")]
    public RectTransform locateRectTrans;

    [ALHeader("点击跳转玩家详情按钮")]
    public GameObject btnToPlayerDetail;
    
    [ALHeader("在线时显示")]
    public List<GameObject> onlineShowList;
    [ALHeader("离线时显示")]
    public List<GameObject> offlineShowList;
}
