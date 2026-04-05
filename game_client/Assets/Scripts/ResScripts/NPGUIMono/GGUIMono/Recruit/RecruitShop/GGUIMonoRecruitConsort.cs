using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 妃子招募窗口
    /// </summary>
    public class GGUIMonoRecruitConsort : _AALBasicUIWndMono
    {
        [ALHeader("妃子详细信息子窗口")]
        public GGUISubMonoConsortDetailInfo monoConsortDetailInfo;

        [ALHeader("兑换按钮")]
        public GGUISubMonoRecruitExchangeBtn monoExchangeBtn;
        
        [ALHeader("更多信息按钮")]
        public GameObject btnMoreInfo;
        
        [ALHeader("返回按钮")]
        public GameObject btnReturn;
        
        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(2705); } }
        public static string objName { get { return UIResPathAssistant.getObjName(2705);} }
    }
}