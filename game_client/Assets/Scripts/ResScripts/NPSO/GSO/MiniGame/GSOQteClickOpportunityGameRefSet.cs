using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// Qte点击时机小游戏表
    /// </summary>
    [Serializable]
    public class QteClickOpportunityGameRefObj : _AMiniGameSubRefObj
    {
        public NPCommonAssetPathInfo ui_prefab_path;//ui加载预制路径
        public float on_success_quit_delay_time;//当成功时, 退出游戏延时
    }
    
    public class GSOQteClickOpportunityGameRefSet : _TALSOBasicRefSet<QteClickOpportunityGameRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return GSORefSetAssetPath.MiniGameRefSetAssetPath; } }
        public static string objName { get { return "qte_click_opportunity_game"; } }
    }
}