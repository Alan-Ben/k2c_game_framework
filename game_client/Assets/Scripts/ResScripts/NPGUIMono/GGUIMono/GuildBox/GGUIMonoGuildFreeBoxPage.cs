using System.Collections.Generic;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟宝箱-免费宝箱页面
    /// </summary>
    public class GGUIMonoGuildFreeBoxPage : GGUIMonoGuildBoxPageBase
    {
        [ALHeader("免费宝箱数量")]
        public Text txtFreeBoxCount;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(8706); } }
        public static string objName { get { return UIResPathAssistant.getObjName(8706);} }
    }
}