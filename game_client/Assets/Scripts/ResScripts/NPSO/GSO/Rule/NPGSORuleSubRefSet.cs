
using ALPackage;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 规则表子表
    /// </summary>
    [Serializable]
    public class NPRuleSubRefObj : _IALBasicRefObj
    {
        public long _refId { get { return id; } }

        public long id;//唯一id
        public string name;//名字
        public _NPPlayerConditionSerializeInfo show_cond;//显示条件
        public _NPPlayerEffectSerializeInfo client_effect;//执行的效果
    }
    public class NPGSORuleSubRefSet : _TALSOBasicRefSet<NPRuleSubRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
        public static string objName { get { return "rule_sub"; } }
    }
}