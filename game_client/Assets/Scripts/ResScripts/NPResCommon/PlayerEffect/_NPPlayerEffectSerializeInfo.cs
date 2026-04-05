using System;
using System.Collections.Generic;
using UnityEngine;
using NPEnum;

namespace GOE
{
    [System.Serializable]
    public class _NPPlayerEffectSerializeInfo
    {
        public string s_effect;

        //是否已经初始化过数据
        [System.NonSerialized]
        private bool _m_bIsInited = false;
        //玩家条件集合数据对象
        [System.NonSerialized]
        private List<NPPlayerEffectSerializeInfo> _m_lEffectList;

        public _NPPlayerEffectSerializeInfo()
        {
            s_effect = string.Empty;

            _m_bIsInited = false;
            _m_lEffectList = null;
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        public bool isEmpty
        {
            get
            {
                if (null == effect || effect.Count <= 0)
                    return true;

                return false;
            }
        }

        /// <summary>
        /// 使用实际的条件对象，根据是否已经初始化过进行判断和处理
        /// </summary>
        public List<NPPlayerEffectSerializeInfo> effect
        {
            get
            {
                if (_m_bIsInited)
                    return _m_lEffectList;

                _m_bIsInited = true;
                _m_lEffectList = NPPlayerEffectSerializeInfo.readEffectList(s_effect);

                //如果已经读取，则在非编辑器环境下需要设置数据为空，注意这里服务器不能设置为空
#if !UNITY_EDITOR
            s_effect = null;
#endif

                return _m_lEffectList;
            }
        }

        /// <summary>
        /// 调用游戏中执行效果的行为函数
        /// </summary>
        /// <param name="_varVariableInfo"></param>
        public void dealEffect()
        {
            dealEffect(null);
        }
        public void dealEffect(NPVarInfo _varVariableInfo)
        {
            NPPlayerEffectSerializeInfo.dealEffect(effect, _varVariableInfo);
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
            s_effect = _str;

            _m_bIsInited = false;
            _m_lEffectList = null;
        }
        public static _NPPlayerEffectSerializeInfo ReadFromString(string _str)
        {
            _NPPlayerEffectSerializeInfo info = new _NPPlayerEffectSerializeInfo();
            info.ParseFromString(_str, string.Empty);

            return info;
        }
        public static _NPPlayerEffectSerializeInfo ReadFromString(string _str, string _fieldName)
        {
            _NPPlayerEffectSerializeInfo info = new _NPPlayerEffectSerializeInfo();
            info.ParseFromString(_str, _fieldName);

            return info;
        }
    }
}

