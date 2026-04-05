using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟派遣主窗口
    /// </summary>
    public class GGUIMonoGuildDispatchMain : _ANPBasicUIWndResBarMono
    {
        [ALHeader("派遣item列表")]
        public GGUIMonoGuildDispatchItemContainer monoDispatchItemContainer;
        
        [ALHeader("委派大臣的数量")]
        public TextEx txtDispatchNum;
        [ALHeader("委派大臣的数量Key(两个参数, 1.当前委派人数, 2.最大可委派人数)")]
        public string txtDispatchNumKey;
        
        [ALHeader("联盟收益")]
        public TextEx txtGuildEarnings;
        [ALHeader("联盟收益Key(一个参数, 1.联盟收益)")]
        public string txtGuildEarningsKey;
        
        [ALHeader("派遣按钮")]
        public GameObject btnDispatch;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(4924); } }
        public static string objName { get { return UIResPathAssistant.getObjName(4924); } }
    }
}