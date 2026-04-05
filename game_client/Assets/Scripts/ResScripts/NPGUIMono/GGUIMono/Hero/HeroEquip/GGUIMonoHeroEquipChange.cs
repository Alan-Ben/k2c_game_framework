using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 伙伴藏品替换界面
    /// </summary>
    public class GGUIMonoHeroEquipChange : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("藏品item")]
        public GGUIMonoEquipCommonItem monoEquipItem;
        [ALHeader("增加实力值")]
        public Text txtPower;
        [ALHeader("有佩戴藏品时需要显示的GO列表")]
        public List<GameObject> goHaveEquipShowList;
        [ALHeader("有佩戴藏品时需要隐藏的GO列表")]
        public List<GameObject> goHaveEquipHideList;
        [ALHeader("藏品列表")]
        public GGUIMonoHeroEquipChangeGrid monoEquipGrid;


        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1017); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1017); } }
    }
}