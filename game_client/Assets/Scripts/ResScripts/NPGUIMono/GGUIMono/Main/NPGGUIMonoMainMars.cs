using UnityEngine;
using System.Collections;
using ALPackage;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using GOE;
using NPEnum;

/// <summary>
/// 火星窗口
/// </summary>
public class NPGGUIMonoMainMars : _ANPBasicUIWndResBarMono
{
    [ALHeader("基地图标")]
    public RawImage imgHomeIcon;
    [ALHeader("基地等级")] 
    public Text txtHomeLevel;
    [ALHeader("人口数量")]
    public Text txtPeopleNum;
    [ALHeader("人口数量key(两个参数 1.当前居民数 2.居民上限)")]
    public string txtPeopleNumKey;
    [ALHeader("人口详情按钮")]
    public GameObject btnPeopleDetail;
    [ALHeader("火星实力")]
    public Text txtMarsPower;
    [ALHeader("氧气显示部分")]
    public List<GGUIMainMarsOxygenShow> listOxygenShow;
    [ALHeader("火星能量产出详情")]
    public GameObject btnEnergyYieldDetail;
    [ALHeader("聊天入口小窗")]
    public NPGGUIMonoMiniChat chatMiniWndMono;

    [ALHeader("火星事件buff列表")]
    public GGUIMonoMarsEventBuffItemContainer monoMarsEventBuffItemContainer;
    [ALHeader("火星事件提示")]
    public GGUISubMonoMarsEventTip monoMarsEventTip;
    [ALHeader("建造队列")]
    public GGUIMonoMarsBuildQueue monoBuildQueue;

    [ALHeader("AI控制按钮")]
    public GameObject btnAIControl;
    [ALHeader("探索按钮")]
    public GameObject btnExplore;
    [ALHeader("火星实力详情按钮")]
    public GameObject btnPowerDetail;
    [ALHeader("火星实力详情tooltip偏移量")]
    public Vector2 powerDetailToolTipInterval;

    public void setOxygenValue(long _value)
    {
        if (listOxygenShow is not { Count: > 0 })
            return;
        
        foreach (GGUIMainMarsOxygenShow data in listOxygenShow)
        {
            if (data == null)
                continue;
            
            ALUGUICommon.setGameObjEnable(data.listShow, false);
        }
        
        foreach (GGUIMainMarsOxygenShow data in listOxygenShow)
        {
            if (data?.range == null || !data.range.inRange(_value))
                continue;
            
            ALUGUICommon.setGameObjEnable(data.listShow, true);
        }
    }
    
    
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(7101); } }
    public static string objName { get { return UIResPathAssistant.getObjName(7101);} }
}

[Serializable]
public class GGUIMainMarsOxygenShow
{
    public WCGIntRange range;
    public List<GameObject> listShow;
}
