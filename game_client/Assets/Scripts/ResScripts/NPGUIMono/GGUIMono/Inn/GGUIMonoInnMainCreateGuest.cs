using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoInnMainCreateGuest : _AALBasicUIWndMono
    {
        [ALHeader("迎宾按钮")]
        public GameObject btnCreateGuest;
        [ALHeader("当前排队人数相关")]
        public Text txtPersonNum;
        public List<GameObject> listNoPersonHide;
        public Transform transPersonAddTipPos;
        public long personAddTipId = 1;
        public Animation animPersonAdd;
        public string animNamePersonAdd;
        [ALHeader("迎宾次数相关内容")]
        public GGUIMonoCommonLazyCDCountResume monoLazyCD;
        [ALHeader("一键迎宾的勾选框和未解锁时展示的对象")]
        public NPGGUIMonoCommonToggleEx monoOneKeyToggle;
        public List<GameObject> listOneKeyLockShow;
        [ALHeader("有特殊客人时的效果配置")]
        public string specialGuestEffectStr;
    }
}