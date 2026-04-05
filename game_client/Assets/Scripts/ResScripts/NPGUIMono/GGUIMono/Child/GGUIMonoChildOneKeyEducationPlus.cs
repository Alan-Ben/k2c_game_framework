using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoChildOneKeyEducationPlus : _AALBasicUIWndMono
    {
        [ALHeader("关闭界面按钮")]
        public GameObject btnBack;
        [ALHeader("开关按钮和两种情况的显隐对象")] 
        public GameObject btnOpen;
        public GameObject btnClose;
        public List<GameObject> listOpenShow;
        public List<GameObject> listCloseShow;
        [ALHeader("已解锁和未解锁展示的对象")]
        public List<GameObject> listUnlockShow;
        public List<GameObject> listLockShow;
        [ALHeader("未解锁时的提示文本")]
        public Text txtUnlockTip;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1211); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1211); } }
        
        
        public void setLockShow(bool _isLock)
        {
            ALUGUICommon.setGameObjEnable(listLockShow, false);
            ALUGUICommon.setGameObjEnable(listUnlockShow, false);
            ALUGUICommon.setGameObjEnable(_isLock ? listLockShow : listUnlockShow, true);
        }
        public void setOpenShow(bool _isOpen)
        {
            ALUGUICommon.setGameObjEnable(listOpenShow, false);
            ALUGUICommon.setGameObjEnable(listCloseShow, false);
            ALUGUICommon.setGameObjEnable(_isOpen ? listOpenShow : listCloseShow, true);
        }
    }
}