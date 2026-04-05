using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星基地 - ai智能控制页面
    /// </summary>
    public class GGUIMonoMarsIntelligentControl : _AALBasicUIWndMono
    {
        [ALHeader("满意值item")]
        public GGUIMonoCommonSimpleItem satisfactionValueItem;
        [ALHeader("增加满意值按钮")]
        public GameObject btnAddSatisfactionValue;

        [ALHeader("智能控制列表")]
        public GGUIMonoMarsIntelligentControlContainer intelligentControlContainer;

        [ALHeader("返回按钮")]
        public GameObject btnReturn;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7203); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7203); } }
    }
}