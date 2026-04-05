using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 通用事件-派遣事件结果表
    /// </summary>
    [Serializable]
    public class CommonEventDispatchResultRefObj : _IALBasicRefObj
    {
        public long _refId { get { return dispatch_result_id; } }

        public long dispatch_result_id;//派遣结果id
        public string event_result_title;//事件处理结果标题
        public string event_result_desc;//事件处理结果描述
    }
    
    public class GSOCommonEventDispatchResultRefSet : _TALSOBasicRefSet<CommonEventDispatchResultRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return GSORefSetAssetPath.CommonEventRefSetAssetPath; } }
        public static string objName { get { return "common_event_dispatch_result"; } }
    }
}