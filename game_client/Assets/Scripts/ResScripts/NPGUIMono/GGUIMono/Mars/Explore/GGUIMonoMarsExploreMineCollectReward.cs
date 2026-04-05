using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoMarsExploreMineCollectReward : _AALBasicUIWndMono
    {
        [ALHeader("采集情况描述")]
        public Text txtDesc;
        [ALHeader("奖励列表")]
        public NPGGUIMonoCommonItemContainer monoRewardItemContainer;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(0); } }
        public static string objName { get { return UIResPathAssistant.getObjName(0); } }
    }
}