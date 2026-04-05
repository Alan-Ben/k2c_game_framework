using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class _AGGUIMonoCommonDispatchEvent : _AGGUIMonoCommonSimpleEvent
    {
        [ALHeader("事件icon")]
        public RawImage eventIcon;
        
        [ALHeader("条件Container")]
        public GGUIMonoConditionDescContainer monoCondContainer;

        [ALHeader("派遣按钮")]
        public GameObject btnDispatch;

        [ALHeader("有已选择的大臣时显示Go列表")]
        public List<GameObject> hasSelectHeroShowGoList;
        [ALHeader("有已选择的大臣时隐藏Go列表")]
        public List<GameObject> hasSelectHeroHideGoList;
        
        [ALHeader("一键派遣")]
        public GameObject btnAkeyDispatch;
        [ALHeader("一键处理解锁时显示Go列表")]
        public List<GameObject> akeyDealUnlockShowGoList;
        [ALHeader("一键处理未解锁时显示Go列表")]
        public List<GameObject> akeyDealLockShowGoList;
        [ALHeader("一键处理未解锁时置灰列表")]
        public List<MaskableGraphic> akeyDealLockGrayList;
        
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
    }
}