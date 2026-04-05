using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoMuseumItemList : _AALBasicUIWndMono
    {
        [ALHeader("收集进度")]
        public Text txtCollectProgress;
        [ALHeader("餐品列表")]
        public GGUIMonoMuseumItemListGrid itemGrid;
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("列表为空时的显示内容")]
        public List<GameObject> listEmptyShow;
        public List<GameObject> listEmptyHide;
        
        
#if NP_GAME
        public void setEmpty(bool _isEmpty)
        {
            ALUGUICommon.setGameObjEnable(listEmptyShow, false);
            ALUGUICommon.setGameObjEnable(listEmptyHide, false);
            ALUGUICommon.setGameObjEnable(_isEmpty ? listEmptyShow : listEmptyHide, true);
        }
#endif
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6422); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6422); } }
    }
}