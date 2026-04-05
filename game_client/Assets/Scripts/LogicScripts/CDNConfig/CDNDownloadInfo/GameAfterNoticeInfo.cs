using System;

namespace GOE
{
    /// <summary>
    /// 游戏内公告信息
    /// </summary>
    [Serializable]
    public class GameAfterNoticeInfo : BaseNoticeInfo
    {
        public int is_show_wnd_after;
        public string mono_asset_path_after;//窗口路径
        public string mono_obj_name_after;//窗口名称
        
        public override string ToString()
        {
            return $"[{nameof(is_show_wnd_after)}: {is_show_wnd_after}], [{nameof(mono_asset_path_after)}: {mono_asset_path_after}], [{nameof(mono_obj_name_after)}: {mono_obj_name_after}]";
        }
    }
}