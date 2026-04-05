using System.Collections.Generic;
using LitJson;

namespace GOE
{
    /// <summary>
    /// general格式的表补丁处理器
    /// </summary>
    public abstract class _ACommonGeneralHotRefPatchDealer: _ICommonHotRefPatchDealer
    {
        public bool patchHotRefData(JsonData _data)
        {
            if (null == _data)
            {
                UnityEngine.Debug.LogError($"配表补丁要补丁的表，data，请检查，表名：{getTableName}");
                return false;
            }
            
            Dictionary<string, string> valueDict = new Dictionary<string, string>();
            
            foreach (string dataKey in _data.Keys)
            {
                valueDict.TryAdd(dataKey, _data[dataKey].ToString());
            }
            
            //子类实现每一行数据怎么补丁
            bool isSuc = _applyPatch(valueDict);
            if (!isSuc)
            {
                UnityEngine.Debug.LogError($"配表补丁要补丁的表，有问题，请检查，表名：{getTableName}");
            }
            return isSuc;
        }
        
        /// <summary>
        /// 应用补丁，子类实现具体逻辑
        /// </summary>
        /// <param name="_valueDict">代表的是每一行的每个字段的数据</param>
        protected abstract bool _applyPatch(Dictionary<string, string> _valueDict);
        
        public abstract string getTableName { get; }
    }
}