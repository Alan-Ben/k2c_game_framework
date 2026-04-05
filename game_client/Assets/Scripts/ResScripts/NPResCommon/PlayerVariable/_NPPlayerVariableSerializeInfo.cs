using System;
using UnityEngine;

namespace GOE
{
    [System.Serializable]
    public class _NPPlayerVariableSerializeInfo
    {
        public string s_variable;

        //是否已经初始化过数据
        [System.NonSerialized]
        private bool _m_bIsInited = false;
        //玩家条件集合数据对象
        [System.NonSerialized]
        private NPPlayerVariableGroupObj _m_cgVariableGroup;

        public _NPPlayerVariableSerializeInfo()
        {
            s_variable = string.Empty;

            _m_bIsInited = false;
            _m_cgVariableGroup = null;
        }

        /// <summary>
        /// 使用实际的条件对象，根据是否已经初始化过进行判断和处理
        /// </summary>
        public NPPlayerVariableGroupObj variable
        {
            get
            {
                if (_m_bIsInited)
                    return _m_cgVariableGroup;

                _m_bIsInited = true;
                _m_cgVariableGroup = NPPlayerVariableGroupObj.readVariableGroup(s_variable, string.Empty);

                //如果已经读取，则在非编辑器环境下需要设置数据为空，注意这里服务器不能设置为空
#if !UNITY_EDITOR
            s_variable = null;
#endif

                return _m_cgVariableGroup;
            }
        }

        /// <summary>
        /// 判断本对象的条件是否匹配
        /// </summary>
        /// <param name="_varVariableInfo"></param>
        /// <returns></returns>
        public long CalculateVariableResult(NPVarInfo _varVariableInfo)
        {
            NPPlayerVariableGroupObj variableGroup = variable;
            if (null == variableGroup)
                return 0;

            return variableGroup.CalculateVariableResult(_varVariableInfo);
        }

        /******************
         * 从带入的字符串内读取属性加成信息
         * 
         * @author alzq.z
         * @time   Aug 27, 2013 10:57:11 PM
         */
        public void ParseFromString(string _str)
        {
            ParseFromString(_str, string.Empty);
        }
        public void ParseFromString(string _str, string _fieldName)
        {
            s_variable = _str;

            _m_bIsInited = false;
            _m_cgVariableGroup = null;
        }
        public static _NPPlayerVariableSerializeInfo ReadFromString(string _str)
        {
            _NPPlayerVariableSerializeInfo info = new _NPPlayerVariableSerializeInfo();
            info.ParseFromString(_str, string.Empty);

            return info;
        }
        public static _NPPlayerVariableSerializeInfo ReadFromString(string _str, string _fieldName)
        {
            _NPPlayerVariableSerializeInfo info = new _NPPlayerVariableSerializeInfo();
            info.ParseFromString(_str, _fieldName);

            return info;
        }
    }
}

