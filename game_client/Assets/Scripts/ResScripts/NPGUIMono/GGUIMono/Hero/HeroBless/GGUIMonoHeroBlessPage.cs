using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 伙伴信息加护页签
    /// </summary>
    public class GGUIMonoHeroBlessPage : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("家人列表")]
        public GGUIMonoHeroBlessNullableContainer monoBlessContainer;
        [ALHeader("描述")]
        public Text txtDesc;
        [ALHeader("加成值")]
        public Text txtAddPower;
        [ALHeader("前往按钮")]
        public GameObject btnGoTo;
        [ALHeader("预览按钮")]
        public GameObject btnPreview;
        [ALHeader("未解锁描述")]
        public Text txtLockDesc;
        [ALHeader("未解锁时显示的GO列表")]
        public List<GameObject> goLockShowList;
        [ALHeader("未解锁时隐藏的GO列表")]
        public List<GameObject> goLockHideList;
    }
}

