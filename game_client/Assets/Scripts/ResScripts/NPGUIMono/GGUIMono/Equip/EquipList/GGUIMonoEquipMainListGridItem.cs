using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 主界面藏品列表item
    /// </summary>
    public class GGUIMonoEquipMainListGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("点击按钮")]
        public GameObject btnClick;
        [ALHeader("藏品item")]
        public GGUIMonoEquipCommonItem monoEquipItem;
        [ALHeader("设置锁定状态不可分解需要显示的GO")]
        public GameObject goSetLockShow;
        [ALHeader("技能加成值")]
        public Text txtAddValue;
        [ALHeader("在图鉴页签需要显示的GO列表")]
        public List<GameObject> goIllustratedHandbookShowList;
        [ALHeader("在图鉴页签需要隐藏的GO列表")]
        public List<GameObject> goIllustratedHandbookHideList;
        [ALHeader("已解锁（拥有或拥有过）需要显示的GO列表")]
        public List<GameObject> goUnlockShowList;
        [ALHeader("已解锁（拥有或拥有过）需要隐藏的GO列表")]
        public List<GameObject> goUnlockHideList;
    }
}