using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 使用和合成道具弹窗
    /// </summary>
    public class GGUIMonoBagPopItemUseAndConvertSimple : _AALBasicUIWndMono
    {
        [ALHeader("名称")]
        public Text itemName;
        [ALHeader("描述")]
        public Text itemDesc;
        [ALHeader("使用条件父节点 当使用条件为空时可以隐藏")]
        public GameObject itemUseDescParent;
        [ALHeader("使用条件")]
        public Text itemUseDesc;

        [ALInfo("====使用道具配置====")]
        [ALHeader("使用按钮")]
        public GameObject btnUse;
        [ALHeader("使用物品不足按钮")]
        public GameObject btnUseNotEnough;
        [ALHeader("使用物品个数不足或者消耗物品个数不足需要置灰的列表")]
        public List<MaskableGraphic> useGrayImgList;
        [ALHeader("使用物品不足时需要显示的GO列表")]
        public List<GameObject> goUseItemNotEnoughShowList;
        [ALHeader("使用物品不足时需要隐藏的GO列表")]
        public List<GameObject> goUseItemNotEnoughHideList;

        [ALInfo("====合成道具配置====")]
        [ALHeader("合成按钮")]
        public GameObject btnCombine;
        [ALHeader("合成物品不足按钮")]
        public GameObject btnCombineNotEnough; 
        [ALHeader("合成数量不足时需要置灰的列表")]
        public List<MaskableGraphic> notCombineEnoughGrayList;
        [ALHeader("合成物品不足时需要显示的GO列表")]
        public List<GameObject> goCombineItemNotEnoughShowList;
        [ALHeader("合成物品不足时需要隐藏的GO列表")]
        public List<GameObject> goCombineItemNotEnoughHideList;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1923); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1923); } }
    }
}