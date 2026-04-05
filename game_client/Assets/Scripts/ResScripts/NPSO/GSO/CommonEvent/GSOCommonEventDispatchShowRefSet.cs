using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 通用事件-派遣事件问题表
    /// </summary>
    [Serializable]
    public class CommonEventDispatchShowRefObj : _IALBasicRefObj
    {
        public long _refId { get { return dispatch_show_id; } }

        public long dispatch_show_id;//展示id
        public string event_name;//事件名称
        public string event_simple_desc;//事件简易描述
        public NPGTextureIndex event_list_icon;//在事件列表中icon
        public string event_detail_desc;//事件详细描述
        public NPGTextureIndex detail_icon;//详情icon图

    }
    
    public class GSOCommonEventDispatchShowRefSet : _TALSOBasicRefSet<CommonEventDispatchShowRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return GSORefSetAssetPath.CommonEventRefSetAssetPath; } }
        public static string objName { get { return "common_event_dispatch_show"; } }
    }
}