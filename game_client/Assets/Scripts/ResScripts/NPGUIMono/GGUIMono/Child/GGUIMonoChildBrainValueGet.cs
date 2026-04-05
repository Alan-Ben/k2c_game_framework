
using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoChildBrainValueGet : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("换取脑力的物品")]
        public NPGGUIMonoCommonItem monoBrainGetItem;
        [ALHeader("选择使用数量的按钮")]
        public GameObject btnUse;
        [ALHeader("子嗣的信息")]
        public GGUIMonoChildInfo monoChildInfo;
        [ALHeader("顺序配置每个阶段子嗣要显示的内容")]
        public List<GGUIMonoChildMainStepShow> listPhaseShow;
        [ALHeader("当前脑力值和目标脑力值")]
        public Text txtEnergyValue;
        [ALHeader("补满当前子嗣的点击按钮")]
        public GameObject btnClickForOne;
        [ALHeader("补满当前子嗣的消耗道具")]
        public NPGGUIMonoCommonItem monoCostItemForOne;
        [ALHeader("补满所有子嗣的点击按钮")]
        public GameObject btnClickForAll;
        [ALHeader("不满所有子嗣的消耗道具")]
        public NPGGUIMonoCommonItem monoCostItemForAll;
        

        public void setStep(int _step)
        {
            for (int i = 0; i < listPhaseShow.Count; i++)
                ALUGUICommon.setGameObjEnable(listPhaseShow[i].showList, false);
            
            if (_step >= 0 && _step < listPhaseShow.Count)
                ALUGUICommon.setGameObjEnable(listPhaseShow[_step].showList, true);
        }
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1208); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1208); } }
    }
}