using ALPackage;
using System.Collections.Generic;
using GOE;
using UnityEngine;
using UnityEngine.UI;
//邮件主界面状态
public enum EMailMainStat
{
    ALL_EMPTY,//所有邮件为空
    ALL_NO_EMPTY,//所有邮件不为空
    LOCK_EMPTY,//收藏邮件为空
    LOCK_NO_EMPTY,//收藏邮件不为空
}

public class GGUIMonoMail : _AALBasicUIWndMono
{
    [ALHeader("邮件列表")]
    public GGUIMonoMailGrid monoMailGrid;//邮件列表
    
    [ALHeader("一键完成按钮")]
    public GameObject btnAkeyFinish;//一键完成

    [ALHeader("一键删除按钮")]
    public GameObject btnAkeyDel;//一键删除
    [ALHeader("删除播放的音效id")]
    public long delAudioId;
    
    [ALHeader("可以一键完成时显示物体列表")]
    public List<GameObject> canAkeyFinishShowGoList;
    [ALHeader("可以一键删除时显示物体列表")]
    public List<GameObject> canAkeyDelShowGoList;
    [ALHeader("有未读必读邮件时显示物体列表")]
    public List<GameObject> hasUnReadNeedReadMailShow;
    
    [ALHeader("邮件列表状态配置")]
    public List<NPCommonEnumStatInfo<EMailMainStat>> statInfos;

    [ALHeader("显示收藏筛选的显示")]
    public NPGGUIMonoCommonToggleEx toggleLock;//显示收藏的勾选框
    [ALHeader("选中收藏时显示")]
    public List<GameObject> isLockOnShow;
    [ALHeader("非选中收藏时显示")]
    public List<GameObject> isLockOffShow;
    [ALHeader("邮件数量显示")]
    public Text textMailCount;

    [ALHeader("关闭按钮")]
    public GameObject btnClose;

    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(3200); } }
    public static string objName { get { return UIResPathAssistant.getObjName(3200);} }
}