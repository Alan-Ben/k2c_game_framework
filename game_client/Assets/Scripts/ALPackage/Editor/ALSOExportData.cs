using System;
using System.Collections.Generic;

using UnityEngine;

/******************
 * excel文件读取通用函数对象
 **/
namespace ALPackage
{
    [System.Serializable]
    public struct ALExportData
    {
        public string key;
        public string value;
        public ALExportData(string _key,string _value)
        {
            key = _key;
            value = _value;
        }
    }
    /********************
     * 导出部分数据存储对象
     **/
    [System.Serializable]
    public class ALSOExportData : ScriptableObject
    {
        /** 对应值的存储队列 */
        public List<ALExportData> valueList;
    }
}
