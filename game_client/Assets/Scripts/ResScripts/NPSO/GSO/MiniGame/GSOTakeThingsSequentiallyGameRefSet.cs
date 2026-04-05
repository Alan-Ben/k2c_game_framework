using System;
using ALPackage;

namespace GOE
{
    [Serializable]
    public class TakeThingsSequentiallyGameRefObj : _AMiniGameSubRefObj
    {
        public NPCommonAssetPathInfo ui_prefab_path;//ui加载预制路径
        public NPCommonAssetPathInfo td_prefab_path;//3d场景加载预制路径
        public float on_success_quit_delay_time;//当成功时, 退出游戏延时
    }
    
    public class GSOTakeThingsSequentiallyGameRefSet : _TALSOBasicRefSet<TakeThingsSequentiallyGameRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return GSORefSetAssetPath.MiniGameRefSetAssetPath; } }
        public static string objName { get { return "take_things_sequentially_game"; } }
    }
}