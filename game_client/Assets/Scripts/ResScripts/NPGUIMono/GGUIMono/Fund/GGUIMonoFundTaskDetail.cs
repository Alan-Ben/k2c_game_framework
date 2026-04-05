using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoFundTaskDetail : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("刷新时间文本")]
        public Text txtRefreshTime;
        [ALHeader("任务详情列表")]
        public GGUIMonoFundTaskDetailGrid monoGrid;
        [ALHeader("加载中的显示和隐藏内容")]
        public List<GameObject> listLoadingShow;
        public List<GameObject> listLoadingHide;
        
        
        public void setLoadingState(bool _loading)
        {
            ALUGUICommon.setGameObjEnable(listLoadingShow, false);
            ALUGUICommon.setGameObjEnable(listLoadingHide, false);
            ALUGUICommon.setGameObjEnable(_loading ? listLoadingShow : listLoadingHide, true);
        }
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(8304); } }
        public static string objName { get { return UIResPathAssistant.getObjName(8304); } }
    }
}