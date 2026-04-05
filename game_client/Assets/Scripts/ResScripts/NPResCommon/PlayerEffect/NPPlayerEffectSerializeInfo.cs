using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using ALPackage;
using NPEnum;

namespace GOE
{
    [System.Serializable]
    public class NPPlayerEffectSerializeInfo
    {
        public ENPPlayerEffectType effect_type;//效果类型
        public string effect_info_str;

        /** 是否初始化过 */
        [System.NonSerialized]
        private bool _m_bHasInit = false;
        /** 对应的具体数据 */
        [System.NonSerialized]
        private _ANPPlayerEffectInfo _m_eiEffectInfo;

        public _ANPPlayerEffectInfo effectInfo
        {
            get
            {
                if (!_m_bHasInit)
                {
                    _m_bHasInit = true;
                    //还未初始化则进行初始化
                    _m_eiEffectInfo = _ANPPlayerEffectInfo.readEffectInfo(effect_type, effect_info_str);
                    if (_m_eiEffectInfo == null)
                        Debug.LogError("效果没有读取成功: " + effect_type + ":" + effect_info_str);
                }

                return _m_eiEffectInfo;
            }
        }

        /**************
         * 将字符串转化为本对象
         **/
        public static List<NPPlayerEffectSerializeInfo> MakeListFromString(string _str)
        {
            return readEffectList(_str);
        }
        public static List<NPPlayerEffectSerializeInfo> readEffectList(string _str)
        {
            if (null == _str || _str == "")
                return null;

            List<NPPlayerEffectSerializeInfo> list = new List<NPPlayerEffectSerializeInfo>();

            string[] effectStrs = _str.Split(';');

            for (int i = 0; i < effectStrs.Length; i++)
            {
                string infoStr = effectStrs[i];
                if(string.IsNullOrEmpty(infoStr))
                    continue;
                
                string[] strs = infoStr.Split(new char[] { ':' }, 2);

                if (strs.Length < 1)
                    continue;

                //创建对象
                NPPlayerEffectSerializeInfo obj = new NPPlayerEffectSerializeInfo();
                obj.effect_type = (ENPPlayerEffectType)ALCommon.EnumParse(typeof(ENPPlayerEffectType), strs[0], true);
                obj.effect_info_str = strs.Length == 2 ? strs[1] : null;

                list.Add(obj);
            }

            return list;
        }

        public static NPPlayerEffectSerializeInfo readEffect(string _str)
        {
            if (null == _str || _str == "")
                return null;

            string[] strs = _str.Split(new char[] { ':' }, 2);

            if (strs.Length < 1)
                return null;

            //创建对象
            NPPlayerEffectSerializeInfo obj = new NPPlayerEffectSerializeInfo();
            obj.effect_type = (ENPPlayerEffectType)ALCommon.EnumParse(typeof(ENPPlayerEffectType), strs[0], true);
            obj.effect_info_str = strs.Length == 2 ? strs[1] : null;

            return obj;
        }

        //处理Tutorial效果
        public void dealEffect(NPVarInfo _varVariableInfo)
        {
            //处理位置效果
            if (effectInfo != null) 
                effectInfo.DealPlayerEffect(_varVariableInfo);
        }

        //处理Tutorial效果列表
        public static void dealEffect(List<NPPlayerEffectSerializeInfo> _effectInfoList, NPVarInfo _varVariableInfo)
        {
            if (_effectInfoList == null)
                return;

            NPPlayerEffectSerializeInfo tmp = null;
            for (int i = 0; i < _effectInfoList.Count; i++)
            {
                tmp = _effectInfoList[i];
                if (null == tmp)
                    continue;

                tmp.dealEffect(_varVariableInfo);
            }
        }
    }
}

