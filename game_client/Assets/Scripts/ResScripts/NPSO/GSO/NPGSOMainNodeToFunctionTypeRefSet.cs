using ALPackage;
using NPEnum;
using System;
using System.Collections.Generic;

namespace GOE
{
    [Serializable]
    public class NPMainNodeToFunctionTypeRefObj : _IALBasicRefObj
    {
        public long _refId { get { return (long)main_node; } }

        public ESysSceneType main_node;//唯一识别标志，界面跳转类型
        public ENPFunctionType function_type;//解锁功能枚举
        public _NPPlayerConditionSerializeInfo condition_info;//条件集合（function_type先判断再走条件流程）
        public string unlock_tip;//解锁提示
        public List<string> unlock_tip_args;//提示对应的参数
        public ESysSceneType condition_fail_main_node;//条件不满足时的界面跳转类型
    }

    public class NPGSOMainNodeToFunctionTypeRefSet : _TALSOBasicRefSet<NPMainNodeToFunctionTypeRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
        public static string objName { get { return "main_node_to_function_type"; } }
    }
}
