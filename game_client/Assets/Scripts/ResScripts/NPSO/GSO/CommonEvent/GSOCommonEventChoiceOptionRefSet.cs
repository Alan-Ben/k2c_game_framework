using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 通用事件-选择事件选项表
    /// </summary>
    [Serializable]
    public class CommonEventChoiceOptionRefObj : _IALBasicRefObj
    {
        public long _refId { get { return option_id; } }
        
        public long option_id;//选项id
        public string option_desc;//选项描述
        public List<string> option_desc_args;//选项描述参数
        public string event_result_title;//事件处理结果标题
        public string event_result_desc;//事件处理结果描述
        public NPGTextureIndex option_icon;//选项icon
    }
    
    public class GSOCommonEventChoiceOptionRefSet : _TALSOBasicRefSet<CommonEventChoiceOptionRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return GSORefSetAssetPath.CommonEventRefSetAssetPath; } }
        public static string objName { get { return "common_event_choice_option"; } }
    }
}