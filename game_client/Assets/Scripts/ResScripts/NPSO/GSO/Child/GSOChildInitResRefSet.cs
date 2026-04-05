
using System.Collections.Generic;
using ALPackage;
using System;
using CommonEnum;
using UnityEngine;

namespace GOE
{
    [Serializable]
    public class ChildInitResRefObj : _IALBasicRefObj
    {
        public long _refId { get { return id; } }

        public long id;
        public EChildSexType sex;
        public List<long> res_id_list; // 阶段对应的形象id
        public long married_res_id; // 婚后的形象id
        public long unnamed_res_id; // 取名之前的形象id

        /// <summary>
        /// 根据阶段获取形象id
        /// </summary>
        public long getResIdByStep(int _step)
        {
            if (res_id_list.Count == 0)
                return 0;
            
            _step = Mathf.Clamp(_step, 0, res_id_list.Count - 1);
            return res_id_list[_step];
        }
    }

    public class GSOChildInitResRefSet : _TALSOBasicRefSet<ChildInitResRefObj>
    {
        public static string assetPath { get { return "refdata/child_refdata.unity3d"; } }
        public static string objName { get { return "child_init_res"; } }
    }
}
