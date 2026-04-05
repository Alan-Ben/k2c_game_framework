
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoAdultMarriedDetail : NPGGUIMonoCommonToolTip
    {
        [ALHeader("子嗣的数据")]
        public GGUIMonoChildInfo monoAdultInfo;  
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(2515); } }
        public static string objName { get { return UIResPathAssistant.getObjName(2515); } }      
    }
}