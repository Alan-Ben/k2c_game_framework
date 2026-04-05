using System;
using System.Collections.Generic;
using ALPackage;
using GOE;
using NPEnum;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class NPGUIQualityAnimData
{
    public EQuality quality;//品质
    public string animName;//品质对应动画触发字名字
}

/// <summary>
/// 物品控件组合，包含物品显示常用需求
/// </summary>
public class NPGGUIMonoCommonItem : _TALUGUIMonoGridItem
{
    [ALHeader("物品点击按钮")]
    public GameObject btnItem;

    [ALHeader("物品长按按钮")]
    public GameObject longPressBtn;

    [ALHeader("窗体动画")]
    public Animation anim;

    [ALHeader("数量为0时是否需要隐藏数量文本")]
    public bool zeroHideTxtNum;

    [ALHeader("数量")]
    public Text txtNum;

    [ALHeader("数量使用的key")]
    public string txtNumKey;

    [ALHeader("数量背景框")]
    public GameObject txtNumImgGo;

    [ALHeader("图片")]
    public RawImage texIcon;

    [ALHeader("物品描述")]
    public Text itemDescTxt;

    [ALHeader("品质")]
    public Image quality;
    [ALHeader("是否需要加载品质特效")]
    public bool needQualitySfx;
    [ALHeader("品质特效父节点")]
    public Transform qualitySfxParent;
    
    [ALHeader("名称")]
    public Text txtName;

    [ALHeader("过期物品需要显示的GO列表")]
    public List<GameObject> goExpireShowList;

    [ALHeader("品质对应的动画名字配置列表")]
    public List<NPGUIQualityAnimData> qualityAnimList;


    [ALHeader("物品详情偏移值")]
    public float detailInterval;

    [ALHeader("点击时是否显示物品详情")]
    public bool isClickShowDetail = false;

    [ALHeader("长按时是否显示物品详情")]
    public bool isLongPressShowDetail = false;
    
    [ALHeader("是否详情要跳转到具体系统页面")]
    public bool isDetailJumpTo = false;
    
    [ALHeader("大数字是否缩写")]
    public bool showLargeNum = false;

    [ALHeader("是否显示已拥有的数量")]
    public bool showHasItemNum = false;

    [ALHeader("物品数量不足时是否改变文字颜色，如果勾选显示已拥有数量，只会改变已拥有数量的文本颜色")]
    public bool useNotEnoughTxtColor = false;

    [ALHeader("物品数量不足时的文字颜色")]
    public Color notEnoughTxtColor = Color.red;


    [ALHeader("是否显示可以被合成时的Go List")]
    public bool isShowCanBeCombined;
    [ALHeader("可以被合成时显示的Go List")]
    public List<GameObject> canBeCombinedShowGoList;

    /// <summary>
    /// 获取品质对应的动画名字
    /// </summary>
    /// <param name="_quality"></param>
    /// <returns></returns>
    public string getQualityAnimName(EQuality _quality)
    {
        if (qualityAnimList == null)
            return "";
        NPGUIQualityAnimData tempAnimData = null;
        for (int i = 0; i < qualityAnimList.Count; ++i)
        {
            tempAnimData = qualityAnimList[i];
            if (tempAnimData == null)
                continue;

            if (tempAnimData.quality == _quality)
                return tempAnimData.animName;
        }
        return "";
    }
}

