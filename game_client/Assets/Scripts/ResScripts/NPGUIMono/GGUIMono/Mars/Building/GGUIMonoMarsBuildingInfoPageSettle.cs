using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoMarsBuildingInfoPageSettle : _AALBasicUIWndMono
    {
        [ALHeader("槽位容器")]
        public GGUIMonoMarsBuildingInfoPageSettleSlotContainer monoSlotContainer;
        [ALHeader("派遣按钮")]
        public GameObject btnSettle;
        [ALHeader("十连开关")]
        public NPGGUIMonoCommonToggleEx monoTenToggle;
        [ALHeader("是否派遣全满的展示")]
        public List<GameObject> listSettleMaxShow;
        public List<GameObject> listSettleNotMaxShow;


        public void setSettleMax(bool _isMax)
        {
            ALUGUICommon.setGameObjEnable(listSettleMaxShow, false);
            ALUGUICommon.setGameObjEnable(listSettleNotMaxShow, false);
            ALUGUICommon.setGameObjEnable(_isMax ? listSettleMaxShow : listSettleNotMaxShow, true);
        }
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7109); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7109); } }
    }
}