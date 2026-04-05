using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 活动通用页签配置脚本
    /// </summary>
    public class GGUIMonoActivityCommonTab : MonoBehaviour
    {
        [ALHeader("页签类型")]
        public string tabTypeStr;
        [ALHeader("通用页签脚本")]
        public NPGGUIMonoCommonTab monoTab;
        [ALHeader("页签对应的子窗口资源id")]
        public long tabSubWndAssetId;
    }
}