using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoAnecdoteEventChoice : _AALBasicUIWndMono
    {
        [ALHeader("选项标题")]
        public Text txtChoiceTitle;
        [ALHeader("选项按钮的容器")]
        public GGUIMonoAnecdoteEventChoiceContainer monoChoiceContainer;
        [ALHeader("确定按钮")] 
        public GameObject btnConfirm;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(3404); } }
        public static string objName { get { return UIResPathAssistant.getObjName(3404); } }
    }
}