using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using LitJson;

namespace GOE
{
    /// <summary>
    /// list格式的表补丁处理器
    /// </summary>
    public abstract class _ACommonListHotRefPatchDealer : _ICommonHotRefPatchDealer
    {
        //打补丁
        public bool patchHotRefData(JsonData _data)
        {
            if (null == _data)
                return false;

            bool isTotalSuc = true;
            
            JsonData fields = _data["fields"];
            if (null == fields)
            {
                UnityEngine.Debug.LogError($"配表补丁要补丁的表，fields字段解析失败，请检查，表名：{getTableName}");
                return false;
            }
            
            //解析字段列表
            List<string> fieldList = new List<string>();
            foreach (JsonData field in fields)
            {
                string fieldName = field.ToString();
                if (string.IsNullOrEmpty(fieldName))
                {
                    UnityEngine.Debug.LogError($"配表补丁要补丁的表，fields字段解析失败，请检查，表名：{getTableName}");
                    return false;
                }
                
                fieldList.Add(fieldName);
            }
            
            JsonData datas = _data["data"];
            if (null == datas)
            {
                UnityEngine.Debug.LogError($"配表补丁要补丁的表，data，请检查，表名：{getTableName}");
                return false;
            }

            //这一行的唯一id
            long refId = 0;
            //这一行的数据字典
            Dictionary<string, string> valueDict = new Dictionary<string, string>();
            foreach (JsonData data in datas)
            {
                if(null == data || data.Count < 2 || data.Count != fieldList.Count)
                {
                    UnityEngine.Debug.LogError($"配表补丁要补丁的表，data数据长度格式有问题，请检查，表名：{getTableName}");
                    continue;
                }
                
                valueDict.Clear();
                JsonData jsonData = data[0];
                if (!long.TryParse(jsonData.ToString(), out refId))
                {
                    UnityEngine.Debug.LogError($"配表补丁要补丁的表，data数据唯一id不是数值类型，请检查，表名：{getTableName}");
                    continue;
                }

                for (int i = 1; i < data.Count; i++)
                {
                    valueDict.Add(fieldList[i], data[i].ToString());
                }

                //子类实现每一行数据怎么补丁
                bool isSuc = _applyPatch(refId, valueDict);
                if (!isSuc)
                {
                    UnityEngine.Debug.LogError($"配表补丁要补丁的表，id:{refId}行有问题，请检查，表名：{getTableName}");
                    isTotalSuc = false;
                }
            }

            return isTotalSuc;
        }
        
        //表名
        public abstract string getTableName { get; }
        
        /// <summary>
        /// 应用补丁，子类实现具体逻辑
        /// </summary>
        /// <param name="_refId">这一行的唯一id</param>
        /// <param name="_valueDict">代表的是每一行的每个字段的数据</param>
        protected abstract bool _applyPatch(long _refId, Dictionary<string, string> _valueDict);
    }
}