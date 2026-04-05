using UnityEngine;
using ALPackage;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class GGUIMonoMailDetail_Base : _AALBasicUIWndMono
{
    [ALHeader("关闭按钮")]
    public GameObject btnClose;

    [ALHeader("邮件标题")]
    public Text txtTitle;
    [ALHeader("邮件副标题")]
    public Text txtSubTitle;
    [ALHeader("邮件内容文本里面的链接颜色")]
    public Color urlColor = Color.blue;
    [ALHeader("邮件内容文本里面的链接是否显示下划线")]
    public bool isShowUnderLine = true;
    [ALHeader("邮件内容文本")]
    public TextExClickBaseOpenUrl txtContent;
    [ALHeader("邮件发送者")]
    public Text txtSender;
    [ALHeader("邮件发送时间")]
    public Text txtCreateTime;
    [ALHeader("邮件剩余有效期")]
    public Text txtRemainTime;

    [ALHeader("会使用动态参数带入拼凑最终显示文本的文本列表")]
    public Text[] argsTextList;

    [ALHeader("删除播放的音效id")]
    public long delAudioId = 11103;
    [ALHeader("删除按钮")]
    public GameObject btnDel;
    [ALHeader("领取按钮")]
    public GameObject btnGain;
    
    [ALHeader("必读未读完的时候变灰的对象列表")]
    public List<MaskableGraphic> grayObjectList;

    [ALHeader("收藏按钮")]
    public GameObject btnLock;
    [ALHeader("取消收藏按钮")]
    public GameObject btnUnLock;

    [ALHeader("收藏状态")]
    public GameObject goLockStat;

    [ALHeader("必读邮件提醒")]
    public GameObject goNeedRead;

    [ALHeader("邮件内容文本拖动的ScrollRect，一般是用于判断是否读到末尾")]
    public ScrollRect transScrollView;
    [ALHeader("ScrollRect到底的时候显示，否则隐藏")]
    public List<GameObject> goScrollEndShow;
    [ALHeader("ScrollRect到底的时候隐藏，否则显示")]
    public List<GameObject> goScrollEndHide;

}