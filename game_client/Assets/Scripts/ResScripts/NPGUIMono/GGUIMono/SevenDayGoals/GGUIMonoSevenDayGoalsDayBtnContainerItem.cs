using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoSevenDayGoalsDayBtnContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("天数的文字")]
        public Text txtDay;
        public Text txtDayGray;
        public Text txtDayWhite;
        [ALHeader("解锁和未解锁时显示的内容")]
        public List<GameObject> listUnlockShow;
        public List<GameObject> listLockShow;
        [ALHeader("选中时显示的内容")]
        public List<GameObject> listSelectShow;
        [ALHeader("有红点时显示的内容")]
        public List<GameObject> listRedShow;
        [ALHeader("点击按钮")]
        public GameObject btnClick;
    }
}