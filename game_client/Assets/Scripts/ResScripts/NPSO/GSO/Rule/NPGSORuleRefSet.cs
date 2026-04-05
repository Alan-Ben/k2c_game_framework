
using ALPackage;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 规则索引表
    /// </summary>
    [Serializable]
    public class NPRuleRefObj : _IALBasicRefObj
    {
        public long _refId { get { return id; } }

        public long id; //唯一id
        public List<long> sub_id_list; //子节点id列表
        public string name; //名字
        public NPGTextureIndex icon; //图标
        public _NPPlayerConditionSerializeInfo show_cond; //显示条件
        public _NPPlayerEffectSerializeInfo client_effect; //执行的效果

#if NP_GAME

        [System.NonSerialized] private List<NPRuleSubRefObj> _m_ruleSubList;

        public List<NPRuleSubRefObj> ruleSubList
        { 
            get 
            {
                if (null == _m_ruleSubList)
                {
                    _m_ruleSubList = new List<NPRuleSubRefObj>();

                }
                return _m_ruleSubList;
            }
        }

        public void addRuleSubRef(NPRuleSubRefObj _subRef)
        {
            if (ruleSubList.Contains(_subRef))
                return;
            ruleSubList.Add(_subRef);
        }

#endif

    }
    public class NPGSORuleRefSet : _TALSOBasicRefSet<NPRuleRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
        public static string objName { get { return "rule"; } }
    }
}