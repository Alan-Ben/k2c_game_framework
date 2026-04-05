using ALPackage;
using System.Collections.Generic;

namespace GOE
{
    [System.Serializable]
    public class NPRemoteEffectRefObj : _IALBasicRefObj
    {
        public long _refId { get { return id; } }

        public long id;//唯一识别标记
        public _NPPlayerConditionSerializeInfo condition;//触发异步效果的条件
        public List<NPCommonCostItem> cost_item_list;//消耗列表

        public _NPPlayerConditionSerializeInfo client_condition;//远端执行后，客户端需要执行对应后续效果的条件
        public _NPPlayerEffectSerializeInfo client_effect;//在服务端执行效果之后，客户端需要本地执行的效果信息
    }

    public class NPGSORemoteEffectRefSet : _TALSOBasicRefSet<NPRemoteEffectRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
        public static string objName { get { return "remote_effect"; } }
    }
}

