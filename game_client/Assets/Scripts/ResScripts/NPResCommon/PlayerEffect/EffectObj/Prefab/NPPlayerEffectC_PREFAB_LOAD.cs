using System;
using System.Collections.Generic;

using ALPackage;
using NPEnum;

#if NP_GAME
using GOE;
#endif

namespace GOE
{
    public class NPPlayerEffectC_PREFAB_LOAD : _ANPPlayerEffectInfo
    {
        private string _m_sTag;     //注册的标记
        private long _m_lUIResPathId;         //资源加载路径
        private float _m_duration;         //持续时间(秒)

        public NPPlayerEffectC_PREFAB_LOAD()
        {
            _m_sTag = string.Empty;
            _m_lUIResPathId = 0;
            _m_duration = 0;
        }

        /************
         * 效果类型
         **/
        public override ENPPlayerEffectType effectType { get { return ENPPlayerEffectType.C_PREFAB_LOAD; } }

        public override void DealPlayerEffect(NPVarInfo _varVariableInfo)
        {
#if NP_GAME
            PrefabControllerMgr.instance.loadPrefab(_m_sTag, _m_lUIResPathId, _m_duration);
#endif
        }

        public static NPPlayerEffectC_PREFAB_LOAD readEffect(string _str)
        {
            string[] strs = _str.Split(':');
            if (strs.Length < 2)
            {
                UnityEngine.Debug.LogError("配置错误 - C_PREFAB_LOAD   example: enum:tag:uiResPathId Error Str: " + _str);
                return null;
            }

            NPPlayerEffectC_PREFAB_LOAD effectObj = new NPPlayerEffectC_PREFAB_LOAD();

            try
            {
                effectObj._m_sTag = strs[0];
                effectObj._m_lUIResPathId = long.Parse(strs[1]);
                effectObj._m_duration = strs.Length > 2 ? ALCommon.ParseFloat(strs[2]) : 0;

                return effectObj;
            }
            catch (Exception)
            {
                UnityEngine.Debug.LogError("配置错误 - C_PREFAB_LOAD   example: enum:tag:uiResPathId Error Str: " + _str);
                return null;
            }
        }
    }
}

