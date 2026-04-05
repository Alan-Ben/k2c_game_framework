using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoMarsBuildingHomeUpgrading : GGUIMonoMarsBuildingUpgrading
    {
        [ALHeader("消耗变化")]
        public CommonUpgradePropertyShow<Text> txtConsume;


        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7114); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7114); } }
    }
}