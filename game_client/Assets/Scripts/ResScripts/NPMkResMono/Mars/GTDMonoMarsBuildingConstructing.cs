using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GTDMonoMarsBuildingConstructing : MonoBehaviour
    {
        [ALHeader("时间跟随的目标")]
        public Transform timeFollowTarget;
        [ALHeader("点击区域")]
        public GTDCommonPosClickMono monoClick;
        [ALHeader("选中和未选中显示的对象")]
        public List<GameObject> listSelectShow;
        public List<GameObject> listUnSelectShow;
        
        [ALHeader("智能控制效果显示")]
        public MonoMarsIntelligentControlEffectShow monoMarsIntelligentControlEffectShow;
        
        public void setSelectState(bool _isSelect)
        {
            ALUGUICommon.setGameObjEnable(listSelectShow, false);
            ALUGUICommon.setGameObjEnable(listUnSelectShow, false);
            ALUGUICommon.setGameObjEnable(_isSelect ? listSelectShow : listUnSelectShow, true);
        }
    }
}