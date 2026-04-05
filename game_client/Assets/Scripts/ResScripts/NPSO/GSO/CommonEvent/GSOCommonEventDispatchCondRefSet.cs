using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 通用事件-派遣事件条件表
    /// </summary>
    [Serializable]
    public class CommonEventDispatchCondRefObj : _IALBasicRefObj
    {
        public long _refId { get { return id; } }

        public long id;//唯一id
        public HeroConditionSerializeInfo condition;//条件
        public int num;//完成条件需要同时满足条件的数量
        public string desc;//描述
        public List<string> desc_args;//描述参数
        public NPGTextureIndex icon;//条件图标
    }
    
    public class GSOCommonEventDispatchCondRefSet : _TALSOBasicRefSet<CommonEventDispatchCondRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return GSORefSetAssetPath.CommonEventRefSetAssetPath; } }
        public static string objName { get { return "common_event_dispatch_cond"; } }
    }
}