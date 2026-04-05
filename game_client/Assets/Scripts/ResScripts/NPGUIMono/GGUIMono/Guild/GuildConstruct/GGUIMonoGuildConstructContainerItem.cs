using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟建设列表item
    /// </summary>
    public class GGUIMonoGuildConstructContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("建设道具/金币图标")]
        public RawImage imgIcon;
        [ALHeader("品质框")]
        public Image imgQuality;
        [ALHeader("名称")]
        public Text txtName;
        [ALHeader("消耗道具")]
        public NPGGUIMonoCommonItem monoCostItem;
        [ALHeader("点击建设按钮")]
        public GameObject btnClick;
        [ALHeader("联盟经验")]
        public Text txtExp;
        [ALHeader("联盟财富")]
        public Text txtWealth;
        [ALHeader("联盟币")]
        public Text txtCoin;
        [ALHeader("当前捐赠类型需要显示的GO列表")]
        public List<GameObject> goCurConstructShowList;
        [ALHeader("免费需要显示的GO列表")]
        public List<GameObject> goFreeShowList;
        [ALHeader("免费需要隐藏的GO列表")]
        public List<GameObject> goFreeHideList;
        [ALHeader("捐赠完成需要显示的GO列表")]
        public List<GameObject> goDoneShowList;
        [ALHeader("捐赠完成需要隐藏的GO列表")]
        public List<GameObject> goDoneHideList;
    }
}