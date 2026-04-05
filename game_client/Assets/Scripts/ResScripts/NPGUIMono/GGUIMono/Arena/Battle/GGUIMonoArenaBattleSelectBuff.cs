using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 竞技场战斗选择增益弹窗
    /// </summary>
    public class GGUIMonoArenaBattleSelectBuff : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("增益值")]
        public Text txtBuffValue;
        [ALHeader("增益道具列表")]
        public GGUIMonoArenaBattleSelectBuffContainer monoSelectBuffContainer;
        [ALHeader("血量条")]
        public Slider sldBlood;
        [ALHeader("血量文本")]
        public Text txtHP;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(5211); } }
        public static string objName { get { return UIResPathAssistant.getObjName(5211); } }
    }
}