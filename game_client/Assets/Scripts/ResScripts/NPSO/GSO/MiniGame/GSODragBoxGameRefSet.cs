using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 拖箱子游戏表
    /// </summary>
    [Serializable]
    public class DragBoxGameRefObj : _AMiniGameSubRefObj
    {
        public NPCommonAssetPathInfo td_prefab_path;//3d场景加载预制路径
    }
    
    public class GSODragBoxGameRefSet : _TALSOBasicRefSet<DragBoxGameRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return GSORefSetAssetPath.MiniGameRefSetAssetPath; } }
        public static string objName { get { return "drag_box_game"; } }
    }
}