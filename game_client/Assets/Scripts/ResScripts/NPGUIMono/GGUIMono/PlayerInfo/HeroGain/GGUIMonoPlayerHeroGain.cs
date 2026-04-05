using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoPlayerHeroGain : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("英雄获取列表")] 
        public GGUIMonoPlayerHeroGainGrid monoHeroGrid;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1715); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1715); } }
    }
}