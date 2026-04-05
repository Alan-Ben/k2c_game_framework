using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class NPGGUIMonoRuleItemBar:_AALBasicUIWndMono
    {
        [ALHeader("item列表")]
        public NPGGUIMonoRuleListSubItemContainer itemContainer;
        [ALHeader("设置数据后需要刷新布局的内容")]
        public List<RectTransform> refreshRectTrans;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(301); } }
        public static string objName { get { return UIResPathAssistant.getObjName(301); } }
    }
}