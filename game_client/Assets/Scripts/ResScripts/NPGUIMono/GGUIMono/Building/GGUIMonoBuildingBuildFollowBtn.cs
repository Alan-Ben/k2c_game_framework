
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoBuildingBuildFollowBtn : ALGGUIMonoCommonFollowItem
    {
        [ALHeader("建造按钮")]
        public GameObject btnBuild;
        [ALHeader("解锁提示")]
        public Text txtUnlockTip;
        [ALHeader("已解锁时展示和隐藏的内容")]
        public List<GameObject> listUnlockShow;
        public List<GameObject> listUnlockHide;


        public void setUnlock(bool _isUnlock)
        {
            ALUGUICommon.setGameObjEnable(listUnlockShow, false);
            ALUGUICommon.setGameObjEnable(listUnlockHide, false);
            ALUGUICommon.setGameObjEnable(_isUnlock ? listUnlockShow : listUnlockHide, true);
        }
    }
}