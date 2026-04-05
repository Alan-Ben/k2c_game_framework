using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟委托处理记录
    /// </summary>
    public class GGUIMonoGuildEntrustRecord : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        
        [ALHeader("委托处理记录itemGrid")]
        public GGUIMonoGuildEntrustRecordItemGrid monoGuildEntrustRecordItemGrid;

        [ALHeader("自身排行")]
        public TextEx txtSelfRank;
        [ALHeader("自身排行Key")]
        public string txtSelfRankKey;
        
        [ALHeader("自身委托处理次数")]
        public TextEx txtSelfEntrustDealCount;
        [ALHeader("自身委托处理次数Key")]
        public string txtSelfEntrustDealCountKey;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(4920); } }
        public static string objName { get { return UIResPathAssistant.getObjName(4920); } }
    }
}