using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoMarsBuildQueueBuy : _AALBasicUIWndMono
    {
        [ALHeader("临时解锁按钮")]
        public GameObject btnTempUnlock;
        [ALHeader("临时解锁的时间")]
        public Text txtTimeUnlockTime;
        [ALHeader("临时解锁的花费")]
        public NPGGUIMonoCommonItem monoTimeUnlockCose;
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("免费或正常状态下显示的内容")]
        public List<GameObject> listFreeShow;
        public List<GameObject> listNormalShow;


        public void setIsFree(bool _free)
        {
            ALUGUICommon.setGameObjEnable(listFreeShow, false);
            ALUGUICommon.setGameObjEnable(listNormalShow, false);
            ALUGUICommon.setGameObjEnable(_free ? listFreeShow : listNormalShow, true);
        }
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7123); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7123); } }
    }
}