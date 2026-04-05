using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoInnStationBuildBtn : ALGGUIMonoCommonFollowItem
    {
        [ALHeader("建造按钮")]
        public GameObject btnBuild;
        [ALHeader("当前需求进度")]
        public Text txtRequireProgress;
        [ALHeader("可建造和不可建造时展示的内容")]
        public List<GameObject> listBuildableShow;
        public List<GameObject> listBuildableHide;
        [ALHeader("不可建造时的置灰列表")]
        public List<MaskableGraphic> listCannotBuildGray;

        
#if NP_GAME
        public void setCanBuild(bool _canBuild)
        {
            ALUGUICommon.setGameObjEnable(listBuildableShow, false);
            ALUGUICommon.setGameObjEnable(listBuildableHide, false);
            ALUGUICommon.setGameObjEnable(_canBuild ? listBuildableShow : listBuildableHide, true);
            if (!_canBuild)
                GGameCommonInfo.grayImage(listCannotBuildGray);
            else
                GGameCommonInfo.disgrayImage(listCannotBuildGray);
        }
#endif
    }
}