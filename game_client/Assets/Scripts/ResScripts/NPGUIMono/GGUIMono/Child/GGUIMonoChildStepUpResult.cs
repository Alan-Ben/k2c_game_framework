
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoChildStepUpResult : _AALBasicUIWndMono
    {
        [ALHeader("子嗣的收益")]
        public GGUIMonoChildInfo monoChildInfo;
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("列表中随机展示一条")]
        public List<GameObject> listRandomShow;
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
        public void refreshRandomShow()
        {
            if (listRandomShow == null || listRandomShow.Count == 0)
                return;
            
            ALUGUICommon.setGameObjEnable(listRandomShow, false);
            
            int index = Random.Range(0, listRandomShow.Count);
            ALUGUICommon.setGameObjEnable(listRandomShow[index], true);
        }
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1210); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1210); } }
    }
}