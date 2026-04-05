using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 通用事件-奖励事件表现表
    /// </summary>
    [Serializable]
    public class CommonEventAwardShowRefObj : _IALBasicRefObj
    {
        public long _refId { get { return award_show_id; } }

        public long award_show_id;//表现id
        public string event_name;//事件名称
        public string event_simple_desc;//事件简易描述
        public NPGTextureIndex event_list_icon;//在事件列表中icon
        public string event_detail_desc;//事件详细描述
        public string event_result_title;//事件处理结果标题
        public string event_result_desc;//事件处理结果描述
        public NPGTextureIndex detail_icon;//详情icon图
        public long ui_res_id;//窗口资源id
    }

    public class GSOCommonEventAwardShowRefSet : _TALSOBasicRefSet<CommonEventAwardShowRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return GSORefSetAssetPath.CommonEventRefSetAssetPath; } }
        public static string objName { get { return "common_event_award_show"; } }
    }
}