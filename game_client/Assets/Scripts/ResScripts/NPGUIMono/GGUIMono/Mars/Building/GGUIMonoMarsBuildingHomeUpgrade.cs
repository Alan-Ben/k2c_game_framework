using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoMarsBuildingHomeUpgrade : GGUIMonoMarsBuildingUpgrade
    {
        [ALHeader("消耗变化和语言 key ")]
        public CommonUpgradePropertyShow<Text> txtConsume;
        public string txtConsumeKey;
        [ALHeader("火星币产出变化和语言 key ")]
        public CommonUpgradePropertyShow<Text> txtOutput;
        public string txtOutputKey;
        [ALHeader("实力变化和语言 key ")]
        public CommonUpgradePropertyShow<Text> txtPower;
        public string txtPowerKey;


        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7115); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7115); } }
    }
}