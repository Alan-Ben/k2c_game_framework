using System.Collections.Generic;
using ALPackage;
using TMPro;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星科技 - 升级中科技显示
    /// </summary>
    public class GGUIMonoMarsUpgradingTechnologyInfo : _AALBasicUIWndMono
    {
        [ALHeader("有升级中科技时显示的物体列表")]
        public List<GameObject> hasUpgradingTechnologyShow;
        [ALHeader("无升级中科技时显示的物体列表")]
        public List<GameObject> noUpgradingTechnologyShow;
        
        [ALHeader("科技信息子窗口")]
        public GGUIMonoMarsTechnologyInfo monoTechnologyInfo;
        
        [ALHeader("倒计时进度条")]
        public NPGGUIMonoProgress countDownProgress;
        [ALHeader("TextMeshPro倒计时文本")]
        public TMP_Text tmpCountDown;
        
        [ALHeader("加速按钮")]
        public GameObject btnSpeedUp;
        [ALHeader("取消按钮")]
        public GameObject btnCancel;
        [ALHeader("确定按钮")]
        public GameObject btnSure;
        [ALHeader("联盟互助按钮")]
        public GameObject btnAssist;
        [ALHeader("可联盟求助需要显隐藏的物体列表")]
        public List<GameObject> canAssistShowGos;
        public List<GameObject> canAssistHideGos;
        
    }
}