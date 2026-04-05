using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 前往火星主界面
    /// </summary>
    public class GGUIMonoMarsGoTo : _ANPBasicUIWndResBarMono
    {
        [ALHeader("背景显示")]
        public GGUIMonoCommonShowCase monoBgShowcase;
        [ALHeader("节点名称")]
        public Text txtName;
        [ALHeader("节点描述")]
        public Text txtDesc;
        [ALHeader("日志标题")]
        public Text txtLogTitle;
        [ALHeader("累积航行时间")]
        public Text txtNavigationTime;
        [ALHeader("日志内容")]
        public Text txtLogContent;
        [ALHeader("日志详情按钮")]
        public GameObject btnLogDetail;
        [ALHeader("到达新节点确认按钮")]
        public GameObject btnArriveConfirm;
        [ALHeader("倒计时组件")]
        public NPGGUIMonoCommonCountDown monoCountDown;
        [ALHeader("处理到达新阶段表现流程时需要显示GO列表")]
        public List<GameObject> goDealingNewStageShowList;
        [ALHeader("处理到达新阶段表现流程时需要隐藏GO列表")]
        public List<GameObject> goDealingNewStageHideList;
        [ALHeader("留言附加窗口")]
        public GGUIMonoMarsGoToSubMsg monoSubMsg;
        [ALHeader("抵达火星总时间文本")]
        public Text txtArriveTotalTime;
        [ALHeader("加速提示按钮")]
        public GameObject btnSpeedUpTip;
        [ALHeader("航行加速描述文本")]
        public Text txtSpeedUpDesc;
        [ALHeader("有加速时需要显示的GO列表")]
        public List<GameObject> goSpeedUpShowList;
        [ALHeader("有加速时需要隐藏的GO列表")]
        public List<GameObject> goSpeedUpHideList;


        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7002); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7002); } }
    }
}