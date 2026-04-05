using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 通用事件-小游戏事件展示表
    /// </summary>
    [Serializable]
    public class CommonEventMiniGameShowRefObj : _IALBasicRefObj
    {
        public long _refId { get { return mini_game_show_id; } }
        
        public long mini_game_show_id;//表现id
        public long mini_game_main_id;//小游戏主id
        public string event_name;//事件名称
        public string event_simple_desc;//事件简易描述
        public NPGTextureIndex event_list_icon;//在事件列表中icon
        public string event_detail_desc;//事件详细描述
        public string event_result_title;//事件处理结果标题
        public string event_result_desc;//事件处理结果描述
    }
    
    public class GSOCommonEventMiniGameShowRefSet : _TALSOBasicRefSet<CommonEventMiniGameShowRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return GSORefSetAssetPath.CommonEventRefSetAssetPath; } }
        public static string objName { get { return "common_event_mini_game_show"; } }
    }
}