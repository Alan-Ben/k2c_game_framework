
using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoChildStepUpSuccess : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;       
        [ALHeader("毕业文本")]
        public Text txtStepUpDesc;
        [ALHeader("背景图片")]
        public RawImage imgStepBg;
        [ALHeader("今日不再提示")]
        public NPGGUIMonoCommonToggleEx monoDontShowToday;
        [ALHeader("确定按钮")]
        public GameObject btnConfirm;
        [ALHeader("子嗣的信息")]
        public GGUIMonoChildInfo monoChildInfo;
        [ALHeader("顺序配置每个阶段要显示的内容")]
        public List<GGUIMonoChildMainStepShow> listPhaseShow;

        public void setStep(int _step)
        {
            _step -= 1;

            for (int i = 0; i < listPhaseShow.Count; i++)
                ALUGUICommon.setGameObjEnable(listPhaseShow[i].showList, false);
            
            if (_step >= 0 && _step < listPhaseShow.Count)
                ALUGUICommon.setGameObjEnable(listPhaseShow[_step].showList, true);
        }
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1209); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1209); } }
    }
}