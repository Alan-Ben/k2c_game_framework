using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星时间立即完成确认窗口
    /// </summary>
    public class GGUIMonoMarsTimeCompleteNowConfim : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        
        [ALHeader("提示文本")]
        public TextEx txtTip; // 消耗:【{0}】来立即完成
     
        [ALHeader("今日不再提示")]
        public NPGGUIMonoCommonToggleEx monoDontShowToday;

        [ALHeader("有剩余时间时显示对象列表")]
        public List<GameObject> hasRemainTimeShowList;
        [ALHeader("剩余时间文本")]
        public TextEx txtRemainTime; // 剩余时间: {0}
        
        [ALHeader("消耗物品")]
        public NPGGUIMonoCommonItem monoCostItem;
        
        [ALHeader("立即完成按钮")]
        public GameObject btnCompleteNow;
        
        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7121); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7121); } }
    }
}