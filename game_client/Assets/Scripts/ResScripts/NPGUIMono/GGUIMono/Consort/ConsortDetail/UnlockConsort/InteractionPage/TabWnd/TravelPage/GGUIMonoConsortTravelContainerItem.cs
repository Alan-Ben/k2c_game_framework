using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoConsortTravelContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("出游名称")]
        public TextEx txtTravelName;

        [ALHeader("出游背景")]
        public RawImage imgTravelBanner;
        
        [ALHeader("出游描述")]
        public TextEx txtTravelDesc;

        [ALHeader("获取加护值")]
        public TextEx txtGainCharm;
        [ALHeader("获取加护值key")]
        public string gainGainCharmKey;
        
        [ALHeader("获取加护点倍数")]
        public TextEx txtGainCharmPointMultiple;
        [ALHeader("获取加护点倍数key")]
        public string gainGainCharmPointMultipleKey;
        
        [ALHeader("这种出游方式支持折扣时显示")]
        public List<GameObject> supportDiscountShow;
        [ALHeader("这种出游方式不支持折扣时显示")]
        public List<GameObject> nonsupportDiscountShow;
        
        [ALHeader("折扣信息按钮")]
        public GameObject btnDiscountInfo;
        [ALHeader("折扣信息tooltip资源id")]
        public long discountToolTipUiResId;
        [ALHeader("折扣信息tooltip间隔")]
        public Vector2 discountToolTipInterval;
        
        // [ALHeader("有剩余折扣次数时显示(这个是基于上面支持折扣的基础上的显示)")]
        // public List<GameObject> hasLeftDiscountShow;
        // [ALHeader("没有剩余折扣次数时显示(这个是基于上面支持折扣的基础上的显示)")]
        // public List<GameObject> noLeftDiscountShow;

        [ALHeader("本次出游有折扣时显示")]
        public List<GameObject> thisTimeTravelHasDiscountShow;
        [ALHeader("本次出游没有折扣时显示")]
        public List<GameObject> thisTimeTravelNoDiscountShow;
        
        [ALHeader("折扣百分比")]
        public TextEx txtDiscount;
        [ALHeader("折扣百分比key")]
        public string discountKey;
        
        [ALHeader("消耗道具")]
        public NPGGUIMonoCommonItem monoCostItem;

        [ALHeader("出游按钮")]
        public GameObject travelBtn;
        [ALHeader("出游按钮描述")]
        public TextEx travelBtnDesc;
    }
}