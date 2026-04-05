using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 火星基地 - ai智能控制详情页面
    /// </summary>
    public class GGUIMonoMarsIntelligentControlDetail : _AALBasicUIWndMono
    {
        [ALHeader("智能控制决策信息")]
        public GGUIMonoMarsIntelligentControlInfo intelligentControlInfo;
        
        [ALHeader("消耗道具")]
        public NPGGUIMonoCommonItem monoCostItem;
        
        [ALHeader("使用按钮")]
        public GameObject btnUse;
        [ALHeader("使用后聚焦建筑时间")]
        public float afterUseFocusBuildingTime = 0.1f;

        [ALHeader("返回按钮")]
        public GameObject btnReturn;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7204); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7204); } }
    }
}